//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：程序文件(领导会签)
// 
// 创 建 者：黄琦、周理
// 创建时间： 
// 创建标识： 
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using FounderSoftware.Framework.UI.WebCtrls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC_LDHuiQian : System.Web.UI.UserControl
    {
        #region 变量定义

        //控件个数
        protected int Count
        {
            get
            {
                if (ViewState["Count"] == null)
                    ViewState["Count"] = 0;
                return (int)ViewState["Count"];
            }
            set
            {
                ViewState["Count"] = value;
            }
        }

        /// <summary>
        /// 部门会签列表 需要赋值
        /// </summary>
        public List<M_ProgramFile.LeaderSign> UCHQList
        {
            get
            {
                if (ViewState["HQList"] == null)
                    ViewState["HQList"] = new List<M_ProgramFile.LeaderSign>();
                return ViewState["HQList"] as List<M_ProgramFile.LeaderSign>;
            }
            set
            {
                ViewState["HQList"] = value;
            }
        }
        /// <summary>
        /// 是否第一次发起
        /// </summary>
        public Boolean UCIsFirst
        {
            get
            {
                if (ViewState["UCIsFirst"] == null)
                    ViewState["UCIsFirst"] = false;
                return (Boolean)ViewState["UCIsFirst"];
            }
            set
            {
                ViewState["UCIsFirst"] = value;
            }
        }

        /// <summary>
        /// 接收人员Id
        /// </summary>
        public string UCReceiveUserID
        {
            get
            {
                if (ViewState["UCReceiveUserID"] == null)
                    ViewState["UCReceiveUserID"] = "";
                return (string)ViewState["UCReceiveUserID"];
            }
            set
            {
                ViewState["UCReceiveUserID"] = value;
            }
        }
        /// <summary>
        /// 是否是历史记录
        /// </summary>
        public bool IsHistory
        {
            get
            {
                if (ViewState["IsHistory"] == null)
                    ViewState["IsHistory"] = false;
                return (bool)ViewState["IsHistory"];
            }
            set
            {
                ViewState["IsHistory"] = value;
            }
        }
        /// <summary>
        /// 是否不可用
        /// </summary>
        public Boolean UCIsDisEnable
        {
            get
            {
                if (ViewState["IsDisEnable"] == null)
                    ViewState["IsDisEnable"] = false;
                return (Boolean)ViewState["IsDisEnable"];
            }
            set
            {
                ViewState["IsDisEnable"] = value;
            }
        }
        #endregion
        private const string DateFormat = "yyyy-MM-dd";//"yyyy-MM-dd HH:mm:ss";

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                if (UCIsDisEnable)
                {
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
            }
            for (int i = 1; i <= Count; i++)
            {
                LoadUserControl(false, i);
            }
        }

        private void BindData()
        {
            if (UCHQList.Count > 0)
            {
                M_ProgramFile.LeaderSign info = new M_ProgramFile.LeaderSign();
                for (int i = 0; i < UCHQList.Count; i++)
                {
                    info = (M_ProgramFile.LeaderSign)UCHQList[i];
                    Count++;
                    LoadUserControl(false, Count);

                    //TBID
                    Label lblTBID = this.PlaceHolder1.Controls[i].FindControl("lblTBID") as Label;
                    lblTBID.Text = info.TBID;

                    //会签人ID
                    DropDownList drpUser = PlaceHolder1.Controls[i].FindControl("drpUser") as DropDownList;
                    Label lbUser = PlaceHolder1.Controls[i].FindControl("lbUser") as Label;

                    lbUser.Text = info.Name;

                    FormsMethod.SelectedDropDownList(drpUser, info.ID, info.Name);

                    //是否同意
                    Label lblTongYi = this.PlaceHolder1.Controls[i].FindControl("lblTongYi") as Label;
                    lblTongYi.Text = info.IsAgree;

                    if (!string.IsNullOrEmpty(info.IsAgree))
                    {
                        drpUser.Enabled = false;
                        drpUser.CssClass = "dropdownlist_blue";
                    }//已通过会签则不可重新选择

                    //会签日期
                    Label lblDate1 = this.PlaceHolder1.Controls[i].FindControl("lblDate1") as Label;
                    if (info.Date != DateTime.MinValue)
                    {
                        lblDate1.Text = info.Date.ToString(DateFormat);
                        lblDate1.ToolTip = info.Date.ToString();

                        lbUser.Text = info.Name;
                        lbUser.Visible = true;
                        drpUser.Visible = false;
                    }
                    else
                    {
                        lblDate1.Text = "";
                        lbUser.Visible = false;
                        drpUser.Visible = true;

                    }

                    //意见
                    Label lblYiJian = this.PlaceHolder1.Controls[i].FindControl("lblYiJian") as Label;
                    lblYiJian.ToolTip = info.Comment;
                    lblYiJian.Text = SysString.TruncationString(info.Comment, 20);

                    ////落实情况
                    //TextBox txtDealCondition = this.PlaceHolder1.Controls[i].FindControl("txtDealCondition") as TextBox;
                    //txtDealCondition.Text = info.DealCondition;

                    HiddenField hfLuoShi = this.PlaceHolder1.Controls[i].FindControl("hfLuoShi") as HiddenField;
                    hfLuoShi.Value = info.DealCondition;

                    //处理落实日期
                    Label lblDate2 = this.PlaceHolder1.Controls[i].FindControl("lblDate2") as Label;
                    if (info.DealDate != DateTime.MinValue)
                    {
                        lblDate2.Text = info.DealDate.ToShortDateString();
                    }
                    else
                    {
                        lblDate2.Text = "";
                    }

                    //绑定会签意见、落实情况//
                    Repeater rptCurrentList = this.PlaceHolder1.Controls[i].FindControl("rptCurrentList") as Repeater;//主键ID
                    Label lblComment = this.PlaceHolder1.Controls[i].FindControl("lblComment") as Label;
                    lblComment.Text = SysString.TruncationString(info.Comment, 20);
                    List<CYiJian> yiJianList = new List<CYiJian>();
                    foreach (B_PF.DetailInfo detailInfo in info.DetailInfoList)
                    {
                        CYiJian yiJian = new CYiJian();

                        yiJian.Content = detailInfo.Comment;
                        yiJian.DealCondition = detailInfo.DealCondition;
                        yiJian.FinishTime = info.Date.ToString();
                        yiJian.ID = info.TBID;
                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN;
                        yiJian.UserName = info.Name;
                        yiJian.UserID = info.ID;
                        yiJianList.Add(yiJian);
                    }
                    rptCurrentList.DataSource = yiJianList;
                    rptCurrentList.DataBind();
                    if (!this.IsHistory && this.UCReceiveUserID == info.ID)
                    {
                        lblDate1.Visible = false;
                        lblYiJian.Visible = false;
                        lblTongYi.Visible = false;
                        lblComment.Visible = false;
                    }
                }
            }
            HiddenField hfLeaderSignCount = this.Parent.Parent.FindControl("hfLeaderSignCount") as HiddenField;
            if (hfLeaderSignCount != null)
            { hfLeaderSignCount.Value = UCHQList.Count.ToString(); }
        }
        #endregion

        #region 动态增加减少
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <param name="isAdd"></param>
        /// <param name="index"></param>
        private void LoadUserControl(bool isAdd, int index)
        {
            Control ctl = this.LoadControl("UC_LDHuiQianOne.ascx");

            ctl.ID = string.Format("UC_LDHuiQianOne_{0}", index); //指定控件ID
            if (PlaceHolder1.Controls.Count < index)
            {
                this.PlaceHolder1.Controls.Add(ctl);
                (this.PlaceHolder1.FindControl(ctl.ID) as UC_LDHuiQianOne).BindUser();
                if (UCIsDisEnable)
                {
                    (this.PlaceHolder1.FindControl(ctl.ID) as UC_LDHuiQianOne).DisEnable();
                }
                if (isAdd)
                {
                    (this.PlaceHolder1.FindControl(ctl.ID) as UC_LDHuiQianOne).IsAdd();
                }
            }
        }

        /// <summary>
        /// 删除控件项
        /// </summary>
        /// <param name="index"></param>
        private void RemoveControl(int index)
        {
            if (this.PlaceHolder1.Controls.Count > 0)
            {
                this.PlaceHolder1.Controls.RemoveAt(index);
            }
        }

        /// <summary>
        /// 添加按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Count++;
            LoadUserControl(true, Count);
            CheckSignBtn();
        }

        /// <summary>
        /// 减少按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (Count == 0)
                return;

            int index = Count - 1;
            Label lblTongYi = PlaceHolder1.Controls[index].FindControl("lblTongYi") as Label;
            if (string.IsNullOrEmpty(lblTongYi.Text))
            {
                Count--;
                RemoveControl(Count);
                CheckSignBtn();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, "该领导存在会签记录，不可删除。");
                ms.Show();
            }
        }

        /// <summary>
        /// 校验会签按钮
        /// </summary>
        private void CheckSignBtn()
        {
            FSButton btnQualitySubmit = this.Parent.Parent.FindControl("btnQGApprove") as FSButton;
            FSButton btnDeptSign = this.Parent.Parent.FindControl("btnDeptSign2") as FSButton;
            FSButton btnLeaderSign = this.Parent.Parent.FindControl("btnLeaderSign2") as FSButton;
            HiddenField hfDeptSignCount = this.Parent.Parent.FindControl("hfDeptSignCount") as HiddenField;
            HiddenField hfLeaderSignCount = this.Parent.Parent.FindControl("hfLeaderSignCount") as HiddenField;
            if (btnLeaderSign != null)
            {
                if (PlaceHolder1.Controls.Count >= 1)
                { btnLeaderSign.Visible = true; }
                else if (hfDeptSignCount.Value != "0")
                { btnLeaderSign.Visible = false; btnDeptSign.Visible = true; }
                else
                { btnLeaderSign.Visible = false; btnQualitySubmit.Visible = true; }// btnApprove.Visible = true;
            }
            if (hfLeaderSignCount != null)
            { hfLeaderSignCount.Value = PlaceHolder1.Controls.Count.ToString(); }
        }
        #endregion

        /// <summary>
        /// 调用方需要调用 得到List
        /// </summary>
        /// <returns></returns>
        public List<M_ProgramFile.LeaderSign> UCGetHQList()
        {
            UCHQList = new List<M_ProgramFile.LeaderSign>();
            M_ProgramFile.LeaderSign info = new M_ProgramFile.LeaderSign();

            for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
            {
                info = new M_ProgramFile.LeaderSign();
                if ((this.PlaceHolder1.Controls[i].FindControl("drpUser") as DropDownList).SelectedValue != "")
                {
                    info.TBID = (this.PlaceHolder1.Controls[i].FindControl("lblTBID") as Label).Text;//主键ID
                    info.ID = (this.PlaceHolder1.Controls[i].FindControl("drpUser") as DropDownList).SelectedValue; //会签人帐号
                    info.Name = (this.PlaceHolder1.Controls[i].FindControl("drpUser") as DropDownList).SelectedItem.Text; //会签人

                    info.IsAgree = (this.PlaceHolder1.Controls[i].FindControl("lblTongYi") as Label).Text;  //是否同意
                    info.Date = SysConvert.ToDateTime((this.PlaceHolder1.Controls[i].FindControl("lblDate1") as Label).ToolTip); //会签日期

                    info.Comment = (this.PlaceHolder1.Controls[i].FindControl("lblYiJian") as Label).ToolTip; //意见
                    //info.DealCondition = (this.PlaceHolder1.Controls[i].FindControl("lblDealCondition") as Label).Text; //落实情况
                    info.DealDate = SysConvert.ToDateTime((this.PlaceHolder1.Controls[i].FindControl("lblDate2") as Label).Text); //落实日期

                    Repeater rptDetialInfo = this.PlaceHolder1.Controls[i].FindControl("rptCurrentList") as Repeater;//主键ID

                    List<B_PF.DetailInfo> detailInfoList = new List<M_ProgramFile.DetailInfo>();

                    foreach (RepeaterItem itm in rptDetialInfo.Items)
                    {
                        B_PF.DetailInfo detailInfo = new B_PF.DetailInfo();
                        Label lblDealCondition = itm.FindControl("lblDealCondition") as Label;
                        Label lblContent = itm.FindControl("lblContent") as Label;
                        detailInfo.Comment = lblContent.Text;
                        detailInfo.DealCondition = lblDealCondition.Text;
                        detailInfoList.Add(detailInfo);
                    }

                    info.DetailInfoList = detailInfoList;//意见落实情况

                    UCHQList.Add(info);
                }
            }
            return UCHQList;
        }
    }
}