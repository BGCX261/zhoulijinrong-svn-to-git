// EditorOnline.cpp : CEditorOnlineApp 和 DLL 注册的实现。

#include "stdafx.h"
#include "EditorOnline.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CEditorOnlineApp theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0x5B07517A, 0x352B, 0x436B, { 0xAC, 0xE1, 0x2B, 0x53, 0x78, 0x53, 0x37, 0x3A } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;



// CEditorOnlineApp::InitInstance - DLL 初始化

BOOL CEditorOnlineApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: 在此添加您自己的模块初始化代码。
        
        #ifdef _DEBUG
            m_msOld.Checkpoint();
        #endif  // _DEBUG
	}

	return bInit;
}



// CEditorOnlineApp::ExitInstance - DLL 终止

int CEditorOnlineApp::ExitInstance()
{
	// TODO: 在此添加您自己的模块终止代码。
    #ifdef _DEBUG
        m_msNew.Checkpoint();
        if (m_msDiff.Difference(m_msOld, m_msNew))
        {
            afxDump<<"\nMemory Leaked :\n";
            m_msDiff.DumpStatistics();
            afxDump<<"Dump Complete !\n\n";
        }
    #endif  // _DEBUG
	return COleControlModule::ExitInstance();
}



// DllRegisterServer - 将项添加到系统注册表

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}



// DllUnregisterServer - 将项从系统注册表中移除

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
