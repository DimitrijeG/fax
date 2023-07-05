from Collections.graph import Graph
from Collections.trie import Trie
from Parsing.parser import Parser


class SearchEngine(object):
    """Model SE za HTML fajlove sa implementiranim PageRank algoritmom."""

    def __init__(self) -> None:
        self.trie = Trie()
        self.graph = Graph()

        # PageRank parametar (<=3)
        self.depth = 2

    def generate_data(self, files: list) -> None:
        """Generise Trie i Graph podatke dok cita listu fajlova

        :param files: lista fajlova koji se ucitavaju 
        :type files: list
        """
        parser = Parser()
        for file in files:
            words, links = parser.parse(file)

            # belezi lowercase rec sa trenutnim fajlom
            for word in words:
                self.trie.insert(word.lower(), file)

            # kreira cvor za svaki link trenutnog fajla (ako nisu vec kreirani)
            # i povezuje ih sa ivicom
            for link in links:
                self.graph.add_vertex(file)
                self.graph.add_vertex(link)
                self.graph.add_edge(file, link)

    def search(self, word: str) -> dict:
        """Pretrazuje Trie za prosledjenu rec, a potom rangira dobijene rezultate
        pomocu __page_rank() metode

        :param word: rec koja se pretrazuje 
        :type word: str
        """
        files_with_word = self.trie.search(word.lower())

        # rangiranje rezultata
        ranked_res = {}
        for file in files_with_word:
            ranked_res[file] = self.__page_rank(
                files_with_word, file, self.depth)

        return ranked_res

    def discard_results(self, results: dict, word: str) -> dict:
        """Ekskluduje rezultat pretrage iz prosledjene kolekcije fajlova
        Postoji samo zbog male optimizacije

        :param results: kolekcija fajlova koja ce biti redukovana 
        :type results: dict
        :param word: rec za koju ce se dobaviti kolekcija fajlova 
        :type word: str

        :returns: redukovane fajlove (NOT operator)
        :rtype: dict
        """
        to_discard = self.trie.search(word)

        discarded_res = {}
        for key, value in results.items():
            if key not in to_discard:
                discarded_res[key] = value

        return discarded_res

    def __page_rank(self, files_with_word: dict, key: str, depth: int) -> int:
        """Rekurzivni algoritam za evaluaciju stranice. Koristi modifikovanu 
        formulu PageRank algoritma.

        :param files_with_word: lookup recnik stranica 
                    sa brojem ponavljanja posmatrane reci 
        :type files_with_word: dict
        :param key: trenutna stranica ciji rank se racuna 
        :type key: str
        :param depth: dubina u koju ce rekurzija da ide
        :type depth: int

        :returns: rank prosledjene stranice
        :rtype: int
        """
        # rank je u pocetku broj ponavljanja kljucne reci
        rank = files_with_word[key] if key in files_with_word else 0

        # ako dubina nije 0 poziva se rekurzija za svaki dolazni link
        if depth != 0:
            for link in self.graph.incoming[key]:
                link_rank = self.__page_rank(files_with_word, link, depth - 1)

                # rank trenutne stranice se uvecava za kolicnik ranka
                # posmatranog linka i broja NJEGOVIH odlaznih linkova
                rank += link_rank / len(self.graph.outgoing[link])

        return rank
