//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：公办文件
// 
// 
// 创建标识：wangbinyi 20100107
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//--------------------------------------------------------------
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask
{
    /// <summary>
    /// 公办文件程序入口类
    /// </summary>
    public class PG_CommonWaitHandle : PageEntityBase
    {
        //加载用户控件路径
        private string m_virtualPath = "~/WorkflowMenu/ToDoTask/UC_CommonWaitHandle.ascx";
        //加载对象
        private UC_CommonWaitHandle m_uc;
        /// <summary>
        /// 加载用户控件
        /// </summary>
        /// <returns></returns>
        protected override Control CreateContentUC()
        {
            this.m_uc = CurrentPage.LoadControl(this.m_virtualPath) as UC_CommonWaitHandle;
            return m_uc;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public override string Title
        {
            get
            {
                return "公办文件";
            }
        }
    }
}
