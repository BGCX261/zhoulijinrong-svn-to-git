//----------------------------------------------------------------
// Copyright (C) 2009 ����������޹�˾
//
// �ļ�������������˾���ݲ�ѯ��ͼ
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

using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.View
{
    /// <summary>
    /// ��˾���ݲ�ѯ��ͼ
    /// </summary>
    public class ViewCompany : ViewBase
    {
        /// <summary>
        /// ���캯��,������ͼ��
        /// </summary>
        public ViewCompany()
        {
            base.Table = Company.TableName;
            //base.Sort = " EditDate Desc   ";   //���ӽ� 2011-04-06
            base.Sort = " Name Desc  ";          //���ӽ� 2011-04-06
            //base.
        }

        /// <summary>
        /// ���ָ��ʵ��
        /// </summary>
        protected override Entity enCurr
        {
            get { return new Company(); }
        }
    }
}