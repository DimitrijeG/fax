from .search_engine import SearchEngine
from Collections.set import Set
from .query import Query
from Utils.common import *
from Utils.colors import *

from pathlib import Path
import time
import os


class Client(object):
    """Klasa koja kreira model korisnickog interfejsa za konzolu.
    Osim funkcije za pretragu ona mimikuje neke unix komande kao sto su 
    cd, ls, pwd...

    :param path: inicijalna putanja
    :type path: str
    """

    def __init__(self, path: str = os.getcwd()) -> None:
        # konvertuje putanju u apsolutnu ako je prosledjena relativna
        self.path = self.__get_abs_path(path)
        os.chdir(self.path)

        self.files = Set()
        self.engine = SearchEngine()
        self.query = Query()

    def map_html_files(self) -> None:
        """citanje liste fajlova tipa HTML u trenutnom direktorijumu"""
        self.files = self.__get_files()

    # change directory
    def cd(self, cli_input: list) -> None:
        """Metod za promenu direktorijuma
        Ako nema drugog argumenta u unosu onda se podrazumeva putanja Home
        . oznacava trenutni direktorijum dok .. oznacava roditeljski
        komanda radi sa apsolutnim kao i relativnim putanjama
        """
        if not self.validate_command(cli_input, 2):
            return

        if len(cli_input) == 1:
            # u slucaju da nema 2 argumenta podrazumeva se Home
            self.path = str(Path.home())
            os.chdir(self.path)

        elif len(cli_input) == 2:
            path = self.__get_abs_path(cli_input[1])
            if os.path.isdir(path):
                self.path = path
                os.chdir(self.path)

            else:
                raise ValueError('Pristupate nepostojecem direktorijumu.')

    # print working directory
    def pwd(self, cli_input: list) -> None:
        """Ispisuje putanju trenutnog direktorijuma"""
        if not self.validate_command(cli_input):
            return

        print(self.path)

    # list (files)
    def ls(self, cli_input: list) -> None:
        """Ispisuje fajlove svih tipova i foldere u trenutnoj putanji"""
        if not self.validate_command(cli_input):
            return

        print(
            '\n'.join(
                sorted(
                    [f.path for f in os.scandir(
                        self.path) if not os.path.basename(f).startswith('.')]
                )
            )
        )

    # clear screen
    def cls(self, cli_input: list) -> None:
        """Cisti ekran terminala"""
        if self.validate_command(cli_input):
            clear_screen()

    def search(self, cli_input: list) -> None:
        """Metod koji inicijalizuje potrebne strukture podataka u trenutnoj putanji
        a potom omogucava korisniku da unese upit.
        """
        if not self.validate_command(cli_input):
            return

        # rekurzivno citanje putanja html fajlova
        self.map_html_files()
        self.query.results_dict = {}

        print("\nUcitavanje podataka...", end='')

        time_start = time.time()
        self.engine.generate_data(self.files)   # kreiranje Trie i Grafa
        time_end = time.time()

        print(color_string(
            f"\nElapsed time: {round(time_end - time_start, 7)}s\n", bcolors.CGREY))

        # petlja pretrage
        while True:
            cli_input = input("> ").strip().split(' ')

            try:
                check_exit(cli_input)

                if cli_input[0] == 'query':
                    if self.validate_command(cli_input):
                        # kreiranje novog upita
                        self.query.make_query(self.engine)

                elif cli_input[0] == 'pwd':
                    self.pwd(cli_input)

                elif cli_input[0] == 'cls':
                    self.cls(cli_input)

                elif cli_input[0] == 'settings':
                    self.settings(cli_input)

                elif cli_input[0] == 'help':
                    if not self.validate_command(cli_input):
                        continue

                    lines = [
                        "\n<query>     Unos upita za pretragu; primeri upita:",
                        color_string("          python java sql",
                                     bcolors.CGREY),
                        color_string(
                            "          python AND java AND sql", bcolors.CGREY),
                        color_string(
                            "         (python java) XOR (sql NOT framework)", bcolors.CGREY),
                        "<pwd>       Ispisi trenutnu putanju",
                        "<cls>       Ocisti terminal",
                        "<settings>  Podesavanja",
                        "<help>      Pomoc",
                        "<x>         Izlaz iz pretrage",
                        "<q>         Izlaz iz aplikacije\n"
                    ]
                    print('\n'.join(lines))

                elif cli_input[0].lower() == 'x':
                    if self.validate_command(cli_input):
                        break

                else:
                    print(warning("Neispravna komanda; <help> za pomoc"))

            except ValueError as e:
                print(failure(e))

    def settings(self, cli_input: list):
        """Metod za podesavanje parametara okruzenja.
        Za sada podrzava menjanje dubine PageRank algoritma i broj rezultata u
        tabeli po strani (step).
        """
        if not self.validate_command(cli_input):
            return

        while True:
            clear_screen()

            lines = [
                "\t    Podesavanja\n",
                "<1>   Dubina PageRank algoritma: " +
                color_string(self.engine.depth, bcolors.CBEIGE),
                "<2>   Step za prikaz rezultata:  " +
                color_string(self.query.step, bcolors.CBEIGE),
                "<x>   Izlaz\n"
            ]
            print('\n'.join(lines))

            cli_input = input("# ").strip().split(' ')

            check_exit(cli_input)

            if cli_input[0].lower() == '1':
                if not self.validate_command(cli_input):
                    continue

                while True:
                    depth = input("Nova PageRank dubina [0,3]: ")

                    try:
                        depth = int(depth)
                        # maksimalna dubina algoritma je 3 zbog performansi
                        if depth >= 0 and depth <= 3:
                            self.engine.depth = depth
                            break

                        else:
                            print(warning("Neispravna PageRank dubina"))

                    except:
                        print(warning("Neispravan unos"))

            elif cli_input[0].lower() == '2':
                if not self.validate_command(cli_input):
                    continue

                while True:
                    step = input("Novi broj stranica za prikaz: ")

                    try:
                        step = int(step)
                        # ispis vise od 500 rezultata po strani nije praktican
                        if step >= 1 and depth <= 500:
                            self.query.step = step
                            break

                        else:
                            print(warning("Validan broj stranica za prikaz je [1,500]"))

                    except:
                        print(warning("Neispravan unos"))

            elif cli_input[0].lower() == 'x':
                if self.validate_command(cli_input):
                    break

            else:
                print(warning("Neispravna komanda"))

        clear_screen()

    def validate_command(self, cli_input: list, words: int = 1) -> bool:
        """Vrsi validaciju unosa sa tastature"""
        if len(cli_input) > words:
            print(warning('Neispravno koriscenje komande. Unesite <help> za pomoc.'))
            return False

        return True

    def __get_files(self) -> Set:
        """Rekurzivno dobavlja sve fajlove tipa HTML u direktorijumu pomocu os.walk()"""
        result = Set()

        for root, _, files in os.walk(self.path):
            for file in files:
                if file.endswith('.htm') or file.endswith('.html'):
                    result.add(os.path.join(root, file))

        return result

    def __get_abs_path(self, path) -> str:
        """Vraca apsolutnu putanju ukoliko je prosledjena relativna"""
        if not os.path.isabs(path):
            return os.path.abspath(path)
        return path
