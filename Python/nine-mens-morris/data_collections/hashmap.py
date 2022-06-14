"""
Modul sadrzi implementacije hes mape
"""
from .map import Map, MapElement
import random


class HashMap(object):
    """
    Klasa modeluje hes mapu
    """

    def __init__(self, capacity=36):
        """
        Konstruktor

        Argumenti:
        - `capacity`: inicijalni broj mesta u lookup nizu
        - `prime`: prost broj neophodan hes funkciji
        """
        self._data = capacity * [None]
        self._capacity = capacity
        self._size = 0
        self.prime = 109345121

        # konstante hesiranja
        self._a = 1 + random.randrange(self.prime-1)
        self._b = random.randrange(self.prime)

    def __len__(self):
        return self._size

    def _hash(self, x):
        """
        Hes funkcija

        Izracunavanje hes koda vrsi se po formuli (ax + b) mod p.

        Argument:
        - `x`: vrednost ciji se kod racuna
        """
        hashed_value = (hash(x)*self._a + self._b) % self.prime
        compressed = hashed_value % self._capacity
        return compressed

    def _resize(self, capacity):
        """
        Skaliranje broja raspolozivih slotova

        Metoda kreira niz sa unapred zadatim kapacitetom u koji
        se prepisuju vrednosti koje se trenutno nalaze u tabeli.

        Argument:
        - `capacity`: kapacitet novog niza
        """
        old_data = list(self.items())
        self._data = capacity * [None]
        self._size = 0

        # prepisivanje podataka u novu tabelu
        for (k, v) in old_data:
            self[k] = v

    def __getitem__(self, key):
        """
        Pristup elementu sa zadatim kljucem

        Apstraktna metoda koja opisuje pristup elementu na osnovu
        njegovog kljuca. Implementacija pristupa bucketu varira u
        zavisnosti od nacina resavanja kolizija.

        Argument:
        - `key`: kljuc elementa kome se pristupa
        """
        bucket_index = self._hash(key)
        return self._bucket_getitem(bucket_index, key)

    def __setitem__(self, key, value):
        bucket_index = self._hash(key)
        self._bucket_setitem(bucket_index, key, value)

        # povecaj broj raspolozivih mesta
        current_capacity = len(self._data)
        if self._size > current_capacity // 2:
            self._resize(2*current_capacity - 1)

    def __delitem__(self, key):
        bucket_index = self._hash(key)
        self._bucket_delitem(bucket_index, key)
        self._size -= 1

    def items(self):
        raise NotImplementedError()
# -----------------------------
    def keys(self):
        raise NotImplementedError()

    def values(self):
        raise NotImplementedError() 
# -----------------------------
    def _bucket_getitem(self, index, key):
        raise NotImplementedError()

    def _bucket_setitem(self, index, key, value):
        raise NotImplementedError()

    def _bucket_delitem(self, index, key):
        raise NotImplementedError()

class ChainedHashMap(HashMap):
    """
    Klasa modeluje hes mapu koja kolizije resava ulancavanjem
    """
    def _bucket_getitem(self, i, key):
        """
        Pristup elementu u okviru bucketa

        Metoda najpre pristupa bucketu sa zadatim indeksom. Ukoliko
        bucket postoji, pretrazuje se. Ako je element pronadjen metoda
        vraca njegovu vrednostu, dok se u suprotnom podize odgovarajuci
        izuzetak.

        Argumenti:
        - `i`: indeks bucketa
        - `key`: kljuc elementa
        """
        bucket = self._data[i]
        if bucket is None:
            raise KeyError('Ne postoji element sa trazenim kljucem.')

        return bucket[key]

    def _bucket_setitem(self, bucket_index, key, value):
        """
        Postavljanje vrednosti elementa sa zadatim kljucem

        Metoda najpre pronalazi bucket sa zadatim indeksom, a zatim
        dodaje novi element ili azurira postojeci na osnovu zadatog
        kljuca. Ukoliko bucket ne postoji, kreira se novi.

        Argumenti:
        - `i`: indeks bucketa
        - `key`: kljuc elementa
        - `value`: vrednost elementa
        """
        bucket = self._data[bucket_index]
        if bucket is None:
            self._data[bucket_index] = Map()

        # broj elemenata u mapi se menja samo u slucaju da je doslo do
        # dodavanja, dok azuriranje ne menja broj elemenata
        current_size = len(self._data[bucket_index])
        self._data[bucket_index][key] = value
        if len(self._data[bucket_index]) > current_size:
            self._size += 1

    def _bucket_delitem(self, bucket_index, key):
        """
        Brisanje elementa sa zadatim kljucem

        Metoda najpre pristupa bucketu sa zadatim indeksom. Ukoliko
        bucket postoji, pretrazuje se. Ako je element pronadjen vrsi
        se njegovo brisanje, u suprotnom se podize odgovarajuci
        izuzetak.

        Argumenti:
        - `i`: indeks bucketa
        - `key`: kljuc elementa
        """
        bucket = self._data[bucket_index]
        if bucket is None:
            raise KeyError('Ne postoji element sa trazenim kljucem.')

        del bucket[key]

    def __iter__(self):
        for bucket in self._data:
            if bucket is not None:
                for key in bucket:
                    yield key

    def items(self):
        for bucket in self._data:
            if bucket is not None:
                for key, value in bucket.items():
                    yield key, value


class LinearHashMap(HashMap):
    """
    Klasa modeluje hes mapu koja resava kolizije linearnim pretrazivanjem
    """
    # sentinel koji oznacava lokaciju u mapi sa koje je obrisan element
    _MARKER = object()

    def _is_available(self, bucket_index):
        """
        Metoda proverava da li je bucket sa zadatim indeksom slobodan

        Argument:
        - `i`: indeks bucketa
        """
        return self._data[bucket_index] is None or self._data[bucket_index] is self._MARKER

    def _find_bucket(self, bucket_index, key):
        """
        Metoda pronalazi bucket koji sadrzi element sa zadatim kljucem
        """
        available_slot = None
        while True:
            if self._is_available(bucket_index):
                if available_slot is None:
                    available_slot = bucket_index

                if self._data[bucket_index] is None:
                    return False, available_slot

            elif key == self._data[bucket_index].key:
                return True, bucket_index

            bucket_index = (bucket_index+1) % len(self._data)

    def _bucket_getitem(self, bucket_index, key):
        """
        Pristup elementu u okviru bucketa

        Metoda najpre pristupa bucketu sa zadatim indeksom. Ukoliko
        bucket postoji, pretrazuje se. Ako je element pronadjen metoda
        vraca njegovu vrednostu, dok se u suprotnom podize odgovarajuci
        izuzetak.

        Argumenti:
        - `i`: indeks bucketa
        - `key`: kljuc elementa
        """
        found, index = self._find_bucket(bucket_index, key)
        if not found:
            return None     # edited
        return self._data[index].value

    def _bucket_setitem(self, bucket_index, key, value):
        """
        Postavljanje vrednosti elementa sa zadatim kljucem

        Metoda najpre pronalazi bucket sa zadatim indeksom, a zatim
        dodaje novi element ili azurira postojeci na osnovu zadatog
        kljuca. Ukoliko bucket ne postoji, kreira se novi.

        Argumenti:
        - `i`: indeks bucketa
        - `key`: kljuc elementa
        - `value`: vrednost elementa
        """
        found, available_bucket_index = self._find_bucket(bucket_index, key)
        if not found:
            self._data[available_bucket_index] = MapElement(key, value)
            self._size += 1
        else:
            self._data[available_bucket_index].value = value

    def _bucket_delitem(self, bucket_index, key):
        """
        Brisanje elementa sa zadatim kljucem

        Metoda najpre pristupa bucketu sa zadatim indeksom. Ukoliko
        bucket postoji, pretrazuje se. Ako je element pronadjen vrsi
        se njegovo brisanje, u suprotnom se podize odgovarajuci
        izuzetak.

        Argumenti:
        - `i`: indeks bucketa
        - `key`: kljuc elementa
        """
        found, index = self._find_bucket(bucket_index, key)
        if not found:
            raise KeyError('Ne postoji element sa trazenim kljucem.')

        # oznaci element markerom
        self._data[index] = self._MARKER

    def __iter__(self):
        total_buckets = len(self._data)
        for i in range(total_buckets):
            if not self._is_available(i):
                yield self._data[i].key

    def items(self):
        total_buckets = len(self._data)
        for i in range(total_buckets):
            if not self._is_available(i):
                yield self._data[i].key, self._data[i].value
    
    def keys(self):
        total_buckets = len(self._data)
        for i in range(total_buckets):
            if not self._is_available(i):
                yield self._data[i].key
    
    def values(self):
        total_buckets = len(self._data)
        for i in range(total_buckets):
            if not self._is_available(i):
                yield self._data[i].value


# if __name__ == '__main__':
#     print("\nChained Hash Map\n---------------------")
#     hash_map = ChainedHashMap()
#     hash_map[3] = 10
#     hash_map[2] = 11
#     hash_map[55] = 11
#     hash_map[43] = 11
#     hash_map[24] = 11
#     hash_map[19] = 11
#     hash_map[190] = 11


#     print('Inicijalno dodavanje')
#     for item in hash_map:
#         print(item, hash_map[item])

#     print('Izmena vrednosti')
#     hash_map[3] = 5
#     for item in hash_map:
#         print(item, hash_map[item])

#     print('Brisanje elementa')
#     del hash_map[2]
#     for item in hash_map:
#         print(item, hash_map[item])

#     print("\nLinear Hash Map\n---------------------")
#     hash_map = LinearHashMap()
#     hash_map[3] = 10
#     hash_map[2] = 11

#     print('Inicijalno dodavanje')
#     for item in hash_map:
#         print(item, hash_map[item])

#     print('Izmena vrednosti')
#     hash_map[3] = 5
#     for item in hash_map:
#         print(item, hash_map[item])

#     print('Brisanje elementa')
#     del hash_map[2]
#     for item in hash_map:
#         print(item, hash_map[item])



