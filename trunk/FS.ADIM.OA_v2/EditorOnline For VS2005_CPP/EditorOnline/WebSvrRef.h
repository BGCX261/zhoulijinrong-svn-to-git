#pragma once
#include "SoapClientBase.h"
using namespace DocumentLibrary;

class CWebSvrRef : public SoapClientBase
{
public:
	CWebSvrRef(void);
	~CWebSvrRef(void);

public:
	int TestWebSvrRefAddr(CString& WebSvrRefAddr);
	//BOOL CheckWebSvrRefAddress(CString& WebSvrRefAddr);
	//BOOL CheckOut(CString& sURL_FleName, ATLSOAP_BLOB& SoapBlob);
	//BOOL CheckIn(CString& sURL_FleName, ATLSOAP_BLOB& SoapBlob);
	//BOOL Delete(CString& sURL_FleName);
	BOOL CWebSvrRef::Upload(CString& sMossSiteURL, CString& sProcessType, CString& FolderName, CString& FileName,
                            ATLSOAP_BLOB& SoapBlob);
    BOOL Download(CString& sMossSiteURL, CString& sProcessType, CString& FolderName, CString& FileName,
                          ATLSOAP_BLOB& SoapBlob);
	//BOOL UnCheckOut(CString& sURL_FleName);
	//BOOL UpdataMeta(BSTR sURL_FleName, BSTR value, int iSubItem);
private:
	void Alert(BSTR sMessage)
	{
		MessageBox(NULL, sMessage, L"����", MB_ICONERROR);
	}

private:
	CDocumentLibraryT<> m_WebSvrRef;	// Web Services���ö�����
    DocumentLibrary::DictionaryEntry *m_dictEntry;   // ��ֵ��
    int                              m_size;
public:
	CString	m_TmpPath;					// �ϴ��ļ�Ĭ���ĵ���ַ
	CString m_FleName;					// �ϴ��ļ�����
	CString m_FleExt;					// �ϴ��ļ���չ��
	bool Init(CString& sWSDLFile);
};
