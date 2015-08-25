//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述部门信息:上级部门;下级部门;部门用户、角色、职位;子部门领导;子部门负责人;
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
// 修改标识：2010-1-13 王敏贤
// 修改描述：获得当前部门大于某职位的人 GetUserByPosition();
//           根据职位ID获得当前部门某些职位的人,支持多个职位 GetUserByPostNames()
//
//----------------------------------------------------------------

using System;
using System.Data;
using FounderSoftware.ADIM.OU.BLL.AutoGene;
using FounderSoftware.ADIM.OU.BLL.View;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 描述部门信息:上级部门;下级部门;部门用户、角色、职位;子部门领导;子部门负责人;
    /// </summary>
    public class Department : GeneDepartment
    {
        #region Define

        private Department m_enParent = null;

        /// <summary>
        /// 部门角色视图
        /// </summary>
        private ViewBase m_vwDeptRole = new ViewDeptRole();

        #endregion

        #region 针对海南

        /// <summary>
        /// 根据职位名称获得当前部门大于该职位的人
        /// </summary>
        /// <param name="sPostName">职位名称</param>
        /// <returns></returns>
        public ViewBase GetUserByPosition(string sPostName)
        {
            Position post = Position.GetPosition(sPostName);
            ViewPostUser vwPostUser = new ViewPostUser();
            vwPostUser.BaseCondition = post == null ? "1<>1" : "b.FK_DeptID=" + base.ID.ToString() + " AND c.SortNum <=" + post.SortNum.ToString();
            return vwPostUser;
        }

        /// <summary>
        /// 根据职位名称获得当前部门大于该职位的人
        /// </summary>
        /// <param name="sPostName">职位名称</param>
        /// <param name="iFloorCode">部门层次: 0自己,>0 子部门层数,-1所有</param>
        /// <returns></returns>
        public ViewBase GetUserByPosition(string sPostName, int iFloorCode)
        {
            Position post = Position.GetPosition(sPostName);
            ViewPostUser vwPostUser = new ViewPostUser();
            string strCondition = post == null ? "1<>1" : " c.SortNum <=" + post.SortNum.ToString();
            string strSubDptIDs = string.Empty;
            if (iFloorCode != 0)
            {
                strSubDptIDs = this.GetChildDeptID(base.ID, iFloorCode);
                strSubDptIDs = strSubDptIDs.Length > 0 ? strSubDptIDs + "," + base.ID.ToString() : base.ID.ToString();
                strCondition += strSubDptIDs.Length > 0 ? " AND b.FK_DeptID IN (" + strSubDptIDs + ")" : "1<>1";
            }
            else
            {
                strCondition += " and b.FK_DeptID=" + base.ID.ToString();
            }

            vwPostUser.BaseCondition = strCondition;
            return vwPostUser;
        }

        /// <summary>
        /// 根据职位ID获得当前部门某些职位的人,支持多个职位
        /// </summary>
        /// <param name="sPostNames">职位名称 ','连接</param>
        /// <param name="iFloorCode">部门层次: 0自己,>0 子部门层数,-1所有</param>
        /// <returns></returns>
        public ViewBase GetUserByPostNames(string sPostNames,int iFloorCode)
        {
            string[] strArray = sPostNames.Split(',');
            string strPosts = string.Empty;
            foreach (string str in strArray)
            {
                if (strPosts.Length > 0)
                {
                    strPosts += ",";
                }
                strPosts += "'" + str + "'";
            }

            string strSubDptIDs = string.Empty;
            string strCondition = string.Format("b.FK_DeptID=" + base.ID.ToString() + " AND c.Name IN ({0})", strPosts);
            ViewPostUser vwPostUser = new ViewPostUser();
            if (iFloorCode != 0)
            {
                strSubDptIDs = this.GetChildDeptID(base.ID, iFloorCode);
                strSubDptIDs = strSubDptIDs.Length > 0 ? strSubDptIDs + "," + base.ID.ToString() : base.ID.ToString();
                strCondition += strSubDptIDs.Length > 0 ? " AND b.FK_DeptID IN (" + strSubDptIDs + ")" : "1<>1";
            }
            vwPostUser.BaseCondition = strCondition;
            return vwPostUser;
        }

        #endregion

        #region 根据部门ID获得:上级部门,下级部门;部门用户、角色、职位;子部门领导;子部门负责人

        /// <summary>
        /// 获取上级部门
        /// </summary>
        public Department Parent
        {
            get
            {
                if (this.m_enParent == null)
                {
                    this.m_enParent = new Department();
                }
                this.m_enParent.ID = base.ParentID;
                return this.m_enParent;
            }
        }

        /// <summary>
        /// 根据部门的FloorCode获得子部门
        /// </summary>
        /// <param name="iFloorCode"> >0 子部门层数, -1所有</param>
        /// <returns></returns>
        public ViewBase GetChildDepts(int iFloorCode)
        {
            ViewBase vbDepts = new ViewDepartment();
            string strSubDptIDs = this.GetChildDeptID(base.ID, iFloorCode);
            vbDepts.BaseCondition = strSubDptIDs.Length > 0 ? "a.ID IN (" + strSubDptIDs + ")" : " 1<>1 ";
            return vbDepts;
        }

        /// <summary>
        /// 根据部门的FloorCode获得子部门，包含自己
        /// </summary>
        /// <param name="iFloorCode"> >0 子部门层数, -1所有</param>
        /// <returns></returns>
        public ViewBase GetChildDeptsConSelf(int iFloorCode)
        {
            ViewBase vbDepts = new ViewDepartment();
            string strSubDptIDs = this.GetChildDeptID(base.ID, iFloorCode);
            vbDepts.BaseCondition = strSubDptIDs.Length > 0 ? "(a.ID IN (" + strSubDptIDs + ") OR a.ID = " + base.ID.ToString() + ")" : " 1<>1 ";
            return vbDepts;
        }

        /// <summary>
        /// 根据部门的FloorCode获得子部门，包含自己
        /// </summary>
        /// <param name="strIDs">部门IDs,用,连接</param>
        /// <param name="iFloorCode"> >0 子部门层数, -1所有</param>
        /// <returns></returns>
        public static ViewBase GetChildDeptsConSelf(string strIDs, int iFloorCode)
        {
            ViewBase vbDepts = new ViewDepartment();
            string[] strArray = strIDs.Split(',');
            string strDeptIDs = string.Empty;
            Department dept = new Department();
            foreach (string str in strArray)
            {
                if (strDeptIDs.Length > 0 && !strDeptIDs.EndsWith(","))
                {
                    strDeptIDs += ",";
                }
                strDeptIDs += dept.GetChildDeptID(Convert.ToInt32(str), iFloorCode);
            }
            if (strDeptIDs.EndsWith(","))
            {
                strDeptIDs = strDeptIDs.Substring(0,strDeptIDs.Length - 1);
            }
            string strSubDptIDs = strDeptIDs;
            vbDepts.BaseCondition = strSubDptIDs.Length > 0 ? "(a.ID IN (" + strSubDptIDs + ") OR a.ID IN (" + strIDs + "))" : " a.ID IN (" + strIDs + ") ";
            return vbDepts;
        }

        /// <summary>
        /// 根据层号得到部门
        /// </summary>
        /// <param name="iFloorCode"> ">0"根据层返回部门 "-1"返回所有部门</param>
        /// <returns></returns>
        public static ViewBase GetDeptsByFCode(int iFloorCode)
        {
            ViewBase vbDepts = new ViewDepartment();
            if (iFloorCode >= 0)
            {
                vbDepts.BaseCondition = "a.FloorCode=" + iFloorCode.ToString();
            }
            return vbDepts;
        }

        /// <summary>
        /// 获取部门用户
        /// </summary>
        public ViewBase Users
        {
            get
            {
                ViewBase vwUser = new ViewUser();
                vwUser.BaseCondition = "b.FK_DeptID = " + base.ID.ToString();
                return vwUser;
            }
        }

        /// <summary>
        /// 获取部门用户
        /// </summary>
        /// <param name="eStatus">状态:注销,正常</param>
        /// <returns></returns>
        public ViewBase GetUsers(Common.UserStatus eStatus)
        {
            ViewBase vwUser = this.Users;
            vwUser.BaseCondition += " AND a.IsCancel = " + ((int)eStatus).ToString();
            return vwUser;
        }

        /// <summary>
        /// 获取部门角色
        /// </summary>
        public ViewBase Roles
        {
            get
            {
                this.m_vwDeptRole.BaseCondition = " (b.FK_DeptID = '" + base.ID.ToString() + "' OR b.FK_DeptID Like '" + base.ID.ToString() + ",%' OR b.FK_DeptID Like '%," + base.ID.ToString() + ",%' OR b.FK_DeptID Like '%," + base.ID.ToString() + "')";
                return this.m_vwDeptRole;
            }
        }

        /// <summary>
        /// 获取部门职位
        /// </summary>
        public ViewBase Positions
        {
            get
            {
                ViewBase vwPost = new ViewPost();
                vwPost.BaseCondition = "b.FK_DeptID = " + base.ID.ToString();
                return vwPost;
            }
        }

        #endregion

        #region 根据部门ID或者部门名称获得:部门信息;子部门领导;子部门负责人

        /// <summary>
        /// 获取部门及子部门领导
        /// <param name="iFloorCode"> 0自己,>0 子部门层数,-1所有</param>
        /// </summary>
        /// <returns></returns>
        public ViewBase GetLeaders(int iFloorCode)
        {
            return this.GetManagerOrLeader(iFloorCode, Common.LeaderType.Leader);
        }

        /// <summary>
        /// 获取部门及子部门负责人
        /// <param name="iFloorCode"> 0自己,>0 子部门层数 -1所有</param>
        /// </summary>
        /// <returns></returns>
        public ViewBase GetManager(int iFloorCode)
        {
            return this.GetManagerOrLeader(iFloorCode, Common.LeaderType.Manager);
        }

        /// <summary>
        /// 获取部门及子部门领导或负责人
        /// </summary>
        /// <param name="iFloorCode">0自己,>0 子部门层数 -1所有</param>
        /// <param name="type">领导或负责人</param>
        /// <returns></returns>
        private ViewBase GetManagerOrLeader(int iFloorCode, Common.LeaderType type)
        {
            string strSubDptIDs = string.Empty;
            int iType = (int)(Common.LeaderType.Leader | Common.LeaderType.Manager);
            string strCondition = "(b.LeaderType = " + iType + " OR b.LeaderType = " + ((int)type).ToString() + ")";
            if (iFloorCode == 0)
            {
                strCondition += " AND d.ID = " + base.ID.ToString();
            }
            else
            {
                strSubDptIDs = this.GetChildDeptID(base.ID, iFloorCode);
                strSubDptIDs = strSubDptIDs.Length > 0 ? strSubDptIDs + "," + base.ID.ToString() : base.ID.ToString();
                strCondition += strSubDptIDs.Length > 0 ? " AND d.ID IN (" + strSubDptIDs + ")" : " AND 1<>1";
            }
            ViewUser vwUser = new ViewUser();
            vwUser.BaseCondition = strCondition;
            return vwUser;
        }

        /// <summary>
        /// 获得部门下的负责人或领导或大于某职位的人
        /// </summary>
        /// <param name="iFloorCode"> ">0"根据层返回部门 "-1"返回所有部门</param>
        /// <param name="type">部门</param>
        /// <param name="strPostName">职位名称</param>
        /// <returns></returns>
        public ViewBase GetManagerOrLeader(int iFloorCode, Common.LeaderType type, string strPostName)
        {
            Position post = Position.GetPosition(strPostName);
            string strCondition = string.Empty;
            string strSubDptIDs = string.Empty;

            strCondition = post == null ? "1<>1" : " e.SortNum <=" + post.SortNum.ToString();

            if (type == Common.LeaderType.LeaderAndManager)
            {
                strCondition = " ( "+strCondition+" or b.LeaderType = " + (int)Common.LeaderType.Leader + " OR b.LeaderType = " + (int)Common.LeaderType.Manager;
                strCondition += " or b.LeaderType = " + (int)Common.LeaderType.LeaderAndManager + ")";
            }
            else if(type != Common.LeaderType.User)
            {
                strCondition = " ( "+strCondition+" or b.LeaderType = " + (int)type + " or b.LeaderType = " + (int)Common.LeaderType.LeaderAndManager + ")"; 
            }

            if (iFloorCode == 0)
            {
                strCondition += " AND d.ID = " + base.ID.ToString();
            }
            else
            {
                strSubDptIDs = this.GetChildDeptID(base.ID, iFloorCode);
                strSubDptIDs = strSubDptIDs.Length > 0 ? strSubDptIDs + "," + base.ID.ToString() : base.ID.ToString();
                strCondition += strSubDptIDs.Length > 0 ? " AND d.ID IN (" + strSubDptIDs + ")" : " AND 1<>1";
            }
            ViewUser vwUser = new ViewUser();
            vwUser.BaseCondition = strCondition;
            return vwUser;
        }

        /// <summary>
        /// 根据部门名称获得部门对象
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static Department GetDepartment(string strName)
        {
            ViewDepartment vwDept = new ViewDepartment(true);
            vwDept.BaseCondition = " a.Name = '" + strName + "'";
            return vwDept.Count > 0 ? vwDept.GetItem(0) as Department : null;
        }

        /// <summary>
        /// 根据部门ID获得部门对象
        /// </summary>
        /// <param name="iID"></param>
        /// <returns></returns>
        public static Department GetDepartment(int iID)
        {
            ViewBase vb = new ViewDepartment(true);
            vb.BaseCondition = "a.ID = " + iID.ToString();
            return vb.Count > 0 ? vb.GetItem(0) as Department : null;
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public static ViewBase GetAllDepartment()
        {
            return new ViewDepartment();
        }

        /// <summary>
        /// 获得所有领导
        /// </summary>
        /// <returns></returns>
        public static ViewBase GetALLLeaders()
        {
            int iType = (int)(Common.LeaderType.Leader | Common.LeaderType.Manager);
            string strCondition = "(b.LeaderType = " + iType + " OR b.LeaderType = " + ((int)(Common.LeaderType.Leader)).ToString() + ")";
            ViewUser vwUser = new ViewUser();
            vwUser.BaseCondition = strCondition;
            return vwUser;
        }

        #region 当前部门下子部门的人

        /// <summary>
        /// 当前部门下子部门的人
        /// </summary>
        /// <param name="ifloorCode"> 0自己,>0 子部门层数,-1所有</param>
        /// <returns></returns>
        public ViewBase GetChildDeptUsers(int ifloorCode)
        {
            return this.GetChildDUser(ifloorCode, Common.UserStatus.Normal | Common.UserStatus.Canceled);
        }

        /// <summary>
        /// 当前部门下子部门的人
        /// </summary>
        /// <param name="ifloorCode"> 0自己,>0 子部门层数,-1所有</param>
        /// <param name="eStatus">状态:注销,正常</param>
        /// <returns></returns>
        public ViewBase GetChildDeptUsers(int ifloorCode, Common.UserStatus eStatus)
        {
            return this.GetChildDUser(ifloorCode, eStatus);
        }
        
        /// <summary>
        /// 获取当前部门下的人
        /// </summary>
        /// <param name="ifloorCode"> 0自己,>0 子部门层数,-1所有</param>
        /// <param name="eStatus">状态</param>
        /// <returns></returns>
        private ViewBase GetChildDUser(int ifloorCode, Common.UserStatus eStatus)
        {
            string strSubDptIDs = ifloorCode == 0 ? string.Empty : this.GetChildDeptID(base.ID, ifloorCode);
            strSubDptIDs = strSubDptIDs.Length > 0 ? strSubDptIDs + "," + base.ID.ToString() : base.ID.ToString();
            ViewBase vwUser = new ViewUser();
            vwUser.BaseCondition = "b.FK_DeptID IN (" + strSubDptIDs + ") AND a.IsCancel = " + ((int)eStatus).ToString();
            return vwUser;
        }

        #endregion

        #endregion

        #region Method 获得上级和下级部门

        /// <summary>
        /// 子部门ID 用逗号连接
        /// </summary>
        /// <param name="iID">部门ID</param>
        /// <param name="iFCode">iFCode:部门层 0自己,>0 子部门层数,-1所有</param>
        /// <returns></returns>
        public string GetChildDeptID(int iID, int iFCode)
        {
            string strChilddeptIds = string.Empty;
            string strSql = "ParentID=" + iID.ToString();
            if (iFCode != -1)
            {
                strSql += " AND FloorCode<=" + iFCode;
            }
            DataTable dt = Entity.GetRecords(Department.TableName, "ID", strSql, true);
            foreach (DataRow dr in dt.Rows)
            {
                if (strChilddeptIds != string.Empty)
                {
                    strChilddeptIds += ",";
                }
                strChilddeptIds += dr["ID"].ToString();
                if (strChilddeptIds != string.Empty)
                {
                    strChilddeptIds += ",";
                }
                strChilddeptIds += this.GetChildDeptID(Convert.ToInt32(dr["ID"]), iFCode);
                strChilddeptIds = strChilddeptIds.EndsWith(",") ? strChilddeptIds.Remove(strChilddeptIds.Length - 1) : strChilddeptIds;
            }
            return strChilddeptIds;
        }

        /// <summary>
        /// 上级部门ID 用逗号连接
        /// </summary>
        /// <param name="dept">部门</param>
        /// <returns></returns>
        public string GetParentDeptID(Department dept)
        {
            string strParentDeptIDs = string.Empty;
            if (dept != null && dept.Parent != null)
            {
                int iParentID = dept.Parent.ID;
                int parentFCode = dept.Parent.FloorCode;
                if (parentFCode != 0 && parentFCode != int.MinValue)
                {
                    strParentDeptIDs += iParentID.ToString();
                    if (strParentDeptIDs != string.Empty)
                    {
                        strParentDeptIDs += ",";
                    }
                    strParentDeptIDs += this.GetParentDeptID(dept.Parent);
                    strParentDeptIDs = strParentDeptIDs.EndsWith(",") ? strParentDeptIDs.Remove(strParentDeptIDs.Length - 1) : strParentDeptIDs;
                }
            }
            return strParentDeptIDs;
        }

        #endregion

        #region

        /// <summary>
        /// 删除前检查是否可以删除
        /// </summary>
        /// <returns></returns>
        private static void BeforeDelete(string strIDs, out string outStr)
        {
            string strShow = string.Empty;
            if (strIDs.Length > 0) strIDs = "(" + strIDs + ")";

            string strSqldisable = @" SELECT DISTINCT ParentID FROM " + Department.TableName + " WHERE ParentID IN " + strIDs +
                                    " union " +
                                    " SELECT DISTINCT FK_DeptID FROM " + DeptPost.TableName + " WHERE FK_DeptID IN " + strIDs;

            string strSql = string.Format(@"SELECT Name FROM " + Department.TableName + " WHERE ID in " + strIDs + " AND ID IN " + "(" + strSqldisable + ")");
            System.Data.DataTable dtDept = Entity.RunQuery(strSql);

            foreach (DataRow dr in dtDept.Rows)
            {
                strShow += dr["Name"].ToString() + "：下有人或者存在子部门,不能删除：" + "\\n";
            }

            outStr = strShow;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="strIDs">删除的ID,用','连接</param>
        /// <returns></returns>
        /// <param name="outStr"></param>
        /// <returns></returns>
        public static bool DeleteData(string strIDs, out string outStr)
        {
            Department.BeforeDelete(strIDs, out outStr);
            if (strIDs.Length > 0) strIDs = "(" + strIDs + ")";
            string strDisableDelIDs = @" SELECT DISTINCT ParentID FROM " + Department.TableName + " WHERE ParentID IN " + strIDs +
                                       " union " +
                                       " SELECT DISTINCT FK_DeptID FROM " + DeptPost.TableName + " WHERE FK_DeptID IN " + strIDs;
            string strAbleDelIDs = string.Format(@"SELECT ID FROM " + Department.TableName + " WHERE ID in " + strIDs + " AND NOT ID IN " + "(" + strDisableDelIDs + ")");
            bool bActual = Entity.Delete(Department.TableName, string.Format("[ID] IN ({0})", strAbleDelIDs), true) > 0;
            return bActual;
        }

        /// <summary>
        /// 子部门ID 用逗号连接
        /// </summary>
        string m_deptids = string.Empty;

        #region Check Data

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeSaveCheck()
        {
            bool r = true;
            if (string.IsNullOrEmpty(this.ParentID.ToString()))
            {
                base.ErrMsgs.Add("上级部门不能为空");
                r = false;
            }
            if (string.IsNullOrEmpty(this.Name.ToString()))
            {
                base.ErrMsgs.Add("部门名不能为空");
                r = false;
            }
            //wangmx20090929
            //当新增数据时才需要进行CheckSameRecord验证
            if ((CheckSameRecord()))
            {
                this.ErrMsgs.Add("部门名重复");
                r = false;
            }
            if (this.Remark.Length > 500)
            {
                base.ErrMsgs.Add("备注不能过长");
                r = false;
            }
            return r;
        }

        /// <summary>
        /// 重复性验证
        /// </summary>
        /// <returns></returns>
        private bool CheckSameRecord()
        {
            string sqlWhere = " AND Name = '" + this.Name + "' AND ID <> " + base.ID.ToString() + " AND ParentID = " + base.ParentID.ToString();
            return Common.IsSameRecord(Department.TableName, sqlWhere);
        }

        /// <summary>
        /// 判断是否在存在子部门
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private bool BeforDelete(int ID)
        {
            bool b1 = Entity.GetRecordCount(Department.TableName, "ParentID = " + ID.ToString(), false) > 0;
            bool b2 = Entity.GetRecordCount(DeptPost.TableName, "FK_DeptID = " + ID.ToString(), false) > 0;
            return !b1 && !b2;
        }

        #endregion

        #region Operate Data

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <returns></returns>
        public new bool Delete()
        {
            bool b = false;
            if (BeforDelete(this.ID))
            {
                string strSql = string.Format("Delete From {0} Where  [ID] = '{1}'", Department.TableName, this.ID);
                b = Entity.RunNoQuery(strSql) > 0;
            }
            return b;
        }

        #endregion

        #region Public interface

        #region

        /// <summary>
        /// 根据部门ID获得子部门的FloorCode
        /// 王敏贤 2009-10-23
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        public int GetFloorCodeByDeptID(int dept_id)
        {
            int floorCode = 0;
            string sql = "SELECT FloorCode FROM " + Department.TableName + " WHERE id=" + dept_id;
            DataTable dt = Entity.RunQuery(sql);
            if (dt.Rows.Count > 0)
            {
                floorCode = int.Parse(dt.Rows[0]["FloorCode"].ToString()) + 1;
            }
            return floorCode;
        }

        /// <summary>
        /// 根据部门id 获得该部门及子部门的User
        ///  王敏贤 2009-10-27
        /// </summary>
        /// <param name="dept_id">部门ID</param>
        /// <param name="floorCode">部门层</param>
        /// <returns></returns>
        public DataTable GetUserByDeptID(string dept_id, int floorCode)
        {
            string sql = string.Format(@"SELECT a.FK_UserID,b.Name,a.FK_DeptID,c.FloorCode
                                       FROM {0} a
                                       INNER JOIN {1} b ON a.FK_UserID = b.ID
                                       INNER JOIN {2} c ON c.ID = a.FK_DeptID
                                       WHERE (b.RecordStatus ='1')", DeptPost.TableName, User.TableName, Department.TableName);
            sql += " AND a.FK_DeptID =" + dept_id;
            string strSubDptIDs = this.GetSubDeptID(dept_id);
            if (strSubDptIDs.Length > 0)
            {
                sql += " OR a.FK_DeptID IN(" + strSubDptIDs + ")";
            }
            if (floorCode != -1)
            {
                sql += " AND c.FloorCode<=" + floorCode;
            }
            return Entity.RunQuery(sql);
        }

        /// <summary>
        /// 根据部门ID获得所有部门IDs
        /// 王敏贤 2009-10-27
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        private string GetSubDeptID(string dept_id)
        {
            string strSql = "SELECT ID FROM " + Department.TableName + " WHERE ParentID=" + dept_id;
            DataTable dt = Entity.RunQuery(strSql);
            foreach (DataRow dr in dt.Rows)
            {
                if (m_deptids != string.Empty)
                {
                    m_deptids += ",";
                }
                m_deptids += dr["ID"].ToString();
                m_deptids = GetSubDeptID(dr["ID"].ToString());
            }
            return m_deptids;
        }

        /// <summary>
        /// 获得符合条件的部门信息
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetDeptDT(string strSql)
        {
            string sql = string.Format("SELECT *,Name AS DeptName FROM {0} WHERE RecordStatus=1 ", Department.TableName);
            if (strSql != string.Empty)
            {
                sql += strSql;
            }
            sql += " ORDER BY SortNum";
            return Entity.RunQuery(sql);
        }


        #endregion

        #endregion

        #endregion
    }
}