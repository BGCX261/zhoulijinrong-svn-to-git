#include "StdAfx.h"
#include "DocOutLst.h"

CDocOutLst::CDocOutLst(void)
{
}

CDocOutLst::~CDocOutLst(void)
{
}

bool CDocOutLst::AddDocChkOuted(DocItemEx& item)
{
	if(&item == NULL) return FALSE;
	this->m_Array.Add(item);
	return TRUE;
}

bool CDocOutLst::DelDocChkOuted(DocItemEx& item)
{
	if(&item == NULL) return FALSE;
	try {
		INT_PTR nIndex = FindDocChkOuted(item);
		this->m_Array.RemoveAt(nIndex);
		return TRUE;
	}
	catch(...) {
		return FALSE;
	}
}

int  CDocOutLst::FindDocChkOuted(DocItemEx &item)
{
	if(&item == NULL) return FALSE;
	INT_PTR ilength = GetLength();
	if(ilength == 0) return -1;	//列表为空
	for(int i=0;i<ilength;i++)
	{
		DocItemEx item1 = this->m_Array.GetAt(i);
		if(CompareItem(item1, item))
		{
			return i;
		}
		else
		{
			continue;
		}
	}
	return -1;					//元素未找到，不存在
}
void CDocOutLst::ClearAll()
{
	this->m_Array.RemoveAll();
}

INT_PTR CDocOutLst::GetLength() const
{
	return this->m_Array.GetCount();
}

bool CDocOutLst::CompareItem(DocItemEx &item1, DocItemEx &item2)
{
	if(&item1 == NULL || &item2 == NULL) return FALSE;

	CComBSTR sTitle1(item1.m_Title);
	CComBSTR sTitle2(item2.m_Title);
	
	if(sTitle1 == sTitle2)
	{
		return TRUE;
	}
	return FALSE;
}

DocItemEx CDocOutLst::GetDocItem(int iIndex)
{
	DocItemEx item;
	return item = this->m_Array.GetAt(iIndex);
}

BOOL operator ==(DocItemEx& item1, DocItemEx& item2)
{
	DocItemEx *pItem1 = &item1;
	DocItemEx *pItem2 = &item2;
	if(pItem1 == pItem2)
		return TRUE;
	else
		return FALSE;
}