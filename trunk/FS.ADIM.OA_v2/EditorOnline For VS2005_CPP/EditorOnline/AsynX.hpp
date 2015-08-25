#include <windows.h>
#include <process.h>

typedef unsigned int (WINAPI *THREADFUNC)(LPVOID);
typedef void (*PFUN)(LPVOID);

class AsynX
{
public:
    AsynX()
        :m_pFun(NULL),m_pNotify(NULL),m_pFunParam(NULL),m_hThread(NULL)
	{
	}
    ~AsynX()
    {
    	if(m_hThread != NULL) 
		{
            TermThread();
			if(!m_hThread) CloseHandle(m_hThread);
		}
    }
private:
	static DWORD WINAPI WorkerThread(LPVOID pParam)
    {
        ((AsynX*)pParam)->ServerWorker();
		return (DWORD)1;
    }

	void ServerWorker()
	{
		try {
			if(m_pFun == NULL) return;
			m_pFun(m_pFunParam);
			if(m_pNotify == NULL) return;
			m_pNotify(m_pFunParam);
			return;
		}
		catch(...) {
			return;
		}
	}
public:
    virtual bool CreateWorder()
    {
        if(NULL == m_hThread || WAIT_OBJECT_0 == WaitForSingleObject(m_hThread, 0))
		    m_hThread = (HANDLE)_beginthreadex(NULL, 0, (THREADFUNC)WorkerThread, this, 0, NULL);
        if(!m_hThread) {
            CloseHandle(m_hThread);
            return false;
		}
        return true;
    }
    
    // 不在万不得以的时候不要调用该方法
    void TermThread()
    {
		DWORD dwExitCode = 0x0;
		GetExitCodeThread(m_hThread, &dwExitCode);
        TerminateThread(m_hThread, dwExitCode);
    }

public:
    PFUN m_pFun;
    PFUN m_pNotify;
    LPVOID m_pFunParam;

private:
    HANDLE m_hThread;
};