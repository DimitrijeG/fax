#!/usr/bin/python

import os

from .binary_file import BinaryFile, SearchError


class SerialFile(BinaryFile):
    def __init__(self, path, record, block_f, empty_key=-1):
        BinaryFile.__init__(self, path, record, block_f, empty_key)
        self.init_file()


    def init_file(self):
        if os.path.isfile(self.path) and os.stat(self.path).st_size != 0:
            return

        with open(self.path, "wb+") as f:
            block = self.block_f*[self.record.get_empty_rec()]  # inicijalizacija datoteke podrazumeva unos bloka koji sadrzi prazne slogove
            self.write_block(f, block)


    def insert_record(self, rec):
        if self.find_by_id(rec["id"]):  # provera da li vec postoji slog sa zadatim id-jem - svakom unosu prethodi pretraga!
            raise SearchError()

        with open(self.path, "rb+") as f:
            f.seek(-self.block_size, 2)  # citamo poslednji blok
            block = self.read_block(f)

            for i in range(self.block_f):
                if block[i]["id"] == self.empty_key:  # trazimo prvi prazan slog
                    block[i] = rec
                    break

            i += 1

            if i == self.block_f:  # provera da li smo popunili trenutni blok ili ne
                f.seek(-self.block_size, 1)
                self.write_block(f, block)
                block = self.block_f*[self.record.get_empty_rec()]
                self.write_block(f, block)
            else:
                block[i] = self.record.get_empty_rec()
                f.seek(-self.block_size, 1)
                self.write_block(f, block)


    def find_by_id(self, id):
        i = 0
        with open(self.path, "rb") as f:
            while True:
                block = self.read_block(f)

                for j in range(self.block_f):
                    if block[j]["id"] == id:
                        return (i, j)
                    elif block[j]["id"] == self.empty_key:  # ukoliko smo naisli na prazan slog - stigli smo do kraja datoteke
                        return None
                i += 1


    def reformat_file(self):
        open(self.path, "w").close()
        self.init_file()
