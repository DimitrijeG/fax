"""
Modul Igrac koji je zaduzen za sve interakcije sa korisnikom.
"""
from exceptions import ExitException, TooManyTriesException
from evaluate import Evaluate
from board import Board
from consts import *


class Player(object):

    def __init__(self, board: Board, stage1: bool, player) -> None:
        self.old_board = board
        self.board = board.copy()
        self.stage1 = stage1
        self.player = player

        self.try_limit = 10

    def make_move(self) -> Board:

        if self.stage1:
            valid_moves = self.get_moves(mode=PLACE)
            field = self.enter_field(valid_moves, "Postavite figuru: ")

            self.board.place_piece(field, self.player)

        else:
            valid_moves = self.get_moves(mode=MOVE)
            selected = self.select_piece(
                list(valid_moves.keys()), "Izaberite figuru: ")
            field = self.move_piece(
                valid_moves[selected], "Polje na koje pomerate figuru: ")

            self.board.move_piece(selected, field)

        if Evaluate.closed_morris(self.board, self.old_board):
            print('\n\n\n' + str(self.board))
            valid_moves = self.get_moves(mode=DELETE)
            if valid_moves:
                field = self.select_piece(valid_moves, "Brisete figuru: ")
                self.board.remove_piece(field)

        return self.board

    def get_moves(self, mode):
        if self.player == WHITE:
            owned, opposite = self.board.white, self.board.black
            opposite_morrises = self.board.black_morrises
        else:
            owned, opposite = self.board.black, self.board.white
            opposite_morrises = self.board.white_morrises

        if mode == PLACE:
            valid_moves = []
            all_pieces = owned.union(opposite)

            for i in range(24):
                if i not in all_pieces:
                    valid_moves.append(i)

        elif mode == MOVE:
            valid_moves = LinearHashMap()

            for piece in owned:
                i, j = derive_index(piece)

                if not Evaluate.is_blocked(self.board, i, j):
                    neighbours = [
                        (i + 1) % 8 + j, (i + 7) % 8 + j
                    ]
                    if i % 2 == 1 and j == 8:
                        neighbours.append(i)
                        neighbours.append(i + 16)
                    elif i % 2 == 1:
                        neighbours.append(i + 8)

                    valid_indexes = []
                    for index in neighbours:
                        if self.board.is_empty(index):
                            valid_indexes.append(index)
                    valid_moves[piece] = valid_indexes

        else:  # mode = DELETE
            valid_moves = []
            for piece in opposite:
                if not self.board.in_morris(piece, self.player):
                    valid_moves.append(piece)

        return valid_moves

    def enter_field(self, valid_moves: list, output: str) -> int:
        self.print_valid_moves(valid_moves)
        for _ in range(self.try_limit):
            field = self.input_index(output)

            if field < 0:
                self.print_warning("\nUnos koordinata nije validan.\n")
            elif field in valid_moves:
                return field
            else:
                self.print_warning("\nPolje je zauzeto.\n")

        raise TooManyTriesException(self.try_limit)

    def select_piece(self, valid_moves: list, output: str) -> int:
        self.print_valid_moves(valid_moves)
        for _ in range(self.try_limit):
            piece = self.input_index(output)

            if piece < 0:
                self.print_warning("\nUnos koordinata nije validan.\n")
            elif piece in valid_moves:
                return piece
            else:
                self.print_warning("\nMozete da selektujete samo svoje figure.\n")

        raise TooManyTriesException(self.try_limit)

    def move_piece(self, valid_moves: list, output: str) -> int:
        self.print_valid_moves(valid_moves)
        for _ in range(self.try_limit):
            field = self.input_index(output)

            if field < 0:
                self.print_warning("\nUnos koordinata nije validan.\n")
            elif field in valid_moves:
                return field
            else:
                self.print_warning("\nUnesite susedno slobodno polje.\n")

        raise TooManyTriesException(self.try_limit)

    def input_index(self, output: str) -> int:
        piece = input(output)
        if piece == "exit":
            raise ExitException()

        if piece and piece != '' and len(piece) == 2 \
                and piece[0].isalpha() and piece[1].isnumeric \
                and piece.upper() in coordinate_map.keys():
            return coordinate_map[piece.upper()]
        return -1

    def print_valid_moves(self, valid_moves: list) -> None:
        valid_moves = [coordinate_map_inv[i] for i in valid_moves]
        if self.player == WHITE:
            color = bcolors.OKCYAN
        else:
            color = bcolors.WARNING

        print(f"\nValidni potezi: {color}" + ' '.join(sorted(valid_moves)) + f'{bcolors.ENDC}\n')

    def print_warning(self, output: str) -> None:
        print(f"{bcolors.WARNING}{output}{bcolors.ENDC}")
