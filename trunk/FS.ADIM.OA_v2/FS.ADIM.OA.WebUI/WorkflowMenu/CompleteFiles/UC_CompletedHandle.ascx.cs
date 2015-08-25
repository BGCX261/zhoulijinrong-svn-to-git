using System;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebCtrls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.WebUI.WorkflowMenu.Circulate;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.OA.Framework;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.CompleteFiles
{
    public partial class UC_CompletedHandle : ListUIBase
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
            M_CompleteFile l_entityTask = GetSearchCondition();

            B_CompletedTaskFile l_busTaskList = new B_CompletedTaskFile();

            l_entityTask.Start = gvTaskList.PageIndex * gvTaskList.PageSize;
            l_entityTask.End = gvTaskList.PageIndex * gvTaskList.PageSize + gvTaskList.PageSize;
            l_entityTask.Sort = SortExpression;

            if (!chkIsCurrentWare.Checked && !chkIsHistoryWare.Checked)
            {
                this.gvTaskList.RecordCount = 0;
                this.gvTaskList.DataSource = new DataTable();
                this.gvTaskList.DataBind();
                return;
            }

            //得到已办文件任务列表
            DataTable l_dtbDataTable = l_busTaskList.GetAllCompleteHandled(l_entityTask);

            //流程代理添加（流程代理开启，显示代理人项）
            this.gvTaskList.Columns[10].Visible = OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" ? true : false;

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

        private void VisibleTR()
        {
            trGF.Visible = false;
            trGS.Visible = false;
            trHS.Visible = false;
            trHF.Visible = false;
            trPF.Visible = false;
            trWR.Visible = false;
            trRR.Visible = false;
        }
        private void LoadProcessSteps()
        {
            if (this.ddlProcessTemplate.SelectedIndex == 0)
            {
                ////流程类型选中空行，设置流程步骤不可编辑
                //this.ddlStepName.Enabled = false;
                //this.ddlStepName.Items.Clear();

                VisibleTR();
            }
            else
            {
                VisibleTR();
                switch (ddlProcessTemplate.SelectedValue)
                {
                    case ProcessConstString.TemplateName.DJGT_Send:
                    case ProcessConstString.TemplateName.COMPANY_SEND: trGF.Visible = true; break;
                    case ProcessConstString.TemplateName.MERGED_RECEIVE:
                    case ProcessConstString.TemplateName.COMPANY_RECEIVE: 
                    //trGS.Visible = true; 
                    break;
                    case ProcessConstString.TemplateName.LETTER_RECEIVE: trHS.Visible = true; break;
                    case ProcessConstString.TemplateName.LETTER_SEND: trHF.Visible = true; break;
                    case ProcessConstString.TemplateName.PROGRAM_FILE: trPF.Visible = true; break;
                    case ProcessConstString.TemplateName.AFFILIATION: trWR.Visible = true; break;
                    case ProcessConstString.TemplateName.INSTUCTION_REPORT: trRR.Visible = true; break;
                    default: break;
                }
                //选中一个流程类型，设置流程步骤为可编辑
                //this.ddlStepName.Enabled = true;
                //得到流程类型对应的流程步骤
                TemplateAdmin TAdmin = new TemplateAdmin();
                String strTemplateID = this.ddlProcessTemplate.SelectedValue;
                DataTable dtStep = TAdmin.Templates.GetTemplate(strTemplateID).GetVersion(1).GetViewList();

                //加入空选择行
                DataRow drStep = dtStep.NewRow();
                drStep[0] = DBNull.Value;
                drStep[1] = "";
                drStep[2] = DBNull.Value;

                dtStep.Rows.InsertAt(drStep, 0);

                ////绑定数据
                //this.ddlStepName.DataSource = dtStep;
                //this.ddlStepName.DataBind();
            }
        }
        /// <summary>
        /// 流程类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProcessSteps();

            if (this.ddlProcessTemplate.SelectedIndex == 0)
            {
                this.ddlStepName.Enabled = false;
                this.ddlStepName.Items.Clear();
            }
            else
            {
                this.ddlStepName.Enabled = true;

                //得到流程类型对应的流程步骤
                TemplateAdmin TAdmin = new TemplateAdmin();
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

            #region 根据流程类型绑定特有字段
            switch (this.ddlProcessTemplate.SelectedValue)
            {
                //公司发文
                case ProcessConstString.TemplateName.COMPANY_SEND:
                case ProcessConstString.TemplateName.DJGT_Send://renjinquan+
                    OADept.GetDeptByIfloor(drpGFDept, 1);
                    break;
                //公司收文
                case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                case ProcessConstString.TemplateName.MERGED_RECEIVE://renjinquan+
                    break;
                //函件收文
                case ProcessConstString.TemplateName.LETTER_RECEIVE:
                    OAList.BindHJLX2(ddlHSLetterType, true);
                    break;
                //函件发文
                case ProcessConstString.TemplateName.LETTER_SEND:
                    OAList.BindHJLX(ddlHFLetterType, true);
                    OADept.GetDeptByIfloor(this.ddlHFDept, 1);
                    break;
                //程序文件
                case ProcessConstString.TemplateName.PROGRAM_FILE:
                    OADept.GetDeptByIfloor(this.ddlPFDept, 1);
                    break;
                //工作联系单
                case ProcessConstString.TemplateName.AFFILIATION:
                    OADept.GetDeptByIfloor(this.ddlWRHostDept, 1);
                    OADept.GetDeptByIfloor(this.ddlWRMainSendDept, 1);
                    break;
                //请示报告
                case ProcessConstString.TemplateName.INSTUCTION_REPORT:
                    OAUser.GetUserByRole(this.ddlMainSendleader, OUConstString.RoleName.COMPANY_LEADER);
                    OADept.GetDeptByIfloor(this.ddlUnderTakeDept, 1);
                    OADept.GetDeptByIfloor(this.ddlHostDept, 1);
                    break;
            }
            #endregion
            //chenye
            //LoadProcessList();
        }
        /// <summary>
        /// 得到检索条件数据实体
        /// </summary>
        /// <returns>检索条件数据实体</returns>
        private M_CompleteFile GetSearchCondition()
        {
            M_CompleteFile l_entQueryCondition = new M_CompleteFile();

            //当前登陆的用户
            l_entQueryCondition.LoginUserID = CurrentUserInfo.UserName;

            //流程模版名称
            l_entQueryCondition.TemplateName = this.ddlProcessTemplate.SelectedValue;

            //步骤名称
            l_entQueryCondition.StepName = this.ddlStepName.SelectedValue;

            //文件标题
            l_entQueryCondition.DocumentTitle = FormsMethod.Filter(txtDocumentTitle.Text);

            //发起人
            l_entQueryCondition.Initiator = FormsMethod.Filter(txtSponsor.Text);

            //发起日期-开始
            l_entQueryCondition.StartTime = this.txtStartDate.ValDate.Date;

            //发起日期-结束
            l_entQueryCondition.EndTime = this.txtEndDate.ValDate.Date;

            //是否已办
            l_entQueryCondition.SingleHandled = chkHandled.Checked;

            //是否自己公办
            l_entQueryCondition.OwnCommonHandled = chkOwnCommon.Checked;

            //是否他人公办
            l_entQueryCondition.OtherCommonHandled = chkOtherCommon.Checked;

            //是否已阅
            l_entQueryCondition.HaveRead = chkRead.Checked;

            //是否承办
            l_entQueryCondition.UnderTake = chkUnderTake.Checked;
            if (chkUnderTake.Checked)
            {
                l_entQueryCondition.UnderTakeStatus = ddlUnderTakeStatus.SelectedValue;
            }

            //是否现行库
            l_entQueryCondition.IsCurrentWare = chkIsCurrentWare.Checked;

            //是否历史库
            l_entQueryCondition.IsHistoryWare = chkIsHistoryWare.Checked;

            //chen
            //文号
            l_entQueryCondition.DocumentNo = FormsMethod.Filter(txtDocumentNo.Text);
            switch (this.ddlProcessTemplate.SelectedValue)
            {
                //公司发文
                case ProcessConstString.TemplateName.COMPANY_SEND:
                case ProcessConstString.TemplateName.DJGT_Send://renjinquan+
                    l_entQueryCondition.GFHostDept = drpGFDept.SelectedValue;
                    l_entQueryCondition.GFHostDeptName = drpGFDept.SelectedItem.Text;
                    l_entQueryCondition.GFMainSenders = FormsMethod.Filter(txtGFZhuSongDanWei.Text);
                    break;

                //公司收文
                case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                case ProcessConstString.TemplateName.MERGED_RECEIVE://renjinquan+
                    l_entQueryCondition.GSReceiveUnit = FormsMethod.Filter(txtGSReceiveUnit.Text);
                    break;

                //函件收文
                case ProcessConstString.TemplateName.LETTER_RECEIVE:
                    l_entQueryCondition.HSLetterType = ddlHSLetterType.SelectedValue;
                    if (this.ddlHSLetterType.SelectedItem != null)
                    {
                        l_entQueryCondition.HSLetterTypeName = this.ddlHSLetterType.SelectedItem.Text;
                    }
                    l_entQueryCondition.HSReceiveUnit = FormsMethod.Filter(txtHSReceiveUnit.Text);
                    l_entQueryCondition.HSFileEncoding = FormsMethod.Filter(txtHSFileEncoding.Text);
                    break;

                //函件发文
                case ProcessConstString.TemplateName.LETTER_SEND:
                    l_entQueryCondition.HFCompany = FormsMethod.Filter(txtHFCompany.Text);
                    l_entQueryCondition.HFHanJianID = ddlHFLetterType.SelectedValue;
                    l_entQueryCondition.HFSendDept = FormsMethod.Filter(ddlHFDept.SelectedValue);
                    if (this.ddlHFLetterType.SelectedItem != null)
                    {
                        l_entQueryCondition.HFHanJianType = this.ddlHFLetterType.SelectedItem.Text;
                    }
                    if (this.ddlHFDept.SelectedItem != null)
                    {
                        l_entQueryCondition.HFSendDeptName = this.ddlHFDept.SelectedItem.Text;
                    }
                    break;

                //程序文件
                case ProcessConstString.TemplateName.PROGRAM_FILE:
                    l_entQueryCondition.PFHostDept = FormsMethod.Filter(this.ddlPFDept.SelectedValue);
                    break;

                //工作联系单
                case ProcessConstString.TemplateName.AFFILIATION:

                    //编制部门
                    if (this.ddlWRHostDept.SelectedItem != null)
                    {
                        l_entQueryCondition.WRHostDept = FormsMethod.Filter(this.ddlWRHostDept.SelectedItem.Text);
                    }

                    //主送部门
                    if (this.ddlWRMainSendDept.SelectedItem != null)
                    {
                        l_entQueryCondition.WRMainSend = FormsMethod.Filter(this.ddlWRMainSendDept.SelectedItem.Text);
                    }

                    break;

                //请示报告
                case ProcessConstString.TemplateName.INSTUCTION_REPORT:

                    //主送领导
                    if (this.ddlMainSendleader.SelectedItem != null)
                    {
                        l_entQueryCondition.RRMainLeader = this.ddlMainSendleader.SelectedItem.Text;
                    }

                    //承办处室
                    l_entQueryCondition.RRUnderTakeDept = FormsMethod.Filter(this.ddlUnderTakeDept.SelectedValue);

                    //承办处室name
                    if (this.ddlUnderTakeDept.SelectedItem != null)
                    {
                        l_entQueryCondition.RRUnderTakeDeptName = this.ddlUnderTakeDept.SelectedItem.Text;
                    }

                    //编制部门
                    if (this.ddlHostDept.SelectedItem != null)
                    {
                        l_entQueryCondition.RRHostDept = this.ddlHostDept.SelectedItem.Text;
                    }
                    break;
            }

            return l_entQueryCondition;
        }


        ///// <summary>chen
        ///// 流程类型选择事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.ddlProcessTemplate.SelectedIndex == 0)
        //    {
        //        this.ddlStepName.Enabled = false;
        //        this.ddlStepName.Items.Clear();
        //    }
        //    else
        //    {
        //        this.ddlStepName.Enabled = true;

        //        //得到流程类型对应的流程步骤
        //        TemplateAdmin TAdmin = new TemplateAdmin();
        //        String l_strTemplateName = this.ddlProcessTemplate.SelectedValue;
        //        DataTable l_dtbDataTable = TAdmin.Templates.GetTemplate(l_strTemplateName).GetVersion(1).GetViewList();

        //        //加入空选择行
        //        DataRow l_dtrDataRow = l_dtbDataTable.NewRow();
        //        l_dtrDataRow[0] = DBNull.Value;
        //        l_dtrDataRow[1] = "";
        //        l_dtrDataRow[2] = DBNull.Value;

        //        l_dtbDataTable.Rows.InsertAt(l_dtrDataRow, 0);

        //        //绑定数据
        //        this.ddlStepName.DataSource = l_dtbDataTable;
        //        this.ddlStepName.DataBind();
        //    }
        //    LoadTaskList();
        //}

        protected void gvTaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("lblAgentUserName") as Label).Text = l_drvRowView["AgentUserName"].ToString();
            }

            DistinctUrgentDegree(l_drvRowView["UrgentDegree"], e.Row.Cells[0]);

            String l_strCategory = l_drvRowView["Category"].ToString();
            String l_strReceiveUserID = l_drvRowView["ReceiveUserID"].ToString();
            string strStatus = l_drvRowView["D_StepStatus"].ToString();
            HyperLink l_hlkView = e.Row.FindControl("hlkView") as HyperLink;
            l_hlkView.NavigateUrl = String.Format("../../Container.aspx?TemplateName={0}&ProcessID={1}&WorkItemID={2}&StepName={3}&TBID={4}&IsHistory=1", l_drvRowView["DEF_NAME"].ToString(), l_drvRowView["ProcessID"].ToString(), l_drvRowView["WorkItemID"].ToString(), l_drvRowView["StepName"].ToString(),l_drvRowView["ID"].ToString());
            e.Row.Cells[1].Controls.Add(l_hlkView);
            switch (l_strCategory)
            {
                case "已阅":
                    e.Row.Cells[2].Text = "<font style='color:#0080FF'>已阅</font>";
                    l_hlkView.NavigateUrl = String.Format("../../Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Circulate.PG_Circulate&TemplateName={0}&ProcessID={1}&WorkItemID={2}&TBID={3}&CirculateID={4}&IsRead=True&RDT={5:yyyy/MM/dd hh时mm分ss秒}&IsHistory=1&MS=3&ID={4}", l_drvRowView["DEF_NAME"].ToString(), l_drvRowView["ProcessID"].ToString(), l_drvRowView["WorkItemID"].ToString(), l_drvRowView["ID"].ToString(), l_drvRowView["CirculateID"].ToString(), l_drvRowView["SendDateTime"].ToString());
                    break;
                case "已办":
                    e.Row.Cells[2].Text = "<font style='color:#FF00FF'>已办</font>";
                    break;
                case "公办":
                    if (strStatus == "Completed")
                    {
                        e.Row.Cells[2].Text = "<font style='color:#2633DE'>自己公办</font>";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = "<font style='color:green'>他人公办</font>";
                    }
                    break;
                default:
                    break;
            }
            //添加关联函件
            if (l_drvRowView["DEF_NAME"].ToString().Contains("函件收文") || l_drvRowView["DEF_NAME"].ToString().Contains("函件发文"))
            {
                System.Web.UI.HtmlControls.HtmlAnchor l_htmlAnchor = new System.Web.UI.HtmlControls.HtmlAnchor();
                l_htmlAnchor.InnerText = "查看";
                l_htmlAnchor.Target = "_blank";
                l_htmlAnchor.HRef = @"../Process/PG_ProcessRelation.aspx?ProcessType=" + l_drvRowView["DEF_NAME"].ToString() + "&ProcessID=" + l_drvRowView["ProcessID"].ToString();
                e.Row.Cells[14].Controls.Add(l_htmlAnchor);
            }
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

        protected void chkUnderTake_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkUnderTake.Checked)
            {
                this.ddlUnderTakeStatus.Enabled = true;
            }
            else
            {
                this.ddlUnderTakeStatus.Enabled = false;
            }
        }
    }
}