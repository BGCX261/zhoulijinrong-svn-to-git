//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：公司信息
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using System;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.AutoGene
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public abstract class GeneCompany : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_Company";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneCompany()
            : base(GeneCompany.TableName)
        {
        }

        /// <summary>
        /// 初始化视图列 true不显示 false不显示
        /// </summary>
        protected sealed override void InitColumnSelf()
        {
            base.InitColumn("Name", "单位名称", true);
            base.InitColumn("OldName1", "曾用名1", true);
            base.InitColumn("OldName2", "曾用名2", true);
            base.InitColumn("EnglishName", "英文名", true);
            base.InitColumn("AnotherName1", "别称1", true);
            base.InitColumn("AnotherName2", "别称2", true);
            base.InitColumn("IsSupply", "是否潜在供方(0:否,1:是)", true);
            base.InitColumn("FaxNumber", "传真", true);
            base.InitColumn("TelphoneNumber", "电话号码", true);
            base.InitColumn("CommunicationAddress", "通信地址", true);
            base.InitColumn("PostCode", "邮政编码", true);
            base.InitColumn("WebAddress", "网址", true);
            base.InitColumn("EmailAddress", "电子邮件", true);
            base.InitColumn("FictitiousPerson", "法人代表", true);
            base.InitColumn("RegisterNumber", "注册号", true);
            base.InitColumn("RegisterDate", "注册日期", true);
            base.InitColumn("FinancingLevel", "资信等级", true);
            base.InitColumn("RegisterFinancing", "注册资金", true);
            base.InitColumn("QuilityLevel", "质保等级", true);
            base.InitColumn("TaxNumber", "公司税号", true);
            base.InitColumn("Country", "所在国家", true);
            base.InitColumn("MadeLicense", "制造许可证", true);
            base.InitColumn("DegisnLicense", "设计许可证", true);
            base.InitColumn("OrganizationKind", "单位类别", true);
            base.InitColumn("QCYCDate", "QualificationsCertificateYearCheckDate(资质证书年审日期)", true);
            base.InitColumn("QCNum", "QualificationsCertificateNumber(资质证书号码)", true);
            base.InitColumn("CLYCDate", "CompanyLicenseYearCheckDate", true);
            base.InitColumn("ContactPerson", "业务联系人", true);
            base.InitColumn("RelateDeptment", "关联部门", true);
            base.InitColumn("SupplerRange", "供货范围", true);
            base.InitColumn("Achievement", "业绩", true);
            base.InitColumn("Remark", "备注", true);
            base.InitColumn("FlowRange", "流程范围", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name
        {
            get { return base.GetValStr("Name"); }
            set { base.SetVal("Name", value); }
        }

        /// <summary>
        /// 流程范围
        /// </summary>
        public string FlowRange
        {
            get { return base.GetValStr("FlowRange"); }
            set { base.SetVal("FlowRange", value); }
        }

        /// <summary>
        /// 曾用名1
        /// </summary>
        public string OldName1
        {
            get { return base.GetValStr("OldName1"); }
            set { base.SetVal("OldName1", value); }
        }

        /// <summary>
        /// 曾用名2
        /// </summary>
        public string OldName2
        {
            get { return base.GetValStr("OldName2"); }
            set { base.SetVal("OldName2", value); }
        }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName
        {
            get { return base.GetValStr("EnglishName"); }
            set { base.SetVal("EnglishName", value); }
        }

        /// <summary>
        /// 别称1
        /// </summary>
        public string AnotherName1
        {
            get { return base.GetValStr("AnotherName1"); }
            set { base.SetVal("AnotherName1", value); }
        }

        /// <summary>
        /// 别称2
        /// </summary>
        public string AnotherName2
        {
            get { return base.GetValStr("AnotherName2"); }
            set { base.SetVal("AnotherName2", value); }
        }

        /// <summary>
        /// 是否潜在供方(0:否,1:是)
        /// </summary>
        public int IsSupply
        {
            get { return base.GetValInt("IsSupply"); }
            set { base.SetVal("IsSupply", value); }
        }

        /// <summary>
        /// 传真
        /// </summary>
        public string FaxNumber
        {
            get { return base.GetValStr("FaxNumber"); }
            set { base.SetVal("FaxNumber", value); }
        }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelphoneNumber
        {
            get { return base.GetValStr("TelphoneNumber"); }
            set { base.SetVal("TelphoneNumber", value); }
        }

        /// <summary>
        /// 通信地址
        /// </summary>
        public string CommunicationAddress
        {
            get { return base.GetValStr("CommunicationAddress"); }
            set { base.SetVal("CommunicationAddress", value); }
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostCode
        {
            get { return base.GetValStr("PostCode"); }
            set { base.SetVal("PostCode", value); }
        }

        /// <summary>
        /// 网址
        /// </summary>
        public string WebAddress
        {
            get { return base.GetValStr("WebAddress"); }
            set { base.SetVal("WebAddress", value); }
        }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string EmailAddress
        {
            get { return base.GetValStr("EmailAddress"); }
            set { base.SetVal("EmailAddress", value); }
        }

        /// <summary>
        /// 法人代表
        /// </summary>
        public string FictitiousPerson
        {
            get { return base.GetValStr("FictitiousPerson"); }
            set { base.SetVal("FictitiousPerson", value); }
        }

        /// <summary>
        /// 注册号
        /// </summary>
        public string RegisterNumber
        {
            get { return base.GetValStr("RegisterNumber"); }
            set { base.SetVal("RegisterNumber", value); }
        }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterDate
        {
            get { return base.GetValDateTime("RegisterDate"); }
            set { base.SetVal("RegisterDate", value); }
        }

        /// <summary>
        /// 资信等级
        /// </summary>
        public string FinancingLevel
        {
            get { return base.GetValStr("FinancingLevel"); }
            set { base.SetVal("FinancingLevel", value); }
        }

        /// <summary>
        /// 注册资金
        /// </summary>
        public string RegisterFinancing
        {
            get { return base.GetValStr("RegisterFinancing"); }
            set { base.SetVal("RegisterFinancing", value); }
        }

        /// <summary>
        /// 质保等级
        /// </summary>
        public string QuilityLevel
        {
            get { return base.GetValStr("QuilityLevel"); }
            set { base.SetVal("QuilityLevel", value); }
        }

        /// <summary>
        /// 公司税号
        /// </summary>
        public string TaxNumber
        {
            get { return base.GetValStr("TaxNumber"); }
            set { base.SetVal("TaxNumber", value); }
        }

        /// <summary>
        /// 所在国家
        /// </summary>
        public string Country
        {
            get { return base.GetValStr("Country"); }
            set { base.SetVal("Country", value); }
        }

        /// <summary>
        /// 制造许可证
        /// </summary>
        public string MadeLicense
        {
            get { return base.GetValStr("MadeLicense"); }
            set { base.SetVal("MadeLicense", value); }
        }

        /// <summary>
        /// 设计许可证
        /// </summary>
        public string DegisnLicense
        {
            get { return base.GetValStr("DegisnLicense"); }
            set { base.SetVal("DegisnLicense", value); }
        }

        /// <summary>
        /// 单位类别
        /// </summary>
        public string OrganizationKind
        {
            get { return base.GetValStr("OrganizationKind"); }
            set { base.SetVal("OrganizationKind", value); }
        }

        /// <summary>
        /// QualificationsCertificateYearCheckDate(资质证书年审日期)
        /// </summary>
        public DateTime QCYCDate
        {
            get { return base.GetValDateTime("QCYCDate"); }
            set { base.SetVal("QCYCDate", value); }
        }

        /// <summary>
        /// QualificationsCertificateNumber(资质证书号码)
        /// </summary>
        public string QCNum
        {
            get { return base.GetValStr("QCNum"); }
            set { base.SetVal("QCNum", value); }
        }

        /// <summary>
        /// CompanyLicenseYearCheckDate
        /// </summary>
        public DateTime CLYCDate
        {
            get { return base.GetValDateTime("CLYCDate"); }
            set { base.SetVal("CLYCDate", value); }
        }

        /// <summary>
        /// 业务联系人
        /// </summary>
        public string ContactPerson
        {
            get { return base.GetValStr("ContactPerson"); }
            set { base.SetVal("ContactPerson", value); }
        }

        /// <summary>
        /// 关联部门
        /// </summary>
        public string RelateDeptment
        {
            get { return base.GetValStr("RelateDeptment"); }
            set { base.SetVal("RelateDeptment", value); }
        }

        /// <summary>
        /// 供货范围
        /// </summary>
        public string SupplerRange
        {
            get { return base.GetValStr("SupplerRange"); }
            set { base.SetVal("SupplerRange", value); }
        }

        /// <summary>
        /// 业绩
        /// </summary>
        public string Achievement
        {
            get { return base.GetValStr("Achievement"); }
            set { base.SetVal("Achievement", value); }
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