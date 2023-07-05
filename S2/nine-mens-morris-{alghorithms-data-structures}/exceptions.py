from consts import bcolors

class TooManyTriesException(Exception):
    def __init__(self, try_count) -> None:
        super().__init__(
            f'\n{bcolors.FAIL}Korisnik je uneo pogresan unos {try_count} puta. Program terminisan{bcolors.ENDC}')


class TooManyMovesException(Exception):
    def __init__(self, move_limit) -> None:
        super().__init__(
            f'\n{bcolors.FAIL}Broj odigranih poteza je presao granicu od {move_limit} poteza. Program terminisan{bcolors.ENDC}')


class ExitException(Exception):
    pass
