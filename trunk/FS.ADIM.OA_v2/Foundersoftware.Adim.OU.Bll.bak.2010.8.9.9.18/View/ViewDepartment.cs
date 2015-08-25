//----------------------------------------------------------------
// Copyright (C) 2009 ����������޹�˾
//
// �ļ������������������ݲ�ѯ��ͼ
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
    /// �������ݲ�ѯ��ͼ
    /// </summary>
    public class ViewDepartment : ViewBase
    {
        /// <summary>
        /// ���ָ��ʵ��
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Department(); }
        }

        /// <summary>
        ///���캯�� ������ͼ��
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
        /// ������ͼ
        /// </summary>
        /// <param name="bFlag">�Ƿ�</param>
        public ViewDepartment(bool bFlag)
        {
            if (bFlag)
            {
                base.Table = Department.TableName;
                base.Field = @"ID,Name";
                base.InitElement("Name", "a.Name", "������", TypeCode.String, true);
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