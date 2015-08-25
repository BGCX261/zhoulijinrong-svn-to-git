// DocMgr.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "EditorOnline.h"
#include "DocMgr.h"
#include <afxdlgs.h>

#include <atlbase.h> 

#include "MSWord12\msword.h"
#include "MSWord12\CDocument0.h"
#include "MSWord12\CDocuments.h"

#include "xmlrdwr\XMLreader.h"
#include "xmlrdwr\xmlwriter.h"

void Debug(long data)
{
    #ifdef _SHAWN
        char tmp[100] = {0};
        ltoa(data, tmp, 10);
        ::MessageBoxA(NULL, tmp, "", MB_OK);
    #endif
}

// CDocMgr �Ի���
IMPLEMENT_DYNAMIC(CDocMgr, CDialog)

CDocMgr::CDocMgr(CWnd* pParent /*=NULL*/)
	: CDialog(CDocMgr::IDD, pParent)
    , m_sAutoItem(_T(""))
{
	this->m_ConfigInfo.Empty();
	this->m_DocLst.Empty();

    m_bExitEditOnline = FALSE;
    
	m_WebSvrAddr = L"";					
	m_TmpPath = L"";						
	m_UploadURL = L"";					
    m_MossSiteURL = L"";
}

//CDocMgr::~CDocMgr()
//{
//	
//}

void CDocMgr::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST, m_List);
	//DDX_Control(pDX, IDC_BUTTON1, m_Btn);
	DDX_Control(pDX, IDC_STATUS, m_Status);
}


BEGIN_MESSAGE_MAP(CDocMgr, CDialog)
	ON_MESSAGE( WM_BN_CLICK, onBnCLick)
	ON_BN_CLICKED(IDOK, &CDocMgr::OnBnClickedOk)
	//ON_BN_CLICKED(IDC_UPLOAD, &CDocMgr::OnBnClickedUpload)
	ON_BN_CLICKED(IDCANCEL, &CDocMgr::OnBnClickedCancel)
END_MESSAGE_MAP()


// CDocMgr ��Ϣ�������
BOOL CDocMgr::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_List.m_pFun = UpdateMeta;		// ��������ı��༭��������¼�
	m_List.m_param = this;

	InitListCtrlEx();

	InitDocLibList();

    //�ж�m_sAutoItem�Ƿ�Ϊ��,��Ϊ�����Զ�Ǩ���ĵ�
    if(!this->m_sAutoItem.IsEmpty())
    {
        int nItem = GetItemByKey(this->m_sAutoItem, ColTITLE);
        if(nItem >= 0)
            //::SendMessage(this->m_hWnd, WM_BN_CLICK, nItem, ColCHKOUT);
            ::PostMessage(this->m_hWnd, WM_BN_CLICK, nItem, ColCHKOUT);
    }
    return TRUE;
}

BOOL CDocMgr::InitConfigInfo(void)
{
	CString sCfgInfo(m_ConfigInfo);
	CString sCfgtok(L",");
	CStringArray resultCfgInfo;

	try{
        int iNum = ParseTokens( resultCfgInfo, sCfgInfo, sCfgtok);
	    if(iNum > 0)
	    {
		    m_WebSvrAddr = resultCfgInfo.GetAt(0);
		    m_TmpPath    = resultCfgInfo.GetAt(1);
		    m_MossSiteURL = resultCfgInfo.GetAt(2);
            m_Comment = resultCfgInfo.GetAt(3);

            //m_UploadURL  = resultCfgInfo.GetAt(2);
		    m_List.m_DefaultURLAddr = m_UploadURL;
            
	    }
    	
	    m_WebServicesRef.m_TmpPath = m_MossSiteURL;//m_UploadURL;

    				
	    if(!CreateDirectory(m_TmpPath, NULL)) {
		    /*MessageBox(L"������ʱĿ¼ʧ��");
		    return 1;*/
	    }
	}
	catch(...)
	{
        CString str("��ʼ��ʧ��");
        MessageBox(str);
		return FALSE;	// Ŀ¼��Ч
	}

	int iRet = -1;
	BSTR sMessage = L"";

    //m_WebServicesRef.TestWebSvrRefAddr(m_WebSvrAddr);

	//TODO: ��֤WEBSERVICES�Ƿ���ȷ
    if(!m_WebServicesRef.TestWebSvrRefAddr(m_WebSvrAddr))
	{
		MessageBox(L"WebServices���õ�ַ��Ч");
		return FALSE;
	}
	
	/*CString sWSDLFile(m_WebSvrAddr);
	sWSDLFile.Append(L"?wsdl");
	m_WebServicesRef.Init(sWSDLFile);*/		// ��ʼ��SOAP CLIENT�ʻ��������ʱ����

	return TRUE;
}

void CDocMgr::InitListCtrlEx(void)
{
	CImageList img;
	img.Create(1, 21, ILC_COLOR8|ILC_MASK,2,2);
	img.Add(AfxGetApp()->LoadIcon(IDR_MAINFRAME));
	m_List.SetImageList(&img,LVSIL_SMALL);
	m_List.SetExtendedStyle( LVS_EX_GRIDLINES|LVS_EX_FULLROWSELECT );
	m_List.InsertColumn( ColALIAS, _T("����"),LVCFMT_LEFT,120);
	m_List.InsertColumn( ColENCODE, _T("����"),LVCFMT_LEFT,120);
	m_List.InsertColumn( ColPAGES, _T("ҳ��"),LVCFMT_LEFT,120); // iPage
    m_List.InsertColumn( ColSIZE, _T("Size"), LVCFMT_LEFT,0);
	m_List.InsertColumn( ColTYPE, _T("Type"),LVCFMT_LEFT,0);
	m_List.InsertColumn( ColTITLE, _T("Title"),LVCFMT_LEFT,0);
	//m_List.InsertColumn( ColUPDATED, _T("Updated"),LVCFMT_LEFT,0);
	m_List.InsertColumn( ColEDITION, _T("Edition"),LVCFMT_LEFT,0);
	m_List.InsertColumn( ColURL, _T("URL"),LVCFMT_LEFT,0);
    m_List.InsertColumn( ColfURL, _T("fURL"), LVCFMT_LEFT, 0);
    m_List.InsertColumn( ColProcType, _T("ProcType"), LVCFMT_LEFT, 0);
    m_List.InsertColumn( ColContent, _T("Content"), LVCFMT_LEFT, 0);
    //m_List.InsertColumn( ColMossSiteURL, _T("MossSiteURL"), LVCFMT_LEFT, 0);
    m_List.InsertColumn( ColFolderName, _T("FolderName"), LVCFMT_LEFT, 0);
    m_List.InsertColumn( ColFileName, _T("FileName"), LVCFMT_LEFT, 0);
    m_List.InsertColumn( ColIsCopy, _T("IsCopy"),LVCFMT_LEFT,0);
    m_List.InsertColumn( ColWorkItemID, _T("WorkItemID"),LVCFMT_LEFT,0);

	m_List.InsertColumn( ColCHKOUT, _T(""),LVCFMT_LEFT,50);		//ǩ����ť��
	m_List.InsertColumn( ColCHKIN, _T(""),LVCFMT_LEFT,50);		//ǩ�밴ť��
	//m_List.InsertColumn( ColDELETE, _T(""),LVCFMT_LEFT,50);		//ɾ����ť��
}

bool CDocMgr::ReadXmlFile(string& szFileName)
{
    //��ȡXml�ļ���������
    XMLRdr xml(szFileName);
    //string str(m_DocLst.GetBuffer(1)); 
    //m_DocLst.ReleaseBuffer();
    //xml.ProcessString(str);
    XMLNode *root = xml.GetRootNode();
    if(root == NULL) return false;
    long cnt = root->GetChildNodeCnt();
    
    for(int i=0;i<cnt;i++)
    {
        XMLNode *chdNode = root->GetNextChild();
        if(chdNode == NULL) return false;
        string sType = chdNode->GetElementValue("Type");
        string sAlias = chdNode->GetElementValue("Alias");
        string sTitle = chdNode->GetElementValue("Title");
        string sEncode = chdNode->GetElementValue("Encode");
        string sPage = chdNode->GetElementValue("iPage");
        string sSize = chdNode->GetElementValue("Size");
        //string sUpdated = chdNode->GetElementValue("Updated");
        string sEdition = chdNode->GetElementValue("Edition");
        string sURL = chdNode->GetElementValue("URL");
        string sfullURL = chdNode->GetElementValue("fullURL");
        string sProcessType = chdNode->GetElementValue("ProcessType");
        string sIsZhengWen = chdNode->GetElementValue("IsZhengWen");
        //string sMossSiteURL = chdNode->GetElementValue("MossSiteURL");
        string sFolderName = chdNode->GetElementValue("FolderName");
        string sFileName = chdNode->GetElementValue("FileName");

        string sIsCopy = chdNode->GetElementValue("IsCopy");
        string sWorkItemID = chdNode->GetElementValue("WorkItemID");

        CString csType(sType.c_str());
        CString csAlias(sAlias.c_str());
        CString csTitle(sTitle.c_str());
        CString csEncode(sEncode.c_str());
        CString csPage(sPage.c_str());
        CString csSize(sSize.c_str());
        //CString csUpdated(sUpdated.c_str());
        CString csEdition(sEdition.c_str());
        CString csURL(sURL.c_str());
        CString csfullURL(sfullURL.c_str());
        CString csProcessType(sProcessType.c_str());
        CString csIsZhengWen(sIsZhengWen.c_str());
        //CString csMossSiteURL(sMossSiteURL.c_str());
        CString csFolderName(sFolderName.c_str());
        CString csFileName(sFileName.c_str());

        CString csIsCopy(sIsCopy.c_str());
        CString csWorkItemID(sWorkItemID.c_str());

        InsertDocItem(csAlias, csEncode, csPage, csSize, csType, csTitle, 
                csEdition, csURL, csfullURL,csProcessType, 
                csIsZhengWen, csFolderName, csFileName, csIsCopy, csWorkItemID);
    }
	return true;
}

BOOL CDocMgr::InitDocLibList(void)
{
	if(!m_DocLst.GetLength())
	{
		return FALSE;
	}
 
    m_DocLst.Replace(L"><", L">\r\n<");

    size_t  strSize;
    FILE*   fileHandle;
    int length = m_DocLst.GetLength() + 128;
    char *tmp = new char[length];
    memset(tmp, 0, length);

    CString path = m_TmpPath;
    path.Append(L"\\D525B95A-5EC7-415e-A328-9B7270EC8654.xml");
    //Create an the xml file in text and Unicode encoding mode.
    if ((fileHandle = _wfopen(path ,L"w")) == NULL) // C4996
    // Note: _wfopen is deprecated; consider using _wfopen_s instead
    {        
        delete tmp;
        return FALSE;
    }
	WideCharToMultiByte(CP_OEMCP, 0, m_DocLst, -1, tmp, length, NULL, NULL);
    strSize = strlen(tmp);
    if (fwrite(tmp, sizeof(char), strSize, fileHandle) != strSize)
    {
        delete tmp;
        fclose(fileHandle);
        return FALSE;
    }
    // Close the file.
    fclose(fileHandle);

    char cPath[1024];
    memset(cPath, 0, 1024);
    WideCharToMultiByte(CP_OEMCP, 0, path, -1, cPath, path.GetLength(), NULL, NULL);
    this->ReadXmlFile((string)cPath);
    delete tmp;

	return TRUE;
}

LRESULT CDocMgr::onBnCLick( WPARAM wParam, LPARAM lParam )
{
	// TODO: ��ɶ�ѡ���ĵ���[ǩ��][ǩ��][ɾ��]����
	int nItem = (int)wParam;
	int nSubItem = (int)lParam;
	nSubItem--;
	
#ifdef USE_TOPINDEX_BUTTON
	int iTopIndex = m_List.GetTopIndex();
	nItem = iTopIndex + nItem;
#endif
	DocItemEx docitem;
	GetItemsValue(nItem, &docitem);

	CFile file;
	
	CString FileInfo[4];
    FileInfo[0] = m_MossSiteURL;            //��һ���ǣ�����ʵ�����MossSiteURL����     ����(http://172.29.128.239/Docs )
    FileInfo[1] = docitem.m_ProcessType;    //�ڶ����ǣ�����ʵ�����ProcessType����     ����(��˾����)
    FileInfo[2] = docitem.m_FolderName;     //�������ǣ�����ʵ�����FolderName����      ����(200909)
    FileInfo[3] = docitem.m_FileName;       //���ĸ��ǣ�����ʵ�����FileName ����

	if(nSubItem == (ColCHKOUT - 1))
	{
        if(docitem.m_Type == "docx" || docitem.m_Type == "doc" || docitem.m_Type == "DOCX" || docitem.m_Type == "DOC")
        {
            m_WatchTrackRevision.m_pFun = (PFUN)(&CDocMgr::WatchTrackThread);
            m_WatchTrackRevision.m_pFunParam = (LPVOID)this;
            m_WatchTrackRevision.CreateWorder();

            SetEditStatus(TRUE);
        }
		//TODO: ǩ������
		int iRet = -1;
		//BSTR sMessage = L"";

		ATLSOAP_BLOB SoapBlob;
        SoapBlob.data = NULL;
        SoapBlob.size = 0;

        try{
            if(!m_WebServicesRef.Download(FileInfo[0], FileInfo[1], FileInfo[2], FileInfo[3], SoapBlob))
		    {
			    MessageBox(L"ǩ������ʧ��");
			    return 1;
		    }

		    CComBSTR pathfile;
		    pathfile.Append(m_TmpPath);
		    pathfile.Append(L"\\");
		    pathfile.Append(docitem.m_Title);
		    //pathfile.Append(L".");
		    //pathfile.Append(docitem.m_Type);

		    if(file.Open(pathfile, CFile::modeCreate|CFile::modeWrite))
		    {
			    m_DocOutList.AddDocChkOuted(docitem);
			    file.Write(SoapBlob.data, SoapBlob.size);
			    file.Close();

                /*if(!this->m_sAutoItem.IsEmpty())
                {
                    CoInitialize(0);
                    IShellDispatch4 * pdisp=NULL;
                    CoCreateInstance(CLSID_Shell,NULL,CLSCTX_ALL,__uuidof(IShellDispatch4),(void **)&pdisp);
                    pdisp->ToggleDesktop();
                    pdisp->Release();
                    CoUninitialize();
                }*/

                m_List.enableButton(FALSE, nItem, ColCHKOUT);
			    m_List.enableButton(TRUE, nItem, ColCHKIN);
			    //m_List.enableButton(FALSE, nItem, ColDELETE);

			    HINSTANCE iRet = ShellExecute(this->m_hWnd, NULL, pathfile, L"", m_TmpPath, SW_MAXIMIZE/*SW_SHOWMAXIMIZED*/ );
			    if((int)iRet == SE_ERR_NOASSOC)
			    {
				    TCHAR szOpen[2 * MAX_PATH] = { 0 };
    				
				    swprintf(szOpen, _T("shell32.dll,OpenAs_RunDLL \"%s\""), pathfile);
				    ShellExecute(NULL,  NULL,  _T("rundll32.exe"), szOpen,  NULL,  SW_MAXIMIZE/*SW_SHOWMAXIMIZED*/ ); 
			    }
                // �Ƿ�ΪWORD�ĵ�,������򿪺ۼ���������,
                // ͬʱ����һ������߳�,����û��Ƿ�رպۼ���������
                //EnableTrackRevision(docitem.m_Type, TRUE);
                if(!this->m_sAutoItem.IsEmpty()) {
                    //HWND hWnd1 = ::FindWindow(NULL, L"��ɽ���˰칫�ĵ�һ�廯ϵͳ - Windows Internet Explorer");
                    HWND hWnd2 = ::FindWindow(NULL, L"���߱༭ v2.6.6.0");
                    if(hWnd2 != NULL) {
                       // ::ShowWindow(hWnd1, SW_MINIMIZE);
                        ::ShowWindow(hWnd2, SW_HIDE);
                        ::ShowWindow(hWnd2, SW_SHOWNOACTIVATE);
                    }
                }
		    }
            delete SoapBlob.data;
            SoapBlob.data = NULL;
            SoapBlob.size = 0;
		    return 0;
        }
        catch(...)
        {
            delete SoapBlob.data;
            SoapBlob.data = NULL;
            SoapBlob.size = 0;
            return 1;

            char val[10];
            memset(val, 0, 10);
            _itoa(SoapBlob.size, val, 10);
            ::MessageBox(NULL, (LPCWSTR)val, L"", MB_OK);
        }
	}
	if(nSubItem == (ColCHKIN - 1))
	{
		//TODO: ǩ�����
        PBYTE pbyte = NULL;
        int iRet = -1;
		try{
			//BSTR sMessage = L"";
			CComBSTR pathfile;
			pathfile.Append(m_TmpPath);
			pathfile.Append(L"\\");
			pathfile.Append(docitem.m_Title);
			//pathfile.Append(L".");
			//pathfile.Append(docitem.m_Type);
			ULONGLONG fleSize = 0;
			if(file.Open(pathfile, CFile::modeRead))
			{
				fleSize = file.GetLength();
				pbyte = (PBYTE)malloc( fleSize * sizeof(byte));
				ATLSOAP_BLOB SoapBlob;
				SoapBlob.data = pbyte;
				SoapBlob.size = fleSize;
				file.Read(SoapBlob.data, SoapBlob.size);
				file.Close();
				/*if(!m_WebServicesRef.CheckIn(URL_Title, SoapBlob))
				{
					MessageBox(L"ǩ�����ʧ��");
				}*/
                if(!m_WebServicesRef.Upload(FileInfo[0], FileInfo[1], FileInfo[2], FileInfo[3], SoapBlob))
				{
					MessageBox(L"ǩ�����ʧ��");
                    free(pbyte);
                    return 1;
				}
                if(pbyte != NULL) {
				    free(pbyte);
                    pbyte = NULL;
                }
				m_DocOutList.DelDocChkOuted(docitem);
				//m_List.SetItemText(nItem, ColUPDATED, L"1");

				m_List.enableButton(TRUE,  nItem, ColCHKOUT);
				m_List.enableButton(FALSE, nItem, ColCHKIN);
				//m_List.enableButton(TRUE,  nItem, ColDELETE);
			}
            else
            {
                MessageBox(L"����ǩ�����ļ���δ�ر�", L"��ʾ");
            }
		}
		catch(...)
		{
			MessageBox(L"����ǩ�����ļ���δ�ر�", L"��ʾ");
            if(pbyte != NULL) {
			    free(pbyte);
                pbyte = NULL;
            }
			return 1;
		}
		return 0;
	}
	//if(nSubItem == (ColDELETE - 1))
	//{
	//	//TODO: ɾ������
	//	if(m_WebServicesRef.Delete(URL_Title))
	//	{
	//		m_List.deleteItemEx(nItem);
	//	}
	//	else
	//	{
	//		MessageBox(L"ɾ������ʧ��");
	//	}
	//	return 0;
	//}
	return 0;
}
void CDocMgr::OnBnClickedOk()
{
	if(!PreClsWndHandleDocsChkOuted()) return;
	
	GenDocLibListEx();
	UnInitDialog();

    SetEditStatus(FALSE);
	OnOK();
}

void CDocMgr::OnBnClickedCancel()
{
	if(!PreClsWndHandleDocsChkOuted()) return;

	GenDocLibListEx();
	UnInitDialog();

    SetEditStatus(FALSE);
	OnCancel();
}

int CDocMgr::ParseTokens(CStringArray& result , CString szString, CString szTokens)
{
	int iNum = 0;

      int iCurrPos= 0;
      CString subString;

      while( -1 != ( iCurrPos = szString.FindOneOf( szTokens ) ) )
      {
            iNum++;
            result.Add( szString.Left( iCurrPos ) );
            szString = szString.Right( szString.GetLength() - iCurrPos - 1 );
      }

      if ( szString.GetLength() > 0 )
      {
            // the last one...
            iNum++;
            result.Add( szString );
      }

      return iNum;
}

int CDocMgr::InsertDocItem(CString& sAlias, CString& sEncode, CString& sPage, CString& sSize, 
						   CString& sType, CString& sTitle, CString& sEdition, CString& sURL,
						   CString& sfullURL, CString& sProcessType, CString& sIsZhengWen, 
                           CString& sFolderName, CString& sFileName, CString& sIsCopy, CString& sWorkItemID )
{
	int iLine = m_List.GetItemCount();
	CString szItemText;
    CString tsAlias(sAlias);
    tsAlias.Replace(L"%", L"%%");
	szItemText.Format( tsAlias,iLine );
	m_List.InsertItem( iLine, szItemText );

	m_List.SetItemText( iLine, ColENCODE, sEncode );
    m_List.SetItemText( iLine, ColPAGES, sPage);
	m_List.SetItemText( iLine, ColSIZE, sSize );
	m_List.SetItemText( iLine, ColTYPE, sType );
	m_List.SetItemText( iLine, ColTITLE, sTitle );
	//m_List.SetItemText( iLine, ColUPDATED, sUpdated );
	m_List.SetItemText( iLine, ColEDITION, sEdition );
	m_List.SetItemText( iLine, ColURL, sURL );
    m_List.SetItemText( iLine, ColfURL, sfullURL );
    m_List.SetItemText( iLine, ColProcType, sProcessType );
    m_List.SetItemText( iLine, ColContent, sIsZhengWen );
    //m_List.SetItemText( iLine, 11, sMossSiteURL );
    m_List.SetItemText( iLine, ColFolderName , sFolderName );
    m_List.SetItemText( iLine, ColFileName, sFileName );

    m_List.SetItemText( iLine, ColIsCopy , sIsCopy );
    m_List.SetItemText( iLine, ColWorkItemID , sWorkItemID );
    
	m_List.createItemButton( iLine, ColCHKOUT, this->GetSafeHwnd(), L"ǩ��" );
	m_List.createItemButton( iLine, ColCHKIN, this->GetSafeHwnd(),  L"ǩ��" );
	//m_List.createItemButton( iLine, ColDELETE, this->GetSafeHwnd(), L"ɾ��" );

	m_List.enableButton(FALSE, iLine, ColCHKIN);
	return 0;
}

bool UpdateMeta(PVOID param, BSTR sURL, BSTR sValue, int iSubItem)
{
	if(NULL == param) return FALSE;
	CComBSTR csValue(sValue);
	// ��֤ALIAS�ֶ���Ч��
	if(iSubItem == ColALIAS) {
		
		if(csValue.Length() == 0) {
			MessageBox(NULL, L"���Ʋ���Ϊ��", L"", MB_OK);
			return FALSE;
		}
		
		CString tmp(csValue);
		BOOL itemAllSpace = TRUE;
		for(int i=0;i<tmp.GetLength();i++) 
		{
			if(tmp.GetAt(i) == L' ') 
			{
				continue;
			}
			else
			{
				itemAllSpace = FALSE;
				break;
			}
		}
		if(itemAllSpace) {
			MessageBox(NULL, L"���Ʋ���ȫΪ�ո�", L"", MB_OK);
			return FALSE;
		}
	}

	// ��֤SIZE�ֶ���Ч��

	if(iSubItem == ColSIZE) {
		if(csValue.Length() == 0) sValue = L"0";
		long lValue = 0L;
		try {
			lValue = _wtol(sValue);
		}
		catch(...)
		{
			MessageBox(NULL, L"����ֵ��Ч������������", L"", MB_OK);
			return FALSE;
		}
		if (lValue > 65535) {
			MessageBox(NULL, L"����ֵ������Ч��Χ��65535��������������", L"", MB_OK);
			return FALSE;
		}
	}

	/*if(!((CDocMgr*)param)->m_WebServicesRef.UpdataMeta(sURL, sValue, iSubItem))
	{
		MessageBox(NULL, L"�����ĵ�������ʧ��", L"", MB_OK);
		return FALSE;
	}*/
	return TRUE;
}

BOOL CDocMgr::UnInitDialog(void)
{
	//TODO: �������
	m_List.DeleteAllItems();
	m_List.deleteAllItemsEx();
    m_List.release();
	for(int i=0;i<DATACOLS;i++)       //���� COLS == 15
	{
		m_List.DeleteColumn(i);
	}
	m_List.m_mBtnChkIn.clear();
	m_List.m_mBtnChkOut.clear();
	//m_List.m_mButton.clear();
	m_DocOutList.ClearAll();

	return 0;
}

// �ر�ģ̬�Ի���ǰ�������Ѿ�ǩ�����ĵ�
BOOL CDocMgr::PreClsWndHandleDocsChkOuted(void)
{
	// TODO: ȷ�����ĵ��Ĳ���
	// ����δǩ����ĵ�ѯ���Ƿ��ǩ��ݻ��ȡ��ǩ����
	// �������ĵ����б��ַ��������÷�
	if(m_DocOutList.GetLength()) {
		if(IDYES == ::MessageBox(this->m_hWnd, L"����δǩ����ĵ����Ƿ���ǩ��", L"��ʾ", MB_YESNO)) 
		{
			// ���У۳���ǩ���ݲ���
			for(int i=0;i<m_DocOutList.GetLength();i++)
			{
				DocItemEx item = m_DocOutList.GetDocItem(i);
				m_DocOutList.DelDocChkOuted(item);
			}
			return TRUE;			// �۳���ǩ���ݲ����ɹ�
		}
		else 
        {
			return FALSE;			// ȡ���۳���ǩ���ݲ������û��ֶ�ǩ���ĵ�
		}
	}
	else 
    {
		return TRUE;				// �����ڻ�δǩ����ĵ�
	}
}
void CDocMgr::GenDocLibListEx(void)
{
    //string sTmp;
    /*xmlwriter xmlwr(sTmp);
    xmlwr.Createtag(_T("attach"));
	for(int i=0; i<m_List.GetItemCount(); i++)
    {
	    xmlwr.Createtag(_T("item"));
	    xmlwr.CreateChild(_T("Type"),m_List.GetItemText(i, 3));
	    xmlwr.CreateChild(_T("Alias"),m_List.GetItemText(i, 0));
	    xmlwr.CreateChild(_T("Title"),m_List.GetItemText(i, 4));
        xmlwr.CreateChild(_T("Encode"),m_List.GetItemText(i, 1));
        xmlwr.CreateChild(_T("Size"),m_List.GetItemText(i, 2));
        xmlwr.CreateChild(_T("Updated"),m_List.GetItemText(i, 5));
        xmlwr.CreateChild(_T("Edition"),m_List.GetItemText(i, 6));
        xmlwr.CreateChild(_T("URL"),m_List.GetItemText(i, 7));
        xmlwr.CreateChild(_T("fullURL"),m_List.GetItemText(i, 8));
        xmlwr.CreateChild(_T("ProcessType"),m_List.GetItemText(i, 9));
        xmlwr.CreateChild(_T("IsZhengWen"),m_List.GetItemText(i, 10));
        xmlwr.CreateChild(_T("MossSiteURL"),m_List.GetItemText(i, 11));
        xmlwr.CreateChild(_T("FolderName"),m_List.GetItemText(i, 12));
        xmlwr.CreateChild(_T("FileName"),m_List.GetItemText(i, 13));
	    xmlwr.CloseLasttag(); 
    }
	xmlwr.CloseLasttag();
    xmlwr.CloseAlltags();

    this->m_DocLst = xmlwr.fp->_base;*/
    CString tmp(L"<?xml version='1.0' encoding='utf-8'?><����>");
    for(int i=0; i<m_List.GetItemCount(); i++)
    {
        tmp.Append(L"<item>");
        tmp.Append(L"<Type>");tmp.Append(m_List.GetItemText(i, ColTYPE));tmp.Append(L"</Type>");
        tmp.Append(L"<Alias>");tmp.Append(m_List.GetItemText(i, ColALIAS));tmp.Append(L"</Alias>");
        tmp.Append(L"<Title>");tmp.Append(m_List.GetItemText(i, ColTITLE));tmp.Append(L"</Title>");
        tmp.Append(L"<Encode>");tmp.Append(m_List.GetItemText(i, ColENCODE));tmp.Append(L"</Encode>");
        tmp.Append(L"<iPage>");tmp.Append(m_List.GetItemText(i, ColPAGES));tmp.Append(L"</iPage>");
        tmp.Append(L"<Size>");tmp.Append(m_List.GetItemText(i, ColSIZE));tmp.Append(L"</Size>");
        //tmp.Append(L"<Updated>");tmp.Append(m_List.GetItemText(i, ColUPDATED));tmp.Append(L"</Updated>");
        tmp.Append(L"<Edition>");tmp.Append(m_List.GetItemText(i, ColEDITION));tmp.Append(L"</Edition>");
        tmp.Append(L"<URL>");tmp.Append(m_List.GetItemText(i, ColURL));tmp.Append(L"</URL>");
        tmp.Append(L"<fullURL>");tmp.Append(m_List.GetItemText(i, ColfURL));tmp.Append(L"</fullURL>");
        tmp.Append(L"<ProcessType>");tmp.Append(m_List.GetItemText(i, ColProcType));tmp.Append(L"</ProcessType>");
        tmp.Append(L"<IsZhengWen>");tmp.Append(m_List.GetItemText(i, ColContent));tmp.Append(L"</IsZhengWen>");
        //tmp.Append(L"<MossSiteURL>");tmp.Append(m_List.GetItemText(i, ColMossSiteURL));tmp.Append(L"</MossSiteURL>");
        tmp.Append(L"<FolderName>");tmp.Append(m_List.GetItemText(i, ColFolderName));tmp.Append(L"</FolderName>");
        tmp.Append(L"<FileName>");tmp.Append(m_List.GetItemText(i, ColFileName));tmp.Append(L"</FileName>");
        tmp.Append(L"<IsCopy>");tmp.Append(m_List.GetItemText(i, ColIsCopy));tmp.Append(L"</IsCopy>");
        tmp.Append(L"<WorkItemID>");tmp.Append(m_List.GetItemText(i, ColWorkItemID));tmp.Append(L"</WorkItemID>");
        tmp.Append(L"</item>");
    }
    tmp.Append(L"</����>");
    this->m_DocLst.Empty();
    this->m_DocLst.Append(tmp);
}

// �����ĵ����ַ���
void CDocMgr::GenDocLibList(void)
{
	int iLine = m_List.GetItemCount();
	CString str;
	for(int i=0;i<iLine;i++)
	{
		str.Append(m_List.GetItemText(i, 3));	// TYPE
		str.Append(L",");
		str.Append(m_List.GetItemText(i, 0));	// Alias
		str.Append(L",");
		str.Append(m_List.GetItemText(i, 4));	// Title
		str.Append(L",");
		str.Append(m_List.GetItemText(i, 1));	// Encode
		str.Append(L",");
		str.Append(m_List.GetItemText(i, 2));	// Size
		str.Append(L",");
		

		str.Append(m_List.GetItemText(i, 5));	// Updated
		str.Append(L",");
		str.Append(m_List.GetItemText(i, 6));	// Edition
		str.Append(L",");
		str.Append(m_List.GetItemText(i, 7));	// URL
		str.Append(L";");
	}
	m_DocLst.Empty();
	m_DocLst.Append(str);
}


void CDocMgr::GetItemsValue(int nItem, DocItemEx* item)
{
	item->m_Alias = m_List.GetItemText(nItem, ColALIAS);
	item->m_Encode = m_List.GetItemText(nItem, ColENCODE);
    item->m_Page = m_List.GetItemText(nItem, ColPAGES);
	item->m_Size = m_List.GetItemText(nItem, ColSIZE);
	item->m_Type = m_List.GetItemText(nItem, ColTYPE);
	item->m_Title = m_List.GetItemText(nItem, ColTITLE);
	//item->m_Updated = m_List.GetItemText(nItem, ColUPDATED);
	item->m_Edition = m_List.GetItemText(nItem, ColEDITION);
	item->m_URL = m_List.GetItemText(nItem, ColURL);
    item->m_fullURL = m_List.GetItemText(nItem, ColfURL);
    item->m_ProcessType = m_List.GetItemText(nItem, ColProcType);
    item->m_IsZhengWen = m_List.GetItemText(nItem, ColContent);
    //item->m_MossSiteURL = m_List.GetItemText(nItem, ColMossSiteURL);
    item->m_FolderName = m_List.GetItemText(nItem, ColFolderName);
    item->m_FileName = m_List.GetItemText(nItem, ColFileName);

    item->m_IsCopy = m_List.GetItemText(nItem, ColIsCopy);
    item->m_WorkItemID = m_List.GetItemText(nItem, ColWorkItemID);
    
}

int CDocMgr::EnableTrackRevision(CString& DocType, bool Enable)
{
    if(DocType == DOCX || DocType == DOC)
    {
        ::CoInitialize(NULL);

        CLSID clsid;
	    HRESULT hr;
        
        //hr = ::CLSIDFromString(L"{000209FF-0000-0000-C000-000000000046}", &clsid); 
	    hr=CLSIDFromProgID(L"Word.Application", &clsid);	//ͨ��ProgIDȡ��CLSID
	    if(FAILED(hr))
	    {
		    AfxMessageBox(_T("δ��װOFFICE"));
            ::CoUninitialize();
		    return ERR_EDITIN;
	    }
    	
	    IUnknown *pUnknown = NULL;
	    //IDispatch *pDispatch = NULL;

	    _Application app = NULL;
	    _Document doc = NULL;
        CDocuments docs = NULL;

        //DWORD dwRegister = 0;
        while(TRUE)
        {
            /*hr = ::RegisterActiveObject(pUnknown, clsid, ACTIVEOBJECT_WEAK, &dwRegister);
            CoLockObjectExternal(pUnknown, TRUE, TRUE);*/
            
            hr = ::GetActiveObject(clsid, NULL, &pUnknown);	//�����Ƿ���WORD����������
            if(FAILED(hr))
            {
	            //AfxMessageBox(_T("û�����������е�WORDӦ�ó���"));
                if(!GetEditStatus()) break;
                continue;
            }

		    hr = pUnknown->QueryInterface(IID_IDispatch, (LPVOID *)&app);
            if( FAILED(hr) ) {
		        pUnknown->Release();	
                pUnknown = NULL;
                continue;
            }
            
            docs = app.get_Documents();
            if(!docs) continue;
            if(docs.get_Count() == 0) continue;//return REMAINEDITIN;

            if(app.get_UserName() != m_Comment){
                app.put_UserName(m_Comment);
            }
            if(app.get_UserInitials() != m_Comment) {
                app.put_UserInitials(m_Comment);
            }

            for(int i=1;i<=docs.get_Count();i++)
            {
                CComVariant var(i);
                doc = docs.Item(&var);
                
                CString fullName = doc.get_FullName();
                CString Path = doc.get_Path();
                CString sTemp(Path);
                sTemp.Replace(L"\\", L"\\\\");
                if(Path == m_TmpPath || sTemp == m_TmpPath) 
                {
                    if(!doc.get_TrackRevisions())
                    {
                        doc.put_TrackRevisions(Enable);
                    }
                    if(!doc.get_ShowRevisions())
                    {
                        doc.put_ShowRevisions(Enable);
                    }
                }
            }

            if(!GetEditStatus()) break;
            ::Sleep((DWORD)5000);
        }

/*FINISHED:*/
        if(pUnknown)	pUnknown->Release();
        if(doc) doc.ReleaseDispatch();
        if(docs) docs.ReleaseDispatch();
        if(app) app.ReleaseDispatch();

        //RevokeActiveObject(dwRegister, NULL);
        ::CoUninitialize();
        return EXITEDITIN;
    }
    else
    {
        return EXITEDITIN;
    }
}

void CDocMgr::SetEditStatus(bool status)
{
    try{
        m_cs.Enter();
        m_bExitEditOnline = status;
        m_cs.Leave();
    }
    catch(...)
    {
        m_cs.Leave();
    }
}

BOOL CDocMgr::GetEditStatus(void)
{
    BOOL bret = FALSE;
    try{
        m_cs.Enter();
        bret = m_bExitEditOnline;
        m_cs.Leave();
        return bret;
    }
    catch(...)
    {
        m_cs.Leave();
        return bret;
    }
}

int CDocMgr::GetItemByKey(CString& sKey, int iCol) const
{
    int ilength = m_List.GetItemCount();
    for(int i = 0;i<ilength;i++)
    {
        CString sItemText = m_List.GetItemText(i, iCol/*ColTITLE*/);
        if(sItemText == sKey)
        {
            return i;
        }
    }
    return -1;
}
