//----------------------------------------------------------------
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：函件发文界面
//
// 
// 创建标识：周理 2010-01-11
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Web.UI;

namespace FS.ADIM.OA.WebUI.WorkFlow.LetterSend
{
    public class PG_LetterSend : PageEntityBase
    {
        private string m_virtualPath = string.Empty;

        protected sealed override Control CreateContentUC()
        {
            m_virtualPath = "WorkFlow/LetterSend/UC_LetterSend.ascx";
            return this.CurrentPage.LoadControl(this.m_virtualPath);
        }

        public override string Title
        {
            get
            {
                return "函件发文";
            }
        }
    }
}
