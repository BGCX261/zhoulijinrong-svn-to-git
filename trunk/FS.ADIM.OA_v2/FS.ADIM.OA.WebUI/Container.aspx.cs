using System;
using System.Collections.Generic;
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.WebUI.UIBase;

namespace FS.ADIM.OA.WebUI
{
    public partial class Container : WebFormContainer
    {
        public override void InitParameters()
        {
            base.m_pageAssemblyName = "FS.ADIM.OA.WebUI";

            //从海南公司门户主页直接进入待办等页面
            if (this.Request.Params["target"] != null)
            {
                String l_strTarget = Request.Params["target"];
                switch (l_strTarget)
                {
                    case "1": base.m_pageClassName = "FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle"; break;
                    case "2": base.m_pageClassName = "FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_CommonWaitHandle"; break;
                    case "3": base.m_pageClassName = "FS.ADIM.OA.WebUI.WorkflowMenu.Circulate.PG_WaitReading"; break;
                    case "4": base.m_pageClassName = "FS.ADIM.OA.WebUI.WorkflowMenu.CompleteFiles.PG_CompletedHandle"; break;
                    default: base.m_pageClassName = "FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle"; break;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ClassName"]))
                {
                    base.m_pageClassName = Request.QueryString["ClassName"];
                }
                else
                {
                    String l_strTemplateName = Server.UrlDecode(Request.QueryString[ConstString.QueryString.TEMPLATE_NAME]);
                    String l_strStepName = Server.UrlDecode(Request.QueryString[ConstString.QueryString.STEP_NAME]);
                    if (l_strTemplateName == "函件收文" || l_strTemplateName == "函件发文")
                    {
                        if (l_strTemplateName == "函件收文" && l_strStepName.Equals("函件收文发起"))//老版函件收文为“函件收文发起”
                        {
                            l_strStepName = ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL;
                        }
                        l_strTemplateName = "新版" + l_strTemplateName;
                    }
                    base.m_pageClassName = GetUiPath(l_strTemplateName, -1, l_strStepName);
                }
            }
            base.m_containerCtls = new Control[] { this.containBody };
        }

        protected override void OnInit(EventArgs e)
        {
            if (base.m_pageEntity != null)
            {
                this.containTitle.InnerText = base.m_pageEntity.Title;
            }
            base.OnInit(e);
        }

        /// <summary>
        /// 获取templateID对应templateVersion版本对应视图的的相对路径
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="templateVersion"></param>
        /// <param name="viewID"></param>
        /// <returns></returns>
        private String GetUiPath(String p_strTemplateName, int p_intVersion, String p_strStepName)
        {
            TemplateAdmin TAdmin = TemplateAdmin.CreateTemplateAdmin(Page);
            String l_strTargetUrl = String.Empty;
            Template template;

            template = TAdmin.Templates.GetTemplate(p_strTemplateName);

            if (p_intVersion == -1)
            {
                l_strTargetUrl = template.GetVersion(template.MaxVersion).GetView(p_strStepName).Path;
            }
            else
            {
                l_strTargetUrl = template.GetVersion(p_intVersion).GetView(p_strStepName).Path;
            }
            if (String.IsNullOrEmpty(l_strTargetUrl))
            {
                throw new Exception("表单模板路径获取失败!");
            }
            return l_strTargetUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //String ToUrl=String.Empty;
            //if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsUseSSO"].ToString() == "1")
            //{
            //    ToUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["SSOLoginURL"].ToString();
            //}
            //else
            //{
            //    ToUrl = "Login.aspx";
            //}
            //if (!CheckPagePowerURL())
            //{
            //   HttpContext.Current.Response.Write(" <script>this.parent.location= '" + ToUrl + "'</script> ");
            //}
        }

        ///当用户通过粘贴url进入工作流表单的时候，检查权限
        private bool CheckPagePowerURL()
        {
            if (null != Session["UrlPowers"])
            {
                List<String> Urls = Session["UrlPowers"] as List<String>;
                foreach (String url in Urls)
                {
                    if (-1 != url.IndexOf(base.m_pageClassName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
