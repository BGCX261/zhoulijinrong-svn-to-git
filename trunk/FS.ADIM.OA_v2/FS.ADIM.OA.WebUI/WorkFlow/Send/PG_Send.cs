//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：发文
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
using System;
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Web.UI;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.Send
{
    public class PG_Send : PageEntityBase
    {
        private string m_virtualPath = string.Empty;

        protected sealed override Control CreateContentUC()
        {
            string ProcessName = this.CurrentPage.Request.QueryString["TemplateName"];
            if (String.IsNullOrEmpty(ProcessName))
            {
                ProcessName = this.CurrentPage.Session[ConstString.Session.TEMPLATE_NAME].ToString();
            }
            switch (ProcessName)
            {
                //公司发文
                case ProcessConstString.TemplateName.COMPANY_SEND:
                    m_virtualPath = "WorkFlow/Send/UC_CompanySend.ascx";
                    break;
                //党纪工团发文
                case ProcessConstString.TemplateName.DJGT_Send:
                    m_virtualPath = "WorkFlow/Send/UC_Send.ascx";
                    break;
                ////工会发文
                //case ProcessConstString.TemplateName.TRADE_UNION_SEND:
                //    m_virtualPath = "WorkFlow/Send/UC_Send.ascx";
                //    break;
                ////纪委发文
                //case ProcessConstString.TemplateName.DISCIPLINE_SEND:
                //    m_virtualPath = "WorkFlow/Send/UC_Send.ascx";
                //    break;
                ////团委发文
                //case ProcessConstString.TemplateName.YOUTH_LEAGUE_SEND:
                //    m_virtualPath = "WorkFlow/Send/UC_Send.ascx";
                //    break;
            }
            return this.CurrentPage.LoadControl(this.m_virtualPath);
        }

        public override string Title
        {
            get
            {
                return "发文流程";
            }
        }
    }
}
