"""
Modul sadrzi implementaciju Graph-a.
"""
from .set import Set

class VertexException(Exception):
    pass


class Graph(object):
    """Model usmerenog Grafa koji sadrzi 2 recnika za dolazne i odlazne ivice."""

    def __init__(self):
        self.outgoing = {}
        self.incoming = {}

    def __vertex_in_graph(self, ver: str) -> bool:
        """Proverava da li je cvor u grafu

        :param ver: cvor za proveru
        :type ver: str

        :returns: da li je u grafu
        :rtype: bool
        """
        if ver in self.outgoing:
            return True
        return False

    def __len__(self) -> int:
        """Broj cvorova u grafu"""
        return len(self.outgoing)

    def outgoing(self, ver: str) -> Set:
        """Vraca skup linkova od trenutnog cvora

        :param ver: trenutni cvor
        :type ver: str
        :raises: IndexError ukoliko cvor nije u grafu

        :returns: skup odlazecih linkova
        :rtype: Set
        """
        if self.__vertex_in_graph(ver):
            return self.outgoing[ver]
        raise VertexException('Vertex se ne nalazi u grafu.')

    def incoming(self, ver: str) -> Set:
        """Vraca skup linkova do trenutnog cvora

        :param ver: trenutni cvor
        :type ver: str
        :raises: IndexError ukoliko cvor nije u grafu
        
        :returns: skup dolazecih linkova
        :rtype: Set
        """
        if self.__vertex_in_graph(ver):
            return self.incoming[ver]
        raise VertexException('Vertex se ne nalazi u grafu.')

    def add_vertex(self, ver: str) -> None:
        """Dodaje cvor ako vec ne postoji u grafu

        :param ver: cvor za dodavanje
        :type ver: str
        """
        if not self.__vertex_in_graph(ver):
            self.outgoing[ver] = Set()
            self.incoming[ver] = Set()

    def add_edge(self, u: str, v: str) -> None:
        """Dodaje ivicu izmedju 2 cvora u graf. 
        Ako ne postoje u grafu baca Error

        :param u: cvor 1
        :type u: str
        :param v: cvor 2
        :type v: str
        :raises: IndexError neki od cvorova nije u grafu
        """
        if self.__vertex_in_graph(u) and self.__vertex_in_graph(v):
            # sprecava povratne linkove
            if u != v:
                self.outgoing[u].add(v)
                self.incoming[v].add(u)
        else:
            raise VertexException(
                f'Oba vertexa moraju da budu u grafu da bi se povezali. {u}, {v}')
