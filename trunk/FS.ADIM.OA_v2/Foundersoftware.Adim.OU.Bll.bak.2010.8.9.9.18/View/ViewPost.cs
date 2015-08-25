//----------------------------------------------------------------
// Copyright (C) 2009 ����������޹�˾
//
// �ļ�����������ְλ���ݲ�ѯ��ͼ
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
    /// ְλ���ݲ�ѯ��ͼ
    /// </summary>
    public class ViewPost : ViewBase
    {
        /// <summary>
        /// ���캯�� ������ͼ��
        /// </summary>
        public ViewPost()
        {
            base.Table = Position.TableName;
            base.Field = @"DISTINCT a.ID,a.No,a.Name,a.SortNum,a.Remark,a.EditDate ";
            base.Join = @" LEFT JOIN " + DeptPost.TableName + " b ON a.ID=b.FK_PostID ";
            base.InitElement("No", "No", "No", TypeCode.String, true);
            base.InitElement("Name", "Name", "ְλ��", TypeCode.String, true);
            base.InitElement("SortNum", "SortNum", "��ʾ˳��", TypeCode.String, true);
            base.InitElement("Remark", "Remark", "��ע", TypeCode.String, true);
            base.InitElement("EditDate", " EditDate", "�޸�ʱ��", TypeCode.String, true);
            base.Sort = "a.SortNum";
        }

        /// <summary>
        /// ���캯�� ������ͼ��
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
        /// ���ָ��ʵ��
        /// </summary>
        protected override FounderSoftware.Framework.Business.Entity enCurr
        {
            get { return new Position(); }
        }
    }
}