//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：角色数据查询视图
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
    /// 角色数据查询视图
    /// </summary>
    public class ViewRole : ViewBase
    {
        /// <summary>
        /// 获得指定实体
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Role(); }
        }

        ///<summary>
        ///构造函数 设置视图列
        ///</summary>
        public ViewRole()
        {
            base.Table = Role.TableName;
            base.Field = @"DISTINCT a.ID,a.No,a.RoleType,a.GetFunction,a.Name,a.Remark,a.SortNum,a.EditDate ";
            base.Join = @" LEFT JOIN " + RoleUser.TableName + " b ON a.ID=b.FK_RoleID ";

            base.InitElement("No", "No", "角色编号", TypeCode.String, true);
            base.InitElement("Name", "Name", "角色名", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "备注", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("RoleType", "RoleType", "角色类型", TypeCode.String, true);
            base.Sort = "a.ID ASC";
        }

        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewRole(bool bFlage)
        {
            if (bFlage)
            {
                base.Table = Role.TableName;
                base.Field = @" ID,No,Name,RoleType,Remark,SortNum,EditDate";
                base.InitElement("Name", "Name", "角色名", TypeCode.String, true);
                base.InitElement("RoleType", "RoleType", "角色类型", TypeCode.String, true);
            }
            else
            {
                base.Table = Role.TableName;
                base.Field = @"DISTINCT a.ID,a.No,a.RoleType,a.GetFunction,a.Name,a.Remark,a.SortNum,a.EditDate ";
                base.Join = @" LEFT JOIN " + RoleUser.TableName + " b ON a.ID=b.FK_RoleID ";

                base.InitElement("No", "No", "角色编号", TypeCode.String, true);
                base.InitElement("Name", "Name", "角色名", TypeCode.String, true);
                base.InitElement("Remark", "Remark", "备注", TypeCode.String, true);
                base.InitElement("SortNum", "SortNum", "显示顺序", TypeCode.String, true);
                base.InitElement("RoleType", "RoleType", "角色类型", TypeCode.String, true);
                base.Sort = "a.ID ASC";
            }
        }
    }
}