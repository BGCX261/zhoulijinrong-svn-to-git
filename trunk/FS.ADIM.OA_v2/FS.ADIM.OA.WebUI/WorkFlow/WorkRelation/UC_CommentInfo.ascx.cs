using System;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public partial class UC_CommentInfo : System.Web.UI.UserControl
    {
        #region 变量定义
     
        /// <summary>
        /// 流程实例ID
        /// </summary>
        /// 
        public String UCProcessID
        {
            get
            {
                if (ViewState["UCProcessID"] == null)
                {
                    if (Request.QueryString["ProcessID"] != null)
                    {
                        ViewState["UCProcessID"] = Request.Params["ProcessID"].ToString();
                    }
                    else
                    {
                        ViewState["UCProcessID"] = "";
                    }
                }
                return ViewState["UCProcessID"] as String;
            }
            set
            {
                ViewState["UCProcessID"] = value;
            }
        }

        /// <summary>
        /// 流程步骤实例ID
        /// </summary>
        /// 
        public String UCWorkItemID
        {
            get
            {
                if (ViewState["UCWorkItemID"] == null)
                {
                    if (Request.QueryString["WorkItemID"] != null)
                    {
                        ViewState["UCWorkItemID"] = Request.Params["WorkItemID"].ToString();
                    }
                    else
                    {
                        ViewState["UCWorkItemID"] = "";
                    }
                }
                return ViewState["UCWorkItemID"] as String;
            }
            set
            {
                ViewState["UCWorkItemID"] = value;
            }
        }

        #endregion
    }
}