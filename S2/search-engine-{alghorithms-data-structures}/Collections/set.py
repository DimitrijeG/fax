"""
Modul sadrzi implementaciju Set-a.
"""
from __future__ import annotations


class SetElementException(Exception):
    pass


class Set(object):
    """Implementacija skupa na osnovu recnika.

    :param elems: inicijalna kolekcija elemenata koji se dodaju
    :type elems: iter
    """

    def __init__(self, elems=[]) -> None:
        self.rep = {}
        self.extend(elems)

    def __contains__(self, x) -> bool:
        return x in self.rep

    def __len__(self) -> int:
        return len(self.rep)

    def __iter__(self) -> iter:
        """Vraca iterator kroz kljuceve internog recnika.

        :returns: iterator nastao kopiranjem internog dict-a
        :rtype: iter
        """
        return iter(self.rep.copy())

    def add(self, x) -> None:
        self.rep[x] = x

    def extend(self, elems: iter) -> None:
        """Dodaje nove elemente filtrirajuci duplikate.

        :param elems: kolekcija elemenata za dodavanje
        :type elems: iter
        """
        for x in elems:
            self.add(x)

    def remove(self, x) -> None:
        """Brise prosledjenu vrednost iz skupa.

        :param x: vrednost za brisanje
        :type x: Any
        :raises: IndexError ukoliko se element ne nalazi u skupu
        """
        if x in self.rep:
            del self.rep[x]
        else:
            raise SetElementException('Element za brisanje nije u setu')

    def items(self) -> list:
        """Vraca listu kljuceva tj. elemenata skupa.

        :returns: elemente
        :rtype: list
        """
        return list(self.rep.keys())

    def copy(self) -> Set:
        """Kopira posmatrani skup u novi preko copy() metoda klase dict.

        :returns: kopirani skup
        :rtype: Set
        """
        c = Set()
        c.rep = self.rep.copy()
        return c

    """Metode operacija sa drugim setom"""
    # OR

    def union(self, s) -> Set:
        r = self.copy()
        for elem in s:
            r.add(elem)
        return r

    # AND
    def intersection(self, s) -> Set:
        r = Set()
        for elem in s:
            if elem in self:
                r.add(elem)
        return r

    # NOT
    def difference(self, s) -> Set:
        r = Set()
        for elem in s:
            if elem not in self:
                r.add(elem)
        return r

    # XOR
    def exclusive_union(self, s) -> Set:
        r = Set()
        t = self.union(s)
        for elem in t:
            if elem not in self or elem not in s:
                r.add(elem)
        return r
