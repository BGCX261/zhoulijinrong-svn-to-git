

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Fri May 14 16:29:43 2010
 */
/* Compiler settings for .\EditorOnline.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __EditorOnlineidl_h__
#define __EditorOnlineidl_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DEditorOnline_FWD_DEFINED__
#define ___DEditorOnline_FWD_DEFINED__
typedef interface _DEditorOnline _DEditorOnline;
#endif 	/* ___DEditorOnline_FWD_DEFINED__ */


#ifndef ___DEditorOnlineEvents_FWD_DEFINED__
#define ___DEditorOnlineEvents_FWD_DEFINED__
typedef interface _DEditorOnlineEvents _DEditorOnlineEvents;
#endif 	/* ___DEditorOnlineEvents_FWD_DEFINED__ */


#ifndef __EditorOnline_FWD_DEFINED__
#define __EditorOnline_FWD_DEFINED__

#ifdef __cplusplus
typedef class EditorOnline EditorOnline;
#else
typedef struct EditorOnline EditorOnline;
#endif /* __cplusplus */

#endif 	/* __EditorOnline_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 


#ifndef __EditorOnlineLib_LIBRARY_DEFINED__
#define __EditorOnlineLib_LIBRARY_DEFINED__

/* library EditorOnlineLib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_EditorOnlineLib;

#ifndef ___DEditorOnline_DISPINTERFACE_DEFINED__
#define ___DEditorOnline_DISPINTERFACE_DEFINED__

/* dispinterface _DEditorOnline */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DEditorOnline;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("38B04295-9189-4AF9-A87A-274E5FC79880")
    _DEditorOnline : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DEditorOnlineVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DEditorOnline * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DEditorOnline * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DEditorOnline * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DEditorOnline * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DEditorOnline * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DEditorOnline * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DEditorOnline * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DEditorOnlineVtbl;

    interface _DEditorOnline
    {
        CONST_VTBL struct _DEditorOnlineVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DEditorOnline_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DEditorOnline_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DEditorOnline_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DEditorOnline_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DEditorOnline_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DEditorOnline_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DEditorOnline_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DEditorOnline_DISPINTERFACE_DEFINED__ */


#ifndef ___DEditorOnlineEvents_DISPINTERFACE_DEFINED__
#define ___DEditorOnlineEvents_DISPINTERFACE_DEFINED__

/* dispinterface _DEditorOnlineEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DEditorOnlineEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("BBDAA0F9-8277-4D3B-B53D-673D82F87042")
    _DEditorOnlineEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DEditorOnlineEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DEditorOnlineEvents * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DEditorOnlineEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DEditorOnlineEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DEditorOnlineEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DEditorOnlineEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DEditorOnlineEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DEditorOnlineEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DEditorOnlineEventsVtbl;

    interface _DEditorOnlineEvents
    {
        CONST_VTBL struct _DEditorOnlineEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DEditorOnlineEvents_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DEditorOnlineEvents_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DEditorOnlineEvents_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DEditorOnlineEvents_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DEditorOnlineEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DEditorOnlineEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DEditorOnlineEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DEditorOnlineEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_EditorOnline;

#ifdef __cplusplus

class DECLSPEC_UUID("B810BF98-8736-4824-B674-521A7E7369B1")
EditorOnline;
#endif
#endif /* __EditorOnlineLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


