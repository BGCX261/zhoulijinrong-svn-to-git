using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.Entity.Menu;
using FS.ADIM.OA.BLL.Busi;
using FS.OA.Framework;
using System.Collections;
using System.Collections.Generic;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Process
{
    public partial class UC_ProcessSearch : ListUIBase
    {
        private DataTable dtALLUser = new DataTable();

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

                ddlProcessStatus.Items.Add(new ListItem("", ""));
                ddlProcessStatus.Items.Add(new ListItem("运行中", ProcessConstString.ProcessStatus.STATUS_RUNNING));
                ddlProcessStatus.Items.Add(new ListItem("已完成", ProcessConstString.ProcessStatus.STATUS_COMPLETED));
                ddlProcessStatus.Items.Add(new ListItem("已取消", ProcessConstString.ProcessStatus.STATUS_CANCELED));
                ddlProcessStatus.Items.Add(new ListItem("已暂停", ProcessConstString.ProcessStatus.STATUS_SUSPENDED));

                //绑定流程列表chen
                //LoadProcessList();
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
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void LoadProcessList()
        {
            //当前登录用户账号
            String l_strUserName = CurrentUserInfo.UserName;

            //得到检索条件           
            M_ProcessSearch l_entityProcess = GetSearchCondition();

            B_ProcessInstance l_busProcessList = new B_ProcessInstance();

            l_entityProcess.Start = gvProcessList.PageIndex * gvProcessList.PageSize;
            l_entityProcess.End = gvProcessList.PageIndex * gvProcessList.PageSize + gvProcessList.PageSize;
            l_entityProcess.Sort = SortExpression;

            DataTable l_dtbDataTable = l_busProcessList.GetProcessList(l_entityProcess);

            SetEachProcessColumns(this.ddlProcessTemplate.SelectedValue);

            //绑定数据
            this.gvProcessList.RecordCount = l_entityProcess.RowCount;
            this.gvProcessList.DataSource = l_dtbDataTable;
            this.gvProcessList.DataBind();
        }

        public void SetEachProcessColumns(String p_strTemplateName)
        {
            BoundField l_bfColumnField = null;

            if (this.gvProcessList.Columns.Count > 13)
            {
                this.gvProcessList.Columns.RemoveAt(9);
                this.gvProcessList.Columns.RemoveAt(8);
                this.gvProcessList.Columns.RemoveAt(7);
            }
            else if (this.gvProcessList.Columns.Count > 12)
            {
                this.gvProcessList.Columns.RemoveAt(8);
                this.gvProcessList.Columns.RemoveAt(7);
            }
            else if (this.gvProcessList.Columns.Count > 11)
            {
                this.gvProcessList.Columns.RemoveAt(7);
            }

            switch (p_strTemplateName)
            {
                case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                case ProcessConstString.TemplateName.MERGED_RECEIVE:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "来文单位";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "ReceiveUnit";
                    l_bfColumnField.HeaderStyle.Width = new Unit(150);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);
                    break;
                case ProcessConstString.TemplateName.COMPANY_SEND:
                case ProcessConstString.TemplateName.DJGT_Send:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "主送单位";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "MainSenders";
                    l_bfColumnField.HeaderStyle.Width = new Unit(150);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "编制部门";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "HostDeptName";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(8, l_bfColumnField);
                    break;
                case ProcessConstString.TemplateName.LETTER_RECEIVE:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "文件编码";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "FileEncoding";
                    l_bfColumnField.HeaderStyle.Width = new Unit(80);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "来文单位";
                    //l_bfColumnField.SortExpression = "CommunicationUnit";
                    l_bfColumnField.DataField = "CommunicationUnit";
                    l_bfColumnField.HeaderStyle.Width = new Unit(150);
                    this.gvProcessList.Columns.Insert(8, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "函件类型";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "LetterType";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(9, l_bfColumnField);
                    break;
                case ProcessConstString.TemplateName.LETTER_SEND:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "编制部门";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "SendDept";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "函件类型";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "LetterType";
                    l_bfColumnField.HeaderStyle.Width = new Unit(80);
                    this.gvProcessList.Columns.Insert(8, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "主送单位";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "SendCompany";
                    l_bfColumnField.HeaderStyle.Width = new Unit(120);
                    this.gvProcessList.Columns.Insert(9, l_bfColumnField);
                    break;
                case ProcessConstString.TemplateName.PROGRAM_FILE:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "编制部门";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "SendDept";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);
                    break;
                case ProcessConstString.TemplateName.AFFILIATION:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "编制部门";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "SendDept";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "主送部门";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "MainDept";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(8, l_bfColumnField);
                    break;
                case ProcessConstString.TemplateName.INSTUCTION_REPORT:
                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "编制部门";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "SendDept";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(7, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "承办处室";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "UnderTakeDept";
                    l_bfColumnField.HeaderStyle.Width = new Unit(100);
                    this.gvProcessList.Columns.Insert(8, l_bfColumnField);

                    l_bfColumnField = new BoundField();
                    l_bfColumnField.HeaderText = "主送领导";
                    //l_bfColumnField.SortExpression = "ReceiveUnit";
                    l_bfColumnField.DataField = "MainLeader";
                    l_bfColumnField.HeaderStyle.Width = new Unit(60);
                    this.gvProcessList.Columns.Insert(9, l_bfColumnField);
                    break;
            }
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
            LoadProcessList();
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
        private M_ProcessSearch GetSearchCondition()
        {
            B_ProcessInstance l_busProcessList = new B_ProcessInstance();
            M_ProcessSearch l_entQueryCondition = new M_ProcessSearch();
            String l_strValue = String.Empty;
            String[] l_strAryUserRoleNames = CurrentUserInfo.RoleName.ToArray();
            String[] l_strAryUserPositions = null;
            String[] l_strAryUserSingles = new String[] { CurrentUserInfo.DisplayName };
            DataTable l_dtbPosition = CurrentUserInfo.DeptPost;
            DataSet l_dstDataSet = OAConfig.GetRankConfig();
            String l_strAllSubDeptID = String.Empty;

            l_entQueryCondition.PositionName = "普通员工";
            l_entQueryCondition.PositionValue = CurrentUserInfo.UserName;

            if (l_dtbPosition != null && l_dtbPosition.Rows.Count > 0)
            {
                l_strAryUserPositions = new String[l_dtbPosition.Rows.Count];
                for (int i = 0; i < l_dtbPosition.Rows.Count; i++)
                {
                    l_strAryUserPositions[i] = l_dtbPosition.Rows[i]["PostName"].ToString();
                    l_strValue += l_dtbPosition.Rows[i]["FK_DeptID"].ToString() + ",";
                }
            }

            foreach (DataTable l_dtbDataTable in l_dstDataSet.Tables)
            {
                if (l_dtbDataTable.TableName == "公司收文" || l_dtbDataTable.TableName == "函件收文" || l_dtbDataTable.TableName == "党纪工团收文")
                {
                    continue;
                }
                String[] l_strAryRoleName = l_dtbDataTable.Rows[0]["角色"].ToString().Split(new char[] { ';' });
                String[] l_strAryPositionName = l_dtbDataTable.Rows[0]["职位"].ToString().Split(new char[] { ';' });
                String[] l_strArySingle = l_dtbDataTable.Rows[0]["人员"].ToString().Split(new char[] { ';' });

                if (l_dtbDataTable.TableName == "处室权限" || l_dtbDataTable.TableName == "科室权限")
                {
                    if (!String.IsNullOrEmpty(l_strValue))
                    {
                        DataTable l_dtbSubDept = l_busProcessList.GetAllSubDeptID(l_strValue.Substring(0, l_strValue.Length - 1));
                        l_strAllSubDeptID = String.Empty;
                        if (l_dtbSubDept.Rows.Count != 0)
                        {
                            l_strAllSubDeptID = l_dtbSubDept.Rows[0][0].ToString().Replace("<U ID=\"", "").Replace("\"/>", ",");
                            if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                            {
                                l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                            }
                        }
                    }
                }

                if (l_strAryUserRoleNames != null && l_strAryUserRoleNames.Count() > 0)
                {
                    IEnumerable<String> l_enumRole = l_strAryUserRoleNames.Intersect(l_strAryRoleName);
                    if (l_enumRole.Count() > 0)
                    {
                        if (l_dtbDataTable.TableName == "领导权限")
                        {
                            l_entQueryCondition.PositionName = ConstString.RoleName.COMPANY_LEADER;
                            l_entQueryCondition.PositionValue = null;
                            break;
                        }
                        else if (l_dtbDataTable.TableName == "处室权限")
                        {
                            if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                            {
                                l_entQueryCondition.PositionName = "处长";
                                l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                            }
                            break;
                        }
                        else if (l_dtbDataTable.TableName == "科室权限")
                        {
                            if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                            {
                                l_entQueryCondition.PositionName = "科长";
                                l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                            }
                        }
                    }
                }

                if (l_strAryUserPositions != null && l_strAryUserPositions.Count() > 0)
                {
                    IEnumerable<String> l_enumPosition = l_strAryUserPositions.Intersect(l_strAryPositionName);
                    if (l_enumPosition.Count() > 0)
                    {
                        if (l_dtbDataTable.TableName == "领导权限")
                        {
                            l_entQueryCondition.PositionName = ConstString.RoleName.COMPANY_LEADER;
                            l_entQueryCondition.PositionValue = null;
                            break;
                        }
                        else if (l_dtbDataTable.TableName == "处室权限")
                        {
                            if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                            {
                                l_entQueryCondition.PositionName = "处长";
                                l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                            }
                            break;
                        }
                        else if (l_dtbDataTable.TableName == "科室权限")
                        {
                            if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                            {
                                l_entQueryCondition.PositionName = "科长";
                                l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                            }
                        }
                    }
                }
                IEnumerable<String> l_enumSingle = l_strAryUserSingles.Intersect(l_strArySingle);
                if (l_enumSingle.Count() > 0)
                {
                    if (l_dtbDataTable.TableName == "领导权限")
                    {
                        l_entQueryCondition.PositionName = ConstString.RoleName.COMPANY_LEADER;
                        l_entQueryCondition.PositionValue = null;
                        break;
                    }
                    else if (l_dtbDataTable.TableName == "处室权限")
                    {
                        if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                        {
                            l_entQueryCondition.PositionName = "处长";
                            l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                        }
                        break;
                    }
                    else if (l_dtbDataTable.TableName == "科室权限")
                    {
                        if (!String.IsNullOrEmpty(l_strAllSubDeptID))
                        {
                            l_entQueryCondition.PositionName = "科长";
                            l_entQueryCondition.PositionValue = l_strAllSubDeptID.Substring(0, l_strAllSubDeptID.Length - 1);
                        }
                    }
                }
            }

            //是否备份查询
            l_entQueryCondition.IsHistorySearch = this.chbHistorySearch.Checked;

            //流程模版名称
            l_entQueryCondition.TemplateName = ddlProcessTemplate.SelectedValue;

            //流程实例名
            l_entQueryCondition.ProcInstName = FormsMethod.Filter(txtProcInstName.Text);

            ////当前步骤
            //l_entQueryCondition.StepName = ddlStepName.SelectedValue;

            //文号
            l_entQueryCondition.DocumentNo = FormsMethod.Filter(txtDocumentNo.Text);

            //文件标题
            l_entQueryCondition.DocumentTitle = FormsMethod.Filter(txtDocumentTitle.Text);

            //发起人
            l_entQueryCondition.Initiator = FormsMethod.Filter(txtSponsor.Text);

            //发起日期-开始
            l_entQueryCondition.StartTime = txtStartDate.ValDate;

            //发起日期-结束
            l_entQueryCondition.EndTime = txtEndDate.ValDate;

            //状态
            l_entQueryCondition.Status = ddlProcessStatus.SelectedValue;

            //接受人
            l_entQueryCondition.ReceiveUser = FormsMethod.Filter(this.txtChuLiRen.Text);

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
                case  ProcessConstString.TemplateName.MERGED_RECEIVE://renjinquan+
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
                    case ProcessConstString.TemplateName.COMPANY_RECEIVE: trGS.Visible = true; break;
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

        protected void gvProcessList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView l_drvRowView = e.Row.DataItem as DataRowView;

            //流程图
            Image img = new Image();
            img.Attributes.Add("onclick", "window.open('/AgilePoint/ProcessViewer.aspx?PIID=" + l_drvRowView["Proc_Inst_ID"].ToString() + "');");
            img.ImageUrl = "../../AgilePoint/resource/en-us/Task.gif";
            img.Style.Add("cursor", "hand");

            //传阅单
            System.Web.UI.HtmlControls.HtmlAnchor l_htmlAnchor = new System.Web.UI.HtmlControls.HtmlAnchor();
            l_htmlAnchor.InnerText = "传阅单";
            l_htmlAnchor.HRef = "#";
            l_htmlAnchor.Attributes.Add("onclick", @"javascript: window.open('/WorkflowMenu/Circulate/ChuanYueDan.aspx?ProcessID=" + l_drvRowView["PROC_INST_ID"].ToString() + "&TemplateName=" + l_drvRowView["PDEF_NAME"].ToString() + "','', 'width=600,height=500,toolbar=no,scrollbars=yes,menubar=no,resizable=yes');event.returnValue=false;");

            e.Row.Cells[1].Text = GetProcessStatus(l_drvRowView["Status"]);

            //添加关联函件
            System.Web.UI.HtmlControls.HtmlAnchor l_htmlAnchorRelation = new System.Web.UI.HtmlControls.HtmlAnchor();
            if (l_drvRowView["PDEF_NAME"].ToString().Contains("函件收文") || l_drvRowView["PDEF_NAME"].ToString().Contains("函件发文"))
            {
                l_htmlAnchorRelation.InnerText = "查看";
                l_htmlAnchorRelation.Target = "_blank";
                l_htmlAnchorRelation.HRef = @"../Process/PG_ProcessRelation.aspx?ProcessType=" + l_drvRowView["PDEF_NAME"].ToString() + "&ProcessID=" + l_drvRowView["Proc_Inst_ID"].ToString();
            }

            if (this.gvProcessList.Columns.Count > 13)
            {
                e.Row.Cells[10].Controls.Add(img);
                e.Row.Cells[12].Controls.Add(l_htmlAnchor);
            }
            else if (this.gvProcessList.Columns.Count > 12)
            {
                e.Row.Cells[9].Controls.Add(img);
                e.Row.Cells[11].Controls.Add(l_htmlAnchor);
            }
            else if (this.gvProcessList.Columns.Count > 11)
            {
                e.Row.Cells[8].Controls.Add(img);
                e.Row.Cells[10].Controls.Add(l_htmlAnchor);
            }
            else
            {
                e.Row.Cells[7].Controls.Add(img);
                e.Row.Cells[9].Controls.Add(l_htmlAnchor);
            }
            e.Row.Cells[this.gvProcessList.Columns.Count - 1].Controls.Add(l_htmlAnchorRelation);
        }

        protected void gvProcessList_ExteriorPaging(GridViewPageEventArgs e)
        {
            LoadProcessList();
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
            LoadProcessList();
        }
    }
}