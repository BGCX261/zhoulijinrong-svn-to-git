using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Process
{
    public class PG_ProcessDevolve : PageEntityBase
    {
        //加载用户控件路径
        private string m_virtualPath = "~/WorkflowMenu/Process/UC_ProcessDevolve.ascx";
        //加载对象
        private UC_ProcessDevolve m_uc;
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_ProcessDevolve;
            return m_uc;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "归档列表";
            }
        }
    }
}
