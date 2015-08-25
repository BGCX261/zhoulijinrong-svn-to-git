//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：部门角色视图
// 
// 创建标识：2010-2-2 王敏贤
//
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------

using System;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// 部门角色用户视图
    /// </summary>
    public class ViewDeptRoleUser:ViewBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewDeptRoleUser()
        {
            base.Table = User.TableName;
            base.Field = @" f.Name AS PostName,a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                            a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,b.ID AS DpuID,b.LeaderType,
                                                            CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue,
                                                            CASE b.LeaderType WHEN '1' THEN '领导' WHEN '2' THEN '负责人' WHEN '3' THEN '领导;负责人' END AS LeaderTypeName ";

            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID = b.FK_UserID "
                    + @" LEFT JOIN " + RoleUser.TableName + " c ON a.ID=c.FK_UserID  "
                    + @" LEFT JOIN " + Role.TableName + " d ON c.FK_RoleID = d.ID  "
                    + @" LEFT JOIN " + Department.TableName + " e ON e.ID=b.FK_DeptID "
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
    }
}
