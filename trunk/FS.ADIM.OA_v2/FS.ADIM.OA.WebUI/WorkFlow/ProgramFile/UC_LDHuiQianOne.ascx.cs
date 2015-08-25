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
using System.Web.UI.WebControls;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OU.OutBLL;

using FS.ADIM.OA.WebUI.PageOU;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC_LDHuiQianOne : OAUCBase
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
                drpUser.ToolTip = "公司领导";
                //意见内容
                tdLeaderYiJian.Attributes.Add("onclick", string.Format(@"OpenSignCommentDetail('{0}','{1}','{2}');ShowPopDiv('popDiv')",
                                                                   Server.UrlEncode(ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN), Server.UrlEncode(drpUser.SelectedValue.ToString()), string.Empty));
                tdLeaderYiJian.Attributes.Add("onmouseover", "this.style.cursor='hand'");
            }
        }

        /// <summary>
        /// 绑定下拉选择领导
        /// </summary>
        public void BindUser()
        {
            if (drpUser.Items.Count == 0 )
                OAUser.GetUserByRole(drpUser, ConstString.RoleName.COMPANY_LEADER);
            
            ListItem temp = drpUser.Items.FindByText("孙云根");

            drpUser.Items.Remove(temp);


        }

        /// <summary>
        /// 是否添加
        /// </summary>
        public void IsAdd()
        {
            //this.drpLuoShi.CssClass = "dropdownlist_blue";
            //this.drpLuoShi.Enabled = false;
            this.pnlInputAndShow.Visible = false;
        }

        /// <summary>
        /// 是否不可用
        /// </summary>
        public void DisEnable()
        {
            this.drpUser.CssClass = "dropdownlist_blue";
            this.drpUser.Enabled = false;
            
            this.drpUser.Visible = false;
            this.lbUser.Visible = true; 

        }
    }
}