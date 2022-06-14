import random
from random import shuffle


def get_questions(source):
    file = open(source, "r", encoding="utf-8")
    lines = str.split(file.read(), '\n')
    file.close()

    questions = []

    # generating 10 random numbers for quesitons in range [1,50]
    # random.sample() did more repetitions than this method
    all_lines = list(range(1, 51))
    random.shuffle(all_lines)
    chosen_lines = all_lines[-10:]

    # reading file and taking just lines with chosen indexes
    line_number = 1
    for line in lines:
        if line_number in chosen_lines:
            parse_line = str.split(line, '|')

            question = {
                'text': parse_line[0],
                'answ1': parse_line[1],
                'answ2': parse_line[2],
                'answ3': parse_line[3],
                # every right answer is in the last segment
                'right': parse_line[4]
            }

            questions.append(question)

        line_number += 1

    return questions


def ko_zna_zna(source):
    questions = get_questions(source)

    print(f'''\n{'IGRA KO ZNA ZNA' : ^50}\n''')
    print(f'''{'"dalje" da preskočite': ^50}''')
    print(f'''{'"izadji" da izađete': ^50}''')
    print(f'''{'max 100 poena': ^50}''')

    points = 0
    count = 1  # just for the question number

    for question in questions:
        print(f'\n\n({count})', question['text'])

        # creating buffer for shuffling answers
        answers = [question['answ1'], question['answ2'],
                   question['answ3'], question['right']]
        shuffle(answers)

        # printing available answers
        for i in range(4):
            print(f'\t{i+1}  {answers[i]}')

        print()

        # forcing valid input
        while True:
            odgovor = input('Vas odgovor: ')

            if odgovor == 'dalje' or odgovor == 'izadji':
                break

            # breaks the loop if input is in range
            if odgovor.isdigit() and int(odgovor) in range(1, 5):
                break
            else:
                print('Neispravan unos!')

        if odgovor == 'izadji':
            return points
        elif odgovor == 'dalje':
            count += 1
            continue

        odgovor = int(odgovor)

        if answers[odgovor - 1] == question['right']:
            print('Tačno!')
            points += 10
        else:
            print('Netačno. Rešenje je:', question['right'])
            points -= 5

        count += 1

    if points == 100:
        print('Skidamo kapu!')
    
    print('\nU Ko zna zna ste osvojili', points, 'poena.')

    return points
