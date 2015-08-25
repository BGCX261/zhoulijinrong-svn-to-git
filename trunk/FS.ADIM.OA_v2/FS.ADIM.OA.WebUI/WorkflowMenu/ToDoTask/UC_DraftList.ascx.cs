using System;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask
{
    /// <summary>
    /// 草稿箱
    /// </summary>
    public partial class UC_DraftList : ListUIBase
    {
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

                //绑定草稿文件列表
                LoadDraftList();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void LoadDraftList()
        {
            //当前登录用户账号
            String l_strUserName = CurrentUserInfo.UserName;

            //得到检索条件
            M_EntityMenu l_entityDraft = GetSearchCondition();

            B_DraftBox l_busDraftBox = new B_DraftBox();

            l_entityDraft.Start = gvDraftList.PageIndex * gvDraftList.PageSize;
            l_entityDraft.End = gvDraftList.PageIndex * gvDraftList.PageSize + gvDraftList.PageSize;
            l_entityDraft.Sort = SortExpression;

            //得到草稿文件列表
            DataTable l_dtbDataTable = l_busDraftBox.GetDraftList(l_entityDraft);

            //绑定数据
            this.gvDraftList.RecordCount = l_entityDraft.RowCount;
            this.gvDraftList.DataSource = l_dtbDataTable;
            this.gvDraftList.DataBind();
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
            LoadDraftList();
        }

        /// <summary>
        /// 初始化流程类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            String[] processType = FS.ADIM.OA.BLL.Common.TableName.GetAllProcessTemplateName();

            ddlProcessTemplate.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
            for (int i = 0; i < processType.Length; i++)
            {
                ddlProcessTemplate.Items.Add(new ListItem(SysString.GetPTDisplayName(processType[i]), processType[i]));
            }
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
                m_strAryMessages.Add("接收日期:开始日期格式不正确！例:XXXX-XX-XX 或 XXXX/XX/XX");
                l_blnIsValid = false;
            }
            if (!String.IsNullOrEmpty(this.txtEndDate.ValStr.TrimEnd()) && !ValidateUtility.IsDateTime(this.txtEndDate.ValStr.TrimEnd()))
            {
                m_strAryMessages.Add("接收日期:结束日期格式不正确！例:XXXX-XX-XX 或 XXXX/XX/XX");
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
                    m_strAryMessages.Add("接收日期:开始日期必须小于等于结束日期");
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

            //文件标题
            l_entQueryCondition.DocumentTitle = FormsMethod.Filter(this.txtDocumentTitle.Text);

            //发起日期-开始
            l_entQueryCondition.StartTime = this.txtStartDate.ValDate.Date;

            //发起日期-结束
            l_entQueryCondition.EndTime = this.txtEndDate.ValDate.Date;

            return l_entQueryCondition;
        }

        //删除
        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            LinkButton l_lbnDelete = sender as LinkButton;
            String id = l_lbnDelete.CommandArgument;
            String l_strTemplateName = l_lbnDelete.CommandName;

            String l_strWorkItemTable = TableName.GetWorkItemsTableName(l_strTemplateName);
            if (!String.IsNullOrEmpty(l_strWorkItemTable))
            {
                String l_strExpression = "DELETE FROM " + l_strWorkItemTable + " WHERE ID = " + id;
                int ret = FounderSoftware.Framework.Business.Entity.RunNoQuery(l_strExpression);
                if (ret > 0)
                    LoadDraftList();
            }
        }

        protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //检查查询条件数据有效性
            if (ValidateQueryCondition())
            {
                LoadDraftList();
            }
        }

        protected void gvDraftList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            DistinctUrgentDegree(l_drvRowView["UrgentDegree"], e.Row.Cells[0]);
        }

        protected void gvDraftList_ExteriorPaging(GridViewPageEventArgs e)
        {
            LoadDraftList();
        }

        protected void gvDraftList_ExteriorSorting(FSGridViewSortEventArgs e)
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
            LoadDraftList();
        }
    }
}