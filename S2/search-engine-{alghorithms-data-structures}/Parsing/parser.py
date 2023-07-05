from html.parser import HTMLParser
import os
import re


class Parser(HTMLParser):

    def handle_starttag(self, tag: str, attrs: list) -> None:
        """Metoda belezi sadrzaj href atributa
        Poziv metode vrsi se implicitno prilikom nailaska na tag
        unutar HTML fajla. Ukoliko je u pitanju anchor tag, belezi
        se vrednost href atributa.

        :param tag: naziv taga
        :type tag: str
        :param attrs: lista atributa
        :type attrs: list
        """
        if tag == 'a':
            # typecast da izbegnem looping
            attrs = dict(attrs)
            link = attrs['href']

            # ignorisi spoljnje linkove i uzmi u obzir samo html fajlove
            if not link.startswith('http'):
                # ukloni sekciju iz linka
                hash_index = link.rfind('#')
                if hash_index > -1:
                    link = link[:hash_index]

                if link.endswith('html') or link.endswith('htm'):
                    relative_path = os.path.join(self.path_root, link)
                    link_path = os.path.abspath(relative_path)
                    self.links.append(link_path)

    def handle_data(self, data: str) -> None:
        """Metoda belezi pronadjene reci
        Poziv metode vrsi se implicitno prilikom nailaska na sadrzaj
        HTML elemenata. Sadrzaj elementa se deli u reci koje se beleze
        u odgovarajucu listu.

        :param data: dobijeni sadrzaj elementa
        :type data: str
        """
        stripped_text = re.sub('[\W]', ' ', data).split()
        if stripped_text:
            self.words.extend(stripped_text)

    def parse(self, path: str) -> tuple[list, list]:
        """Metoda ucitava sadrzaj fajla i prosledjuje ga parseru

        :param path: putanja do fajla
        :type path: str
        """
        self.links = []
        self.words = []

        try:
            with open(path, 'r', encoding='utf-8') as document:
                self.path_root = os.path.abspath(os.path.dirname(path))
                content = document.read()
                self.feed(content)

                # ocisti duplikate
                self.links = list(set(self.links))

        except IOError as e:
            print(e)
        finally:
            return self.words, self.links
