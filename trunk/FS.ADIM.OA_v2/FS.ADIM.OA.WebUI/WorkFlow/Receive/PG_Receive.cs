using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public class PG_Receive : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Receive/UC_Receive.ascx";
        private UC_Receive m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_Receive;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "";
            }
        }
    }
}
