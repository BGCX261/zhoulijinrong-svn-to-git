

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


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


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, LIBID_EditorOnlineLib,0x5B07517A,0x352B,0x436B,0xAC,0xE1,0x2B,0x53,0x78,0x53,0x37,0x3A);


MIDL_DEFINE_GUID(IID, DIID__DEditorOnline,0x38B04295,0x9189,0x4AF9,0xA8,0x7A,0x27,0x4E,0x5F,0xC7,0x98,0x80);


MIDL_DEFINE_GUID(IID, DIID__DEditorOnlineEvents,0xBBDAA0F9,0x8277,0x4D3B,0xB5,0x3D,0x67,0x3D,0x82,0xF8,0x70,0x42);


MIDL_DEFINE_GUID(CLSID, CLSID_EditorOnline,0xB810BF98,0x8736,0x4824,0xB6,0x74,0x52,0x1A,0x7E,0x73,0x69,0xB1);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



