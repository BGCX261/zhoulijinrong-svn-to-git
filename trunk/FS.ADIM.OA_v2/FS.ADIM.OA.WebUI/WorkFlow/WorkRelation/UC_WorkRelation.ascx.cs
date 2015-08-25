//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：工作联系单视图
// 
// 
// 创建标识：wangbinyi 2009-12-28
//
// 修改标识：任金权 2010-5-10
// 修改描述：修改EntityToControl、ControlToEntity。去除使用HtmlToTextCode、HtmlToTextCode。不需要使用，会统一处理
//
// 修改标识：任金权 2010-5-21
// 修改描述：修改ControlToEntity。核搞增加核搞日期。
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
using System.Web.UI.HtmlControls;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public partial class UC_WorkRelation : FormsUIBase
    {
        private const string strNewLine = "<br/>";

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
            B_WorkRelation entity = base.EntityData != null ? base.EntityData as B_WorkRelation : new B_WorkRelation();

            //附件
            ucAttachment.UCTemplateName = base.TemplateName;
            ucAttachment.UCProcessID = base.ProcessID;
            ucAttachment.UCWorkItemID = base.WorkItemID;
            ucAttachment.UCTBID = base.IdentityID.ToString();

            //抄送
            this.OASelectUC1.UCSelectType = "2";
            this.OASelectUC1.UCDeptIDControl = this.txtChaoSongDeptIDs.ClientID;
            this.OASelectUC1.UCDeptUserIDControl = this.txtChaoSongIDs.ClientID;
            this.OASelectUC1.UCDeptAndUserControl = this.txtChaoSong.ClientID;
            this.OASelectUC1.UCTemplateName = base.TemplateName;
            this.OASelectUC1.UCFormName = "抄送";

            //核稿人
            this.OASelectUC2.UCSelectType = "1";
            this.OASelectUC2.UCDeptUserIDControl = this.txtHeGaoRenID.ClientID;
            this.OASelectUC2.UCDeptUserNameControl = this.txtHeGaoRen.ClientID;
            this.OASelectUC2.UCIsSingle = "1";
            this.OASelectUC2.UCTemplateName = base.TemplateName;
            this.OASelectUC2.UCFormName = "核稿人";

            if (this.ddlBianZhiBuMen.Items.Count > 0)
            {
                this.OASelectUC2.UCShowDeptID = OADept.GetChildDeptIDSConSelf(this.ddlBianZhiBuMen.SelectedValue, -1);
            }

            //承办传阅
            this.OASelectUC3.UCSelectType = "1";
            this.OASelectUC3.UCDeptUserIDControl = this.txtChuanYueRenYuanID.ClientID;
            this.OASelectUC3.UCDeptUserNameControl = this.txtChuanYueRenYuan.ClientID;
            this.OASelectUC3.UCIsSingle = "0";
            this.OASelectUC3.UCTemplateName = base.TemplateName;
            this.OASelectUC3.UCFormName = "承办传阅";

            if (this.ddlZhuSong.Items.Count > 0)
            {
                this.OASelectUC3.UCShowDeptID = OADept.GetChildDeptIDSConSelf(this.ddlZhuSong.SelectedValue, -1);
            }

            OAControl controls = new OAControl();

            //if (!base.IsPreview)
            //{
            //    this.txtZhuTi.ToolTip = "50字符以内";
            //    this.txtNeiRong.ToolTip = "2000字符以内";
            //    this.ddlFuZeRen.ToolTip = "部门负责人、部门领导、大于副处长";
            //    this.ddlKeShiLingDao.ToolTip = "科室（部门负责人）";
            //    this.ddlChengBanRen.ToolTip = "处室承办（处室下所有人包括科室），科室承办（科室下所有人）";
            //    this.ddlBianZhiBuMen.ToolTip = "自己所属的处室";
            //    this.txtHeGaoRen.ToolTip = this.ddlBianZhiBuMen.Items.Count > 0 ? (this.ddlBianZhiBuMen.SelectedItem.Text + "的成员") : string.Empty;
            //}

            if (!base.IsPreview)
            {
                switch (base.StepName)
                {
                    #region 拟稿
                    case ProcessConstString.StepName.STEP_DRAFT:
                        //部门会签按钮不可编辑
                        //this.ucBuMenHuiQian.UCIsDisEnable = true;

                        this.btnCancel.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");

                        //控制撤销按钮显示
                        this.btnCancel.Visible = this.txtIsBack.Text == "True";

                        this.txtNiGaoRen.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.txtNiGaoRenID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;

                        controls.EnableControls = new Control[] { this.btnTiJiaoQianFa, this.btnTiJiaoHeGao, this.btnSave, this.OASelectUC1, this.OASelectUC2 };
                        controls.YellowControls = new Control[] { this.txtChaoSong, this.txtHeGaoRen };
                        break;
                    #endregion

                    #region 核稿
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_CHECK:
                        ucBuMenHuiQian.UCIsAllowDel = base.IdentityID == 0 ? false : true;//可勾选checkbox逻辑删除已会签信息

                        controls.EnableControls = new Control[] { this.btnTiJiaoQianFa, this.btnTuiHui, this.btnSave, this.OASelectUC1, this.btnDeptSign };
                        controls.DisEnableControls = new Control[] { this.txtHeGaoRen, this.ddlBianZhiBuMen };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 部门会签
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN:
                        this.ucAttachment.UCIsEditable = false;//附件不可编辑
                        this.ucBuMenHuiQian.UCIsDisEnable = true;

                        trYiJianHead.Visible = YiJianInfoList.Count > 0;
                        rptComment.DataSource = YiJianInfoList;
                        rptComment.DataBind();

                        controls.EnableControls = new Control[] { this.tdDeptSign };
                        controls.DisEnableControls = new Control[] { this.txtHeGaoRen, this.ddlBianZhiBuMen, this.ddlFuZeRen, this.txtTianJia, this.txtNeiRong, 
                            this.txtChaoSong,this.txtZhuTi,this.ddlZhuSong };
                        break;
                    #endregion

                    #region 签发
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_SIGN:
                        this.btnFenFa.Attributes.Add("onclick", "javascript: if(!checkChaoSong()){return false;}else{DisableButtons();}");
                        this.ucBuMenHuiQian.UCIsDisEnable = true;

                        controls.EnableControls = new Control[] { this.btnFenFa, this.btnTuiHui, this.btnSave, this.OASelectUC1 };
                        controls.DisEnableControls = new Control[] { this.txtHeGaoRen, this.ddlBianZhiBuMen, this.ddlFuZeRen };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 处室承办
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR:
                        //获取任务的部门领导
                        this.txtBuMenLingDao.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.txtBuMenLingDaoID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                        this.ucBuMenHuiQian.UCIsDisEnable = true;

                        //if (base.SubAction != ProcessConstString.SubmitAction.ACTION_COMPLETE && this.cbISChuanYue.Checked == true)
                        //{

                        //}
                        //this.btnWanCheng.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");

                        controls.EnableControls = new Control[] {this.tbChenBanChuanYue, this.tbChenBan, this.trBanLiYiJian, this.btnJiaoBanKeShi,this.btnJiaoBanRenYuan, 
                        this.btnWanCheng, this.btnSave,this.OASelectUC3 };
                        controls.DisEnableControls = new Control[] { this.txtHeGaoRen, this.ddlBianZhiBuMen, this.ddlFuZeRen,
                        this.txtChaoSong, this.txtZhuTi,this.txtNeiRong,this.ddlZhuSong,this.OASelectUC1};
                        controls.YellowControls = new Control[] { this.txtChuanYueRenYuan };
                        break;
                    #endregion

                    #region 科室承办
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF:
                        if (entity.DirectorDate != DateTime.MinValue)
                        {
                            this.txtBuMenLingDao.Visible = true;
                            this.txtBuMenLingDao.Visible = false;
                            this.lblDirector.Visible = true;
                            this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(ConstString.DateFormat.Long);
                        }

                        this.ucBuMenHuiQian.UCIsDisEnable = true;

                        //if (this.cbISChuanYue.Checked == true)
                        //{
                        //    this.btnWanCheng.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");
                        //}

                        controls.EnableControls = new Control[] {this.tbChenBan,this.tbChenBanChuanYue, this.trBanLiYiJian,  this.btnJiaoBanRenYuan, 
                        this.btnWanCheng, this.btnSave,this.OASelectUC3 };
                        controls.DisEnableControls = new Control[] { this.txtHeGaoRen, this.ddlBianZhiBuMen, this.ddlFuZeRen,
                        this.txtChaoSong, this.txtZhuTi,this.txtNeiRong, this.ddlZhuSong,this.ddlKeShiLingDao};
                        controls.YellowControls = new Control[] { this.txtChuanYueRenYuan };
                        break;
                    #endregion

                    #region 人员承办
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER:
                        if (entity.DirectorDate != DateTime.MinValue)
                        {
                            this.txtBuMenLingDao.Visible = true;
                            this.txtBuMenLingDao.Visible = false;
                            this.lblDirector.Visible = true;
                            this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(ConstString.DateFormat.Long);
                        }

                        this.ucBuMenHuiQian.UCIsDisEnable = true;

                        if (entity.SectionDate != DateTime.MinValue)
                        {
                            this.ddlKeShiLingDao.Visible = false;
                            this.lblSection.Visible = true;
                            this.lblSection.Text = entity.SectionLeader + strNewLine + entity.SectionDate.ToString(ConstString.DateFormat.Long);
                        }


                        //if (this.cbISChuanYue.Checked == true)
                        //{
                        //    this.btnWanCheng.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");
                        //}

                        controls.EnableControls = new Control[] {this.tbChenBan,tbChenBanChuanYue,this.trBanLiYiJian,  this.btnWanCheng, 
                        this.btnSave,this.OASelectUC3 };
                        controls.DisEnableControls = new Control[] { this.txtHeGaoRen, this.ddlBianZhiBuMen, this.ddlFuZeRen, this.ddlZhuSong,
                            this.txtChaoSong, this.txtZhuTi,this.txtNeiRong,this.ddlChengBanRen, this.ddlKeShiLingDao };
                        controls.YellowControls = new Control[] { this.txtChuanYueRenYuan };
                        break;
                    #endregion
                }

                //设置所有控件状态
                controls.SetControls();
            }
            else
            {
                this.ucAttachment.UCIsEditable = false;
                this.ucBuMenHuiQian.UCIsDisEnable = true;

                FormsMethod.SetControlAll(this);

                #region 已承办的显示在lable上
                if (base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR ||
                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF ||
                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER)
                {
                    if (entity.DirectorDate != DateTime.MinValue)
                    {
                        this.txtBuMenLingDao.Visible = true;
                        this.txtBuMenLingDao.Visible = false;
                        this.lblDirector.Visible = true;
                        this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(ConstString.DateFormat.Long);
                    }

                    if (entity.SectionDate != DateTime.MinValue)
                    {
                        this.ddlKeShiLingDao.Visible = false;
                        this.lblSection.Visible = true;
                        this.lblSection.Text = entity.SectionLeader + strNewLine + entity.SectionDate.ToString(ConstString.DateFormat.Long);
                    }

                    if (entity.MemberDate != DateTime.MinValue)
                    {
                        this.ddlChengBanRen.Visible = false;
                        this.lblMember.Visible = true;
                        this.lblMember.Text = entity.Contractor + strNewLine + entity.MemberDate.ToString(ConstString.DateFormat.Long);
                    }
                }
                #endregion

                switch (base.StepName)
                {
                    #region 拟稿
                    case ProcessConstString.StepName.STEP_DRAFT:
                        //this.pnlDealSign.Visible = true;
                        break;
                    #endregion

                    #region 处室承办
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR:
                        this.tbChenBanChuanYue.Visible = true;
                        this.tbChenBan.Visible = true;
                        this.txtChengBanRiQi.Text = entity.DirectorDate.ToString(ConstString.DateFormat.Long);
                        break;
                    #endregion

                    #region 科室承办
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF:
                        this.tbChenBanChuanYue.Visible = true;
                        this.tbChenBan.Visible = true;
                        this.txtChengBanRiQi.Text = entity.SectionDate.ToString(ConstString.DateFormat.Long);
                        break;
                    #endregion

                    #region 人员承办
                    case ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER:
                        this.tbChenBanChuanYue.Visible = true;
                        this.tbChenBan.Visible = true;
                        this.txtChengBanRiQi.Text = entity.MemberDate.ToString(ConstString.DateFormat.Long);
                        break;
                    #endregion

                    #region 传阅    //yangzj 20110702
                    case ProcessConstString.StepName.STEP_CIRCULATE:
                        this.tbChenBan.Visible = true;
                            if (entity.DirectorDate != DateTime.MinValue)
                            {
                                this.txtBuMenLingDao.Visible = true;
                                this.txtBuMenLingDao.Visible = false;
                                this.lblDirector.Visible = true;
                                this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(ConstString.DateFormat.Long);
                            }

                            if (entity.SectionDate != DateTime.MinValue)
                            {
                                this.ddlKeShiLingDao.Visible = false;
                                this.lblSection.Visible = true;
                                this.lblSection.Text = entity.SectionLeader + strNewLine + entity.SectionDate.ToString(ConstString.DateFormat.Long);
                            }

                            if (entity.MemberDate != DateTime.MinValue)
                            {
                                this.ddlChengBanRen.Visible = false;
                                this.lblMember.Visible = true;
                                this.lblMember.Text = entity.Contractor + strNewLine + entity.MemberDate.ToString(ConstString.DateFormat.Long);
                            }
                            break;
                        #endregion

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
            B_WorkRelation entity = base.EntityData != null ? base.EntityData as B_WorkRelation : new B_WorkRelation();

            if (entity != null)
            {
                this.wfTimesFlag.Text = entity.TimesFlag;//次数标示

                //附件
                ucAttachment.UCDataList = entity.FileList;

                //主送
                if ((base.StepName == ProcessConstString.StepName.STEP_DRAFT ||
                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHECK ||
                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_SIGN) && !base.IsPreview)
                {
                    OADept.GetDeptByIfloor(this.ddlZhuSong, 1);
                    FormsMethod.SelectedDropDownList(this.ddlZhuSong, entity.MainSendDeptID);
                }
                else
                {
                    FormsMethod.SetDropDownList(this.ddlZhuSong, entity.MainSendDeptID, entity.MainSend);
                }

                //编制部门
                if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
                {
                    OADept.GetDeptByUser(this.ddlBianZhiBuMen, string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID, 1, true, false);

                    FormsMethod.SelectedDropDownList(this.ddlBianZhiBuMen, entity.BianZhiBuMenID);
                }
                else
                {
                    FormsMethod.SetDropDownList(this.ddlBianZhiBuMen, entity.BianZhiBuMenID, entity.Department);
                }

                //部门负责人
                if (base.StepName == ProcessConstString.StepName.STEP_DRAFT ||
                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHECK && !base.IsPreview)
                {
                    if (this.ddlBianZhiBuMen.Items.Count > 0)
                    {
                        OAUser.GetUserByDeptPost(this.ddlFuZeRen, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.FUCHUZHANG, true, true);
                    }
                    FormsMethod.SelectedDropDownList(this.ddlFuZeRen, entity.FuZeRenID);
                }
                else
                {
                    FormsMethod.SetDropDownList(this.ddlFuZeRen, entity.FuZeRenID, entity.DeptPrincipal);
                }

                //承办
                if (base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR && !base.IsPreview)
                {
                    OADept.GetChildDept(this.ddlKeShiLingDao, entity.MainSendDeptID, 2, true, true);
                    OAUser.GetUserByDeptID(this.ddlChengBanRen, entity.MainSendDeptID, -1);
                    FormsMethod.SelectedDropDownList(this.ddlKeShiLingDao, entity.KeShiID);
                    FormsMethod.SelectedDropDownList(this.ddlChengBanRen, entity.ChengBanRenID);

                    //部门领导
                    this.txtBuMenLingDao.Text = entity.DeptLeader;
                }
                else if (base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF && !base.IsPreview)
                {
                    OAUser.GetUserByDeptID(this.ddlChengBanRen, entity.KeShiID, 1);
                    FormsMethod.SelectedDropDownList(this.ddlChengBanRen, entity.ChengBanRenID);
                    FormsMethod.SetDropDownList(this.ddlKeShiLingDao, entity.KeShiLingDaoID, entity.SectionLeader);

                    this.txtBuMenLingDao.Text = entity.DeptLeader;
                }
                else
                {
                    this.txtBuMenLingDao.Text = entity.DeptLeader;
                    FormsMethod.SetDropDownList(this.ddlKeShiLingDao, entity.KeShiLingDaoID, entity.SectionLeader);
                    FormsMethod.SetDropDownList(this.ddlChengBanRen, entity.ChengBanRenID, entity.Contractor);
                }

                //拟稿人
                if (entity.DraftDate != DateTime.MinValue)
                {
                    this.lblNiGaoRiQi.Text = entity.DraftDate.ToString(ConstString.DateFormat.Long);

                    this.txtNiGaoRen.Visible = false;
                    this.lbNiGaoRen.Visible = true;
                    this.lbNiGaoRen.Text = entity.Drafter + strNewLine + entity.DraftDate.ToString(ConstString.DateFormat.Long);
                }

                //签发人
                if (entity.ConfirmDate != DateTime.MinValue)
                {
                    this.txtQianFaRiQi.Text = entity.ConfirmDate.ToString(ConstString.DateFormat.Long);
                    this.txtQianFaRiQi.Visible = false;
                    this.ddlFuZeRen.Visible = false;
                    this.lbRiQiTitle.Visible = false;
                    this.lbLeader.Visible = true;
                    this.lbLeader.Text = entity.DeptPrincipal + strNewLine + entity.ConfirmDate.ToString(ConstString.DateFormat.Long);
                }

                //核稿人
                if (entity.CheckDate != DateTime.MinValue)
                {
                    this.txtHeGaoRen.Visible = false;
                    this.OASelectUC2.Visible = false;
                    this.lbHeGaoRen.Visible = true;
                    this.lbHeGaoRen.Text = entity.CheckDrafter + strNewLine + entity.CheckDate.ToString(ConstString.DateFormat.Long);
                }

                //编号
                this.txtBianHao.Text = entity.Number;

                //抄送
                this.txtChaoSong.Text = entity.CopySend;
                this.txtChaoSongIDs.Text = entity.ChaoSongID;
                this.txtChaoSongDeptIDs.Text = entity.ChaoSongBuMenID;

                //主题
                this.txtZhuTi.Text = entity.DocumentTitle;

                //内容
                //this.txtNeiRong.Text = SysString.HtmlToTextCode(entity.Content);
                this.txtNeiRong.Text =entity.Content;

                //办理意见
                this.txtChuLiYiJian.Text = entity.UndertakeCircs;
                this.txtBanLiYiJian.Text = entity.BanLiYiJian;

                //提示信息
                this.txtTiShiXinXi.Text = entity.Message;
                this.txtTianJia.Text = entity.MessageAdd;

                this.wfReceiveUserName.Text = entity.ReceiveUserName;//renjinquan+
                this.wfReceiveUserID.Text = entity.ReceiveUserID;
                //核稿人
                this.txtHeGaoRen.Text = entity.CheckDrafter;
                this.txtHeGaoRenID.Text = entity.HeGaoRenID;

                //拟稿人
                this.txtNiGaoRen.Text = entity.Drafter;
                this.txtNiGaoRenID.Text = entity.NiGaoRenID;

                //是否核稿或签发退回
                this.txtIsBack.Text = entity.IsBack.ToString();

                //传阅
                if (base.IsPreview == false)
                {
                    this.txtChuanYueRenYuan.Text = entity.IsFormSave == true ? entity.ChuanYueRenYuan : string.Empty;
                    this.txtChuanYueRenYuanID.Text = entity.IsFormSave == true ? entity.ChuanYueRenYuanID : string.Empty;
                }
                else
                {
                    this.txtChuanYueRenYuan.Text = entity.ChuanYueRenYuan;
                    this.txtChuanYueRenYuanID.Text = entity.ChuanYueRenYuanID;
                }

                //当前部门会签意见
                if (entity.IsFormSave && base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN)
                {
                    foreach (B_WorkRelation.DeptSign deptSign in entity.DeptSignList)
                    {
                        foreach (B_WorkRelation.DetailInfo detailInfo in deptSign.DetailInfoList)
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
                                    yiJian.ViewName = ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN;
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
                                    yiJian.ViewName = ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN;
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

                if (base.StepName == ProcessConstString.StepName.STEP_DRAFT)
                {
                    //部门会签
                    List<B_WorkRelation.YiJian> deptYiJianList = new List<B_WorkRelation.YiJian>();
                    foreach (B_WorkRelation.DeptSign deptSign in entity.DeptSignList)
                    {
                        foreach (B_WorkRelation.DetailInfo detailInfo in deptSign.DetailInfoList)
                        {
                            B_WorkRelation.YiJian yiJian = new B_WorkRelation.YiJian();

                            yiJian.Content = detailInfo.Comment;
                            yiJian.DealCondition = detailInfo.DealCondition;
                            yiJian.FinishTime = deptSign.SubmitDate.ToString();
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
                        //pnlDeptComment.Visible = true;
                        //rptDept.DataSource = deptYiJianList;
                        //rptDept.DataBind();
                    }
                }
            }
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
            B_WorkRelation entity = base.EntityData != null ? base.EntityData as B_WorkRelation : new B_WorkRelation();

            #region 提示信息、意见
            if (!IsSave)
            {
                if (!string.IsNullOrEmpty(this.txtTianJia.Text))
                {
                    entity.MessageAdd = string.Empty;
                    entity.Message = this.txtTiShiXinXi.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:(" + base.StepName + ")" + SysString.InputText(this.txtTianJia.Text) + "\n";
                }

                if (!string.IsNullOrEmpty(this.txtBanLiYiJian.Text))
                {
                    entity.BanLiYiJian = string.Empty;
                    entity.UndertakeCircs = this.txtChuLiYiJian.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:(" + base.StepName + ")" + SysString.InputText(this.txtBanLiYiJian.Text) + "\n";
                }
            }
            else
            {
                entity.Message = this.txtTiShiXinXi.Text;
                entity.MessageAdd = this.txtTianJia.Text;

                entity.UndertakeCircs = this.txtChuLiYiJian.Text;
                entity.BanLiYiJian = this.txtBanLiYiJian.Text;
            }
            #endregion

            //附件
            entity.FileList = ucAttachment.UCDataList;

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:
                    //拟稿日期
                    entity.DraftDate = DateTime.Now;

                    //拟稿人
                    entity.Drafter = this.txtNiGaoRen.Text;
                    entity.NiGaoRenID = this.txtNiGaoRenID.Text;
                    entity.DrafterID = this.txtNiGaoRenID.Text;

                    //主送
                    if (this.ddlZhuSong.Items.Count > 0)
                    {
                        entity.MainSend = this.ddlZhuSong.SelectedItem.Text;
                        entity.MainSendDeptID = this.ddlZhuSong.SelectedValue;
                    }

                    //编制部门
                    if (this.ddlBianZhiBuMen.Items.Count > 0)
                    {
                        entity.Department = this.ddlBianZhiBuMen.SelectedItem.Text;
                        entity.BianZhiBuMenID = this.ddlBianZhiBuMen.SelectedValue;
                    }

                    //抄送
                    entity.CopySend = this.txtChaoSong.Text;
                    entity.ChaoSongBuMenID = this.txtChaoSongDeptIDs.Text;
                    entity.ChaoSongID = this.txtChaoSongIDs.Text;

                    //标题、主题
                    entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);
                    //entity.Subject = this.txtZhuTi.Text;

                    //内容
                    //entity.Content = SysString.TextToHtmlCode(this.txtNeiRong.Text);
                    entity.Content = this.txtNeiRong.Text;

                    //部门负责人
                    if (this.ddlFuZeRen.Items.Count > 0)
                    {
                        entity.DeptPrincipal = this.ddlFuZeRen.SelectedItem.Text;
                        entity.FuZeRenID = this.ddlFuZeRen.SelectedValue;
                    }

                    //核稿人
                    entity.CheckDrafter = this.txtHeGaoRen.Text;
                    entity.HeGaoRenID = this.txtHeGaoRenID.Text;

                    //entity.IsSignReject = ConstString.Miscellaneous.STATUS_FALSE;

                    //选择部门会签
                    //entity.DeptSignList = rptDept.Items.Count > 0 ? B_WorkRelation.GetDeptSignList(rptDept, ucBuMenHuiQian.UCGetHQList()) : ucBuMenHuiQian.UCGetHQList();
                    entity.DeptSignList = ucBuMenHuiQian.UCGetHQList();
                    break;
                #endregion

                #region 核稿
                case ProcessConstString.StepName.WorkRelationStepName.STEP_CHECK:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //主送
                        if (this.ddlZhuSong.Items.Count > 0)
                        {
                            entity.MainSend = this.ddlZhuSong.SelectedItem.Text;
                            entity.MainSendDeptID = this.ddlZhuSong.SelectedItem.Value;
                        }

                        //抄送
                        entity.CopySend = this.txtChaoSong.Text;
                        entity.ChaoSongBuMenID = this.txtChaoSongDeptIDs.Text;
                        entity.ChaoSongID = this.txtChaoSongIDs.Text;

                        //主题
                        entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);

                        //内容
                        //entity.Content = SysString.TextToHtmlCode(this.txtNeiRong.Text);
                        entity.Content = this.txtNeiRong.Text;

                        //部门负责人
                        if (this.ddlFuZeRen.Items.Count > 0)
                        {
                            entity.DeptPrincipal = this.ddlFuZeRen.SelectedItem.Text;
                            entity.FuZeRenID = this.ddlFuZeRen.SelectedValue;
                        }

                        //选择部门会签
                        //entity.DeptSignList = rptDept.Items.Count > 0 ? B_WorkRelation.GetDeptSignList(rptDept, ucBuMenHuiQian.UCGetHQList()) : ucBuMenHuiQian.UCGetHQList();
                        entity.DeptSignList = ucBuMenHuiQian.UCGetHQList();
                    }
                    else
                    {
                        //退回，控制撤销按钮显示
                        entity.IsBack = true;
                    }

                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                    {
                        entity.TimesFlag = (int.Parse(string.IsNullOrEmpty(this.wfTimesFlag.Text) ? "1" : this.wfTimesFlag.Text) + 1).ToString();//次数标示
                    }
                    else
                    {
                        entity.TimesFlag = wfTimesFlag.Text;//次数标示
                    }
                    if (!IsSave)//renjinquan+ 核搞时间
                    {
                        entity.CheckDate = System.DateTime.Now;
                    }
                    entity.IsSignReject = ConstString.Miscellaneous.STATUS_FALSE;
                    break;
                #endregion

                #region 部门会签
                case ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN:
                    M_WorkRelation.DeptSign deptSign = new M_WorkRelation.DeptSign();
                    string strAgreeOld = string.Empty;
                    if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1" && wfReceiveUserID.Text != CurrentUserInfo.UserName)
                    {
                        foreach (M_WorkRelation.DeptSign signer in ucBuMenHuiQian.UCGetHQList())
                        {
                            if (signer.ID == wfReceiveUserID.Text && signer.IsExclude == false)
                            {
                                deptSign = signer;
                                strAgreeOld = deptSign.IsAgree;//记录上次的意见
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (M_WorkRelation.DeptSign signer in ucBuMenHuiQian.UCGetHQList())
                        {
                            if (signer.ID == CurrentUserInfo.UserName && signer.IsExclude == false)
                            {
                                deptSign = signer;
                                strAgreeOld = deptSign.IsAgree;//记录上次的意见
                                break;
                            }
                        }
                    }

                    //非保存操作
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                    {
                        deptSign.IsAgree = YiJianInfoList.Count == 0 ? ConstString.ProgramFile.PROGRAM_AGREE : ConstString.ProgramFile.PROGRAM_REJECT;
                        if (deptSign.IsAgree == ConstString.ProgramFile.PROGRAM_REJECT || deptSign.SubmitDate == DateTime.MinValue || strAgreeOld == ConstString.ProgramFile.PROGRAM_REJECT)
                        {
                            deptSign.SubmitDate = DateTime.Now;
                        }
                        //判断是否会签退回（如果为同意，则需判断当前会签结果状态，否则为拒绝）
                        entity.IsSignReject = entity.IsSignReject == ConstString.Miscellaneous.STATUS_FALSE ? deptSign.IsAgree == ConstString.ProgramFile.PROGRAM_AGREE ?
                                                ConstString.Miscellaneous.STATUS_FALSE : ConstString.Miscellaneous.STATUS_TRUE : ConstString.Miscellaneous.STATUS_TRUE;

                        //在CommentList添加当前意见
                        entity.CommentList.Clear();
                        foreach (CYiJian objYj in YiJianInfoList)
                        {
                            objYj.FinishTime = DateTime.Now.ToString();
                        }
                        entity.CommentList = YiJianInfoList;
                    }

                    List<M_WorkRelation.DetailInfo> detailInfoList = new List<M_WorkRelation.DetailInfo>();
                    string strComment = string.Empty;//单条会签意见，用于表单列表显示

                    foreach (RepeaterItem item in this.rptComment.Items)
                    {
                        M_WorkRelation.DetailInfo detailInfo = new M_WorkRelation.DetailInfo();
                        Label lblContent = item.FindControl("lblContent") as Label;
                        detailInfo.Comment = lblContent.Text;

                        strComment = lblContent.Text;
                        detailInfoList.Add(detailInfo);
                    }

                    //deptSign.DealCondition = string.Empty;//清空处理情况
                    //deptSign.DealDate = DateTime.MinValue;//清空处理日期
                    deptSign.TBID = base.IdentityID.ToString();
                    deptSign.Comment = SysString.InputText(strComment);
                    deptSign.DetailInfoList = detailInfoList;//会签信息集合

                    entity.DeptSignList = B_WorkRelation.SetDeptSignList(deptSign, ucBuMenHuiQian.UCGetHQList());
                    break;
                #endregion

                #region 签发
                case ProcessConstString.StepName.WorkRelationStepName.STEP_SIGN:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //主送
                        if (this.ddlZhuSong.Items.Count > 0)
                        {
                            entity.MainSend = this.ddlZhuSong.SelectedItem.Text;
                            entity.MainSendDeptID = this.ddlZhuSong.SelectedItem.Value;
                        }

                        //抄送
                        entity.CopySend = this.txtChaoSong.Text;
                        entity.ChaoSongBuMenID = this.txtChaoSongDeptIDs.Text;
                        entity.ChaoSongID = this.txtChaoSongIDs.Text;

                        //主题
                        entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);

                        //内容
                        //entity.Content = SysString.TextToHtmlCode(this.txtNeiRong.Text);
                        entity.Content = this.txtNeiRong.Text;

                        //签发日期
                        if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_QF)
                        {
                            entity.ConfirmDate = DateTime.Now;
                        }

                        //部门领导(多人)
                        //string[] leaders = OAUser.GetDeptLeaderArray(entity.MainSendDeptID, 0);
                        string[] leaders = OAUser.GetUserByDeptPostArray(entity.MainSendDeptID, OUConstString.PostName.FUKEZHANG, true, true);
                        entity.ZhuSongID = leaders[0];

                        //编号
                        if (this.ddlBianZhiBuMen.Items.Count > 0)
                        {
                            entity.Number = WRRRNum.GetHNCode(ProcessConstString.TemplateName.AFFILIATION, this.ddlBianZhiBuMen.SelectedItem.Text);
                            //entity.No = entity.Number.Substring(entity.Number.Length - 5);
                            entity.DocumentNo = entity.Number;
                        }
                    }
                    else
                    {
                        entity.IsBack = true;
                    }

                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                    {
                        entity.TimesFlag = (int.Parse(string.IsNullOrEmpty(this.wfTimesFlag.Text) ? "1" : this.wfTimesFlag.Text) + 1).ToString();//次数标示
                    }
                    else
                    {
                        entity.TimesFlag = wfTimesFlag.Text;//次数标示
                    }
                    break;
                #endregion

                #region 处室承办
                case ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR:
                    if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_JBKS)
                    {
                        entity.ChengBanRenID = string.Empty;
                        entity.Contractor = string.Empty;
                        entity.MemberDate = DateTime.MinValue;

                        if (this.ddlKeShiLingDao.Items.Count > 0)
                        {
                            //科室
                            entity.KeShiID = this.ddlKeShiLingDao.SelectedValue;
                            entity.KeShi = this.ddlKeShiLingDao.SelectedItem.Text;

                            string[] id = OAUser.GetDeptManagerArray(this.ddlKeShiLingDao.SelectedValue, 0);
                            entity.KeShiLingDaoID = id[0].ToString();
                            entity.SectionLeader = id[1].ToString();
                        }
                    }

                    if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_JBRY)
                    {
                        entity.KeShiID = string.Empty;
                        entity.KeShi = string.Empty;
                        entity.KeShiLingDaoID = string.Empty;
                        entity.SectionLeader = string.Empty;
                        entity.SectionDate = DateTime.MinValue;

                        if (this.ddlChengBanRen.Items.Count > 0)
                        {
                            entity.ChengBanRenID = this.ddlChengBanRen.SelectedValue;
                            entity.Contractor = this.ddlChengBanRen.SelectedItem.Text;
                        }
                    }

                    //部门领导
                    entity.BuMenLingDaoID = this.txtBuMenLingDaoID.Text;
                    entity.DeptLeader = this.txtBuMenLingDao.Text;

                    //承办日期
                    entity.DirectorDate = DateTime.Now;

                    //传阅
                    entity.ChuanYueRenYuan = this.txtChuanYueRenYuan.Text;
                    entity.ChuanYueRenYuanID = this.txtChuanYueRenYuanID.Text;
                    break;
                #endregion

                #region 科室承办
                case ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF:
                    //直接承办人
                    if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_JBRY)
                    {
                        if (this.ddlChengBanRen.Items.Count > 0)
                        {
                            entity.ChengBanRenID = this.ddlChengBanRen.SelectedValue;
                            entity.Contractor = this.ddlChengBanRen.SelectedItem.Text;
                        }
                    }

                    entity.SectionDate = DateTime.Now;
                    entity.ChuanYueRenYuan = this.txtChuanYueRenYuan.Text;
                    entity.ChuanYueRenYuanID = this.txtChuanYueRenYuanID.Text;
                    break;
                #endregion

                #region 人员承办
                case ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER:
                    entity.MemberDate = DateTime.Now;
                    entity.ChuanYueRenYuan = this.txtChuanYueRenYuan.Text;
                    entity.ChuanYueRenYuanID = this.txtChuanYueRenYuanID.Text;
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
            this.btnSave3.Click += SubmitHandler;
            //提交签发
            this.btnTiJiaoQianFa.Click += SubmitHandler;
            //提交核稿
            this.btnTiJiaoHeGao.Click += SubmitHandler;
            //交办科室
            this.btnJiaoBanKeShi.Click += SubmitHandler;
            //交办人员
            this.btnJiaoBanRenYuan.Click += SubmitHandler;
            //完成
            this.btnWanCheng.Click += SubmitHandler;
            //撤销
            this.btnCancel.Click += SubmitHandler;
            //签发
            this.btnFenFa.Click += SubmitHandler;
            //退回
            this.btnTuiHui.Click += SubmitHandler;
            //部门会签
            this.btnDeptSign.Click += SubmitHandler;
            //添加意见 添加
            this.btnAdd.Click += SubmitHandler;
            //添加意见 取消
            this.btnCancel1.Click += SubmitHandler;
            //添加意见 确定
            this.btnConfirm.Click += SubmitHandler;
            //提交（多条意见）
            this.btnSubmits.Click += SubmitHandler;
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
                base.SubAction = ((Button)sender).Text.Trim();

                string strErrorMessage = string.Empty;

                #region 添加多条意见
                //添加意见按钮事件
                if (base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_ADD_COMMENT)
                {
                    this.pnlComment.Visible = true;
                    this.btnCancel1.Visible = true;
                    this.btnConfirm.Visible = true;
                    this.btnAdd.Visible = false;
                    return;
                }
                //确定按钮事件
                if (base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_CONFIRM)
                {
                    if (string.IsNullOrEmpty(txtInfo2.Text.Trim()))
                    {
                        JScript.Alert(ConstString.PromptInfo.ACTION_CHECK_ADD_COMMENT, true);
                        return;
                    }
                    if (txtInfo2.Text.Trim().Length > 100)
                    {
                        JScript.Alert(ConstString.PromptInfo.ACTION_CHECK_CONTENT_LEN100, true);
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
                        objYj.DeptID = base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN ? B_WorkRelation.GetDeptIDByUserID(ucBuMenHuiQian.UCGetHQList(), objYj.UserID) : string.Empty;

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
                if (base.SubAction == ProcessConstString.SubmitAction.ProgramFile.ACTION_CANCLE)
                {
                    this.pnlComment.Visible = false;
                    this.btnCancel1.Visible = false;
                    this.btnConfirm.Visible = false;
                    this.btnAdd.Visible = true;
                    txtInfo2.Text = string.Empty;
                    hfEditIndex.Value = string.Empty;
                    return;
                }
                #endregion

                //#region 处理落实情况
                ////编写节点 处理 质保、部门会签、领导会签、批准意见
                //if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && pnlDealSign.Visible == true)
                //{
                //    B_WorkRelation bWorkRelation = new B_WorkRelation();

                //    List<M_WorkRelation.DeptSign> deptSignList = B_WorkRelation.GetDeptSignList(rptDept, ucBuMenHuiQian.UCGetHQList());//部门会签意见
                //    List<B_WorkRelation> entityList = B_WorkRelation.GetpfEntityList(deptSignList);
                //    if (!bWorkRelation.EnTransSave(entityList))
                //    {
                //        JScript.Alert(ConstString.PromptInfo.ACTION_SUBMIT_DEAL_FAIL, true);
                //        return;
                //    }//更新当前提交的意见落实情况

                //}
                //#endregion

                //保存
                if (base.SubAction == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_WorkRelation entity = ControlToEntity(true) as B_WorkRelation;
                    entity.SubmitAction = base.SubAction;
                    base.FormSubmit(true, base.SubAction, null, entity);
                }
                else
                {
                    B_WorkRelation entity = ControlToEntity(false) as B_WorkRelation;
                    entity.SubmitAction = base.SubAction;

                    //撤销
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_CANCEL)
                    {
                        base.FormCancel(entity);
                    }
                    else
                    {
                        //返回验证提示和流程提示
                        entity.GetSubmitMessage(base.StepName, base.SubAction, ref strErrorMessage);
                        if (!string.IsNullOrEmpty(strErrorMessage))
                        {
                            JScript.Alert(strErrorMessage, false);
                            return;
                        }
                        else
                        {
                            switch (base.StepName)
                            {
                                case ProcessConstString.StepName.STEP_DRAFT:
                                    if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_TJQF)
                                    {
                                        if (!string.IsNullOrEmpty(entity.HeGaoRenID))
                                        {
                                            if (!B_WorkRelation.IsHaveChecked(base.ProcessID, entity.HeGaoRenID))
                                            {
                                                JScript.Alert("核稿人未处理过核稿，请提交核稿或清空核稿人", false);
                                                return;
                                            }
                                        }
                                    }
                                    break;

                                case ProcessConstString.StepName.WorkRelationStepName.STEP_SIGN:
                                    if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_QF &&
                                       (!string.IsNullOrEmpty(entity.ChaoSongBuMenID) || !string.IsNullOrEmpty(entity.ChaoSongID)))
                                    {
                                        base.Circulate(entity.ChaoSongBuMenID, "1", string.Empty, entity.ChaoSongID, "1", false, string.Empty, false);
                                    }
                                    break;

                                case ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR:
                                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_COMPLETE)
                                    {
                                        //传阅给编校审及“抄送”
                                        base.Circulate(entity.ChaoSongBuMenID, "1", string.Empty, FormsMethod.FilterRepeat(entity.ChaoSongID + ";" +
                                            entity.DrafterID + ";" + entity.HeGaoRenID + ";" + entity.FuZeRenID), "1", true, string.Empty, false);

                                        //工作联系单归档
                                        //Devolve();
                                    }
                                    //处室承办可选传阅
                                    else if (!string.IsNullOrEmpty(entity.ChuanYueRenYuanID))
                                    {
                                        base.Circulate(string.Empty, string.Empty, string.Empty, entity.ChuanYueRenYuanID, "1", false, string.Empty, false);
                                    }
                                    break;

                                case ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF:
                                    if (!string.IsNullOrEmpty(entity.ChuanYueRenYuanID))
                                    {
                                        base.Circulate(string.Empty, string.Empty, string.Empty, entity.ChuanYueRenYuanID, "1", false, string.Empty, false);
                                    }
                                    break;

                                case ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER:
                                    if (!string.IsNullOrEmpty(entity.ChuanYueRenYuanID))
                                    {
                                        base.Circulate(string.Empty, string.Empty, string.Empty, entity.ChuanYueRenYuanID, "1", false, string.Empty, false);
                                    }
                                    break;
                            }

                            //调用工作流                  
                            Hashtable nValues = entity.GetProcNameValue(base.StepName, base.SubAction);
                            base.FormSubmit(false, base.SubAction, nValues, entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JScript.Alert(ex.Message, false);
                return;
            }
        }
        #endregion

        protected void ddlBianZhiBuMen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //部门负责人
            if (this.ddlBianZhiBuMen.Items.Count > 0)
            {
                OAUser.GetUserByDeptPost(this.ddlFuZeRen, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.FUCHUZHANG, true, true);
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

        #region 部门会签列表绑定行
        /// <summary>
        /// 部门会签列表绑定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptDept_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (base.IsPreview)
                {
                    HtmlAnchor l_htmlAnchor = e.Item.FindControl("htmlAnchor") as HtmlAnchor;
                    l_htmlAnchor.Visible = false;
                }
                else
                {
                    TextBox txtCondition = e.Item.FindControl("txtCondition") as TextBox;
                    txtCondition.Attributes.Add("readOnly", "true");
                    HtmlAnchor l_htmlAnchor = e.Item.FindControl("htmlAnchor") as HtmlAnchor;
                    l_htmlAnchor.Attributes.Add("onclick", "OpenConditionDialog(document.getElementById('" + txtCondition.ClientID + "').value,'" + txtCondition.ClientID + "')");
                }
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
    }
}