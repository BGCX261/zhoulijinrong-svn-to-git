// EditorOnlinePropPage.cpp : CEditorOnlinePropPage 属性页类的实现。

#include "stdafx.h"
#include "EditorOnline.h"
#include "EditorOnlinePropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CEditorOnlinePropPage, COlePropertyPage)



// 消息映射

BEGIN_MESSAGE_MAP(CEditorOnlinePropPage, COlePropertyPage)
END_MESSAGE_MAP()



// 初始化类工厂和 guid

IMPLEMENT_OLECREATE_EX(CEditorOnlinePropPage, "EDITORONLINE.EditorOnlinePropPage.1",
	0xd3933432, 0x3c91, 0x4114, 0xb5, 0x42, 0xb2, 0x32, 0x3e, 0xa3, 0xa2, 0xf4)



// CEditorOnlinePropPage::CEditorOnlinePropPageFactory::UpdateRegistry -
// 添加或移除 CEditorOnlinePropPage 的系统注册表项

BOOL CEditorOnlinePropPage::CEditorOnlinePropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_EDITORONLINE_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// CEditorOnlinePropPage::CEditorOnlinePropPage - 构造函数

CEditorOnlinePropPage::CEditorOnlinePropPage() :
	COlePropertyPage(IDD, IDS_EDITORONLINE_PPG_CAPTION)
{
}



// CEditorOnlinePropPage::DoDataExchange - 在页和属性间移动数据

void CEditorOnlinePropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// CEditorOnlinePropPage 消息处理程序
