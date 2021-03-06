//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：部门信息
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
    /// 部门信息
    /// </summary>
    public abstract class GeneDepartment : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_Department";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneDepartment()
            : base(GeneDepartment.TableName)
        {
        }

        /// <summary>
        /// 初始化视图列 true不显示 false不显示
        /// </summary>
        protected sealed override void InitColumnSelf()
        {
            base.InitColumn("Name", "部门名", true);
            base.InitColumn("ParentID", "上级部门ID，顶级为0", true);
            base.InitColumn("ParentName", "上级部门Name", false);
            base.InitColumn("DeptPath", "部门路径", true);
            base.InitColumn("FloorCode", "层号", true);
            base.InitColumn("SortNum", "显示顺序", true);
            base.InitColumn("Remark", "备注", true);
            base.InitColumn("IsCancel", "是否注销", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 部门名
        /// </summary>
        public string Name
        {
            get { return base.GetValStr("Name"); }
            set { base.SetVal("Name", value); }
        }

        /// <summary>
        /// 上级部门ID，顶级为0
        /// </summary>
        public int ParentID
        {
            get { return base.GetValInt("ParentID"); }
            set { base.SetVal("ParentID", value); }
        }

        /// <summary>
        /// 层号
        /// </summary>
        public int FloorCode
        {
            get { return base.GetValInt("FloorCode"); }
            set { base.SetVal("FloorCode", value); }
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
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return base.GetValStr("Remark"); }
            set { base.SetVal("Remark", value); }
        }

        /// <summary>
        /// 部门简称
        /// </summary>
        public string DeptSortName
        {
            get { return base.GetValStr("DeptPath"); }
            set { base.SetVal("DeptPath", value); }
        }

        /// <summary>
        /// 是否注销
        /// </summary>
        public bool IsCancel
        {
            get { return base.GetValBool("IsCancel"); }
            set { base.SetVal("IsCancel", value); }
        }

        #endregion
    }
}