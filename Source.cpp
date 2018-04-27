#include <iostream>
#include<iomanip>
#include <string>
#define MAX_DIGIT 50
#define Init_Number 99
using namespace std;
int main()
{
	string a;

	int digitArrary[MAX_DIGIT];

	for (int i = 0; i < MAX_DIGIT; i++)
	{
		digitArrary[i] = Init_Number;
	}

	do {

		cout << "“ü—Í‚µ‚Ä‚­‚¾‚³‚¢";

		cin >> a;

		for (int i = 0; i < a.size(); i++)
		{
			digitArrary[i] = a[i]-'0';
		}

		for (int i = 0; i < MAX_DIGIT; i++)
		{
			if (digitArrary[i] != Init_Number)
			{
				cout << digitArrary[i];
			}
		}

		//if (a.size() <= 9)
		//{
		//	digitArrary[0] = stod(a);
		//}
		//else if (a.size() >= 10 && a.size() <= 18)
		//{
		//	for (int i = 0; i < 9; i++)
		//	{

		//	}
		//}

	} while (a != "0");


}