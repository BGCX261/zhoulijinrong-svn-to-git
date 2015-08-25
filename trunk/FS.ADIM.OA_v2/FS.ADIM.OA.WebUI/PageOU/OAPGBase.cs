using System;
using System.Collections;
using System.Web.UI;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public class OAPGBase : Page
    {
        #region 变量
        /// <summary>
        /// 用户控件ID 为找上层的div的id
        /// </summary>
        protected string UCID
        {
            get
            {
                if (ViewState["UCID"] == null)
                {
                    if (Request.QueryString["UCID"] != null)
                    {
                        ViewState["UCID"] = Request.QueryString["UCID"].ToString();
                    }
                    else
                    {
                        ViewState["UCID"] = "";
                    }
                }
                return ViewState["UCID"] as string;
            }
        }
        //选中的ID
        protected string SelectedID
        {
            get
            {
                if (ViewState["SelectedID"] == null)
                {
                    ViewState["SelectedID"] = "";
                }
                return ViewState["SelectedID"] as string;
            }
            set
            {
                ViewState["SelectedID"] = value;
            }
        }
        //是否是第一次绑定
        protected Boolean IsFirstBind
        {
            get
            {
                if (ViewState["IsFirstBind"] == null)
                {

                    ViewState["IsFirstBind"] = false;

                }
                return (Boolean)ViewState["IsFirstBind"];
            }
            set
            {
                ViewState["IsFirstBind"] = value;
            }
        }
        //2个层的ID
        protected string divMainID = "MainDivID_";
        protected string divPopDivID = "PopDivID_";

        protected string style1 = "<font style='color:Red;'>";
        protected string style1_1 = "</font>";
        #endregion

        //获得script语句 给文本框的value赋值
        protected string GetJSscriptXMLValue(string cltID, string value)
        {
            value = SysString.JSFilter(value);
            string script = "";
            script += string.Format(@"if(parent.document.getElementById('{0}')==null)alert('找不到ID为\'{0}\'的控件');", cltID);
            script += "else\n";
            script += string.Format("parent.document.getElementById('{0}').value='{1}';", cltID, value.Replace("\r\n", ""));
            return script;
        }

        //获得script语句 给文本框的value赋值
        protected string GetJSscriptValue(string cltID, string value)
        {
            value = SysString.JSFilter(value);
            string script = "";
            script += string.Format(@"if(parent.document.getElementById('{0}')==null)alert('找不到ID为\'{0}\'的控件');", cltID);
            script += "else\n";
            script += string.Format("parent.document.getElementById('{0}').value += '{1}';", cltID, value);
            return script;
        }
        //给文本框的title赋值
        protected string GetJSscriptTitle(string cltID, string value)
        {
            value = SysString.JSFilter(value);
            string script = "";
            script += string.Format(@"if(parent.document.getElementById('{0}')==null)alert('找不到ID为\'{0}\'的控件');", cltID);
            script += "else\n";
            script += string.Format("parent.document.getElementById('{0}').title='{1}';", cltID, value);
            return script;
        }

        //从ArrList里组成分号分隔的字符串
        protected string GetStringText(ArrayList arrList)
        {
            string str = "";
            foreach (object o in arrList)
            {
                if (o.ToString() != string.Empty)
                    str += ";" + o.ToString();
            }
            if (str.Length > 0)
            {
                str = str.Substring(1);
            }
            return str;
        }

        //获得域帐号 如果没有域名则返回帐号
        protected string GetDomainID(string domain, string userid)
        {
            if (domain.Length > 0)
                return domain + @"\" + userid;
            else
                return userid;
        }

        protected bool IsChecked(string checkValue, string sID)
        {
            bool bFlag = false;
            if (string.IsNullOrEmpty(checkValue) == false && checkValue.Length > 0)
            {
                string[] sArrID = checkValue.Replace(",", ";").Split(';');
                for (int i = 0; i < sArrID.Length; i++)
                {
                    if (sArrID[i] == sID)
                    {
                        bFlag = true;
                    }
                }
            }
            return bFlag;
        }

        /// <summary>
        /// 获得url参数传递的值
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        protected string GetQueryString(string name)
        {
            if (Request.QueryString[name] != null)
            {
                return Server.UrlDecode(Request.QueryString[name].ToString());
            }
            else
            {
                return "";
            }
        }
        protected Boolean GetQueryBoolean(string name)
        {
            if (Request.QueryString[name] == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 暂时没用到
        protected void GetStringList(ArrayList arrList, string str, bool bAdd)
        {
            if (str == string.Empty)
            {
                return;
            }

            int index = -1;

            if (bAdd) //如果是添加
            {
                index = arrList.IndexOf(str);
                if (index == -1)
                {
                    arrList.Add(str);
                }
            }
            else//如果是取消
            {
                index = arrList.IndexOf(str);
                if (index > -1)
                {
                    arrList.RemoveAt(index);
                }
            }
        }
        #endregion
    }
}
