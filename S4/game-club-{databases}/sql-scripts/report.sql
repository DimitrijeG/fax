CREATE OR REPLACE PROCEDURE Report IS
    CURSOR curKlijent IS
        SELECT Idkli, Korime, Ime, Prz, COUNT(Idrec) RecCount
        FROM klijent k INNER JOIN recenzija r ON k.Idkli = r.Pise
        GROUP BY Idkli, Korime, Ime, Prz;
    CURSOR curRecenzija(idKlijenta klijent.Idkli%TYPE) IS
        SELECT Idrec, Ocena, Tekst, Pise
        FROM recenzija
        WHERE Pise = idKlijenta;
    vKlijent curKlijent%ROWTYPE;
    vRecenzija curRecenzija%ROWTYPE;
BEGIN
    OPEN curKlijent;
    LOOP
        FETCH curKlijent INTO vKlijent;
        EXIT WHEN curKlijent%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE('Id klijenta: ' || vKlijent.Idkli || ', Korisnicko ime: ' || vKlijent.Korime || ', Ime: ' || vKlijent.Ime || ', Prezime: ' || vKlijent.Prz || ', Broj recenzija: ' || vKlijent.RecCount);
        OPEN curRecenzija(vKlijent.Idkli);
        LOOP
            FETCH curRecenzija INTO vRecenzija;
            EXIT WHEN curRecenzija%NOTFOUND;
            DBMS_OUTPUT.PUT_LINE('    Id recenzije: ' || vRecenzija.Idrec || ', Ocena: ' || vRecenzija.Ocena || ', Tekst: ' || vRecenzija.Tekst);
        END LOOP;
        CLOSE curRecenzija;
    END LOOP;
    CLOSE curKlijent;
END Report;
/

exec Report;