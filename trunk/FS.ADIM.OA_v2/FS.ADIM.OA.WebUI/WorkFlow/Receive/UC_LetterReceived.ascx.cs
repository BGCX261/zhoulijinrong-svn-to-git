using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.Busi;
using FounderSoftware.Framework.Business;
using System.Data;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public partial class UC_LetterReceived : FormsUIBase
    {
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
            OAControl controls = new OAControl();
            B_LetterReceive rentity = base.EntityData != null ? base.EntityData as B_LetterReceive : new B_LetterReceive();
            //收文号
            this.txtReceiveNo.Attributes.Add("readonly", "true");

            //行文时间
            this.txtReceiveTime.Attributes.Add("readonly", "true");

            //紧急程度
            this.txtUrgentDegree.Attributes.Add("readonly", "true");

            //编码
            this.txtFileEncoding.Attributes.Add("readonly", "true");

            //来文单位
            this.txtCommunicationUnit.Attributes.Add("readonly", "true");

            //标题
            this.txtDocumentTitle.Attributes.Add("readonly", "true");

            //备注
            this.txtRemark.Attributes.Add("readonly", "true");

            //附件
            this.ucAttachment.UCTemplateName = base.TemplateName;
            if (base.IdentityID.ToString() != "0")
            {
                this.ucAttachment.UCProcessID = base.ProcessID;
                this.ucAttachment.UCWorkItemID = base.WorkItemID;
                this.ucAttachment.UCTBID = base.IdentityID.ToString();
            }
            //this.ucAttachment.UCIsEditable = false;

            //拟办人控件初始化
            this.ucRole.UCIsSingle = true;
            this.ucRole.UCRoleName = OUConstString.RoleName.HANJIANNIBANZU;
            this.ucRole.UCUserNameControl = this.txtPlotMember.ClientID;
            this.ucRole.UCUserIDControl = this.txtPlotMemberID.ClientID;

            //传阅控件初始化
            this.OASelectUC1.UCDeptTreeUserIDControl = this.txtCirculateDeptID.ClientID;
            this.OASelectUC1.UCDeptNameControl = this.txtCirculateDeptName.ClientID;
            this.OASelectUC1.UCRole = OUConstString.RoleName.COMPANY_LEADER;
            this.OASelectUC1.UCRoleUserIDControl = this.txtCirculateLeaderID.ClientID;
            this.OASelectUC1.UCRoleUserNameControl = this.txtCirculateLeader.ClientID;
            this.OASelectUC1.UCLevel = "2";
            this.OASelectUC1.UCSelectType = "0";
            this.OASelectUC1.UCDeptShowType = "1010";

            //承办
            this.OASelectUC2.UCDeptIDControl = this.txtUnderTakeID.ClientID;
            this.OASelectUC2.UCDeptNameControl = this.txtUnderTake.ClientID;
            this.OASelectUC2.UCLevel = "1";
            this.OASelectUC2.UCSelectType = "0";
            this.OASelectUC2.UCDeptShowType = "1010";
            this.OASelectUC2.UCIsSingle = "1";

            //协办
            this.OASelectUC3.UCDeptNameControl = this.txtAssistance.ClientID;
            this.OASelectUC3.UCDeptIDControl = this.txtAssistID.ClientID;
            this.OASelectUC3.UCLevel = "2";
            this.OASelectUC3.UCSelectType = "0";
            this.OASelectUC3.UCDeptShowType = "1000";

            this.OASelectUC5.UCSelectType = "1";
            this.OASelectUC5.UCDeptUserIDControl = this.txtAssignPersonID.ClientID;
            this.OASelectUC5.UCDeptUserNameControl = this.txtUDPeople.ClientID;
            this.OASelectUC5.UCIsSingle = "1";

            //传阅控件初始化
            this.OASelectUC6.UCDeptUserNameControl = txtCirculatePerson.ClientID;
            this.OASelectUC6.UCDeptUserIDControl = txtCirculatePersonID.ClientID;

            this.OASelectUC6.UCShowDeptID = OADept.GetDeptIDByUser(rentity.ReceiveUserID);
            this.OASelectUC6.UCSelectType = "1";
            this.OASelectUC6.UCTemplateName = base.TemplateName.Replace("新版","");
            this.OASelectUC6.UCFormName = "传阅";

            switch (base.StepName)
            {
                #region 发起流程
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL://
                    controls.EnableControls = new Control[] { this.txtCirculateComment, };
                    controls.YellowControls = new Control[] { this.txtCirculatePerson, this.txtPlotMember, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance };
                    controls.DisEnableControls = new Control[] { this.txtPlotTime, this.ddlLeadShip, this.txtPlotComment, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUnderTakeComment, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeader, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtUDPeopleTime };
                    this.ucAttachment.UCIsEditable = true;
                    //动作初始化
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_TJNB;//提交拟办
                    this.btnCommon1.Visible = true;
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_TJCB;//提交承办
                    this.btnCommon2.Visible = true;
                    this.btnCommon2.OnClientClick = "";
                    this.btnCommon2.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{if(!checkUnderDept()){return false;}else{DisableButtons();}};");
                    this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;//保存
                    this.btnCommon3.Visible = true;
                    if (!string.IsNullOrEmpty(base.WorkItemID) && base.EntryAction != "7")
                    {
                        this.btnCommon4.Text = ProcessConstString.SubmitAction.ACTION_CANCEL;//撤销
                        this.btnCommon4.OnClientClick = "";
                        this.btnCommon4.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");
                        this.btnCommon4.Visible = true;
                    }
                    this.OASelectUC4.Visible = false;
                    this.OASelectUC5.Visible = false;
                    break;
                #endregion

                #region 拟办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_CHECK://

                    if (!this.IsPostBack)
                    {
                        //绑定公司领导
                        OAUser.GetUserByRole(this.ddlLeadShip, OUConstString.RoleName.COMPANY_LEADER);
                        ListItem lt = new ListItem(rentity.LeaderShipName, rentity.LeaderShip);
                        if (!(this.ddlLeadShip.Items.Contains(lt)))
                        {
                            this.ddlLeadShip.Items.Add(lt);
                        }
                        this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
                    }
                    controls.EnableControls = new Control[] { this.ddlLeadShip, this.ddlLeadShip, this.txtCirculateComment, this.txtPlotComment };
                    controls.YellowControls = new Control[] { this.txtCirculatePerson, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance };
                    controls.DisEnableControls = new Control[] { this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUnderTakeComment, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeader, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtUDPeopleTime };
                    this.ucRole.Visible = false;
                    this.OASelectUC4.Visible = false;
                    this.OASelectUC5.Visible = false;
                    //动作初始化
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_TJLDPS;//领导批示
                    this.btnCommon1.Visible = true;
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_TJCB;//提交承办
                    this.btnCommon2.Visible = true;
                    this.btnCommon2.OnClientClick = "";
                    this.btnCommon2.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{if(!checkUnderDept()){return false;}else{DisableButtons();}};");
                    this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_DENY;//退回
                    this.btnCommon3.Visible = true;
                    this.btnCommon3.OnClientClick = "";
                    this.btnCommon3.Attributes.Add("onclick", "javascript: if(!confirm('确定退回到发起流程节点吗？')){return false;}else{DisableButtons();}");
                    this.btnCommon4.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;//保存
                    this.btnCommon4.Visible = true;
                    break;
                #endregion

                #region 领导批示
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_INSTRUCTION://
                    controls.EnableControls = new Control[] { this.txtLeaderComment };
                    controls.DisEnableControls = new Control[] { this.ddlLeadShip, this.txtPlotComment, this.txtCirculatePerson, this.txtUnderTakeComment, this.txtCirculateComment, this.txtUDPeople, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance };
                    this.tblPlot.Visible = true;
                    this.OASelectUC1.Visible = false;
                    this.ucRole.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.OASelectUC3.Visible = false;
                    this.OASelectUC4.Visible = false;
                    this.OASelectUC5.Visible = false;
                    this.OASelectUC6.Visible = false;
                    this.btnInstructionSave.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;
                    this.btnComplete.Text = ProcessConstString.SubmitAction.ACTION_SUBMIT;
                    break;
                #endregion

                #region 协办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_HELP_DIRECTOR://
                    controls.EnableControls = new Control[] { this.txtCirculateComment };
                    controls.YellowControls = new Control[] { this.txtCirculatePerson};
                    controls.DisEnableControls = new Control[] { this.ddlLeadShip, this.txtPlotComment, this.txtUDPeople, this.txtUnderTakeComment, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance, this.txtUDPeopleTime };
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF;//分发，不用了。
                    this.btnCommon1.Visible = false;
                    this.btnCY.Visible = true;
                    this.btnCY.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF;//分发
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_COMPLETE;//承办完成
                    this.btnCommon2.Visible = true;
                    this.divCirculatePerson.Visible = true;
                    this.OASelectUC1.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.OASelectUC3.Visible = false;
                    this.OASelectUC4.Visible = false;
                    this.OASelectUC5.Visible = false;
                    this.ucRole.Visible = false;
                    break;
                #endregion

                #region 处室承办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR://
                    controls.EnableControls = new Control[] { this.txtUnderTakeComment, this.txtCirculateComment };
                    controls.YellowControls = new Control[] { this.txtUDSectionLeader, this.txtUDPeople, this.txtCirculatePerson };
                    controls.DisEnableControls = new Control[] { this.txtPlotComment, this.ddlLeadShip, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance,this.txtUDPeopleTime };
                    //承办科室
                    this.OASelectUC4.UCShowDeptID = OADept.GetChildDeptIDSConSelf(rentity.UnderTakeID, -1);
                    this.OASelectUC4.UCLevel = "2";
                    this.OASelectUC4.UCDeptShowType = "1000";
                    this.OASelectUC4.UCSelectType = "0";
                    this.OASelectUC4.UCIsSingle = "1";
                    this.OASelectUC4.UCDeptTreeUserIDControl = this.txtAssignID.ClientID;
                    this.OASelectUC4.UCDeptTreeUserNameControl = this.txtUDSectionLeader.ClientID;
                    this.OASelectUC4.UCDeptIDControl = this.txtShiefID.ClientID;
                    this.divCirculatePerson.Visible = true;
                    this.OASelectUC5.UCShowDeptID = OADept.GetChildDeptIDSConSelf(rentity.UnderTakeID, -1);

                    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_JBKS;//交办科室
                    this.btnCommon1.Visible = true;
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_JBRY;//交办人员
                    this.btnCommon2.Visible = true;
                    this.btnCommon3.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF;//分发，不用了。
                    this.btnCommon3.Visible = false;
                    this.btnCY.Visible = true;
                    this.btnCY.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF;//分发
                    this.btnCommon4.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_COMPLETE;//承办完成
                    this.btnCommon4.Visible = true;
                    this.btnCommon5.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;//保存
                    this.btnCommon5.Visible = true;
                    this.ucRole.Visible = false;
                    this.OASelectUC1.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.OASelectUC3.Visible = false;
                    break;
                #endregion

                #region 科室承办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_CHIEF://
                    controls.EnableControls = new Control[] { this.txtUnderTakeComment, this.txtCirculateComment };
                    controls.YellowControls = new Control[] { this.txtUDPeople, this.txtCirculatePerson };
                    controls.DisEnableControls = new Control[] { this.ddlLeadShip, this.txtPlotComment, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance, this.txtUDPeopleTime };
                    //补偿老版的科室
                    if (rentity.DraftDate < base.OAStartTime)
                    {
                        ViewBase vb = OADept.GetDeptByDeptUser(rentity.UnderTakeID, rentity.ReceiveUserID, 2);
                        rentity.UDSection = vb != null && vb.Count > 0 ? vb.GetFieldVals("ID", ",") : "";
                    }
                    this.txtUDDeptLeader.Visible = false;
                    this.lblchushi.Visible = true;
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_JBRY;//交办人员
                    this.btnCommon1.Visible = true;
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF;//分发,不用了。
                    this.btnCommon2.Visible = false;
                    this.btnCY.Visible = true;
                    this.btnCY.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF;//分发
                    this.btnCommon3.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_COMPLETE;//承办完成
                    this.btnCommon3.Visible = true;
                    this.btnCommon4.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;//保存
                    this.btnCommon4.Visible = true;
                    this.OASelectUC5.UCShowDeptID = OADept.GetChildDeptIDSConSelf(rentity.UDSection, -1);
                    this.divCirculatePerson.Visible = true;
                    this.ucRole.Visible = false;
                    this.OASelectUC1.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.OASelectUC3.Visible = false;
                    this.OASelectUC4.Visible = false;
                    break;
                #endregion

                #region 人员承办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_MEMBER://
                    controls.EnableControls = new Control[] { this.txtUnderTakeComment, this.txtCirculateComment };
                    controls.YellowControls = new Control[] { this.txtCirculatePerson };
                    controls.DisEnableControls = new Control[] { this.ddlLeadShip, this.txtPlotComment, this.txtUDPeople, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance, this.txtUDPeopleTime };
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_COMPLETE;//承办完成
                    this.btnCommon1.Visible = true;
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;//保存
                    this.btnCommon2.Visible = true;
                    this.ucRole.Visible = false;
                    this.OASelectUC1.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.OASelectUC3.Visible = false;
                    this.OASelectUC4.Visible = false;
                    this.OASelectUC5.Visible = false;
                    if (!string.IsNullOrEmpty(rentity.UDSectionLeadName))
                    {
                        this.txtUDSectionLeader.Visible = false;
                        OASelectUC4.Visible = false;
                        this.lblkeshi.Visible = true;
                    }
                    this.txtUDDeptLeader.Visible = false;
                    this.lblchushi.Visible = true;
                    break;
                #endregion

                #region 函件管理员处理
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_LETTER_ADMIN://
                    //补偿老版oa处室承办人数据
                    if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UnderTakeLeaders))
                    {
                        rentity.UnderTakeLeaders = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                    }
                    controls.EnableControls = new Control[] { this.txtUnderTakeComment, this.txtCirculateComment };
                    controls.YellowControls = new Control[] { this.txtCirculatePerson };
                    controls.DisEnableControls = new Control[] { this.txtUnderTakeComment, this.ddlLeadShip, this.txtPlotComment, this.txtUDPeople, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance, this.txtUDPeopleTime };
                    this.ucAttachment.UCIsEditable = true;
                    //this.divCirculatePerson.Visible = true;
                    this.btnCommon1.Text = ProcessConstString.SubmitAction.ACTION_SUBMIT;//提交
                    this.btnCommon1.Visible = true;
                    this.btnCommon2.Text = ProcessConstString.SubmitAction.ACTION_DENY;//退回
                    this.btnCommon2.Visible = !string.IsNullOrEmpty(rentity.UnderTakeLeaders);
                    this.btnCommon2.OnClientClick = "";
                    this.btnCommon2.Attributes.Add("onclick", "javascript: if(!confirm('确定退回到处室承办节点吗？')){return false;}else{DisableButtons();}");
                    this.btnCommon3.Text = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;//保存
                    this.btnCommon3.Visible = true;
                    this.btnGuidang.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btnGuidang.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                    }
                    //this.btnCommon4.Text = ProcessConstString.StepAction.LetterReceiveAction.ACTION_GD;//归档
                    //this.btnCommon4.Visible = true;
                    this.ucRole.Visible = false;
                    this.OASelectUC1.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.OASelectUC3.Visible = false;
                    this.OASelectUC4.Visible = false;
                    this.OASelectUC5.Visible = false;
                    if (!string.IsNullOrEmpty(rentity.UDSectionLeadName))
                    {
                        this.txtUDSectionLeader.Visible = false;
                        this.OASelectUC4.Visible = false;
                        this.lblkeshi.Visible = true;
                    }
                    if (!string.IsNullOrEmpty(rentity.UDSectionPeopleName))
                    {
                        this.txtUDPeople.Visible = false;
                        OASelectUC5.Visible = false;
                        this.lblrenyuan.Visible = true;
                    }
                    if (!string.IsNullOrEmpty(rentity.UDDeptLeadName))
                    {
                        this.txtUDDeptLeader.Visible = false;
                        this.lblchushi.Visible = true;
                    }
                    break;
                #endregion

                #region Circulate
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_CIRCULATE://
                    //this.ucAttachment.UCIsEditable = false;
                    break;
                #endregion

            }

            #region 历史表单
            if (base.IsPreview)
            {
                this.ucCommentList.UCDateTime = B_FormsData.GetPWSubDate(base.ProcessID, base.WorkItemID, base.TemplateName);
                controls.EnableControls = new Control[] { };
                controls.YellowControls = new Control[] { };
                controls.DisEnableControls = new Control[] { this.txtPlotComment, this.ddlLeadShip, this.txtPlotMember, this.txtCirculateLeader, this.txtUnderTake, this.txtAssistance, this.txtCirculateDeptName, this.txtCirculatePerson, this.txtUnderTakeComment, this.txtCirculateComment, this.txtUDPeople, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance, this.txtUDPeopleTime };
                //if ((base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_LETTER_ADMIN && rentity.LetterAdmin == rentity.ReceiveUserID) || (rentity.DraftDate < base.OAStartTime && FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_LETTER_ADMIN, "", ProcessConstString.StepStatus.STATUS_COMPLETED) == rentity.ReceiveUserID))
                //{
                //    //来文单位
                //    this.ucCompany.UCNameControl = this.txtCommunicationUnit.ClientID;
                //    this.ucCompany.Visible = true;

                //    //紧急程度设置
                //    this.txtUrgentDegree.Visible = false;
                //    this.ddlJinJi.Visible = true;

                //    //收文时间
                //    this.txtReceiveTime.Visible = false;
                //    this.txtReceiveDate.Visible = true;

                //    controls.EnableControls = new Control[] { this.txtCirculateComment, this.txtReceiveNo, this.txtFileEncoding, this.txtDocumentTitle, this.ddlJinJi, this.txtRemark };
                //    controls.YellowControls = new Control[] { this.txtCirculatePerson, this.txtReceiveDate, this.txtCommunicationUnit };
                //    controls.DisEnableControls = new Control[] { this.txtPlotComment, this.ddlLeadShip, this.txtPlotMember, this.txtCirculateLeader, this.txtUnderTake, this.txtAssistance, this.txtCirculateDeptName, this.txtUnderTakeComment, this.txtUDPeople, this.txtUDSectionLeader, this.txtPlotMember, this.txtPlotTime, this.txtLeadShipName, this.txtLeadShipTime, this.txtLSComment, this.txtAssign, this.txtUDDeptLeader, this.txtUDDeptLeaderTime, this.txtUDSectionLeaderTime, this.txtUDPeople, this.txtCirculateLeader, this.txtUnderTake, this.txtCirculateDeptName, this.txtAssistance };
                //    this.divCirculatePerson.Visible = true;
                //    this.btnCommon1.Text = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF2;
                //    this.ucAttachment.UCIsAgain = "0";
                //    //this.divCirculatePerson.Visible = true;
                //    this.btnCommon1.Visible = false;
                //    this.OASelectUC6.Visible = true;
                //}
                //else
                //{
                    this.btnCommon1.Visible = false;
                    this.OASelectUC6.Visible = false;
                //}
                if (base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_INSTRUCTION)
                {
                    this.txtLeaderComment.Attributes.Add("readonly", "true");
                    this.btnInstructionSave.Visible = false;
                    this.btnComplete.Visible = false;
                }
                this.btnCY.Visible = false;
                this.ucRole.Visible = false;
                this.btnGuidang.Visible = false;
                this.OASelectUC1.Visible = false;
                this.OASelectUC2.Visible = false;
                this.OASelectUC3.Visible = false;
                this.OASelectUC4.Visible = false;
                this.OASelectUC5.Visible = false;
                //this.OASelectUC6.Visible = false;
                this.btnCommon2.Visible = false;
                this.btnCommon3.Visible = false;
                this.btnCommon4.Visible = false;
                this.btnCommon5.Visible = false;
                //this.ucAttachment.UCIsEditable = false; //yangzj 20110630
                if (base.IsCanDevolve)
                {
                    this.btnGuidang.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btnGuidang.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                    }
                }
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
            B_LetterReceive rentity = base.EntityData != null ? base.EntityData as B_LetterReceive : new B_LetterReceive();
            this.RegisterID = Request.QueryString[ConstString.QueryString.REGISTER_ID];
            if (!String.IsNullOrEmpty(this.RegisterID))
            {
                B_HSEdit hsedit = new B_HSEdit();
                hsedit.ID = Convert.ToInt32(this.RegisterID);
                if (hsedit == null)
                {
                    JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "当前选择的收文登记信息不存在或者已经被删除,无法继续操作", "Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle");
                    return;
                }
                #region 函件收文基本信息
                this.ucAttachment.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(hsedit.FileData);
                this.txtReceiveNo.Text = hsedit.DocumentNo;
                this.txtReceiveTime.Text = hsedit.ReceiptDate.ToString(ConstString.DateFormat.Normal);
                this.txtUrgentDegree.Text = hsedit.UrgentDegree;
                this.txtFileEncoding.Text = hsedit.FileEncoding;
                this.txtCommunicationUnit.Text = hsedit.CommunicationUnit;
                this.txtDocumentTitle.Text = hsedit.DocumentTitle;
                this.txtRemark.Text = hsedit.Remarks;
                this.txtPageNum.Text = hsedit.Pages.ToString();
                #endregion
            }
            else
            {
                #region 函件收文基本信息
                this.ucAttachment.UCDataList = rentity.FileList; //实体绑定到控件时赋值
                this.txtReceiveNo.Text = rentity.DocumentNo;
                this.txtReceiveTime.Text = rentity.ReceiptDate.ToString(ConstString.DateFormat.Normal);
                this.txtReceiveDate.Text = rentity.ReceiptDate.ToString(ConstString.DateFormat.Normal);
                this.txtUrgentDegree.Text = rentity.UrgentDegree;
                this.ddlJinJi.SelectedValue = rentity.UrgentDegree;
                this.txtFileEncoding.Text = rentity.FileEncoding;
                this.txtCommunicationUnit.Text = rentity.CommunicationUnit;
                this.txtDocumentTitle.Text = rentity.DocumentTitle;
                this.txtRemark.Text = rentity.Remarks;
                this.txtPageNum.Text = rentity.Pages;
                #endregion
            }
            if (this.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_CHECK)//公司领导
            {
                this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
            }
            else
            {
                this.ddlLeadShip.Items.Add(new ListItem(rentity.LeaderShipName, rentity.LeaderShip));
            }
            #region 拟办
            //拟办人
            this.txtPlotMember.Text = rentity.NiBanRenName;
            //传阅领导
            this.txtCirculateLeader.Text = rentity.ChuanYueLeader;
            //承办部门
            this.txtUnderTake.Text = rentity.UnderTake;
            //拟办时间
            if (this.txtPlotMember.Text != string.Empty)
            {
                this.txtPlotTime.Text = rentity.NiBanDate;
            }
            //传阅部门
            this.txtCirculateDeptName.Text = rentity.ChuanYueDept;
            //协办部门
            this.txtAssistance.Text = rentity.AssistDeptName;
            //拟办人意见
            this.txtPlotComment.Text = rentity.NiBanComment;

            //ListItem lt = new ListItem(rentity.LeaderShipName, rentity.LeaderShip);
            //if (!(this.ddlLeadShip.Items.Contains(lt)))
            //{
            //    this.ddlLeadShip.Items.Add(lt);
            //}
            //else
            //{
            //    this.ddlLeadShip.Items.Add(lt);
            //}
            //公司领导
            //this.ddlLeadShip.SelectedValue = rentity.LeaderShip;
            #endregion

            #region 公司领导批示
            this.txtLeadShipName.Text = rentity.LeaderShipName;
            this.txtLeadShipTime.Text = rentity.LS_Date;
            this.txtLSComment.Text = rentity.LS_Comment;
            this.txtLeaderComment.Text = rentity.LS_CommentAdd;
            #endregion

            #region 承办
            this.txtAssign.Text = rentity.UnderTake;
            this.txtUnderTakeComment.Text = rentity.UnderTake_Comment;
            //if (base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR)
            //{
            //    this.txtUDDeptLeader.Text = rentity.UDDeptLeadName;
            //}
            //else
            //{
            this.txtUDDeptLeader.Text = rentity.UDDeptLeadName;
            //}
            //if (base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_CHIEF)
            //{
            //    this.txtUDSectionLeader.Text = CurrentUserInfo.DisplayName;
            //}
            //else
            //{
            this.txtUDSectionLeader.Text = rentity.UDSectionLeadName;
            //}
            this.txtUDDeptLeaderTime.Text = rentity.UDDeptLeadNameTime;

            this.txtUDSectionLeaderTime.Text = rentity.UDSectionLeadNameTime;

            this.lblchushi.Text = rentity.UDDeptLeadName + "<br>" + rentity.UDDeptLeadNameTime;
            this.lblkeshi.Text = rentity.UDSectionLeadName + "<br>" + rentity.UDSectionLeadNameTime;
            this.lblrenyuan.Text = rentity.UDSectionPeopleName + "<br>" + rentity.UDSectionPeopleNameTime;


            this.txtUDPeopleTime.Text = rentity.UDSectionPeopleNameTime;
            this.txtUDPeople.Text = rentity.UDSectionPeopleName;
            #endregion

            #region 传阅
            this.txtCirculatePerson.Text = rentity.CirculateUserName;
            this.txtCirculateComment.Text = rentity.CirculateUserComment;
            this.ucAttachment.UCIsEditable = false;
            #endregion

            //拟稿处理后显示label形式的拟稿人姓名与时间
            if (rentity.NiBanDate != "" && rentity.NiBanDate != null)
            {
                //this.txtPlotMember.Visible = false;
                //this.lbPlotMember.Visible = true;
                //this.lbPlotMember.Text = rentity.NiBanRenName;// +strNewLine + rentity.NiBanDate;
                this.txtPlotMember.Visible = true;
                this.lbPlotMember.Visible = false;
                this.txtPlotMember.Text = rentity.NiBanRenName;// +strNewLine + rentity.NiBanDate;
                this.txtPlotMemberID.Text = rentity.NiBanRen;

                this.txtPlotTime.Visible = false;
                this.lbPlotTime.Visible = true;
                this.lbPlotTime.Text = rentity.NiBanDate;
            }

            //领导；批示处理后显示label形式的领导人姓名与时间
            if (rentity.LS_Date != "" && rentity.LS_Date != null)
            {
                this.txtLeadShipName.Visible = false;
                this.lbLeadShipName.Visible = true;
                this.lbLeadShipName.Text = rentity.LeaderShipName;// +strNewLine + rentity.LS_Date;

                this.txtLeadShipTime.Visible = false;
                this.lbLeadShipTime.Visible = true;
                this.lbLeadShipTime.Text = rentity.LS_Date;
            }

            //处室领导承办后显示label形式的领导人姓名与时间
            if (rentity.UDDeptLeadNameTime != "" && rentity.UDDeptLeadNameTime != null)
            {
                this.txtUDDeptLeader.Visible = false;
                this.lblchushi.Visible = true;
                this.lblchushi.Text = rentity.UDDeptLeadName;// +strNewLine + rentity.UDDeptLeadNameTime;

                this.txtUDDeptLeaderTime.Visible = false;
                this.lbUDDetpLeaderTime.Visible = true;
                this.lbUDDetpLeaderTime.Text = rentity.UDDeptLeadNameTime;
            }

            //科室领导承办后显示label形式的领导人姓名与时间
            if (rentity.UDSectionLeadNameTime != "" && rentity.UDSectionLeadNameTime != null)
            {
                //this.txtLeadShipName.Visible = false;
                this.lblkeshi.Visible = true;
                this.lblkeshi.Text = rentity.UDSectionLeadName;// +strNewLine + rentity.UDSectionLeadNameTime;

                this.txtUDSectionLeaderTime.Visible = false;
                this.lbUDSectionLeaderTime.Visible = true;
                this.lbUDSectionLeaderTime.Text = rentity.UDSectionLeadNameTime;
            }

            //直接承办人承办后显示label形式的承办人姓名与时间
            if (rentity.UDSectionPeopleNameTime != "" && rentity.UDSectionPeopleNameTime != null)
            {
                //this.txtUDPeople.Visible = false;
                this.lblrenyuan.Visible = true;
                this.lblrenyuan.Text = rentity.UDSectionPeopleName;// +strNewLine + rentity.UDSectionPeopleNameTime;

                this.txtUDPeopleTime.Visible = false;
                this.lbUDPeopleTime.Visible = true;
                this.lbUDPeopleTime.Text = rentity.UDSectionPeopleNameTime;
            }


        }

        /// <summary>
        /// 控件填充实体
        /// </summary>
        /// <param name="IsSave">是否保存</param>
        /// <returns>EntityBase</returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_LetterReceive rentity = base.EntityData != null ? base.EntityData as B_LetterReceive : new B_LetterReceive();
            switch (base.StepName)
            {
                #region 发起流程
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL:
                    #region 函件收文基本信息
                    if (rentity.Drafter == string.Empty)
                    {
                        rentity.Drafter = CurrentUserInfo.DisplayName;
                        rentity.DrafterID = CurrentUserInfo.UserName;
                        rentity.ReceiveUserName = rentity.Drafter;
                        rentity.ReceiveUserID = rentity.DrafterID;
                    }
                    rentity.DraftDate = rentity.DraftDate.ToString() == DateTime.MinValue.ToString() ? DateTime.Now : DateTime.MinValue;
                    rentity.FileList = this.ucAttachment.UCDataList;
                    rentity.DocumentNo = this.txtReceiveNo.Text;
                    rentity.ReceiptDate = string.IsNullOrEmpty(this.txtReceiveTime.Text) ? System.DateTime.MinValue : DateTime.Parse(this.txtReceiveTime.Text);
                    rentity.UrgentDegree = this.txtUrgentDegree.Text;
                    rentity.FileEncoding = this.txtFileEncoding.Text;
                    rentity.CommunicationUnit = this.txtCommunicationUnit.Text;
                    rentity.DocumentTitle = this.txtDocumentTitle.Text;
                    rentity.Remarks = this.txtRemark.Text;
                    rentity.Pages = this.txtPageNum.Text;
                    #endregion

                    //附件
                    rentity.FileList = this.ucAttachment.UCDataList;
                    rentity.RegisterID = this.RegisterID;

                    //拟办人
                    rentity.NiBanRenName = this.txtPlotMember.Text;
                    if (!string.IsNullOrEmpty(this.txtPlotMemberID.Text) || string.IsNullOrEmpty(this.txtPlotMember.Text))
                    {
                        rentity.NiBanRen = this.txtPlotMemberID.Text;
                    }

                    //传阅领导
                    if (!string.IsNullOrEmpty(this.txtCirculateLeaderID.Text))
                    {
                        rentity.ChuanYueLeader = this.txtCirculateLeader.Text;
                        rentity.ChuanYueLeaderID = this.txtCirculateLeaderID.Text;
                    }

                    //传阅部门
                    if (!string.IsNullOrEmpty(this.txtCirculateDeptID.Text))
                    {
                        rentity.ChuanYueDept = this.txtCirculateDeptName.Text;
                        if (string.IsNullOrEmpty(rentity.ChuanYueLeaderID))
                        {
                            rentity.ChuanYueLeaderID = this.txtCirculateDeptID.Text;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.txtCirculateDeptID.Text))
                            {
                                rentity.ChuanYueLeaderID = rentity.ChuanYueLeaderID + "," + this.txtCirculateDeptID.Text;
                            }
                        }
                    }

                    //承办
                    if (!string.IsNullOrEmpty(this.txtUnderTakeID.Text))
                    {
                        rentity.UnderTake = this.txtUnderTake.Text;
                        rentity.UnderTakeID = this.txtUnderTakeID.Text;
                    }

                    //协办
                    if (!string.IsNullOrEmpty(this.txtAssistID.Text))
                    {
                        rentity.AssistDeptName = this.txtAssistance.Text;
                        rentity.AssistDept = this.txtAssistID.Text;
                    }

                    //传阅
                    if (!string.IsNullOrEmpty(this.txtCirculatePersonID.Text))
                    {
                        rentity.CirculateUserName = this.txtCirculatePerson.Text;
                        rentity.CirculateUserID = this.txtCirculatePersonID.Text;
                        rentity.CirculateUserComment = this.txtCirculateComment.Text;
                        this.ucAttachment.UCIsEditable = false;
                    }

                    rentity.LetterAdmin = FormsMethod.GetUserIDByViewBase(OAUser.GetUserByRole(OUConstString.RoleName.LETTER_ADMIN));
                    rentity.UnderTakeLeaders = OAUser.GetUserByDeptPostArray(rentity.UnderTakeID,null,true,true)[0];
                    //rentity.AssistMenber = FormsMethod.GetUserIDByViewBase(OAUser.GetDeptLeader(rentity.AssistDept, 0));
                    string[] strAssistMember1 = rentity.AssistDept.Split(';');
                    if (strAssistMember1.Length > 0)
                    {
                        rentity.AssistMenber = String.Empty;
                    }
                    foreach (string str in strAssistMember1)
                    {
                        rentity.AssistMenber += FormsMethod.GetUserIDByViewBase(OAUser.GetDeptManager(str, 0)) + ";";
                    }
                    if (rentity.AssistMenber.Length > 0)
                    {
                        rentity.AssistMenber = rentity.AssistMenber.Substring(0, rentity.AssistMenber.Length - 1);
                    }
                    break;
                #endregion

                #region 拟办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_CHECK:
                    //拟办意见
                    rentity.NiBanComment = this.txtPlotComment.Text;
                    rentity.NiBanDate = string.IsNullOrEmpty(this.txtPlotTime.Text) ? System.DateTime.Now.ToString(ConstString.DateFormat.Long) : rentity.NiBanDate;
                    if (rentity.FirstDraftDate == DateTime.MinValue)
                    {
                        rentity.FirstDraftDate = DateTime.Now;
                    }

                    //传阅领导
                    if (!string.IsNullOrEmpty(this.txtCirculateLeaderID.Text))
                    {
                        rentity.ChuanYueLeader = this.txtCirculateLeader.Text;
                        rentity.ChuanYueLeaderID = this.txtCirculateLeaderID.Text;
                    }
                    //传阅部门
                    if (!string.IsNullOrEmpty(this.txtCirculateDeptID.Text))
                    {
                        rentity.ChuanYueDept = this.txtCirculateDeptName.Text;
                        if (string.IsNullOrEmpty(rentity.ChuanYueLeaderID))
                        {
                            rentity.ChuanYueLeaderID = this.txtCirculateDeptID.Text;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.txtCirculateDeptID.Text))
                            {
                                rentity.ChuanYueLeaderID = rentity.ChuanYueLeaderID + "," + this.txtCirculateDeptID.Text;
                            }
                        }
                    }
                    //承办
                    if (!string.IsNullOrEmpty(this.txtUnderTakeID.Text))
                    {
                        rentity.UnderTake = this.txtUnderTake.Text;
                        rentity.UnderTakeID = this.txtUnderTakeID.Text;
                    }
                    //协办
                    if (!string.IsNullOrEmpty(this.txtAssistID.Text))
                    {
                        rentity.AssistDeptName = this.txtAssistance.Text;
                        rentity.AssistDept = this.txtAssistID.Text;
                    }
                    //传阅
                    if (!string.IsNullOrEmpty(this.txtCirculatePersonID.Text))
                    {
                        rentity.CirculateUserName = this.txtCirculatePerson.Text;
                        rentity.CirculateUserID = this.txtCirculatePersonID.Text;
                        rentity.CirculateUserComment = this.txtCirculateComment.Text;
                    }
                    //公司领导
                    rentity.LeaderShipName = this.ddlLeadShip.SelectedItem.Text;
                    rentity.LeaderShip = this.ddlLeadShip.SelectedValue;
                    //rentity.LeaderShip = "hnpc\\wangran";

                    if (!IsSave && !string.IsNullOrEmpty(this.txtPlotComment.Text))
                    {
                        rentity.CommentList.Clear();
                        CYiJian l_objComment = new CYiJian();
                        l_objComment.Content = this.txtPlotComment.Text;
                        l_objComment.FinishTime = DateTime.Now.ToString();
                        l_objComment.UserID = rentity.ReceiveUserID;
                        l_objComment.UserName = rentity.ReceiveUserName;
                        l_objComment.ViewName = base.StepName;
                        //l_objComment.Action = base.SubAction;
                        rentity.CommentList.Add(l_objComment);
                    }

                    rentity.LetterAdmin = FormsMethod.GetUserIDByViewBase(OAUser.GetUserByRole(OUConstString.RoleName.LETTER_ADMIN));
                    rentity.UnderTakeLeaders = OAUser.GetUserByDeptPostArray(rentity.UnderTakeID,null,true,true)[0];
                    string[] strAssistMember = rentity.AssistDept.Split(';');
                    if (strAssistMember.Length > 0)
                    {
                        rentity.AssistMenber = String.Empty;
                    }
                    foreach (string str in strAssistMember)
                    {
                        rentity.AssistMenber += FormsMethod.GetUserIDByViewBase(OAUser.GetDeptManager(str, 0)) + ";";
                    }
                    if (rentity.AssistMenber.Length > 0)
                    {
                        rentity.AssistMenber = rentity.AssistMenber.Substring(0, rentity.AssistMenber.Length - 1);
                    }
                    break;
                #endregion

                #region 领导批示
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_INSTRUCTION:

                    if (!IsSave && !string.IsNullOrEmpty(this.txtLeaderComment.Text))
                    {
                        rentity.LS_Comment = this.txtLeaderComment.Text;
                        rentity.LS_Date = System.DateTime.Now.ToString(ConstString.DateFormat.Long);
                        rentity.LS_CommentAdd = "";
                        rentity.CommentList.Clear();
                        rentity.LS_Comment = this.txtLeaderComment.Text;
                        CYiJian l_objComment = new CYiJian();
                        l_objComment.Content = this.txtLeaderComment.Text;
                        l_objComment.FinishTime = DateTime.Now.ToString();
                        l_objComment.UserID = rentity.ReceiveUserID;
                        l_objComment.UserName = rentity.ReceiveUserName;
                        l_objComment.ViewName = base.StepName;
                        //l_objComment.Action = base.SubAction;
                        rentity.CommentList.Add(l_objComment);
                    }
                    else
                    {
                        rentity.LS_CommentAdd = this.txtLeaderComment.Text;
                    }
                    break;
                #endregion

                #region 协办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_HELP_DIRECTOR:
                    //传阅
                    rentity.AssistMenber = rentity.ReceiveUserID;
                    rentity.CirculateUserName = this.txtCirculatePerson.Text;
                    rentity.CirculateUserID = this.txtCirculatePersonID.Text;
                    rentity.CirculateUserComment = this.txtCirculateComment.Text;
                    break;
                #endregion

                #region 处室承办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR:
                    //处室承办领导
                    rentity.UnderTakeLeaders = rentity.ReceiveUserID;
                    rentity.UDDeptLeadName = rentity.ReceiveUserName;
                    rentity.UnderTake_Comment = this.txtUnderTakeComment.Text;
                    rentity.UDDeptLeadNameTime = System.DateTime.Now.ToString(ConstString.DateFormat.Long);
                    //承办科室
                    if (base.SubAction == ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_JBKS)
                    {
                        rentity.IsChuShiToPeople = false;
                        rentity.UDSectionLeadName = this.txtUDSectionLeader.Text;
                        if (!string.IsNullOrEmpty(this.txtAssignID.Text) || string.IsNullOrEmpty(this.txtUDSectionLeader.Text))
                        {
                            rentity.UDSectionLeadID = this.txtAssignID.Text;
                        }
                    }
                    if (!string.IsNullOrEmpty(this.txtShiefID.Text))
                    {
                        rentity.UDSection = this.txtShiefID.Text;
                    }
                    //rentity.UDSectionLeadID = "hnpc\\wangran";
                    //人员承办
                    if (base.SubAction == ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_JBRY)
                    {
                        rentity.IsChuShiToPeople = true;
                        rentity.UDSectionPeopleName = this.txtUDPeople.Text;
                        if (!string.IsNullOrEmpty(this.txtAssignPersonID.Text) || string.IsNullOrEmpty(this.txtUDPeople.Text))
                        {
                            rentity.UDSectionPeopleID = this.txtAssignPersonID.Text;
                        }
                    }
                    //rentity.UDSectionPeopleID = "hnpc\\wangran";
                    //传阅
                    if (!string.IsNullOrEmpty(this.txtCirculatePersonID.Text))
                    {
                        rentity.CirculateUserName = this.txtCirculatePerson.Text;
                        rentity.CirculateUserID = this.txtCirculatePersonID.Text;
                        rentity.CirculateUserComment = this.txtCirculateComment.Text;
                    }
                    if (string.IsNullOrEmpty(rentity.LetterAdmin))
                    {
                        rentity.LetterAdmin = FormsMethod.GetUserIDByViewBase(OAUser.GetUserByRole(OUConstString.RoleName.LETTER_ADMIN));
                    }
                    goto Default1;
                #endregion

                #region 科室承办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_CHIEF:
                    if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UnderTakeLeaders))
                    {
                        rentity.UnderTakeLeaders = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                    }
                    rentity.UDSectionLeadID = rentity.ReceiveUserID;
                    rentity.UDSectionLeadName =rentity.ReceiveUserName;
                    rentity.UnderTake_Comment = this.txtUnderTakeComment.Text;
                    rentity.UDSectionLeadNameTime = System.DateTime.Now.ToString(ConstString.DateFormat.Long);

                    //人员承办
                    rentity.UDSectionPeopleName = this.txtUDPeople.Text;
                    if (!string.IsNullOrEmpty(this.txtAssignPersonID.Text) || string.IsNullOrEmpty(this.txtUDPeople.Text))
                    {
                        rentity.UDSectionPeopleID = this.txtAssignPersonID.Text;
                    }
                    //传阅
                    if (!string.IsNullOrEmpty(this.txtCirculatePersonID.Text))
                    {
                        rentity.CirculateUserName = this.txtCirculatePerson.Text;
                        rentity.CirculateUserID = this.txtCirculatePersonID.Text;
                        rentity.CirculateUserComment = this.txtCirculateComment.Text;
                    }
                    goto Default1;
                #endregion

                #region 人员承办
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_MEMBER:
                    if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UDSectionLeadID))
                    {
                        rentity.UDSectionLeadID = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_CHIEF, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                        if (string.IsNullOrEmpty(rentity.UDSectionLeadID))
                        {
                            rentity.IsChuShiToPeople = true;
                            rentity.UnderTakeLeaders = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                        }
                        else
                        {
                            rentity.IsChuShiToPeople = false;
                        }
                    }
                    rentity.UnderTake_Comment = this.txtUnderTakeComment.Text;
                    rentity.UDSectionPeopleNameTime = System.DateTime.Now.ToString(ConstString.DateFormat.Long);

                Default1: if (!IsSave)
                    {
                        rentity.CommentList.Clear();
                        rentity.UnderTake_Comment = this.txtUnderTakeComment.Text;
                        CYiJian l_objComment = new CYiJian();
                        l_objComment.Content = this.txtUnderTakeComment.Text;
                        l_objComment.FinishTime = DateTime.Now.ToString();
                        l_objComment.UserID = rentity.ReceiveUserID;
                        l_objComment.UserName = rentity.ReceiveUserName;
                        l_objComment.ViewName = base.StepName;
                        //l_objComment.Action = base.SubAction;
                        rentity.CommentList.Add(l_objComment);
                    }
                break;
                #endregion

                #region 函件管理员处理
                case ProcessConstString.StepName.LetterReceiveStepName.STEP_LETTER_ADMIN:
                //附件
                rentity.FileList = this.ucAttachment.UCDataList;
                //补偿老版oa处室承办人数据
                if (rentity.DraftDate < base.OAStartTime && string.IsNullOrEmpty(rentity.UnderTakeLeaders))
                {
                    rentity.UnderTakeLeaders = FormsMethod.GetReceiveUserID(base.ProcessID, ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR, "", ProcessConstString.StepStatus.STATUS_COMPLETED);
                }
                //传阅
                if (!string.IsNullOrEmpty(this.txtCirculatePersonID.Text))
                {
                    rentity.CirculateUserName = this.txtCirculatePerson.Text;
                    rentity.CirculateUserID = this.txtCirculatePersonID.Text;
                    rentity.CirculateUserComment = this.txtCirculateComment.Text;
                }
                rentity.LetterAdmin =rentity.ReceiveUserID;

                break;
                #endregion
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
                B_LetterReceive bworkitem = ControlToEntity(false) as B_LetterReceive;
                bworkitem.SubmitDate = System.DateTime.Now;
                if (base.SubAction == ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF)
                {
                    base.Circulate("", "1", "", bworkitem.CirculateUserID, "1", false, "[" + bworkitem.ReceiveUserName + "][" + System.DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:" + this.txtCirculateComment.Text, false);
                }
                else
                {
                    if (strActionName == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                    {
                        B_LetterReceive bworkitem1 = ControlToEntity(true) as B_LetterReceive;
                        bworkitem1.SubmitAction = strActionName;
                        base.FormSubmit(true, strActionName, null, bworkitem1 as EntityBase);
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
                            if (strActionName == ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF2)
                            {
                                base.WorkItemID = Guid.NewGuid().ToString("N");
                                base.StepName = strActionName;
                                bworkitem.SubmitDate = System.DateTime.Now;
                                bworkitem.WorkItemID = base.WorkItemID;
                                bworkitem.CommentList.Clear();
                                bworkitem.UrgentDegree = this.ddlJinJi.SelectedValue;
                                bworkitem.DocumentNo = this.txtReceiveNo.Text;
                                bworkitem.ReceiptDate = this.txtReceiveDate.Text != string.Empty ? DateTime.Parse(this.txtReceiveDate.Text) : DateTime.MinValue;
                                bworkitem.FileEncoding = this.txtFileEncoding.Text;
                                bworkitem.CommunicationUnit = this.txtCommunicationUnit.Text;
                                bworkitem.DocumentTitle = this.txtDocumentTitle.Text;
                                bworkitem.Remarks = this.txtRemark.Text;
                                string filedata = "<FileList />";
                                if (this.ucAttachment.UCDataList.Count > 0)
                                {
                                    string formdata = XmlUtility.SerializeXml<B_LetterReceive>(bworkitem);
                                    int iStartIndex = formdata.IndexOf("<FileList>");
                                    int iEndIndex = formdata.IndexOf("</FileList>");
                                    if (iEndIndex > iStartIndex)
                                    {
                                        filedata = formdata.Substring(iStartIndex, iEndIndex - iStartIndex);
                                        filedata += "</FileList>";
                                    }
                                }
                                base.SaveNewEntity(strActionName, bworkitem);
                                this.updatefile();
                                //更新待办协办的附件
                                FormsMethod.UpdateAssignFileData(base.ProcessID, base.TemplateName, ProcessConstString.StepName.LetterReceiveStepName.STEP_HELP_DIRECTOR, filedata);

                                base.Circulate("", "1", "", this.GetCYPeople(bworkitem), "1", true, "[" +bworkitem.ReceiveUserName + "][" + System.DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:" + this.txtCirculateComment.Text, false);
                            }
                            else
                            {
                                //验证实体
                                string strMessage = B_LetterReceive.CheckEntity(base.StepName, base.SubAction, bworkitem);
                                if (!string.IsNullOrEmpty(strMessage))
                                {
                                    JScript.Alert(strMessage);
                                    return;
                                }
                                if (base.SubAction == ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_TJCB && (!string.IsNullOrEmpty(bworkitem.ChuanYueDeptID) || !string.IsNullOrEmpty(bworkitem.ChuanYueLeaderID)))
                                {
                                    base.Circulate("", "1", "", bworkitem.ChuanYueLeaderID, "1", false, "", false);
                                }
                                Hashtable ht = B_LetterReceive.GetProcNameValue(base.StepName, strActionName, bworkitem);//ap属性
                                base.FormSubmit(false, strActionName, ht, bworkitem as EntityBase);
                                if (base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL || base.EntryAction == "7")
                                {
                                    B_ReceiveEdit.UpdateLetterEdit(base.ProcessID, this.txtReceiveNo.Text);
                                }
                                if (base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_LETTER_ADMIN || base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL)
                                {
                                    this.updatefile();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JScript.Alert(ex.Message);
            }

        }
        #endregion

        /// <summary>
        /// 获取流程参与的人。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetCYPeople(B_LetterReceive entity)
        {
            string strUser = "";
            if (!string.IsNullOrEmpty(entity.DrafterID))
            {
                strUser += entity.DrafterID + ";";
            }
            if (!string.IsNullOrEmpty(entity.NiBanRen))
            {
                strUser += entity.NiBanRen + ";";
            }
            if (!string.IsNullOrEmpty(entity.ChuanYueLeaderID))
            {
                strUser += entity.ChuanYueLeaderID + ";";
            }
            if (!string.IsNullOrEmpty(entity.LeaderShip))
            {
                strUser += entity.ChuanYueLeaderID + ";";
            }
            if (!string.IsNullOrEmpty(entity.UnderTakeLeaders))
            {
                strUser += entity.UnderTakeLeaders + ";";
            }
            if (!string.IsNullOrEmpty(entity.AssistMenber))
            {
                strUser += entity.AssistMenber + ";";
            }
            if (!string.IsNullOrEmpty(entity.UDSectionPeopleID))
            {
                strUser += entity.UDSectionPeopleID + ";";
            }
            if (!string.IsNullOrEmpty(entity.UDSectionLeadID))
            {
                strUser += entity.UDSectionLeadID + ";";
            }
            if (!string.IsNullOrEmpty(this.txtCirculatePersonID.Text))
            {
                strUser += this.txtCirculatePersonID.Text;
            }
            return strUser;
        }

        protected void btnGuidang_Click(object sender, EventArgs e)
        {
            string strMessage = string.Empty;
            try
            {
                this.Devolve(out strMessage);
                base.Devolved(base.ProcessID, base.TemplateName);
                base.IsDevolve = true;
                updatefile();
                JScript.Alert("归档成功！\\n流水号：" + strMessage, false);
            }
            catch (Exception ex)
            {
                base.WriteLog(ex.Message);
                JScript.Alert("归档失败！请查看配置是否正确！", false);
            }
        }
        public void updatefile()
        {
            string filedata = XmlUtility.SerializeXml<List<CFuJian>>(this.ucAttachment.UCDataList);
            B_ReceiveEdit.UpdateFileData(filedata, this.txtReceiveNo.Text, base.TemplateName, true);
        }
    }
}