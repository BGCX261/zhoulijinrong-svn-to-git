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

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC_HuiQianOne : OAUCBase
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
                lblUserName.ToolTip = "部门负责人";
                //意见内容
                tdYiJian.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv')",
                Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN), Server.UrlEncode(lblUserID.Text), drpDept.SelectedValue));
                tdYiJian.Attributes.Add("onmouseover", "this.style.cursor='hand'");
            }
        }

        /// <summary>
        /// 绑定部门
        /// </summary>
        public void BindDept()
        {
            if (drpDept.Items.Count == 0)
            {
                OADept.GetDeptByIfloor(drpDept, 1);
                drpDept_SelectedIndexChanged(null, null);
            }
        }

        /// <summary>
        /// 下拉选择部门事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void drpDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDept.SelectedIndex != 0 && drpDept.SelectedIndex != -1)
            {
                lblUserName.Text = string.Empty;
                lblUserID.Text = string.Empty;

                string[] strManager = OAUser.GetDeptManagerArray(drpDept.SelectedValue, 0);

                if (string.IsNullOrEmpty(strManager[0].ToString()))
                {
                    JScript.Alert("该处室不存在负责人，请分配。", true);
                    drpDept.SelectedIndex = 0;
                    return;
                }
                if (strManager[0].ToString().IndexOf(";") != -1)
                {
                    JScript.Alert("该处室存在多个负责人，请联系系统管理员。", true);
                    drpDept.SelectedIndex = 0;
                    return;
                }
                lblUserID.Text = strManager[0].ToString();
                lblUserName.Text = strManager[1].ToString();
            }
            else
            {
                lblUserName.Text = string.Empty;
                lblUserID.Text = string.Empty;
            }
        }

        /// <summary>
        /// 设置部门下拉控件不可用
        /// </summary>
        public void DisEnable()
        {
            this.drpDept.CssClass = "dropdownlist_blue";
            this.drpDept.Enabled = false;
            this.cb.Enabled = false;

            this.drpDept.Visible = false;
            this.lbDept.Visible = true;
            //this.lbDept.Text = this.drpDept.SelectedItem.Text;
        }

        /// <summary>
        /// 是否添加
        /// </summary>
        public void IsAdd()
        {
            this.pnlInputAndShow.Visible = false;
        }

        /// <summary>
        /// 是否发起
        /// </summary>
        public void IsFirst()
        {
            cb.Visible = false;
        }
    }
}