// xmlwriter.cpp : Definiert den Einstiegspunkt für die Konsolenanwendung.
//
#include "stdafx.h"

#include "xmlwriter.h"
#include <stdarg.h>

xmlwriter::xmlwriter(string sTmp)
{
	//wsprintf(sXmlFile,_T("%s"),sTmp);

    fp = NULL;
    iLevel = 0;
    fp = _wfopen(sXmlFile,_T("w"));
    if(fp == NULL)
    {
// 		std::cout<<"Unable to open output file";
		return;
  	}
	else
	{
		fwprintf(fp,_T("<?xml version=\"1.0\" encoding=\"GB2312\" \?>"));
	}
}

xmlwriter::~xmlwriter()
{
    if(fp != NULL)
        fclose(fp);
    vectAttrData.clear();
}


void xmlwriter::Createtag(LPTSTR  sTag)
{
	fwprintf(fp,_T("\n"));
	//Indent properly
	for(int iTmp =0;iTmp<iLevel;iTmp++)
		fwprintf(fp,_T("\t"));
	fwprintf(fp,_T("<%s"),sTag);
	//Add Attributes
	while(0 < vectAttrData.size()/2)
	{
		LPTSTR sTmp = vectAttrData.back();
		fwprintf(fp,_T(" %s="), sTmp);
		vectAttrData.pop_back();
		sTmp = vectAttrData.back();
		fwprintf(fp,_T("\"%s\""), sTmp);
		vectAttrData.pop_back();
	}
	vectAttrData.clear();
	fwprintf(fp,_T(">"));
	sTagStack.push(sTag);
	iLevel++;

}
void xmlwriter::CloseLasttag()
{
	fwprintf(fp,_T("\n"));
	iLevel--;
    //Indent properly
	for(int iTmp =0;iTmp<iLevel;iTmp++)
		fwprintf(fp,_T("\t"));
	fwprintf(fp,_T("</%s>"),sTagStack.top());
	sTagStack.pop();//pop out the last tag
	return;
}

void xmlwriter::CloseAlltags()
{
	while(sTagStack.size() != 0)
	{
	   fwprintf(fp,_T("\n"));
	   iLevel--;
        //Indent properly
	   for(int iTmp =0;iTmp<iLevel;iTmp++)
	       fwprintf(fp,_T("\t"));
	   fwprintf(fp,_T("</%s>"),sTagStack.top());
	   sTagStack.pop();//pop out the last tag
    }
	return;
}
void xmlwriter::CreateChild(LPCTSTR  sTag,LPCTSTR  sValue)
{
	fwprintf(fp,_T("\n"));
	//Indent properly
	for(int iTmp =0;iTmp<iLevel;iTmp++)
		fwprintf(fp,_T("\t"));
	fwprintf(fp,_T("<%s"),sTag);
	//Add Attributes
	while(0 < vectAttrData.size()/2)
	{
		LPTSTR sTmp = vectAttrData.back();
		fwprintf(fp,_T(" %s="), sTmp);
		vectAttrData.pop_back();
		sTmp = vectAttrData.back();
		fwprintf(fp,_T("\"%s\""), sTmp);
		vectAttrData.pop_back();
	}
	vectAttrData.clear();
	//add value and close tag
	fwprintf(fp,_T(">%s</%s>"),sValue,sTag);
}

void xmlwriter::AddAtributes(LPTSTR  sKey, LPTSTR  sVal)
{
	vectAttrData.push_back(sVal);
	vectAttrData.push_back(sKey);
}


void xmlwriter::AddComment(LPTSTR  sComment)
{
	fwprintf(fp,_T("\n"));
	//Indent properly
	for(int iTmp =0;iTmp<iLevel;iTmp++)
		fwprintf(fp,_T("\t"));
	fwprintf(fp,_T("<!--%s-->"),sComment);
}
