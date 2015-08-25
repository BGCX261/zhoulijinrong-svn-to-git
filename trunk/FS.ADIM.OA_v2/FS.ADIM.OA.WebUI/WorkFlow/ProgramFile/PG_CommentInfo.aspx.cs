using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.BLL.Busi.Process;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class PG_CommentInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strProcessID = string.Empty;
                string strStepName = string.Empty;
                string strWorkItem = string.Empty;

                if (Request.QueryString["processID"] == null)
                { return; }
                else
                { strProcessID = Request.QueryString["processID"].ToString(); }

                if (Request.QueryString["WorkItemID"] == null)
                { return; }
                else
                { strWorkItem = Request.QueryString["WorkItemID"].ToString(); }

                if (Request.QueryString["stepName"] == null)
                { return; }
                else
                { strStepName = Request.QueryString["stepName"].ToString(); }

                rptComment.DataSource = B_PF.GetComment(strProcessID, strWorkItem, strStepName);
                rptComment.DataBind();
            }
        }
    }
}
