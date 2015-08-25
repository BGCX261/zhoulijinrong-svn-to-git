//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：部门数据查询视图
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
    /// 部门数据查询视图
    /// </summary>
    public class ViewUserDept:ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewUserDept()
        {
            base.Table = Department.TableName;
            base.Field = @" a.ID,a.No,a.Name,a.SortNum,a.FloorCode,a.ParentID,a.DeptPath,a.Remark,a.EditDate,a.Name AS DeptName ";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_DeptID";

            base.InitElement("No", "a.No", "部门编号", TypeCode.String, false);
            base.InitElement("Name", "a.Name", "部门名", TypeCode.String, true);
            base.InitElement("SortNum", "a.SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "a.Remark", "备注", TypeCode.String, true);
            base.InitElement("FloorCode", "a.FloorCode", "部门层次", TypeCode.String, false);
            base.InitElement("EditDate", "a.EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.SortNum  ,a.EditDate Desc   ";
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Department(); }
        }
    }
}
