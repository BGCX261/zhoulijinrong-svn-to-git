//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：请示报告视图
// 
// 
// 创建标识：wangbinyi 2009-12-28
//
// 修改标识：任金权 2010-5-10
// 修改描述：修改EntityToControl、ControlToEntity。去除使用HtmlToTextCode、HtmlToTextCode。不需要使用，会统一处理
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

namespace FS.ADIM.OA.WebUI.WorkFlow.RequestReport
{
    public partial class UC_RequestReport : FormsUIBase
    {
        //客户可能会改
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string strNewLine = "<br/>";

        #region 页面加载
        protected string FileTitle = "海南核电有限公司请示";

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
            B_RequestReport entity = base.EntityData != null ? base.EntityData as B_RequestReport : new B_RequestReport();

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

            //核稿人
            this.OASelectUC3.UCSelectType = "1";
            this.OASelectUC3.UCDeptUserIDControl = this.txtHeGaoRenID.ClientID;
            this.OASelectUC3.UCDeptUserNameControl = this.txtHeGaoRen.ClientID;
            this.OASelectUC3.UCIsSingle = "1";
            this.OASelectUC3.UCTemplateName = base.TemplateName;
            this.OASelectUC3.UCFormName = "核稿人";

            if (this.ddlBianZhiBuMen.Items.Count > 0)
            {
                this.OASelectUC3.UCShowDeptID = OADept.GetChildDeptIDSConSelf(this.ddlBianZhiBuMen.SelectedValue, -1);
            }
            this.OASelectUC3.UCLevel = "3";

            //传阅
            this.OASelectUC4.UCDeptIDControl = this.hDeptID.ClientID;
            this.OASelectUC4.UCDeptNameControl = this.txtDeptName.ClientID;
            this.OASelectUC4.UCRole = OUConstString.RoleName.COMPANY_LEADER;
            this.OASelectUC4.UCRoleUserIDControl = this.hUserID.ClientID;
            this.OASelectUC4.UCRoleUserNameControl = this.txtUserName.ClientID;
            this.OASelectUC4.UCSelectType = "0";
            this.OASelectUC4.UCDeptShowType = "1010";

            //承办部门
            this.OASelectUC5.UCSelectType = "0";
            this.OASelectUC5.UCDeptIDControl = this.txtChengBanBuMenID.ClientID;
            this.OASelectUC5.UCDeptNameControl = this.txtChengBanBuMen.ClientID;
            this.OASelectUC5.UCIsSingle = "1";
            this.OASelectUC5.UCLevel = "1";
            this.OASelectUC5.UCDeptShowType = "1010";

            #endregion

            OAControl controls = new OAControl();

            if (!base.IsPreview)
            {
                this.txtZhuTi.ToolTip = "50字符以内";
                this.txtNeiRong.ToolTip = "2000字符以内";
                this.txtPiShiYiJian.ToolTip = "2000字符以内";
                this.txtChengBanBuMen.ToolTip = "承办部门(处室)";
                this.ddlZhuSong.ToolTip = "公司领导";
                this.ddlFuZeRen.ToolTip = "部门负责人、部门领导、大于副处长";
                this.ddlKeShiLingDao.ToolTip = "科室（部门负责人）";
                this.ddlChengBanRen.ToolTip = "处室承办（处室下所有人包括科室），科室承办（科室下所有人）";
                this.ddlBianZhiBuMen.ToolTip = "自己所属的处室";
                this.txtHeGaoRen.ToolTip = this.ddlBianZhiBuMen.Items.Count > 0 ? (this.ddlBianZhiBuMen.SelectedItem.Text + "的成员") : string.Empty;
            }

            if (this.ddlType.SelectedValue == "请示")
            {
                FileTitle = "海南核电有限公司请示";
            }
            else
            {
                FileTitle = "海南核电有限公司报告";
            }

            //DateTime DirectorDate = DateTime.MinValue;
            //DateTime ChiefDate = DateTime.MinValue;
            //DateTime MemberDate = DateTime.MinValue;
            //string strDirector = string.Empty;
            //string strChief = string.Empty;
            //string strMember = string.Empty;
            //DataTable dt1 = null;
            //DataTable dt2 = null;
            //DataTable dt3 = null;

            ////承办时获取承办人和时间(兼容以前用)
            //if (base.StepName == ProcessConstString.StepName.RequestReportStepName.STEP_DIRECTOR ||
            //    base.StepName == ProcessConstString.StepName.RequestReportStepName.STEP_CHIEF ||
            //    base.StepName == ProcessConstString.StepName.RequestReportStepName.STEP_MEMBER)
            //{
            //    dt1 = FormsMethod.GetUndertakeInfo(base.ProcessID, ProcessConstString.StepName.RequestReportStepName.STEP_DIRECTOR, ProcessConstString.TemplateName.INSTUCTION_REPORT);
            //    dt2 = FormsMethod.GetUndertakeInfo(base.ProcessID, ProcessConstString.StepName.RequestReportStepName.STEP_CHIEF, ProcessConstString.TemplateName.INSTUCTION_REPORT);
            //    dt3 = FormsMethod.GetUndertakeInfo(base.ProcessID, ProcessConstString.StepName.RequestReportStepName.STEP_MEMBER, ProcessConstString.TemplateName.INSTUCTION_REPORT);

            //    if (dt1.Rows.Count > 0)
            //    {
            //        DirectorDate = Convert.ToDateTime(dt1.Rows[0][1].ToString());
            //        strDirector = dt1.Rows[0][0].ToString();
            //    }

            //    if (dt2.Rows.Count > 0)
            //    {
            //        ChiefDate = Convert.ToDateTime(dt2.Rows[0][1].ToString());
            //        strChief = dt2.Rows[0][0].ToString();
            //    }

            //    if (dt3.Rows.Count > 0)
            //    {
            //        MemberDate = Convert.ToDateTime(dt3.Rows[0][1].ToString());
            //        strMember = dt3.Rows[0][0].ToString();
            //    }
            //}

            if (!base.IsPreview)
            {
                switch (base.StepName)
                {
                    #region 拟稿
                    case ProcessConstString.StepName.STEP_DRAFT:
                        this.btnCancel.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");

                        //控制撤销按钮显示
                        this.btnCancel.Visible = this.txtIsBack.Text == "True";

                        this.txtNiGaoRen.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                        this.txtNiGaoRenID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;

                        this.ddlType.Attributes.Add("onchange", "SelectedChanged()");
                        if (string.IsNullOrEmpty(base.WorkItemID))
                        {
                            this.lbJs.Text = "<script>ShowMyDiv();</script>";
                        }

                        controls.EnableControls = new Control[] { this.OASelectUC1, this.OASelectUC3, this.btnTiJiaoQianFa, this.btnTiJiaoHeGao, this.btnSave };
                        controls.YellowControls = new Control[] { this.txtHeGaoRen, this.txtChaoSong };
                        break;
                    #endregion

                    #region 核稿
                    case ProcessConstString.StepName.RequestReportStepName.STEP_CHECK:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnTiJiaoQianFa, this.btnTuiHui, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.ddlBianZhiBuMen, this.txtHeGaoRen };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 签发
                    case ProcessConstString.StepName.RequestReportStepName.STEP_SIGN:
                        this.btnFenFa.Attributes.Add("onclick", "javascript: if(!checkChaoSong()){return false;}else{DisableButtons();}");

                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnFenFa, this.btnTuiHui, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.ddlBianZhiBuMen, this.ddlFuZeRen, this.txtHeGaoRen };
                        controls.YellowControls = new Control[] { this.txtChaoSong };
                        break;
                    #endregion

                    #region 部门会签

                    #endregion

                    #region 主管领导审核

                    #endregion

                    #region 秘书审核
                    case ProcessConstString.StepName.RequestReportStepName.STEP_MSVERIFY:
                        controls.EnableControls = new Control[] { this.btnLingDaoPiShi, this.btnZhuRenShenHe, this.btnTuiHui, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen };
                        break;
                    #endregion

                    #region 主任审核
                    case ProcessConstString.StepName.RequestReportStepName.STEP_ZRVERIFY:
                        controls.EnableControls = new Control[] { this.btnTongGuoShenHe, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen };
                        break;
                    #endregion

                    #region 领导批示
                    case ProcessConstString.StepName.RequestReportStepName.STEPNAME_INSTRUCTION:
                        controls.EnableControls = new Control[] { this.TdYiJian };
                        controls.DisEnableControls = new Control[] { this.TaTishi, this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen, this.txtTianJia };
                        break;
                    #endregion

                    #region 秘书分发
                    case ProcessConstString.StepName.RequestReportStepName.STEP_MSFF:
                        this.btnChengBanFenFa.Attributes.Add("onclick", "javascript: if(!checkChuanYue()){return false;}else{DisableButtons();}");

                        controls.EnableControls = new Control[] { this.OASelectUC4, this.OASelectUC5, this.tbChuanYue, this.btnChengBanFenFa, this.btnLingDaoPiShi, this.btnSave, this.txtChengBanBuMen };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen };
                        controls.YellowControls = new Control[] { this.txtDeptName, this.txtUserName };
                        break;
                    #endregion

                    #region 处室承办
                    case ProcessConstString.StepName.RequestReportStepName.STEP_DIRECTOR:
                        //获取任务的部门领导
                        this.txtBuMenLingDao.Text = string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName; ;
                        this.txtBuMenLingDaoID.Text = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                        controls.EnableControls = new Control[] { this.btnJiaoBanKeshi, this.btnJiaoBanRenYuan, this.btnWanCheng, this.btnSave, this.TrBanLiYiJian, this.TrChenBan };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen };
                        break;
                    #endregion

                    #region 科室承办
                    case ProcessConstString.StepName.RequestReportStepName.STEP_CHIEF:
                        if (entity.DirectorDate != DateTime.MinValue)
                        {
                            this.txtBuMenLingDao.Visible = false;
                            this.lblDirector.Visible = true;
                            this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(DateFormat);
                        }
                        //else
                        //{
                        //    if (DirectorDate != DateTime.MinValue)
                        //    {
                        //        this.txtBuMenLingDao.Visible = false;
                        //        this.lblDirector.Visible = true;
                        //        this.lblDirector.Text = OAUser.GetUserName(strDirector) + strNewLine + DirectorDate.ToString(ConstString.DateFormat.Normal);
                        //    }
                        //}

                        controls.EnableControls = new Control[] { this.btnJiaoBanRenYuan, this.btnWanCheng, this.btnSave, this.TrBanLiYiJian, this.TrChenBan };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen, this.ddlKeShiLingDao };
                        break;
                    #endregion

                    #region 人员承办
                    case ProcessConstString.StepName.RequestReportStepName.STEP_MEMBER:
                        if (entity.DirectorDate != DateTime.MinValue)
                        {
                            this.txtBuMenLingDao.Visible = false;
                            this.lblDirector.Visible = true;
                            this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(DateFormat);
                        }
                        //else
                        //{
                        //    //老数据
                        //    if (DirectorDate != DateTime.MinValue)
                        //    {
                        //        this.txtBuMenLingDao.Visible = false;
                        //        this.lblDirector.Visible = true;
                        //        this.lblDirector.Text = OAUser.GetUserName(strDirector) + strNewLine + DirectorDate.ToString(ConstString.DateFormat.Normal);
                        //    }
                        //}

                        if (entity.SectionDate != DateTime.MinValue)
                        {
                            this.ddlKeShiLingDao.Visible = false;
                            this.lblSection.Visible = true;
                            this.lblSection.Text = entity.SectionLeader + strNewLine + entity.SectionDate.ToString(DateFormat);
                        }
                        //else
                        //{
                        //    if (ChiefDate != DateTime.MinValue)
                        //    {
                        //        this.ddlKeShiLingDao.Visible = false;
                        //        this.lblSection.Visible = true;
                        //        this.lblSection.Text = OAUser.GetUserName(strChief) + strNewLine + ChiefDate.ToString(ConstString.DateFormat.Normal);
                        //    }
                        //}

                        controls.EnableControls = new Control[] { this.btnWanCheng, this.btnSave, this.TrBanLiYiJian, this.TrChenBan };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen, this.ddlKeShiLingDao, this.ddlChengBanRen };
                        break;
                    #endregion

                    #region 承办审核
                    case ProcessConstString.StepName.RequestReportStepName.STEP_CBVERIFY:
                        this.btnWanChengGuiDang.Attributes.Add("onclick", "javascript: if(!checkFenFa()){return false;}else{DisableButtons();}");

                        controls.EnableControls = new Control[] { this.OASelectUC2, this.btnWanChengGuiDang, this.btnSave, this.TrChuanYue };
                        controls.DisEnableControls = new Control[] { this.ddlZhuSong, this.ddlBianZhiBuMen, this.txtChaoSong, this.txtZhuTi, this.txtNeiRong, this.ddlFuZeRen, this.txtHeGaoRen, this.txtTianJia };
                        controls.YellowControls = new Control[] { this.txtChuanYueRenYuan };
                        break;
                    #endregion
                }

                //设置所有控件状态
                controls.SetControls();
            }
            else
            {
                ucAttachment.UCIsEditable = false;

                FormsMethod.SetControlAll(this);

                #region 已承办的显示在lable上
                if (base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR ||
                                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF ||
                                    base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER)
                {
                    if (entity.DirectorDate != DateTime.MinValue)
                    {
                        this.txtBuMenLingDao.Visible = false;
                        this.lblDirector.Visible = true;
                        this.lblDirector.Text = entity.DeptLeader + strNewLine + entity.DirectorDate.ToString(DateFormat);
                    }
                    //else
                    //{
                    //    //老数据
                    //    if (DirectorDate != DateTime.MinValue)
                    //    {
                    //        this.txtBuMenLingDao.Visible = false;
                    //        this.lblDirector.Visible = true;
                    //        this.lblDirector.Text = OAUser.GetUserName(strDirector) + strNewLine + DirectorDate.ToString(ConstString.DateFormat.Normal);
                    //    }
                    //}

                    if (entity.SectionDate != DateTime.MinValue)
                    {
                        this.ddlKeShiLingDao.Visible = false;
                        this.lblSection.Visible = true;
                        this.lblSection.Text = entity.SectionLeader + strNewLine + entity.SectionDate.ToString(DateFormat);
                    }
                    //else
                    //{
                    //    if (ChiefDate != DateTime.MinValue)
                    //    {
                    //        this.ddlKeShiLingDao.Visible = false;
                    //        this.lblSection.Visible = true;
                    //        this.lblSection.Text = OAUser.GetUserName(strChief) + strNewLine + ChiefDate.ToString(ConstString.DateFormat.Normal);
                    //    }
                    //}

                    if (entity.MemberDate != DateTime.MinValue)
                    {
                        this.ddlChengBanRen.Visible = false;
                        this.lblMember.Visible = true;
                        this.lblMember.Text = entity.Contractor + strNewLine + entity.MemberDate.ToString(DateFormat);
                    }
                    //else
                    //{
                    //    if (MemberDate != DateTime.MinValue)
                    //    {
                    //        this.ddlChengBanRen.Visible = false;
                    //        this.lblMember.Visible = true;
                    //        this.lblMember.Text = OAUser.GetUserName(strMember) + strNewLine + MemberDate.ToString(ConstString.DateFormat.Normal);
                    //    }
                    //}
                }
                #endregion

                switch (base.StepName)
                {
                    #region 领导批示
                    case ProcessConstString.StepName.RequestReportStepName.STEPNAME_INSTRUCTION:
                        this.TaTishi.Visible = false;
                        this.TdYiJian.Visible = true;
                        break;
                    #endregion

                    #region 秘书分发
                    case ProcessConstString.StepName.RequestReportStepName.STEP_MSFF:
                        this.tbChuanYue.Visible = true;
                        break;
                    #endregion

                    #region 处室承办
                    case ProcessConstString.StepName.RequestReportStepName.STEP_DIRECTOR:
                        this.tbChuanYue.Visible = true;
                        this.TrChenBan.Visible = true;

                        if (entity.DirectorDate != DateTime.MinValue)
                        {
                            this.txtChengBanRiQi.Text = entity.DirectorDate.ToString(DateFormat);
                        }
                        break;
                    #endregion

                    #region 科室承办
                    case ProcessConstString.StepName.RequestReportStepName.STEP_CHIEF:
                        this.tbChuanYue.Visible = true;
                        this.TrChenBan.Visible = true;

                        if (entity.SectionDate != DateTime.MinValue)
                        {
                            this.txtChengBanRiQi.Text = entity.SectionDate.ToString(DateFormat);
                        }
                        break;
                    #endregion

                    #region 人员承办
                    case ProcessConstString.StepName.RequestReportStepName.STEP_MEMBER:
                        this.tbChuanYue.Visible = true;
                        this.TrChenBan.Visible = true;

                        if (entity.MemberDate != DateTime.MinValue)
                        {
                            this.txtChengBanRiQi.Text = entity.MemberDate.ToString(DateFormat);
                        }
                        break;
                    #endregion

                    #region 承办审核
                    case ProcessConstString.StepName.RequestReportStepName.STEP_CBVERIFY:
                        this.TrChuanYue.Visible = true;

                        //查看自己办理公办
                        if (base.EntryAction == "4")
                        {
                            this.btnZhuiJiaFenFa.Visible = true;
                        }
                        break;
                    #endregion
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
            B_RequestReport entity = base.EntityData != null ? base.EntityData as B_RequestReport : new B_RequestReport();

            //新版OA时间精确到秒（控制时间的显示格式）
            //bool isOld = entity.DraftDate < base.OAStartTime;

            //附件
            ucAttachment.UCDataList = entity.FileList;

            //主送
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT ||
                base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHECK ||
                base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_SIGN && !base.IsPreview)
            {
                OAUser.GetUserByRole(this.ddlZhuSong, OUConstString.RoleName.COMPANY_LEADER);
                FormsMethod.SelectedDropDownList(this.ddlZhuSong, entity.ZhuSongID);
            }
            else
            {
                FormsMethod.SetDropDownList(this.ddlZhuSong, entity.ZhuSongID, entity.MainSend);
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
                OADept.GetChildDept(this.ddlKeShiLingDao, entity.ChengBanBuMenID, 2, true, true);
                OAUser.GetUserByDeptID(this.ddlChengBanRen, entity.ChengBanBuMenID, -1);
                FormsMethod.SelectedDropDownList(this.ddlKeShiLingDao, entity.KeShiID);
                FormsMethod.SelectedDropDownList(this.ddlChengBanRen, entity.ChengBanRenID);

                //部门领导
                this.txtBuMenLingDao.Text = entity.DeptLeader;
            }
            else if (base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF && !base.IsPreview)
            {
                OAUser.GetUserByDeptID(this.ddlChengBanRen, entity.KeShiID, 1);
                FormsMethod.SelectedDropDownList(this.ddlChengBanRen, entity.ChengBanRenID);

                //科室领导
                FormsMethod.SetDropDownList(this.ddlKeShiLingDao, entity.KeShiLingDaoID, entity.SectionLeader);

                //部门领导
                this.txtBuMenLingDao.Text = entity.DeptLeader;
            }
            else
            {
                this.txtBuMenLingDao.Text = entity.DeptLeader;
                FormsMethod.SetDropDownList(this.ddlKeShiLingDao, entity.KeShiLingDaoID, entity.SectionLeader);
                FormsMethod.SetDropDownList(this.ddlChengBanRen, entity.ChengBanRenID, entity.Contractor);
            }

            //承办日期
            if (entity.ChengBanRiQi != DateTime.MinValue)
            {
                this.txtChengBanRiQi.Text = entity.ChengBanRiQi.ToString(DateFormat);
            }

            //是否退回
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT)
            {
                this.txtIsBack.Text = entity.IsBack.ToString();
            }

            //if (isOld == true)
            //{
            //    //拟稿日期
            //    if (entity.DraftDate != DateTime.MinValue)
            //    {
            //        this.lblNiGaoRiQi.Text = entity.DraftDate.ToString(ConstString.DateFormat.Normal);
            //    }

            //    //签发日期
            //    if (entity.ConfirmDate != DateTime.MinValue)
            //    {
            //        this.txtQianFaRiQi.Text = entity.ConfirmDate.ToString(ConstString.DateFormat.Normal);
            //    }
            //}
            //else
            //{
            //拟稿人及日期
            if (entity.DraftDate != DateTime.MinValue)
            {
                this.lblNiGaoRiQi.Text = entity.DraftDate.ToString(DateFormat);

                //拟稿人显示非下拉列表框形式
                this.txtNiGaoRen.Visible = false;
                this.lbNiGaoRen.Visible = true;
                this.lbNiGaoRen.Text = entity.Drafter + strNewLine + entity.DraftDate.ToString(ConstString.DateFormat.Long);

            }

            //核稿人
            if (entity.CheckDate != DateTime.MinValue)
            {
                //核稿人签发后显示非文本框形式
                this.txtHeGaoRen.Visible = false;
                this.lbHeGaoRen.Visible = true;
                this.lbHeGaoRen.Text = entity.CheckDrafter + strNewLine + entity.CheckDate.ToString(ConstString.DateFormat.Long);

            }

            //部门负责人
            if (entity.ConfirmDate != DateTime.MinValue)
            {
                this.txtQianFaRiQi.Text = entity.ConfirmDate.ToString(DateFormat);
                this.txtQianFaRiQi.Visible = false;
                this.lbQianFaRiQi.Visible = true;
                this.lbQianFaRiQi.Text = entity.ConfirmDate.ToString(DateFormat);
                //部门负责人签发后显示非下拉列表框形式

                this.lbLeader.Text = entity.ConfirmDate.ToString(ConstString.DateFormat.Long);

                this.ddlFuZeRen.Visible = false;
                this.lbLeader.Visible = true;
                this.lbLeader.Text = entity.DeptLeader;// + strNewLine + entity.ConfirmDate.ToString(ConstString.DateFormat.Long);
            }

            if (base.StepName == ProcessConstString.StepName.RequestReportStepName.STEP_CBVERIFY)
            {
                this.txtChuanYueRenYuan.Text = entity.ChuanYueRenYuan;
                this.txtChuanYueRenIDs.Text = entity.ChuanYueRenYuanID;
            }

            //抄送
            this.txtChaoSong.Text = entity.CopySend;
            this.txtChaoSongID.Text = entity.ChaoSongID;
            this.txtChaoSongDeptID.Text = entity.ChaoSongDeptID;

            //主题
            //this.txtZhuTi.Text = entity.Subject;
            this.txtZhuTi.Text = entity.DocumentTitle;

            //编号
            this.txtBianHao.Text = entity.Number;

            //内容
            //this.txtNeiRong.Text = SysString.HtmlToTextCode(entity.Content);
            this.txtNeiRong.Text = entity.Content;

            //领导批示
            this.txtLingDaoPiShi.Text = entity.LeaderOpinion;
            this.txtPiShiYiJian.Text = entity.LeaderOpinionAdd;

            //承办情况、办理意见
            this.txtChuLiYiJian.Text = entity.UndertakeCircs;
            this.txtBanLiYiJian.Text = entity.BanLiYiJian;

            //承办部门
            this.txtChengBanBuMen.Text = entity.ChengBanBuMen;
            this.txtChengBanBuMenID.Text = entity.ChengBanBuMenID;

            //传阅
            if (base.IsPreview == false)
            {
                this.txtDeptName.Text = entity.IsFormSave ? entity.FenFaFanWei : string.Empty;
                this.hDeptID.Value = entity.IsFormSave ? entity.FenFaFanWeiID : string.Empty;
                this.txtUserName.Text = entity.IsFormSave ? entity.GongSiLingDao : string.Empty;
                this.hUserID.Value = entity.IsFormSave ? entity.GongSiLingDaoID : string.Empty;
            }
            else
            {
                this.txtDeptName.Text = entity.FenFaFanWei;
                this.hDeptID.Value = entity.FenFaFanWeiID;
                this.txtUserName.Text = entity.GongSiLingDao;
                this.hUserID.Value = entity.GongSiLingDaoID;
            }


            //核稿人
            this.txtHeGaoRen.Text = entity.CheckDrafter;
            this.txtHeGaoRenID.Text = entity.HeGaoRenID;

            //拟稿人
            this.txtNiGaoRen.Text = entity.Drafter;
            this.txtNiGaoRenID.Text = entity.NiGaoRenID;

            //提示信息
            this.txtTiShiXinXi.Text = entity.Message;
            this.txtBanShuiXinXi.Text = entity.Message;

            //提示信息添加
            this.txtTianJia.Text = entity.MessageAdd;

            //流程类型
            this.ddlType.Text = entity.Type;
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
            B_RequestReport entity = base.EntityData != null ? base.EntityData as B_RequestReport : new B_RequestReport();

            //附件
            entity.FileList = ucAttachment.UCDataList;

            #region 提示信息、承办情况
            //提示信息、承办情况
            if (!IsSave)
            {
                if (!string.IsNullOrEmpty(this.txtTianJia.Text))
                {
                    entity.MessageAdd = string.Empty;
                    entity.Message = this.txtTiShiXinXi.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(DateFormat) + "]:(" + base.StepName + ")" + this.txtTianJia.Text + "\n";
                }

                if (!string.IsNullOrEmpty(this.txtBanLiYiJian.Text))
                {
                    entity.BanLiYiJian = string.Empty;
                    entity.UndertakeCircs = this.txtChuLiYiJian.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(DateFormat) + "]:(" + base.StepName + ")" + this.txtBanLiYiJian.Text + "\n";
                }

                if (!string.IsNullOrEmpty(this.txtPiShiYiJian.Text))
                {
                    entity.LeaderOpinionAdd = string.Empty;
                    entity.LeaderOpinion = this.txtLingDaoPiShi.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(DateFormat) + "]:(" + base.StepName + ")" + this.txtPiShiYiJian.Text + "\n";
                }
            }
            else
            {
                entity.Message = this.txtTiShiXinXi.Text;
                entity.MessageAdd = this.txtTianJia.Text;

                entity.UndertakeCircs = this.txtChuLiYiJian.Text;
                entity.BanLiYiJian = this.txtBanLiYiJian.Text;

                entity.LeaderOpinion = this.txtLingDaoPiShi.Text;
                entity.LeaderOpinionAdd = this.txtPiShiYiJian.Text;
            }
            #endregion

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:
                    //主送
                    if (this.ddlZhuSong.Items.Count > 0)
                    {
                        entity.MainSend = this.ddlZhuSong.SelectedItem.Text;
                        entity.ZhuSongID = this.ddlZhuSong.SelectedValue;
                    }

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

                    //主题、标题
                    //entity.Subject = SysString.InputText(this.txtZhuTi.Text);
                    entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);

                    //内容
                    //entity.Content = SysString.TextToHtmlCode(this.txtNeiRong.Text);
                    entity.Content =this.txtNeiRong.Text;

                    //部门负责人
                    if (this.ddlFuZeRen.Items.Count > 0)
                    {
                        entity.DeptPrincipal = this.ddlFuZeRen.SelectedItem.Text;
                        entity.FuZeRenID = this.ddlFuZeRen.SelectedValue;
                    }

                    //核稿人
                    entity.CheckDrafter = this.txtHeGaoRen.Text;
                    entity.HeGaoRenID = this.txtHeGaoRenID.Text;

                    //拟稿人
                    entity.Drafter = this.txtNiGaoRen.Text;
                    entity.DrafterID = this.txtNiGaoRenID.Text;

                    //老版本用到
                    entity.NiGaoRenID = this.txtNiGaoRenID.Text;

                    //拟稿日期
                    entity.DraftDate = DateTime.Now;

                    //流程类型
                    entity.Type = this.ddlType.Text;
                    break;
                #endregion

                #region 核稿
                case ProcessConstString.StepName.RequestReportStepName.STEP_CHECK:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //主送
                        if (this.ddlZhuSong.Items.Count > 0)
                        {
                            entity.MainSend = this.ddlZhuSong.SelectedItem.Text;
                            entity.ZhuSongID = this.ddlZhuSong.SelectedValue;
                        }

                        //抄送
                        entity.CopySend = this.txtChaoSong.Text;
                        entity.ChaoSongDeptID = this.txtChaoSongDeptID.Text;
                        entity.ChaoSongID = this.txtChaoSongID.Text;

                        //主题、标题
                        //entity.Subject = SysString.InputText(this.txtZhuTi.Text);
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
                    }
                    else
                    {
                        entity.IsBack = true;
                    }
                    break;
                #endregion

                #region 签发
                case ProcessConstString.StepName.RequestReportStepName.STEP_SIGN:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //主送
                        if (this.ddlZhuSong.Items.Count > 0)
                        {
                            entity.MainSend = this.ddlZhuSong.SelectedItem.Text;
                            entity.ZhuSongID = this.ddlZhuSong.SelectedValue;
                        }

                        //抄送
                        entity.CopySend = this.txtChaoSong.Text;
                        entity.ChaoSongDeptID = this.txtChaoSongDeptID.Text;
                        entity.ChaoSongID = this.txtChaoSongID.Text;

                        //主题、标题
                        //entity.Subject = SysString.InputText(this.txtZhuTi.Text);
                        entity.DocumentTitle = SysString.InputText(this.txtZhuTi.Text);

                        //内容
                        //entity.Content = SysString.TextToHtmlCode(this.txtNeiRong.Text);
                        entity.Content = this.txtNeiRong.Text;

                        //公司办秘书科秘书（公办）、签发日期
                        if (base.SubAction == ProcessConstString.SubmitAction.RequestReportAction.ACTION_TJSH)
                        {
                            string[] arraySecretary = OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_SECRETARY);
                            entity.CompanySecretaryIDs = arraySecretary[0];
                            entity.CompanySecretarys = arraySecretary[1];

                            entity.ConfirmDate = DateTime.Now;
                        }

                        //编号
                        if (this.ddlBianZhiBuMen.Items.Count > 0)
                        {
                            entity.Number = WRRRNum.GetHNCode(this.ddlType.SelectedValue, this.ddlBianZhiBuMen.SelectedItem.Text);
                            entity.DocumentNo = entity.Number;
                        }
                    }
                    else
                    {
                        entity.IsBack = true;
                    }
                    break;
                #endregion

                #region 部门会签

                #endregion

                #region 主管领导审核

                #endregion

                #region 秘书审核
                case ProcessConstString.StepName.RequestReportStepName.STEP_MSVERIFY:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        //获取任务的秘书
                        entity.CompanySecretaryID = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;

                        //公司办主任
                        string[] arrayDirecotr = OAUser.GetUserByRoleName(OUConstString.RoleName.COMPANY_CHIEF);
                        entity.CompanyDirectorIDs = arrayDirecotr[0].ToString();
                        entity.CompanyDirectors = arrayDirecotr[1].ToString();
                    }
                    else
                    {
                        entity.Number = string.Empty;
                    }
                    break;

                #endregion

                #region 领导批示
                case ProcessConstString.StepName.RequestReportStepName.STEPNAME_INSTRUCTION:
                    //领导批示
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                    {
                        entity.LeaderOpinion = FormsMethod.GetPrompt(this.txtLingDaoPiShi.Text, this.txtPiShiYiJian.Text, base.SubAction, true);
                        entity.LeaderOpinionAdd = SysString.InputText(this.txtPiShiYiJian.Text);
                    }
                    else
                    {
                        entity.LeaderOpinion = FormsMethod.GetPrompt(this.txtLingDaoPiShi.Text, this.txtPiShiYiJian.Text, base.SubAction, false);
                    }
                    break;
                #endregion

                #region 秘书分发
                case ProcessConstString.StepName.RequestReportStepName.STEP_MSFF:
                    //承办部门
                    entity.ChengBanBuMen = this.txtChengBanBuMen.Text;
                    entity.ChengBanBuMenID = this.txtChengBanBuMenID.Text;

                    //分发范围
                    entity.FenFaFanWei = this.txtDeptName.Text;
                    entity.FenFaFanWeiID = this.hDeptID.Value;
                    entity.GongSiLingDao = this.txtUserName.Text;
                    entity.GongSiLingDaoID = this.hUserID.Value;

                    if (base.SubAction == ProcessConstString.SubmitAction.RequestReportAction.ACTION_CBFF)
                    {
                        //string[] arrayLeader = OAUser.GetDeptLeaderArray(entity.ChengBanBuMenID, 0);
                        string[] arrayLeader = OAUser.GetUserByDeptPostArray(entity.ChengBanBuMenID, OUConstString.PostName.FUKEZHANG, true, true);
                        entity.BuMenLingDaoID = arrayLeader[0];
                    }
                    break;
                #endregion

                #region 处室承办
                case ProcessConstString.StepName.RequestReportStepName.STEP_DIRECTOR:
                    //科室领导（部门负责人）
                    if (base.SubAction == ProcessConstString.SubmitAction.RequestReportAction.ACTION_JBKS)
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


                    if (base.SubAction == ProcessConstString.SubmitAction.RequestReportAction.ACTION_JBRY)
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
                    break;
                #endregion

                #region 科室承办
                case ProcessConstString.StepName.RequestReportStepName.STEP_CHIEF:
                    //直接承办人
                    if (base.SubAction == ProcessConstString.SubmitAction.WorkRelationAction.ACTION_JBRY)
                    {
                        if (this.ddlChengBanRen.Items.Count > 0)
                        {
                            entity.ChengBanRenID = this.ddlChengBanRen.SelectedValue;
                            entity.Contractor = this.ddlChengBanRen.SelectedItem.Text;
                        }
                    }

                    //承办日期
                    entity.SectionDate = DateTime.Now;
                    break;
                #endregion

                #region 人员承办
                case ProcessConstString.StepName.RequestReportStepName.STEP_MEMBER:
                    //承办日期
                    entity.MemberDate = DateTime.Now;
                    break;
                #endregion

                #region 承办审核
                case ProcessConstString.StepName.RequestReportStepName.STEP_CBVERIFY:
                    entity.ChuanYueRenYuan = this.txtChuanYueRenYuan.Text;
                    entity.ChuanYueRenYuanID = this.txtChuanYueRenIDs.Text;
                    entity.ChuanYueDeptID = this.txtChuanYueDeptIDs.Text;
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
            //保存批示
            this.btnSavePishi.Click += SubmitHandler;
            //批示完成
            this.btnComplete.Click += SubmitHandler;
            //完成
            this.btnWanCheng.Click += SubmitHandler;
            //撤销
            this.btnCancel.Click += SubmitHandler;
            //退回
            this.btnTuiHui.Click += SubmitHandler;
            //交办科室
            this.btnJiaoBanKeshi.Click += SubmitHandler;
            //交办人员
            this.btnJiaoBanRenYuan.Click += SubmitHandler;
            //提交审核
            this.btnFenFa.Click += SubmitHandler;
            //提交签发
            this.btnTiJiaoQianFa.Click += SubmitHandler;
            //提交核稿
            this.btnTiJiaoHeGao.Click += SubmitHandler;
            //承办分发
            this.btnChengBanFenFa.Click += SubmitHandler;
            //领导批示
            this.btnLingDaoPiShi.Click += SubmitHandler;
            //通过审核
            this.btnTongGuoShenHe.Click += SubmitHandler;
            //追加分发
            this.btnZhuiJiaFenFa.Click += SubmitHandler;
            //完成归档
            this.btnWanChengGuiDang.Click += SubmitHandler;
            //主任审核
            this.btnZhuRenShenHe.Click += SubmitHandler;
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

                //处理原来海南子流程
                //if (base.StepName == ProcessConstString.ProcessStepName.RequestReportStepName.STEP_DIRECTOR)
                //{
                //    if (base.SubAction != ProcessConstString.StepAction.ACTION_WC)
                //    {
                //        //if (!string.IsNullOrEmpty(base.SubProcessID))
                //        //{
                //        //    base.ProcessID = FormsMethod.GetParentProcessID(base.SubProcessID);
                //        //}
                //        base.ProcessID = base.SubProcessID;
                //    }
                //    //else
                //    //{

                //    //}
                //}

                //if (base.StepName == ProcessConstString.ProcessStepName.RequestReportStepName.STEP_CHIEF ||
                //    base.StepName == ProcessConstString.ProcessStepName.RequestReportStepName.STEP_MEMBER)
                //{
                //    base.ProcessID = base.SubProcessID;
                //}

                //保存
                if (strActionName == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_RequestReport entity = ControlToEntity(true) as B_RequestReport;
                    entity.SubmitAction = strActionName;
                    base.FormSubmit(true, strActionName, null, entity);
                }
                else
                {
                    B_RequestReport entity = ControlToEntity(false) as B_RequestReport;
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
                                    if (strActionName == ProcessConstString.SubmitAction.RequestReportAction.ACTION_TJQF)
                                    {
                                        if (!string.IsNullOrEmpty(entity.HeGaoRenID))
                                        {
                                            if (!B_RequestReport.IsHaveChecked(base.ProcessID, entity.HeGaoRenID))
                                            {
                                                JScript.Alert("核稿人未处理过核稿，请提交核稿或清空核稿人", false);
                                                return;
                                            }
                                        }
                                    }
                                    break;

                                case ProcessConstString.StepName.RequestReportStepName.STEP_SIGN:
                                    if (strActionName == ProcessConstString.SubmitAction.RequestReportAction.ACTION_TJSH &&
                                                                    (!string.IsNullOrEmpty(entity.ChaoSongDeptID) || !string.IsNullOrEmpty(entity.ChaoSongID)))
                                    {
                                        base.Circulate(entity.ChaoSongDeptID, "1", string.Empty, entity.ChaoSongID, "1", false, string.Empty, false);
                                    }
                                    break;

                                case ProcessConstString.StepName.RequestReportStepName.STEP_MSFF:
                                    if (strActionName == ProcessConstString.SubmitAction.RequestReportAction.ACTION_CBFF &&
                                                                    (!string.IsNullOrEmpty(entity.FenFaFanWeiID) || !string.IsNullOrEmpty(entity.GongSiLingDaoID)))
                                    {
                                        base.Circulate(entity.FenFaFanWeiID, "1", string.Empty, entity.GongSiLingDaoID, "1", false, string.Empty, false);
                                    }
                                    break;

                                case ProcessConstString.StepName.RequestReportStepName.STEP_CBVERIFY:
                                    if (strActionName == ProcessConstString.SubmitAction.RequestReportAction.ACTION_WCGD)
                                    {
                                        if (!string.IsNullOrEmpty(entity.ChuanYueDeptID) || !string.IsNullOrEmpty(entity.ChuanYueRenYuanID))
                                        {
                                            base.Circulate(entity.ChuanYueDeptID, "1", string.Empty, entity.ChuanYueRenYuanID, "1", false, string.Empty, false);
                                        }

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
                OAUser.GetUserByDeptPost(this.ddlFuZeRen, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.FUCHUZHANG, true, true);
            }
        }
        #endregion

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