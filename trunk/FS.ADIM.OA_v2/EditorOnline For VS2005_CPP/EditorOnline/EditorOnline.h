#pragma once

// EditorOnline.h : EditorOnline.DLL ����ͷ�ļ�

#if !defined( __AFXCTL_H__ )
#error "�ڰ������ļ�֮ǰ������afxctl.h��"
#endif

#include "resource.h"       // ������


// CEditorOnlineApp : �й�ʵ�ֵ���Ϣ������� EditorOnline.cpp��

class CEditorOnlineApp : public COleControlModule
{
public:
	BOOL InitInstance();
	int ExitInstance();

#ifdef _DEBUG
protected:
      CMemoryState m_msOld, m_msNew, m_msDiff;
#endif  // _DEBUG


};

extern const GUID CDECL _tlid;
extern const WORD _wVerMajor;
extern const WORD _wVerMinor;

