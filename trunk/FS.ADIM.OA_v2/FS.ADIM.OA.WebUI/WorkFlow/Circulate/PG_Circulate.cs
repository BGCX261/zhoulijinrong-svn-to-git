using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Circulate
{
    public class PG_Circulate : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Circulate/UC_Circulate.ascx";
        private UC_Circulate m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_Circulate;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "传阅";
            }
        }
    }
}
