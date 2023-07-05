import os
import struct
from datetime import datetime


class SearchError(BaseException):
    pass


class BinaryFile:
    def __init__(self, path, record, blocking_factor, empty_key):
        self.path = path
        self.record = record
        self.record_size = struct.calcsize(self.record.format)
        self.blocking_factor = blocking_factor
        self.block_size = self.blocking_factor * self.record_size
        self.empty_key = empty_key

        self.init_file()

    def init_file(self):
        if os.path.isfile(self.path) and os.stat(self.path).st_size != 0:
            return

        with open(self.path, "wb+") as f:
            block = self.blocking_factor*[self.record.get_empty_rec()]  # inicijalizacija datoteke podrazumeva unos bloka koji sadrzi prazne slogove
            self.write_block(f, block)

    def write_block(self, file, block):
        binary_data = bytearray()   # Niz bita koji bi trebalo da se upise u datoteku

        # Svaki slog u bloku serijalizujemo i dodamo u niz bajta
        for rec in block:
            rec_binary_data = self.record.dict_to_encoded_values(rec)
            binary_data.extend(rec_binary_data)

        file.write(binary_data)

    def read_block(self, file):
        # Citanje od trenutne pozicije
        binary_data = file.read(self.block_size)
        block = []

        if len(binary_data) == 0:
            return block

        for i in range(self.blocking_factor):   # slajsingom izdvajamo niz bita za svaki slog, i potom vrsimo otpakivanje
            begin = self.record_size*i
            end = self.record_size*(i+1)
            block.append(self.record.encoded_tuple_to_dict(
                binary_data[begin:end]))

        return block

    def write_record(self, f, rec):
        binary_data = self.record.dict_to_encoded_values(rec)
        f.write(binary_data)

    def read_record(self, f):
        binary_data = f.read(self.record_size)

        if len(binary_data) != self.record_size:
            return None

        return self.record.encoded_tuple_to_dict(binary_data)

    def print_block(self, b):
        for i in range(self.blocking_factor):
            rec = b[i].copy()
            rec["manufactured"] = datetime.fromtimestamp(rec["manufactured"]/1000).strftime("%d/%m/%Y %H:%M")
            print(i, rec)

    def reformat_file(self):
        open(self.path, "wb").close
        self.init_file()