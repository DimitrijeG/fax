"""
Modul sadrzi implementaciju steka na osnovu liste.
"""


class EmptyStackError(Exception):
    """Klasa modeluje izuzetke vezane za klasu Stack."""
    pass


class FullStackError(Exception):
    """Klasa modeluje izuzetke vezane za klasu Stack."""
    pass


class Stack(object):
    """Implementacija steka na osnovu liste."""

    def __init__(self, capacity=10):
        self._data = []
        self._capacity = capacity

    def __len__(self):
        return len(self._data)

    def is_empty(self):
        """Metoda proverava da li je stek prazan."""
        return len(self._data) == 0

    def push(self, e):
        """Metoda vrsi ubacivanje elementa na stek.

        :param e: novi element
        :type e: Any
        """
        if len(self._data) >= self._capacity:
            FullStackError("Stek je pun")
        self._data.append(e)

    def top(self):
        """Metoda vraca element na vrhu steka."""
        if self.is_empty():
            raise EmptyStackError('Stek je prazan.')
        return self._data[-1]

    def pop(self):
        """Metoda izbacuje element sa vrha steka."""
        if self.is_empty():
            raise EmptyStackError('Stek je prazan.')
        return self._data.pop()
