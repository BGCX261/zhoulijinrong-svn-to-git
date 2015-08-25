using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask
{
    /// <summary>
    /// 草稿箱程序入口类
    /// </summary>
    public class PG_DraftList : PageEntityBase
    {
        //加载用户控件路径
        private string m_virtualPath = "~/WorkflowMenu/ToDoTask/UC_DraftList.ascx";
        //加载对象
        private UC_DraftList m_uc;
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_DraftList;
            return m_uc;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "草稿箱";
            }
        }
    }
}
