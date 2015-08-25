using System;
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Web.UI;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Report
{
    public class PG_HSReport : PageEntityBase
    {
        private string m_virtualPath = "WorkflowMenu/Report/UC_HSReport.ascx";
        private UC_HSReport m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = base.PageLoadControl(this.m_virtualPath) as UC_HSReport;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "函件遗漏";
            }
        }
    }
}
