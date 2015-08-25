// EditorOnlineCtrl.cpp : CEditorOnlineCtrl ActiveX 控件类的实现。

#include "stdafx.h"
#include "EditorOnline.h"
#include "EditorOnlineCtrl.h"
#include "EditorOnlinePropPage.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CEditorOnlineCtrl, COleControl)



// 消息映射

BEGIN_MESSAGE_MAP(CEditorOnlineCtrl, COleControl)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()



// 调度映射

BEGIN_DISPATCH_MAP(CEditorOnlineCtrl, COleControl)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "ShowAxWnd", dispidShowAxWnd, ShowAxWnd, VT_UI4, VTS_NONE)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "SetDocLibLst", dispidSetDocLibLst, SetDocLibLst, VT_BSTR, VTS_BSTR)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "GetDocLibLst", dispidGetDocLibLst, GetDocLibLst, VT_BSTR, VTS_NONE)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "SetConfigInfo", dispidSetConfigInfo, SetConfigInfo, VT_BSTR, VTS_BSTR)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "GetConfigInfo", dispidGetConfigInfo, GetConfigInfo, VT_BSTR, VTS_NONE)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "SetEnableEditCol", dispidSetEnableEditCol, SetEnableEditCol, VT_EMPTY, VTS_UI4 VTS_BOOL)
    DISP_FUNCTION_ID(CEditorOnlineCtrl, "AutoShowAxWnd", dispidAutoShowAxWnd, AutoShowAxWnd, VT_EMPTY, VTS_BSTR)
END_DISPATCH_MAP()



// 事件映射

BEGIN_EVENT_MAP(CEditorOnlineCtrl, COleControl)
END_EVENT_MAP()



// 属性页

// TODO: 按需要添加更多属性页。请记住增加计数!
BEGIN_PROPPAGEIDS(CEditorOnlineCtrl, 1)
	PROPPAGEID(CEditorOnlinePropPage::guid)
END_PROPPAGEIDS(CEditorOnlineCtrl)



// 初始化类工厂和 guid

IMPLEMENT_OLECREATE_EX(CEditorOnlineCtrl, "EDITORONLINE.EditorOnlineCtrl.1",
	0xb810bf98, 0x8736, 0x4824, 0xb6, 0x74, 0x52, 0x1a, 0x7e, 0x73, 0x69, 0xb1)



// 键入库 ID 和版本

IMPLEMENT_OLETYPELIB(CEditorOnlineCtrl, _tlid, _wVerMajor, _wVerMinor)



// 接口 ID

const IID BASED_CODE IID_DEditorOnline =
		{ 0x38B04295, 0x9189, 0x4AF9, { 0xA8, 0x7A, 0x27, 0x4E, 0x5F, 0xC7, 0x98, 0x80 } };
const IID BASED_CODE IID_DEditorOnlineEvents =
		{ 0xBBDAA0F9, 0x8277, 0x4D3B, { 0xB5, 0x3D, 0x67, 0x3D, 0x82, 0xF8, 0x70, 0x42 } };



// 控件类型信息

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
        IDS_EDITORONLINE, //这里的工程名为大写
        IDB_EDITORONLINE, //这里的工程名为大写
        afxRegApartmentThreading,
        _dwEditorOnlineOleMisc,
        _tlid,
        _wVerMajor,
        _wVerMinor);
    else
        return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}


// CEditorOnlineCtrl::CEditorOnlineCtrl - 构造函数

CEditorOnlineCtrl::CEditorOnlineCtrl()
{
	InitializeIIDs(&IID_DEditorOnline, &IID_DEditorOnlineEvents);
	// TODO: 在此初始化控件的实例数据。
}



// CEditorOnlineCtrl::~CEditorOnlineCtrl - 析构函数

CEditorOnlineCtrl::~CEditorOnlineCtrl()
{
	// TODO: 在此清理控件的实例数据。
}



// CEditorOnlineCtrl::OnDraw - 绘图函数

void CEditorOnlineCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	if (!pdc)
		return;

	// TODO: 用您自己的绘图代码替换下面的代码。
	pdc->FillRect(rcBounds, CBrush::FromHandle((HBRUSH)GetStockObject(WHITE_BRUSH)));
	pdc->Ellipse(rcBounds);

	if (!IsOptimizedDraw())
	{
		// 容器不支持优化绘图。

		// TODO: 如果将任何 GDI 对象选入到设备上下文 *pdc 中，
		//		请在此处恢复先前选定的对象。
	}
}



// CEditorOnlineCtrl::DoPropExchange - 持久性支持

void CEditorOnlineCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: 为每个持久的自定义属性调用 PX_ 函数。
}



// CEditorOnlineCtrl::GetControlFlags -
// 自定义 MFC 的 ActiveX 控件实现的标志。
//
DWORD CEditorOnlineCtrl::GetControlFlags()
{
	DWORD dwFlags = COleControl::GetControlFlags();


	// 在活动和不活动状态之间进行转换时，
	// 不会重新绘制控件。
	dwFlags |= noFlickerActivate;

	// 控件通过不还原设备上下文中的
	// 原始 GDI 对象，可以优化它的 OnDraw 方法。
	dwFlags |= canOptimizeDraw;
	return dwFlags;
}



// CEditorOnlineCtrl::OnResetState - 将控件重置为默认状态

void CEditorOnlineCtrl::OnResetState()
{
	COleControl::OnResetState();  // 重置 DoPropExchange 中找到的默认值

	// TODO: 在此重置任意其他控件状态。
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

// CEditorOnlineCtrl 消息处理程序

ULONG CEditorOnlineCtrl::ShowAxWnd(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    // TODO: 在此添加调度处理程序代码
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
	// TODO:清楚临时目录
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

    // TODO: 在此添加调度处理程序代码

    m_DocLibList = sDocLst;
	return m_DocLibList.AllocSysString();
}

BSTR CEditorOnlineCtrl::GetDocLibLst(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: 在此添加调度处理程序代码

    return m_DocLibList.AllocSysString();
}

BSTR CEditorOnlineCtrl::SetConfigInfo(LPCTSTR CfgInfo)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: 在此添加调度处理程序代码
    m_CfgInfo = CfgInfo;
	return m_CfgInfo.AllocSysString();
}

BSTR CEditorOnlineCtrl::GetConfigInfo(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    CString strResult;

    // TODO: 在此添加调度处理程序代码

    return m_CfgInfo.AllocSysString();
}

void CEditorOnlineCtrl::SetEnableEditCol(ULONG iItem, VARIANT_BOOL Enable)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());

    // TODO: 在此添加调度处理程序代码
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

    // TODO: 在此添加调度处理程序代码
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
