using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using Ascentn.Workflow.Base;
using Ascentn.Workflow.WebControls;
using FS.OA.Framework.WorkFlow;

namespace FS.ADIM.OA.WebUI.AgilePoint
{
    public partial class ProcessViewer : WFCommonPage
    {
        public const int MAX_ERROR_LENGTH = 50;
        public const int MIN_MAGIN = 20;
        public const int MIN_WIDTH = 500;
        public const int MIN_HEIGHT = 200;
        public const int EDGE_WIDTH = 2;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ProcInstImage.ImageUrl = "Resource/blank.gif";
            ProcInstImage.Attributes.Add("ismap", null);
            ProcInstImage.Attributes.Add("usemap", "#ActivityInfo");

            String piID = Request.QueryString["PIID"];
            if (piID != null)
            {
                lblTitleName.Text = "Process - ";
                hiddenProcInstID.Value = piID;
                lblTitleName.Text += GetProcInstName(piID);
                UpdateImagePage(piID);
            }

        }

        private String GetProcInstName(String piID)
        {
            String name = "";
            IWFWorkflowService api = WFFactory.GetWF(WFType.AgilePoint).GetAPI();
            WFBaseProcessInstance pi = api.GetProcInst(piID);
            if (pi != null)
            {
                name = string.Format("{0},{1}", pi.ProcInstName, pi.Status);
            }
            return name;
        }

        private void UpdateImagePage(String piID)
        {
            IWFWorkflowService api = WFFactory.GetWF(WFType.AgilePoint).GetAPI();

            string xml = api.GetProcDefGraphics(piID);
            if (xml == null || xml.Length == 0)
                return;

            GraphicImage g = new GraphicImage();
            g.FromXml(xml);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(g.Image);
            Bitmap bitmap = new Bitmap(ms);

            Size size = Calculate(bitmap.Width, bitmap.Height);
            Size bitmapSize = ZoomView(bitmap.Width, bitmap.Height);

            int maginX = (size.Width - bitmapSize.Width) / 2;
            int maginY = (size.Height - bitmapSize.Height) / 2;

            Bitmap canvas = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            Graphics gdi = Graphics.FromImage(canvas);
            gdi.FillRectangle(new SolidBrush(Color.White), 0, 0, canvas.Width, canvas.Height);

            gdi.DrawImage(bitmap, maginX, maginY, bitmapSize.Width, bitmapSize.Height);

            ArrayList shapes = g.Normalize(bitmapSize.Width, bitmapSize.Height, maginX, maginY);

            WFBaseActivityInstance[] ais = api.GetActivityInstsByPIID(piID);
            gdi.Flush();
            Session["ProcessImage"] = canvas;
            ProcInstImage.ImageUrl = "ProcessImage.aspx";

            string map = "";
            foreach (NamedRectangle shp in shapes)
            {
                AddImageContrl(shp, WFBaseActivityInstance.ACTIVE, true);
                AddImageContrl(shp, WFBaseActivityInstance.PENDING, true);
                AddImageContrl(shp, WFBaseActivityInstance.PASSED, true);
                AddImageContrl(shp, WFBaseActivityInstance.CANCELLED, true);
                AddImageContrl(shp, WFBaseActivityInstance.ACTIVATED, true);
                if (ais != null)
                {
                    foreach (WFBaseActivityInstance ai in ais)
                    {
                        if (shp.Name.Equals(ai.Name))
                        {
                            int participantCount = CountParticipantOnActivity(api, piID, ai);

                            if (participantCount > 1)
                            {
                                DrawParticipantImage(gdi, shp, "MultiParticipant", participantCount);
                            }
                            else if (participantCount == 1)
                            {
                                DrawParticipantImage(gdi, shp, "SingleParticipant", participantCount);
                            }

                        }
                    }
                }
                map += AddAreaControl(shp);
            }
            ActivityInfoMap.Text = map;

            // add hidden text to store activity (name, id) map
            foreach (WFBaseActivityInstance ai in ais)
            {
                HtmlInputHidden ctrl = new HtmlInputHidden();
                ctrl.ID = ai.Name;
                ctrl.Value = ai.ID;
                PlaceHolder.Controls.Add(ctrl);
            }
        }
        private int CountParticipantOnActivity(IWFWorkflowService api, string piID, WFBaseActivityInstance ai)
        {
            ArrayList participantList = new ArrayList();

            //Retreive Activity definition to know the type of activity
            WFBaseProcessInstance pi = api.GetProcInst(piID);
            string xmlString = api.GetProcDefXml(pi.DefID);
            WFProcessDefinition processDef = new WFProcessDefinition();
            ProcDefXmlParser xmlParser = new ProcDefXmlParser(processDef);
            xmlParser.Parse(xmlString);
            IWFActivityDefinition ad = processDef.FindActivityByName(ai.Name);
            WFManualActivityDefinition activityDef = null;
            //Get the type of activity
            if (ad != null && ad.GetType() == typeof(WFManualActivityDefinition))
            {
                activityDef = (WFManualActivityDefinition)ad;
            }

            // Get manual work items for the given Activity
            WFAny any = WFAny.Create(ai.ID);
            WFQueryExpr expr = new WFQueryExpr("ACTIVITY_INST_ID", SQLExpr.EQ, any, true);

            WFManualWorkItem[] wks = api.QueryWorkList(expr);

            if (wks != null)
            {
                foreach (WFManualWorkItem wk in wks)
                {
                    if (wk.Status == WFManualWorkItem.ASSIGNED || wk.Status == WFManualWorkItem.OVERDUE || wk.Status == WFManualWorkItem.COMPLETED)
                    {
                        if (!String.IsNullOrEmpty(wk.UserID) && !participantList.Contains(wk.UserID.ToLower()))
                        {
                            participantList.Add(wk.UserID.ToLower());
                        }
                    }
                }
            }

            // if activity is Agilework of type ProcessAdaptation
            if (activityDef != null && activityDef.CustomProperties.Contains("Ascentn.AgileWork.Premier.ProcessAdaptation"))
            {
                //Get type of the AgileWork
                string activityType = api.GetCustomAttr(pi.WorkObjectID, ai.ID + "_ApprovalType") as string;

                //Count number of participant in case of sequential type
                if (activityType == "Sequential")
                {
                    string activityProperties = api.GetCustomAttr(pi.WorkObjectID, ai.ID + "_ActivityProperties") as string;
                    if (!String.IsNullOrEmpty(activityProperties))
                    {
                        string[] approverInfoList = activityProperties.Split(';');
                        foreach (string approverInfo in approverInfoList)
                        {
                            string[] userInfoList = approverInfo.Split('|');
                            string user = userInfoList[0];
                            if (!String.IsNullOrEmpty(user) && !participantList.Contains(user.ToLower()))
                            {
                                participantList.Add(user.ToLower());
                            }
                        }
                    }
                }
            }
            return participantList.Count;
        }

        private string AddAreaControl(NamedRectangle shp)
        {
            string sCoords = string.Format("{0},{1},{2},{3}", (int)shp.Left, (int)shp.Top, (int)shp.Right, (int)shp.Bottom);
            string sOnclick = string.Format("showActivityInfo('{0}',{1},{2})", shp.Name, (int)((shp.Left + shp.Right) * 0.5), (int)((shp.Top + shp.Bottom) * 0.5));
            string sOnMouseOver = string.Format("highlightActivity({0},{1},{2},{3},'{4}')", (int)shp.Left - EDGE_WIDTH, (int)shp.Top - EDGE_WIDTH, (int)(shp.Right - shp.Left), (int)(shp.Bottom - shp.Top), shp.Name);
            return string.Format(
                "<area coords=\"{0}\" href=\"#\" onmouseover=\"{1}\">\n", sCoords, sOnMouseOver);
        }

        private void AddImageContrl(NamedRectangle shp, String type, bool hide)
        {
            HtmlImage image = new HtmlImage();
            image.Src = "resource/en-us/" + type + ".gif";
            image.ID = shp.Name + "_" + type;
            image.Style["Z-INDEX"] = "200";
            image.Style["LEFT"] = String.Format("{0}px", (int)shp.Left - 12);
            image.Style["TOP"] = String.Format("{0}px", (int)shp.Top - 12);
            image.Style["POSITION"] = "absolute";
            image.Style["DISPLAY"] = hide ? "none" : "";
            PlaceHolder.Controls.Add(image);
        }

        private Size Calculate(int w, int h)
        {
            Size size = ZoomView(w, h);

            size.Width = (size.Width + 2 * MIN_MAGIN < MIN_WIDTH) ? MIN_WIDTH : size.Width + 2 * MIN_MAGIN;
            size.Height = (size.Height + 2 * MIN_MAGIN < MIN_HEIGHT) ? MIN_HEIGHT : size.Height + 2 * MIN_MAGIN;

            return size;
        }

        private Size ZoomView(int w, int h)
        {
            Size size = new Size();
            int zoomRate = Convert.ToUInt16(ddlZoomRate.SelectedValue);

            size.Width = w * zoomRate / 100;
            size.Height = h * zoomRate / 100;

            return size;
        }
        private void DrawParticipantImage(Graphics gdi, NamedRectangle shape, string imageFile, int participantNo)
        {
            int x = Convert.ToInt32(shape.Right);
            int y = Convert.ToInt32(shape.Top);

            string fileName = Server.MapPath(@"~/AgilePoint/resource/en-us/" + imageFile + ".gif");
            if (!System.IO.File.Exists(fileName)) return;

            System.Drawing.Image image = Bitmap.FromFile(fileName);
            if (image != null)
            {
                x = x - image.Width / 2;
                y = y - image.Height / 2;
                gdi.DrawImage(image, x, y, image.Width, image.Height);

                // Draw number of participant in case of multiparticipant
                if (participantNo > 1)
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    string text = "(" + participantNo.ToString() + ")";
                    Font f = new Font("Arial", 10, FontStyle.Bold);
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x + image.Width - 5, y, 24, 16);
                    gdi.DrawString(text, f, new SolidBrush(Color.Red), rect, sf);
                }
            }


        }
    }
}
