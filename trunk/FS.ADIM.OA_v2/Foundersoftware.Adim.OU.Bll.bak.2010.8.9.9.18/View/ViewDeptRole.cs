//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：部门角色数据查询视图
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

using System;
using FounderSoftware.Framework.Business;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.ADIM.OU.BLL.AutoGene;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// 部门角色数据查询视图
    /// </summary>
    public class ViewDeptRole : ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewDeptRole()
        {
            base.Table = Role.TableName;
            base.Field = @" a.Name,a.Remark,a.SortNum,a.EditDate, b.ID, b.FK_RoleID,b.FK_DeptID,b.FK_PostID,a.Name AS RoleName";
            base.Join = @" RIGHT JOIN " + DeptRole.TableName + " b ON b.FK_RoleID=a.ID ";

            base.InitElement("a.No", "No", "No", TypeCode.String, true);
            base.InitElement("a.Name", "Name", "职位名", TypeCode.String, true);
            base.InitElement("a.SortNum", "SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("a.Remark", "Remark", "备注", TypeCode.String, true);
            base.InitElement("a.EditDate", " EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.EditDate Desc ";
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new DeptRole(); }
        }
    }
}
