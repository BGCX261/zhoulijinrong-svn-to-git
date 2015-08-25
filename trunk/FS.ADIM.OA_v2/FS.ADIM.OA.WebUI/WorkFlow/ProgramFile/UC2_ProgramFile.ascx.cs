/*****************************************************************/
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：程序文件流程
//
// 创 建 者：黄 琦
// 创建标识: C_2010.01.13
//
// 修改标识：
// 修改描述：
/*****************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.OA.Framework;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.OA.Framework.WorkFlow.AgilePoint;
using Ascentn.Workflow.Base;
using FS.OA.Framework.WorkFlow;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC2_ProgramFile : FormsUIBase
    {
        #region 页面加载

        #region 当前处理的多条意见信息
        /// <summary>
        /// 当前处理的多条意见信息
        /// </summary>
        private List<CYiJian> _yiJianList = new List<CYiJian>();
        private List<CYiJian> YiJianInfoList
        {
            get
            {
                if (ViewState["YiJianList"] == null)
                {
                    ViewState["YiJianList"] = _yiJianList;
                }
                return ViewState["YiJianList"] as List<CYiJian>;
            }
            set
            {
                ViewState["YiJianList"] = value;
            }
        }
        #endregion

        #region pageLoad事件
        /// <summary>
        /// pageLoad事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitPrint();
            //事件绑定
            this.SubmitEvents();
            if (!Page.IsPostBack)
            {
                if (base.IdentityID == 0)
                {
                    LoadInfo();
                }
                else
                {
                    GetChiefDeptAndSort();
                }
            }
        }
        #endregion

        #region 获取页面传值（程序发起）
        /// <summary>
        /// 获取页面传值
        /// </summary>
        private void LoadInfo()
        {
            wfTimesFlag.Text = "1";
            wfChiefDept.Text = Server.UrlDecode(Request.QueryString["deptID"].Trim());
            wfSort.Text = Server.UrlDecode(Request.QueryString["sort"].Trim());
            wfProgramID.Text = Server.UrlDecode(Request.QueryString["id"].Trim());
            wfSerialID.Text = Server.UrlDecode(Request.QueryString["num"].Trim());

            lblApplyStyle.Text = Server.UrlDecode(Request.QueryString["style"].Trim());
            txtName.Text = Server.UrlDecode(Request.QueryString["name"].Trim());
            txtCode.Text = Server.UrlDecode(Request.QueryString["code"].Trim());
            txtEdition.Text = Server.UrlDecode(Request.QueryString["edition"].Trim());

            //编写
            lblWrite.Text = CurrentUserInfo.DisplayName;
            wfWriteID.Text = CurrentUserInfo.UserName;
            //发起
            wfDrafter.Text = CurrentUserInfo.DisplayName;
            wfDrafterID.Text = CurrentUserInfo.UserName;
            //wfDraftDate.Text = DateTime.Now.ToString();

            string[] QGMember = OAUser.GetUserByRoleName(ConstString.RoleName.QUALITY_MEMBER);
            if (QGMember.Length == 0)
            {
                lblQG.ToolTip = "质保人员尚无，请联系系统管理员。";
                lblQG.Text = lblQG.ToolTip.Substring(0, 3) + "...";
            }//是否存在质保审查人员
            else
            {
                lblQG.ToolTip = QGMember[1].ToString();//质保审查人
                lblQG.Text = lblQG.ToolTip.Length > 3 ? lblQG.ToolTip.Substring(0, 3) + "..." : lblQG.ToolTip;
                wfQualityIDs.Text = QGMember[0].ToString();//质保审查人
            }
        }
        #endregion

        #region 组织机构绑定
        /// <summary>
        /// 绑定组织机构
        /// </summary>
        protected override void BindOUControl()
        {
            switch (wfSort.Text)
            {
                case ConstString.ProgramFile.PROGRAM_SORT_MANAGE://（公司）管理程序
                    //审核
                    //+处级以上 +负责人 +部门领导
                    //ddlAudit  wfChiefDept   ConstString.PostName.FU_CHUZHANG
                    OAUser.GetUserByDeptPost(ddlAudit, wfChiefDept.Text, ConstString.PostName.FU_CHUZHANG, true, true);
                    //批准 绑定公司领导
                    OAUser.GetUserByRole(ddlApprove, ConstString.RoleName.COMPANY_LEADER);

                    ddlAudit.ToolTip = OADept.GetDeptName(wfChiefDept.Text) + " 副处长以上+部门负责人+部门领导";
                    ddlApprove.ToolTip = "角色--公司领导";
                    break;
                case ConstString.ProgramFile.PROGRAM_SORT_DEPTMANAGE://部门级管理程序
                    //审核
                    //+科级以上 +负责人 +部门领导
                    //ddlAudit  wfChiefDept   ConstString.PostName.FU_KEZHANG
                    OAUser.GetUserByDeptPost(ddlAudit, wfChiefDept.Text, ConstString.PostName.FU_KEZHANG, true, true);
                    //批准 根据部门ID绑定处长
                    OAUser.GetUserByDeptPost(ddlApprove, wfChiefDept.Text, ConstString.PostName.FU_CHUZHANG, true, true);
                    ddlAudit.ToolTip = OADept.GetDeptName(wfChiefDept.Text) + " 副科长以上+部门负责人+部门领导";
                    ddlApprove.ToolTip = OADept.GetDeptName(wfChiefDept.Text) + " 副处长以上+部门负责人+部门领导";
                    break;
                case ConstString.ProgramFile.PROGRAM_SORT_WORK://工作程序
                    //审核
                    //+科级以上 +负责人 +部门领导
                    //ddlAudit  wfChiefDept   ConstString.PostName.FU_KEZHANG
                    OAUser.GetUserByDeptPost(ddlAudit, wfChiefDept.Text, ConstString.PostName.FU_KEZHANG, true, true, -1);
                    //OAUser.GetUserByPost(ddlApprove, wfChiefDept.Text, ConstString.PostName.CHUZHANG, ConstString.Grade.ZERO);//批准 根据部门ID绑定处长
                    //M_20100414 huangqi des:批准人员 根据部门ID绑定部门负责人
                    ddlApprove.Items.Clear();  //杨子江 20110721
                    OAUser.GetDeptManagerByDeptID(ddlApprove, wfChiefDept.Text, -1);
                    OAUser.GetUserByRole(ddlApprove, ConstString.RoleName.COMPANY_LEADER);
                    ddlAudit.ToolTip = OADept.GetDeptName(wfChiefDept.Text) + " 副科长以上+部门负责人+部门领导";
                    ddlApprove.ToolTip = OADept.GetDeptName(wfChiefDept.Text) + " 部门负责人";
                    break;
            }
            /*  暂时使用根据发起部门加载人员  */
            //OAUser.GetUserByDeptID(ddlApprove, wfChiefDept.Text, -1);
            //OAUser.GetUserByDeptID(ddlAudit, wfChiefDept.Text, -1);
        }
        #endregion

        #region 控件初始设置
        protected override void SetControlStatus()
        {
            //txtInfo.ToolTip = "500字符以内";
            //txtInfo2.ToolTip = "500字符以内";
            //txtWriteExplain.ToolTip = "200字符以内";

            ucFileControl.UCTemplateName = base.TemplateName;
            ucFileControl.UCProcessID = base.ProcessID;
            ucFileControl.UCWorkItemID = base.WorkItemID;
            ucFileControl.UCTBID = base.IdentityID.ToString();

            txtSendDeptName.Attributes.Add("ReadOnly", "true");
            txtSendUserName.Attributes.Add("ReadOnly", "true");
            txtCirculateDeptName.Attributes.Add("ReadOnly", "true");
            txtCirculateUserName.Attributes.Add("ReadOnly", "true");

            OAControl controls = new OAControl();

            switch (base.StepName)
            {
                case ProcessConstString.StepName.ProgramFile.STEP_WRITE: //编制
                    
                    //选择分发部门、人员
                    ucSelectSender.UCSelectType = "2";
                    ucSelectSender.UCDeptIDControl = hfSendDeptID.ClientID;
                    ucSelectSender.UCDeptNameControl = txtSendDeptName.ClientID;
                    ucSelectSender.UCDeptUserIDControl = hfSendUserID.ClientID;
                    ucSelectSender.UCDeptUserNameControl = txtSendUserName.ClientID;
                    ucSelectSender.UCAllSelect = "1";

                    //选择校核人员
                    ucSelectAuditor.UCSelectType = "1";
                    ucSelectAuditor.UCIsSingle = "1";
                    ucSelectAuditor.UCDeptUserIDControl = wfCheckID.ClientID;
                    ucSelectAuditor.UCDeptUserNameControl = txtCheckName.ClientID;

                    //控件状态
                    needCheck.ForeColor = System.Drawing.Color.Red;
                    needApprove.ForeColor = System.Drawing.Color.Red;

                    ucBuMenHuiQian.UCIsAllowDel = base.IdentityID == 0 ? false : true;//可勾选checkbox逻辑删除已会签信息

                    if (!IsPostBack)
                    {
                        //工作程序（部门会签或领导会签）
                        rdolstSignStyle.Visible = wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_WORK ? true : false;
                        if (wfSort.Text != ConstString.ProgramFile.PROGRAM_SORT_MANAGE)
                        {
                            if (ucLDHuiQian.UCHQList.Count > 0)
                            {
                                rdolstSignStyle.SelectedIndex = 1;
                                pnlDeptSign.Visible = false;
                                pnlLeaderSign.Visible = true;
                            }//存在领导会签
                            else
                            {
                                rdolstSignStyle.SelectedIndex = 0;
                                pnlDeptSign.Visible = true;
                                pnlLeaderSign.Visible = false;
                            }
                        }//部门级管理程序、工作程序
                    }
                    controls.EnableControls = new Control[] { cbIsPrint, txtPages, txtName };
                    controls.DisEnableControls = new Control[] { btnSend, btnBack, btnArchive, txtCheckName, tdSignRegion };//pnlDealSign, pnlCommentInfo 
                    controls.YellowControls = new Control[] { txtCheckName };
                    //处理落实面板
                    pnlDealSign.Visible = this.pnlDeptComment.Visible || this.pnlLeaderComment.Visible || pnlQGComment.Visible || this.pnlApproveComment.Visible ? true : false;
                    this.pnlPromptInfo.Visible = false;//校核、审核添加意见面板
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_CHECK: //校核
                    controls.DisEnableControls =
                        new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, ddlApprove, divButtomBtns,
                        btnDeptSign,btnLeaderSign,btnAuditApprove,txtPages,btnAuditCirculate};

                    needAudit.ForeColor = System.Drawing.Color.Red;
                    pnlDealSign.Visible = false;//处理落实面板
                    pnlSend.Visible = false;//分发面板
                    pnlComments.Visible = false;//领导添加意见面板
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_AUDIT: //审核
                    //非管理程序 并且经过部门会签，则显示传阅按钮
                    //btnAuditCirculate.Visible = wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_MANAGE ? false : true;

                    pnlDealSign.Visible = false;//处理落实面板
                    pnlSend.Visible = false;//分发面板
                    pnlComments.Visible = false;//领导添加意见面板
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;

                    switch (wfSort.Text)
                    {
                        case ConstString.ProgramFile.PROGRAM_SORT_MANAGE:
                            controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, 
                        divButtomBtns, btnDeptSign, btnLeaderSign, btnAuditApprove, txtPages,ddlApprove,ddlAudit,btnAuditCirculate };
                            break;
                        case ConstString.ProgramFile.PROGRAM_SORT_DEPTMANAGE:
                            controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages, ddlApprove, ddlAudit, btnAuditApprove, btnLeaderSign, btnAuditCirculate };
                            CheckBMHQStatus();
                            break;
                        case ConstString.ProgramFile.PROGRAM_SORT_WORK:
                            controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages, ddlApprove, ddlAudit, btnSubmit, btnAuditCirculate };
                            CheckLDHQStatus();
                            CheckBMHQStatus();
                            break;
                        default:
                            break;
                    }
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN: //部门会签
                    #region 协助会签区域相关设置
                    txtAssistInfo.Attributes.Add("ReadOnly", "true");
                    txtAssistInfo.CssClass = "textarea_blue";
                    txtAssignMember.Attributes.Add("ReadOnly", "true");
                    divAssign.Visible = true;
                    pnlComments.Visible = true;//协助会签主区域
                    divAssignDeal.Visible = false;//协助会签按钮区域

                    if (base.IsPreview == false)
                    {
                        //选择交办人员
                        ucSelectAssign.UCSelectType = "1";
                        ucSelectAssign.UCIsSingle = "1";
                        ucSelectAssign.UCDeptUserIDControl = wfAssignUserID.ClientID;
                        ucSelectAssign.UCDeptUserNameControl = txtAssignMember.ClientID;
                        string strShowDeptID = string.Empty;
                        if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                        {
                            foreach (M_ProgramFile.DeptSign sign in ucBuMenHuiQian.UCHQList)
                            {
                                if (sign.ID == wfReceiveUserID.Text)
                                {
                                    strShowDeptID += OADept.GetChildDeptIDSConSelf(sign.DeptID, -1) + ",";
                                }
                            }
                        }
                        else
                        {
                            foreach (M_ProgramFile.DeptSign sign in ucBuMenHuiQian.UCHQList)
                            {
                                if (sign.ID == CurrentUserInfo.UserName)
                                {
                                    strShowDeptID += OADept.GetChildDeptIDSConSelf(sign.DeptID, -1) + ",";
                                }
                            }
                        }
                        strShowDeptID = strShowDeptID.Substring(0, strShowDeptID.Length - 1);
                        ucSelectAssign.UCShowDeptID = strShowDeptID;
                    }//非查看历史

                    #endregion

                    this.ucFileControl.UCIsEditable = false;//附件不可编辑
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlDealSign.Visible = false;//处理落实面板
                    this.pnlSend.Visible = false;//分发面板
                    this.pnlPromptInfo.Visible = false;//校核、审核添加意见面板
                    controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages,btnLeaderSign,
                                                                 btnDeptSign2,btnLeaderSign2,btnQGApprove,btnQGCirculate,btnQGBack,ddlApprove,ddlAudit};
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_ASSIST_SIGN://协助会签
                    #region 协助会签区域相关设置
                    pnlComments.Visible = false;//会签提交区域
                    divAssign.Visible = true; //协助会签主区域
                    trLeaderDeal.Visible = false;//协助会签交办选择区域
                    #endregion

                    this.ucFileControl.UCIsEditable = false;//附件不可编辑
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlDealSign.Visible = false;//处理落实面板
                    this.pnlSend.Visible = false;//分发面板
                    this.pnlPromptInfo.Visible = false;//校核、审核添加意见面板
                    controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages,btnLeaderSign,
                                                                 btnDeptSign2,btnLeaderSign2,btnQGApprove,btnQGCirculate,btnQGBack,ddlApprove,ddlAudit};
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_QG: //质保审查
                    if (!IsPostBack)
                    {
                        //处理落实面板（取决于批准落实面板状态）
                        this.pnlDealSign.Visible = this.pnlApproveComment.Visible;
                        //校核、审核添加意见面板
                        this.pnlPromptInfo.Visible = false;
                        //分发 分发意见
                        this.txtSendComemnt.Visible = true;
                        //pnlDistributeComment.Visible = true;
                        //传阅控件属性设置
                        ucSelectCirculate.UCSelectType = "2";
                        ucSelectCirculate.UCAllSelect = "1";
                        ucSelectCirculate.UCDeptIDControl = hfCirculateDeptID.ClientID;
                        ucSelectCirculate.UCDeptNameControl = txtCirculateDeptName.ClientID;
                        ucSelectCirculate.UCDeptUserIDControl = hfCirculateUserID.ClientID;
                        ucSelectCirculate.UCDeptUserNameControl = txtCirculateUserName.ClientID;
                        CheckLDHQStatus();//领导会签按钮状态
                        CheckBMHQStatus();//部门会签按钮状态
                        this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见

                        if (wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_DEPTMANAGE)
                        {
                            btnSubmitFenFa.Visible = string.IsNullOrEmpty(lblApproveDate.Text) ? false : true;//提交分发按钮
                            controls.EnableControls = new Control[] { cbIsPrint, btnQGApprove, txtSendComemnt };
                            //20100505 huangqi des：修改为可选择批准人
                            controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, btnDeptSign2, btnLeaderSign2, btnSubmits, txtPages, btnQGCirculate, ddlAudit };
                            pnlLeaderSign.Visible = false;//无领导会签
                        }//部门级管理程序
                        else
                        {
                            if (wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_MANAGE)
                            {
                                QualityCheck(); //质保审查节点按钮状态设置
                                CheckCirculateStatus();//设置传阅相关控件状态
                            }//管理程序
                            btnSubmitFenFa.Visible = string.IsNullOrEmpty(lblApproveDate.Text) ? false : true;//提交分发按钮
                            needApprove.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    controls.EnableControls = new Control[] { cbIsPrint, txtSendComemnt };
                    //20100505 huangqi des：修改为可选择批准人
                    controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages, ddlAudit };

                    this.btnQGApprove.Visible = true;////////////////////////杨子江，20110119
                    this.btnSubmit.Visible = false;//////////////////////////杨子江，20110119
                    this.btnSubmits.Visible = false;//////////////////////////杨子江，20110120
                    break;
                //btnDeptSign, btnLeaderSign,
                case ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN: //领导会签
                    this.ucFileControl.UCIsEditable = false;
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlDealSign.Visible = false;//处理落实面板
                    this.pnlSend.Visible = false;//分发面板
                    this.pnlPromptInfo.Visible = false;//校核、审核添加意见面板
                    
                    //ListItem temp = ddlApprove.Items.FindByText("孙云根");

                    //ddlApprove.Items.Remove(temp);

                    controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages, btnLeaderSign,
                                                                 btnDeptSign2,btnLeaderSign2,btnQGApprove,btnQGCirculate,btnQGBack,ddlApprove,ddlAudit};
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_APPROVE: //批准
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlDealSign.Visible = false;//处理落实面板
                    this.pnlSend.Visible = false;//分发面板
                    this.pnlPromptInfo.Visible = false;//校核、审核添加意见面板

                    controls.DisEnableControls = new Control[] { ucSelectAuditor, ucSelectSender, txtCheckName, divButtomBtns, txtPages, btnLeaderSign,
                                                                 btnDeptSign2,btnLeaderSign2,btnQGApprove,btnQGCirculate,btnQGBack,ddlApprove,ddlAudit};
                    break;

                case ProcessConstString.StepName.ProgramFile.STEP_SEND: //分发
                    //选择分发部门、人员
                    ucSelectSender.UCSelectType = "2";
                    ucSelectSender.UCDeptIDControl = hfSendDeptID.ClientID;
                    ucSelectSender.UCDeptNameControl = txtSendDeptName.ClientID;
                    ucSelectSender.UCDeptUserIDControl = hfSendUserID.ClientID;
                    ucSelectSender.UCDeptUserNameControl = txtSendUserName.ClientID;
                    ucSelectSender.UCAllSelect = "1";
                    if (base.IsDevolve)
                    {
                        this.btnArchive.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableCtrls();}");
                    }
                    //部门、领导会签 不可操作
                    this.ucBuMenHuiQian.UCIsDelInvisible = true;//被删已会签部门不可见
                    this.ucBuMenHuiQian.UCIsDisEnable = true;
                    this.ucLDHuiQian.UCIsDisEnable = true;
                    needPages.ForeColor = System.Drawing.Color.Red;
                    this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                    this.pnlDealSign.Visible = false;//处理落实面板
                    this.txtSendComemnt.Visible = true;//分发意见
                    //pnlDistributeComment.Visible = true;

                    controls.EnableControls = new Control[] { txtPages };
                    controls.DisEnableControls = new Control[] { ucSelectAuditor, btnSubmitCheck, txtCheckName, tdSignRegion, ddlApprove, ddlAudit };//pnlDealSign, pnlPromptInfo 

                    //工作程序无质保节点，暂无回退
                    btnBack.Visible = wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_WORK ? false : true;
                    break;
                case ProcessConstString.StepName.ProgramFile.STEP_CRICULATE://传阅
                    controls.DisEnableControls = new Control[] { tdSignRegion };
                    pnlDealSign.Visible = false;
                    break;
                default:
                    break;
            }
            //设置所有控件状态
            controls.SetControls();

            //工作程序
            if (wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_WORK)
            {
                //设置radio选项
                if (ucLDHuiQian.UCHQList.Count > 0)
                {
                    rdolstSignStyle.SelectedIndex = 1;
                }
                pnlQGRegion.Visible = false;//工作程序不经过质保节点
            }
            //是否查看历史
            if (base.IsPreview)
            {
                FormsMethod.SetControlAll(this);
                this.ucFileControl.UCIsEditable = false;
                this.ucBuMenHuiQian.UCIsDisEnable = true;
                this.ucLDHuiQian.UCIsDisEnable = true;
                this.pnlDeptSign.Visible = ucBuMenHuiQian.UCHQList.Count != 0 ? true : false;
                this.pnlLeaderSign.Visible = ucLDHuiQian.UCHQList.Count != 0 ? true : false;
                this.ucSelectAssign.Visible = false;
                this.ucSelectAuditor.Visible = false;
                this.ucSelectCirculate.Visible = false;
                this.ucSelectSender.Visible = false;
                if (base.IsCanDevolve)
                {
                    this.btnArchive.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btnArchive.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableCtrls();}");
                    }
                }
            }
        }

        /// <summary>
        /// 判断领导会签按钮状态、显示
        /// </summary>
        private void CheckLDHQStatus()
        {
            if (ucLDHuiQian.UCHQList.Count != 0)
            {
                btnSubmit.Visible = false;
                btnSubmits.Visible = false;
                btnQGApprove.Visible = false;//质保提交批准按钮
                btnAuditApprove.Visible = false;
                foreach (M_ProgramFile.LeaderSign sign in ucLDHuiQian.UCHQList)
                {
                    if (!string.IsNullOrEmpty(sign.IsAgree))
                    {
                        switch (wfSort.Text)
                        {
                            case ConstString.ProgramFile.PROGRAM_SORT_DEPTMANAGE:
                            case ConstString.ProgramFile.PROGRAM_SORT_MANAGE:
                                btnQGApprove.Visible = true;//质保提交批准按钮
                                btnAuditApprove.Visible = true;
                                break;
                            case ConstString.ProgramFile.PROGRAM_SORT_WORK:
                                btnAuditApprove.Visible = true;
                                break;
                        }
                        break;
                    }//已经过领导会签
                }
            }
            else
            {
                btnLeaderSign.Visible = false;
                btnLeaderSign2.Visible = false;
            }
        }

        /// <summary>
        /// 判断部门会签、提交、提交批准按钮状态、显示
        /// </summary>
        private void CheckBMHQStatus()
        {
            if (ucBuMenHuiQian.UCHQList.Count != 0 && ucBuMenHuiQian.UCGetLoadSignUser().Count != 0)
            {
                btnAuditApprove.Visible = false;
                btnQGApprove.Visible = false;//质保提交批准按钮
                btnLeaderSign.Visible = false;
                btnLeaderSign2.Visible = false;
                btnSubmits.Visible = false;
                btnSubmit.Visible = false;
                foreach (M_ProgramFile.DeptSign sign in ucBuMenHuiQian.UCHQList)
                {
                    if (!string.IsNullOrEmpty(sign.IsAgree))
                    {
                        switch (wfSort.Text)
                        {
                            case ConstString.ProgramFile.PROGRAM_SORT_MANAGE:
                                btnLeaderSign.Visible = ucLDHuiQian.UCHQList.Count > 0 ? true : false;
                                btnLeaderSign2.Visible = ucLDHuiQian.UCHQList.Count > 0 ? true : false;
                                btnQGApprove.Visible = true;//质保提交批准按钮
                                break;
                            case ConstString.ProgramFile.PROGRAM_SORT_WORK:
                                btnAuditApprove.Visible = true;
                                break;
                            case ConstString.ProgramFile.PROGRAM_SORT_DEPTMANAGE:
                                btnSubmit.Visible = true;     //提交按钮
                                this.btnSubmits.Visible = true;
                                break;
                        }
                        break;
                    }//已经过部门会签
                }
            }
            else
            {
                btnDeptSign.Visible = false;
                btnDeptSign2.Visible = false;
            }
        }

        /// <summary>
        /// 判断传阅状态、显示
        /// </summary>
        private void CheckCirculateStatus()
        {
            if (ucBuMenHuiQian.UCHQList.Count == 0 && ucLDHuiQian.UCHQList.Count == 0)
            {
                btnAuditCirculate.Visible = false;
                btnQGCirculate.Visible = false;
                pnlCirculate.Visible = false;
            }
        }

        /// <summary>
        /// 质保审查节点按钮状态设置
        /// </summary>
        private void QualityCheck()
        {
            pnlCirculate.Visible = true;
            btnQGCirculate.Visible = true;
            TimeSpan ts = new TimeSpan();
            if (!string.IsNullOrEmpty(wfCirculateDate.Text))
            {
                ts = DateTime.Parse(wfCirculateDate.Text) - DateTime.Now;
            }

            if (ts.Days <= 0)
            {
                //btnApprove.Visible = btnApprove.Visible == false ? false : true;
                btnQGApprove.Visible = btnQGApprove.Visible;//质保提交批准按钮
                btnBack2.Visible = true;
                //质保节点 隐藏退回按钮
                this.btnQGBack.Visible = base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG;
            }//超时或质保无分发
            else
            {
                //btnApprove.Visible = false;
                btnQGApprove.Visible = false;
                this.btnQGBack.Visible = false;
            }
            this.btnSubmits.Visible = false;
        }

        /// <summary>
        /// 工作程序、部门级管理程序审核节点按钮状态设置
        /// </summary>
        private void AuditCheck()
        {
            pnlCirculate.Visible = true;
            btnAuditCirculate.Visible = true;
            TimeSpan ts = new TimeSpan();
            if (!string.IsNullOrEmpty(wfCirculateDate.Text))
            {
                ts = DateTime.Parse(wfCirculateDate.Text) - DateTime.Now;
            }

            if (ts.Days <= 0)
            {
                btnSubmit.Visible = btnSubmit.Visible;
                btnBack.Visible = true;
                this.btnBack.Visible = base.StepName == ProcessConstString.StepName.ProgramFile.STEP_AUDIT;
            }//超时或审核无分发
            else
            {
                btnSubmit.Visible = false;
                btnBack.Visible = false;
            }
        }

        #endregion

        #endregion

        #region 实体加载与赋值

        /// <summary>
        /// 获取部门ID
        /// </summary>
        private void GetChiefDeptAndSort()
        {
            B_PF entity = base.EntityData as B_PF;
            if (entity != null)
            {
                wfChiefDept.Text = entity.SendDeptID;
                wfSort.Text = entity.ProgramSort;
            }
        }
        /// <summary>
        /// 实体加载
        /// </summary>
        protected override void EntityToControl()
        {
            B_PF entity = base.EntityData as B_PF;

            if (entity != null)
            {
                this.cbIsPrint.Checked = entity.IsProgramCompanCheck;//是否需要工程公司会签

                wfTimesFlag.Text = entity.TimesFlag;//次数标示

                wfChiefDept.Text = entity.SendDeptID;//主办部门ID
                wfSort.Text = entity.ProgramSort;//程序分类

                txtName.Text = entity.DocumentTitle;//程序文件名称
                txtCode.Text = entity.ProgramCode;//程序编码
                txtEdition.Text = entity.Edition;//程序版次
                txtPages.Text = entity.TextPageSum;//总页数
                lblApplyStyle.Text = entity.ApplyStyle;//申请类型
                wfReceiveUserID.Text = entity.ReceiveUserID;//接收人用户ID
                wfReceiveUserName.Text = entity.ReceiveUserName;//接收人用户姓名

                //if (lblApplyStyle.Text == ConstString.Miscellaneous.PROGRAM_DELETE)
                //{
                //    btnSend.Text = "注销分发";
                //}//注销程序

                txtWriteExplain.Text = entity.WriteExplain;//编制修订说明
                lblWrite.Text = entity.WriteName;//编制人员
                wfWriteID.Text = entity.WriteID;

                this.txtCheckName.Text = entity.CheckName;//校核人员  renjinquan+
                this.wfCheckID.Text = entity.CheckID;

                //ListItem temp = ddlApprove.Items.FindByText("孙云根");

                //ddlApprove.Items.Remove(temp);

                FormsMethod.SelectedDropDownList(ddlAudit, entity.AuditID, entity.AuditName);//审核人员
                FormsMethod.SelectedDropDownList(ddlApprove, entity.ApproverID, entity.ApproveName);//批准人员

                //同意、否决
                lblCheckAgree.Text = entity.CheckerIsAgree;
                lblAuditAgree.Text = entity.AuditorIsAgree;
                lblQGAgree.Text = entity.QualityIsAgree;
                lblApproveAgree.Text = entity.ApproverIsAgree;

                //校核
                lblCheckComment.ToolTip = entity.CheckComment;
                lblCheckComment.Text = SysString.TruncationString(entity.CheckComment, 30);
                tdCheckComment.Attributes.Add("onmouseover", "this.style.cursor='hand'");
                tdCheckComment.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}');ShowPopDiv('popDiv'); ",
                                                           Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_CHECK), string.Empty));
                if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.ProgramFile.STEP_CHECK)
                {
                    txtInfo.Text = entity.CheckComment;
                }

                //审核
                lblAuditComment.ToolTip = entity.AuditComment;
                lblAuditComment.Text = SysString.TruncationString(entity.AuditComment, 30);
                tdAuditComment.Attributes.Add("onmouseover", "this.style.cursor='hand'");
                tdAuditComment.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}');ShowPopDiv('popDiv'); ",
                                                              Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_AUDIT), string.Empty));
                if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.ProgramFile.STEP_AUDIT)
                {
                    txtInfo.Text = entity.AuditComment;
                }

                //质保
                lblQGComment.ToolTip = entity.QualityComment;
                lblQGComment.Text = SysString.TruncationString(entity.QualityComment, 30);
                tdQulityComment.Attributes.Add("onmouseover", "this.style.cursor='hand'");
                //判断质保审查是否已存在接收人员，存在：返回质保人员ID,否则为空，供查看质保意见历史信息使用
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG && base.IsPreview == false)
                {
                    if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                    {
                        lblQG.ToolTip = wfReceiveUserName.Text;
                        lblQG.Text = wfReceiveUserName.Text;
                        tdQulityComment.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv'); ",
                                                                  Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_QG), Server.UrlEncode(wfReceiveUserID.Text), string.Empty));
                        this.wfQualityIDs.Text = wfReceiveUserID.Text;
                    }
                    else
                    {
                        lblQG.ToolTip = CurrentUserInfo.DisplayName;
                        lblQG.Text = CurrentUserInfo.DisplayName;
                        tdQulityComment.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv'); ",
                                                                  Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_QG), Server.UrlEncode(CurrentUserInfo.UserName), string.Empty));
                        this.wfQualityIDs.Text = CurrentUserInfo.UserName;
                    }
                }
                else
                {
                    lblQG.ToolTip = entity.QualityNames;//质保人员
                    lblQG.Text = entity.QualityNames.Length > 3 ? entity.QualityNames.Substring(0, 3).Replace(";", "") + "..." : entity.QualityNames.Replace(";", "");
                    string qualityID = entity.QualityIDs.IndexOf(";") == -1 ? entity.QualityIDs : string.Empty;
                    tdQulityComment.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv'); ",
                                                                  Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_QG), Server.UrlEncode(qualityID), string.Empty));
                    this.wfQualityIDs.Text = entity.QualityIDs;
                }

                //批准
                string pizhunID = entity.ApproverID.IndexOf(";") == -1 ? entity.ApproverID : string.Empty;
                lblApproveComment.ToolTip = entity.ApproveComment;
                lblApproveComment.Text = SysString.TruncationString(entity.ApproveComment, 30);
                tdApprove.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv'); ",
                                                               Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_APPROVE), Server.UrlEncode(pizhunID), string.Empty));
                tdApprove.Attributes.Add("onmouseover", "this.style.cursor='hand'");//renjinquan 修改

                //提交日期
                lblWriteDate.Text = entity.WriteDate == DateTime.MinValue ? string.Empty : entity.WriteDate.ToString(ConstString.DateFormat.Normal);
                lblWriteDate.ToolTip = entity.WriteDate == DateTime.MinValue ? string.Empty : entity.WriteDate.ToString();
                lblCheckDate.Text = entity.CheckDate == DateTime.MinValue ? string.Empty : entity.CheckDate.ToString(ConstString.DateFormat.Normal);
                lblCheckDate.ToolTip = entity.CheckDate == DateTime.MinValue ? string.Empty : entity.CheckDate.ToString();
                lblAuditDate.Text = entity.AuditDate == DateTime.MinValue ? string.Empty : entity.AuditDate.ToString(ConstString.DateFormat.Normal);
                lblAuditDate.ToolTip = entity.AuditDate == DateTime.MinValue ? string.Empty : entity.AuditDate.ToString();
                lblQGDate.Text = entity.QualityDate == DateTime.MinValue ? string.Empty : entity.QualityDate.ToString(ConstString.DateFormat.Normal);
                lblQGDate.ToolTip = entity.QualityDate == DateTime.MinValue ? string.Empty : entity.QualityDate.ToString();
                lblApproveDate.Text = entity.ApproveDate == DateTime.MinValue ? string.Empty : entity.ApproveDate.ToString(ConstString.DateFormat.Normal);
                lblApproveDate.ToolTip = entity.ApproveDate == DateTime.MinValue ? string.Empty : entity.ApproveDate.ToString();

                ucFileControl.UCDataList = entity.FileList;//附件

                wfProcessID.Text = base.ProcessID;
                wfWorkItemID.Text = base.WorkItemID;

                wfDrafter.Text = entity.Drafter;//程序发起人
                wfDrafterID.Text = entity.DrafterID;//程序发起人ID
                //wfDraftDate.Text = entity.DraftDate.ToString(ConstString.DateFormat.Normal);//程序发起时间

                wfDocTitle.Text = entity.DocumentTitle;//文件标题
                wfProgramID.Text = entity.ProgramFileID;//程序文件ID
                wfSerialID.Text = entity.SerialID;//流水号
                wfYear.Text = entity.Year;//年份

                //发送传阅时间
                wfCirculateDate.Text = entity.CirculateDate == DateTime.MinValue ? string.Empty : entity.CirculateDate.ToString();//质保发起传阅时间

                //分发节点
                wfSenderIDs.Text = entity.SenderIDs;//分发人（公办）
                wfSender.Text = entity.Senders;
                wfSendDate.Text = entity.SendDate == DateTime.MinValue ? string.Empty : entity.SendDate.ToString();

                //传阅
                hfCirculateUserID.Value = entity.CirculateSignUserID;//被传阅人ID
                txtCirculateUserName.Text = entity.CirculateSignUserName;//被传阅人姓名
                txtCirculateDeptName.Text = entity.CirculateSignDept;
                hfCirculateDeptID.Value = entity.CirculateSignDeptID;

                //分发
                hfSendDeptID.Value = entity.CirculateDeptID;
                hfSendUserID.Value = entity.CirculateID;
                txtSendDeptName.Text = entity.CirculateDeptName;
                txtSendUserName.Text = entity.CirculateName;
                txtSendComemnt.Text = entity.CirculateComment;

                ucBuMenHuiQian.UCHQList = entity.DeptSignList;//部门会签
                ucBuMenHuiQian.IsHistory = base.IsPreview;
                ucBuMenHuiQian.UCReceiveUserID = entity.ReceiveUserID;
                ucLDHuiQian.UCHQList = entity.LeaderSignList;//领导会签
                ucLDHuiQian.IsHistory = base.IsPreview;
                ucLDHuiQian.UCReceiveUserID = entity.ReceiveUserID;

                #region 当前会签意见
                if (base.IsPreview == false)
                {
                    //当前质保意见
                    if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG)
                    {
                        foreach (B_PF.QualityCheck qualityCheck in entity.QualityCheckList)
                        {
                            if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                            {
                                if (entity.QualityIDs == wfReceiveUserID.Text)
                                {
                                    CYiJian yiJian = new CYiJian();

                                    yiJian.Content = qualityCheck.Comment;
                                    yiJian.DealCondition = qualityCheck.DealCondition;
                                    yiJian.FinishTime = entity.QualityDate == DateTime.MinValue ? string.Empty : entity.QualityDate.ToString();
                                    yiJian.ID = base.IdentityID.ToString();
                                    yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_QG;
                                    yiJian.UserName = entity.QualityNames;
                                    yiJian.UserID = entity.QualityIDs;
                                    yiJian.DealTime = DateTime.Now.ToString();
                                    YiJianInfoList.Add(yiJian);
                                }
                            }
                            else
                            {
                                if (entity.QualityIDs == CurrentUserInfo.UserName)
                                {
                                    CYiJian yiJian = new CYiJian();

                                    yiJian.Content = qualityCheck.Comment;
                                    yiJian.DealCondition = qualityCheck.DealCondition;
                                    yiJian.FinishTime = entity.QualityDate == DateTime.MinValue ? string.Empty : entity.QualityDate.ToString();
                                    yiJian.ID = base.IdentityID.ToString();
                                    yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_QG;
                                    yiJian.UserName = entity.QualityNames;
                                    yiJian.UserID = entity.QualityIDs;
                                    yiJian.DealTime = DateTime.Now.ToString();
                                    YiJianInfoList.Add(yiJian);
                                }
                            }
                        }
                    }
                    //当前部门会签意见
                    if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN)
                    {
                        foreach (B_PF.DeptSign deptSign in entity.DeptSignList)
                        {
                            foreach (B_PF.DetailInfo detailInfo in deptSign.DetailInfoList)
                            {
                                if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                                {
                                    if (deptSign.ID == wfReceiveUserID.Text)
                                    {
                                        CYiJian yiJian = new CYiJian();

                                        yiJian.Content = detailInfo.Comment;
                                        yiJian.DealCondition = detailInfo.DealCondition;
                                        yiJian.FinishTime = deptSign.SubmitDate.ToString();
                                        yiJian.ID = deptSign.TBID;
                                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN;
                                        yiJian.UserName = deptSign.Name;
                                        yiJian.UserID = deptSign.ID;
                                        yiJian.DeptID = deptSign.DeptID;
                                        yiJian.DealTime = DateTime.Now.ToString();
                                        YiJianInfoList.Add(yiJian);
                                    }
                                }
                                else
                                {
                                    if (deptSign.ID == CurrentUserInfo.UserName)
                                    {
                                        CYiJian yiJian = new CYiJian();

                                        yiJian.Content = detailInfo.Comment;
                                        yiJian.DealCondition = detailInfo.DealCondition;
                                        yiJian.FinishTime = deptSign.SubmitDate.ToString();
                                        yiJian.ID = deptSign.TBID;
                                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN;
                                        yiJian.UserName = deptSign.Name;
                                        yiJian.UserID = deptSign.ID;
                                        yiJian.DeptID = deptSign.DeptID;
                                        yiJian.DealTime = DateTime.Now.ToString();
                                        YiJianInfoList.Add(yiJian);
                                    }
                                }
                            }
                        }
                    }//部门会签保存
                    ucBuMenHuiQian.UCHQList = entity.DeptSignList;//部门会签

                    //部门会签
                    if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN)
                    {
                        if (!string.IsNullOrEmpty(entity.ChildProcessID))
                        {
                            btnAssign.Visible = false;
                            ucSelectAssign.Visible = false;
                            lblAssign.Text = "已交办：";
                        }//ChildProcessID不为空，存在子流程，不可再次交办
                    }

                    //当前领导会签意见
                    if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN)
                    {
                        foreach (B_PF.LeaderSign leaderSign in entity.LeaderSignList)
                        {
                            foreach (B_PF.DetailInfo detailInfo in leaderSign.DetailInfoList)
                            {
                                if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                                {
                                    if (leaderSign.ID == wfReceiveUserID.Text)
                                    {
                                        CYiJian yiJian = new CYiJian();

                                        yiJian.Content = detailInfo.Comment;
                                        yiJian.DealCondition = detailInfo.DealCondition;
                                        yiJian.FinishTime = leaderSign.Date.ToString();
                                        yiJian.ID = leaderSign.TBID;
                                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN;
                                        yiJian.UserName = leaderSign.Name;
                                        yiJian.UserID = leaderSign.ID;
                                        yiJian.DealTime = DateTime.Now.ToString();
                                        YiJianInfoList.Add(yiJian);
                                    }
                                }
                                else
                                {
                                    if (leaderSign.ID == CurrentUserInfo.UserName)
                                    {
                                        CYiJian yiJian = new CYiJian();

                                        yiJian.Content = detailInfo.Comment;
                                        yiJian.DealCondition = detailInfo.DealCondition;
                                        yiJian.FinishTime = leaderSign.Date.ToString();
                                        yiJian.ID = leaderSign.TBID;
                                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN;
                                        yiJian.UserName = leaderSign.Name;
                                        yiJian.UserID = leaderSign.ID;
                                        yiJian.DealTime = DateTime.Now.ToString();
                                        YiJianInfoList.Add(yiJian);
                                    }
                                }
                            }
                        }
                    }//领导会签保存
                    ucLDHuiQian.UCHQList = entity.LeaderSignList;//领导会签

                    //当前批准意见
                    if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.ProgramFile.STEP_APPROVE)
                    {
                        foreach (B_PF.PiZhun pizhun in entity.PiZhunList)
                        {
                            if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                            {
                                if (entity.ApproverID == wfReceiveUserID.Text)
                                {
                                    CYiJian yiJian = new CYiJian();
                                    yiJian.Content = pizhun.Comment;
                                    yiJian.DealCondition = pizhun.DealCondition;
                                    yiJian.ID = base.IdentityID.ToString();
                                    yiJian.UserName = pizhun.Name;
                                    yiJian.FinishTime = pizhun.SubmitDate == DateTime.MinValue ? string.Empty : pizhun.SubmitDate.ToString(); ;
                                    yiJian.UserID = wfReceiveUserID.Text;
                                    yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_APPROVE;
                                    yiJian.DealTime = DateTime.Now.ToString();
                                    YiJianInfoList.Add(yiJian);
                                }
                            }
                            else
                            {
                                if (entity.ApproverID == CurrentUserInfo.UserName)
                                {
                                    CYiJian yiJian = new CYiJian();
                                    yiJian.Content = pizhun.Comment;
                                    yiJian.DealCondition = pizhun.DealCondition;
                                    yiJian.ID = base.IdentityID.ToString();
                                    yiJian.UserName = pizhun.Name;
                                    yiJian.FinishTime = pizhun.SubmitDate == DateTime.MinValue ? string.Empty : pizhun.SubmitDate.ToString(); ;
                                    yiJian.UserID = CurrentUserInfo.UserName;
                                    yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_APPROVE;
                                    yiJian.DealTime = DateTime.Now.ToString();
                                    YiJianInfoList.Add(yiJian);
                                }
                            }
                        }
                    }
                    if (YiJianInfoList.Count > 0)
                    {
                        trYiJianHead.Visible = true;
                        rptComment.DataSource = YiJianInfoList;
                        rptComment.DataBind();
                    }
                }
                else
                {
                    if (entity.CommentList.Count > 0 && (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG ||
                        base.StepName == ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN ||
                        base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN ||
                        base.StepName == ProcessConstString.StepName.ProgramFile.STEP_APPROVE))
                    {
                        rptComment.ItemDataBound += new RepeaterItemEventHandler(rptComment_ItemDataBound);
                        trYiJianHead.Visible = true;
                        btnAdd.Visible = false;
                        rptComment.DataSource = entity.CommentList;
                        rptComment.DataBind();
                    }
                }//查看多条历史意见

                #endregion

                #region 多条处理意见
                //编制节点 处理部门会签、领导会签、质保意见落实
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE)
                {
                    //部门会签
                    List<B_PF.YiJian> deptYiJianList = new List<B_PF.YiJian>();
                    foreach (B_PF.DeptSign deptSign in entity.DeptSignList)
                    {
                        foreach (B_PF.DetailInfo detailInfo in deptSign.DetailInfoList)
                        {
                            B_PF.YiJian yiJian = new B_PF.YiJian();

                            yiJian.Content = detailInfo.Comment;
                            yiJian.DealCondition = detailInfo.DealCondition;
                            yiJian.FinishTime = deptSign.SubmitDate.ToString();//renjinquan 修改
                            yiJian.ID = deptSign.TBID;
                            yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN;
                            yiJian.UserName = deptSign.Name;
                            yiJian.UserID = deptSign.ID;
                            yiJian.DeptID = deptSign.DeptID;
                            yiJian.DeptName = deptSign.DeptName;
                            deptYiJianList.Add(yiJian);
                        }
                    }
                    //当前部门会签意见信息
                    if (deptYiJianList.Count > 0)
                    {
                        pnlDeptComment.Visible = true;
                        rptDept.DataSource = deptYiJianList;
                        rptDept.DataBind();
                    }
                    //领导会签
                    List<CYiJian> leadYiJianList = new List<CYiJian>();
                    foreach (B_PF.LeaderSign leaderSign in entity.LeaderSignList)
                    {
                        foreach (B_PF.DetailInfo detailInfo in leaderSign.DetailInfoList)
                        {
                            CYiJian yiJian = new CYiJian();

                            yiJian.Content = detailInfo.Comment;
                            yiJian.DealCondition = detailInfo.DealCondition;
                            yiJian.FinishTime = leaderSign.Date.ToString();//renjinquan修改
                            yiJian.ID = leaderSign.TBID;
                            yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN;
                            yiJian.UserName = leaderSign.Name;
                            yiJian.UserID = leaderSign.ID;
                            leadYiJianList.Add(yiJian);
                        }
                    }
                    //当前领导会签意见信息
                    if (leadYiJianList.Count > 0)
                    {
                        pnlLeaderComment.Visible = true;
                        rptLeader.DataSource = leadYiJianList;
                        rptLeader.DataBind();
                    }
                    //质保意见
                    List<CYiJian> qgYiJianList = new List<CYiJian>();
                    foreach (B_PF.QualityCheck qg in entity.QualityCheckList)
                    {
                        CYiJian yiJian = new CYiJian();
                        yiJian.Content = qg.Comment;
                        yiJian.DealCondition = qg.DealCondition;
                        yiJian.FinishTime = entity.QualityDate.ToString();//renjinquan修改
                        yiJian.ID = qg.TBID;
                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_QG;
                        yiJian.UserName = qg.Name;
                        yiJian.UserID = entity.QualityIDs;
                        qgYiJianList.Add(yiJian);
                    }
                    if (qgYiJianList.Count > 0)
                    {
                        pnlQGComment.Visible = true;
                        rptQG.DataSource = qgYiJianList;
                        rptQG.DataBind();
                    }
                }
                //编制、质保节点 处理批准意见落实
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE ||
                    base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG)
                {
                    //批准意见
                    List<CYiJian> PiZhunList = new List<CYiJian>();
                    foreach (B_PF.PiZhun pizhun in entity.PiZhunList)
                    {
                        CYiJian yiJian = new CYiJian();
                        yiJian.UserID = ""; //CurrentUserInfo.UserName;
                        yiJian.UserName = pizhun.Name;
                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_APPROVE;
                        yiJian.Content = pizhun.Comment;
                        yiJian.DealCondition = pizhun.DealCondition;
                        yiJian.FinishTime = pizhun.DealDate.ToString();
                        yiJian.ID = pizhun.TBID;
                        PiZhunList.Add(yiJian);
                    }
                    if (PiZhunList.Count > 0)
                    {
                        this.pnlApproveComment.Visible = true;
                        this.rptPiZhun.DataSource = PiZhunList;
                        this.rptPiZhun.DataBind();
                    }
                }
                #endregion

                //子流程+
                txtAssistInfo.Text = entity.AssistContent;
                wfChildProcessID.Text = entity.ChildProcessID;
                txtAssignMember.Text = entity.AssignedUserName;
                wfAssignUserName.Text = entity.AssignedUserName;
                wfAssignUserID.Text = entity.AssignedUserID;
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_ASSIST_SIGN)
                {
                    wfParentTBID.Text = entity.ParentTBID;
                }//协助会签
                else
                {
                    wfParentTBID.Text = base.IdentityID.ToString();
                }

                //批准后显示label形式的批准人
                if (entity.ApproveDate != DateTime.MinValue)
                {
                    //this.ddlApprove.Visible = false;
                    //this.needApprove.Visible = true;
                    //this.needApprove.Text = entity.ApproveName;
                }

                //审核后显示label形式的审核人
                if (entity.AuditDate != DateTime.MinValue)
                {
                    this.ddlAudit.Visible = false;
                    this.needAudit.Visible = true;
                    this.needAudit.Text = entity.AuditName;
                }

                //校核后显示label形式的校核人
                if (entity.CheckDate != DateTime.MinValue)
                {
                    this.txtCheckName.Visible = false;
                    this.needCheck.Visible = true;
                    this.needCheck.Text = entity.CheckName;
                }



            }
        }

        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="IsSave"></param>
        /// <returns></returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_PF entity = base.EntityData != null ? base.EntityData as B_PF : new B_PF();
            entity.IsProgramCompanCheck = this.cbIsPrint.Checked;

            entity.SendDeptID = wfChiefDept.Text;//主办部门ID
            entity.DocumentTitle = txtName.Text;//程序文件名称-DocumentTitle
            entity.DocumentNo = txtCode.Text;//程序编码
            entity.ProgramCode = txtCode.Text;//程序编码
            entity.Edition = txtEdition.Text;//程序版次
            entity.TextPageSum = txtPages.Text.Trim();//总页数
            entity.ApplyStyle = lblApplyStyle.Text;//申请类型             
            entity.WriteExplain = SysString.InputText(txtWriteExplain.Text);//编制修订说明

            entity.Drafter = wfDrafter.Text;//程序发起人
            entity.DrafterID = wfDrafterID.Text;//程序发起人ID

            //编制
            entity.WriteID = wfWriteID.Text.ToString();//ddlWrite.SelectedValue.ToString();
            entity.WriteName = lblWrite.Text.ToString();
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE)
            {
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    entity.DraftDate = entity.DraftDate == DateTime.MinValue ? DateTime.Now : entity.DraftDate;//程序发起时间
                    entity.FirstDraftDate = entity.FirstDraftDate == DateTime.MinValue ? entity.DraftDate : entity.FirstDraftDate;
                    //将会签退回标识初始化（置为“False”）
                    entity.IsSignReject = ConstString.Miscellaneous.STATUS_FALSE;
                    entity.WriteDate = lblWriteDate.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(lblWriteDate.ToolTip);
                }
                //编写节点处理落实情况
                if (wfSort.Text == ConstString.ProgramFile.PROGRAM_SORT_WORK)
                {
                    if (rdolstSignStyle.SelectedIndex == 0)
                    {
                        entity.DeptSignList = rptDept.Items.Count > 0 ? B_PF.GetDeptSignList(rptDept, ucBuMenHuiQian.UCGetHQList()) : ucBuMenHuiQian.UCGetHQList();//选择部门会签
                        entity.LeaderSignList.Clear();
                    }//选择部门会签
                    else
                    {
                        entity.DeptSignList.Clear();
                        entity.LeaderSignList = rptLeader.Items.Count > 0 ? B_PF.GetLeaderSignList(rptLeader, ucLDHuiQian.UCGetHQList()) : ucLDHuiQian.UCGetHQList(); ;//选择领导会签
                    }//选择领导会签
                }//工作程序
                else
                {
                    entity.DeptSignList = rptDept.Items.Count > 0 ? B_PF.GetDeptSignList(rptDept, ucBuMenHuiQian.UCGetHQList()) : ucBuMenHuiQian.UCGetHQList();//部门会签
                    entity.LeaderSignList = rptLeader.Items.Count > 0 ? B_PF.GetLeaderSignList(rptLeader, ucLDHuiQian.UCGetHQList()) : ucLDHuiQian.UCGetHQList();//领导会签
                    entity.QualityCheckList = B_PF.GetQualityCheckList(rptQG);//质保审查
                } //(部门级）管理程序
                entity.PiZhunList = B_PF.GetPiZhunList(rptPiZhun);
            }

            //校核
            entity.CheckID = this.wfCheckID.Text;
            entity.CheckName = this.txtCheckName.Text;
            //校核时间  任金权 改  使用第一次时间
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_CHECK)
            {
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    entity.CheckDate = DateTime.Now; //string.IsNullOrEmpty(this.lblCheckDate.Text) ? DateTime.Now : Convert.ToDateTime(lblCheckDate.Text.ToString());
                    entity.CheckerIsAgree = base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY ? ConstString.ProgramFile.PROGRAM_REJECT : ConstString.ProgramFile.PROGRAM_AGREE;
                }//提交
                else
                {
                    //entity.CheckDate = DateTime.MinValue;
                    entity.CheckerIsAgree = string.Empty;
                }//保存
                entity.CheckComment = SysString.InputText(txtInfo.Text.Trim());
            }

            //审核
            entity.AuditID = ddlAudit.SelectedValue.ToString();
            entity.AuditName = ddlAudit.SelectedItem.Text.ToString();
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_AUDIT)
            {
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    entity.AuditDate = DateTime.Now; //string.IsNullOrEmpty(this.lblAuditDate.Text) ? DateTime.Now : Convert.ToDateTime(lblAuditDate.Text);
                    entity.AuditorIsAgree = base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY ? ConstString.ProgramFile.PROGRAM_REJECT : ConstString.ProgramFile.PROGRAM_AGREE;
                }//提交
                else
                {
                    //entity.AuditDate = DateTime.MinValue;
                    entity.AuditorIsAgree = string.Empty;
                }//保存
                entity.AuditComment = SysString.InputText(txtInfo.Text.Trim());
            }

            //部门会签
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN && base.SubAction != ProcessConstString.SubmitAction.ProgramFile.ACTION_ASSIGN)
            {
                M_ProgramFile.DeptSign deptSign = new M_ProgramFile.DeptSign();
                string strAgreeOld = "";
                if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                {
                    foreach (M_ProgramFile.DeptSign signer in ucBuMenHuiQian.UCGetHQList())
                    {
                        if (signer.ID == wfReceiveUserID.Text && signer.IsExclude == false)
                        {
                            deptSign = signer;
                            strAgreeOld = deptSign.IsAgree;//记录上次的意见  任金权  
                            break;
                        }
                    }
                }
                else
                {
                    foreach (M_ProgramFile.DeptSign signer in ucBuMenHuiQian.UCGetHQList())
                    {
                        if (signer.ID == CurrentUserInfo.UserName && signer.IsExclude == false)
                        {
                            deptSign = signer;
                            strAgreeOld = deptSign.IsAgree;//记录上次的意见  任金权  
                            break;
                        }
                    }
                }

                //非保存操作
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)//任金权修改会签时间
                {
                    deptSign.IsAgree = YiJianInfoList.Count == 0 ? ConstString.ProgramFile.PROGRAM_AGREE : ConstString.ProgramFile.PROGRAM_REJECT;
                    if (deptSign.IsAgree == ConstString.ProgramFile.PROGRAM_REJECT || deptSign.SubmitDate == DateTime.MinValue || strAgreeOld == ConstString.ProgramFile.PROGRAM_REJECT)
                    {
                        deptSign.SubmitDate = DateTime.Now;
                    }
                    //判断是否会签退回（如果为同意，则需判断当前会签结果状态，否则为拒绝）
                    entity.IsSignReject = entity.IsSignReject == ConstString.Miscellaneous.STATUS_FALSE ? deptSign.IsAgree == ConstString.ProgramFile.PROGRAM_AGREE ?
                                            ConstString.Miscellaneous.STATUS_FALSE : ConstString.Miscellaneous.STATUS_TRUE : ConstString.Miscellaneous.STATUS_TRUE;

                    //    //修改标识：M_201004013 
                    //    //修 改 者：黄琦
                    //    //修改描述：兼容老版本，IsSignReject为空的问题
                    //    if (entity.IsSignReject == ConstString.Miscellaneous.STATUS_FALSE)
                    //    {
                    //        if (deptSign.IsAgree == ConstString.ProgramFile.PROGRAM_AGREE)
                    //        {
                    //            entity.IsSignReject = ConstString.Miscellaneous.STATUS_FALSE;
                    //        }
                    //        else
                    //        {
                    //            entity.IsSignReject = ConstString.Miscellaneous.STATUS_TRUE;
                    //        }
                    //    }
                    //    else if (entity.IsSignReject == ConstString.Miscellaneous.STATUS_TRUE)
                    //    {
                    //        entity.IsSignReject = ConstString.Miscellaneous.STATUS_TRUE;
                    //    }
                    //    else
                    //    {
                    //        entity.IsSignReject = ConstString.Miscellaneous.STATUS_FALSE;
                    //        foreach (M_ProgramFile.DeptSign item in entity.DeptSignList)
                    //        {
                    //            if (item.IsAgree == ConstString.ProgramFile.PROGRAM_REJECT)
                    //            {
                    //                entity.IsSignReject = ConstString.Miscellaneous.STATUS_TRUE;
                    //                break;
                    //            }
                    //        }
                    //    }
                }

                List<M_ProgramFile.DetailInfo> detailInfoList = new List<M_ProgramFile.DetailInfo>();
                string strComment = string.Empty;//单条会签意见，用于表单列表显示

                foreach (RepeaterItem item in this.rptComment.Items)
                {
                    M_ProgramFile.DetailInfo detailInfo = new M_ProgramFile.DetailInfo();
                    Label lblContent = item.FindControl("lblContent") as Label;
                    detailInfo.Comment = lblContent.Text;

                    strComment = lblContent.Text;
                    detailInfoList.Add(detailInfo);
                }

                deptSign.DealCondition = string.Empty;//清空处理情况
                deptSign.DealDate = DateTime.MinValue;//清空处理日期
                deptSign.TBID = base.IdentityID.ToString();
                deptSign.Comment = SysString.InputText(strComment);
                deptSign.DetailInfoList = detailInfoList;//会签信息集合

                entity.DeptSignList = B_PF.SetDeptSignList(deptSign, ucBuMenHuiQian.UCGetHQList());
            }
            //领导会签
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN)
            {
                M_ProgramFile.LeaderSign leaderSign = new M_ProgramFile.LeaderSign();
                string strAgreeOld = "";//记录上次的意见  任金权  
                if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                {
                    foreach (M_ProgramFile.LeaderSign signer in ucLDHuiQian.UCGetHQList())
                    {
                        if (signer.ID == wfReceiveUserID.Text)
                        {
                            leaderSign = signer;
                            strAgreeOld = leaderSign.IsAgree;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (M_ProgramFile.LeaderSign signer in ucLDHuiQian.UCGetHQList())
                    {
                        if (signer.ID == CurrentUserInfo.UserName)
                        {
                            leaderSign = signer;
                            strAgreeOld = leaderSign.IsAgree;
                            break;
                        }
                    }
                }
                //非保存操作
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)//任金权修改会签时间
                {
                    leaderSign.IsAgree = YiJianInfoList.Count == 0 ? ConstString.ProgramFile.PROGRAM_AGREE : ConstString.ProgramFile.PROGRAM_REJECT;
                    if (leaderSign.IsAgree == ConstString.ProgramFile.PROGRAM_REJECT || leaderSign.Date == DateTime.MinValue || strAgreeOld == ConstString.ProgramFile.PROGRAM_REJECT)
                    {
                        leaderSign.Date = DateTime.Now;
                    }//过滤多次连续同意（取第一次同意时提交的时间）
                    //判断是否会签退回（如果为同意，则需判断当前会签结果状态，否则为拒绝）
                    entity.IsSignReject = entity.IsSignReject == ConstString.Miscellaneous.STATUS_FALSE ? leaderSign.IsAgree == ConstString.ProgramFile.PROGRAM_AGREE ?
                                                ConstString.Miscellaneous.STATUS_FALSE : ConstString.Miscellaneous.STATUS_TRUE : ConstString.Miscellaneous.STATUS_TRUE;
                }

                List<M_ProgramFile.DetailInfo> detailInfoList = new List<M_ProgramFile.DetailInfo>();
                string strComment = string.Empty;//单条会签意见，用于表单列表显示

                foreach (RepeaterItem item in this.rptComment.Items)
                {
                    B_PF.DetailInfo detailInfo = new B_PF.DetailInfo();
                    Label lblContent = item.FindControl("lblContent") as Label;
                    detailInfo.Comment = lblContent.Text;

                    strComment = lblContent.Text;
                    detailInfoList.Add(detailInfo);
                }

                leaderSign.DealCondition = string.Empty;//清空处理情况
                leaderSign.DealDate = DateTime.MinValue;//清空处理日期
                leaderSign.TBID = base.IdentityID.ToString();
                leaderSign.Comment = SysString.InputText(strComment);
                leaderSign.DetailInfoList = detailInfoList;//会签信息

                entity.LeaderSignList = B_PF.SetLeaderSignList(leaderSign, ucLDHuiQian.UCGetHQList());
            }

            //批准
            entity.ApproverID = ddlApprove.SelectedValue.ToString();
            entity.ApproveName = ddlApprove.SelectedItem.Text.ToString();
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_APPROVE)
            {
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    entity.ApproverIsAgree = YiJianInfoList.Count == 0 ? ConstString.ProgramFile.PROGRAM_AGREE : ConstString.ProgramFile.PROGRAM_REJECT;
                    entity.ApproveDate = DateTime.Now; //string.IsNullOrEmpty(lblApproveDate.Text) ? DateTime.Now : Convert.ToDateTime(lblApproveDate.Text.ToString());
                    string[] sendMember = OAUser.GetUserByRoleName(ConstString.RoleName.PROGRAM_ADMIN);
                    entity.SenderIDs = sendMember[0].ToString();//分发人（公办）
                    entity.Senders = sendMember[1].ToString();
                }
                else
                {
                    entity.ApproverIsAgree = string.Empty;
                }
                //批准

                entity.PiZhunList.Clear();
                foreach (CYiJian objYiJian in YiJianInfoList)
                {
                    M_ProgramFile.PiZhun approveInfo = new M_ProgramFile.PiZhun();
                    approveInfo.TBID = objYiJian.ID;
                    approveInfo.Name = objYiJian.UserName;
                    approveInfo.Comment = objYiJian.Content;
                    approveInfo.DealCondition = objYiJian.DealCondition;
                    approveInfo.SubmitDate = DateTime.Now;
                    entity.PiZhunList.Add(approveInfo);
                }
                //表单显示的批准意见信息
                if (YiJianInfoList.Count == 0)
                {
                    entity.ApproveComment = string.Empty;
                }
                else
                {
                    entity.ApproveComment = YiJianInfoList[YiJianInfoList.Count - 1].Content;
                }
            }
            //分发节点
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_SEND)
            {
                entity.SendDate = DateTime.Now; //wfSendDate.Text == string.Empty ? DateTime.MinValue : Convert.ToDateTime(wfSendDate.Text);
            }

            entity.ProgramSort = wfSort.Text;//程序分类
            entity.ProgramFileID = wfProgramID.Text;//程序文件ID
            entity.SerialID = wfSerialID.Text;//流水号
            entity.Year = wfYear.Text;//年份

            //质保审查
            entity.QualityIDs = wfQualityIDs.Text;//质保审查人ID
            entity.QualityNames = lblQG.ToolTip;//质保审查人
            //entity.QualityDate = lblQGDate.Text == string.Empty ? DateTime.MinValue : Convert.ToDateTime(lblQGDate.ToolTip.ToString());
            //entity.QualityComment = lblQGComment.ToolTip;
            //发起传阅时间
            entity.CirculateDate = wfCirculateDate.Text == string.Empty ? DateTime.MinValue : Convert.ToDateTime(wfCirculateDate.Text);
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG)
            {
                if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    entity.QualityIsAgree = YiJianInfoList.Count == 0 ? ConstString.ProgramFile.PROGRAM_AGREE : ConstString.ProgramFile.PROGRAM_REJECT;
                    entity.QualityDate = lblQGDate.ToolTip == string.Empty ? DateTime.Now : Convert.ToDateTime(lblQGDate.ToolTip.ToString());
                    string[] sendMember = OAUser.GetUserByRoleName(ConstString.RoleName.PROGRAM_ADMIN);
                    entity.SenderIDs = sendMember[0].ToString();//分发人（公办）
                    entity.Senders = sendMember[1].ToString();
                }
                else
                {
                    entity.QualityIsAgree = string.Empty;
                }
                //表单显示的质保意见信息
                if (YiJianInfoList.Count == 0)
                {
                    entity.QualityComment = string.Empty;
                }
                else
                {
                    entity.QualityComment = YiJianInfoList[YiJianInfoList.Count - 1].Content;
                }
                entity.QualityCheckList.Clear();
                if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                {
                    foreach (CYiJian yiJian in YiJianInfoList)
                    {
                        B_PF.QualityCheck qg = new B_PF.QualityCheck();
                        //Label lblContent = itm.FindControl("lblContent") as Label;
                        qg.Name = wfReceiveUserName.Text;
                        qg.TBID = base.IdentityID.ToString();
                        qg.Comment = yiJian.Content;
                        entity.QualityCheckList.Add(qg);
                    }
                }
                else
                {
                    foreach (CYiJian yiJian in YiJianInfoList)
                    {
                        B_PF.QualityCheck qg = new B_PF.QualityCheck();
                        //Label lblContent = itm.FindControl("lblContent") as Label;
                        qg.Name = CurrentUserInfo.DisplayName;
                        qg.TBID = base.IdentityID.ToString();
                        qg.Comment = yiJian.Content;
                        entity.QualityCheckList.Add(qg);
                    }
                }
                entity.PiZhunList = B_PF.GetPiZhunList(rptPiZhun);

                entity.DeptSignList = ucBuMenHuiQian.UCGetHQList();//部门会签
                entity.LeaderSignList = ucLDHuiQian.UCGetHQList();//领导会签
            }

            //传阅
            entity.CirculateSignUserID = hfCirculateUserID.Value;//被传阅人ID
            entity.CirculateSignUserName = txtCirculateUserName.Text;//被传阅人姓名
            entity.CirculateSignDept = txtCirculateDeptName.Text;//被传阅部门
            entity.CirculateSignDeptID = hfCirculateDeptID.Value;//被传阅部门ID

            //分发
            entity.CirculateDeptID = hfSendDeptID.Value;
            entity.CirculateID = hfSendUserID.Value;
            entity.CirculateDeptName = txtSendDeptName.Text;
            entity.CirculateName = txtSendUserName.Text;
            entity.CirculateComment = txtSendComemnt.Text;

            if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
            {
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG ||
                    base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN ||
                    base.StepName == ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN ||
                    base.StepName == ProcessConstString.StepName.ProgramFile.STEP_APPROVE)
                {
                    entity.CommentList.Clear();
                    foreach (CYiJian objYj in YiJianInfoList)
                    {
                        objYj.FinishTime = DateTime.Now.ToString();
                    }
                    entity.CommentList = YiJianInfoList;
                }
                else if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_CHECK ||
                         base.StepName == ProcessConstString.StepName.ProgramFile.STEP_AUDIT)
                {
                    //意见保存
                    CYiJian objYj = new CYiJian();
                    objYj.ID = base.IdentityID.ToString();
                    if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                    {
                        objYj.UserID = wfReceiveUserID.Text;
                        objYj.UserName = wfReceiveUserName.Text;
                    }
                    else
                    {
                        objYj.UserID = CurrentUserInfo.UserName;
                        objYj.UserName = CurrentUserInfo.DisplayName;
                    }
                    objYj.ViewName = base.StepName;//视图名称（当前流程步骤）
                    objYj.FinishTime = DateTime.Now.ToString();
                    objYj.Content = SysString.InputText(txtInfo.Text.Trim());
                    entity.CommentList.Clear();
                    entity.CommentList.Add(objYj);
                }//校核、审核
            }//非保存操作

            //交办相关
            entity.ParentTBID = wfParentTBID.Text;
            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_ASSIST_SIGN)
            {
                entity.AssistContent = txtAssistInfo.Text;
            }//协助部门会签步骤
            if (base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_ASSIGN)
            {
                entity.AssistContent = string.Empty;
            }

            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE ||
             base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG ||
             base.StepName == ProcessConstString.StepName.ProgramFile.STEP_APPROVE)
            {
                entity.AssistContent = string.Empty;
                entity.AssignedUserID = string.Empty;
                entity.AssignedUserName = string.Empty;
            }
            else
            {
                entity.AssignedUserName = string.IsNullOrEmpty(txtAssignMember.Text) ? wfAssignUserName.Text : txtAssignMember.Text;
                entity.AssignedUserID = wfAssignUserID.Text;
            }
            entity.ChildProcessID = wfChildProcessID.Text;
            if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT &&
                (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE ||
                 base.StepName == ProcessConstString.StepName.ProgramFile.STEP_AUDIT ||
                 base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG ||
                 base.StepName == ProcessConstString.StepName.ProgramFile.STEP_APPROVE))
            {
                entity.TimesFlag = (int.Parse(string.IsNullOrEmpty(this.wfTimesFlag.Text) ? "1" : this.wfTimesFlag.Text) + 1).ToString();//次数标示
            }
            else
            {
                entity.TimesFlag = wfTimesFlag.Text;//次数标示
            }
            entity.FileList = ucFileControl.UCDataList;//附件信息

            return entity;
        }
        #endregion

        #region 表单事件
        /// <summary>
        /// 绑定表单事件
        /// </summary>
        protected void SubmitEvents()
        {
            EventHandler SubmitHandler = new EventHandler(SubmitBtn_Click);
            //保存
            this.btnSave.Click += SubmitHandler;
            this.btnSave2.Click += SubmitHandler;
            this.btnSave3.Click += SubmitHandler;
            //撤销
            this.btnCancel.Click += SubmitHandler;
            //退回
            this.btnBack.Click += SubmitHandler;
            this.btnBack2.Click += SubmitHandler;
            this.btnQGBack.Click += SubmitHandler;
            //提交
            this.btnSubmit.Click += SubmitHandler;
            //提交（多条意见）
            this.btnSubmits.Click += SubmitHandler;
            //提交校核
            this.btnSubmitCheck.Click += SubmitHandler;
            //部门会签
            this.btnDeptSign.Click += SubmitHandler;
            this.btnDeptSign2.Click += SubmitHandler;
            //领导会签
            this.btnLeaderSign.Click += SubmitHandler;
            this.btnLeaderSign2.Click += SubmitHandler;
            //质保 提交批准
            this.btnQGApprove.Click += SubmitHandler;
            //质保 提交分发
            this.btnSubmitFenFa.Click += SubmitHandler;
            //审核 提交批准
            this.btnAuditApprove.Click += SubmitHandler;
            //传阅
            this.btnQGCirculate.Click += SubmitHandler;
            this.btnAuditCirculate.Click += SubmitHandler;
            //分发节点 分发完成
            this.btnSend.Click += SubmitHandler;
            //归档
            this.btnArchive.Click += SubmitHandler;
            //添加意见 添加
            this.btnAdd.Click += SubmitHandler;
            //添加意见 取消
            this.btnCancel.Click += SubmitHandler;
            //添加意见 确定
            this.btnConfirm.Click += SubmitHandler;
            //协助会签 保存
            this.btnAssignSave.Click += SubmitHandler;
            //协助会签 完成
            this.btnFinish.Click += SubmitHandler;
            //协助会签 交办
            this.btnAssign.Click += SubmitHandler;
        }

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

                string strErrMsg = string.Empty;
                string strMessage = string.Empty;
                string strErrMessage = string.Empty;

                #region 传阅
                if (strActionName == ProcessConstString.SubmitAction.ProgramFile.ACTION_CIRCULATE)
                {
                    if (string.IsNullOrEmpty(hfCirculateUserID.Value) && string.IsNullOrEmpty(hfCirculateDeptID.Value))
                    {
                        JScript.Alert(ConstString.PromptInfo.ACTION_CHECK_CIRCULATE, true);
                        return;
                    }
                    try
                    {
                        base.Circulate(hfCirculateDeptID.Value, "0", string.Empty, hfCirculateUserID.Value, "1", true, string.Empty, false);

                        string strDay = OAConfig.GetConfig("传阅有效期", "天数");
                        int day = string.IsNullOrEmpty(strDay) ? -1 : int.Parse(strDay);
                        wfCirculateDate.Text = DateTime.Now.AddDays(day).ToString();
                        if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG)
                        {
                            QualityCheck();
                        }//质保审查
                        else if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_AUDIT)
                        {
                            AuditCheck();
                        }//审核
                        JScript.Alert(ConstString.PromptInfo.ACTION_CIRCULATE_SUC, true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        JScript.Alert(ex.ToString(), true);
                        return;
                    }
                }
                #endregion

                #region 添加多条意见
                //添加意见按钮事件
                if (strActionName == ProcessConstString.SubmitAction.ProgramFile.ACTION_ADD_COMMENT)
                {
                    this.pnlComment.Visible = true;
                    this.btnCancel.Visible = true;
                    this.btnConfirm.Visible = true;
                    this.btnAdd.Visible = false;
                    return;
                }
                //确定按钮事件
                if (strActionName == ProcessConstString.SubmitAction.ProgramFile.ACTION_CONFIRM)
                {
                    if (string.IsNullOrEmpty(txtInfo2.Text.Trim()))
                    {
                        JScript.Alert(ConstString.PromptInfo.ACTION_CHECK_ADD_COMMENT, true);
                        return;
                    }
                    if (txtInfo2.Text.Trim().Length > 500)
                    {
                        JScript.Alert(ConstString.PromptInfo.ACTION_CHECK_CONTENT_LEN500, true);
                        return;
                    }
                    if (string.IsNullOrEmpty(hfEditIndex.Value))
                    {
                        CYiJian objYj = new CYiJian();

                        objYj.Content = SysString.InputText(txtInfo2.Text.Trim());
                        objYj.FinishTime = DateTime.Now.ToString();
                        objYj.ID = base.IdentityID.ToString();
                        objYj.ViewName = base.StepName;

                        /*开始    流程代理         */
                        if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                        {
                            objYj.UserName = wfReceiveUserName.Text;
                            objYj.UserID = wfReceiveUserID.Text;
                        }
                        else
                        {
                            objYj.UserName = CurrentUserInfo.DisplayName;
                            objYj.UserID = CurrentUserInfo.UserName;
                        }
                        /*结束              */
                        objYj.DeptID = base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN ? B_PF.GetDeptIDByUserID(ucBuMenHuiQian.UCGetHQList(), objYj.UserID) : string.Empty;

                        YiJianInfoList.Add(objYj);
                        txtInfo2.Text = string.Empty;
                        trYiJianHead.Visible = YiJianInfoList.Count > 0 ? true : false; ;

                        rptComment.DataSource = YiJianInfoList;
                        rptComment.DataBind();
                    }//添加意见
                    else
                    {
                        int index = int.Parse(hfEditIndex.Value);
                        Label lblContent = rptComment.Items[index].FindControl("lblContent") as Label;
                        lblContent.Text = SysString.InputText(txtInfo2.Text);
                        YiJianInfoList[index].Content = lblContent.Text;

                        this.pnlComment.Visible = false;
                        this.btnCancel.Visible = false;
                        btnConfirm.Visible = false;
                        this.btnAdd.Visible = true;
                        txtInfo2.Text = string.Empty;
                        hfEditIndex.Value = string.Empty;
                    }//修改意见
                    return;
                }
                //取消按钮事件
                if (strActionName == ProcessConstString.SubmitAction.ProgramFile.ACTION_CANCLE)
                {
                    this.pnlComment.Visible = false;
                    this.btnCancel.Visible = false;
                    this.btnConfirm.Visible = false;
                    this.btnAdd.Visible = true;
                    txtInfo2.Text = string.Empty;
                    hfEditIndex.Value = string.Empty;
                    return;
                }
                #endregion

                #region 处理落实情况
                //编写节点 处理 质保、部门会签、领导会签、批准意见
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE && pnlDealSign.Visible == true)
                {
                    B_PF bllProFile = new B_PF();

                    List<M_ProgramFile.DeptSign> deptSignList = B_PF.GetDeptSignList(rptDept, ucBuMenHuiQian.UCGetHQList());//部门会签意见
                    List<M_ProgramFile.LeaderSign> leaderSignList = B_PF.GetLeaderSignList(rptLeader, ucLDHuiQian.UCGetHQList());//领导会签
                    List<M_ProgramFile.QualityCheck> qualityCheckList = B_PF.GetQualityCheckList(rptQG);//质保审查
                    List<M_ProgramFile.PiZhun> piZhunList = B_PF.GetPiZhunList(rptPiZhun);//批准意见
                    List<B_PF> entityList = B_PF.GetpfEntityList(deptSignList, leaderSignList, qualityCheckList, piZhunList);
                    if (!bllProFile.EnTransSave(entityList))
                    {
                        JScript.Alert(ConstString.PromptInfo.ACTION_SUBMIT_DEAL_FAIL, true);
                        return;
                    }//更新当前提交的意见落实情况

                }
                //质保审查节点 处理批准意见
                if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_QG)
                {
                    List<M_ProgramFile.PiZhun> piZhunList = B_PF.GetPiZhunList(rptPiZhun);//批准意见
                    if (piZhunList.Count > 0)
                    {
                        B_PF.UpdateApproveDealCondition(piZhunList);
                    }
                }

                #endregion

                //保存
                if (strActionName == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_PF bllProFile = ControlToEntity(true) as B_PF;

                    //用于提示信息
                    bllProFile.IsFormSave = true;

                    bllProFile.SubmitAction = strActionName;
                    if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_ASSIST_SIGN)
                    {
                        ReturnInfo l_objReturnInfo = FormSubmitForProFile(true, strActionName, string.Empty, FSWFStatus.Assigned.ToString(), bllProFile as EntityBase);
                        if (!l_objReturnInfo.IsSucess)
                        {
                            return;
                        }
                        JScript.Alert(ConstString.PromptInfo.ACTION_SAVE_SUC, true);
                    }
                    else
                    {
                        base.FormSubmit(true, strActionName, null, bllProFile as EntityBase);
                    }
                }
                else
                {
                    B_PF bllProFile = ControlToEntity(false) as B_PF;
                    bllProFile.IsFormSave = false;
                    bllProFile.SubmitAction = strActionName;
                    //撤销
                    if (strActionName == ProcessConstString.SubmitAction.ACTION_CANCEL)
                    {
                        base.FormCancel(bllProFile as EntityBase);
                    }
                    else
                    {
                        //验证及提示
                        bllProFile.GetSubmitMessage(base.StepName, strActionName, bllProFile, ref strErrMessage, ref strMessage);
                        if (!string.IsNullOrEmpty(strErrMessage))
                        {
                            JScript.Alert(strErrMessage, true);
                            return;
                        }

                        //表单验证
                        strErrMsg = bllProFile.SeverCheck(StepName, bllProFile);

                        if (!string.IsNullOrEmpty(strErrMsg))
                        {
                            JScript.Alert(strErrMsg, true);
                            return;
                        }
                        else
                        {
                            #region 分发节点 点击分发完成按钮 执行分发
                            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_SEND &&
                                base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_SEND_COMPLETE)
                            {
                                if (string.IsNullOrEmpty(hfSendDeptID.Value) && string.IsNullOrEmpty(hfSendUserID.Value))
                                {
                                    JScript.Alert(ConstString.PromptInfo.ACTION_CHECK_SEND, true);
                                    return;
                                }
                                try
                                {
                                    base.Circulate(hfSendDeptID.Value, "0", string.Empty, hfSendUserID.Value, "1", true, string.Empty, false);
                                }
                                catch (Exception ex)
                                {
                                    JScript.Alert(ex.ToString(), true);
                                    return;
                                }
                            }
                            #endregion

                            #region 分发节点 点击归档按钮 执行归档
                            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_SEND &&
                               base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_ARCHIVE)
                            {
                                bllProFile.ProgramType1 = B_ProgramFileInfo.GetProTypeName(bllProFile.ProgramFileID);
                                bllProFile.ProgramType2 = B_ProgramFileInfo.GetProSubTypeName(bllProFile.ProgramFileID);
                                string strDeptName = OADept.GetDeptByDeptID(bllProFile.SendDeptID).Name;
                                string strRes = string.Empty;
                                try//renjinquan改
                                {
                                    string strArchiveResult = Devolve(bllProFile, strDeptName, CurrentUserInfo.DisplayName, out strRes);
                                    base.Devolved(base.ProcessID, base.TemplateName);
                                    JScript.Alert("归档成功！\\n流水号：" + strRes, true);
                                }
                                catch (Exception ex)
                                {
                                    base.WriteLog(ex.Message);
                                    JScript.Alert("归档失败！请查看配置是否正确！", true);
                                }
                                //Regex.IsMatch(strArchiveResult, @"^\d+$") ? ConstString.PromptInfo.ACTION_ARCHIVE_SUC : strArchiveResult, true);
                                return;
                            }
                            #endregion

                            #region 程序发起、分发节点 更新程序文件信息
                            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_WRITE)
                            {
                                ReturnInfo retInfo = B_ProgramFileInfo.GetUpdateMsg(bllProFile, ConstString.ProgramFile.PROGRAM_UNFINISHED);
                                if (retInfo.IsSucess == false)
                                {
                                    JScript.Alert(retInfo.ErrMessage, true);
                                    return;
                                }

                            }
                            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_SEND &&
                                base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_SEND_COMPLETE)
                            {
                                ReturnInfo retInfo = new ReturnInfo();
                                if (bllProFile.ApplyStyle == ConstString.ProgramFile.PROGRAM_DELETE)
                                {
                                    B_ProgramFileInfo bllFileInfo = new B_ProgramFileInfo();
                                    retInfo = bllFileInfo.IsLogout(bllProFile, ConstString.ProgramFile.PROGRAM_LOGOUT);
                                }//已注销
                                else
                                {
                                    retInfo = B_ProgramFileInfo.GetUpdateMsg(bllProFile, ConstString.ProgramFile.PROGRAM_ARCHIVED);
                                }//已归档
                                if (retInfo.IsSucess == false)
                                {
                                    JScript.Alert(retInfo.ErrMessage, true);
                                    return;
                                }
                            }

                            #endregion

                            Hashtable nValues = new Hashtable();

                            #region 协助会签
                            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN)
                            {
                                if (strActionName == ProcessConstString.SubmitAction.ProgramFile.ACTION_ASSIGN)
                                {
                                    nValues = bllProFile.GetProcNameValue(base.StepName, strActionName, bllProFile);
                                    string strSubProcInstID = GetCreateSubProcessID(ProcessConstString.TemplateName.PROGRAM_FILE_ASSIGN_SIGN, nValues);
                                    bllProFile.ChildProcessID = strSubProcInstID;
                                    ReturnInfo l_objReturnInfo = FormSubmitForProFile(false, strActionName, string.Empty, FSWFStatus.Assigned.ToString(), bllProFile as EntityBase);
                                    if (!l_objReturnInfo.IsSucess)
                                    {
                                        return;
                                    }
                                    this.ShowMsgBox(this.Page, MsgType.VbInformation, strMessage, base.EntryAction);
                                    return;
                                }//交办
                                if (!string.IsNullOrEmpty(bllProFile.ChildProcessID) && strActionName == ProcessConstString.SubmitAction.ACTION_SUBMIT)
                                {
                                    AgilePointWF ag = new AgilePointWF();
                                    WorkflowService api = ag.GetAPI();
                                    WFEvent wfEvnet = api.CancelProcInst(bllProFile.ChildProcessID);
                                    if (wfEvnet.Error != null)
                                    {
                                        JScript.Alert(wfEvnet.Error, true);
                                        return;
                                    }
                                    bllProFile.ChildProcessID = string.Empty;
                                }//存在交办子流程 继续提交 撤销子流程
                            }
                            if (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_ASSIST_SIGN)
                            {
                                if (strActionName == ProcessConstString.SubmitAction.ACTION_COMPLETE)
                                {
                                    ReturnInfo l_objReturnInfo = FormSubmitForProFile(false, strActionName, bllProFile.ParentTBID, FSWFStatus.Completed.ToString(), bllProFile as EntityBase);
                                    if (!l_objReturnInfo.IsSucess)
                                    {
                                        return;
                                    }
                                }
                                this.ShowMsgBox(this.Page, MsgType.VbInformation, strMessage, base.EntryAction);
                                return;
                            }

                            #endregion

                            //调用工作流                  
                            nValues = bllProFile.GetProcNameValue(base.StepName, strActionName, bllProFile);
                            base.FormSubmit(false, strActionName, nValues, bllProFile as EntityBase);
                        }
                    }
                }

                //流程节点结束提示
                if (!string.IsNullOrEmpty(strMessage))
                {
                    JScript.Alert(strMessage, true);
                }
            }
            catch (Exception ex)
            {
                JScript.Alert(ex.Message, true);
            }
        }

        #region 工作程序-选择会签类型
        /// <summary>
        /// 工作程序-选择会签类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdolstSignStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdolstSignStyle.SelectedIndex == 0)
            {
                pnlDeptSign.Visible = true;
                pnlLeaderSign.Visible = false;
            }
            else
            {
                pnlLeaderSign.Visible = true;
                pnlDeptSign.Visible = false;
            }
        }
        #endregion

        #region 意见列表信息处理操作
        /// <summary>
        /// 意见列表信息处理操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptComment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                YiJianInfoList.RemoveAt(e.Item.ItemIndex);
                trYiJianHead.Visible = YiJianInfoList.Count > 0 ? true : false;
                hfEditIndex.Value = string.Empty;
                rptComment.DataSource = YiJianInfoList;
                rptComment.DataBind();
            }
            else if (e.CommandName == "Edit")
            {
                this.pnlComment.Visible = true;
                this.btnCancel.Visible = true;
                btnConfirm.Visible = true;
                this.btnAdd.Visible = false;
                Label lblContent = e.Item.FindControl("lblContent") as Label;
                txtInfo2.Text = lblContent.Text;
                hfEditIndex.Value = e.Item.ItemIndex.ToString();
            }
        }
        #endregion

        #region 质保审查列表绑定行
        /// <summary>
        /// 质保审查列表绑定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptQG_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (base.IsPreview == false && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TextBox txtCondition = e.Item.FindControl("txtCondition") as TextBox;
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Attributes.Add("onclick", "OpenConditionDialog(document.getElementById('" + txtCondition.ClientID + "').value,'" + txtCondition.ClientID + "')");
            }
            else
            {
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Visible = false;
            }
        }
        #endregion

        #region 部门会签列表绑定行
        /// <summary>
        /// 部门会签列表绑定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptDept_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (base.IsPreview == false && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TextBox txtCondition = e.Item.FindControl("txtCondition") as TextBox;
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Attributes.Add("onclick", "OpenConditionDialog(document.getElementById('" + txtCondition.ClientID + "').value,'" + txtCondition.ClientID + "')");
            }
            else
            {
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Visible = false;
            }
        }
        #endregion

        #region 领导会签列表绑定行
        /// <summary>
        /// 领导会签列表绑定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptLeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (base.IsPreview == false && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TextBox txtCondition = e.Item.FindControl("txtCondition") as TextBox;
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Attributes.Add("onclick", "OpenConditionDialog(document.getElementById('" + txtCondition.ClientID + "').value,'" + txtCondition.ClientID + "')");
            }
            else
            {
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Visible = false;
            }
        }
        #endregion

        #region 批准意见列表绑定行
        /// <summary>
        /// 批准意见列表绑定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPiZhun_ItemDataBound(object sender, RepeaterItemEventArgs e)//renjinquan+
        {
            if (base.IsPreview == false && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TextBox txtCondition = e.Item.FindControl("txtCondition") as TextBox;
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Attributes.Add("onclick", "OpenConditionDialog(document.getElementById('" + txtCondition.ClientID + "').value,'" + txtCondition.ClientID + "')");
            }
            else
            {
                Label lblDeal = e.Item.FindControl("lblDeal") as Label;
                lblDeal.Visible = false;
            }
        }
        #endregion

        #region 多条意见信息列表绑定事件
        /// <summary>
        /// 多条意见信息列表绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptComment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (base.IsPreview)
            {
                ((LinkButton)e.Item.FindControl("lnkbtnDel")).Visible = false;
                ((LinkButton)e.Item.FindControl("lnkbtnEdit")).Visible = false;
            }
        }
        #endregion


        #endregion
    }
}