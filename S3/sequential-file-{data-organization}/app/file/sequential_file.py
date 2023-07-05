#!/usr/bin/python

import os

from .binary_file import BinaryFile, SearchError


class SequentialFile(BinaryFile):
    def __init__(self, path, record, blocking_factor, empty_key=-1):
        BinaryFile.__init__(self, path, record, blocking_factor, empty_key)

    def __find_in_block(self, block, rec):
        for j in range(self.blocking_factor):
            if block[j]["id"] == self.empty_key or block[j]["id"] > rec["id"]:
                return (True, j)

        return (False, None)

    def insert_record(self, rec):
        with open(self.path, "rb+") as f:
            while True:
                block = self.read_block(f)

                if not block:           # EOF
                    break

                last = self.__is_last(block)
                here, j = self.__find_in_block(block, rec)

                if not here:
                    continue

                # save last record for inserting into next block
                tmp_rec = block[self.blocking_factor-1]
                for k in range(self.blocking_factor-1, j, -1):
                    block[k] = block[k-1]               # move records
                block[j] = rec                          # insert
                rec = tmp_rec                           # new record for insertion

                f.seek(-self.block_size, 1)
                self.write_block(f, block)

                # last block without empty rec?
                if last and block[self.blocking_factor-1]["id"] != self.empty_key:
                    block = self.blocking_factor*[self.record.get_empty_rec()]
                    self.write_block(f, block)

    def edit_record(self, rec):
        found = self.find_by_id(rec["id"])

        if not found:
            return

        block_idx = found[0]
        rec_idx = found[1]

        with open(self.path, "rb+") as f:
            f.seek(block_idx * self.block_size)

            block = self.read_block(f)
            block[rec_idx] = rec
            self.write_block(f, block)
            block = self.read_block(f)

    
    def __is_last(self, block):
        for i in range(self.blocking_factor):
            if block[i]["id"] == self.empty_key:
                return True
        return False

    def print_file(self):
        i = 0
        with open(self.path, "rb") as f:
            while True:
                block = self.read_block(f)

                if not block:
                    break

                i += 1
                print("Block {}".format(i))
                self.print_block(block)

    def find_by_id(self, id):
        i = 0
        with open(self.path, "rb") as f:
            while True:
                block = self.read_block(f)

                if not block:
                    return None

                for j in range(self.blocking_factor):
                    if block[j]["id"] == id:
                        return (i, j)
                    if block[j]["id"] > id:
                        return None

                i += 1

    def get_rec_by_id(self, f, id):
        found = self.find_by_id(id)
        if not found:
            return None

        f.seek(found[0] * self.block_size + found[1] * self.record_size)
        return self.read_record(f)

    def delete_by_id(self, id):
        found = self.find_by_id(id)
        if not found:
            return

        block_idx = found[0]
        rec_idx = found[1]
        next_block = None

        with open(self.path, "rb+") as f:
            while True:
                f.seek(block_idx * self.block_size)  # last block
                block = self.read_block(f)

                for i in range(rec_idx, self.blocking_factor-1):
                    block[i] = block[i+1]       # move records

                if self.__is_last(block):              # is last block full?
                    f.seek(-self.block_size, 1)
                    self.write_block(f, block)
                    break

                next_block = self.read_block(f)
                # first record of next block is now the last of current one
                block[self.blocking_factor-1] = next_block[0]
                f.seek(-2*self.block_size, 1)
                self.write_block(f, block)

                block_idx += 1
                rec_idx = 0

        if next_block and next_block[0]["id"] == self.empty_key:
            os.ftruncate(os.open(self.path, os.O_RDWR),
                         block_idx * self.block_size)
