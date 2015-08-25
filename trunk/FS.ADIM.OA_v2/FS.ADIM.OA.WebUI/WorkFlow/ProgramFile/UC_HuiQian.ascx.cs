//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：程序文件(部门会签)
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
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Reflection;
using FounderSoftware.Framework.UI.WebCtrls;
using System.Web.UI.HtmlControls;
using System.Collections;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC_HuiQian : System.Web.UI.UserControl
    {
        #region 变量定义

        /// <summary>
        /// 控件个数
        /// </summary>
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
        public List<M_ProgramFile.DeptSign> UCHQList
        {
            get
            {
                if (ViewState["HQList"] == null)
                    ViewState["HQList"] = new List<M_ProgramFile.DeptSign>();
                return ViewState["HQList"] as List<M_ProgramFile.DeptSign>;
            }
            set
            {
                ViewState["HQList"] = value;
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

        /// <summary>
        /// 是否允许删除
        /// </summary>
        public Boolean UCIsAllowDel
        {
            get
            {
                if (ViewState["IsAllowDel"] == null)
                    ViewState["IsAllowDel"] = false;
                return (Boolean)ViewState["IsAllowDel"];
            }
            set
            {
                ViewState["IsAllowDel"] = value;
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
        /// 当前步骤
        /// </summary>
        public string UCStepName
        {
            get
            {
                if (ViewState["UCStepName"] == null)
                    ViewState["UCStepName"] = "";
                return (string)ViewState["UCStepName"];
            }
            set
            {
                ViewState["UCStepName"] = value;
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
        /// 流程作业ID
        /// </summary>
        public string UCProcessID
        {
            get
            {
                if (ViewState["UCProcessID"] == null)
                    ViewState["UCProcessID"] = "";
                return (string)ViewState["UCProcessID"];
            }
            set
            {
                ViewState["UCProcessID"] = value;
            }
        }

        /// <summary>
        /// tbID
        /// </summary>
        public string UCTBID
        {
            get
            {
                if (ViewState["UCTBID"] == null)
                    ViewState["UCTBID"] = "";
                return (string)ViewState["UCTBID"];
            }
            set
            {
                ViewState["UCTBID"] = value;
            }
        }

        /// <summary>
        /// 是否删除的会签部门不可见
        /// </summary>
        public Boolean UCIsDelInvisible
        {
            get
            {
                if (ViewState["IsDelInvisible"] == null)
                    ViewState["IsDelInvisible"] = false;
                return (Boolean)ViewState["IsDelInvisible"];
            }
            set
            {
                ViewState["IsDelInvisible"] = value;
            }
        }
        #endregion

        #region 页面加载
        private const string DateFormat = "yyyy-MM-dd";//"yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (UCIsDelInvisible)
                {
                    for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
                    {
                        CheckBox cb = this.PlaceHolder1.Controls[i].FindControl("cb") as CheckBox;
                        if (cb.Checked)
                        {
                            this.PlaceHolder1.Controls[i].Visible = false;
                        }
                    }
                }
            }
            for (int i = 1; i <= Count; i++)
            {
                LoadUserControl(i, false);
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            if (UCHQList.Count > 0)
            {
                M_ProgramFile.DeptSign info = new M_ProgramFile.DeptSign();
                for (int i = 0; i < UCHQList.Count; i++)
                {
                    info = (M_ProgramFile.DeptSign)UCHQList[i];
                    Count++;
                    LoadUserControl(Count, false);

                    //TBID
                    Label lblTBID = this.PlaceHolder1.Controls[i].FindControl("lblTBID") as Label;
                    lblTBID.Text = info.TBID;

                    //部门ID
                    DropDownList drpDept = PlaceHolder1.Controls[i].FindControl("drpDept") as DropDownList;
                    
                    Label lbDept = PlaceHolder1.Controls[i].FindControl("lbDept") as Label;
                    lbDept.Text = info.DeptName;

                    FormsMethod.SelectedDropDownList(drpDept, info.DeptID, info.DeptName);

                    //会签人ID
                    Label lblUserID = this.PlaceHolder1.Controls[i].FindControl("lblUserID") as Label;
                    lblUserID.Text = info.ID;
                    //会签人
                    Label lblUserName = this.PlaceHolder1.Controls[i].FindControl("lblUserName") as Label;
                    lblUserName.Text = info.Name;

                    if (!string.IsNullOrEmpty(info.IsAgree))
                    {
                        drpDept.Enabled = false;
                        drpDept.CssClass = "dropdownlist_blue";
                        CheckBox cb = this.PlaceHolder1.Controls[i].FindControl("cb") as CheckBox;
                        if (UCIsAllowDel && UCIsDisEnable == false)
                        {
                            cb.Enabled = true;//显示是否参加会签的checkbox
                            cb.Checked = info.IsExclude;//设置checkbox的状态
                            //cb.Enabled = !UCIsDisEnable;
                        }//部门会签控件允许删除并且可用
                        else
                        {
                            cb.Enabled = false;//隐藏是否参加会签的checkbox
                            cb.Checked = info.IsExclude;//设置checkbox的状态
                        }//不可用
                    }//已通过会签则不可重新选择


                    Label lblDate1 = this.PlaceHolder1.Controls[i].FindControl("lblDate1") as Label;
                    if (info.SubmitDate != DateTime.MinValue)
                    {
                        //会签日期
                        lblDate1.Text = info.SubmitDate.ToString(DateFormat);
                        lblDate1.ToolTip = info.SubmitDate.ToString();

                        lbDept.Text = info.DeptName;
                        lbDept.Visible = true;
                        drpDept.Visible = false;

                    }
                    else
                    {
                        lblDate1.Text = "";
                        lbDept.Visible = false;
                        drpDept.Visible = true;
                    }

                    //意见
                    Label lblYiJian = this.PlaceHolder1.Controls[i].FindControl("lblYiJian") as Label;
                    lblYiJian.ToolTip = info.Comment;
                    lblYiJian.Text = SysString.TruncationString(info.Comment, 20);

                    //是否同意
                    Label lblTongYi = this.PlaceHolder1.Controls[i].FindControl("lblTongYi") as Label;
                    lblTongYi.Text = info.IsAgree;

                    ////落实情况
                    //TextBox txtDealCondition = this.PlaceHolder1.Controls[i].FindControl("txtDealCondition2") as TextBox;
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
                    //绑定会签意见、落实情况///////////
                    Repeater rptCurrentList = this.PlaceHolder1.Controls[i].FindControl("rptCurrentList") as Repeater;//主键ID
                    Label lblComment = this.PlaceHolder1.Controls[i].FindControl("lblComment") as Label;
                    lblComment.Text = SysString.TruncationString(info.Comment, 20);

                    List<CYiJian> yiJianList = new List<CYiJian>();
                    foreach (B_PF.DetailInfo detailInfo in info.DetailInfoList)
                    {
                        CYiJian yiJian = new CYiJian();

                        yiJian.Content = detailInfo.Comment;
                        yiJian.DealCondition = detailInfo.DealCondition;
                        yiJian.FinishTime = info.SubmitDate.ToString();
                        yiJian.ID = info.TBID;
                        yiJian.ViewName = ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN;
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
            HiddenField hfDeptSignCount = this.Parent.Parent.FindControl("hfDeptSignCount") as HiddenField;
            if (hfDeptSignCount != null)
            { hfDeptSignCount.Value = UCHQList.Count.ToString(); }
        }
        #endregion

        #region 动态增加减少
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isAdd"></param>
        private void LoadUserControl(int index, bool isAdd)
        {
            Control ctl = this.LoadControl("UC_HuiQianOne.ascx");
            //给插入的用户控件中的控件赋值
            //HiddenField hfStepName = ctl.FindControl("hfStepName") as HiddenField;
            //HiddenField hfDealUserID = ctl.FindControl("hfDealUserID") as HiddenField;
            //HiddenField hfProcessID = ctl.FindControl("hfProcessID") as HiddenField;
            //HiddenField hftbID = ctl.FindControl("hftbID") as HiddenField;
            //hfStepName.Value = UCStepName;//当前步骤
            //hfDealUserID.Value = UCDealUserID;//当前处理人员ID
            //hfProcessID.Value = UCProcessID;//当前作业ID

            ctl.ID = string.Format("UC_HuiQianOne_{0}", index); //指定控件ID
            if (PlaceHolder1.Controls.Count < index)
            {
                this.PlaceHolder1.Controls.Add(ctl);
                (this.PlaceHolder1.FindControl(ctl.ID) as UC_HuiQianOne).BindDept();

                if (UCIsDisEnable)
                {
                    (this.PlaceHolder1.FindControl(ctl.ID) as UC_HuiQianOne).DisEnable();
                }
                if (UCIsFirst)
                {
                    (this.PlaceHolder1.FindControl(ctl.ID) as UC_HuiQianOne).IsFirst();
                }
                if (isAdd)
                {
                    (this.PlaceHolder1.FindControl(ctl.ID) as UC_HuiQianOne).IsAdd();
                }
            }
        }

        /// <summary>
        /// 减少控件项
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
            LoadUserControl(Count, true);
            CheckSignBtn();

            Control ctl = this.LoadControl("UC_HuiQianOne.ascx");
            ctl.ID = string.Format("UC_HuiQianOne_{0}", Count); //指定控件ID
            UC_HuiQianOne huiqianOne = this.PlaceHolder1.FindControl(ctl.ID) as UC_HuiQianOne;
            DropDownList ddl = huiqianOne.FindControl("drpDept") as DropDownList;

            foreach (string strDeptID in UCGetExcludeDeptID())
            {
                foreach (ListItem itm in ddl.Items)
                {
                    if (itm.Value == strDeptID)
                    {
                        ddl.Items.Remove(itm);
                        break;
                    }
                }
            }
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
                IMessage ms = new WebFormMessage(Page, "该部门存在会签记录，不可删除。");
                ms.Show();
            }
        }
        /// <summary>
        /// 校验会签按钮
        /// </summary>
        private void CheckSignBtn()
        {
            FSButton btnDeptSign = this.Parent.Parent.FindControl("btnDeptSign") as FSButton;
            FSButton btnLeaderSign = this.Parent.Parent.FindControl("btnLeaderSign") as FSButton;
            FSButton btnDeptSign2 = this.Parent.Parent.FindControl("btnDeptSign2") as FSButton;
            FSButton btnLeaderSign2 = this.Parent.Parent.FindControl("btnLeaderSign2") as FSButton;
            //FSButton btnApprove = this.Parent.Parent.FindControl("btnApprove") as FSButton;
            FSButton btnQualitySubmit = this.Parent.Parent.FindControl("btnQGApprove") as FSButton;

            HiddenField hfDeptSignCount = this.Parent.Parent.FindControl("hfDeptSignCount") as HiddenField;
            HiddenField hfLeaderSignCount = this.Parent.Parent.FindControl("hfLeaderSignCount") as HiddenField;
            if (btnDeptSign != null)
            {
                int iUseCount = 0;
                foreach (Control ctrl in this.PlaceHolder1.Controls)
                {
                    if (ctrl.Visible == true)
                    {
                        iUseCount++;
                    }
                }
                if (iUseCount > 0)
                {
                    btnDeptSign.Visible = true;
                    btnDeptSign2.Visible = true;
                }
                else if (hfLeaderSignCount.Value != "0")
                {
                    btnDeptSign.Visible = false;
                    btnLeaderSign.Visible = true;
                    btnDeptSign2.Visible = false;
                    btnLeaderSign2.Visible = true;
                }
                else
                {
                    btnDeptSign.Visible = false;
                    btnDeptSign2.Visible = false;
                    btnQualitySubmit.Visible = true;
                }//btnApprove.Visible = true;
            }
            if (hfDeptSignCount != null)
            { hfDeptSignCount.Value = PlaceHolder1.Controls.Count.ToString(); }
        }

        #endregion

        /// <summary>
        /// 设置控件状态为不可用(暂未使用)
        /// </summary>
        public void SetDisEnable()
        {
            for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
            {
                (this.PlaceHolder1.Controls[i].FindControl("drpDept") as WebControl).Enabled = false;
            }
        }

        /// <summary>
        /// 调用方需要调用 得到List
        /// </summary>
        /// <returns></returns>
        public List<M_ProgramFile.DeptSign> UCGetHQList()
        {
            UCHQList = new List<M_ProgramFile.DeptSign>();
            M_ProgramFile.DeptSign info = new M_ProgramFile.DeptSign();

            for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
            {
                info = new M_ProgramFile.DeptSign();
                if ((this.PlaceHolder1.Controls[i].FindControl("drpDept") as DropDownList).SelectedValue != "")
                {
                    info.TBID = (this.PlaceHolder1.Controls[i].FindControl("lblTBID") as Label).Text;//主键ID
                    info.DeptID = (this.PlaceHolder1.Controls[i].FindControl("drpDept") as DropDownList).SelectedValue; //部门ID
                    info.DeptName = (this.PlaceHolder1.Controls[i].FindControl("drpDept") as DropDownList).SelectedItem.Text;//部门名称
                    info.ID = (this.PlaceHolder1.Controls[i].FindControl("lblUserID") as Label).Text; //会签人帐号
                    info.Name = (this.PlaceHolder1.Controls[i].FindControl("lblUserName") as Label).Text;  //会签人姓名
                    info.IsAgree = (this.PlaceHolder1.Controls[i].FindControl("lblTongYi") as Label).Text;  //是否同意
                    info.SubmitDate = SysConvert.ToDateTime((this.PlaceHolder1.Controls[i].FindControl("lblDate1") as Label).ToolTip); //会签日期
                    info.Comment = (this.PlaceHolder1.Controls[i].FindControl("lblYiJian") as Label).ToolTip; //意见
                    info.DealDate = SysConvert.ToDateTime((this.PlaceHolder1.Controls[i].FindControl("lblDate2") as Label).Text); //落实日期
                    info.IsExclude = (this.PlaceHolder1.Controls[i].FindControl("cb") as FSCheckBox).Checked ? true : false;//是否进行会签

                    Repeater rptDetialInfo = this.PlaceHolder1.Controls[i].FindControl("rptCurrentList") as Repeater;//主键ID

                    List<M_ProgramFile.DetailInfo> detailInfoList = new List<M_ProgramFile.DetailInfo>();

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

        /// <summary>
        /// 获取需要部门会签人ID(提交部门会签)
        /// </summary>
        /// <returns></returns>
        public List<string> UCGetSignUser()
        {
            List<string> userList = new List<string>();
            for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
            {
                if ((this.PlaceHolder1.Controls[i].FindControl("drpDept") as DropDownList).SelectedValue != "")
                {
                    bool isExclude = (this.PlaceHolder1.Controls[i].FindControl("cb") as FSCheckBox).Checked ? true : false;//是否进行会签
                    if (isExclude == false)
                    {
                        string userID = (this.PlaceHolder1.Controls[i].FindControl("lblUserID") as Label).Text; //会签人帐号
                        userList.Add(userID);
                    }//不排除
                }
            }
            return userList;
        }

        /// <summary>
        /// 获取需要部门会签人ID(加载页面控制部门会签按钮显示)
        /// </summary>
        /// <returns></returns>
        public List<string> UCGetLoadSignUser()
        {
            List<string> userList = new List<string>();
            foreach (M_ProgramFile.DeptSign sign in UCHQList)
            {
                if (sign.IsExclude == false)
                {
                    userList.Add(sign.ID);
                }
            }
            return userList;
        }

        /// <summary>
        /// 获取删除（隐藏）部门会签人ID
        /// </summary>
        /// <returns></returns>
        public List<string> UCGetExcludeDeptID()
        {
            List<string> deptIDList = new List<string>();
            foreach (M_ProgramFile.DeptSign sign in UCHQList)
            {
                if (sign.IsExclude)
                {
                    deptIDList.Add(sign.DeptID);
                }
            }
            return deptIDList;
        }
    }
}