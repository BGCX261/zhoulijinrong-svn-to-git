#pragma once

//类名：DocItem
//功能描述：保存一条签出文档信息，字段信息包括，文档名称（sName），URL地址（sUR），文档类型（sType），文档大小（sSize）等
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


// 类名：CDocOutLst
// 功能描述：CDocOutLst类用于管理在本地用户签入签出文档记录，
//			当签出文档到本地后添加记录到该类中，签入文档后，删除该类中保存的记录信息
class CDocOutLst
{
public:
	CDocOutLst(void);
	~CDocOutLst(void);

public:
	bool AddDocChkOuted(DocItemEx& item);						//添加签出文档信息
	bool DelDocChkOuted(DocItemEx& item);						//删除签出文档信息
	int  FindDocChkOuted(DocItemEx &item);						//找到指定签出文档信息所在索引位置
	void ClearAll();											//清除所有记录信息
	INT_PTR  GetLength() const;									//获取记录信息长度
	bool CompareItem(DocItemEx &item1, DocItemEx &item2);		//比较两个签出文档信息是否相同
	DocItemEx GetDocItem(int iIndex);							//获取指定索引位置的签出文档信息
private:
	CArray<DocItemEx, DocItemEx&> m_Array;						//保存签出文档信息的数组结构
};

BOOL operator ==(DocItemEx& item1, DocItemEx& item2);