import time
from ko_zna_zna import ko_zna_zna
from asocijacije import asocijacije
from spojnice import spojnice

 
def main():
    # paths for .txt containing data required for games
    #   (yours may differ)
    ko_zna_zna_source = 'assets/ko_zna_zna.txt'
    asocijacije_source = 'assets/asocijacije.txt'
    spojnice_source = 'assets/spojnice.txt'

    all_points = 0

    points1 = spojnice(spojnice_source)
    input('Unesite bilo koji karakter za sledeću igru: ')
    print('\n' * 20)
    points2 = ko_zna_zna(ko_zna_zna_source)
    input('Unesite bilo koji karakter za sledeću igru: ')
    print('\n' * 20)
    points3 = asocijacije(asocijacije_source)

    all_points = points1 + points2 + points3

    # little output detail
    print('\n')
    for i in range(19):
        output = f'''{'=' * i}{'': ^{36 - 2 * i}}{'=' * i}'''
        print(output, end='\r')
        time.sleep(0.05)

    print(f'''\n\n{'': <11}{'OSVOJENI POENI'}\n''')
    print(f'''{'': <10}{'Spojnice:': <{16 - len(str(points1))}}{points1}''')
    print(f'''{'': <10}{'Ko zna zna:': <{16 - len(str(points2))}}{points2}''')
    print(f'''{'': <10}{'Asocijacije:': <{16 - len(str(points3))}}{points3}''')
    print(f'''{'': <10}{'-' * 16}''')
    print(f'''{'': <10}{'Ukupno:': <{16 - len(str(all_points))}}{all_points}''')
    print('\nHvala Vam što ste igrali Slagalicu :)')


if __name__ == "__main__":
    main()
