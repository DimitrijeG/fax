#include <iostream>
#include <fstream>

#include "Calculator.h"
#include "Generate.h"

using namespace chrono;


// used for loading input files
vector<string> loadRounds(const string& path) {
	ifstream in(path);
	if (!in) {
		cout << "Nepostojeci fajl." << endl;
		exit(-1);
	}

	string str;
	vector<string> lines;
	while (getline(in, str))
		if (str.size() > 0) lines.push_back(str);
	return lines;
}


// extracting rounds from string lines
vector<int> getGiven(const string& line) { 
	vector<int> givenNumbers; 
	stringstream ss(line);
	string s;

	while (getline(ss, s, ' '))
		givenNumbers.push_back(stoi(s));
	return givenNumbers;
}


void printGivenNumbers(const vector<int> givenNumbers) {
	cout << "Ponudjeni brojevi:";
	for (int i = 0; i < givenNumbers.size() - 1; i++)
		cout << " " << givenNumbers[i];

	cout << "\nTrazeni broj: " << givenNumbers[givenNumbers.size() - 1] << "\n\n";
}


// used for benchmarking execution time
steady_clock::time_point beginT;
steady_clock::time_point endT;
long long int el;


int main(int) {
	vector<string> lines = loadRounds("rounds6.txt");
	vector<int> givenNumbers;
	Expression serial, parallel;
	int c;

	for (const string& line : lines) {
		givenNumbers = getGiven(line);
		printGivenNumbers(givenNumbers);

		
		cout << "Serijska pretraga...\n";
		beginT = steady_clock::now();
		tie(serial, c) = generateSerial(givenNumbers); // serial 
		endT = steady_clock::now();
		el = duration_cast<milliseconds>(endT - beginT).count();
		cout << "Generisano resenja: " << c << "\n\n";
		cout << "Vreme serijske pretrage: " << (double)el / 1000 << " sekundi\n";
		
	
		cout << "Paralelna pretraga...\n";
		beginT = steady_clock::now();
		tie(parallel, c) = generateParallel(givenNumbers); // parallel 
		endT = steady_clock::now();
		el = duration_cast<milliseconds>(endT - beginT).count();
		cout << "Generisano resenja: " << c << endl;
		cout << "Vreme paralelne pretrage: " << (double)el / 1000 << " sekundi\n";

		cout << "\n---------------------------------------------------\n\n";
		cout << "Pronadjeno resenje: " << postfixToInfix(parallel.exp) << " = " << parallel.value << endl;
		cout << "Resenja ista: " << (serial.exp == parallel.exp ? "da" : "ne") << endl;
		cout << "\n===================================================\n\n";
	}
	cin.get();
	return 0;
}