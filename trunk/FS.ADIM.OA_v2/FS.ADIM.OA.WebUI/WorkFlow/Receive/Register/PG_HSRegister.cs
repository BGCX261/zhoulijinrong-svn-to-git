using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.Register
{
    public class PG_HSRegister : PageEntityBase
    {
        private string m_virtualPath = "/WorkFlow/Receive/Register/UC_HSRegister.ascx";
        private UC_HSRegister m_uc;
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_HSRegister;
            return m_uc;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "函件收文 -- 收文登记";
            }
        }
    }
}
