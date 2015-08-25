using System.Web.UI;

using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain
{
    public class PG_ProgramTypeSub : PageEntityBase
    {
        private string m_virtualPath = "~/WorkflowMenu/ProgramFileMaintain/UC_ProgramTypeSub.ascx";
        private UC_ProgramTypeSub m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = this.CurrentPage.LoadControl(this.m_virtualPath) as UC_ProgramTypeSub;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "程序子类维护";
            }
        }
    }
}
