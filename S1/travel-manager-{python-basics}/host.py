from guest import print_reservations
from datetime import date
from database import *
from utils import *


# host 12
def add_an_apartment(user):
    apartments = read_apartments()
    all_amenities = read_amenities()

    clear_console()
    print("\t\tDODAVANJE APARTMANA\n")

    print("Tip apartmana")
    while True:
        print("1 apartman")
        print("2 soba")
        type = input("\nIzbor: ")

        if type == '1':
            type = 'apartment'
        elif type == '2':
            type = 'room'
            room_num = 1
        else:
            print("\nUnos nije validan. Pokušajte ponovo\n")
            continue
        break

    if type == 'apartment':
        room_num = input_int("Broj soba: ")

    guest_num = input_int("\nKapacitet (broj osoba): ")

    print("\nLokacija")
    while True:
        street = input("Ulica: ")
        if ',' not in street and street != '':
            break
        print("Ulica nije validna. Pokušajte ponovo")
    while True:
        city = input("Grad: ")
        if ',' not in city and city != '':
            break
        print("Grad nije validan. Pokušajte ponovo")
    postal = str(input_int("Poštanski broj: "))

    while True:
        latitude = input_float(
            "Geografska širina (-90 do 90): ", allow_neg=True)
        longitude = input_float(
            "Geografska dužina (-180 do 80): ", allow_neg=True)

        if latitude >= -90 and latitude <= 90 \
                and longitude >= -180 and longitude <= 80:
            break
        print("Koordinate nisu validne. Pokušajte ponovo")

    while True:
        str_terms = input("\nUnesite slobodne termine (d/m/g-d/m/g,...): ")

        str_terms = str_terms.split(',')
        terms = str_to_dates(str_terms)

        error = False
        if terms:
            for term in terms:
                if term[0] < date.today() or term[0] >= term[1]:
                    error = True
                    break
            if not error:
                break
            print("\nDatum nekog od termina nije validan. Pokušajte ponovo")
        else:
            print("\nGreška u unosu. Pokušajte ponovo")

    terms = sorted(terms, key=lambda tup: tup[0])
    price = input_float("\nCena apartmana po noćenju: ")

    print_amenities(all_amenities)
    while True:
        amenities = input("\nDodatna oprema (x,y...): ")

        if amenities == '':
            break

        amenities = amenities.replace(' ', '').split(',')

        error = False
        for amenity in amenities:
            if amenity not in all_amenities.keys():
                error = True
                break

        if not error:
            break
        print("Neka šifra nije validna. Pokušajte ponovo")

    print("Status")
    while True:
        print("1 dostupan")
        print("2 nedostupan")
        status = input("Izbor: ")

        if status == '1':
            active = True
        elif status == '2':
            active = False
        else:
            print("Unos nije validan. Pokušajte ponovo")
            continue
        break

    apartment = {
        'type': type,
        'room_num': room_num,
        'guest_num': guest_num,
        'location': {
            'position': (latitude, longitude),
            'address': {
                'street': street,
                'city': city,
                'postal': postal
            }
        },
        'terms': terms,
        'availability': terms,
        'host': user['username'],
        'price': price,
        'active': active,
        'amenities': amenities
    }
    id = get_last_id(apartments, 6)

    print_apartments_host({id: apartment}, all_amenities)
    if not yn_prompt("želite da dodate ovaj apartman"):
        if yn_prompt("ste sigurni da želite da odustanete"):
            return user

    apartments[id] = apartment
    update_apartments(apartments)

    return user


# host 13a
def print_apartments_for_host(user):
    apartments = read_apartments()
    all_amenities = read_amenities()

    filtered = filter_apartments_host(apartments, user['username'])

    clear_console()
    print_apartments_host(filtered, all_amenities)

    input("\nPritisnite 'Enter' za dalje ")
    return user


# host 13
def edit_an_apartment(user):
    apartments = read_apartments()
    reservations = read_reservations()
    all_amenities = read_amenities()

    clear_console()
    print("\t\tIZMENA PODATAKA O APARTMANU\n")

    filtered = filter_apartments_host(apartments, user['username'])

    rows = [
        ['1', 'ispis apartmana'],
        ['2', 'unos šifre apartmana'],
        ['3', 'izađi']
    ]
    table = tabulate(rows)
    while True:
        print(table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            clear_console()
            print_apartments_host(filtered, all_amenities)
            break
        elif choice == '2':
            break
        elif choice == '3':
            return user
        print("\nNeispravan unos. Pokušajte ponovo\n")

    while True:
        apartment_id = input("\nUnesite šifru apartmana: ")

        if apartment_id in filtered.keys():
            break
        elif not apartment_id.isdigit():
            print("\nNeispravna šifra")
            input("\nPritisnite 'Enter' za dalje ")
            return user
        else:
            print("\nNe postoji vaš apartman sa datom šifrom")
            input("\nPritisnite 'Enter' za dalje ")
            return user

    updated_apartment = filtered[apartment_id].copy()
#
    rows1 = [
        ['1', 'tipa'],
        ['2', 'broja soba'],
        ['3', 'kapaciteta (broja osoba)'],
        ['4', 'adrese'],
        ['5', 'dostupnih termina'],
        ['6', 'cene po noći'],
        ['7', 'dodatne opreme'],
        ['8', 'statusa']
    ]
    rows2 = [
        ['9', 'sačuvaj izmene'],
        ['10', 'odustani']
    ]

    table1 = tabulate(rows1, ['', 'IZMENA'])
    table2 = tabulate(rows2)
    while True:
        clear_console()
        print_apartments_host({apartment_id: updated_apartment}, all_amenities)

        print('\n' + table1)
        print(table2)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            print("\nIzmena tipa")
            while True:
                print("1 apartman")
                print("2 soba")
                choice1 = input("Izbor: ")

                if choice1 == '1':
                    updated_apartment['type'] = 'apartment'
                    break
                elif choice1 == '2':
                    updated_apartment['type'] = 'room'
                    updated_apartment['room_num'] = 1
                    break
                print("\nUnos nije validan. Pokušajte ponovo\n")

        elif choice == '2':
            if not updated_apartment['type'] == 'room':
                print(updated_apartment['type'])
                updated_apartment['room_num'] = input_int("Novi broj soba: ")
            else:
                print("\nTip apartmana je soba, stoga menjanje broja soba nije moguće")
                input("Pritisnite 'Enter' za dalje ")

        elif choice == '3':
            updated_apartment['guest_num'] = input_int(
                "Novi kapacitet (broj osoba): ")

        elif choice == '4':
            print("Izmena adrese (Enter da preskočite)")
            while True:
                street = input("Ulica: ")
                if street == '':
                    break
                elif ',' not in street:
                    updated_apartment['location']['address']['street'] = street
                    break
                print("Ulica nije validna. Pokušajte ponovo")

            while True:
                city = input("Grad: ")
                if city == '':
                    break
                elif ',' not in city:
                    updated_apartment['location']['address']['city'] = city
                    break
                print("Grad nije validan. Pokušajte ponovo")

            postal = input_int("Poštanski broj: ", '')
            if postal:
                updated_apartment['location']['address']['postal'] = str(
                    postal)

            if yn_prompt("želite da izmenite koordinate"):
                while True:
                    latitude = input_float(
                        "Geografska širina (-90 do 90): ", allow_neg=True)
                    longitude = input_float(
                        "Geografska dužina (-180 do 80): ", allow_neg=True)

                    if latitude >= -90 and latitude <= 90 \
                            and longitude >= -180 and longitude <= 80:
                        updated_apartment['location']['position'] = (
                            latitude, longitude)
                        break
                    print("Koordinate nisu validne. Pokušajte ponovo")

        elif choice == '5':
            clear_console()
            while True:
                print("Izmena dostupnih termina")

                updated_apartment['terms'] = sorted(
                    updated_apartment['terms'], key=lambda tup: tup[0])
                str_dates = dates_to_str(updated_apartment['terms'], 'list')
                term_len = len(str_dates)
                for i in range(term_len):
                    str_dates[i].insert(0, i + 1)
                print(
                    tabulate(str_dates, ['', 'dostupni termini'], tablefmt='fancy_grid'))

                print("1 dodavanje")
                print("2 brisanje")
                print("3 izmena")
                print("4 gotovo")
                choice1 = input("Izbor: ")

                if choice1 == '1':
                    print(
                        "Format {d/m/g-d/m/g,d/m/g-...}; Enter da preskočite")
                    str_terms = input("Unesite nove termine: ")

                    if not str_terms:
                        clear_console()
                        break

                    str_terms = str_terms.split(',')
                    terms = str_to_dates(str_terms)

                    error = False
                    if terms:
                        for term in terms:
                            if term[0] < date.today() or term[0] >= term[1]:
                                error = True
                                break
                        if not error:
                            updated_apartment['terms'] += list(
                                set(terms) - set(updated_apartment['terms']))
                            clear_console()
                        else:
                            clear_console()
                            print(
                                "Datum nekog od termina nije validan. Pokušajte ponovo")
                    else:
                        clear_console()
                        print("Greška u unosu. Pokušajte ponovo")

                elif choice1 == '2':
                    term_index = input_int(
                        "Unesite indeks termina koji brišete: ") - 1
                    if term_index in range(term_len):
                        clear_console()
                        del updated_apartment['terms'][term_index]
                    else:
                        clear_console()
                        print("Ne postoji termin sa datim indeksom")

                elif choice1 == '3':
                    term_index = input_int(
                        "Unesite indeks termina koji menjate: ") - 1
                    if term_index in range(term_len):
                        str_term = input(
                            "Unesite izmenjen termin {d/m/g-d/m/g}: ")
                        term = str_to_dates([str_term])[0]
                        if term and term[0] >= date.today() and term[0] < term[1]:

                            updated_apartment['terms'][term_index] = term
                        else:
                            clear_console()
                            print("Neispravan termin")
                    else:
                        clear_console()
                        print("Ne postoji termin sa datim indeksom")

                elif choice1 == '4':
                    break

                else:
                    clear_console()
                    print("Unos nije validan. Pokušajte ponovo")

        elif choice == '6':
            updated_apartment['price'] = input_float("Nova cena po noći: ")

        elif choice == '7':
            updated_amenities = updated_apartment['amenities'].copy()

            clear_console()
            while True:
                print("Izmena dodatne opreme")
                updated_amenities = sorted(updated_amenities)
                str_amenities = decode_list(updated_amenities, all_amenities)

                amenities_to_print = []
                for i in range(len(updated_amenities)):
                    amenities_to_print.append(
                        [updated_amenities[i], str_amenities[i]])

                print(tabulate(amenities_to_print, [
                      'šifra', 'dodatna oprema'], tablefmt='fancy_grid'))

                print("1 dodavanje")
                print("2 brisanje")
                print("3 gotovo")
                choice1 = input("\nIzbor: ")

                if choice1 == '1':
                    amenities_to_print = []
                    for amenity_id, amenity in all_amenities.items():
                        amenities_to_print.append([amenity_id, amenity_id])

                    print("\nDOSTUPNA DODATNA OPREMA")
                    print(tabulate(amenities_to_print, [
                          'šifra', 'dodatna oprema'], tablefmt='fancy_grid'))

                    new_amenities = input("Šifre nove opreme (x,y...): ")
                    new_amenities = new_amenities.replace(' ', '').split(',')

                    error = False
                    for amenity in new_amenities:
                        if amenity not in all_amenities.keys():
                            error = True
                            break

                    if not error:
                        updated_amenities += list(set(new_amenities) -
                                                  set(updated_amenities))
                        clear_console()
                    else:
                        clear_console()
                        print("Neka šifra nije validna. Pokušajte ponovo")

                elif choice1 == '2':
                    amenity_code = input("Unesite šifru opreme koju brišete: ")
                    if amenity_code in updated_amenities:
                        updated_amenities.remove(amenity_code)
                        clear_console()
                    else:
                        clear_console()
                        print("Ne postoji oprema sa datom šifrom")

                elif choice1 == '3':
                    updated_apartment['amenities'] = updated_amenities
                    break

                else:
                    print("Unos nije validan. Pokušajte ponovo")
                    clear_console()

        elif choice == '8':
            print("Izmena statusa")

            while True:
                print("1 aktivan")
                print("2 neaktivan")
                status = input("Izbor: ")

                if status == '1':
                    updated_apartment['active'] = True
                    break
                elif status == '2':
                    updated_apartment['active'] = False
                    break
                print("Unos nije validan. Pokušajte ponovo")

        elif choice == '9':
            conflict_num = 0
            for reservation in reservations.values():
                if reservation['apartment'] == apartment_id \
                        and conflict_apartment_reservation(updated_apartment, reservation):
                    conflict_num += 1

            if conflict_num:
                chr = 'a'
                word = 'koje su'
                if conflict_num in (2, 3, 4):
                    chr = 'e'
                if conflict_num == 1:
                    word = 'koja je'

                print(
                    f"\nPostoji {conflict_num} rezervacij{chr} {word} u konfliktu sa izmenjenim apartmanom.")
                if not yn_prompt("želite da izmenite apartman i otkažete ove rezervacije"):
                    return user

            apartments[apartment_id] = updated_apartment

            update_apartments(apartments)
            update_reservation_statuses()
            resolve_reservation_statuses()

            print("Izmene su sačuvane. Pritisnite 'Enter' za dalje ")
            return user

        elif choice == '10':
            return user

        else:
            print("Izbor nije validan. Pokušajte ponovo")


# host 14
def remove_an_apartment(user):
    apartments = read_apartments()
    reservations = read_reservations()
    all_amenities = read_amenities()

    clear_console()
    print("\t\tBRISANJE APARTMANA\n")

    filtered = filter_apartments_host(apartments, user['username'])

    rows = [
        ['1', 'ispis apartmana'],
        ['2', 'unos šifre apartmana'],
        ['3', 'izađi']
    ]
    table = tabulate(rows)
    while True:
        print(table)
        choice = input("\nVaš izbor: ")

        if choice == '1':
            clear_console()
            print_apartments_host(filtered, all_amenities)
            break
        elif choice == '2':
            break
        elif choice == '3':
            return user
        else:
            print("\nNeispravan unos. Pokušajte ponovo\n")

    while True:
        apartment_id = input("\nUnesite šifru apartmana: ")

        if apartment_id in filtered.keys():
            break
        elif not apartment_id.isdigit():
            print("\nNeispravna šifra")
            input("\nPritisnite 'Enter' za dalje ")
            return user
        else:
            print("\nNe postoji apartman sa datom šifrom")
            input("\nPritisnite 'Enter' za dalje ")
            return user

    conflict_num = 0
    for reservation in reservations.values():
        if reservation['apartment'] == apartment_id:
            conflict_num += 1

    if conflict_num != 0:
        chr = 'a'
        if conflict_num in (2, 3, 4):
            chr = 'e'

        print(f"\nPostoji {conflict_num} rezervacij{chr} na dati apartman.")
        if not yn_prompt("ste sigurni da želite da obrišete apartman i otkažete rezervacije"):
            return user
    else:
        if not yn_prompt("ste sigurni da želite da obrišete apartman"):
            return user

    input("\nApartman je obrisan. Pritisnite 'Enter' za dalje ")

    del apartments[apartment_id]
    update_apartments(apartments)
    update_reservation_statuses()
    resolve_reservation_statuses()

    return user


# host 15, 16
def reservations_manager(user):
    apartments = read_apartments()
    reservations = read_reservations()

    filtered = filter_reservations_host(
        apartments, reservations, user['username'])

    rows = [
        ['1', 'potvrdi rezervaciju'],
        ['2', 'odbij rezervaciju'],
        ['3', 'izađi']
    ]

    print('')
    table = tabulate(rows)
    while True:
        print_reservations(user, filtered)
        print("\n\t\tMENADZMENT REZERVACIJA\n")
        print(table)

        while True:
            choice = input("\nVaš izbor: ")

            print('')
            if choice == '1':
                id = input("\nŠifra rezervacije koju potvrđujete: ")

                if id not in filtered.keys():
                    print("\nRezervacija sa datom šifrom ne postoji")
                else:
                    reservations[id]['status'] = 'accepted'
                    del filtered[id]
                    input("\nRezervacija je potvrđena! Pritisnite 'Enter' za dalje ")

                    update_reservations(reservations)
                    break

            elif choice == '2':
                id = input("\nŠifra rezervacije koju odbijate: ")

                if id not in filtered.keys():
                    print("\nRezervacija sa datom šifrom ne postoji")
                else:
                    if not yn_prompt("ste sigurni da želite da odbijete ovu rezervaciju"):
                        break

                    reservations[id]['status'] = 'declined'
                    del filtered[id]
                    input("\nRezervacija je odbijena! Pritisnite 'Enter' za dalje ")

                    update_reservations(reservations)
                    recalculate_apartments_availability()
                    break

            elif choice == '3':
                return user

            else:
                print("Neispravan izbor.")


def print_apartments_host(apartments, all_amenities):
    headers = ['šifra', 'tip', 'broj soba', 'kapacitet', 'adresa',
               'dostupnost', 'cena po noći', 'dodatna oprema', 'status']
    rows = []

    for id, apartment in apartments.items():
        row = apartment_to_row_host(id, apartment, all_amenities)
        rows.append(row)

    print(tabulate(rows, headers, tablefmt='fancy_grid'))


def apartment_to_row_host(id, apartment, all_amenities):
    return [
        id,
        'apartman' if apartment['type'] == 'apartment' else 'soba',
        str(apartment['room_num']),
        str(apartment['guest_num']),
        list_to_str(apartment['location']['address'].values(), ', '),
        dates_to_str(apartment['terms']),
        str(apartment['price'])+' €',
        list_to_str(decode_list(apartment['amenities'], all_amenities)),
        'aktivan' if apartment['active'] else 'neaktivan'
    ]


def filter_apartments_host(apartments, host):
    filtered = {}
    for id, apartment in apartments.items():
        if apartment['host'] == host:
            filtered[id] = apartment

    return filtered


def filter_reservations_host(apartments, reservations, username, admin=False):
    filtered_apartments = filter_apartments_host(apartments, username)
    filtered_reservations = {}

    for id, reservation in reservations.items():
        if reservation['apartment'] in filtered_apartments.keys():
            if (not admin and reservation['status'] == 'pending') or admin:
                filtered_reservations[id] = reservation

    return filtered_reservations


def print_amenities(amenities):
    amenities_rows = []
    for id, amenity in sorted(amenities.items()):
        amenities_rows.append([id, amenity])
    print(tabulate(amenities_rows, [
          "šifra", "dodatna oprema"], tablefmt='fancy_grid'))
