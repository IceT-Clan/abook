#include <iostream>
#include <fstream>
#include <string>
#include <stdlib.h>
#include <locale>
#include <codecvt>

using namespace std;


bool Read(ifstream& i, wstring& pers, bool end = true) {
	static std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter;
	char buffer[4096];
	i.getline(buffer, 4069);
	if ((i.rdstate() & std::ifstream::eofbit) != 0)
		return false;

	pers += converter.from_bytes(buffer);
	if(end) pers += L";";
	return true;
}

int main() {
	bool do_output = false;
	cout << "Show Output (y/N): ";
	string in;
	cin >> in;
	if (in == "y" || in == "Y") {
		do_output = true;
	}
	cout << "Limit: ";
	cin >> in;
	int limit = -1;
	try {
		limit = stoi(in);
	}
	catch (std::exception e) {
		limit = -1;
	}

	bool error = false;

	ifstream vornamen("../firstname.txt");
	if (!vornamen.is_open()) {
		cerr << "Error vornamen open" << endl;
		error = true;
	}

	ifstream nachnamen("../lastname.txt");
	if (!nachnamen.is_open()) {
		cerr << "Error nachnamen open" << endl;
		error = true;
	}

	ifstream straßen("../streets.txt");
	if (!straßen.is_open()) {
		cerr << "Error nachnamen open" << endl;
		error = true;
	}

	wofstream output("../adressbuch.csv", ios::out | ios::trunc);
	if (!output.is_open()) {
		cerr << "Error output open" << endl;
		error = true;
	}
	
	if (!error) {
		std::locale mylocale("");
		output.imbue(mylocale);

		wstring person;
		int count;
		for(count = 0; count < limit || limit == -1; count++) {
			person.clear();

			if (!Read(vornamen, person)) break;

			if (!Read(nachnamen, person)) break;

			person += to_wstring(rand() % 28 + 1) + L".";
			person += to_wstring(rand() % 12 + 1) + L".";
			person += to_wstring(rand() % 70 + 1940) + L";"; //Geburtstag

			person += to_wstring(rand() % 89999 + 10000) + L";";//PLZ

			if (!Read(straßen, person)) break;

			person += to_wstring(rand() % 50 + 1) + L";"; //hausnummer

			person += to_wstring(rand() % 89999 + 10000) + to_wstring(rand() % 89999 + 10000) + L";"; //telefon
			person += to_wstring(rand() % 89999 + 10000) + to_wstring(rand() % 89999 + 10000) + L";"; //mobil telefon

			output << person << endl;
			static std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter;
			if (do_output) cout << converter.to_bytes(person) << endl;
		}
		cout << "loaded " << count << " persons" << endl;

		vornamen.close();
		nachnamen.close();
		straßen.close();
		output.close();
		cout << "File created!" << endl;
	}
	else {
		cerr << "Programm exited" << endl;
	}
#ifdef _DEBUG
	system("PAUSE");
#else
	while (true);
#endif
	return 0;
}