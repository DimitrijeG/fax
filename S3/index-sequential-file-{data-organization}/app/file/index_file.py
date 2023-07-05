import os
from math import ceil, log2

from .binary_file import BinaryFile, SearchError
from .rec.record import Record
from .rec.index_record import IndexRecord
from .rec.overflow_record import OverflowRecord


class IndexFile(BinaryFile):
    def __init__(self, path, record, block_f, empty_key=-1):
        BinaryFile.__init__(self, path, record, block_f, empty_key)
        self.i_record = IndexRecord()
        self.o_record = OverflowRecord()
        self.pointer_record = Record(["i"], "i")

        self.main = 0
        self.overflow = 0
        self.index = 0
        self.tree = []


    def init_file(self, seq, ibf=0.8) -> None:
        self.index = 2 * self.pointer_record.size
 

        rec_num = seq.get_record_number()
        empty_per_block = ceil((1 - ibf) * self.block_f)
        # računanje ukupnog broja slogova nakon dodavanja praznih
        recs = rec_num + empty_per_block * (rec_num // self.block_f)

        b = ceil(recs / self.block_f) # ukupan broj blokova
        c = self.__generate_tree_lookup(b)

        i_block_size = 2 * self.i_record.size
        self.main = self.index + c * i_block_size # pokazivac na pocetak primarne

        with open(self.path, "wb+") as f, open(seq.path, "rb") as seqf:
            # ------ formiranje index i primarne zone ------
            p_main = self.main
            p_tree = p_main - i_block_size * len(self.tree[0]) # levi list
            self.leaves = p_tree
            buff = [] # overflow slogova koji ne stanu u blok
            tree_block = []
            i = 0
            while True:
                buff += seq.read_block(seqf)
                if not buff or buff[0]['id'] == -1:
                    break

                # ----------------- čitanje bloka -----------------
                block = buff[:(self.block_f - empty_per_block)]
                buff = buff[(self.block_f - empty_per_block):] # odsecanje slogova koje smo upisali
                block += (self.block_f - len(block)) * [self.record.get_empty_rec()]

                # ------------------ upis bloka ------------------
                f.seek(p_main)
                self.write_block(f, block)
                p_main = f.tell()

                # --------- određivanje najmanjeg ključa ---------
                min_k = block[0]["id"]
                if p_main - self.block_size == self.main: # prvi ključ u stablu
                    min_k = 00
                
                tree_block += [self.i_record.get_rec(min_k, i, min_k, -1)]
                i += 1

                # ------------ upis u indeksno stablo ------------
                if len(tree_block) == 2:
                    f.seek(p_tree)
                    self.write_block_with(f, tree_block, self.i_record)
                    p_tree = f.tell()
                    tree_block = []
                
            if tree_block: # neparan broj blokova, poslednji nije upisan
                tree_block += [self.i_record.get_empty_rec()]
                f.seek(p_tree)
                self.write_block_with(f, tree_block, self.i_record)

            # -------- izgradnja stabla od listova --------
            self.__build_upper_tree(f)

            self.overflow = p_main # pokazivač na zonu prekoračenja
            f.seek(self.overflow)
            self.__write_pointer(f, p_main + self.pointer_record.size)
            self.__write_header(f, self.main, self.overflow) # ažuriranje headera


    def __generate_tree_lookup(self, b) -> int:
        h = ceil(log2(b))
        c = 0
        levels = []
        for i in range(1, h+1):
            elements = ceil(b / 2**(h-i+1))
            levels = [elements] + levels
            c += elements
        
        self.tree = []
        for i in range(0, len(levels)-1):
            base = sum(levels[i+1:])
            indexes = []
            for j in range(levels[i]):
                indexes += [base + j]
            self.tree += [indexes]
        self.tree += [[0]]
        return c


    def __build_upper_tree(self, f):
        i_block_size = 2 * self.i_record.size

        for i in range(1, len(self.tree)):
            level = self.tree[i]
            for j in range(len(level)):
                left_i = self.tree[i-1][2*j]
                right_i = 2*j+1

                f.seek(self.index + left_i * i_block_size)
                left, _ = self.read_block_with(f, self.i_record, 2)

                if right_i < len(self.tree[i-1]):
                    f.seek(self.index + self.tree[i-1][right_i] * i_block_size)
                    right, _ = self.read_block_with(f, self.i_record, 2)
                else: right = self.i_record.get_empty_rec()

                f.seek(self.index + level[j] * i_block_size)
                block = [left, right]
                self.write_block_with(f, block, self.i_record)


    def __read_pointer(self, f):
        bin_data = f.read(self.pointer_record.size)
        if len(bin_data) != self.pointer_record.size:
            return None
        return self.pointer_record.encoded_tuple_to_dict(bin_data)

    def __read_header(self, f):
        f.seek(0)
        main = self.__read_pointer(f)
        overflow = self.__read_pointer(f)
        if not main or not overflow:
            return None
        return {"main": main["i"], "overflow": overflow["i"]}
    

    def __write_pointer(self, f, pointer) -> None:
        bin_data = self.pointer_record.dict_to_encoded_values({"i": pointer})
        f.write(bin_data)

    def __write_header(self, f, main, overflow) -> None:
        f.seek(0)
        self.__write_pointer(f, main)
        self.__write_pointer(f, overflow)

    
    def find_by_id(self, id: int) -> tuple[int, int, dict]:
        with open(self.path, "rb") as f:
            i, j, of, _ = self.__get_location(f, id)

            if not of:    # primarna zona
                f.seek(self.main + i * self.block_size)
                block = self.read_block(f)
                if not block:
                    return None
                
                for j in range(self.block_f):
                    if block[j]["id"] == id and block[j]["status"] != 2:
                        return i+1, j+1, block[j]
                    elif block[j]["id"] > id:
                        return None

            else:    # zona prekoračenja
                if j == -1:
                    return None

                f.seek(j)
                current = self.read_block_with(f, self.o_record, 1)[0]
                while current["id"] != id:
                    n = current["n"]
                    if n == -1:
                        return None
                    f.seek(n)
                    current = self.read_block_with(f, self.o_record, 1)[0]
                if current["status"] != 2:
                    return j, 0, current
        return None


    def __get_location(self, f, id: int) -> tuple[int, int, bool, int]:
        i_block_size = 2 * self.i_record.size
        if len(self.tree) == 1:
            return 0, 0, False, None

        f.seek(self.index)
        j = 0
        for i in range(len(self.tree)-1, 0, -1):
            current = self.read_block_with(f, self.i_record, 2)

            if current[1]["idm"] == -1 or id < current[1]["idm"]:
                f.seek(self.index + self.tree[i-1][2*j] * i_block_size)
                if current[1]["idm"] == -1:
                    j += 1
            else:
                f.seek(self.index + self.tree[i-1][2*j+1] * i_block_size)
                j += 1
        leaf = self.read_block_with(f, self.i_record, 2)

        rec = leaf[1]
        i_loc = f.tell() - self.i_record.size
        if id < rec["idm"] or rec["idm"] == -1:
            rec = leaf[0]
            i_loc -= self.i_record.size

        if rec["idm"] != rec["idp"] and id >= rec["idp"]:
            return rec["im"], rec["ip"], True, i_loc
        return rec["im"], rec["ip"], False, i_loc



    def insert_record(self, rec: dict) -> None:
        found = self.find_by_id(rec["id"])
        if found:
            raise SearchError()

        with open(self.path, "rb+") as f:
            i, _, of, i_loc = self.__get_location(f, rec["id"])

            if not of:    # primarna zona
                f.seek(self.main + i * self.block_size)
                block = self.read_block(f)
                if not block:
                    raise SearchError()
                
                of_rec = rec
                for k in range(self.block_f):
                    r = block[k]
                    if rec["id"] < r["id"]:
                        of_rec = r
                        block[k] = rec
                        break
                    elif r["id"] == -1:
                        block[k] = rec
                        of_rec = None
                        break

                if of_rec:
                    for k in range(k+1, self.block_f):
                        block[k], of_rec = of_rec, block[k]

                f.seek(-self.block_size, 1)
                self.write_block(f, block)

                if not of_rec or of_rec["id"] == -1:
                    return
                rec = of_rec
                
            # zona prekoračenja

            # ažuriranje stabla pretrage
            f.seek(self.overflow)
            p = self.__read_pointer(f)
            new_loc = p["i"]
            p["i"] += self.o_record.size
            f.seek(self.overflow)
            self.__write_pointer(f, p["i"])

            f.seek(i_loc)
            i_rec = self.read_record_with(f, self.i_record)
            next = i_rec["ip"]
            i_rec["idp"] = rec["id"]
            i_rec["ip"] = new_loc
            f.seek(-self.i_record.size, 1)
            self.write_record_with(f, i_rec, self.i_record)

            f.seek(new_loc)
            rec["n"] = next
            self.write_block_with(f, [rec], self.o_record)


    def update_record(self, rec) -> None:
        found = self.find_by_id(rec["id"])
        if not found:
            raise SearchError()

        with open(self.path, "rb+") as f:
            i, j, of, _ = self.__get_location(f, rec["id"])

            if not of:    # primarna zona
                f.seek(self.main + i * self.block_size)
                block = self.read_block(f)
                for k in range(self.block_f):
                    if rec["id"] == block[k]["id"]:
                        block[k] = rec
                        break
                f.seek(-self.block_size, 1)
                self.write_block(f, block)
            else:    # zona prekoračenja
                if j == -1:
                    return None

                f.seek(j)
                current = self.read_block_with(f, self.o_record, 1)[0]
                while current["id"] != rec["id"]:
                    f.seek(current["n"])
                    current = self.read_block_with(f, self.o_record, 1)[0]

                f.seek(-self.o_record.size, 1)
                self.write_block_with(f, [rec], self.o_record)


    def delete_by_id(self, id):
        found = self.find_by_id(id)
        if not found:
            raise SearchError()

        _, _, current = found
        current["status"] = 2
        self.update_record(current)
    

    def __read_o_record(self, f):
        binary_data = f.read(self.o_record.size)
        if len(binary_data) != self.o_record.size:
            return None

        return self.o_record.encoded_tuple_to_dict(binary_data)
    

    def print_file(self):
        with open(self.path, "rb") as f:
            header = self.__read_header(f)
            if not header:
                print("Datoteka je prazna.")
                return
                
            print("Header", header)
            main = header["main"]
            overflow = header["overflow"]
            i = 1
            print("\nIndeksna zona:")
            while f.tell() < main:
                block = self.read_block_with(f, self.i_record, 2)
                print(f"Block {i} ", end="")
                print(block[0], "", end="")
                print(block[1])
                i += 1

            i = 1
            print("\nPrimarna zona:")
            while f.tell() < overflow:
                block = self.read_block(f)
                print(f"Block {i}")
                self.print_block(block)
                i += 1


            print("\nZona prekoračenja:")
            e = self.__read_pointer(f)
            print("Lanac slobodnih pozicija", e)
            while True:
                rec = self.__read_o_record(f)
                if not rec:
                    break
                print(rec)


    def reformat_file(self):
        open(self.path, "w").close()
