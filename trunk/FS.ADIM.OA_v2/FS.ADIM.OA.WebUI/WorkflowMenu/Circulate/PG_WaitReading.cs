using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Circulate
{
    public class PG_WaitReading : PageEntityBase
    {
        private string m_virtualPath = "WorkflowMenu/Circulate/UC_WaitReading.ascx";
        private UC_WaitReading m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = base.PageLoadControl(this.m_virtualPath) as UC_WaitReading;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "待阅文件";
            }
        }
    }
}
