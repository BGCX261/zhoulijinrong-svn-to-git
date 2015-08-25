//----------------------------------------------------------------
// Copyright (C) 2013
//
// 文件功能描述：出差培训报销明细
// 
// 创 建 者：周理
// 创建时间： 
// 创建标识： 
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections;


using FounderSoftware.Framework.UI.WebPageFrame;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.WebUI.PageOU;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OA.BLL.Common.Utility;
using System.Web.UI;

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public partial class UC_CCBXDetailOne : OAUCBase
    {
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        /// <summary>
        /// 设置培训时的表单项
        /// </summary>
        public void SetPeiXunForm()
        {
            this.tdJiPiaoZheKou.Visible = true;
        }
        /// <summary>
        /// 设置出差时的表单项
        /// </summary>
        public void SetChuChaForm()
        {
            this.tdJiPiaoZheKou.Visible = false;
        }

        /// <summary>
        /// 设置控件不可用
        /// </summary>
        public void SetBuKeYong()
        {
            OAControl controls = new OAControl();
            this.txtStartMD.ReadOnly=true;
            this.txtStartMD.CssClass = "txtbox_blue";
            this.txtQiCheng.ReadOnly=true;
            this.txtQiCheng.CssClass = "txtbox_blue";
            this.txtEndMD.ReadOnly=true;
            this.txtEndMD.CssClass = "txtbox_blue";
            this.txtDaoDa.ReadOnly=true;
            this.txtDaoDa.CssClass = "txtbox_blue";
            this.txtJiPiaoZheKou.ReadOnly=true;
            this.txtJiPiaoZheKou.CssClass = "txtbox_blue";

            this.txtCheChuanPiao.ReadOnly=true;
            this.txtCheChuanPiao.CssClass = "txtbox_blue";

            this.txtShiNeiJiaoTong.ReadOnly=true;
            this.txtShiNeiJiaoTong.CssClass = "txtbox_blue";


        }
    }
}