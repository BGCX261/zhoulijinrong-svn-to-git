using System;
using System.Data;
using FS.ADIM.OA.BLL.Common;
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI.PageWF
{
    /// <summary>
    /// 查看意见
    /// </summary>
    public partial class PG_Comment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String l_strTemplateName = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];
                String l_strStepName = Request.QueryString[ConstString.QueryString.STEP_NAME];
                String l_strProcessID = Request.QueryString[ConstString.QueryString.PROCESS_ID];
                String l_strWorkItemID = Request.QueryString[ConstString.QueryString.WORKITEM_ID];

                LoadComments(l_strProcessID, l_strTemplateName, l_strStepName, l_strWorkItemID);
            }
        }

        private void LoadComments(String p_strProcessID, String p_strTemplateName, String p_strStepName, String p_strWorkItemID)
        {
            DataTable l_dtbDataTable = FormsMethod.GetSignInfo(p_strProcessID, p_strWorkItemID,p_strTemplateName, p_strStepName);

            if (l_dtbDataTable != null && l_dtbDataTable.Rows.Count != 0)
            {
                rptComments.DataSource = l_dtbDataTable;
                rptComments.DataBind();
            }
            else
            {
                MsgLabel.Text = "目前没有任何意见！";
            }
        }
    }
}