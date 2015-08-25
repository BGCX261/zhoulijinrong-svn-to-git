//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：公司发文视图
// 
// 
// 创建标识：wangbinyi 2009-12-28
//
// 修改标识：renjinquan 2010-5-10
// 修改描述：修改ControlToEntity函数，去除DocumenTitle赋值时符号的替换

//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Text.RegularExpressions;
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
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI.WorkFlow.Send
{
    public partial class UC_CompanySend : FormsUIBase
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
            EntitySend entity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();

            //附件
            ucAttachment.UCTemplateName = base.TemplateName;
            ucAttachment.UCProcessID = base.ProcessID;
            ucAttachment.UCWorkItemID = base.WorkItemID;
            ucAttachment.UCTBID = base.IdentityID.ToString();

            //设置意见用户控件的属性
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
            this.ucMainSender.UCTemplateName = base.TemplateName;
            this.ucMainSender.UCFormName = "主送";
            this.ucMainSender.UCDeptAndUserControl = this.txtMainSender.ClientID;

            //抄送
            this.ucCopySender.UCSelectType = "2";
            this.ucCopySender.UCDeptAndUserControl = this.txtCopySender.ClientID;
            this.ucCopySender.UCTemplateName = base.TemplateName;
            this.ucCopySender.UCFormName = "抄送";

            //部门会签
            this.ucDeptCounterSigns.UCSelectType = "0";
            this.ucDeptCounterSigns.UCDeptIDControl = this.txtCounterSignDept.ClientID;
            this.ucDeptCounterSigns.UCDeptNameControl = this.txtDeptSigners.ClientID;
            this.ucDeptCounterSigns.UCDeptTreeUserIDControl = this.txtCounterSignDeptLeaders.ClientID;
            this.ucDeptCounterSigns.UCLevel = "1";
            this.ucDeptCounterSigns.UCDeptShowType = "1000";

            //会签领导
            this.ucRole.UCUserIDControl = this.txtComCounterSignLeaders.ClientID;
            this.ucRole.UCUserNameControl = this.txtLeadSigners.ClientID;
            this.ucRole.UCRoleName = ConstString.RoleName.COMPANY_LEADER;
            this.ucRole.UCIsSingle = false;

            //传阅
            this.ucOUCirculate.UCDeptIDControl = this.hDeptID.ClientID;
            this.ucOUCirculate.UCDeptNameControl = this.txtDeptName.ClientID;
            this.ucOUCirculate.UCRoleUserIDControl = this.hUserID.ClientID;
            this.ucOUCirculate.UCRoleUserNameControl = this.txtUserName.ClientID;
            this.ucOUCirculate.UCRole = OUConstString.RoleName.COMPANY_LEADER;
            this.ucOUCirculate.UCSelectType = "0";
            this.ucOUCirculate.UCDeptShowType = "1010";

            //追加分发
            this.ucOUCirculateAppend.UCDeptIDControl = this.hDeptID1.ClientID;
            this.ucOUCirculateAppend.UCDeptNameControl = this.txtDeptName1.ClientID;
            this.ucOUCirculateAppend.UCRoleUserIDControl = this.hUserID1.ClientID;
            this.ucOUCirculateAppend.UCRoleUserNameControl = this.txtUserName1.ClientID;
            this.ucOUCirculateAppend.UCRole = OUConstString.RoleName.COMPANY_LEADER;
            this.ucOUCirculateAppend.UCSelectType = "0";
            this.ucOUCirculateAppend.UCDeptShowType = "1010";

            //发文卡
            this.ucSendCard.UCTemplateName = base.TemplateName;
            this.ucSendCard.UCProcessID = base.ProcessID;
            this.ucSendCard.UCWorkItemID = base.WorkItemID;
            this.ucSendCard.UCStepName = "发文卡";

            txtLeadSigners.Attributes.Add("readOnly", "readOnly");
            txtDeptSigners.Attributes.Add("readOnly", "readOnly");

            OAControl controls = new OAControl();

            //if (!base.IsPreview)
            //{
            //    this.txtDocumentTitle.ToolTip = "100字符以内";
            //    this.txtSubjectWord.ToolTip = "100字符以内";
            //    this.txtComment.ToolTip = "2000字符以内";
            //    this.ddlVerifier.ToolTip = "负责人+处级以上+部门领导";
            //    this.txtShareCount.ToolTip = "正整数";
            //    this.txtSheetCount.ToolTip = "正整数";
            //}

            if (!base.IsPreview)
            {
                switch (base.StepName)
                {
                    #region 拟稿
                    case ProcessConstString.StepName.STEP_DRAFT:
                        this.btnCancel.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");

                        //是否显示撤销按钮
                        this.btnCancel.Visible = this.txtIsDeny.Text == "True";

                        //考虑代理人
                        this.txtDrafter.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.txtDrafterID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                        //this.txtPhoneNum.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.OfficePhone : entity.PhoneNum;

                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender, 
                            this.RedSpanCheckDrafter,this.RedSpanTitle, this.RedSpanMainSender, this.ddlVerifier,
                            this.ddlHostDept, this.txtPhoneNum, this.btnSave, this.btnCheckDraft };
                        break;
                    #endregion

                    #region 审稿
                    case ProcessConstString.StepName.SendStepName.STEP_CHECK:
                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender, this.ucDeptCounterSigns, 
                            this.RedSpanTitle, this.RedSpanMainSender, this.btnSave,this.btnDeptSign, this.btnVerify, this.btnBack };
                        controls.YellowControls = new Control[] { this.txtDeptSigners };
                        break;
                    #endregion

                    #region 部门会签
                    case ProcessConstString.StepName.SendStepName.STEP_DEPT:
                        this.ucAttachment.UCIsEditable = false;
                        this.txtSendDate.Enabled = false;

                        controls.EnableControls = new Control[] { this.TdSign, this.TdSign, this.btnSaveSign, this.btnCompleteSign };
                        controls.DisEnableControls = new Control[] { this.ddlUrgentDegree, this.txtDocumentTitle, this.txtSubjectWord, 
                        this.txtMainSender, this.txtCopySender, this.txtMyPrompt ,this.txtSendDate};
                        break;
                    #endregion

                    #region 核稿
                    case ProcessConstString.StepName.SendStepName.STEP_VERIFY:
                        //获取任务的秘书
                        this.txtSecretaryChecker.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.txtVerifierID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;

                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender, this.ucRole,this.RedSpanTitle, this.RedSpanSubjectWord,
                            this.RedSpanMainSender,this.ddlSigner, this.txtLeadSigners, this.btnSave, this.btnZhuRenSign, 
                        this.btnLeadSign, this.btnSign, this.btnBack };
                        //controls.DisEnableControls = new Control[] { this.txtMainSender, this.txtCopySender };
                        controls.YellowControls = new Control[] { this.txtLeadSigners };
                        break;
                    #endregion

                    #region 主任核稿
                    case ProcessConstString.StepName.SendStepName.STEP_ZRVERIFY:
                        controls.EnableControls = new Control[] { this.ucMainSender, this.ucCopySender, this.RedSpanTitle, this.RedSpanSubjectWord, this.RedSpanMainSender, this.btnSave, this.btnComplete };
                        //controls.DisEnableControls = new Control[] { this.txtMainSender, this.txtCopySender };

                        //处长副处长
                        this.txtDirectorChecker.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.txtDirectorCheckerID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                        break;
                    #endregion

                    #region 领导会签
                    case ProcessConstString.StepName.SendStepName.STEP_LEADER:
                        this.ucAttachment.UCIsEditable = false;
                        this.txtSendDate.Enabled = false;

                        controls.EnableControls = new Control[] { this.TdSign, this.btnSaveSign, this.btnCompleteSign };
                        controls.DisEnableControls = new Control[] { this.ddlUrgentDegree, this.txtDocumentTitle, 
                            this.txtSubjectWord, this.txtMainSender, this.txtCopySender, this.txtMyPrompt,this.txtSendDate };
                        break;
                    #endregion

                    #region 签发
                    case ProcessConstString.StepName.SendStepName.STEP_SIGN:
                        this.txtSendDate.Enabled = false;

                        controls.EnableControls = new Control[] { this.TdSign, this.btnDistribution, this.btnBackVerify };
                        controls.DisEnableControls = new Control[] { this.ddlUrgentDegree, this.txtDocumentTitle, 
                            this.txtSubjectWord, this.txtMainSender, this.txtCopySender, this.txtMyPrompt,this.txtSendDate};
                        break;
                    #endregion

                    #region 分发
                    case ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE:
                        //controls.EnableControls = new Control[] {this.RedSpan_No, this.RedSpan_Year, 
                        //    this.RedSpan_Num, this.RedSpanTitle,this.RedSpanMainSender, this.TdYearNum, this.txtDocumentYear, 
                        //    this.txtDocumentNum, this.txtDocumentNo, this.txtShareCount, this.txtSheetCount, this.txtTypist, 
                        //    this.txtReChecker, this.btnSave, this.btnCheck,this.ucOUCirculate };
                        controls.EnableControls = new Control[] {this.RedSpan_No, this.RedSpan_Year, 
                            this.RedSpan_Num, this.RedSpanTitle,this.RedSpanMainSender, this.TdYearNum, this.txtDocumentYear, 
                            this.txtDocumentNum, this.txtDocumentNo, this.txtShareCount, this.txtSheetCount, this.txtTypist, 
                            this.txtReChecker, this.btnSave, this.btnCheck,this.ucOUCirculate,this.ucMainSender,this.ucCopySender};//2010-4-29
                        //controls.DisEnableControls = new Control[] { this.txtMainSender, this.txtCopySender };
                        controls.YellowControls = new Control[] { this.txtDeptName, this.txtUserName };

                        if (entity.IsSubmitCheck)
                        {
                            this.btnCompleteAll.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");
                            this.btnCompleteAll.Visible = true;
                            this.trChuanYue.Visible = true;
                        }
                        else
                        {
                            this.trChuanYue.Visible = false;
                        }
                        break;
                    #endregion

                    #region 校对
                    case ProcessConstString.StepName.SendStepName.STEP_PROOF:
                        controls.EnableControls = new Control[] { this.RedSpanTitle, this.RedSpanMainSender,
                        this.TdYearNum, this.txtPhoneNum, this.btnSave, this.btnComplete ,this.ucMainSender,this.ucCopySender};

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

                    //查看自己
                    if (entity.ReceiveUserID == CurrentUserInfo.UserName)
                    {
                        this.btnAddFenFa.Attributes.Add("onclick", "javascript: if(!checkJiXuChuanYue()){return false;}else{DisableButtons();}");
                        this.btnAddFenFa.Visible = true;
                        this.ucSendCard.Visible = true;
                        this.trAddFenFa.Visible = true;
                        controls.YellowControls = new Control[] { this.txtUserName1, this.txtDeptName1 };
                        controls.SetControls();
                    }
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
                if (base.IsCanDevolve)
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
            EntitySend entity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();

            ucAttachment.UCDataList = entity.FileList;

            if (entity != null)
            {
                if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
                {
                    //主办部门
                    OADept.GetDeptByUser(this.ddlHostDept, string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID, 1, true, false);

                    FormsMethod.SelectedDropDownList(this.ddlHostDept, entity.HostDept);

                    //负责人+处级以上+部门领导
                    if (this.ddlHostDept.Items.Count > 0)
                    {
                        OAUser.GetUserByDeptPost(this.ddlVerifier, this.ddlHostDept.SelectedValue, OUConstString.PostName.FUCHUZHANG, true, true);
                    }
                    FormsMethod.SelectedDropDownList(this.ddlVerifier, entity.CheckDrafterID);
                }
                else
                {
                    FormsMethod.SetDropDownList(this.ddlVerifier, entity.CheckDrafterID, entity.CheckDrafterName);

                    FormsMethod.SetDropDownList(this.ddlHostDept, entity.HostDept, entity.HostDeptName);
                }

                //发文年度、发文序号、发文号
                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE && !base.IsPreview)
                {
                    //预设发文号
                    if (entity.IsSubmitCheck == false)
                    {
                        B_DocumentNo_SN sn = new B_DocumentNo_SN();
                        this.txtDocumentYear.Text = DateTime.Now.Year.ToString();
                        this.txtDocumentNum.Text = sn.GetNo(base.TemplateName);
                        this.txtDocumentNo.Text = "〔" + DateTime.Now.Year.ToString() + "〕";
                    }
                    else
                    {
                        this.txtDocumentYear.Text = entity.DocumentYear;
                        this.txtDocumentNum.Text = entity.DocumentNum;
                        this.txtDocumentNo.Text = entity.DocumentNo;
                    }
                }
                else
                {
                    this.txtDocumentYear.Text = entity.DocumentYear;
                    this.txtDocumentNum.Text = entity.DocumentNum;
                    this.txtDocumentNo.Text = entity.DocumentNo;
                }

                this.ddlUrgentDegree.SelectedValue = entity.UrgentDegree;

                this.txtDocumentTitle.Text = entity.DocumentTitle;
                this.txtSubjectWord.Text = entity.SubjectWord;
                this.txtMainSender.Text = entity.MainSenders;
                this.txtCopySender.Text = entity.CopySenders;

                //发文日期
                this.txtSendDate.Text = entity.SendDate == DateTime.MinValue ? string.Empty : entity.SendDate.ToString(ConstString.DateFormat.Long);

                //签发日期
                this.txtSignDate.Text = entity.SignDate == DateTime.MinValue ? string.Empty : entity.SignDate.ToString(ConstString.DateFormat.Long);
                this.txtSignCommentView.Text = entity.SignComment;

                //会签部门
                if (entity.IsFormSave)
                {
                    this.txtDeptSigners.Text = entity.NewDeptSigners;
                    this.txtCounterSignDept.Text = entity.NewDeptSIDs;
                    this.txtCounterSignDeptLeaders.Text = entity.NewDeptSignerIDs;
                }
                else
                {
                    this.txtDeptSigners.Text = entity.DeptSigners;
                    this.txtCounterSignDept.Text = entity.DeptSIDs;
                    this.txtCounterSignDeptLeaders.Text = entity.DeptSignerIDs;
                }
                //公司领导会签
                this.txtLeadSigners.Text = entity.IsFormSave ? entity.NewLeadSigners : entity.LeadSigners;
                this.txtComCounterSignLeaders.Text = entity.IsFormSave ? entity.NewLeadSignerIDs : entity.LeadSignerIDs;

                //审稿日期
                this.txtVerifyDate.Text = entity.CheckDate == DateTime.MinValue ? string.Empty : entity.CheckDate.ToString(ConstString.DateFormat.Long);

                this.txtDrafter.Text = entity.Drafter;
                this.txtDraftDate.Text = entity.DraftDate == DateTime.MinValue ? string.Empty : entity.DraftDate.ToString(ConstString.DateFormat.Long);

                //this.txtPhoneNum.Text = entity.PhoneNum;
                this.txtPhoneNum.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.OfficePhone : entity.PhoneNum;
                this.txtShareCount.FSText = entity.ShareCount;
                this.txtSheetCount.FSText = entity.SheetCount;
                this.txtTypist.Text = entity.Typist;
                this.txtChecker.Text = entity.Checker;
                this.txtReChecker.Text = entity.ReChecker;

                //是否核稿退回
                this.txtIsDeny.Text = entity.IsHeGaoBack.ToString();

                //会签意见
                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_DEPT)
                {
                    this.txtCounterSigners.Text = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName);
                    this.txtComment.Text = entity.DeptSignComment;
                }
                else if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_LEADER)
                {
                    this.txtCounterSigners.Text = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName) +
                        FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_LEADER, base.TemplateName);
                    this.txtComment.Text = entity.LeadSignComment;
                }
                else if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_SIGN)
                {
                    this.txtDeptSigners.Text = FormsMethod.GetSingers(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName);
                    this.txtLeadSigners.Text = FormsMethod.GetSingers(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_LEADER, base.TemplateName);

                    string sign1 = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_DEPT, base.TemplateName);
                    string sign2 = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_LEADER, base.TemplateName);
                    string sign3 = FormsMethod.GetHaveSignInfo(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.SendStepName.STEP_SIGN, base.TemplateName);
                    this.txtCounterSigners.Text = sign1 + "\n" + sign2 + "\n" + sign3;
                    this.txtComment.Text = entity.SignComment;
                }
                else
                {
                    this.txtMyPrompt.Text = entity.MyPrompt;
                }

                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_VERIFY && !base.IsPreview)
                {
                    //签发人
                    OAUser.GetUserByRole(this.ddlSigner, OUConstString.RoleName.COMPANY_LEADER);
                    FormsMethod.SelectedDropDownList(this.ddlSigner, entity.Signer);
                }
                else
                {
                    if (string.IsNullOrEmpty(entity.SignerName))
                    {
                        FormsMethod.SetDropDownList(this.ddlSigner, entity.Signer, OAUser.GetUserName(entity.Signer));
                    }
                    else
                    {
                        FormsMethod.SetDropDownList(this.ddlSigner, entity.Signer, entity.SignerName);
                    }
                }

                if (base.StepName == ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE ||
                    base.StepName == ProcessConstString.StepName.SendStepName.STEP_PROOF)
                {
                    this.txtDeptSigners.Text = entity.DeptHaveSigners;
                    this.txtLeadSigners.Text = entity.LeadHaveSigners;
                }

                //秘书核稿
                this.txtSecretaryChecker.Text = entity.Verifier;
                this.txtSecretaryCheckDate.Text = entity.VerifyDate == DateTime.MinValue ? string.Empty : entity.VerifyDate.ToString(ConstString.DateFormat.Long);

                //主任核稿
                this.txtDirectorChecker.Text = entity.ZhuRenSigner;
                this.txtDirectorCheckDate.Text = entity.ZhuRenSignDate == DateTime.MinValue ? string.Empty : entity.ZhuRenSignDate.ToString(ConstString.DateFormat.Long);

                //提示信息
                this.txtMyPrompt.Text = entity.MyPrompt;
                this.txtAllPrompt.Text = entity.Prompt;

                //传阅
                this.hDeptID.Value = entity.CirculateIDs;
                this.txtDeptName.Text = entity.CirculateNames;
                this.hUserID.Value = entity.CirculateLeadIDs;
                this.txtUserName.Text = entity.CirculateLeadNames;

                this.hDeptID1.Value = entity.CirculateAddIDs;
                this.txtDeptName1.Text = entity.CirculateAddNames;
                this.hUserID1.Value = entity.CirculateAddLeadIDs;
                this.txtUserName1.Text = entity.CirculateAddLeadNames;

                //审稿后显示label形式的审稿人与时间
                if (entity.CheckDate != DateTime.MinValue)
                {
                    this.ddlVerifier.Visible = false;
                    this.txtVerifyDate.Visible = false;
                    this.lbShenGaoRen.Visible = true;
                    this.lbVerifyDate.Visible = true;
                    this.lbShenGaoRen.Text = entity.CheckDrafterName;
                    this.lbVerifyDate.Text = entity.CheckDate.ToString(ConstString.DateFormat.Long);
                }

                //秘书核稿后显示label形式的秘书与时间
                if (entity.VerifyDate != DateTime.MinValue)
                {
                    this.txtSecretaryChecker.Visible = false;
                    this.txtSecretaryCheckDate.Visible = false;
                    this.lbHeGaoRenMiShu.Visible = true;
                    this.lbSecretaryCheckDate.Visible = true;
                    this.lbHeGaoRenMiShu.Text = entity.Verifier;
                    this.lbSecretaryCheckDate.Text = entity.VerifyDate.ToString(ConstString.DateFormat.Long);
                }

                //主任核稿后显示label形式的主任与时间
                if (entity.ZhuRenSignDate != DateTime.MinValue)
                {
                    this.txtDirectorChecker.Visible = false;
                    this.txtDirectorCheckDate.Visible = false;
                    this.lbHeGaoZhuRen.Visible = true;
                    this.lbHeGaoZhuRenDate.Visible = true;
                    this.lbHeGaoZhuRen.Text = entity.ZhuRenSigner;
                    this.lbHeGaoZhuRenDate.Text = entity.ZhuRenSignDate.ToString(ConstString.DateFormat.Long);
                }

                //拟稿后显示label形式的拟稿人与时间
                if (entity.DraftDate != DateTime.MinValue)
                {
                    this.txtDrafter.Visible = false;
                    this.txtDraftDate.Visible = false;
                    this.lbNiGaoRen.Visible = true;
                    this.lbDraftDate.Visible = true;
                    this.lbNiGaoRen.Text = entity.Drafter;
                    this.lbDraftDate.Text = entity.DraftDate.ToString(ConstString.DateFormat.Long);
                }

                //////校对后显示label形式的校对人与时间
                ////if (entity.CheckDate != DateTime.MinValue)
                ////{
                //    this.txtChecker.Visible = false;
                //    this.lbJiaoDuiRen.Visible = true;
                //    this.lbJiaoDuiRen.Text = entity.Checker + strNewLine + entity.CreateDate.ToString(ConstString.DateFormat.Long);
                //}

                //签发后显示label形式的签发人与时间
                if (entity.SignDate != DateTime.MinValue)
                {
                    //this.ddlSigner.Visible = false;
                    //this.txtSignDate.Visible = false;
                    //this.lbSignDate.Visible = true;
                    //this.lbSigner.Visible = true;
                    this.lbSigner.Text = entity.SignerName;
                    this.lbSignDate.Text = entity.SignDate.ToString(ConstString.DateFormat.Long);
                }

            }
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
            EntitySend entity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();

            entity.SendDate = this.txtSendDate.Text == string.Empty ? DateTime.MinValue : Convert.ToDateTime(this.txtSendDate.Text);

            //附件
            entity.FileList = ucAttachment.UCDataList;

            //意见列表
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

            //数据补偿（老版本没保存）
            if (string.IsNullOrEmpty(entity.HostDeptName))
            {
                entity.HostDeptName = OADept.GetDeptName(entity.HostDept);
            }

            if (string.IsNullOrEmpty(entity.SignerName))
            {
                entity.SignerName = OAUser.GetUserName(entity.Signer);
            }

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:
                    entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;

                    if (this.ddlHostDept.Items.Count > 0)
                    {
                        entity.HostDept = this.ddlHostDept.SelectedValue;
                        entity.HostDeptName = this.ddlHostDept.SelectedItem.Text;
                    }

                    if (this.ddlVerifier.Items.Count > 0)
                    {
                        entity.CheckDrafterID = this.ddlVerifier.SelectedValue;
                        entity.CheckDrafter = this.ddlVerifier.SelectedValue;
                        entity.CheckDrafterName = this.ddlVerifier.SelectedItem.Text;
                    }

                    entity.PhoneNum = this.txtPhoneNum.Text;
                    //entity.DocumentTitle = SysString.Text2Html(this.txtDocumentTitle.Text);
                    entity.DocumentTitle = this.txtDocumentTitle.Text;//renjinquan+
                    entity.SubjectWord = this.txtSubjectWord.Text;
                    entity.MainSenders = this.txtMainSender.Text;
                    entity.CopySenders = this.txtCopySender.Text;

                    //拟稿人、拟稿日期
                    entity.Drafter = this.txtDrafter.Text;
                    entity.DrafterID = this.txtDrafterID.Text;

                    entity.DraftDate = DateTime.Now;
                    if (entity.FirstDraftDate == DateTime.MinValue)//第一次的拟稿日期,以前的实体FirstDraftDate没值，取DraftDate。renjinquan+
                    {
                        entity.FirstDraftDate = DateTime.Now;
                    }

                    entity.IsHeGaoBack = Convert.ToBoolean(this.txtIsDeny.Text);
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
                        //会签部门
                        entity.NewDeptSIDs = this.txtCounterSignDept.Text;
                        entity.NewDeptSignerIDs = this.txtCounterSignDeptLeaders.Text;
                        entity.NewDeptSigners = this.txtDeptSigners.Text;
                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_BMHQ)
                        {
                            entity.DeptSIDs = SysString.FilterRepeat(entity.DeptSIDs + (entity.DeptSIDs != string.Empty ? ";" : "") + this.txtCounterSignDept.Text);
                            entity.DeptSigners = SysString.FilterRepeat(entity.DeptSigners + (entity.DeptSigners != string.Empty ? ";" : "") + this.txtDeptSigners.Text);
                            //entity.DeptSIDs = this.txtCounterSignDept.Text;
                            //entity.DeptSigners = this.txtDeptSigners.Text;
                            entity.DeptSignerIDs = SysString.FilterRepeat(entity.DeptSignerIDs + (entity.DeptSignerIDs != string.Empty ? ";" : "") + this.txtCounterSignDeptLeaders.Text);
                            entity.NewDeptSIDs = "";
                            entity.NewDeptSignerIDs = "";
                            entity.NewDeptSigners = "";
                        }
                        //string[] array = OAUser.GetDeptManagerArrays(this.txtCounterSignDept.Text, 0);
                        //entity.DeptSignerIDs = array[0].ToString().Replace(',', ';');
                        //entity.DeptSignerIDs = this.txtCounterSignDeptLeaders.Text.Replace(',', ';');

                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_TJHG)
                        {
                            //公司办秘书科秘书
                            string[] array = OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_SECRETARY);
                            entity.VerifierID = array[0].ToString();
                            entity.Verifiers = array[1].ToString();
                        }
                        entity.CheckDate = DateTime.Now;
                        //清除CommonList值
                        entity.CommentList.Clear();
                    }
                    else
                    {
                        entity.IsHeGaoBack = true;
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

                    //是否已提交部门会签
                    entity.IsSubmitDeptSign = "true";
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
                            entity.Signer = this.ddlSigner.SelectedValue;
                            entity.SignerName = this.ddlSigner.SelectedItem.Text;
                        }

                        //领导会签
                        //if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_LDHQ)//将新的会签人添加到已经会签过的人。多次会签
                        //{
                        entity.NewLeadSignerIDs = this.txtComCounterSignLeaders.Text;
                        entity.NewLeadSigners = this.txtLeadSigners.Text;
                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_LDHQ)//将新的会签人添加到已经会签过的人。多次会签
                        {
                            entity.LeadSignerIDs = SysString.FilterRepeat(entity.LeadSignerIDs + (entity.LeadSignerIDs != string.Empty ? ";" : "") + this.txtComCounterSignLeaders.Text);
                            entity.LeadSigners = SysString.FilterRepeat(entity.LeadSigners + (entity.LeadSigners != string.Empty ? ";" : "") + this.txtLeadSigners.Text);
                            entity.NewLeadSignerIDs = "";
                            entity.NewLeadSigners = "";
                        }
                        //}

                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_ZRHG)
                        {
                            //处长副处长
                            //string strDeptID = OAConfig.GetConfig("公司发文主任核稿", "部门ID");
                            //string[] array = OAUser.GetUserByDeptPostArray(strDeptID, "公司办主任", 0);
                            string[] array = OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_CHIEF);//2010-4-29 改成角色取人
                            entity.ZhuRenSignerID = array[0].ToString();
                            entity.ZhuRenSigners = array[1].ToString();
                        }

                        //秘书核稿
                        entity.MainSenders = this.txtMainSender.Text;
                        entity.CopySenders = this.txtCopySender.Text;
                        entity.Verifier = this.txtSecretaryChecker.Text;
                        entity.VerifierMainID = this.txtVerifierID.Text;
                        entity.VerifyDate = DateTime.Now;

                        //清除CommonList值
                        entity.CommentList.Clear();
                    }
                    break;
                #endregion

                #region 主任核稿
                //主任核稿
                case ProcessConstString.StepName.SendStepName.STEP_ZRVERIFY:
                    entity.MainSenders = this.txtMainSender.Text;
                    entity.CopySenders = this.txtCopySender.Text;
                    entity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;
                    entity.DocumentTitle = this.txtDocumentTitle.Text;
                    entity.SubjectWord = this.txtSubjectWord.Text;

                    //主任核稿
                    entity.ZhuRenSigner = this.txtDirectorChecker.Text;
                    entity.ZhuRenSignerMainID = this.txtDirectorCheckerID.Text;
                    entity.ZhuRenSignDate = DateTime.Now;
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

                    //是否已领导会签
                    entity.IsSubmitLeadSign = true;
                    break;
                #endregion

                #region 签发
                case ProcessConstString.StepName.SendStepName.STEP_SIGN:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //公司办文书
                        string[] array = OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_OFFICE_CLERK);
                        entity.FenFaRenIDs = array[0].ToString();
                        entity.FenFaRenNames = array[1].ToString();
                        entity.SignDate = DateTime.Now;
                        entity.SendDate = DateTime.Now;

                        if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_QF)
                        {
                            entity.DeptHaveSigners = this.txtDeptSigners.Text;
                            entity.LeadHaveSigners = this.txtLeadSigners.Text;
                        }

                        entity.SubjectWord = this.txtSubjectWord.Text.Trim();
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

                    entity.ShareCount = this.txtShareCount.Text == string.Empty ? 0 : SysConvert.ToInt32(this.txtShareCount.Text);
                    entity.SheetCount = this.txtSheetCount.Text == string.Empty ? 0 : SysConvert.ToInt32(this.txtSheetCount.Text);

                    entity.Typist = this.txtTypist.Text;
                    entity.ReChecker = this.txtReChecker.Text;

                    entity.FenFaMainIDs = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;

                    //传阅
                    entity.CirculateIDs = this.hDeptID.Value;
                    entity.CirculateNames = this.txtDeptName.Text;
                    entity.CirculateLeadIDs = this.hUserID.Value;
                    entity.CirculateLeadNames = this.txtUserName.Text;

                    //追加分发
                    entity.CirculateAddIDs = this.hDeptID1.Value;
                    entity.CirculateAddNames = this.txtDeptName1.Text;
                    entity.CirculateAddLeadIDs = this.hUserID1.Value;
                    entity.CirculateAddLeadNames = this.txtUserName1.Text;

                    entity.CommentList.Clear();
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

                    entity.IsSubmitCheck = true;
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
            //追加分发
            this.btnAddFenFa.Click += SubmitHandler;
            //完成
            this.btnComplete.Click += SubmitHandler;
            this.btnCompleteSign.Click += SubmitHandler;
            //主任核稿
            this.btnZhuRenSign.Click += SubmitHandler;
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
                    EntitySend entity = ControlToEntity(true) as EntitySend;
                    entity.SubmitAction = strActionName;
                    base.FormSubmit(true, strActionName, null, entity);
                }
                else
                {
                    if (this.ServerCheck())
                    {
                        EntitySend entity = ControlToEntity(false) as EntitySend;
                        entity.SubmitAction = strActionName;

                        if (strActionName == ProcessConstString.SubmitAction.CompanySendAction.ACTION_TJJD)
                        {
                            if (!string.IsNullOrEmpty(entity.CirculateIDs) || !string.IsNullOrEmpty(entity.CirculateLeadIDs))
                            {
                                base.Circulate(entity.CirculateIDs, "1", string.Empty, entity.CirculateLeadIDs, "1", false, string.Empty, false);
                            }

                            B_DocumentNo_SN sn = new B_DocumentNo_SN();
                            if (!sn.UpdateNo(base.TemplateName, base.ProcessID, entity.DocumentYear, entity.DocumentNum, entity.DocumentNo))
                            {
                                return;
                            }
                        }

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
                                if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_WCGD)
                                {
                                    if (!string.IsNullOrEmpty(entity.CirculateIDs) || !string.IsNullOrEmpty(entity.CirculateLeadIDs))
                                    {
                                        base.Circulate(entity.CirculateIDs, "1", string.Empty, entity.CirculateLeadIDs, "1", false, string.Empty, false);
                                    }

                                    //公司发文归档
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

                                if (base.SubAction == ProcessConstString.SubmitAction.CompanySendAction.ACTION_ZJFF &&
                                    (!string.IsNullOrEmpty(entity.CirculateAddIDs) || !string.IsNullOrEmpty(entity.CirculateAddLeadIDs)))
                                {
                                    base.Circulate(entity.CirculateAddIDs, "1", string.Empty, entity.CirculateAddLeadIDs, "2", false, string.Empty, false);
                                }

                                //调用工作流                  
                                Hashtable nValues = entity.GetProcNameValue(base.StepName, strActionName);
                                base.FormSubmit(false, strActionName, nValues, entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JScript.ShowMsgBox(this.Page, ex.Message, false);
            }
        }

        //获取审核人
        protected void ddlHostDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlHostDept.Items.Count > 0)
            {
                OAUser.GetDeptLeader(this.ddlVerifier, this.ddlHostDept.SelectedValue, 0);
            }
        }
        #endregion

        #region 验证份数张数
        /// <summary>
        /// 验证份数张数
        /// </summary>
        /// <returns></returns>
        private bool ServerCheck()
        {
            string strErrMessage = string.Empty;
            if (!string.IsNullOrEmpty(txtShareCount.Text.Trim()))
            {
                if (!Regex.IsMatch(txtShareCount.Text.Trim(), @"^\d+$", RegexOptions.IgnoreCase))
                { strErrMessage += "份数应为非负整数！\\n"; }
            }
            if (!string.IsNullOrEmpty(txtSheetCount.Text.Trim()))
            {
                if (!Regex.IsMatch(txtSheetCount.Text.Trim(), @"^\d+$", RegexOptions.IgnoreCase))
                { strErrMessage += "张数应为非负整数！\\n"; }
            }

            if (!string.IsNullOrEmpty(strErrMessage))
            {
                JScript.ShowMsgBox(this.Page, strErrMessage, false);
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        protected void btn_GuiDang_Click(object sender, EventArgs e)
        {
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