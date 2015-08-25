//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：角色用户查询视图
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
    /// 角色用户查询视图
    /// </summary>
    public class ViewRoleUsers : ViewBase
    {
        /// <summary>
        /// 角色用户查询视图
        /// </summary>
        public ViewRoleUsers()
        {
            base.Table = User.TableName;
            base.Field = @"a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,
                           a.MobilePhone,a.Email,a.SortNum,a.RecordStatus,a.Remark,a.EditDate,a.IsCancel,d.Name AS RoleName,a.Name AS UserName,c.FK_RoleID";
            base.Join = @" INNER JOIN " + RoleUser.TableName + " c ON a.ID=c.FK_UserID "
                      + @" INNER JOIN " + Role.TableName + " d ON d.ID=c.FK_RoleID  ";

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
        /// 
        /// </summary>
        /// <param name="bFlag"></param>
        public ViewRoleUsers(bool bFlag)
        {
            base.Table = RoleUser.TableName;
            base.Field = @"e.Name AS DeptName,f.Name AS PostName,b.ID,a.NO,b.Name,b.Domain,b.UserID,b.PWD,b.OfficePhone,b.MobilePhone,b.Email,b.SortNum,
                            b.Remark,b.EditDate,b.D_Class,b.ID AS UID,b.UserID AS ADCode,d.ID AS DpuID,d.LeaderType,
                            CASE b.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue,
                            CASE d.LeaderType WHEN '1' THEN '领导' WHEN '2' THEN '负责人' WHEN '3' THEN '领导;负责人' END AS LeaderTypeName ";
            
            base.Join = @" LEFT JOIN t_ou_user b ON b.ID=a.FK_UserID "
                            + @" LEFT JOIN " + Role.TableName + " c ON c.ID=a.FK_RoleID "
                            + @" LEFT JOIN " + DeptPost.TableName + " d ON b.ID=d.FK_UserID"
                            + @" LEFT JOIN " + Department.TableName + " e ON e.ID=d.FK_DeptID"
                            + @" LEFT JOIN " + Position.TableName + " f ON f.ID=d.FK_PostID ";
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
