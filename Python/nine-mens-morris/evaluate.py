"""
Modul za evaluaciju tabele. Sadrzi 7 glavnih heuristika.
"""
from consts import *
from board import Board


class Evaluate(object):

    @staticmethod
    def evboard(board: Board, p_board: Board, stage1: bool) -> int:
        closed_mor = Evaluate.closed_morris(board, p_board)
        morris_difference = Evaluate._all_morrises(board)

        blocked_pieces = 0
        pieces_num = 0
        two_piece = 0
        three_piece = 0
        double_morris = 0

        for piece in board.white:
            i, j = derive_index(piece)

            blocked_pieces -= Evaluate.is_blocked(board, i, j)
            pieces_num += 1
            if stage1:
                two_piece += Evaluate._two_piece(board.white, i, j)
                three_piece += Evaluate._three_piece(board.white, i, j)
            else:
                double_morris += Evaluate._double_morris(
                    board.white, board.black, i, j)

        for piece in board.black:
            i, j = derive_index(piece)

            blocked_pieces += Evaluate.is_blocked(board, i, j)
            pieces_num -= 1
            if stage1:
                two_piece -= Evaluate._two_piece(board.black, i, j)
                three_piece -= Evaluate._three_piece(board.black, i, j)
            else:
                double_morris -= Evaluate._double_morris(
                    board.black, board.white, i, j)

        if stage1:
            return 18 * closed_mor + \
                26 * morris_difference + \
                1 * blocked_pieces + \
                9 * pieces_num + \
                10 * two_piece + \
                7 * three_piece
        else:
            return 14 * closed_mor + \
                43 * morris_difference + \
                10 * blocked_pieces + \
                11 * pieces_num + \
                8 * double_morris

    @staticmethod
    def closed_morris(board: Board, p_board: Board) -> int:
        for morris in board.white_morrises:
            if morris not in p_board.white_morrises:
                return 1
        for morris in board.black_morrises:
            if morris not in p_board.black_morrises:
                return -1
        return 0

    @staticmethod
    def _all_morrises(board: Board) -> int:
        return len(board.white_morrises) - len(board.black_morrises)

    @staticmethod
    def is_blocked(board: Board, i: int, j: int) -> int:
        if board.is_empty(i + j):
            return 0

        id1 = (i + 1) % 8 + j   # field += 1
        id2 = (i + 7) % 8 + j   # field -= 1
        if board.is_empty(id1) or board.is_empty(id2):
            return 0

        if i % 2 == 1:
            if j == 8:
                if board.is_empty(i) or board.is_empty(i + 16):
                    return 0
            else:
                if board.is_empty(i + 8):
                    return 0
        return 1

    @staticmethod
    def _two_piece(pieces: list, i: int, j: int) -> int:
        id2 = (i + 1) % 8 + j  # + 1
        id3 = (i + 2) % 8 + j  # + 2
        id4 = (i + 7) % 8 + j  # - 1
        horizontal_condition = (id2 in pieces) and (
            id3 not in pieces) and (id4 not in pieces)

        if i % 2 == 0:
            if horizontal_condition:
                return 1
        else:
            if j == 0:
                id5 = i + 8
                id6 = i + 16
                if (id5 not in pieces and horizontal_condition) ^ \
                        (id5 in pieces and id6 not in pieces and id2 not in pieces and id4 not in pieces):
                    return 1
            elif j == 8:
                id5 = i
                id6 = i + 16
                if (id5 not in pieces and id6 not in pieces and horizontal_condition) ^ \
                        (id5 not in pieces and id6 in pieces and id2 not in pieces and id4 not in pieces):
                    return 1
            else:
                id5 = i + 8
                if (id5 not in pieces and horizontal_condition):
                    return 1
        return 0

    @staticmethod
    def _three_piece(pieces: list, i: int, j: int) -> int:
        id2 = (i + 1) % 8 + j  # + 1
        id3 = (i + 2) % 8 + j  # + 2
        id4 = (i + 7) % 8 + j  # - 1
        id5 = (i + 6) % 8 + j  # - 2

        if i % 2 == 0:
            if id2 in pieces and id4 in pieces and id3 not in pieces and id5 not in pieces:
                return 1
        else:
            if j == 0:
                id5 = i + 8
                id6 = i + 16

                if ((id4 in pieces and id5 not in pieces) ^ (id2 in pieces and id3 not in pieces)) and \
                        id5 in pieces and id6 not in pieces:
                    return 1
            elif j == 8:
                id5 = i
                id6 = i + 16

                if id5 not in pieces and id6 in pieces and \
                        ((id4 in pieces and id5 not in pieces) ^ (id2 in pieces and id3 not in pieces)):
                    return 1
        return 0

    @staticmethod
    def _double_morris_condition(owned: list, opposite: list, id1, id2, id3, id4, id5, id6):
        if (id1 in owned and id2 in owned and id3 in owned and id4 in owned) and \
            ((id5 in owned and id6 not in owned and id6 not in opposite) ^
             (id6 in owned and id5 not in owned and id5 not in opposite)):
            return 1
        return 0

    @staticmethod
    def _double_morris(owned: list, opposite: list, i: int, j: int) -> int:
        count = 0
        if i % 2 == 0:
            id1 = i + j
            id2 = (i + 1) % 8 + j
            id3 = (i + 2) % 8 + j

            id4 = (i + 3) % 8 + 16
            id5 = (i + 3) % 8 + 8
            id6 = (i + 3) % 8

            id7 = (i + 7) % 8 + 16
            id8 = (i + 7) % 8 + 8
            id9 = (i + 7) % 8

            if j == 0:
                count = Evaluate._double_morris_condition(
                    owned, opposite, id1, id2, id4, id5, id3, id6)
                count += Evaluate._double_morris_condition(
                    owned, opposite, id2, id3, id7, id8, id1, id9)

                id10 = i + 8
                id11 = i + 9
                id12 = (i + 2) % 8 + 8
                count += Evaluate._double_morris_condition(
                    owned, opposite, id1, id3, id10, id12, id2, id11)

            elif j == 8:
                count = Evaluate._double_morris_condition(
                    owned, opposite, id1, id2, id4, id6, id3, id5)
                count += Evaluate._double_morris_condition(
                    owned, opposite, id2, id3, id7, id9, id1, id8)

                id10 = i + 16
                id11 = i + 17
                id12 = (i + 2) % 8 + 16
                count += Evaluate._double_morris_condition(
                    owned, opposite, id1, id3, id10, id12, id2, id11)

            elif j == 16:
                count = Evaluate._double_morris_condition(
                    owned, opposite, id1, id2, id5, id6, id3, id4)
                count += Evaluate._double_morris_condition(
                    owned, opposite, id2, id3, id8, id9, id1, id7)

        return count
