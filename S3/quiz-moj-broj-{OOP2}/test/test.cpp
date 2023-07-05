#include "../src/solver/Solver.h"
#include "../src/calculator/Calculator.h"
#include <iostream>
#include <vector>


struct AssertionError : logic_error {
    explicit AssertionError(const string& str) : logic_error(str) {}
};

void testCalculator() {
    vector<tuple<int, string, vector<int>>> toPass = {
            {-1, "1-2", {1, 2}},
            {7, "1+2*3", {1, 2, 3}},
            {4, "2*6/3", {2, 6, 3}},
            {16, "(6+2)*2", {6, 2, 2}},
    };
    vector<tuple<string, vector<int>>> toFail = {
            {"5", {5}},
            {"5+2", {5}},
            {"5+5+2", {5, 2}},
            {"5/2", {5, 2}},
            {"5/0", {5, 2}}
    };

    for (auto a : toPass)
        assert(get<0>(a) == calculateWithGiven(get<1>(a), get<2>(a)));
    for (auto a : toFail) { // treba da baci InvalidExpression()
        try {
            calculateWithGiven(get<0>(a), get<1>(a));

            throw AssertionError("Assertion error while testing bad input.");
        } catch (const InvalidExpression&) {}
        catch (const exception& e) { // neki drugi izuzetak
            throw AssertionError("Unexpected exception while asserting: " + string(e.what()));
        }
    }
}


void testSolver() {
    Solver s;

    vector<tuple<int, string, vector<int>>> toPass = {
            {999, "10 * (2 + 3) * 20 - 1", {1, 2, 3, 4, 10, 20, 999}},
            {871, "10 * (15 * 6 - 2) - 9", {2, 4, 6, 9, 10, 15}},
            {980, "10 * 100 - (10 + 10)", {10, 10, 10, 10, 10, 100}},
            {723, "2 * (2 + 50) * 7 - 5", {2, 2, 3, 5, 7, 50}}
    };
    for (auto a : toPass) {
        Expression e = s.solve(get<2>(a));
        assert(e.val == get<0>(a));
        assert(e.exp == get<1>(a));
    }
}

int main() {
    cout << "Testing calculator... ";
    testCalculator();
    cout << "Testing solver.. ";
    testSolver();

    return 0;
}