using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public  class PG_LetterReceived : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Receive/UC_LetterReceived.ascx";
        private UC_LetterReceived m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_LetterReceived;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "函件收文 -- 函件收文发起";
            }
        }
    }
}
