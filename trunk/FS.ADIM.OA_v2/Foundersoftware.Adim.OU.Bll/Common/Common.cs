//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：公共方法
// 
// 创建标识：2009-11-12 王敏贤
//
// 修改标识：2009-12-21 胥寿春
// 修改描述：代码重构
//
//----------------------------------------------------------------

using System;
using System.Data;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OU.BLL
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public class Common
    {
        #region LeaderType, UserStatus

        /// <summary>
        /// 部门领导类型枚举
        /// </summary>
        public enum LeaderType
        {
            /// <summary>
            /// 部门领导
            /// </summary>
            Leader = 1,

            /// <summary>
            /// 部门负责人
            /// </summary>
            Manager = 2,

            /// <summary>
            /// 既是领导也是负责人
            /// </summary>
            LeaderAndManager = 3,

            /// <summary>
            /// 普通用户
            /// </summary>
            User = 4,
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserStatus
        {
            /// <summary>
            /// 已注销
            /// </summary>
            Canceled = 0,

            /// <summary>
            /// 正常,启用状态
            /// </summary>
            Normal = 1,
        }

        #endregion

        #region IsSameRecord

        /// <summary>
        /// 判断数据是否重复
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="strSqlCondition">过滤条件</param>
        /// <returns></returns>
        public static bool IsSameRecord(string tbName, string strSqlCondition)
        {
            string strSql = "SELECT COUNT(1) FROM " + tbName + " WHERE 1=1 ";
            if (strSqlCondition.Length > 0)
            {
                strSql += strSqlCondition;
            }           
            DataTable dt = Entity.RunQuery(strSql);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        /// <summary>
        /// 操作符枚举
        /// </summary>
        public enum Operators
        {
            /// <summary>
            /// 大于
            /// </summary>
            gt = 1,
            /// <summary>
            /// 等于
            /// </summary>
            eq = 2,
            /// <summary>
            /// 小于
            /// </summary>
            lt = 4,
            /// <summary>
            /// 小于等于
            /// </summary>
            le = 8,
            /// <summary>
            /// 大于等于
            /// </summary>
            ge = 16,
            /// <summary>
            /// 不等于
            /// </summary>
            ne = 32,
        }

        /// <summary>
        /// 根据枚举值返回操作符
        /// </summary>
        /// <param name="op">枚举值</param>
        /// <returns></returns>
        public static string GetOperator(Operators op)
        {
            switch(op)
            {
                case Operators.gt:
                    {
                        return ">";
                    }
                case Operators.lt:
                    {
                        return "<";
                    }
                case Operators.eq:
                    {
                        return "=";
                    }
                case Operators.le:
                    {
                        return "<=";
                    }
                case Operators.ge:
                    {
                        return ">=";
                    }
                case Operators.ne:
                    {
                        return "!=";
                    }
                default:
                    {
                        return "";
                    }
            }
        }
        #endregion
    }
}