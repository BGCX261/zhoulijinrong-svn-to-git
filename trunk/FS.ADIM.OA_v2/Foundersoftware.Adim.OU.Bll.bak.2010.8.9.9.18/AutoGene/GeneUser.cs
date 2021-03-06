//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：用户信息
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.AutoGene
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public abstract class GeneUser : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_User";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneUser()
            : base(GeneUser.TableName)
        {
        }

        /// <summary>
        /// 初始化视图列 true不显示 false不显示
        /// </summary>
        protected sealed override void InitColumnSelf()
        {
            base.InitColumn("Name", "姓名", true);
            base.InitColumn("Domain", "Domain", true);
            base.InitColumn("UserID", "人员域帐号", true);
            base.InitColumn("PWD", "PWD", true);
            base.InitColumn("OfficePhone", "办公电话", true);
            base.InitColumn("MobilePhone", "手机", true);
            base.InitColumn("Email", "电子邮件", true);
            base.InitColumn("SortNum", "显示顺序", true);
            base.InitColumn("D_SecretLevel", "密级", true);
            base.InitColumn("D_Class", "人员类别：商务，合同，财务", true);
            base.InitColumn("Remark", "备注", true);
            base.InitColumn("IsCancel", "是否注销", true);
            base.InitColumn("Image", "签名图片", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return base.GetValStr("Name"); }
            set { base.SetVal("Name", value); }
        }

        /// <summary>
        /// 签名图片
        /// </summary>
        public byte[] ImgContent
        {
            get { return base.GetValBytes("Image"); }
            set { base.SetVal("Image", value); }
        }

        /// <summary>
        /// 是否注销
        /// </summary>
        public bool IsCancel
        {
            get { return base.GetValBool("IsCancel"); }
            set { base.SetVal("IsCancel", value); }
        }

        /// <summary>
        /// Domain
        /// </summary>
        public string Domain
        {
            get { return base.GetValStr("Domain"); }
            set { base.SetVal("Domain", value); }
        }

        /// <summary>
        /// 人员域帐号
        /// </summary>
        public string UserID
        {
            get { return base.GetValStr("UserID"); }
            set { base.SetVal("UserID", value); }
        }

        /// <summary>
        /// PWD
        /// </summary>
        public string PWD
        {
            get { return base.GetValStr("PWD"); }
            set { base.SetVal("PWD", value); }
        }

        /// <summary>
        /// 办公电话
        /// </summary>
        public string OfficePhone
        {
            get { return base.GetValStr("OfficePhone"); }
            set { base.SetVal("OfficePhone", value); }
        }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone
        {
            get { return base.GetValStr("MobilePhone"); }
            set { base.SetVal("MobilePhone", value); }
        }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
        {
            get { return base.GetValStr("Email"); }
            set { base.SetVal("Email", value); }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int SortNum
        {
            get { return base.GetValInt("SortNum"); }
            set { base.SetVal("SortNum", value); }
        }
        
        /// <summary>
        /// 密级
        /// </summary>
        public string D_SecretLevel
        {
            get { return base.GetValStr("D_SecretLevel"); }
            set { base.SetVal("D_SecretLevel", value); }
        }
        
        /// <summary>
        /// 人员类别：商务，合同，财务
        /// </summary>
        public string D_Class
        {
            get { return base.GetValStr("D_Class"); }
            set { base.SetVal("D_Class", value); }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return base.GetValStr("Remark"); }
            set { base.SetVal("Remark", value); }
        }

        #endregion
    }
}