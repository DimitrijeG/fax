import os
import shlex
import shutil

from .constants import *
from .util.cli import *

from .file.record.data_record import DataRecord
from .file.record.mod_record import CommitRecord

from .file.serial_file import SerialFile
from .file.sequential_file import SequentialFile


class App:
    def __init__(self, init_data_path: str) -> None:
        self.data_rec = DataRecord()
        self.mod_rec = CommitRecord()

        self.db = SequentialFile(def_active_path, self.data_rec, F)
        self.ser_db = SerialFile(def_ser_path, self.mod_rec, F)
        self.seq_db = SequentialFile(def_seq_path, self.mod_rec, F)

        self.__init_records(init_data_path)
        
    
    def __init_records(self, init_data_path: str) -> None:
        with open(init_data_path, "r") as f:
            for line in f.readlines():
                cols = shlex.split(line)
                self.db.insert_record(
                    self.data_rec.get_rec(int(cols[0]), int(cols[1]),  
                                    float(cols[2]),  cols[3],  cols[4]))


    def make_new_db(self) -> None:
        def valid_path(path):
            try:
                open(path, "wb").close()
            except:
                return False
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

        self.db = SequentialFile(new_path, self.data_rec, F)


    def print_data(self) -> None:
        print()
        self.db.print_file()
        input("\nUnesite karakter da nastavite ")
    
    def print_serial(self):
        self.ser_db.print_file()

    def print_sequential(self):
        self.seq_db.print_file()


    def add_record(self) -> None:
        self.__add_mod_record(status=0)


    def edit_record(self) -> None:
        self.__add_mod_record(status=1)
    

     # dodavanje izmene u serijsku datoteku promena
    def __add_mod_record(self, status: int) -> None:
        print()
        try:
            rec = self.__get_record(status)
            self.ser_db.insert_record(rec)
        except InputError as e:
            Cli.print_warning(e)


    def delete_record(self) -> None:
        print()
        try:
            rec = self.mod_rec.get_empty_rec()
            rec["id"] = Cli.input_digit("ID za brisanje: ")
            rec["status"] = 2
            self.ser_db.insert_record(rec)
        except InputError as e:
            Cli.print_warning(e)


    def generate_sequential(self) -> None:
        self.seq_db.reformat_file()
        records = []
        with open(self.ser_db.path, "rb") as f:
            while True:
                block = self.ser_db.read_block(f)
                if not block:
                    break
                for rec in block:
                    if rec["id"] == -1: break
                    # records.append(rec)
                    self.seq_db.insert_record(rec)
                    
        # records = sorted(records, lambda d:d["id"])

        # for rec in records:
        #     self.seq_db.insert_record(rec)


    def reset_changes(self) -> None:
        self.ser_db.reformat_file()
        self.seq_db.reformat_file()


    def generate_output(self) -> None:
        out = input("Putanja izlazne datoteke: ").strip()
        out = out if out else def_out_path
        out = os.path.abspath(out)

        try:
            open(out, "wb").close()

            if os.path.samefile(self.db.path, out):
                Cli.print_warning("Putanja izlazne datoteke ne sme da bude jednaka aktivnoj.")
                return
        except:
            Cli.print_warning("Putanja nije validna.")
            return

        print("\nObrada podataka...")
        self.__process_sequential(out)

        print("Izlazna datoteka je kreirana na putanji:", out)
        input("\nUnesite karakter da nastavite ")


    def __process_sequential(self, out_path: str) -> None:
        out_db = SequentialFile(out_path, self.data_rec, F)

        self.__copy_active(out_db)
        self.__process_modifications(out_db)
        self.reset_changes()


    def __copy_active(self, out_db) -> None:
        with open(self.db.path, "rb") as f:
            while True:
                block = self.db.read_block(f)
                if not block:
                    break

                for rec in block:
                    if rec and rec["id"] != -1:
                        out_db.insert_record(rec)


    # prepisivanje aktivne uz simultano editovanje i brisanje
    def __process_modifications(self, out_db) -> None:
        with open(self.seq_db.path, "rb") as f:
            while True:
                block = self.seq_db.read_block(f)
                if not block:
                    break

                for rec in block:
                    if rec["id"] == -1: break
                    status = rec.pop("status")
                    id = rec["id"]

                    if status == 0:
                        if not out_db.find_by_id(id):
                            out_db.insert_record(rec)
                        else: Cli.print_collision(f"Pokušaj dodavanja postojećeg sloga sa ID-jem {id}!")
                    elif status == 1:
                        if out_db.find_by_id(id):
                            out_db.edit_record(rec)
                        else: Cli.print_collision(f"Pokušaj izmene nepostojećeg sloga sa ID-jem {id}!")
                    elif status == 2:
                        if out_db.find_by_id(id):
                            out_db.delete_by_id(id)
                        else: Cli.print_collision(f"Pokušaj brisanja nepostojećeg sloga sa ID-jem {id}!")


    # dobavljanje sloga iz konzole
    def __get_record(self, status: int) -> dict:
        new_id = Cli.input_digit("ID namestaja: ", max_len=9)
        type = Cli.input_str("Tip namestaja: ", 70)
        manufactured = Cli.input_datetime("Datum proizvodnje (dd/MM/YYYY hh:mm): ")
        model = Cli.input_str("Model: ", 50)
        weight = Cli.input_float("Težina (kg): ", pos=True)

        return self.mod_rec.get_rec(new_id, manufactured, weight, type, model, status)
