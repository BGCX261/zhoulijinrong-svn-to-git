//----------------------------------------------------------------
// Copyright (C) 2009 ����������޹�˾
//
// �ļ������������û���ѯ��ͼ
// 
// 
// ������ʶ��2009-11-6 ������
//
// �޸ı�ʶ��
// �޸�������
//
// �޸ı�ʶ��
// �޸�������
//----------------------------------------------------------------

using System;
using FounderSoftware.Framework.Business;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.ADIM.OU.BLL.AutoGene;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// �û���ѯ��ͼ
    /// </summary>
    public class ViewUser : ViewBase
    {
        /// <summary>
        /// ���캯�� ������ͼ��
        /// </summary>
        public ViewUser()
        {
            base.Table = User.TableName;
            base.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,e.SortNum AS PostSortNum,e.Name AS PostName,
                                                CASE a.IsCancel WHEN '1' THEN '����' ELSE 'ע��'END AS HideStatue,b.FK_DeptID,d.Name as DeptName";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                      + @" LEFT JOIN " + Department.TableName + " d ON d.ID=b.FK_DeptID "
                      + @"LEFT JOIN " + Position.TableName + " e ON e.ID=b.FK_PostID  ";

            base.InitElement("No", "No", "Ա�����", TypeCode.String, true);
            base.InitElement("Name", "Name", "����", TypeCode.String, true);
            base.InitElement("Name", "DeptName", "����", TypeCode.String, true);
            base.InitElement("Domain", "Domain", "Domain", TypeCode.String, true);
            base.InitElement("UserID", "UserID", "��Ա���ʺ�", TypeCode.String, true);
            base.InitElement("PWD", "PWD", "PWD", TypeCode.String, true);
            base.InitElement("OfficePhone", "OfficePhone", "�칫�绰", TypeCode.String, true);
            base.InitElement("MobilePhone", "MobilePhone", "�ֻ�", TypeCode.String, true);
            base.InitElement("Email", "Email", "�����ʼ�", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "��ʾ˳��", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "��ע", TypeCode.String, true);
            base.InitElement("EditDate", "EditDate", "�޸�ʱ��", TypeCode.String, true);
            base.Sort = "a.SortNum ASC,a.Name ASC";
        }

        /// <summary>
        /// ���캯�� ������ͼ��
        /// </summary>
        /// <param name="bFalg">��ʾ</param>
        public ViewUser(bool bFalg)
        {
            base.Table = User.TableName;
            if (bFalg)
            {
                base.Field = @"a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                           a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,
                           CASE a.IsCancel WHEN '1' THEN '����' ELSE 'ע��'END AS HideStatue";
            }
            else
            {
                base.Field = @" e.Name AS PostName,a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,b.ID AS DpuID,b.LeaderType,
                                                CASE a.IsCancel WHEN '1' THEN '����' ELSE 'ע��'END AS HideStatue,
                                                CASE b.LeaderType WHEN '1' THEN '�쵼' WHEN '2' THEN '������' WHEN '3' THEN '�쵼;������' END AS LeaderTypeName ";
                base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_UserID "
                          + @" LEFT JOIN " + Department.TableName + " d ON d.ID=b.FK_DeptID "
                          + @" LEFT JOIN " + Position.TableName + " e ON e.ID=b.FK_PostID ";
            }

            base.InitElement("No", "No", "Ա�����", TypeCode.String, true);
            base.InitElement("Name", "Name", "����", TypeCode.String, true);
            base.InitElement("Domain", "Domain", "Domain", TypeCode.String, true);
            base.InitElement("UserID", "UserID", "��Ա���ʺ�", TypeCode.String, true);
            base.InitElement("PWD", "PWD", "PWD", TypeCode.String, true);
            base.InitElement("OfficePhone", "OfficePhone", "�칫�绰", TypeCode.String, true);
            base.InitElement("MobilePhone", "MobilePhone", "�ֻ�", TypeCode.String, true);
            base.InitElement("Email", "Email", "�����ʼ�", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "��ʾ˳��", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "��ע", TypeCode.String, true);
            base.InitElement("EditDate", "EditDate", "�޸�ʱ��", TypeCode.String, true);
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
        /// ���ָ��ʵ��
        /// </summary>
        protected override Entity enCurr
        {
            get { return new User(); }
        }
    }
}