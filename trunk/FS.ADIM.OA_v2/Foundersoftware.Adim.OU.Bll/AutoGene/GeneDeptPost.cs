//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：部门职位信息
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using FounderSoftware.Framework.Business;
using FounderSoftware.ADIM.OU.BLL.Busi;

namespace FounderSoftware.ADIM.OU.BLL.AutoGene
{
    /// <summary>
    /// 部门职位信息
    /// </summary>
    public class GeneDeptPost : EntityMaster
    {
        #region Define

        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "T_OU_DeptPostUser";

        #endregion

        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GeneDeptPost()
            : base(GeneDeptPost.TableName)
        {
        }

        /// <summary>
        /// 初始化列信息
        /// </summary>
        protected override void InitColumnSelf()
        {
            base.InitColumn("FK_UserID", "FK_UserID", true);
            base.InitColumn("FK_DeptID", "FK_DeptID", true);
            base.InitColumn("FK_PostID", "FK_PostID", true);
            base.InitColumn("LeaderType", "LeaderType", true);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 用户ID
        /// </summary>
        public int FK_UserID
        {
            get { return base.GetValInt("FK_UserID"); }
            set { base.SetVal("FK_UserID", value); }
        }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int FK_DeptID
        {
            get { return base.GetValInt("FK_DeptID"); }
            set { base.SetVal("FK_DeptID", value); }
        }
        
        /// <summary>
        /// 职位ID
        /// </summary>
        public int FK_PostID
        {
            get { return base.GetValInt("FK_PostID"); }
            set { base.SetVal("FK_PostID", value); }
        }

        /// <summary>
        /// 领导类型（部门领导、部门负责人等）
        /// </summary>
        public int LeaderType
        {
            get { return base.GetValInt("LeaderType"); }
            set { base.SetVal("LeaderType", value); }
        }

        /// <summary>
        /// 部门实体
        /// </summary>
        public User user
        {
            get
            {
                User user = User.GetUser(this.FK_UserID);
                return user;
            }
        }

        private string m_DupID;
        /// <summary>
        /// 人员维护 虚拟的主键
        /// </summary>
        public string DupID
        {
            get { return this.m_DupID; }
            set { this.m_DupID = value; }
        }

        private bool m__IsChecked;
        /// <summary>
        ///  人员维护选中状态,做删除用
        /// </summary>
        public bool IsChecked
        {
            get { return this.m__IsChecked; }
            set { this.m__IsChecked = value; }
        }

        #endregion
    }
}