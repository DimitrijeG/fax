from general import search_apartments_general
from tabulate import tabulate
from database import *
from utils import *


# guest 9
def make_a_reservations(user):
    while True:
        user = make_a_reservation(user)

        if not yn_prompt("želite da napravite još rezervacija"):
            return user


def make_a_reservation(user):
    apartments = read_apartments()
    reservations = read_reservations()
    holidays = read_holidays()

    clear_console()
    print("\t\tREZERVACIJA APARTMANA\n")

    reservation = {}

    rows = [
        ['1', 'pretraga apartmana'],
        ['2', 'unos šifre apartmana'],
        ['3', 'izađi']
    ]
    table = tabulate(rows)
    while True:
        print(table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            search_apartments_general(user)
            break
        elif choice == '2':
            break
        elif choice == '3':
            return user
        else:
            print("\nNeispravan unos. Pokušajte ponovo\n")

    print('')
    while True:
        apartment_id = input("Unesite šifru apartmana: ")

        if apartment_id in apartments.keys() \
                and apartments[apartment_id]['active']:
            break
        elif not apartment_id.isdigit():
            print("\nNeispravna šifra\n")
        else:
            print("\nNe postoji aktivan apartman sa datom šifrom\n")
#
    clear_console()
    apartment = apartments[apartment_id]
    str_dates = dates_to_str(apartment['availability'], 'list')
    print(tabulate(str_dates, ['dostupni termini'],
          tablefmt='fancy_grid') + '\n')

    while True:
        start_date = input_date("Unesite početni datum rezervacije: ")
        nights = input_int("Broj noćenja: ")
        end_date = start_date + timedelta(days=nights)

        valid = False
        for date_range in apartment['availability']:
            if start_date >= date_range[0] and end_date <= date_range[1]:
                valid = True
                range_to_be_updated = date_range
                break

        if valid:
            break
        else:
            print("\nApartman nije dostupan u datom terminu")
            if yn_prompt("želite da odustanete od rezervacije"):
                return user
            print('')

    updated_apartment = apartment.copy()
    updated_availability = []
    for date_range in apartment['availability']:
        if date_range == range_to_be_updated:
            if start_date != date_range[0]:
                updated_availability.append((date_range[0], start_date))
            if end_date != date_range[1]:
                updated_availability.append((end_date, date_range[1]))
        else:
            updated_availability.append(date_range)

    updated_apartment['availability'] = updated_availability

#
    print('')
    while True:
        guests = [user['username']]
        print("Rezervišete za: ")
        print("1 sebe")
        print("2 sebe i još gostiju")
        print("3 nekoga drugog")
        choice = input("\nVaš izbor: ")

        if choice == '1' or choice == '2':
            guests.append(f"{user['name']} {user['surname']}")
        if choice == '2' or choice == '3':
            guests_str = input(
                "\nUnesite goste (ime1 prezime1,ime2 prezime2...): ")

            if guests_str == '':
                print("\nNeispravan unos. Pokušajte ponovo\n")
                continue

            guests_str = guests_str.split(',')
            guests_num = len(guests_str)
            if choice == '2':
                guests_num += 1
            if guests_num > updated_apartment['guest_num']:
                print("\nUkupan broj gostiju je veći od kapaciteta apartmana.")
                if yn_prompt("želite da odustanete od rezervacije"):
                    return user

            for guest in guests_str:
                guests.append(guest.strip())
            break
        if choice != '1':
            print("\nNeispravan unos. Pokušajte ponovo\n")
            continue
        break
#
    price = 0.0

    discount = 0
    for reservation in reservations.values():
        if reservation['guests'][0] == user['username']:
            discount = 0.05
            break

    for date in [start_date + timedelta(days=x) for x in range(nights)]:
        date = date.replace(year=1900)
        discount_per_day = 0
        if date in holidays:
            discount_per_day = -0.05
        elif date.weekday() > 3:
            discount_per_day = 0.1

        price += (1 - discount_per_day) * updated_apartment['price']

    price *= (1 - discount)

    reservation_id = get_last_id(reservations, 8)
    reservation = {
        'apartment': apartment_id,
        'start_date': start_date,
        'nights_num': nights,
        'full_price': price,
        'guests': guests,
        'status': 'pending'
    }

    print_reservations(user, {reservation_id: reservation})

    print('')
    if not yn_prompt("potvrđujete rezervaciju"):
        if yn_prompt("ste sigurni da želite da odustanete"):
            return user

    reservations[reservation_id] = reservation
    apartments[apartment_id] = updated_apartment

    update_reservations(reservations)
    update_apartments(apartments)

    return user


# guest 10
def print_reservations(user, arg_reservations=None):
    if arg_reservations is None:
        reservations = read_reservations()
    else:
        reservations = arg_reservations.copy()

    if user and user['role'] == 'guest':
        reservations = filter_reservations_by_username(
            reservations, user['username'])

    clear_console()

    headers = ["ID rezervacije", "ID apartmana", "početni datum",
               "broj noćenja", "ukupna cena", "gosti", "status"]
    rows = []

    for id, reservation in reservations.items():
        row = list(reservation.values())
        row.insert(0, id)
        row[2] = date_to_str(reservation['start_date'])
        row[4] = str(round(reservation['full_price'], 2)) + ' €'
        row[5] = list_to_str(reservation['guests'][1:])

        if reservation['status'] == 'pending':
            row[6] = 'kreirana'
        elif reservation['status'] == 'declined':
            row[6] = 'odbijena'
        elif reservation['status'] == 'withdrawed':
            row[6] = 'odustanak'
        elif reservation['status'] == 'accepted':
            row[6] = 'prihvaćena'
        else:
            row[6] = 'završena'
        rows.append(row)

    print(tabulate(rows, headers, tablefmt='fancy_grid'))

    if arg_reservations is None:
        input("\nUnesite 'Enter' za dalje ")

    return user


def filter_reservations_by_username(reservations, username):
    filtered = {}
    for id, reservation in reservations.items():
        if reservation['guests'][0] == username:
            filtered[id] = reservation

    return filtered


def filter_reservations_by_status(reservations, status):
    filtered = {}
    for id, reservation in reservations.items():
        if reservation['status'] in status:
            filtered[id] = reservation

    return filtered


# guest 11
def withdraw_a_reservation(user):
    reservations = read_reservations()

    clear_console()
    print("\t\tPONIŠTAVANJE REZERVACIJE\n")

    arg_reservations = filter_reservations_by_status(
        reservations, ['accepted', 'pending'])

    rows = [
        ['1', 'ispis rezervacija'],
        ['2', 'unos šifre rezervacije'],
        ['3', 'izađi']
    ]
    table = tabulate(rows)
    while True:
        print(table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            if arg_reservations != {}:
                print_reservations(user, arg_reservations)
            break
        elif choice == '2':
            break
        elif choice == '3':
            return user
        print("\nNeispravan unos. Pokušajte ponovo")

    if arg_reservations == {}:
        print("\nTrenutno nemate rezervacija sa statusom 'kreirana' ili 'prihvaćena'")
        input("Pritisnite 'Enter' za dalje ")
        return user

    while True:
        id = input("\nŠifra rezervacije: ")
        if id not in reservations.keys():
            print("\nNe postoji rezervacija sa datom šifrom")
            if yn_prompt("želite da odustanete"):
                return user
        else:
            reservations[id]['status'] = 'withdrawed'
            break

    print('')
    if not yn_prompt("ste sigurni da želite da otkažete ovu rezervaciju"):
        return user

    update_reservations(reservations)
    recalculate_apartments_availability()

    input("\n\nRezervacija je otkazana. Pritisnite 'Enter' za dalje ")

    return user
