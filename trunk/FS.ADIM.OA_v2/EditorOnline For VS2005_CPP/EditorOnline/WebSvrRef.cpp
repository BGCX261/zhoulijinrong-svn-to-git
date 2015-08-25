#include "StdAfx.h"
#include "WebSvrRef.h"
#include "DocOutLst.h"

CWebSvrRef::CWebSvrRef(void):SoapClientBase()
{
	//NetworkCredential^ myCred = gcnew NetworkCredential(
	//SecurelyStoredUserName,SecurelyStoredPassword,SecurelyStoredDomain );

	//CredentialCache^ myCache = gcnew CredentialCache;

	//myCache->Add( gcnew Uri( "www.contoso.com" ), "Basic", myCred );
	//myCache->Add( gcnew Uri( "app.contoso.com" ), "Basic", myCred );

	//WebRequest^ wr = WebRequest::Create( "www.contoso.com" );
	//wr->Credentials = myCache;
}

CWebSvrRef::~CWebSvrRef(void)
{
}

int CWebSvrRef::TestWebSvrRefAddr(CString& WebSvrRefAddr)
{
	m_WebSvrRef.SetUrl(WebSvrRefAddr);
    int iRet = -1;
    m_WebSvrRef.Test(&iRet);
    return iRet;
}

//BOOL CWebSvrRef::CheckWebSvrRefAddress(CString& WebSvrRefAddr)
//{
//	int iRet = -1;
//	BSTR sMessage = ::SysAllocString(L"");
//	BSTR sRefAddr = WebSvrRefAddr.AllocSysString();
//	try{
//		m_WebSvrRef.SetUrl(sRefAddr);
//		//TODO: 验证WEBSERVICES是否正确
//		m_WebSvrRef.CheckWebServicesRef(sRefAddr, &iRet, &sMessage);
//		if(iRet == -1) {
//			throw -1;
//		}
//		if(!iRet) {
//			throw -2;
//		}
//	}
//	catch(...)
//	{
//		::SysFreeString(sMessage);
//		::SysFreeString(sRefAddr);
//		return FALSE;
//	}
//	::SysFreeString(sMessage);
//	::SysFreeString(sRefAddr);
//	return TRUE;
//}

//BOOL CWebSvrRef::CheckOut(CString& sURL_FleName, ATLSOAP_BLOB& SoapBlob)
//{
//	int iRet = -1;
//	BSTR sMessage = ::SysAllocString(L"");
//	BSTR sURLFleNme = sURL_FleName.AllocSysString();
//	/*SoapBlob.data = pbyte;
//	SoapBlob.size = fleSize;*/
//	try
//	{
//        m_WebSvrRef.CheckOut(&sURLFleNme, SoapBlob.size, &iRet);
//		if(iRet == -1) {
//			throw -1;
//		}
//		if(iRet) {
//			m_WebSvrRef.Download(&sURLFleNme, SoapBlob.size, &SoapBlob);
//			if(!iRet) {
//				throw -2;
//			}
//		}
//		else {
//			Alert(sMessage);
//		}
//	}
//	catch(...)
//	{
//		::SysFreeString(sMessage);
//		::SysFreeString(sURLFleNme);
//		return FALSE;
//	}
//	//TODO:返回后需要在本地创建文件，并打开
//	::SysFreeString(sMessage);
//	::SysFreeString(sURLFleNme);
//	return TRUE;
//}

//BOOL CWebSvrRef::CheckIn(CString& sURL_FleName, ATLSOAP_BLOB& SoapBlob)
//{
//	int iRet = -1;
//	BSTR *sMessage = (BSTR*)::SysAllocString(L"");
//	BSTR sURLFleNme = sURL_FleName.AllocSysString();
//	try
//	{
//		m_WebSvrRef.Upload(&sURLFleNme, SoapBlob.size, SoapBlob, m_dictEntry, m_size, 1, &sMessage, &iRet);
//	if(iRet == -1) {
//		throw -1;
//	}
//	if(!iRet) {
//		throw -2;
//	}
//
//	m_WebSvrRef.CheckIn(&sURLFleNme, SoapBlob.size, &iRet);
//	if(!iRet) {
//		throw -3;
//	}
//	}
//	catch(...)
//	{
//		::SysFreeString(*sMessage);
//		::SysFreeString(sURLFleNme);
//		return FALSE;
//	}
//	::SysFreeString(*sMessage);
//	::SysFreeString(sURLFleNme);
//	return TRUE;
//}

//BOOL CWebSvrRef::Delete(CString& sURL_FleName)
//{
//	int iRet = -1;
//	BSTR sMessage = ::SysAllocString(L"");
//	BSTR sURLFleNme = sURL_FleName.AllocSysString();
//	try
//	{
//		m_WebSvrRef.Remove(&sURLFleNme, 0, &iRet);
//		if(iRet == -1) {
//			throw -1;
//		}
//		if(!iRet)
//		{
//			throw -2;
//		}
//	}
//	catch(...)
//	{
//		::SysFreeString(sMessage);
//		::SysFreeString(sURLFleNme);
//		return FALSE;
//	}
//	::SysFreeString(sMessage);
//	::SysFreeString(sURLFleNme);
//	return TRUE;
//}



BOOL CWebSvrRef::Download(CString& sMossSiteURL, CString& sProcessType, CString& FolderName, CString& FileName,
                          ATLSOAP_BLOB& SoapBlob)
{
    int iUploadRet = -1;
	//BSTR *sMessage = (BSTR*)::SysAllocString(L"");

    BSTR FileInfo[4];
    FileInfo[0] = sMossSiteURL.AllocSysString();
    FileInfo[1] = sProcessType.AllocSysString();
    FileInfo[2] = FolderName.AllocSysString();
    FileInfo[3] = FileName.AllocSysString();

    HRESULT HRet = m_WebSvrRef.Download(FileInfo, 4, &SoapBlob);
    if(HRet != S_OK)
    {
        ::SysFreeString(FileInfo[0]);
        ::SysFreeString(FileInfo[1]);
        ::SysFreeString(FileInfo[2]);
        ::SysFreeString(FileInfo[3]);
        return FALSE;
    }

    ::SysFreeString(FileInfo[0]);
    ::SysFreeString(FileInfo[1]);
    ::SysFreeString(FileInfo[2]);
    ::SysFreeString(FileInfo[3]);
    return TRUE;
}

BOOL CWebSvrRef::Upload(CString& sMossSiteURL, CString& sProcessType, CString& FolderName, CString& FileName,
                        ATLSOAP_BLOB& SoapBlob)
{
	int iUploadRet = -1;
	BSTR *sMessage = (BSTR*)::SysAllocString(L"");
	//BSTR sURLFleNme = sURL_FleName.AllocSysString();
    BSTR FileInfo[4];
    FileInfo[0] = sMossSiteURL.AllocSysString();
    FileInfo[1] = sProcessType.AllocSysString();
    FileInfo[2] = FolderName.AllocSysString();
    FileInfo[3] = FileName.AllocSysString();
    HRESULT HRet;
    try
	{
        HRet = m_WebSvrRef.Upload(FileInfo, 4, SoapBlob, NULL, 0, 1, &sMessage, &iUploadRet);
		if(HRet == S_OK) {		// 上传成功
			//m_WebSvrRef.CheckIn(sURLFleNme, &iUploadRet, &sMessage);	
			//if(!iUploadRet) {	// 签入失败
			//	throw -1;								
			//}
		}
		else {					// 上传失败
            //HRet = m_WebSvrRef.Upload(FileInfo, 4, SoapBlob, NULL, 0, 1, &sMessage, &iUploadRet);
		    throw -2;
		}
	}
    catch(...)
    {
        HRet = m_WebSvrRef.Upload(FileInfo, 4, SoapBlob, NULL, 0, 1, &sMessage, &iUploadRet);
        if(HRet != S_OK)
        {
            ::SysFreeString(*sMessage);
            ::SysFreeString(FileInfo[0]);
            ::SysFreeString(FileInfo[1]);
            ::SysFreeString(FileInfo[2]);
            ::SysFreeString(FileInfo[3]);
            return FALSE;
        }

        ::SysFreeString(*sMessage);
        ::SysFreeString(FileInfo[0]);
        ::SysFreeString(FileInfo[1]);
        ::SysFreeString(FileInfo[2]);
        ::SysFreeString(FileInfo[3]);
        return TRUE;
    }

    ::SysFreeString(*sMessage);
    ::SysFreeString(FileInfo[0]);
    ::SysFreeString(FileInfo[1]);
    ::SysFreeString(FileInfo[2]);
    ::SysFreeString(FileInfo[3]);
    return TRUE;
}

//BOOL CWebSvrRef::UnCheckOut(CString& sURL_FleName)
//{
//	int iRet = -1;
//	BSTR sMessage = ::SysAllocString(L"");
//	BSTR sURLFleNme = sURL_FleName.AllocSysString();
//	try
//	{
//		m_WebSvrRef.UndoCheckOut(&sURLFleNme, 0, &iRet);
//		if(iRet == -1) {
//			throw -1;
//		}
//
//		if(!iRet) {				
//			throw -2;	//存在［撤销签出］操作失败
//		}
//	}
//	catch(...)
//	{
//		::SysFreeString(sMessage);
//		::SysFreeString(sURLFleNme);
//		return FALSE;
//	}
//	::SysFreeString(sMessage);
//	::SysFreeString(sURLFleNme);
//	return TRUE;
//}

//BOOL CWebSvrRef::UpdataMeta(BSTR sURL_FleName, BSTR value, int iSubItem)
//{
//	int iRetVal = -1;
//	BSTR sMessage = L"";
//	switch(iSubItem)
//	{
//	case 0:
//		//m_WebSvrRef.UpdateMeta(sURL_FleName, L"Alias", value, &iRetVal, &sMessage);
//		break;
//	case 1:
//		//m_WebSvrRef.UpdateMeta(sURL_FleName, L"Encode", value, &iRetVal, &sMessage);
//		break;
//	case 2:
//		//m_WebSvrRef.UpdateMeta(sURL_FleName, L"Size", value, &iRetVal, &sMessage);
//		break;
//	default:
//		break;
//	}
//	if(iRetVal == -1)
//	{
//		return FALSE;
//	}
//	if(iRetVal)
//		return TRUE;
//	else
//		return FALSE;
//}

bool CWebSvrRef::Init(CString& sWSDLFile)
{
	// 将CString转换为char*
	//char* pchWSDLFile = sWSDLFile.GetBuffer(sWSDLFile.GetLength());
	CString sService(L"");
	CString sPort(L"");
	return SoapClientBase::Init(sWSDLFile, sService, sPort);
	//return SoapClientBase::Init("http://172.29.129.210/service/documentlibrary.asmx?wsdl", "", "");
	//return SoapClientBase::Init("http://172.29.129.210/service/documentlibrary.asmx", "http://172.29.129.210", "80");
}
