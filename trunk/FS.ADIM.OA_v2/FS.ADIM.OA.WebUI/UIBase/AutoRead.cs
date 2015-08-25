using System;
using System.IO;
using System.Text;
using System.Web;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.UIBase
{
    public class AutoRead
    {
        private System.Timers.Timer m_timer;

        private const string split = " @@ ";
        /// <summary>
        /// 单例模式的接口
        /// </summary>
        public static readonly AutoRead Instance = new AutoRead();


        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private AutoRead()
        {
            string strSpan = OAConfig.GetConfig("自动传阅期限", "定时执行");

            this.m_timer = new System.Timers.Timer();
            //this.m_timer.Enabled = false;
            //this.m_timer.Interval = 12 * 60 * 60 * 1000;    //默认12个小时执行一次
            if (strSpan == "")
            {
                this.m_timer.Interval = 3600000;//1小时
            }
            else
            {
                strSpan = strSpan + "000";
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
        /// 定时阅知 30天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Read();
        }

        /// <summary>
        /// 自动阅知
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="contents"></param>
        public void Read()
        {
            try
            {
                string s = "";
                if (DateTime.Now.Month.ToString().Length == 1)
                {
                    s = "0";//补0
                }
                string date = DateTime.Now.Year.ToString().Substring(2) + s + DateTime.Now.Month.ToString();

                string dirPath = HttpRuntime.AppDomainAppPath + "Log\\AutoRead\\";
                string fileName = "Log" + date + ".txt";
                if (!File.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                B_Circulate circulate = new B_Circulate("");
                int i = circulate.AutoRead();
                if (i > 0)
                {
                    string contents = "执行自动阅知 " + DateTime.Now.ToString() + "\r\n";
                    File.AppendAllText(dirPath + fileName, contents, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public void DeleteTempFile()
        {

        }

    }
}
