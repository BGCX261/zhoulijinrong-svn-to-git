//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：角色用户查询视图
// 
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-12-22 王敏贤
// 修改描述：重载构造函数 ViewRoleUser(bool bFlag)只关联User表
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
    /// 角色用户查询视图
    /// </summary>
    public class ViewRoleUser : ViewBase
    {
        /// <summary>
        /// User构造函数
        /// </summary>
        public ViewRoleUser()
        {
            base.Table = User.TableName;
            base.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.D_Class,
                           a.MobilePhone,a.Email,a.SortNum,a.RecordStatus,a.Remark,a.EditDate,a.IsCancel,d.Name AS RoleName,a.Name AS UserName,c.FK_RoleID,
                           CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";

            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                    + @" LEFT JOIN " + RoleUser.TableName + " c ON a.ID=c.FK_UserID  "
                    + @" LEFT JOIN " + Role.TableName + " d ON d.ID=c.FK_RoleID  "
                    + @" LEFT JOIN " + Department.TableName + " e ON e.ID=b.FK_DeptID  "
                    + @" LEFT JOIN " + Position.TableName + " f ON f.ID=b.FK_PostID ";

            base.InitElement("ID", "a.ID", "ID", TypeCode.String, false);          
            base.InitElement("No", "a.No", "员工编号", TypeCode.String, false);
            base.InitElement("Name", "a.Name", "姓名", TypeCode.String, true);
            base.InitElement("Domain", "a.Domain", "Domain", TypeCode.String, true);
            base.InitElement("UserID", "a.UserID", "人员域帐号", TypeCode.String, true);
            base.InitElement("PWD", "PWD", "a.PWD", TypeCode.String, true);
            base.InitElement("OfficePhone", "a.OfficePhone", "办公电话", TypeCode.String, true);
            base.InitElement("MobilePhone", "a.MobilePhone", "手机", TypeCode.String, true);
            base.InitElement("Email", "a.Email", "电子邮件", TypeCode.String, true);
            base.InitElement("SortNum", "a.SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "a.Remark", "备注", TypeCode.String, true);
            base.InitElement("EditDate", "a.EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.SortNum ASC,a.Name ASC";
        }

        /// <summary>
        /// 标识只关联User表
        /// </summary>
        public ViewRoleUser(bool bFlag)
        {
            base.Table = User.TableName;
            if (bFlag)
            {
                base.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,c.FK_RoleID,
                           a.MobilePhone,a.Email,a.SortNum,a.RecordStatus,a.Remark,a.EditDate,a.IsCancel,d.Name AS RoleName,a.Name AS UserName";

                base.Join = @" INNER JOIN " + RoleUser.TableName + " c ON a.ID=c.FK_UserID "
                        + @" INNER JOIN " + Role.TableName + " d ON d.ID=c.FK_RoleID  ";
            }
            else
            {
                base.Field = @"DISTINCT f.Name AS PostName,a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.D_Class,
                           a.MobilePhone,a.Email,a.SortNum,a.RecordStatus,a.Remark,a.EditDate,a.IsCancel,d.Name AS RoleName,a.Name AS UserName,c.FK_RoleID,
                           CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";

                base.Join = @" INNER JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                        + @" INNER JOIN " + RoleUser.TableName + " c ON a.ID=c.FK_UserID  "
                        + @" INNER JOIN " + Role.TableName + " d ON d.ID=c.FK_RoleID  "
                        + @" INNER JOIN " + Department.TableName + " e ON e.ID=b.FK_DeptID  "
                        + @" INNER JOIN " + Position.TableName + " f ON f.ID=b.FK_PostID  ";
            }

            base.InitElement("ID", "a.ID", "ID", TypeCode.String, false);
            base.InitElement("No", "a.No", "员工编号", TypeCode.String, false);
            base.InitElement("Name", "a.Name", "姓名", TypeCode.String, true);
            base.InitElement("Domain", "a.Domain", "Domain", TypeCode.String, true);
            base.InitElement("UserID", "a.UserID", "人员域帐号", TypeCode.String, true);
            base.InitElement("PWD", "PWD", "a.PWD", TypeCode.String, true);
            base.InitElement("OfficePhone", "a.OfficePhone", "办公电话", TypeCode.String, true);
            base.InitElement("MobilePhone", "a.MobilePhone", "手机", TypeCode.String, true);
            base.InitElement("Email", "a.Email", "电子邮件", TypeCode.String, true);
            base.InitElement("SortNum", "a.SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "a.Remark", "备注", TypeCode.String, true);
            base.InitElement("EditDate", "a.EditDate", "修改时间", TypeCode.String, true);

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
