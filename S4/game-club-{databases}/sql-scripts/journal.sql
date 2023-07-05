CREATE TABLE recenzija_JN (
    Dat DATE NOT NULL,
    Ope varchar(3) NOT NULL,
    Idrec integer NOT NULL,
    Ocena integer,
    Tekst varchar(50),
    Pise integer,
    CONSTRAINT poruka_JN_PK PRIMARY KEY (Dat, Ope, Idrec)
);


CREATE OR REPLACE TRIGGER Trg_recenzija_JN_INSUPDDEL
BEFORE INSERT OR UPDATE OF Ocena, Tekst, Pise OR DELETE
ON recenzija
FOR EACH ROW
BEGIN
IF INSERTING THEN
    INSERT INTO recenzija_JN (Dat, Ope, Idrec, Ocena, Tekst, Pise)
    VALUES (SYSDATE, 'INS', :NEW.Idrec, :NEW.Ocena, :NEW.Tekst, :NEW.Pise);
ELSIF UPDATING ('Ocena') THEN
    INSERT INTO recenzija_JN (Dat, Ope, Idrec, Tekst)
    VALUES (SYSDATE, 'UPD', :OLD.Idrec, :OLD.Tekst);
ELSIF UPDATING ('Tekst') THEN
    INSERT INTO recenzija_JN (Dat, Ope, Idrec, Ocena)
    VALUES (SYSDATE, 'UPD', :OLD.Idrec, :OLD.Ocena);
ELSIF UPDATING ('Pise') THEN
    INSERT INTO recenzija_JN (Dat, Ope, Idrec, Pise)
    VALUES (SYSDATE, 'UPD', :OLD.Idrec, :OLD.Pise);
ELSIF DELETING THEN
    INSERT INTO recenzija_JN (Dat, Ope, Idrec, Ocena, Tekst, Pise)
    VALUES (SYSDATE, 'DEL', :OLD.Idrec, :OLD.Ocena, :OLD.Tekst, :OLD.Pise);
END IF;
END Trg_recenzija_JN_INSUPDDEL;
