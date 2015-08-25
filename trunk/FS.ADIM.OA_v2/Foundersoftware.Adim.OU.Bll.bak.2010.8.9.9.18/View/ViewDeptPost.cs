//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：部门职位数据查询视图
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
    /// 部门职位数据查询视图
    /// </summary>
    public class ViewDeptPost : ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewDeptPost()
        {
            base.Table = DeptPost.TableName;
            base.Field = @"DISTINCT a.FK_DeptID,b.Name,c.ID AS PostID,c.Name AS PostName,c.Remark,c.SortNum,a.EditDate,a.ID,a.LeaderType,case a.leaderType 
                            when '0' then ''
                            when '1' then '部门负责人'
                            when '2' then '部门领导'
                            when '3' then '部门负责人  部门领导' end AS LeaderManager ";
            base.Join = @" LEFT JOIN " + Department.TableName + " b ON a.FK_DeptID=b.ID "
                    + @" LEFT JOIN " + Position.TableName + " c ON a.FK_PostID=c.ID ";    
   
            base.InitElement("Name", "a.Name", "部门名", TypeCode.String, true);
            base.InitElement("Name", "c.ID", "职位编号", TypeCode.String, true);
            base.InitElement("Name", "c.Name", "职位名", TypeCode.String, true);
            base.InitElement("SortNum", "c.SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "c.Remark", "备注", TypeCode.String, true);
            base.InitElement("EditDate", "a.EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.EditDate Desc";
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
