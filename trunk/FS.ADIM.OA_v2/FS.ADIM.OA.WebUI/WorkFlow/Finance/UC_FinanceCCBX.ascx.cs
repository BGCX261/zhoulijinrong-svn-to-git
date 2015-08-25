//----------------------------------------------------------------
// Copyright (C) 2013 
//
// 文件功能描述：费用
// 
// 
// 创建标识：ZHOULI 2013-05-29
//
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
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FounderSoftware.ADIM.OU.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public partial class UC_FinanceCCBX : FormsUIBase
    {
        #region 页面加载
        protected string FileTitle = "海南核电有限公司出差报销单";

        protected void Page_Load(object sender, EventArgs e)
        {
            //InitPrint();
            this.SubmitEvents();
            B_Finance b_Finance = new B_Finance();
            string url = b_Finance.GetFinanceUrl(txtChuChaDanHao.Text);

            linkShow.NavigateUrl = url;
        }

        #endregion

        #region 控件初始设置 页面加载时触发 先触发实体加载
        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            B_FinanceCCBX entity = base.EntityData != null ? base.EntityData as B_FinanceCCBX : new B_FinanceCCBX();

            if (this.ddlType.SelectedValue == "出差")
            {
                FileTitle = "海南核电有限公司出差报销单";
            }
            else
            {
                FileTitle = "海南核电有限公司培训报销单";
            }

            OAControl controls = new OAControl();
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

                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,
                            this.txtNiGaoRiQi, this.txtBianHao, this.txtQiTaXiaoJi, this.txtBuTieXiaoJi, this.txtHeJi,
                            this.txtZhuQinRT,this.txtZhuQinJE,this.txtZhuSuJYRT,this.txtZhuSuJYJE,
                            this.txtWeiWoPuBuJE,this.txtWeiWoPuRT,this.txtZaiTuRT,this.txtZaiTuJE,
                            this.txtCaiWuJE,this.ddlCaiWu,this.ddlPeiXunChu,this.txtChangQiBuTie,
                            this.txtZiXingTianShu2,this.txtZiXingJinE,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnCal2, 
                            this.btnPeiXunChu, this.btnGongSiLingDao, this.btnCaiWu, this.btnTuiHui, this.ucPrint,
                            this.btnCaiWuPass, this.btnWanCheng,
                        };
                        this.txtBianHao.ToolTip = "提交后生成";

                        if (entity.IsBack == true)
                        {
                            btnCancel.Visible = true;
                        }
                        break;
                    #endregion

                    #region 主管领导
                    case ProcessConstString.StepName.FinanceCCBXStepName.STEP_DEPTVERIFY:

                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtChuChaDanHao,this.txtNiGaoRen,this.txtNiGaoRiQi,this.txtBianHao,
                            this.ddlZhiCheng,this.ddlBianZhiBuMen,this.ddlJieKuan,this.txtPhone,this.txtDanJuZhangShu,
                            this.txtShiYou,this.txtZhuSuRT,this.txtZhuSuJE,this.txtTuoYunRT,this.txtTuoYunJE,this.txtQiTaRT,
                            this.txtQiTaJE,this.txtQiTaXiaoJi,this.txtZhuQinRT,this.txtZhuQinJE,this.txtZhuSuJYRT,this.txtZhuSuJYJE,
                            this.txtWeiWoPuRT,this.txtWeiWoPuBuJE,this.txtZaiTuRT,this.txtZaiTuJE,this.txtBuTieXiaoJi,
                            this.txtHeJi,this.txtCaiWuJE,this.ddlBuMenZhuGuan,
                            this.txtLiXiangHao,this.txtLiXiangJE,this.ddlYuSuanNei,this.txtChangQiBuTie,
                            this.txtZiXingTianShu1,this.txtZiXingTianShu2,this.txtZiXingJinE,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                           this.btnCal1,this.btnCal2, 
                           this.btnBuMenZhuGuan, this.ucPrint,
                           this.btnCaiWuPass,this.btnCancel,this.btnWanCheng,
                        };
                        ucChuXingMingXi.UCIsDisEnable = true;
                        this.tableCCCS.Visible = false;
                        if (this.ddlType.SelectedValue == "出差")
                        {
                            this.btnPeiXunChu.Visible = false;
                        }
                        else
                        {
                            this.btnGongSiLingDao.Visible = false;
                            this.btnCaiWu.Visible = false;
                        }
                        break;
                    #endregion

                    #region 培训处
                    case ProcessConstString.StepName.FinanceCCBXStepName.STEP_PXCVERIFY:
                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtChuChaDanHao,this.txtNiGaoRen,this.txtNiGaoRiQi,this.txtBianHao,
                            this.ddlZhiCheng,this.ddlBianZhiBuMen,this.ddlJieKuan,this.txtPhone,this.txtDanJuZhangShu,
                            this.txtShiYou,this.txtZhuSuRT,this.txtZhuSuJE,this.txtTuoYunRT,this.txtTuoYunJE,this.txtQiTaRT,
                            this.txtQiTaJE,this.txtQiTaXiaoJi,this.txtZhuQinRT,this.txtZhuQinJE,this.txtZhuSuJYRT,this.txtZhuSuJYJE,
                            this.txtWeiWoPuRT,this.txtWeiWoPuBuJE,this.txtZaiTuRT,this.txtZaiTuJE,this.txtBuTieXiaoJi,
                            this.txtHeJi,this.txtCaiWuJE,this.ddlBuMenZhuGuan,this.ddlPeiXunChu,
                            this.txtLiXiangHao,this.txtLiXiangJE,this.ddlYuSuanNei,this.txtChangQiBuTie,
                            this.txtZiXingTianShu1,this.txtZiXingTianShu2,this.txtZiXingJinE,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnCal1,this.btnCal2, 
                            this.btnPeiXunChu, this.btnBuMenZhuGuan, this.ucPrint,
                            this.btnCaiWuPass,this.btnCancel,this.btnWanCheng,
                        };

                        ucChuXingMingXi.UCIsDisEnable = true;
                        this.tableCCCS.Visible = false;
                        if (this.ddlType.SelectedValue == "出差")
                        {
                            this.btnPeiXunChu.Visible = false;
                        }
                        break;
                    #endregion

                    #region 公司领导
                    case ProcessConstString.StepName.FinanceCCBXStepName.STEP_GSLDVERIFY:
                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtChuChaDanHao,this.txtNiGaoRen,this.txtNiGaoRiQi,this.txtBianHao,
                            this.ddlZhiCheng,this.ddlBianZhiBuMen,this.ddlJieKuan,this.txtPhone,this.txtDanJuZhangShu,
                            this.txtShiYou,this.txtZhuSuRT,this.txtZhuSuJE,this.txtTuoYunRT,this.txtTuoYunJE,this.txtQiTaRT,
                            this.txtQiTaJE,this.txtQiTaXiaoJi,this.txtZhuQinRT,this.txtZhuQinJE,this.txtZhuSuJYRT,this.txtZhuSuJYJE,
                            this.txtWeiWoPuRT,this.txtWeiWoPuBuJE,this.txtZaiTuRT,this.txtZaiTuJE,this.txtBuTieXiaoJi,
                            this.txtHeJi,this.txtCaiWuJE,this.ddlBuMenZhuGuan,this.ddlGSLingDao,this.ddlPeiXunChu,
                            this.txtLiXiangHao,this.txtLiXiangJE,this.ddlYuSuanNei,this.txtChangQiBuTie,
                            this.txtZiXingTianShu1,this.txtZiXingTianShu2,this.txtZiXingJinE,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                           this.btnCal1, this.btnCal2, 
                           this.btnBuMenZhuGuan, this.btnPeiXunChu, this.btnGongSiLingDao, this.ucPrint,
                           this.btnCaiWuPass,this.btnCancel,this.btnWanCheng,
                        };

                        ucChuXingMingXi.UCIsDisEnable = true;
                        this.tableCCCS.Visible = false;
                        if (this.ddlType.SelectedValue == "出差")
                        {
                            this.btnPeiXunChu.Visible = false;
                        }
                        break;
                    #endregion

                    #region 财务
                    case ProcessConstString.StepName.FinanceCCBXStepName.STEP_CWVERIFY:
                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtChuChaDanHao,this.txtNiGaoRen,this.txtNiGaoRiQi,this.txtBianHao,
                            this.ddlZhiCheng,this.ddlBianZhiBuMen,this.ddlJieKuan,this.txtPhone,this.txtDanJuZhangShu,
                            this.txtShiYou,
                            this.ddlBuMenZhuGuan,this.ddlGSLingDao,this.ddlCaiWu,this.ddlPeiXunChu,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                           
                           this.btnBuMenZhuGuan, this.btnPeiXunChu, this.btnGongSiLingDao, this.btnCaiWu, this.ucPrint,
                           this.btnCancel,this.btnWanCheng,
                        };

                        break;
                    #endregion

                    #region 反馈报销人
                    case ProcessConstString.StepName.FinanceCCBXStepName.STEP_FANKUI:
                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtChuChaDanHao,this.txtNiGaoRen,this.txtNiGaoRiQi,this.txtBianHao,
                            this.ddlZhiCheng,this.ddlBianZhiBuMen,this.ddlJieKuan,this.txtPhone,this.txtDanJuZhangShu,
                            this.txtShiYou,this.txtZhuSuRT,this.txtZhuSuJE,this.txtTuoYunRT,this.txtTuoYunJE,this.txtQiTaRT,
                            this.txtQiTaJE,this.txtQiTaXiaoJi,this.txtZhuQinRT,this.txtZhuQinJE,this.txtZhuSuJYRT,this.txtZhuSuJYJE,
                            this.txtWeiWoPuRT,this.txtWeiWoPuBuJE,this.txtZaiTuRT,this.txtZaiTuJE,this.txtBuTieXiaoJi,
                            this.txtHeJi,this.txtCaiWuJE,this.ddlBuMenZhuGuan,this.ddlGSLingDao,this.ddlCaiWu,this.ddlPeiXunChu,
                            this.txtLiXiangHao,this.txtLiXiangJE,this.ddlYuSuanNei,
                            this.txtZiXingTianShu1,this.txtZiXingTianShu2,this.txtZiXingJinE,this.txtChangQiBuTie,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnCal1, this.btnCal2, 
                            this.btnBuMenZhuGuan, this.btnPeiXunChu, this.btnGongSiLingDao, this.btnCaiWu, this.btnTuiHui, this.ucPrint,
                            this.btnCaiWuPass,this.btnCancel,this.btnSave,
                        };

                        ucChuXingMingXi.UCIsDisEnable = true;
                        this.chkIsYiXian.Enabled = false;
                        if (this.ddlType.SelectedValue == "出差")
                        {
                            this.btnPeiXunChu.Visible = false;
                        }

                        break;
                    #endregion
                }

                //设置所有控件状态
                controls.SetControls();
            }
            else
            {
                FormsMethod.SetControlAll(this);
            }
        }
        #endregion

        #region 实体加载 页面加载时触发
        /// <summary>
        /// 实体加载
        /// </summary>
        protected override void EntityToControl()
        {
            B_FinanceCCBX entity = base.EntityData != null ? base.EntityData as B_FinanceCCBX : new B_FinanceCCBX();

            //拟稿
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
            {
                //编制部门
                OADept.GetDeptByUser(this.ddlBianZhiBuMen, CurrentUserInfo.UserName, 1, true, false);
                //部门负责人
                if (this.ddlBianZhiBuMen.Items.Count > 0)
                {
                    ddlBianZhiBuMen_SelectedIndexChanged(null, null);
                }

                //拟稿日期
                txtNiGaoRiQi.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //拟稿人
                this.txtNiGaoRen.Text = CurrentUserInfo.DisplayName;
                this.txtNiGaoRenID.Text = CurrentUserInfo.UserName;
                this.txtPhone.Text = CurrentUserInfo.OfficePhone;

                if (entity.ChuXingDetails.Count == 0) //默认加载一行 不然日期控件会失效
                {
                    ucChuXingMingXi.UCIsFirst = true;
                }

                OAUser.GetUserByRole(this.ddlGSLingDao, OUConstString.RoleName.COMPANY_LEADER);

                if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
                {
                    if (string.IsNullOrEmpty(base.WorkItemID))
                    {
                        this.lbJs.Text = "<script>ShowMyDiv();</script>";
                    }
                }
            }
            else
            {
                FormsMethod.SetDropDownList(this.ddlBianZhiBuMen, entity.DepartmentID, entity.Department);
                FormsMethod.SetDropDownList(this.ddlBuMenZhuGuan, entity.BuMenZhuGuanID, entity.BuMenZhuGuan);
                FormsMethod.SetDropDownList(this.ddlGSLingDao, entity.GongSiLingDaoID, entity.GongSiLingDao);

                this.txtNiGaoRen.Text = entity.Drafter;
                this.txtNiGaoRenID.Text = entity.DrafterID;
            }

            //主管领导
            if (base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_DEPTVERIFY)
            {
                OAUser.GetUserByRole(this.ddlGSLingDao, OUConstString.RoleName.COMPANY_LEADER);
                OAUser.GetUserByRole(this.ddlCaiWu, OUConstString.RoleName.CaiWu);

                OAUser.GetUserByRole(this.ddlPeiXunChu, OUConstString.RoleName.PeiXunChuLD);

            }
            //培训处
            if (base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_PXCVERIFY)
            {
                OAUser.GetUserByRole(this.ddlGSLingDao, OUConstString.RoleName.COMPANY_LEADER);
                OAUser.GetUserByRole(this.ddlCaiWu, OUConstString.RoleName.CaiWu);
            }
            //公司领导
            if (base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_GSLDVERIFY)
            {
                OAUser.GetUserByRole(this.ddlCaiWu, OUConstString.RoleName.CaiWu);
            }
            //财务
            if (base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_CWVERIFY)
            {

            }
            //反馈报销人
            if (base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_FANKUI)
            {

            }

            if (txtNiGaoRen.Text == "")
                txtNiGaoRen.Text = entity.Drafter;
            if (txtPhone.Text == "")
                txtPhone.Text = entity.Phone;
            if (txtNiGaoRiQi.Text == "")
                txtNiGaoRiQi.Text = entity.DraftDate.ToString("yyyy-MM-dd");

            FormsMethod.SelectedDropDownList(this.ddlType, entity.Type);
            txtType.Text = entity.Type;
            if (txtType.Text != "")
            {
                ddlType_SelectedIndexChanged(null, null);
            }
            txtChuChaDanHao.Text = entity.ChuChaDanHao;
            txtBianHao.Text = entity.DocumentNo;

            FormsMethod.SelectedDropDownList(this.ddlZhiCheng, entity.ZhiWuZhiChengID, entity.ZhiWuZhiCheng);
            FormsMethod.SelectedDropDownList(this.ddlBianZhiBuMen, entity.DepartmentID, entity.Department);
            FormsMethod.SelectedDropDownList(this.ddlJieKuan, entity.GeRenJieKuan);

            if (entity.DanJuZhangShu == Int32.MinValue)
                txtDanJuZhangShu.Text = "";
            else
                txtDanJuZhangShu.Text = entity.DanJuZhangShu.ToString();



            txtShiYou.Text = entity.ShiYou;

            ucChuXingMingXi.UCCXList = entity.ChuXingDetails;
            foreach (var item in entity.QiTaFeiYongDetails)
            {
                if (item.Name == "住宿费")
                {
                    txtZhuSuRT.Text = item.RenTian.ToString();
                    txtZhuSuJE.Text = item.JinE.ToString();
                }
                else if (item.Name == "托运费")
                {
                    txtTuoYunRT.Text = item.RenTian.ToString();
                    txtTuoYunJE.Text = item.JinE.ToString();
                }
                else if (item.Name == "其他")
                {
                    txtQiTaRT.Text = item.RenTian.ToString();
                    txtQiTaJE.Text = item.JinE.ToString();
                }
            }
            //其他费用小计
            if (entity.QiTaFeiYongXiaoJi == Decimal.MinValue)
                txtQiTaXiaoJi.Text = "";
            else
                txtQiTaXiaoJi.Text = entity.QiTaFeiYongXiaoJi.ToString();

            foreach (var item in entity.BuTieDetails)
            {
                if (item.Name == "住勤补贴")
                {
                    txtZhuQinRT.Text = item.RenTian.ToString();
                    txtZhuQinJE.Text = item.JinE.ToString();
                }
                else if (item.Name == "住宿节约补贴")
                {
                    txtZhuSuJYRT.Text = item.RenTian.ToString();
                    txtZhuSuJYJE.Text = item.JinE.ToString();
                }
                else if (item.Name == "未乘坐卧铺补贴")
                {
                    txtWeiWoPuRT.Text = item.RenTian.ToString();
                    txtWeiWoPuBuJE.Text = item.JinE.ToString();
                }
                else if (item.Name == "在途补贴")
                {
                    txtZaiTuRT.Text = item.RenTian.ToString();
                    txtZaiTuJE.Text = item.JinE.ToString();
                }
            }
            if (entity.BuTieXiaoJi == Decimal.MinValue)
                txtBuTieXiaoJi.Text = "";
            else
                txtBuTieXiaoJi.Text = entity.BuTieXiaoJi.ToString();

            //培训特有
            txtLiXiangHao.Text = entity.LiXiangHao;
            FormsMethod.SelectedDropDownList(this.ddlYuSuanNei, entity.YuSuanNei);
            txtLiXiangJE.Text = entity.LiXiangJE.ToString();

            if (entity.HeJi == Decimal.MinValue)
                txtHeJi.Text = "";
            else
                txtHeJi.Text = entity.HeJi.ToString();

            txtCaiWuJE.Text = entity.CaiWuShenHeJinE.ToString();

            txtZhuQinJE.ToolTip = entity.ZhuQinToolTip;
            txtZhuSuJE.ToolTip = entity.ZhuSuToolTip;
            txtZiXingJinE.ToolTip = entity.ZiXingToolTip;

            FormsMethod.SelectedDropDownList(this.ddlBuMenZhuGuan, entity.BuMenZhuGuanID, entity.BuMenZhuGuan);
            FormsMethod.SelectedDropDownList(this.ddlGSLingDao, entity.GongSiLingDaoID, entity.GongSiLingDao);
            FormsMethod.SelectedDropDownList(this.ddlCaiWu, entity.CaiWuID, entity.CaiWu);
            FormsMethod.SelectedDropDownList(this.ddlPeiXunChu, entity.PeiXunChuLingDaoID, entity.PeiXunChuLingDao);

            txtChangQiBuTie.Text = entity.ChangQiBuTie;
            if (entity.ZiXingTianShu == Int32.MinValue)
            {
                txtZiXingTianShu1.Text = "";
                txtZiXingTianShu2.Text = "";
            }
            else
            {
                txtZiXingTianShu1.Text = entity.ZiXingTianShu.ToString();
                txtZiXingTianShu2.Text = entity.ZiXingTianShu.ToString();
            }
            if (entity.ZiXingJinE == Decimal.MinValue)
                txtZiXingJinE.Text = "";
            else
                txtZiXingJinE.Text = entity.ZiXingJinE.ToString();
            //提示信息
            this.txtTiShiXinXi.Text = entity.Message;
            this.txtTianJia.Text = entity.MessageAdd;
        }
        #endregion

        #region 实体赋值 保存 提交时触发
        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="IsSave"></param>
        /// <returns></returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_FinanceCCBX entity = base.EntityData != null ? base.EntityData as B_FinanceCCBX : new B_FinanceCCBX();

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:

                    entity.Type = ddlType.SelectedValue;
                    entity.ChuChaDanHao = txtChuChaDanHao.Text;
                    entity.DocumentNo = txtBianHao.Text;

                    //拟稿日期
                    entity.DraftDate = DateTime.Now;
                    //拟稿人 
                    entity.Drafter = this.txtNiGaoRen.Text;
                    entity.DrafterID = this.txtNiGaoRenID.Text;

                    //标题、主题
                    entity.DocumentTitle = entity.Drafter + "-" + ddlType.SelectedValue + "报销单";

                    if (ddlZhiCheng.SelectedItem != null)
                        entity.ZhiWuZhiCheng = ddlZhiCheng.SelectedItem.Text;
                    entity.ZhiWuZhiChengID = ddlZhiCheng.SelectedValue;

                    //编制部门
                    if (this.ddlBianZhiBuMen.Items.Count > 0)
                    {
                        entity.Department = this.ddlBianZhiBuMen.SelectedItem.Text;
                        entity.DepartmentID = this.ddlBianZhiBuMen.SelectedValue;
                    }
                    entity.GeRenJieKuan = ddlJieKuan.SelectedValue;
                    entity.Phone = txtPhone.Text;
                    entity.DanJuZhangShu = SysConvert.ToInt32(txtDanJuZhangShu.Text);
                    entity.ShiYou = txtShiYou.Text;

                    //主管领导
                    entity.BuMenZhuGuan = this.ddlBuMenZhuGuan.SelectedItem.Text;
                    entity.BuMenZhuGuanID = this.ddlBuMenZhuGuan.SelectedValue;

                    //公司领导
                    if (this.ddlGSLingDao.SelectedItem != null)
                        entity.GongSiLingDao = this.ddlGSLingDao.SelectedItem.Text;
                    entity.GongSiLingDaoID = this.ddlGSLingDao.SelectedValue;

                    //提交后产生报销单编号 报销单的编号为BX+部门代码+年度+四位流水号。
                    if (IsSave == false && entity.DocumentNo == "")
                    {
                        string year = DateTime.Now.Year.ToString();

                        string deptNo = OADept.GetDeptByDeptID(entity.DepartmentID).No;

                        if (ddlType.SelectedValue == "出差")
                            entity.DocumentNo = "CCBX" + "-" + deptNo + "-" + entity.GenerateBianHao(year, "出差报销单");
                        else
                            entity.DocumentNo = "PXBX" + "-" + deptNo + "-" + entity.GenerateBianHao(year, "培训报销单");
                    }
                    break;
                #endregion

                #region 主管领导审核
                case ProcessConstString.StepName.FinanceCCBXStepName.STEP_DEPTVERIFY:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;
                        entity.BuMenZhuGuanRiQi = DateTime.Now.ToString("yyyy-MM-dd");
                        //公司领导
                        if (this.ddlGSLingDao.SelectedItem != null)
                        {
                            entity.GongSiLingDao = this.ddlGSLingDao.SelectedItem.Text;
                            entity.GongSiLingDaoID = this.ddlGSLingDao.SelectedValue;
                        }
                        //财务
                        if (this.ddlCaiWu.SelectedItem != null)
                        {
                            entity.CaiWu = this.ddlCaiWu.SelectedItem.Text;
                            entity.CaiWuID = this.ddlCaiWu.SelectedValue;
                        }
                        //归口部门主管 培训处领导
                        if (this.ddlPeiXunChu.SelectedItem != null)
                        {
                            entity.PeiXunChuLingDao = this.ddlPeiXunChu.SelectedItem.Text;
                            entity.PeiXunChuLingDaoID = this.ddlPeiXunChu.SelectedValue;
                        }
                    }

                    break;
                #endregion

                #region 培训处审核
                case ProcessConstString.StepName.FinanceCCBXStepName.STEP_PXCVERIFY:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;
                        entity.PeiXunChuLingDaoRiQi = DateTime.Now.ToString("yyyy-MM-dd");
                        //公司领导
                        if (this.ddlGSLingDao.SelectedItem != null)
                        {
                            entity.GongSiLingDao = this.ddlGSLingDao.SelectedItem.Text;
                            entity.GongSiLingDaoID = this.ddlGSLingDao.SelectedValue;
                        }
                        //财务
                        if (this.ddlCaiWu.SelectedItem != null)
                        {
                            entity.CaiWu = this.ddlCaiWu.SelectedItem.Text;
                            entity.CaiWuID = this.ddlCaiWu.SelectedValue;
                        }
                    }

                    break;
                #endregion

                #region 公司领导
                case ProcessConstString.StepName.FinanceCCBXStepName.STEP_GSLDVERIFY:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;
                        entity.GongSiLingDaoRiQi = DateTime.Now.ToString("yyyy-MM-dd");
                        //财务
                        if (this.ddlCaiWu.SelectedItem != null)
                        {
                            entity.CaiWu = this.ddlCaiWu.SelectedItem.Text;
                            entity.CaiWuID = this.ddlCaiWu.SelectedValue;
                        }
                    }

                    break;
                #endregion

                #region 财务
                case ProcessConstString.StepName.FinanceCCBXStepName.STEP_CWVERIFY:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;
                        entity.CaiWuRiQi = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    break;
                #endregion
            }

            //费用相关 拟稿人和财务都会改
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT || base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_CWVERIFY)
            {
                entity.LiXiangHao = txtLiXiangHao.Text;
                entity.LiXiangJE = txtLiXiangJE.Text.ToString();
                entity.YuSuanNei = ddlYuSuanNei.SelectedValue;

                #region 费用
                entity.ChuXingDetails = ucChuXingMingXi.UCGetCXList();

                //其他费用
                entity.QiTaFeiYongDetails = new List<M_FinanceCCBX.QiTaFeiYongDetail>();
                M_FinanceCCBX.QiTaFeiYongDetail qiTaFeiYongDetail = new M_FinanceCCBX.QiTaFeiYongDetail();
                qiTaFeiYongDetail.Name = "住宿费";
                qiTaFeiYongDetail.RenTian = SysConvert.ToDecimal(txtZhuSuRT.Text);
                qiTaFeiYongDetail.JinE = SysConvert.ToDecimal(txtZhuSuJE.Text);
                entity.QiTaFeiYongDetails.Add(qiTaFeiYongDetail);

                qiTaFeiYongDetail = new M_FinanceCCBX.QiTaFeiYongDetail();
                qiTaFeiYongDetail.Name = "托运费";
                qiTaFeiYongDetail.RenTian = SysConvert.ToDecimal(txtTuoYunRT.Text);
                qiTaFeiYongDetail.JinE = SysConvert.ToDecimal(txtTuoYunJE.Text);
                entity.QiTaFeiYongDetails.Add(qiTaFeiYongDetail);

                qiTaFeiYongDetail = new M_FinanceCCBX.QiTaFeiYongDetail();
                qiTaFeiYongDetail.Name = "其他";
                qiTaFeiYongDetail.RenTian = SysConvert.ToDecimal(txtQiTaRT.Text);
                qiTaFeiYongDetail.JinE = SysConvert.ToDecimal(txtQiTaJE.Text);
                entity.QiTaFeiYongDetails.Add(qiTaFeiYongDetail);

                //其他费用小计
                Decimal qiTaXiaoJi = 0;
                foreach (var item in entity.QiTaFeiYongDetails)
                {
                    qiTaXiaoJi += item.JinE;
                }
                entity.QiTaFeiYongXiaoJi = qiTaXiaoJi;

                //出差补贴
                entity.BuTieDetails = new List<M_FinanceCCBX.BuTieDetail>();
                M_FinanceCCBX.BuTieDetail buTieDetail = new M_FinanceCCBX.BuTieDetail();
                buTieDetail.Name = "住勤补贴";
                buTieDetail.RenTian = SysConvert.ToDecimal(txtZhuQinRT.Text);
                buTieDetail.JinE = SysConvert.ToDecimal(txtZhuQinJE.Text);
                entity.BuTieDetails.Add(buTieDetail);

                buTieDetail = new M_FinanceCCBX.BuTieDetail();
                buTieDetail.Name = "住宿节约补贴";
                buTieDetail.RenTian = SysConvert.ToDecimal(txtZhuSuJYRT.Text);
                buTieDetail.JinE = SysConvert.ToDecimal(txtZhuSuJYJE.Text);
                entity.BuTieDetails.Add(buTieDetail);

                buTieDetail = new M_FinanceCCBX.BuTieDetail();
                buTieDetail.Name = "未乘坐卧铺补贴";
                buTieDetail.RenTian = SysConvert.ToDecimal(txtWeiWoPuRT.Text);
                buTieDetail.JinE = SysConvert.ToDecimal(txtWeiWoPuBuJE.Text);
                entity.BuTieDetails.Add(buTieDetail);

                buTieDetail = new M_FinanceCCBX.BuTieDetail();
                buTieDetail.Name = "在途补贴";
                buTieDetail.RenTian = SysConvert.ToDecimal(txtZaiTuRT.Text);
                buTieDetail.JinE = SysConvert.ToDecimal(txtZaiTuJE.Text);
                entity.BuTieDetails.Add(buTieDetail);

                //出差补贴小计
                Decimal chuChaXiaoJi = 0;
                foreach (var item in entity.BuTieDetails)
                {
                    chuChaXiaoJi += item.JinE;
                }
                entity.BuTieXiaoJi = chuChaXiaoJi;

                //合计
                entity.HeJi = SysConvert.ToDecimal(txtHeJi.Text);
                entity.CaiWuShenHeJinE = txtCaiWuJE.Text;

                entity.ZhuQinToolTip = txtZhuQinJE.ToolTip;
                entity.ZhuSuToolTip = txtZhuSuJE.ToolTip;
                entity.ZiXingToolTip = txtZiXingJinE.ToolTip;
                #endregion

                entity.ChangQiBuTie = txtChangQiBuTie.Text;
                entity.ZiXingTianShu = SysConvert.ToInt32(txtZiXingTianShu1.Text);
                entity.ZiXingJinE = SysConvert.ToDecimal(txtZiXingJinE.Text);
            }
            #region 提示信息、意见
            if (!IsSave)
            {
                if (!string.IsNullOrEmpty(this.txtTianJia.Text))
                {
                    entity.MessageAdd = string.Empty;
                    entity.Message = this.txtTiShiXinXi.Text + (string.IsNullOrEmpty(entity.ReceiveUserName) ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + "[" + DateTime.Now.ToString(ConstString.DateFormat.Long) + "]:(" + base.StepName + ")" + SysString.InputText(this.txtTianJia.Text) + "\n";
                }

            }
            else
            {
                entity.Message = this.txtTiShiXinXi.Text;
                entity.MessageAdd = this.txtTianJia.Text;
            }
            #endregion
            return entity;
        }
        #endregion

        #region 表单事件 按钮点击 下拉选择等

        #region 绑定按钮提交事件
        /// <summary>
        /// 绑定表单事件
        /// </summary>
        protected void SubmitEvents()
        {
            EventHandler SubmitHandler = new EventHandler(SubmitBtn_Click);
            //保存
            this.btnSave.Click += SubmitHandler;
            //提交主管领导审核
            this.btnBuMenZhuGuan.Click += SubmitHandler;
            this.btnPeiXunChu.Click += SubmitHandler;
            this.btnCancel.Click += SubmitHandler;
            this.btnGongSiLingDao.Click += SubmitHandler;
            this.btnCaiWu.Click += SubmitHandler;
            this.btnCaiWuPass.Click += SubmitHandler;
            this.btnWanCheng.Click += SubmitHandler;
            //退回
            this.btnTuiHui.Click += SubmitHandler;
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

                //保存
                if (base.SubAction == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_FinanceCCBX entity = ControlToEntity(true) as B_FinanceCCBX;
                    entity.SubmitAction = base.SubAction;
                    base.FormSubmit(true, base.SubAction, null, entity);
                }
                else
                {
                    B_FinanceCCBX entity = ControlToEntity(false) as B_FinanceCCBX;
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
                            JScript.Alert(strErrorMessage, true);
                            return;
                        }
                        else
                        {
                            //调用工作流                  
                            Hashtable nValues = entity.GetProcNameValue(base.StepName, base.SubAction);
                            base.FormSubmit(false, base.SubAction, nValues, entity);

                            //财务审核通过后调用
                            if (base.StepName == ProcessConstString.StepName.FinanceCCBXStepName.STEP_CWVERIFY)
                            {
                                if (base.SubAction == btnCaiWuPass.ToolTip)
                                {
                                    CaiWuFeiYong();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JScript.Alert(ex.Message, true);
                return;
            }
        }
        #endregion

        protected void ddlBianZhiBuMen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //部门负责人
            if (this.ddlBianZhiBuMen.Items.Count > 0)
            {
                OAUser.GetUserByDeptPost(this.ddlBuMenZhuGuan, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.FUCHUZHANG, true, true);

                if (this.ddlBuMenZhuGuan.Items.Count == 2)
                {
                    if (this.ddlBuMenZhuGuan.Items[1].Value == CurrentUserInfo.UserName)
                    {
                        ddlBuMenZhuGuan.Items.Clear();
                        OAUser.GetUserByRole(this.ddlBuMenZhuGuan, OUConstString.RoleName.COMPANY_LEADER);
                    }
                }
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlType.SelectedValue == "出差")
            {
                lblFileTitle.Text = "海南核电有限公司出差报销单";
                divPeiXun1.Visible = false;
                tablePeiXun1.Visible = false;
                tablePeiXunChu.Visible = false;
                ucChuXingMingXi.UCProcessType = "出差";

            }
            else
            {
                lblFileTitle.Text = "海南核电有限公司培训报销单";
                divPeiXun1.Visible = true;
                tablePeiXun1.Visible = true;
                tablePeiXunChu.Visible = true;
                ucChuXingMingXi.UCProcessType = "培训";
                ucChuXingMingXi.SetPeiXunForm();
            }
            txtType.Text = this.ddlType.SelectedValue;
        }

        #endregion

        #region 表单辅助 自动计算
        //自动计算补贴
        protected void btnCal1_Click(object sender, EventArgs e)
        {
            if (ddlZhiCheng.SelectedValue == "")
            {
                JScript.Alert("请选择职务/职称", true);
                return;
            }
            int iZhiChengID = SysConvert.ToInt32(ddlZhiCheng.SelectedValue); //职务或职称

            #region 住勤补贴
            //住勤补贴：天数=出差结束时间-起址时间+1;
            //补贴金额: 公司领导、处级干部或研高职称：X天*50元。
            //科级干部或高级职称、普通员工：10天*70元+20天*60元+X天*40
            //1 公司领导 2处级干部或研高职称 3科级干部或高级职称 4普通员工
            int zhu10A_70 = 70; //处级以下公式=10天*70元+20天*60元+X天*40; 
            int zhu20A_60 = 60;
            int zhuXA_40 = 40;
            int zhuXB_50 = 50; //处级以上公式=X天*50 元

            List<M_FinanceCCBX.ChuXingDetail> list = ucChuXingMingXi.UCGetCXList();
            int chuChaTianShu = 0;
            string zhuQinText = "";//住勤补贴算法
            if (list.Count > 0)
            {
                DateTime dtStart = list[0].StartMD;
                DateTime dtEnd = list[list.Count - 1].EndMD;
                TimeSpan ts = dtEnd - dtStart;
                chuChaTianShu = SysConvert.ToInt32(ts.TotalDays) + 1; //出差天数
                decimal zhuQinBuTie = 0;
                if (iZhiChengID == 1 || iZhiChengID == 2) //公司领导 处级干部或研高职称
                {
                    zhuQinBuTie = chuChaTianShu * zhuXB_50;
                    zhuQinText = chuChaTianShu.ToString() + "×" + zhuXB_50.ToString();
                }
                else //科级干部或高级职称、普通员工
                {
                    int[] iDays = GetZhuQinTianShu(chuChaTianShu);
                    zhuQinBuTie += iDays[0] * zhu10A_70;
                    zhuQinBuTie += iDays[1] * zhu20A_60;
                    zhuQinBuTie += iDays[2] * zhuXA_40;
                    zhuQinText = iDays[0].ToString() + "×" + zhu10A_70.ToString();
                    zhuQinText += "+" + iDays[1].ToString() + "×" + zhu20A_60.ToString();
                    zhuQinText += "+" + iDays[2].ToString() + "×" + zhuXA_40.ToString();
                }
                txtZhuQinRT.Text = chuChaTianShu.ToString();
                txtZhuQinJE.Text = zhuQinBuTie.ToString();
                txtZhuQinJE.ToolTip = zhuQinText;
            }
            #endregion

            #region 住宿节约补贴
            //住宿节约补贴: 天数等于住宿天数;住宿节约补贴公式:
            //公司领导、处级干部或研高职称：没有此项补贴。
            //科级干部或高级职称：(住宿标准金额-实际住宿金额)*30%;
            //普通员工=（住宿标准金额-实际住宿金额)*50%
            //出差天数10天内算补贴，超出10天部分不算补贴。

            //获得住宿标准
            Decimal zhuSuBiaoZhun = GetZhuSuBiaoZhun(iZhiChengID, chkIsYiXian.Checked);
            //从表单获得住宿天数
            int iZhuSuTianShu = SysConvert.ToInt32(txtZhuSuRT);

            string zhuSuText = "";
            //补贴金额
            Decimal jyBuTieJE = 0;
            //计算补贴天数
            int keBuTianShu = 0;
            if (iZhuSuTianShu <= 10)
                keBuTianShu = iZhuSuTianShu;
            else
                keBuTianShu = 10;

            //计算补贴
            Decimal dZhuShu = 0;
            if (txtZhuSuJE.Text == "")
                dZhuShu = 0;
            else
                dZhuShu = SysConvert.ToDecimal(txtZhuSuJE.Text);

            Decimal dMeiTianZhuSu;
            if (iZhuSuTianShu == 0)
                dMeiTianZhuSu = 0;
            else
                dMeiTianZhuSu = decimal.Round(dZhuShu / iZhuSuTianShu, 2);//每天住宿费用

            if (iZhiChengID == 1 || iZhiChengID == 2)  //公司领导 处级干部或研高职称
            {
                jyBuTieJE = 0;
                keBuTianShu = 0;
                zhuSuText = jyBuTieJE.ToString();
            }
            else if (iZhiChengID == 3) //科级干部或高级职称
            {
                jyBuTieJE = (zhuSuBiaoZhun - dMeiTianZhuSu) * SysConvert.ToDecimal(0.3) * keBuTianShu;

                zhuSuText = "(" + zhuSuBiaoZhun.ToString() + "-" + dMeiTianZhuSu.ToString() + "）" + "×30%×" + keBuTianShu.ToString();
            }
            else if (iZhiChengID == 4) //普通员工
            {
                jyBuTieJE = (zhuSuBiaoZhun - dMeiTianZhuSu) * SysConvert.ToDecimal(0.5) * keBuTianShu;
                zhuSuText = "(" + zhuSuBiaoZhun.ToString() + "-" + dMeiTianZhuSu.ToString() + "）" + "×50%×" + keBuTianShu.ToString();
            }
            //住宿节约 人日
            txtZhuSuJYRT.Text = keBuTianShu.ToString();
            txtZhuSuJYJE.Text = jyBuTieJE.ToString();
            txtZhuSuJYJE.ToolTip = zhuSuText;
            #endregion

            //自行住宿补贴
            if (txtZiXingTianShu1.Text == "")
                txtZiXingTianShu1.Text = "0";
            int iZiXingTianShu = SysConvert.ToInt32(txtZiXingTianShu1.Text);
            txtZiXingTianShu2.Text = iZiXingTianShu.ToString();
            txtZiXingJinE.Text = (iZiXingTianShu * 50).ToString();
            txtZiXingJinE.ToolTip = iZiXingTianShu.ToString() + "×50";

            //其他费用小计
            decimal qiTaXiaoJi = SysConvert.ToDecimal(txtZhuSuJE.Text) + SysConvert.ToDecimal(txtTuoYunJE.Text) +
                SysConvert.ToDecimal(txtQiTaJE.Text);
            txtQiTaXiaoJi.Text = qiTaXiaoJi.ToString();

            //补贴小计
            decimal buTieXiaoJi = SysConvert.ToDecimal(txtZhuQinJE.Text) + SysConvert.ToDecimal(txtZhuSuJYJE.Text) +
                SysConvert.ToDecimal(txtWeiWoPuBuJE.Text) + SysConvert.ToDecimal(txtZaiTuJE.Text);
            txtBuTieXiaoJi.Text = buTieXiaoJi.ToString();

            Decimal total = 0;

            foreach (var item in list)
            {
                total += item.CheChuanPiao + item.ShiNeiJiaoTong;
            }
            total += qiTaXiaoJi;
            total += buTieXiaoJi;
            txtHeJi.Text = total.ToString();
        }
        /// <summary>
        /// 获得住勤天数 
        /// </summary>
        /// <returns></returns>
        private int[] GetZhuQinTianShu(int chuChaTianShu)
        {
            int[] iDays = new int[3];
            if (chuChaTianShu <= 10)
            {
                iDays[0] = chuChaTianShu;
                iDays[1] = 0;
                iDays[2] = 0;
            }
            else if (chuChaTianShu <= (10 + 20))
            {
                iDays[0] = 10;
                iDays[1] = chuChaTianShu - 10;
                iDays[2] = 0;
            }
            else
            {
                iDays[0] = 10;
                iDays[1] = 20;
                iDays[2] = chuChaTianShu - (10 + 20);
            }
            return iDays;
        }

        /// <summary>
        /// 住宿标准
        /// </summary>
        /// <param name="postID">职务或职称ID</param>
        /// <param name="isYiXian">是否一线城市</param>
        /// <returns></returns>
        private Decimal GetZhuSuBiaoZhun(int iZhiChengID, bool isYiXian)
        {
            //1）：公司领导：实际发生费用。
            //2）：处级干部或研高职称：深圳、广州、上海、北京500元，其他城市480元；
            //3）：科级干部或高级职称：深圳、广州、上海、北京400元，其他城市350元；
            //4）：普通员工：深圳、广州、上海、北京300元，其他城市240元
            if (iZhiChengID == 1) //公司领导
            {
                return 0;
            }
            else if (iZhiChengID == 2) //处级（含研高）
            {
                if (isYiXian)
                    return 500;
                else
                    return 480;
            }
            else if (iZhiChengID == 3) //科级（含高工）
            {
                if (isYiXian)
                    return 400;
                else
                    return 350;
            }
            else //普通员工
            {
                if (isYiXian)
                    return 300;
                else
                    return 240;
            }
        }
        protected void btnCal2_Click(object sender, EventArgs e)
        {
            if (txtHeJi.Text == "")
            {
                return;
            }
            double d = Convert.ToDouble(txtHeJi.Text);
            MoneyCal moneyCal = new MoneyCal(d);
            txtCaiWuJE.Text = moneyCal.Convert();
        }
        #endregion

        private void CaiWuFeiYong()
        {
            //财务审核完成后调用 处室发生费用
            string year = DateTime.Now.Year.ToString();
            B_FinanceDeptInfo bllFinanceInfo = new B_FinanceDeptInfo();
            int result = 0;
            double heji = 0;
            if (txtHeJi.Text != "")
                heji = Convert.ToDouble(txtHeJi.Text);
            //取得处室
            string chuShiID = ddlBianZhiBuMen.SelectedValue;
            if (txtType.Text == "出差")
            {
                result = bllFinanceInfo.UpdateFinanceFeesByDeptID(year, chuShiID, heji, 0);
            }
            else if (txtType.Text == "培训")
            {
                result = bllFinanceInfo.UpdateFinanceFeesByDeptID(year, chuShiID, 0, heji);
            }
        }

    }
}