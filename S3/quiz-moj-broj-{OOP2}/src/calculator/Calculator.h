/**
 * Modul za manipulaciju aritmetičkim string izrazima.
 * Dodatno sadrži funkcije za konverziju iz infix u postfix i obrnuto.
 * Podržane operacije su [+, -, *, /].
 * Podržani tipovi su [int, double] iako je {calculate} templejt funkcije
 * i u planu je dodavanje podrške za sve odgovarajuće tipove.
 *
 * @author Dimitrije Gašić
 * @date    2023/01/04
*/

#pragma once
#include "../util/Stack.h"
#include <string>
using namespace std;


struct InvalidExpression : runtime_error {
    explicit InvalidExpression(const string& str) : runtime_error("Greška: " + str) {}
    InvalidExpression() : InvalidExpression("izraz nije validan") {}
};


string toPostfix(const string& infix);
string toInfix(const string& postfix);


/**
 * if-else logika implementirana u vidu šablona
 * bira odgovarajuću strukturu u odnosu na to da li je tip T int ili double
 */
template <bool is_integral, typename T> struct numberUtil;

// za celobrojne tipove
template <typename T> struct numberUtil<true, T> {
    int parse(const string& str) { return stoi(str); }
    /**
     * Proverava da li se dobija ostatak pri deljenju.
     * @throws InvalidExpression ako može da dođe do gubitka informacija
     */
    void validateDivision(T a, T b) {
        if (a % b != 0) throw InvalidExpression("dobijen ostatak pri celobrojnom deljenju");
    }
};

// za double
template <typename T> struct numberUtil<false, T> {
    double parse(const string& str) { return stod(str); }
    void validateDivision(T, T) {}
};


/**
 * Evaluira infiksni izraz za proizvoljan tip brojeva.
 * Trenutno podržani samo int i double. U budućnosti moguće proširiti za
 * proizvoljne tipove uključujući i korisnički definisane.
 *
 * @param infix izraz za računanje
 * @param validate predikat koji validira svaki broj u izraz
 * @returns vrednost izraza
 * @throws InvalidExpression ako broj, izraz ili neka operacija nisu validni
 */
template <typename T, typename F>
T calculate(const string& infix, F validate) {
    numberUtil<is_integral<T>::value, T> numberUtil;
    string postfix = toPostfix(infix);
    Stack<T> s;
    string num;
    T val;

    for (char c : postfix) {
        if ( c >= '0' &&  c <= '9') {
            num += c;
            continue;
        }
        if (!num.empty()) {
            val = numberUtil.parse(num);
            validate(val);
            s.push(val);
            num = "";
        }
        if (isblank(c)) continue;

        // c je operacija
        if (s.size() < 2)
            throw InvalidExpression();

        T b = s.pop();
        T a = s.pop();

        switch (c) {
            case '+': s.push(a + b); break;
            case '-': s.push(a - b); break;
            case '*': s.push(a * b); break;
            case '/':
                if (b == 0) throw InvalidExpression("pokušano deljenje nulom");
                numberUtil.validateDivision(a, b);
                s.push(a / b);
                break;
            default: break;
        }
    }
    if (s.empty()) throw InvalidExpression();
    return s.top();
}


/**
 * Specijalizacija funkcije {calculate} tako da računa samo izraze koji
 * su neka varijacija dozvoljenih brojeva.
 *
 * @param infix izraz za računanje
 * @param given dozvoljeni brojevi
 * @returns vrednost izraza
 * @throws InvalidExpression specifično i ako broj nije u dozvoljenim
 */
template <typename T>
T calculateWithGiven(const string& infix, vector<T> given) {
    return calculate<T>(infix, [given] (const T& val) mutable {
        if (find(given.begin(), given.end(), val) == given.end())
            throw InvalidExpression("unesen broj nije među ponuđenim");
        given.erase(find(given.begin(), given.end(), val));
    });
}
