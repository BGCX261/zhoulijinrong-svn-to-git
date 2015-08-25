using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

namespace FS.ADIM.OU.OutBLL
{
    /// <summary>
    /// 登录
    /// </summary>
    public class OALogin
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName">登录账号</param>
        /// <returns></returns>
        public static LoginUserInfo Login(string loginName)
        {
            LoginUserInfo info = new LoginUserInfo();
            DeptPost deptPost = new DeptPost();

            List<string> listRoleName = new List<string>();
            List<string> listRoleID = new List<string>();
            DataTable dtDeptPost = new DataTable();

            User user = User.GetUser(loginName);
            if (user != null)
            {
                info.ID = user.ID;
                info.LoginName = user.UserID;
                info.DisplayName = user.Name;
                info.UserName = user.DomainUserID;
                info.Domain = user.Domain;
                info.OfficePhone = user.OfficePhone;
                info.Email = user.Email;
                info.IsDel = user.IsCancel;

                ViewBase vbRole = user.GetRoles(false);
                if (vbRole.Count > 0)
                {
                    foreach (Role role in vbRole.Ens)
                    {
                        listRoleID.Add(role.ID.ToString());
                        listRoleName.Add(role.Name);
                    }
                }
                ViewBase vbDeptPost = user.DeptPosts;
                if (vbDeptPost != null)
                {
                    dtDeptPost = vbDeptPost.DtTable;
                }
                info.RoleName = listRoleName;
                info.RoleID = listRoleID;
                info.DeptPost = dtDeptPost;
            }

            return info;


        }

        /// <summary>
        /// 登陆用户获得当前用户信息
        /// </summary>
        public class LoginUserInfo
        {
            private int _ID = -1;//ID
            /// <summary>
            /// 主键
            /// </summary>
            public int ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            private String _LoginName = string.Empty;//用户帐号 不包括域名
            /// <summary>
            /// 用户帐号 不包括域名
            /// </summary>
            public String LoginName
            {
                get { return _LoginName; }
                set { _LoginName = value; }
            }


            private String _Domain = string.Empty;//域名
            /// <summary>
            /// 域名
            /// </summary>
            public String Domain
            {
                get { return _Domain; }
                set { _Domain = value; }
            }


            private String _UserName = string.Empty;//用户帐号 包括域名
            /// <summary>
            /// 用户帐号 包括域名
            /// </summary>
            public String UserName
            {
                get { return _UserName; }
                set { _UserName = value; }
            }


            private String _DisplayName = string.Empty;//显示名
            /// <summary>
            /// 显示名
            /// </summary>
            public String DisplayName
            {
                get { return _DisplayName; }
                set { _DisplayName = value; }
            }

            private String _OfficePhone = "";//办公电话
            /// <summary>
            /// 办公电话
            /// </summary>
            public String OfficePhone
            {
                get { return _OfficePhone; }
                set { _OfficePhone = value; }
            }

            private String _Email = "";//电子邮件

            /// <summary>
            /// 电子邮件
            /// </summary>
            public String Email
            {
                get { return _Email; }
                set { _Email = value; }
            }

            private bool _IsDel = false;//是否已被注销
            /// <summary>
            /// 是否已被注销
            /// </summary>
            public bool IsDel
            {
                get { return _IsDel; }
                set { _IsDel = value; }
            }


            /// <summary>
            /// 角色名称
            /// </summary>
            public List<string> RoleName
            {
                get;
                set;
            }

            /// <summary>
            /// 角色ID
            /// </summary>
            public List<string> RoleID
            {
                get;
                set;
            }

            /// <summary>
            /// 用户所在部门担当的职位
            /// </summary>
            public DataTable DeptPost
            {
                get;
                set;
            }


            private bool _IsOAAdmin = false;//是否是OA系统管理员

            /// <summary>
            /// 是否是OA系统管理员
            /// </summary>
            public bool IsOAAdmin
            {
                get { return _IsOAAdmin; }
                set { _IsOAAdmin = value; }
            }

            private bool _IsOUAdmin = false;//是否是OU系统管理员

            /// <summary>
            /// 是否是OU系统管理员
            /// </summary>
            public bool IsOUAdmin
            {
                get { return _IsOUAdmin; }
                set { _IsOUAdmin = value; }
            }
        }

    }
}
