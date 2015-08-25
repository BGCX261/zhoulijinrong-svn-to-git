using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Entity.Menu;
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.FormInfoAdd
{
    /// <summary>
    /// 待办文件
    /// </summary>
    public partial class UC_FormInfoList : ListUIBase
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
                LoadProcessTemplate();

                //流程总数
                B_FormInfoSearch bCompleteFile = new B_FormInfoSearch();
                this.lblCount.Text = bCompleteFile.GetProcInstCount();
            }
        }

        /// <summary>
        /// 初始化流程类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            String[] processType = TableName.GetAllProcessTemplateName();

            ddlProcessTemplate.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
            for (int i = 0; i < processType.Length; i++)
            {
                ddlProcessTemplate.Items.Add(new ListItem(SysString.GetPTDisplayName(processType[i]), processType[i]));
            }
        }

        /// <summary>
        /// 绑定List数据
        /// </summary>
        private void LoadTaskList()
        {
            //得到检索条件
            M_ProcessSearch l_entityTask = GetSearchCondition();

            B_FormInfoSearch l_busFormInfoSearch = new B_FormInfoSearch();

            l_entityTask.Start = gvTaskList.PageIndex * gvTaskList.PageSize;
            l_entityTask.End = gvTaskList.PageIndex * gvTaskList.PageSize + gvTaskList.PageSize;
            l_entityTask.Sort = SortExpression;

            //得到已办文件任务列表数据
            DataTable l_dtbDataTable = l_busFormInfoSearch.GetTable(l_entityTask);

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
        private M_ProcessSearch GetSearchCondition()
        {
            M_ProcessSearch l_entQueryCondition = new M_ProcessSearch();

            //流程模版名称
            l_entQueryCondition.TemplateName = ddlProcessTemplate.SelectedValue;

            //流程实例名称
            l_entQueryCondition.ProcInstName = FormsMethod.Filter(txtFlowName.Text);

            //发起日期-开始
            l_entQueryCondition.StartTime = txtStartDate.ValDate;

            //发起日期-结束
            l_entQueryCondition.EndTime = txtEndDate.ValDate;

            //发起人
            l_entQueryCondition.Initiator = FormsMethod.Filter(txtSponsor.Text);

            //无数据
            l_entQueryCondition.IsNoData = chkIsNoData.Checked;

            return l_entQueryCondition;
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected String GetUserSatus(String userID, String status)
        {
            if (userID.Contains(@"\"))
            {
                userID = userID.Substring(userID.IndexOf(@"\") + 1, userID.Length - userID.IndexOf(@"\") - 1);
                if (userID.LastIndexOf(":") > -1)
                {
                    userID = userID.Substring(0, userID.Length - 1);
                }
            }
            String userName = "";

            userName = OAUser.GetUserName(userID);

            return userName + " " + GetCNStatus(status);
        }

        private String GetCNStatus(String name)
        {
            String status = "";
            switch (name)
            {
                case "New": status = "待获取"; break;
                case "Assigned": status = "待处理"; break;
                case "Completed": status = "已完成"; break;
                case "Overdue": status = "已过期"; break;
                case "Cancelled": status = "已取消"; break;
                case "Removed": status = "已移除"; break;
                case "Suspend": status = "已暂停"; break;
                case "ReAssigned": status = "重指派"; break;
                default: status = name; break;
            }
            return status;
        }

        private void GetCheckItemID()
        {
            foreach (GridViewRow selectRow in this.gvTaskList.Rows)
            {
                FSCheckBox cb = (FSCheckBox)selectRow.FindControl("cbSelect");
                if (cb.Checked)
                {
                    FSLabel lblID = (FSLabel)selectRow.FindControl("lblID");
                    txtID.Text = lblID.Text.ToString().Trim();
                    break;
                }
            }
        }

        protected void gvTaskList_ExteriorPaging(GridViewPageEventArgs e)
        {
            GetCheckItemID();

            LoadTaskList();
        }

        #region 复制动作

        #region 程序文件
        /// <summary>
        /// 复制程序文件
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="stepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyProgramFileInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                //选择的实体
                B_PF selectEntity = new B_PF();
                selectEntity.ID = int.Parse(ID); //ID赋值时自动装载实体

                //新实体
                B_PF newEntity = new B_PF();
                selectEntity.Clone(newEntity); //克隆实体
                newEntity.WorkItemID = p_strWorkItemID;

                //其他不同的属性
                newEntity.StepName = p_strStepName; //步骤
                newEntity.ReceiveUserID = p_strUserID; //接收用户
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                String e = ex.ToString();
                return false;
            }
        }
        #endregion

        #region 公司发文
        /// <summary>
        /// 复制公司发文
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyCompanySendInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                //选择的实体
                EntitySend selectEntity = new EntitySend();
                selectEntity.ID = int.Parse(ID); //ID赋值时自动装载实体

                //新实体
                EntitySend newEntity = new EntitySend();
                selectEntity.Clone(newEntity); //克隆实体
                newEntity.WorkItemID = p_strWorkItemID;

                //其他不同的属性
                newEntity.StepName = p_strStepName; //步骤
                newEntity.ReceiveUserID = p_strUserID; //接收用户
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 公司收文
        /// <summary>
        /// 复制公司收文
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyCompanyReceiveInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                B_GS_WorkItems selectEntity = new B_GS_WorkItems();
                selectEntity.ID = int.Parse(ID);

                B_GS_WorkItems newEntity = new B_GS_WorkItems();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 工作联系单
        /// <summary>
        /// 复制工作联系单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyWRInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                B_WorkRelation selectEntity = new B_WorkRelation();
                selectEntity.ID = int.Parse(ID);

                B_WorkRelation newEntity = new B_WorkRelation();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 请示报告
        /// <summary>
        /// 复制请示报告
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyRRInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                B_RequestReport selectEntity = new B_RequestReport();
                selectEntity.ID = int.Parse(ID);

                B_RequestReport newEntity = new B_RequestReport();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 函件发文
        /// <summary>
        /// 复制函件发文
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyLSInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                EntityLetterSend selectEntity = new EntityLetterSend();
                selectEntity.ID = int.Parse(ID);

                EntityLetterSend newEntity = new EntityLetterSend();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 函件收文
        /// <summary>
        /// 复制函件收文
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyLRInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                B_LetterReceive selectEntity = new B_LetterReceive();
                selectEntity.ID = int.Parse(ID);

                B_LetterReceive newEntity = new B_LetterReceive();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 党纪工团发文
        /// <summary>
        /// 复制党纪工团发文
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyDJGTInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                M_DJGTSend selectEntity = new M_DJGTSend();
                selectEntity.ID = int.Parse(ID);

                M_DJGTSend newEntity = new M_DJGTSend();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 党纪工团收文
        /// <summary>
        /// 复制党纪工团收文
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="p_strStepName"></param>
        /// <param name="receiveUserID"></param>
        /// <param name="p_strProcessID"></param>
        /// <param name="p_strWorkItemID"></param>
        /// <returns></returns>
        private bool CopyDJGTSInfo(String ID, String p_strStepName, String p_strUserID, String p_strProcessID, String p_strWorkItemID, String p_strPoolID)
        {
            try
            {
                M_MergeReceiveBase selectEntity = new M_MergeReceiveBase();
                selectEntity.Load(int.Parse(ID));

                M_MergeReceiveBase newEntity = new M_MergeReceiveBase();
                selectEntity.Clone(newEntity);

                newEntity.WorkItemID = p_strWorkItemID;
                newEntity.StepName = p_strStepName;
                newEntity.ReceiveUserID = p_strUserID;
                newEntity.D_StepStatus = String.IsNullOrEmpty(p_strPoolID) ? "Assign" : "New";//公办：New 其他：Assign
                return newEntity.Save();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion


        /// <summary>
        /// girdview绑定行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                FSCheckBox cbSelect = (FSCheckBox)e.Row.FindControl("cbSelect");
                FSLinkButton lbtnCopy = (FSLinkButton)e.Row.FindControl("lbtnCopy");
                FSLabel lblID = (FSLabel)e.Row.FindControl("lblID");

                GetCheckItemID();

                if (String.IsNullOrEmpty(lblID.Text.Trim()) || SysConvert.ToInt32(lblID.Text.Trim()) <= 0)
                {
                    cbSelect.Visible = false;
                    lbtnCopy.Visible = true;
                }
                else
                {
                    cbSelect.Visible = true;
                    lbtnCopy.Visible = false;
                    if (txtID.Text == lblID.Text)
                    {
                        cbSelect.Checked = true;
                    }
                }
                DataRowView dr = e.Row.DataItem as DataRowView;
                ListUIBase lu = new ListUIBase();
                lu.IndicateNoData(dr["MARK"], e.Row);
            }
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

        /// <summary>
        /// gridview更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTaskList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GetCheckItemID();

            GridViewRow row = this.gvTaskList.Rows[e.RowIndex];
            FSLabel lblPID = (FSLabel)row.FindControl("lblPID");
            FSLabel lblWID = (FSLabel)row.FindControl("lblWID");
            FSLabel lblReceiveUserID = (FSLabel)row.FindControl("lblUserID");
            FSLabel lblPoolID = (FSLabel)row.FindControl("lblPoolID");
            FSLabel lblTemplateName = (FSLabel)row.FindControl("lblTemplateName");
            FSLabel lblStepName = (FSLabel)row.FindControl("lblStepName");

            bool isSucc = false;
            switch (lblTemplateName.Text)
            {
                case ProcessConstString.TemplateName.PROGRAM_FILE: //程序文件
                    isSucc = CopyProgramFileInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.COMPANY_SEND://公司发文
                    isSucc = CopyCompanySendInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.COMPANY_RECEIVE://公司收文
                    isSucc = CopyCompanyReceiveInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.INSTUCTION_REPORT://请示报告
                    isSucc = CopyRRInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.AFFILIATION://工作联系单
                    isSucc = CopyWRInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.LETTER_RECEIVE://函件收文
                    isSucc = CopyLRInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.LETTER_SEND://函件发文
                    isSucc = CopyLSInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.LETTER_RECEIVE_OLD://函件收文
                    isSucc = CopyLRInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.LETTER_SEND_OLD://函件发文
                    isSucc = CopyLSInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.DJGT_Send://党纪工团发文
                    isSucc = CopyDJGTInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                case ProcessConstString.TemplateName.MERGED_RECEIVE://党纪工团收文
                    isSucc = CopyDJGTSInfo(txtID.Text, lblStepName.Text, lblReceiveUserID.Text, lblPID.Text, lblWID.Text, lblPoolID.Text);
                    break;
                default:
                    break;
            }
            if (isSucc)
            {
                txtID.Text = String.Empty;
                LoadTaskList();
                IMessage ms = new WebFormMessage(Page, "复制成功。");
                ms.Show();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, "复制失败。");
                ms.Show();
            }
        }
        #endregion
    }
}