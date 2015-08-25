//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：部门角色信息
// 
// 创建标识：2009-11-30 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.AutoGene
{
    /// <summary>
    /// 部门角色信息
    /// </summary>
    public class GeneDeptRole : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_RoleDeptPost";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneDeptRole()
            : base(GeneDeptRole.TableName)
        {
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        protected override void InitColumnSelf()
        {
            base.InitColumn("FK_RoleID", "FK_RoleID", true);
            base.InitColumn("FK_DeptID", "FK_DeptID", true);
            base.InitColumn("FK_PostID", "FK_PostID", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 部门ID
        /// </summary>
        public string FK_DeptID
        {
            get { return base.GetValStr("FK_DeptID"); }
            set { base.SetVal("FK_DeptID", value); }
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int FK_RoleID
        {
            get { return base.GetValInt("FK_RoleID"); }
            set { base.SetVal("FK_RoleID", value); }
        }

        /// <summary>
        /// 职位ID
        /// </summary>
        public string FK_PostID
        {
            get { return base.GetValStr("FK_PostID"); }
            set { base.SetVal("FK_PostID", value); }
        }

        #endregion
    }
}