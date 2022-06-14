from board import Board
from evaluate import *
import pytest


def get_board(table) -> Board:
    table = [x for x in table]
    return Board(table)


@pytest.mark.parametrize(
    "board,p_board,expected", [
        (get_board("111020122220111210212021"),
         get_board("111020122221011210212021"), 1),
        (get_board("121020122221011212012021"),
         get_board("121020122221011210212021"), -1)
    ]
)
def test_closed_morris(board, p_board, expected):
    assert Evaluate.closed_morris(board, p_board) == expected


@pytest.mark.parametrize(
    "board,expected", [
        (get_board("111020120220111210212021"), 2),
        (get_board("101020122221011212012021"), -1),
        (get_board("101020120220121210212021"), 0),
        (get_board("111020110221011212012021"), 2),
        (get_board("222020110221111212012021"), -1)
    ]
)
def test_all_morrises(board, expected):
    assert Evaluate._all_morrises(board) == expected


@pytest.mark.parametrize(
    "board,i,j,expected", [
        (get_board("120020020220101210212120"), 0, 0, 1),
        (get_board("120020020220101210212120"), 4, 16, 1)
    ]
)
def test_blocked_piece(board, i, j, expected):
    assert Evaluate.is_blocked(board, i, j) == expected


@pytest.mark.parametrize(
    "board,i,j,expected", [
        (get_board("120020010220101210212120").white, 7, 0, 1),
        (get_board("110020010220101210212120").black, 1, 8, 1),
        (get_board("110020010222101210212120").black, 1, 8, 0),
        (get_board("110020010222101210212120").black, 2, 8, 0)
    ]
)
def test_two_piece(board, i, j, expected):
    assert Evaluate._two_piece(board, i, j) == expected


@pytest.mark.parametrize(
    "board,i,j,expected", [
        (get_board("110020010220101210212120").white, 0, 0, 1),
        (get_board("110020012220101210212120").black, 0, 8, 0),
        (get_board("120020010222101210212120").black, 2, 8, 1),
        (get_board("110020010222101210122120").black, 3, 8, 1)
    ]
)
def test_three_piece(board, i, j, expected):
    assert Evaluate._three_piece(board, i, j) == expected


@pytest.mark.parametrize(
    "owned,opposite,i,j,expected", [
        (get_board("111020011010101210212120").white, get_board(
            "111020011010101210212120").black, 0, 0, 1),
        (get_board("111020011011101210212120").white, get_board(
            "111020011011101210212120").black, 0, 0, 2),
        (get_board("111120011210101210212120").white, get_board(
            "111120011210101210212120").black, 0, 0, 0),
        (get_board("111222010022201220202220").black, get_board(
            "111222010022201220202220").white, 2, 8, 2),
        (get_board("121222012022202222202220").black, get_board(
            "121222012022202222202220").white, 6, 8, 3),
        (get_board("222111020011122110101110").white, get_board(
            "222111020011122110101110").black, 2, 8, 1),
        (get_board("111222020022211220202220").black, get_board(
            "111222020022211220202220").white, 4, 16, 2)
    ]
)
def test_double_morrises(owned, opposite, i, j, expected):
    assert Evaluate._double_morris(owned, opposite, i, j) == expected
