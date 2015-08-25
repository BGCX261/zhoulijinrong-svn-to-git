//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：党纪工团视图
// 
// 
// 创建标识：wangbinyi 2009-3-17
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.WorkFlow.Send
{
    public partial class UC_Send : FormsUIBase
    {
        private const string strNewLine = "<br/>";

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrint();
            this.SubmitEvents();
        }

        #region 控件初始设置
        protected override void SetControlStatus()
        {
            B_DJGTSend entity = base.EntityData != null ? base.EntityData as B_DJGTSend : new B_DJGTSend();

            //附件
            ucAttachment.UCTemplateName = base.TemplateName;
            ucAttachment.UCProcessID = base.ProcessID;
            ucAttachment.UCWorkItemID = base.WorkItemID;
            ucAttachment.UCTBID = base.IdentityID.ToString();

            ucDeptCounterSignComments.UCTemplateName = base.TemplateName;
            ucDeptCounterSignComments.UCProcessID = base.ProcessID;
            ucDeptCounterSignComments.UCWorkItemID = base.WorkItemID;
            ucDeptCounterSignComments.UCStepName = "部门会签";

            ucLeadCounterSignComments.UCTemplateName = base.TemplateName;
            ucLeadCounterSignComments.UCProcessID = base.ProcessID;
            ucLeadCounterSignComments.UCWorkItemID = base.WorkItemID;
            ucLeadCounterSignComments.UCStepName = "领导会签";

            //主送
            this.ucMainSender.UCSelectType = "2";
            this.ucMainSender.UCDeptAndUserControl = this.txtMainSender.ClientID;
            this.ucMainSender.UCTemplateName = base.TemplateName;
            this.ucMainSender.UCFormName = "主送";

            //抄送
            this.ucCopySender.UCSelectType = "2";
            this.ucCopySender.UCDeptAndUserControl = this.txtCopySender.ClientID;
            this.ucCopySender.UCTemplateName = base.TemplateName;
            this.ucCopySender.UCFormName = "抄送";

            //部门会签
            this.ucDeptCounterSigns.UCUserIDControl = this.wfDeptSignIDs.ClientID;
            this.ucDeptCounterSigns.UCUserNameControl = this.txtDeptSigners.ClientID;

            switch (this.ddlType.SelectedValue)
            {
                case ProcessConstString.TemplateName.PARTY_SEND:
                    this.ucDeptCounterSigns.UCRoleName = OUConstString.RoleName.PartyBranchSign;
                    break;
                case ProcessConstString.TemplateName.DISCIPLINE_SEND:
                    this.ucDeptCounterSigns.UCRoleName = OUConstString.RoleName.PartyBranchSign;
                    break;
                case ProcessConstString.TemplateName.TRADE_UNION_SEND:
                    this.ucDeptCounterSigns.UCRoleName = OUConstString.RoleName.TradeUnionBranchSign;
                    break;
                case ProcessConstString.TemplateName.YOUTH_LEAGUE_SEND:
                    this.ucDeptCounterSigns.UCRoleName = OUConstString.RoleName.YouthLeagueBranchSign;
                    break;
            }
            this.ucDeptCounterSigns.UCIsSingle = false;


            //领导会签
            this.ucRole.UCUserIDControl = this.wfLeaderSignIDs.ClientID;
            this.ucRole.UCUserNameControl = this.txtLeadSigners.ClientID;

            switch (this.ddlType.SelectedValue)
            {
                case ProcessConstString.TemplateName.PARTY_SEND:
                    this.ucRole.UCRoleName = OUConstString.RoleName.PartySign;
                    break;
                case ProcessConstString.TemplateName.DISCIPLINE_SEND:
                    this.ucRole.UCRoleName = OUConstString.RoleName.PartySign;
                    break;
                case ProcessConstString.TemplateName.TRADE_UNION_SEND:
                    this.ucRole.UCRoleName = OUConstString.RoleName.TradeUnionSign;
                    break;
                case ProcessConstString.TemplateName.YOUTH_LEAGUE_SEND:
                    this.ucRole.UCRoleName = OUConstString.RoleName.YouthLeagueSign;
                    break;
            }
            this.ucRole.UCIsSingle = false;

            //传阅
            this.ucOUCirculate.UCDeptIDControl = this.hDeptID.ClientID;
            this.ucOUCirculate.UCDeptNameControl = this.txtDeptName.ClientID;
            this.ucOUCirculate.UCRoleUserIDControl = this.hUserID.ClientID;
            this.ucOUCirculate.UCRoleUserNameControl = this.txtUserName.ClientID;
            this.ucOUCirculate.UCRole = OUConstString.RoleName.COMPANY_LEADER;
            this.ucOUCirculate.UCSelectType = "0";
            this.ucOUCirculate.UCDeptShowType = "1010";

            txtLeadSigners.Attributes.Add("readOnly", "readOnly");
            txtDeptSigners.Attributes.Add("readOnly", "readOnly");

            OAControl controls = new OAControl();

            if (!base.IsPreview)
            {
                //this.txtDocumentTitle.ToolTip = "100字符以内";
                //this.txtSubjectWord.ToolTip = "100字符以内";
                //this.txtComment.ToolTip = "2000字符以内";
                //this.txtShareCount.ToolTip = "正整数";
                //this.txtSheetCount.ToolTip = "正整数";

                string strCheckDraftToolTip = string.Empty;
                switch (this.ddlType.SelectedValue)
                {
                    case ProcessConstString.TemplateName.PARTY_SEND:
                        strCheckDraftToolTip = "党纪审稿组";
                        break;

                    case ProcessConstString.TemplateName.DISCIPLINE_SEND:
                        strCheckDraftToolTip = "党纪审稿组";
                        break;

                    case ProcessConstString.TemplateName.TRADE_UNION_SEND:
                        strCheckDraftToolTip = "工会审稿组";
                        break;

                    case ProcessConstString.TemplateName.YOUTH_LEAGUE_SEND:
                        strCheckDraftToolTip = "团委审稿组";
                        break;
                }
                this.ddlCheckDrafter.ToolTip = string.IsNullOrEmpty(this.ddlType.SelectedValue) ? "请先选择发文类型" : strCheckDraftToolTip;

                switch (base.StepName)
                {
                    #region 拟稿
                    case ProcessConstString.StepName.STEP_DRAFT:
                        this.btnCancel.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");

                        //是否显示撤销按钮
                        this.btnCancel.Visible = this.wfIsDeny.Text == "True";

                        //考虑代理人
                        this.txtDrafter.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.wfDrafterID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                        //this.txtPhoneNum.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.OfficePhone : entity.PhoneNum;

                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender, this.ddlType,
                            this.RedSpanCheckDrafter,this.RedSpanTitle, this.RedSpanMainSender, this.ddlCheckDrafter,
                            this.ddlHostDept, this.txtPhoneNum, this.btnSave, this.btnCheckDraft };
                        break;
                    #endregion

                    #region 审稿
                    case ProcessConstString.StepName.SendStepName.STEP_CHECK:
                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender, this.ucDeptCounterSigns, 
                            this.RedSpanTitle, this.RedSpanMainSender, this.btnSave,this.btnDeptSign, this.btnVerify, this.btnBack };
                        controls.YellowControls = new Control[] { this.txtDeptSigners };
                        controls.DisEnableControls = new Control[] { this.ddlType };
                        break;
                    #endregion

                    #region 部门会签
                    case ProcessConstString.StepName.SendStepName.STEP_DEPT:
                        this.ucAttachment.UCIsEditable = false;
                        this.txtSendDate.Enabled = false;

                        controls.EnableControls = new Control[] { this.TdSign, this.TdSign, this.btnSaveSign, this.btnCompleteSign };
                        controls.DisEnableControls = new Control[] { this.ddlUrgentDegree, this.txtDocumentTitle, this.txtSubjectWord,this.ddlType, 
                        this.txtMainSender, this.txtCopySender, this.txtMyPrompt ,this.txtSendDate};
                        break;
                    #endregion

                    #region 核稿
                    case ProcessConstString.StepName.SendStepName.STEP_VERIFY:
                        //获取任务的秘书
                        this.txtSecretaryChecker.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.wfVerifierID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;

                        controls.EnableControls = new Control[] {this.ucMainSender, this.ucCopySender, this.ucRole,this.RedSpanTitle, this.RedSpanSubjectWord,
                            this.RedSpanMainSender,this.ddlSigner, this.txtLeadSigners, this.btnSave,
                        this.btnLeadSign, this.btnSign, this.btnBack };
                        controls.DisEnableControls = new Control[] { this.ddlType };
                        controls.YellowControls = new Control[] { this.txtLeadSigners };
                        break;
                    #endregion

                    #region 领导会签
                    case ProcessConstString.StepName.SendStepName.STEP_LEADER:
                        this.ucAttachment.UCIsEditable = false;
                        this.txtSendDate.Enabled = false;

                        controls.EnableControls = new Control[] { this.TdSign, this.btnSaveSign, this.btnCompleteSign };
                        controls.DisEnableControls = new Control[] { this.ddlUrgentDegree, this.txtDocumentTitle,this.ddlType, this.ddlType,
                            this.txtSubjectWord, this.txtMainSender, this.txtCopySender, this.txtMyPrompt,this.txtSendDate };
                        break;
                    #endregion

                    #region 签发
                    case ProcessConstString.StepName.SendStepName.STEP_SIGN:
                        this.txtSendDate.Enabled = false;

                        controls.EnableControls = new Control[] { this.TdSign, this.btnDistribution, this.btnBackVerify };
                        controls.DisEnableControls = new Control[] { this.ddlUrgentDegree, this.txtDocumentTitle, this.ddlType,
                            this.txtSubjectWord, this.txtMainSender, this.txtCopySender, this.txtMyPrompt,this.txtSendDate};
                        break;
                    #endregion

                    #region 分发
                    case ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE:
                        if (entity.IsHaveChecked == true)
                        {
                            this.btnCompleteAll.Visible = true;
                            this.btnCompleteAll.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");
                        }

                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender,this.trChuanYue,this.RedSpan_No, this.RedSpan_Year, 
                            this.RedSpan_Num, this.RedSpanTitle,this.RedSpanMainSender, this.TdYearNum, this.txtDocumentYear, 
                            this.txtDocumentNum, this.txtDocumentNo, this.txtShareCount, this.txtSheetCount, this.txtTypist, 
                            this.txtReChecker, this.btnSave, this.btnCheck,this.ucOUCirculate };
                        controls.DisEnableControls = new Control[] { this.ddlType };
                        controls.YellowControls = new Control[] { this.txtDeptName, this.txtUserName };
                        break;
                    #endregion

                    #region 校对
                    case ProcessConstString.StepName.SendStepName.STEP_PROOF:
                        controls.EnableControls = new Control[] { this.RedSpanTitle, this.RedSpanMainSender,
                        this.TdYearNum, this.txtPhoneNum, this.btnSave, this.btnComplete,this.ucMainSender, this.ucCopySender };
                        controls.DisEnableControls = new Control[] { this.ddlType, this.txtLeadSigners, this.txtDeptSigners };

                        this.txtChecker.Text = entity.ReceiveUserName;
                        break;
                    #endregion
                }
                //设置所有控件状态
                controls.SetControls();
            }
            else
            {
                FormsMethod.SetControlAll(this);

                this.ucAttachment.UCIsEditable = false;

                this.txtSendDate.Enabled = false;

                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE)
                {
                    this.trChuanYue.Visible = true;
                    this.TdYearNum.Visible = true;
                    this.ucOUCirculate.Visible = false;
                }

                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_DEPT ||
                    base.StepName == ProcessConstString.StepName.SendStepName.STEP_LEADER ||
                    base.StepName == ProcessConstString.StepName.SendStepName.STEP_SIGN)
                {
                    this.TdSign.Visible = true;
                }

                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_PROOF)
                {
                    this.TdYearNum.Visible = true;
                }
                if (base.IsPreview && base.IsCanDevolve)
                {
                    this.btn_GuiDang.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btn_GuiDang.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                    }
                }
            }
        }
        #endregion

        #region 实体加载
        /// <summary>
        /// 实体加载
        /// </summary>
        protected override void EntityToControl()
        {
            B_DJGTSend entity = base.EntityData != null ? base.EntityData as B_DJGTSend : new B_DJGTSend();

            ucAttachment.UCDataList = entity.FileList;

            //提示信息
            this.txtMyPrompt.Text = entity.MyPrompt;
            this.txtAllPrompt.Text = entity.Prompt;

            //是否核稿退回
            this.wfIsDeny.Text = entity.IsCheckDraftBack.ToString();

            FormsMethod.SetDropDownList(this.ddlCheckDrafter, entity.CheckDrafterID, entity.CheckDrafter);
            FormsMethod.SetDropDownList(this.ddlHostDept, entity.HostDeptID, entity.HostDept);
            FormsMethod.SetDropDownList(this.ddlSigner, entity.SignerID, entity.Signer);

            this.txtDocumentYear.Text = entity.DocumentYear;
            this.txtDocumentNum.Text = entity.DocumentNum;
            this.txtDocumentNo.Text = entity.DocumentNo;
            this.ddlUrgentDegree.SelectedValue = entity.UrgentDegree;
            this.txtDocumentTitle.Text = entity.DocumentTitle;
            this.txtSubjectWord.Text = entity.SubjectWord;
            this.txtMainSender.Text = entity.MainSenders;
            this.txtCopySender.Text = entity.CopySenders;
            //this.txtPhoneNum.Text = entity.PhoneNum;
            this.txtPhoneNum.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.OfficePhone : entity.PhoneNum;
            this.txtShareCount.FSText = entity.ShareCount;
            this.txtSheetCount.FSText = entity.SheetCount;
            this.txtTypist.Text = entity.Typist;
            this.txtChecker.Text = entity.Checker;
            this.txtReChecker.Text = entity.ReChecker;

            //发文日期
            this.txtSendDate.Text = entity.SendDate == DateTime.MinValue ? string.Empty : entity.SendDate.ToString(ConstString.DateFormat.Long);

            //签发日期
            this.txtSignDate.Text = entity.SignDate == DateTime.MinValue ? string.Empty : entity.SignDate.ToString(ConstString.DateFormat.Long);
            this.txtSignCommentView.Text = entity.SignComment;

            //核稿日期
            this.txtSecretaryCheckDate.Text = entity.VerifyDate == DateTime.MinValue ? string.Empty : entity.VerifyDate.ToString(ConstString.DateFormat.Long);

            //审稿日期
            this.txtVerifyDate.Text = entity.CheckDraftDate == DateTime.MinValue ? string.Empty : entity.CheckDraftDate.ToString(ConstString.DateFormat.Long);

            //拟稿日期
            this.txtDraftDate.Text = entity.DraftDate == DateTime.MinValue ? string.Empty : entity.DraftDate.ToString(ConstString.DateFormat.Long);

            //拟稿人
            this.txtDrafter.Text = entity.Drafter;
            this.wfDrafterID.Text = entity.DrafterID;

            //会签部门
            this.txtDeptSigners.Text = entity.IsFormSave ? entity.NewDeptSigners : entity.DeptSigners;
            this.wfDeptSignIDs.Text = entity.IsFormSave ? entity.NewDeptSignerIDs : entity.DeptSignerIDs;

            //公司领导会签
            this.txtLeadSigners.Text = entity.IsFormSave ? entity.NewLeadSigners : entity.LeadSigners;
            this.wfLeaderSignIDs.Text = entity.IsFormSave ? entity.NewLeadSignerIDs : entity.LeadSignerIDs;

            //秘书核稿
            this.txtSecretaryChecker.Text = entity.Verifier;

            //传阅
            this.hDeptID.Value = entity.CirculateDeptIDs;
            this.txtDeptName.Text = entity.CirculateDepts;
            this.hUserID.Value = entity.CirculateLeaderIDs;
            this.txtUserName.Text = entity.CirculateLeaders;

            //发文类型
            this.ddlType.SelectedValue = entity.SendType;

            if (entity != null)
            {
                switch (base.StepName)
                {
                    case ProcessConstString.StepName.STEP_DRAFT:
                        if (!base.IsPreview)
                        {
                            //主办部门
                            OADept.GetDeptByUser(this.ddlHostDept, string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID, 1, true, false);
                            FormsMethod.SelectedDropDownList(this.ddlHostDept, entity.HostDeptID);
                            FormsMethod.SelectedDropDownList(this.ddlCheckDrafter, entity.CheckDrafterID);
                        }
                        break;

                    case ProcessConstString.StepName.SendStepName.STEP_VERIFY:
                        //签发人
                        if (!base.IsPreview)
                        {
                            OAUser.GetUserByRole(this.ddlSigner, OUConstString.RoleName.PartysLead);
                            FormsMethod.SelectedDropDownList(this.ddlSigner, entity.SignerID);
                        }
                        break;

                    case ProcessConstString.StepName.SendStepName.STEP_DEPT:
                        this.txtCounterSigners.Text = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName);
                        this.txtComment.Text = entity.DeptSignComment;
                        break;

                    case ProcessConstString.StepName.SendStepName.STEP_LEADER:
                        this.txtCounterSigners.Text = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName) +
                        FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_LEADER, base.TemplateName);
                        this.txtComment.Text = entity.LeadSignComment;
                        break;

                    case ProcessConstString.StepName.SendStepName.STEP_SIGN:
                        this.txtDeptSigners.Text = FormsMethod.GetSingers(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName);
                        this.txtLeadSigners.Text = FormsMethod.GetSingers(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_LEADER, base.TemplateName);

                        string sign1 = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName);
                        string sign2 = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_LEADER, base.TemplateName);
                        string sign3 = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_SIGN, base.TemplateName);
                        this.txtCounterSigners.Text = sign1 + "\n" + sign2 + "\n" + sign3;
                        this.txtComment.Text = entity.SignComment;
                        break;

                    case ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE:
                        //预设发文号
                        if (!base.IsPreview)
                        {
                            if (entity.IsHaveChecked == false)
                            {
                                B_DocumentNo_SN sn = new B_DocumentNo_SN();
                                this.txtDocumentYear.Text = DateTime.Now.Year.ToString();
                                this.txtDocumentNum.Text = sn.GetNo(base.TemplateName);
                                this.txtDocumentNo.Text = "海核发〔" + DateTime.Now.Year.ToString() + "〕" + this.txtDocumentNum.Text + "号";
                            }
                            else
                            {
                                this.txtDocumentYear.Text = entity.DocumentYear;
                                this.txtDocumentNum.Text = entity.DocumentNum;
                                this.txtDocumentNo.Text = entity.DocumentNo;
                            }
                        }

                        this.txtDeptSigners.Text = entity.DeptHaveSigners;
                        this.txtLeadSigners.Text = entity.LeadHaveSigners;
                        break;

                    case ProcessConstString.StepName.SendStepName.STEP_PROOF:
                        this.txtDeptSigners.Text = entity.DeptHaveSigners;
                        this.txtLeadSigners.Text = entity.LeadHaveSigners;
                        break;
                }
            }

            //党群工作处处长处理后显示label形式的处长姓名与时间
            if (entity.SignDate != DateTime.MinValue)
            {
                this.ddlSigner.Visible = false;
                this.txtSignDate.Visible = false;
                this.lbSigner.Visible = true;
                this.lbSignDate.Visible = true;
                this.lbSigner.Text = entity.Signer;
                this.lbSignDate.Text = entity.SignDate.ToString(ConstString.DateFormat.Long);
            }

            //核稿处理后显示label形式的核稿人姓名与时间
            if (entity.VerifyDate != DateTime.MinValue)
            {
                this.txtSecretaryChecker.Visible = false;
                this.txtSecretaryCheckDate.Visible = false;
                this.lbChecker.Visible = true;
                this.lbSecretaryCheckDate.Visible = true;
                this.lbChecker.Text = entity.Verifier;
                this.lbSecretaryCheckDate.Text = entity.VerifyDate.ToString(ConstString.DateFormat.Long);
            }

            //审稿处理后显示label形式的审稿人姓名与时间
            if (entity.CheckDraftDate != DateTime.MinValue)
            {
                this.ddlCheckDrafter.Visible = false;
                this.txtVerifyDate.Visible = false;
                this.lbCheckDrafter.Visible = true;
                this.lbVerifyDate.Visible = true;
                this.lbCheckDrafter.Text = entity.CheckDrafter;
                this.lbVerifyDate.Text = entity.CheckDraftDate.ToString(ConstString.DateFormat.Long);
            }

            //拟稿处理后显示label形式的拟稿人姓名与时间
            if (entity.DraftDate != DateTime.MinValue)
            {
                this.txtDrafter.Visible = false;
                this.txtDraftDate.Visible = false;
                this.lbDrafter.Visible = true;
                this.lbDraftDate.Visible = true;
                this.lbDrafter.Text = entity.Drafter;
                this.lbDraftDate.Text = entity.DraftDate.ToString(ConstString.DateFormat.Long);
            }

            ////校对处理后显示label形式的校对人姓名与时间
            //if (entity.CreateDate != DateTime.MinValue)
            //{
            //    this.txtChecker.Visible = false;
            //    this.lbCChecker.Visible = true;
            //    this.lbCChecker.Text = entity.Checker + strNewLine + entity.CreateDate;
            //}

        }
        #endregion
        #endregion

        #region 实体赋值
        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="IsSave">是否保存</param>
        /// <returns></returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_DJGTSend entity = base.EntityData != null ? base.EntityData as B_DJGTSend : new B_DJGTSend();

            entity.SendDate = this.txtSendDate.Text == string.Empty ? DateTime.MinValue : Convert.ToDateTime(this.txtSendDate.Text);

            //附件
            entity.FileList = ucAttachment.UCDataList;

            CYiJian YJ = new CYiJian();

            //提示信息
            if (!IsSave)
            {
                if (!string.IsNullOrEmpty(this.txtMyPrompt.Text))
                {
                    entity.MyPrompt = string.Empty;
                    entity.Prompt = this.txtAllPrompt.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" +
                        System.DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:(" + base.StepName + ")" + this.txtMyPrompt.Text + "\n";
                }
            }
            else
            {
                entity.Prompt = this.txtAllPrompt.Text;
                entity.MyPrompt = this.txtMyPrompt.Text;
            }

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:
                    entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;

                    if (this.ddlHostDept.Items.Count > 0)
                    {
                        entity.HostDeptID = this.ddlHostDept.SelectedValue;
                        entity.HostDept = this.ddlHostDept.SelectedItem.Text;
                    }

                    if (this.ddlCheckDrafter.Items.Count > 0)
                    {
                        entity.CheckDrafterID = this.ddlCheckDrafter.SelectedValue;
                        entity.CheckDrafter = this.ddlCheckDrafter.SelectedItem.Text;
                    }

                    entity.PhoneNum = this.txtPhoneNum.Text;
                    entity.DocumentTitle = this.txtDocumentTitle.Text;
                    entity.SubjectWord = this.txtSubjectWord.Text;
                    entity.MainSenders = this.txtMainSender.Text;
                    entity.CopySenders = this.txtCopySender.Text;

                    //拟稿人、拟稿日期
                    entity.Drafter = this.txtDrafter.Text;
                    entity.DrafterID = this.wfDrafterID.Text;
                    entity.DraftDate = DateTime.Now;
                    if (entity.FirstDraftDate == DateTime.MinValue)//第一次的拟稿日期,以前的实体FirstDraftDate没值，取DraftDate。renjinquan+
                    {
                        entity.FirstDraftDate = DateTime.Now;
                    }
                    entity.IsCheckDraftBack = Convert.ToBoolean(this.wfIsDeny.Text);

                    //发文类型
                    entity.SendType = this.ddlType.SelectedValue;
                    break;
                #endregion

                #region 审稿
                case ProcessConstString.StepName.SendStepName.STEP_CHECK:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;
                        entity.PhoneNum = this.txtPhoneNum.Text;
                        entity.DocumentTitle = this.txtDocumentTitle.Text;
                        entity.SubjectWord = this.txtSubjectWord.Text;
                        entity.MainSenders = this.txtMainSender.Text;
                        entity.CopySenders = this.txtCopySender.Text;

                        //if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_BMHQ)//renjinquan改。防止覆盖掉前面的会签人
                        //{
                        //部门会签
                        entity.NewDeptSignerIDs = this.wfDeptSignIDs.Text;
                        entity.NewDeptSigners = this.txtDeptSigners.Text;
                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_BMHQ)//renjinquan改。防止覆盖掉前面的会签人
                        {
                            entity.DeptSignerIDs = SysString.FilterRepeat(entity.DeptSignerIDs + (entity.DeptSignerIDs != string.Empty ? ";" : "") + this.wfDeptSignIDs.Text);
                            entity.DeptSigners = SysString.FilterRepeat(entity.DeptSigners + (entity.DeptSigners != string.Empty ? ";" : "") + this.txtDeptSigners.Text);
                            entity.NewDeptSignerIDs = "";
                            entity.NewDeptSigners = "";
                        }
                        //entity.DeptSignerIDs = this.wfDeptSignIDs.Text;
                        //entity.DeptSigners = this.txtDeptSigners.Text;
                        //}

                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_TJHG)
                        {
                            //党群秘书组
                            string[] array = OAUser.GetUserByRoleName(OUConstString.RoleName.PartysSecretary);
                            entity.VerifierIDs = array[0].ToString();
                            entity.Verifiers = array[1].ToString();
                        }
                        entity.CheckDraftDate = DateTime.Now;

                        //清除CommonList值
                        entity.CommentList.Clear();
                    }
                    else
                    {
                        entity.IsCheckDraftBack = true;
                    }
                    break;
                #endregion

                #region 部门会签
                case ProcessConstString.StepName.SendStepName.STEP_DEPT:
                    if (IsSave)
                    {
                        entity.DeptSignComment = this.txtComment.Text;
                    }
                    else
                    {
                        entity.DeptSignComment = string.Empty;

                        if (!string.IsNullOrEmpty(this.txtComment.Text.Trim()))
                        {
                            //意见列表
                            YJ.UserID = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                            YJ.UserName = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                            YJ.ViewName = ProcessConstString.StepName.SendStepName.STEP_DEPT;
                            YJ.FinishTime = DateTime.Now.ToString();
                            YJ.Content = this.txtComment.Text;
                            entity.CommentList.Add(YJ);
                        }
                    }
                    break;

                #endregion

                #region 核稿
                //秘书核稿
                case ProcessConstString.StepName.SendStepName.STEP_VERIFY:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;
                        entity.DocumentTitle = this.txtDocumentTitle.Text;
                        entity.SubjectWord = this.txtSubjectWord.Text;

                        if (this.ddlSigner.Items.Count > 0)
                        {
                            //签发人
                            entity.SignerID = this.ddlSigner.SelectedValue;
                            entity.Signer = this.ddlSigner.SelectedItem.Text;
                        }

                        //领导会签
                        entity.NewLeadSignerIDs = this.wfLeaderSignIDs.Text;
                        entity.NewLeadSigners = this.txtLeadSigners.Text;
                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_LDHQ)//renjinquan改。防止覆盖掉前面的会签人
                        {
                            entity.LeadSignerIDs = SysString.FilterRepeat(entity.LeadSignerIDs + (entity.LeadSignerIDs != string.Empty ? ";" : "") + this.wfLeaderSignIDs.Text);
                            entity.LeadSigners = SysString.FilterRepeat(entity.LeadSigners + (entity.LeadSigners != string.Empty ? ";" : "") + this.txtLeadSigners.Text);
                            entity.NewLeadSignerIDs = "";
                            entity.NewLeadSigners = "";
                            //entity.LeadSignerIDs = this.wfLeaderSignIDs.Text;
                            //entity.LeadSigners = this.txtLeadSigners.Text;
                        }

                        //秘书核稿
                        entity.Verifier = this.txtSecretaryChecker.Text;
                        entity.VerifierID = this.wfVerifierID.Text;
                        entity.VerifyDate = DateTime.Now;

                        entity.MainSenders = this.txtMainSender.Text;
                        entity.CopySenders = this.txtCopySender.Text;

                        //清除CommonList值
                        entity.CommentList.Clear();
                    }
                    break;
                #endregion

                #region 领导会签
                case ProcessConstString.StepName.SendStepName.STEP_LEADER:
                    if (IsSave)
                    {
                        entity.LeadSignComment = this.txtComment.Text;
                    }
                    else
                    {
                        entity.LeadSignComment = string.Empty;
                        entity.CommentList.Clear();
                        if (!string.IsNullOrEmpty(this.txtComment.Text.Trim()))
                        {
                            YJ.UserID = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                            YJ.UserName = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                            YJ.ViewName = ProcessConstString.StepName.SendStepName.STEP_LEADER;
                            YJ.FinishTime = DateTime.Now.ToString();
                            YJ.Content = this.txtComment.Text;
                            entity.CommentList.Add(YJ);
                        }
                    }
                    break;
                #endregion

                #region 签发
                case ProcessConstString.StepName.SendStepName.STEP_SIGN:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //党群文书组
                        string[] array = OAUser.GetUserByRoleName(OUConstString.RoleName.PartysDocument);
                        entity.AssignerIDs = array[0].ToString();
                        entity.Assigners = array[1].ToString();
                        entity.SignDate = DateTime.Now;
                        entity.SendDate = DateTime.Now;

                        entity.SubjectWord = this.txtSubjectWord.Text;

                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_QF)
                        {
                            entity.DeptHaveSigners = this.txtDeptSigners.Text;
                            entity.LeadHaveSigners = this.txtLeadSigners.Text;
                        }
                    }

                    if (IsSave)
                    {
                        entity.SignComment = this.txtComment.Text;
                    }
                    else
                    {
                        entity.SignComment = this.txtComment.Text;
                        if (!string.IsNullOrEmpty(this.txtComment.Text.Trim()))
                        {
                            YJ.UserID = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                            YJ.UserName = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                            YJ.ViewName = ProcessConstString.StepName.SendStepName.STEP_SIGN;
                            YJ.FinishTime = DateTime.Now.ToString();
                            YJ.Content = this.txtComment.Text;
                            entity.CommentList.Add(YJ);
                        }
                    }
                    break;

                #endregion

                #region 分发
                case ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE:
                    entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;
                    entity.DocumentYear = this.txtDocumentYear.Text;
                    entity.DocumentNum = this.txtDocumentNum.Text;
                    entity.DocumentNo = this.txtDocumentNo.Text;

                    entity.DocumentTitle = this.txtDocumentTitle.Text;
                    entity.SubjectWord = this.txtSubjectWord.Text;
                    entity.MainSenders = this.txtMainSender.Text;
                    entity.CopySenders = this.txtCopySender.Text;

                    entity.ShareCount = this.txtShareCount.Text;
                    entity.SheetCount = this.txtSheetCount.Text;

                    entity.Typist = this.txtTypist.Text;
                    entity.ReChecker = this.txtReChecker.Text;

                    entity.AssignerID = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                    entity.Assigner = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;

                    //传阅
                    entity.CirculateDeptIDs = this.hDeptID.Value;
                    entity.CirculateDepts = this.txtDeptName.Text;
                    entity.CirculateLeaderIDs = this.hUserID.Value;
                    entity.CirculateLeaders = this.txtUserName.Text;
                    break;
                #endregion

                #region 校对
                case ProcessConstString.StepName.SendStepName.STEP_PROOF:
                    entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;
                    entity.PhoneNum = this.txtPhoneNum.Text;

                    entity.DocumentTitle = this.txtDocumentTitle.Text;
                    entity.SubjectWord = this.txtSubjectWord.Text;
                    entity.MainSenders = this.txtMainSender.Text;
                    entity.CopySenders = this.txtCopySender.Text;
                    entity.Checker = this.txtChecker.Text;

                    entity.IsHaveChecked = true;
                    break;
                #endregion
            }
            return entity;
        }
        #endregion

        #region 表单事件

        #region 绑定表单事件
        /// <summary>
        /// 绑定表单事件
        /// </summary>
        protected void SubmitEvents()
        {
            EventHandler SubmitHandler = new EventHandler(SubmitBtn_Click);
            //保存
            this.btnSave.Click += SubmitHandler;
            this.btnSaveSign.Click += SubmitHandler;
            //撤销
            this.btnCancel.Click += SubmitHandler;
            //退回
            this.btnBack.Click += SubmitHandler;
            this.btnBackVerify.Click += SubmitHandler;
            //提交核稿
            this.btnCheckDraft.Click += SubmitHandler;
            //提交审核
            this.btnVerify.Click += SubmitHandler;
            //提交部门会签
            this.btnDeptSign.Click += SubmitHandler;
            //提交签发
            this.btnSign.Click += SubmitHandler;
            //签发
            this.btnDistribution.Click += SubmitHandler;
            //提交领导会签
            this.btnLeadSign.Click += SubmitHandler;
            //提交校对
            this.btnCheck.Click += SubmitHandler;
            //完成
            this.btnComplete.Click += SubmitHandler;
            //签发完成
            this.btnCompleteSign.Click += SubmitHandler;
            //完成归档
            this.btnCompleteAll.Click += SubmitHandler;
        }
        #endregion

        /// <summary>
        /// 提交按钮处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //提交动作
                string strActionName = ((Button)sender).Text.Trim();
                base.SubAction = strActionName;

                string strErrorMessage = string.Empty;

                //保存
                if (strActionName == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_DJGTSend entity = ControlToEntity(true) as B_DJGTSend;
                    entity.SubmitAction = strActionName;
                    base.FormSubmit(true, strActionName, null, entity);
                }
                else
                {
                    B_DJGTSend entity = ControlToEntity(false) as B_DJGTSend;
                    entity.SubmitAction = strActionName;

                    //撤销
                    if (strActionName == ProcessConstString.SubmitAction.ACTION_CANCEL)
                    {
                        base.FormCancel(entity);
                    }
                    else
                    {
                        //返回验证提示和流程提示
                        entity.GetSubmitMessage(base.StepName, strActionName, ref strErrorMessage);
                        if (!string.IsNullOrEmpty(strErrorMessage))
                        {
                            JScript.ShowMsgBox(this.Page, strErrorMessage, false);
                            return;
                        }
                        else
                        {
                            switch (base.SubAction)
                            {
                                case ProcessConstString.SubmitAction.CompanySendAction.ACTION_TJJD:
                                    B_DocumentNo_SN sn = new B_DocumentNo_SN();
                                    if (!sn.UpdateNo(ProcessConstString.TemplateName.DJGT_Send, entity.ProcessID, entity.DocumentYear, entity.DocumentNum, entity.DocumentNo))
                                    {
                                        return;
                                    }
                                    break;

                                case ProcessConstString.SubmitAction.CompanySendAction.ACTION_WCGD:
                                    if (!string.IsNullOrEmpty(entity.CirculateDeptIDs) || !string.IsNullOrEmpty(entity.CirculateLeaderIDs))
                                    {
                                        base.Circulate(entity.CirculateDeptIDs, "1", string.Empty, entity.CirculateLeaderIDs, "1", false, string.Empty, false);
                                    }

                                    //党纪工团归档
                                    try
                                    {
                                        string strMessage = string.Empty;
                                        this.Devolve(out strMessage);
                                        base.Devolved(base.ProcessID, base.TemplateName);
                                        JScript.Alert("归档成功！\\n流水号：" + strMessage, false);
                                    }
                                    catch (Exception ex)
                                    {
                                        base.WriteLog(ex.Message);
                                        JScript.Alert("归档失败！请查看配置是否正确！", false);
                                        return;
                                    }
                                    break;
                            }

                            //调用工作流                  
                            Hashtable nValues = entity.GetProcNameValue(base.StepName, strActionName);
                            base.FormSubmit(false, strActionName, nValues, entity);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                JScript.ShowMsgBox(this.Page, ex.Message, false);
            }
        }

        /// <summary>
        /// 获取审稿人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //党纪工团审稿
            switch (this.ddlType.SelectedValue)
            {
                case ProcessConstString.TemplateName.PARTY_SEND:
                    OAUser.GetUserByRole(this.ddlCheckDrafter, OUConstString.RoleName.PartyDisciplineCheckDraft);
                    break;

                case ProcessConstString.TemplateName.DISCIPLINE_SEND:
                    OAUser.GetUserByRole(this.ddlCheckDrafter, OUConstString.RoleName.PartyDisciplineCheckDraft);
                    break;

                case ProcessConstString.TemplateName.TRADE_UNION_SEND:
                    OAUser.GetUserByRole(this.ddlCheckDrafter, OUConstString.RoleName.TradeUnionCheckDraft);
                    break;

                case ProcessConstString.TemplateName.YOUTH_LEAGUE_SEND:
                    OAUser.GetUserByRole(this.ddlCheckDrafter, OUConstString.RoleName.YouthLeagueCheckDraft);
                    break;
            }
        }
        #endregion

        protected void btn_GuiDang_Click(object sender, EventArgs e)
        {
            //党纪工团归档
            try
            {
                string strMessage = string.Empty;
                this.Devolve(out strMessage);
                base.Devolved(base.ProcessID, base.TemplateName);
                JScript.Alert("归档成功！\\n流水号：" + strMessage, false);
            }
            catch (Exception ex)
            {
                base.WriteLog(ex.Message);
                JScript.Alert("归档失败！请查看配置是否正确！", false);
                return;
            }
        }
    }
}