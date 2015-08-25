//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：请示报告
// 
// 
// 创建标识：wangbinyi 2009-12-28
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.RequestReport
{
    public class PG_RequestReport:PageEntityBase
    {
        private string m_virtualPath = string.Empty;

        protected sealed override Control CreateContentUC()
        {
            m_virtualPath = "WorkFlow/RequestReport/UC_RequestReport.ascx";
            return this.CurrentPage.LoadControl(this.m_virtualPath);
        }

        public override string Title
        {
            get
            {
                return "请示报告";
            }
        }
    }
}
