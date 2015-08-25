using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Process
{
    public partial class PG_ProcessRelation : System.Web.UI.Page
    {
        /// <summary>
        /// 流程类型
        /// </summary>
        private string ProcessType
        {
            get
            {
                if (ViewState["ProcessType"] == null)
                {
                    ViewState["ProcessType"] = Request.QueryString["ProcessType"];
                }
                return ViewState["ProcessType"].ToString();
            }
        }

        /// <summary>
        /// 流程ID
        /// </summary>
        private string ProcessID
        {
            get
            {
                if (ViewState["ProcessID"] == null)
                {
                    ViewState["ProcessID"] = Request.QueryString["ProcessID"];
                }
                return ViewState["ProcessID"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvProcessList.DataSource = B_ProcessInstance.GetRelationProcess(this.ProcessType, this.ProcessID);

            this.gvProcessList.DataBind();
        }

        protected String GetProcessStatus(Object p_strStatusName)
        {
            String l_strProcessStatus = "";
            switch (p_strStatusName.ToString())
            {
                case ProcessConstString.ProcessStatus.STATUS_RUNNING: l_strProcessStatus = "<font style='color:green'>运行中</font>"; break;
                case ProcessConstString.ProcessStatus.STATUS_COMPLETED: l_strProcessStatus = "<font style='color:red'>已完成</font>"; break;
                case ProcessConstString.ProcessStatus.STATUS_CANCELED: l_strProcessStatus = "<font style='color:blue'>已取消</font>"; break;
                case ProcessConstString.ProcessStatus.STATUS_SUSPENDED: l_strProcessStatus = "<font style='color:purple'>已暂停</font>"; break;
                default: break;
            }
            return l_strProcessStatus;
        }

        protected void gvProcessList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            e.Row.Cells[1].Text = GetProcessStatus(l_drvRowView["Status"]);
            if (l_drvRowView["ProcessID"].ToString() == this.ProcessID)
            {
                e.Row.BackColor = System.Drawing.Color.YellowGreen;
                e.Row.ToolTip = "当前函件";
            }
        }
    }
}
