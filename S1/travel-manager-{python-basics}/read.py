from utils import *


def get_rows(filecontent):
    result = []

    for line in filecontent.split('\n'):
        if line:
            row = line.split('|')
            result.append(row)

    return result


def read_data(filepath, format: str) -> tuple:
    file = open(filepath, 'r')
    content = file.read()
    file.close()

    rows = get_rows(content)
    data = {}
    error = None
    column_count = 0

    if format == 'users':
        column_count = 9
    elif format == 'apartments':
        column_count = 11
    elif format == 'reservations':
        column_count = 7
    elif format == 'amenities':
        column_count = 2

    for i in range(len(rows)):
        row = rows[i]
        if column_count == 0:
            error = (f'greška u pozivanju funkcije argument "{format}"', '/')
            break
        elif len(row) != column_count:
            error = ('pogrešan broj polja', i + 1)
            break
        elif row[0] in data.keys():
            error = ('ponovljeno ID polje', i + 1)
            break

        if format == 'users':
            entity = parse_user(row)
        elif format == 'apartments':
            entity = parse_apartment(row)
        elif format == 'reservations':
            entity = parse_reservation(row)
        elif format == 'amenities':
            entity = parse_amenity(row)

        data[row[0]] = entity

    if not error:
        return data
    else:
        message = f'greška: čitanje podataka, {error[0]}\n' + \
            f'fajl "{filepath}", linija {error[1]}'
        print(message)
        exit(0)


def read_dates(filepath) -> tuple:
    file = open(filepath, 'r')
    content = file.read()
    file.close()

    rows = content.split('\n')
    dates = set()
    error = None

    for i in range(len(rows)):
        row = rows[i]

        if not row:
            continue

        try:
            date = datetime.strptime(row, '%d/%m').date()
            dates.add(date)
        except:
            error = ('datum u pogrešnom formatu', i + 1)
            break

    if not error:
        return dates
    else:
        message = f'greška: čitanje podataka, {error[0]}\n' + \
            f'fajl "{filepath}", linija {error[1]}'
        print(message)
        exit(0)


def parse_user(row):
    return {
        'password': row[1],
        'name': row[2],
        'surname': row[3],
        'gender': row[4],
        'phone': row[5],
        'email': row[6],
        'role': row[7],
        'blocked': str_to_bool(row[8]),
    }


def parse_apartment(row):
    location = row[4].split(';')
    address = location[2].split(',')
    terms = row[5].split(',')
    terms = str_to_dates(terms)
    if row[6]:
        availability = row[6].split(',')
        availability = str_to_dates(availability)
    else:
        availability = []
    amenities = row[10].split(',')

    return {
        'type': row[1],
        'room_num': int(row[2]),
        'guest_num': int(row[3]),
        'location': {
            'position': (float(location[0]), float(location[1])),
            'address': {
                'street': address[0],
                'city': address[1],
                'postal': address[2]
            }
        },
        'terms': terms,
        'availability': availability,
        'host': row[7],
        'price': float(row[8]),
        'active': str_to_bool(row[9]),
        'amenities': amenities
    }


def parse_reservation(row):
    guests = row[5].split(',')
    return {
        'apartment': row[1],
        'start_date': str_to_date(row[2]),
        'nights_num': int(row[3]),
        'full_price': float(row[4]),
        'guests': guests,
        'status': row[6]
    }


def parse_amenity(row):
    return row[1]
