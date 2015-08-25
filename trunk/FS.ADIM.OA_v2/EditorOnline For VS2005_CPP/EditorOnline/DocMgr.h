#pragma once
#include "listctrlex.h"
#include "DocOutLst.h"
#include "afxwin.h"
#include "WebSvrRef.h"
#include "AsynX.hpp"

#pragma warning(disable:4244)

using namespace DocumentLibrary;

//#define _SHAWN

#define DOCX "docx"
#define DOC  "doc"

#define EXITEDITIN      0
#define REMAINEDITIN    1
#define ERR_EDITIN      2

//typedef void (*GETDLGRETVALUE)();
//void GetDocMgrRetValue();
// CDocMgr 对话框
//GETDLGRETVALUE pGetRetValue;
class CDocMgr : public CDialog
{
	DECLARE_DYNAMIC(CDocMgr)

public:
	CDocMgr(CWnd* pParent = NULL);   // 标准构造函数
//	virtual ~CDocMgr();

// 对话框数据
	enum { IDD = IDD_DLG_DOCMGR };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	virtual BOOL OnInitDialog();
	afx_msg LRESULT onBnCLick( WPARAM wParam, LPARAM lParam );	//处理ListCtrlEx控件中的列按钮事件
	DECLARE_MESSAGE_MAP()
public:
	CListCtrlEx m_List;
	//CButton m_Btn;
	afx_msg void OnBnClickedOk();
	//afx_msg void OnBnClickedUpload();

private:
	int ParseTokens(CStringArray& result , CString szString, CString szTokens);
	int InsertDocItem( CString& sAlias, CString& sEncode, CString& sPage, CString& sSize, 
						CString& sType, CString& sTitle, CString& sEdition, CString& sURL,
						CString& sfullURL, CString& sProcessType, CString& sIsZhengWen,
                        CString& sFolderName, CString& sFileName, CString& sIsCopy, CString& sWorkItemID);
public:
	afx_msg void OnBnClickedCancel();	
	BOOL InitConfigInfo(void);			//初始化配置信息
	void InitListCtrlEx(void);
	BOOL InitDocLibList(void);
	bool ReadXmlFile(string& szFileName);

	friend bool UpdateMeta(PVOID param, BSTR sURL, BSTR sValue, int iSubItem);	//处理ListCtrlEx完成编辑后的行为处理

    void GetItemsValue(int nItem, DocItemEx* item);
    int EnableTrackRevision(CString& DocType, bool Enable);

    BOOL UnInitDialog(void);
	BOOL PreClsWndHandleDocsChkOuted(void);
	void GenDocLibList(void);
    void GenDocLibListEx(void);

    void SetEditStatus(bool status);
    BOOL GetEditStatus(void);

    // WORD监视线程
    void static WatchTrackThread(LPVOID lpParam)
    {
        int retval = ((CDocMgr*)lpParam)->EnableTrackRevision(CString(DOCX), TRUE);
        //if(retval == EXITEDITIN || retval == ERR_EDITIN)
        //    break;
        //}
        
        return;
    }
	//afx_msg void OnLvnKeydownList(NMHDR *pNMHDR, LRESULT *pResult);
private:
    BOOL       m_bExitEditOnline;       // 标识用户是否结束在线编辑,若处于在线编辑状态则为TRUE,否则为FALSE
    CCriticalSection m_cs;

public:
    CString m_DocLst;					// 保存文档信息字符串
										
	CString m_ConfigInfo;				// 保存配置信息，包括 Web Services URL引用地址，签出后保存到本机的临时目录，上传默认地址
										// "webserviceaddress,tmppath,MOSSWebSite,UserName"

	CString m_WebSvrAddr;				// Web Services URL引用地址
	CString m_TmpPath;					// 签出后保存到本机的临时目录
	CString m_UploadURL;				// 上传默认URL地址
    CString m_MossSiteURL;              // MOSS站点URL
    CString m_Comment;

	CWebSvrRef m_WebServicesRef;		// 对CDocumentLibraryT<> m_WebSvrRef对象的封装

	CDocOutLst m_DocOutList;			// 记录签出文档信息对象
    AsynX      m_WatchTrackRevision;    // 异步类,创建一个WORD监视线程,监视WORD的痕迹保留,以及修改人
    
	CStatic m_Status;
    CString m_sAutoItem;                // 保存将自动迁出的文档KEY值,若为空则无自动迁出文档
    int GetItemByKey(CString& sKey, int iCol) const;
};
