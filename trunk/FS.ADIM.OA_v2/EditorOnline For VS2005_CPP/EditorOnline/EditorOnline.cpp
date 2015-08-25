// EditorOnline.cpp : CEditorOnlineApp �� DLL ע���ʵ�֡�

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



// CEditorOnlineApp::InitInstance - DLL ��ʼ��

BOOL CEditorOnlineApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: �ڴ�������Լ���ģ���ʼ�����롣
        
        #ifdef _DEBUG
            m_msOld.Checkpoint();
        #endif  // _DEBUG
	}

	return bInit;
}



// CEditorOnlineApp::ExitInstance - DLL ��ֹ

int CEditorOnlineApp::ExitInstance()
{
	// TODO: �ڴ�������Լ���ģ����ֹ���롣
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



// DllRegisterServer - ������ӵ�ϵͳע���

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}



// DllUnregisterServer - �����ϵͳע������Ƴ�

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
