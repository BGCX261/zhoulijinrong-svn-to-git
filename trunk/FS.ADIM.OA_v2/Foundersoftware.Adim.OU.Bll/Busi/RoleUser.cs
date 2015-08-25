//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述角色:角色下的人
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using FounderSoftware.ADIM.OU.BLL.AutoGene;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 描述角色:角色下的人
    /// </summary>
    public class RoleUser : GeneRoleUser
    {
        #region Operate Datas

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="ids">ids</param>
        /// <param name="bActual">物理删除还是逻辑删除</param>
        /// <returns></returns>
        public static bool Delete(string ids, bool bActual)
        {
            string strSql = string.Format("[ID] IN ({0})", ids);
            return Entity.Delete(RoleUser.TableName, strSql, bActual) > 0;
        }

        /// <summary>
        /// 功能说明：保存记录
        /// </summary>
        /// <param name="roleMember"></param>
        /// <returns></returns>
        public bool SaveDal(RoleUser roleMember)
        {
            string insertSql = string.Format(@"INSERT INTO {0} (ID,FK_RoleID,FK_UserID,RecordStatus) 
                                               VALUES('{1}','{2}','{3}',{4})", RoleUser.TableName, roleMember.ID, roleMember.FK_RoleID, roleMember.FK_UserID, "1");
            int i = Entity.RunNoQuery(insertSql);
            return i > 0;
        }

        #endregion
    }
}