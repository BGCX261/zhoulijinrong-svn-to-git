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
		MessageBox(NULL, sMessage, L"错误", MB_ICONERROR);
	}

private:
	CDocumentLibraryT<> m_WebSvrRef;	// Web Services引用对象类
    DocumentLibrary::DictionaryEntry *m_dictEntry;   // 键值对
    int                              m_size;
public:
	CString	m_TmpPath;					// 上传文件默认文档地址
	CString m_FleName;					// 上传文件名称
	CString m_FleExt;					// 上传文件扩展名
	bool Init(CString& sWSDLFile);
};
