//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述职位信息:职位的部门;通过职位名获得职位;
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：
// 修改描述：
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
    /// 描述职位信息:职位的部门;通过职位名获得职位;
    /// </summary>
    public class Position : GenePosition
    {
        #region Define

        /// <summary>
        /// 部门视图
        /// </summary>
        private ViewBase m_vwDept = new ViewDepartment();

        #endregion

        #region 刷新相关实体

        /// <summary>
        /// 刷新相关实体
        /// </summary>
        protected override void RefreshObjects()
        {
            this.m_vwDept.BaseCondition = "d.Name = '" + base.Name + "'";
        }

        #endregion

        #region 根据职位名称 获得所属的部门,自动生成职位流水号

        /// <summary>
        /// 所属部门(哪些部门具有当前职位)
        /// </summary>
        public ViewBase Depts
        {
            get { return this.m_vwDept; }
        }

        /// <summary>
        /// 根据职位名称获得大于该职位的职位
        /// </summary>
        /// <param name="sPostName">职位名称</param>
        /// <returns></returns>
        public static ViewBase GetPositions(string sPostName)
        {
            Position post = Position.GetPosition(sPostName);
            ViewPost vwPost = new ViewPost(true);
            vwPost.BaseCondition = post == null ? "1<>1" : "SortNum <=" + post.SortNum.ToString();
            return vwPost;
        }

        #endregion

        #region 通过主键ID,职位名获得职位对象

        /// <summary>
        /// 通过职位id返回职位对象
        /// </summary>
        /// <param name="iPostID">职位id</param>
        /// <returns></returns>
        public static Position GetPosition(int iPostID)
        {
            ViewPost vwPost = new ViewPost(true);
            vwPost.BaseCondition = "a.ID='" + iPostID + "'";
            return vwPost.Count > 0 ? vwPost.GetItem(0) as Position : null;
        }

        /// <summary>
        /// 通过职位名返回职位对象
        /// </summary>
        /// <param name="strPostName">职位名</param>
        /// <returns></returns>
        public static Position GetPosition(string strPostName)
        {
            ViewPost vwPost = new ViewPost(true);
            vwPost.BaseCondition = "a.Name='" + strPostName + "'";
            return vwPost.Count > 0 ? vwPost.GetItem(0) as Position : null;
        }

        /// <summary>
        /// 新增记录时生成职位流水号
        /// </summary>
        public static string GetFlowCode()
        {
            string strSql = "Select Max(No) AS No from " + Position.TableName;
            DataTable dt = Entity.RunQuery(strSql);
            int iMaxID = dt.Rows[0][0].ToString() != string.Empty ? Convert.ToInt32(dt.Rows[0][0]) + 1 : 1;
            return iMaxID.ToString();
        }

        /// <summary>
        /// 获得所有职位
        /// </summary>
        /// <returns></returns>
        public static ViewBase GetAllPosition()
        {
            ViewPost vPost = new ViewPost(true);
            return vPost;
        }

        #endregion

        #region 内部功能代码

        /// <summary>
        /// 获取职位信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSortNumDT(int iId)
        {
            string strSql = "SELECT Distinct MaxSortNum,MinSortNum FROM " + Position.TableName + " WHERE MaxSortNum>=0 AND MinSortNum>=0 AND ID <>" + iId.ToString();
            DataTable dt = Entity.RunQuery(strSql);
            return dt;
        }

        /// <summary>
        /// 验证职位上下限是否在原来的范围内
        /// </summary>
        /// <param name="sortNum"></param>
        /// <returns></returns>
        private bool CheckSortNum(int sortNum)
        {
            bool bFlag = true;
            if (sortNum != -1)
            {
                DataTable dt = Position.GetSortNumDT(base.ID);
                foreach (DataRow dr in dt.Rows)
                {
                    if (sortNum <= Convert.ToInt32(dr["MinSortNum"]) && sortNum >= Convert.ToInt32(dr["MaxSortNum"]))
                    {
                        bFlag = false;
                        break;
                    }
                }
            }
            return bFlag;
        }

        /// <summary>
        /// 验证职位上下限是否跨越多个区间
        /// </summary>
        /// <returns></returns>
        private bool CheckSortNum()
        {
            bool bFlag = true;
            if (base.MinSortNum > 0 && base.MaxSortNum > 0)
            {
                DataTable dt = Position.GetSortNumDT(base.ID);
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["MinSortNum"]) < base.MinSortNum && Convert.ToInt32(dr["MinSortNum"]) > base.MaxSortNum)
                    {
                        bFlag = false;
                        break;
                    }
                }
            }
            return bFlag;
        }

        /// <summary>
        /// 保存数据前进行合法性验证
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeSaveCheck()
        {
            bool bActual = true;
            if (SysUtility.FilteStr(this.Name) == false)
            {
                base.ErrMsgs.Add("职位名称有危险字符");
                bActual = false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                base.ErrMsgs.Add("职位名不能为空");
                bActual = false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                base.ErrMsgs.Add("职位名不能为空");
                bActual = false;
            }

            //ID < 0 表示新增记录
            if (Common.IsSameRecord(Position.TableName, " AND Name = '" + this.Name + "' AND ID<>'" + base.ID.ToString() + "'"))
            {
                base.ErrMsgs.Add("职位名重复");
                bActual = false;
            }

            if (base.MaxSortNum < 0)
            {
                base.ErrMsgs.Add("职位上限不能为负数");
                bActual = false;
            }

            if (base.MinSortNum < 0)
            {
                base.ErrMsgs.Add("职位下限不能为负数");
                bActual = false;
            }

            if (base.MinSortNum < base.MaxSortNum)
            {
                base.ErrMsgs.Add("职位下限必须大于职位上限");
                bActual = false;
            }

            if (this.CheckSortNum(base.MaxSortNum) == false)
            {
                base.ErrMsgs.Add("职位上限在其他职位区间内");
                bActual = false;
            }

            if (this.CheckSortNum(base.MinSortNum) == false)
            {
                base.ErrMsgs.Add("职位下限在其他职位区间内");
                bActual = false;
            }

            if (this.CheckSortNum() == false)
            {
                base.ErrMsgs.Add("职位上下限跨越多个区间");
                bActual = false;
            }

            if (base.SortNum >= base.MinSortNum || base.SortNum <= base.MaxSortNum)
            {
                base.ErrMsgs.Add("默认序号必须在职位上限和职位下限范围内");
                bActual = false;
            }

            if (this.Remark.Length > 500)
            {
                base.ErrMsgs.Add("备注不能过长");
                bActual = false;
            }

            return bActual;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="strIDs">删除的ID,用','连接</param>
        /// <param name="bActual">物理删除还是逻辑删除</param>
        /// <returns></returns>
        public static bool Delete(string strIDs, bool bActual)
        {
            string strSql = string.Format("[ID] IN ({0})", strIDs);
            return Entity.Delete(Position.TableName, strSql, bActual) > 0;
        }

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <returns></returns>
        public new bool Delete()
        {
            string strPostSql = " AND (FK_PostID = '" + base.ID.ToString() + "' OR FK_PostID Like '" + base.ID.ToString() + ",%' OR FK_PostID Like '%," + base.ID.ToString() + ",%' OR FK_PostID Like '%," + base.ID.ToString() + "')";
            bool bActual = false;
            string strDpu = string.Format(" AND FK_PostID = '{0}' AND RecordStatus=1 ", base.ID);
            if (!Common.IsSameRecord(DeptPost.TableName, strDpu) && !Common.IsSameRecord(DeptRole.TableName, strPostSql))
            {
                strDpu = string.Format("DELETE FROM " + Position.TableName + "  WHERE [ID] = '{0}'", base.ID);
                bActual = Entity.RunNoQuery(strDpu) > 0;
            }
            return bActual;
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
                strSql += "Update " + Position.TableName + " set SortNum = " + (i + 1).ToString() + " WHERE ID = " + strArray[i] + ";";
            }
            return Entity.RunNoQuery(strSql) > 0;
        }

        /// <summary>
        /// 获取默认职位ID
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultPost()
        {
            ViewPost vbPost = new ViewPost();
            vbPost.BaseCondition = "a.Name = '员工'";
            return vbPost.Count > 0 ? vbPost.GetItem(0).ID : -1;
        }

        #endregion
    }
}