from host import *
from collections import OrderedDict


# admin 17
def filter_reservations(user):
    apartments = read_apartments()
    reservations = read_reservations()

    clear_console()
    print("\t\tPRETRAGA REZERVACIJA\n")

    rows = [
        ['1', 'status'],
        ['2', 'adresa'],
        ['3', 'korisničko ime domaćina'],
        ['4', 'izađi']
    ]
    table = tabulate(rows)
    while True:
        clear_console()
        print("\t\tPRETRAGA REZERVACIJA\n")
        print(table)

        choice = input("\nIzaberite kriterijum pretrage: ")

        filtered = {}
        if choice == '1':
            while True:
                print("\nStatus rezervacije")
                print("1 prihvaćena")
                print("2 odbijena")
                status = input("\nIzbor: ")

                if status == '1':
                    status = 'accepted'
                elif status == '2':
                    status = 'declined'
                else:
                    print("\nUnos nije validan. Pokušajte ponovo\n")
                    continue
                break
            filtered = filter_reservations_status(reservations, status)
        elif choice == '2':
            while True:
                address = input("\nUnesite celu ili deo adrese: ")
                if address != '':
                    break
                print("\nUnos nije validan. Pokušajte ponovo\n")

            filtered = filter_reservations_address(
                apartments, reservations, address)
        elif choice == '3':
            while True:
                username = input("\nUnesite korisničko ime domaćina: ")
                if username != '':
                    break
                print("\nUnos nije validan. Pokušajte ponovo\n")

            filtered = filter_reservations_host(
                apartments, reservations, username, admin=True)
        elif choice == '4':
            return user
        else:
            print("Neispravan unos. Pokušajte ponovo\n")
            continue
        print_reservations(user, filtered)

        input("\nPritisnite 'Enter' za dalje ")


# admin 19,20
def edit_amenities(user):
    amenities = read_amenities()

    clear_console()
    print("\t\tUPRAVLJANJE DODATNOM OPREMOM\n")

    updated_amenities = amenities.copy()

    rows = [
        ['1', 'dodaj dodatnu opremu'],
        ['2', 'obriši dodatnu opremu'],
        ['3', 'sačuvaj izmene'],
        ['4', 'odustani']
    ]
    table = tabulate(rows)
    while True:
        clear_console()
        print_amenities(updated_amenities)
        print('\n' + table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            new_amenities = input(
                "\nUnesite novu opremu (oprema1,oprema2...): ")

            new_amenities = new_amenities.split(',')

            for amenity in new_amenities:
                if amenity != '':
                    id = get_last_id(updated_amenities, 3)
                    updated_amenities[id] = amenity.strip()

        elif choice == '2':
            id = input("\nUnesite šifru opreme koju brišete: ")

            if id in updated_amenities.keys():
                del updated_amenities[id]
            else:
                print("\nNe postoji oprema sa datom šifrom\n")

        elif choice == '3':
            update_amenities(updated_amenities)
            return user

        elif choice == '4':
            return user

        else:
            print("\nPogrešan izbor. Pokušajte ponovo")


def filter_reservations_status(reservations, status):
    filtered = {}
    for id, reservation in reservations.items():
        if reservation['status'] == status:
            filtered[id] = reservation

    return filtered


def filter_reservations_address(apartments, reservations, address):
    filtered = {}
    words_to_check = address.split(' ')

    for id, reservation in reservations.items():
        apartment_id = reservation['apartment']
        address = list_to_str(
            apartments[apartment_id]['location']['address'].values(), '')

        for word in words_to_check:
            if word in address:
                filtered[id] = reservation

    return filtered


# admin 21
def block_user(user):
    users = read_users()

    clear_console()
    print("\t\tUPRAVLJANJE BLOKIRANIM KORISNICIMA\n")

    rows = [
        ['1', 'blokiraj korisnika'],
        ['2', 'odblokiraj korisnika'],
        ['3', 'sačuvaj izmene'],
        ['4', 'odustani']
    ]
    table = tabulate(rows)
    while True:
        print_users(OrderedDict(sorted(users.items(), key=lambda x: (
            x[1]['blocked'], x[1]['role']), reverse=True)))
        print('\n' + table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            while True:
                username = input("\nUnesite korisničko ime: ")
                if username != '':
                    break
                print("Unos nije validan. Pokušajte ponovo\n")

            if username not in users.keys():
                clear_console()
                print("\nNe postoji korisnik sa datim korisničkim imenom\n")
            if username == user['username']:
                clear_console()
                print("\nNe možete da blokirate sebe\n")
            elif users[username]['blocked']:
                clear_console()
                print("\nKorisnik sa datim korisničkim imenom je već blokiran.\n")
            elif not users[username]['blocked']:
                clear_console()
                users[username]['blocked'] = True

        elif choice == '2':
            while True:
                username = input("\nUnesite korisničko ime: ")
                if username != '':
                    break
                print("\nUnos nije validan. Pokušajte ponovo\n")

            if username not in users.keys():
                clear_console()
                print("\nNe postoji korisnik sa datim korisničkim imenom\n")
            elif not users[username]['blocked']:
                clear_console()
                print("\nKorisnik sa datim korisničkim imenom nije blokiran.\n")
            elif users[username]['blocked']:
                users[username]['blocked'] = False
                clear_console()

        elif choice == '3':
            update_users(users)
            return user

        elif choice == '4':
            return user

        else:
            clear_console()
            print("Pogrešan izbor. Pokušajte ponovo")


def print_users(users):
    headers = ['korisničko ime', 'uloga', 'blokiran', 'ime',
               'prezime', 'pol', 'broj telefona', 'email adresa']
    rows = []

    for username, user in users.items():
        row = user_to_row(username, user)
        rows.append(row)

    print(tabulate(rows, headers, tablefmt='fancy_grid'))


def user_to_row(username, user):
    role = 'admin'
    if user['role'] == 'host':
        role = 'domaćin'
    elif user['role'] == 'guest':
        role = 'gost'
    gender = 'other'
    if user['gender'] == 'male':
        gender = 'muški'
    elif user['gender'] == 'female':
        gender = 'ženski'
    return [
        username,
        role,
        'da' if user['blocked'] else 'ne',
        user['name'],
        user['surname'],
        gender,
        user['phone'],
        user['email']
    ]


# admin 22
def report(user):
    users = read_users()
    apartments = read_apartments()
    reservations = read_reservations()
    amenities = read_amenities()

    clear_console()

    headers = ['', 'IZVEŠTAJI']
    rows = [
        ['1', 'potvrđeni rezervisani apartmani za dan i domaćina'],
        ['2', 'angažovanje domaćina'],
        ['3', 'ukupan broj i cena rezervacija za dan i domaćina'],
        ['4', 'pregled angažovanja po gradovima'],
        ['5', 'izlaz']
    ]
    table = tabulate(rows, headers)
    while True:
        clear_console()
        print(table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            print("\nBarem jedno polje je obavezno, 'Enter' da preskočite")
            while True:
                date = input("Unesite datum pretrage (d/m/g): ")
                if date != '':
                    date = str_to_date(date)

                if date is None:
                    print("\nNeispravan unos. Pokušajte ponovo\n")
                    continue

                username = input("Unesite korisničko ime domaćina: ")

                if (date, username) == ('', ''):
                    print("\nPopunite barem jedan kriterijum pretrage\n")
                    continue

                if username == '' or username in users.keys():
                    break
                else:
                    print("\nNe postoji korisnik sa datim korisničkim imenom\n")

            filtered = filter_apartments_admin(
                apartments, reservations, username, date)
            print_apartments_host(filtered, amenities)

            input("\nPritisnite 'Enter' za dalje ")

        elif choice == '2':
            while True:
                print("1 godišenje angažovanje")
                print("2 mesečno angažovanje")
                choice1 = input("\nVaš izbor: ")

                if choice1 == '1':
                    annual = True
                    break
                elif choice1 == '2':
                    annual = False
                    break
                else:
                    print("\nPogrešan izbor. Probajte ponovo\n")
            print_host_engagement(users, apartments, reservations, annual)

            input("\nPritisnite 'Enter' za dalje ")

        elif choice == '3':
            print("Barem jedno polje je obavezno, 'Enter' da preskočite")
            while True:
                date = input("Unesite datum pretrage (d/m/g): ")
                if date != '':
                    date = str_to_date(date)

                if date is None:
                    print("Neispravan unos. Pokušajte ponovo")
                    continue

                username = input("Unesite korisničko ime domaćina: ")

                if (date, username) == ('', ''):
                    print("Popunite barem jedan kriterijum pretrage")
                    continue

                if username == '' or username in users.keys():
                    break
                else:
                    print("Ne postoji korisnik sa datim korisničkim imenom")
            filtered = filter_reservations_admin(
                apartments, reservations, username, date)
            print_reservations(user, filtered)

            input("\nPritisnite 'Enter' za dalje ")

        elif choice == '4':
            print_city_engagement(apartments, reservations)

            input("\nPritisnite 'Enter' za dalje ")

        elif choice == '5':
            return user

        else:
            print("Pogrešan izbor. Pokušajte ponovo")


def filter_apartments_admin(apartments, reservations, username, date):
    filtered = {}
    for reservation in reservations.values():
        if reservation['status'] == 'accepted':
            apartment_id = reservation['apartment']

            start_date = reservation['start_date']
            end_date = start_date + timedelta(days=reservation['nights_num'])

            condition1 = username and apartments[apartment_id]['host'] == username
            condition2 = date and start_date <= date and date <= end_date

            if username and not date and condition1 \
                    or (not username and date and condition2) \
                    or (username and date and condition1 and condition2):
                filtered[apartment_id] = apartments[apartment_id]

    return filtered


def print_host_engagement(users, apartments, reservations, annual):
    engagement = {}
    for id, apartment in apartments.items():
        host = apartment['host']
        if host not in engagement.keys():
            engagement[host] = {
                'name': users[host]['name'],
                'surname': users[host]['surname'],
                'reservation_num': 0,
                'earnings': 0.0
            }

        delta = 365 if annual else 31
        frame_date = (datetime.now() - timedelta(days=delta)).date()

        for reservation in reservations.values():
            end_date = reservation['start_date'] + \
                timedelta(days=reservation['nights_num'])

            if reservation['status'] == 'completed' \
                    and reservation['apartment'] == id \
                    and end_date >= frame_date:
                engagement[host]['reservation_num'] += 1
                engagement[host]['earnings'] += reservation['full_price']

    headers = ['korisničko ime', 'ime',
               'prezime', 'broj rezervacija', 'zarada']
    rows = []

    for host, data in sorted(engagement.items(), key=lambda x: (x[1]['earnings'], x[1]['name']), reverse=True):
        rows.append([
            host,
            data['name'],
            data['surname'],
            data['reservation_num'],
            str(round(data['earnings'], 2)) + ' €'
        ])

    print("{} angažovanje".format('Godišnje' if annual else 'Mesečno'))
    print(tabulate(rows, headers, tablefmt='fancy_grid'))


def filter_reservations_admin(apartments, reservations, username, date):
    filtered = {}
    for id, reservation in reservations.items():
        if reservation['status'] == 'accepted':
            apartment_id = reservation['apartment']

            start_date = reservation['start_date']
            end_date = start_date + timedelta(days=reservation['nights_num'])

            condition1 = username and apartments[apartment_id]['host'] == username
            condition2 = date and start_date <= date and date <= end_date

            if username and not date and condition1 \
                    or (not username and date and condition2) \
                    or (username and date and condition1 and condition2):
                filtered[id] = reservation

    return filtered


def print_city_engagement(apartments, reservations):
    engagement = {}
    all_reservations = len(reservations)

    for reservation in reservations.values():
        apartment_id = reservation['apartment']
        city = apartments[apartment_id]['location']['address']['city']

        if city not in engagement.keys():
            engagement[city] = 0

        engagement[city] += 1

    headers = ['grad', 'odnos rezervacija', 'procenat od ukupnih rezervacija']
    rows = []

    for city, reservation_num in sorted(engagement.items(), key=lambda x: (x[1], x[0]), reverse=True):
        rows.append([
            city,
            f'{reservation_num}/{all_reservations}',
            str(round((reservation_num / all_reservations) * 100)) + '%'
        ])

    print("Zastupljenost rezervacija po gradovima")
    print(tabulate(rows, headers, tablefmt='fancy_grid'))


def edit_holidays(user):
    holidays = read_holidays()

    clear_console()
    print("\t\tUPRAVLJANJE PRAZNICIMA\n")

    updated_holidays = list(holidays.copy())

    rows = [
        ['1', 'dodaj praznik'],
        ['2', 'obriši praznik'],
        ['3', 'sačuvaj izmene'],
        ['4', 'odustani']
    ]
    table = tabulate(rows)
    while True:
        clear_console()
        updated_holidays = sorted(updated_holidays)
        str_holidays = []
        i = 1
        for holiday in updated_holidays:
            str_holidays.append([str(i), date_to_str(holiday, '/', False)])
            i += 1
        hol_len = len(str_holidays)
        print(tabulate(str_holidays, ['', 'praznici'], tablefmt='fancy_grid'))
        print('\n' + table)

        choice = input("\nVaš izbor: ")

        if choice == '1':
            print('')
            new_holiday = input_date(
                "Unesite novi praznik (d/m): ", allow_chr=None, year=False)

            if new_holiday not in updated_holidays:
                updated_holidays.append(new_holiday)
                print(sorted(updated_holidays))
            else:
                print("\nPraznik je već dodat")

        elif choice == '2':
            hol_index = input_int("Unesite indeks praznika koji brišete: ") - 1
            if hol_index in range(hol_len):
                clear_console()
                del updated_holidays[hol_index]
            else:
                clear_console()
                print("Ne postoji praznik sa datim indeksom")

        elif choice == '3':
            update_holidays(sorted(set(updated_holidays)))
            return user

        elif choice == '4':
            return user

        else:
            print("\nPogrešan izbor. Pokušajte ponovo")
