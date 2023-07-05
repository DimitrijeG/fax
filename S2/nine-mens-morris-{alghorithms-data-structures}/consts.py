from data_collections.hashmap import LinearHashMap
from sys import maxsize


table_template1 = '''{} ------- {} ------- {}
   {} ---- {} ---- {}   
      {} - {} - {}      
{}  {}  {} ----- {}  {}  {}
      {} - {} - {}      
   {} ---- {} ---- {}   
{} ------- {} ------- {}'''


table_template2 = """  A    B    C    D    E    F    G
1 {} ------------ {} ------------ {}
                                 
2 |    {} ------- {} ------- {}    |
  |                             |
3 |    |    {} -- {} -- {}    |    |
                                 
4 {} -- {} -- {}         {} -- {} -- {}
                                 
5 |    |    {} -- {} -- {}    |    |
  |                             |          
6 |    {} ------- {} ------- {}    |
                                 
7 {} ------------ {} ------------ {}"""


key_order = [
    0, 1, 2, 8, 9, 10, 16, 17,
    18, 7, 15, 23, 19, 11, 3, 22,
    21, 20, 14, 13, 12, 6, 5, 4
]


coordinate_map = LinearHashMap()
coordinate_map['A1'] = 0
coordinate_map['A4'] = 7
coordinate_map['A7'] = 6
coordinate_map['B2'] = 8
coordinate_map['B4'] = 15
coordinate_map['B6'] = 14
coordinate_map['C3'] = 16
coordinate_map['C4'] = 23
coordinate_map['C5'] = 22
coordinate_map['D1'] = 1
coordinate_map['D2'] = 9
coordinate_map['D3'] = 17
coordinate_map['D5'] = 21
coordinate_map['D6'] = 13
coordinate_map['D7'] = 5
coordinate_map['E3'] = 18
coordinate_map['E4'] = 19
coordinate_map['E5'] = 20
coordinate_map['F2'] = 10
coordinate_map['F4'] = 11
coordinate_map['F6'] = 12
coordinate_map['G1'] = 2
coordinate_map['G4'] = 3
coordinate_map['G7'] = 4

coordinate_map_inv = LinearHashMap()
for key, val in coordinate_map.items():
    coordinate_map_inv[val] = key

minsize = -1 * maxsize

EMPTY = '0'
WHITE = '1'
BLACK = '2'
HUMAN = 'human'
AI = 'ai'

PLACE = 'place'
SELECT = 'select'
MOVE = 'move'
DELETE = 'delete'


def derive_index(index):
    rem = index % 8
    return (rem, index - rem)


class bcolors:
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKCYAN = '\033[96m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    UNDERLINE = '\033[4m'
    OKORANGE = ''
