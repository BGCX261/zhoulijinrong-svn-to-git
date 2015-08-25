//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：取部门信息
//
// 
// 创建标识：ZhangXueling  2009-12-29
//
// 修改标识：2010-02-08
// 修改描述：去除了没有用到的接口方法
//
// 修改标识：2010-03-05
// 修改描述：修改了GetDeptName()接口去除了内部的缓存
//----------------------------------------------------------------

using System;
using FounderSoftware.Framework.Business;
using FounderSoftware.ADIM.OU.BLL.Busi;
using System.Web.UI.WebControls;
using System.Collections;

namespace FS.ADIM.OU.OutBLL
{
    /// <summary>
    /// 根据各种条件获取部门信息
    /// </summary>
    public class OADept
    {
        #region 根据部门的ID或名称获取部门信息
        /// <summary>
        /// 根据部门的ID获取部门信息
        /// </summary>
        /// <param name="strDeptID">部门的ID</param>
        /// <returns>Department</returns>
        public static Department GetDeptByDeptID(string strDeptID)
        {
            int iDeptID = ConvertInt(strDeptID);
            return iDeptID != int.MinValue ? Department.GetDepartment(iDeptID) : null;
        }

        /// <summary>
        /// 根据部门的ID返回部门的名称
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <returns>部门名称</returns>
        public static string GetDeptName(string strDeptID)
        {
            Department dept = OADept.GetDeptByDeptID(strDeptID);
            return dept != null ? dept.Name : string.Empty;
        }
        #endregion

        #region 根据处室部门的ID和用户账号找出用户所属该处室下的科室
        /// <summary>
        /// 根据处室部门的ID和用户账号找出用户所属该处室下的科室
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="strUserID">用户账号</param>
        /// <param name="iFloorCode">层级(1.处室，2.科室……)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetDeptByDeptUser(string strDeptID, string strUserID, int iFloorCode)
        {
            ViewBase vb = null;
            if (string.IsNullOrEmpty(strUserID) == false)
            {
                vb = OADept.GetDeptByUser(strUserID, iFloorCode);
                if (string.IsNullOrEmpty(strDeptID) == false && OADept.ConvertInt(strDeptID) != int.MinValue)
                {
                    if (vb != null)
                    {
                        vb.Condition = "a.ParentID=" + OADept.ConvertInt(strDeptID);
                    }
                }
            }
            return vb;
        }
        #endregion

        #region 根据部门的ID获取子部门
        /// <summary>
        /// 根据部门的ID获取子部门
        /// </summary>
        /// <param name="strDeptID">部门的ID</param>
        /// <param name="iFloorCode" >层级(>0 子部门层数, -1所有)</param>
        /// <returns>ViewBase</returns>
        private static ViewBase GetChildDept(string strDeptID, int iFloorCode)
        {
            ViewBase vb = null;
            Department dept = Department.GetDepartment(ConvertInt(strDeptID));
            if (dept != null)
            {
                vb = dept.GetChildDepts(iFloorCode);
            }
            return vb;
        }

        /// <summary>
        ///  根据部门的ID获取子部门包括自己支持多个部门的ID以","分隔
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(>0 子部门层数, -1所有)</param>
        /// <returns>ViewBase</returns>
        private static ViewBase GetChildDeptsConSelf(string strDeptID, int iFloorCode)
        {
            ViewBase vb = null;
            if (string.IsNullOrEmpty(strDeptID) == false)
            {
                vb = Department.GetChildDeptsConSelf(strDeptID, iFloorCode);
            }
            return vb;
        }

        /// <summary>
        /// 根据部门的ID获取子部门ID字符串包括自己以","分隔
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0:自己 -1：所有 >0:层数)</param>
        /// <returns>String</returns>
        public static String GetChildDeptIDSConSelf(string strDeptID, int iFloorCode)
        {
            ViewBase vb = null;
            string strIDS = string.Empty;
            vb = OADept.GetChildDeptsConSelf(strDeptID, iFloorCode);
            if (vb != null)
            {
                strIDS = vb.GetFieldVals("ID", ",");
            }
            else
            {
                strIDS = strDeptID;
            }
            return strIDS;
        }

        /// <summary>
        /// 根据部门的ID获取子部门ID字符串以","分隔
        /// </summary>
        /// <param name="strDeptID">部门ID</param>
        /// <param name="iFloorCode">层级(0:自己 -1：所有 >0:层数)</param>
        /// <returns>String</returns>
        public static String GetChildDeptIDString(string strDeptID, int iFloorCode)
        {
            string strIDS = string.Empty;
            ViewBase vb = OADept.GetChildDept(strDeptID, iFloorCode);
            if (vb != null)
            {
                strIDS = vb.GetFieldVals("ID", ",");
            }
            return strIDS;
        }

        /// <summary>
        /// 根据部门的ID获取子部门
        /// </summary>
        /// <param name="ddl" >下拉菜单ID</param >
        /// <param name="strDeptID">部门的ID</param>
        /// <param name="iFloorCode" >层级(>0 子部门层数, -1所有)</param>
        public static void GetChildDept(DropDownList ddl, string strDeptID, int iFloorCode)
        {
            OADept.BindDropDownList(ddl, OADept.GetChildDept(strDeptID, iFloorCode));
        }

        /// <summary>
        /// 根据部门的ID获取子部门
        /// </summary>
        /// <param name="ddl" >下拉菜单ID</param >
        /// <param name="strDeptID">部门的ID</param>
        /// <param name="iFloorCode" >层级(>0 子部门层数, -1所有)</param>
        /// <param name="bClear" >是否清除(true:清除，false :追加)</param>
        /// <param name="bBlank" >是否加空行(true:加空行 false:反之)</param>
        public static void GetChildDept(DropDownList ddl, string strDeptID, int iFloorCode, bool bClear, bool bBlank)
        {
            OADept.BindDropDownList(ddl, OADept.GetChildDept(strDeptID, iFloorCode), bClear, bBlank);
        }

        #endregion

        #region 获取指定层级的部门信息

        /// <summary>
        /// 获取指定层级的部门信息
        /// </summary>
        /// <param name="iFloorCode">层级(1.处级部门；2.科级部门；-1.所有部门)</param>
        /// <returns>ViewBase</returns>
        private static ViewBase GetDeptByIfloor(int iFloorCode)
        {
            return Department.GetDeptsByFCode(iFloorCode);
        }

        /// <summary>
        /// 获取指定层级的部门信息绑定到DropDownList
        /// </summary>
        /// <param name="ddl" >下拉菜单ID</param>
        /// <param name="iFloorCode">层级(1.处级部门；2.科级部门；-1.所有部门)</param>
        public static void GetDeptByIfloor(DropDownList ddl, int iFloorCode)
        {
            OADept.BindDropDownList(ddl, OADept.GetDeptByIfloor(iFloorCode));
        }

        #endregion

        #region 根据用户账号获取他所在部门信息

        /// <summary>
        /// 根据用户账号获取他所在部门信息
        /// </summary>
        /// <param name="strUserID">用户账号</param>
        /// <param name="iFloorCode">层级(0.自己所在部门;1.处级部门；2.科级部门；-1.所有部门)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetDeptByUser(string strUserID, int iFloorCode)
        {
            ViewBase vb = null;
            User user = OAUser.GetUser(strUserID);
            if (user != null)
            {
                vb = user.GetChildOrParentDepts(iFloorCode);
            }
            return vb;
        }

        /// <summary>
        /// 根据用户账号获取他所在部门信息绑定到DropDownList
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strUserID">用户账号</param>
        /// <param name="iFloorCode">层级(0.自己所在部门;1.处级部门；2.科级部门；-1.所有部门)</param>
        /// <param name="bClear" >是否清除(true:清除，false :追加)</param>
        /// <param name="bBlank" >是否加空行(true:加空行 false:反之)</param>
        public static void GetDeptByUser(DropDownList ddl, string strUserID, int iFloorCode, bool bClear, bool bBlank)
        {
            OADept.BindDropDownList(ddl, OADept.GetDeptByUser(strUserID, iFloorCode), bClear, bBlank);
        }

        /// <summary>
        /// 根据用户账号获取他所在部门信息绑定到DropDownList
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strUserID">用户账号</param>
        /// <param name="iFloorCode">层级(0.自己所在部门;1.处级部门；2.科级部门；-1.所有部门)</param>
        /// <param name="bIsNeedBlank">是否需要空行</param>
        public static void GetDeptByUser(DropDownList ddl, string strUserID, int iFloorCode, bool bIsNeedBlank)
        {
            OADept.BindDropDownList(ddl, OADept.GetDeptByUser(strUserID, iFloorCode), bIsNeedBlank);
        }

        #endregion

        #region 获得所有单位信息

        /// <summary>
        /// 根据条件查询单位的信息(默认是所有单位的信息)
        /// </summary>
        /// <param name="condition">根据ddl的索引查询</param>
        /// <param name="txtContent">文本框的内容</param>
        /// <returns>视图对象ViewBase</returns>
        public static ViewBase GetCompany(int condition, string txtContent)
        {
            return Company.GetCompany(txtContent, condition);
        }

        #endregion

        #region 绑定对应部门到DropDownList

        /// <summary>
        /// 绑定对应部门到DropDownList
        /// </summary>
        /// <param name="ddl">下拉菜单ID</param>
        /// <param name="strDeptID">部门ID</param>
        public static void BindDeptByDeptID(DropDownList ddl, string strDeptID)
        {
            ddl.Items.Clear();
            if (string.IsNullOrEmpty(strDeptID) == false)
            {
                ddl.Items.Add(new ListItem(OADept.GetDeptName(strDeptID), strDeptID));
            }
        }

        #endregion

        #region  绑定DropDownList控件的方法

        /// <summary>
        /// 绑定DropDownList控件的方法（增加空行）
        /// </summary>
        /// <param name="ddl">控件的ID</param>
        /// <param name="vb">需要绑定的表的视图</param>
        private static void BindDropDownList(DropDownList ddl, ViewBase vb)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem());//增加空行
            if (vb != null)
            {
                foreach (Department dept in vb.Ens)
                {
                    ddl.Items.Add(new ListItem(dept.Name, dept.ID.ToString()));
                }
            }
        }

        /// <summary>
        ///  绑定DropDownList控件的方法
        /// </summary>
        /// <param name="ddl">控件的ID</param>
        /// <param name="vb">需要绑定的表的视图</param>
        /// <param name="bClear" >是否清除(true:清除，false :追加)</param>
        /// <param name="bBlank" >是否加空行(true:加空行 false:反之)</param>
        private static void BindDropDownList(DropDownList ddl, ViewBase vb, bool bClear, bool bBlank)
        {
            if (bClear)
            {
                ddl.Items.Clear();
            }
            if (bBlank)
            {
                ddl.Items.Add(new ListItem());
            }
            if (vb != null)
            {
                foreach (Department dept in vb.Ens)
                {
                    ListItem Item = new ListItem(dept.Name, dept.ID.ToString());
                    if (!ddl.Items.Contains(Item)) //重复的不加载
                    {
                        ddl.Items.Add(Item);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="vb"></param>
        /// <param name="bIsNeedBlank"></param>
        private static void BindDropDownList(DropDownList ddl, ViewBase vb, bool bIsNeedBlank)
        {
            ddl.Items.Clear();
            if (vb != null)
            {
                if (bIsNeedBlank)
                {
                    ddl.Items.Add(new ListItem());//增加空行
                }
                else
                {
                    if (vb.Count > 1)
                    {
                        ddl.Items.Add(new ListItem());//增加空行
                    }
                }
                foreach (Department dept in vb.Ens)
                {
                    ddl.Items.Add(new ListItem(dept.Name, dept.ID.ToString()));
                }
            }
        }

        #endregion

        #region 把String类型转换成Int类型

        /// <summary>
        /// 把String类型转换成Int类型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>int</returns>
        public static int ConvertInt(string str)
        {
            int iRuslt;
            return int.TryParse(str, out iRuslt) ? iRuslt : int.MinValue;
        }

        #endregion

        #region 获得所有的部门信息

        /// <summary>
        /// 获得所有部门的ID和名称
        /// </summary>
        /// <returns></returns>
        //private static ViewBase GetAllDeptIDAndName()
        //chen
        //{
        //    ViewBase vb = Department.GetAllDepartment();
        //    if (vb != null)
        //    {
        //        vb.Field = "Distinct a.ID,a.Name";
        //        vb.Sort = "a.id";
        //    }
        //    return vb;
        //}
        public static ViewBase GetAllDeptIDAndName()
        {
            ViewBase vb = Department.GetAllDepartment();
            if (vb != null)
            {
                vb.Field = "Distinct a.ID,a.Name";
                vb.Sort = "a.id";
            }
            return vb;
        }
        /// <summary>
        /// 获得小于某个层级的部门
        /// </summary>
        /// <param name="iFloorCode">层级(1.到处室，2.到科室…… -1.所有)</param>
        /// <returns>ViewBase</returns>
        public static ViewBase GetDeptInfo(int iFloorCode)
        {
            ViewBase vb = Department.GetAllDepartment();
            if (iFloorCode != -1)
            {
                if (vb != null)
                {
                    vb.Condition = "a.FloorCode<=" + iFloorCode;
                }
            }
            return vb;
        }

        #endregion

        #region 获取可传阅部门和交办部门的信息

        /// <summary>
        /// 根据用户的账号(不包含域名)获取可交办和传阅的部门的ID以","分隔
        /// </summary>
        /// <param name="strUserID">用户账号</param>
        /// <returns>string</returns>
        public static String GetDeptIDByUser(string strUserID)
        {
            return OADept.GetDeptIDByUser(strUserID, OUConstString.PostName.FUKEZHANG);
        }

        /// <summary>
        /// 根据用户的账号(不包含域名)获取可交办和传阅的部门的ID以","分隔
        /// </summary>
        /// <param name="strUserID">用户账号</param>
        /// <returns>string</returns>
        public static string GetDeptIDByUser2(string strUserID)
        {
            return OADept.GetDeptIDByUser(strUserID, OUConstString.PostName.YUANGONG);
        }

        /// <summary>
        /// 根据用户的账号(不包含域名)和职位名获取可交办和传阅的部门的ID以","分隔
        /// </summary>
        /// <param name="strUserID">用户账号</param>
        /// <param name="strPostName">职位名称</param>
        /// <returns></returns>
        private static string GetDeptIDByUser(string strUserID, string strPostName)
        {
            string strIDS = string.Empty;
            int iCount = 0;
            ViewBase vbDept = null;
            string strDeptIDS = string.Empty;
            ArrayList arrDepts = new ArrayList();
            ArrayList strPostNames = OAUser.GetSupPostNameAarray(strPostName);
            ViewBase vbCompanyLeaders = OAUser.GetUserByRole(OUConstString.RoleName.COMPANY_LEADER);
            if (vbCompanyLeaders != null && vbCompanyLeaders.Count > 0)
            {
                vbCompanyLeaders.Condition = "a.userid=" + "'" + strUserID + "'";
            }
            if (vbCompanyLeaders != null && vbCompanyLeaders.DtTable != null)
            {
                iCount = vbCompanyLeaders.DtTable.Rows.Count;
            }
            if (iCount == 0)
            {
                User user = OAUser.GetUser(strUserID);
                if (user != null)
                {
                    ViewBase vbDeptPost = user.DeptPosts;
                    if (vbDeptPost != null)
                    {
                        foreach (DeptPost deptpost in vbDeptPost.Ens)
                        {
                            if (deptpost.Dept != null && deptpost.Post != null)
                            {
                                if (strPostNames.Contains(deptpost.Post.Name))
                                {
                                    arrDepts.Add(deptpost.Dept.ID);
                                }
                            }
                        }
                    }
                    ViewBase vbDeptManger = user.ManagerDepts;
                    if (vbDeptManger != null)
                    {
                        foreach (Department dept in vbDeptManger.Ens)
                        {
                            arrDepts.Add(dept.ID);
                        }
                    }
                    ViewBase vbDeptLeader = user.LeaderDepts;
                    if (vbDeptLeader != null)
                    {
                        foreach (Department dept in vbDeptLeader.Ens)
                        {
                            arrDepts.Add(dept.ID);
                        }
                    }
                    strDeptIDS = OADept.GetStringFormatForArrayList(arrDepts, ",");
                    vbDept = OADept.GetChildDeptsConSelf(strDeptIDS, -1);
                    if (vbDept != null)
                    {
                        strIDS = vbDept.GetFieldVals("ID", ",");
                    }
                    else
                    {
                        strIDS = "-1";
                    }
                }
            }
            return strIDS;

        }

        #endregion

        #region 把ArrayList转换成以指定字符分隔的字符串

        /// <summary>
        /// 把ArrayList转换成以指定字符分隔的字符串
        /// </summary>
        /// <param name="arrList">字符串数组</param>
        /// <param name="split">分隔符</param>
        /// <returns>string</returns>
        private static string GetStringFormatForArrayList(ArrayList arrList, string split)
        {
            if (arrList.Count == 0)
            {
                return string.Empty;
            }
            string str = string.Empty;
            foreach (object o in arrList)
            {
                if (o.ToString() != string.Empty)
                {
                    str += split + o.ToString();
                }
            }
            if (str.Length > 0)
            {
                str = str.Substring(1);
            }
            return str;
        }

        #endregion

        /// <summary>
        /// 根据部门的Name获取部门信息
        /// </summary>
        /// <param name="strDeptName">部门的ID</param>
        /// <returns>Department</returns>
        public static Department GetDeptByDeptName(string strDeptName)
        {
            return Department.GetDepartment(strDeptName);
        }

        /// <summary>
        /// 根据部门的Name返回部门ID
        /// </summary>
        /// <param name="strDeptName">部门Name</param>
        /// <returns>部门名称</returns>
        public static string GetDeptID(string strDeptName)
        {
            Department dept = OADept.GetDeptByDeptName(strDeptName);
            return dept != null ? dept.ID.ToString() : string.Empty;
        }
    }
}
