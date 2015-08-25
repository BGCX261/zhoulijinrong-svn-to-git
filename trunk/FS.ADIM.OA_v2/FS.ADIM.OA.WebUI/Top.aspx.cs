using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.SystemM;

namespace FS.ADIM.OA.WebUI
{
    public partial class Top : System.Web.UI.Page
    {
        protected string style1 = "<font style='color:Red;'>";
        protected string style1_1 = "</font>";

        protected string style2 = "<font style='color:Blue;'>";
        protected string style2_1 = "</font>";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //如果使用单点登陆
                string isUse = ConfigurationManager.AppSettings["IsUseSSO"];

                if (isUse == "1")
                {
                    btnLogOutSSO.Visible = true;
                }
                else
                {
                    btnLogOut.Visible = true;
                }

                #region 显示当前人 部门 角色

                string role = SysString.GetStringFormatForList(CurrentUserInfo.RoleName, ",");
                string dept = string.Empty;
                if (CurrentUserInfo.DeptPost != null)
                {
                    foreach (DataRow dr in CurrentUserInfo.DeptPost.Rows)
                    {
                        string post = string.Empty;
                        if (string.IsNullOrEmpty(dr["PostName"].ToString()) == false)
                        {
                            post = dr["PostName"].ToString();
                        }
                        else
                        {
                            post = "无职位";
                        }
                        dept += "," + dr["Name"].ToString() + "--" + style2 + post + style2_1;
                    }
                    if (dept.Length > 0)
                    {
                        dept = dept.Substring(1);
                    }
                }
                string titleStyle = CurrentUserInfo.DisplayName;
                string title = CurrentUserInfo.DisplayName;
                if (role != string.Empty)
                {
                    titleStyle += "(所属角色：" + style2 + SysString.CutHtml(role, 30) + style2_1 + ")";
                    title += "(所属角色：" + role + ")";
                }
                if (dept != string.Empty)
                {
                    titleStyle += "(所属部门：" + style1 + dept + style1_1 + ")";
                    title += "(所属部门：" + dept.Replace(style2, "").Replace(style2_1, "") + ")";
                }

                lblUserInfo.Text = titleStyle + " " + DateTime.Now.ToShortDateString();

                lblUserInfo.ToolTip = title;
                #endregion

                if (CurrentUserInfo.LoginName == string.Empty)
                {
                    if (isUse == "1")
                    {
                        btnLogOutSSO_Click(null, null);
                    }
                    else
                    {
                        btnLogOut_Click(null, null);
                    }
                }
                #region 其他系统地址
                string sqlSys = "SELECT Name,Path FROM T_RS_SYS_Module WHERE ParentID=0";
                DataTable dtSys = FounderSoftware.Framework.Business.Entity.RunQuery(sqlSys);
                if (dtSys.Rows.Count > 0)
                {
                    string UserName = CookieHelper.Get("NewOA", "UserName");
                    string AuID = CookieHelper.Get("NewOA", "AuID");

                    DataRow[] drs1 = dtSys.Select("Name='组织机构'");
                    if (drs1.Length > 0)
                    {
                        btnOU.CommandName = drs1[0]["Path"].ToString() + "?UserName=" + UserName + "&AuID" + AuID;
                        btnOU.ToolTip = btnOU.CommandName;
                    }
                    DataRow[] drs2 = dtSys.Select("Name='系统管理'");
                    if (drs2.Length > 0)
                    {
                        btnSys.CommandName = drs2[0]["Path"].ToString() + "?UserName=" + UserName + "&AuID" + AuID;
                        btnSys.ToolTip = btnSys.CommandName;
                    }
                }
                #endregion
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            ClientScriptM.ResponseScript(Page, "window.top.location.href='login.aspx';");
        }

        protected void btnLogOutSys_Click(object sender, EventArgs e)
        {
            ClientScriptM.ResponseScript(Page, "window.top.location.href='login.aspx';");
        
        }
        protected void btnLogOutSSO_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["UserName"] == null)
            {
                string url = System.Web.Configuration.WebConfigurationManager.AppSettings["SSOLoginURL"].ToString();
                ClientScriptM.ResponseScript(Page, string.Format("window.top.location.href='{0}';", url));
            }
            else
            {
                string url = System.Web.Configuration.WebConfigurationManager.AppSettings["SSOLoginURL"].ToString();
                ClientScriptM.ResponseScript(Page, string.Format("window.top.location.href='{0}';", url));
            }
        }

        protected void btnSys_Click(object sender, EventArgs e)
        {
            ClientScriptM.ResponseScript(Page, string.Format("window.top.location.href='{0}';", (sender as LinkButton).CommandName));
        }
    }
}
