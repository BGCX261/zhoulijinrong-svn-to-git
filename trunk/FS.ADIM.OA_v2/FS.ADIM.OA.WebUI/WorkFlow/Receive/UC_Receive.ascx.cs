using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;
using System.Data;
using FounderSoftware.ADIM.OU.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public partial class UC_Receive : FormsUIBase
    {
        private const string strNewLine = "<br/>";
        String m_strSubmitAction = null;
        Hashtable l_htAttribute = new Hashtable();
        /// <summary>
        /// 流程模版名称
        /// </summary>
        protected String SubTemplateName
        {
            get
            {
                if (ViewState["SubTemplateName"] == null)
                {
                    return String.Empty;
                }
                return Convert.ToString(ViewState["SubTemplateName"]);
            }
            set
            {
                ViewState["SubTemplateName"] = value;
            }
        }

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrint();
        }
        #endregion

        /// <summary>
        /// 绑定组织机构
        /// </summary>
        protected override void BindOUControl()
        {
            //附件列表
            this.ucAttachment.UCTemplateName = base.TemplateName;
            this.ucAttachment.UCProcessID = base.ProcessID;
            this.ucAttachment.UCWorkItemID = base.WorkItemID;
            this.ucAttachment.UCTBID = base.IdentityID.ToString();
            this.ucAttachment.UCIsEditable = false;

            //传阅控件
            this.ucCirculatePeople.UCDeptIDControl = this.txtCirculateDeptID.ClientID;
            this.ucCirculatePeople.UCDeptNameControl = this.txtCirculateDeptName.ClientID;
            this.ucCirculatePeople.UCDeptUserIDControl = this.txtCirculatePeopleID.ClientID;
            this.ucCirculatePeople.UCDeptUserNameControl = this.txtCirculatePeopleName.ClientID;
            this.ucCirculatePeople.UCSelectType = "2";
            this.ucCirculatePeople.UCTemplateName = base.TemplateName;
            this.ucCirculatePeople.UCFormName = "分发范围";

            //承办部门
            this.ucUnderTakeDept.UCDeptIDControl = this.txtUnderTakeDeptID.ClientID;
            this.ucUnderTakeDept.UCDeptNameControl = this.txtUnderTakeDeptName.ClientID;
            this.ucUnderTakeDept.UCLevel = "1";
            this.ucUnderTakeDept.UCSelectType = "0";
            this.ucUnderTakeDept.UCDeptShowType = "1010";
            this.ucUnderTakeDept.UCIsSingle = "1";

            //公司领导
            OAUser.GetUserByRole(this.ddlLeadShip, OUConstString.RoleName.PartysLead);
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="p_objControl"></param>
        private void SetControlReadonly(TextBox p_objControl)
        {
            p_objControl.CssClass = "txtbox_blue";
            p_objControl.TabIndex = -1;
            p_objControl.ReadOnly = true;
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="p_objControl"></param>
        private void SetDropDownListDisabled(DropDownList p_objControl)
        {
            p_objControl.CssClass = "dropdownlist_blue";
            p_objControl.TabIndex = -1;
            p_objControl.Enabled = false;
        }

        #region 控件初始设置
        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            OAControl controls = new OAControl();
            String l_strStepName = base.StepName;

            if (base.IsPreview)
            {
                l_strStepName += "()";
            }
            switch (l_strStepName)
            {
                case ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL:
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlLeadShip);
                    SetControlReadonly(this.txtUnderTakeDeptName);
                    SetControlReadonly(this.txtUnderTakeCommentEdit);

                    this.btnSumitInspect.Visible = true;
                    this.btnSaveDraft.Visible = true;
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_PLOT:
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);
                    SetControlReadonly(this.txtUnderTakeDeptName);
                    SetControlReadonly(this.txtUnderTakeCommentEdit);

                    this.btnSubmit.Visible = true;
                    this.btnDeny.Visible = true;
                    this.btnSubmitInstruct.Visible = true;
                    this.btnSaveDraft.Visible = true;
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER:    //收文处理中心
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    this.ucUnderTakeDept.Visible = true;
                    this.divCirculates.Visible = true;

                    this.btnSubmit.Visible = true;//////////////////////////////////////////

                    this.btnSubmitInstruct.Visible = true;
                    this.btnSaveDraft.Visible = true;
                    this.btnSubmitUnderTake.Visible = true;
                    this.btnCirculate.Visible = true;
                    this.btnCirculate.Text = ProcessConstString.SubmitAction.ReceiveBase.DISTRIBUTION;

                    this.txtCirculateDeptName.Attributes.Add("readonly", "readonly");
                    this.txtCirculatePeopleName.Attributes.Add("readonly", "readonly");
                    this.txtUnderTakeDeptName.Attributes.Add("readonly", "readonly");
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION:      //领导批示
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    SetDropDownListDisabled(this.ddlLeadShip);

                    SetControlReadonly(this.txtUnderTakeDeptName);
                    SetControlReadonly(this.txtUnderTakeCommentEdit);

                    SetControlReadonly(this.txtPromptEdit);

                    this.tblPlot.Visible = true;
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR:  //处室承办
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    SetDropDownListDisabled(this.ddlLeadShip);

                    trUnderTakeDept.Visible = false;
                    trUnderTakeSection.Visible = true;
                    trUnderTakeMember.Visible = true;

                    this.btnSaveDraft.Visible = true;
                    this.btnAssignChief.Visible = true;
                    this.btnAssignMember.Visible = true;
                    this.btnSubmit.Visible = true;
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF:      //科室承办
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    SetDropDownListDisabled(this.ddlLeadShip);

                    trUnderTakeDept.Visible = false;
                    trUnderTakeSection.Visible = false;
                    trUnderTakeMember.Visible = true;

                    this.btnSaveDraft.Visible = true;
                    this.btnAssignMember.Visible = true;
                    this.btnSubmit.Visible = true;
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER:     //人员承办
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    SetDropDownListDisabled(this.ddlLeadShip);

                    trUnderTakeDept.Visible = false;
                    trUnderTakeSection.Visible = false;
                    trUnderTakeMember.Visible = false;

                    btnSubmit.Visible = true;
                    btnSaveDraft.Visible = true;
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION:
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    SetDropDownListDisabled(this.ddlLeadShip);

                    SetControlReadonly(this.txtUnderTakeDeptName);
                    SetControlReadonly(this.txtUnderTakeCommentEdit);

                    SetControlReadonly(this.txtPromptEdit);

                    this.txtCirculateDeptName.Attributes.Add("readonly", "readonly");
                    this.txtCirculatePeopleName.Attributes.Add("readonly", "readonly");

                    this.ucAttachment.UCIsEditable = true;
                    this.divCirculates.Visible = true;

                    btnSubmit.Visible = true;
                    break;
                default:
                    SetControlReadonly(this.txtPoliticalOfficerComment);
                    SetDropDownListDisabled(this.ddlPoliticalOfficer);

                    SetDropDownListDisabled(this.ddlLeadShip);

                    SetControlReadonly(this.txtUnderTakeDeptName);
                    SetControlReadonly(this.txtUnderTakeCommentEdit);

                    SetControlReadonly(this.txtPromptEdit);
                    break;
            }
            controls.SetControls();
            if (base.IsCanDevolve)
            {
                this.btnArchive.Visible = true;
                if (base.IsDevolve)
                {
                    this.btnArchive.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                }
            }
        }
        #endregion

        #region 实体与控件之间的绑定
        /// <summary>
        /// 实体填充控件
        /// </summary>
        protected override void EntityToControl()
        {
            B_MergeReceiveBase l_objReceiveBase = null;

            //收文登记号
            this.RegisterID = Request.QueryString[ConstString.QueryString.REGISTER_ID];

            if (!String.IsNullOrEmpty(this.RegisterID))
            {
                B_ReceiveEdit l_BusReceiveEdit = new B_ReceiveEdit();
                l_BusReceiveEdit.ID = Convert.ToInt32(this.RegisterID);
                if (l_BusReceiveEdit == null)
                {
                    JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "当前选择的收文登记信息不存在或者已经被删除,无法继续操作", "Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle");
                    return;
                }

                //附件列表
                this.ucAttachment.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(l_BusReceiveEdit.FileData);

                //收文号
                this.txtReceiveNo.Text = l_BusReceiveEdit.ReceiveNo;

                //收文日期
                this.txtReceiveDate.Text = l_BusReceiveEdit.ReceiveDate.ToString(ConstString.DateFormat.Normal);

                //原文号
                this.txtSendLetterNo.Text = l_BusReceiveEdit.SendLetterNo;

                //来文单位
                this.txtCommunicationUnit.Text = l_BusReceiveEdit.ReceiveUnit;

                //卷号
                this.txtPreVolumeNo.Text = l_BusReceiveEdit.PreVolumeNo;

                //文件名称
                this.txtDocumentTitle.Text = l_BusReceiveEdit.DocumentTitle;

                //紧急程度
                this.txtUrgentDegree.Text = l_BusReceiveEdit.UrgentDegree;

                //步骤名称
                base.StepName = ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL;

                this.SubTemplateName = l_BusReceiveEdit.ProcessName;

                //党群工作处处长
                OAUser.GetUserByRole(this.ddlPoliticalOfficer, OUConstString.RoleName.PARTYS_DIRECTOR);
            }
            else
            {
                l_objReceiveBase = base.EntityData as B_MergeReceiveBase;

                this.SubTemplateName = l_objReceiveBase.TemplateName;

                this.RegisterID = l_objReceiveBase.RegisterID;

                //党群工作处处长
                OAUser.GetUserByRole(this.ddlPoliticalOfficer, OUConstString.RoleName.PARTYS_DIRECTOR);

                ddlPoliticalOfficer.SelectedValue = l_objReceiveBase.Officer;
                txtPoliticalOfficerComment.Text = l_objReceiveBase.Officer_Comment;

                //附件列表
                this.ucAttachment.UCDataList = l_objReceiveBase.FileList;

                //收文号
                this.txtReceiveNo.Text = l_objReceiveBase.DocumentNo;

                //收文日期
                this.txtReceiveDate.Text = l_objReceiveBase.DocumentReceiveDate.ToString(ConstString.DateFormat.Normal);

                //原文号
                this.txtSendLetterNo.Text = l_objReceiveBase.SendNo;

                //来文单位
                this.txtCommunicationUnit.Text = l_objReceiveBase.CommunicationUnit;

                //卷号
                this.txtPreVolumeNo.Text = l_objReceiveBase.VolumeNo;

                //文件名称
                this.txtDocumentTitle.Text = l_objReceiveBase.DocumentTitle;

                //紧急程度
                this.txtUrgentDegree.Text = l_objReceiveBase.UrgentDegree;

                //发起人ID
                this.txtDrafter.Text = l_objReceiveBase.DrafterID;

                //发起日期
                this.txtDraftDate.Text = l_objReceiveBase.DraftDate.ToString();

                //公司领导
                this.ddlLeadShip.SelectedValue = l_objReceiveBase.LeaderShip;

                //公司批示意见
                this.txtLeadCommentView.Text = l_objReceiveBase.LS_Comment;
                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION)
                {
                    this.txtLeadCommentEdit.Text = l_objReceiveBase.LS_Comment;
                }

                switch (base.StepName)
                {
                    case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR:
                        //获取处室下属科室
                        OADept.GetChildDept(this.ddlUnderTakeSection, l_objReceiveBase.UnderTakeDept, 2);

                        //获取处室下属人员
                        OAUser.GetUserByDeptID(this.ddlUnderTakePeople, l_objReceiveBase.UnderTakeDept, -1);
                        break;
                    case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF:
                        //获取科室下属人员
                        OAUser.GetUserByDeptID(this.ddlUnderTakePeople, l_objReceiveBase.UnderTakeChief, -1);
                        break;
                    case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER:
                        break;
                }

                //承办部门
                this.txtUnderTakeDeptID.Text = l_objReceiveBase.UnderTakeDept;
                this.txtUnderTakeDeptName.Text = l_objReceiveBase.UnderTakeDeptName;

                //承办科室
                this.ddlUnderTakeSection.SelectedValue = l_objReceiveBase.UnderTakeChief;

                //承办人员
                this.ddlUnderTakePeople.SelectedValue = l_objReceiveBase.UnderTakePeople;

                //承办意见
                this.txtUnderTakeCommentEdit.Text = l_objReceiveBase.UnderTake_Comment;
                this.txtCirculatePeopleName.Text = l_objReceiveBase.CPeopleName;
                this.txtCirculatePeopleID.Text = l_objReceiveBase.CPeopleID;
                this.txtCirculateDeptName.Text = l_objReceiveBase.CDeptName;
                this.txtCirculateDeptID.Text = l_objReceiveBase.CDeptID;

                //党群工作处处长处理后显示label形式的处长姓名与时间
                if (l_objReceiveBase.DraftDate == DateTime.MinValue)
                {
                    this.ddlPoliticalOfficer.Visible = false;
                    this.lbParty.Visible = true;
                    this.lbParty.Text = l_objReceiveBase.Officer + strNewLine + l_objReceiveBase.Officer_Date;
                }

                //领导；批示处理后显示label形式的领导人姓名与时间
                if (l_objReceiveBase.LS_Date != "" && l_objReceiveBase.LS_Date != null)
                {
                    this.ddlLeadShip.Visible = false;
                    this.lbLeadShip.Visible = true;
                    this.lbLeadShip.Text = l_objReceiveBase.LeaderShipName + strNewLine + l_objReceiveBase.LS_Date;
                }

                //提示信息
                this.txtPrompt.Text = l_objReceiveBase.Prompt;
                if (l_objReceiveBase.IsFormSave)
                {
                    this.txtPromptEdit.Text = l_objReceiveBase.PromptEdit;
                }
            }
        }

        /// <summary>
        /// 控件填充实体
        /// </summary>
        /// <param name="IsSave">是否保存</param>
        /// <returns>EntityBase</returns>
        protected override EntityBase ControlToEntity(Boolean p_blnIsSaveDraft)
        {
            B_MergeReceiveBase l_objReceiveBase = base.EntityData != null ? base.EntityData as B_MergeReceiveBase : new B_MergeReceiveBase();

            l_objReceiveBase.TemplateName = this.SubTemplateName;

            l_objReceiveBase.RegisterID = base.RegisterID;

            l_objReceiveBase.Officer = ddlPoliticalOfficer.SelectedValue;
            if (!String.IsNullOrEmpty(l_objReceiveBase.Officer))
            {
                l_objReceiveBase.OfficerName = ddlPoliticalOfficer.SelectedItem.Text;
            }
            l_objReceiveBase.Officer_Comment = txtPoliticalOfficerComment.Text.TrimEnd();

            //附件信息
            l_objReceiveBase.FileList = this.ucAttachment.UCDataList;

            //意见列表
            l_objReceiveBase.CommentList = base.Comments;

            //收文号
            l_objReceiveBase.DocumentNo = this.txtReceiveNo.Text;

            //收文日期
            l_objReceiveBase.DocumentReceiveDate = this.txtReceiveDate.ValDate;

            //文件名称
            l_objReceiveBase.DocumentTitle = this.txtDocumentTitle.Text;

            //原文号
            l_objReceiveBase.SendNo = this.txtSendLetterNo.Text;

            //来文单位
            l_objReceiveBase.CommunicationUnit = this.txtCommunicationUnit.Text;

            //预立卷号
            l_objReceiveBase.VolumeNo = this.txtPreVolumeNo.Text;

            //紧急程度
            l_objReceiveBase.UrgentDegree = this.txtUrgentDegree.Text;

            //提交时间
            l_objReceiveBase.SubmitDate = DateTime.Now;

            if (String.IsNullOrEmpty(l_objReceiveBase.Drafter))
            {
                //发起人ID
                l_objReceiveBase.DrafterID = CurrentUserInfo.UserName;

                //发起人姓名
                l_objReceiveBase.Drafter = CurrentUserInfo.DisplayName;

                //发起日期
                l_objReceiveBase.DraftDate = DateTime.Now;
            }
            else
            {
                //发起人ID
                l_objReceiveBase.DrafterID = l_objReceiveBase.ReceiveUserID;

                //发起人姓名
                l_objReceiveBase.Drafter = l_objReceiveBase.ReceiveUserName;

                //发起日期
                l_objReceiveBase.DraftDate = txtDraftDate.ValDate;
            }

            //公司领导批示
            l_objReceiveBase.LeaderShip = this.ddlLeadShip.SelectedValue;
            if (!String.IsNullOrEmpty(this.ddlLeadShip.SelectedValue))
            {
                l_objReceiveBase.LeaderShipName = this.ddlLeadShip.SelectedItem.Text;
            }
            if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION)
            {
                l_objReceiveBase.LS_Comment = this.txtLeadCommentEdit.Text.TrimEnd();
                l_objReceiveBase.LS_Date = System.DateTime.Now.ToString();
            }
            else
            {
                l_objReceiveBase.LS_Comment = this.txtLeadCommentView.Text.TrimEnd();
            }

            //承办部门
            if (!String.IsNullOrEmpty(this.txtUnderTakeDeptID.Text.TrimEnd()))
            {
                l_objReceiveBase.UnderTakeDept = this.txtUnderTakeDeptID.Text;
                l_objReceiveBase.UnderTakeDeptName = this.txtUnderTakeDeptName.Text;
                l_objReceiveBase.UnderTakeDeptLeaderID = OAUser.GetUserByDeptPostArray(this.txtUnderTakeDeptID.Text,null,true,true)[0];
            }

            //承办科室
            if (!String.IsNullOrEmpty(this.ddlUnderTakeSection.SelectedValue))
            {
                l_objReceiveBase.UnderTakeChief = this.ddlUnderTakeSection.SelectedValue;
                l_objReceiveBase.UnderTakeChiefName = this.ddlUnderTakeSection.SelectedItem.Text;
                l_objReceiveBase.UnderTakeChiefLeaderID = FormsMethod.GetUserIDByViewBase(OAUser.GetDeptManager(this.ddlUnderTakeSection.SelectedValue, 0));
            }

            //承办人员
            if (!String.IsNullOrEmpty(this.ddlUnderTakePeople.SelectedValue))
            {
                l_objReceiveBase.UnderTakePeople = this.ddlUnderTakePeople.SelectedValue;
                l_objReceiveBase.UnderTakePeopleName = this.ddlUnderTakePeople.SelectedItem.Text;
            }

            if (p_blnIsSaveDraft)
            {
                l_objReceiveBase.UnderTake_Comment = this.txtUnderTakeCommentEdit.Text;
            }

            //传阅
            l_objReceiveBase.CPeopleName = this.txtCirculatePeopleName.Text;
            l_objReceiveBase.CPeopleID = this.txtCirculatePeopleID.Text;
            l_objReceiveBase.CDeptName = this.txtCirculateDeptName.Text;
            l_objReceiveBase.CDeptID = this.txtCirculateDeptID.Text;

            if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR ||
                base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF ||
                base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER)
            {
                if (!p_blnIsSaveDraft)
                {
                    CYiJian l_objComment = new CYiJian();
                    l_objComment.Content = this.txtUnderTakeCommentEdit.Text;
                    l_objComment.FinishTime = DateTime.Now.ToString();
                    l_objComment.UserID = l_objReceiveBase.ReceiveUserID;
                    l_objComment.UserName = l_objReceiveBase.ReceiveUserName;
                    l_objComment.ViewName = base.StepName;
                    l_objComment.Action = m_strSubmitAction;
                    l_objReceiveBase.CommentList.Add(l_objComment);                    
                }
            }
            else if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_PLOT)
            {
                if (!p_blnIsSaveDraft)
                {
                    CYiJian l_objComment = new CYiJian();
                    l_objComment.Content = this.txtPoliticalOfficerComment.Text;
                    l_objComment.FinishTime = DateTime.Now.ToString();
                    l_objComment.UserID = l_objReceiveBase.ReceiveUserID;
                    l_objComment.UserName = l_objReceiveBase.ReceiveUserName;
                    l_objComment.ViewName = base.StepName;
                    l_objComment.Action = m_strSubmitAction;
                    l_objReceiveBase.CommentList.Add(l_objComment);
                }
            }
            else if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION)
            {
                if (!p_blnIsSaveDraft)
                {
                    CYiJian l_objComment = new CYiJian();
                    l_objComment.Content = this.txtLeadCommentEdit.Text;
                    l_objComment.FinishTime = DateTime.Now.ToString();
                    l_objComment.UserID = l_objReceiveBase.ReceiveUserID;
                    l_objComment.UserName = l_objReceiveBase.ReceiveUserName;
                    l_objComment.ViewName = base.StepName;
                    l_objComment.Action = m_strSubmitAction;
                    l_objReceiveBase.CommentList.Add(l_objComment);
                }
            }
            else if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER)
            {
                if (!p_blnIsSaveDraft && m_strSubmitAction == ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_UNDERTAKE)
                {
                    CYiJian l_objComment = new CYiJian();
                    l_objComment.Content = this.txtUnderTakeCommentEdit.Text;
                    l_objComment.FinishTime = DateTime.Now.ToString();
                    l_objComment.UserID = l_objReceiveBase.ReceiveUserID;
                    l_objComment.UserName = l_objReceiveBase.ReceiveUserName;
                    l_objComment.ViewName = base.StepName;
                    l_objComment.Action = m_strSubmitAction;
                    l_objReceiveBase.CommentList.Add(l_objComment);
                }
            }

            l_objReceiveBase.IsFormSave = p_blnIsSaveDraft;

            if (p_blnIsSaveDraft)
            {
                l_objReceiveBase.Prompt = this.txtPrompt.Text;
                l_objReceiveBase.PromptEdit = this.txtPromptEdit.Text;
            }
            else
            {
                if (!String.IsNullOrEmpty(this.txtPromptEdit.Text.TrimEnd()))
                {
                    l_objReceiveBase.Prompt = this.txtPrompt.Text + "[" + (l_objReceiveBase.ReceiveUserName == string.Empty ? CurrentUserInfo.DisplayName : l_objReceiveBase.ReceiveUserName) + "][" + System.DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:" + this.txtPromptEdit.Text + ConstString.Miscellaneous.NEW_LINE;
                }
            }

            return l_objReceiveBase;
        }
        #endregion

        /// <summary>
        /// 元素验证
        /// </summary>
        /// <returns></returns>
        private bool VerifyField()
        {
            Boolean l_blnIsValid = true;

            if (txtPoliticalOfficerComment.Text.TrimEnd().Length > 200)
            {
                m_strAryMessages.Add("拟办:拟办意见长度不能超过200字长");
                l_blnIsValid = false;
            }

            if (txtLeadCommentEdit.Text.TrimEnd().Length > 200)
            {
                m_strAryMessages.Add("领导批示:批示意见长度不能超过200字长");
                l_blnIsValid = false;
            }

            if (txtUnderTakeCommentEdit.Text.TrimEnd().Length > 200)
            {
                m_strAryMessages.Add("承办:意见长度不能超过200字长");
                l_blnIsValid = false;
            }

            String l_strPrompt = String.Concat(txtPromptEdit.Text.TrimEnd(), txtPrompt.Text.TrimEnd());

            if (l_strPrompt.Length > 2000)
            {
                m_strAryMessages.Add("提示信息:提示信息总长度不能超过2000字长");
                l_blnIsValid = false;
            }

            return l_blnIsValid;
        }

        protected void btnSumitInspect_Click(object sender, EventArgs e)
        {
            B_ReceiveEdit l_BusReceiveEdit = null;
            String l_strInspector = String.Empty;

            if (String.IsNullOrEmpty(ddlPoliticalOfficer.SelectedValue))
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有选择党群工作处处长");
                return;
            }
            l_strInspector = ddlPoliticalOfficer.SelectedValue;

            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_INSPECT;

            if (String.IsNullOrEmpty(WorkItemID))
            {
                //流程创建时候设置自定义属性
                l_htAttribute.Add(ConstString.CustomAttr.Receive.Initiator, CurrentUserInfo.UserName);

                l_BusReceiveEdit = new B_ReceiveEdit();
                l_BusReceiveEdit.ID = Convert.ToInt32(this.RegisterID);

                if (l_BusReceiveEdit != null)
                {
                    l_BusReceiveEdit.ProcessID = base.ProcessID;
                    l_BusReceiveEdit.Save();
                }
            }

            ViewBase l_objClerks = OAUser.GetUserByRole(OUConstString.RoleName.PartysDocument);
            String l_strClerkField = String.Empty;
            foreach (User user in l_objClerks.Ens)
            {
                l_strClerkField += user.DomainUserID + ";";
            }
            l_strClerkField = SysString.TrimLastWord(l_strClerkField);

            l_htAttribute.Add(ConstString.CustomAttr.Receive.Officer, l_strInspector);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.Clerks, l_strClerkField);

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            this.m_strSubmitAction = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;

            base.FormSubmit(true, m_strSubmitAction, null, this.ControlToEntity(true));
        }

        protected void btnSubmitInstruct_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlLeadShip.SelectedValue))
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有选择批示领导");
                return;
            }

            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_POSTIL;

            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsPlot, true);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsDeny, false);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.LeadShip, ddlLeadShip.SelectedValue);

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnSubmitUnderTake_Click(object sender, EventArgs e)
        {
            String l_strUndertakeInfo = String.Empty;
            String l_strMessageInfo = String.Empty;
            Boolean l_blnIsUnderTake = false;
            String l_strDeptLeaderID = null;

            if (String.IsNullOrEmpty(txtUnderTakeDeptID.Text.TrimEnd()))
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有选择承办部门");
                return;
            }

            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_UNDERTAKE;

            //获取承办部门ID
            String[] l_strAryUnderTakeDeptID = txtUnderTakeDeptID.Text.TrimEnd().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (l_strAryUnderTakeDeptID.Length > 0)
            {
                foreach (String l_strUnderTakeDeptID in l_strAryUnderTakeDeptID)
                {
                    //流程创建时候设置自定义属性
                    l_strDeptLeaderID = OAUser.GetUserByDeptPostArray(this.txtUnderTakeDeptID.Text,null,true,true)[0];

                    String l_strDeptName = OADept.GetDeptByDeptID(l_strUnderTakeDeptID).Name;

                    if (String.IsNullOrEmpty(l_strDeptLeaderID))
                    {
                        l_strUndertakeInfo += l_strDeptName + ":无处级领导,该部门不参与承办\\n";
                    }
                    else
                    {
                        l_blnIsUnderTake = true;
                    }
                }
            }

            //不能承办
            if (!l_blnIsUnderTake)
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "提交承办处理失败:承办部门都无处级领导,无法承办");
                return;
            }

            l_htAttribute.Add(ConstString.CustomAttr.Receive.DeptDirector, l_strDeptLeaderID);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsMultipleDept, false);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsDirectArchive, false);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsPlot, false);

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnAssignChief_Click(object sender, EventArgs e)
        {
            if (ddlUnderTakeSection.SelectedValue == String.Empty)
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有选择承办科室");
                return;
            }

            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ReceiveBase.ASSIGN_SECTION;

            //获取科级领导
            String[] l_strArray = OAUser.GetDeptManagerArray(ddlUnderTakeSection.SelectedValue, 0);
            String l_strSectionLeaders = l_strArray[0];

            if (String.IsNullOrEmpty(l_strArray[0]))
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "组织结构:未能获取科级领导,无法交办科室");
                return;
            }

            String l_strSectionLeaderNames = l_strArray[1];

            l_htAttribute.Add(ConstString.CustomAttr.Receive.SectionChief, l_strSectionLeaders);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignSection, true);
            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignPeople, false);

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnAssignMember_Click(object sender, EventArgs e)
        {
            if (ddlUnderTakePeople.SelectedValue == String.Empty)
            {
                JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有选择承办人员");
                return;
            }

            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            switch (base.StepName)
            {
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR:
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignSection, false);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignPeople, true);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.SectionMember, ddlUnderTakePeople.SelectedValue);
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF:
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignSection, true);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.SectionMember, ddlUnderTakePeople.SelectedValue);
                    break;
                default:
                    break;
            }

            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ReceiveBase.ASSIGN_PEOPLE;

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnCirculate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtCirculateDeptID.Text.TrimEnd()) && String.IsNullOrEmpty(this.txtCirculatePeopleID.Text.TrimEnd()))
            {
                m_strAryMessages.Add("分发:没有选择分发范围！");
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }
            base.Circulate(this.txtCirculateDeptID.Text,"1", "", this.txtCirculatePeopleID.Text, "1", true, "", false);
        }

        protected void btnDeny_Click(object sender, EventArgs e)
        {
            //表单的合法性验证
            if (!VerifyField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ACTION_DENY;

            l_htAttribute.Add(ConstString.CustomAttr.Receive.IsDeny, true);

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            base.FormCancel(this.ControlToEntity(false));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //提交动作
            this.m_strSubmitAction = ProcessConstString.SubmitAction.ACTION_SUBMIT;

            switch (base.StepName)
            {
                case ProcessConstString.StepName.ReceiveStepName.STEP_PLOT:
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsDeny, false);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsPlot, false);
                    ViewBase l_objClerks = OAUser.GetUserByRole(OUConstString.RoleName.PartysDocument);
                    String l_strClerkField = String.Empty;
                    foreach (User user in l_objClerks.Ens)
                    {
                        l_strClerkField += user.DomainUserID + ";";
                    }
                    l_strClerkField = SysString.TrimLastWord(l_strClerkField);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.Clerks, l_strClerkField);
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION:
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER:
                    //l_htAttribute.Add(ConstString.CustomAttr.Receive.IsPlot, false);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignSection, false);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignPeople, false);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsDirectArchive,true);
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER:
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF:
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignSection, false);
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR:
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignSection, false);
                    l_htAttribute.Add(ConstString.CustomAttr.Receive.IsAssignPeople, false);
                    break;
                case ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION:
                    break;
                default:
                    break;
            }

            base.FormSubmit(false, m_strSubmitAction, l_htAttribute, this.ControlToEntity(false));
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            B_ReceiveEdit l_BusReceiveEdit = new B_ReceiveEdit();
            l_BusReceiveEdit.ID = Convert.ToInt32(RegisterID);

            if (l_BusReceiveEdit != null)
            {
                l_BusReceiveEdit.ArchiveStatus = "已归档";
                l_BusReceiveEdit.Save();
            }
            string strMessage = string.Empty;
            try
            {
                this.Devolve(out strMessage);
                base.Devolved(base.ProcessID, base.TemplateName);
                JScript.Alert("归档成功！\\n流水号：" + strMessage, false);
            }
            catch (Exception ex)
            {
                base.WriteLog(ex.Message);
                JScript.Alert("归档失败！请查看配置是否正确！", false);
            }
        }
    }
}