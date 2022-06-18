from database import *
import re


# general 1
def login(user=None):
    users = read_users()

    clear_console()
    print("\t\tPRIJAVA\n")
    count = 1
    while count < 4:
        username = input("\nKorisničko ime: ")

        if username not in users.keys():
            if count != 3:
                print("\nKorisničko ime ne postoji, pokušajte ponovo.")
            count += 1
            continue

        count = 1
        while count < 4:
            password = input("Lozinka: ")

            if users[username]['password'] != password:
                if count != 3:
                    print("\nPogrešna lozinka, pokušajte ponovo.\n")
                count += 1
            elif users[username]['blocked']:
                print(
                    "\nKorisnik je blokiran. Kontaktirajte administratora za više detalja.")
                input("Pritisnite 'Enter' za dalje ")
                return None
            else:
                print("\n\nUspešna prijava")
                input("Pritisnite 'Enter' za dalje ")
                user = users[username].copy()
                user['username'] = username

                return user
        break

    input("\nNemate više pokušaja. Pritisnite 'Enter' za dalje ")
    return None


# general 8
def logout(user=None):
    return None


# general 3
def print_apartments_general(user=None):
    apartments = read_apartments()
    amenities = read_amenities()

    clear_console()

    def condition(apartment):
        return apartment['active']
    filtered = filter_apartments_with_condition(apartments, condition)

    clear_console()
    print_apartments(filtered, amenities)
    input("\nPritisnite 'Enter' za dalje ")

    return user


# general 4
def search_apartments_general(user=None):
    apartments = read_apartments()
    amenities = read_amenities()

    clear_console()

    def condition(apartment):
        return apartment['active']
    filtered = filter_apartments_with_condition(apartments, condition)
    filtered = filter_apartments(filtered)

    print_apartments(filtered, amenities)
    if user is None or user['role'] != 'guest':
        input("\nPritisnite 'Enter' za dalje ")

    return user


def print_apartments(apartments, amenities):
    headers = ['šifra', 'adresa', 'tip', 'broj soba',
               'kapacitet', 'dostupnost', 'cena po noći', 'sadržaji']
    rows = []

    for id, apartment in apartments.items():
        row = apartment_to_row_general(id, apartment, amenities)
        rows.append(row)

    print(tabulate(rows, headers, tablefmt='fancy_grid'))


def apartment_to_row_general(id, apartment, amenities):
    return [
        id,
        list_to_str(apartment['location']['address'].values(), ', '),
        'apartman' if apartment['type'] == 'apartment' else 'soba',
        apartment['room_num'],
        apartment['guest_num'],
        dates_to_str(apartment['availability']),
        str(apartment['price']) + ' €',
        list_to_str(decode_list(apartment['amenities'], amenities))
    ]


def filter_apartments(apartments):
    filtered = {}

    headers = ['', 'FILTERI']
    filters = {
        '1': 'mesto',
        '2': 'dostupni termini',
        '3': 'broj soba',
        '4': 'broj osoba',
        '5': 'cena'
    }
    rows = [
        ['1', 'mesto'],
        ['2', 'dostupni termini'],
        ['3', 'broj soba'],
        ['4', 'broj osoba'],
        ['5', 'cena']
    ]
    table = tabulate(rows, headers, tablefmt='presto')
    while True:
        print(table)
        choice = input(
            "\nIzaberite jedan ili više filtera (Enter da preskočite): ")

        if choice == '':
            break

        choice = choice.replace(' ', '').replace(',', '')

        for char in choice:
            if char not in filters.keys():
                print("Neispravan unos. Pokušajte ponovo\n")
                continue
        break

    print("\nBilo koje polje možete da preskočite unošenjem 'Enter'")

    applied_filters = []
    filtered = apartments.copy()
    for filter in choice:
        if filter in filters.keys():
            applied_filters.append(filters[filter])

        if filter == '1':
            city = input("\nUnesite deo ili ceo naziv mesta: ")

            def condition(apartment):
                return city.lower() in apartment['location']['address']['city'].lower()
        elif filter == '2':
            print("\nFormat {dan/mesec/godina}; Enter da preskočite")
            start_date = input_date("Unesite početni datum pretrage: ", '')
            end_date = input_date("Unesite krajnji datum pretrage: ", '')

            def condition(apartment):
                for range in apartment['availability']:
                    if (start_date and not end_date and range[0] >= start_date) \
                            or (not start_date and end_date and range[0] <= end_date) \
                            or (start_date and end_date and start_date <= range[0] and range[1] <= end_date) \
                            or (not start_date and not end_date):
                        return True
                return False

        elif filter == '3':
            start_num = input_int("\nBroj soba veći od: ", '')
            stop_num = input_int("Broj soba manji od: ", '')

            def condition(apartment):
                return (start_num and not stop_num and start_num < apartment['room_num']) \
                    or (not start_num and stop_num and apartment['room_num'] < stop_num) \
                    or (start_num and stop_num and start_num < apartment['room_num'] and apartment['room_num'] < stop_num) \
                    or (not start_num and not stop_num)

        elif filter == '4':
            start_num = input_int("\nBroj gostiju veći od: ", '')
            stop_num = input_int("Broj gostiju manji od: ", '')

            def condition(apartment):
                return (start_num and not stop_num and start_num < apartment['guest_num']) \
                    or (not start_num and stop_num and apartment['guest_num'] < stop_num) \
                    or (start_num and stop_num and start_num < apartment['guest_num'] and apartment['guest_num'] < stop_num) \
                    or (not start_num and not stop_num)

        elif filter == '5':
            start_num = input_float("\nCena po noći veća od: ", '')
            stop_num = input_float("Cena po noći manja od: ", '')

            def condition(apartment):
                return (start_num and not stop_num and start_num < apartment['price']) \
                    or (not start_num and stop_num and apartment['price'] < stop_num) \
                    or (start_num and stop_num and start_num < apartment['price'] and apartment['price'] < stop_num) \
                    or (not start_num and not stop_num)

        filtered = filter_apartments_with_condition(filtered, condition)

    clear_console()
    print("Primenjeni filteri:", list_to_str(applied_filters, ', '), '\n')
    return filtered


def filter_apartments_with_condition(apartments, condition):
    filtered = {}
    for id, apartment in apartments.items():
        if condition(apartment):
            filtered[id] = apartment

    return filtered


# general 6
def most_engaged_cities(user=None):
    apartments = read_apartments()
    reservations = read_reservations()

    clear_console()
    headers = ['', '10 NAJPOPULARNIJIH GRADOVA']

    engagement = {}
    frame_date = (datetime.now() - timedelta(days=365)).date()

    for reservation in reservations.values():
        apartment_id = reservation['apartment']
        city = apartments[apartment_id]['location']['address']['city']
        end_date = reservation['start_date'] + \
            timedelta(days=reservation['nights_num'])

        if reservation['status'] == 'completed' and end_date >= frame_date:
            if city not in engagement.keys():
                engagement[city] = 0
            engagement[city] += 1

    rows = []
    engagement = dict(
        sorted(engagement.items(), key=lambda item: item[1], reverse=True))

    i = 1
    for city in engagement.keys():
        if i > 10:
            break

        rows.append([i, city])
        i += 1

    print(tabulate(rows, headers, tablefmt='fancy_grid'))

    input("\nUnesite 'Enter' za dalje ")

    return user


# general 7, admin 18
def register(user=None):
    users = read_users()

    clear_console()
    print("\t\tREGISTRACIJA\n")
    new_user = {}

    while True:
        username = input("Unesite korisničko ime: ")

        if len(username) < 4:
            print("\nKorisničko ime treba da bude minimum dužine 4\n")
            continue
        if not username.isalnum():
            print("Korisničko ime ne sme da sadrži specijalne karaktere")
            continue
        if username in users.keys():
            print("Korisničko ime je zauzeto. Pokušajte ponovo")
        else:
            break

    while True:
        password = input("Unesite lozinku: ")

        if len(password) < 4:
            print("\nLozinka je preslaba\n")
        elif ' ' in password:
            print("\nNeispravna lozinka. Pokušajte ponovo\n")
        else:
            break
    new_user['password'] = password

    new_user['name'] = input("Ime: ")
    new_user['surname'] = input("Prezime: ")

    while True:
        print("1 muški, 2 ženski, 3 drugo")
        gender = input("Pol (1/2/3): ")

        if gender == '1':
            gender = 'male'
        elif gender == '2':
            gender = 'female'
        elif gender == '3':
            gender == 'other'
        else:
            print("\nNeispravan unos. Pokušajte ponovo\n")
            continue
        break
    new_user['gender'] = gender

    while True:
        phone = input("Broj telefona (+)xxxxxxxx: ")

        if len(phone) > 7 and \
                ((phone[0] == '+' and phone[1:].isdigit()) or phone.isdigit()):
            break
        else:
            print("\nNeispravan broj. Pokušajte ponovo\n")
    new_user['phone'] = phone

    while True:
        email = input("Email: ")

        if re.match(r"[^@]+@[^@]+\.[^@]+", email):
            break
        else:
            print("\nEmail adresa nije validna. Pokušajte ponovo\n")
    new_user['email'] = email

    role = 'host' if user else 'guest'

    new_user['role'] = role
    new_user['blocked'] = False
    users[username] = new_user

    update_users(users)

    if not user:
        user = new_user.copy()
        user['username'] = username

    print("\n\nUspešna registracija")
    input("Pritisnite 'Enter' za dalje ")

    return user
