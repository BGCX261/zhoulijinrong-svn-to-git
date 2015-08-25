//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述角色信息:角色的人;角色的部门;角色的人;
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-11-12 实现当前角色所属部门
// 修改描述：
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using System.Data;
using FounderSoftware.ADIM.OU.BLL.AutoGene;
using FounderSoftware.ADIM.OU.BLL.View;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 描述角色信息:角色的人;角色的部门;角色的人;
    /// </summary>
    public class Role : GeneRole
    {
        #region Define

        /// <summary>
        /// 部门视图
        /// </summary>
        ViewDepartment m_vwDept = new ViewDepartment();

        /// <summary>
        /// 角色用户视图
        /// </summary>
        private ViewBase m_vwRoleUsers = new ViewRoleUsers();

        #endregion

        #region 角色所属哪些部门;具有角色的人

        /// <summary>
        /// 当前角色所属哪些部门(角色的部门)
        /// </summary>
        public ViewBase Depts
        {
            get
            {
                string strDeptIDs = this.GetDeptIDByRole();
                this.m_vwDept.BaseCondition = strDeptIDs.Length > 0 ? "a.ID IN (" + strDeptIDs + ")" : " 1<>1 ";
                return this.m_vwDept;
            }
        }

        /// <summary>
        /// 具有当前角色的人
        /// </summary>
        public ViewBase Users
        {
            get
            {
                this.m_vwRoleUsers.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,
                                             a.MobilePhone,a.Email,a.SortNum,a.RecordStatus,a.Remark,a.EditDate,a.IsCancel,d.Name AS RoleName,a.Name AS UserName,c.FK_RoleID";
                this.m_vwRoleUsers.BaseCondition = "d.ID = " + base.ID.ToString();
                return m_vwRoleUsers;
            }
        }

        /// <summary>
        /// 获取部门用户
        /// </summary>
        /// <param name="eStatus">状态:注销,正常</param>
        /// <returns></returns>
        public ViewBase GetUsers(Common.UserStatus eStatus)
        {
            ViewRoleUser vwRoleUesr = new ViewRoleUser();
            vwRoleUesr.BaseCondition = "d.ID = " + base.ID.ToString() + " AND a.IsCancel = " + ((int)eStatus).ToString();
            return vwRoleUesr;
        }

        /// <summary>
        /// 获得所有角色
        /// </summary>
        /// <returns></returns>
        public static ViewBase GetALLRole()
        {
            ViewRole vRole = new ViewRole(true);
            return vRole;
        }

        #endregion

        #region 根据部门名称获得角色下的人;根据部门名称获得角色

        /// <summary>
        /// 根据部门获得当前角色下的人
        /// </summary>
        /// <param name="iDeptID">部门ID</param>
        /// <returns></returns>
        public ViewBase GetUserByCurrDept(int iDeptID)
        {
            ViewBase vw = new ViewRoleUser();
            string strCon = iDeptID == 0 ? " a.IsCancel = 1 " : "a.IsCancel = 1 AND b.FK_DeptID = " + iDeptID.ToString();
            vw.Field = @"DISTINCT e.Name AS PostName,a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,b.ID AS DpuID,b.LeaderType,
                                                CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue,
                                                CASE b.LeaderType WHEN '1' THEN '领导' WHEN '2' THEN '负责人' WHEN '3' THEN '领导;负责人' END AS LeaderTypeName ";
            vw.BaseCondition = strCon + " AND d.ID = " + base.ID.ToString();
            return vw;
        }

        /// <summary>
        /// 根据部门ID和角色获得用户
        /// </summary>
        /// <param name="iDeptID">部门ID</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public  static ViewBase GetUserByDeptAndRoles(int iDeptID,string strWhere)
        {
            ViewBase vw = new ViewRoleUser();
            string strCon = iDeptID == 0 ? " a.IsCancel = 1 " : "a.IsCancel = 1 AND b.FK_DeptID = " + iDeptID.ToString();
            strCon = strWhere == string.Empty ? strCon : strCon + strWhere;
            vw.Field = @"DISTINCT e.Name AS DeptName,f.Name AS PostName,a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,b.ID AS DpuID,b.LeaderType,
                                                CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue,
                                                CASE b.LeaderType WHEN '1' THEN '领导' WHEN '2' THEN '负责人' WHEN '3' THEN '领导;负责人' END AS LeaderTypeName ";
            vw.BaseCondition = strCon;
            return vw;
        }

        /// <summary>
        /// 根据部门获得当前角色下的人
        /// </summary>
        /// <param name="iDeptID">部门ID</param>
        /// <param name="eStatus">用户状态:注销/启用</param>
        /// <returns></returns>
        public ViewBase GetUserByCurrDept(int iDeptID, Common.UserStatus eStatus)
        {
            ViewBase vw = this.GetUsers(eStatus);
            vw.BaseCondition += " b.FK_DeptID = '" + iDeptID + "'";
            return vw;
        }
        
        /// <summary>
        /// 根据部门ID获得角色下的人(在某个部门具有当前角色的人)
        /// </summary>
        /// <param name="iDeptID">部门ID</param>
        /// <returns></returns>
        public ViewBase GetUserByDept(int iDeptID)
        {
            Department dpt = new Department();
            string strSubDptIDs = dpt.GetChildDeptID(iDeptID, -1);
            string strCondition = "(d.ID = " + base.ID.ToString() + " AND b.FK_DeptID = " + iDeptID.ToString() + ")";
            strCondition = strSubDptIDs.Length > 0 ? "(" + strCondition + "OR (d.ID = " + base.ID.ToString() + " AND b.FK_DeptID IN (" + strSubDptIDs + ")))" : strCondition;
            ViewBase vw = new ViewRoleUser(false);
            vw.BaseCondition = strCondition;
            return vw;
        }

        /// <summary>
        /// 根据部门ID获得角色下的人(在某个部门具有当前角色的人)
        /// </summary>
        /// <param name="iDeptID">部门ID</param>
        /// <param name="eStatus">eStatus</param>
        /// <returns></returns>
        public ViewBase GetUserByDept(int iDeptID, Common.UserStatus eStatus)
        {
            Department dpt = new Department();
            string strSubDptIDs = dpt.GetChildDeptID(iDeptID, -1);
            string strCondition = "(d.ID = " + base.ID.ToString() + " AND b.FK_DeptID = " + iDeptID.ToString() + ")";
            strCondition = strSubDptIDs.Length > 0 ? "(" + strCondition + "OR (d.ID = " + base.ID.ToString() + " AND b.FK_DeptID IN (" + strSubDptIDs + ")))" : strCondition;
            ViewBase vw = new ViewRoleUser();
            vw.BaseCondition = strCondition + " AND a.IsCancel = " + ((int)eStatus).ToString();
            return vw;
        }

        /// <summary>
        /// 根据部门名称获得他的子部门下这个角色的人
        /// </summary>
        /// <param name="strDeptName">部门名称</param>
        /// <returns></returns>
        public ViewBase GetUserByDept(string strDeptName)
        {
            int iDeptID = Department.GetDepartment(strDeptName).ID;
            return this.GetUserByDept(iDeptID);
        }

        /// <summary>
        /// 通过角色id获得角色
        /// </summary>
        /// <param name="iRoleID">角色id</param>
        /// <returns></returns>
        public static Role GetRole(int iRoleID)
        {
            ViewRole vwRole = new ViewRole();
            vwRole.BaseCondition = "a.ID='" + iRoleID + "'";
            return vwRole.Count > 0 ? vwRole.GetItem(0) as Role : null;
        }

        /// <summary>
        /// 通过角色名称获得角色
        /// </summary>
        /// <param name="strName">角色名称</param>
        /// <returns></returns>
        public static Role GetRole(string strName)
        {
            ViewRole vwRole = new ViewRole();
            vwRole.BaseCondition = "a.Name='" + strName + "'";
            return vwRole.Count > 0 ? vwRole.GetItem(0) as Role : null;
        }

        /// <summary>
        /// 获得所有角色类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRoleType()
        {
            return Entity.RunQuery("SELECT * FROM T_OU_RoleType");
        }

        #endregion

        #region  根据角色获得部门 格式用逗号连接方法

        /// <summary>
        /// 根据角色获得部门 格式用逗号连接
        /// </summary>
        /// <returns></returns>
        private string GetDeptIDByRole()
        {
            string strDepts = string.Empty;
            string strSql = "SELECT FK_DeptID FROM " + DeptRole.TableName + " a INNER JOIN T_OU_Role b ON a.FK_RoleID=b.ID WHERE b.Name = '" + base.Name + "'";
            DataTable dt = Entity.RunQuery(strSql);
            if (dt.Rows.Count > 0)
            {
                strDepts = dt.Rows[0]["FK_DeptID"].ToString();
            }
            return strDepts;
        }

        #endregion

        #region 内部功能代码

        #region CheckData

        /// <summary>
        /// 重复性验证
        /// </summary>
        /// <returns></returns>
        private bool CheckSameRecord(string name)
        {
            string sqlWhere = string.Format(" AND Name = '{0}' AND RecordStatus=1 AND RoleType = '" + base.RoleType + "' AND ID<> '" + base.ID + "' ", name);
            return Common.IsSameRecord(Role.TableName, sqlWhere);
        }

        /// <summary>
        /// 保存前检查数据合法性
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeSaveCheck()
        {
            bool bActual = true;
            if (string.IsNullOrEmpty(this.Name))
            {
                bActual = false;
                base.ErrMsgs.Add("角色名不能为空");
            }

            if (SysUtility.FilteStr(this.Name) == false)
            {
                bActual = false;
                base.ErrMsgs.Add("角色名有危险字符");
            }

            if (this.CheckSameRecord(this.Name))
            {
                bActual = false;
                base.ErrMsgs.Add("角色名重复");
            }

            if (this.Remark.Length > 500)
            {
                bActual = false;
                base.ErrMsgs.Add("备注不能过长");
            }
            return bActual;
        }

        #endregion

        #region Public Interface

        /// <summary>
        /// 获得角色信息
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetRoleDT(string strSql)
        {
            string sql = string.Format("SELECT * FROM " + Role.TableName + " WHERE RecordStatus=1 ");
            if (strSql != string.Empty)
            {
                sql += strSql;
            }
            sql += " ORDER BY SortNum";
            return Entity.RunQuery(sql);
        }

        /// <summary>
        /// 获取人员的不同来源－－角色、职位、（角色＋职位）
        /// 如果是0按照角色取人,如果是1按照职位取人，如果是2二者取并
        /// 注意：一次调用只支持1个角色，多个角色可多次调用
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public int QueryMethod(string roleName)
        {
            DataTable dt = GetRoleDT(" AND Name='" + roleName + "'");
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                i = int.Parse(dt.Rows[0]["GetFunction"].ToString());
            }
            return i;
        }

        #endregion

        #region Operate Data

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <returns></returns>
        public new bool Delete()
        {
            bool bActual = false;
            string strSql = string.Format(" AND FK_RoleID = '{0}' AND RecordStatus=1 ", base.ID);
            string strSqlDept = string.Format(" AND FK_RoleID = '{0}' AND RecordStatus=1 ", base.ID);
            if (!Common.IsSameRecord(RoleUser.TableName, strSql) && !Common.IsSameRecord(DeptRole.TableName, strSql))
            {
                strSqlDept = string.Format("DELETE FROM " + Role.TableName + "  WHERE [ID] = '{0}'", base.ID);
                bActual = Entity.RunNoQuery(strSqlDept) > 0;
            }
            return bActual;
        }

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="ids">ids</param>
        /// <param name="bActual">物理删除还是逻辑删除</param>
        /// <returns></returns>
        public static bool Delete(string ids, bool bActual)
        {
            string strSql = string.Format("[ID] IN ({0})", ids);
            return Entity.Delete(TableName, strSql, bActual) > 0;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="strIDs">用户IDs 用","连接</param>
        public static bool Hide(string strIDs)
        {
            string strSql = string.Format("Update " + BLL.Busi.Role.TableName + " set IsCancel = 0 WHERE ID IN {(0)}", strIDs);
            return Entity.RunNoQuery(strSql) > 0;
        }

        #endregion

        #endregion
    }
}