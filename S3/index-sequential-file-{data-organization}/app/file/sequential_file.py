import os

from .binary_file import BinaryFile


class SequentialFile(BinaryFile):
    def __init__(self, path, record, block_f, empty_key=-1):
        BinaryFile.__init__(self, path, record, block_f, empty_key)
        self.init_file()


    def init_file(self):
        if os.path.isfile(self.path) and os.stat(self.path).st_size != 0:
            return

        with open(self.path, "wb+") as f:
            # inicijalizacija datoteke podrazumeva unos bloka koji sadrzi prazne slogove
            block = self.block_f*[self.record.get_empty_rec()]
            self.write_block(f, block)


    def insert_record(self, rec):
        with open(self.path, "rb+") as f:
            while True:
                block = self.read_block(f)
                if not block:
                    break # EOF

                last = self.__is_last(block)
                here, j = self.__find_in_block(block, rec)
                if not here:
                    continue

                # save last record for inserting into next block
                tmp_rec = block[self.block_f-1]
                for k in range(self.block_f-1, j, -1):
                    block[k] = block[k-1]               # move records
                block[j] = rec                          # insert
                rec = tmp_rec                           # new record for insertion

                f.seek(-self.block_size, 1)
                self.write_block(f, block)

                # last block without empty rec?
                if last and block[self.block_f-1]["id"] != self.empty_key:
                    block = self.block_f*[self.record.get_empty_rec()]
                    self.write_block(f, block)


    def __find_in_block(self, block, rec):
        for j in range(self.block_f):
            if block[j]["id"] == self.empty_key or block[j]["id"] > rec["id"]:
                return True, j
        return False, None


    def __is_last(self, block):
        for i in range(self.block_f):
            if block[i]["id"] == self.empty_key:
                return True
        return False


    def edit_record(self, rec):
        i, j = self.find_by_id(rec["id"])
        if not i:
            return

        with open(self.path, "rb+") as f:
            f.seek(i * self.block_size)

            block = self.read_block(f)
            block[j] = rec
            self.write_block(f, block)
            block = self.read_block(f)


    def find_by_id(self, id: int) -> tuple:
        i = 0
        with open(self.path, "rb") as f:
            while True:
                block = self.read_block(f)

                if not block:
                    return None, None

                for j in range(self.block_f):
                    if block[j]["id"] == id:
                        return i, j
                    if block[j]["id"] > id:
                        return None, None
                i += 1
    
    def reformat_file(self):
        open(self.path, "w").close()
        self.init_file()
