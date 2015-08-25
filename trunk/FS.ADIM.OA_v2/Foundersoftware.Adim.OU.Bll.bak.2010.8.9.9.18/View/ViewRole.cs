//----------------------------------------------------------------
// Copyright (C) 2009 ����������޹�˾
//
// �ļ�������������ɫ���ݲ�ѯ��ͼ
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
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// ��ɫ���ݲ�ѯ��ͼ
    /// </summary>
    public class ViewRole : ViewBase
    {
        /// <summary>
        /// ���ָ��ʵ��
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Role(); }
        }

        ///<summary>
        ///���캯�� ������ͼ��
        ///</summary>
        public ViewRole()
        {
            base.Table = Role.TableName;
            base.Field = @"DISTINCT a.ID,a.No,a.RoleType,a.GetFunction,a.Name,a.Remark,a.SortNum,a.EditDate ";
            base.Join = @" LEFT JOIN " + RoleUser.TableName + " b ON a.ID=b.FK_RoleID ";

            base.InitElement("No", "No", "��ɫ���", TypeCode.String, true);
            base.InitElement("Name", "Name", "��ɫ��", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "��ע", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "��ʾ˳��", TypeCode.String, true);
            base.InitElement("RoleType", "RoleType", "��ɫ����", TypeCode.String, true);
            base.Sort = "a.ID ASC";
        }

        /// <summary>
        /// ���캯�� ������ͼ��
        /// </summary>
        public ViewRole(bool bFlage)
        {
            if (bFlage)
            {
                base.Table = Role.TableName;
                base.Field = @" ID,No,Name,RoleType,Remark,SortNum,EditDate";
                base.InitElement("Name", "Name", "��ɫ��", TypeCode.String, true);
                base.InitElement("RoleType", "RoleType", "��ɫ����", TypeCode.String, true);
            }
            else
            {
                base.Table = Role.TableName;
                base.Field = @"DISTINCT a.ID,a.No,a.RoleType,a.GetFunction,a.Name,a.Remark,a.SortNum,a.EditDate ";
                base.Join = @" LEFT JOIN " + RoleUser.TableName + " b ON a.ID=b.FK_RoleID ";

                base.InitElement("No", "No", "��ɫ���", TypeCode.String, true);
                base.InitElement("Name", "Name", "��ɫ��", TypeCode.String, true);
                base.InitElement("Remark", "Remark", "��ע", TypeCode.String, true);
                base.InitElement("SortNum", "SortNum", "��ʾ˳��", TypeCode.String, true);
                base.InitElement("RoleType", "RoleType", "��ɫ����", TypeCode.String, true);
                base.Sort = "a.ID ASC";
            }
        }
    }
}