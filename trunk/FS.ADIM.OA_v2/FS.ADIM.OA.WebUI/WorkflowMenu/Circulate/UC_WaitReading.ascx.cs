using System;
using System.Data;
using System.Web.UI.WebControls;

using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Entity;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Circulate
{
    public partial class UC_WaitReading : ListUIBase
    {
        protected DataTable m_dtbLeader;

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

                //绑定待阅文件列表
                LoadTaskList();
            }
        }

        /// <summary>
        /// 初始化流程类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            String[] l_strAryProcessTemplates = TableName.GetAllProcessTemplateName();

            ddlProcessTemplate.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
            for (int i = 0; i < l_strAryProcessTemplates.Length; i++)
            {
                ddlProcessTemplate.Items.Add(new ListItem(SysString.GetPTDisplayName(l_strAryProcessTemplates[i]), l_strAryProcessTemplates[i]));
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void LoadTaskList()
        {
            m_dtbLeader = OAList.GetGSLDToTable();

            //当前登录用户账号
            String l_strUserName = CurrentUserInfo.UserName;

            //得到检索条件           
            M_EntityMenu l_entityTask = GetSearchCondition();

            B_Circulate l_busTaskList = new B_Circulate(String.Empty);

            l_entityTask.Start = gvTaskList.PageIndex * gvTaskList.PageSize;
            l_entityTask.End = gvTaskList.PageIndex * gvTaskList.PageSize + gvTaskList.PageSize;
            l_entityTask.Sort = SortExpression;

            //得到待阅文件列表数据
            DataTable l_dtbDataTable = l_busTaskList.GetWaitingReadList(l_entityTask);

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
                m_strAryMessages.Add("分发日期:开始日期格式不正确！例:XXXX-XX-XX 或 XXXX/XX/XX");
                l_blnIsValid = false;
            }
            if (!String.IsNullOrEmpty(this.txtEndDate.ValStr.TrimEnd()) && !ValidateUtility.IsDateTime(this.txtEndDate.ValStr.TrimEnd()))
            {
                m_strAryMessages.Add("分发日期:结束日期格式不正确！例:XXXX-XX-XX 或 XXXX/XX/XX");
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
                    m_strAryMessages.Add("分发日期:开始日期必须小于等于结束日期");
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
            l_entQueryCondition.TemplateName = ddlProcessTemplate.SelectedValue;

            //文号
            l_entQueryCondition.DocumentNo = FormsMethod.Filter(txtDocumentNo.Text);

            //文件标题
            l_entQueryCondition.DocumentTitle = FormsMethod.Filter(txtDocumentTitle.Text);

            //分发日期-开始
            l_entQueryCondition.StartTime = this.txtStartDate.ValDate.Date;

            //分发日期-结束
            l_entQueryCondition.EndTime = this.txtEndDate.ValDate.Date;

            //分发人(发人)
            l_entQueryCondition.Initiator = FormsMethod.Filter(txtSponsor.Text);

            l_entQueryCondition.Is_Inbox = false;

            l_entQueryCondition.Is_Read = 0;

            return l_entQueryCondition;
        }

        /// <summary>
        /// 快速阅知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnQuickMarker_Click(object sender, EventArgs e)
        {
            LinkButton l_lbnQuickMarker = sender as LinkButton;

            String l_strProcessTemplate = l_lbnQuickMarker.CommandName;

            String l_strTableName = TableName.GetCirculateTableName(l_strProcessTemplate);

            B_Circulate l_burCirculate = new B_Circulate(l_strTableName);
            l_burCirculate.ID = Convert.ToInt32(l_lbnQuickMarker.CommandArgument);

            l_burCirculate.Is_Read = true;
            bool ret = l_burCirculate.Save();
            if (ret)
            {
                LoadTaskList();
                ClientScriptM.RefreshLeft(Page);
            }
        }

        protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //检查查询条件数据有效性
            if (!ValidateQueryCondition())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }
            LoadTaskList();
        }

        protected void gvTaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }
            if (m_dtbLeader == null)
            {
                m_dtbLeader = OAList.GetGSLDToTable();
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            //是否领导传阅
            String l_strSendUserID = l_drvRowView["SendUserID"].ToString();

            if (OAList.IsGSLD(m_dtbLeader, l_strSendUserID))
            {
                Label l_lblFromLeader = e.Row.FindControl("lblLeaderCirculate") as Label;
                l_lblFromLeader.Text = "<Img src='Img/lead.jpg' />";
                l_lblFromLeader.ToolTip = "领导传阅";
            }
            DistinctUrgentDegree(l_drvRowView["UrgentDegree"], e.Row.Cells[1]);
        }

        /// <summary>
        /// 获取二次分发的流程模板
        /// </summary>
        /// <param name="step">步揍</param>
        /// <returns></returns>
        protected String GetIsAgaionCirculate(String step)
        {
            String strsff = "";
            if (step == ProcessConstString.StepName.STEP_Send_FENFA)
            {
                strsff += "(" + step + ")";
            }
            return strsff;
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
        //chen
        protected void btnMarker_Click(object sender, EventArgs e)
        {           
            for (int i = 0; i <= gvTaskList.Rows.Count - 1; i++)
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox cbx = (System.Web.UI.HtmlControls.HtmlInputCheckBox)gvTaskList.Rows[i].FindControl("cbxContact");
                if (cbx.Checked)
                {
                    LinkButton l_btnMarker = (LinkButton)gvTaskList.Rows[i].FindControl("lbnQuickMarker");
                     

                    String l_strProcessTemplate = l_btnMarker.CommandName;

                    String l_strTableName = TableName.GetCirculateTableName(l_strProcessTemplate);

                    B_Circulate l_burCirculate = new B_Circulate(l_strTableName);
                    l_burCirculate.ID = Convert.ToInt32(l_btnMarker.CommandArgument);

                    l_burCirculate.Is_Read = true;
                    try
                    {
                        l_burCirculate.MultiRead(l_strTableName, l_burCirculate.ID);
                    }
                    catch (Exception err)
                    {
                    }
                }
            }
            LoadTaskList();
            ClientScriptM.RefreshLeft(Page);
        }        
    }
}