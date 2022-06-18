from utils import *


def write_data(filepath, data, format: str):
    file = open(filepath, 'w')

    lines = []

    for id, entity in data.items():
        row = []

        if format == 'users':
            row = write_user(id, entity)
        elif format == 'apartments':
            row = write_apartment(id, entity)
        elif format == 'reservations':
            row = write_reservation(id, entity)
        elif format == 'amenities':
            row = write_amenity(id, entity)
        else:
            message = f'greška: pisanje podataka\n' + \
                f'fajl "{filepath}", id {id}'
            return message

        lines.append('|'.join(row))

    content = list_to_str(lines)

    file.write(content)
    file.close


def write_holidays(filepath, holidays):
    file = open(filepath, 'w')

    lines = []

    for holiday in holidays:
        lines.append(date_to_str(holiday, '/', False))

    content = list_to_str(lines)

    file.write(content)
    file.close()


def write_user(username, user):
    return [
        username,
        user['password'],
        user['name'],
        user['surname'],
        user['gender'],
        user['phone'],
        user['email'],
        user['role'],
        str(user['blocked'])
    ]


def write_apartment(id, apartment):
    address = list_to_str(apartment['location']['address'].values(), ',')
    position = str(round(apartment['location']['position'][0], 4)) + ';' \
        + str(round(apartment['location']['position'][1], 4))
    location = position + ';' + address

    return [
        id,
        apartment['type'],
        str(apartment['room_num']),
        str(apartment['guest_num']),
        location,
        dates_to_str(apartment['terms'], 'str', '-', ','),
        dates_to_str(apartment['availability'], 'str', '-', ','),
        apartment['host'],
        str(round(apartment['price'], 2)),
        str(apartment['active']),
        ','.join(apartment['amenities'])
    ]


def write_reservation(id, reservation):
    guests = ','.join(reservation['guests'])
    return [
        id,
        reservation['apartment'],
        date_to_str(reservation['start_date']),
        str(reservation['nights_num']),
        str(round(reservation['full_price'], 2)),
        guests,
        reservation['status']
    ]


def write_amenity(id, amenity):
    return [
        id,
        amenity
    ]
