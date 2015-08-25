using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OU.OutBLL;
using FS.OA.Framework;


using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OA.WebUI
{
    public partial class _Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSubSys.Items.Add("OA");

                string strDataSource = OAConfig.GetConfig("数据库", "DataSource");

                string strAGUrl = OAConfig.GetConfig("AgilePoint认证", "ServerUrl");

                string strOUDataSource = OAConfig.GetConfig("数据库", "ADIMOUSqlServer");
               
                
                    //ViewBase vb =FS.ADIM.OU.OutBLL.OADept.GetAllDeptIDAndName();
                    //string a = vb.GetFieldVals("ID",";");
                //Label1.Text = "数据库：" + strDataSource + "<br/>" + "AgilePoint服务器：" + strAGUrl + "<br/>" + "OU数据库" + strOUDataSource;
            }
        }
        protected void imgbtnLogin_Click(object sender, ImageClickEventArgs e)
        {
            string l_strUserName = txtUserName.Text;

            //登陆
            OALogin.LoginUserInfo info = OALogin.Login(l_strUserName);

            if (info.ID == -1)
            {
                IMessage im = new WebFormMessage(Page, "没有该用户");
                im.Show();
                return;
            }

            if (info.Domain == "")
            {
                IMessage im = new WebFormMessage(Page, "OA子系统用户必须为域用户");
                im.Show();
                return;
            }

            HttpCookie cookie = new HttpCookie("NewOA");

            TimeSpan ts = new TimeSpan(365, 0, 0, 0);
            cookie.Expires = DateTime.Now.Add(ts);
            cookie.Values.Remove("LoginID");
            cookie.Values.Add("LoginID", l_strUserName);
            this.Response.AppendCookie(cookie);

            Session["LoginUserInfo"] = info;
            

            Response.Redirect("~/Index.aspx", true);
        }

        protected void imgbtnLogin_Click(object sender, EventArgs e)
        {
            String strFile = "D:\\HNPCADIM\\FS.ADIM.OA\\FS.ADIM.OA.WebUI\\template\\Singer\\孟辉.png";//this.TextBoxFile.Text;
            String strName = "孟辉";// this.TextBoxName.Text;
            String sPath = strFile;
            String sTableName = "T_OU_User";

            MemoryStream ms = new MemoryStream();
            System.Drawing.Image image = System.Drawing.Image.FromFile(sPath);

            string sSQL = "update " + sTableName + " set [Image] = " + image + " where Name = '" + strName + "'";


            System.Data.DataTable dtImgData = FounderSoftware.Framework.Business.Entity.RunQuery(sSQL, "Sql",
                                            OAConfig.GetConfig("数据库", "DataSource"),
                                            OAConfig.GetConfig("数据库", "DataBase"),
                                            OAConfig.GetConfig("数据库", "uid"),
                                            OAConfig.GetConfig("数据库", "pwd"));

            //Bitmap newbitmap = new Bitmap(image.Width, image.Height);
            //Graphics g = Graphics.FromImage(newbitmap);
            //g.DrawImageUnscaled(image, 0, 0);
            //newbitmap.Save(sPath, ImageFormat.Png);
            //System.Drawing.Image.FromStream(ms)..Save(sPath);
            //g.Dispose();
            //newbitmap.Dispose();
            //image.Dispose();

        }
    }
}
