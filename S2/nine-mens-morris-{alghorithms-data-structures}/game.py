"""
Glavni modul koji sadrzi logiku igre, minimax i game loop; takodje cuva stanje igre.
"""
from data_collections.list_dict import ListDict
from evaluate import Evaluate
from player import Player
from board import Board
from exceptions import *
from consts import *

from heuristics_test import *

import time


class Game(object):

    def __init__(self):
        self.board = None
        self.player = '0'
        self.move_counter = 0

        self.board_backup = None
        self.player_backup = 0
        self.move_counter_backup = 0

        self.evaluated_boards = None   # optimizacija cuva evaluirane table
        self.move_limit = 250

        self.initialize_game()

    def initialize_game(self):
        self.board = Board()
        self.move_counter = 0

        self.player = WHITE

        self.reset_hashed_boards()

    def backup_game(self) -> tuple:
        return (self.board.copy(), self.player, self.move_counter)

    def restore_game(self, backup):
        self.board = backup[0]
        self.player = backup[1]
        self.move_counter = backup[2]

    def switch_player(self):
        self.player = WHITE if self.player == BLACK else BLACK

    def reset_hashed_boards(self):
        self.evaluated_boards = {}

    def set_board_eval(self, p_board: Board, evaluation: int):
        self.evaluated_boards[p_board.hash() + self.board.hash()] = evaluation

    def get_board_eval(self, p_board: Board) -> int:
        return self.evaluated_boards.get(p_board.hash() + self.board.hash(), None)

    def minimax(self, depth: int, alpha: int, beta: int, p_board: Board):
        end = self.is_end()
        if end != 0:
            return end, self.board

        if depth == 0:
            evaluation = self.get_board_eval(p_board)

            if not evaluation:
                evaluation = Evaluate.evboard(
                    self.board, p_board, self.move_counter < 18)
                self.set_board_eval(p_board, evaluation)
            return evaluation, self.board

        best_moves = ListDict()
        backup = self.backup_game()

        if self.player == WHITE:
            max_eval = minsize

            for move in self.get_moves():
                self.board = move
                self.move_counter += 1
                self.switch_player()

                move_eval = self.minimax(depth - 1, alpha, beta, backup[0])[0]
                self.restore_game(backup)

                # last top move

                # max_eval = max(max_eval, move_eval)
                # if max_eval == move_eval:
                #     best_moves = move

                # random top move
                if move_eval > max_eval:
                    max_eval = move_eval
                    best_moves = ListDict()
                    best_moves.add_item(move)
                elif move_eval == max_eval:
                    best_moves.add_item(move)

                alpha = max(alpha, move_eval)
                if beta < alpha:
                    break

            return max_eval, best_moves.choose_random_item()

        elif self.player == BLACK:
            min_eval = maxsize

            for move in self.get_moves():
                self.board = move
                self.move_counter += 1
                self.switch_player()

                move_eval = self.minimax(depth - 1, alpha, beta, backup[0])[0]
                self.restore_game(backup)

                # last top move

                # min_eval = min(min_eval, move_eval)
                # if min_eval == move_eval:
                #     best_moves = move

                # random top move
                if move_eval < min_eval:
                    min_eval = move_eval
                    best_moves = ListDict()
                    best_moves.add_item(move)
                elif move_eval == min_eval:
                    best_moves.add_item(move)

                beta = min(beta, move_eval)
                if beta < alpha:
                    break

            return min_eval, best_moves.choose_random_item()

    def get_moves(self):
        moves = []
        board = self.board
        if self.player == WHITE:
            owned, opposite = board.white, board.black
        elif self.player == BLACK:
            owned, opposite = board.black, board.white

        # helper function
        def add_board_to_moves(new_board: Board):
            if Evaluate.closed_morris(new_board, board):
                for piece in opposite:
                    updated_board = new_board.copy()
                    if not updated_board.in_morris(piece, self.player):
                        updated_board.remove_piece(piece)
                        moves.append(updated_board)
            else:
                moves.append(new_board)

        all_pieces = owned.union(opposite)
        if self.move_counter < 18:   # faza 1    postavljanje
            for i in range(8):
                for j in (0, 8, 16):
                    if (i + j) not in all_pieces:
                        new_board = board.copy()
                        new_board.place_piece(i + j, self.player)
                        add_board_to_moves(new_board)
        else:                        # faza 2    pomeranje
            for piece_index in owned:
                i, j = derive_index(piece_index)

                indexes_to_check = []
                indexes_to_check.append((i + 1) % 8 + j)   # field += 1
                indexes_to_check.append((i + 7) % 8 + j)   # field -= 1
                if i % 2 == 1:
                    if j == 8:
                        indexes_to_check.append(i)
                        indexes_to_check.append(i + 16)
                    else:
                        indexes_to_check.append(i + 8)

                for index in indexes_to_check:
                    if board.is_empty(index):
                        new_board = board.copy()
                        new_board.move_piece(i + j, index)
                        add_board_to_moves(new_board)
        return moves

    def is_end(self) -> int:
        if self.move_counter < 18:
            return 0

        board = self.board
        blocked1, blocked2 = True, True

        for piece in board.white:
            i, j = derive_index(piece)
            if not Evaluate.is_blocked(board, i, j):
                blocked1 = False
                break

        for piece in board.black:
            i, j = derive_index(piece)
            if not Evaluate.is_blocked(board, i, j):
                blocked2 = False
                break

        if len(board.white) <= 2 or blocked1:
            return minsize
        if len(board.black) <= 2 or blocked2:
            return maxsize
        return 0

    def player_move(self, player, ai_depth):
        if player == HUMAN:
            human = Player(self.board, self.move_counter < 18, self.player)
            self.board = human.make_move()

            self.switch_player()
            self.move_counter += 1

        elif player == AI:

            start = time.time()
            self.board = self.minimax(ai_depth, minsize, maxsize, None)[1]
            end = time.time()
            print('\nEvaluation time: {}s'.format(round(end - start, 7)))

            self.switch_player()
            self.move_counter += 1

    def play(self, player1, player2, ai_depth=3):
        self.initialize_game()
        exit = False
        end = 0

        print("\n\t\tMICE\n")
        print(f"Ai depth: {ai_depth}")

        while not exit:
            end = self.is_end()
            if end != 0:
                break

            if self.move_counter == 18:
                self.reset_hashed_boards()
            elif self.move_counter > self.move_limit:
                raise TooManyMovesException(self.move_limit)

            print('\n\n\n' + str(self.board))

            try:
                self.player_move(player1, ai_depth)
            except ExitException:
                exit = True

            player1, player2 = player2, player1

        if not exit:
            print('\n\n\n' + str(self.board))

        if end == maxsize:
            print("\nPobedio je plavi igrac.")
        elif end == minsize:
            print("\nPobedio je zuti igrac.")
        else:
            print("\nNema pobednika")

        print("\nBroj odigranih poteza:", self.move_counter)
