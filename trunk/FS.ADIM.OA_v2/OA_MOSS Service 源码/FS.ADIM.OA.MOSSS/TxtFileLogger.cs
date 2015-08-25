using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace FS.ADIM.OA.MOSSS
{
   public class TxtFileLogger 
    {
       private string m_strPath=string.Empty;

       public TxtFileLogger(string path,string ProcessTemplate)
       {
           string fileName = DateTime.Now.ToString("yyyyMMdd");

           this.m_strPath = path + "\\" + ProcessTemplate+"\\"+fileName + ".log";
          
           string m_path = path + "\\" + ProcessTemplate;
           DirectoryInfo m_DirInfo = new DirectoryInfo(m_path);

           if (!m_DirInfo.Exists) //检测文件夹是否存在
           {
               Directory.CreateDirectory(m_path);   
           }
          
           // If the file is not present, create it
           if (!File.Exists(m_strPath))
           {
               FileStream fs = File.Create(m_strPath);
               fs.Close();
           }
       }

       public void WriteLog(string Message)
       {
           string text = "[" + DateTime.Now + "]" + string.Format(Message);
           //string text =  string.Format(Message);
           TextWriter tw = File.AppendText(m_strPath);
           tw.WriteLine(text);
           tw.Close();
       }

    }
}
