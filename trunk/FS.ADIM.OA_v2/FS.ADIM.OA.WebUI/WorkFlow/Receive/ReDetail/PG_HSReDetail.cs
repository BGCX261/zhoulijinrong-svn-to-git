using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail
{
    public class PG_HSReDetail : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Receive/ReDetail/UC_HSReDetail.ascx";
        private UC_HSReDetail m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_HSReDetail;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "函件收文 -- 详细信息";
            }
        }
    }
}
