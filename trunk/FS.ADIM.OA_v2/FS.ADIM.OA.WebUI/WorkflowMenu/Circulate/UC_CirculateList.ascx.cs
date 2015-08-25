using System;
using FS.ADIM.OA.WebUI.PageOU;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Circulate
{
    public partial class UC_CirculateList : OAUCBase
    {
        #region 变量定义
        /// <summary>
        /// 流程ProcessID
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
        /// 步骤名称
        /// </summary>
        public void SetButtonVisible()
        {
            JScript.ResponseScript(this.Page, "$('btnCirculateList').style.display='';$('linkCirculateList').style.display='none';", true);
        }
        #endregion
    }
}