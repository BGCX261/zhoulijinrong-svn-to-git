#pragma once

/****************************************************************************************************
Soap client helper class for C++
Usage: 
	1. Inherit a class from this base class
	2. Call Init in constructor, like this:
		Init("http://localhost/AuthService/AuthService.asmx?wsdl", "AuthService", "");
	3. Wrap the webservice by writing proxy function for each web method
	4. Call Invoke to execute the function, processing the input params and return value
		be careful, the order of parameters should be reversed in the parameter array while callin Invoke
	5. Then use this class to call webservice, like this:
		AuthServiceClient	service;
		bool ret = service.IsAuthorized(strCode);

*******************************************************************************************************/

//import soapsdk
//modify the path if needed
#import "C:\Program Files\Common Files\MSSoap\Binaries\mssoap30.dll" \
	/*exclude("IStream", "IErrorInfo", "ISequentialStream", "_LARGE_INTEGER", \
	"_ULARGE_INTEGER", "tagSTATSTG", "_FILETIME")*/
using namespace MSSOAPLib30;
int const TIMEOUT_VALUE = (60 * 1000L) * 20;
///////////////////////////////////////////////////////////////////////////////////////
// Base class for calling a webservice using soap
class SoapClientBase
{
protected:
	ISoapClient	*m_pSoapClient;
    //ISoapConnector *m_pSoapConnector;

	char*		m_pError;
	HRESULT		m_hr;

public:
	SoapClientBase(void)
	{
		m_pSoapClient = NULL;
		m_pError = NULL;
		m_hr = 0;
	}

	virtual ~SoapClientBase(void)
	{
		Reset();
	}

	///////////////////////////////////////////////////////////////////
	// Check error message issued by last call(if any)
	char* GetLastError()
	{
		return m_pError;
	}

	////////////////////////////////////////////////////////////////
	// Init SoapClient object
	bool Init(CString& szWSDLFile, CString& szService, CString& szPort)
	{
		Reset();

		//create soapclient object
		m_hr = ::CoCreateInstance(__uuidof(SoapClient30), NULL, CLSCTX_INPROC_SERVER, __uuidof(ISoapClient), (LPVOID *)&m_pSoapClient);
		if (m_pSoapClient==NULL)
		{
			IssueError("Create soap client object fail");
			return false;
		}

		//init soap client
		_variant_t	varWSDL	= szWSDLFile;
		_variant_t	varWSML = "";
		_bstr_t bstrService	= szService;
		_bstr_t bstrPort	= szPort;
		_bstr_t bstrNS		= "";
		m_hr = m_pSoapClient->MSSoapInit2(varWSDL, varWSML, bstrService, bstrPort, bstrNS);
		if (FAILED(m_hr))
		{
			IssueError("Error calling MSSoapInit2");
			return false;
		}

		m_pSoapClient->ConnectorProperty["AuthUser"] = L"spadmin";
		m_pSoapClient->ConnectorProperty["AuthPassword"] = L"0";
		m_pSoapClient->ConnectorProperty["Timeout"] = TIMEOUT_VALUE;

        /*m_hr = ::CoCreateInstance(__uuidof(HttpConnector30), NULL, CLSCTX_INPROC_SERVER, __uuidof(ISoapConnector), (LPVOID *)&m_pSoapConnector);
		if (m_pSoapClient==NULL)
		{
			IssueError("Create soap client object fail");
			return false;
		}
        m_pSoapConnector->Property["TimeOut"] = TIMEOUT_VALUE;
        m_pSoapConnector->Connect();*/
		return true;
	}


protected:
	///////////////////////////////////////////////////////////////////
	// Record an error message issued by this class or inherited
	void IssueError(char* szError)
	{
		if (m_pError)
			delete [] m_pError;

		m_pError = new char[strlen(szError)+1];
		strcpy(m_pError, szError);
	}

	////////////////////////////////////////////////////////////////////
	// Release interface, free memory, clean everything
	void Reset()
	{
		if (m_pSoapClient)
		{
			m_pSoapClient->Release();
			m_pSoapClient = NULL;
		}
		if (m_pError)
		{
			delete [] m_pError;
			m_pError = NULL;
		}
		m_hr = S_OK;
	}

protected:
	/////////////////////////////////////////////////////////////////////////////////////////
	// the following code is copied from ATL code(CComDispatchDriver), and modified

	HRESULT GetIDOfName(LPCOLESTR lpsz, DISPID* pdispid)
	{
		return m_pSoapClient->GetIDsOfNames(IID_NULL, (LPOLESTR*)&lpsz, 1, LOCALE_USER_DEFAULT, pdispid);
	}
	// Invoke a method by DISPID with N parameters
	HRESULT Invoke(DISPID dispid, VARIANT* pvarParams, int nParams, VARIANT* pvarRet = NULL)
	{
		DISPPARAMS dispparams = { pvarParams, NULL, nParams, 0};
		return m_pSoapClient->Invoke(dispid, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &dispparams, pvarRet, NULL, NULL);
	}
	// Invoke a method by name with Nparameters
	HRESULT Invoke(LPCOLESTR lpszName, VARIANT* pvarParams, int nParams, VARIANT* pvarRet = NULL)
	{
		HRESULT hr;
		DISPID dispid;
		hr = GetIDOfName(lpszName, &dispid);
		if (SUCCEEDED(hr))
			hr = Invoke(dispid, pvarParams, nParams, pvarRet);
		return hr;
	}
public:

	int AddVerificationInfo(void)
	{
		MSSOAPLib30::IHeaderHandlerPtr header = NULL;
		MSSOAPLib30::IStreamAttachmentPtr stream = NULL;
		
		m_hr = ::CoCreateInstance(__uuidof(StreamAttachment30), NULL, CLSCTX_INPROC_SERVER, __uuidof(IHeaderHandler), (LPVOID *)&header);
		if (header == NULL)
		{
			IssueError("Create soap client header object fail");
			return false;
		}
		m_pSoapClient->HeaderHandler = header;
		MSSOAPLib30::ISoapSerializerPtr soapSer;
		m_hr = ::CoCreateInstance(__uuidof(SoapSerializer30), NULL, CLSCTX_INPROC_SERVER, __uuidof(ISoapSerializer), (LPVOID *)&soapSer);
		if (soapSer == NULL)
		{
			IssueError("Create soap client soapSer object fail");
			return false;
		}
		header->WriteHeaders(soapSer, NULL);
		
		soapSer->StartHeaderElement("Verification", L"", 1, L"", L"", L"");

		soapSer->StartElement("USER", L"", L"", L"");
		soapSer->WriteString(L"username");
		soapSer->EndElement();

		soapSer->StartElement("PWD", L"", L"", L"");
		soapSer->WriteString(L"password");
		soapSer->EndElement();

		soapSer->EndHeaderElement();

		header->WriteHeaders(soapSer, NULL);
		return 0;
	}
};
