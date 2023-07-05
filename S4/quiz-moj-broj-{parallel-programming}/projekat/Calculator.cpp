#include "Calculator.h"


// returns the priority of given operation
int priority(const string &op) {
	if (op == "+" || op == "-")
		return 1;
	else if (op == "*" || op == "/")
		return 2;
	else
		return -1;
}


bool isDigits(const string& str) {
	return str.find_first_not_of("0123456789") == string::npos;
}


vector<string> splitBySpace(string str) {
	stringstream ss(str);
	string s;
	vector<string> result;

	while (getline(ss, s, ' '))
		if(!s.empty()) result.push_back(s);

	return result;
}


// see Calculator.h
string postfixToInfix(string postfix) {
	stack<string> s;
	vector<string> res = splitBySpace(postfix);
	stack<string> oop;
	string result;

	for (string c : res) {
		if (isDigits(c)) {
			s.push(c);
			oop.push(" ");
		}
		else {
			string opp2 = oop.top(); 
			oop.pop();
			string opp1 = oop.top();
			oop.pop();

			string op2 = s.top();
			s.pop();
			string op1 = s.top();
			s.pop();

			oop.push(c);
			result = "";

			if (opp1 != " " && priority(opp1) < priority(c)) 
				result += "(" + op1 + ")";
			else
				result += op1;

			result += " " + c + " ";

			if (opp2 != " " && (priority(opp2) < priority(c) || (priority(opp2) == priority(c) && (c=="/" || c=="-"))))
				result += "(" + op2 + ")";
			else
				result += op2;
			s.push(result);
		}
	}
	return s.top();
}
