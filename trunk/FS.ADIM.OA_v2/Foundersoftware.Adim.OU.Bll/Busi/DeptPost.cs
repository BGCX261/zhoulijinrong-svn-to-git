//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述部门职位
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：
// 修改描述：修改GetUdp()方法 LeaderType加 try...catch
//
// 修改标识：20101-7 王敏贤
// 修改描述：设置领导和负责人SetUserLeaderType,设置职位 SetPosition
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using FounderSoftware.ADIM.OU.BLL.AutoGene;
using FounderSoftware.ADIM.OU.BLL.View;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 描述部门职位
    /// </summary>
    public class DeptPost : GeneDeptPost
    {
        #region Define

        string m_strDeptHtml = string.Empty;

        /// <summary>
        /// 部门实体
        /// </summary>
        public Department Dept
        {
            get
            {
                Department dept = Department.GetDepartment(this.FK_DeptID);
                return dept;
            }
        }

        /// <summary>
        /// 职位实体
        /// </summary>
        public Position Post
        {
            get
            {
                Position post = Position.GetPosition(this.FK_PostID);
                return post;
            }
        }

        #endregion

        #region Check Data,获得用户所有职位

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="strIds">ids</param>
        /// <param name="iDeptID">iDeptID</param>
        /// <param name="bActual">物理删除还是逻辑删除</param>
        /// <returns></returns>
        public static bool Delete(string strIds, int iDeptID, bool bActual)
        {
            string strSql = string.Format("[ID] IN ({0})", strIds);
            return Entity.Delete(DeptPost.TableName, strSql, bActual) > 0;
        }

        #endregion

        #region 设置用户领导负责人,修改用户排序号

        /// <summary>
        /// 设置领导和负责人
        /// </summary>
        /// <param name="mType">领导或负责人枚举</param>
        /// <param name="strIDs">用户IDs</param>
        /// <param name="bSetOrReset">取消或者设置</param>
        /// <param name="strMessage">错误消息</param>
        /// <returns></returns>
        public static bool SetUserLeaderType(Common.LeaderType mType, List<string> strIDs, bool bSetOrReset,out string strMessage)
        {
            bool bRet = false;
            strMessage = string.Empty;
            DeptPost dp = new DeptPost();
            User user = new User();
            ViewDeptPost v_dp = new ViewDeptPost();
            dp.EnTrans.Begin();
            dp.EnTrans.IsBatch = true;
            foreach (string strID in strIDs)
            {
                if (strID.Length == 0) continue;
                bRet = dp.Load(Convert.ToInt32(strID));

                if (mType == Common.LeaderType.Manager && bSetOrReset)
                {
                    v_dp.EnTrans = dp.EnTrans;
                    v_dp.Field = "a.ID";
                    v_dp.Condition = " FK_DeptID = " + dp.FK_DeptID + " and LeaderType in (" + (int)Common.LeaderType.Manager + "," + (int)Common.LeaderType.LeaderAndManager + ")";

                    if (v_dp.DtTable != null)
                    {
                        if (v_dp.DtTable.Rows.Count > 0)
                        {
                            user.EnTrans = dp.EnTrans;
                            user.Load(dp.FK_UserID);
                            bRet = false;
                            strMessage = string.Format("({0})所在的部门以存在负责人",user.Name);
                            return bRet;
                        }
                    }
                }
                if (dp.LeaderType == int.MinValue)
                {
                    dp.LeaderType = 0;
                }
                if (bSetOrReset)
                {
                    dp.LeaderType |= (int)mType;
                }
                else
                {
                    if (dp.LeaderType == (int)mType || (dp.LeaderType == ((int)(Common.LeaderType.Leader | Common.LeaderType.Manager))))
                    {
                        dp.LeaderType ^= (int)mType;
                    }
                }
                bRet = dp.Save();
            }
            dp.EnTrans.Commit();
            return bRet;
        }

        /// <summary>
        /// 修改用户排序号
        /// </summary>
        /// <param name="iUserID">用户ID</param>
        /// <param name="post">职位对象</param>
        /// <returns></returns>
        private static bool UpdateUserSortNum(int iUserID, Position post)
        {
            User u = User.GetUser(iUserID);
            if (u != null)
            {
                ViewBase vbDeptPost = u.DeptPosts;
                if (vbDeptPost.Count > 0)
                {
                    int intMax = post.MaxSortNum;
                    int iDefaultSortNum = post.SortNum;
                    foreach (DeptPost dp in vbDeptPost.Ens)
                    {
                        if (dp.Post.MaxSortNum < intMax)
                        {
                            intMax = dp.Post.MaxSortNum;
                            iDefaultSortNum = dp.Post.SortNum;
                        }
                    }
                    u.SortNum = iDefaultSortNum;
                    bool b = u.Save();
                }
            }
            return true;
        }

        #endregion

        #region  内部方法

        /// <summary>
        /// 设置职位
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public static bool SetPosition(string postID, List<string> IDs)
        {
            bool bRet = false;
            if (string.IsNullOrEmpty(postID) == false)
            {
                DeptPost dp = new DeptPost();
                dp.EnTrans.Begin();
                dp.EnTrans.IsBatch = true;
                foreach (string strID in IDs)
                {
                    bRet = dp.Load(Convert.ToInt32(strID));
                    dp.FK_PostID = Convert.ToInt32(postID);
                    bRet = dp.Save() && DeptPost.UpdateUserSortNum(dp.FK_UserID, dp.Post);
                }
                dp.EnTrans.Commit();
            }
            return bRet;
        }

        /// <summary>
        /// 绑定部门
        /// </summary>
        public string BindDept1()
        {
            string strCount = "";
            ViewDepartment vbDepts = new ViewDepartment(true);
            vbDepts.BaseCondition = "a.ParentID=0";
            this.m_strDeptHtml = "<option value='-1'>├选择部门</option>";
            foreach (Department dept in vbDepts.Ens)
            {
                strCount = "";
                this.m_strDeptHtml += "<option value='" + dept.ID.ToString() + "'>" + "├" + dept.Name + "</option>";
                this.BindChildDept1(dept.ID.ToString(), strCount);
            }
            return m_strDeptHtml;
        }

        #region wangmx 2009-12-10

        /// <summary>
        /// 绑定部门,选中给定的部门ID的部门
        /// </summary>
        public string BindDept1(int iDeptID)
        {
            string strCount = "";
            ViewDepartment vbDepts = new ViewDepartment(true);
            vbDepts.BaseCondition = "a.ParentID=0";
            this.m_strDeptHtml = "<option value='-1'>├选择部门</option>";
            foreach (Department dept in vbDepts.Ens)
            {
                strCount = "";
                if (dept.ID == iDeptID)
                {
                    this.m_strDeptHtml += "<option selected value='" + dept.ID.ToString() + "'>" + "├" + dept.Name + "</option>";
                }
                else
                {
                    this.m_strDeptHtml += "<option value='" + dept.ID.ToString() + "'>" + "├" + dept.Name + "</option>";
                }

                this.BindChildDept1(dept.ID.ToString(), strCount, iDeptID);
            }
            return m_strDeptHtml;
        }

        /// <summary>
        /// 绑定子部门
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="strCount"></param>
        private void BindChildDept1(string pid, string strCount)
        {
            strCount = "";
            ViewDepartment vbDepts = new ViewDepartment(true);
            vbDepts.BaseCondition = "a.ParentID=" + pid;
            foreach (Department dept in vbDepts.Ens)
            {
                int level = int.Parse(dept.FloorCode.ToString());
                while (level != 0)
                {
                    strCount += "　";
                    level--;
                }
                this.m_strDeptHtml += "<option value='" + dept.ID.ToString() + "'>" + strCount + "├" + dept.Name + "</option>";
                this.BindChildDept1(dept.ID.ToString(), strCount);
                if (level == 0)
                {
                    strCount = "";
                }
            }
        }

        /// <summary>
        /// 绑定子部门
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="strCount"></param>
        /// <param name="seleceid"></param>
        private void BindChildDept1(string pid, string strCount, int seleceid)
        {
            strCount = "";
            ViewDepartment vbDepts = new ViewDepartment(true);
            vbDepts.BaseCondition = "a.ParentID=" + pid;
            foreach (Department dept in vbDepts.Ens)
            {
                int level = int.Parse(dept.FloorCode.ToString());
                while (level != 0)
                {
                    strCount += "　";
                    level--;

                }
                if (dept.ID == seleceid)
                {
                    this.m_strDeptHtml += "<option selected value='" + dept.ID.ToString() + "'>" + strCount + "├" + dept.Name + "</option>";
                }
                else
                {
                    this.m_strDeptHtml += "<option value='" + dept.ID.ToString() + "'>" + strCount + "├" + dept.Name + "</option>";
                }
                this.BindChildDept1(dept.ID.ToString(), strCount, seleceid);
                if (level == 0)
                {
                    strCount = "";
                }
            }
        }

        #endregion

        /// <summary>
        /// 绑定职位
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public string BindPosition2(string deptid)
        {
            ViewBase vbPost = new ViewPost();
            vbPost.BaseCondition = "a.ID<>0 ";
            vbPost.Sort = "a.SortNum DESC";
            string html = string.Empty;
            foreach (Position dp in vbPost.Ens)
            {
                html += "<option value='" + dp.ID.ToString() + "'>" + "├" + dp.Name + "</option>";
            }
            return html;
        }

        /// <summary>
        /// 绑定职位
        /// </summary>
        /// <param name="selectpPostID"></param>
        /// <returns></returns>
        public string BindPost2(int selectpPostID)
        {
            ViewBase vbPost = new ViewPost();
            vbPost.BaseCondition = "a.ID<>0 ";
            vbPost.Sort = "a.SortNum DESC";
            string html = string.Empty;
            html = "<option value='-1'>|-选择职位</option>";

            int i = 0;

            foreach (Position dp in vbPost.Ens)
            {
                if (dp.ID == selectpPostID)
                {
                    html += "<option selected  valueMax='" + dp.MaxSortNum.ToString() + "' valueMin='" + dp.MinSortNum.ToString() + "' valueMinDefault='"+dp.SortNum.ToString()+"' value='" + dp.ID.ToString() + "'>" + "├" + dp.Name + "</option>";
                }
                else
                {
                    html += "<option valueMax='" + dp.MaxSortNum.ToString() + "' valueMin='" + dp.MinSortNum.ToString() + "' valueMinDefault='"+dp.SortNum.ToString()+"' value='" + dp.ID.ToString() + "'>" + "├" + dp.Name + "</option>";
                }
                i += 50;
            }
            return html;
        }

        /// <summary>
        /// 根据userid返回相关部门信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<DeptPost> GetUdp(string userid)
        {
            List<DeptPost> dpuList = new List<DeptPost>();
            string strDeleteSql = string.Format(@"SELECT b.ID AS DeptID, b.Name AS DeptName, a.FK_PostID as PostID,a.LeaderType
	                                            FROM " + DeptPost.TableName + @" a
	                                            INNER JOIN " + Department.TableName + @" b ON a.FK_DeptID = b.ID 
	                                            Where a.RecordStatus=1 and a.FK_UserID = '" + userid + "'");
            DataTable dt = RunQuery(strDeleteSql);
            foreach (DataRow dr in dt.Rows)
            {
                DeptPost dpu = new DeptPost();
                dpu.LeaderType = 0;
                if (!string.IsNullOrEmpty(dr["DeptID"].ToString()))
                {
                    dpu.FK_DeptID = int.Parse(dr["DeptID"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["PostID"].ToString()))
                {
                    dpu.FK_PostID = int.Parse(dr["PostID"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["LeaderType"].ToString()))
                {
                    try
                    {
                        dpu.LeaderType = Convert.ToInt32(dr["LeaderType"]);
                    }
                    catch { }
                }
                dpuList.Add(dpu);
            }
            return dpuList;
        }

        /// <summary>
        /// 获取用户的最高职位序号
        /// </summary>
        /// <param name="iPostID">当前职位序号</param>
        /// <returns></returns>
        private int GetPostSortNum(int iPostID)
        {           
            User u = User.GetUser(this.FK_UserID);
            int iMaxSortNum = iPostID;
            if (u != null)
            {
                ViewBase vbDP = u.DeptPosts;
                for (int i = 0; i < vbDP.Count; i++)
                {
                    if ((vbDP.GetItem(i) as DeptPost).Post.SortNum > iMaxSortNum)
                    {
                        iMaxSortNum = (vbDP.GetItem(i) as DeptPost).Post.SortNum;
                    }
                }
            }
            return iMaxSortNum;
        }

        #endregion
    }
}