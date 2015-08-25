//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：流程视图
// 
//
// 创建标识：2010-1-12 王敏贤
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// 流程视图
    /// </summary>
    public class ViewFlow:ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewFlow()
        {
            base.Table = Flow.TableName;
            base.Field = "ID,Name";
            base.Sort = "ID";
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Flow(); }
        }
    }
}
