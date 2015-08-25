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

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
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
                //意见内容
                tdYiJian.Attributes.Add("onclick", String.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv')",
                Server.UrlEncode(ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN), Server.UrlEncode(lblUserID.Text), ddlSignDept.SelectedValue));
                tdYiJian.Attributes.Add("onmouseover", "this.style.cursor='hand'");
            }
        }

        /// <summary>
        /// 绑定部门
        /// </summary>
        public void BindDept()
        {
            if (ddlSignDept.Items.Count == 0)
            {
                OADept.GetDeptByIfloor(ddlSignDept, 1);
                ddlSignDept_SelectedIndexChanged(null, null);
            }
        }

        /// <summary>
        /// 下拉选择部门事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSignDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSignDept.SelectedIndex != 0 && ddlSignDept.SelectedIndex != -1)
            {
                lblUserName.Text = String.Empty;
                lblUserID.Text = String.Empty;

                String[] strManager = OAUser.GetDeptManagerArray(ddlSignDept.SelectedValue, 0);

                if (String.IsNullOrEmpty(strManager[0]))
                {
                    JScript.Alert("该处室不存在负责人，请分配。", true);
                    ddlSignDept.SelectedIndex = 0;
                    return;
                }
                if (strManager[0].IndexOf(";") != -1)
                {
                    JScript.Alert("该处室存在多个负责人，请联系系统管理员。", true);
                    ddlSignDept.SelectedIndex = 0;
                    return;
                }
                lblUserID.Text = strManager[0].ToString();
                lblUserName.Text = strManager[1].ToString();
            }
            else
            {
                lblUserName.Text = String.Empty;
                lblUserID.Text = String.Empty;
            }
        }

        /// <summary>
        /// 设置部门下拉控件不可用
        /// </summary>
        public void DisEnable()
        {
            this.ddlSignDept.CssClass = "dropdownlist_blue";
            this.ddlSignDept.Enabled = false;
            this.cb.Enabled = false;
        }

        ///// <summary>
        ///// 是否添加
        ///// </summary>
        //public void IsAdd()
        //{
        //    this.pnlInputAndShow.Visible = false;
        //}

         ///<summary>
         ///是否发起
         ///</summary>
        public void IsFirst()
        {
            cb.Visible = false;
        }
    }
}