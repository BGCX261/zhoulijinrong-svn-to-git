//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：出差申请单
// 
// 
// 创建标识：
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.DocumentId;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;
using FS.OA.Framework;
using System.Collections.Generic;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public partial class UC_Finance : FormsUIBase
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string strNewLine = "<br/>";
        private string FeeRate = OAConfig.GetConfig("差旅费大于预算比率", "Rate");
        private int SubmitStatus
        {
            get
            {
                if (ViewState["SubmitStatus"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(ViewState["SubmitStatus"]);
            }
            set { ViewState["SubmitStatus"] = value; }
        }

        #region 页面加载
        protected string FileTitle = "海南核电有限公司出差（培训）申请单";

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrint();
            this.SubmitEvents();
        }

        #region 控件初始设置
        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            B_Finance entity = base.EntityData != null ? base.EntityData as B_Finance : new B_Finance();

            //附件
            ucAttachment.UCTemplateName = base.TemplateName;
            ucAttachment.UCProcessID = base.ProcessID;
            ucAttachment.UCWorkItemID = base.WorkItemID;
            ucAttachment.UCTBID = base.IdentityID.ToString();

            #region 弹出窗口
            //抄送
            this.OASelectUC1.UCSelectType = "2";
            this.OASelectUC1.UCDeptIDControl = this.txtChaoSongDeptID.ClientID;
            this.OASelectUC1.UCDeptUserIDControl = this.txtChaoSongID.ClientID;
            this.OASelectUC1.UCDeptAndUserControl = this.txtChaoSong.ClientID;
            this.OASelectUC1.UCTemplateName = base.TemplateName;
            this.OASelectUC1.UCFormName = "抄送";

            //分发范围
            this.OASelectUC2.UCSelectType = "2";
            this.OASelectUC2.UCDeptIDControl = this.txtChuanYueDeptIDs.ClientID;
            this.OASelectUC2.UCDeptUserIDControl = this.txtChuanYueRenIDs.ClientID;
            this.OASelectUC2.UCDeptAndUserControl = this.txtChuanYueRenYuan.ClientID;
            this.OASelectUC2.UCTemplateName = base.TemplateName;
            this.OASelectUC2.UCFormName = "分发范围";

            #endregion

            OAControl controls = new OAControl();

            if (!base.IsPreview)
            {
                this.txtPiShiYiJian.ToolTip = "2000字符以内";
                this.ddlBianZhiBuMen.ToolTip = "自己所属的处室";
                FileTitle = "海南核电有限公司出差（培训）申请单";
            }

            if (!base.IsPreview)
            {
                switch (base.StepName)
                {
                    #region 拟稿
                    case ProcessConstString.StepName.STEP_DRAFT:
                        this.btnCancel.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");

                        //控制撤销按钮显示
                        this.btnCancel.Visible = this.txtIsBack.Text == "True";

                       

                        this.txtBianHao.ToolTip = "提交后自动生成";
                        if (string.IsNullOrEmpty(base.WorkItemID))
                        {
                            this.lbJs.Text = "<script>ShowMyDiv();</script>";
                        }

                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnTiJiaoShenHe, this.btnSave };
                        controls.YellowControls = new Control[] { this.txtChaoSong };

                        if (this.SubmitStatus == 1)
                        {
                            controls.DisEnableControls = new Control[] { this.txtFeeYuSuan, this.txtFeeFaSheng, ddlZhuGuanLingDao, ddlChuLingDao };
                        }
                        else if (this.SubmitStatus == 2)
                        {
                            controls.DisEnableControls = new Control[] { this.txtFeeYuSuan, this.txtFeeFaSheng, ddlZongJingLi, ddlChuLingDao };
                        }
                        else if (this.SubmitStatus == 3)
                        {
                            controls.DisEnableControls = new Control[] { this.txtFeeYuSuan, this.txtFeeFaSheng, ddlZongJingLi, ddlZhuGuanLingDao };
                        }
                        else
                        {
                            controls.DisEnableControls = new Control[] { this.txtFeeYuSuan, this.txtFeeFaSheng };
                        }
                        break;
                    #endregion

                    #region 处领导审核
                    case ProcessConstString.StepName.FinanceStepName.STEP_DepartLeader:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, this.txtShenPiYiJian };
                        controls.DisEnableControls = new Control[] { this.ddlBianZhiBuMen, this.txtTongXing, this.txtDestination, this.txtFeeYuSuan, this.txtFeeFaSheng, this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 主管领导审核
                    case ProcessConstString.StepName.FinanceStepName.STEP_ChargeLeader:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, this.txtShenPiYiJian };
                        controls.DisEnableControls = new Control[] { this.ddlBianZhiBuMen, this.txtTongXing, this.txtDestination, this.txtFeeYuSuan, this.txtFeeFaSheng, this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 总经理审核
                    case ProcessConstString.StepName.FinanceStepName.STEP_GeneralManager:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, this.txtShenPiYiJian };
                        controls.DisEnableControls = new Control[] { this.ddlBianZhiBuMen, this.txtTongXing, this.txtDestination, this.txtFeeYuSuan, this.txtFeeFaSheng, this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 订票处
                    case ProcessConstString.StepName.FinanceStepName.STEP_BookingOffice:

                        controls.EnableControls = new Control[] { this.OASelectUC1, this.txtShangWu, this.btnDingPiao, this.btnTuiHui, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.txtZhuTi, this.ddlBianZhiBuMen, this.txtTongXing, this.txtDestination, this.txtChuChaiRenWu,this.txtFeeYuSuan, this.txtFeeFaSheng, this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 反馈报销人
                    case ProcessConstString.StepName.FinanceStepName.STEP_FeedBack:

                        FormsMethod.SetControlAll(this);
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnApplyComplete, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.txtZhuTi, this.ddlBianZhiBuMen, this.txtTongXing, this.txtDestination, this.txtFeeYuSuan, this.txtFeeFaSheng };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                }

                //设置所有控件状态
                controls.SetControls();
            }
            else
            {
                //ucAttachment.UCIsEditable = false;
                FormsMethod.SetControlAll(this);
            }
        }
        #endregion

        #region 实体加载
        /// <summary>
        /// 实体加载
        /// </summary>
        protected override void EntityToControl()
        {
            B_Finance entity = base.EntityData != null ? base.EntityData as B_Finance : new B_Finance();
            //拟稿人
            this.txtNiGaoRen.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
            this.txtNiGaoRenID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
            //新版OA时间精确到秒（控制时间的显示格式）
            //bool isOld = entity.DraftDate < base.OAStartTime;
            //职务 
            drpZhiWu.DataSource = GetPostList();
            drpZhiWu.DataBind();
            drpZhiCheng.DataSource = GetTitleList();
            drpZhiCheng.DataBind();
            //附件
            ucAttachment.UCDataList = entity.FileList;
            this.txtFeeYuSuan.Text = entity.FeeYuSuan;
            this.txtFeeFaSheng.Text = entity.FeeFaSheng;
            this.txtShenPiYiJian.Text = entity.ShenPiYiJian;
            //编制部门及预算
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
            {
                OADept.GetDeptByUser(this.ddlBianZhiBuMen, string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID, 1, true, false);
                FormsMethod.SelectedDropDownList(this.ddlBianZhiBuMen, entity.BianZhiBuMenID); B_FinanceDeptInfo bllInfo = new B_FinanceDeptInfo();
                M_FinanceDeptInfo info = bllInfo.GetFinanceDeptInfoByDeptID(DateTime.Now.Year.ToString(), this.ddlBianZhiBuMen.SelectedValue);

                this.txtFeeYuSuan.Text = info.TripBudgetCost;
                this.txtFeeFaSheng.Text = info.TripUseCost;

                string ManagerDeptID = OADept.GetDeptID("总经理部");

                Double FeeFa = Convert.ToDouble(string.IsNullOrEmpty(txtFeeFaSheng.Text) ? "0" : txtFeeFaSheng.Text);
                Double FeeYu = Convert.ToDouble(string.IsNullOrEmpty(txtFeeYuSuan.Text) ? "0" : txtFeeYuSuan.Text);
                Double Rate = (string.IsNullOrEmpty(FeeRate) == true ? 0.1 : Convert.ToDouble(FeeRate));
                if ((FeeFa - FeeYu) / FeeYu > Rate)
                {
                    OAUser.GetUserByDeptPost(ddlZongJingLi, ManagerDeptID, OUConstString.PostName.ZONGJINGLI, true, false, 1);
                    ddlZongJingLi.Enabled = true;
                    this.SubmitStatus = 1;
                }
                else if (FeeFa > FeeYu)
                {
                    OAUser.GetUserByDeptPost(ddlZhuGuanLingDao, ManagerDeptID, OUConstString.PostName.FUZONGJINGLI, true, false, 1);
                    ddlZhuGuanLingDao.Enabled = true;
                    this.SubmitStatus = 2;
                }
                else if (FeeFa <= FeeYu)
                {
                    OAUser.GetUserByDeptPost(ddlChuLingDao, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.CHUZHANG, false, true, 0);
                    ddlChuLingDao.Enabled = true;
                    this.SubmitStatus = 3;
                }
                ListItem item = new ListItem(this.txtNiGaoRen.Text, this.txtNiGaoRenID.Text);
                if (ddlChuLingDao.Items.Contains(item))
                {
                    ddlChuLingDao.Items.Clear();
                    OAUser.GetUserByDeptPost(ddlZhuGuanLingDao, ManagerDeptID, OUConstString.PostName.FUZONGJINGLI, true, false, 1);
                    ddlZhuGuanLingDao.Enabled = true;
                    this.SubmitStatus = 2;
                }
                else if (ddlZhuGuanLingDao.Items.Contains(item))
                {
                    ddlZhuGuanLingDao.Items.Clear();
                    OAUser.GetUserByDeptPost(ddlZongJingLi, ManagerDeptID, OUConstString.PostName.ZONGJINGLI, true, false, 1);
                    ddlZongJingLi.Enabled = true;
                    this.SubmitStatus = 1;
                }
            }
            else
            {
                FormsMethod.SetDropDownList(this.ddlBianZhiBuMen, entity.BianZhiBuMenID, entity.Department);
                FormsMethod.SetDropDownList(this.ddlZongJingLi, entity.GeneralManagerID, entity.GeneralManager);
                FormsMethod.SetDropDownList(this.ddlZhuGuanLingDao, entity.ChargeLeaderID, entity.ChargeLeader);
                FormsMethod.SetDropDownList(this.ddlChuLingDao, entity.DepartmentLeaderID, entity.DepartmentLeader);
            }
            //申请单编号
            this.txtBianHao.Text = entity.DocumentNo;
            FormsMethod.SelectedDropDownList(this.drpZhiWu, entity.ZhiWu);
            FormsMethod.SelectedDropDownList(this.drpZhiCheng, entity.ZhiCheng);

            //部门负责人
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
            {
                if (this.ddlBianZhiBuMen.Items.Count > 0)
                {
                    OAUser.GetUserByDeptPost(this.ddlFuZeRen, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.CHUZHANG, true, true);
                }
                FormsMethod.SelectedDropDownList(this.ddlFuZeRen, entity.FuZeRenID);
            }
            else
            {
                FormsMethod.SetDropDownList(this.ddlFuZeRen, entity.FuZeRenID, entity.DeptPrincipal);
            }

            //订票处
            if (base.StepName == ProcessConstString.StepName.FinanceStepName.STEP_DepartLeader ||
                base.StepName == ProcessConstString.StepName.FinanceStepName.STEP_ChargeLeader ||
                base.StepName == ProcessConstString.StepName.FinanceStepName.STEP_GeneralManager
                && !base.IsPreview)
            {              
                string[] arrayBookingOffice = OAUser.GetUserByRoleName(OUConstString.RoleName.BookingOffice);
                this.txtBookingOfficeID.Text = arrayBookingOffice[0];

            }

            //是否退回
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT)
            {
                this.txtIsBack.Text = entity.IsBack.ToString();
            }

            //拟稿人及日期
            if (entity.DraftDate != DateTime.MinValue)
            {
                this.lblNiGaoRiQi.Text = entity.DraftDate.ToString(DateFormat);

                //拟稿人显示非下拉列表框形式
                this.txtNiGaoRen.Visible = false;
                this.lbNiGaoRen.Visible = true;
                //this.lbNiGaoRen.Text = entity.Drafter + strNewLine + entity.DraftDate.ToString(ConstString.DateFormat.Long);
                this.lbNiGaoRen.Text = entity.Drafter;

            }
            else
            {
                this.lblNiGaoRiQi.Text = DateTime.Now.ToString(DateFormat);
            }

            //抄送
            this.txtChaoSong.Text = entity.CopySend;
            this.txtChaoSongID.Text = entity.ChaoSongID;
            this.txtChaoSongDeptID.Text = entity.ChaoSongDeptID;

            //主题
            this.txtZhuTi.Text = entity.DocumentTitle;

            this.txtTongXing.Text = entity.TongXingRenYuan;

            //出差任务
            //this.txtNeiRong.Text = SysString.HtmlToTextCode(entity.Content);
            this.txtChuChaiRenWu.Text = entity.ChuChaiRenWu;
            this.timeChuFa.Text = entity.ChuFaShiJian;
            this.timeHuiCheng.Text = entity.HuiChengShiJian;

            //商务信息
            this.txtShangWu.Text = entity.ShangWuXinXi;

            //目的地
            this.txtDestination.Text = entity.Destination;

            //拟稿人
            //this.txtNiGaoRen.Text = entity.Drafter;
            //this.txtNiGaoRenID.Text = entity.NiGaoRenID;

            //提示信息
            this.txtTiShiXinXi.Text = entity.Message;
            this.txtBanShuiXinXi.Text = entity.Message;

            //提示信息添加
            this.txtTianJia.Text = entity.MessageAdd;

            this.txtGeneralManagerID.Text = entity.GeneralManagerID;

            this.txtChargeLeaderID.Text = entity.ChargeLeaderID;

            this.txtDepartmentLeaderID.Text = entity.DepartmentLeaderID;
        }
        #endregion

        #endregion

        #region 实体赋值
        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="IsSave"></param>
        /// <returns></returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_Finance entity = base.EntityData != null ? base.EntityData as B_Finance : new B_Finance();

            //附件
            entity.FileList = ucAttachment.UCDataList;

            entity.ShenPiYiJian = this.txtShenPiYiJian.Text;
            #region 提示信息、承办情况
            //提示信息、承办情况
            if (!IsSave)
            {
                if (!string.IsNullOrEmpty(this.txtTianJia.Text))
                {
                    entity.MessageAdd = string.Empty;
                    entity.Message = this.txtTiShiXinXi.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(DateFormat) + "]:(" + base.StepName + ")" + this.txtTianJia.Text + "\n";
                }
            }
            else
            {
                entity.Message = this.txtTiShiXinXi.Text;
                entity.MessageAdd = this.txtTianJia.Text;
            }
            #endregion

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:

                    //抄送
                    entity.CopySend = this.txtChaoSong.Text;
                    entity.ChaoSongDeptID = this.txtChaoSongDeptID.Text;
                    entity.ChaoSongID = this.txtChaoSongID.Text;

                    //编制部门、编制部门ID
                    if (this.ddlBianZhiBuMen.Items.Count > 0)
                    {
                        entity.Department = this.ddlBianZhiBuMen.SelectedItem.Text;
                        entity.BianZhiBuMenID = this.ddlBianZhiBuMen.SelectedValue;
                    }
                    //部门负责人
                    if (this.ddlFuZeRen.Items.Count > 0)
                    {
                        entity.DeptPrincipal = this.ddlFuZeRen.SelectedItem.Text;
                        entity.FuZeRenID = this.ddlFuZeRen.SelectedValue;
                    }
                    if (IsSave == false)
                    {
                        //申请单编号
                        entity.DocumentNo = this.GetFinanceTripNo(entity);
                    }
                    entity.FeeYuSuan = this.txtFeeYuSuan.Text;
                    entity.FeeFaSheng = this.txtFeeFaSheng.Text;
                    entity.ZhiWu = this.drpZhiWu.SelectedValue;
                    entity.ZhiCheng = this.drpZhiCheng.SelectedValue;
                    entity.TongXingRenYuan = this.txtTongXing.Text;

                    //主题、标题
                    //entity.Subject = SysString.InputText(this.txtZhuTi.Text);
                    entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);

                    //内容
                    //entity.Content = SysString.TextToHtmlCode(this.txtChuChaiRenWu.Text);
                    entity.Content = this.txtChuChaiRenWu.Text;
                    entity.ChuChaiRenWu = this.txtChuChaiRenWu.Text;
                    entity.ChuFaShiJian = this.timeChuFa.Text;
                    entity.HuiChengShiJian = this.timeHuiCheng.Text;
                    entity.ShangWuXinXi = this.txtShangWu.Text;

                    //拟稿人
                    entity.Drafter = this.txtNiGaoRen.Text;
                    entity.DrafterID = this.txtNiGaoRenID.Text;

                    //老版本用到
                    entity.NiGaoRenID = this.txtNiGaoRenID.Text;

                    //拟稿日期
                    entity.DraftDate = DateTime.Now;

                    this.GetFeeComparedSigner(entity);

                    break;
                #endregion

                #region 处领导审核
                case ProcessConstString.StepName.FinanceStepName.STEP_DepartLeader:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {

                        //抄送
                        entity.CopySend = this.txtChaoSong.Text;
                        entity.ChaoSongDeptID = this.txtChaoSongDeptID.Text;
                        entity.ChaoSongID = this.txtChaoSongID.Text;
                        entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);
                        entity.Content = this.txtChuChaiRenWu.Text;
                    }
                    else
                    {
                        entity.IsBack = true;
                    }
                    break;
                #endregion

                #region 订票处
                case ProcessConstString.StepName.FinanceStepName.STEP_BookingOffice:
                    entity.ShangWuXinXi = this.txtShangWu.Text;
                    break;
                #endregion

            }

            //订票处
            entity.BookingOfficeID = this.txtBookingOfficeID.Text;
            //目的地
            entity.Destination = this.txtDestination.Text;
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
            //保存批示
            this.btnSavePishi.Click += SubmitHandler;
            //批示完成
            this.btnComplete.Click += SubmitHandler;
            //完成
            this.btnShenHeWanCheng.Click += SubmitHandler;
            //撤销
            this.btnCancel.Click += SubmitHandler;
            //退回
            this.btnTuiHui.Click += SubmitHandler;

            this.btnApplyComplete.Click += SubmitHandler;
            //提交审核
            this.btnTiJiaoShenHe.Click += SubmitHandler;
            //通过审核
            this.btnTongGuoShenHe.Click += SubmitHandler;
            //订票
            this.btnDingPiao.Click += SubmitHandler;
            //追加分发
            this.btnZhuiJiaFenFa.Click += SubmitHandler;
            //完成归档
            this.btnWanChengGuiDang.Click += SubmitHandler;

        }
        #endregion

        #region 提交按钮处理事件
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
                    B_Finance entity = ControlToEntity(true) as B_Finance;
                    entity.SubmitAction = strActionName;
                    base.FormSubmit(true, strActionName, null, entity);
                }
                else
                {
                    B_Finance entity = ControlToEntity(false) as B_Finance;
                    entity.SubmitAction = strActionName;

                    //撤销
                    if (strActionName == ProcessConstString.SubmitAction.ACTION_CANCEL)
                    {
                        base.FormCancel(entity);
                    }
                    else
                    {
                        //返回验证信息
                        entity.GetSubmitMessage(base.StepName, strActionName, ref strErrorMessage);
                        if (!string.IsNullOrEmpty(strErrorMessage))
                        {
                            JScript.ShowMsgBox(this.Page, strErrorMessage, false);
                            return;
                        }
                        else
                        {
                            switch (base.StepName)
                            {
                                case ProcessConstString.StepName.STEP_DRAFT:
                                    strActionName = base.SubAction;
                                    break;

                                case ProcessConstString.StepName.FinanceStepName.STEP_DISTRIBUTION:
                                    if (strActionName == ProcessConstString.SubmitAction.FinanceAction.ACTION_WCGD)
                                    {
                                        if (!string.IsNullOrEmpty(entity.ChuanYueDeptID) || !string.IsNullOrEmpty(entity.ChuanYueRenYuanID))
                                        {
                                            base.Circulate(entity.ChuanYueDeptID, "1", string.Empty, entity.ChuanYueRenYuanID, "1", false, string.Empty, false);
                                        }

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
                                    break;
                            }
                        }

                        //调用工作流                  
                        Hashtable nValues = entity.GetProcNameValue(base.StepName, strActionName);
                        base.FormSubmit(false, strActionName, nValues, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowMsgBox(this.Page, MsgType.VbExclamation, ex.Message);
            }
        }
        #endregion

        protected void ddlBianZhiBuMen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //部门负责人
            if (this.ddlBianZhiBuMen.Items.Count > 0)
            {
                OAUser.GetUserByDeptPost(this.ddlFuZeRen, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.CHUZHANG, true, true);
            }
        }
        #endregion

        private string GetFeeComparedSigner(B_Finance entity)
        {
            string SignIDs = string.Empty;

            Double FeeFa = Convert.ToDouble(string.IsNullOrEmpty(txtFeeFaSheng.Text) ? "0" : txtFeeFaSheng.Text);
            Double FeeYu = Convert.ToDouble(string.IsNullOrEmpty(txtFeeYuSuan.Text) ? "0" : txtFeeYuSuan.Text);
            Double Rate = (string.IsNullOrEmpty(FeeRate) == true ? 0.1 : Convert.ToDouble(FeeRate));
            if ((FeeFa - FeeYu) / FeeYu > Rate)
            {
                if (ddlZongJingLi.SelectedItem != null)
                {
                    //总经理 - 以及大于10%否（大于就总经理签）
                    entity.GeneralManager = ddlZongJingLi.SelectedItem.Text;
                    entity.GeneralManagerID = ddlZongJingLi.SelectedValue;
                    base.SubAction = "提交总经理";
                }
            }
            else if (FeeFa > FeeYu)
            {
                if (ddlZhuGuanLingDao.SelectedItem != null)
                {
                    //主管领导 - 已经发生的差旅费是否大于预算（大于就主管领导签）
                    entity.ChargeLeader = ddlZhuGuanLingDao.SelectedItem.Text;
                    entity.ChargeLeaderID = ddlZhuGuanLingDao.SelectedValue;
                    base.SubAction = "提交主管领导";
                }
            }
            else if (FeeFa <= FeeYu)
            {
                //处领导 -    
                if (ddlChuLingDao.SelectedItem != null)
                {
                    entity.DepartmentLeader = ddlChuLingDao.SelectedItem.Text;
                    entity.DepartmentLeaderID = ddlChuLingDao.SelectedValue;
                }
                base.SubAction = "提交处领导";
            }
        
            if (this.SubmitStatus==2)
            {
                entity.ChargeLeader = ddlZhuGuanLingDao.SelectedItem.Text;
                entity.ChargeLeaderID = ddlZhuGuanLingDao.SelectedValue;
                base.SubAction = "提交主管领导";
            }
            else if (this.SubmitStatus==1)
            {
                entity.GeneralManager = ddlZongJingLi.SelectedItem.Text;
                entity.GeneralManagerID = ddlZongJingLi.SelectedValue;
                base.SubAction = "提交总经理";
            }
            return SignIDs;
        }

        /// <summary>
        ///申请单的编号规则 CC+部门代码+年度+四位流水号
        /// </summary>
        /// <returns></returns>
        protected string GetFinanceTripNo(B_Finance entity)
        {
            Department dept = OADept.GetDeptByDeptID(this.ddlBianZhiBuMen.SelectedValue);
            string strLine = "-";
            string strYear = DateTime.Now.Year.ToString();
            B_DocumentNo_A b_documentno_a = new B_DocumentNo_A();
            return "CCSQ" + strLine + dept.No + strLine + b_documentno_a.GetNo(ProcessConstString.TemplateName.FINANCE_TRIPAPPLY, strYear);
        }

        public List<String> GetPostList()
        {
            List<String> postList = new List<string>();
            postList.Add("");
            postList.Add("公司领导");
            postList.Add("处级干部");
            postList.Add("科级干部");
            postList.Add("普通员工");
            return postList;
        }

        public List<String> GetTitleList()
        {
            List<String> titleList = new List<string>();
            titleList.Add("");
            titleList.Add("研高职称");
            titleList.Add("高级职称");
            titleList.Add("中级职称");
            titleList.Add("初级职称");
            titleList.Add("无");
            return titleList;
        }

        protected void btn_GuiDang_Click(object sender, EventArgs e)
        {
            //请示报告归档
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