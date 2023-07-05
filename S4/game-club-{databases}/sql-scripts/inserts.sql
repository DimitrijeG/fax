-- KLIJENTI

INSERT INTO klijent VALUES (10,  'milan20',  'Milan',    'Tesic', 'milan@gmail.com', 'milan',     to_date('01-08-2001', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (20,  'dovla',    'Vladimir', 'Vladimirovic', 'vlada@gmail.com', 'vladimir', to_date('14-03-1998', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (30,  'maki',     'Marko',    'Markovic', 'marko@gmail.com', 'marko',  to_date('23-10-2002', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (40,  'jelena02', 'Jelena',   'Jelic', 'joca@gmail.com', 'jelena',     to_date('09-12-2002', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (50,  'vaske',    'Valisije', 'Vasic', 'vaske@gmail.com', 'vasilije',  to_date('14-07-2000', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (60,  'petar',    'Petar',    'Petrovic', 'petar@gmail.com', 'petar',  to_date('02-10-2002', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (70,  'comi',     'Milica',   'Milic', 'milica@gmail.com', 'milica',   to_date('09-08-1999', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (80,  'maric',    'Nikola',   'Nikolic', 'maric@gmail.com', 'nikola',  to_date('01-02-2001', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (90,  'anjaaa',   'Anja',     'Anjic', 'anja@gmail.com', 'anja',       to_date('07-03-2001', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (100, 'jovan99',  'Jovan',    'Jovanic', 'jovan@gmail.com', 'jovan',   to_date('01-05-2001', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (110, 'bora',     'Aleksa',   'Aleksic', 'bora@gmail.com', 'aleksa',   to_date('06-08-1998', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (120, 'miv',      'Milan',    'Milanovic', 'miv@gmail.com', 'milan',   to_date('24-02-2002', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (130, 'viki',     'Viktor',   'Dadic', 'viki@gmail.com', 'viktor',     to_date('11-09-2004', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (140, 'mihajlo',  'Mihajlo',  'Mihajlovic', 'mihajlo@gmail.com', 'mihajlo', to_date('24-05-2001', 'DD-MM-YYYY'));
INSERT INTO klijent VALUES (150, 'lidija',   'Lidija',   'Lidic', 'lidija@gmail.com', 'lidija',   to_date('09-10-2000', 'DD-MM-YYYY'));


-- POKLON KARTICE

INSERT INTO poklonkartica VALUES (10,  'Diamond supskripcija (1 godina)');
INSERT INTO poklonkartica VALUES (20,  'Diamond supskripcija (1 mesec)');
INSERT INTO poklonkartica VALUES (30,  'Gold supskripcija (1 godina)');
INSERT INTO poklonkartica VALUES (40,  'Gold supskripcija (1 mesec)');
INSERT INTO poklonkartica VALUES (50,  'Silver supskripcija (1 godina)');
INSERT INTO poklonkartica VALUES (60,  'Silver supskripcija (1 mesec)');
INSERT INTO poklonkartica VALUES (70,  'Paket minuta (200 sati)');
INSERT INTO poklonkartica VALUES (80,  'Paket minuta (50 sati)');
INSERT INTO poklonkartica VALUES (90, 'Paket minuta (10 sati)');
INSERT INTO poklonkartica VALUES (100, 'Paket minuta (1 sat)');
INSERT INTO poklonkartica VALUES (110,  'Ucesce u turniru');


-- CENOVNICI

INSERT INTO cenovnik VALUES (10, to_date('01-01-2019', 'DD-MM-YYYY'), to_date('31-12-2019', 'DD-MM-YYYY'));
INSERT INTO cenovnik VALUES (20, to_date('01-01-2020', 'DD-MM-YYYY'), to_date('31-12-2020', 'DD-MM-YYYY'));
INSERT INTO cenovnik VALUES (30, to_date('01-01-2021', 'DD-MM-YYYY'), to_date('31-12-2021', 'DD-MM-YYYY'));
INSERT INTO cenovnik VALUES (40, to_date('01-01-2022', 'DD-MM-YYYY'), to_date('31-12-2022', 'DD-MM-YYYY'));
INSERT INTO cenovnik VALUES (50, to_date('01-01-2023', 'DD-MM-YYYY'), NULL);


-- KOSTA

INSERT INTO kosta VALUES (10, 30, 20000);
INSERT INTO kosta VALUES (10, 40, 21000);
INSERT INTO kosta VALUES (10, 50, 18000);
INSERT INTO kosta VALUES (20, 30, 5000);
INSERT INTO kosta VALUES (20, 40, 5300);
INSERT INTO kosta VALUES (20, 50, 5300);
INSERT INTO kosta VALUES (30, 30, 10000);
INSERT INTO kosta VALUES (30, 40, 12000);
INSERT INTO kosta VALUES (30, 50, 11500);
INSERT INTO kosta VALUES (40, 30, 2000);
INSERT INTO kosta VALUES (40, 40, 2400);
INSERT INTO kosta VALUES (40, 50, 2200);
INSERT INTO kosta VALUES (50, 30, 8000);
INSERT INTO kosta VALUES (50, 40, 8100);
INSERT INTO kosta VALUES (50, 50, 8000);
INSERT INTO kosta VALUES (60, 30, 1000);
INSERT INTO kosta VALUES (60, 40, 1200);
INSERT INTO kosta VALUES (60, 50, 1200);
INSERT INTO kosta VALUES (70, 10, 6000);
INSERT INTO kosta VALUES (70, 20, 7000);
INSERT INTO kosta VALUES (70, 30, 7500);
INSERT INTO kosta VALUES (70, 40, 7200);
INSERT INTO kosta VALUES (70, 50, 7300);
INSERT INTO kosta VALUES (80, 10, 2000);
INSERT INTO kosta VALUES (80, 20, 2300);
INSERT INTO kosta VALUES (80, 30, 2500);
INSERT INTO kosta VALUES (80, 40, 2000);
INSERT INTO kosta VALUES (80, 50, 2000);
INSERT INTO kosta VALUES (90, 10, 350);
INSERT INTO kosta VALUES (90, 20, 400);
INSERT INTO kosta VALUES (90, 30, 400);
INSERT INTO kosta VALUES (90, 40, 420);
INSERT INTO kosta VALUES (90, 50, 400);
INSERT INTO kosta VALUES (100, 10, 50);
INSERT INTO kosta VALUES (100, 20, 80);
INSERT INTO kosta VALUES (100, 30, 100);
INSERT INTO kosta VALUES (100, 40, 100);
INSERT INTO kosta VALUES (100, 50, 80);
INSERT INTO kosta VALUES (110, 10, 500);
INSERT INTO kosta VALUES (110, 20, 500);
INSERT INTO kosta VALUES (110, 30, 600);
INSERT INTO kosta VALUES (110, 40, 550);
INSERT INTO kosta VALUES (110, 50, 500);


-- PROMENE

INSERT INTO vrstapromene VALUES (10, 'dobitak', 'dobitak');

INSERT INTO realizacijapromene VALUES (10, to_date('01-05-2019', 'DD-MM-YYYY'), 2000, '', 10);
INSERT INTO realizacijapromene VALUES (20, to_date('01-05-2022', 'DD-MM-YYYY'), 100, '', 10);
INSERT INTO realizacijapromene VALUES (30, to_date('01-05-2023', 'DD-MM-YYYY'), 2200, '', 10);
INSERT INTO realizacijapromene VALUES (40, to_date('01-05-2019', 'DD-MM-YYYY'), 500, '', 10);
INSERT INTO realizacijapromene VALUES (50, to_date('01-05-2023', 'DD-MM-YYYY'), 80, '', 10);
INSERT INTO realizacijapromene VALUES (60, to_date('01-05-2020', 'DD-MM-YYYY'), 80, '', 10);
INSERT INTO realizacijapromene VALUES (70, to_date('01-05-2021', 'DD-MM-YYYY'), 7500, '', 10);
INSERT INTO realizacijapromene VALUES (80, to_date('01-05-2023', 'DD-MM-YYYY'), 11500, '', 10);
INSERT INTO realizacijapromene VALUES (90, to_date('01-05-2019', 'DD-MM-YYYY'), 500, '', 10);
INSERT INTO realizacijapromene VALUES (100, to_date('01-05-2023', 'DD-MM-YYYY'), 500, '', 10);
INSERT INTO realizacijapromene VALUES (110, to_date('01-05-2021', 'DD-MM-YYYY'), 2500, '', 10);
INSERT INTO realizacijapromene VALUES (120, to_date('01-05-2022', 'DD-MM-YYYY'), 420, '', 10);
INSERT INTO realizacijapromene VALUES (130, to_date('01-05-2019', 'DD-MM-YYYY'), 500, '', 10);
INSERT INTO realizacijapromene VALUES (140, to_date('01-05-2023', 'DD-MM-YYYY'), 500, '', 10);
INSERT INTO realizacijapromene VALUES (150, to_date('01-05-2023', 'DD-MM-YYYY'), 8000, '', 10);
INSERT INTO realizacijapromene VALUES (160, to_date('01-05-2020', 'DD-MM-YYYY'), 500, '', 10);
INSERT INTO realizacijapromene VALUES (170, to_date('01-05-2021', 'DD-MM-YYYY'), 100, '', 10);
INSERT INTO realizacijapromene VALUES (180, to_date('01-05-2019', 'DD-MM-YYYY'), 350, '', 10);
INSERT INTO realizacijapromene VALUES (190, to_date('01-05-2022', 'DD-MM-YYYY'), 420, '', 10);
INSERT INTO realizacijapromene VALUES (200, to_date('01-05-2021', 'DD-MM-YYYY'), 10000, '', 10);
INSERT INTO realizacijapromene VALUES (210, to_date('01-05-2022', 'DD-MM-YYYY'), 550, '', 10);
INSERT INTO realizacijapromene VALUES (220, to_date('01-05-2023', 'DD-MM-YYYY'), 11500, '', 10);
INSERT INTO realizacijapromene VALUES (230, to_date('01-05-2022', 'DD-MM-YYYY'), 2400, '', 10);
INSERT INTO realizacijapromene VALUES (240, to_date('01-05-2019', 'DD-MM-YYYY'), 6000, '', 10);
INSERT INTO realizacijapromene VALUES (250, to_date('01-05-2022', 'DD-MM-YYYY'), 2000, '', 10);


-- KUPUJE

INSERT INTO kupuje VALUES (10, 80, 10);
INSERT INTO kupuje VALUES (10, 100, 20);
INSERT INTO kupuje VALUES (10, 110, 210);
INSERT INTO kupuje VALUES (20, 40, 30);
INSERT INTO kupuje VALUES (20, 110, 40);
INSERT INTO kupuje VALUES (30, 30, 220);
INSERT INTO kupuje VALUES (40, 100, 50);
INSERT INTO kupuje VALUES (40, 100, 60);
INSERT INTO kupuje VALUES (40, 70, 70);
INSERT INTO kupuje VALUES (40, 30, 80);
INSERT INTO kupuje VALUES (40, 110, 90);
INSERT INTO kupuje VALUES (50, 40, 230);
INSERT INTO kupuje VALUES (50, 110, 100);
INSERT INTO kupuje VALUES (60, 80, 110);
INSERT INTO kupuje VALUES (70, 90, 120);
INSERT INTO kupuje VALUES (70, 110, 130);
INSERT INTO kupuje VALUES (70, 110, 140);
INSERT INTO kupuje VALUES (90, 70, 240);
INSERT INTO kupuje VALUES (90, 50, 150);
INSERT INTO kupuje VALUES (90, 110, 160);
INSERT INTO kupuje VALUES (100, 100, 170);
INSERT INTO kupuje VALUES (110, 90, 180);
INSERT INTO kupuje VALUES (110, 80, 250);
INSERT INTO kupuje VALUES (120, 90, 190);
INSERT INTO kupuje VALUES (140, 30, 200);


-- RECENZIJE

INSERT INTO recenzija VALUES (10, 5, '', 10);
INSERT INTO recenzija VALUES (20, 4, '', 10);
INSERT INTO recenzija VALUES (30, 3, '', 20);
INSERT INTO recenzija VALUES (40, 4, '', 20);
INSERT INTO recenzija VALUES (50, 5, '', 30);
INSERT INTO recenzija VALUES (60, 1, '', 40);
INSERT INTO recenzija VALUES (70, 2, '', 40);
INSERT INTO recenzija VALUES (80, 3, '', 50);
INSERT INTO recenzija VALUES (90, 1, '', 50);
INSERT INTO recenzija VALUES (100, 4, '', 60);
INSERT INTO recenzija VALUES (110, 1, '', 60);
INSERT INTO recenzija VALUES (120, 2, '', 60);
INSERT INTO recenzija VALUES (130, 3, '', 70);
INSERT INTO recenzija VALUES (140, 4, '', 70);
INSERT INTO recenzija VALUES (150, 1, '', 90);
INSERT INTO recenzija VALUES (160, 2, '', 90);
INSERT INTO recenzija VALUES (170, 5, '', 90);
INSERT INTO recenzija VALUES (180, 3, '', 110);
INSERT INTO recenzija VALUES (190, 2, '', 120);
INSERT INTO recenzija VALUES (200, 4, '', 120);
INSERT INTO recenzija VALUES (210, 1, '', 130);
INSERT INTO recenzija VALUES (220, 4, '', 140);
INSERT INTO recenzija VALUES (230, 2, '', 140);
INSERT INTO recenzija VALUES (240, 2, '', 140);
INSERT INTO recenzija VALUES (250, 3, '', 140);

COMMIT;
