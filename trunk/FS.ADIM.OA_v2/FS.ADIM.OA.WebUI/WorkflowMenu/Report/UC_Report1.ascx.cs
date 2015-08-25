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
using FS.OA.Framework;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.WebUI.UIBase;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Report
{
    public partial class UC_Report1 : UCBase
    {
        private string styleRed1 = "<font style='color:Red;'>";
        private string styleRed2 = "</font>";

        private string styleBlue1 = "<font style='color:Blue;'>";
        private string styleBlue2 = "</font>";

        private string styleSilver1 = "<font style='color:Purple;'>";
        private string styleSilver2 = "</font>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProcessTemplate();
            }
        }
        private void LoadProcessTemplate()
        {
            string sqlDEF = "SELECT DEF_Name FROM dbo.WF_PROC_DEFS WHERE STATUS='Released'";
            DataTable dtDEF = SQLHelper.GetDataTable2(sqlDEF);

            ddlFlowType.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dtDEF.Rows)
            {
                ddlFlowType.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
            }
        }
        private void GetALLProcess()
        {
            string sInfo = "";

            if (ddlFlowType.SelectedValue == "")
            {
                string sqlDEF = "SELECT DEF_Name FROM dbo.WF_PROC_DEFS WHERE STATUS='Released'";
                DataTable dtDEF = SQLHelper.GetDataTable2(sqlDEF);

                foreach (DataRow dr in dtDEF.Rows)
                {
                    string def = dr[0].ToString();
                    string sqlP = string.Format("SELECT Count(*) FROM WF_PROC_INSTS WHERE DEF_Name='{0}' ", def);
                    sqlP += GetWhere();
                    DataTable dtP = SQLHelper.GetDataTable2(sqlP);
                    if (dtP.Rows.Count > 0)
                    {
                        sInfo += def + " " + dtP.Rows[0][0].ToString() + "条。<br/>";
                    }
                }
            }
            else
            {
                #region 根据流程
                string def = ddlFlowType.SelectedValue;
                string sqlP = string.Format("SELECT Count(*) FROM WF_PROC_INSTS WHERE DEF_Name='{0}' ", def);
                sqlP += GetWhere();
                DataTable dtP = SQLHelper.GetDataTable2(sqlP);
                if (dtP.Rows.Count > 0)
                {
                    sInfo += def + " " + dtP.Rows[0][0].ToString() + "条。<br/>";
                }
                string[] pStatus = new string[] { "Running", "Completed", "Cancelled", "Suspended" };
                string[] pStatusCN = new string[] { "运行中", "已完成", "已取消","已暂停" };
                for (int pp = 0; pp < pStatus.Length; pp++)
                {
                    string sqlRunning = sqlP + string.Format(" AND STATUS='{0}'", pStatus[pp]);
                    DataTable dtRunning = SQLHelper.GetDataTable2(sqlRunning);
                    if (dtRunning.Rows.Count > 0)
                    {
                        sInfo += pStatusCN[pp] + "：" + dtRunning.Rows[0][0].ToString() + "条 ";
                    }
                }
                #endregion


                sInfo += "<br/><br/>";
                //根据步骤
                TemplateAdmin TAdmin = new TemplateAdmin();
                DataTable dtStep = TAdmin.Templates.GetTemplate(def).GetVersion(1).GetViewList();

                sInfo += "<Table border='1'>";

                foreach (DataRow dr in dtStep.Rows)
                {
;                    //Removed Completed Assigned New
                    string[] stepStatus = new string[] { "Completed", "Assigned", "New", "Removed", "Overdue", "Carbon", "Reassigned" };
                    string[] stepStatusCN = new string[] { "已完成", "待处理", "待获取", "已移除", "已过期", "已暂停", "重新指派" };
                    string step = dr["Name"].ToString();

                    sInfo += "<tr>";
                    sInfo += "<td>";
                    sInfo += styleRed1 + step + styleRed2 + " ";
                    sInfo += "</td>";
                    for (int kk = 0; kk < stepStatus.Length; kk++)
                    {
                        string sql = string.Format(@"SELECT Count(*) FROM WF_MANUAL_WORKITEMS a INNER JOIN
WF_PROC_INSTS b ON a.PROC_INST_ID=b.PROC_INST_ID
WHERE a.NAME='{0}' AND a.Status='{1}' ", step, stepStatus[kk]);
                        DataTable dt = SQLHelper.GetDataTable2(sql);
                        if (dt.Rows.Count > 0)
                        {
                            sInfo += "<td>";
                            if (stepStatusCN[kk] == "已完成")
                            {
                                sInfo += styleBlue1;
                            }
                            else if (stepStatusCN[kk] == "待处理" || stepStatusCN[kk] == "待获取")
                            {
                                sInfo += styleSilver1;
                            }
                            sInfo += stepStatusCN[kk] + " " + dt.Rows[0][0].ToString() + "条";
                            if (stepStatusCN[kk] == "已完成")
                            {
                                sInfo += styleBlue2;
                            }
                            else if (stepStatusCN[kk] == "待处理" || stepStatusCN[kk] == "待获取")
                            {
                                sInfo += styleSilver2;
                            }
                            sInfo += "</td>";
                        }
                    }
                    sInfo += "</tr>";
                }
                sInfo += "</Table>";
            }
            lblInfo.Text = sInfo;
        }
        private string GetWhere()
        {
            string where = "";
            if (this.cldStartDate.Text != "")
            {
                where = string.Format(" AND STARTED_DATE>='{0}'", this.cldStartDate.Text.ToString());
            }
            if (this.cldEndDate.Text != "")
            {
                where = string.Format(" AND STARTED_DATE<='{0}' ", this.cldEndDate.ValDate.AddDays(1).ToShortDateString());
            }
            return where;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetALLProcess();
        }
    }
}