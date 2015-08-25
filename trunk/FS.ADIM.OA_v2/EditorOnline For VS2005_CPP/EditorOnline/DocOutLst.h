#pragma once

//������DocItem
//��������������һ��ǩ���ĵ���Ϣ���ֶ���Ϣ�������ĵ����ƣ�sName����URL��ַ��sUR�����ĵ����ͣ�sType�����ĵ���С��sSize����
class DocItemEx
{
public:
	explicit DocItemEx()
		: m_Alias(L""), m_Encode(L""),  m_Size(L""), m_Type(L""), m_Title(L""), m_Edition(L"")
	{};
	explicit DocItemEx(CString sAlias, CString sEncode, CString sSize, CString sType, 
			CString sTitle, CString sUpdated, CString sEdition)
		: m_Alias(sAlias), m_Encode(sEncode),  m_Size(sSize), m_Type(sType), 
		m_Title(sTitle), m_Edition(sEdition)
	{};

	DocItemEx(DocItemEx& docitem)
	{
		m_Alias = docitem.m_Alias;
		m_Encode = docitem.m_Encode;
		m_Size = docitem.m_Size;
		m_Type = docitem.m_Type;
		m_Title = docitem.m_Title;
		//m_Updated = docitem.m_Updated;
		m_Edition = docitem.m_Edition;
	};

	DocItemEx& operator =(DocItemEx& docitem)
	{
		m_Alias = docitem.m_Alias;
		m_Encode = docitem.m_Encode;
		m_Size = docitem.m_Size;
		m_Type = docitem.m_Type;
		m_Title = docitem.m_Title;
		//m_Updated = docitem.m_Updated;
		m_Edition = docitem.m_Edition;
		return *this;
	};

public:
	CString m_Alias;
	CString m_Encode;
	CString m_Size;
	CString m_Page;
    CString m_Type;
	CString m_Title;
	//CString m_Updated;
	CString m_Edition;
	CString m_URL;
    CString m_fullURL;
    CString m_ProcessType;
    CString m_IsZhengWen;
    //CString m_MossSiteURL;
    CString m_FolderName;
    CString m_FileName;

    CString m_IsCopy;
    CString m_WorkItemID;
};


// ������CDocOutLst
// ����������CDocOutLst�����ڹ����ڱ����û�ǩ��ǩ���ĵ���¼��
//			��ǩ���ĵ������غ���Ӽ�¼�������У�ǩ���ĵ���ɾ�������б���ļ�¼��Ϣ
class CDocOutLst
{
public:
	CDocOutLst(void);
	~CDocOutLst(void);

public:
	bool AddDocChkOuted(DocItemEx& item);						//���ǩ���ĵ���Ϣ
	bool DelDocChkOuted(DocItemEx& item);						//ɾ��ǩ���ĵ���Ϣ
	int  FindDocChkOuted(DocItemEx &item);						//�ҵ�ָ��ǩ���ĵ���Ϣ��������λ��
	void ClearAll();											//������м�¼��Ϣ
	INT_PTR  GetLength() const;									//��ȡ��¼��Ϣ����
	bool CompareItem(DocItemEx &item1, DocItemEx &item2);		//�Ƚ�����ǩ���ĵ���Ϣ�Ƿ���ͬ
	DocItemEx GetDocItem(int iIndex);							//��ȡָ������λ�õ�ǩ���ĵ���Ϣ
private:
	CArray<DocItemEx, DocItemEx&> m_Array;						//����ǩ���ĵ���Ϣ������ṹ
};

BOOL operator ==(DocItemEx& item1, DocItemEx& item2);