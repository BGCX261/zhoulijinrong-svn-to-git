using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.CompleteFiles
{
    public class PG_CompletedHandle : PageEntityBase
    {
        private string m_virtualPath = "~/WorkflowMenu/CompleteFiles/UC_CompletedHandle.ascx";
        private UC_CompletedHandle m_uc;

        /// <summary>
        /// 加载已办文件页面
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_CompletedHandle;
            return m_uc;
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "已办文件";
            }
        }
    }
}
