using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.RePrint
{
    public class PG_RePrint : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Receive/RePrint/UC_RePrint.ascx";
        private UC_RePrint m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = base.PageLoadControl(this.m_virtualPath) as UC_RePrint;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "收文登记 - 清单打印";
            }
        }
    }
}
