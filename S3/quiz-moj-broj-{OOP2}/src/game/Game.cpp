#include "Game.h"
#include "../solver/Solver.h"
#include "../calculator/Calculator.h"
#include "../util/util.h"
#include <fstream>
#include <iostream>

using namespace chrono;


Expression playRound(const string& name, const vector<int>& round);
steady_clock::time_point beginT; // za benchmark performansi traženja najboljeg izraza
steady_clock::time_point endT;
int max_penal = 5; // najviše dozvoljenih uzastopnih loših unosa


/**
 * Pokreće igru prolazeći kroz prethodno učitane runde.
 *
 * @throws ExitSignal ako neki igrač unese 'x' za izraz
 */
void Game::start() {
    Solver s;
    Expression e1, e2, eComp;
    int err1, err2, errComp;
    int result, current;

    cout << "=================== MOJ BROJ ===================" << endl;

    initOutFile(); // formatira izlazni fajl
    getPlayer(1);
    getPlayer(2);

    int i = 1;
    for (vector<int> round : rounds) {
        if (i % 2) current = 1;
        else current = -1;

        result = round[6];

        e1 = playRound(players[current].name, round); // igra prvi igrač
        err1 = abs(result - e1.val);

        if (err1) {
            // ako prvi nije našao tačno rešenje drugi igrač dobija šansu
            current *= -1;
            e2 = playRound(players[current].name, round);
            err2 = abs(result - e2.val);

            if (err2 < err1) {
                swapObjects(e1, e2);
                swapObjects(err1, err2);
            } else
                current *= -1;
        }
        // u ovom momentu current predstavlja igrača koji je pobednik runde

        cout << "Pretraga rešenja..." << endl;
        beginT = steady_clock::now();
        eComp = s.solve(round);
        endT = steady_clock::now();

        long long int el = duration_cast<milliseconds>(endT - beginT).count();
        cout << "Proteklo vreme: " << (double) el / 1000 << " sekundi" << endl;

        // ispis rezultata u konzolu i u izlazni fajl
        errComp = abs(result - eComp.val);
        logResult(i++, round, {
            {players[current].name + " (pobednik)", toPostfix(e1.exp), to_string(e1.val), to_string(err1)},
            {players[-1*current].name, toPostfix(e2.exp), to_string(e2.val), to_string(err2)},
            {"Računar", eComp.exp, to_string(eComp.val), to_string(errComp)}
        });
        players[current].addWin();
    }
    logWinner(); // upis pobednika u fajl
}

// vraća trenutno vreme
string getTime() {
    time_t now = time(nullptr);
    char buff[32];
    strftime(buff, 32, "%d/%m/%Y %H:%M", localtime(&now));
    return buff;
}

/**
 * Formatira izlaznu datoteku i zapisuje trenutno vreme
 *
 * @throws NonExistingFile ako datoteka ne može da se otvori
 */
void Game::initOutFile() {
    ofstream out(outPath, ios_base::out);

    if (out.is_open()) {
        out << "REZULTATI IGRE (" << getTime() << ")" << endl;
        out.flush();
        out.close();
    } else
        throw NonExistingFile("izlazni fajl sa putanjom \"" + outPath + "\" ne postoji");
}


/**
 * Kreira igrača podacima dobijenim iz konzole.
 *
 * @param key redni broj igrača
 * @throws TooManyTries ako se previše puta unese ime koje nije validno
 */
void Game::getPlayer(int key) {
    string n1;

    int penal = 0;
    while (true) {
        if (penal == max_penal) throw TooManyTries();
        cout << "Igrač " << to_string(key) + ": ";
        getline(cin, n1);
        if (!n1.empty()) break;
        cout << "Ime nije validno." << endl;
        ++penal;
    }
    key = key==1 ? 1 : -1;
    players[key] = Player(n1);
}


void printRound(const string& name, vector<int> round);

/**
 * Dobavlja izraz iz konzole prethodno ispisujući podatke o rundi.
 *
 * @param name ime trenutnog igrača
 * @param round trenutna runda
 * @throws TooManyTries ako se previše puta unese loš izraz
 */
Expression playRound(const string& name, const vector<int>& round) {
    string exp;
    cout << endl << "-----------------------------------------------" << endl;
    printRound(name, round);

    int penal = 0;
    while (true) {
        if (penal == max_penal) throw TooManyTries();
        cout << endl << "Unesite izraz: ";
        getline(cin, exp);
        if (exp == "x") throw ExitSignal();

        try {
            int val = calculateWithGiven<int>(exp, round);
            return {exp, val};
        } catch (InvalidExpression& e) {
            cout << e.what();
            ++penal;
        }
    }
}


/**
 * Učitava runde iz ulazne datoteke.
 *
 * @param path putanja do datoteke
 * @throws NonExistingFile ako ne može da se otvori
 */
void Game::readRounds(const string& path) {
    ifstream infile(path, ios_base::in);
    vector<int> tokens;
    string line, num;

    if (infile.is_open()) {
        while (getline(infile, line)) {
            if (line.empty()) continue; // preskače prazne linije

            num = "";
            tokens.clear();
            for (char c : line) {
                if (isspace(c) and !num.empty()) {
                    tokens.push_back(stoi(num));
                    num = "";
                } else num += c;
            }
            if (!num.empty()) // pročitan novi broj
                tokens.push_back((stoi(num)));
            sort(tokens.begin(), tokens.end()); // sortiranje brojeva runde

            rounds.push_back(tokens);
        }
        infile.close();
    }
    else
        throw NonExistingFile("ulazni fajl sa putanjom \"" + path + "\" ne postoji");
}


/**
 * Postavlja putanju izlazne datoteke, validira je i briše njen sadržaj.
 * Preporučljivo pre pokretanja igre.
 *
 * @param path putanja do datoteke
 * @throws NonExistingFile ako ne može da se otvori
 */
void Game::setOutPath(const string& path) {
    ofstream out(path, ios_base::out);
    if (!out.is_open())
        throw NonExistingFile("izlazni fajl sa putanjom \"" + path + "\" ne postoji");
    else {
        out.flush();
        out.close();
    }
    outPath = path;
}


// formatira podatke o igraču (ili računaru)
string formatStats(vector<string> stats) {
    return stats[0] + ": " + toInfix(stats[1]) + " = " + stats[2] + ", greška: " + stats[3];
}

/**
 * Appenduje rezultate runde u izlaznu datoteku i ispisuje ih u konzoli.
 *
 *
 * @param i broj runde
 * @param round odigrana runda
 * @param details detalji o subjektima koji su učestvovali
 * @throws NonExistingFile ako izlazna datoteka ne može da se otvori
 */
void Game::logResult(int i, const vector<int>& round, const vector<vector<string>>& details) {
    ofstream fileout(outPath, ios_base::app);
    string formatted;
    string banner = "\n=================== RUNDA " + to_string(i) + " ===================\n";

    if (fileout.is_open()) {
        fileout << banner;
        cout << banner;

        printRound("", round); // ispis runde u konzolu
        for (const vector<string>& stats : details) {
            if (stats[1].empty()) continue; // ako je izraz prazan preskače se
            formatted = formatStats(stats);
            fileout << formatted << endl;
            cout << formatted << endl;
        }
        fileout.close();
    } else
        throw NonExistingFile("izlazni fajl sa putanjom \"" + outPath + "\" ne postoji");
}


/**
 * Određuje pobednika i upisuje ga na kraj izlazne datoteke.
 *
 * @throws NonExistingFile ako izlazna datoteka ne može da se otvori
 */
void Game::logWinner() {
    ofstream fileout(outPath, ios_base::app);
    Player winner;
    string out;
    string banner = "\n===============================================\n";

    if (fileout.is_open()) {
        fileout << banner;
        cout << banner;

        // određivanje pobednika ako ga ima
        if (players[1].won == players[-1].won)
            out = "Nerešeno";
        else {
            if (players[1].won > players[-1].won)
                winner = players[1];
            else winner = players[-1];
            out = "Pobednik je " + winner.name + " sa " + to_string(winner.won) + " pobeda.";
        }

        fileout << out << endl;
        cout << out << endl;
        fileout.close();
    } else
        throw NonExistingFile("izlazni fajl sa putanjom \"" + outPath + "\" ne postoji");
}


/**
 * Ispisuje rundu u konzolu.
 *
 * @param name ime igrača koji je na potezu (prazno ako je kraj runde)
 * @param round runda koja se igra
 */
void printRound(const string& name, vector<int> round) {
    int result = round[6];
    round.pop_back();

    if (!name.empty())
        cout << "   Igrač na redu: " << name << endl;
    cout << "Ponuđeni brojevi: ";
    for (int num : round) cout << num << " ";
    cout << endl << "\t\t  Tražen: " << result << endl;
}
