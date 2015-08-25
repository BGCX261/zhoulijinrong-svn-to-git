//----------------------------------------------------------------
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：函件发文界面
//
// 
// 创建标识：周理 2010-01-11
//
// 修改标识：任金权 2010-5-10
// 修改描述：修改EntityToControl、ControlToEntity。去除使用HtmlToTextCode、HtmlToTextCode。不需要使用，会统一处理
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OU.OutBLL;


namespace FS.ADIM.OA.WebUI.WorkFlow.LetterSend
{
    public partial class UC_LetterSend : FormsUIBase
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string strNewLine = "<br/>";

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitPrint();
            if (!IsPostBack)
            {

            }
        }
        protected override void BindOUControl()
        {
            if (IsPreview == false)
            {
                OADept.GetDeptByUser(drpSendDept, CurrentUserInfo.LoginName, 1, false, false); //发文部门
            }
        }
        private void HeGaoRen()
        {
            //核稿人
            string deptIDS = OADept.GetChildDeptIDSConSelf(drpSendDept.SelectedValue, -1);

            UCHeGao.UCShowDeptID = deptIDS;  //* 获得下属所有部门ID
            UCHeGao.UCIsSingle = "1";
            UCHeGao.UCDeptUserIDControl = wfHeGaoRenID.ClientID;
            UCHeGao.UCDeptUserNameControl = txtHeGaoRen.ClientID;
            UCHeGao.UCSelectType = "1";
            this.UCHeGao.UCTemplateName = base.TemplateName.Replace("新版","");
            this.UCHeGao.UCFormName = "核稿人";
            if (drpSendDept.SelectedItem != null)
                txtHeGaoRen.ToolTip = drpSendDept.SelectedItem.Text + " 的成员";
        }
        /// <summary>
        /// 控件初始设置
        /// </summary>
        protected override void SetControlStatus()
        {
            btnSetNo.Visible = false;
            btnCheck.Visible = false;

            lblPages.Visible = false;
            lblOurRef.Visible = false;

            ucAttachment.UCTemplateName = base.TemplateName;
            ucAttachment.UCProcessID = base.ProcessID;
            ucAttachment.UCWorkItemID = base.WorkItemID;
            ucAttachment.UCTBID = base.IdentityID.ToString();

            OAControl controls = new OAControl();

            OAControl controlsCommon = new OAControl();

            controlsCommon.DisEnableControls = new Control[]
            {
                txtSignDate,txtHuiQianRenDates,txtHeGaoRenDate,txtNiGaoRenDate
            };

            controlsCommon.DisVisibleControls = new Control[]
            {
                //btnSetNo,btnCheck
            };


            if (IsPreview == false)
            {
                //给于用户提示
                txtSubject.ToolTip = "200字符以内";
                txtSubject.MaxLength = 200;

                txtQianFaRen.ToolTip = "公司领导";
                txtHuiQianRen.ToolTip = "函件会签组";

                txtSignDate.ToolTip = "签发后自动生成";
                txtHeGaoRenDate.ToolTip = "核稿后自动生成";
                txtHuiQianRenDates.ToolTip = "会签后自动生成";
                txtNiGaoRenDate.ToolTip = "拟稿后自动生成";

                #region 弹出选择
                //签发人
                UCQianFa.UCRoleName = "函件签发组";
                UCQianFa.UCUserIDControl = wfQianFaRenID.ClientID;
                UCQianFa.UCUserNameControl = txtQianFaRen.ClientID;
                UCQianFa.UCIsSingle = true;

                //会签人
                UCHuiQian.UCRoleName = "函件会签组";
                UCHuiQian.UCUserIDControl = wfHuiQianRenIDs.ClientID;
                UCHuiQian.UCUserNameControl = txtHuiQianRen.ClientID;
                UCHuiQian.UCIsSingle = false;

                //抄送部门
                UCDeptcc.UCDeptIDControl = txtccDeptIDs.ClientID;
                UCDeptcc.UCDeptNameControl = txtccDept.ClientID;
                UCDeptcc.UCDeptShowType = "1010";
                UCDeptcc.UCSelectType = "0";
                UCDeptcc.UCLevel = "2";
                UCDeptcc.UCALLChecked = "1";

                //抄送领导
                UCccLingDao.UCIsSingle = false;
                UCccLingDao.UCRoleName = "公司领导";
                UCccLingDao.UCUserIDControl = txtccLeaderIDs.ClientID;
                UCccLingDao.UCUserNameControl = txtccLeader.ClientID;

                //主送单位
                UCCompany.UCNoControl = txtCompanyID.ClientID;
                UCCompany.UCNameControl = txtCompany.ClientID;
                UCCompany.UCIsSingle = true;

                //抄送单位
                UCCompanycs.UCNameControl = txtccCompany.ClientID;
                UCCompanycs.UCIsSingle = false;
                ////UCCompanycc.UCNameControl = txtccCompany.ClientID;
                //////UCCompanycc.UCIsSingle = false;
                #endregion

                switch (base.StepName)
                {
                    #region 发起函件
                    case ProcessConstString.StepName.LetterSend.发起函件:

                        wfFaQiRen.Text = CurrentUserInfo.DisplayName;
                        wfFaQiRenID.Text = CurrentUserInfo.UserName;
                        drpSendDept.ToolTip = "我所属的处室";
                        lblYiJian.Text = "备注";

                        //核稿人
                        HeGaoRen();

                        controls.EnableControls = new Control[] 
                        {
                            chkJinJi, chkHuiZhi,txtPages,drpHanJian,txtYourRef,txtEquipmentCode,txtContractNo,drpSendDept,
                            txtSubject,txtContent,txtTo
                        };
                        controls.YellowControls = new Control[] 
                        {
                           txtHeGaoRen, txtQianFaRen,txtHuiQianRen,txtCompany,txtccDept,txtccLeader,txtccCompany
                        };
                        controls.DisEnableControls = new Control[] 
                        { 
                            txtOurRef,
                        };
                        if (string.IsNullOrEmpty(base.WorkItemID))
                        {
                            controls.DisVisibleControls = new Control[] 
                            { 
                                btnAddFenfa,btnCheck,btnGD,btnSencondFenfa,btnSetNo,btnQianFa,btnWanCheng,btnCancel,btnBack
                            };
                        }
                        else //是被退回的 可撤销流程
                        {
                            if (base.IsFromDraft == false)
                            {
                                controls.DisVisibleControls = new Control[] 
                                { 
                                    btnAddFenfa,btnCheck,btnGD,btnSencondFenfa,btnSetNo,btnQianFa,btnWanCheng,btnBack
                                };
                                btnCancel.Attributes.Add("onclick", "javascript: if(!confirm('确定要撤销该流程吗？')){return false;}else{DisableButtons();}");
                            }
                            else
                            {
                                controls.DisVisibleControls = new Control[] 
                                { 
                                    btnAddFenfa,btnCheck,btnGD,btnSencondFenfa,btnSetNo,btnQianFa,btnWanCheng,btnBack,btnCancel
                                };
                            }
                        }
                        break;
                    #endregion

                    #region 核稿
                    case ProcessConstString.StepName.LetterSend.核稿:
                        controls.EnableControls = new Control[] 
                        {
                            chkJinJi, chkHuiZhi,txtPages,drpHanJian,txtYourRef,txtEquipmentCode,txtContractNo,drpSendDept,
                            txtSubject,txtContent,txtTo,txtccCompany
                        };
                        controls.DisEnableControls = new Control[] 
                        { 
                            txtOurRef,txtHeGaoRen,drpSendDept
                        };
                        controls.YellowControls = new Control[] 
                        {
                            txtQianFaRen,txtHuiQianRen,txtCompany,txtccDept,txtccLeader
                        };
                        controls.DisVisibleControls = new Control[] 
                        { 
                            UCHeGao,
                            btnAddFenfa,btnCheck,btnGD,btnSencondFenfa,btnSetNo,btnQianFa,btnWanCheng,btnCancel,btnSubmitHeGao
                        };
                        break;
                    #endregion

                    #region 会签
                    case ProcessConstString.StepName.LetterSend.会签:
                        ucAttachment.UCIsEditable = false;
                        btnSumitQianFa.Text = "同意";
                        controls.EnableControls = new Control[] 
                        {
                            chkJinJi, chkHuiZhi,txtPages,drpHanJian,txtYourRef,txtEquipmentCode,txtContractNo,
                            txtSubject,txtContent,txtTo,txtccCompany
                        };
                        controls.DisEnableControls = new Control[] 
                        { 
                            txtOurRef,txtHeGaoRen,drpSendDept,txtHuiQianRen,txtQianFaRen,
                        };
                        controls.YellowControls = new Control[] 
                        {
                            txtQianFaRen,txtHuiQianRen,txtCompany,txtccDept,txtccLeader
                        };
                        controls.DisVisibleControls = new Control[] 
                        {
                            UCHuiQian,UCHeGao,
                            btnAddFenfa,btnCheck,btnGD,btnSencondFenfa,btnSetNo,btnCancel,btnSubmitHeGao,btnSubmitHuiQian,btnWanCheng,btnQianFa
                        };
                        break;
                    #endregion

                    #region 签发
                    case ProcessConstString.StepName.LetterSend.签发:
                        controls.EnableControls = new Control[] 
                        {
                            chkJinJi, chkHuiZhi,txtPages,drpHanJian,txtYourRef,txtEquipmentCode,txtContractNo,
                            txtSubject,txtContent,txtTo,txtccCompany
                        };
                        controls.DisEnableControls = new Control[] 
                        { 
                            txtOurRef,txtHeGaoRen,drpSendDept,txtHuiQianRen,txtQianFaRen,
                        };
                        controls.YellowControls = new Control[] 
                        {
                            txtCompany,txtccDept,txtccLeader
                        };
                        controls.DisVisibleControls = new Control[] 
                        {
                            UCHuiQian,UCHeGao,UCQianFa,
                            btnAddFenfa,btnCheck,btnGD,btnSencondFenfa,btnSetNo,btnCancel,btnSubmitHeGao,btnSubmitHuiQian,btnSumitQianFa,btnWanCheng
                        };

                        btnQianFa.ToolTip = "提交给函件管理员：" + OAUser.GetUserByRoleName("函件管理员")[1];
                        break;
                    #endregion

                    #region 函件分发
                    case ProcessConstString.StepName.LetterSend.函件分发:
                        btnSetNo.Visible = true;
                        btnCheck.Visible = true;

                        lblPages.Visible = true;
                        lblOurRef.Visible = true;
                        drpHanJian.SelectedIndexChanged += new EventHandler(drpHanJian_SelectedIndexChanged);
                        txtCompanyID.TextChanged += new EventHandler(txtCompanyID_TextChanged);

                        if (base.IsDevolve)
                        {
                            this.btnGD.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                        }
                        controls.EnableControls = new Control[] 
                        {
                            chkJinJi, chkHuiZhi,txtPages,drpHanJian,txtYourRef,txtEquipmentCode,txtContractNo,
                            txtSubject,txtContent,txtOurRef,txtTo,txtccCompany
                        };
                        controls.DisEnableControls = new Control[] 
                        { 
                            txtHeGaoRen,drpSendDept,txtHuiQianRen,txtQianFaRen,
                        };
                        controls.YellowControls = new Control[] 
                        {
                            txtCompany,txtccDept,txtccLeader
                        };
                        controls.DisVisibleControls = new Control[] 
                        {
                            UCHuiQian,UCHeGao,UCQianFa,
                            btnAddFenfa,btnSencondFenfa,btnCancel,btnSubmitHeGao,btnSubmitHuiQian,btnSumitQianFa,btnQianFa
                        };
                        break;
                    #endregion

                    case ProcessConstString.StepName.LetterSend.二次分发:
                        break;
                    default: break;
                }

                controlsCommon.SetControls();
                controls.SetControls();
            }
            //历史表单
            else
            {
                EntityLetterSend entity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();
                ucAttachment.UCIsEditable = false;
                controls.DisEnableControls = new Control[] 
                { 
                     txtOurRef,txtHeGaoRen,drpSendDept,txtHuiQianRen,txtQianFaRen,
                    chkJinJi, chkHuiZhi,txtPages,drpHanJian,txtYourRef,txtEquipmentCode,txtContractNo,
                    txtSubject,txtContent,txtTo,
                    txtCompany,txtccCompany,txtccDept,txtccLeader,txtComment,
                };
                controls.DisVisibleControls = new Control[] 
                {
                    UCHeGao,UCHuiQian,UCQianFa,UCCompany,UCCompanycc,UCccLingDao,UCDeptcc,
                    btnSubmitHeGao,btnSubmitHuiQian,btnSumitQianFa,btnSave,btnSencondFenfa,btnSetNo,btnWanCheng,btnGD,btnCancel,btnQianFa,btnBack,btnAddFenfa
                };

                controlsCommon.SetControls();
                controls.SetControls();
                switch (base.StepName)
                {

                    #region 发起函件
                    case ProcessConstString.StepName.LetterSend.发起函件:

                        break;
                    #endregion

                    #region 核稿
                    case ProcessConstString.StepName.LetterSend.核稿:
                        break;
                    #endregion

                    #region 会签
                    case ProcessConstString.StepName.LetterSend.会签:
                        break;
                    #endregion

                    #region 签发
                    case ProcessConstString.StepName.LetterSend.签发:
                        break;
                    #endregion

                    #region 函件分发
                    case ProcessConstString.StepName.LetterSend.函件分发:
                        if (entity.ReceiveUserID == CurrentUserInfo.UserName) //自己办理的公办才允许追加分发
                        {
                            btnAddFenfa.Visible = true;
                            btnSencondFenfa.Visible = true;

                            //抄送部门
                            UCDeptcc.UCDeptIDControl = txtccDeptIDs.ClientID;
                            UCDeptcc.UCDeptNameControl = txtccDept.ClientID;
                            UCDeptcc.UCDeptShowType = "1010";
                            UCDeptcc.UCSelectType = "0";
                            UCDeptcc.UCLevel = "2";

                            //抄送领导
                            UCccLingDao.UCIsSingle = false;
                            UCccLingDao.UCRoleName = "公司领导";
                            UCccLingDao.UCUserIDControl = txtccLeaderIDs.ClientID;
                            UCccLingDao.UCUserNameControl = txtccLeader.ClientID;

                            UCDeptcc.Visible = true;
                            UCccLingDao.Visible = true;
                            base.StepName = "二次分发";
                            btnSencondFenfa.Visible = true;
                            btnAddFenfa.Visible = true;

                            ucAttachment.UCIsAgain = "1";
                        }
                        break;
                    #endregion
                    default: break;
                }
                if (base.IsCanDevolve)
                {
                    this.btnGD.Visible = true;
                    if (base.IsDevolve)
                    {
                        this.btnGD.Attributes.Add("onclick", "javascript: if(!confirm('该流程已经归档，是否重新归档？')){return false;}else{DisableButtons();}");
                    }
                }
            }
        }
        #endregion

        #region 实体与控件之间的绑定
        private void EntityToHuiQian(EntityLetterSend entity)
        {
            if (entity.huiQian.Count > 0)//新版从list里取
            {
                string shuiqian = "";
                if (entity.huiQian.Count > 0)
                {
                    for (int i = 0; i < entity.huiQian.Count; i++)
                    {
                        if (entity.huiQian[i].ICount == entity.iHuiQianCount)
                        {
                            shuiqian += ";" + entity.huiQian[i].UserName + " " + entity.huiQian[i].Date.ToString(DateFormat);
                        }
                    }
                    if (shuiqian.Length > 0)
                    {
                        shuiqian = shuiqian.Substring(1);
                    }
                }
                this.txtHuiQianRenDates.Text = shuiqian;
            }
            else
            {            //兼容旧版
                if (entity.huiqianDates != "")
                {
                    this.txtHuiQianRenDates.Text = entity.huiqianDates;
                }
            }
        }
        /// <summary>
        /// 实体填充控件
        /// </summary>
        protected override void EntityToControl()
        {
            EntityLetterSend entity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();
            this.wfWorkItemID.Text = entity.WorkItemID;
            this.wfProcessID.Text = entity.ProcessID;
            this.txtccCompany.Text = entity.ccCompany;
            this.txtCompany.Text = entity.company;
            this.txtCompany.ToolTip = entity.companyID;
            //this.txtContent.Text = SysString.HtmlToTextCode(entity.content);
            this.txtContent.Text = entity.content;
            this.txtHeGaoRenDate.Text = entity.heGaoRenDate;
            this.txtNiGaoRenDate.Text = entity.niGaoRenDate;
            EntityToHuiQian(entity);
            this.txtOurRef.Text = entity.ourRef;
            this.txtPages.Text = entity.pages;
            this.txtSignDate.Text = entity.signDate;
            this.txtSubject.Text = entity.subject;
            this.txtTitle.Text = entity.title;
            this.txtTo.Text = entity.to;
            this.txtYourRef.Text = entity.yourRef;
            this.chkJinJi.Checked = entity.jinJi;
            this.chkHuiZhi.Checked = entity.huiZhi;
            this.txtccDept.Text = entity.ccDept;
            this.txtccDeptIDs.Text = entity.ccDeptIDs;
            this.txtccLeader.Text = entity.ccLeader;
            if (entity.isSave)
                this.txtComment.Text = entity.remarks1;

            //单位 抄送单位
            this.txtCompanyID.Text = entity.companyID;

            //意见 Repeater
            this.Repeater1.DataSource = entity.yiJian;
            this.Repeater1.DataBind();

            this.txtComment.Text = entity.syiJian;
            //附件数据绑定
            this.ucAttachment.UCDataList = entity.FileList;

            //流程数据              
            this.wfFaQiRen.Text = entity.Drafter;
            this.wfFaQiRenID.Text = entity.wfFaQiRenID;
            this.wfHeGaoRenID.Text = entity.wfHeGaoRenID;
            this.wfHuiQianRenIDs.Text = entity.wfHuiQianRenIDs;
            this.wfQianFaRenID.Text = entity.wfQianFaRenID;
            this.txt_UserDate.Text = entity.UserDate;
            this.txtDeptLeaderIDs.Text = entity.deptLeaderIDs;
            this.txtccLeaderIDs.Text = entity.ccLeaderIDs;
            this.wfChuanYueRenIDs.Text = entity.wfChuanYueRenIDs;

            this.txtEquipmentCode.Text = entity.equipmentCode1;
            this.txtContractNo.Text = entity.contractNo1;

            //下拉控件
            if (!Page.IsPostBack)
            {
                OAList.BindHJLX(drpHanJian);
            }
            FormsMethod.SelectedDropDownList(drpHanJian, entity.hanJianID1, entity.hanJian1);
            FormsMethod.SelectedDropDownList(drpSendDept, entity.sendDeptID1, entity.sendDept1);

            txtHeGaoRen.Text = entity.heGaoRen;
            txtHuiQianRen.Text = entity.huiQianRen;
            txtQianFaRen.Text = entity.qianFaRen;
            if (IsPreview == false)
            {
                switch (base.StepName)
                {
                    case ProcessConstString.StepName.LetterSend.发起函件:
                        break;
                    case ProcessConstString.StepName.LetterSend.核稿:
                        break;
                    case ProcessConstString.StepName.LetterSend.会签:
                        break;
                    case ProcessConstString.StepName.LetterSend.签发:
                        break;
                    case ProcessConstString.StepName.LetterSend.函件分发:
                        break;
                    case ProcessConstString.StepName.LetterSend.二次分发:
                        break;
                    default: break;
                }
            }
            else
            {
            }

            //领导签发示后显示label形式的领导与时间
            if (entity.signDate != "" && entity.signDate != null)
            {
                this.txtQianFaRen.Visible = false;
                this.lbQianFaRen.Visible = true;
                this.lbQianFaRen.Text = entity.qianFaRen;// +strNewLine + entity.signDate;

                this.txtSignDate.Visible = false;
                this.lbSignDate.Visible = true;
                this.lbSignDate.Text = entity.signDate;
            }

            //核稿后显示label形式的核稿人与时间
            if (entity.heGaoRenDate != "" && entity.heGaoRenDate != null)
            {
                //this.txtHeGaoRen.Visible = false;
                //this.lbHeGaoRen.Visible = true;
                this.lbHeGaoRen.Text = entity.heGaoRen;// +strNewLine + entity.heGaoRenDate;

                this.txtHeGaoRenDate.Visible = false;
                this.lbHeGaoRenDate.Visible = true;
                this.lbHeGaoRenDate.Text = entity.heGaoRenDate;
            }

            //拟稿后显示label形式的拟稿人与时间
            if (entity.niGaoRenDate != "" && entity.niGaoRenDate != null)
            {
                this.txtNiGaoRenDate.Visible = false;
                this.lbNiGaoRen.Visible = true;
                this.lbNiGaoRen.Text = entity.niGaoRenDate;
            }

        }
        /// <summary>
        /// 控件填充实体
        /// </summary>
        /// <param name="IsSave">是否保存</param>
        /// <returns>EntityBase</returns>
        protected override EntityBase ControlToEntity(bool IsSave)
        {
            EntityLetterSend entity = null;
            entity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();


            entity.DocumentTitle = txtSubject.Text;
            entity.ccCompany = txtccCompany.Text;
            entity.company = txtCompany.Text;
            entity.company1 = txtCompany.Text;
            //entity.content = SysString.TextToHtmlCode(txtContent.Text);
            entity.content = txtContent.Text;

            entity.pages = txtPages.Text;

            entity.subject = txtSubject.Text;
            entity.title = txtTitle.Text;
            entity.to = txtTo.Text;
            entity.yourRef = txtYourRef.Text;
            entity.jinJi = chkJinJi.Checked;
            entity.UrgentDegree = chkJinJi.Checked ? ConstString.CommonStr.Urgent : ConstString.CommonStr.Normal;
            entity.huiZhi = chkHuiZhi.Checked;
            entity.ccDept = txtccDept.Text;
            entity.ccDeptIDs = this.txtccDeptIDs.Text;
            entity.ccLeader = txtccLeader.Text;
            entity.companyID = txtCompanyID.Text;

            //流程数据和隐藏数据 
            entity.deptLeaderIDs = txtDeptLeaderIDs.Text;

            entity.ccLeaderIDs = txtccLeaderIDs.Text;
            entity.wfChuanYueRenIDs = wfChuanYueRenIDs.Text;

            if (wfChuanYueRenIDs.Text != "")
            {
                string[] strCYarr = wfChuanYueRenIDs.Text.Split(';');

                for (int i = 0; i < strCYarr.Length; i++)
                {
                    ChuanYues cy = new ChuanYues();
                    bool isHas = false;
                    for (int j = 0; j < entity.chuanyues.Count; j++)
                    {
                        if (entity.chuanyues[j].UserID.ToLower() == strCYarr[i].ToLower())
                        {
                            isHas = true;
                        }
                    }
                    if (!isHas)
                    {
                        cy.UserID = strCYarr[i];
                        cy.UserName = OAUser.GetUserName(strCYarr[i]);
                        cy.Date = DateTime.Now.ToShortDateString();
                        entity.chuanyues.Add(cy);
                    }
                }
            }
            //附件数据绑定
            entity.FileList = this.ucAttachment.UCDataList;

            //函件类型
            entity.hanJian1 = drpHanJian.SelectedItem == null ? "" : drpHanJian.SelectedItem.Text;
            entity.hanJianID1 = drpHanJian.SelectedValue == null ? "" : drpHanJian.SelectedValue;

            //add
            entity.equipmentCode1 = this.txtEquipmentCode.Text;
            entity.contractNo1 = this.txtContractNo.Text;

            //意见
            if (IsSave == false)
            {
                entity.isSave = false;
                CYiJian so = new CYiJian();
                so.UserID = entity.ReceiveUserID == string.Empty ? CurrentUserInfo.UserName : entity.ReceiveUserID;
                so.UserName = entity.ReceiveUserName == string.Empty ? CurrentUserInfo.DisplayName : entity.ReceiveUserName;
                so.ViewName = base.StepName;
                so.FinishTime = DateTime.Now.ToString();
                so.Content = "(" + base.SubAction + ")" + txtComment.Text;
                entity.yiJian.Add(so);
            }
            else
            {
                entity.isSave = true;
                entity.syiJian = txtComment.Text;
            }

            switch (base.StepName)
            {
                case ProcessConstString.StepName.LetterSend.发起函件:

                    //核稿
                    entity.heGaoRen = txtHeGaoRen.Text;
                    entity.wfHeGaoRenID = wfHeGaoRenID.Text;
                    //会签
                    entity.huiQianRen = txtHuiQianRen.Text;
                    entity.wfHuiQianRenIDs = wfHuiQianRenIDs.Text;
                    //签发
                    entity.qianFaRen = txtQianFaRen.Text;
                    entity.wfQianFaRenID = wfQianFaRenID.Text;

                    if (txtNiGaoRenDate.Text.Trim() == "") //不是第一次
                    {
                        //发起人
                        if (IsSave == false)
                        {
                            entity.UserDate = DateTime.Now.ToString();
                            entity.niGaoRenDate = (entity.niGaoRenDate == string.Empty ? CurrentUserInfo.DisplayName : entity.ReceiveUserName) + " " + entity.UserDate;
                            entity.DraftDate = SysConvert.ToDateTime(entity.UserDate);
                        }
                        if (entity.Drafter == string.Empty)
                        {
                            entity.Drafter = CurrentUserInfo.DisplayName;
                            entity.DrafterID = CurrentUserInfo.UserName;
                            entity.wfFaQiRenID = CurrentUserInfo.UserName;
                        }
                    }
                    if (base.SubAction == "提交会签" || base.SubAction == "提交签发") //如果是被退回的 再次提交会签 不经过核稿 则清空核稿人和核稿日期
                    {
                        if (IsSave == false && base.WorkItemID != "" && base.IsFromDraft == false)
                        {
                            entity.heGaoRen = "";
                            entity.heGaoRenDate = "";
                            entity.wfHeGaoRenID = "";
                            entity.heGaoYiJian = "";
                        }
                        else
                        {
                            entity.heGaoRen = txtHeGaoRen.Text;
                            entity.wfHeGaoRenID = wfHeGaoRenID.Text;
                        }
                    }
                    if (base.SubAction == "提交签发") //如果是被退回的 再次提交签发 不经过会签 则清空会签人和会签日期
                    {
                        if (IsSave == false && base.WorkItemID != "" && base.IsFromDraft == false)
                        {
                            entity.huiqianDates = "";
                            entity.huiQianRen = "";
                            entity.huiQian.Clear();
                        }
                    }

                    //发文部门
                    entity.sendDept1 = drpSendDept.SelectedItem == null ? "" : drpSendDept.SelectedItem.Text;
                    entity.sendDeptID1 = drpSendDept.SelectedValue;

                    //是否会签驳回
                    entity.isHuiQianBoHui = false;
                    if (base.SubAction == "提交会签")
                    {
                        entity.iHuiQianCount = entity.iHuiQianCount + 1;
                    }
                    break;
                case ProcessConstString.StepName.LetterSend.核稿:
                    //核稿
                    entity.heGaoRen = entity.ReceiveUserName;
                    if (IsSave == false)
                    {
                        entity.heGaoRenDate = DateTime.Now.ToString();
                    }
                    entity.heGaoYiJian = txtComment.Text;

                    //会签
                    entity.huiQianRen = txtHuiQianRen.Text;
                    entity.wfHuiQianRenIDs = wfHuiQianRenIDs.Text;

                    //签发
                    entity.qianFaRen = txtQianFaRen.Text;
                    entity.wfQianFaRenID = wfQianFaRenID.Text;
                    if (base.SubAction == "提交会签")
                    {
                        entity.iHuiQianCount = entity.iHuiQianCount + 1;
                    }
                    if (base.SubAction == "提交签发") //如果是被核稿退回的 再次提交签发 不经过会签 则清空会签人和会签日期
                    {
                        if (IsSave == false && base.WorkItemID != "" && base.IsFromDraft == false)
                        {
                            entity.huiqianDates = "";
                            entity.huiQianRen = "";
                            entity.huiQian.Clear();
                        }
                    }
                    break;
                case ProcessConstString.StepName.LetterSend.会签:
                    if (IsSave == false)
                    {
                        if (entity.huiqianDates == "")
                            entity.huiqianDates = entity.ReceiveUserName + " " + DateTime.Now.ToString(DateFormat);
                        else
                            entity.huiqianDates = entity.huiqianDates + ";" + entity.ReceiveUserName + " " + DateTime.Now.ToString(DateFormat);

                        HanJianHuiQian hjhq = new HanJianHuiQian();
                        hjhq.UserID = entity.ReceiveUserID;
                        hjhq.UserName = entity.ReceiveUserName;
                        hjhq.Date = DateTime.Now;
                        hjhq.YiJian = txtComment.Text;
                        hjhq.ICount = entity.iHuiQianCount;
                        entity.huiQian.Add(hjhq);
                    }
                    //签发
                    entity.qianFaRen = txtQianFaRen.Text;
                    entity.wfQianFaRenID = wfQianFaRenID.Text;

                    if (base.SubAction == "退回")
                    {
                        entity.isHuiQianBoHui = true;
                    }
                    break;
                case ProcessConstString.StepName.LetterSend.签发:
                    if (IsSave == false)
                    {
                        if (base.SubAction != "退回")
                            entity.signDate = DateTime.Now.ToString(DateFormat);
                    }
                    entity.HanJianAdminID = OAUser.GetUserByRoleName("函件管理员")[0];
                    entity.HanJianAdmin = OAUser.GetUserByRoleName("函件管理员")[1];
                    break;
                case ProcessConstString.StepName.LetterSend.函件分发:
                    entity.ourRef = txtOurRef.Text;
                    entity.DocumentNo = txtOurRef.Text;
                    break;
                case ProcessConstString.StepName.LetterSend.二次分发:
                    break;
                default: break;
            }

            return entity;
        }
        #endregion

        #region 提交按钮事件
        private bool CheckData(EntityLetterSend entity)
        {
            List<String> arrErr = new List<string>();

            if (entity.DocumentTitle == "")
            {
                arrErr.Add("标题不能为空!");
            }
            if (entity.DocumentTitle.Contains("#") || entity.DocumentTitle.Contains("'"))
            {
                arrErr.Add("含有特殊字符，请替换后再上传");                
            }          
            if (entity.company == "")
            {
                arrErr.Add("主送单位不能为空!");
            }
            if (entity.pages != "")
            {
                if (!Regex.IsMatch(entity.pages.Trim(), @"^[0-9]*[1-9][0-9]*$", RegexOptions.IgnoreCase))
                {
                    arrErr.Add("页数应为正整数!");
                }
            }

            if (entity.content == "")
            {
                arrErr.Add("正文不能为空!");
            }

            switch (base.StepName)
            {
                #region 发起函件
                case ProcessConstString.StepName.LetterSend.发起函件:
                    if (entity.sendDeptID1 == "")
                    {
                        arrErr.Add("发文部门不能为空!");
                    }

                    //根据提交动作做不同的判断
                    if (entity.SubmitAction == "提交核稿")
                    {
                        if (string.IsNullOrEmpty(entity.wfHeGaoRenID))
                        {
                            arrErr.Add("核稿人不能为空!");
                        }
                    }
                    if (entity.SubmitAction == "提交会签")
                    {
                        if (string.IsNullOrEmpty(wfHeGaoRenID.Text) == false)
                        {
                            arrErr.Add("请先清空核稿人后再提交会签!");
                        }
                        if (string.IsNullOrEmpty(entity.wfHuiQianRenIDs))
                        {
                            arrErr.Add("会签人不能为空!");
                        }
                    }
                    if (entity.SubmitAction == "提交签发")
                    {
                        if (string.IsNullOrEmpty(wfHeGaoRenID.Text) == false)
                        {
                            arrErr.Add("请先清空核稿人后再提交签发!");
                        }
                        if (string.IsNullOrEmpty(wfHuiQianRenIDs.Text) == false)
                        {
                            arrErr.Add("请先清空会签人后再提交签发!");
                        }
                        if (string.IsNullOrEmpty(entity.wfQianFaRenID))
                        {
                            arrErr.Add("签发人不能为空!");
                        }
                    }
                    if (string.IsNullOrEmpty(entity.wfFaQiRenID))
                    {
                        arrErr.Add("发起人不能为空!");
                    }
                    if (string.IsNullOrEmpty(entity.wfQianFaRenID))
                    {
                        arrErr.Add("签发人不能为空!");
                    }

                    break;
                #endregion

                #region 核稿
                case ProcessConstString.StepName.LetterSend.核稿:
                    if (entity.SubmitAction == "提交会签")
                    {
                        if (string.IsNullOrEmpty(entity.wfHuiQianRenIDs))
                        {
                            arrErr.Add("会签人不能为空!");
                        }
                        if (string.IsNullOrEmpty(entity.wfQianFaRenID))
                        {
                            arrErr.Add("签发人不能为空!");
                        }
                    }
                    if (entity.SubmitAction == "提交签发")
                    {
                        if (string.IsNullOrEmpty(entity.wfHuiQianRenIDs) == false)
                        {
                            arrErr.Add("请先清空会签人后再提交签发!");
                        }
                    }
                    break;
                #endregion

                #region 会签
                case ProcessConstString.StepName.LetterSend.会签:
                    if (entity.SubmitAction == "提交签发")
                    {
                        if (string.IsNullOrEmpty(entity.wfQianFaRenID))
                        {
                            arrErr.Add("签发人不能为空!");
                        }
                    }
                    break;
                #endregion

                #region 签发

                case ProcessConstString.StepName.LetterSend.签发:
                    if (entity.HanJianAdminID == "")
                    {
                        arrErr.Add("角色：函件管理员没有成员!");
                    }
                    break;
                #endregion

                #region 函件分发
                case ProcessConstString.StepName.LetterSend.函件分发:
                    if (entity.SubmitAction == "完成")
                    {
                        if (entity.pages == "")
                        {
                            arrErr.Add("页数不能为空!");
                        }
                        if (txtOurRef.Text.Trim() == "")
                        {
                            arrErr.Add("我方发文号不能为空!");
                        }
                        if (ucAttachment.UCDataList.Count == 0)
                        {
                            arrErr.Add("附件不能为空!");
                        }
                    }
                    if (entity.SubmitAction == "退回")
                    {

                    }
                    break;
                #endregion

                case ProcessConstString.StepName.LetterSend.二次分发:
                    entity.ourRef = txtOurRef.Text;
                    entity.DocumentNo = txtOurRef.Text;
                    break;
                default: break;
            }

            if (arrErr.Count > 0)
            {
                //JScript.ShowErrMsg(Page, arrErr);
                string strMessages = string.Empty;
                if (arrErr.Count > 0)
                {
                    for (int i = 0; i < arrErr.Count; i++)
                    {
                        if (i == 0)
                        {
                            strMessages = arrErr[i];
                        }
                        else
                        {
                            strMessages += "\\n" + arrErr[i];
                        }
                    }
                }
                JScript.Alert(strMessages, false);
                return false;
            }
            return true;
        }
        protected void SubmitEvents(object sender, EventArgs e)
        {
            try
            {
                if (IsPreview == false)
                {
                    string strActionName = ((Button)sender).Text.Trim();
                    base.SubAction = strActionName;

                    if (strActionName == ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT) //保存
                    {
                        EntityLetterSend entity = ControlToEntity(true) as EntityLetterSend;
                        entity.SubmitAction = strActionName;
                        base.FormSubmit(true, strActionName, null, entity as EntityBase);
                    }
                    else
                    {
                        EntityLetterSend entity = ControlToEntity(false) as EntityLetterSend;
                        entity.SubmitAction = strActionName;
                        if (CheckData(entity) == false)
                        {
                            return;
                        }

                        if (strActionName == ProcessConstString.SubmitAction.ACTION_CANCEL)
                        {
                            base.FormCancel(entity as EntityBase);
                        }
                        else
                        {
                            if (base.StepName == ProcessConstString.StepName.LetterSend.函件分发)
                            {
                                if (base.SubAction == "完成")
                                {
                                    //新增文号
                                    LetterNum lnum = new LetterNum();
                                    lnum.UpdateNo(txtOurRef.Text, txtCompanyID.Text, drpHanJian.SelectedValue);
                                    entity.ReceiveUserID = entity.ReceiveUserID;
                                }
                            }
                            Hashtable ht = EntityLetterSend.GetProcNameValue(base.StepName, strActionName, entity);//ap属性
                            base.FormSubmit(false, strActionName, ht, entity as EntityBase);

                            if (base.StepName == ProcessConstString.StepName.LetterSend.函件分发)
                            {
                                if (base.SubAction == "完成")
                                {
                                    base.Circulate(entity.ccDeptIDs, "1", "", GetCYNames(entity), "2", false, "", true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string strActionName = ((Button)sender).Text.Trim();
                    base.SubAction = strActionName;

                    EntityLetterSend entity = ControlToEntity(false) as EntityLetterSend;

                    if (strActionName == "追加分发")
                    {
                        base.Circulate(entity.ccDeptIDs, "1", "", GetCYNames(entity), "2", false, "", true);
                    }
                    else if (strActionName == "二次分发")
                    {
                        base.WorkItemID = Guid.NewGuid().ToString("N");
                        base.StepName = "二次分发";
                        base.SaveNewEntity(strActionName, entity);
                        base.Circulate(entity.ccDeptIDs, "1", "", GetCYNames(entity), "4", true, "", true);
                    }
                }
            }
            catch (Exception ex)
            {
                //JScript.ShowMsgBox(Page, MsgType.VbCritical, ex.Message);
                JScript.Alert(ex.Message, false);
                return;
            }
        }

        #endregion

        #region 内部测试
        //测试
        protected void btnStep_Click(object sender, EventArgs e)
        {
            base.StepName = txtStep.Text;
            SetControlStatus();
            //base.Circulate(txtccDeptIDs.Text, "3", "", txtccLeaderIDs.Text, "1", false, "",false);
        }
        protected void chkHistory_CheckedChanged(object sender, EventArgs e)
        {
            base.IsPreview = chkHistory.Checked;
            SetControlStatus();
        }

        #endregion

        #region 用户操作事件
        //生成文号
        protected void btnSetNo_Click(object sender, EventArgs e)
        {
            try
            {
                LetterNum numf = new LetterNum();

                txtOurRef.Text = numf.GetNo(txtCompanyID.Text, drpHanJian.SelectedValue);
            }
            catch (Exception ex)
            {
                //JScript.ShowMsgBox(Page, MsgType.VbInformation, ex.Message);
                JScript.Alert(ex.Message, false);
            }
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtOurRef.Text != "")
                {
                    string sNum = "";
                    //检查 因为单位编码CANH是4位 从第5位开始截取6个字符 为发文流水号
                    try
                    {
                        sNum = txtOurRef.Text.Substring(4, 6);
                    }
                    catch
                    {
                        //JScript.ShowMsgBox(Page, MsgType.VbInformation, "函件发文号必须为CANH+6位流水号+对方单位编码！");
                        JScript.Alert("函件发文号必须为CANH+6位流水号+对方单位编码！", false);
                        return;
                    }
                    LetterNum num = new LetterNum();

                    string sMessage = "";
                    bool b = num.CheckSameNo(sNum, txtCompanyID.Text, drpHanJian.SelectedValue, ref sMessage);
                    if (b)
                    {
                        //JScript.ShowMsgBox(Page, MsgType.VbInformation, "发文号重复！");
                        JScript.Alert("发文号重复！", false);
                    }
                    else
                    {
                        //JScript.ShowMsgBox(Page, MsgType.VbInformation, "此发文号可以使用！" + sMessage);
                        JScript.Alert("此发文号可以使用！" + sMessage, false);
                    }
                }
                else
                {
                    //JScript.ShowMsgBox(Page, MsgType.VbInformation, "请先生成发文号！");
                    JScript.Alert("请先生成发文号！", false);
                }
            }
            catch (Exception ex)
            {
                //JScript.ShowMsgBox(Page, MsgType.VbInformation, ex.Message);
                JScript.Alert(ex.Message, false);
            }
        }
        protected void drpSendDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtHeGaoRen.Text = "";
            wfHeGaoRenID.Text = "";
            //核稿人
            HeGaoRen();

            txtHeGaoRen.ToolTip = drpSendDept.SelectedItem.Text + " 的成员";
        }

        protected void drpHanJian_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSetNo_Click(null, null);
        }
        protected void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            btnSetNo_Click(null, null);
        }
        #endregion

        #region 临时方法
        private string GetCYNames(EntityLetterSend entity)
        {
            string chuanYueIDs = "";
            if (entity.ccLeaderIDs != "")
            {
                chuanYueIDs += entity.ccLeaderIDs + ";";
            }
            if (entity.deptLeaderIDs != "") //旧版才有
            {
                chuanYueIDs += entity.deptLeaderIDs + ";";
            }
            if (entity.DrafterID != "") // 发起人
            {
                chuanYueIDs += entity.DrafterID + ";";
            }
            if (entity.wfHeGaoRenID != "") //核稿人
            {
                chuanYueIDs += entity.wfHeGaoRenID + ";";
            }
            if (entity.wfHuiQianRenIDs != "") //会签人
            {
                chuanYueIDs += entity.wfHuiQianRenIDs + ";";
            }
            if (entity.wfQianFaRenID != "") //签发人
            {
                chuanYueIDs += entity.wfQianFaRenID + ";";
            }

            return chuanYueIDs;
        }
        #endregion

        protected void btnGD_Click(object sender, EventArgs e)
        {
            string strMessage = string.Empty;
            try
            {
                Devolve(out strMessage);
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