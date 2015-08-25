using System; 
using System.Collections; 
using System.Text.RegularExpressions;

namespace FounderSoftware.ADIM.OU.BLL.Busi
{ 
    /// <summary> 
    /// RegularMatch 的摘要说明。 
    /// </summary>     
    internal class Validate 
    { 
        private static string _String; 
        private static bool _IsEntirety; 
  
        /// <summary> 
        /// 可以进行判断的类型 
        /// </summary> 
        public enum Operation 
        { 
            /// <summary>
            /// 
            /// </summary>
            Byte,
            /// <summary>
            /// 
            /// </summary>
            SByte,
            /// <summary>
            /// 
            /// </summary>
            Int16,
            /// <summary>
            /// 
            /// </summary>
            Int32,
            /// <summary>
            /// 
            /// </summary>
            Int64,
            /// <summary>
            /// 
            /// </summary>
            Single,
            /// <summary>
            /// 
            /// </summary>
            Double,
            /// <summary>
            /// 
            /// </summary>
            Boolean,
            /// <summary>
            /// 
            /// </summary>
            Char,
            /// <summary>
            /// 
            /// </summary>
            Decimal,
            /// <summary>
            /// 
            /// </summary>
            DateTime,
            /// <summary>
            /// 
            /// </summary>
            Date,
            /// <summary>
            /// 
            /// </summary>
            Time,
            /// <summary>
            /// 
            /// </summary>
            EMail,
            /// <summary>
            /// 
            /// </summary>
            URL,
            /// <summary>
            /// 
            /// </summary>
            ChinaPhone,
            /// <summary>
            /// 
            /// </summary>
            ChineseWord,
            /// <summary>
            /// 
            /// </summary>
            ChinesePostalCode,
            /// <summary>
            /// 
            /// </summary>
            Number,
            /// <summary>
            /// 
            /// </summary>
            StringModel_01,
            /// <summary>
            /// 
            /// </summary>
            StringModel_02,
            /// <summary>
            /// 
            /// </summary>
            English,
            /// <summary>
            /// 
            /// </summary>
            WideWord,
            /// <summary>
            /// 
            /// </summary>
            NarrowWord,
            /// <summary>
            /// 
            /// </summary>
            IPAddress,
            /// <summary>
            /// 
            /// </summary>
            ChineseMobile,
            /// <summary>
            /// 
            /// </summary>
            ChineseID 
        }; 
  
        //用于判断字符串//是否是对应类型（默认为包含匹配）#region 用于判断字符串//是否是对应类型（默认为包含匹配） 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strVerifyString"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsAccordType(string strVerifyString, Operation op) 
        {
            return IsAccordType(strVerifyString, op, false); 
        } 
    
        //用于判断字符串//是否是对应类型（或//是否包含对应类型的字符）#region 用于判断字符串//是否是对应类型（或//是否包含对应类型的字符） 
        /// <summary> 
        /// 用于判断字符串//是否是对应类型 
        /// </summary> 
        /// <param name="strVerifyString">String，需要判断的字符串</param> 
        /// <param name="op">Operation枚举，用于选择需要进行的操作</param> 
        /// <param name="IsEntirety">Boolean，判断是完全匹配还是包含匹配模式（仅适用于非类型判断时）</param> 
        /// <returns></returns> 
        public static  bool IsAccordType(string strVerifyString, Operation op, bool IsEntirety) 
        {
            _String = strVerifyString; 
            _IsEntirety = IsEntirety; 
  
            switch (op) 
            { 
                case Operation.Byte: 
                    { 
                        return IsByte(); 
                    } 
                case Operation.SByte: 
                    { 
                        return IsSByte(); 
                    } 
                case Operation.Int16: 
                    { 
                        return IsInt16(); 
                    } 
                case Operation.Int32: 
                    { 
                        return IsInt32(); 
                    } 
                case Operation.Int64: 
                    { 
                        return IsInt64(); 
                    } 
                case Operation.Single: 
                    { 
                        return IsSingle(); 
                    } 
                case Operation.Double: 
                    { 
                        return IsDouble(); 
                    } 
                case Operation.Boolean: 
                    { 
                        return IsBoolean(); 
                    } 
                case Operation.Char: 
                    { 
                        return IsChar(); 
                    } 
                case Operation.Decimal: 
                    { 
                        return IsDecimal(); 
                    } 
                case Operation.DateTime: 
                    { 
                        return IsDateTime(); 
                    } 
                case Operation.Date: 
                    { 
                        return IsDate(); 
                    } 
                case Operation.Time: 
                    { 
                        return IsTime(); 
                    } 
                case Operation.IPAddress: 
                    { 
                        return IsIPAddress(); 
                    } 
                case Operation.ChinaPhone: 
                    { 
                        return IsChinaPhone(); 
                    } 
                case Operation.ChinesePostalCode: 
                    { 
                        return IsChinesePostalCode(); 
                    } 
                case Operation.ChineseMobile: 
                    { 
                        return IsChineseMobile(); 
                    } 
                case Operation.EMail: 
                    { 
                        return IsEmail(); 
                    } 
                case Operation.URL: 
                    { 
                        return IsURL(); 
                    } 
                case Operation.ChineseWord: 
                    { 
                        return IsChineseWord(); 
                    } 
                case Operation.Number: 
                    { 
                        return IsNumber(); 
                    } 
                case Operation.StringModel_01: 
                    { 
                        return IsStringModel_01(); 
                    } 
                case Operation.StringModel_02: 
                    { 
                        return IsStringModel_02();    
                    }

                case Operation.English:
                    {
                        return IsEnglish();
                    }
                case Operation.WideWord: 
                    { 
                        return IsWideWord(); 
                    } 
                case Operation.NarrowWord: 
                    { 
                        return IsNarrowWord(); 
                    } 
                case Operation.ChineseID: 
                    { 
                        return IsChineseID(); 
                    } 
                default: 
                    { 
                        return false; 
                    } 
            } 
        }

        /// <summary>
        /// 功能:判断是否是英文
        /// 作者:王敏贤
        /// 日期:2009-11-18
        /// </summary>
        /// <param name="strEnName"></param>
        /// <returns></returns>
        public static bool IsEnglish(string strEnName)
        {
            return string.IsNullOrEmpty(strEnName) == false && Validate.IsAccordType(strEnName, Validate.Operation.English, true) == false;
        }

        /// <summary>
        /// 功能:判断Email是否正确
        /// 作者:王敏贤
        /// 日期:2009-11-19
        /// </summary>
        /// <param name="strEmail"></param>
        public static bool IsEmail(string strEmail)
        {
            return string.IsNullOrEmpty(strEmail) == false && Validate.IsAccordType(strEmail, Validate.Operation.EMail, true) == false;
        }

        /// <summary>
        /// 功能:判断电话号码是否正确
        /// 作者:王敏贤
        /// 日期:2009-11-19
        /// </summary>
        /// <param name="strTelephone"></param>
        public static bool IsTelephone(string strTelephone)
        {
            return string.IsNullOrEmpty(strTelephone) == false && SysUtility.IsTelephone(strTelephone) == false;
        }
     
  
        //具体验证方法#region 具体验证方法 
  
        //是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数#region //是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数 
        /// <summary> 
        /// 是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsByte() 
        { 
            try 
            { 
                Byte.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
    
        //是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数#region //是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数 
        /// <summary> 
        /// 是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsSByte() 
        { 
            try 
            { 
                SByte.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        }        
  
        //是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数#region //是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数 
        /// <summary> 
        /// 是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsInt16() 
        { 
            try 
            { 
                Int16.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数#region ////是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数 
        /// <summary> 
        /// 是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsInt32() 
        { 
            try 
            { 
                Int32.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数#region //是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数 
        /// <summary> 
        /// 是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsInt64() 
        { 
            try 
            { 
                Int64.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
         
        //是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字#region //是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字 
        /// <summary> 
        /// //是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsSingle() 
        { 
            try 
            { 
                Single.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字#region //是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字 
        /// <summary> 
        /// //是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsDouble() 
        { 
            try 
            { 
                Double.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否Boolean类型（布尔值）：true 或 false#region //是否Boolean类型（布尔值）：true 或 false 
        /// <summary> 
        /// 是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsBoolean() 
        { 
            try 
            { 
                Boolean.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF#region //是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF 
        /// <summary> 
        /// 是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsChar() 
        { 
            try 
            { 
                Char.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        }      
  
        //是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数#region //是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数 
        /// <summary> 
        /// 是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsDecimal() 
        { 
            try 
            { 
                Decimal.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间#region //是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间 
        /// <summary> 
        /// 是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsDateTime() 
        { 
            try 
            { 
                DateTime.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
    
        //是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期#region //是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期 
        /// <summary> 
        /// 是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsDate() 
        { 
            DateTime Value; 
            try 
            { 
                Value = DateTime.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            if (Value.Date.ToString() == _String) 
            { 
                return true; 
            } 
            else 
            { 
                return false; 
            } 
        } 
        
        //是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间#region //是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间 
        /// <summary> 
        /// 是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsTime() 
        { 
            DateTime Value; 
            try 
            { 
                Value = DateTime.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            if (Value.Year == 1 && Value.Month == 1 && Value.Day == 1) 
            { 
                return true; 
            } 
            else 
            { 
                return false; 
            } 
        }        
  
        //是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示）#region //是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示） 
        /// <summary> 
        /// 是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsIPAddress() 
        { 
            try 
            { 
                System.Net.IPAddress.Parse(_String); 
            } 
            catch 
            { 
                return false; 
            } 
            return true; 
        } 
        
        //是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断//是否是（区号：3或4位）-（电话号码：7或8位）#region //是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断//是否是（区号：3或4位）-（电话号码：7或8位） 
        /// <summary> 
        /// 是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断//是否是（区号：3或4位）-（电话号码：7或8位） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsChinaPhone() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"(\d{3,4})-?\d{7,8}", RegexOptions.None, ref aryResult, _IsEntirety); 
        }         
  
        //是否中国邮政编码（6位数字 \d{6}）#region //是否中国邮政编码（6位数字 \d{6}） 
        /// <summary> 
        /// 是否中国邮政编码（6位数字 \d{6}） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsChinesePostalCode() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"\d{6}", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
        
        //是否中国移动电话号码（13开头的总11位数字 13\d{9}）#region //是否中国移动电话号码（13开头的总11位数字 13\d{9}） 
        /// <summary> 
        /// 是否中国移动电话号码（13开头的总11位数字 13\d{9}） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsChineseMobile() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"13\d{9}", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
        
        //是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*）#region //是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*） 
        /// <summary> 
        /// 是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsEmail() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
        
        //是否Internet URL地址类型（http://）#region //是否Internet URL地址类型（http://） 
        /// <summary> 
        /// 是否Internet URL地址类型（http://） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsURL() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
        
        //是否中文字符（[\u4e00-\u9fa5]）#region //是否中文字符（[\u4e00-\u9fa5]） 
        /// <summary> 
        /// 是否中文字符（[\u4e00-\u9fa5]） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsChineseWord() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"[\u4e00-\u9fa5]", RegexOptions.None, ref aryResult, _IsEntirety); 
        }
 
        //是否是数字（0到9的数字[\d]+）：不包括符号"."和"-"#region //是否是数字（0到9的数字[\d]+）：不包括符号"."和"-" 
        /// <summary> 
        /// 是否是数字（0到9的数字[\d]+）：不包括符号"."和"-" 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsNumber() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"[\d]+", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
        
        //是否只包含数字，英文和下划线（[\w]+）#region //是否只包含数字，英文和下划线（[\w]+） 
        /// <summary> 
        /// 是否只包含数字，英文和下划线（[\w]+） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsStringModel_01() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"[\w]+", RegexOptions.None, ref aryResult, _IsEntirety); 
        }       
  
        //是否大写首字母的英文字母（[A-Z][a-z]+）#region //是否大写首字母的英文字母（[A-Z][a-z]+） 
        /// <summary> 
        /// 是否大写首字母的英文字母（[A-Z][a-z]+） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsStringModel_02() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"[A-Z][a-z]+", RegexOptions.None, ref aryResult, _IsEntirety); 
        }
              
        /// <summary> 
        ///是否英文字母和'（[A-Z][a-z|']+\s[A-Z][a-z|']+） 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static bool IsEnglish()
        {
            char[] cs = _String.ToCharArray();
            foreach (char c in cs)
            {
                if (!ENstr.ToString().Contains(c.ToString().ToUpper()))
                {
                    return false;
                }
            }
            return true;
        }
       
        //是否全角字符（[^\x00-\xff]）：包括汉字在内#region //是否全角字符（[^\x00-\xff]）：包括汉字在内 
        /// <summary> 
        /// 是否全角字符（[^\x00-\xff]）：包括汉字在内 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsWideWord() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"[^\x00-\xff]", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
  
        //是否半角字符（[\x00-\xff]）#region //是否半角字符（[\x00-\xff]） 
        /// <summary> 
        /// 是否半角字符（[^\x00-\xff]）：包括汉字在内 
        /// </summary> 
        /// <returns>Boolean</returns> 
        protected static  bool IsNarrowWord() 
        { 
            ArrayList aryResult = new ArrayList(); 
            return CommRegularMatch(_String, @"[\x00-\xff]", RegexOptions.None, ref aryResult, _IsEntirety); 
        } 
        
        //是否合法的中国//身份证号码#region //是否合法的中国//身份证号码 
        protected static  bool IsChineseID() 
        { 
            if (_String.Length == 15) 
            { 
                _String = CidUpdate(_String); 
            } 
            if (_String.Length == 18) 
            { 
                string strResult = CheckCidInfo(_String); 
                if (strResult == "非法地区" || strResult == "非法生日" || strResult == "非法证号") 
                { 
                    return false; 
                } 
                else 
                { 
                    return true; 
                } 
            } 
            else 
            { 
                return false; 
            } 
        } 
                
        //通用正则表达式判断函数#region //通用正则表达式判断函数 
        /// <summary> 
        /// 通用正则表达式判断函数 
        /// </summary> 
        /// <param name="strVerifyString">String，用于匹配的字符串</param> 
        /// <param name="strRegular">String，正则表达式</param> 
        /// <param name="regOption">RegexOptions，配置正则表达式的选项</param> 
        /// <param name="aryResult">ArrayList，分解的字符串内容</param> 
        /// <param name="IsEntirety">Boolean，//是否需要完全匹配</param> 
        /// <returns></returns> 
        public static  bool CommRegularMatch(string strVerifyString, string strRegular, System.Text.RegularExpressions.RegexOptions regOption, ref System.Collections.ArrayList aryResult, bool IsEntirety) 
        { 
            System.Text.RegularExpressions.Regex r; 
            System.Text.RegularExpressions.Match m; 
  
            //如果需要完全匹配的处理#region //如果需要完全匹配的处理 
            if (IsEntirety) 
            { 
                strRegular = strRegular.Insert(0, @"\A"); 
                strRegular = strRegular.Insert(strRegular.Length, @"\z"); 
            }          
            try 
            { 
                r = new System.Text.RegularExpressions.Regex(strRegular, regOption); 
            } 
            catch (System.Exception e) 
            { 
                throw (e); 
            } 
  
            for (m = r.Match(strVerifyString); m.Success; m = m.NextMatch()) 
            { 
                aryResult.Add(m); 
            } 
  
            if (aryResult.Count == 0) 
            { 
                return false; 
            } 
            else 
            { 
                return true; 
            } 
        }         
  
        //中国//身份证号码验证#region 中国//身份证号码验证 
        private static string CheckCidInfo(string cid) 
        { 
            string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" }; 
            double iSum = 0; 
            string info = string.Empty; 
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$"); 
            System.Text.RegularExpressions.Match mc = rg.Match(cid); 
            if (!mc.Success) 
            { 
                return string.Empty; 
            } 
            cid = cid.ToLower(); 
            cid = cid.Replace("x", "a"); 
            if (aCity[int.Parse(cid.Substring(0, 2))] == null) 
            { 
                return "非法地区"; 
            } 
            try 
            { 
                DateTime.Parse(cid.Substring(6, 4) + " - " + cid.Substring(10, 2) + " - " + cid.Substring(12, 2)); 
            } 
            catch 
            { 
                return "非法生日"; 
            } 
            for (int i = 17; i >= 0; i--) 
            { 
                iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber); 
            } 
            if (iSum % 11 != 1) 
            { 
                return ("非法证号"); 
            } 
            else 
            { 
                return (aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女")); 
            } 
        }       
  
        //身份证号码15升级为18位#region //身份证号码15升级为18位 
        private static string CidUpdate(string ShortCid) 
        { 
            char[] strJiaoYan = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' }; 
            int[] intQuan = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 }; 
            string strTemp; 
            int intTemp = 0; 
  
            strTemp = ShortCid.Substring(0, 6) + "19" + ShortCid.Substring(6); 
            for (int i = 0; i <= strTemp.Length - 1; i++) 
            { 
                intTemp += int.Parse(strTemp.Substring(i, 1)) * intQuan[i]; 
            } 
            intTemp = intTemp % 11; 
            return strTemp + strJiaoYan[intTemp]; 
        }

        /// <summary>
        /// 英文字符and "."
        /// </summary>
        private static  string ENstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ. ";                     
    } 
} 