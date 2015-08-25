//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：程序发起（创建程序、升版程序）
// 
// 创 建 者：黄琦
// 创建时间：2009-07-11
// 创建标识：C_20090711
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System.Web.UI;

using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public class PG1_ProgramFileList : PageEntityBase
    {
        private string m_virtualPath = "WorkFlow/ProgramFile/UC1_ProgramFileList.ascx";
        private UC1_ProgramFileList m_uc;

        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC1_ProgramFileList;
            return m_uc;
        }

        public override string Title
        {
            get
            {
                return "程序文件 -- 程序列表";
            }
        }
    }
}
