//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：角色用户信息
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
    /// 角色用户信息
    /// </summary>
    public class GeneRoleUser : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_RoleUser";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneRoleUser()
            : base(GeneRoleUser.TableName)
        {
        }

        /// <summary>
        /// 初始化视图列 true显示 false不显示
        /// </summary>
        protected override void InitColumnSelf()
        {
            base.InitColumn("FK_RoleID", "FK_RoleID", true);
            base.InitColumn("FK_UserID", "FK_UserID", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 角色ID
        /// </summary>
        public int FK_RoleID
        {
            get { return base.GetValInt("FK_RoleID"); }
            set { base.SetVal("FK_RoleID", value); }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int FK_UserID
        {
            get { return base.GetValInt("FK_UserID"); }
            set { base.SetVal("FK_UserID", value); }
        }

        #endregion
    }
}