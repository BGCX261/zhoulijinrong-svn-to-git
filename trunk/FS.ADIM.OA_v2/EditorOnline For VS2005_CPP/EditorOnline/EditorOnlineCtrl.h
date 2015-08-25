#pragma once
#include "docmgr.h"
// EditorOnlineCtrl.h : CEditorOnlineCtrl ActiveX �ؼ����������
#include <objsafe.h>

// CEditorOnlineCtrl : �й�ʵ�ֵ���Ϣ������� EditorOnlineCtrl.cpp��

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


// ���캯��
public:
	CEditorOnlineCtrl();

// ��д
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual DWORD GetControlFlags();

// ʵ��
protected:
	~CEditorOnlineCtrl();

	DECLARE_OLECREATE_EX(CEditorOnlineCtrl)    // �๤���� guid
	DECLARE_OLETYPELIB(CEditorOnlineCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CEditorOnlineCtrl)     // ����ҳ ID
	DECLARE_OLECTLTYPE(CEditorOnlineCtrl)		// �������ƺ�����״̬

// ��Ϣӳ��
	DECLARE_MESSAGE_MAP()

// ����ӳ��
	DECLARE_DISPATCH_MAP()

// �¼�ӳ��
	DECLARE_EVENT_MAP()

// ���Ⱥ��¼� ID
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

