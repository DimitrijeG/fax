import os
from app.constants import *
from app.app import App
from app.util.cli import Cli
  

def main():
    app = App(init_data_path=data_model_path)

    menu = {
            "1": app.make_new_db,
            "2": app.change_db,

            "3": app.add_ser,
            "4": app.generate_sequential,
            "5": app.generate_active,

            "6": app.find_active,
            "7": app.add_active,
            "8": app.update_active,
            "9": app.remove_active,

            "10": app.print_active,
            "11": app.print_serial,
            "12": app.print_sequential,
    }

    while True:
        out = ["Aktivna datoteka:", app.active.path,
            "\nOpcije:",
            "1 nova datoteka",
            "2 promeni aktivnu datoteku\n",

            "3 unos u serijsku datoteku",
            "4 generiši sekvencijalnu datoteku",
            "5 generiši aktivnu datoteku\n",

            "6 pronađi podatak",
            "7 upis podatka",
            "8 izmeni datum i vreme odstrela",
            "9 obriši podatak\n",

            "10 ispiši aktivnu datoteku",
            "11 ispiši serijsku datoteku",
            "12 ispiši sekvencijalnu datoteku",
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

    os.remove(app.ser.path)
    os.remove(app.seq.path)
    os.remove(app.active.path)


if __name__ == "__main__":
    main()
