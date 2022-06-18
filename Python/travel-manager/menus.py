from general import *
from admin import *
from host import *
from guest import *


def general_menu():
    return [
        ('Pregled aktivnih apartmana', print_apartments_general),
        ('Pretraga aktivnih apartmana', search_apartments_general),
        ('Prikaz 10 najpopularnijih gradova', most_engaged_cities)
    ]


def unsigned_menu():
    return [
        ('Prijava na sistem', login),
        ('Registracija', register)
    ]


def signed_menu():
    return [
        ('Odjava sa sistema', logout)
    ]


def guest_menu():
    return [
        ('Rezervacija apartmana', make_a_reservations),
        ('Pregled rezervacija', print_reservations),
        ('Poništavanje rezervacije', withdraw_a_reservation)
    ]


def host_menu():
    return [
        ('Pregled sopstvenih apartmana', print_apartments_for_host),
        ('Dodavanje apartmana', add_an_apartment),
        ('Izmena podataka o apartmanu', edit_an_apartment),
        ('Brisanje apartmana', remove_an_apartment),
        ('Menadžment rezervacija', reservations_manager)
    ]


def admin_menu():
    return [
        ('Pretraga rezervacija', filter_reservations),
        ('Registracija novih domaćina', register),
        ('Menadžment dodatne opreme', edit_amenities),
        ('Blokiranje korisnika', block_user),
        ('Izveštaji', report),
        ('Upravljanje praznicima', edit_holidays)
    ]


def blank_line():
    return [('', 'blank')]


def activate_menu(menu):
    output = ""
    activated_menu = {}

    headers = ['', 'MENI']
    rows = []

    i = 1
    for option in menu:
        if option[1] == 'blank':
            rows.append(['', ''])
        else:
            rows.append([str(i), option[0]])
            activated_menu[str(i)] = option[1]
            i += 1

    rows.append(['', ''])
    rows.append(['x', 'Izlaz iz aplikacije'])

    print(tabulate(rows, headers))
    return activated_menu
