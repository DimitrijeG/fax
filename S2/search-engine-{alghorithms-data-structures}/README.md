# HTML Search Engine
Autor: *Dimitrije Gasic,  SV 31/2021*

Konzolna aplikacija napisana u Python 3 programskom jeziku. 
Glavna svrha aplikacije je pretraga kljucnih reci u velikom broju HTML fajlova.

Kao i vecina Search Engine algoritama, koristi Trie i Graph za ucitavanje kako 
reci iz fajlova tako i medjusobnih linkova. Od ostalih struktura podataka implementirani su 
Set (pomocu recnika) i Stack kao pomocne kolekcije, a od internih tipova koriscene su liste i 
recnici. 

Program se u sustini sastoji iz 3 velika entiteta koji medjusobno komuniciraju: Client, Query i sam Search Engine.

### Client

Modul koji uglavnom sluzi kao korisnicki interfejs aplikacije. Pruza sledece funkcionalnosti:
* cd       - promena putanje
* ls       - ispis fajlova i foldera u trenutnoj putanji
* pwd      - ispis trenutne putanje
* cls      - ciscenje terminala
* search   - pretraga HTML fajlova *
* settings - podesavanja globalnih parametara

Pored ovih metoda, Client takodje interno cita fajlove u putanji putem os.walk().

### Query

Funkcija ovog modula je pribavljanje, validacija i evaluacija upita. Podrzava bilo koju kombinaciju sledecih logickih operatora kao i zagrada: `AND, OR, NOT, XOR`. U slucaju da se izostavi operator izmedju 2 reci, Query na to mesto dodaje `OR` operator.

Nakon validacije i prompta korisniku da li zeli da nastavi, vrsi se sama evaluacija izraza pomocu postfiksne notacije i Search Engina (za detaljnije vidite ***Search Engine*** i dokumentaciju). 

Pre ispisa se vrsi sortiranje rezultata pomocu `Heap Sort` algoritma (*broj rezultata retko prelazi 1000 pa nema potrebe za merge ili quicksortom*) i kompresija rankova u opseg [0,10] zbog preglednosti

Na kraju se sortirani i rangirani rezultati ispisuju u vidu tabele sa fiksnim brojem rezultata po strani (moguce promeniti u *settings*). Za top rezultat se takodje ispisuje isecak HTML fajla sa numerisanim linijama i oznacenim recima koriscenim u pretrazi. Detaljno objasnjenje kako ovaj parser radi mozete pronaci u `Parsing/sample_file.py`

### Search Engine

Modul koji se bavi generisanjem pogodnih struktura za pretragu (Trie, Graph), pretragom i rangiranjem rezultata.

Generisanje podataka se sprovodi prolaskom kroz sve prosledjene fajlove i upisivanjem svake *vidljive* reci u Trie (sa fajlom iz kog potice). Pored toga, Graph belezi svaki odlazni link putem ivica i cvorova.

Pretraga se prakticno svodi na pozivanje funkcije `search()` iz modula `Collections/trie.py` i rangiranje dobijenih rezulata. 
Rangiranje se sprovodi modifikovanim `PageRank` algoritmom. Ovaj algoritam se krece u dubinu (preporucljivo 2 vidi *settings*) i pritom racuna rank stranice na osnovu broja kljucnih reci, ranka dolaznih linkova i broja odlaznih linkova za dolazne.
Ovo oslikava i sledeca formula:
$ R(V_i) = reci(V_i) + \sum_{V_j \in dolazni(V_i)}^n \frac{R(V_j)}{len(odlazni(V_j))} $

Za vise mozete pogledati: [TextRank: Bringing Order into Texts](https://web.eecs.umich.edu/~mihalcea/papers/mihalcea.emnlp04.pdf)

