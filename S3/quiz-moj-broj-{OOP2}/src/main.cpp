#include <iostream>
#include "game/Game.h"

using namespace std;


int main(int, char** argv) {
    Game g;
    try {
        g.readRounds(argv[1]);
        g.setOutPath(argv[2]);

        g.start();
    } catch (const NonExistingFile& e) {
        cout << e.what() << endl;
        cout << "Terminacija programa." << endl;
    } catch (const TooManyTries&) {
        cout << "Previše pogrešnih ulaza." << endl;
        cout << "Terminacija programa." << endl;
    } catch (const ExitSignal&) {
        cout << "Kraj." << endl;
    }
    return 0;
}
