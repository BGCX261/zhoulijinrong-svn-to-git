//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：取用户信息
//
// 
// 创建标识：ZhangXueling  2009-12-29
//
// 修改标识：2010-02-08
// 修改描述：去除了没有用到的接口方法
//
// 修改标识：2010-02-26
// 修改描述：取消了GetUserByDeptPost()接口中PostSortNum的排序
//
// 修改标志：2010-03-04
// 修改描述：取消了FilterUser()方法中域名为空的判断，
//           修改了GetUserName()方法中缓存中取值的方式，VB子查询
//
// 修改标志：2010-03-05
// 修改描述：修改了GetUserName()方法中从缓存读取数据，
//
// 修改标识：2010-04-15
// 修改描述：修改了GetSupUserByPost()接口增加了层级参数
//           重载了两个GetUserByDeptPost()接口增加了层级参数
//
// 修改标识：2010-04-16
// 修改描述：修改了GetSupUserByPost()接口内部方法
//
//----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FounderSoftware.Framework.Business;
using FounderSoftware.ADIM.OU.BLL;

namespace FS.ADIM.OU.OutBLL
{
    /// <summary>
    /// 根据不同的条件获取人员的信息
    /// </summary>
    public class OAUser
    {
        #region 读取域名

        private static String strDomain = FS.OA.Framework.OAConfig.GetConfig("数据库", "domain").ToString();

        //private static String strDomain = "hnpc";

        #endregion

        #region 根据用户账号得到用户的信息

        /// <summary>
        /// 根据用户账号得到用户的信息
        /// </summary>
        /// <param name="strUserID">用户账号</param>
        /// <returns>User</returns>
        public static User GetUser(String strUserID)
        {
            User user = null;
            if (!String.IsNullOrEmpty(strUserID))
            {
                int intInex = strUserID.IndexOf("\\");
                if (intInex != -1)
                {
                    strUserID = strUserID.Substring(intInex + 1);
                }
                user = User.GetUser(strUserID);
                if (user != null)
                {
                    if (String.Equals(user.Domain, strDomain) == false)
                    {
                        user = null;
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// 根据用户账号得到用户的姓名
        /// </summary>
        /// <param name="strUserID">用户账号</param>
        /// <returns>String</returns>
        public static String GetUserName(String strUserID)
        {
            User user = OAUser.GetUser(strUserID);
            return user != null ? user.Name : String.Empty;
        }

        #endregion

        #region 根据角色取人

        /// <summary>
        /// 根据角色的名称获取用户信息(支持格式：“处级领导,科级领导”)
        /// </summary>
        /// <param name="strRoleName">角色名称</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetUserByRole(String strRoleName)
        {
            return OAUser.FilterUser(User.GetUserByRoles(strRoleName));
        }

        /// <summary>
        /// 根据角色取人返回一个二维数组并转化为小写(数组0：用户域账号包括域名以“；”分隔  数组1：用户姓名以“；”分隔)
        /// </summary>
        /// <param name="strRoleName">角色名称</param>
        /// <returns>字符串数组</returns>
        public static String[] GetUserByRoleName(String strRoleName)
        {
            return OAUser.GetUserArray(OAUser.GetUserByRole(strRoleName));
        }

        /// <summary>
        /// 根据角色的名称获取用户信息绑定到DropDownList
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strRoleName">角色名称</param>
        public static void GetUserByRole(DropDownList ddl, String strRoleName)
        {
            OAUser.BindDropDownList(ddl, OAUser.GetUserByRole(strRoleName));
        }

        #endregion

        #region 根据部门ID或名称获取部门人员信息

        /// <summary>
        /// 根据部门ID获取部门人员信息
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">向下遍历的层级(0.自己 ；-1.所有)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetUserByDeptID(String strDeptID, int iFloorCode)
        {
            ViewBase vb = null;
            int index = OADept.ConvertInt(strDeptID);
            if (index != int.MinValue)
            {
                Department dept = OADept.GetDeptByDeptID(strDeptID);
                if (dept != null)
                {
                    vb = OAUser.FilterUser(dept.GetChildDeptUsers(iFloorCode));
                }
                vb.Sort = "e.SortNum,a.Userid";
            }
            return vb;
        }

        /// <summary>
        /// 根据部门ID获取部门人员信息绑定到DropDownList
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">向下遍历的层级(0.自己 ；-1.所有)</param>
        public static void GetUserByDeptID(DropDownList ddl, String strDeptID, int iFloorCode)
        {
            OAUser.BindDropDownList(ddl, OAUser.GetUserByDeptID(strDeptID, iFloorCode));
        }

        #endregion

        #region 根据部门ID或名称和角色取人员信息

        /// <summary>
        /// 根据部门的ID和角色的名称获取用户的信息 
        /// </summary>
        /// <param name="strDeptID"> 部门ID</param>
        /// <param name="strRoleName">角色名称</param>
        /// <param name="iFloorCode">向下遍历的层级(0.自己 ；-1.所有)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetUserByDeptIDRole(String strDeptID, String strRoleName, int iFloorCode)
        {
            ViewBase vb = null;
            if (String.IsNullOrEmpty(strDeptID) == false && String.IsNullOrEmpty(strRoleName) == false)
            {
                Role role = Role.GetRole(strRoleName);
                if (role != null)
                {
                    vb = OAUser.FilterUser(role.GetUserByDept(OADept.ConvertInt(strDeptID)));
                }
            }
            return vb;
        }

        #endregion

        #region 根据部门ID获取部门负责人

        /// <summary>
        ///  根据部门ID获取部门负责人
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetDeptManager(String strDeptID, int iFloorCode)
        {
            ViewBase vb = null;
            Department dept = Department.GetDepartment(OADept.ConvertInt(strDeptID));
            if (dept != null)
            {
                vb = OAUser.FilterUser(dept.GetManager(iFloorCode));
            }
            return vb;
        }

        /// <summary>
        /// 根据部门ID获取部门负责人绑定到DropdownList
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        public static void GetDeptManagerByDeptID(DropDownList ddl, string strDeptID, int iFloorCode)
        {
            OAUser.BindDropDownList(ddl, GetDeptManager(strDeptID, iFloorCode));
        }

        /// <summary>
        ///  根据部门ID获取部门负责人(支持多个id的传入以逗号分隔)
        /// </summary>
        /// <param name="strDeptIDS">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        /// <returns> 泛型ViewBase</returns>
        private static List<ViewBase> GetDeptManagers(String strDeptIDS, int iFloorCode)
        {
            List<ViewBase> listVB = new List<ViewBase>();
            if (String.IsNullOrEmpty(strDeptIDS) == false)
            {
                strDeptIDS = strDeptIDS.Replace(';', ',');
                String[] strIDS = strDeptIDS.Split(',');
                if (strIDS.Length > 0)
                {
                    foreach (String IDS in strIDS)
                    {
                        listVB.Add(OAUser.GetDeptManager(IDS, iFloorCode));
                    }
                }
            }
            return listVB;
        }

        /// <summary>
        ///  根据部门ID获取部门负责人的账号和姓名（0：领导账号集合的字符串，1：姓名的字符串）
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        /// <returns>字符串数组</returns>
        public static String[] GetDeptManagerArray(String strDeptID, int iFloorCode)
        {
            return OAUser.GetUserArray(OAUser.GetDeptManager(strDeptID, iFloorCode));
        }

        /// <summary>
        ///  根据部门ID获取部门负责人的账号和姓名(支持多个id的传入以逗号分隔)（0：领导账号集合的字符串，1：姓名的字符串）
        /// </summary>
        /// <param name="strDeptIDS">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        /// <returns>字符串数组</returns>
        public static String[] GetDeptManagerArrays(String strDeptIDS, int iFloorCode)
        {
            return OAUser.GetUserArray(OAUser.GetDeptManagers(strDeptIDS, iFloorCode));
        }

        #endregion

        #region 根据部门ID获取部门领导

        /// <summary>
        /// 根据部门ID获取部门领导
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetDeptLeader(String strDeptID, int iFloorCode)
        {
            ViewBase vb = null;
            Department dept = Department.GetDepartment(OADept.ConvertInt(strDeptID));
            if (dept != null)
            {
                vb = OAUser.FilterUser(dept.GetLeaders(iFloorCode));
            }
            return vb;
        }

        /// <summary>
        /// 根据部门ID获取部门领导账号和姓名的数组（0：领导账号集合的字符串，1：姓名的字符串）
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        /// <returns>字符串数组</returns>
        public static String[] GetDeptLeaderArray(String strDeptID, int iFloorCode)
        {
            return OAUser.GetUserArray(OAUser.GetDeptLeader(strDeptID, iFloorCode));
        }

        /// <summary>
        /// 根据部门ID获取部门领导
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0.自己；-1.所有)</param>
        public static void GetDeptLeader(DropDownList ddl, String strDeptID, int iFloorCode)
        {
            OAUser.BindDropDownList(ddl, OAUser.GetDeptLeader(strDeptID, iFloorCode));
        }


        #endregion

        #region 根据部门的ID或名称和职位名称取人

        /// <summary>
        /// 根据部门的ID和职位名称取人支持多个职位以逗号分隔
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="iFloorCode">层级</param>
        /// <returns >ViewBase</returns>
        public static ViewBase GetUserByPost(string strDeptID, string strPostName, int iFloorCode)
        {
            ViewBase vb = null;
            int deptID = OADept.ConvertInt(strDeptID);
            if (deptID != int.MinValue && string.IsNullOrEmpty(strPostName) == false)
            {
                Department dept = Department.GetDepartment(deptID);
                if (dept != null)
                {
                    vb = OAUser.FilterUser(dept.GetUserByPostNames(strPostName, iFloorCode));
                }
            }
            return vb;
        }

        /// <summary>
        ///  根据部门的ID和职位名称取人支持多个职位以逗号分隔
        /// </summary>
        /// <param name="strDeptID">部门名称</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="iFloorCode">层级</param>
        /// <returns>string二维数组（0：用户账号包括域名 1：用户姓名）</returns>
        public static string[] GetUserByDeptPostArray(string strDeptID, string strPostName, int iFloorCode)
        {
            return OAUser.GetUserArray(OAUser.GetUserByPost(strDeptID, strPostName, iFloorCode));
        }

        /// <summary>
        /// 根据部门的ID和职位名称取人支持多个职位以逗号分隔
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="iFloorCode">层级</param>
        public static void GetUserByPost(DropDownList ddl, string strDeptID, string strPostName, int iFloorCode)
        {
            OAUser.BindDropDownList(ddl, GetUserByPost(strDeptID, strPostName, iFloorCode));
        }

        #endregion

        #region 根据部门的ID和职位名称取出大于这个职位的人

        /// <summary>
        ///  根据部门的ID和职位名称取出大于这个职位的人(向下遍历)
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="iFloorCode">层级(0自己,>0 子部门层数,-1所有)</param>
        /// <returns >ViewBase</returns>
        private static ViewBase GetSupUserByPost(string strDeptID, string strPostName, int iFloorCode)
        {
            ViewBase vb = null;
            Department dept = Department.GetDepartment(OADept.ConvertInt(strDeptID));
            if (dept != null)
            {
                vb = OAUser.FilterUser(dept.GetUserByPosition(strPostName, iFloorCode));
            }
            return vb;
        }

        #endregion

        #region 获得大于某个职位的职位信息

        /// <summary>
        /// 获得大于某个职位的职位信息
        /// </summary>
        /// <param name="strPostName"></param>
        /// <returns>ViewBase</returns>
        private static ViewBase GetSupPost(string strPostName)
        {
            return Position.GetPositions(strPostName);
        }

        /// <summary>
        /// 获得大于某个职位的职位名称数组
        /// </summary>
        /// <param name="strPostName">职位名称</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetSupPostNameAarray(string strPostName)
        {
            ArrayList arrNames = new ArrayList();
            ViewBase vb = OAUser.GetSupPost(strPostName);
            if (vb != null && vb.Count > 0)
            {
                foreach (Position post in vb.Ens)
                {
                    arrNames.Add(post.Name);
                }
            }
            return arrNames;
        }

        #endregion

        #region 根据部门ID绑定部门负责人领导和大于某职位的人

        /// <summary>
        /// 根据部门ID取出部门负责人、部门领导和大于某职位的人(向下遍历)
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="bDeptManger">是否绑定部门负责人</param>
        /// <param name="bDeptLeader">是否绑定部门领导</param>
        /// <param name="iFloorCode">层级(0自己,>0 子部门层数,-1所有)</param>
        /// <returns>DataTable</returns>
        private static ViewBase GetUserByDeptPost(string strDeptID, string strPostName, bool bDeptManger, bool bDeptLeader, int iFloorCode)
        {
            Common.LeaderType enmuType = Common.LeaderType.User;
            if (bDeptLeader && bDeptManger)
            {
                enmuType = Common.LeaderType.LeaderAndManager;
            }
            if (bDeptManger == false && bDeptLeader)
            {
                enmuType = Common.LeaderType.Leader;
            }
            if (bDeptManger && bDeptLeader == false)
            {
                enmuType = Common.LeaderType.Manager;
            }
            if (bDeptLeader == false && bDeptManger == false)
            {
                enmuType = Common.LeaderType.User;
            }
            return OAUser.GetUserByDeptPost(strDeptID, strPostName, enmuType, iFloorCode);
        }

        /// <summary>
        /// 根据部门ID绑定部门负责人,部门领导和大于某职位的人
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="bDeptManger">是否绑定部门负责人</param>
        /// <param name="bDeptLeader">是否绑定部门领导</param>
        /// <returns>返回以分号分隔的二维字符串数组</returns>
        public static string[] GetUserByDeptPostArray(string strDeptID, string strPostName, bool bDeptManger, bool bDeptLeader)
        {
            return OAUser.GetUserArray(OAUser.GetUserByDeptPost(strDeptID, strPostName, bDeptManger, bDeptLeader, 0));
        }

        /// <summary>
        /// 根据部门ID绑定部门负责人、部门领导和大于某职位的人(可以传空值)
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="bDeptManger">是否绑定部门负责人</param>
        /// <param name="bDeptLeader">是否绑定部门领导</param>
        public static void GetUserByDeptPost(DropDownList ddl, string strDeptID, string strPostName, bool bDeptManger, bool bDeptLeader)
        {
            OAUser.BindDropDownList(ddl, OAUser.GetUserByDeptPost(strDeptID, strPostName, bDeptManger, bDeptLeader, 0));
        }

        /// <summary>
        /// 根据部门ID绑定部门负责人、部门领导和大于某职位的人(可以传空值)
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPostName">职位名称</param>
        /// <param name="bDeptManger">是否绑定部门负责人</param>
        /// <param name="bDeptLeader">是否绑定部门领导</param>
        /// <param name="iFloorCode">层级(0自己,>0 子部门层数,-1所有)</param>
        public static void GetUserByDeptPost(DropDownList ddl, string strDeptID, string strPostName, bool bDeptManger, bool bDeptLeader, int iFloorCode)
        {
            OAUser.BindDropDownList(ddl, OAUser.GetUserByDeptPost(strDeptID, strPostName, bDeptManger, bDeptLeader, iFloorCode));
        }

        /// <summary>
        /// 根据部门ID取出部门负责人、部门领导和大于某职位的人
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strPost">职位名称</param>
        /// <param name="enmuType">职务类型枚举</param>
        /// <param name="iFloorCode">层级(0自己,>0 子部门层数,-1所有)</param>
        /// <returns></returns>
        private static ViewBase GetUserByDeptPost(string strDeptID, string strPost, Common.LeaderType enmuType, int iFloorCode)
        {
            ViewBase vb = null;
            Department dept = Department.GetDepartment(OADept.ConvertInt(strDeptID));
            if (dept != null)
            {
                vb = OAUser.FilterUser(dept.GetManagerOrLeader(iFloorCode, enmuType, strPost));
            }
            return vb;
        }

        #endregion

        #region  绑定DropDownList控件的方法

        /// <summary>
        ///  绑定DropDownList控件的方法
        /// </summary>
        /// <param name="ddl">控件的ID</param>
        /// <param name="vb">需要绑定的表的视图</param>
        private static void BindDropDownList(DropDownList ddl, ViewBase vb)
        {
            if (!(vb.BaseCondition.Contains("公司领导"))) //杨子江 20110721
            {
                ddl.Items.Clear();
            }
            ddl.Items.Add(new ListItem());
            if (vb != null)
            {
                foreach (User user in vb.Ens)
                {
                    ListItem Item = new ListItem(user.Name, user.DomainUserID);
                    if (!ddl.Items.Contains(Item)) //重复的不加载
                    {
                        ddl.Items.Add(Item);
                    }
                }
            }
        }

        ///// <summary>
        /////  绑定DropDownList控件的方法
        ///// </summary>
        ///// <param name="ddl">控件的ID</param>
        ///// <param name="dt">需要绑定的表</param>
        //private static void BindDropDownList(DropDownList ddl, DataTable dt)
        //{
        //    ddl.Items.Clear();
        //    ddl.Items.Add(new ListItem());
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            ListItem Item = new ListItem(dr["Name"].ToString(), (dr["Domain"] + "\\" + dr["Userid"]).ToString());
        //            if (!ddl.Items.Contains(Item)) //重复的不加载
        //            {
        //                ddl.Items.Add(Item);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region 获得所有用户

        /// <summary>
        /// 获得所有用户
        /// </summary>
        /// <returns>ViewBase</returns>
        private static ViewBase GetAllUser()
        {
            ViewBase vb = User.GetAllUser();
            if (vb != null)
            {
                vb.Field = "a.ID,a.UserID,a.Name,a.Domain";
                vb.Sort = "a.ID";
                vb = OAUser.FilterUser(vb);
            }
            return vb;
        }

        #endregion

        #region 返回用户账号和姓名字符串数组

        /// <summary>
        /// 返回用户账号和姓名字符串数组(数组0：用户域账号包括域名以“；”分隔  数组1：用户姓名以“；”分隔)
        /// </summary>
        /// <param name="vb"></param>
        /// <returns></returns>
        private static string[] GetUserArray(ViewBase vb)
        {
            string[] strUser = new string[2];
            string strIDS = string.Empty;
            string strNames = string.Empty;
            if (vb != null)
            {
                foreach (User user in vb.Ens)
                {
                    strIDS += user.DomainUserID + ";";
                    strNames += user.Name + ";";
                }
            }
            strUser[0] = OAUser.FilterRepeat(strIDS);
            strUser[1] = OAUser.FilterRepeat(strNames);
            return strUser;
        }

        /// <summary>
        /// 返回用户账号和姓名字符串数组(数组0：用户域账号包括域名以“；”分隔  数组1：用户姓名以“；”分隔)
        /// </summary>
        /// <param name="listvb">泛型ViewBase的集合</param>
        /// <returns>string[]</returns>
        private static string[] GetUserArray(List<ViewBase> listvb)
        {
            string[] strUser = new string[2];
            string strIDS = string.Empty;
            string strNames = string.Empty;
            foreach (ViewBase vb in listvb)
            {
                if (vb != null)
                {
                    foreach (User user in vb.Ens)
                    {
                        strIDS += user.DomainUserID + ";";
                        strNames += user.Name + ";";
                    }
                }
            }

            strUser[0] = OAUser.FilterRepeat(strIDS);
            strUser[1] = OAUser.FilterRepeat(strNames);
            return strUser;
        }

        #endregion

        #region 过滤非域名和注销用户

        /// <summary>
        /// 过滤非域名用户和注销用户
        /// </summary>
        /// <param name="vb">视图基类对象</param>
        /// <returns>ViewBase</returns>
        private static ViewBase FilterUser(ViewBase vb)
        {
            if (vb != null)
            {
                vb.Condition = "a.domain=" + "'" + strDomain + "'" + " and a.IsCancel=1";
            }
            return vb;
        }

        #endregion

        #region 过滤重复字符串和空,多个分号分隔,并转化成小写

        /// <summary>
        /// 过滤重复字符串和空,多个分号分隔,并转化成小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>string</returns>
        private static string FilterRepeat(string str)
        {
            string strStr = string.Empty;
            if (string.IsNullOrEmpty(str) == false)
            {
                str = str.Replace(',', ';');
                string[] strArr = str.Split(';');
                ArrayList arrNames = new ArrayList();
                foreach (string s in strArr)
                {
                    if (arrNames.IndexOf(s.ToLower()) == -1)
                    {
                        if (s != string.Empty)
                            arrNames.Add(s.ToLower());
                    }
                }
                if (arrNames.Count > 0)
                {
                    foreach (object o in arrNames)
                    {
                        strStr += ";" + o.ToString();
                    }
                    strStr = strStr.Substring(1);
                }
            }
            return strStr;
        }

        #endregion
    }
}
