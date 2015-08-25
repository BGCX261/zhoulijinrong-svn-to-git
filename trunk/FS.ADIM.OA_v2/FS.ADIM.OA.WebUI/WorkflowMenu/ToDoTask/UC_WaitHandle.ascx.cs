using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.WebUI.WorkflowMenu.Circulate;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask
{
    /// <summary>
    /// 待办文件
    /// </summary>
    public partial class UC_WaitHandle : ListUIBase
    {
        private TemplateAdmin TAdmin;

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定流程类型
                LoadProcessTemplate();

                //绑定待办文件列表
                LoadTaskList();
            }
        }

        /// <summary>
        /// 初始化流程类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            String[] processType = TableName.GetAllProcessDisplayName();
            String[] l_strAryProcessType = TableName.GetAllProcessTemplateName();

            ddlProcessTemplate.Items.Add(new ListItem("", ""));
            for (int i = 0; i < processType.Length; i++)
            {
                ddlProcessTemplate.Items.Add(new ListItem(SysString.GetPTDisplayName(processType[i]), l_strAryProcessType[i]));
            }
            this.ddlStepName.Enabled = false;
            this.ddlStepName.Items.Clear();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void LoadTaskList()
        {
            //当前登录用户账号
            String l_strUserName = CurrentUserInfo.UserName;

            //得到检索条件
            M_EntityMenu l_entityTask = GetSearchCondition();

            B_TaskFile l_busTaskList = new B_TaskFile();

            l_entityTask.Start = gvTaskList.PageIndex * gvTaskList.PageSize;
            l_entityTask.End = gvTaskList.PageIndex * gvTaskList.PageSize + gvTaskList.PageSize;
            l_entityTask.Sort = SortExpression;

            //得到待办文件任务列表数据
            DataTable l_dtbDataTable = l_busTaskList.GetWaitingHandleList(l_entityTask);

            //绑定数据
            this.gvTaskList.RecordCount = l_entityTask.RowCount;
            this.gvTaskList.DataSource = l_dtbDataTable;
            this.gvTaskList.DataBind();
        }


        /// <summary>
        /// 查询按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //检查查询条件数据有效性
            if (!ValidateQueryCondition())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }
            LoadTaskList();
        }

        /// <summary>
        /// 检查查询条件数据有效性
        /// </summary>
        /// <returns></returns>
        private bool ValidateQueryCondition()
        {
            Boolean l_blnIsValid = true;
            if (!String.IsNullOrEmpty(this.txtStartDate.ValStr.TrimEnd()) && !ValidateUtility.IsDateTime(this.txtStartDate.ValStr.TrimEnd()))
            {
                m_strAryMessages.Add("发起日期:开始日期格式不正确！例:XXXX-XX-XX 或 XXXX/XX/XX");
                l_blnIsValid = false;
            }
            if (!String.IsNullOrEmpty(this.txtEndDate.ValStr.TrimEnd()) && !ValidateUtility.IsDateTime(this.txtEndDate.ValStr.TrimEnd()))
            {
                m_strAryMessages.Add("发起日期:结束日期格式不正确！例:XXXX-XX-XX 或 XXXX/XX/XX");
                l_blnIsValid = false;
            }
            if (!l_blnIsValid)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(this.txtStartDate.ValStr.TrimEnd()) && !String.IsNullOrEmpty(this.txtEndDate.ValStr.TrimEnd()))
            {
                if (this.txtStartDate.ValDate > this.txtEndDate.ValDate)
                {
                    m_strAryMessages.Add("发起日期:开始日期必须小于等于结束日期");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 得到检索条件数据实体
        /// </summary>
        /// <returns>检索条件数据实体</returns>
        private M_EntityMenu GetSearchCondition()
        {
            M_EntityMenu l_entQueryCondition = new M_EntityMenu();

            //当前登陆的用户
            l_entQueryCondition.LoginUserID = CurrentUserInfo.UserName;

            //流程模版名称
            l_entQueryCondition.TemplateName = this.ddlProcessTemplate.SelectedValue;

            //当前步骤
            l_entQueryCondition.StepName = this.ddlStepName.SelectedValue;

            //文件标题
            l_entQueryCondition.DocumentTitle = FormsMethod.Filter(this.txtDocumentTitle.Text);

            //发起人
            l_entQueryCondition.Initiator = FormsMethod.Filter(this.txtSponsor.Text);

            //发起日期-开始
            l_entQueryCondition.StartTime = this.txtStartDate.ValDate.Date;

            //发起日期-结束
            l_entQueryCondition.EndTime = this.txtEndDate.ValDate.Date;

            return l_entQueryCondition;
        }

        /// <summary>
        /// 流程类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlProcessTemplate.SelectedIndex == 0)
            {
                //流程类型选中空行，设置流程步骤不可编辑
                this.ddlStepName.Enabled = false;
                this.ddlStepName.Items.Clear();
            }
            else
            {
                //选中一个流程类型，设置流程步骤为可编辑
                this.ddlStepName.Enabled = true;
                //得到流程类型对应的流程步骤
                TAdmin = new TemplateAdmin();
                String l_strTemplateName = this.ddlProcessTemplate.SelectedValue;
                DataTable l_dtbDataTable = TAdmin.Templates.GetTemplate(l_strTemplateName).GetVersion(1).GetViewList();

                //加入空选择行
                DataRow l_dtrDataRow = l_dtbDataTable.NewRow();
                l_dtrDataRow[0] = DBNull.Value;
                l_dtrDataRow[1] = "";
                l_dtrDataRow[2] = DBNull.Value;

                l_dtbDataTable.Rows.InsertAt(l_dtrDataRow, 0);

                //绑定数据
                this.ddlStepName.DataSource = l_dtbDataTable;
                this.ddlStepName.DataBind();
            }
            LoadTaskList();
        }

        protected void gvTaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            DistinctUrgentDegree(l_drvRowView["UrgentDegree"], e.Row.Cells[0]);

            IndicateNoData(l_drvRowView["MARK"], e.Row);
        }
        protected void gvTaskList_ExteriorPaging(GridViewPageEventArgs e)
        {
            LoadTaskList();
        }

        protected void gvTaskList_ExteriorSorting(FSGridViewSortEventArgs e)
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
            LoadTaskList();
        }
    }
}