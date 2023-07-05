#include "Solver.h"
#include <unordered_map>


template <typename T>
vector<T> copyWithout(vector<T> vec, const T& val);


/**
 * Započinje generisanje izraza pozivajući rekurziju {generate} za svaki monom,
 * jednobrojni izraz iz dobijenih brojeva.
 *
 * @param round prosleđeni brojevi sa rešenjem
 */
Expression Solver::solve(vector<int> round) {
    count = 0;
    result = round[6];
    smallestErr = INT_MAX;

    round.pop_back();
    vector<int> cp;
    try {
        for (int num : round) {
            cp = copyWithout(round, num);
            generate(Expression(num), cp); // rekurzija
        }
    } catch (SolutionFound &) {} // pronađeno rešenje sa 2 broja
    return best;
}


// Rekurzivna funkcija koja na prosleđenom izrazu prispaja nove monome i binome sa svim
// smislenim operacijama. Simultano rangira date izraze i prekida pretragu u slučaju pronađenog rešenja
void Solver::generate(const Expression& previous, const vector<int>& given) {
    Expression mon1, mon2;
    vector<int> cp1, cp2;

    for (int num1 : given) {
        mon1 = Expression(num1);
        cp1 = copyWithout(given, num1);

        // dodavanje monoma
        for (const Expression& mon : getCombinations(previous, mon1)) {
            generate(mon, cp1);
        }

        for (int num2 : cp1) {
            mon2 = Expression(num2);
            cp2 = copyWithout(cp1, num2);

            // dodavanje binoma
            for (const Expression& bin: getCombinations(mon1, mon2)) {
                for (const Expression& exp: getCombinations(previous, bin)) {
                    generate(exp, cp2);
                }
            }
        }
    }
}

static vector<Expression> comb;

// generise sve kombinacije operacija sa 2 izraza
vector<Expression> Solver::getCombinations(const Expression& left, const Expression& right) {
    int l = left.val, r = right.val;
    comb.clear();

    if (!l || !r) return comb;

    bool leftSmaller = left.exp < right.exp;

    if (leftSmaller) // leksikografsko uređivanje novog izraza
        combine(left, right, '+');
    else combine(right, left, '+');

    if (l > r) // oduzimanje uvek od većeg izraza
        combine(left, right, '-');
    else combine(right, left, '-');


    if ((l == 1) || (r == 1)) return comb; // mnozenje i deljenje nema smisla sa jedinicama

    if (leftSmaller) // leksikografska provera
        combine(left, right, '*');
    else combine(right, left, '*');


    if (l > r && l % r == 0) // imenilac mora da bude manji i ne sme biti ostatka
        combine(left, right, '/');
    else if (r > l && r % l == 0)
        combine(right, left, '/');

    return comb;
}


void Solver::combine(const Expression& left, const Expression& right, char op) {
    string exp = left.exp + " " + right.exp + " " + op;
    Expression e(exp, -1);
    e.used = left.used + right.used;

    switch (op) { // NOLINT(hicpp-multiway-paths-covered)
        case '+': e.val = left.val + right.val; break;
        case '-': e.val = left.val - right.val; break;
        case '*': e.val = left.val * right.val; break;
        case '/': e.val = left.val / right.val;
    }
    checkExpression(e);
    comb.push_back(e);
    ++count;
}


// čuva najbolji izraz i baca izuzetak u slučaju pronalaska rešenja
void Solver::checkExpression(const Expression& exp) {
    int err = abs(result - exp.val);
    if (err < smallestErr || (err == smallestErr && exp.used < best.used)) {
        best = exp;
        if (exp.val == result && exp.used == 2) throw SolutionFound();
        smallestErr = err;
    }

}


template <typename T>
vector<T> copyWithout(vector<T> vec, const T& val) {
    vec.erase(find(vec.begin(), vec.end(), val));
    return vec;
}
