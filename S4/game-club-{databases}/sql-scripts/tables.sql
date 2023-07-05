-- PODSISTEM RACUNARSKE OPREME

CREATE TABLE igraonica (
    Idigr integer NOT NULL,
    Mes varchar(20) NOT NULL,
    Adr varchar(30) NOT NULL,
    CONSTRAINT igraonica_pk PRIMARY KEY (Idigr)
);

CREATE TABLE soba (
    Brsobe integer NOT NULL,
    Dimsobe integer NOT NULL,
    Idigr integer NOT NULL,
    CONSTRAINT soba_PK PRIMARY KEY (Idigr, Brsobe),
    CONSTRAINT soba_igr_FK FOREIGN KEY (Idigr) REFERENCES igraonica (Idigr)
);

CREATE TABLE sto (
    Brsto integer NOT NULL,
    Brsobe integer NOT NULL,
    Idigr integer NOT NULL,
    Idrac integer NOT NULL,
    CONSTRAINT sto_PK PRIMARY KEY (Brsto, Brsobe, Idigr),
    CONSTRAINT sto_FK FOREIGN KEY (Brsobe, Idigr) REFERENCES soba (Brsobe, Idigr)
);

-- FINANSIJE

CREATE TABLE vrstapromene (
    Idpro integer NOT NULL,
    Tip varchar(10) NOT NULL,
    Naziv varchar(30),
    CONSTRAINT vrstapromene_PK PRIMARY KEY (Idpro)
);

CREATE TABLE realizacijapromene (
    Idrp integer NOT NULL,
    Datpro date NOT NULL,
    Iznos integer NOT NULL,
    Opis varchar(40),
    Idpro integer NOT NULL,
    CONSTRAINT realizacijapromene_PK PRIMARY KEY (Idrp),
    CONSTRAINT realizacijapromene_FK FOREIGN KEY (Idpro) REFERENCES vrstapromene (Idpro)
);

-- KORISNICKI PODSISTEM

CREATE TABLE klijent (
    Idkli integer NOT NULL,
    Korime varchar(30) NOT NULL,
    Ime varchar(30) NOT NULL,
    Prz varchar(30) NOT NULL,
    Email varchar(30) NOT NULL,
    Loz varchar(50) NOT NULL,
    God date,
    CONSTRAINT klijent_PK PRIMARY KEY (Idkli),
    CONSTRAINT radnik_ime_UK UNIQUE (Korime),
    CONSTRAINT radnik_eml_UK UNIQUE (Email) 
);


CREATE TABLE sesija (
    Idses integer NOT NULL,
    Dat date NOT NULL,
    Tra integer NOT NULL,
    Idkli integer NOT NULL,
    Brsto integer NOT NULL,
    Brsobe INTEGER NOT NULL,
    Idigr INTEGER NOT NULL,
    CONSTRAINT sesija_PK PRIMARY KEY (Idses),
    CONSTRAINT sesija_CH CHECK (Tra>=0),
    CONSTRAINT sesija_kor_FK FOREIGN KEY (Idkli) REFERENCES klijent (Idkli),
    CONSTRAINT sesija_sto_FK FOREIGN KEY (Brsto, Brsobe, Idigr) REFERENCES sto (Brsto, Brsobe, Idigr)
);


CREATE TABLE cenovnik (
    Idcen integer NOT NULL,
    Poc date NOT NULL,
    Kra date,
    CONSTRAINT cenovnik_PK PRIMARY KEY (Idcen),
    CONSTRAINT cenovnik_CH CHECK (Kra is null or Poc<Kra)
);


CREATE TABLE poklonkartica (
    Idpok integer NOT NULL,
    Naz varchar(40) NOT NULL,
    CONSTRAINT pokkartica_PK PRIMARY KEY (Idpok)
);


CREATE TABLE kupuje (
    Idkli integer NOT NULL,
    Idpok integer NOT NULL,
    Idrp integer NOT NULL,
    CONSTRAINT kupuje_PK PRIMARY KEY (Idkli, Idpok, Idrp),
    CONSTRAINT kupuje_kor_FK FOREIGN KEY (Idkli) REFERENCES klijent (Idkli),
    CONSTRAINT kupuje_pok_FK FOREIGN KEY (Idpok) REFERENCES poklonkartica (Idpok),
    CONSTRAINT kupuje_rp_FK FOREIGN KEY (Idrp) REFERENCES realizacijapromene (Idrp)
);


CREATE TABLE kosta (
    Idpok integer NOT NULL,
    Idcen integer NOT NULL,
    Cena decimal(10, 2) NOT NULL,
    CONSTRAINT kosta_PK PRIMARY KEY (Idpok, Idcen),
    CONSTRAINT kosta_CH CHECK (Cena>=0)
);


CREATE TABLE supskripcija (
    Idsup integer NOT NULL,
    Naz varchar(20) NOT NULL,
    CONSTRAINT supskripcija_PK PRIMARY KEY (Idsup)
);


CREATE TABLE supskribuje (
    Idkli integer NOT NULL,
    Idsup integer NOT NULL,
    Dat date NOT NULL,
    CONSTRAINT supskribuje_PK PRIMARY KEY (Idkli, Idsup),
    CONSTRAINT supskribuje_kor_FK FOREIGN KEY (Idkli) REFERENCES klijent (Idkli),
    CONSTRAINT supskribuje_sup_FK FOREIGN KEY (Idsup) REFERENCES supskripcija (Idsup)
);


CREATE TABLE recenzija (
    Idrec integer NOT NULL,
    Ocena integer NOT NULL,
    Tekst varchar(50),
    Pise integer NOT NULL,
    CONSTRAINT recenzija_PK PRIMARY KEY (Idrec),
    CONSTRAINT recenzija_FK FOREIGN KEY (Pise) REFERENCES klijent (Idkli),
    CONSTRAINT recenzija_CH CHECK (Ocena in (1, 2, 3, 4, 5))
);


CREATE TABLE poruka (
    Idpor integer NOT NULL,
    Dat date NOT NULL,
    Tekst varchar(50) NOT NULL,
    Salje integer NOT NULL,
    CONSTRAINT poruka_PK PRIMARY KEY (Idpor),
    CONSTRAINT poruka_FK FOREIGN KEY (Salje) REFERENCES klijent (Idkli)
);


CREATE TABLE primaporuku (
    Idkli integer NOT NULL,
    Idpor integer NOT NULL,
    CONSTRAINT primaporuku_PK PRIMARY KEY (Idkli, Idpor),
    CONSTRAINT primaporuku_kor_FK FOREIGN KEY (Idkli) REFERENCES klijent (Idkli),
    CONSTRAINT primaporuku_por_FK FOREIGN KEY (Idpor) REFERENCES poruka (Idpor)
);


CREATE TABLE notifikacija (
    Idnot integer NOT NULL,
    Dat date NOT NULL,
    Tekst varchar(50) NOT NULL,
    CONSTRAINT notifikacija_PK PRIMARY KEY (Idnot)
);


CREATE TABLE primanotif (
    Idkli integer NOT NULL,
    Idnot integer NOT NULL,
    CONSTRAINT primanotif_PK PRIMARY KEY (Idkli, Idnot),
    CONSTRAINT primanotif_kor_FK FOREIGN KEY (Idkli) REFERENCES klijent (Idkli),
    CONSTRAINT primanotif_not_FK FOREIGN KEY (Idnot) REFERENCES notifikacija (Idnot)
);

COMMIT;
