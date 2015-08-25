//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：工作联系单(部门会签)
// 
// 创 建 者：王斌毅
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
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
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
        public List<M_WorkRelation.DeptSign> UCHQList
        {
            get
            {
                if (ViewState["HQList"] == null)
                    ViewState["HQList"] = new List<M_WorkRelation.DeptSign>();
                return ViewState["HQList"] as List<M_WorkRelation.DeptSign>;
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
        public String UCStepName
        {
            get
            {
                if (ViewState["UCStepName"] == null)
                    ViewState["UCStepName"] = "";
                return (String)ViewState["UCStepName"];
            }
            set
            {
                ViewState["UCStepName"] = value;
            }
        }

        /// <summary>
        /// 当前步骤处理人员
        /// </summary>
        public String UCDealUserID
        {
            get
            {
                if (ViewState["UCDealUserID"] == null)
                    ViewState["UCDealUserID"] = "";
                return (String)ViewState["UCDealUserID"];
            }
            set
            {
                ViewState["UCDealUserID"] = value;
            }
        }

        /// <summary>
        /// 流程作业ID
        /// </summary>
        public String UCProcessID
        {
            get
            {
                if (ViewState["UCProcessID"] == null)
                    ViewState["UCProcessID"] = "";
                return (String)ViewState["UCProcessID"];
            }
            set
            {
                ViewState["UCProcessID"] = value;
            }
        }

        /// <summary>
        /// tbID
        /// </summary>
        public String UCTBID
        {
            get
            {
                if (ViewState["UCTBID"] == null)
                    ViewState["UCTBID"] = "";
                return (String)ViewState["UCTBID"];
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
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadBindData();

                if (UCIsDisEnable)
                {
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }

                if (UCIsDelInvisible)
                {
                    for (int i = 0; i < pnlCountSignList.Controls.Count; i++)
                    {
                        CheckBox cb = this.pnlCountSignList.Controls[i].FindControl("cb") as CheckBox;
                        if (cb.Checked)
                        {
                            this.pnlCountSignList.Controls[i].Visible = false;
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
        private void LoadBindData()
        {
            if (UCHQList.Count > 0)
            {
                M_WorkRelation.DeptSign info = new M_WorkRelation.DeptSign();
                for (int i = 0; i < UCHQList.Count; i++)
                {
                    info = (M_WorkRelation.DeptSign)UCHQList[i];
                    Count++;
                    LoadUserControl(Count, false);

                    //TBID
                    Label lblTBID = this.pnlCountSignList.Controls[i].FindControl("lblTBID") as Label;
                    lblTBID.Text = info.TBID;

                    //部门ID
                    DropDownList ddlSignDept = pnlCountSignList.Controls[i].FindControl("ddlSignDept") as DropDownList;

                    FormsMethod.SelectedDropDownList(ddlSignDept, info.DeptID, info.DeptName);

                    //会签人ID
                    Label lblUserID = this.pnlCountSignList.Controls[i].FindControl("lblUserID") as Label;
                    lblUserID.Text = info.ID;

                    //会签人
                    Label lblUserName = this.pnlCountSignList.Controls[i].FindControl("lblUserName") as Label;
                    lblUserName.Text = info.Name;

                    //是否同意
                    Label lblTongYi = this.pnlCountSignList.Controls[i].FindControl("lblTongYi") as Label;
                    lblTongYi.Text = info.IsAgree;

                    if (!String.IsNullOrEmpty(info.IsAgree))
                    {
                        ddlSignDept.Enabled = false;
                        ddlSignDept.CssClass = "dropdownlist_blue";
                        CheckBox cb = this.pnlCountSignList.Controls[i].FindControl("cb") as CheckBox;
                        if (UCIsAllowDel && UCIsDisEnable == false)
                        {
                            cb.Enabled = true;
                            cb.Checked = info.IsExclude;
                        }
                        else
                        {
                            cb.Enabled = false;
                            cb.Checked = info.IsExclude;
                        }
                    }

                    //会签日期
                    Label lblDate = this.pnlCountSignList.Controls[i].FindControl("lblDate") as Label;
                    if (info.SubmitDate != DateTime.MinValue)
                    {
                        lblDate.Text = info.SubmitDate.ToString(ConstString.DateFormat.Normal);
                        lblDate.ToolTip = info.SubmitDate.ToString();
                    }
                    else
                    {
                        lblDate.Text = "";
                    }

                    //意见
                    Label lblYiJian = this.pnlCountSignList.Controls[i].FindControl("lblComment") as Label;
                    lblYiJian.ToolTip = info.Comment;
                    lblYiJian.Text = SysString.TruncationString(info.Comment, 20);

                    //Repeater rptCurrentList = this.pnlCountSignList.Controls[i].FindControl("rptCurrentList") as Repeater;//主键ID
                    Label lblComment = this.pnlCountSignList.Controls[i].FindControl("lblComment") as Label;
                    lblComment.Text = SysString.TruncationString(info.Comment, 20);

                    List<CYiJian> yiJianList = new List<CYiJian>();
                    foreach (B_WorkRelation.DetailInfo detailInfo in info.DetailInfoList)
                    {
                        CYiJian yiJian = new CYiJian();

                        yiJian.Content = detailInfo.Comment;
                        yiJian.DealCondition = detailInfo.DealCondition;
                        yiJian.FinishTime = info.SubmitDate.ToString();
                        yiJian.ID = info.TBID;
                        yiJian.ViewName = ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN;
                        yiJian.UserName = info.Name;
                        yiJian.UserID = info.ID;
                        yiJianList.Add(yiJian);
                    }
                    //rptCurrentList.DataSource = yiJianList;
                    //rptCurrentList.DataBind();
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
            ctl.ID = String.Format("UC_HuiQianOne_{0}", index);
            if (pnlCountSignList.Controls.Count < index)
            {
                this.pnlCountSignList.Controls.Add(ctl);
                (this.pnlCountSignList.FindControl(ctl.ID) as UC_HuiQianOne).BindDept();

                if (UCIsDisEnable)
                {
                    (this.pnlCountSignList.FindControl(ctl.ID) as UC_HuiQianOne).DisEnable();
                }
                //if (UCIsFirst)
                //{
                //    (this.pnlCountSignList.FindControl(ctl.ID) as UC_HuiQianOne).IsFirst();
                //}
                //if (isAdd)
                //{
                //    (this.pnlCountSignList.FindControl(ctl.ID) as UC_HuiQianOne).IsAdd();
                //}
            }
        }

        /// <summary>
        /// 减少控件项
        /// </summary>
        /// <param name="index"></param>
        private void RemoveControl(int index)
        {
            if (this.pnlCountSignList.Controls.Count > 0)
            {
                this.pnlCountSignList.Controls.RemoveAt(index);
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

            Control l_ucSign = this.LoadControl("UC_HuiQianOne.ascx");
            l_ucSign.ID = String.Format("UC_HuiQianOne_{0}", Count);
            UC_HuiQianOne huiqianOne = this.pnlCountSignList.FindControl(l_ucSign.ID) as UC_HuiQianOne;
            DropDownList l_ddlSignDept = huiqianOne.FindControl("ddlSignDept") as DropDownList;

            foreach (String l_strDeptID in UCGetExcludeDeptID())
            {
                foreach (ListItem itm in l_ddlSignDept.Items)
                {
                    if (itm.Value == l_strDeptID)
                    {
                        l_ddlSignDept.Items.Remove(itm);
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
            Int32 l_intCount = pnlCountSignList.Controls.Count;

            for (int i = l_intCount - 1; i >= 0; i--)
            {
                if (!(pnlCountSignList.Controls[i].FindControl("cb") as CheckBox).Checked)
                {
                    continue;
                }
                Label l_lblHandleResult = pnlCountSignList.Controls[i].FindControl("lblTongYi") as Label;
                if (String.IsNullOrEmpty(l_lblHandleResult.Text))
                {
                    RemoveControl(i);
                }
                else
                {
                    IMessage ms = new WebFormMessage(Page, "该部门存在会签记录，不可删除。");
                    ms.Show();
                }
            }
            Count = pnlCountSignList.Controls.Count;
        }

        #endregion
        ///// <summary>
        ///// 设置控件状态为不可用(暂未使用)
        ///// </summary>
        //public void SetDisEnable()
        //{
        //    for (int i = 0; i < pnlCountSignList.Controls.Count; i++)
        //    {
        //        (this.pnlCountSignList.Controls[i].FindControl("ddlSignDept") as WebControl).Enabled = false;
        //    }
        //}

        /// <summary>
        /// 调用方需要调用 得到List
        /// </summary>
        /// <returns></returns>
        public List<M_WorkRelation.DeptSign> UCGetHQList()
        {
            UCHQList = new List<M_WorkRelation.DeptSign>();
            M_WorkRelation.DeptSign info = new M_WorkRelation.DeptSign();

            for (int i = 0; i < pnlCountSignList.Controls.Count; i++)
            {
                info = new M_WorkRelation.DeptSign();
                if ((this.pnlCountSignList.Controls[i].FindControl("ddlSignDept") as DropDownList).SelectedValue != "")
                {
                    info.TBID = (this.pnlCountSignList.Controls[i].FindControl("lblTBID") as Label).Text;//主键ID
                    info.DeptID = (this.pnlCountSignList.Controls[i].FindControl("ddlSignDept") as DropDownList).SelectedValue; //部门ID
                    info.DeptName = (this.pnlCountSignList.Controls[i].FindControl("ddlSignDept") as DropDownList).SelectedItem.Text;//部门名称
                    info.ID = (this.pnlCountSignList.Controls[i].FindControl("lblUserID") as Label).Text; //会签人帐号
                    info.Name = (this.pnlCountSignList.Controls[i].FindControl("lblUserName") as Label).Text;  //会签人姓名
                    info.IsAgree = (this.pnlCountSignList.Controls[i].FindControl("lblTongYi") as Label).Text;  //是否同意
                    info.SubmitDate = SysConvert.ToDateTime((this.pnlCountSignList.Controls[i].FindControl("lblDate") as Label).ToolTip); //会签日期
                    info.Comment = (this.pnlCountSignList.Controls[i].FindControl("lblComment") as Label).ToolTip; //意见
                    //info.DealDate = SysConvert.ToDateTime((this.pnlCountSignList.Controls[i].FindControl("lblDate") as Label).Text); //落实日期
                    //info.IsExclude = (this.pnlCountSignList.Controls[i].FindControl("cb") as FSCheckBox).Checked ? true : false;//是否进行会签
                    info.IsExclude = false;

                    Repeater rptDetialInfo = this.pnlCountSignList.Controls[i].FindControl("rptCurrentList") as Repeater;//主键ID

                    //List<M_WorkRelation.DetailInfo> detailInfoList = new List<M_WorkRelation.DetailInfo>();

                    //foreach (RepeaterItem itm in rptDetialInfo.Items)
                    //{
                    //    B_WorkRelation.DetailInfo detailInfo = new B_WorkRelation.DetailInfo();
                    //    //Label lblDealCondition = itm.FindControl("lblDealCondition") as Label;
                    //    Label lblContent = itm.FindControl("lblContent") as Label;
                    //    detailInfo.Comment = lblContent.Text;
                    //    //detailInfo.DealCondition = lblDealCondition.Text;
                    //    detailInfoList.Add(detailInfo);
                    //}

                    //info.DetailInfoList = detailInfoList;//意见

                    UCHQList.Add(info);
                }
            }
            return UCHQList;
        }

        /// <summary>
        /// 获取需要部门会签人ID(提交部门会签)
        /// </summary>
        /// <returns></returns>
        public List<String> UCGetSignUser()
        {
            List<String> userList = new List<String>();
            for (int i = 0; i < pnlCountSignList.Controls.Count; i++)
            {
                if ((this.pnlCountSignList.Controls[i].FindControl("ddlSignDept") as DropDownList).SelectedValue != "")
                {
                    bool isExclude = (this.pnlCountSignList.Controls[i].FindControl("cb") as FSCheckBox).Checked ? true : false;//是否进行会签
                    if (isExclude == false)
                    {
                        String userID = (this.pnlCountSignList.Controls[i].FindControl("lblUserID") as Label).Text; //会签人帐号
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
        public List<String> UCGetLoadSignUser()
        {
            List<String> userList = new List<String>();
            foreach (M_WorkRelation.DeptSign sign in UCHQList)
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
        public List<String> UCGetExcludeDeptID()
        {
            List<String> deptIDList = new List<String>();
            foreach (M_WorkRelation.DeptSign sign in UCHQList)
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