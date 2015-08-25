using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Ascentn.Workflow.Base;
using Ascentn.Workflow.WebControls;
namespace FS.ADIM.OA.WebUI.AgilePoint
{
    public partial class ProcessImage : WFCommonPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
            if (this.IsPostBack) return;
            Bitmap image = (Bitmap)Session["ProcessImage"];
            if (image == null) return;
            image.Save(Response.OutputStream, ImageFormat.Jpeg);
            Session.Remove("ProcessImage");
        }
    }
}
