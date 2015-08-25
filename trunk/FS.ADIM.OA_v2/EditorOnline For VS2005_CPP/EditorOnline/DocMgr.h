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
// CDocMgr �Ի���
//GETDLGRETVALUE pGetRetValue;
class CDocMgr : public CDialog
{
	DECLARE_DYNAMIC(CDocMgr)

public:
	CDocMgr(CWnd* pParent = NULL);   // ��׼���캯��
//	virtual ~CDocMgr();

// �Ի�������
	enum { IDD = IDD_DLG_DOCMGR };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��
	virtual BOOL OnInitDialog();
	afx_msg LRESULT onBnCLick( WPARAM wParam, LPARAM lParam );	//����ListCtrlEx�ؼ��е��а�ť�¼�
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
	BOOL InitConfigInfo(void);			//��ʼ��������Ϣ
	void InitListCtrlEx(void);
	BOOL InitDocLibList(void);
	bool ReadXmlFile(string& szFileName);

	friend bool UpdateMeta(PVOID param, BSTR sURL, BSTR sValue, int iSubItem);	//����ListCtrlEx��ɱ༭�����Ϊ����

    void GetItemsValue(int nItem, DocItemEx* item);
    int EnableTrackRevision(CString& DocType, bool Enable);

    BOOL UnInitDialog(void);
	BOOL PreClsWndHandleDocsChkOuted(void);
	void GenDocLibList(void);
    void GenDocLibListEx(void);

    void SetEditStatus(bool status);
    BOOL GetEditStatus(void);

    // WORD�����߳�
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
    BOOL       m_bExitEditOnline;       // ��ʶ�û��Ƿ�������߱༭,���������߱༭״̬��ΪTRUE,����ΪFALSE
    CCriticalSection m_cs;

public:
    CString m_DocLst;					// �����ĵ���Ϣ�ַ���
										
	CString m_ConfigInfo;				// ����������Ϣ������ Web Services URL���õ�ַ��ǩ���󱣴浽��������ʱĿ¼���ϴ�Ĭ�ϵ�ַ
										// "webserviceaddress,tmppath,MOSSWebSite,UserName"

	CString m_WebSvrAddr;				// Web Services URL���õ�ַ
	CString m_TmpPath;					// ǩ���󱣴浽��������ʱĿ¼
	CString m_UploadURL;				// �ϴ�Ĭ��URL��ַ
    CString m_MossSiteURL;              // MOSSվ��URL
    CString m_Comment;

	CWebSvrRef m_WebServicesRef;		// ��CDocumentLibraryT<> m_WebSvrRef����ķ�װ

	CDocOutLst m_DocOutList;			// ��¼ǩ���ĵ���Ϣ����
    AsynX      m_WatchTrackRevision;    // �첽��,����һ��WORD�����߳�,����WORD�ĺۼ�����,�Լ��޸���
    
	CStatic m_Status;
    CString m_sAutoItem;                // ���潫�Զ�Ǩ�����ĵ�KEYֵ,��Ϊ�������Զ�Ǩ���ĵ�
    int GetItemByKey(CString& sKey, int iCol) const;
};
