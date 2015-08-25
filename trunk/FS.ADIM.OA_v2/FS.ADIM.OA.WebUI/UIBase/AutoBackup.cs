using System;
using System.IO;
using System.Text;
using System.Web;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.UIBase
{
    public class AutoBackup
    {
        private System.Timers.Timer m_timer;

        private const string split = " @@ ";
        /// <summary>
        /// 单例模式的接口
        /// </summary>
        public static readonly AutoBackup Instance = new AutoBackup();


        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private AutoBackup()
        {
            string strSpan = OAConfig.GetConfig("自动迁移旧数据", "定时执行");

            this.m_timer = new System.Timers.Timer();
            //this.m_timer.Enabled = false;
            //this.m_timer.Interval = 24 * 60 * 60 * 1000;    //默认24个小时执行一次
            if (strSpan == "")
            {
                this.m_timer.Interval = 86400000;//24小时
                //this.m_timer.Interval = 180000;
            }
            else
            {
                strSpan = strSpan + "0000";
                this.m_timer.Interval = Convert.ToInt32(strSpan);
            }
            this.m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
        }

        /// <summary>
        /// 定时器开始
        /// </summary>
        public void TimerStart()
        {
            this.m_timer.Enabled = true;
        }
        /// <summary>
        /// 设置定时器的频率，单位是毫秒
        /// </summary>
        /// <param name="Interval">毫秒</param>
        public void SetTimerInterval(int Interval)
        {
            this.m_timer.Interval = Interval;
        }

        /**/
        /// <summary>
        /// 定时迁移旧数据 每天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Backup();
        }

        /// <summary>
        /// 自动阅知
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="contents"></param>
        public void Backup()
        {
            try
            {
                string s = "";
                string t = "";
                if (DateTime.Now.Month.ToString().Length == 1)
                {
                    s = "0";//补0
                }
                if (DateTime.Now.Day.ToString().Length == 1)
                {
                    t = "0";//补0
                }
                string date = DateTime.Now.Year.ToString().Substring(2) + s + DateTime.Now.Month.ToString() + t + DateTime.Now.Day.ToString();

                string dirPath = HttpRuntime.AppDomainAppPath + "Log\\AutoBackup\\";
                string fileName = "Log" + date + ".txt";
                if (!File.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                B_OldToNew backup = new B_OldToNew();
                int i=backup.DataTranslate();
                if (i > 0)
                {
                    string contents = "执行自动迁移旧数据 " + DateTime.Now.ToString() + "\r\n";
                    File.AppendAllText(dirPath + fileName, contents, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                WriteLog writelog = new WriteLog();
                writelog.WriteErrLog("AutoBackup",DateTime.Now.ToString()+ex.ToString());
                throw ex;
            }
        }       
        public void DeleteTempFile()
        {

        }

    }
}
