// EditorOnlineCtrl.cpp : CEditorOnlineCtrl ActiveX �ؼ����ʵ�֡�

#include "stdafx.h"
#include "EditorOnline.h"
#include "EditorOnlineCtrl.h"
#include "EditorOnlinePropPage.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CEditorOnlineCtrl, COleControl)



// ��Ϣӳ��

BEGIN_MESSAGE_MAP(CEditorOnlineCtrl, COleControl)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()



// ����ӳ��

BEGIN_DISPATCH_MAP(CEditorOnlineCtrl, COleControl)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "ShowAxWnd", dispidShowAxWnd, ShowAxWnd, VT_UI4, VTS_NONE)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "SetDocLibLst", dispidSetDocLibLst, SetDocLibLst, VT_BSTR, VTS_BSTR)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "GetDocLibLst", dispidGetDocLibLst, GetDocLibLst, VT_BSTR, VTS_NONE)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "SetConfigInfo", dispidSetConfigInfo, SetConfigInfo, VT_BSTR, VTS_BSTR)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "GetConfigInfo", dispidGetConfigInfo, GetConfigInfo, VT_BSTR, VTS_NONE)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "SetEnableEditCol", dispidSetEnableEditCol, SetEnableEditCol, VT_EMPTY, VTS_UI4 VTS_BOOL)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "AutoShowAxWnd", dispidAutoShowAxWnd, AutoShowAxWnd, VT_EMPTY, VTS_BSTR)
END_DISPATCH_MAP()



// �¼�ӳ��

BEGIN_EVENT_MAP(CEditorOnlineCtrl, COleControl)
END_EVENT_MAP()



// ����ҳ

// TODO: ����Ҫ��Ӹ�������ҳ�����ס���Ӽ���!
BEGIN_PROPPAGEIDS(CEditorOnlineCtrl, 1)
	PROPPAGEID(CEditorOnlinePropPage::guid)
END_PROPPAGEIDS(CEditorOnlineCtrl)



// ��ʼ���๤���� guid

IMPLEMENT_OLECREATE_EX(CEditorOnlineCtrl, "EDITORONLINE.EditorOnlineCtrl.1",
	0xb810bf98, 0x8736, 0x4824, 0xb6, 0x74, 0x52, 0x1a, 0x7e, 0x73, 0x69, 0xb1)



// ����� ID �Ͱ汾

IMPLEMENT_OLETYPELIB(CEditorOnlineCtrl, _tlid, _wVerMajor, _wVerMinor)



// �ӿ� ID

const IID BASED_CODE IID_DEditorOnline =
		{ 0x38B04295, 0x9189, 0x4AF9, { 0xA8, 0x7A, 0x27, 0x4E, 0x5F, 0xC7, 0x98, 0x80 } };
const IID BASED_CODE IID_DEditorOnlineEvents =
		{ 0xBBDAA0F9, 0x8277, 0x4D3B, { 0xB5, 0x3D, 0x67, 0x3D, 0x82, 0xF8, 0x70, 0x42 } };



// �ؼ�������Ϣ

static const DWORD BASED_CODE _dwEditorOnlineOleMisc =
	OLEMISC_ACTIVATEWHENVISIBLE |
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(CEditorOnlineCtrl, IDS_EDITORONLINE, _dwEditorOnlineOleMisc)



/////////////////////////////////////////////////////////////////////////////
// Interface map for IObjectSafety

BEGIN_INTERFACE_MAP( CEditorOnlineCtrl, COleControl )
    INTERFACE_PART(CEditorOnlineCtrl, IID_IObjectSafety, ObjSafe)
END_INTERFACE_MAP()

/////////////////////////////////////////////////////////////////////////////
// IObjectSafety member functions

// Delegate AddRef, Release, QueryInterface

ULONG FAR EXPORT CEditorOnlineCtrl::XObjSafe::AddRef()
{
    METHOD_PROLOGUE(CEditorOnlineCtrl, ObjSafe)
        return pThis->ExternalAddRef();
}

ULONG FAR EXPORT CEditorOnlineCtrl::XObjSafe::Release()
{
    METHOD_PROLOGUE(CEditorOnlineCtrl, ObjSafe)
        return pThis->ExternalRelease();
}

HRESULT FAR EXPORT CEditorOnlineCtrl::XObjSafe::QueryInterface(
    REFIID iid, void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CEditorOnlineCtrl, ObjSafe)
        return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

const DWORD dwSupportedBits =
INTERFACESAFE_FOR_UNTRUSTED_CALLER |
INTERFACESAFE_FOR_UNTRUSTED_DATA;
const DWORD dwNotSupportedBits = ~ dwSupportedBits;

/////////////////////////////////////////////////////////////////////////////
// CStopLiteCtrl::XObjSafe::GetInterfaceSafetyOptions
// Allows container to query what interfaces are safe for what. We're
// optimizing significantly by ignoring which interface the caller is
// asking for.
HRESULT STDMETHODCALLTYPE
CEditorOnlineCtrl::XObjSafe::GetInterfaceSafetyOptions(
    REFIID riid,
    DWORD __RPC_FAR *pdwSupportedOptions,
    DWORD __RPC_FAR *pdwEnabledOptions)
{
    METHOD_PROLOGUE(CEditorOnlineCtrl, ObjSafe)

        HRESULT retval = ResultFromScode(S_OK);

    // does interface exist?
    IUnknown FAR* punkInterface;
    retval = pThis->ExternalQueryInterface(&riid,
        (void * *)&punkInterface);
    if (retval != E_NOINTERFACE) { // interface exists
        punkInterface->Release(); // release it--just checking!
    }

    // we support both kinds of safety and have always both set,
    // regardless of interface
    *pdwSupportedOptions = *pdwEnabledOptions = dwSupportedBits;

    return retval; // E_NOINTERFACE if QI failed
}

/////////////////////////////////////////////////////////////////////////////
// CStopLiteCtrl::XObjSafe::SetInterfaceSafetyOptions
// Since we're always safe, this is a no-brainer--but we do check to make
// sure the interface requested exists and that the options we're asked to
// set exist and are set on (we don't support unsafe mode).
HRESULT STDMETHODCALLTYPE
CEditorOnlineCtrl::XObjSafe::SetInterfaceSafetyOptions(
    REFIID riid,
    DWORD dwOptionSetMask,
    DWORD dwEnabledOptions)
{
    METHOD_PROLOGUE(CEditorOnlineCtrl, ObjSafe)

        // does interface exist?
        IUnknown FAR* punkInterface;
    pThis->ExternalQueryInterface(&riid, (void * *)&punkInterface);
    if (punkInterface) { // interface exists
        punkInterface->Release(); // release it--just checking!
    }
    else { // interface doesn't exist
        return ResultFromScode(E_NOINTERFACE);
    }

    // can't set bits we don't support
    if (dwOptionSetMask & dwNotSupportedBits) {
        return ResultFromScode(E_FAIL);
    }

    // can't set bits we do support to zero
    dwEnabledOptions &= dwSupportedBits;
    // (we already know there are no extra bits in mask )
    if ((dwOptionSetMask & dwEnabledOptions) !=
        dwOptionSetMask) {
            return ResultFromScode(E_FAIL);
    }       

    // don't need to change anything since we're always safe
    return ResultFromScode(S_OK);
}
/////////////////////////////////////////////////////////////////////////////
// CEditorOnlineCtrl::CEditorOnlineCtrlFactory::UpdateRegistry -
// Adds or removes system registry entries for CEditorOnlineCtrl

BOOL CEditorOnlineCtrl::CEditorOnlineCtrlFactory::UpdateRegistry(BOOL bRegister)
{
    // TODO: Verify that your control follows apartment-model threading rules.
    // Refer to MFC TechNote 64 for more information.
    // If your control does not conform to the apartment-model rules, then
    // you must modify the code below, changing the 6th parameter from
    // afxRegApartmentThreading to 0.

    if (bRegister)
        return AfxOleRegisterControlClass(
        AfxGetInstanceHandle(),
        m_clsid,
        m_lpszProgID,
        IDS_EDITORONLINE, //����Ĺ�����Ϊ��д
        IDB_EDITORONLINE, //����Ĺ�����Ϊ��д
        afxRegApartmentThreading,
        _dwEditorOnlineOleMisc,
        _tlid,
        _wVerMajor,
        _wVerMinor);
    else
        return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}


// CEditorOnlineCtrl::CEditorOnlineCtrl - ���캯��

CEditorOnlineCtrl::CEditorOnlineCtrl()
{
	InitializeIIDs(&IID_DEditorOnline, &IID_DEditorOnlineEvents);
	// TODO: �ڴ˳�ʼ���ؼ���ʵ�����ݡ�
}



// CEditorOnlineCtrl::~CEditorOnlineCtrl - ��������

CEditorOnlineCtrl::~CEditorOnlineCtrl()
{
	// TODO: �ڴ�����ؼ���ʵ�����ݡ�
}



// CEditorOnlineCtrl::OnDraw - ��ͼ����

void CEditorOnlineCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	if (!pdc)
		return;

	// TODO: �����Լ��Ļ�ͼ�����滻����Ĵ��롣
	pdc->FillRect(rcBounds, CBrush::FromHandle((HBRUSH)GetStockObject(WHITE_BRUSH)));
	pdc->Ellipse(rcBounds);

	if (!IsOptimizedDraw())
	{
		// ������֧���Ż���ͼ��

		// TODO: ������κ� GDI ����ѡ�뵽�豸������ *pdc �У�
		//		���ڴ˴��ָ���ǰѡ���Ķ���
	}
}



// CEditorOnlineCtrl::DoPropExchange - �־���֧��

void CEditorOnlineCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: Ϊÿ���־õ��Զ������Ե��� PX_ ������
}



// CEditorOnlineCtrl::GetControlFlags -
// �Զ��� MFC �� ActiveX �ؼ�ʵ�ֵı�־��
//
DWORD CEditorOnlineCtrl::GetControlFlags()
{
	DWORD dwFlags = COleControl::GetControlFlags();


	// �ڻ�Ͳ��״̬֮�����ת��ʱ��
	// �������»��ƿؼ���
	dwFlags |= noFlickerActivate;

	// �ؼ�ͨ������ԭ�豸�������е�
	// ԭʼ GDI ���󣬿����Ż����� OnDraw ������
	dwFlags |= canOptimizeDraw;
	return dwFlags;
}



// CEditorOnlineCtrl::OnResetState - ���ؼ�����ΪĬ��״̬

void CEditorOnlineCtrl::OnResetState()
{
	COleControl::OnResetState();  // ���� DoPropExchange ���ҵ���Ĭ��ֵ

	// TODO: �ڴ��������������ؼ�״̬��
}

void DeleteAllFiles(CString& folderPath)
{
    WIN32_FIND_DATA info;
    wchar_t cstr[128];

    HANDLE hp; 
    wsprintf(cstr, L"%s\\*.*", folderPath);
    hp = FindFirstFile(cstr, &info);
    do
    {
        wsprintf(cstr,L"%s\\%s", folderPath, info.cFileName);
        DeleteFile(cstr);

    }while(FindNextFile(hp, &info)); 
    FindClose(hp);
}

// CEditorOnlineCtrl ��Ϣ�������

ULONG CEditorOnlineCtrl::ShowAxWnd(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    // TODO: �ڴ���ӵ��ȴ���������
    m_DocMgr.m_ConfigInfo = this->m_CfgInfo;
	m_DocMgr.m_DocLst = this->m_DocLibList;
	if(!m_DocMgr.InitConfigInfo())
		return 0;
    m_DocMgr.m_sAutoItem = "";
	INT_PTR ipt = m_DocMgr.DoModal();
	
	/*if(IDOK == ipt)
	{*/
		this->m_CfgInfo = m_DocMgr.m_ConfigInfo;
		this->m_DocLibList = m_DocMgr.m_DocLst;
	//}
    //::MessageBox(NULL, L"test", L"", MB_OK);
	// TODO:�����ʱĿ¼
	//CString DosCmd(L"cmd.exe /c del /s /q ");
	//DosCmd.Append(m_DocMgr.m_TmpPath);

	////char* pch = DosCmd.GetBuffer(DosCmd.GetLength());

	//char cstr[128];
	//memset(cstr, 0, 128);
	//WideCharToMultiByte(CP_OEMCP, 0, DosCmd, -1, cstr, DosCmd.GetLength(), NULL, NULL);

	////system(cstr);
	//WinExec(cstr, SW_HIDE);

    DeleteAllFiles(m_DocMgr.m_TmpPath);

	return 0;
}

BSTR CEditorOnlineCtrl::SetDocLibLst(LPCTSTR sDocLst)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: �ڴ���ӵ��ȴ���������

    m_DocLibList = sDocLst;
	return m_DocLibList.AllocSysString();
}

BSTR CEditorOnlineCtrl::GetDocLibLst(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: �ڴ���ӵ��ȴ���������

    return m_DocLibList.AllocSysString();
}

BSTR CEditorOnlineCtrl::SetConfigInfo(LPCTSTR CfgInfo)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: �ڴ���ӵ��ȴ���������
    m_CfgInfo = CfgInfo;
	return m_CfgInfo.AllocSysString();
}

BSTR CEditorOnlineCtrl::GetConfigInfo(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: �ڴ���ӵ��ȴ���������

    return m_CfgInfo.AllocSysString();
}

void CEditorOnlineCtrl::SetEnableEditCol(ULONG iItem, VARIANT_BOOL Enable)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    // TODO: �ڴ���ӵ��ȴ���������
    switch(iItem)
    {
    case 0:
        m_DocMgr.m_List.m_bColALIASEdit = Enable;
        break;
    case 1:
        m_DocMgr.m_List.m_bColENCODEEdit = Enable;
        break;
    case 2:
        m_DocMgr.m_List.m_bColPAGESEdit = Enable;
        break;
    }
}

void CEditorOnlineCtrl::AutoShowAxWnd(LPCTSTR sKey)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    // TODO: �ڴ���ӵ��ȴ���������
    m_DocMgr.m_ConfigInfo = this->m_CfgInfo;
	m_DocMgr.m_DocLst = this->m_DocLibList;
	if(!m_DocMgr.InitConfigInfo())
		return;
    m_DocMgr.m_sAutoItem = sKey;

	INT_PTR ipt = m_DocMgr.DoModal();
	
	this->m_CfgInfo = m_DocMgr.m_ConfigInfo;
	this->m_DocLibList = m_DocMgr.m_DocLst;

    DeleteAllFiles(m_DocMgr.m_TmpPath);

    return;
    
}
