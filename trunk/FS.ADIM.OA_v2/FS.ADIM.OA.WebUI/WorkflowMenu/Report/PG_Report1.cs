using System;
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Web.UI;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Report
{
    public class PG_Report1 : PageEntityBase
    {
        private string m_virtualPath = "WorkflowMenu/Report/UC_Report1.ascx";
        private UC_Report1 m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = base.PageLoadControl(this.m_virtualPath) as UC_Report1;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "流程统计";
            }
        }
    }
}
