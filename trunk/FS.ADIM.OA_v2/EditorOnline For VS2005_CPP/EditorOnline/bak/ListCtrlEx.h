/********************************************************************
* 	Project  : 
* 	FileName : ListCtrlEx.h
* 	Change   : 	
* 	Brief    : 支持列中添加按钮；支持双击项改变文本内容。
*			   除ListCtrlEx.h,ListCtrlEx.cpp还需要支持类文件ButtonEx.h,ButtonEx.cpp
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
// 僐儞僗僩儔僋僔儑儞
public:
	CListCtrlEx();

// 傾僩儕價儏乕僩
public:

// 僆儁儗乕僔儑儞
public:

// 僆乕僶乕儔僀僪
	// ClassWizard 偼壖憐娭悢偺僆乕僶乕儔僀僪傪惗惉偟傑偡丅
	//{{AFX_VIRTUAL( CListCtrlEx )
	protected:
	virtual void PreSubclassWindow();
	//}}AFX_VIRTUAL

// 僀儞僾儕儊儞僥乕僔儑儞
public:

	virtual ~CListCtrlEx();

	// 惗惉偝傟偨儊僢僙乕僕 儅僢僾娭悢
protected:
	//{{AFX_MSG( CListCtrlEx )
		// 儊儌 - ClassWizard 偼偙偺埵抲偵儊儞僶娭悢傪捛壛傑偨偼嶍彍偟傑偡丅
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
	//button_map m_mButton;		//删除按钮MAP
	button_map m_mBtnChkOut;	//签出按钮MAP
	button_map m_mBtnChkIn;		//签入按钮MAP
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
	int m_iSubItem;            //子项标识符
	int m_iItem;               //主项标识符
	BOOL m_bHighLight;         //是否高亮文本
	BOOL m_bFocus;             //是否绘制焦点框
	CItemEdit m_edtItemEdit;   // 用于子类化EditLabel函数返回的CEdit*指针

public:
	UPDATEMETA m_pFun;		   //保存用于到完成项文本编辑后回调函数地址
	PVOID	   m_param;		   //保存ListCtrlEx控件所在父窗体类this指针
	void deleteAllItemsEx(void);
	CString	   m_DefaultURLAddr;

    BOOL m_bColALIASEdit;
    BOOL m_bColENCODEEdit;
    BOOL m_bColPAGESEdit;
    BOOL EnableEdit(int iItem);
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ 偼慜峴偺捈慜偵捛壛偺愰尵傪憓擖偟傑偡丅

#endif  // !defined( AFX_LISTCTRLEX_H__3D2C6B4A_4031_48EF_8162_492882D99D43__INCLUDED_ )
