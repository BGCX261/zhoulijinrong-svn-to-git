using System;
using System.Configuration;
using System.Web;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OU.OutBLL;
using FounderSoftware.ADIM.SSO.Utility;

namespace FS.ADIM.OA.WebUI
{
    public partial class Index : System.Web.UI.Page
    {
        protected string target = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserName = Request.QueryString["UserName"];
            string AuID = Request.QueryString["AuID"];
            target = Request.QueryString["target"];

            if (!String.IsNullOrEmpty(UserName))
            {
                Login(UserName, AuID);
                SSOUtility.Login(this);
            }
        }

        private void Login(string strUserName, string AuID)
        {
            OALogin.LoginUserInfo info = OALogin.Login(strUserName);

            Session["LoginUserInfo"] = info;

            HttpCookie cookie = new HttpCookie("NewOA");

            TimeSpan ts = new TimeSpan(365, 0, 0, 0);
            cookie.Expires = DateTime.Now.Add(ts);
            cookie.Values.Remove("LoginID");
            cookie.Values.Add("LoginID", strUserName);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
    }
}
