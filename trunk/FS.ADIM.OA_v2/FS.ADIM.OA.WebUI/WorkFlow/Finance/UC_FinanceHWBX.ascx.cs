//----------------------------------------------------------------
// Copyright (C) 2013 
//
// 文件功能描述：出差(培训)
// 
// 
// 创建标识：ZHOULI 2013-04-20
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
    public partial class UC_FinanceHWBX : FormsUIBase
    {
        #region 页面加载
        protected string FileTitle = "海南核电有限公司会务费用报销单";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SubmitEvents();
        }

        #endregion

        #region 控件初始设置 页面加载时触发 先触发实体加载
        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            B_FinanceHWBX entity = base.EntityData != null ? base.EntityData as B_FinanceHWBX : new B_FinanceHWBX();

            //base.StepName = "审核";
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
                            this.txtNiGaoRiQi, 
                            this.txtDocumentNo,txtLiXiangHao,txtLiXiangJinE,txtLiXiangLeiJiJinE,
                            this.ddlShenQianRen,ddlShenHe,ddlShenPi,ddlCaiWu
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnTongYi,this.btnBack,this.btnShenPi,this.btnShenHe,this.btnCaiWu,this.btnShenQian,this.btnWanCheng
                        };
                        this.txtDocumentNo.ToolTip = "提交后生成";

                        if (entity.IsBack == true)
                        {
                            btnCancel.Visible = true;
                        }
                        break;
                    #endregion

                    #region 验收
                    case ProcessConstString.StepName.FinanceHWBXStepName.STEP_YANSHOU:

                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,this.txtNiGaoRiQi, 
                            this.txtDocumentNo,txtLiXiangHao,txtLiXiangJinE,txtLiXiangLeiJiJinE,
                            ddlShenHe,ddlShenPi,ddlCaiWu,
                            this.ddlYanShouRen,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                             this.btnShenPi,this.btnShenHe,this.btnCaiWu,this.btnYanShou,this.btnTongYi,this.btnCancel,this.btnWanCheng
                        };
                        break;
                    #endregion

                    #region 立项审签
                    case ProcessConstString.StepName.FinanceHWBXStepName.STEP_LIXIANGSHENQIAN:

                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,this.txtNiGaoRiQi, 
                            this.txtDocumentNo,this.txtShouKuanYinHang,
                            this.ddlShenQianRen,ddlShenPi,ddlCaiWu,
                            this.txtDanJuZhangShu,this.ddlBianZhiBuMen,this.txtYongTu,
                            this.txtShouKuanDanWei,this.txtShouKuanZhangHao,this.txtBaoXiaoJinE,this.txtBaoXiaoJinEDaXie,
                            this.ddlYanShouRen,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnShenQian,this.btnShenPi,this.btnCaiWu,this.btnYanShou,this.btnTongYi,this.btnCancel,this.btnWanCheng
                        };
                        break;
                    #endregion

                    #region 审核
                    case ProcessConstString.StepName.FinanceHWBXStepName.STEP_SHENHE:
                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,this.txtNiGaoRiQi,
                            this.txtDocumentNo,txtLiXiangHao,txtLiXiangJinE,txtLiXiangLeiJiJinE,
                            this.ddlShenQianRen,ddlShenHe,
                            this.ddlBianZhiBuMen,
                            this.ddlYanShouRen,
                            //this.txtDanJuZhangShu,this.txtYongTu,this.txtShouKuanDanWei, this.txtShouKuanYinHang,
                            //this.txtShouKuanZhangHao,this.txtBaoXiaoJinE,this.txtBaoXiaoJinEDaXie,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                           this.btnShenHe,  this.btnShenQian,this.btnYanShou,this.btnTongYi,this.btnCancel,this.btnWanCheng
                        };
                        break;
                    #endregion

                    #region 审批
                    case ProcessConstString.StepName.FinanceHWBXStepName.STEP_SHENPI:
                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,this.txtNiGaoRiQi, this.txtShouKuanYinHang,
                            this.txtDocumentNo,txtLiXiangHao,txtLiXiangJinE,txtLiXiangLeiJiJinE,
                            this.ddlShenQianRen,ddlShenHe,ddlShenPi,
                            this.txtDanJuZhangShu,this.ddlBianZhiBuMen,this.txtYongTu,
                            this.txtShouKuanDanWei,this.txtShouKuanZhangHao,this.txtBaoXiaoJinE,this.txtBaoXiaoJinEDaXie,
                            this.ddlYanShouRen,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnShenPi,  this.btnShenHe,  this.btnShenQian,this.btnYanShou,this.btnTongYi,this.btnCancel,this.btnWanCheng
                        };
                        break;
                    #endregion

                    #region 财务审核
                    case ProcessConstString.StepName.FinanceHWBXStepName.STEP_CAIWUSHENHE:

                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,this.txtNiGaoRiQi, 
                            this.txtDocumentNo,txtLiXiangHao,txtLiXiangJinE,txtLiXiangLeiJiJinE,
                            this.ddlShenQianRen,ddlShenHe,ddlShenPi,ddlCaiWu,
                            this.txtDanJuZhangShu,this.ddlBianZhiBuMen,this.txtYongTu,
                            this.ddlYanShouRen,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnShenPi,this.btnShenHe,this.btnCaiWu, this.btnShenQian,this.btnYanShou,this.btnCancel,this.btnWanCheng
                        };
                        break;
                    #endregion

                    #region 反馈报销人
                    case ProcessConstString.StepName.FinanceHWBXStepName.STEP_FANKUI:

                        //控件状态控制
                        controls.DisEnableControls = new Control[] 
                        { 
                            this.txtNiGaoRen,this.txtNiGaoRiQi, this.txtShouKuanYinHang,
                            this.txtDocumentNo,txtLiXiangHao,txtLiXiangJinE,txtLiXiangLeiJiJinE,
                            this.ddlShenQianRen,ddlShenHe,ddlShenPi,ddlCaiWu,
                            this.txtDanJuZhangShu,this.ddlBianZhiBuMen,this.txtYongTu,
                            this.txtShouKuanDanWei,this.txtShouKuanZhangHao,this.txtBaoXiaoJinE,this.txtBaoXiaoJinEDaXie,
                            this.ddlYanShouRen,
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            this.btnSave,  this.btnShenPi,this.btnShenHe,this.btnCaiWu,this.btnBack,this.btnTongYi,   this.btnShenQian,this.btnYanShou,this.btnCancel
                        };
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
            B_FinanceHWBX entity = base.EntityData != null ? base.EntityData as B_FinanceHWBX : new B_FinanceHWBX();

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
            }
            else
            {
                FormsMethod.SetDropDownList(this.ddlBianZhiBuMen, entity.DepartmentID, entity.Department);

                FormsMethod.SetDropDownList(this.ddlYanShouRen, entity.YanShouRenID, entity.YanShouRen);
                FormsMethod.SetDropDownList(this.ddlShenQianRen, entity.ShenQianRenID, entity.ShenQianRen);
                FormsMethod.SetDropDownList(this.ddlShenHe, entity.ShenHeRenID, entity.ShenHeRen);
                FormsMethod.SetDropDownList(this.ddlShenPi, entity.ShenPiRenID, entity.ShenPiRen);

                this.txtNiGaoRen.Text = entity.Drafter;
                this.txtNiGaoRenID.Text = entity.DrafterID;
            }

            //验收
            if (base.StepName == ProcessConstString.StepName.FinanceHWBXStepName.STEP_YANSHOU)
            {
                OAUser.GetUserByRole(this.ddlShenQianRen, "立项审签");
            }
            //立项审签
            if (base.StepName == ProcessConstString.StepName.FinanceHWBXStepName.STEP_LIXIANGSHENQIAN)
            {
                if (this.ddlBianZhiBuMen.Items.Count > 0)
                {
                    OAUser.GetUserByDeptPost(this.ddlShenHe, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.FUCHUZHANG, true, true);
                }

            }
            //审核
            if (base.StepName == ProcessConstString.StepName.FinanceHWBXStepName.STEP_SHENHE)
            {
                OAUser.GetUserByRole(this.ddlShenPi, OUConstString.RoleName.COMPANY_LEADER);
                OAUser.GetUserByRole(this.ddlCaiWu, OUConstString.RoleName.CaiWu);
            }
            //审批
            if (base.StepName == ProcessConstString.StepName.FinanceHWBXStepName.STEP_SHENPI)
            {
                OAUser.GetUserByRole(this.ddlCaiWu, OUConstString.RoleName.CaiWu);
            }
            //财务
            if (base.StepName == ProcessConstString.StepName.FinanceHWBXStepName.STEP_CAIWUSHENHE)
            {

            }
            //反馈报销人
            if (base.StepName == ProcessConstString.StepName.FinanceHWBXStepName.STEP_FANKUI)
            {

            }

            if (txtNiGaoRen.Text == "")
                txtNiGaoRen.Text = entity.Drafter;

            if (txtNiGaoRiQi.Text == "")
                txtNiGaoRiQi.Text = entity.DraftDate.ToString("yyyy-MM-dd");

            txtDocumentNo.Text = entity.DocumentNo;

            FormsMethod.SelectedDropDownList(this.ddlBianZhiBuMen, entity.DepartmentID, entity.Department);

            if (entity.DanJuZhangShu == Int32.MinValue)
                txtDanJuZhangShu.Text = "";
            else
                txtDanJuZhangShu.Text = entity.DanJuZhangShu.ToString();

            txtYongTu.Text = entity.YongTu;

            txtShouKuanYinHang.Text = entity.ShouKuanYinHang;
            txtShouKuanDanWei.Text = entity.ShouKuanDanWei;
            txtShouKuanZhangHao.Text = entity.ShouKuanZhangHao;
            txtBaoXiaoJinE.Text = entity.BaoXiaoJinE;
            txtBaoXiaoJinEDaXie.Text = entity.BaoXiaoJinEDaXie;

            txtLiXiangHao.Text = entity.LiXiangHao;

            txtLiXiangJinE.Text = entity.LiXiangJinE.ToString();

            txtLiXiangLeiJiJinE.Text = entity.LiXiangLeiJiJinE.ToString();

            FormsMethod.SelectedDropDownList(this.ddlYanShouRen, entity.YanShouRenID, entity.YanShouRen);
            FormsMethod.SelectedDropDownList(this.ddlShenQianRen, entity.ShenQianRenID, entity.ShenQianRen);
            FormsMethod.SelectedDropDownList(this.ddlShenHe, entity.ShenHeRenID, entity.ShenHeRen);
            FormsMethod.SelectedDropDownList(this.ddlShenPi, entity.ShenPiRenID, entity.ShenPiRen);

            FormsMethod.SelectedDropDownList(this.ddlCaiWu, entity.CaiWuID, entity.CaiWu);

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
            B_FinanceHWBX entity = base.EntityData != null ? base.EntityData as B_FinanceHWBX : new B_FinanceHWBX();

            switch (base.StepName)
            {
                #region 拟稿
                case ProcessConstString.StepName.STEP_DRAFT:

                    entity.DocumentNo = txtDocumentNo.Text;

                    //拟稿日期
                    entity.DraftDate = DateTime.Now;
                    //拟稿人 
                    entity.Drafter = this.txtNiGaoRen.Text;
                    entity.DrafterID = this.txtNiGaoRenID.Text;

                    //标题、主题
                    entity.DocumentTitle = entity.Drafter + "-" + "会务报销单";

                    //编制部门
                    if (this.ddlBianZhiBuMen.Items.Count > 0)
                    {
                        entity.Department = this.ddlBianZhiBuMen.SelectedItem.Text;
                        entity.DepartmentID = this.ddlBianZhiBuMen.SelectedValue;
                    }

                    //验收
                    entity.YanShouRen = this.ddlYanShouRen.SelectedItem.Text;
                    entity.YanShouRenID = this.ddlYanShouRen.SelectedValue;

                    //提交后产生报销单编号 报销单的编号为BX+部门代码+年度+四位流水号。
                    if (IsSave == false && entity.DocumentNo == "")
                    {
                        string year = DateTime.Now.Year.ToString();

                        string deptNo = OADept.GetDeptByDeptID(entity.DepartmentID).No;

                        entity.DocumentNo = "HWBX" + "-" + deptNo + "-" + entity.GenerateBianHao(year, "会务报销单");
                    }
                    break;
                #endregion

                #region 验收
                case ProcessConstString.StepName.FinanceHWBXStepName.STEP_YANSHOU:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;

                        entity.YanShouRiQi = DateTime.Now.ToString("yyyy-MM-dd");

                        entity.ShenQianRen = this.ddlShenQianRen.SelectedItem.Text;
                        entity.ShenQianRenID = this.ddlShenQianRen.SelectedValue;
                    }

                    break;
                #endregion

                #region 立项审签
                case ProcessConstString.StepName.FinanceHWBXStepName.STEP_LIXIANGSHENQIAN:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;

                        entity.ShenQianRiQi = DateTime.Now.ToString("yyyy-MM-dd");

                        entity.ShenHeRen = this.ddlShenHe.SelectedItem.Text;
                        entity.ShenHeRenID = this.ddlShenHe.SelectedValue;
                    }
                    break;
                #endregion

                #region 审核
                case ProcessConstString.StepName.FinanceHWBXStepName.STEP_SHENHE:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;

                        entity.ShenHeRiQi = DateTime.Now.ToString("yyyy-MM-dd");

                        entity.ShenPiRen = this.ddlShenPi.SelectedItem.Text;
                        entity.ShenPiRenID = this.ddlShenPi.SelectedValue;

                        entity.CaiWu = this.ddlCaiWu.SelectedItem.Text;
                        entity.CaiWuID = this.ddlCaiWu.SelectedValue;
                    }
                    break;
                #endregion

                #region 审批
                case ProcessConstString.StepName.FinanceHWBXStepName.STEP_SHENPI:
                    if (base.SubAction == ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        entity.IsBack = true;
                    }
                    else
                    {
                        entity.IsBack = false;
                        entity.ShenPiRiQi = DateTime.Now.ToString("yyyy-MM-dd");

                        entity.CaiWu = this.ddlCaiWu.SelectedItem.Text;
                        entity.CaiWuID = this.ddlCaiWu.SelectedValue;
                    }

                    break;
                #endregion

                #region 财务
                case ProcessConstString.StepName.FinanceHWBXStepName.STEP_CAIWUSHENHE:
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

            entity.LiXiangHao = txtLiXiangHao.Text;
            entity.LiXiangJinE = txtLiXiangJinE.Text;
            entity.LiXiangLeiJiJinE = txtLiXiangLeiJiJinE.Text;
            entity.ShouKuanYinHang = txtShouKuanYinHang.Text;
            entity.ShouKuanDanWei = txtShouKuanDanWei.Text;
            entity.ShouKuanZhangHao = txtShouKuanZhangHao.Text;
            entity.BaoXiaoJinE = txtBaoXiaoJinE.Text;
            entity.BaoXiaoJinEDaXie = txtBaoXiaoJinEDaXie.Text;
            entity.DanJuZhangShu = SysConvert.ToInt32(txtDanJuZhangShu.Text);
            entity.YongTu = txtYongTu.Text;

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

        #region 绑定按钮提交事件
        /// <summary>
        /// 绑定表单事件
        /// </summary>
        protected void SubmitEvents()
        {
            EventHandler SubmitHandler = new EventHandler(SubmitBtn_Click);
            this.btnYanShou.Click += SubmitHandler;
            this.btnShenQian.Click += SubmitHandler;
            this.btnTongYi.Click += SubmitHandler;
            this.btnShenPi.Click += SubmitHandler;
            this.btnShenHe.Click += SubmitHandler;
            this.btnCaiWu.Click += SubmitHandler;
            this.btnCancel.Click += SubmitHandler;
            this.btnWanCheng.Click += SubmitHandler;
            //保存
            this.btnSave.Click += SubmitHandler;
            //退回
            this.btnBack.Click += SubmitHandler;
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
                base.SubAction = ((Button)sender).ToolTip.Trim();

                string strErrorMessage = string.Empty;

                //保存
                if (base.SubAction == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT)
                {
                    B_FinanceHWBX entity = ControlToEntity(true) as B_FinanceHWBX;
                    entity.SubmitAction = base.SubAction;
                    base.FormSubmit(true, base.SubAction, null, entity);
                }
                else
                {
                    B_FinanceHWBX entity = ControlToEntity(false) as B_FinanceHWBX;
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

        protected void ddlBianZhiBuMen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlBianZhiBuMen.Items.Count > 0)
            {
                //GetChildDeptIDSConSelf
                OAUser.GetUserByDeptPost(this.ddlYanShouRen, this.ddlBianZhiBuMen.SelectedValue, OUConstString.PostName.FUKEZHANG, true, true,-1);
            }
        }
        #endregion
    }
}