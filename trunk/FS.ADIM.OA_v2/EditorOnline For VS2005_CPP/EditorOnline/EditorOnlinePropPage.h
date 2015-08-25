#pragma once

// EditorOnlinePropPage.h : CEditorOnlinePropPage 属性页类的声明。


// CEditorOnlinePropPage : 有关实现的信息，请参阅 EditorOnlinePropPage.cpp。

class CEditorOnlinePropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(CEditorOnlinePropPage)
	DECLARE_OLECREATE_EX(CEditorOnlinePropPage)

// 构造函数
public:
	CEditorOnlinePropPage();

// 对话框数据
	enum { IDD = IDD_PROPPAGE_EDITORONLINE };

// 实现
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 消息映射
protected:
	DECLARE_MESSAGE_MAP()
};

