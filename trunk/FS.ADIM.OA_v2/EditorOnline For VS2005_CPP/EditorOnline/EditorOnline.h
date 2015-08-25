#pragma once

// EditorOnline.h : EditorOnline.DLL 的主头文件

#if !defined( __AFXCTL_H__ )
#error "在包括此文件之前包括“afxctl.h”"
#endif

#include "resource.h"       // 主符号


// CEditorOnlineApp : 有关实现的信息，请参阅 EditorOnline.cpp。

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

