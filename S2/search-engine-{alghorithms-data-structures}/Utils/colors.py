import re

"""Kolekcija boja zasnovanim na ANSI escape sekvencama
Radi samo za unix sisteme"""
class bcolors:
    CEND       = '\33[0m'
    CBOLD      = '\33[1m'
    CITALIC    = '\33[3m'
    CURL       = '\33[4m'
    CBLINK     = '\33[5m'
    CBLINK2    = '\33[6m'
    CSELECTED  = '\33[7m'

    CBLACK     = '\33[30m'
    CRED       = '\33[31m'
    CGREEN     = '\33[32m'
    CYELLOW    = '\33[33m'
    CBLUE      = '\33[34m'
    CVIOLET    = '\33[35m'
    CBEIGE     = '\33[36m'
    CWHITE     = '\33[37m'

    CBLACKBG   = '\33[40m'
    CREDBG     = '\33[41m'
    CGREENBG   = '\33[42m'
    CYELLOWBG  = '\33[43m'
    CBLUEBG    = '\33[44m'
    CVIOLETBG  = '\33[45m'
    CBEIGEBG   = '\33[46m'
    CWHITEBG   = '\33[47m'

    CGREY      = '\33[90m'
    CRED2      = '\33[91m'
    CGREEN2    = '\33[92m'
    CYELLOW2   = '\33[93m'
    CBLUE2     = '\33[94m'
    CVIOLET2   = '\33[95m'
    CBEIGE2    = '\33[96m'
    CWHITE2    = '\33[97m'

    CGREYBG    = '\33[100m'
    CREDBG2    = '\33[101m'
    CGREENBG2  = '\33[102m'
    CYELLOWBG2 = '\33[103m'
    CBLUEBG2   = '\33[104m'
    CVIOLETBG2 = '\33[105m'
    CBEIGEBG2  = '\33[106m'
    CWHITEBG2  = '\33[107m'


"""Boji prosledjeni tekst"""
def color_string(text: int, color: str) -> str:
    return f"{color}{text}{bcolors.CEND}"


"""Boji tekst u crveno"""
def failure(text: int) -> str:
    return f"{bcolors.CRED}{text}{bcolors.CEND}"


"""Boji tekst u zuto"""
def warning(text: int) -> str:
    return f"{bcolors.CYELLOW}{text}{bcolors.CEND}"


def replace_color(text: str, match_indexes: list[tuple[int, int]], color1: str, color2: str) -> str:
    """Boji prosledjeni tekst u 2 boje u zavisnosti od liste indexa

    :param text: tekst za bojenje
    :type text: str
    :param match_indexes: lista indeks parova koji odredjuju 
                koji intervali teksta ce imati drugu boju
    :type match_indexes: list
    :param color1: boja za sekvence van prosledjenih indeksa
    :type color1: str
    :param color2: 'highlight' boja
    :type color2: str

    :returns: obojeni tekst
    :rtype: str
    """
    new_text = ""

    ex_end = 0
    for start, end in sorted(match_indexes):
        piece1 = color_string(text[ex_end:start], color1)
        word = color_string(text[start:end], color2)
        new_text += piece1 + word
        ex_end = end
    new_text += color_string(text[ex_end:len(text)], color1)

    return new_text


"""Uklanja boju sa teksta pomocu Regex-a"""
def remove_color(text: str) -> str:
    ansi_escape = re.compile(r'\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])')
    return ansi_escape.sub('', text)
