using System;
using System.Collections.Generic;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public partial class UCAddInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtComment.Text = Server.UrlDecode(Request.QueryString["content"]);
                hfucID.Value = Request.QueryString["ucID"];
            }
        }
    }
}