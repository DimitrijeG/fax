import os
from app.constants import *
from app.app import App
from app.util.cli import Cli
  

def main():
    app = App(init_data_path=data_model_path)

    menu = {
            "1": app.make_new_db,
            "2": app.change_db,
            "3": app.print_data,
            "4": app.add_record,
            "5": app.edit_record,
            "6": app.delete_record,
            "7": app.generate_sequential,
            "8": app.generate_output,
            "9": app.reset_changes,
            "10": app.print_serial,
            "11": app.print_sequential
    }

    while True:
        out = ["Aktivna datoteka:", app.db.path,
            "\nOpcije:",
            "1 nova datoteka",
            "2 promeni aktivnu datoteku",
            "3 ispiši podatke\n",
            "4 dodaj namestaj",
            "5 izmeni namestaj",
            "6 obriši namestaj\n",
            "7 generiši sekvencijalnu datoteku promena",
            "8 generiši izlaznu datoteku",
            "9 resetuj datoteke promena\n",
            "x izađi"
        ]
        Cli.clear_screen()
        print("\n".join(out))

        choice = input("\nIzbor: ").strip()
        if choice == "x":
            break
        elif choice not in menu:
            Cli.print_warning("\nNepostojeća opcija. Pokušajte ponovo ")
        else:
            menu[choice]()

    os.truncate(def_active_path, 0)


if __name__ == "__main__":
    main()
