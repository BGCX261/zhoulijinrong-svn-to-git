#pragma once
#include "docmgr.h"
// EditorOnlineCtrl.h : CEditorOnlineCtrl ActiveX 控件类的声明。
#include <objsafe.h>

// CEditorOnlineCtrl : 有关实现的信息，请参阅 EditorOnlineCtrl.cpp。

class CEditorOnlineCtrl : public COleControl
{
	DECLARE_DYNCREATE(CEditorOnlineCtrl)

    DECLARE_INTERFACE_MAP()

    BEGIN_INTERFACE_PART(ObjSafe, IObjectSafety)
        STDMETHOD_(HRESULT, GetInterfaceSafetyOptions) (
        REFIID riid,
        DWORD __RPC_FAR *pdwSupportedOptions,
        DWORD __RPC_FAR *pdwEnabledOptions
        );

        STDMETHOD_(HRESULT, SetInterfaceSafetyOptions) (
            REFIID riid,
            DWORD dwOptionSetMask,
            DWORD dwEnabledOptions
            );
    END_INTERFACE_PART(ObjSafe);


// 构造函数
public:
	CEditorOnlineCtrl();

// 重写
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual DWORD GetControlFlags();

// 实现
protected:
	~CEditorOnlineCtrl();

	DECLARE_OLECREATE_EX(CEditorOnlineCtrl)    // 类工厂和 guid
	DECLARE_OLETYPELIB(CEditorOnlineCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CEditorOnlineCtrl)     // 属性页 ID
	DECLARE_OLECTLTYPE(CEditorOnlineCtrl)		// 类型名称和杂项状态

// 消息映射
	DECLARE_MESSAGE_MAP()

// 调度映射
	DECLARE_DISPATCH_MAP()

// 事件映射
	DECLARE_EVENT_MAP()

// 调度和事件 ID
public:
	enum {
        dispidAutoShowAxWnd = 7L,
        dispidSetEnableEditCol = 6L,
        dispidGetConfigInfo = 5L,
        dispidSetConfigInfo = 4L,
        dispidGetDocLibLst = 3L,
        dispidSetDocLibLst = 2L,
        dispidShowAxWnd = 1L
    };
	
public:
    CDocMgr m_DocMgr;

public:
	CString m_DocLibList;
	CString m_CfgInfo;

protected:
    ULONG ShowAxWnd(void);
    BSTR SetDocLibLst(LPCTSTR sDocLst);
    BSTR GetDocLibLst(void);
    BSTR SetConfigInfo(LPCTSTR CfgInfo);
    BSTR GetConfigInfo(void);
    void SetEnableEditCol(ULONG iItem, VARIANT_BOOL Enable);
    void AutoShowAxWnd(LPCTSTR sKey);
};

