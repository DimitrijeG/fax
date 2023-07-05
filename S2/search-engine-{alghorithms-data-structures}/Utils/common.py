import traceback

class ExitException(Exception):
    """Klasa modeluje izuzetak vezan za tok programa."""
    pass


"""Pomocna funkcija za izlaz iz aplikacije"""
def check_exit(cli_input: list) -> None:
    if len(cli_input) == 1 and cli_input[0].lower() == 'q':
        raise ExitException()
    

"""Brise ekran pomocu ANSI escape sekvenci"""
def clear_screen():
    print("\033[H\033[J", end="")
