CREATE OR REPLACE VIEW
NAJVAZNIJE_RECENZIJE (idkli, ime, prezime, avg_ocena, potroseno) AS
SELECT k.idkli, ime, prz, round(avg(ocena), 2), sum(iznos)
FROM klijent k 
INNER JOIN recenzija r ON k.idkli = r.pise
INNER JOIN kupuje kup ON k.idkli = kup.idkli
INNER JOIN realizacijapromene rp ON kup.idrp = rp.idrp
WHERE extract(year from rp.datpro) = extract(year from SYSDATE)
GROUP BY k.idkli, ime, prz
HAVING sum(iznos) > 10000
ORDER BY sum(iznos), k.idkli;

COMMIT;


SELECT * FROM NAJVAZNIJE_RECENZIJE;