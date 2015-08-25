using System;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.Common;
using FS.OA.Framework;
using FS.ADIM.OA.WebUI.UIBase;
using FS.OA.Framework.Caching;
using System.Xml;

namespace FS.ADIM.OA.WebUI
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            String l_strDataSource = OAConfig.GetConfig("数据库", "DataSource");
            String l_strDatabase = OAConfig.GetConfig("数据库", "DataBase");
            String l_strUserId = OAConfig.GetConfig("数据库", "uid");
            String l_strPassword = OAConfig.GetConfig("数据库", "pwd");

            //初始化数据库连接
            if (!Entity.InitDB(ConstString.Miscellaneous.DATA_BASE_TYPE, l_strDataSource, l_strDatabase, l_strUserId, l_strPassword, 30))
            {
                throw new Exception("数据库连接错误");
            }

            SQLHelper.InitDB1(OAConfig.GetConfig("数据库", "ADIMSqlServer"));
            SQLHelper.InitDB2(OAConfig.GetConfig("数据库", "AgilePointSqlServer"));

            //自动阅知
            AutoRead.Instance.TimerStart();
            //自动迁移旧数据
            AutoBackup.Instance.TimerStart();

            //把XML文件加载到缓存中
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"Config\SelectGroup.xml";
            if (!string.IsNullOrEmpty(strPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strPath);
                DataCache cache = new DataCache();
                cache.ExpireTime = 1440;
                cache.CacheName = "SelectGroup";
                cache.CacheItemName = "SelectGroupItem";
                cache.SetCache(doc);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }
    }
}