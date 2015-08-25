using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public class PG2_ProgramFile : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/ProgramFile/UC2_ProgramFile.ascx";
        private UC2_ProgramFile m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC2_ProgramFile;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "程序文件";
            }
        }
    }
}
