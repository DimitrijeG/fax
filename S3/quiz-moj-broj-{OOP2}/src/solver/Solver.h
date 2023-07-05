/**
 * Modul algoritma koji pronalazi najbolji izraz za dat set brojeva.
 * Izrazi se generišu u postfix notaciji pomoću rekurzije.
 *
 * @author Dimitrije Gašić
 * @date    2023/01/09
*/

#pragma once
#include <string>
#include <vector>
using namespace std;


/**
 * Klasa za opis izraza. Čuva string izraz, njegovu vrednost i broj
 * monoma (iskorišćenih brojeva) od kojih je napravljen.
 */
struct Expression {
    string exp;
    int val; // vrednost izraza
    int used; // ukupno iskorišćenih cifara

    Expression() : val(INT_MAX), used(0) {}
    explicit Expression(int val) : exp(to_string(val)), val(val), used(1) {}
    Expression(string exp, int val) : exp(std::move(exp)), val(val), used(1) {}
};


/**
 * Klasa za pronalaženje najboljeg izraza.
 */
class Solver {
    void generate(const Expression& previous, const vector<int>& given);
    vector<Expression> getCombinations(const Expression&, const Expression&);
    void combine(const Expression&, const Expression&, char op);
    void checkExpression(const Expression&);

    class SolutionFound : exception {}; // baca se ako je pronađen binom koji daje rešenje

    Expression best; // najbolji izgenerisani izraz do sad
    int result, smallestErr; // traženi rezultat i greška trenutno najboljeg izraza
    long count; // broj generisanih izraza

public:
    Solver() : result(-1), smallestErr(INT_MAX), count(-1) {};
    Expression solve(vector<int> round);
//    long getCount() const { return count; }
};
