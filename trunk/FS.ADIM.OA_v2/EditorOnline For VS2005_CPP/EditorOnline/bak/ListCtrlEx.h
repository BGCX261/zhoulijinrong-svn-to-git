/********************************************************************
* 	Project  : 
* 	FileName : ListCtrlEx.h
* 	Change   : 	
* 	Brief    : ֧��������Ӱ�ť��֧��˫����ı��ı����ݡ�
*			   ��ListCtrlEx.h,ListCtrlEx.cpp����Ҫ֧�����ļ�ButtonEx.h,ButtonEx.cpp
* 	Author   : 
*********************************************************************/

#if !defined( AFX_LISTCTRLEX_H__3D2C6B4A_4031_48EF_8162_492882D99D43__INCLUDED_ )
#define AFX_LISTCTRLEX_H__3D2C6B4A_4031_48EF_8162_492882D99D43__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif  // _MSC_VER > 1000

#include "ButtonEx.h"
#include <map>
using namespace std;
#define DATACOLS        15

#define ColALIAS		0
#define ColENCODE		1
#define ColPAGES		2
#define ColSIZE         3
#define ColTYPE			4
#define ColTITLE		5
//#define ColUPDATED      6
#define ColEDITION		6
#define ColURL			7
#define ColfURL         8
#define ColProcType     9
#define ColContent      10
//#define ColMossSiteURL  12
#define ColFolderName   11
#define ColFileName     12
#define ColIsCopy       13
#define ColWorkItemID   14

#define ColCHKOUT		15
#define ColCHKIN		16
//#define ColDELETE		17

typedef map<int,CButtonEx*>button_map;
typedef bool (*UPDATEMETA)(PVOID param, BSTR sURL, BSTR sValue, int iSubItem);

/////////////////////////////////////////////////////////////////////////////
//CItemEdit window

class CItemEdit : public CEdit
{
	// Construction
public:
	CItemEdit();
	
	// Attributes
public:
	
	// Operations
public:
	
	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CItemEdit)
	//}}AFX_VIRTUAL
	
	// Implementation
public:
	int m_iSubItem;
	int m_iXPos;
	virtual ~CItemEdit();
	
	// Generated message map functions
protected:
	//{{AFX_MSG(CItemEdit)
	afx_msg void OnWindowPosChanging(WINDOWPOS FAR* lpwndpos);
	//}}AFX_MSG
	
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
};

/************************************************************************/
/* 
* this CListCtrlEx class inherits from ListCtrl
* it display the terminal list and record interrelated infomation
*/ 
/************************************************************************/
class CListCtrlEx : public CListCtrl
{
// �R���X�g���N�V����
public:
	CListCtrlEx();

// �A�g���r���[�g
public:

// �I�y���[�V����
public:

// �I�[�o�[���C�h
	// ClassWizard �͉��z�֐��̃I�[�o�[���C�h�𐶐����܂��B
	//{{AFX_VIRTUAL( CListCtrlEx )
	protected:
	virtual void PreSubclassWindow();
	//}}AFX_VIRTUAL

// �C���v�������e�[�V����
public:

	virtual ~CListCtrlEx();

	// �������ꂽ���b�Z�[�W �}�b�v�֐�
protected:
	//{{AFX_MSG( CListCtrlEx )
		// ���� - ClassWizard �͂��̈ʒu�Ƀ����o�֐���ǉ��܂��͍폜���܂��B
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnPaint();
	afx_msg void OnBeginlabeledit(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnEndlabeledit(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnKillFocus(CWnd* pNewWnd);
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	//}}AFX_MSG
	afx_msg void OnCustomDraw(NMHDR* pNMHDR, LRESULT* pResult);
	DECLARE_MESSAGE_MAP()

public:
	void createItemButton( int nItem, int nSubItem, HWND hMain, BSTR sName );
	void release();
	void deleteItemEx( int nItem );
	//button_map m_mButton;		//ɾ����ťMAP
	button_map m_mBtnChkOut;	//ǩ����ťMAP
	button_map m_mBtnChkIn;		//ǩ�밴ťMAP
public:
	afx_msg void OnLvnEndScroll(NMHDR *pNMHDR, LRESULT *pResult);
public:
	afx_msg void OnDrawItem(int nIDCtl, LPDRAWITEMSTRUCT lpDrawItemStruct);
public:
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	UINT m_uID;
	UINT m_uID1;
	UINT m_uID2;
	void updateListCtrlButtonPos();
	void enableButton( BOOL bFlag, int iItem );
	void enableButton( BOOL bFlag, int iItem, int iSubItem );

private:
	int m_iSubItem;            //�����ʶ��
	int m_iItem;               //�����ʶ��
	BOOL m_bHighLight;         //�Ƿ�����ı�
	BOOL m_bFocus;             //�Ƿ���ƽ����
	CItemEdit m_edtItemEdit;   // �������໯EditLabel�������ص�CEdit*ָ��

public:
	UPDATEMETA m_pFun;		   //�������ڵ�������ı��༭��ص�������ַ
	PVOID	   m_param;		   //����ListCtrlEx�ؼ����ڸ�������thisָ��
	void deleteAllItemsEx(void);
	CString	   m_DefaultURLAddr;

    BOOL m_bColALIASEdit;
    BOOL m_bColENCODEEdit;
    BOOL m_bColPAGESEdit;
    BOOL EnableEdit(int iItem);
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ �͑O�s�̒��O�ɒǉ��̐錾��}�����܂��B

#endif  // !defined( AFX_LISTCTRLEX_H__3D2C6B4A_4031_48EF_8162_492882D99D43__INCLUDED_ )
