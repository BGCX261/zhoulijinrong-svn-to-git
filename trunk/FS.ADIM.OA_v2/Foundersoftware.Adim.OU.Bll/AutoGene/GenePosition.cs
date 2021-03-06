//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：职位信息
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
    /// 职位信息
    /// </summary>
    public abstract class GenePosition : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_Post";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GenePosition()
            : base(GenePosition.TableName)
        {
        }

        /// <summary>
        /// 初始化视图列 true显示 false不显示
        /// </summary>
        protected sealed override void InitColumnSelf()
        {
            base.InitColumn("Name", "职位名", true);
            base.InitColumn("SortNum", "显示顺序", true);
            base.InitColumn("Remark", "备注", true);
            base.InitColumn("MaxSortNum", "序号上限", true);
            base.InitColumn("MinSortNum", "序号下限", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 职位名
        /// </summary>
        public string Name
        {
            get { return base.GetValStr("Name"); }
            set { base.SetVal("Name", value); }
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
        /// 显示顺序
        /// </summary>
        public int MaxSortNum
        {
            get { return base.GetValInt("MaxSortNum"); }
            set { base.SetVal("MaxSortNum", value); }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int MinSortNum
        {
            get { return base.GetValInt("MinSortNum"); }
            set { base.SetVal("MinSortNum", value); }
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