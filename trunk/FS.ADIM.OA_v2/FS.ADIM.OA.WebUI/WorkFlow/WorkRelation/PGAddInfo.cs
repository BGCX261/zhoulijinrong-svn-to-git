using System;
using System.Collections.Generic;
using System.Web;
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Web.UI;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public class PGAddInfo : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/WorkRelation/UCAddInfo.ascx";
        private UCAddInfo m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UCAddInfo;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "添加落实情况";
            }
        }
    }
}
