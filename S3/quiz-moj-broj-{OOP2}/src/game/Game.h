/**
 * Modul koji opisuje igru Moj broj za konzolu sa 2 igrača i
 * algoritamskim generisanjem rešenja.
 *
 * @author Dimitrije Gašić
 * @date    2023/01/08
*/

#pragma once
#include <vector>
#include <string>
#include <unordered_map>
using namespace std;


/**
 * Klasa za opis igrača sa poljima za ime i broj pobeđenih rundi.
 */
struct Player {
    string name;
    int won;

    Player() : won(0) {}
    explicit Player(string  name) : name(std::move(name)), won(0) {}
    void addWin() { ++won; }
};


/**
 * Korisnički definisani izuzeci.
 */
struct NonExistingFile : runtime_error {
    explicit NonExistingFile(const string& str) : runtime_error(str) {}
};
struct TooManyTries : exception {};
struct ExitSignal : exception {};


/**
 * Klasa za opis igre.
 */
class Game {
    unordered_map<int, Player> players; // uvek veličine 2
    vector<vector<int>> rounds;
    string outPath; // za logovanje rezultata igre

    void logResult(int i, const vector<int>& round, const vector<vector<string>>& details);
    void logWinner();
    void getPlayer(int key); // učitavanje detalja iz komandne linije
    void initOutFile();

public:
    Game() = default;

    void start(); // pokretanje igre
    void readRounds(const string& path);
    void setOutPath(const string& path);

    // TODO generisanje rundi sa random brojevima
    // void generateRounds(int roundNumber);
};
