// EditorOnlinePropPage.cpp : CEditorOnlinePropPage ����ҳ���ʵ�֡�

#include "stdafx.h"
#include "EditorOnline.h"
#include "EditorOnlinePropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CEditorOnlinePropPage, COlePropertyPage)



// ��Ϣӳ��

BEGIN_MESSAGE_MAP(CEditorOnlinePropPage, COlePropertyPage)
END_MESSAGE_MAP()



// ��ʼ���๤���� guid

IMPLEMENT_OLECREATE_EX(CEditorOnlinePropPage, "EDITORONLINE.EditorOnlinePropPage.1",
	0xd3933432, 0x3c91, 0x4114, 0xb5, 0x42, 0xb2, 0x32, 0x3e, 0xa3, 0xa2, 0xf4)



// CEditorOnlinePropPage::CEditorOnlinePropPageFactory::UpdateRegistry -
// ��ӻ��Ƴ� CEditorOnlinePropPage ��ϵͳע�����

BOOL CEditorOnlinePropPage::CEditorOnlinePropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_EDITORONLINE_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// CEditorOnlinePropPage::CEditorOnlinePropPage - ���캯��

CEditorOnlinePropPage::CEditorOnlinePropPage() :
	COlePropertyPage(IDD, IDS_EDITORONLINE_PPG_CAPTION)
{
}



// CEditorOnlinePropPage::DoDataExchange - ��ҳ�����Լ��ƶ�����

void CEditorOnlinePropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// CEditorOnlinePropPage ��Ϣ�������
