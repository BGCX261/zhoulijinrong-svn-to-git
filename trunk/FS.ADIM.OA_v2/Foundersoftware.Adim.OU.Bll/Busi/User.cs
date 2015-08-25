//-----------------------------------------------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述用户信息:用户部门;角色;职位;领导的处室、科室;负责的处室、科室;用户登陆;
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-11-26 王敏贤
// 修改描述：添加:用户所在部门的处室或科室 GetChildOrParentDepts(int iFloorCode)
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
// 修改标识：2010-1-13 王敏贤
// 修改描述：当前用户所在部门的子部门包含自己 GetChildDeptConSelf();职位找人GetUserByPosition(),
//           角色找人GetUserByRoles();不在某个角色的人 GetUserNotInRoles(string sRoles)
//----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FounderSoftware.ADIM.OU.BLL.AutoGene;
using FounderSoftware.ADIM.OU.BLL.View;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 描述用户信息:用户部门;角色;职位;领导的处室、科室;负责的处室、科室;用户登陆;
    /// </summary>
    public class User : GeneUser
    {
        #region Define

        /// <summary>
        /// 登陆验证结果
        /// </summary>
        public enum LoginResult
        {
            /// <summary>
            /// 登陆成功
            /// </summary>
            Succeed = 1,

            /// <summary>
            /// 密码错误
            /// </summary>
            PwdError = 2,

            /// <summary>
            /// 帐号非法
            /// </summary>
            UserIDError = 4,
        }

        /// <summary>
        /// 用户部门视图
        /// </summary>
        private ViewBase m_vwUserDept = new ViewUserDept();

        /// <summary>
        /// 部门职位视图
        /// </summary>
        private ViewBase m_vwDeptPost = new ViewDeptPost();

        #endregion

        #region 刷新相关实体

        /// <summary>
        /// 刷新相关实体
        /// </summary>
        protected override void RefreshObjects()
        {
            this.m_vwUserDept.BaseCondition = "b.FK_UserID=" + base.ID.ToString();
            this.m_vwDeptPost.BaseCondition = "a.FK_UserID=" + base.ID.ToString();
        }

        #endregion

        #region 针对海南

        /// <summary>
        /// 根据职位找人，支持多个职位
        /// </summary>
        /// <param name="sPostNames">职位名字 ","连接</param>
        /// <returns></returns>
        public static ViewBase GetUserByPosition(string sPostNames)
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
            ViewPostUser vwPostUser = new ViewPostUser();
            vwPostUser.BaseCondition = sPostNames.Length > 0 ? "c.Name in (" + strPosts + ")" : "1<>1";
            return vwPostUser;
        }

        /// <summary>
        /// 根据职位ID获得用户
        /// </summary>
        /// <param name="iPostID">职位ID</param>
        /// <returns></returns>
        public static ViewBase GetUserByPositionID(int iPostID)
        {
            ViewPostUser vwPostUser = new ViewPostUser();
            vwPostUser.BaseCondition = iPostID > 0 ? "c.ID = " + iPostID.ToString() : "1<>1";
            return vwPostUser;
        }

        /// <summary>
        /// 根据角色找人，支持多个角色
        /// </summary>
        /// <param name="sRoles">角色名字 ","连接</param>
        /// <returns></returns>
        public static ViewBase GetUserByRoles(string sRoles)
        {
            return User.GetUserByRoles(sRoles, true);
        }

        /// <summary>
        /// 根据角色找人，支持多个角色
        /// </summary>
        /// <param name="sRoles">角色名字 ","连接</param>
        /// <returns></returns>
        public static ViewBase GetUserNotInRoles(string sRoles)
        {
            return User.GetUserByRoles(sRoles, false);
        }

        /// <summary>
        /// 根据部门ID获得用户，支持多个部门
        /// </summary>
        /// <param name="sDeptIDs">部门ID，用','连接</param>
        /// <returns></returns>
        public static ViewBase GetUserByDeptIDs(string sDeptIDs)
        {
            ViewBase vwUser = new ViewUser();
            vwUser.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                                                a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,
                                                CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";
            vwUser.BaseCondition = sDeptIDs.Length > 0 ? "b.FK_DeptID IN (" + sDeptIDs + ")" : "1<>1";
            return vwUser;
        }

        /// <summary>
        /// 根据角色获得用户，支持多个
        /// </summary>
        /// <param name="sRoles">角色名,支持多个</param>
        /// <param name="bFalg">属于或不属于该角色的用户</param>
        /// <returns></returns>
        private static ViewBase GetUserByRoles(string sRoles,bool bFalg)
        {
            string[] strArray = sRoles.Split(',');
            string strRoles = string.Empty;
            foreach (string str in strArray)
            {
                if (strRoles.Length > 0)
                {
                    strRoles += ",";
                }
                strRoles += "'" + str + "'";
            }
            string strCondition = bFalg ? "d.Name in (" + strRoles + ")" : "d.Name not in (" + strRoles + ")";
            ViewRoleUser vwRoleUser = new ViewRoleUser(true);
            vwRoleUser.Field = @"DISTINCT a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.D_Class,
                                a.MobilePhone,a.Email,a.SortNum,a.RecordStatus,a.Remark,a.EditDate,a.IsCancel,
                                CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";
            vwRoleUser.BaseCondition = sRoles.Length > 0 ? strCondition : "1<>1";
            return vwRoleUser;               
        }

        /// <summary>
        /// 当前用户所在部门的所有字部门
        /// </summary>
        /// <returns></returns>
        public ViewBase GetChildDeptConSelf()
        {
            Department dpt = new Department();
            string strDepts = string.Empty;
            foreach (Department dept in this.Depts.Ens)
            {
                if (strDepts.Length > 0 && !strDepts.EndsWith(","))
                {
                    strDepts += ",";
                }
                strDepts += dpt.GetChildDeptID(dept.ID,-1);
            }
            strDepts = strDepts.Length > 0 && strDepts.EndsWith(",") ? strDepts.Substring(0, strDepts.Length - 1) : strDepts;
            ViewBase vwDept = new ViewDepartment();
            vwDept.BaseCondition = strDepts.Length > 0 ? "(a.ID IN (" + strDepts + ") OR a.ID IN(" + this.Depts.IDs + "))" : " a.ID IN(" + this.Depts.IDs + ")";
            return vwDept;
        }

        #endregion

        #region  根据人的ID 获得所在部门 角色 职位 领导的处室、科室，负责的处室、科室

        /// <summary>
        /// 获取域名帐号
        /// </summary>
        public string DomainUserID
        {
            get { return base.Domain + @"\" + base.UserID; }
        }

        /// <summary>
        /// 获取所在部门
        /// </summary>
        public ViewBase Depts
        {
            get
            { 
                return this.m_vwUserDept;
            }
        }

        /// <summary>
        /// 获取人的角色有四种方式:
        /// 1.通过User取角色
        /// 2.通过人所在的部门的职位取角色
        /// 3.通过部门取角色
        /// 4.通过职位取角色
        /// </summary>
        /// <param name="bIncludeDept">是否包含所在部门的角色</param>
        /// <returns></returns>
        public ViewBase GetRoles(bool bIncludeDept)
        {
            ViewBase vwRole = new ViewRole();
            string strCondition = "b.FK_UserID=" + base.ID.ToString();
            if (bIncludeDept)
            {
                //人所在部门职位的角色  
                string strRole = this.GetRoleByDeptID();
                if (strRole.Length > 0)
                {
                    strCondition += " OR a.ID IN (" + strRole + ")";
                }
            }
            vwRole.BaseCondition = strCondition;
            return vwRole;
        }

        /// <summary>
        /// 用户所在的角色
        /// </summary>
        public ViewBase Roles
        {
            get { return this.GetRoles(false); }
        }

        /// <summary>
        /// 获取所在部门的职位
        /// </summary>
        public ViewBase DeptPosts
        {
            get { return this.m_vwDeptPost; }
        }     

        /// <summary>
        /// 获取所在某个部门的职位
        /// </summary>
        /// <param name="iDeptID">部门ID</param>
        /// <returns></returns>
        public ViewBase Posts(int iDeptID)
        {
            ViewPost vbPost = new ViewPost();
            vbPost.BaseCondition = " b.FK_UserID=" + base.ID.ToString() + " AND b.FK_DeptID =" + iDeptID.ToString();
            return vbPost;
        }

        /// <summary>
        /// 负责的部门(哪些部门的负责人)
        /// </summary>
        public ViewBase ManagerDepts
        {
            get
            {
                ViewBase vbDepts = new ViewDepartment();
                vbDepts.BaseCondition = "c.ID=" + base.ID.ToString() + " AND (b.LeaderType=" + ((int)(Common.LeaderType.Manager)).ToString() + " or b.LeaderType=" + ((int)(Common.LeaderType.LeaderAndManager)).ToString() + ")";
                return vbDepts;
            }
        }

        /// <summary>
        /// 领导的部门(哪些部门的领导)
        /// </summary>
        public ViewBase LeaderDepts
        {
            get
            {
                ViewBase vbDepts = new ViewDepartment();
                vbDepts.BaseCondition = "c.ID=" + base.ID.ToString() + " AND (b.LeaderType=" + ((int)(Common.LeaderType.Leader)).ToString() + " or b.LeaderType=" + ((int)(Common.LeaderType.LeaderAndManager)).ToString() + ")"; ;
                return vbDepts;
            }
        }

        /// <summary>
        /// 获得所有非注销用户
        /// </summary>
        /// <returns></returns>
        public static ViewBase GetAllUser()
        {
            ViewBase vbUser = new ViewUser(true);
            vbUser.BaseCondition = "a.IsCancel = 1";
            return vbUser;
        }

        #endregion 

        #region 根据登陆帐号,密码 获取人,领导的处室,负责的科室

        /// <summary>
        /// 领导的处室或科室(哪些处室或科室的领导)
        /// </summary>
        /// <param name="iFloorCode">部门等级1,2,3,4;-1所有子部门</param>
        /// <returns></returns>
        public  ViewBase LeaderDeptsByCode(int iFloorCode)
        {
            ViewBase vw = this.LeaderDepts;
            if (iFloorCode != -1)
            {
                vw.BaseCondition = " a.FloorCode <= '" + iFloorCode + "'";
            }
            return vw;
        }

        /// <summary>
        /// 负责的处室或科室(哪些处室或科室的负责)
        /// </summary>
        /// <param name="iFloorCode">部门等级1,2,3,4;-1所有子部门</param>
        public ViewBase ManagerDeptsByCode(int iFloorCode)
        {
            ViewBase vw = this.ManagerDepts;
            if (iFloorCode != -1)
            {
                vw.BaseCondition = " a.FloorCode <= '" + iFloorCode + "'";
            }
            return vw;
        }

        /// <summary>
        /// 登陆验证,返回验证结果
        /// 登陆成功：返回LoginResult.Succeed的枚举,同时返回对应的User
        /// 登陆失败:返回 LoginResult.UserIDError或LoginResult.PwdError的枚举,并且返回User为null
        /// </summary>
        /// <param name="strID">帐号</param>
        /// <param name="strPWD">密码</param>
        /// <param name="user">用户对象(帐号或密码错误，返回null)</param>
        /// <returns></returns>
        public static LoginResult Login(string strID, string strPWD, out User user)
        {
            ViewUser vwUser = new ViewUser(true);
            LoginResult emStatus = LoginResult.UserIDError;
            user = null;
            vwUser.BaseCondition = "a.UserID='" + strID + "'";
            switch (vwUser.Count)
            {
                case 0:
                    break;

                case 1:
                    user = vwUser.GetItem(0) as User;
                    if (user.PWD.Equals(strPWD, StringComparison.CurrentCultureIgnoreCase))//密码正确
                    {
                        emStatus = LoginResult.Succeed;
                    }
                    else
                    {
                        user = null;
                        emStatus = LoginResult.PwdError;
                    }
                    break;

                default:
                    throw (new Exception("存在多个帐号相同的用户"));
            }
            return emStatus;
        }

        /// <summary>
        /// 获得用户
        /// </summary>
        /// <param name="strColName">列名</param>
        /// <param name="strValue">用户或帐号</param>
        /// <param name="binclude"></param>
        private static User GetUser(string strColName, string strValue, bool binclude)
        {
            User user = null;
            ViewUser vwUser = new ViewUser(true);
            vwUser.BaseCondition = binclude ? (strColName + "='" + strValue + "'") : (strColName + "='" + strValue + "' AND IsCancel = 1");
            if (vwUser.Count > 0)
            {
                user = vwUser.GetItem(0) as User;
            }
            return user;
        }

        /// <summary>
        /// 通过登陆帐号获得用户
        /// </summary>
        /// <param name="strUserID">用户帐号</param>
        /// <returns></returns>
        public static User GetUser(string strUserID)
        {
            return User.GetUser("UserID", strUserID, false);
        }

        /// <summary>
        /// 根据用户ID获得用户
        /// </summary>
        /// <param name="strID">用户ID</param>
        /// <returns></returns>
        public static User GetUserByID(string strID)
        {
            return User.GetUser("ID", strID, false);
        }

        /// <summary>
        /// 通过用户主键ID获得用户
        /// </summary>
        /// <param name="iUserID">用户ID</param>
        /// <returns></returns>
        public static User GetUser(int iUserID)
        {
            return User.GetUser("ID", iUserID.ToString(), false);
        }

        /// <summary>
        /// 通过用户主键ID获得用户(包含注销和启用)
        /// </summary>
        /// <param name="iUserID">用户ID</param>
        /// <returns></returns>
        public static User GetEntiyUser(int iUserID)
        {
            return User.GetUser("ID", iUserID.ToString(), true);
        }

        #endregion

        #region 用户所在部门的处室或科室

        /// <summary>
        /// 用户所在部门的处室或科室
        /// </summary>
        /// <param name="iFloorCode"> >0:自己  >0:子部门层数, -1:所有</param>
        /// <returns></returns>
        public ViewBase GetChildOrParentDepts(int iFloorCode)
        {
            Department dpt = new Department();
            string strDepts = string.Empty;
            foreach (Department dept in this.Depts.Ens)
            {
                if (strDepts.Length > 0 && !strDepts.EndsWith(","))
                {
                    strDepts += ",";
                }
                strDepts += dpt.GetParentDeptID(dept);
            }
            strDepts = strDepts.Length > 0 && strDepts.EndsWith(",") ? strDepts.Substring(0, strDepts.Length - 1) : strDepts;
            ViewBase vwDept = new ViewDepartment();
            string strCondition = strDepts.Length > 0 ? "(a.ID IN (" + strDepts + ") OR a.ID IN(" + this.Depts.IDs + "))" : " a.ID IN(" + this.Depts.IDs + ")";
            if (iFloorCode == 0)
            {
                strCondition = " a.ID IN(" + this.Depts.IDs + ")";
            }
            else if (iFloorCode > 0)
            {
                strCondition += " AND a.FloorCode =" + iFloorCode.ToString();
            }
            vwDept.BaseCondition = strCondition;
            return vwDept;
        }

        #endregion
    
        #region 通过部门和职位获得角色字符串方法
       
        /// <summary>
        /// 1.所在部门的角色
        /// 2.所在部门所在职位的角色
        /// 3.所在职位的角色
        /// </summary>
        /// <returns></returns>
        private string GetRoleByDeptID()
        {
            string strRoles = string.Empty;
            
            //1.所在部门
            string strDeptSql = string.Empty;
            //2.所在部门所在职位
            string strDeptPostSql = string.Empty;
            //3.所在职位
            string strPostSql = string.Empty;

            //一.所在部门
            ViewBase vbDepts = this.Depts;
            foreach (Department depts in vbDepts.Ens)
            {
                if (strDeptSql.Length > 0)
                {
                    strDeptSql += " OR ";
                }
                strDeptSql += " FK_DeptID = '" + depts.ID + "' OR FK_DeptID Like '" + depts.ID + ",%' OR FK_DeptID Like '%," + depts.ID + ",%' OR FK_DeptID Like '%," + depts.ID + "'";
            }

            //二.所在部门所在职位
            ViewBase vbPosts = this.DeptPosts;          
            foreach(DataRow dr in vbPosts.DtTable.Rows)           
            {
                if (strDeptPostSql.Length > 0)
                {
                    strDeptPostSql += " OR ";
                }
                strDeptPostSql += " FK_PostID = '" + dr["PostID"].ToString() + "' OR FK_PostID Like '" + dr["PostID"].ToString() + ",%' OR FK_PostID Like '%," + dr["PostID"].ToString() + ",%' OR FK_PostID Like '%," + dr["PostID"].ToString() + "'";
            }

            strPostSql = strDeptPostSql;
            
            strDeptPostSql = "(" + strDeptSql + ") AND (" + strDeptPostSql + ")";
            strDeptPostSql = " FK_PostID IS NOT NULL AND (" + strDeptPostSql + ")";//所在部门所在职位
            strDeptSql = " FK_PostID IS NULL AND (" + strDeptSql + ")";//所在部门

            //三.所在职位
            strPostSql = " FK_DeptID IS NULL AND (" + strPostSql + ")";//所在职位


            //获取这三种方式取得的角色
            string strSql = strDeptSql + " OR " + strDeptPostSql + " OR " + strPostSql;          
            DataTable dtRole = Entity.GetRecords(DeptRole.TableName, "FK_RoleID",strSql,false);
            foreach (DataRow dr in dtRole.Rows)
            {
                if (strRoles.Length > 0)
                {
                    strRoles += ",";
                }
                strRoles += dr["FK_RoleID"].ToString();
            }
            return strRoles;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="strIDs">删除的ID,用','连接</param>
        /// <param name="bActual">物理删除还是逻辑删除</param>
        /// <returns></returns>
        public static bool Delete(string strIDs, bool bActual)
        {
            string strSql = string.Format("[ID] IN ({0})", strIDs);
            return Entity.Delete(User.TableName, strSql, bActual) > 0;
        }

        /// <summary>
        /// 删除这个用户所关联的数据
        /// </summary>
        /// <returns></returns>
        private bool HideRelation(string strIDs,int iFlag)
        {
            bool bFlag = true;
            base.EnTrans.Begin();
            string strDelDpu = string.Format(" UPDATE " + DeptPost.TableName + " SET RecordStatus = " + iFlag + " WHERE [FK_UserID] IN ({0})", strIDs);
            string strDelRuser = string.Format("  UPDATE " + RoleUser.TableName + " SET RecordStatus = " + iFlag + "  WHERE [FK_UserID] IN ({0})", strIDs);
            int iStatus = iFlag == 2 ? 0 : 1;
            string strDelUser = string.Format("  UPDATE " + User.TableName + " SET IsCancel = " + iStatus + "  WHERE [ID] IN ({0})", strIDs);
            try
            {
                base.EnTrans.ExeCmd(strDelDpu);
                base.EnTrans.ExeCmd(strDelRuser);
                base.EnTrans.ExeCmd(strDelUser);
                base.EnTrans.Commit();
            }
            catch
            {
                bFlag = false;
                base.EnTrans.Rollback();
            }
            return bFlag;
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="strIDs">strIDs用户ID,用","连接</param>
        /// <returns></returns>
        public bool Hide(string strIDs)
        {
            bool bFlag = this.HideRelation(strIDs,2);
            return bFlag;
        }

        /// <summary>
        /// 用户状态设为启用
        /// </summary>
        /// <param name="strIDs">strIDs用户ID,用","连接</param>
        /// <returns></returns>
        public bool Use(string strIDs)
        {
            bool bFlag = this.HideRelation(strIDs, 1);
            return bFlag;
        }

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="strIDs">strIDs用户ID,用","连接</param>
        /// <returns></returns>
        public bool Delete(string strIDs)
        {
            bool bFlag = this.DelRelation(strIDs);
            return bFlag;
        }

        /// <summary>
        /// 判断是否该人是否关联了角色
        /// </summary>
        /// <returns></returns>
        private bool BeforDelete()
        {
            return Entity.GetRecordCount(RoleUser.TableName, "FK_RoleID = '" + base.ID + "'", false) > 0;
        }

        /// <summary>
        /// 删除这个用户所关联的数据
        /// </summary>
        /// <returns></returns>
        private bool DelRelation(string strIDs)
        {
            bool bFlag = true;
            base.EnTrans.Begin();
            string strDelDpu = string.Format(" DELETE FROM " + BLL.Busi.DeptPost.TableName + " WHERE [FK_UserID] IN ({0})", strIDs);
            string strDelRuser = string.Format(" DELETE FROM " + RoleUser.TableName + " WHERE [FK_UserID] IN ({0})", strIDs);
            string strDelUser = string.Format(" DELETE FROM " + User.TableName + " WHERE [ID] IN ({0})", strIDs);
            try
            {
                base.EnTrans.ExeCmd(strDelDpu);
                base.EnTrans.ExeCmd(strDelRuser);
                base.EnTrans.ExeCmd(strDelUser);
                base.EnTrans.Commit();
            }
            catch
            {
                bFlag = false;
                base.EnTrans.Rollback();
            }
            return bFlag;
        }

        /// <summary>
        /// 删除这个用户所关联的数据
        /// </summary>
        /// <returns></returns>
        private bool DeleteRelation()
        {
            bool bFlag = true;
            base.EnTrans.Begin();
            string strDeleteSql = string.Format(" DELETE FROM " + BLL.Busi.DeptPost.TableName + " WHERE [FK_UserID] = '{0}'", base.ID);
            try
            {
                base.EnTrans.ExeCmd(strDeleteSql);
            }
            catch
            {
                bFlag = false;
                base.EnTrans.Rollback();
            }

            if (bFlag)
            {
                strDeleteSql = string.Format(" DELETE FROM " + RoleUser.TableName + " WHERE [FK_UserID] = '{0}'", base.ID);
                try
                {
                    base.EnTrans.ExeCmd(strDeleteSql);
                }
                catch
                {
                    bFlag = false;
                    base.EnTrans.Rollback();
                }
                if (bFlag)
                {
                    strDeleteSql = string.Format(" DELETE FROM " + User.TableName + " WHERE [ID] = '{0}'", base.ID);
                    try
                    {
                        base.EnTrans.ExeCmd(strDeleteSql);
                    }
                    catch
                    {
                        bFlag = false;
                        base.EnTrans.Rollback();
                    }
                }
            }
            if (bFlag)
            {
                base.EnTrans.Commit();
            }
            return bFlag;
        }

        #endregion

        #region 内部功能代码

        /// <summary>
        /// 保存部门职位
        /// </summary>
        /// <param name="subList">部门职位</param>
        /// <returns></returns>
        private bool SaveDeptSub(List<DeptPost> subList)
        {
            bool bFlag = true;
            if (subList != null)
            {
                string sql = string.Format("Delete from " + DeptPost.TableName + " WHERE FK_UserID = '{0}'", this.ID);
                Entity.RunNoQuery(sql);
                foreach (DeptPost dpost in subList)
                {
                    if (dpost.FK_DeptID != int.MinValue)
                    {
                        dpost.FK_UserID = this.ID;
                        bFlag = dpost.Save();
                    }
                }
            }
            return bFlag;
        }

        /// <summary>
        /// 保存角色用户
        /// </summary>
        /// <param name="strRoleIDs">角色,用','分搁</param>
        /// <returns></returns>
        private bool SaveRoleUser(string strRoleIDs)
        {
            bool bFlag = true;
            if (strRoleIDs.Length > 0)
            {
                string[] Ids = strRoleIDs.Split(';');
                for (int i = 0; i < Ids.Length; i++)
                {
                    RoleUser roleUser = new RoleUser();
                    roleUser.FK_UserID = base.ID;
                    roleUser.FK_RoleID = int.Parse(Ids[i]);
                    if (roleUser.Save() == false)
                    {
                        bFlag = false;
                    }
                }
            }
            return bFlag;
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="subList">部门职位</param>
        /// <param name="strRoleIDs">角色ID,逗号搁开</param>
        /// <returns></returns>
        public bool SaveUser(List<DeptPost> subList, string strRoleIDs)
        {
            return this.Save() && this.SaveDeptSub(subList) && this.SaveRoleUser(strRoleIDs);
        }

        /// <summary>
        /// 保存前检查
        /// wangmx20090928
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeSaveCheck()
        {
            bool r = true;
            if (SysUtility.FilteStr(this.Name) == false)
            {
                base.ErrMsgs.Add("名字有危险字符");
                return false;
            }

            if (string.IsNullOrEmpty(this.UserID) == true)
            {
                r = false;
                base.ErrMsgs.Add("人员账号不能为空");
            }

            if (Common.IsSameRecord(User.TableName, " AND UserID='" + this.UserID + "' And ID <> " + base.ID.ToString()))
            {
                r = false;
                base.ErrMsgs.Add("人员账号已经被人使用");
            }

            if (this.UserID.Length > 20)
            {
                r = false;
                base.ErrMsgs.Add("人员账号不能超过20个字符");
            }

            if (this.No.Length > 20)
            {
                r = false;
                base.ErrMsgs.Add("人员编号号不能超过20个字符");
            }

            if (string.IsNullOrEmpty(this.Name) == true)
            {
                r = false;
                base.ErrMsgs.Add("人员名不能为空");
            }

            if (this.Name.Length > 20)
            {
                r = false;
                base.ErrMsgs.Add("人员名不能过长");
            }

            if (this.Remark.Length > 500)
            {
                r = false;
                base.ErrMsgs.Add("备注不能过长");
            }

            return r;
        }

        /// <summary>
        /// 排序 更新索引
        /// </summary>
        /// <param name="strIDs">用户ID</param>
        /// <returns></returns>
        public static bool Sort(string strIDs)
        {
            string strSql = string.Empty;
            string[] strArray = strIDs.Split(',');
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strSql.Length > 0)
                {
                    strSql += ";";
                }
                strSql += "Update " + User.TableName + " set SortNum =" + (i + 1) + " WHERE ID=" + strArray[i].ToString();
            }
            return Entity.RunNoQuery(strSql) > 0;
        } 

        /// <summary>
        /// 子实体list
        /// </summary>
        private List<DeptPost> m_subList;
        private string strRoleIDs = string.Empty;

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="subList">部门职位list</param>
        /// <param name="strRoles">角色,用逗号搁开</param>
        /// <returns></returns>
        public bool Save(List<DeptPost> subList,string strRoles)
        {
            m_subList = subList;
            this.strRoleIDs = strRoles;
            return base.Save();
        }

        /// <summary>
        /// 根据角色ID得到角色下的所有人
        /// 王敏贤
        /// 20091014
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public static DataTable GetUserByRoleID(int roleID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.ID,a.FK_RoleID,a.FK_UserID,b.Name,b.RecordStatus ");
            sql.Append("FROM " + RoleUser.TableName + " a  ");
            sql.Append("INNER JOIN " + User.TableName + " b ON  a.FK_UserID =  b.ID ");
            sql.Append("WHERE 1 =1 ");
            if (roleID > 0)
            {
                sql.Append(" and a.FK_RoleID = " + roleID + "");
            }
            sql.Append(" ORDER BY ID");
            return Entity.RunQuery(sql.ToString());
        }

        /// <summary>
        /// 返回某部门（大于，小于，等于..）某角色的人员
        /// </summary>
        /// <param name="strRoleName">角色名称(多个以“，”分隔)</param>
        /// <param name="enumOp">操作符枚举</param>
        /// <param name="strDeptId">部门ID（多个以“，”分隔）</param>
        /// <param name="iFloorCode">iFCode:部门层 0自己,>0 子部门层数,-1所有</param>
        /// <returns></returns>
        public static ViewBase GetUserByRole(string strRoleName, Common.Operators enumOp, string strDeptId, int iFloorCode)
        {
            ViewUser vUser = new ViewUser(true);
            vUser.Field = @" Distinct a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                           a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,
                           CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";
            vUser.SetJoin();
            StringBuilder strWhere = new StringBuilder(100);
            strWhere.Append(" A.RecordStatus = 1 and RU.RecordStatus = 1 and R.RecordStatus = 1 and DPU.RecordStatus = 1 and A.IsCancel = 1 ");

            if (!string.IsNullOrEmpty(strRoleName))
            {
                string[] strRoleNames = strRoleName.Split(',');
                for (int i = 0; i < strRoleNames.Length; i++)
                {
                    if (enumOp == Common.Operators.ne)
                    {
                        strWhere.Append(" and R.Name " + Common.GetOperator(enumOp) + "'" + strRoleNames[i] + "'");
                    }
                    else
                    {
                        if (i == 0)
                        {
                            strWhere.Append(" and ( R.Name " + Common.GetOperator(enumOp) + "'" + strRoleNames[i] + "'");
                        }
                        else
                        {
                            strWhere.Append(" or R.Name " + Common.GetOperator(enumOp) + "'" + strRoleNames[i] + "'");
                        }
                    }
                }

                if (enumOp != Common.Operators.ne)
                {
                    strWhere.Append(" ) ");
                }
            }

            if(!string.IsNullOrEmpty(strDeptId))
            {
                strWhere.Append(" and DPU.FK_DeptID in ( ");
                string[] strDeptIds = strDeptId.Split(',');
                Department enDept = new Department();
                for (int i = 0; i < strDeptIds.Length; i++)
                {
                    strWhere.Append(strDeptIds[i] + "," + enDept.GetChildDeptID(int.Parse(strDeptIds[i]), iFloorCode));
                    if (!strWhere.ToString().EndsWith(","))
                    {
                        strWhere.Append(",");
                    }
                }
                strWhere.Remove(strWhere.Length - 1,1);
                strWhere.Append(" ) ");
            }
            
            vUser.BaseCondition = strWhere.ToString();
            return vUser;
        }

        /// <summary>
        /// 返回某部门（大于，小于，等于..）某职位的人员
        /// </summary>
        /// <param name="strPostName">职位名称(多个以“，”分隔)</param>
        /// <param name="enumOp">操作符枚举</param>
        /// <param name="strDeptId">部门ID（多个以“，”分隔）</param>
        /// <param name="iFloorCode">iFCode:部门层 0自己,>0 子部门层数,-1所有</param>
        /// <returns></returns>
        public static ViewBase GetUserByPosition(string strPostName, Common.Operators enumOp, string strDeptId, int iFloorCode)
        {
            ViewUser vUser = new ViewUser(false);
            vUser.Field = @" Distinct a.ID,a.NO,a.Name,a.Domain,a.UserID,a.PWD,a.OfficePhone,a.MobilePhone,a.Email,a.SortNum,
                           a.Remark,a.EditDate,D_Class,a.ID AS UID,a.UserID AS ADCode,
                           CASE a.IsCancel WHEN '1' THEN '启用' ELSE '注销'END AS HideStatue";
            StringBuilder strWhere = new StringBuilder(100);
            strWhere.Append("A.RecordStatus = 1 and E.RecordStatus = 1 and B.RecordStatus = 1 and A.IsCancel = 1");

            if (!string.IsNullOrEmpty(strPostName))
            {
                string[] strPostNames = strPostName.Split(',');
                for (int i = 0; i < strPostNames.Length; i++)
                {
                    Position enPost = Position.GetPosition(strPostNames[i]);
                    if(enPost == null)
                    {
                        return null;
                    }
                    if (enumOp == Common.Operators.ne)
                    {
                        strWhere.Append(" and '" + enPost.SortNum + "' " + Common.GetOperator(enumOp) + " E.SortNum ");
                    }
                    else
                    {
                        if (i == 0)
                        {
                            strWhere.Append(" and ( '" + enPost.SortNum + "' " + Common.GetOperator(enumOp) + " E.SortNum ");
                        }
                        else
                        {
                            strWhere.Append(" or '" + enPost.SortNum + "' " + Common.GetOperator(enumOp) + " E.SortNum ");
                        }
                    }
                }

                if (enumOp != Common.Operators.ne)
                {
                    strWhere.Append(" ) ");
                }
            }

            if (!string.IsNullOrEmpty(strDeptId))
            {
                strWhere.Append(" and B.FK_DeptID in ( ");
                string[] strDeptIds = strDeptId.Split(',');
                Department enDept = new Department();
                for (int i = 0; i < strDeptIds.Length; i++)
                {
                    strWhere.Append(strDeptIds[i] + "," + enDept.GetChildDeptID(int.Parse(strDeptIds[i]), iFloorCode));
                    if (!strWhere.ToString().EndsWith(","))
                    {
                        strWhere.Append(",");
                    }
                }
                strWhere.Remove(strWhere.Length - 1, 1);
                strWhere.Append(" ) ");
            }
            vUser.BaseCondition = strWhere.ToString();
            return vUser;
        }
        #endregion
    }
}