//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：部门的用户数据查询视图
// 
// 
// 创建标识：2009-12-2 王敏贤
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
    /// 部门的用户数据查询视图
    /// </summary>
    public class ViewDeptUser : ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewDeptUser()
        {
            base.Table = DeptPost.TableName;
            base.Field = @" a.FK_DeptID, b.Name, b.Domain, b.UserID, a.FK_UserID ";
            base.Join = @" INNER JOIN " + User.TableName + " b ON a.FK_UserID = b.ID ";
            base.Sort = " a.FK_DeptID";
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new DeptPost(); }
        }
    }
}
