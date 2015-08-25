//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：数据验证类
// 
// 创建标识：2009-11-12 王敏贤
//
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{
    /// <summary>
    /// 公共静态方法类
    /// </summary>
    internal class SysUtility
    {
        #region 校验函数

        /// <summary>
        /// 判断集合是否为NULL或空集合
        /// </summary>
        /// <param name="collection">集合</param>
        /// <returns>结果</returns>
        public static bool IsNull(System.Collections.ICollection collection)
        {
            return (collection == null || collection.Count == 0);
        }

        /// <summary>
        /// 整数校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsInteger(string s)
        {
            return new Regex(@"^(0|-?[1-9]\d*)$").IsMatch(s);
        }

        /// <summary>
        /// 正整数校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsPositiveInteger(string s)
        {
            return new Regex(@"^(0|[1-9]\d*)$").IsMatch(s);
        }

        /// <summary>
        /// 负整数校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsNegativeInteger(string s)
        {
            return new Regex(@"^(0|-[1-9]\d*)$").IsMatch(s);
        }

        /// <summary>
        /// 整字校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsNumeric(string s)
        {
            return new Regex(@"^[-]?\d+[.]?\d*$").IsMatch(s);
        }

        /// <summary>
        /// Email校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsEmail(string s)
        {
            return new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(s);
        }

        /// <summary>
        /// 手机号校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsMobilePhone(string s)
        {
            return new Regex(@"^(\+(86)?)?1[358]\d{9}$").IsMatch(s);
        }

        /// <summary>
        /// 电话号校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsTelephone(string s)
        {          
            bool b1 = new Regex(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|((\(\d{3}\) ?)|(\d{4}-))?\d{4}-\d{4}$").IsMatch(s);
            bool b2 = new Regex(@"^((\(\d{4}\) ?)|(\d{4}-))?\d{4}-\d{4}|((\(\d{3}\) ?)|(\d{4}-))?\d{4}-\d{4}$").IsMatch(s);
            return b1 || b2;
        }

        /// <summary>
        /// 身份证号校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsIdNumber(string s)
        {
            return new Regex(@"^\d{15}$|^\d{18}$").IsMatch(s);
        }

        /// <summary>
        /// 网络地址校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsURL(string s)
        {
            return new Regex(@"^http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$").IsMatch(s);
        }

        /// <summary>
        /// 日期校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsDate(string s)
        {
            return new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$").IsMatch(s);
        }

        /// <summary>
        /// 日期时间校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsDateTime(string s)
        {
            return new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$").IsMatch(s);
        }

        /// <summary>
        /// 金额校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsMoney(string s)
        {
            return new Regex(@"^([0-9]|[0-9].[0-9]{0-2}|[1-9][0-9]*.[0-9]{0,2})$").IsMatch(s);
        }
    
        /// <summary>
        /// 校验危险字符
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool FilteStr(string inputStr)
        {
            bool r = true;
            if (inputStr.Contains(@"'")
                || inputStr.Contains(@",")
                || inputStr.Contains(@"\")
                || inputStr.Contains(@"/")
                || inputStr.Contains("\"")
                || inputStr.Contains(@"%")
                || inputStr.Contains(@"@")
                || inputStr.Contains(@"?")
                || inputStr.Contains(@".")
                || inputStr.Contains(@"&")

                 || inputStr.Contains(@"*")
                 || inputStr.Contains(@"$")
                 || inputStr.Contains(@"#")
                 || inputStr.Contains(@"!")
                 || inputStr.Contains(@"+")
                 || inputStr.Contains(@"-")
                 || inputStr.Contains(@"<")
                 || inputStr.Contains(@">")
                 || inputStr.Contains(@"<HTML>")
                 || inputStr.Contains(@"</HTML>")
            )
            {
                r = false;
            }
            return r;
        }    

        /// <summary>
        /// IP校验
        /// </summary>
        /// <param name="s">输入值</param>
        /// <returns>校验结果</returns>
        public static bool IsIP(string s)
        {
            return new Regex(@"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9]):\d{1,5}?$").IsMatch(s);
        }

        /// <summary>
        /// 英文字母校验
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsABC(string strIn)
        {
            return Regex.IsMatch(strIn, @"^[a-z]{4,12}$");
        }

        #endregion
    }
}
