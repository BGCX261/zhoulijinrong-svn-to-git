using System.Web.UI;

using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain
{
    public class PG_ProgramFileInfo : PageEntityBase
    {
        private string m_virtualPath = "~/WorkflowMenu/ProgramFileMaintain/UC_ProgramFileInfo.ascx";
        private UC_ProgramFileInfo m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = this.CurrentPage.LoadControl(this.m_virtualPath) as UC_ProgramFileInfo;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "程序文件维护";
            }
        }
    }
}
