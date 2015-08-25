//
// sproxy.exe generated file
// do not modify this file
//
// Created: 05/14/2010@16:29:43
//

#pragma once


#if !defined(_WIN32_WINDOWS) && !defined(_WIN32_WINNT) && !defined(_WIN32_WCE)
#pragma message("warning: defining _WIN32_WINDOWS = 0x0410")
#define _WIN32_WINDOWS 0x0410
#endif

#include <atlsoap.h>

namespace DocumentLibrary
{

struct DictionaryEntry
{
	BSTR Key;
	BSTR Value;
};

template <typename TClient = CSoapSocketClientT<> >
class CDocumentLibraryT : 
	public TClient, 
	public CSoapRootHandler
{
protected:

	const _soapmap ** GetFunctionMap();
	const _soapmap ** GetHeaderMap();
	void * GetHeaderValue();
	const wchar_t * GetNamespaceUri();
	const char * GetServiceName();
	const char * GetNamespaceUriA();
	HRESULT CallFunction(
		void *pvParam, 
		const wchar_t *wszLocalName, int cchLocalName,
		size_t nItem);
	HRESULT GetClientReader(ISAXXMLReader **ppReader);

public:

	HRESULT __stdcall QueryInterface(REFIID riid, void **ppv)
	{
		if (ppv == NULL)
		{
			return E_POINTER;
		}

		*ppv = NULL;

		if (InlineIsEqualGUID(riid, IID_IUnknown) ||
			InlineIsEqualGUID(riid, IID_ISAXContentHandler))
		{
			*ppv = static_cast<ISAXContentHandler *>(this);
			return S_OK;
		}

		return E_NOINTERFACE;
	}

	ULONG __stdcall AddRef()
	{
		return 1;
	}

	ULONG __stdcall Release()
	{
		return 1;
	}

	CDocumentLibraryT(ISAXXMLReader *pReader = NULL)
		:TClient(_T("http://172.29.128.239/MossService/DocumentLibrary.asmx"))
	{
		SetClient(true);
		SetReader(pReader);
	}
	
	~CDocumentLibraryT()
	{
		Uninitialize();
	}
	
	void Uninitialize()
	{
		UninitializeSOAP();
	}	

	HRESULT Test(
		int* TestResult
	);

	HRESULT CheckIn(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		int* CheckInResult
	);

	HRESULT Remove(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		int* RemoveResult
	);

	HRESULT UpdateMeta(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		DictionaryEntry* deMeta, int deMeta_nSizeIs, 
		int* UpdateMetaResult
	);

	HRESULT CheckOut(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		int* CheckOutResult
	);

	HRESULT ConvertToDE(
		DictionaryEntry* entries, int entries_nSizeIs, 
		DictionaryEntry** ConvertToDEResult, int* ConvertToDEResult_nSizeIs
	);

	HRESULT UndoCheckOut(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		int* UndoCheckOutResult
	);

	HRESULT CopyToNew(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		BSTR* newFileInfo, int newFileInfo_nSizeIs, 
		bool overwrite, 
		BSTR** CopyToNewResult, int* CopyToNewResult_nSizeIs
	);

	HRESULT Download(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		ATLSOAP_BLOB* DownloadResult
	);

	HRESULT CopyTo(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		BSTR newFileName, 
		bool overwrite, 
		BSTR** CopyToResult, int* CopyToResult_nSizeIs
	);

	HRESULT Upload(
		BSTR* fileInfo, int fileInfo_nSizeIs, 
		ATLSOAP_BLOB bStream, 
		DictionaryEntry* deMeta, int deMeta_nSizeIs, 
		bool overwrite, 
		BSTR** UploadResult, int* UploadResult_nSizeIs
	);
};

typedef CDocumentLibraryT<> CDocumentLibrary;

__if_not_exists(__DictionaryEntry_entries)
{
extern __declspec(selectany) const _soapmapentry __DictionaryEntry_entries[] =
{
	{ 
		0x00014C89, 
		"Key", 
		L"Key", 
		sizeof("Key")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_FIELD | SOAPFLAG_NULLABLE, 
		offsetof(DictionaryEntry, Key),
		NULL, 
		NULL, 
		-1 
	},
	{ 
		0x064B46FD, 
		"Value", 
		L"Value", 
		sizeof("Value")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_FIELD | SOAPFLAG_NULLABLE, 
		offsetof(DictionaryEntry, Value),
		NULL, 
		NULL, 
		-1 
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __DictionaryEntry_map =
{
	0xF92B6EE8,
	"DictionaryEntry",
	L"DictionaryEntry",
	sizeof("DictionaryEntry")-1,
	sizeof("DictionaryEntry")-1,
	SOAPMAP_STRUCT,
	__DictionaryEntry_entries,
	sizeof(DictionaryEntry),
	2,
	-1,
	SOAPFLAG_NONE,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};
}

struct __CDocumentLibrary_Test_struct
{
	int TestResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_Test_entries[] =
{

	{
		0x12321ADF, 
		"TestResult", 
		L"TestResult", 
		sizeof("TestResult")-1, 
		SOAPTYPE_INT, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_Test_struct, TestResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_Test_map =
{
	0x66CB6EEF,
	"Test",
	L"TestResponse",
	sizeof("Test")-1,
	sizeof("TestResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_Test_entries,
	sizeof(__CDocumentLibrary_Test_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_CheckIn_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	int CheckInResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_CheckIn_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CheckIn_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CheckIn_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xBF61D734, 
		"CheckInResult", 
		L"CheckInResult", 
		sizeof("CheckInResult")-1, 
		SOAPTYPE_INT, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_CheckIn_struct, CheckInResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_CheckIn_map =
{
	0x1EDB9484,
	"CheckIn",
	L"CheckInResponse",
	sizeof("CheckIn")-1,
	sizeof("CheckInResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_CheckIn_entries,
	sizeof(__CDocumentLibrary_CheckIn_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_Remove_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	int RemoveResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_Remove_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Remove_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_Remove_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0x91BA0C2D, 
		"RemoveResult", 
		L"RemoveResult", 
		sizeof("RemoveResult")-1, 
		SOAPTYPE_INT, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_Remove_struct, RemoveResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_Remove_map =
{
	0xE814EBBD,
	"Remove",
	L"RemoveResponse",
	sizeof("Remove")-1,
	sizeof("RemoveResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_Remove_entries,
	sizeof(__CDocumentLibrary_Remove_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_UpdateMeta_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	DictionaryEntry *deMeta;
	int __deMeta_nSizeIs;
	int UpdateMetaResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_UpdateMeta_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_UpdateMeta_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_UpdateMeta_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xF0937FF0, 
		"deMeta", 
		L"deMeta", 
		sizeof("deMeta")-1, 
		SOAPTYPE_UNK, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_UpdateMeta_struct, deMeta),
		NULL,
		&__DictionaryEntry_map,
		2+1,
	},
	{
		0xF0937FF0,
		"__deMeta_nSizeIs",
		L"__deMeta_nSizeIs",
		sizeof("__deMeta_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_UpdateMeta_struct, __deMeta_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xAC6FFD09, 
		"UpdateMetaResult", 
		L"UpdateMetaResult", 
		sizeof("UpdateMetaResult")-1, 
		SOAPTYPE_INT, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_UpdateMeta_struct, UpdateMetaResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_UpdateMeta_map =
{
	0x880A8399,
	"UpdateMeta",
	L"UpdateMetaResponse",
	sizeof("UpdateMeta")-1,
	sizeof("UpdateMetaResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_UpdateMeta_entries,
	sizeof(__CDocumentLibrary_UpdateMeta_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_CheckOut_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	int CheckOutResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_CheckOut_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CheckOut_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CheckOut_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xE83FF975, 
		"CheckOutResult", 
		L"CheckOutResult", 
		sizeof("CheckOutResult")-1, 
		SOAPTYPE_INT, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_CheckOut_struct, CheckOutResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_CheckOut_map =
{
	0xF7CB4B05,
	"CheckOut",
	L"CheckOutResponse",
	sizeof("CheckOut")-1,
	sizeof("CheckOutResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_CheckOut_entries,
	sizeof(__CDocumentLibrary_CheckOut_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_ConvertToDE_struct
{
	DictionaryEntry *entries;
	int __entries_nSizeIs;
	DictionaryEntry *ConvertToDEResult;
	int __ConvertToDEResult_nSizeIs;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_ConvertToDE_entries[] =
{

	{
		0x67C4D71A, 
		"entries", 
		L"entries", 
		sizeof("entries")-1, 
		SOAPTYPE_UNK, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_ConvertToDE_struct, entries),
		NULL,
		&__DictionaryEntry_map,
		0+1,
	},
	{
		0x67C4D71A,
		"__entries_nSizeIs",
		L"__entries_nSizeIs",
		sizeof("__entries_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_ConvertToDE_struct, __entries_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xDA450AAC, 
		"ConvertToDEResult", 
		L"ConvertToDEResult", 
		sizeof("ConvertToDEResult")-1, 
		SOAPTYPE_UNK, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_ConvertToDE_struct, ConvertToDEResult),
		NULL,
		&__DictionaryEntry_map,
		2+1,
	},
	{
		0xDA450AAC,
		"__ConvertToDEResult_nSizeIs",
		L"__ConvertToDEResult_nSizeIs",
		sizeof("__ConvertToDEResult_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_ConvertToDE_struct, __ConvertToDEResult_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_ConvertToDE_map =
{
	0x7F5985FC,
	"ConvertToDE",
	L"ConvertToDEResponse",
	sizeof("ConvertToDE")-1,
	sizeof("ConvertToDEResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_ConvertToDE_entries,
	sizeof(__CDocumentLibrary_ConvertToDE_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_UndoCheckOut_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	int UndoCheckOutResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_UndoCheckOut_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_UndoCheckOut_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_UndoCheckOut_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0x9FF3CD6B, 
		"UndoCheckOutResult", 
		L"UndoCheckOutResult", 
		sizeof("UndoCheckOutResult")-1, 
		SOAPTYPE_INT, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_UndoCheckOut_struct, UndoCheckOutResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_UndoCheckOut_map =
{
	0x6BC3F47B,
	"UndoCheckOut",
	L"UndoCheckOutResponse",
	sizeof("UndoCheckOut")-1,
	sizeof("UndoCheckOutResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_UndoCheckOut_entries,
	sizeof(__CDocumentLibrary_UndoCheckOut_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_CopyToNew_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	BSTR *newFileInfo;
	int __newFileInfo_nSizeIs;
	bool overwrite;
	BSTR *CopyToNewResult;
	int __CopyToNewResult_nSizeIs;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_CopyToNew_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CopyToNew_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CopyToNew_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0x3BB78416, 
		"newFileInfo", 
		L"newFileInfo", 
		sizeof("newFileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CopyToNew_struct, newFileInfo),
		NULL,
		NULL,
		2+1,
	},
	{
		0x3BB78416,
		"__newFileInfo_nSizeIs",
		L"__newFileInfo_nSizeIs",
		sizeof("__newFileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CopyToNew_struct, __newFileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xD99CE427, 
		"overwrite", 
		L"overwrite", 
		sizeof("overwrite")-1, 
		SOAPTYPE_BOOLEAN, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_CopyToNew_struct, overwrite),
		NULL,
		NULL,
		-1,
	},
	{
		0x0F07CF27, 
		"CopyToNewResult", 
		L"CopyToNewResult", 
		sizeof("CopyToNewResult")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CopyToNew_struct, CopyToNewResult),
		NULL,
		NULL,
		5+1,
	},
	{
		0x0F07CF27,
		"__CopyToNewResult_nSizeIs",
		L"__CopyToNewResult_nSizeIs",
		sizeof("__CopyToNewResult_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CopyToNew_struct, __CopyToNewResult_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_CopyToNew_map =
{
	0xEFDF5537,
	"CopyToNew",
	L"CopyToNewResponse",
	sizeof("CopyToNew")-1,
	sizeof("CopyToNewResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_CopyToNew_entries,
	sizeof(__CDocumentLibrary_CopyToNew_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_Download_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	ATLSOAP_BLOB DownloadResult;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_Download_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Download_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_Download_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xCA0213D7, 
		"DownloadResult", 
		L"DownloadResult", 
		sizeof("DownloadResult")-1, 
		SOAPTYPE_BASE64BINARY, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Download_struct, DownloadResult),
		NULL,
		NULL,
		-1,
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_Download_map =
{
	0x527D85E7,
	"Download",
	L"DownloadResponse",
	sizeof("Download")-1,
	sizeof("DownloadResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_Download_entries,
	sizeof(__CDocumentLibrary_Download_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_CopyTo_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	BSTR newFileName;
	bool overwrite;
	BSTR *CopyToResult;
	int __CopyToResult_nSizeIs;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_CopyTo_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CopyTo_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CopyTo_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0x3BBA0B8B, 
		"newFileName", 
		L"newFileName", 
		sizeof("newFileName")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CopyTo_struct, newFileName),
		NULL,
		NULL,
		-1,
	},
	{
		0xD99CE427, 
		"overwrite", 
		L"overwrite", 
		sizeof("overwrite")-1, 
		SOAPTYPE_BOOLEAN, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_CopyTo_struct, overwrite),
		NULL,
		NULL,
		-1,
	},
	{
		0x231ADF1D, 
		"CopyToResult", 
		L"CopyToResult", 
		sizeof("CopyToResult")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_CopyTo_struct, CopyToResult),
		NULL,
		NULL,
		4+1,
	},
	{
		0x231ADF1D,
		"__CopyToResult_nSizeIs",
		L"__CopyToResult_nSizeIs",
		sizeof("__CopyToResult_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_CopyTo_struct, __CopyToResult_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_CopyTo_map =
{
	0x54F63AAD,
	"CopyTo",
	L"CopyToResponse",
	sizeof("CopyTo")-1,
	sizeof("CopyToResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_CopyTo_entries,
	sizeof(__CDocumentLibrary_CopyTo_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};


struct __CDocumentLibrary_Upload_struct
{
	BSTR *fileInfo;
	int __fileInfo_nSizeIs;
	ATLSOAP_BLOB bStream;
	DictionaryEntry *deMeta;
	int __deMeta_nSizeIs;
	bool overwrite;
	BSTR *UploadResult;
	int __UploadResult_nSizeIs;
};

extern __declspec(selectany) const _soapmapentry __CDocumentLibrary_Upload_entries[] =
{

	{
		0xB81874CC, 
		"fileInfo", 
		L"fileInfo", 
		sizeof("fileInfo")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Upload_struct, fileInfo),
		NULL,
		NULL,
		0+1,
	},
	{
		0xB81874CC,
		"__fileInfo_nSizeIs",
		L"__fileInfo_nSizeIs",
		sizeof("__fileInfo_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_Upload_struct, __fileInfo_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0x41DAC64E, 
		"bStream", 
		L"bStream", 
		sizeof("bStream")-1, 
		SOAPTYPE_BASE64BINARY, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Upload_struct, bStream),
		NULL,
		NULL,
		-1,
	},
	{
		0xF0937FF0, 
		"deMeta", 
		L"deMeta", 
		sizeof("deMeta")-1, 
		SOAPTYPE_UNK, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Upload_struct, deMeta),
		NULL,
		&__DictionaryEntry_map,
		3+1,
	},
	{
		0xF0937FF0,
		"__deMeta_nSizeIs",
		L"__deMeta_nSizeIs",
		sizeof("__deMeta_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_Upload_struct, __deMeta_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{
		0xD99CE427, 
		"overwrite", 
		L"overwrite", 
		sizeof("overwrite")-1, 
		SOAPTYPE_BOOLEAN, 
		SOAPFLAG_NONE | SOAPFLAG_IN | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		offsetof(__CDocumentLibrary_Upload_struct, overwrite),
		NULL,
		NULL,
		-1,
	},
	{
		0x2D24A9C4, 
		"UploadResult", 
		L"UploadResult", 
		sizeof("UploadResult")-1, 
		SOAPTYPE_STRING, 
		SOAPFLAG_NONE | SOAPFLAG_OUT | SOAPFLAG_DYNARR | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL | SOAPFLAG_NULLABLE,
		offsetof(__CDocumentLibrary_Upload_struct, UploadResult),
		NULL,
		NULL,
		6+1,
	},
	{
		0x2D24A9C4,
		"__UploadResult_nSizeIs",
		L"__UploadResult_nSizeIs",
		sizeof("__UploadResult_nSizeIs")-1,
		SOAPTYPE_INT,
		SOAPFLAG_NOMARSHAL,
		offsetof(__CDocumentLibrary_Upload_struct, __UploadResult_nSizeIs),
		NULL,
		NULL,
		-1
	},
	{ 0x00000000 }
};

extern __declspec(selectany) const _soapmap __CDocumentLibrary_Upload_map =
{
	0x089D4B14,
	"Upload",
	L"UploadResponse",
	sizeof("Upload")-1,
	sizeof("UploadResponse")-1,
	SOAPMAP_FUNC,
	__CDocumentLibrary_Upload_entries,
	sizeof(__CDocumentLibrary_Upload_struct),
	1,
	-1,
	SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
	0xC2E575C3,
	"http://tempuri.org/",
	L"http://tempuri.org/",
	sizeof("http://tempuri.org/")-1
};

extern __declspec(selectany) const _soapmap * __CDocumentLibrary_funcs[] =
{
	&__CDocumentLibrary_Test_map,
	&__CDocumentLibrary_CheckIn_map,
	&__CDocumentLibrary_Remove_map,
	&__CDocumentLibrary_UpdateMeta_map,
	&__CDocumentLibrary_CheckOut_map,
	&__CDocumentLibrary_ConvertToDE_map,
	&__CDocumentLibrary_UndoCheckOut_map,
	&__CDocumentLibrary_CopyToNew_map,
	&__CDocumentLibrary_Download_map,
	&__CDocumentLibrary_CopyTo_map,
	&__CDocumentLibrary_Upload_map,
	NULL
};

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::Test(
		int* TestResult
	)
{
    if ( TestResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_Test_struct __params;
	memset(&__params, 0x00, sizeof(__params));

	__atlsoap_hr = SetClientStruct(&__params, 0);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/Test\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*TestResult = __params.TestResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::CheckIn(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		int* CheckInResult
	)
{
    if ( CheckInResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_CheckIn_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 1);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/CheckIn\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*CheckInResult = __params.CheckInResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::Remove(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		int* RemoveResult
	)
{
    if ( RemoveResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_Remove_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 2);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/Remove\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*RemoveResult = __params.RemoveResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::UpdateMeta(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		DictionaryEntry* deMeta, int __deMeta_nSizeIs, 
		int* UpdateMetaResult
	)
{
    if ( UpdateMetaResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_UpdateMeta_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;
	__params.deMeta = deMeta;
	__params.__deMeta_nSizeIs = __deMeta_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 3);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/UpdateMeta\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*UpdateMetaResult = __params.UpdateMetaResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::CheckOut(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		int* CheckOutResult
	)
{
    if ( CheckOutResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_CheckOut_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 4);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/CheckOut\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*CheckOutResult = __params.CheckOutResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::ConvertToDE(
		DictionaryEntry* entries, int __entries_nSizeIs, 
		DictionaryEntry** ConvertToDEResult, int* __ConvertToDEResult_nSizeIs
	)
{
    if ( ConvertToDEResult == NULL )
		return E_POINTER;
	if( __ConvertToDEResult_nSizeIs == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_ConvertToDE_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.entries = entries;
	__params.__entries_nSizeIs = __entries_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 5);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/ConvertToDE\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*ConvertToDEResult = __params.ConvertToDEResult;
	*__ConvertToDEResult_nSizeIs = __params.__ConvertToDEResult_nSizeIs;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::UndoCheckOut(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		int* UndoCheckOutResult
	)
{
    if ( UndoCheckOutResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_UndoCheckOut_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 6);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/UndoCheckOut\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*UndoCheckOutResult = __params.UndoCheckOutResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::CopyToNew(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		BSTR* newFileInfo, int __newFileInfo_nSizeIs, 
		bool overwrite, 
		BSTR** CopyToNewResult, int* __CopyToNewResult_nSizeIs
	)
{
    if ( CopyToNewResult == NULL )
		return E_POINTER;
	if( __CopyToNewResult_nSizeIs == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_CopyToNew_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;
	__params.newFileInfo = newFileInfo;
	__params.__newFileInfo_nSizeIs = __newFileInfo_nSizeIs;
	__params.overwrite = overwrite;

	__atlsoap_hr = SetClientStruct(&__params, 7);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/CopyToNew\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*CopyToNewResult = __params.CopyToNewResult;
	*__CopyToNewResult_nSizeIs = __params.__CopyToNewResult_nSizeIs;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::Download(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		ATLSOAP_BLOB* DownloadResult
	)
{
    if ( DownloadResult == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_Download_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;

	__atlsoap_hr = SetClientStruct(&__params, 8);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/Download\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*DownloadResult = __params.DownloadResult;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::CopyTo(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		BSTR newFileName, 
		bool overwrite, 
		BSTR** CopyToResult, int* __CopyToResult_nSizeIs
	)
{
    if ( CopyToResult == NULL )
		return E_POINTER;
	if( __CopyToResult_nSizeIs == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_CopyTo_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;
	__params.newFileName = newFileName;
	__params.overwrite = overwrite;

	__atlsoap_hr = SetClientStruct(&__params, 9);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/CopyTo\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*CopyToResult = __params.CopyToResult;
	*__CopyToResult_nSizeIs = __params.__CopyToResult_nSizeIs;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
inline HRESULT CDocumentLibraryT<TClient>::Upload(
		BSTR* fileInfo, int __fileInfo_nSizeIs, 
		ATLSOAP_BLOB bStream, 
		DictionaryEntry* deMeta, int __deMeta_nSizeIs, 
		bool overwrite, 
		BSTR** UploadResult, int* __UploadResult_nSizeIs
	)
{
    if ( UploadResult == NULL )
		return E_POINTER;
	if( __UploadResult_nSizeIs == NULL )
		return E_POINTER;

	HRESULT __atlsoap_hr = InitializeSOAP(NULL);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_INITIALIZE_ERROR);
		return __atlsoap_hr;
	}
	
	CleanupClient();

	CComPtr<IStream> __atlsoap_spReadStream;
	__CDocumentLibrary_Upload_struct __params;
	memset(&__params, 0x00, sizeof(__params));
	__params.fileInfo = fileInfo;
	__params.__fileInfo_nSizeIs = __fileInfo_nSizeIs;
	__params.bStream = bStream;
	__params.deMeta = deMeta;
	__params.__deMeta_nSizeIs = __deMeta_nSizeIs;
	__params.overwrite = overwrite;

	__atlsoap_hr = SetClientStruct(&__params, 10);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_OUTOFMEMORY);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = GenerateResponse(GetWriteStream());
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_GENERATE_ERROR);
		goto __skip_cleanup;
	}
	
	__atlsoap_hr = SendRequest(_T("SOAPAction: \"http://tempuri.org/Upload\"\r\n"));
	if (FAILED(__atlsoap_hr))
	{
		goto __skip_cleanup;
	}
	__atlsoap_hr = GetReadStream(&__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_READ_ERROR);
		goto __skip_cleanup;
	}
	
	// cleanup any in/out-params and out-headers from previous calls
	Cleanup();
	__atlsoap_hr = BeginParse(__atlsoap_spReadStream);
	if (FAILED(__atlsoap_hr))
	{
		SetClientError(SOAPCLIENT_PARSE_ERROR);
		goto __cleanup;
	}

	*UploadResult = __params.UploadResult;
	*__UploadResult_nSizeIs = __params.__UploadResult_nSizeIs;
	goto __skip_cleanup;
	
__cleanup:
	Cleanup();
__skip_cleanup:
	ResetClientState(true);
	memset(&__params, 0x00, sizeof(__params));
	return __atlsoap_hr;
}

template <typename TClient>
ATL_NOINLINE inline const _soapmap ** CDocumentLibraryT<TClient>::GetFunctionMap()
{
	return __CDocumentLibrary_funcs;
}

template <typename TClient>
ATL_NOINLINE inline const _soapmap ** CDocumentLibraryT<TClient>::GetHeaderMap()
{
	static const _soapmapentry __CDocumentLibrary_Test_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_Test_atlsoapheader_map = 
	{
		0x66CB6EEF,
		"Test",
		L"TestResponse",
		sizeof("Test")-1,
		sizeof("TestResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_Test_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_CheckIn_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_CheckIn_atlsoapheader_map = 
	{
		0x1EDB9484,
		"CheckIn",
		L"CheckInResponse",
		sizeof("CheckIn")-1,
		sizeof("CheckInResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_CheckIn_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_Remove_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_Remove_atlsoapheader_map = 
	{
		0xE814EBBD,
		"Remove",
		L"RemoveResponse",
		sizeof("Remove")-1,
		sizeof("RemoveResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_Remove_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_UpdateMeta_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_UpdateMeta_atlsoapheader_map = 
	{
		0x880A8399,
		"UpdateMeta",
		L"UpdateMetaResponse",
		sizeof("UpdateMeta")-1,
		sizeof("UpdateMetaResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_UpdateMeta_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_CheckOut_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_CheckOut_atlsoapheader_map = 
	{
		0xF7CB4B05,
		"CheckOut",
		L"CheckOutResponse",
		sizeof("CheckOut")-1,
		sizeof("CheckOutResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_CheckOut_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_ConvertToDE_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_ConvertToDE_atlsoapheader_map = 
	{
		0x7F5985FC,
		"ConvertToDE",
		L"ConvertToDEResponse",
		sizeof("ConvertToDE")-1,
		sizeof("ConvertToDEResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_ConvertToDE_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_UndoCheckOut_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_UndoCheckOut_atlsoapheader_map = 
	{
		0x6BC3F47B,
		"UndoCheckOut",
		L"UndoCheckOutResponse",
		sizeof("UndoCheckOut")-1,
		sizeof("UndoCheckOutResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_UndoCheckOut_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_CopyToNew_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_CopyToNew_atlsoapheader_map = 
	{
		0xEFDF5537,
		"CopyToNew",
		L"CopyToNewResponse",
		sizeof("CopyToNew")-1,
		sizeof("CopyToNewResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_CopyToNew_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_Download_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_Download_atlsoapheader_map = 
	{
		0x527D85E7,
		"Download",
		L"DownloadResponse",
		sizeof("Download")-1,
		sizeof("DownloadResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_Download_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_CopyTo_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_CopyTo_atlsoapheader_map = 
	{
		0x54F63AAD,
		"CopyTo",
		L"CopyToResponse",
		sizeof("CopyTo")-1,
		sizeof("CopyToResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_CopyTo_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};

	static const _soapmapentry __CDocumentLibrary_Upload_atlsoapheader_entries[] =
	{
		{ 0x00000000 }
	};

	static const _soapmap __CDocumentLibrary_Upload_atlsoapheader_map = 
	{
		0x089D4B14,
		"Upload",
		L"UploadResponse",
		sizeof("Upload")-1,
		sizeof("UploadResponse")-1,
		SOAPMAP_HEADER,
		__CDocumentLibrary_Upload_atlsoapheader_entries,
		0,
		0,
		-1,
		SOAPFLAG_NONE | SOAPFLAG_PID | SOAPFLAG_DOCUMENT | SOAPFLAG_LITERAL,
		0xC2E575C3,
		"http://tempuri.org/",
		L"http://tempuri.org/",
		sizeof("http://tempuri.org/")-1
	};


	static const _soapmap * __CDocumentLibrary_headers[] =
	{
		&__CDocumentLibrary_Test_atlsoapheader_map,
		&__CDocumentLibrary_CheckIn_atlsoapheader_map,
		&__CDocumentLibrary_Remove_atlsoapheader_map,
		&__CDocumentLibrary_UpdateMeta_atlsoapheader_map,
		&__CDocumentLibrary_CheckOut_atlsoapheader_map,
		&__CDocumentLibrary_ConvertToDE_atlsoapheader_map,
		&__CDocumentLibrary_UndoCheckOut_atlsoapheader_map,
		&__CDocumentLibrary_CopyToNew_atlsoapheader_map,
		&__CDocumentLibrary_Download_atlsoapheader_map,
		&__CDocumentLibrary_CopyTo_atlsoapheader_map,
		&__CDocumentLibrary_Upload_atlsoapheader_map,
		NULL
	};
	
	return __CDocumentLibrary_headers;
}

template <typename TClient>
ATL_NOINLINE inline void * CDocumentLibraryT<TClient>::GetHeaderValue()
{
	return this;
}

template <typename TClient>
ATL_NOINLINE inline const wchar_t * CDocumentLibraryT<TClient>::GetNamespaceUri()
{
	return L"http://tempuri.org/";
}

template <typename TClient>
ATL_NOINLINE inline const char * CDocumentLibraryT<TClient>::GetServiceName()
{
	return NULL;
}

template <typename TClient>
ATL_NOINLINE inline const char * CDocumentLibraryT<TClient>::GetNamespaceUriA()
{
	return "http://tempuri.org/";
}

template <typename TClient>
ATL_NOINLINE inline HRESULT CDocumentLibraryT<TClient>::CallFunction(
	void *, 
	const wchar_t *, int,
	size_t)
{
	return E_NOTIMPL;
}

template <typename TClient>
ATL_NOINLINE inline HRESULT CDocumentLibraryT<TClient>::GetClientReader(ISAXXMLReader **ppReader)
{
	if (ppReader == NULL)
	{
		return E_INVALIDARG;
	}
	
	CComPtr<ISAXXMLReader> spReader = GetReader();
	if (spReader.p != NULL)
	{
		*ppReader = spReader.Detach();
		return S_OK;
	}
	return TClient::GetClientReader(ppReader);
}

} // namespace DocumentLibrary

template<>
inline HRESULT AtlCleanupValue<DocumentLibrary::DictionaryEntry>(DocumentLibrary::DictionaryEntry *pVal)
{
	pVal;
	AtlCleanupValue(&pVal->Key);
	AtlCleanupValue(&pVal->Value);
	return S_OK;
}

template<>
inline HRESULT AtlCleanupValueEx<DocumentLibrary::DictionaryEntry>(DocumentLibrary::DictionaryEntry *pVal, IAtlMemMgr *pMemMgr)
{
	pVal;
	pMemMgr;
	
	AtlCleanupValueEx(&pVal->Key, pMemMgr);
	AtlCleanupValueEx(&pVal->Value, pMemMgr);
	return S_OK;
}
