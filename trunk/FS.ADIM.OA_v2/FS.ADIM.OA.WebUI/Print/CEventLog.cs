using System;
using System.Diagnostics;

namespace FS.ADIM.OA.WebUI.Print
{
    public class CEventLog
    {
        private const string sSource = "FS.ADMIN.PRINT";
        private const string sLogName = "ADIMLog";

        private static EventLog myLog = null;
        private readonly static object oLock = new object();

        protected CEventLog()
        { 
            myLog = new EventLog();
            if (myLog == null) throw new Exception("EventLog Fialed");
            
            if (!EventLog.SourceExists(sSource))
            {
                EventLog.CreateEventSource(sSource, sLogName);
            }
            myLog.Source = sSource;
            
        }

        public static void Instance()
        {
            if (myLog == null)
            {
                lock (oLock)
                {
                    if (myLog == null)
                        new CEventLog();
                }
            }
        }
        
        public static void Log(string sMsg)
        {
            lock (oLock)
            {
                if (sMsg.Contains("正在中止线程。")) return;
                myLog.WriteEntry(sMsg);
            }
        }

        public static void Delete()
        {
            lock (oLock)
            {
                if (EventLog.SourceExists(sSource))
                {
                    EventLog.DeleteEventSource(sSource);
                    if (EventLog.Exists(sLogName))
                        EventLog.Delete(sLogName);
                }
            }
        }
    }
}
