"""
Modul Tabla koji cuva pozicije figura i sklopljene mice. Takodje, zaduzen je za
bilo kakve akcije nad tablom ukljucujuci postavljanje, pomeranje, brisanje figura 
kao i hesiranje i kloniranje tabele.
"""
from consts import *


class Board(object):

    def __init__(self, table=[EMPTY] * 24, white=None, black=None, white_morrises=None, black_morrises=None):
        if type(table) == str:
            table = list(table.split())

        self.table = table

        if not white:
            white = set()
            black = set()
            for i in range(24):
                if table[i] == WHITE:
                    white.add(i)
                elif table[i] == BLACK:
                    black.add(i)
        self.white = white
        self.black = black

        if not white_morrises:
            white_morrises = self.calculate_morrises(white)
            black_morrises = self.calculate_morrises(black)
        self.white_morrises = white_morrises
        self.black_morrises = black_morrises

    def __str__(self) -> str:
        fields = []
        for i in key_order:
            field = self.table[i]
            if field == WHITE:
                fields.append(f'{bcolors.OKCYAN}●{bcolors.ENDC}')  # ○ ⚪
            elif field == BLACK:
                fields.append(f'{bcolors.WARNING}●{bcolors.ENDC}')  # ● ⚫
            else:
                fields.append('.')
        return table_template2.format(*fields)

    def copy(self):
        table = self.table.copy()
        white = self.white.copy()
        black = self.black.copy()
        white_morrises = self.white_morrises.copy()
        black_morrises = self.black_morrises.copy()
        return Board(table, white, black, white_morrises, black_morrises)

    # hash tabele za optimizacije
    def hash(self) -> str:
        return ''.join(self.table)

    # prazno polje
    def is_empty(self, index: int) -> bool:
        return self.table[index] == EMPTY

    def in_morris(self, index: int, player) -> bool:
        if player == WHITE:
            morrises = self.white_morrises
            pieces = self.white
        else:
            morrises = self.black_morrises
            pieces = self.black
            
        if len(morrises) * 3 == pieces:
            return False
        for morris in morrises:
            if index in morris:
                return True

        return False

    # postavljanje
    def place_piece(self, index: int, value: str) -> None:
        self.table[index] = value
        if value == WHITE:
            self.white.add(index)
            self.white_morrises = self.calculate_morrises(self.white)
        else:
            self.black.add(index)
            self.black_morrises = self.calculate_morrises(self.black)

    # pomeranje
    def move_piece(self, index1: int, index2: int) -> None:
        self.table[index1], self.table[index2] = self.table[index2], self.table[index1]

        if self.table[index2] == WHITE:
            self.white.remove(index1)
            self.white.add(index2)
            self.white_morrises = self.calculate_morrises(self.white)
        elif self.table[index2] == BLACK:
            self.black.remove(index1)
            self.black.add(index2)
            self.black_morrises = self.calculate_morrises(self.black)

    # brisanje figure
    def remove_piece(self, index: int) -> None:
        value = self.table[index]
        if value == WHITE:
            self.white.remove(index)
            self.white_morrises = self.calculate_morrises(self.white)
        elif value == BLACK:
            self.black.remove(index)
            self.black_morrises = self.calculate_morrises(self.black)
        self.table[index] = EMPTY

    def calculate_morrises(self, pieces) -> set:
        morrises = set()

        for piece in pieces:
            i, j = derive_index(piece)

            if i % 2 == 0:
                index2 = (i + 1) % 8 + j   # field += 1
                index3 = (i + 2) % 8 + j   # field += 2
                if index2 in pieces and index3 in pieces:
                    morrises.add((i + j, index2, index3))

            if i % 2 == 1:
                if i in pieces and (i + 8) in pieces and (i + 16) in pieces:
                    morrises.add((i, i + 8, i + 16))

        return morrises
