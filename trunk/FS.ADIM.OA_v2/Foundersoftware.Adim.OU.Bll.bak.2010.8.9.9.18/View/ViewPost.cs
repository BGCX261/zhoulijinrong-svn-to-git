//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：职位数据查询视图
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
    /// 职位数据查询视图
    /// </summary>
    public class ViewPost : ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewPost()
        {
            base.Table = Position.TableName;
            base.Field = @"DISTINCT a.ID,a.No,a.Name,a.SortNum,a.Remark,a.EditDate ";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_PostID ";
            base.InitElement("No", "No", "No", TypeCode.String, true);
            base.InitElement("Name", "Name", "职位名", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "备注", TypeCode.String, true);
            base.InitElement("EditDate", " EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.SortNum";
        }

        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewPost(bool bFlag)
        {
            if (bFlag)
            {
                base.Table = Position.TableName;
                base.Field = @"a.ID,a.No,a.Name,a.SortNum,a.Remark,a.EditDate,a.MaxSortNum,a.MinSortNum ";
                base.Sort = "a.SortNum";
            }
            else
            {
                base.Table = Position.TableName;
                base.Field = @"a.ID,a.No,a.Name,a.SortNum,a.Remark,a.EditDate,a.MaxSortNum,a.MinSortNum ";
                base.Sort = "a.SortNum";
            }
        }

        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override FounderSoftware.Framework.Business.Entity enCurr
        {
            get { return new Position(); }
        }
    }
}