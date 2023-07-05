#include "Generate.h"

atomic<int> c;

Expression serialRecursion(const vector<int>& factors, const Expression& current, int solution);
Expression parallelRecursion(const vector<int>& factors, const Expression& current, int solution);


/**
* @brief helper function for replacing the best expression with new one if it is better
* @param new expression, current best and solution to be found
*/
void checkSolution(const Expression& result, Expression& best, const int solution) {
	int resultDiff = abs(solution - result.value);
	int bestDiff = abs(solution - best.value);

	if (resultDiff < bestDiff || (resultDiff == bestDiff && result.size < best.size))
		best = result;
}


/**
* @brief removes given number from vector
* @param current vector
* @param number to be removed
*/
void removeNumber(vector<int>& factors, int i) {
	vector<int>::iterator position;
	position = find(factors.begin(), factors.end(), i);
	if (position != factors.end())
		factors.erase(position);
}


// see Generate.h
tuple<Expression, int> generateSerial(vector<int> round) {
	int solution = round[round.size() - 1];
	Expression best, result;
	c = 0;

	round.pop_back();
	vector<int> temp;

	for (int i : round) {
		temp = round;
		removeNumber(temp, i);

		result = serialRecursion(temp, Expression(i), solution);
		checkSolution(result, best, solution);
	}
	return { best, c };
}


/**
* @brief funds the best expression for given numbers and solution to be found using recursion
* @param given numbers
* @param previous expression
* @param solution to be found
* @return the best expression
*/
Expression serialRecursion(const vector<int>& factors, const Expression& current, int solution) {
	Expression newExp, result, best = current;
	vector<int> temp;

	for (int i : factors) {
		temp = factors;
		removeNumber(temp, i);

		if (i == 0 || current.value == 0) continue;

		result = serialRecursion(temp, current.Add(i), solution);
		checkSolution(result, best, solution);

		if (i != 1 && current.value != 1) {
			result = serialRecursion(temp, current.Multiply(i), solution);
			checkSolution(result, best, solution);
		}

		if (current.value - i > 0) {
			result = serialRecursion(temp, current.Subtract(i, false), solution);
			checkSolution(result, best, solution);
		}
		if (i - current.value > 0) {
			result = serialRecursion(temp, current.Subtract(i, true), solution);
			checkSolution(result, best, solution);
		}

		if (i != 1 && current.value % i == 0) {
			result = serialRecursion(temp, current.Divide(i, false), solution);
			checkSolution(result, best, solution);
		}
		if (current.value != 1 && i % current.value == 0) {
			result = serialRecursion(temp, current.Divide(i, true), solution);
			checkSolution(result, best, solution);
		}
	}
	return best;
}


/**
* Structure for parallel iteration through given vector and collecting expressions in output array.
* Expressions are calculated using parallelRecursion function.
*/
struct BestExpression {
	const vector<int>& input;
	Expression* output;
	const int solution;

	void operator()(const blocked_range<int>& range) const {
		vector<int> temp;

		for (int i = range.begin(); i != range.end(); ++i) {
			temp = input;
			removeNumber(temp, input[i]);

			output[i] = parallelRecursion(temp, Expression(input[i]), solution);
		}
	}
	BestExpression(const vector<int>& input, Expression* output, int solution)
		: input(input), output(output), solution(solution) {}
};


// see Generate.h
tuple<Expression, int> generateParallel(vector<int> round) {
	int factorsCount = round.size() - 1;
	int solution = round[factorsCount];
	round.pop_back();
	c = 0;

	Expression* results = new Expression[factorsCount];
	Expression best;

	BestExpression bexp(round, results, solution);
	parallel_for(blocked_range<int>(0, factorsCount), bexp);

	for (int i = 0; i < factorsCount; ++i)
		checkSolution(results[i], best, solution);

	return { best, c };
}


// parallel implementation of serialRecursion which uses task_group from tbb
Expression parallelRecursion(const vector<int>& factors, const Expression& current, int solution) {
	Expression best = current;
	Expression* res = new Expression[6];
	vector<int> temp;

	auto f = factors.size() <= CUTOFF ? serialRecursion : parallelRecursion;

	task_group g;
	for (int v : factors) {
		temp = factors;
		removeNumber(temp, v);

		if (v == 0 || current.value == 0) continue;

		g.run([&]() { res[0] = f(temp, current.Add(v), solution); });
		if (v != 1 && current.value != 1)
			g.run([&]() { res[1] = f(temp, current.Multiply(v), solution); });

		if (current.value - v > 0)
			g.run([&]() { res[2] = f(temp, current.Subtract(v, false), solution); });
		if (v - current.value > 0)
			g.run([&]() { res[3] = f(temp, current.Subtract(v, true), solution); });

		if (v != 1 && current.value % v == 0)
			g.run([&]() { res[4] = f(temp, current.Divide(v, false), solution); });
		if (current.value != 1 && v % current.value == 0)
			g.run([&]() { res[5] = f(temp, current.Divide(v, true), solution); });

		g.wait();

		for (int i = 0; i < 6; ++i)
			checkSolution(res[i], best, solution);
	}

	delete[] res;
	return best;
}


// see Generate.h
Expression& Expression::operator=(Expression other)
{
	value = other.value;
	size = other.size;
	exp = other.exp;
}

Expression Expression::Add(int i) const {
	++c;
	return Expression(value + i, size + 1, exp + " " + to_string(i) + " +");
}

Expression Expression::Multiply(int i) const {
	++c;
	return Expression(value * i, size + 1, exp + " " + to_string(i) + " *");
}

Expression Expression::Subtract(int i, bool reversed) const {
	++c;
	if (!reversed)
		return Expression(value - i, size + 1, exp + " " + to_string(i) + " -");
	return Expression(i - value, size + 1, to_string(i) + " " + exp + " -");
}

Expression Expression::Divide(int i, bool reversed) const {
	++c;
	if (!reversed)
		return Expression(value / i, size + 1, exp + " " + to_string(i) + " /");
	return Expression(i / value, size + 1, to_string(i) + " " + exp + " /");
}