#include <iostream>
#include <fstream>
#include <string>
#include <stdlib.h> 

using namespace std;

bool Read(ifstream& i, string& pers, bool end = true) {
	char buffer[4096];
	i.getline(buffer, 4069);
	if ((i.rdstate() & std::ifstream::eofbit) != 0)
		return false;
	pers += buffer;
	if(end) pers += ";";
}

int main() {
	ifstream vornamen;
	if (vornamen.is_open()) {
		cerr << "Error vornamen open" << endl;
		return 0;
	}

	ifstream nachnamen;
	if (nachnamen.is_open()) {
		cerr << "Error nachnamen open" << endl;
		return 0;
	}

	ifstream straßen;
	if (straßen.is_open()) {
		cerr << "Error nachnamen open" << endl;
		return 0;
	}

	ofstream output;
	if (output.is_open()) {
		cerr << "Error output open" << endl;
		return 0;
	}
	
	string person;

	while (true) {
		person.clear();

		Read(vornamen, person);

		Read(nachnamen, person);

		person += to_string(rand() % 28 + 1) + ".";
		person += to_string(rand() % 12 + 1) + ".";
		person += to_string(rand() % 60 + 1940) + ";"; //Geburtstag

		person += to_string(rand() % 89999 + 10000) + ";";//PLZ

		Read(straßen, person);

		person += to_string(rand() % 50 + 1) + ";"; //hausnummer

		output << person << endl;
	}
	cerr << "loaded all" << endl;

#ifdef _DEBUG
	system("PAUSE");
#endif
	return 0;
}