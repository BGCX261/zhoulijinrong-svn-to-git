#pragma once

#define  WM_BN_CLICK  WM_USER + 100
#define  USE_TOPINDEX_BUTTON
// CButtonEx

// 类名：CButtonEx
// 功能描述：用户动态加载到ListCtrlEx中的列中
class CButtonEx : public CButton
{
	DECLARE_DYNAMIC(CButtonEx)

public:
	CButtonEx();
	CButtonEx( int nItem, int nSubItem, CRect rect, HWND hParent );
	virtual ~CButtonEx();

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClicked();
	int m_inItem;
	int m_inSubItem;
	CRect m_rect;
	HWND m_hParent;
	BOOL bEnable;
};


