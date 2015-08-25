//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：用户查询视图
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
    /// 用户查询视图
    /// </summary>
    public class ViewUser : ViewBase
    {
        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        public ViewUser()
        {
            base.Table = User.TableName;
            base.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,e.SortNum AS PostSortNum,e.Name AS PostName,
                                                CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue,b.FK_DeptID,d.Name as DeptName";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                      + @" LEFT JOIN " + Department.TableName + " d ON d.ID=b.FK_DeptID "
                      + @"LEFT JOIN " + Position.TableName + " e ON e.ID=b.FK_PostID  ";

            base.InitElement("No", "No", "员工编号", TypeCode.String, true);
            base.InitElement("Name", "Name", "姓名", TypeCode.String, true);
            base.InitElement("Name", "DeptName", "部门", TypeCode.String, true);
            base.InitElement("Domain", "Domain", "Domain", TypeCode.String, true);
            base.InitElement("UserID", "UserID", "人员域帐号", TypeCode.String, true);
            base.InitElement("PWD", "PWD", "PWD", TypeCode.String, true);
            base.InitElement("OfficePhone", "OfficePhone", "办公电话", TypeCode.String, true);
            base.InitElement("MobilePhone", "MobilePhone", "手机", TypeCode.String, true);
            base.InitElement("Email", "Email", "电子邮件", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "备注", TypeCode.String, true);
            base.InitElement("EditDate", "EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.SortNum ASC,a.Name ASC";
        }

        /// <summary>
        /// 构造函数 设置视图列
        /// </summary>
        /// <param name="bFalg">标示</param>
        public ViewUser(bool bFalg)
        {
            base.Table = User.TableName;
            if (bFalg)
            {
                base.Field = @"a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                           a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,
                           CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";
            }
            else
            {
                base.Field = @" e.Name AS PostName,a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,b.ID AS DpuID,b.LeaderType,
                                                CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue,
                                                CASE b.LeaderType WHEN '1' THEN '领导' WHEN '2' THEN '负责人' WHEN '3' THEN '领导;负责人' END AS LeaderTypeName ";
                base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                          + @" LEFT JOIN " + Department.TableName + " d ON d.ID=b.FK_DeptID "
                          + @" LEFT JOIN " + Position.TableName + " e ON e.ID=b.FK_PostID ";
            }

            base.InitElement("No", "No", "员工编号", TypeCode.String, true);
            base.InitElement("Name", "Name", "姓名", TypeCode.String, true);
            base.InitElement("Domain", "Domain", "Domain", TypeCode.String, true);
            base.InitElement("UserID", "UserID", "人员域帐号", TypeCode.String, true);
            base.InitElement("PWD", "PWD", "PWD", TypeCode.String, true);
            base.InitElement("OfficePhone", "OfficePhone", "办公电话", TypeCode.String, true);
            base.InitElement("MobilePhone", "MobilePhone", "手机", TypeCode.String, true);
            base.InitElement("Email", "Email", "电子邮件", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "显示顺序", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "备注", TypeCode.String, true);
            base.InitElement("EditDate", "EditDate", "修改时间", TypeCode.String, true);
            base.Sort = "a.SortNum ASC,a.Name ASC";
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetJoin()
        {
            base.Join = @" LEFT JOIN " + RoleUser.TableName + " RU on A.ID = RU.FK_UserID "
                      + @" LEFT JOIN " + Role.TableName + " R on RU.FK_RoleID = R.ID "
                      + @" LEFT JOIN " + DeptPost.TableName + " DPU on A.ID = DPU.FK_UserID ";
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