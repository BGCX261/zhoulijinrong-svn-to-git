#pragma once

// EditorOnlinePropPage.h : CEditorOnlinePropPage ����ҳ���������


// CEditorOnlinePropPage : �й�ʵ�ֵ���Ϣ������� EditorOnlinePropPage.cpp��

class CEditorOnlinePropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(CEditorOnlinePropPage)
	DECLARE_OLECREATE_EX(CEditorOnlinePropPage)

// ���캯��
public:
	CEditorOnlinePropPage();

// �Ի�������
	enum { IDD = IDD_PROPPAGE_EDITORONLINE };

// ʵ��
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

// ��Ϣӳ��
protected:
	DECLARE_MESSAGE_MAP()
};

