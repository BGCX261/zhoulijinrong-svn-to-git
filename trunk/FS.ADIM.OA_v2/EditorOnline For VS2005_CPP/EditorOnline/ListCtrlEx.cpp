/********************************************************************
* 	Project  : 
* 	FileName : ListCtrlEx.cpp
* 	Change   : 	
* 	Brief    : 支持列中添加按钮；支持双击项改变文本内容。
*			   除ListCtrlEx.h,ListCtrlEx.cpp还需要支持类文件ButtonEx.h,ButtonEx.cpp
* 	Author   : 
*********************************************************************/

#include "stdafx.h"
#include "ListCtrlEx.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif 

#define IDC_BTN_CHKIN	0x1235
#define IDC_BTN_CHKOUT	0x1236
#define IDC_BTN_DEL		0x1237




/////////////////////////////////////////////////////////////////////////////
// CListCtrlEx


CListCtrlEx::CListCtrlEx()
{
	m_uID = IDC_BTN_DEL;
	m_iItem = -1;
	m_iSubItem = -1;
	
	m_bHighLight = FALSE;
	m_bFocus = FALSE;

    m_bColALIASEdit = TRUE;
    m_bColENCODEEdit = TRUE;
    m_bColPAGESEdit = TRUE;
}



CListCtrlEx::~CListCtrlEx()
{
	
}



BEGIN_MESSAGE_MAP( CListCtrlEx, CListCtrl )
	//{{AFX_MSG_MAP( CListCtrlEx )
	ON_WM_LBUTTONDOWN()
	ON_WM_PAINT()
	ON_NOTIFY_REFLECT(LVN_BEGINLABELEDIT, OnBeginlabeledit)
	ON_NOTIFY_REFLECT(LVN_ENDLABELEDIT, OnEndlabeledit)
	ON_WM_KILLFOCUS()
	ON_WM_SETFOCUS()
	//}}AFX_MSG_MAP
	ON_NOTIFY_REFLECT(LVN_ENDSCROLL, &CListCtrlEx::OnLvnEndScroll)
	ON_NOTIFY_REFLECT(NM_CUSTOMDRAW, OnCustomDraw)
	ON_WM_DRAWITEM()
	ON_WM_VSCROLL()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CListCtrlEx 

void CListCtrlEx::createItemButton( int nItem, int nSubItem, HWND hMain, BSTR sName )
{
	CRect rect;
	int offset = 0;

	// Make sure that the item is visible
	if( !EnsureVisible(nItem, TRUE)) 
		return ;

	GetSubItemRect(nItem, nSubItem, LVIR_BOUNDS, rect);
	
	// Now scroll if we need to expose the column
	CRect rcClient;
	GetClientRect(rcClient);
	if( offset + rect.left < 0 || offset + rect.left > rcClient.right )
	{
		CSize size;
		size.cx = offset + rect.left;
		size.cy = 0;
		Scroll(size);
		rect.left -= size.cx;
	}

	rect.left += offset;	
	rect.right = rect.left + GetColumnWidth(nSubItem);
	if(rect.right > rcClient.right) 
		rect.right = rcClient.right;
	//basic code end

	rect.bottom = rect.top + rect.Height();

	int iPageCout = GetCountPerPage();
	if ( nItem >= iPageCout )
	{
		rect.top += rect.Height();
		rect.bottom += rect.Height();
	}

	DWORD dwStyle =  WS_CHILD | WS_VISIBLE;
	CButtonEx *pButton = new CButtonEx(nItem,nSubItem,rect,hMain);
	m_uID++;
	pButton->Create(sName,dwStyle, rect, this, m_uID);
	
	//int index = nItem * 10 + nSubItem;
	CComBSTR bsName(sName);
	/*if(bsName == L"删除")
	{
		m_mButton.insert( make_pair( nItem, pButton ) );
	}*/
	if(bsName == L"签出")
	{
		m_mBtnChkOut.insert( make_pair( nItem, pButton ) );
	}
	if(bsName == L"签入")
	{
		m_mBtnChkIn.insert( make_pair( nItem, pButton ) );
	}
	int iTopIndex = GetTopIndex();
	if ( iTopIndex > 0 )
	{
		updateListCtrlButtonPos();
	}
	
	return;
}

void CListCtrlEx::release()
{
	button_map::iterator iter;
	/*iter = m_mButton.begin();
	while ( iter != m_mButton.end() )
	{
		delete iter->second;
		iter->second = NULL;
		iter++;
	}*/

	iter = m_mBtnChkOut.begin();
	while ( iter != m_mBtnChkOut.end() )
	{
		delete iter->second;
		iter->second = NULL;
		iter++;
	}

	iter = m_mBtnChkIn.begin();
	while ( iter != m_mBtnChkIn.end() )
	{
		delete iter->second;
		iter->second = NULL;
		iter++;
	}
}
void CListCtrlEx::deleteItemEx( int nItem )
{
	int iCount = GetItemCount();
	DeleteItem( nItem );
	button_map::iterator iter;
	button_map::iterator iterNext;
#ifdef USE_TOPINDEX_BUTTON
	//add-----------------------------------
	/*iter = m_mButton.find( nItem );
	iterNext = iter;
	iterNext++;
	while ( iterNext != m_mButton.end() )
	{
		iter->second->bEnable = iterNext->second->bEnable;
		iterNext++;
		iter++;
	}*/
	//------------------------------
	//add-----------------------------------
	iter = m_mBtnChkOut.find( nItem );
	iterNext = iter;
	iterNext++;
	while ( iterNext != m_mBtnChkOut.end() )
	{
		iter->second->bEnable = iterNext->second->bEnable;
		iterNext++;
		iter++;
	}
	//------------------------------
	//add-----------------------------------
	iter = m_mBtnChkIn.find( nItem );
	iterNext = iter;
	iterNext++;
	while ( iterNext != m_mBtnChkIn.end() )
	{
		iter->second->bEnable = iterNext->second->bEnable;
		iterNext++;
		iter++;
	}
	//------------------------------
#endif
	/*iter = m_mButton.find( iCount - 1 );
	if ( iter != m_mButton.end() )
	{
		delete iter->second;
		iter->second = NULL;
		m_mButton.erase( iter );
		updateListCtrlButtonPos();
	}*/
	iter = m_mBtnChkOut.find( iCount - 1 );
	if ( iter != m_mBtnChkOut.end() )
	{
		delete iter->second;
		iter->second = NULL;
		m_mBtnChkOut.erase( iter );
		updateListCtrlButtonPos();
	}
	iter = m_mBtnChkIn.find( iCount - 1 );
	if ( iter != m_mBtnChkIn.end() )
	{
		delete iter->second;
		iter->second = NULL;
		m_mBtnChkIn.erase( iter );
		updateListCtrlButtonPos();
	}
}
void CListCtrlEx::OnLvnEndScroll(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLVSCROLL pStateChanged = reinterpret_cast<LPNMLVSCROLL>(pNMHDR);
	updateListCtrlButtonPos();
    *pResult = 0;
    
}

void CListCtrlEx::OnDrawItem(int nIDCtl, LPDRAWITEMSTRUCT lpDrawItemStruct)
{
	CListCtrl::OnDrawItem(nIDCtl, lpDrawItemStruct);
}

void CListCtrlEx::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	CListCtrl::OnVScroll(nSBCode, nPos, pScrollBar);
	updateListCtrlButtonPos();
}

void CListCtrlEx::updateListCtrlButtonPos()
{
	int iTopIndex = GetTopIndex();
	int nItem = iTopIndex;
	button_map::iterator iter;
	button_map::iterator iterUp;
	int iLine = 0;
#ifdef USE_TOPINDEX_BUTTON
	/*iter = m_mButton.find( iTopIndex );
	iterUp = m_mButton.begin();
	while ( iter != m_mButton.end() )
	{
		iterUp->second->EnableWindow( iter->second->bEnable );
		iter ++;
		iterUp++;
	}*/

	iter = m_mBtnChkOut.find( iTopIndex );
	iterUp = m_mBtnChkOut.begin();
	while ( iter != m_mBtnChkOut.end() )
	{
		iterUp->second->EnableWindow( iter->second->bEnable );
		iter ++;
		iterUp++;
	}

	iter = m_mBtnChkIn.find( iTopIndex );
	iterUp = m_mBtnChkIn.begin();
	while ( iter != m_mBtnChkIn.end() )
	{
		iterUp->second->EnableWindow( iter->second->bEnable );
		iter ++;
		iterUp++;
	}
#else
	for ( ; nItem < GetItemCount(); nItem++ )
	{
		iter = m_mButton.find(nItem);
		if( iter!= m_mButton.end() )
		{
			CRect rect;
			iterUp = m_mButton.find(iLine);
			rect = iterUp->second->m_rect;
			iter->second->MoveWindow( &rect );
			iter->second->ShowWindow( SW_SHOW );
			if( iLine < iTopIndex )
			{
				iterUp->second->ShowWindow( SW_HIDE );
			}
			iLine++;
		}
	}
#endif

}

void CListCtrlEx::enableButton( BOOL bFlag, int iItem )
{
	button_map::iterator iter;
#ifdef USE_TOPINDEX_BUTTON
	int iTopIndex = GetTopIndex();
	int nItem = iItem - iTopIndex;
	/*iter = m_mButton.find( iItem );
	if ( iter != m_mButton.end() )
	{
		iter->second->bEnable = bFlag;
	}
	iter = m_mButton.find( nItem );
	if ( iter != m_mButton.end() )
	{
		iter->second->EnableWindow( bFlag );
	}*/
#else
	iter = m_mButton.find( iItem );
	if ( iter != m_mButton.end() )
	{
		iter->second->EnableWindow( bFlag );
	}
#endif

}

void CListCtrlEx::enableButton( BOOL bFlag, int iItem, int iSubItem )
{
	button_map::iterator iter;
	int iTopIndex;
	int nItem;
#ifdef USE_TOPINDEX_BUTTON
	/*if(iSubItem == ColDELETE)
	{
		iTopIndex = GetTopIndex();
		nItem = iItem - iTopIndex;
		iter = m_mButton.find( iItem );
		if ( iter != m_mButton.end() )
		{
			iter->second->bEnable = bFlag;
		}
		iter = m_mButton.find( nItem );
		if ( iter != m_mButton.end() )
		{
			iter->second->EnableWindow( bFlag );
		}
	}*/
	if(iSubItem == ColCHKOUT)
	{
		iTopIndex = GetTopIndex();
		nItem = iItem - iTopIndex;
		iter = m_mBtnChkOut.find( iItem );
		if ( iter != m_mBtnChkOut.end() )
		{
			iter->second->bEnable = bFlag;
		}
		iter = m_mBtnChkOut.find( nItem );
		if ( iter != m_mBtnChkOut.end() )
		{
			iter->second->EnableWindow( bFlag );
		}
	}

	if(iSubItem == ColCHKIN)
	{
		iTopIndex = GetTopIndex();
		nItem = iItem - iTopIndex;
		iter = m_mBtnChkIn.find( iItem );
		if ( iter != m_mBtnChkIn.end() )
		{
			iter->second->bEnable = bFlag;
		}
		iter = m_mBtnChkIn.find( nItem );
		if ( iter != m_mBtnChkIn.end() )
		{
			iter->second->EnableWindow( bFlag );
		}
	}
#else
	iter = m_mButton.find( iItem );
	if ( iter != m_mButton.end() )
	{
		iter->second->EnableWindow( bFlag );
	}
#endif

}

void CListCtrlEx::OnLButtonDown(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default

	m_bFocus = TRUE;

	LVHITTESTINFO  lvhit;
	lvhit.pt = point;
	int item = SubItemHitTest(&lvhit);
	
    
	//if (over a item/subitem)
	if (item != -1 && (lvhit.flags & LVHT_ONITEM))
	{
		CListCtrl::OnLButtonDown(nFlags, point);
		
		if(m_bHighLight && m_iItem == lvhit.iItem && m_iSubItem == lvhit.iSubItem)
		{
			if(m_iSubItem >=3 || !EnableEdit(m_iSubItem)) 
                return;
			//第二次单击
			EditLabel(m_iItem);
			return;
		}
		else
		{
			//第一次单击
			m_iItem = lvhit.iItem;
			m_iSubItem = lvhit.iSubItem;
			m_bHighLight = TRUE;
		}
	}
	else
	{
		if(m_edtItemEdit.m_hWnd == NULL)
		{
			//未出现文本编辑框时
			m_bHighLight = FALSE;
		}
		
		CListCtrl::OnLButtonDown(nFlags, point);
	}
	
	Invalidate();
}

//EditLabel() cause this function has been called
void CListCtrlEx::OnBeginlabeledit(NMHDR* pNMHDR, LRESULT* pResult) 
{
	LV_DISPINFO* pDispInfo = (LV_DISPINFO*)pNMHDR;
    if(!EnableEdit(m_iSubItem)) {
        m_iSubItem = -1;
    }
	// TODO: Add your control notification handler code here
	if (m_iSubItem >= 0)
    {
		ASSERT(m_iItem == pDispInfo->item.iItem);

		CRect  rcSubItem;
		GetSubItemRect( pDispInfo->item.iItem, m_iSubItem, LVIR_BOUNDS, rcSubItem);
		
		//get edit control and subclass
		HWND hWnd= (HWND)SendMessage(LVM_GETEDITCONTROL);
		ASSERT(hWnd != NULL);
		VERIFY(m_edtItemEdit.SubclassWindow(hWnd));

		//move edit control text 4 pixel to the right of org label,
		//as Windows does it...
		m_edtItemEdit.m_iXPos = rcSubItem.left + 4;
		m_edtItemEdit.m_iSubItem = m_iSubItem;
		m_edtItemEdit.SetWindowText(GetItemText(pDispInfo->item.iItem, m_iSubItem));
	}

	*pResult = 0;
}


void CListCtrlEx::OnPaint() 
{	
	//CPaintDC dc(this); // device context for painting	
	
	if (m_iSubItem >= 0 && m_edtItemEdit.m_hWnd)	
	{
		CRect	rect;
		CRect	rcEdit;
		m_edtItemEdit.GetWindowRect(rcEdit);
		ScreenToClient(rcEdit);

		GetSubItemRect(m_iItem, m_iSubItem, LVIR_LABEL, rect);

		//当文本编辑框缩小时,擦除露出的项文本高亮部分
		if (rcEdit.right < rect.right)
		{
			rect.left = rcEdit.right;
			CClientDC dc(this);
			dc.FillRect(rect, &CBrush(::GetSysColor(COLOR_WINDOW)));
			ValidateRect(rect);
		}
	}

	CListCtrl::OnPaint();
}


void CListCtrlEx::OnEndlabeledit(NMHDR* pNMHDR, LRESULT* pResult) 
{
	LV_DISPINFO* pDispInfo = (LV_DISPINFO*)pNMHDR;
	LV_ITEM	*plvItem = &pDispInfo->item;
	
	if (m_iSubItem >= 0)
	{
		if (plvItem->pszText != NULL )
		{
			BSTR sURL = GetItemText(plvItem->iItem, ColURL).AllocSysString();
			BSTR sTitle = GetItemText(plvItem->iItem, ColTITLE).AllocSysString();
			BSTR sType = GetItemText(plvItem->iItem, ColTYPE).AllocSysString();
			CComBSTR sURL_Title;
			if(wcslen(sURL) <= 0) {
				sURL_Title.Append(m_DefaultURLAddr);
			}
			else {
				sURL_Title.Append(sURL);
			}
			sURL_Title.Append(L"/");
			sURL_Title.Append(sTitle);
			sURL_Title.Append(L".");
			sURL_Title.Append(sType);
			BSTR sValue = plvItem->pszText;
			//TODO: 调用UpdataMeta更新NAME属性到服务器
			
			if(!m_pFun(m_param, sURL_Title.m_str, sValue, m_iSubItem)) {
				
				::SysFreeString(sURL);
				::SysFreeString(sTitle);
				::SysFreeString(sType);
				return;
			}

			CString tmp(plvItem->pszText);
			if(tmp.GetLength() == 0 && m_iSubItem == ColSIZE) {
				SetItemText(plvItem->iItem, m_iSubItem, L"0");
			}
			else {
				SetItemText(plvItem->iItem, m_iSubItem, plvItem->pszText);
			}
			::SysFreeString(sURL);
			::SysFreeString(sTitle);
			::SysFreeString(sType);
		}
		
		VERIFY(m_edtItemEdit.UnsubclassWindow()!=NULL);
		*pResult = 0;
	}

	//编辑文本时对控件父窗口操作(如单击其它控件)引发"OnEndlabeledit"时刷新控件
	CRect rect;
	GetWindowRect(&rect);
	CPoint point;
	::GetCursorPos(&point);
	if(!rect.PtInRect(point))
	{
		m_iItem = -1;
		m_iSubItem = -1;
		m_bFocus = FALSE;
		m_bHighLight = FALSE;
	}
}

void CListCtrlEx::OnCustomDraw(NMHDR* pNMHDR, LRESULT* pResult)
{
	//draw each item.set txt color,bkcolor....
	NMLVCUSTOMDRAW* pNMLVCustomDraw = (NMLVCUSTOMDRAW*)pNMHDR;
	
	// Take the default processing unless we set this to something else below.
	*pResult = CDRF_DODEFAULT;
	
	// First thing - check the draw stage. If it's the control's prepaint
	// stage, then tell Windows we want messages for every item.
	
	if (pNMLVCustomDraw->nmcd.dwDrawStage == CDDS_PREPAINT)
	{
		*pResult = CDRF_NOTIFYITEMDRAW;
	}
	else if (pNMLVCustomDraw->nmcd.dwDrawStage == CDDS_ITEMPREPAINT)
	{
		// This is the notification message for an item.  We'll request
		// notifications before each subitem's prepaint stage.
		*pResult = CDRF_NOTIFYSUBITEMDRAW;
	}
	else if (pNMLVCustomDraw->nmcd.dwDrawStage == (CDDS_ITEMPREPAINT | CDDS_SUBITEM))
	{
		// store the colors back in the NMLVCUSTOMDRAW struct
		// but it's effective only when *pResult = CDRF_DODEFAULT 

		//	pNMLVCustomDraw->clrText = RGB(0, 0, 255);
		//	pNMLVCustomDraw->clrTextBk = RGB(0, 255, 0);
		//	*pResult = CDRF_DODEFAULT;
		
		// This is the prepaint stage for a subitem. Here's where we set the
		// item's text and background colors. Our return value will tell
		// Windows to draw the subitem itself, but it will use the new colors
		// we set here.
		int iItem = (int)pNMLVCustomDraw->nmcd.dwItemSpec;
		int iSubItem = pNMLVCustomDraw->iSubItem;
		
		CDC* pDC = CDC::FromHandle(pNMLVCustomDraw->nmcd.hdc);
		
		CString strItemText = GetItemText(iItem, iSubItem);
		CRect rcItem, rcText;
		GetSubItemRect(iItem, iSubItem, LVIR_LABEL, rcItem);
		rcText = rcItem;
		
		CSize size = pDC->GetTextExtent(strItemText);
		if(strItemText == _T(""))
		{
			size.cx = 41; 
		}
		
		//设置文本高亮矩形
		rcText.left += 4;
		rcText.right = rcText.left + size.cx + 6;
		if(rcText.right > rcItem.right)
		{
			rcText.right = rcItem.right;
		}
		
		COLORREF crOldTextColor = pDC->GetTextColor();

		//绘制项焦点/高亮效果
		if(m_bFocus)
		{
			
			if((m_iItem == iItem) && (m_iSubItem == iSubItem))
			{	
				if(m_bHighLight)
				{					
					pDC->SetTextColor(::GetSysColor(COLOR_HIGHLIGHTTEXT));
					pDC->FillSolidRect(&rcText, ::GetSysColor(COLOR_HIGHLIGHT));
				}
				pDC->DrawFocusRect(&rcText);
			}		
		}
		
		//绘制项文本
		rcItem.left += 6;
		pDC->DrawText(strItemText, &rcItem, DT_LEFT | DT_VCENTER | DT_SINGLELINE | DT_END_ELLIPSIS | DT_NOCLIP);

		pDC->SetTextColor(crOldTextColor);
		*pResult = CDRF_SKIPDEFAULT;// We've painted everything.
	}	
}


void CListCtrlEx::PreSubclassWindow() 
{
	// TODO: Add your specialized code here and/or call the base class
	CListCtrl::PreSubclassWindow();
	ModifyStyle(0, LVS_EDITLABELS);
}

void CListCtrlEx::OnKillFocus(CWnd* pNewWnd) 
{
	CListCtrl::OnKillFocus(pNewWnd);
	
	// TODO: Add your message handler code here
	CRect rect;
	GetWindowRect(&rect);
	CPoint point;
	::GetCursorPos(&point);
	if(!rect.PtInRect(point) && GetParent()->GetFocus() != NULL)
	{
		m_iItem = -1;
		m_iSubItem = -1;
		m_bFocus = FALSE;
		m_bHighLight = FALSE;
		Invalidate();
	}
	
}

void CListCtrlEx::OnSetFocus(CWnd* pOldWnd) 
{
//	CListCtrl::OnSetFocus(pOldWnd);
	
	// TODO: Add your message handler code here
	
}

void CListCtrlEx::deleteAllItemsEx(void)
{
	int iCount = GetItemCount();
	for(int i=0;i<iCount;i++)
	{
		deleteItemEx(i);
	}
}

// 指定列是否可编辑,若可编辑则返回TRUE,否则返回FALSE;
BOOL CListCtrlEx::EnableEdit(int iItem)
{
    //m_bColALIASEdit;
    //m_bColENCODEEdit;
    //m_bColPAGESEdit;
    switch(iItem)
    {
    case 0:
        return (m_bColALIASEdit);
    case 1:
        return (m_bColENCODEEdit);
    case 2:
        return (m_bColPAGESEdit);
    }
    return FALSE;
}
/////////////////////////////////////////////////////////////////////////////
// CItemEdit

CItemEdit::CItemEdit()
{
    //SetLimitText(10);
}

CItemEdit::~CItemEdit()
{
}


BEGIN_MESSAGE_MAP(CItemEdit, CEdit)
    //{{AFX_MSG_MAP(CItemEdit)
    ON_WM_WINDOWPOSCHANGING()
    //}}AFX_MSG_MAP
	ON_WM_CHAR()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CItemEdit message handlers

void CItemEdit::OnWindowPosChanging(WINDOWPOS FAR* lpwndpos) 
{
	lpwndpos->x = m_iXPos;
	
	CEdit::OnWindowPosChanging(lpwndpos);
	
	// TODO: Add your message handler code here
	
}

void CItemEdit::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

    int nAOldLen = this->GetWindowTextLengthW();
    if(nAOldLen >= IMAXLENGTH) return;

	if (m_iSubItem == ColPAGES && nChar != 0x08)	// 处理第三列PAGE字段，过滤删除虚拟键（0x08）
	{
		if(nChar >= 0x30 && nChar <= 0x3A) {	// 第三列PAGE字段只接受0～9的字符输入，以及删除按键
			CEdit::OnChar(nChar, nRepCnt, nFlags);
			return;
		}
		else {
			return;
		}
	}
    if (m_iSubItem == ColENCODE && nChar != 0x08)	// 处理第一列ALIAS字段，过滤删除虚拟键（0x08）
	{
        if(nChar == '\\' || nChar== '/' || nChar == ':' || 
            nChar == '*' || nChar == '.' || nChar == '?' || nChar == '%' ||
            nChar == '"' || nChar == '<' || nChar == '>' || nChar == '|') {
                return;
        }
        else
        {
		    CEdit::OnChar(nChar, nRepCnt, nFlags);
		    return;
        }
    }
    if (m_iSubItem == ColALIAS && nChar != 0x08)	// 处理第一列ALIAS字段，过滤删除虚拟键（0x08）
	{
        if(nChar == '\\' || nChar== '/' || nChar == ':' || 
            nChar == '*' || nChar == '.' || nChar == '?' || nChar == '%' ||
            nChar == '"' || nChar == '<' || nChar == '>' || nChar == '|') {
                return;
        }
        else
        {
		    CEdit::OnChar(nChar, nRepCnt, nFlags);
		    return;
        }
        
	}
	CEdit::OnChar(nChar, nRepCnt, nFlags);
}


