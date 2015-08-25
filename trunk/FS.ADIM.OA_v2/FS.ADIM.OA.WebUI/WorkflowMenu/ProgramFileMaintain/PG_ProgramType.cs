using System.Web.UI;

using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain
{
    public class PG_ProgramType : PageEntityBase
    {
        private string m_virtualPath = "~/WorkflowMenu/ProgramFileMaintain/UC_ProgramType.ascx";
        private UC_ProgramType m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = this.CurrentPage.LoadControl(this.m_virtualPath) as UC_ProgramType;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "程序类型维护";
            }
        }
    }
}
