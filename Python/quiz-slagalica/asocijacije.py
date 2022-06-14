from random import randint
import time


def ucitaj_asocijaciju(source):
    file = open(source, "r", encoding="utf-8")
    lines = str.split(file.read(), '\n')
    file.close()

    chosen_line = randint(1, 10)
    association = []

    line_number = 1
    for line in lines:
        if line_number == chosen_line:
            parse_line = str.split(line, '|')

            # parsing like this for easier later iteration
            association = [
                {'key': 'A1', 'value': parse_line[0], 'hidden': True},
                {'key': 'B1', 'value': parse_line[5], 'hidden': True},
                {'key': 'A2', 'value': parse_line[1], 'hidden': True},
                {'key': 'B2', 'value': parse_line[6], 'hidden': True},
                {'key': 'A3', 'value': parse_line[2], 'hidden': True},
                {'key': 'B3', 'value': parse_line[7], 'hidden': True},
                {'key': 'A4', 'value': parse_line[3], 'hidden': True},
                {'key': 'B4', 'value': parse_line[8], 'hidden': True},
                {'key': 'A', 'value': parse_line[4], 'hidden': True},
                {'key': 'B', 'value': parse_line[9], 'hidden': True},
                {'key': '???', 'value': parse_line[20], 'hidden': True},
                {'key': 'C', 'value': parse_line[14], 'hidden': True},
                {'key': 'D', 'value': parse_line[19], 'hidden': True},
                {'key': 'C4', 'value': parse_line[13], 'hidden': True},
                {'key': 'D4', 'value': parse_line[18], 'hidden': True},
                {'key': 'C3', 'value': parse_line[12], 'hidden': True},
                {'key': 'D3', 'value': parse_line[17], 'hidden': True},
                {'key': 'C2', 'value': parse_line[11], 'hidden': True},
                {'key': 'D2', 'value': parse_line[16], 'hidden': True},
                {'key': 'C1', 'value': parse_line[10], 'hidden': True},
                {'key': 'D1', 'value': parse_line[15], 'hidden': True}
            ]

            break
        line_number += 1

    return association


# calculating the longest revealed word
def get_max_string(association):
    max_len = 8

    for field in association:
        if field['hidden'] == False and len(field['value']) > max_len:
            max_len = len(field['value'])

    if max_len % 2 == 1:
        max_len += 1  # makes printing lines easier

    return max_len


# returns key if the field is hidden
def print_field(field):
    if field['hidden'] == True:
        return field['key']

    return field['value']


# printing line with necessary spaces and indents by given parameters
#   (messy but does the job)
def print_line(field1, field2, max_len, indent, width):
    print(f'''█{'': <{indent}}|{field1: ^{max_len}}|{'': ^{width - 2 * indent - 2 * (max_len + 2)}}|{field2: ^{max_len}}|{'': >{indent}}█''')


def print_association(association):
    max_len = get_max_string(association)

    step = max_len // 1.5  # this can be tweaked, bigger numbers - more narrow output (>0)
    width = int((3 * step + max_len) * 2 + 7)

    print('▅' * (width + 2))  # upper outline

    indent = 0

    # printing first 4 lines
    for i in range(0, 9, 2):
        field1 = print_field(association[i])
        field2 = print_field(association[i + 1])

        print_line(field1, field2, max_len, indent, width)

        if i < 6:
            indent += step  # incrementing indent just 3 times

    # printing central field
    solution = association[10]
    if solution['hidden']:
        solution = '???'
    else:
        solution = print_field(association[10])

    print(f'''█{'|' + solution + '|': ^{width}}█''')

    # printing last 4 lines
    for i in range(11, 21, 2):
        field1 = print_field(association[i])
        field2 = print_field(association[i + 1])

        print_line(field1, field2, max_len, indent, width)

        if i != 11:
            indent -= step  #decrementing indent

    print('▀' * (width + 2))  # lower outline

    return width


def convert_utf_chars(input_string):
    edited = input_string
    edited = edited.replace('Č', 'C')
    edited = edited.replace('Ć', 'C')
    edited = edited.replace('Š', 'S')
    edited = edited.replace('Đ', 'DJ')
    edited = edited.replace('Ž', 'Z')

    return edited


def reveal_column(association, column):
    for field in association:
        if field['key'][0] == column:
            field['hidden'] = False


# poor solution using this function in just one case
# of guessing one column in midgame
def remove_column_from_list(hidden_fields, column):
    points = 0
    temp = hidden_fields
    for field in temp:
        if field[0] == column:
            hidden_fields.remove(field)
            points += 1
    
    return points


def reveal_association(association):
    for field in association:
        field['hidden'] = False
    
    print(f'''{'REŠENJE': ^53}\n''')


def asocijacije(source):
    association = ucitaj_asocijaciju(source)

    all_fields = [i["key"] for i in association]
    valid_guess = ['???', 'A', 'B', 'C', 'D']
    hidden_fields = []

    for field in all_fields:
        if field not in valid_guess:
            hidden_fields.append(field)

    def clearConsole(): return print('\n' * 20)

    print(f'''\n{'IGRA ASOCIJACIJE' : ^53}\n''')
    print(f'''{'"dalje" da preskočite': ^53}''')
    print(f'''{'"izadji" da izađete': ^53}''')
    print(f'''{'max 34 poena': ^53}\n\n''')

    counter = 17  # number of allowed tries
    points = 0   # the value of all fields (columns = 3; solution = 7)
    revealed_fields = 0

    while counter > 0:
        print_association(association)

        print('\n')
        # if there are no fields to be opened skipping this
        if hidden_fields:
            while True:
                open_field = input('Otvorite polje: ').upper()

                if open_field in hidden_fields:
                    hidden_fields.remove(open_field)
                    revealed_fields += 1
                    break
                elif open_field == 'IZADJI':
                    break
                else:
                    print('Neispravan unos!')

            if open_field == 'IZADJI':
                clearConsole()
                reveal_association(association)
                print_association(association)
                return 0

            for field in association:
                if field['key'] == open_field:
                    field['hidden'] = False
                    break

        false_answer = False
        solved_association = False

        clearConsole()
        print_association(association)

        print('\n')
        # guessing until wrong answer or association solution
        while len(valid_guess) != 0:
            guess_field = input('Pogađate polje: ').upper()

            if guess_field == 'IZADJI':
                clearConsole()
                reveal_association(association)
                print_association(association)
                return 0
            elif guess_field == 'DALJE':
                break
            elif guess_field in valid_guess:
                false_answer = False
                guess = convert_utf_chars(input('Unos: ').upper())

                for field in association:
                    if field['key'] == guess_field:
                        # first variant - it's association solution
                        if guess_field == '???':
                            if convert_utf_chars(field['value']) == guess:
                                print('Tačno')
                                time.sleep(2)
                                points += 7

                                valid_guess.remove('???')

                                # terribly optimized calculating points from each column
                                for column in valid_guess:
                                    points += 3
                                    reveal_column(association, column[0])
                                    for field in hidden_fields:
                                        if field[0] == column[0]:
                                            points += 1

                                solved_association = True
                                break
                            else:
                                print('Netačno.')
                                time.sleep(2)
                                false_answer = True
                                break
                        # second variant - it's a column
                        else:
                            if convert_utf_chars(field['value']) == guess:
                                print('Tačno!')
                                time.sleep(2)
                                points += 3  #points for guessing column

                                reveal_column(association, guess_field[0])
                                # points for each hidden field
                                points += remove_column_from_list(hidden_fields, guess_field[0])
                                valid_guess.remove(field['key'])

                                clearConsole()
                                print_association(association)
                                print('\n')
                                break
                            else:
                                print('Netačno.')
                                time.sleep(2)
                                false_answer = True
                                break
                if false_answer or solved_association:
                    break
            else:
                print('Neispravan unos.')

        if solved_association:
            break

        clearConsole()
        counter -= 1

    clearConsole()
    reveal_association(association)
    print_association(association)

    if counter == 0:
        print('Nemate više dozvoljenih pokušaja.')
    if solved_association:
        print('Čestitamo!!')
    
    print('U Asocijacijama ste osvojili', points, 'poena.\n')

    return points
