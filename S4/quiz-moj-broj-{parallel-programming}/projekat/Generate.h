#pragma once

#include <tuple>
#include <atomic>
#include <tbb/task_group.h>
#include <tbb/parallel_for.h>
#include <tbb/blocked_range.h>

#define CUTOFF 4

using namespace std;
using namespace tbb;


/**
* Structure for modeling expressions including helper methods.
* Size field is used for ranking expressions by length and exp field is postfix string representation.
* 
* Methods include overriden assignment operator and methods for basic arithmetic operations
* wich all return new Expression object.
*/
struct Expression {
	int value, size;
	string exp;

	Expression() : value(INT_MAX), size(INT_MAX), exp("") {}
	Expression(int i) : value(i), size(1), exp(to_string(i)) {}
	Expression(int value, int size, string exp) : value(value), size(size), exp(exp) {}

	Expression& operator=(Expression other);
	Expression Add(int i) const;
	Expression Multiply(int i) const;
	Expression Subtract(int i, bool) const;
	Expression Divide(int i, bool) const;
};

/**
* @brief generates all possible expressions for given numbers and returns
*	the one closest to solution
* @param vector round with the last element representing solution
* @return tuple with the best expression and number of generated expressions
*/
tuple<Expression, int> generateSerial(vector<int> round);

/**
* @brief generates all possible expressions for given numbers and returns
*	the one closest to solution; parallel implementation which uses tbb library
* @param vector round with the last element representing solution
* @return tuple with the best expression and number of generated expressions
*/
tuple<Expression, int> generateParallel(vector<int> round);
