using System;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.Business;
using FounderSoftware.ADIM.OA.OA2DC;
using FounderSoftware.ADIM.OA.OA2DP;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Process
{
    public partial class UC_ProcessDevolve : ListUIBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定流程类型
                LoadProcessTemplate();

                //加载归档列表
                LoadDevelveList();
            }
        }

        /// <summary>
        /// 初始化流程类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            String[] processType = TableName.GetAllProcessDisplayName();
            String[] l_strAryProcessType = TableName.GetAllWorkItemTableName();

            ddlProcessTemplate.Items.Add(new ListItem("", ""));
            for (int i = 0; i < processType.Length; i++)
            {
                ddlProcessTemplate.Items.Add(new ListItem(SysString.GetPTDisplayName(processType[i]), l_strAryProcessType[i]));
            }
        }

        /// <summary>
        /// 加载归档列表
        /// </summary>
        private void LoadDevelveList()
        {
            if (ddlProcessTemplate.SelectedItem == null)
            {
                return;
            }

            ViewOADevolveHistory view = new ViewOADevolveHistory(this.ddlProcessTemplate.SelectedValue,ddlProcessTemplate.SelectedItem.Text);

            view.Page.Enabled = true;
            view.Page.PageSize = gvProcessList.PageSize;
            gvProcessList.RecordCount = view.Page.RecordCount;
            view.Page.GoTo(this.gvProcessList.PageIndex + 1);

            this.gvProcessList.DataSource = view.DtTable;
            this.gvProcessList.DataBind();
        }

        protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDevelveList();
        }

        /// <summary>
        /// 跳转到详细页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnView_Command(object sender, CommandEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                String l_strTemplateName = "";
                if (e.CommandName.Contains("函件发文") || e.CommandName.Contains("函件收文"))
                {
                    l_strTemplateName = "新版" + e.CommandName;
                }
                else
                {
                    l_strTemplateName = e.CommandName;
                }
                String[] result = (e.CommandArgument as String).Split(',');
                String url = String.Format(@"~/Container.aspx?TemplateName={0}&TBID={1}&StepName={2}&ProcessID={3}&WorkItemID={4}&IsHistory=1", Server.UrlEncode(l_strTemplateName), result[0], Server.UrlEncode(result[1]), result[2], result[3]);
                this.Response.Redirect(url);
            }
        }

        protected void gvProcessList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gvProcessList_ExteriorPaging(GridViewPageEventArgs e)
        {
            LoadDevelveList();
        }

        protected void gvProcessList_ExteriorSorting(FSGridViewSortEventArgs e)
        {
            SortExpression = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending)
            {
                SortExpression += " ASC";
            }
            else
            {
                SortExpression += " DESC";
            }
            LoadDevelveList();
        }
    }
}