using System;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class UC_Comment : OAUCBase
    {
        #region 变量定义
        /// <summary>
        /// UCProcessID
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

        /// <summary>
        /// UCWorkItemID
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
        #endregion
    }
}