using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.WebUI.UIBase;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Entity;
using System.Collections;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public partial class UC_FinanceZDBX : FormsUIBase
    {

        protected string FileTitle = "海南核电有限公司出差（培训）申请单";

        protected void Page_Load(object sender, EventArgs e)
        {
            //InitPrint();
            this.SubmitEvents();
        }

        #region 控件初始设置
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string strNewLine = "<br/>";
        private string FeeRate = OAConfig.GetConfig("差旅费大于预算比率", "Rate");
        private int SubmitStatus = 0;

        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            B_FinanceZDBX entity = base.EntityData != null ? base.EntityData as B_FinanceZDBX : new B_FinanceZDBX();

            //附件
            ucAttachment.UCTemplateName = base.TemplateName;
            ucAttachment.UCProcessID = base.ProcessID;
            ucAttachment.UCWorkItemID = base.WorkItemID;
            ucAttachment.UCTBID = base.IdentityID.ToString();

            OAControl controls = new OAControl();

            if (!base.IsPreview)
            {
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

                        this.txtDocumentNo.ToolTip = "提交后自动生成";

                        controls.EnableControls = new Control[] { this.btnTiJiaoShenHe, this.btnSave };
                        controls.YellowControls = new Control[] { this.txtChaoSong };

                        if (txtIsManager.Text == "1")
                        {
                            controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlZongJingLi, ddlZhuGuanLingDao, ddlJingShouRen, ddlYanShouRen };
                        }
                        else
                        {
                            controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlZongJingLi, ddlZhuGuanLingDao, ddlJingShouRen, ddlChuLingDao };
                        }

                        break;
                    #endregion

                    #region 验收

                    case ProcessConstString.StepName.FinanceZDBXStepName.STEP_KeZhang:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave };
                        controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlZongJingLi, ddlZhuGuanLingDao, ddlJingShouRen, ddlYanShouRen };
                        break;
                    #endregion

                    #region 处长审核
                    case ProcessConstString.StepName.FinanceZDBXStepName.STEP_ChuZhang:
                        if (txtIsManager.Text != "1")
                        {
                            Double FeeFa = Convert.ToDouble(string.IsNullOrEmpty(txtLeiJiBaoXiaoJinE.Text) ? "0" : txtLeiJiBaoXiaoJinE.Text);
                            Double FeeYu = Convert.ToDouble(string.IsNullOrEmpty(txtYuSuanJinE.Text) ? "0" : txtYuSuanJinE.Text);
                            Double Rate = (string.IsNullOrEmpty(FeeRate) == true ? 0.1 : Convert.ToDouble(FeeRate));
                            if ((FeeFa - FeeYu) / FeeYu > Rate)
                            {
                                controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, ddlZongJingLi };
                                controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlYanShouRen, ddlChuLingDao,ddlZhuGuanLingDao,ddlJingShouRen };
                            }
                            else if (FeeFa > FeeYu)
                            {

                                controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, ddlZhuGuanLingDao };
                                controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlYanShouRen, ddlChuLingDao, ddlZongJingLi, ddlJingShouRen };
                            }
                            else
                            {
                                controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave };
                                controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlYanShouRen, ddlChuLingDao, ddlZhuGuanLingDao ,ddlZongJingLi};
                            }
                        }

                        else
                        {
                            controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, ddlZongJingLi, ddlZhuGuanLingDao };
                            controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, ddlYanShouRen, ddlChuLingDao };
                        }
                 
                        break;
                    #endregion

                    #region 领导审核
                    case ProcessConstString.StepName.FinanceZDBXStepName.STEP_LingDao:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave, ddlJingShouRen };
                        controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao, this.ddlYanShouRen };
                        break;
                    #endregion

                    #region 总经理审核
                    case ProcessConstString.StepName.FinanceStepName.STEP_GeneralManager:
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnShenHeWanCheng, this.btnTuiHui, this.btnSave };       
                        controls.DisEnableControls = new Control[] { this.txtYuSuanJinE, this.txtLeiJiBaoXiaoJinE, this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao, this.ddlYanShouRen };
                        break;
                    #endregion

                    #region 财务
                    case ProcessConstString.StepName.FinanceZDBXStepName.STEP_Finance:

                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnTuiHui, this.btnSave, this.btnShenHeWanCheng };
                        controls.DisEnableControls = new Control[] { this.ddlZongJingLi, this.ddlZhuGuanLingDao, this.ddlChuLingDao, this.ddlYanShouRen, ddlJingShouRen };

                        break;
                    #endregion

                    #region 反馈报销人
                    case ProcessConstString.StepName.FinanceStepName.STEP_FeedBack:

                        FormsMethod.SetControlAll(this);
                        controls.EnableControls = new Control[] { this.OASelectUC1, this.btnApplyComplete, this.btnSave, txtTianJia };

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
            B_FinanceZDBX entity = base.EntityData != null ? base.EntityData as B_FinanceZDBX : new B_FinanceZDBX();
            //附件
            ucAttachment.UCDataList = entity.FileList;

            this.txtYuSuanJinE.Text = entity.YuSuanJinE;
            this.txtLeiJiBaoXiaoJinE.Text = entity.LeiJiBaoXiaoJinE;
            this.txtIsChaoYuSuan.Text = entity.IsChaoYuSuan == true ? "1" : "";
            this.txtIsManager.Text = entity.IsLeader == true ? "1" : "";
            string userID = string.IsNullOrEmpty(entity.ReceiveUserID) ? CurrentUserInfo.UserName : entity.ReceiveUserID;
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT && !base.IsPreview)
            {
                //拟稿日期
                txtNiGaoRiQi.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //拟稿人
                this.txtNiGaoRen.Text = CurrentUserInfo.DisplayName;
                this.txtNiGaoRenID.Text = CurrentUserInfo.UserName;
                OADept.GetDeptByUser(this.ddlDepartment, userID, 1, true, false);
                FormsMethod.SelectedDropDownList(this.ddlDepartment, entity.DepartmentID);

                string[] ManagerIds = OAUser.GetDeptManagerArray(this.ddlDepartment.SelectedValue, -1);

                if (ManagerIds[0].Contains(CurrentUserInfo.UserName))
                {
                    txtIsManager.Text = "1";
                }
                if (txtIsManager.Text == "1")
                {
                    OAUser.GetUserByDeptPost(ddlChuLingDao, this.ddlDepartment.SelectedValue, OUConstString.PostName.FUCHUZHANG, false, true, 0);
                }
                else
                {
                    FounderSoftware.Framework.Business.ViewBase vDept = OADept.GetDeptByDeptUser(this.ddlDepartment.SelectedValue, userID, 2);
                    //根据处室部门的ID和用户账号找出用户所属该处室下的科室-修改bug
                    if (!string.IsNullOrEmpty(vDept.IDs))
                    {
                        OAUser.GetUserByDeptPost(ddlYanShouRen, vDept.IDs, OUConstString.PostName.FUKEZHANG, false, true, 2);
                    }
                }

                B_FinanceDeptInfo bllInfo = new B_FinanceDeptInfo();
                M_FinanceDeptInfo info = bllInfo.GetFinanceDeptInfoByDeptID(DateTime.Now.Year.ToString(), this.ddlDepartment.SelectedValue);
                this.txtYuSuanJinE.Text = info.ZDBudgetCost;
                this.txtLeiJiBaoXiaoJinE.Text = info.ZDUseCost;

                if (!string.IsNullOrEmpty(entity.ChuLingDaoID))
                {
                    FormsMethod.SelectedDropDownList(this.ddlChuLingDao, entity.ChuLingDaoID);
                }

                if (!string.IsNullOrEmpty(entity.YanShouRenID))
                {
                    FormsMethod.SelectedDropDownList(this.ddlYanShouRen, entity.YanShouRenID);
                }
            }
            else
            {
                FormsMethod.SetDropDownList(this.ddlDepartment, entity.DepartmentID, entity.Department);
                FormsMethod.SetDropDownList(this.ddlZongJingLi, entity.ZongJingLiID, entity.ZongJingLi);
                FormsMethod.SetDropDownList(this.ddlZhuGuanLingDao, entity.ZhuGuanLingDaoID, entity.ZhuGuanLingDao);
                FormsMethod.SetDropDownList(this.ddlChuLingDao, entity.ChuLingDaoID, entity.ChuLingDao);
                FormsMethod.SetDropDownList(this.ddlYanShouRen, entity.YanShouRenID, entity.YanShouRen);
                FormsMethod.SetDropDownList(this.ddlJingShouRen, entity.JingShouRenID, entity.JingShouRen);
            }

            //验收
            if (base.StepName == ProcessConstString.StepName.FinanceZDBXStepName.STEP_KeZhang && !base.IsPreview)
            {
                OAUser.GetUserByDeptPost(ddlChuLingDao, this.ddlDepartment.SelectedValue, OUConstString.PostName.CHUZHANG, false, true, 0);
            }

            //处长审核
            if (base.StepName == ProcessConstString.StepName.FinanceZDBXStepName.STEP_ChuZhang && !base.IsPreview)
            {
                OAUser.GetUserByRole(this.ddlZongJingLi, OUConstString.RoleName.COMPANY_LEADER);
                OAUser.GetUserByRole(this.ddlZhuGuanLingDao, OUConstString.RoleName.COMPANY_LEADER);
                OAUser.GetUserByRole(this.ddlJingShouRen, OUConstString.RoleName.CaiWu);
                if (txtIsManager.Text != "1")
                {
                    B_FinanceDeptInfo bllInfo = new B_FinanceDeptInfo();
                    M_FinanceDeptInfo info = bllInfo.GetFinanceDeptInfoByDeptID(DateTime.Now.Year.ToString(), this.ddlDepartment.SelectedValue);
                    this.txtYuSuanJinE.Text = info.ZDBudgetCost;
                    this.txtLeiJiBaoXiaoJinE.Text = info.ZDUseCost;
                    Double FeeFa = Convert.ToDouble(string.IsNullOrEmpty(txtLeiJiBaoXiaoJinE.Text) ? "0" : txtLeiJiBaoXiaoJinE.Text);
                    Double FeeYu = Convert.ToDouble(string.IsNullOrEmpty(txtYuSuanJinE.Text) ? "0" : txtYuSuanJinE.Text);
                    Double Rate = (string.IsNullOrEmpty(FeeRate) == true ? 0.1 : Convert.ToDouble(FeeRate));
                    if (FeeFa > FeeYu)
                    {
                        txtIsChaoYuSuan.Text = "1";
                    }
                }
            }

            //领导审核
            if (base.StepName == ProcessConstString.StepName.FinanceZDBXStepName.STEP_LingDao && !base.IsPreview)
            {
                OAUser.GetUserByRole(this.ddlJingShouRen, OUConstString.RoleName.CaiWu);
            }

            //是否退回
            if (base.StepName == ProcessConstString.StepName.STEP_DRAFT)
            {
                //this.txtIsBack.Text = entity.IsBack.ToString();
            }

            //拟稿人及日期
            if (entity.DraftDate != DateTime.MinValue)
            {
                this.txtNiGaoRiQi.Text = entity.DraftDate.ToString(DateFormat);

                //拟稿人显示非下拉列表框形式
                this.txtNiGaoRen.Visible = false;
                this.lblNiGaoRen.Visible = true;
                //this.lbNiGaoRen.Text = entity.Drafter + strNewLine + entity.DraftDate.ToString(ConstString.DateFormat.Long);
                this.lblNiGaoRen.Text = entity.Drafter;

            }
            else
            {
                this.txtNiGaoRiQi.Text = DateTime.Now.ToString(DateFormat);
            }
            //申请单编号
            this.txtDocumentNo.Text = entity.DocumentNo;

            this.txtDanJuZhangShu.Text = entity.DanJuZhangShu == Int32.MinValue ? "" : entity.DanJuZhangShu.ToString();

            this.txtYongTu.Text = entity.YongTu;

            txtBaoXiaoJinE.Text = entity.BaoXiaoJinE;
            txtBaoXiaoJinEDaXie.Text = entity.BaoXiaoJinEDaXie;

            this.txtShouKuanDanWei.Text = entity.ShouKuanDanWei;

            this.txtShouKuanZhangHao.Text = entity.ShouKuanZhangHao;

            this.txtShouKuanYinHang.Text = entity.ShouKuanYinHang;

            this.txtShouKuanZhangHao.Text = entity.ShouKuanZhangHao;

            this.txtYanQingRenShu.Text = entity.YanQingRenShu;

            this.txtRenJunXiaoFeiE.Text = entity.RenJunXiaoFeiE;

            ////拟稿人
            //this.txtNiGaoRen.Text = entity.Drafter;
            //this.txtNiGaoRenID.Text = entity.NiGaoRenID;

            //提示信息
            this.txtTiShiXinXi.Text = entity.Message;

            //提示信息添加
            this.txtTianJia.Text = entity.MessageAdd;

            //this.txtGeneralManagerID.Text = entity.GeneralManagerID;

            //this.txtChargeLeaderID.Text = entity.ChargeLeaderID;

            //this.txtDepartmentLeaderID.Text = entity.DepartmentLeaderID;
        }
        #endregion

        #region 实体赋值
        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="IsSave"></param>
        /// <returns></returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            B_FinanceZDBX entity = base.EntityData != null ? base.EntityData as B_FinanceZDBX : new B_FinanceZDBX();

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

                    entity.DocumentNo = txtDocumentNo.Text;

                    //拟稿日期
                    entity.DraftDate = DateTime.Now;
                    //拟稿人 
                    entity.Drafter = this.txtNiGaoRen.Text;
                    entity.DrafterID = this.txtNiGaoRenID.Text;

                    //标题、主题
                    entity.DocumentTitle = entity.Drafter + "-" + "招待报销单";

                    //编制部门
                    if (this.ddlDepartment.Items.Count > 0)
                    {
                        entity.Department = this.ddlDepartment.SelectedItem.Text;
                        entity.DepartmentID = this.ddlDepartment.SelectedValue;
                    }

                    //验收
                    if (this.ddlYanShouRen.SelectedItem != null)
                    {
                        entity.YanShouRen = this.ddlYanShouRen.SelectedItem.Text;
                        entity.YanShouRenID = this.ddlYanShouRen.SelectedValue;
                    }

                    if (this.ddlChuLingDao.SelectedItem != null)
                    {
                        entity.ChuLingDao = this.ddlChuLingDao.SelectedItem.Text;
                        entity.ChuLingDaoID = this.ddlChuLingDao.SelectedValue;
                    }

                    //提交后产生报销单编号 报销单的编号为BX+部门代码+年度+四位流水号。
                    if (IsSave == false && entity.DocumentNo == "")
                    {
                        entity.DocumentNo = GetFinanceNo(entity);
                    }

                    entity.LeiJiBaoXiaoJinE = this.txtLeiJiBaoXiaoJinE.Text;
                    entity.YuSuanJinE = this.txtYuSuanJinE.Text;

                    //拟稿人
                    entity.Drafter = this.txtNiGaoRen.Text;
                    //entity.DrafterID = this.txtNiGaoRenID.Text;
                    //拟稿日期
                    entity.DraftDate = DateTime.Now;
                    //this.GetFeeComparedSigner(entity);

                    break;
                #endregion

                #region 验收-科长
                case ProcessConstString.StepName.FinanceZDBXStepName.STEP_KeZhang:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        if (this.ddlChuLingDao.SelectedItem != null)
                        {
                            entity.ChuLingDao = this.ddlChuLingDao.SelectedItem.Text;
                            entity.ChuLingDaoID = this.ddlChuLingDao.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IsBack = true;
                    }
                    break;
                #endregion

                #region 处长
                case ProcessConstString.StepName.FinanceZDBXStepName.STEP_ChuZhang:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        if (this.ddlZhuGuanLingDao.SelectedItem != null)
                        {
                            entity.ZhuGuanLingDao = this.ddlZhuGuanLingDao.SelectedItem.Text;
                            entity.ZhuGuanLingDaoID = this.ddlZhuGuanLingDao.SelectedValue;
                        }

                        if (this.ddlZongJingLi.SelectedItem != null)
                        {
                            entity.ZongJingLi = this.ddlZongJingLi.SelectedItem.Text;
                            entity.ZongJingLiID = this.ddlZongJingLi.SelectedValue;
                        }
                        if (this.ddlJingShouRen.SelectedItem != null)
                        {
                            entity.JingShouRen = this.ddlJingShouRen.SelectedItem.Text;
                            entity.JingShouRenID = this.ddlJingShouRen.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IsBack = true;
                    }
                    break;
                #endregion

                #region 领导审核
                case ProcessConstString.StepName.FinanceZDBXStepName.STEP_LingDao:
                    if (base.SubAction != ProcessConstString.SubmitAction.ACTION_DENY)
                    {
                        if (this.ddlJingShouRen.SelectedItem != null)
                        {
                            entity.JingShouRen = this.ddlJingShouRen.SelectedItem.Text;
                            entity.JingShouRenID = this.ddlJingShouRen.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IsBack = true;
                    }
                    break;
                #endregion
                #region 财务审核
                case ProcessConstString.StepName.FinanceZDBXStepName.STEP_Finance:
                    //entity.ShangWuXinXi = this.txtShangWu.Text;
                    break;
                #endregion

            }
            entity.DanJuZhangShu = Convert.ToInt32(txtDanJuZhangShu.Text);
            entity.ShouKuanYinHang = txtShouKuanYinHang.Text;
            entity.ShouKuanDanWei = txtShouKuanDanWei.Text;
            entity.ShouKuanZhangHao = txtShouKuanZhangHao.Text;
            entity.BaoXiaoJinE = txtBaoXiaoJinE.Text;
            entity.BaoXiaoJinEDaXie = txtBaoXiaoJinEDaXie.Text;
            entity.DanJuZhangShu = SysConvert.ToInt32(txtDanJuZhangShu.Text);
            entity.YongTu = txtYongTu.Text;
            entity.YanQingRenShu = this.txtYanQingRenShu.Text;
            entity.RenJunXiaoFeiE = this.txtRenJunXiaoFeiE.Text;
            if (txtIsManager.Text == "1")
            {
                entity.IsLeader = true;
            }
            entity.IsChaoYuSuan =( this.txtIsChaoYuSuan.Text == "1" ? true : false);
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
            //审核完成
            this.btnShenHeWanCheng.Click += SubmitHandler;
            ////撤销
            //this.btnCancel.Click += SubmitHandler;
            //退回
            this.btnTuiHui.Click += SubmitHandler;
            //this.btnApplyComplete.Click += SubmitHandler;
            ////提交审核
            this.btnTiJiaoShenHe.Click += SubmitHandler;
            //流程完成
            this.btnApplyComplete.Click += SubmitHandler;
            ////完成归档
            //this.btnWanChengGuiDang.Click += SubmitHandler;

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
                    B_FinanceZDBX entity = ControlToEntity(true) as B_FinanceZDBX;
                    entity.SubmitAction = strActionName;
                    base.FormSubmit(true, strActionName, null, entity);
                }
                else
                {
                    B_FinanceZDBX entity = ControlToEntity(false) as B_FinanceZDBX;
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
                                        try
                                        {
                                            string strMessage = string.Empty;
                                            //this.Devolve(out strMessage);
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

        private string GetFeeComparedSigner(B_FinanceZDBX entity)
        {
            string SignIDs = string.Empty;

            Double FeeFa = Convert.ToDouble(string.IsNullOrEmpty(txtLeiJiBaoXiaoJinE.Text) ? "0" : txtLeiJiBaoXiaoJinE.Text);
            Double FeeYu = Convert.ToDouble(string.IsNullOrEmpty(txtYuSuanJinE.Text) ? "0" : txtYuSuanJinE.Text);
            Double Rate = (string.IsNullOrEmpty(FeeRate) == true ? 0.1 : Convert.ToDouble(FeeRate));
            if ((FeeFa - FeeYu) / FeeYu > Rate)
            {
                //总经理 - 以及大于10%否（大于就总经理签）
                entity.ZongJingLi = ddlZongJingLi.SelectedItem.Text;
                entity.ZongJingLiID = ddlZongJingLi.SelectedValue;
                base.SubAction = "提交总经理";
            }
            else if (FeeFa > FeeYu)
            {
                //主管领导 - 已经发生的差旅费是否大于预算（大于就主管领导签）
                entity.ZhuGuanLingDao = ddlZhuGuanLingDao.SelectedItem.Text;
                entity.ZhuGuanLingDaoID = ddlZhuGuanLingDao.SelectedValue;
                base.SubAction = "提交主管领导";
            }
            else if (FeeFa <= FeeYu)
            {
                //处领导 - 
                entity.ChuLingDao = ddlChuLingDao.SelectedItem.Text;
                entity.ChuLingDaoID = ddlChuLingDao.SelectedValue;
                base.SubAction = "提交处领导";
            }
            return SignIDs;
        }

        protected string GetFinanceNo(B_FinanceZDBX entity)
        {
            Department dept = OADept.GetDeptByDeptID(this.ddlDepartment.SelectedValue);
            string strLine = "-";
            string strYear = DateTime.Now.Year.ToString();
            B_DocumentNo_A b_documentno_a = new B_DocumentNo_A();
            return "ZDBX" + strLine + dept.No + strLine + b_documentno_a.GetNo(ProcessConstString.TemplateName.FinanceZDBX_APPLY, strYear);
        }
        #endregion
    }
}