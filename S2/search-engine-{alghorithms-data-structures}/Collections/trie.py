"""
Modul sadrzi implementaciju Trie sa TrieNode.
"""


class TrieNode(object):
    """Klasa za modelovanje cvora Trie
    Sadrzi recnik potomaka mapiranih karakterima kao i recnik putanja do fajlova
    cija je vrednost broj ponavljanja same reci
    """

    def __init__(self):
        self.children = {}
        self.files = {}


class Trie(object):
    """Trie klasa
    Koristi TrieNode klasu za svoje cvorove. 
    Sadrzi metod za dodavanje i pretragu cvorova
    """

    def __init__(self):
        self.root = TrieNode()

    def insert(self, word: str, file: str) -> None:
        """Dodaje novi cvor u Trie. Krece se od 'root'-a dok ne dodje do kraja prosledjene reci
        Ako prilikom kretanja ne postoji vec cvor za neki karakter on se kreira.
        Na kraju se u recnik fajlova belezi dodati fajl.

        :param word: rec koja se dodaje
        :type word: str
        :param file: putanja do fajla koji se belezi
        :type file: str
        """
        node = self.root

        for char in word:
            if char not in node.children:
                node.children[char] = TrieNode()
            node = node.children[char]

        # ukoliko fajl nije dodat ranije inicijalizuje se u dict
        if file not in node.files:
            node.files[file] = 1
        else:
            node.files[file] += 1

    def search(self, word: str) -> dict:
        """Pretrazuje Trie pomocu word. U slucaju da je ne nadje, vraca prazan recnik.
        U suprotnom vraca recnik fajlova asociranih sa tom recju.

        :param word: rec koja se pretrazuje
        :type word: str

        :returns: recnik putanja do fajlova sa brojem ponavljanja reci u njima
        :rtype: dict
        """
        node = self.root
        for char in word:
            if char not in node.children:
                # rec nije nadjena
                return {}
            node = node.children[char]

        return node.files
