import os


class SearchError(BaseException):
    pass


class BinaryFile:
    def __init__(self, path, record, block_f, empty_key):
        self.path = path
        self.record = record
        self.block_f = block_f
        self.block_size = self.block_f * record.size
        self.empty_key = empty_key


    def write_block_with(self, file, block, record):
        binary_data = bytearray()   # Niz bita koji bi trebalo da se upise u datoteku

        # Svaki slog u bloku serijalizujemo i dodamo u niz bajta
        for rec in block:
            rec_binary_data = record.dict_to_encoded_values(rec)
            binary_data.extend(rec_binary_data)

        file.write(binary_data)

    def write_block(self, file, block):
        self.write_block_with(file, block, self.record)


    def read_block_with(self, f, record, block_f) -> list:
        # Citanje od trenutne pozicije
        binary_data = f.read(block_f * record.size)
        block = []

        if len(binary_data) == 0:
            return block

        for i in range(block_f):   # slajsingom izdvajamo niz bita za svaki slog, i potom vrsimo otpakivanje
            begin = record.size*i
            end = record.size*(i+1)
            block.append(record.encoded_tuple_to_dict(
                binary_data[begin:end]))

        return block

    def read_block(self, f) -> list:
        return self.read_block_with(f, self.record, self.block_f)


    def write_record_with(self, f, rec, record) -> None:
        binary_data = record.dict_to_encoded_values(rec)
        f.write(binary_data)

    def write_record(self, f, rec: dict) -> None:
        self.write_record_with(f, rec, self.record)


    def read_record_with(self, f, record):
        binary_data = f.read(record.size)
        if len(binary_data) != record.size:
            return None

        return record.encoded_tuple_to_dict(binary_data)

    def read_record(self, f):
        return self.read_record_with(f, self.record)


    def print_block(self, b: list) -> None:
        for i in range(self.block_f):
            rec = b[i].copy()
            print(i+1, rec)


    def print_file(self) -> None:
        i = 1
        with open(self.path, "rb") as f:
            while True:
                block = self.read_block(f)
                if not block:
                    break
                
                print(f"Block {i}")
                self.print_block(block)
                i += 1

    def  get_record_number(self) -> int:
        return os.path.getsize(self.path) // self.record.size
