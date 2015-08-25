#ifndef xmlwriter_h
#define xmlwriter_h

//#include<iostream>
#include<vector>
#include <stack>
using namespace std;
typedef stack<LPTSTR> StackStrings;


class xmlwriter{
public:
	xmlwriter(string  sTmp);
	~xmlwriter();
	void CreateChild(LPCTSTR  sTag,LPCTSTR  sValue);
	void Createtag(LPTSTR  sTag);
	void CloseLasttag();
	void CloseAlltags();
	void AddAtributes(LPTSTR  sAttrName, LPTSTR  sAttrvalue);
	void AddComment(LPTSTR  sComment);
private:
	TCHAR sXmlFile[MAX_PATH];
	vector<LPTSTR > vectAttrData;
public:
	FILE *fp;
	int iLevel;
	StackStrings sTagStack;
};

#endif // xmlwriter_h


