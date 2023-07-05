from datetime import datetime

class InputError(BaseException):
    pass


CRED     = '\33[31m'
CYELLOW  = '\33[93m'
CEND     = '\33[0m'

class Cli:
    def print_error(s: str) -> None:
        print(f"{CRED}{s}{CEND}")

    def print_warning(s: str) -> None:
        print(f"{CYELLOW}{s}{CEND}")
        input("\nUnesite karakter da nastavite ")

    
    def print_collision(s: str) -> None:
        Cli.print_error("Greška: " + s)

    
    def clear_screen() -> None:
        print("\033[H\033[J", end="")


    def input_digit(s: str, max_len: int=-1) -> int:
        dig = input(s).strip()

        if not dig.isdigit():
            raise InputError("ID mora da bude pozitivan ceo broj.")
        elif max_len != -1 and len(dig) > max_len:
            raise InputError(f"Maksimalna dužina ID je {max_len}.")
        return int(dig)


    def input_str(s: str, max_size: int) -> str:
        instr = input(s).strip()

        if len(instr) > max_size:
            raise InputError(f"Maksimalna dužina ovog polja je {max_size}.")
        return instr


    def input_datetime(s: str, past: bool=True) -> int:
        instr = input(s).strip()

        try: t = datetime.strptime(instr, "%d/%m/%Y %H:%M")
        except: raise InputError("Neispravan format datuma.")
        
        if past and t > datetime.now():
            raise InputError("Datum ne sme da bude u budućnosti.")
        return int(t.timestamp()) * 1000 # posto treba da vraca milisekunde


    def input_float(s: str, pos: bool=False):
        num = input(s).strip()

        try: num = float(num)
        except: raise InputError("Neispravan realan broj.")

        if pos and num < 0:
            raise InputError("Broj mora da bude pozitivan.")
        return num
