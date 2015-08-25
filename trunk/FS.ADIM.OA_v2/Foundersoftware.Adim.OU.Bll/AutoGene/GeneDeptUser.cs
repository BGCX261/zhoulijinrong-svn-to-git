//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：部门用户信息
// 
// 创建标识：2009-12-2 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.AutoGene
{
    /// <summary>
    /// 部门用户信息
    /// </summary>
    public class GeneDeptUser : EntityMaster
    {
        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneDeptUser()
            : base(GeneDeptPost.TableName)
        {
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        protected override void InitColumnSelf()
        {
            base.InitColumn("Name", "Name", true);
            base.InitColumn("UserID", "UserID", true);
            base.InitColumn("Domain", "Domain", true);
            base.InitColumn("FK_DeptID", "FK_DeptID", true);
            base.InitColumn("FK_UserID", "FK_UserID", true);
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
        /// 用户帐号
        /// </summary>
        public string UserID
        {
            get { return base.GetValStr("UserID"); }
            set { base.SetVal("UserID", value); }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public string Domain
        {
            get { return base.GetValStr("Domain"); }
            set { base.SetVal("Domain", value); }
        }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string FK_DeptID
        {
            get { return base.GetValStr("FK_DeptID"); }
            set { base.SetVal("FK_DeptID", value); }
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