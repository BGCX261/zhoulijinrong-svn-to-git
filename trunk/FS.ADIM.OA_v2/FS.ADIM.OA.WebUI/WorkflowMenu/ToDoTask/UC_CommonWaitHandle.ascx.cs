//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：公办
// 
// 
// 创建标识：
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//--------------------------------------------------------------
using System;
using System.Data;
using System.Web.UI.WebControls;
using Ascentn.Workflow.Base;
using FounderSoftware.Framework.UI.WebCtrls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;
using FS.OA.Framework;
using FS.OA.Framework.WorkFlow;
using FS.OA.Framework.WorkFlow.AgilePoint;
using FS.ADIM.OA.WebUI.WorkflowMenu.Circulate;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask
{
    public partial class UC_CommonWaitHandle : ListUIBase
    {
        private TemplateAdmin TAdmin;

        #region 页面加载
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

                //绑定公办文件数据
                LoadTaskList();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void LoadTaskList()
        {
            //当前登录用户账号
            String l_strUserName = CurrentUserInfo.UserName;

            //得到检索条件           
            M_EntityMenu l_entityCommonTask = GetSearchCondition();

            B_CommonTaskFile l_busCommonTaskList = new B_CommonTaskFile();

            l_entityCommonTask.Start = gvTaskList.PageIndex * gvTaskList.PageSize;
            l_entityCommonTask.End = gvTaskList.PageIndex * gvTaskList.PageSize + gvTaskList.PageSize;
            l_entityCommonTask.Sort = SortExpression;

            //得到待办文件任务列表数据
            DataTable l_dtbDataTable = l_busCommonTaskList.GetCommonWaitingHandleList(l_entityCommonTask);

            //绑定数据
            this.gvTaskList.RecordCount = l_entityCommonTask.RowCount;
            this.gvTaskList.DataSource = l_dtbDataTable;
            this.gvTaskList.DataBind();
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

            //流程类型
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

        protected void gvTaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            HyperLink l_hlkView = e.Row.FindControl("hlkView") as HyperLink;
            HyperLink l_hlkHandle = e.Row.FindControl("hlkHandle") as HyperLink;
            LinkButton l_lbnRetrieve = e.Row.FindControl("lbnRetrieve") as LinkButton;
            LinkButton l_lbnRestore = e.Row.FindControl("lbnRestore") as LinkButton;
            Label l_lblInfo = e.Row.FindControl("lblInfo") as Label;

            String l_strStatus = l_drvRowView["STATUS"].ToString();
            String l_strProcInstID = l_drvRowView["PROC_INST_ID"].ToString();
            String l_strWorkItemID = l_drvRowView["WORK_ITEM_ID"].ToString();
            String l_strTemplateName = l_drvRowView["PDEF_NAME"].ToString();
            String l_strIdentityID = l_drvRowView["TBID"].ToString();
            String l_strPoolID = l_drvRowView["POOL_ID"].ToString();
            String l_strUserID = l_drvRowView["USER_ID"].ToString();
            String l_strStepName = l_drvRowView["STEP_NAME"].ToString();

            String l_strTargetUrl = String.Format(@"~/Container.aspx?ProcessID={0}&WorkItemID={1}&TemplateName={2}&StepName={3}&TBID={4}&MS=2", l_strProcInstID, l_strWorkItemID, l_strTemplateName, l_strStepName, l_strIdentityID);
            String l_strViewTargetUrl = ""; //查看


            if (l_strStatus == WFManualWorkItem.NEW) //待获取
            {
                l_lbnRetrieve.Visible = true;
            }
            else if (l_strStatus == WFManualWorkItem.ASSIGNED) //待处理
            {
                l_hlkHandle.Visible = true;

                l_lbnRestore.Visible = true;
            }
            else if (l_strStatus == WFManualWorkItem.REMOVED)
            {
                l_lblInfo.Visible = true;
                l_lblInfo.Text = "他人获取";

                l_hlkView.Visible = true;
                l_strViewTargetUrl = String.Format(@"~/Container.aspx?ProcessID={0}&WorkItemID={1}&TemplateName={2}&StepName={3}&TBID={4}&IsHistory=1&IsGongBan=1&MS=2", l_strProcInstID, l_strWorkItemID, l_strTemplateName, l_strStepName, l_strIdentityID);

            }

            l_hlkHandle.NavigateUrl = l_strTargetUrl;
            l_lbnRetrieve.CommandName = l_strWorkItemID;
            l_lbnRetrieve.CommandArgument = l_strTemplateName;
            l_lbnRestore.CommandName = l_strWorkItemID;
            l_lbnRestore.CommandArgument = l_strTemplateName;
            l_hlkView.NavigateUrl = l_strViewTargetUrl;

            DistinctUrgentDegree(l_drvRowView["UrgentDegree"], e.Row.Cells[0]);

            IndicateNoData(l_drvRowView["MARK"], e.Row);
        }
        #endregion

        /// <summary>
        /// 初始化流程类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            String[] processType = TableName.GetAllProcessTemplateName();

            ddlProcessTemplate.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
            for (int i = 0; i < processType.Length; i++)
            {
                ddlProcessTemplate.Items.Add(new System.Web.UI.WebControls.ListItem(SysString.GetPTDisplayName(processType[i]), processType[i]));
            }
            this.ddlStepName.Enabled = false;
            this.ddlStepName.Items.Clear();
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
        /// 流程类型选择
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
                String strTemplateID = this.ddlProcessTemplate.SelectedValue;
                DataTable dtStep = TAdmin.Templates.GetTemplate(strTemplateID).GetVersion(1).GetViewList();

                //加入空选择行
                DataRow l_dtrDataRow = dtStep.NewRow();
                l_dtrDataRow[0] = DBNull.Value;
                l_dtrDataRow[1] = "";
                l_dtrDataRow[2] = DBNull.Value;

                dtStep.Rows.InsertAt(l_dtrDataRow, 0);

                //绑定数据
                this.ddlStepName.DataSource = dtStep;
                this.ddlStepName.DataBind();
            }
            LoadTaskList();
        }

        #region 获取 归还
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                String l_strWorkItemID = (sender as LinkButton).CommandName;
                String l_strTemplateName = (sender as LinkButton).CommandArgument;

                AgilePointWF ag = new AgilePointWF();

                WorkflowService l_objWorkFlowService = ag.GetAPI();

                FSWFManualWorkItem l_objManaulWorkItem = ag.GetWorkItem(l_strWorkItemID);

                if (l_objManaulWorkItem.Status == ProcessConstString.StepStatus.STATUS_NEW)
                {
                    l_objWorkFlowService.AssignWorkItem(l_strWorkItemID);
                    System.Threading.Thread.Sleep(500);

                    String sqlAP = String.Format("SELECT WORK_ITEM_ID FROM WF_MANUAL_WORKITEMS WHERE POOL_ID='{0}'", l_objManaulWorkItem.PoolID);
                    DataTable l_dtbDataTable = SQLHelper.GetDataTable2(sqlAP);
                    String l_strOthersWorkItemID = String.Empty;
                    String l_strOwnWorkItemID = String.Empty;
                    for (int i = 0; i < l_dtbDataTable.Rows.Count; i++)
                    {
                        if (i != 0)
                        {
                            l_strOthersWorkItemID += ",";
                        }
                        l_strOthersWorkItemID += "'" + l_dtbDataTable.Rows[i]["WORK_ITEM_ID"].ToString() + "'";

                        if (l_objManaulWorkItem.WorkItemID != l_dtbDataTable.Rows[i]["WORK_ITEM_ID"].ToString())
                        {
                            if (l_strOwnWorkItemID != "")
                            {
                                l_strOwnWorkItemID += ",";
                            }
                            l_strOwnWorkItemID += "'" + l_dtbDataTable.Rows[i]["WORK_ITEM_ID"].ToString() + "'";
                        }
                    }

                    //获得表名
                    String l_strTableName = TableName.GetWorkItemsTableName(l_strTemplateName);

                    String l_strExpression = String.Format("UPDATE {0} SET Is_Common=1 WHERE WorkItemID IN ({2})", l_strTableName, l_objManaulWorkItem.PoolID, l_strOthersWorkItemID);

                    if (l_strOwnWorkItemID != "")
                    {
                        l_strExpression += String.Format(";UPDATE {0} SET D_StepStatus='{1}' WHERE WorkItemID in ({2})", l_strTableName, ProcessConstString.StepStatus.STATUS_REMOVED, l_strOwnWorkItemID);
                    }
                    l_strExpression += String.Format(";UPDATE {0} SET D_StepStatus='{1}' WHERE WorkItemID ='{2}'", l_strTableName, ProcessConstString.StepStatus.STATUS_ASSIGNED, l_objManaulWorkItem.WorkItemID);

                    int ret = FounderSoftware.Framework.Business.Entity.RunNoQuery(l_strExpression);
                    if (ret > 0)
                    {
                        LoadTaskList();
                    }
                }
                else
                {
                    IMessage im = new WebFormMessage(Page, String.Format("这个任务已经被'{0}'接收了。", l_objManaulWorkItem.UserID));
                    im.Show();

                    LoadTaskList();
                }
            }
            catch (Exception ex)
            {
                IMessage im = new WebFormMessage(Page, ex.ToString());
                im.Show();
            }
        }
        /// <summary>
        /// 归还任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                String l_strWorkItemID = (sender as LinkButton).CommandName;
                String l_strTemplateName = (sender as LinkButton).CommandArgument;
                AgilePointWF ag = new AgilePointWF();

                WorkflowService l_objWorkFlowService = ag.GetAPI();

                FSWFManualWorkItem l_objManaulWorkItem = ag.GetWorkItem(l_strWorkItemID);
                if (l_objManaulWorkItem.Status == ProcessConstString.StepStatus.STATUS_ASSIGNED && String.Compare(CurrentUserInfo.UserName, l_objManaulWorkItem.UserID, true) == 0)
                {
                    l_objWorkFlowService.UndoAssignWorkItem(l_strWorkItemID);
                    System.Threading.Thread.Sleep(500);

                    String l_strTableName = TableName.GetWorkItemsTableName(l_strTemplateName);
                    String sql = String.Format(";UPDATE {0} SET D_StepStatus='{1}' WHERE CommonID ='{2}'", l_strTableName, ProcessConstString.StepStatus.STATUS_NEW, l_objManaulWorkItem.PoolID);
                    int ret = FounderSoftware.Framework.Business.Entity.RunNoQuery(sql);
                    if (ret > 0)
                    {
                        LoadTaskList();
                    }
                }
                else
                {
                    IMessage im = new WebFormMessage(Page, String.Format("这个任务已经被'{0}'接收了。", l_objManaulWorkItem.UserID));
                    im.Show();
                    LoadTaskList();
                }
            }
            catch (Exception ex)
            {
                IMessage im = new WebFormMessage(Page, ex.ToString());
                im.Show();
            }
        }
        #endregion

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