using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class UC_Role : OAUCBase
    {
        #region 变量定义

        /// <summary>
        /// 角色名(需要赋值)
        /// </summary>
        public String UCRoleName
        {
            get
            {
                if (ViewState["UCRoleName"] == null)
                {
                    ViewState["UCRoleName"] = string.Empty;
                }
                return ViewState["UCRoleName"] as String;
            }
            set
            {
                ViewState["UCRoleName"] = value;
            }
        }

        /// <summary>
        /// 需要得到用户帐号的控件名,多个分号分隔(需要赋值)
        /// </summary>
        public String UCUserIDControl
        {
            get
            {
                if (ViewState["UCUserIDControl"] == null)
                {
                    ViewState["UCUserIDControl"] = string.Empty;
                }
                return ViewState["UCUserIDControl"] as String;
            }
            set
            {
                ViewState["UCUserIDControl"] = value;
            }
        }

        /// <summary>
        /// 需要得到用户姓名的控件名(需要赋值)
        /// </summary>
        public String UCUserNameControl
        {
            get
            {
                if (ViewState["UCUserNameControl"] == null)
                {
                    ViewState["UCUserNameControl"] = string.Empty;
                }
                return ViewState["UCUserNameControl"] as String;
            }
            set
            {
                ViewState["UCUserNameControl"] = value;
            }
        }

        /// <summary>
        /// 是否单选 true:单选 false:多选 (默认多选)
        /// </summary>
        public Boolean UCIsSingle
        {
            get
            {
                if (ViewState["UCIsSingle"] == null)
                {
                    ViewState["UCIsSingle"] = false;
                }
                return Convert.ToBoolean(ViewState["UCIsSingle"]);
            }
            set
            {
                ViewState["UCIsSingle"] = value;
            }
        }
       
        #endregion
    }
}