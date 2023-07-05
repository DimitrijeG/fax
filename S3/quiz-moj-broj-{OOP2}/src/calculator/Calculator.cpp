#include "Calculator.h"
#include "../util/util.h"


/**
 * Lookup prioriteta za dobijenu operaciju
 *
 * @throws InvalidExpression u slučaju da ne postoji
 */
int priority(char op) {
    switch (op) {
        case '+':
        case '-':
            return 1;
        case '*':
        case '/':
            return 2;
        default:
            throw InvalidExpression("nepostojeća operacija " + string(1, op));
    }
}


/**
 * Konverzija infix -> postfix
 *
 * @throws InvalidExpression u slučaju lošeg izraza ili nepostojeće operacije
 */
string toPostfix(const string& infix) {
    Stack<char> s;
    string num;
    string postfix;

    for (char c : infix) {
        if (isblank(c)) continue;
        if (c >= '0' && c <= '9') {
            num += c;
            continue;
        }

        // trenutni karakter nije broj
        if (!num.empty()) {
            postfix += " " + num;
            num = "";
        }

        if (c == '(') s.push(c);
        else if (c == ')') {
            while (s.top() != '(')
                postfix += " " + string(1, s.pop());
            s.pop();
        }
        else {
            while (!s.empty() && s.top() != '(' && s.top() != ')' && priority(c) <= priority(s.top()))
                postfix += " " + string(1, s.pop());
            s.push(c);
        }
    }

    if (!num.empty()) // ukoliko poslednji karakter pripada broju on još nije upisan u postfix
        postfix += " " + num;
    while(!s.empty()) // preostale operacije iz steka
        postfix += " " + string(1, s.pop());

    postfix.erase(0, 1); // razmak sa pocetka
    return postfix;
}


bool after(const string& a, const string& b);

/**
 * Konverzija postfix -> infix; postavlja zagrade samo gde je neophodno.
 * Sortira sve izraze prema veličini brojeva (koliko operacije dopuštaju).
 *
 * @throws InvalidExpression u slučaju nepostojeće operacije
 */
string toInfix(const string& postfix) {
    Stack<string> s;
    Stack<char> op;
    string num;

    for (char c : postfix) {
        if ( c >= '0' &&  c <= '9') {
            num += c;
            continue;
        }
        if (!num.empty()) {
            s.push(num);
            op.push(' ');
            num = "";
        }
        if (isblank(c)) continue;

        if (s.size() < 2)
            throw InvalidExpression();

        string b = s.pop();
        string a = s.pop();
        char opb = op.pop(); // operacije levog i desnog operanda (ako su kompozitni)
        char opa = op.pop();
        string res;

        if ((c == '+' || c == '*') && after(a, b)) { // leksikografsko uredjenje
            swapObjects(a, b);
            swapObjects(opa, opb);
        }

        if (!isblank(opa) && priority(opa) < priority(c))
            res = "(" + a + ")";
        else res = a;

        res += " " + string(1, c) + " ";

        if (!isblank(opb) &&
                ((priority(opb) < priority(c)) ||
                ((c == '/' || c == '-') && priority(c) == priority(opb))))
                res += "(" + b + ")";
        else res += b;

        // dobijeni izraz se vraća u stek
        s.push(res);
        op.push(c);
    }
    return s.top();
}


/**
 * Pronalazi sledeći broj u stringu i vraća poziciju nakon njega.
 * Ako nema sledećeg broja vraća -1.
 *
 * @param str za pretragu
 * @param offset od kog se pretražuje
 * @param num bafer za sledeći broj
 * @returns pozicija iza pronađenog broja
 */
int nextNumber(const string& str, int offset, string& num) {
    num = "";
    if (offset >= str.size()) return -1;
    for (int k = offset; k < str.size(); ++k) {
        if (str[k] < '0' || str[k] > '9') return k+1;
        num += str[k];
    }
    return -1; // kraj stringa
}


/**
 * Provera da li je prvi izraz numerički gledano posle drugog
 * tako što redom poredi brojeve iz prosleđenih izraza.
 */
bool after(const string& a, const string& b) {
    int i=0, j=0;

    string num1, num2;
    int n1, n2;

    while(true) {
        i = nextNumber(a, i, num1);
        j = nextNumber(b, j, num2);

        if (num1.empty() || num2.empty()) return false;
        n1 = stoi(num1);
        n2 = stoi(num2);

        if (n1 > n2) return true;
        else if (n2 > n1) return false;
        if (i == -1 || j == -1) return false;
    }
}
