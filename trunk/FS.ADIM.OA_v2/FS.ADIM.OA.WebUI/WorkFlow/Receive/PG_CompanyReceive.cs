using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public class PG_CompanyReceive : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Receive/UC_CompanyReceive.ascx";
        private UC_CompanyReceive m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_CompanyReceive;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "公司收文 -- 处理单";
            }
        }
    }
}
