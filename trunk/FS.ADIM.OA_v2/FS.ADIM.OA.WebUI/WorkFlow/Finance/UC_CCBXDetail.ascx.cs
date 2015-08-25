//----------------------------------------------------------------
// Copyright (C) 2013
//
// 文件功能描述：出差报销明细列表
// 
// 创 建 者：周理 
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

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public partial class UC_CCBXDetail : System.Web.UI.UserControl
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
        /// 出行明细列表 需要赋值
        /// </summary>
        public List<M_FinanceCCBX.ChuXingDetail> UCCXList
        {
            get
            {
                if (ViewState["CXList"] == null)
                    ViewState["CXList"] = new List<M_FinanceCCBX.ChuXingDetail>();
                return ViewState["CXList"] as List<M_FinanceCCBX.ChuXingDetail>;
            }
            set
            {
                ViewState["CXList"] = value;
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
        /// 单独设置财务可用项
        /// </summary>
        public Boolean UCSetCaiWu
        {
            get
            {
                if (ViewState["UCSetCaiWu"] == null)
                    ViewState["UCSetCaiWu"] = false;
                return (Boolean)ViewState["UCSetCaiWu"];
            }
            set
            {
                ViewState["UCSetCaiWu"] = value;
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
        /// 流程类型 出差还是报销
        /// </summary>
        public string UCProcessType
        {
            get
            {
                if (ViewState["UCProcessType"] == null)
                    ViewState["UCProcessType"] = "";
                return (string)ViewState["UCProcessType"];
            }
            set
            {
                ViewState["UCProcessType"] = value;
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
            if (!IsPostBack)
            {
                if (UCIsFirst)
                {
                    btnAdd_Click(null, null);
                }
                BindData();
                if (UCIsDisEnable)
                {
                    //if (UCProcessType == "出差")
                    //{

                    //}
                    //else if (UCProcessType == "培训")
                    //{
                    //    trCC1.Visible = false;
                    //    trCC2.Visible = false;
                    //    trPX1.Visible = true;
                    //    trPX2.Visible = true;
                    //}
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                if (UCSetCaiWu)
                {
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
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
            if (UCCXList.Count > 0)
            {
                M_FinanceCCBX.ChuXingDetail info = new M_FinanceCCBX.ChuXingDetail();
                for (int i = 0; i < UCCXList.Count; i++)
                {
                    info = (M_FinanceCCBX.ChuXingDetail)UCCXList[i];
                    Count++;
                    LoadUserControl(Count, false);

                    //TBID
                    Label lblTBID = this.PlaceHolder1.Controls[i].FindControl("lblTBID") as Label;
                    lblTBID.Text = info.TBID;

                    FSTextBox txtStartMD = this.PlaceHolder1.Controls[i].FindControl("txtStartMD") as FSTextBox;
                    txtStartMD.Text = info.StartMD.ToString("yyyy-MM-dd");

                    FSTextBox txtQiCheng = this.PlaceHolder1.Controls[i].FindControl("txtQiCheng") as FSTextBox;
                    txtQiCheng.Text = info.QiCheng;

                    FSTextBox txtEndMD = this.PlaceHolder1.Controls[i].FindControl("txtEndMD") as FSTextBox;
                    txtEndMD.Text = info.EndMD.ToString("yyyy-MM-dd"); ;

                    FSTextBox txtDaoDa = this.PlaceHolder1.Controls[i].FindControl("txtDaoDa") as FSTextBox;
                    txtDaoDa.Text = info.DaoDa;

                    FSTextBox txtJiPiaoZheKou = this.PlaceHolder1.Controls[i].FindControl("txtJiPiaoZheKou") as FSTextBox;
                    txtJiPiaoZheKou.Text = info.JiPiaoZheKou.ToString();

                    FSTextBox txtCheChuanPiao = this.PlaceHolder1.Controls[i].FindControl("txtCheChuanPiao") as FSTextBox;
                    txtCheChuanPiao.Text = info.CheChuanPiao.ToString();

                    FSTextBox txtShiNeiJiaoTong = this.PlaceHolder1.Controls[i].FindControl("txtShiNeiJiaoTong") as FSTextBox;
                    txtShiNeiJiaoTong.Text = info.ShiNeiJiaoTong.ToString();

                    if (!this.IsHistory)
                    {
                        //lblDate1.Visible = false;
                        //lblYiJian.Visible = false;
                        //lblTongYi.Visible = false;
                        //lblComment.Visible = false;
                    }
                }
            }
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
            Control ctl = this.LoadControl("UC_CCBXDetailOne.ascx");

            ctl.ID = string.Format("UC_CCBXDetailOne_{0}", index); //指定控件ID
            if (PlaceHolder1.Controls.Count < index)
            {
                this.PlaceHolder1.Controls.Add(ctl);
                //if (UCProcessType == "出差")
                //{
                //    ((UC_CCBXDetailOne)ctl).SetChuChaForm();
                    
                //}
                //else if (UCProcessType == "培训")
                //{
                //    ((UC_CCBXDetailOne)ctl).SetPeiXunForm();
                //}
                if(UCIsDisEnable)
                {
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                    ((UC_CCBXDetailOne)ctl).SetBuKeYong();
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

            Control ctl = this.LoadControl("UC_CCBXDetailOne.ascx");
            ctl.ID = string.Format("UC_CCBXDetailOne_{0}", Count); //指定控件ID
            UC_CCBXDetailOne huiqianOne = this.PlaceHolder1.FindControl(ctl.ID) as UC_CCBXDetailOne;
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
            Count--;
            RemoveControl(Count);
        }

        #endregion

        /// <summary>
        /// 设置培训表单
        /// </summary>
        public void SetPeiXunForm()
        {
            return;
            trCC1.Visible = false;
            trCC2.Visible = false;
            trPX1.Visible = true;
            trPX2.Visible = true;

            for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
            {
                (this.PlaceHolder1.Controls[i] as UC_CCBXDetailOne).SetPeiXunForm();
            }
        }

        /// <summary>
        /// 调用方需要调用 得到List
        /// </summary>
        /// <returns></returns>
        public List<M_FinanceCCBX.ChuXingDetail> UCGetCXList()
        {
            UCCXList = new List<M_FinanceCCBX.ChuXingDetail>();
            M_FinanceCCBX.ChuXingDetail info = new M_FinanceCCBX.ChuXingDetail();

            for (int i = 0; i < PlaceHolder1.Controls.Count; i++)
            {
                info = new M_FinanceCCBX.ChuXingDetail();
                if ((this.PlaceHolder1.Controls[i].FindControl("txtStartMD") as FSTextBox).Text != "")
                {
                    info.TBID = (this.PlaceHolder1.Controls[i].FindControl("lblTBID") as Label).Text;//主键ID
                    info.StartMD = SysConvert.ToDateTime((this.PlaceHolder1.Controls[i].FindControl("txtStartMD") as FSTextBox).Text);
                    info.QiCheng = (this.PlaceHolder1.Controls[i].FindControl("txtQiCheng") as FSTextBox).Text;
                    info.EndMD = SysConvert.ToDateTime((this.PlaceHolder1.Controls[i].FindControl("txtEndMD") as FSTextBox).Text);
                    info.DaoDa = (this.PlaceHolder1.Controls[i].FindControl("txtDaoDa") as FSTextBox).Text;
                    info.JiPiaoZheKou = SysConvert.ToDecimal((this.PlaceHolder1.Controls[i].FindControl("txtJiPiaoZheKou") as FSTextBox).Text);
                    info.CheChuanPiao = SysConvert.ToDecimal((this.PlaceHolder1.Controls[i].FindControl("txtCheChuanPiao") as FSTextBox).Text);
                    info.ShiNeiJiaoTong = SysConvert.ToDecimal((this.PlaceHolder1.Controls[i].FindControl("txtShiNeiJiaoTong") as FSTextBox).Text);

                    //计算天数
                    TimeSpan ts = info.EndMD - info.StartMD;
                    info.Day = SysConvert.ToInt32(ts.TotalDays);
                    if (SysConvert.ToInt32(ts.TotalDays) < 0)
                        info.Day = 0;
                    else
                        info.Day = SysConvert.ToInt32(ts.TotalDays) + 1;
                    UCCXList.Add(info);
                }
            }
            return UCCXList;
        }
    }
}