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
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// 部门数据查询视图
    /// </summary>
    public class ViewDepartment : ViewBase
    {
        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Department(); }
        }

        /// <summary>
        ///构造函数 设置视图列
        /// </summary>
        public  ViewDepartment()
        {
            base.Table = Department.TableName;
            base.Field = @"Distinct a.ID,a.No,a.Name, a.SortNum,a.FloorCode,a.ParentID,a.DeptPath,a.Remark,a.EditDate,a.Name AS DeptName,e.Name AS ParentName,b.FK_DeptID";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_DeptID"
                       + @" LEFT JOIN " + User.TableName + " c ON c.ID=b.FK_UserID"
                       + @" LEFT JOIN " + Position.TableName + " d ON d.ID=b.FK_PostID"
                       + @" LEFT JOIN " + Department.TableName + " e ON a.ParentID=e.ID";
            base.Sort = "a.SortNum,a.EditDate Desc";
        }

        /// <summary>
        /// 部门视图
        /// </summary>
        /// <param name="bFlag">是否</param>
        public ViewDepartment(bool bFlag)
        {
            if (bFlag)
            {
                base.Table = Department.TableName;
                base.Field = @"ID,Name";
                base.InitElement("Name", "a.Name", "部门名", TypeCode.String, true);
                base.Sort = "ID";
            }
            else
            {
                base.Table = Department.TableName;
                base.Field = @"a.ID, a.No, a.Name, b.Name as ParentName, a.Remark";
                base.Join = "LEFT Join [" + Department.TableName + "] b On a.ParentID = b.ID";
                base.Sort = "a.SortNum Asc, a.Name Asc";
            }
        }        
   }
}