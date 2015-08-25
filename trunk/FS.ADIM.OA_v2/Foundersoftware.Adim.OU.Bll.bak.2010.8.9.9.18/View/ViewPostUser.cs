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
    /// 职位用户视图
    /// </summary>
    public class ViewPostUser : ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewPostUser()
        {
            base.Table = User.TableName;
            base.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                      + @" LEFT JOIN " + Position.TableName + " c ON c.ID=b.FK_PostID ";
            base.Sort = "a.SortNum ASC,a.Name ASC";
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new User(); }
        }
    }
}
