from random import randint
from random import shuffle


def ucitaj_spojnicu(source):
    file = open(source, "r", encoding="utf-8")
    lines = str.split(file.read(), '\n')
    file.close()

    chosen_line = randint(1, 10)
    spojnica = {}

    line_number = 1
    for line in lines:
        if line_number == chosen_line:
            parse_line = str.split(line, '|')

            spojnica = {
                'text': parse_line[0]
            }

            for i in range(1, 9):
                spojnica[parse_line[i]] = parse_line[i + 8]

            break

        line_number += 1

    return spojnica


# returns the length of the longest word in list
def get_max_str(list):
    max_len = 0
    for field in list:
        if len(field) > max_len:
            max_len = len(field)

    return max_len


def print_line(str1, str2, max_len1, max_len2, i):
    print(f'''█|{str1: ^{max_len1 + 2}}| {i} - {i} |{str2: ^{max_len2 + 2}}|█''')


def izmesaj_spojnicu(original):
    column1 = []
    column2 = []

    for key, value in original.items():
        if key != 'text':
            column1.append(key)
            column2.append(value)

    shuffle(column1)
    shuffle(column2)

    izmesana = {}
    izmesana['text'] = original['text']
    for i in range(0, 8):
        izmesana[column1[i]] = column2[i]

    return izmesana


# swapping 2 values in right column with indexes
def update_spojnicu(spojnica, index1, index2):
    column1 = []
    column2 = []

    for key, value in spojnica.items():
        if key != 'text':
            column1.append(key)
            column2.append(value)

    # before: left1-right1
    #         left2-right2
    left1 = column1[index1]
    left2 = column1[index2]
    right1 = column2[index1]
    right2 = column2[index2]

    izmenjena = spojnica

    izmenjena[left1] = right2
    izmenjena[left2] = right1

    return izmenjena


def ispisi_spojnicu(spojnica):
    column1 = []
    column2 = []

    print(spojnica['text'])
    for key, value in spojnica.items():
        if key != 'text':
            column1.append(key)
            column2.append(value)

    column1_width = get_max_str(column1)
    column2_width = get_max_str(column2)

    width = column1_width + column2_width + 17

    print('▅' * width)  # upper outline

    for i in range(0, 8):
        print_line(column1[i], column2[i], column1_width, column2_width, i + 1)

    print('▀' * width)  #lower outline


def spojnice(source):
    original = ucitaj_spojnicu(source)
    spojnica = izmesaj_spojnicu(original)

    print(f'''\n{'IGRA SPOJNICE' : ^50}\n''')
    print(f'''{'"kraj" da završite povezivanje': ^50}''')
    print(f'''{'"izadji" da izađete': ^50}''')
    print(f'''{'max 16 poena': ^50}\n\n''')

    counter = 20  # the user has 20 tries
    points = 0

    while counter != 0:
        ispisi_spojnicu(spojnica)

        # forcing valid input (white spaces allowed)
        while True:
            unos = input('Povezujete (x-x): ')

            if unos == 'izadji' or unos == 'kraj':
                break

            unos = unos.replace(' ', '')
            unos = unos.replace('(', '')
            unos = unos.replace(')', '')

            if len(unos) == 3 and unos[0].isdigit() and unos[1] == '-' and unos[2].isdigit:
                break
            else:
                print('Neispravan unos')

        if unos == 'izadji':
            return 0
        elif unos == 'kraj':
            break

        index1 = int(unos[0])
        index2 = int(unos[2])

        spojnica = update_spojnicu(spojnica, index1 - 1, index2 - 1)
        counter -= 1

        print('\n' * 20)

    # calculating points by comparing original and edited pairs
    for key, value in spojnica.items():
        if key != 'text':
            if original[key] == value:
                points += 2

    print('\n' * 20)
    print(f'''{'REŠENJE' : ^50}\n''')
    
    for key, value in original.items():
        spojnica[key] = value
    
    ispisi_spojnicu(spojnica)

    if points == 16:
        print('Čestitamo!!')

    if counter == 0:
        print('Nemate više dozvoljenih pokušaja.', end='')
    
    print('U Spojnicama ste osvojili', points, 'poena.\n')

    return points
