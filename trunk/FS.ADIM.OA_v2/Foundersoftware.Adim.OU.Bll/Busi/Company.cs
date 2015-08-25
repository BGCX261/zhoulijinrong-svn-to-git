//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：描述公司信息
// 
// 创建标识：2009-11-6 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using FounderSoftware.ADIM.OU.BLL.AutoGene;
using FounderSoftware.ADIM.OU.BLL.View;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 描述公司信息:根据公司名获得公司
    /// </summary>
    public class Company : GeneCompany
    {
        #region GetCompany

        /// <summary>
        /// 通过公司名获得公司对象
        /// </summary>
        /// <param name="strName">公司名,如果公司名不存在返回null</param>
        /// <returns></returns>
        public static Company GetCompany(string strName)
        {
            return Company.GetCompany("Name", strName);
        }

        /// <summary>
        /// 根据单位ID获得单位
        /// </summary>
        /// <param name="iID">公司ID,如果ID不存在返回null</param>
        /// <returns></returns>
        public static Company GetCompany(int iID)
        {
            return Company.GetCompany("ID", iID.ToString());
        }

        /// <summary>
        /// 获得公司对象
        /// </summary>
        /// <param name="strColName">列名</param>
        /// <param name="strValue">公司名称或公司ID</param>
        /// <returns></returns>
        private static Company GetCompany(string strColName, string strValue)
        {
            ViewBase vwCompany = new ViewCompany();
            vwCompany.BaseCondition = strColName + "='" + strValue + "'";
            return vwCompany.Count > 0 ? vwCompany.GetItemByIndex(0) as Company : null;
        }

        /// <summary>
        /// 根据单位名称,单位编号,单位联系获得单位
        /// 1：根据单位名称
        /// 2：根据单位编号
        /// 3：根据单位联系人
        /// </summary>
        /// <param name="strName">单位名称,单位编号,单位联系人</param>
        /// <param name="iSearchType">查询类别(根据单位名称,编号,联系人查询 1:单位名称;2:单位编号;3:单位联系人)</param>
        /// <returns></returns>
        public static ViewBase GetCompany(string strName, int iSearchType)
        {
            ViewBase vwComany = new ViewCompany();
            if (string.IsNullOrEmpty(strName) == false)
            {
                string strField = string.Empty;
                switch (iSearchType)
                {
                    case 1:
                    default:
                        strField = "Name";
                        break;

                    case 2:
                        strField = "No";
                        break;

                    case 3:
                        strField = "ContactPerson";
                        break;
                }
                vwComany.BaseCondition = strField + " LIKE '%" + strName + "%'";
            }
            return vwComany;
        }

        #endregion

        #region 内部功能代码

        /// <summary>
        /// 保存数据前进行合法性验证
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeSaveCheck()
        {
            bool bRet = true;
            if (string.IsNullOrEmpty(base.Name))
            {
                base.ErrMsgs.Add("单位名不能为空");
                bRet = false;
            }

            if (base.Name.Length > 80)
            {
                bRet = false;
                base.ErrMsgs.Add("单位名长度超出范围");
            }

            //重复验证
            if (Common.IsSameRecord(Company.TableName, " AND Name='" + base.Name + "' And ID <> " + base.ID.ToString()))
            {
                base.ErrMsgs.Add("单位名重复");
                bRet = false;
            }

            if (base.No.Length > 20)
            {
                bRet = false;
                base.ErrMsgs.Add("单位编码不能过长");
            }

            if (base.Remark.Length > 500)
            {
                bRet = false;
                base.ErrMsgs.Add("备注不能过长");
            }

            if (Validate.IsEnglish(base.EnglishName))
            {
                bRet = false;
                base.ErrMsgs.Add("英文名中只能是英文或 . ");
            }

            if (Validate.IsEmail(base.EmailAddress))
            {
                bRet = false;
                base.ErrMsgs.Add("请填写正确的email");
            }

            return bRet;
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
            return Entity.Delete(Company.TableName, strSql, bActual) > 0;
        }

        #endregion
    }
}