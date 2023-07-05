# Slagalica
Autor: *Dimitrije Gašić,  SV 31/2021*

Konzolna aplikacija napisana u Python 3 jeziku. Sastoji se iz 3 igre:
* Spojnice
* Ko zna zna
* Asocijacije

Uz izvorni kod u paketu se nalaze 3 `.txt` fajla iz kojih se učitavaju sredstva za igre. Svaka igra se pokreće iz `main()` funkcije u fajlu `slagalica.py`. Iz svake igre je moguće "iskočiti" u bilo kom momentu unošenjem komande ```izadji```. Maksimalan broj poena koji je moguće osvojiti iznosi **150**.

### Spojnice

Igra je identična poznatoj igri Spojnica iz TV Slagalice, sa razlikom u broju polja za povezivanje kojih ima **8**. Samim tim maksimalan broj bodova koji može da se osvoji je **16**. Zbog jednostavnosti, algoritam za povezivanje pomera polja desne kolone onako kako korisnik odabere, kao što je prikazano u sledećem primeru.

**Povežite hemijske elemente sa simbolima**
|          | | |       |    
|:---------|-|-|------:|    
|Magnezijum|1|1|  N    |    
|Olovo     |2|2|  Pb   |    
|Azot      |3|3|  Mg   |

*Unos (x-x):* **1-3**

|          | | |       |    
|:---------|-|-|------:|    
|Magnezijum|1|1|  Mg   |    
|Olovo     |2|2|  Pb   |    
|Azot      |3|3|  N    |

Podaci postojećih **10** tabela su smešteni u `assets/spojnice.txt` fajlu u sledećem obliku:
```
tekst spojnice|svi elementi kolone1|svi elementi kolone2
```

### Ko zna zna

Klon poznate igre. Svakim pokretanjem se selektuje 10 nasumičnih pitanja iz pruženog `.txt` fajla. Maksimalan broj bodova je **100**. Postojećih **50** pitanja je smešteno u `assets/ko_zna_zna.txt` fajlu u sledećem obliku:
```
tekst pitanja|1. ponuđen|2. ponuđen|3. ponuđen odgovor|rešenje
```
Ponuđeni odgovori se prilikom svakog prikazivanja pitanja nasumično mešaju.

### Asocijacije

Poslednja igra u ovoj aplikaciji dozvoljava osvajanje maksimalno **34** boda, budući da sva polja zajedno vrede **35** bodova u sledećem poretku:

|neotvoreno polje|pogođena kolona|pogođeno rešenje|
|:--------------:|:-------------:|:--------------:|
| 1 poen         | 3 poena       | 7 poena        |

Kao i kod *Ko zna zna*, računanje poena se vrši u toku izvršavanja petlje igre. 
Posle svakog otvorenog polja korisnik ima mogućnost da pogađa bilo koje validno polje. U slučaju da pogodi, dozvoljeno mu je da pokuša ostala (naravno ako nije rešio asocijaciju). Unos podržava latinična slova kao i slova abecede.

Postojećih **10** asocijacija je smešteno u `assets/asocijacije.txt` fajlu u sledećem obliku:
```
A1|A2|A3|A4|A|B1|B2|B3|B4|B|C1|C2|C3|C4|C|D1|D2|D3|D4|D|rešenje
```
