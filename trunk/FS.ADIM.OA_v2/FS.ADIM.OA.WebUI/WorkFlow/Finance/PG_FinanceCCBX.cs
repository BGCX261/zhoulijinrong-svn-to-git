//----------------------------------------------------------------
// Copyright (C) 2013
//
// 文件功能描述：出差(培训)报销单
// 
// 
// 创建标识：ZHOULI 2013-04-2
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public class PG_FinanceCCBX : PageEntityBase
    {
        private string m_virtualPath = string.Empty;

        protected sealed override Control CreateContentUC()
        {
            m_virtualPath = "WorkFlow/Finance/UC_FinanceCCBX.ascx";
            return this.CurrentPage.LoadControl(this.m_virtualPath);
        }

        public override string Title
        {
            get
            {
                return "出差(培训)报销单";
            }
        }
    }
}
