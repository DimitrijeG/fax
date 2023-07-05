from .search_engine import SearchEngine
from Collections.stack import Stack
from Collections.set import Set
from Utils.common import *
from Utils.colors import *

from Parsing.sample_file import sample_html_file
from Utils.sort import heap_sort
import time


class Query(object):
    """Klasa koja se bavi kreiranjem, obradom i evaluacijom sirovih upita, 
    kao i ispisom rezultata."""
    
    # prioritet operacija
    priority = {'NOT': 2, 'AND': 2, 'OR': 2, 'XOR': 2, '(': 1, ')': 1}
    
    def __init__(self) -> None:
        self.results_dict = {}
        
        # parametar koji je moguce menjati u podesavanjima
        self.step = 10

    def make_query(self, se: SearchEngine) -> None:
        """Funkcija koja kreira upit za pretragu"""

        cli_input = input(":")

        # validacija i formatiranje izraza za korisnicku potvrdu
        tokens = self.__validate(cli_input)
        output_tokens = []
        for token in tokens:
            if token in self.priority:
                color = bcolors.CBEIGE
            else:
                color = bcolors.CGREY
                token = token.lower()
            output_tokens += [color_string(token, color)]
        
        if not output_tokens:
            print(warning("Neispravan upit"))
            return
        else:
            print("\nIzraz za pretragu:", ' '.join(output_tokens))

        # prompt da li nastaviti sa racunanjem
        while True:
            cli_input = input("(y/n): ")

            if cli_input == 'n':
                return
            elif cli_input == 'y':
                print("")
                break
            else:
                print(warning("Neispravan unos"))

        # evaluacija izraza
        time_start = time.time()
        results, words = self.__evaluate(tokens, se)
        time_end = time.time()

        max_path_len = self.__get_max_len(results)

        if not results:
            print("Nema rezultata pretrage\n")
            return

        # loop ispisa rezultata u kom je omoguceno scrollovanje kroz stranice
        start = 0
        while True:
            clear_screen()
            print(color_string(
                f"\nElapsed time: {round(time_end - time_start, 7)}s", 
                bcolors.CGREY
            ))

            print("Rezultati {}-{} / {}\n".format(
                start, 
                min(start + self.step, len(results)), 
                len(results)
            ))

            self.__display_results(results, start, self.step, words, max_path_len)

            print('')
            if start != 0:
                print("<p>  <- strana")
            if start + self.step < len(results):
                print("<n>     strana ->")
            print("<x>  nazad\n")
            while True:
                cli_input = input(":")
                check_exit(cli_input.split(' '))

                if cli_input == 'p' and start != 0:
                    start -= self.step
                    break
                elif cli_input == 'n' and start + self.step < len(results):
                    start += self.step
                    break
                elif cli_input == 'x':
                    return
                else:
                    print(warning("Neispravan unos"))
                

    def __evaluate(self, expression: list[str], se: SearchEngine) -> tuple[dict, list]:
        """Vrsi evaluaciju dobijene liste tokena pomocu postfiks notacije.
        Kalkulacije sprovodi pomocu Steka.

        :param expression: validiran izraz 
        :type expression: list
        :param se: SE za pretragu reci i rangiranje
        :type se: SearchEngine

        :returns: rezultat izraza u vidu recnika stranica sa rankovima i liste reci
        :rtype: tuple
        """
        tokens = self.__to_postfix(expression)

        result = {}
        words = []   # kozmeticke svrhe
        if len(tokens) == 1:
            # slucaj kada je prosledjena 1 rec
            result = se.search(tokens[0])
            words = [tokens[0]]
        else:
            operands = Stack()
            for token in tokens:
                if token not in self.priority:
                    # token nije operacije tako da se loguje kao rec
                    operands.push(token)
                    words.append(token)
                else:
                    # token je operacija tako da se izvrsava
                    second = operands.pop()
                    first = operands.pop()

                    if not isinstance(first, dict):
                        # provera da li je operand rec ili prethodno 
                        # izracunat rezultat
                        first_dict = se.search(first)
                    else:
                        first_dict = first

                    if not isinstance(second, dict):
                        if token == 'NOT':
                            # nepotrebna optimizacija operacije
                            result = se.discard_results(first_dict, second)
                            operands.push(result)
                            continue
                        second_dict = se.search(second)

                    else:
                        second_dict = second

                    # snimanje operanada u argument klase zbog rankova 
                    # i konverzija u skupove jer se oni koriste u izracunavanju
                    first_set = self.__log_result(first_dict)
                    second_set = self.__log_result(second_dict)

                    # izvrsavanje operacije nad skupovima
                    result = self.__calculate(token, first_set, second_set)
                    # 'nalepljivanje' prethodno snimljenih rankova
                    result = self.__enrich_result(result)
                    
                    operands.push(result)
            result = operands.pop()

        # sortiranje i kompresija rankova
        return self.__finalize_result(result), words
    

    def __display_results(
        self, results: list[tuple[int, str]], start: int, step: int, 
        words: list, max_path_len: int
    ) -> None:
        """Ispisuje listu rezultata u vidu tabele kao i 
        isecak stranice za top rezultat."""
        for i in range(start, min(start + step, len(results))):
            # zaokruzivanje rangova
            rang = '%.2f' % results[i][0] if int(results[i][0]) != 10 else '10.0'

            print(f"{f'{i + 1}.' : <5}| {rang : ^4} | {results[i][1] : <{max_path_len + 2}}")

            if i == 0:
                # ispis primerka html stranice samo za top rezultat
                sample_html_file(results[i][1], words, max_path_len + 30)
    
    def __get_max_len(self, results: list[tuple[int, str]]) -> int:
        """Vraca max duzinu linka (koristi se za ispis)"""
        max_len = 0
        for result in results:
            max_len = max(max_len, len(result[1]))
        return max_len


    def __calculate(self, operator: str, a: Set, b: Set) -> Set:
        """Poziva odgovarajuce metode Set klase za operacije nad skupovima"""
        if operator == 'NOT':
            return a.difference(b)
        elif operator == 'AND':
            return a.intersection(b)
        elif operator == 'OR':
            return a.union(b)
        else:
            return a.exclusive_union(b)

    def __log_result(self, result: dict) -> Set:
        """Konvertuje recnik stranica sa rankovima u skup stranica dok pritom
        belezi rankove u atribut klase 'results_dict'. Ako prilikom cuvanja rankova
        dodje do kolizije na rank se dodaje dodata vrednost.

        :param result: kolekcija stranica sa rankovima 
        :type result: dict

        :returns: skup stranica
        :rtype: Set
        """
        set_result = Set()
        for key, val in result.items():
            set_result.add(key)
            if key in self.results_dict:
                # ako je stranica vec logovana, rank se uvecava za dodat
                self.results_dict[key] += val
            else:
                self.results_dict[key] = val

        return set_result
    
    def __enrich_result(self, result: Set) -> dict:
        """Koristeci argument klase sa prethodno mapiranim rankovima 
        mapira redukovan skup stranica na te rankove i vraca dobijeni recnik.

        :param result: rezultat bez rankova 
        :type result: Set

        :returns: rezultat sa rankovima
        :rtype: dict
        """
        dict_result = {}
        for key in result:
            dict_result[key] = self.results_dict[key]
        self.results_dict.clear()

        return dict_result
    
    def __finalize_result(self, results: dict) -> list[tuple[int, str]]:
        """Sortira i kompresuje rankove rezultata

        :param results: recnik stranica sa rankovima 
        :type results: dict

        :returns: listu rezultata pogodnu za ispis (rank, page)
        :rtype: list
        """
        if not results:
            return []

        final_results = []
        result_list = [(rang, page) for page, rang in results.items()]

        # sortira rezultate koristeci modifikovani Heap Sort
        sorted_results = heap_sort(result_list)

        # kompresuje rankove proporcionalno tako da najveci ima vrednost 10.00
        max_rang = sorted_results[0][0]
        for rang, page in sorted_results:
            final_results.append((rang / max_rang * 10, page))

        return final_results


    def __to_postfix(self, expression: list) -> list:
        """Konverzija izraza u postfiksnu notaciju pomocu Steka"""

        postfix = []
        operators = Stack()
        for token in expression:
            if token not in self.priority:
                # operandi se beleze u izlazni bafer
                postfix.append(token)
            elif token == '(':
                # leve zagrade dodaju se na stek
                operators.push(token)
            elif token == ')':
                # ako je token desna zagrada, izbaci sve sa steka do prve leve
                top = operators.pop()
                while top != '(':
                    postfix.append(top)
                    top = operators.pop()
            else:
                # operatori se dodaju na stek, ali se najpre svi "stariji" izbacuju
                while not operators.is_empty() and \
                    self.priority[operators.top()] >= self.priority[token]:
                    postfix.append(operators.pop())
                operators.push(token)

        # generisanje rezultujuceg izraza
        while not operators.is_empty():
            postfix.append(operators.pop())

        return postfix

    def __validate(self, raw_expr: str) -> list[str]:
        """Bavi se validacijom i formatiranjem upita

        :param raw_expr: upit za obradu dobijen iz konzole 
        :type raw_expr: str
        :raises: ValueError ako prosledjeni upit nije validan

        :returns: obradjen upit u vidu tokena
        :rtype: list
        """
        # izolovanje zagrada razmacima
        raw_expr = raw_expr.replace('(', ' ( ').replace(')', ' ) ')

        # izdvajanje neobradjenih tokena
        tokens = [x.strip() for x in raw_expr.split(' ') if x]
        clean_tokens = tokens.copy()

        # brisanje nepotrebnih zagrada ukoliko postoje
        j = 0
        for i in range(len(tokens) - 2):
            if tokens[i] == '(' and tokens[i + 2] == ')':
                if tokens[i + 1] not in self.priority:
                    del clean_tokens[i - j]
                    del clean_tokens[i + 1 - j]
                    j += 2
                else:
                    raise ValueError('Neispravan upit.')
        
        validated = []
        # flag koji ukazuje da li operacija sme da bude sledeci token
        operation = False
        # proverava da li su sve otvorene zagrade, zatvorene
        brackets = 0

        # validacija upita i dodavanje OR operacije 
        # izmedju reci odvojenih razmakom
        for token in clean_tokens:
            # najpre se proveravaju zagrade
            if token == '(':
                validated.append(token)
                brackets += 1
                operation = False

            elif token == ')':
                validated.append(token)
                brackets -= 1
                operation = True

            # ako je operacija na True, ipak je dozvoljeno da se pojavi rec,
            # i u tom slucaju se ispred ubacuje OR
            elif operation:
                if token not in self.priority:
                    validated.append('OR')
                    validated.append(token)
                    operation = True

                else:
                    validated.append(token)
                    operation = False

            # ako nije red na operaciju a ona se ipak pojavi, izbacuje se
            # ValueError zato sto upit nije validan
            else:
                if token not in self.priority:
                    validated.append(token)
                    operation = True

                else:
                    raise ValueError('Neispravan upit.')

        # konacno, ako broj zatvorenih i otvorenih zagrada nije jednak, ili
        # je upit zavrsen operacijom, pogodili ste, upit nije validan
        if brackets != 0 or operation == False:
            raise ValueError('Neispravan upit.')

        return validated
