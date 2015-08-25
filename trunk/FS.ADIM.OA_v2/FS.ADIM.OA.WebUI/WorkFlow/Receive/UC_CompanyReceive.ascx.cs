//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：公司收文界面
//
// 
// 创建标识：任金权 2009-12-29
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
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

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    /// <summary>
    /// 公司收文界面
    /// </summary>
    public partial class UC_CompanyReceive : FormsUIBase
    {
        string m_strHuanHang = "\n";
        private const string strNewLine = "<br/>";

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrint();
        }
        #endregion

        #region 控件初始设置
        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            B_GS_WorkItems rentity = base.EntityData != null ? base.EntityData as B_GS_WorkItems : new B_GS_WorkItems();
            //补偿老版本数据
            if (rentity.DraftDate < base.OAStartTime)
            {
                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR)//处室承办、兼容老版本
                {
                    if (string.IsNullOrEmpty(rentity.UnderTakeChief))
                    {
                        ViewBase vb = OADept.GetDeptByDeptUser(rentity.UnderTakeDept, FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF, "", ""), 2);
                        rentity.UnderTakeChief = vb != null && vb.Count > 0 ? (vb.GetItem(0) as FounderSoftware.ADIM.OU.BLL.Busi.Department).ID.ToString() : "";
                        rentity.UnderTakeChiefName = vb != null && vb.Count > 0 ? (vb.GetItem(0) as FounderSoftware.ADIM.OU.BLL.Busi.Department).Name : "";
                    }
                    if (string.IsNullOrEmpty(rentity.UnderTakePeople))
                    {
                        //ViewBase vb = OAUser.GetUserName();
                        string userid = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER, "", "");
                        rentity.UnderTakePeople = userid;
                        rentity.UnderTakePeopleName = OAUser.GetUserName(userid);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(rentity.UnderTakePeopleName))
                        {
                            rentity.UnderTakePeopleName = OAUser.GetUserName(rentity.UnderTakePeople);
                        }
                    }
                    if (!this.dllUnderDept.Items.Contains(new ListItem(rentity.UnderTakeChiefName, rentity.UnderTakeChief)))
                    {
                        this.dllUnderDept.Items.Add(new ListItem(rentity.UnderTakeChiefName, rentity.UnderTakeChief));
                    }
                    if (!this.ddlUnderPeople.Items.Contains(new ListItem(rentity.UnderTakePeopleName, rentity.UnderTakePeople)))
                    {
                        this.ddlUnderPeople.Items.Add(new ListItem(rentity.UnderTakePeopleName, rentity.UnderTakePeople));
                    }
                }
                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF)
                {
                    ViewBase vb = OADept.GetDeptByDeptUser(rentity.UnderTakeDept, FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF, base.WorkItemID, ""), 2);
                    rentity.UnderTakeChief = vb != null && vb.Count > 0 ? (vb.GetItem(0) as FounderSoftware.ADIM.OU.BLL.Busi.Department).ID.ToString() : "";
                    if (string.IsNullOrEmpty(rentity.UnderTakePeople))
                    {
                        //ViewBase vb = OAUser.GetUserName();
                        string userid = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER, "", "");
                        rentity.UnderTakePeople = userid;
                        rentity.UnderTakePeopleName = OAUser.GetUserName(userid);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(rentity.UnderTakePeopleName))
                        {
                            rentity.UnderTakePeopleName = OAUser.GetUserName(rentity.UnderTakePeople);
                        }
                    }
                    if (!this.ddlUnderPeople.Items.Contains(new ListItem(rentity.UnderTakePeopleName, rentity.UnderTakePeople)))
                    {
                        this.ddlUnderPeople.Items.Add(new ListItem(rentity.UnderTakePeopleName, rentity.UnderTakePeople));
                    }
                }
                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER)
                {
                    string userid = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER, base.WorkItemID, "");
                    rentity.UnderTakePeople = userid;
                    rentity.UnderTakePeopleName = OAUser.GetUserName(userid);
                }
            }
            OAControl controls = new OAControl();
            //附件
            this.ucAttachment.UCTemplateName = base.TemplateName;
            this.ucAttachment.UCProcessID = base.ProcessID;
            this.ucAttachment.UCWorkItemID = base.WorkItemID;
            this.ucAttachment.UCTBID = base.IdentityID.ToString();
            this.ucAttachment.UCIsEditable = true;///////////////////////////////////////////// false;

            //传阅控件初始化
            this.ucCirculatePeople.UCDeptIDControl = this.txtCdeptID.ClientID;
            this.ucCirculatePeople.UCDeptNameControl = this.txtCirculateDeptName.ClientID;
            this.ucCirculatePeople.UCDeptUserIDControl = this.txtCPeopleID.ClientID;
            this.ucCirculatePeople.UCDeptUserNameControl = this.txtCirculatePeopleName.ClientID;
            this.ucCirculatePeople.UCSelectType = "2";
            this.ucCirculatePeople.UCTemplateName = base.TemplateName;
            this.ucCirculatePeople.UCFormName = "分发范围";

            //收文号
            this.txtReceiveNo.Attributes.Add("readonly", "true");

            //收文日期
            this.txtReceiveDate.Attributes.Add("readonly", "true");

            //原文号
            this.txtSendLetterNo.Attributes.Add("readonly", "true");

            //卷号
            this.txtPreVolumeNo.Attributes.Add("readonly", "true");

            //文件名称
            this.txtDocumentTitle.Attributes.Add("readonly", "true");

            switch (base.StepName)
            {
                #region 发起流程
                case ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL:
                    controls.EnableControls = new Control[] { this.ddlOfficer, this.txtPromptEdit };
                    controls.DisEnableControls = new Control[] { this.txtOfficerComment, this.ddlLeadShip, this.txtLeadCommentView, this.txtUnderTakeComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtPrompt };
                    //绑定公司办主任
                    if (!this.IsPostBack)
                    {
                        OAUser.GetUserByRole(this.ddlOfficer, OUConstString.RoleName.COMPANY_CHIEF);
                        if (!string.IsNullOrEmpty(rentity.Officer) && string.IsNullOrEmpty(rentity.OfficerName))//兼容老版本
                        {
                            //绑定公司办领导
                            rentity.OfficerName = OAUser.GetUserName(rentity.Officer);
                        }
                        ListItem lt = new ListItem(rentity.OfficerName, rentity.Officer);
                        if (!(this.ddlOfficer.Items.Contains(lt)))
                        {
                            this.ddlOfficer.Items.Add(lt);
                        }
                        this.ddlOfficer.SelectedValue = rentity.Officer;
                    }

                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_INSPECT;
                    this.btnCommon1.Visible = true;

                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;
                    this.btnCommon2.Visible = true;

                    if (!string.IsNullOrEmpty(base.WorkItemID) && base.EntryAction != "7")//如果是退回，发起节点可以启动撤销
                    {
                        this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_CANCEL;
                        this.btnCommon3.Visible = true;
                        this.btnCommon3.OnClientClick = "";
                        this.btnCommon3.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");
                    }
                    break;
                #endregion

                #region 办公室批阅
                case ProcessConstString.StepName.ReceiveStepName.STEP_OFFICE://办公室批阅
                    controls.EnableControls = new Control[] { this.txtOfficerComment, this.ddlLeadShip, this.txtPromptEdit };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtLeaderComment, this.txtLeadCommentView, this.txtUnderTakeUserName, this.txtUnderTakeDeptName, this.txtUnderTakeComment, this.txtPrompt };

                    //绑定公司领导
                    if (!this.IsPostBack)
                    {
                        OAUser.GetUserByRole(this.ddlLeadShip, OUConstString.RoleName.COMPANY_LEADER);
                        if (!string.IsNullOrEmpty(rentity.LeaderShip) && string.IsNullOrEmpty(rentity.LeaderShipName))//兼容老版本
                        {
                            //绑定公司领导
                            //this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
                            rentity.LeaderShipName = OAUser.GetUserName(rentity.LeaderShip);
                        }
                        ListItem lt = new ListItem(rentity.LeaderShipName, rentity.LeaderShip);
                        if (!(this.ddlLeadShip.Items.Contains(lt)))
                        {
                            this.ddlLeadShip.Items.Add(lt);
                        }
                        this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
                    }

                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_POSTIL;
                    this.btnCommon1.Visible = true;

                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ACTION_SUBMIT;
                    this.btnCommon2.Visible = true;

                    this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_DENY;
                    this.btnCommon3.Visible = true;
                    this.btnCommon3.OnClientClick = "";
                    this.btnCommon3.Attributes.Add("onclick", "javascript: if(!confirm('确定退回到发起流程节点吗？')){return false;}else{DisableButtons();}");

                    this.btnCommon4.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;
                    this.btnCommon4.Visible = true;
                    break;
                #endregion

                #region 收文处理中心
                case ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER:
                    controls.EnableControls = new Control[] { this.ddlLeadShip, this.txtPromptEdit };//待加传阅分发
                    controls.YellowControls = new Control[] { this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtCirculateDeptName, this.txtCirculatePeopleName };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtOfficerComment, this.txtLeadCommentView, this.txtLeaderComment, this.txtUnderTakeComment, this.txtPrompt };

                    //承办
                    this.OASelectUC1.UCDeptIDControl = this.txtUnderTakeDeptID.ClientID;
                    this.OASelectUC1.UCDeptNameControl = this.txtUnderTakeDeptName.ClientID;
                    this.OASelectUC1.UCLevel = "1";
                    this.OASelectUC1.UCSelectType = "0";
                    this.OASelectUC1.UCDeptShowType = "1010";
                    this.OASelectUC1.UCIsSingle = "1";
                    this.OASelectUC1.Visible = true;

                    this.OASelectUC2.Visible = true;
                    this.OASelectUC2.UCIsSingle = "1";
                    this.OASelectUC2.UCSelectType = "1";
                    this.OASelectUC2.UCDeptUserIDControl = this.txtUnderTakeUserID.ClientID;
                    this.OASelectUC2.UCDeptUserNameControl = this.txtUnderTakeUserName.ClientID;
                    this.OASelectUC2.UCTemplateName = base.TemplateName;
                    this.OASelectUC2.UCFormName = "承办人员";

                    this.ucCirculatePeople.Visible = true;
                    //绑定公司领导
                    if (!this.IsPostBack)
                    {
                        OAUser.GetUserByRole(this.ddlLeadShip, OUConstString.RoleName.COMPANY_LEADER);
                        if (!string.IsNullOrEmpty(rentity.LeaderShip) && string.IsNullOrEmpty(rentity.LeaderShipName))//兼容老版本
                        {
                            //绑定公司领导
                            rentity.LeaderShipName = OAUser.GetUserName(rentity.LeaderShip);
                        }
                        ListItem lt = new ListItem(rentity.LeaderShipName, rentity.LeaderShip);
                        if (!(this.ddlLeadShip.Items.Contains(lt)))
                        {
                            this.ddlLeadShip.Items.Add(lt);
                        }
                        this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
                    }

                    this.divCirculates.Visible = true;//显示传阅层

                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_UNDERTAKE;
                    this.btnCommon1.Visible = true;

                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ReceiveBase.SUBMIT_POSTIL;
                    this.btnCommon2.Visible = true;

                    this.btnDistribute.Text = ProcessConstString.SubmitAction.ReceiveBase.DISTRIBUTION;
                    this.btnDistribute.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");
                    this.btnDistribute.OnClientClick = "";
                    this.btnDistribute.Visible = true;

                    this.btnCommon4.Text = ProcessConstString.SubmitAction.ACTION_SUBMIT;
                    this.btnCommon4.Visible = true;

                    this.btnCommon5.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;
                    this.btnCommon5.Visible = true;
                    break;
                #endregion

                #region 领导批示
                case ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION://
                    controls.EnableControls = new Control[] { this.txtLeaderComment };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.ddlLeadShip, this.txtOfficerComment, this.txtUnderTakeComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtPrompt, this.txtPromptEdit, this.txtLeadCommentView };
                    this.tblPlot.Visible = true;//显示领导批示层
                    break;
                #endregion

                #region 处室承办
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR://
                    controls.EnableControls = new Control[] { this.txtCirculateDeptName, this.txtCirculatePeopleName, this.txtPromptEdit, this.txtUnderComment, this.ddlUnderPeople, this.dllUnderDept };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtOfficerComment, this.ddlLeadShip, this.txtLeaderComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtUnderTakeComment, this.txtPrompt, this.txtLeadCommentView };

                    if (!string.IsNullOrEmpty(rentity.UnderTakeDept) && !IsPostBack)
                    {
                        //根据部门(如果是科室获取处室ID)ID绑定科室
                        OADept.GetChildDept(this.dllUnderDept, rentity.UnderTakeDept, 2);
                        ListItem lt = new ListItem(rentity.UnderTakeChiefName, rentity.UnderTakeChief);
                        if (!(this.dllUnderDept.Items.Contains(lt)))
                        {
                            this.dllUnderDept.Items.Add(lt);
                        }
                        this.dllUnderDept.SelectedValue = rentity.UnderTakeChief;
                        //绑定处室人员（如果是科室获取处室ID）
                        OAUser.GetUserByDeptID(this.ddlUnderPeople, rentity.UnderTakeDept, -1);
                        lt = new ListItem(rentity.UnderTakePeopleName, rentity.UnderTakePeople);
                        if (!(this.ddlUnderPeople.Items.Contains(lt)))
                        {
                            this.ddlUnderPeople.Items.Add(lt);
                        }
                        this.ddlUnderPeople.SelectedValue = rentity.UnderTakePeople;
                    }

                    //如果是承办是人员禁止交办
                    this.dllUnderDept.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);
                    this.ddlUnderPeople.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);
                    this.lbldept.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);
                    this.lblmember.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);

                    this.divUndertake.Visible = true;//显示承办层

                    this.td1.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ReceiveBase.ASSIGN_SECTION;
                    this.btnCommon1.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);//如果是承办是人员禁止交办
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ReceiveBase.ASSIGN_PEOPLE;
                    this.btnCommon2.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDept);//如果是承办是人员禁止交办

                    this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_COMPLETE;
                    this.btnCommon3.Visible = true;
                    break;
                #endregion

                #region 科室承办
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF://
                    controls.EnableControls = new Control[] { this.txtPromptEdit, this.ddlUnderPeople, this.txtUnderComment };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtOfficerComment, this.ddlLeadShip, this.txtLeaderComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtUnderTakeComment, this.txtPrompt, this.txtLeadCommentView };

                    if (!string.IsNullOrEmpty(rentity.UnderTakeChief) && !IsPostBack)
                    {
                        //根据科室绑定科室人员（所有）
                        OAUser.GetUserByDeptID(this.ddlUnderPeople, rentity.UnderTakeChief, -1);
                        ListItem lt = new ListItem(rentity.UnderTakePeopleName, rentity.UnderTakePeople);
                        if (!(this.ddlUnderPeople.Items.Contains(lt)))
                        {
                            this.ddlUnderPeople.Items.Add(lt);
                        }
                        this.ddlUnderPeople.SelectedValue = rentity.UnderTakePeople;
                    }

                    this.dllUnderDept.Visible = false;
                    this.lbldept.Visible = false;
                    this.divUndertake.Visible = true;//显示承办层

                    this.ddlUnderPeople.Visible = true;//如果是承办是人员禁止交办

                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ReceiveBase.ASSIGN_PEOPLE;
                    this.btnCommon1.Visible = this.ddlUnderPeople.Items.Count > 0;//如果是承办是人员禁止交办

                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ACTION_COMPLETE;
                    this.btnCommon2.Visible = true;
                    break;
                #endregion

                #region 人员承办
                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER://
                    controls.EnableControls = new Control[] { this.txtPromptEdit, this.txtUnderComment };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtOfficerComment, this.ddlLeadShip, this.txtLeaderComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtUnderTakeComment, this.txtPrompt, this.txtLeadCommentView };

                    this.ddlUnderPeople.Visible = false;
                    this.dllUnderDept.Visible = false;
                    this.divUndertake.Visible = true;
                    this.lbldept.Visible = false;
                    this.lblmember.Visible = false;
                    this.td1.Visible = false;
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ACTION_COMPLETE;
                    this.btnCommon1.Visible = true;
                    break;
                #endregion

                #region 收文处理中心归档
                case ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION://
                    //补偿老版oa处室承办人数据
                    if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UnderTakeDeptLeaderID))
                    {
                        rentity.UnderTakeDeptLeaderID = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                    }
                    controls.EnableControls = new Control[] { this.txtPromptEdit };
                    controls.YellowControls = new Control[] { this.txtCirculateDeptName, this.txtCirculatePeopleName };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtOfficerComment, this.ddlLeadShip, this.txtLeaderComment, this.txtUnderTakeComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtPrompt, this.txtLeadCommentView };
                    this.ucAttachment.UCIsEditable = true;
                    this.ucCirculatePeople.Visible = true;
                    this.divCirculates.Visible = true;//显示传阅层

                    this.btnGuiDang.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btnGuiDang.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                    }

                    this.btnDistribute.Text = ProcessConstString.SubmitAction.ReceiveBase.DISTRIBUTION;
                    this.btnDistribute.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");
                    this.btnDistribute.OnClientClick = "";
                    this.btnDistribute.Visible = true;

                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ACTION_COMPLETE;
                    this.btnCommon2.Visible = true;

                    this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_DENY;
                    this.btnCommon3.Attributes.Add("onclick", "javascript: if(!confirm('确定退回到处室承办节点吗？')){return false;}else{DisableButtons();}");
                    this.btnCommon3.OnClientClick = "";
                    this.btnCommon3.Visible = !string.IsNullOrEmpty(rentity.UnderTakeDeptLeaderID);

                    this.btnCommon4.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;
                    this.btnCommon4.Visible = true;
                    break;
                #endregion
            }
            #region 历史表单
            if (base.IsPreview)
            {
                this.ucCommentList.UCDateTime = B_FormsData.GetPWSubDate(base.ProcessID, base.WorkItemID, base.TemplateName);
                controls.EnableControls = new Control[] { };
                controls.YellowControls = new Control[] { };
                controls.DisEnableControls = new Control[] { this.txtCirculateDeptName, this.txtCirculatePeopleName, this.ddlOfficer, this.txtOfficerComment, this.ddlLeadShip, this.txtLeaderComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtUnderTakeComment, this.txtPrompt, this.txtLeadCommentView, this.txtPromptEdit, this.txtUnderComment, this.ddlUnderPeople, this.dllUnderDept };
                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION && rentity.CompanyWS == CurrentUserInfo.UserName)//rentity.ReceiveUserID)
                {
                    controls.YellowControls = new Control[] { this.txtCirculateDeptName, this.txtCirculatePeopleName };
                    controls.DisEnableControls = new Control[] { this.ddlOfficer, this.txtOfficerComment, this.ddlLeadShip, this.txtLeaderComment, this.txtUnderTakeDeptName, this.txtUnderTakeUserName, this.txtUnderTakeComment, this.txtPrompt, this.txtLeadCommentView, this.txtPromptEdit, this.txtUnderComment, this.ddlUnderPeople, this.dllUnderDept };
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ReceiveBase.APPENDED_DISTRIBUTION;
                    this.divCirculates.Visible = true;//显示传阅层
                    this.btnCommon1.Visible = true;
                    this.ucCirculatePeople.Visible = true;
                    this.btnGuiDang.Visible = false;
                }
                else
                {
                    this.btnCommon1.Visible = false;
                    this.ucCirculatePeople.Visible = false;
                    this.btnDistribute.Visible = false;
                    //this.btnCommon1.Visible = false;
                    //this.ucCirculatePeople.Visible = false;
                }
                this.OASelectUC1.Visible = false;
                this.OASelectUC2.Visible = false;
                this.btnCommon2.Visible = false;
                this.btnCommon3.Visible = false;
                this.btnCommon4.Visible = false;
                this.btnCommon5.Visible = false;
                this.btnComplete.Visible = false;
                this.btnGuiDang.Visible = false;
                this.btnInstructionSave.Visible = false;
                if ( base.IsCanDevolve && rentity.D_StepStatus != ProcessConstString.StepStatus.STATUS_REMOVED&&rentity.ReceiveUserID == CurrentUserInfo.UserName)//(base.IsCanDevolve)
                {
                    this.btnGuiDang.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btnGuiDang.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                    }
                }
                this.ucAttachment.UCIsEditable = true;

            }
            #endregion
            controls.SetControls();
        }
        #endregion

        #region 实体与控件之间的绑定

        /// <summary>
        /// 实体填充控件
        /// </summary>
        protected override void EntityToControl()
        {
            B_GS_WorkItems rentity = base.EntityData != null ? base.EntityData as B_GS_WorkItems : new B_GS_WorkItems();
            // ViewState[ConstString.ViewState.ENTITY_DATA] = rentity;
            //收文编辑号
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
                this.ucAttachment.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(l_BusReceiveEdit.FileData);
                this.txtReceiveNo.Text = l_BusReceiveEdit.ReceiveNo;
                this.txtReceiveDate.Text = l_BusReceiveEdit.ReceiveDate.ToString(ConstString.DateFormat.Normal);
                this.txtSendLetterNo.Text = l_BusReceiveEdit.SendLetterNo;
                this.txtDocumentTitle.Text = l_BusReceiveEdit.DocumentTitle;
                this.txtCommunicationUnit.Text = l_BusReceiveEdit.ReceiveUnit;
                this.txtPreVolumeNo.Text = l_BusReceiveEdit.PreVolumeNo;
                this.txtJinJi.Text = l_BusReceiveEdit.UrgentDegree;
                base.StepName = ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL;
            }
            else
            {
                if (ViewState["filelist"] != null)
                {
                    this.ucAttachment.UCDataList = ViewState["filelist"] as List<CFuJian>;
                }
                else
                {
                    if (rentity != null)
                    {
                        this.ucAttachment.UCDataList = rentity.FileList; //实体绑定到控件时赋值
                    }
                }
                //收文号
                this.txtReceiveNo.Text = rentity.DocumentNo;

                //收文日期
                this.txtReceiveDate.Text = rentity.DocumentReceiveDate.ToString(ConstString.DateFormat.Normal);

                //原文号
                this.txtSendLetterNo.Text = rentity.SendNo;

                //来文单位
                this.txtCommunicationUnit.Text = rentity.CommunicationUnit;

                //卷号
                this.txtPreVolumeNo.Text = rentity.VolumeNo;

                //文件名称
                this.txtDocumentTitle.Text = rentity.DocumentTitle;
            }


            if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL && !base.IsPreview)//公司办主任禁用状态不从数据库抓值，添加已有的值。
            {
                this.ddlOfficer.SelectedValue = rentity.Officer;
            }
            else
            {
                if (!string.IsNullOrEmpty(rentity.Officer) && string.IsNullOrEmpty(rentity.OfficerName))//兼容老版本
                {
                    //绑定公司办领导
                    //this.ddlOfficer.SelectedValue = rentity.Officer;
                    rentity.OfficerName = OAUser.GetUserName(rentity.Officer);
                }
                if (!string.IsNullOrEmpty(rentity.Officer) && !string.IsNullOrEmpty(rentity.OfficerName))
                {
                    this.ddlOfficer.Items.Add(new ListItem(rentity.OfficerName, rentity.Officer));
                }
            }

            if ((base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_OFFICE || base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER) && !base.IsPreview)//公司领导禁用状态不从数据库抓值，添加已有的值。
            {
                this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
            }
            else
            {
                if (!string.IsNullOrEmpty(rentity.LeaderShip) && string.IsNullOrEmpty(rentity.LeaderShipName))//兼容老版本
                {
                    rentity.LeaderShipName = OAUser.GetUserName(rentity.LeaderShip);
                }
                if (!string.IsNullOrEmpty(rentity.LeaderShip) && !string.IsNullOrEmpty(rentity.LeaderShipName))
                {
                    this.ddlLeadShip.Items.Add(new ListItem(rentity.LeaderShipName, rentity.LeaderShip));
                }
            }

            this.dllUnderDept.SelectedValue = rentity.UnderTakeChief;
            this.ddlUnderPeople.SelectedValue = rentity.UnderTakePeople;

            if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION || base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER)
            {
                //传阅
                this.txtCirculatePeopleName.Text = rentity.CPeopleName;
                this.txtCPeopleID.Text = rentity.CPeopleID;
                this.txtCirculateDeptName.Text = rentity.CDeptName;
                this.txtCdeptID.Text = rentity.CDeptID;
            }
            if (!string.IsNullOrEmpty(rentity.UnderTakeDept) && string.IsNullOrEmpty(rentity.UnderTakeDeptName))
            {
                rentity.UnderTakeDeptName = OADept.GetDeptName(rentity.UnderTakeDept);
            }
            this.txtUnderTakeDeptName.Text = rentity.UnderTakeDeptName;
            if (!string.IsNullOrEmpty(rentity.UnderTakePeople) && string.IsNullOrEmpty(rentity.UnderTakePeopleName))
            {
                rentity.UnderTakePeopleName = OAUser.GetUserName(rentity.UnderTakePeople);
            }
            if (string.IsNullOrEmpty(rentity.UnderTakeDept))
            {
                this.txtUnderTakeUserName.Text = rentity.UnderTakePeopleName;
            }

            //批阅意见
            this.txtOfficerComment.Text = rentity.Officer_Comment;

            //批示意见
            this.txtLeaderComment.Text = rentity.LS_CommentAdd;
            this.txtLeadCommentView.Text = rentity.LS_Comment;

            //承办意见
            this.txtUnderTakeComment.Text = rentity.UnderTake_Comment;
            this.txtUnderComment.Text = rentity.UnderTake_CommentAdd;

            //提示信息
            this.txtPrompt.Text = rentity.Prompt;
            if (rentity.IsFormSave)
            {
                this.txtPromptEdit.Text = rentity.PromptEdit;
            }
            this.txtUnderTakeComment.Text = rentity.UnderTake_Comment;

            //主任后显示label形式的办公室主任与时间
            if (rentity.Officer_Date != "" && rentity.Officer_Date != null)
            {
                //this.ddlOfficer.Visible = false;
                //this.lbOfficer.Visible = true;
                //this.lbOfficer.Text = rentity.OfficerName + strNewLine + rentity.Officer_Date;
            }

            //领导批示后显示label形式的办领导与时间
            if (rentity.LS_Date != "" && rentity.LS_Date != null)
            {
            //    this.ddlLeadShip.Visible = false;
                this.lbLeadShip.Visible = true;
                this.lbLeadShip.Text = rentity.LS_Date;//rentity.LeaderShipName + strNewLine + 
            }
            //else
                this.ddlLeadShip.Visible = true;

            ////承办后显示label形式的承办人员与时间
            //if (rentity.Officer_Date != "" || rentity.Officer_Date != null)
            //{
            //    this.ddlOfficer.Visible = false;
            //    this.lbOfficer.Visible = true;
            //    this.lbOfficer.Text = rentity.OfficerName + strNewLine + rentity.Officer_Date;
            //}



        }

        /// <summary>
        /// 控件填充实体
        /// </summary>
        /// <param name="IsSave">是否保存</param>
        /// <returns>EntityBase</returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_GS_WorkItems rentity = base.EntityData != null ? base.EntityData as B_GS_WorkItems : new B_GS_WorkItems();
            //附件信息
            ViewState["filelist"] = this.ucAttachment.UCDataList;
            rentity.FileList = this.ucAttachment.UCDataList;
            rentity.CommentList.Clear();

            if (!IsSave)
            {
                rentity.PromptEdit = this.txtPromptEdit.Text;
                if (!string.IsNullOrEmpty(this.txtPromptEdit.Text))
                {
                    rentity.IsFormSave = false;
                    rentity.Prompt = this.txtPrompt.Text + "[" + (rentity.ReceiveUserName == string.Empty ? CurrentUserInfo.DisplayName : rentity.ReceiveUserName )+ "][" + System.DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:" + this.txtPromptEdit.Text + m_strHuanHang;
                }
            }
            else
            {
                rentity.IsFormSave = true;
                rentity.Prompt = this.txtPrompt.Text;
                rentity.PromptEdit = this.txtPromptEdit.Text;
            }

            switch (base.StepName)
            {

                case ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL://发起流程
                    //收文号
                    rentity.DocumentNo = this.txtReceiveNo.Text;

                    if (!string.IsNullOrEmpty(this.txtJinJi.Text))
                    {
                        rentity.UrgentDegree = this.txtJinJi.Text;
                    }
                    //发起人及日期
                    if (rentity.Drafter == string.Empty)
                    {
                        rentity.Drafter = CurrentUserInfo.DisplayName;
                        rentity.DrafterID = CurrentUserInfo.UserName;
                        rentity.ReceiveUserID = rentity.DrafterID;
                        rentity.ReceiveUserName = rentity.Drafter;
                    }
                    if (rentity.DraftDate == DateTime.MinValue)
                    {
                        rentity.DraftDate = System.DateTime.Now;
                    }

                    if (this.SubAction == ProcessConstString.SubmitAction.ACTION_SUBMIT)//公司办文书
                    {
                        rentity.CompanyWS = (OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_OFFICE_CLERK))[0].ToString();
                    }

                    //收文日期
                    rentity.DocumentReceiveDate = string.IsNullOrEmpty(this.txtReceiveDate.Text) ? DateTime.MinValue : DateTime.Parse(this.txtReceiveDate.Text);

                    //原文号
                    rentity.SendNo = this.txtSendLetterNo.Text;

                    //来文单位
                    rentity.CommunicationUnit = this.txtCommunicationUnit.Text;

                    //卷号
                    rentity.VolumeNo = this.txtPreVolumeNo.Text;

                    //文件名称
                    rentity.DocumentTitle = this.txtDocumentTitle.Text;

                    //公司办主任
                    rentity.OfficerName = this.ddlOfficer.SelectedItem.Text;
                    rentity.Officer = this.ddlOfficer.SelectedValue;

                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_OFFICE://办公室批阅

                    rentity.LeaderShipName = this.ddlLeadShip.SelectedItem.Text;
                    rentity.LeaderShip = this.ddlLeadShip.SelectedValue;
                    rentity.Officer_Date = DateTime.Now.ToString();

                    rentity.Officer_Comment = this.txtOfficerComment.Text;
                    if (this.SubAction == ProcessConstString.SubmitAction.ACTION_SUBMIT)//公司办文书
                    {
                        rentity.CompanyWS = (OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_OFFICE_CLERK))[0].ToString();
                    }
                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION://领导批示
                    if (!IsSave)
                    {
                        rentity.LS_Comment = this.txtLeaderComment.Text;
                        rentity.LS_CommentAdd = "";
                        rentity.LS_Date = DateTime.Now.ToString();
                        rentity.LS_Comment = this.txtLeaderComment.Text;
                        CYiJian l_objComment = new CYiJian();
                        l_objComment.Content = this.txtLeaderComment.Text;
                        l_objComment.FinishTime = DateTime.Now.ToString();
                        l_objComment.UserID = rentity.ReceiveUserID;
                        l_objComment.UserName = rentity.ReceiveUserName;
                        l_objComment.ViewName = base.StepName;
                        rentity.CommentList.Add(l_objComment);
                    }
                    else
                    {
                        rentity.LS_CommentAdd = this.txtLeaderComment.Text;
                    }
                    if (string.IsNullOrEmpty(rentity.CompanyWS))
                    {
                        rentity.CompanyWS = (OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_OFFICE_CLERK))[0].ToString();
                    }
                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_PROCESS_CENTER://收文处理中心
                    if (this.SubAction == ProcessConstString.SubmitAction.ACTION_SUBMIT)
                    {
                        rentity.CompanyWS = (OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_OFFICE_CLERK))[0].ToString();
                    }
                    else
                    {
                        rentity.CompanyWS = rentity.ReceiveUserID;
                    }
                    //公司领导
                    rentity.LeaderShipName = this.ddlLeadShip.SelectedItem.Text;
                    rentity.LeaderShip = this.ddlLeadShip.SelectedValue;

                    //传阅
                    rentity.CPeopleName = this.txtCirculatePeopleName.Text;
                    rentity.CPeopleID = this.txtCPeopleID.Text;
                    rentity.CDeptName = this.txtCirculateDeptName.Text;
                    rentity.CDeptID = this.txtCdeptID.Text;

                    rentity.UnderTakePeopleName = this.txtUnderTakeUserName.Text;
                    rentity.UnderTakeDeptName = this.txtUnderTakeDeptName.Text;

                    //承办部门、人员
                    if (!string.IsNullOrEmpty(this.txtUnderTakeDeptID.Text) || !string.IsNullOrEmpty(this.txtUnderTakeUserID.Text))
                    {
                        rentity.UnderTakeDept = this.txtUnderTakeDeptID.Text;
                        rentity.UnderTakeDeptLeaderID = OAUser.GetUserByDeptPostArray(this.txtUnderTakeDeptID.Text,null,true,true)[0];
                        rentity.UnderTakePeople = this.txtUnderTakeUserID.Text;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this.txtUnderTakeDeptName.Text))
                        {
                            rentity.UnderTakeDept = "";
                        }
                        else
                        {
                            rentity.UnderTakeDeptLeaderID =OAUser.GetUserByDeptPostArray(rentity.UnderTakeDept,null,true,true)[0];
                        }
                        if (string.IsNullOrEmpty(this.txtUnderTakeUserName.Text))
                        {
                            rentity.UnderTakePeople = "";
                        }
                    }
                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR://处室承办
                    if (this.dllUnderDept.SelectedIndex >= 0)
                    {
                        rentity.UnderTakeChief = this.dllUnderDept.SelectedValue;
                        rentity.UnderTakeChiefName = this.dllUnderDept.SelectedItem.Text;
                    }
                    if (this.ddlUnderPeople.SelectedIndex >= 0)
                    {
                        rentity.UnderTakePeople = this.ddlUnderPeople.SelectedValue;
                        rentity.UnderTakePeopleName = this.ddlUnderPeople.SelectedItem.Text;
                    }
                    rentity.UnderTakeChiefLeaderID = FormsMethod.GetUserIDByViewBase(OAUser.GetDeptManager(this.dllUnderDept.SelectedValue, 0));
                    rentity.UnderTakeDeptLeaderID = rentity.ReceiveUserID;

                    if (!IsSave)
                    {
                        if (this.SubAction == ProcessConstString.SubmitAction.ACTION_COMPLETE)//公司办文书
                        {
                            rentity.CompanyWS = (OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_OFFICE_CLERK))[0].ToString();
                        }
                        goto case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER;
                    }
                    else
                    {
                        rentity.UnderTake_CommentAdd = this.txtUnderComment.Text;
                    }
                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF://科室承办
                    if (this.ddlUnderPeople.SelectedIndex >= 0)
                    {
                        rentity.UnderTakePeople = this.ddlUnderPeople.SelectedValue;
                        rentity.UnderTakePeopleName = this.ddlUnderPeople.SelectedItem.Text;
                    }
                    rentity.UnderTakeChiefLeaderID = rentity.ReceiveUserID;
                    if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UnderTakeDeptLeaderID))
                    {
                        rentity.UnderTakeDeptLeaderID = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                    }
                    if (!IsSave)
                    {
                        goto case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER;
                    }
                    else
                    {
                        rentity.UnderTake_CommentAdd = this.txtUnderComment.Text;
                    }
                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER://人员承办
                    if (rentity.DraftDate < base.OAStartTime)
                    {
                        rentity.UnderTakeDeptLeaderID = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                        rentity.UnderTakeChiefLeaderID = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                    }
                    if (!IsSave)
                    {
                        rentity.UnderTake_CommentAdd = this.txtUnderComment.Text;
                        if (base.SubAction == ProcessConstString.SubmitAction.ACTION_COMPLETE)
                        {
                            rentity.UnderTake_Comment = this.txtUnderComment.Text;
                        }
                        CYiJian l_objComment = new CYiJian();
                        l_objComment.Content = this.txtUnderComment.Text;
                        l_objComment.FinishTime = DateTime.Now.ToString();
                        l_objComment.UserID = rentity.ReceiveUserID;
                        l_objComment.UserName = rentity.ReceiveUserName;
                        l_objComment.ViewName = base.StepName;
                        rentity.CommentList.Add(l_objComment);
                    }
                    else
                    {
                        rentity.UnderTake_CommentAdd = this.txtUnderComment.Text;
                    }
                    break;

                case ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION:
                    if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UnderTakeDeptLeaderID))
                    {
                        rentity.UnderTakeDeptLeaderID = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                    }

                    if (!string.IsNullOrEmpty(this.txtCPeopleID.Text) || !string.IsNullOrEmpty(this.txtCdeptID.Text))
                    {
                        rentity.CPeopleName = this.txtCirculatePeopleName.Text;
                        rentity.CPeopleID = this.txtCPeopleID.Text;
                        rentity.CDeptName = this.txtCirculateDeptName.Text;
                        rentity.CDeptID = this.txtCdeptID.Text;
                    }
                    rentity.CompanyWS = rentity.ReceiveUserID;
                    break;
            }
            return rentity;
        }
        #endregion

        #region 表单事件

        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitEvents(object sender, EventArgs e)
        {
            try
            {
                string strActionName = ((Button)sender).Text.Trim();

                this.SubAction = strActionName;
                if (strActionName == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_GS_WorkItems bworkitem1 = ControlToEntity(true) as B_GS_WorkItems;
                    bworkitem1.SubmitAction = strActionName;
                    base.FormSubmit(true, strActionName, null, bworkitem1 as EntityBase);
                }
                else
                {
                    B_GS_WorkItems bworkitem = ControlToEntity(false) as B_GS_WorkItems;
                    bworkitem.SubmitDate = System.DateTime.Now;
                    if (strActionName == ProcessConstString.SubmitAction.ReceiveBase.DISTRIBUTION)
                    {
                        //保存实体数据
                        FormSave.SaveEntity(bworkitem, this, true, ProcessConstString.SubmitAction.ReceiveBase.DISTRIBUTION, base.TemplateName);

                        base.Circulate(bworkitem.CDeptID, "1", "", bworkitem.CPeopleID, "1", false, "", false);
                    }
                    else
                    {
                        bworkitem.SubmitAction = strActionName;
                        if (strActionName == ProcessConstString.SubmitAction.ACTION_CANCEL)
                        {
                            base.FormCancel(bworkitem as EntityBase);
                        }
                        else
                        {
                            if (strActionName == ProcessConstString.SubmitAction.ReceiveBase.APPENDED_DISTRIBUTION)
                            {
                                base.Circulate(this.txtCdeptID.Text, "1", "", this.txtCPeopleID.Text, "1", true, "", false);
                            }
                            else
                            {
                                //验证实体
                                string strMessage = B_GS_WorkItems.CheckEntity(bworkitem, base.SubAction, base.StepName);
                                if (!string.IsNullOrEmpty(strMessage))
                                {
                                    JScript.Alert(strMessage);
                                    return;
                                }
                                Hashtable ht = B_GS_WorkItems.GetProcNameValue(base.StepName, strActionName, bworkitem);//ap属性
                                base.FormSubmit(false, strActionName, ht, bworkitem as EntityBase);
                                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL || base.EntryAction == "7")//第一次发起(包括从草稿箱发起)，同步更新T_OA_Receive_Edit（添加ProcessID）
                                {
                                    B_ReceiveEdit.UpdateProcessEdit(base.ProcessID, this.txtReceiveNo.Text);
                                }
                                if (base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_DISTRIBUTION || base.StepName == ProcessConstString.StepName.ReceiveStepName.STEP_APPENDED)
                                {
                                    //更新收文登记表（完成状况,附件信息）
                                    string filedata = XmlUtility.SerializeXml<List<CFuJian>>(this.ucAttachment.UCDataList);
                                    B_ReceiveEdit.UpdateFileData(filedata, this.txtReceiveNo.Text, base.TemplateName, true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowMsgBox(this.Page, MsgType.VbCritical, ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 获取流程参与的人。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetCYPeople(B_GS_WorkItems entity)
        {
            string strPeople = "";
            if (!string.IsNullOrEmpty(entity.DrafterID))
            {
                strPeople += entity.DrafterID + ";";
            }
            if (!string.IsNullOrEmpty(entity.Officer))
            {
                strPeople += entity.Officer + ";";
            }
            if (!string.IsNullOrEmpty(entity.LeaderShip))
            {
                strPeople += entity.LeaderShip + ";";
            }
            if (!string.IsNullOrEmpty(entity.UnderTakeDeptLeaderID))
            {
                strPeople += entity.UnderTakeDeptLeaderID + ";";
            }
            if (!string.IsNullOrEmpty(entity.UnderTakeChiefLeaderID))
            {
                strPeople += entity.UnderTakeChiefLeaderID + ";";
            }
            if (!string.IsNullOrEmpty(entity.UnderTakePeople))
            {
                strPeople += entity.UnderTakePeople + ";";
            }
            if (!string.IsNullOrEmpty(entity.CPeopleID))
            {
                strPeople += this.txtCPeopleID.Text + ";";
            }
            return strPeople;
        }

        protected void btnGuiDang_Click(object sender, EventArgs e)
        {
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