//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：发文卡
// 
// 
// 创建标识：wangbinyi 20100118
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class UC_SendCard : OAUCBase
    {
        #region 变量定义
        /// <summary>
        /// TableID
        /// </summary>
        public String UCProcessID
        {
            get
            {
                if (ViewState[ConstString.ViewState.PROCESS_ID] == null)
                    return String.Empty;
                return ViewState[ConstString.ViewState.PROCESS_ID] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.PROCESS_ID] = value;
            }
        }
        /// <summary>
        /// 流程类型
        /// </summary>
        public String UCTemplateName
        {
            get
            {
                if (ViewState[ConstString.ViewState.TEMPLATE_NAME] == null)
                    return String.Empty;
                return ViewState[ConstString.ViewState.TEMPLATE_NAME] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.TEMPLATE_NAME] = value;
            }
        }
        /// <summary>
        /// WorkItemID
        /// </summary>
        public String UCWorkItemID
        {
            get
            {
                if (ViewState[ConstString.ViewState.WORKITEM_ID] == null)
                    return String.Empty;
                return ViewState[ConstString.ViewState.WORKITEM_ID] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.WORKITEM_ID] = value;
            }
        }


        /// <summary>
        /// 视图名
        /// </summary>
        public String UCStepName
        {
            get
            {
                if (ViewState[ConstString.ViewState.STEP_NAME] == null)
                    return String.Empty;
                return ViewState[ConstString.ViewState.STEP_NAME] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.STEP_NAME] = value;
            }
        }
        #endregion
    }
}