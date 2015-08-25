using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Web.UI;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.FormInfoAdd
{
    public class PG_FormInfoList : PageEntityBase
    {
        //加载用户控件路径
        private string m_virtualPath = "~/WorkflowMenu/FormInfoAdd/UC_FormInfoList.ascx";
        //加载对象
        private UC_FormInfoList m_uc;
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_FormInfoList;
            return m_uc;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "表单数据补偿";
            }
        }
    }
}
