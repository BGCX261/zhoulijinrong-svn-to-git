//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述部门角色
// 
// 创建标识：2009-11-30 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
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
    /// 描述部门角色
    /// </summary>
    public class DeptRole : GeneDeptRole
    {
        #region Check Data

        /// <summary>
        /// 重复性验证
        /// </summary>
        /// <param name="iRoleID">角色ID</param>
        /// <returns></returns>
        public bool CheckSameRecord(string iRoleID)
        {
            bool bActual = false;
            ViewDeptRole vbDeptRole = new ViewDeptRole();
            vbDeptRole.BaseCondition = "b.FK_RoleID=" + iRoleID.ToString();
            if (vbDeptRole.Count > 0)
            {
                bActual = true;
            }
            return bActual;
        }

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeSaveCheck()
        {
            bool bRet = true;
            if (string.IsNullOrEmpty(base.FK_RoleID.ToString()))
            {
                base.ErrMsgs.Add("角色不能为空");
                bRet = false;
            }
            if (string.IsNullOrEmpty(this.FK_PostID) && string.IsNullOrEmpty(this.FK_DeptID))
            {
                base.ErrMsgs.Add("部门职位不能都为空");
                bRet = false;
            }
            return bRet;
        }

        #endregion

        #region 数据操作

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public bool SaveS()
        {
            bool suc = false;
            if (this.BeforeSaveCheck())
            {
                string strSql = string.Empty;
                strSql = string.Format(@"delete from " + DeptRole.TableName + " where FK_RoleID='{0}'", base.FK_RoleID);
                suc = Entity.RunNoQuery(strSql) > 0;               
                suc = base.Save();
            }
            return suc;
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
            bool bRet = Entity.Delete(DeptRole.TableName, strSql, bActual) > 0;
            return bRet;
        }

        #endregion

        #region 获得部门角色对象

        /// <summary>
        /// 根据RoleID获得对象
        /// </summary>
        /// <param name="iRoleID">角色ID</param>
        /// <returns></returns>
        public static DeptRole GetDeptRole(int iRoleID)
        {
            DeptRole deptRole = null;
            ViewBase vbDeptRole = new ViewDeptRole();
            vbDeptRole.BaseCondition = "b.ID=" + iRoleID.ToString();
            if (vbDeptRole.Count > 0)
            {
                deptRole = vbDeptRole.GetItem(0) as DeptRole;
            }
            return deptRole;
        }

        #endregion
    }
}