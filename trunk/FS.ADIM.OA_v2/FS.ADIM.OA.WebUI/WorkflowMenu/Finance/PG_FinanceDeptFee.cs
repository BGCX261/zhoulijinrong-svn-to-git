using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Finance
{
    public class PG_FinanceDeptFee : PageEntityBase
    {
        //加载用户控件路径
        private string m_virtualPath = "~/WorkflowMenu/Finance/UC_FinanceDeptFee.ascx";
        //加载对象
        private UC_FinanceDeptFee m_uc;
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_FinanceDeptFee;
            return m_uc;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "财务费用";
            }
        }
    }
}