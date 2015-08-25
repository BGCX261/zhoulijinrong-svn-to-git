using System;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Busi.Process;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public partial class PG_SignCommentInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string processID = string.Empty;
                string stepName = string.Empty;
                string workItemID = string.Empty;
                string userID = string.Empty;
                string deptID = string.Empty;

                if (Request.QueryString["processID"] == null)
                { return; }
                else
                { processID = Request.QueryString["processID"].ToString(); }

                if (Request.QueryString["workItemID"] == null)
                { return; }
                else
                { workItemID = Request.QueryString["workItemID"].ToString(); }

                if (Request.QueryString["stepName"] == null)
                { return; }
                else
                { stepName = Request.QueryString["stepName"].ToString(); }

                if (Request.QueryString["userID"] == null)
                { return; }
                else
                { userID = Server.UrlDecode(Request.QueryString["userID"].ToString()); }

                if (Request.QueryString["deptID"] == null)
                { return; }
                else
                { deptID = Request.QueryString["deptID"].ToString(); }

                B_WorkRelation bWorkRelation = new B_WorkRelation();

                rptComment.DataSource = bWorkRelation.GetSignComment(processID, workItemID, stepName, userID, deptID);
                rptComment.DataBind();
            }
        }
    }
}
