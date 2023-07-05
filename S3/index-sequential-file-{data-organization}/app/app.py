import os
import shlex

from .util.cli import *
from .constants import *

from .file.rec.data_record import DataRecord

from .file.binary_file import SearchError
from .file.serial_file import SerialFile
from .file.sequential_file import SequentialFile
from .file.index_file import IndexFile


class App:
    def __init__(self, init_data_path: str) -> None:
        self.rec = DataRecord()

        self.ser = SerialFile(ser_path, self.rec, F)
        self.seq = SequentialFile(seq_path, self.rec, F)
        self.active = IndexFile(active_path, self.rec, F)

        self.__init_data(init_data_path)


    def __init_data(self, init_data_path: str) -> None:
        with open(init_data_path, "r") as f:
            lines = f.readlines()
            for line in lines:
                cols = shlex.split(line)
                self.seq.insert_record(
                    self.rec.get_rec(int(cols[0]), cols[1],  
                                    cols[2],  int(cols[3]),  float(cols[4]), 0))
        self.generate_active()


    def make_new_db(self) -> None:
        def valid_path(path):
            try: open(path, "wb").close()
            except: return False
            return True

        self.__new_db_path(
            valid=valid_path,
            err="Putanja nije validna.")


    def change_db(self) -> None:
        self.__new_db_path(
            valid=lambda path : os.path.isfile(path),
            err="Datoteka ne postoji ili putanja nije validna.")
        

    def __new_db_path(self, valid, err: str) -> None:
        new_path = input("Putanja datoteke: ").strip()
        new_path = os.path.abspath(new_path)

        if not valid(new_path):
            Cli.print_warning(err)
            return

        self.active = IndexFile(new_path, self.rec, F)


# --------------------------------------
    def print_active(self) -> None:
        print()
        self.active.print_file()
        input("\nUnesite karakter da nastavite ")
    
    def print_serial(self):
        print()
        self.ser.print_file()
        input("\nUnesite karakter da nastavite ")

    def print_sequential(self):
        print()
        self.seq.print_file()
        input("\nUnesite karakter da nastavite ")
# --------------------------------------


    def add_ser(self) -> None:
        print()
        try:
            rec = self.__get_record()
            self.ser.insert_record(rec)

            input("\nUnesite karakter da nastavite ")
        except InputError as e1:
            Cli.print_warning(e1)
        except SearchError:
            Cli.print_warning("Podaci sa unetim evidencionim brojem već postoje.")



    def generate_sequential(self) -> None:
        self.seq.reformat_file()
        records = []
        with open(self.ser.path, "rb") as f:
            while True:
                block = self.ser.read_block(f)
                if not block:
                    break

                for rec in block:
                    records.append(rec)

        records = sorted(records, key=lambda d:d["id"])
        for rec in records:
            self.seq.insert_record(rec)
        self.ser.reformat_file()

    
    def generate_active(self) -> None:
        self.active.reformat_file()
        self.active.init_file(self.seq)


    def find_active(self) -> None:
        print()
        try:
            id = Cli.input_digit("Evidencioni broj za pretragu: ")
            found = self.active.find_by_id(id)
            if found:
                i, j, rec = found
                if j == 0:
                    print("\nzona prekoračenja", rec)
                else:
                    print(f"\nblok {i}, slog {j}", rec)

                input("\nUnesite karakter da nastavite ")
            else:
                Cli.print_warning("Podaci sa unetim evidencionim brojem ne postoje.")
        except InputError as e:
            Cli.print_warning(e)


    def add_active(self) -> None:
        print()
        try:
            rec = self.__get_record()
            self.active.insert_record(rec)

            input("\nUnesite karakter da nastavite ")
        except InputError as e1:
            Cli.print_warning(e1)
        except SearchError:
            Cli.print_warning("Podaci sa unetim evidencionim brojem već postoje.")


    def update_active(self) -> None:
        print()
        try:
            id = Cli.input_digit("Evidencioni broj: ", max_len=9)
            date = Cli.input_datetime("Datum i vreme odstrela (dd/MM/YYYY hh:mm): ")
            found = self.active.find_by_id(id)
            if found:
                _, _, rec = found
                rec["date"] = date
                self.active.update_record(rec)

                input("\nUnesite karakter da nastavite ")
            else:
                Cli.print_warning("Podaci sa unetim evidencionim brojem ne postoje.")
        except InputError as e:
            Cli.print_warning(e)


    def remove_active(self) -> None:
        print()
        try:
            id = Cli.input_digit("Evidencioni broj za brisanje: ")
            self.active.delete_by_id(id)

            input("\nUnesite karakter da nastavite ")
        except InputError as e:
            Cli.print_warning(e)
        except SearchError as e:
            Cli.print_warning("Podaci sa unetim evidencionim brojem ne postoje.")


    # dobavljanje sloga iz konzole
    def __get_record(self) -> dict:
        new_id = Cli.input_digit("Evidencioni broj: ", max_len=9)
        type = Cli.input_str("Vrsta divljači: ", 40)
        date = Cli.input_datetime("Datum i vreme odstrela (dd/MM/YYYY hh:mm): ")
        ammunition = Cli.input_digit("Oznaka municije: ", max_len=7)
        weight = Cli.input_float("Težina (kg): ", pos=True)

        return self.rec.get_rec(new_id, type, date, ammunition, weight, 0)
