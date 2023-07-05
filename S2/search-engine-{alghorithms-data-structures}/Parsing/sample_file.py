from Utils.colors import *
import re
import os


def sample_html_file(path: str, tokens: list, max_len: int = 150, line_span: int = 4) -> None:
    """Custom parser html fajla koji pri pronalasku neke od 'vidljivih' kljucnih reci iseca deo fajla 
    i ispisuje ga sa rasponom od (2 * linespan + 1) linija. Nakon sto dobije index anchor linije
    uzima +n -n linija oko nje, trimmuje razmake sa pocetka koliko je moguce, numerise, i dodaje 
    boju na kljucne reci koje su pronadjene u tom segmentu fajla.

    :param path: putanja do html fajla
    :type path: str
    :param tokens: kljucne reci za pretragu
    :type tokens: list
    :param max_len: maksimalna duzina linije, smanjuje se ako je veca od sirine terminala
    :type max_len: int
    :param line_span: raspon linija koje ce biti ispisane
    :type line_span: int
    :raises: IndexError kada dodje do interne greske
    """
    cli_size = os.get_terminal_size().columns
    # max duzina linije ne moze biti veca od sirine terminala
    max_len = min(max_len, cli_size)

    try:
        with open(path, 'r', encoding='utf-8') as document:
            lines = document.read().split('\n')

            # pronalazi broj linije u kojoj se prvi put pojavljuje neki od vidljivih tokena
            line_num = __find_first_line(lines, tokens)

            if line_num != -1:
                first_line = line_num - line_span
                # uzima primerak linija u odredjenom rasponu (line_span)
                start = max(line_num - line_span, 0)
                stop = min(line_num + line_span + 1, len(lines) - 1)
                output_lines = lines[start:stop]

                # trimovanje razmaka sa pocetka linija koliko je to moguce
                # numerisanje linija
                # trimovanje linija sa kraja ako prelazi maksimalnu duzinu
                trimmed = False
                while not trimmed:
                    for line in output_lines:
                        if line and line[0] != ' ':
                            trimmed = True
                            break
                    if not trimmed:
                        for k in range(len(output_lines)):
                            if output_lines[k]:
                                output_lines[k] = output_lines[k][1:]
                    else:
                        for k in range(len(output_lines)):
                            if output_lines[k]:
                                output_lines[k] = f'{first_line + k + 1 : <9}' + \
                                    output_lines[k]
                                output_lines[k] = output_lines[k][
                                    : min(len(output_lines[k]), max_len)
                                ]

                # dodavanje boja za sve linije kao i kljucne reci
                base_color = bcolors.CGREY
                highlight = bcolors.CBEIGE
                for i in range(len(output_lines)):
                    output_lines[i] = color_string(output_lines[i], base_color)
                    line = output_lines[i]
                    match_indexes = []

                    for token in tokens:
                        match = re.finditer(r'\b({})\b'.format(
                            token), line, re.IGNORECASE)
                        for m in match:
                            match_indexes.append((m.start(), m.end()))
                    if match_indexes:
                        output_lines[i] = replace_color(
                            line,
                            match_indexes,
                            base_color,
                            highlight
                        )

                sample = '\n'.join(output_lines)

                print(color_string('-' * max_len, highlight))
                print(sample)
                print(color_string('-' * max_len, highlight))

            else:
                # izvrsice se jedino ako dodje do greske u radu parsera
                raise IndexError(
                    'Interna greska prilikom kreiranja primerka fajla.')

    except IOError as e:
        # nepostojeci fajl
        print(e)


def __find_first_line(lines: list, tokens: list) -> int:
    """Nalazi prvu liniju u listi koja sadrzi vidljivu rec iz liste tokena.
    Zbog optimizacije preskace <head> tag. Kada nadje izolovanu rec (van ili unutar taga),
    tada iteratorom prolazi okolne karaktere trazeci kraj ili pocetak taga.
    Ako 'sa leva' naidje na tag '>' vraca index linije. Analogno vazi i za desnu stranu.
    U slucaju da ne bude pogotka vraca index -1 koji automatski triggeruje Exception.

    :param lines: linije koje se pretrazuju
    :type lines: list
    :param tokens: kljucne reci za koje se trazi prvi vidljivi pogodak
    :type tokens: list

    :returns: index prve linije sa pogotkom
    :rtype: int
    """
    body = False
    for i in range(len(lines)):
        line = lines[i].strip().lower()
        # preskace linije do body
        if not body:
            if line[:8] == '</head>':
                body = True
        else:
            matches = []
            # mapiranje potencijalnih pogodaka (potrebna provera da li je vidljiv)
            # r'\b()\b' za word boundary
            for token in tokens:
                match = re.finditer(r'\b({})\b'.format(
                    token), line, re.IGNORECASE)
                if match:
                    matches += match

            visible = False
            for match in matches:
                j = 0
                start = match.start() + 1
                end = match.end() - 2

                # 2 iteratora koji pocinju od pocetka/kraja reci
                # i krecu se prema krajevima linije
                while not visible:
                    m = start - j
                    n = end + j
                    if m >= 0:
                        if line[m] == '>':
                            visible = True
                        elif line[m] == '<':
                            break
                    if n < len(line):
                        if line[n] == '<':
                            visible = True
                        elif line[n] == '>':
                            break
                    if m < 0 and n >= len(line):
                        visible = True
                    j += 1

                if visible:
                    return i
    return -1
