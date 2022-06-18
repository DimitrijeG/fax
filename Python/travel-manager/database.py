from read import read_data, read_dates
from write import write_data, write_holidays
import pathlib
from utils import *


def get_directory() -> str:
    return str(pathlib.Path(__file__).parent.resolve()) + '/'


def read_users(filepath='data/users.txt'):
    filepath = get_directory() + filepath
    return read_data(filepath, 'users')


def read_apartments(filepath='data/apartments.txt'):
    filepath = get_directory() + filepath
    return read_data(filepath, 'apartments')


def read_reservations(filepath='data/reservations.txt'):
    filepath = get_directory() + filepath
    return read_data(filepath, 'reservations')


def read_amenities(filepath='data/amenities.txt'):
    filepath = get_directory() + filepath
    return read_data(filepath, 'amenities')


def read_holidays(filepath='data/holidays.txt'):
    filepath = get_directory() + filepath
    return read_dates(filepath)


def update_users(users, filepath='data/users.txt'):
    filepath = get_directory() + filepath
    write_data(filepath, users, 'users')


def update_apartments(apartments, filepath='data/apartments.txt'):
    filepath = get_directory() + filepath
    write_data(filepath, apartments, 'apartments')


def update_reservations(reservations, filepath='data/reservations.txt'):
    filepath = get_directory() + filepath
    write_data(filepath, reservations, 'reservations')


def update_amenities(amenities, filepath='data/amenities.txt'):
    filepath = get_directory() + filepath
    write_data(filepath, amenities, 'amenities')


def update_holidays(holidays, filepath='data/holidays.txt'):
    filepath = get_directory() + filepath
    write_holidays(filepath, holidays)


def recalculate_apartments_availability():
    apartments = read_apartments()
    reservations = read_reservations()

    updated = {}
    for id, apartment in apartments.items():
        updated[id] = update_apartment_availability(
            id, apartment, reservations)

    update_apartments(updated)


def update_apartment_availability(id, apartment, reservations):
    updated_availability = apartment['terms'].copy()
    for res_id, reservation in reservations.items():
        if reservation['apartment'] == id and reservation['status'] in ('pending', 'accepted'):
            updated_availability = []
            reservation_start = reservation['start_date']
            reservation_end = reservation_start + \
                timedelta(days=reservation['nights_num'])

            error = True
            for date_range in apartment['terms']:
                if reservation_start >= date_range[0] and reservation_end <= date_range[1]:
                    if reservation_start != date_range[0]:
                        updated_availability.append(
                            (date_range[0], reservation_start))
                    if reservation_end != date_range[1]:
                        updated_availability.append(
                            (reservation_end, date_range[1]))
                    error = False
                else:
                    updated_availability.append(date_range)

            if error:
                message = f'greška: konflikt termina rezervacije i dostupnog termina' + \
                    f'\nid apartmana: {id}; id rezervacije: {res_id}'
                print(message)
                exit(0)
            break

    apartment['availability'] = updated_availability

    return apartment


# posle menjanja/brisanja apartmana
def update_reservation_statuses():
    apartments = read_apartments()
    reservations = read_reservations()

    reservations_to_decline = set()

    for ap_id, apartment in apartments.items():
        for res_id, reservation in reservations.items():
            if (reservation['apartment'] == ap_id
                and conflict_apartment_reservation(apartment, reservation)) \
                    or reservation['apartment'] not in apartments.keys():
                reservations_to_decline.add(res_id)

    for id in reservations_to_decline:
        reservations[id]['status'] = 'removed'

    update_reservations(reservations)


def conflict_apartment_reservation(apartment, reservation) -> bool:
    if not apartment['active'] or not apartment['terms']:
        return True
    if reservation['status'] not in ('pending', 'accepted'):
        return False

    guest_num = len(reservation['guests']) - 1

    if guest_num > apartment['guest_num']:
        return True

    start_date = reservation['start_date']
    end_date = start_date + timedelta(days=reservation['nights_num'])

    for date_range in apartment['terms']:
        if start_date >= date_range[0] and end_date <= date_range[1]:
            return False
    return True


# kraj i pocetak app
def resolve_reservation_statuses():
    reservations = read_reservations()

    removed_ids = []
    for id, reservation in reservations.items():
        end_date = reservation['start_date'] + \
            timedelta(days=reservation['nights_num'])

        if reservation['status'] == 'removed':
            removed_ids.append(id)
        if reservation['status'] == 'accepted' and end_date < datetime.now().date():
            reservation['status'] = 'completed'
        if reservation['status'] == 'pending' and end_date < datetime.now().date():
            reservation['status'] = 'declined'

    for id in removed_ids:
        del reservations[id]

    update_reservations(reservations)
