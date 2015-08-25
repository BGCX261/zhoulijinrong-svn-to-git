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

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail
{
    public class PG_ReDetail : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/Receive/ReDetail/UC_ReDetail.ascx";
        private UC_ReDetail m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = base.PageLoadControl(this.m_virtualPath) as UC_ReDetail;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "收文详细信息";
            }
        }
    }
}
