//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：公司数据查询视图
// 
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// 公司数据查询视图
    /// </summary>
    public class ViewCompany : ViewBase
    {
        /// <summary>
        /// 构造函数,设置视图列
        /// </summary>
        public ViewCompany()
        {
            base.Table = Company.TableName;
            //base.Sort = " EditDate Desc   ";   //杨子江 2011-04-06
            base.Sort = " Name Desc  ";          //杨子江 2011-04-06
            //base.
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Company(); }
        }
    }
}