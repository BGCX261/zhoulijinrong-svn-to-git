//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：流程信息
// 
// 创建标识：2010-1-12 王敏贤
//
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------

using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.AutoGene
{
    /// <summary>
    /// 流程类型
    /// </summary>
    public abstract class GeneFlow : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_Flow";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneFlow()
            : base(GeneFlow.TableName)
        {
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        protected override void InitColumnSelf()
        {
            base.InitColumn("Name", "角色名", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name
        {
            get { return base.GetValStr("Name"); }
            set { base.SetVal("Name", value); }
        }

        #endregion
    }
}
