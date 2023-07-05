from datetime import datetime, timedelta
from tabulate import tabulate
from pprint import pprint


def clear_console():
    print("\033[H\033[J", end="")


def str_to_bool(string):
    return True if string.lower() == "true" else False


def list_to_str(list, separator='\n'):
    if list:
        return separator.join(list)
    else:
        return ""


def str_to_dates(ranges):
    dates = []
    for range in ranges:
        range = range.split('-')
        if len(range) != 2:
            return None
        start = str_to_date(range[0])
        end = str_to_date(range[1])
        if not (start and end):
            return None

        dates.append((start, end))
    return dates


def dates_to_str(dates, format='str', separator1=' - ', separator2='\n'):
    output = []
    if not dates:
        return ''
    for pair in dates:
        start, end = pair
        output.append(f"{date_to_str(start)}{separator1}{date_to_str(end)}")
    if format == 'str':
        return list_to_str(output, separator2)
    elif format == 'list':
        result = []
        for pair in output:
            result.append([pair])
        return result


def date_to_str(date, separator='/', year=True):
    if date:
        if year:
            return date.strftime(f"%d{separator}%m{separator}%Y")
        else:
            return date.strftime(f"%d{separator}%m")
    else:
        return None


def str_to_date(string):
    try:
        return datetime.strptime(string, '%d/%m/%Y').date()
    except:
        return None


def time_to_str(time):
    if time:
        return time.strftime(f"%d/%m/%Y %H:%M:%S")
    else:
        return None


def decode_list(coded_list, key):
    decoded_list = []
    for item in coded_list:
        decoded_list.append(key[item])
    return decoded_list


def get_last_id(dictionary, format: int) -> str:
    if dictionary:
        reservation_id_int = int(sorted(dictionary.keys())[-1]) + 1
        return str(reservation_id_int).rjust(format, '0')
    else:
        return '1'.rjust(format, '0')


def input_date(message, allow_chr=None, year=True):
    while True:
        date = input(message)
        if date == allow_chr:
            return None

        try:
            if year:
                date = datetime.strptime(date, '%d/%m/%Y').date()
            else:
                date = datetime.strptime(date, '%d/%m').date()

        except:
            print("\nNeispravan format. Pokušajte ponovo\n")

        else:
            return date


def input_int(message, allow_chr=None, allow_neg=False):
    while True:
        num = input(message)
        if num == allow_chr:
            return None

        try:
            num = int(num)

        except:
            print("\nNeispravan unos. Pokušajte ponovo\n")
            continue

        if not allow_neg and num < 0:
            print("\nNegativni brojevi nisu dozvoljeni\n")
        else:
            return num


def input_float(message, allow_chr=None, allow_neg=False):
    while True:
        num = input(message)
        if num == allow_chr:
            return None

        try:
            num = float(num)

        except:
            print("\nNeispravan unos. Pokušajte ponovo\n")
            continue

        if not allow_neg and num < 0:
            print("\nNegativni brojevi nisu dozvoljeni\n")
        else:
            return num


def yn_prompt(message) -> bool:
    while True:
        choice = input(f"Da li {message} (da/ne): ")
        if choice.strip() == 'da':
            return True
        elif choice.strip() == 'ne':
            return False
        else:
            print("\nNeispravan unos\n")
