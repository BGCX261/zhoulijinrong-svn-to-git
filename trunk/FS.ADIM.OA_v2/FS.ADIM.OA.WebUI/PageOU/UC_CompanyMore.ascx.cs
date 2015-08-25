using System;


namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class UC_CompanyMore : OAUCBase
    {
        #region 变量

        /// <summary>
        /// IDS控件集合
        /// </summary>
        public string UCIDControl
        {
            get
            {
                if (ViewState["UCIDControl"] == null)
                {
                    ViewState["UCIDControl"] = string.Empty;
                }
                return ViewState["UCIDControl"] as string;
            }
            set
            {
                ViewState["UCIDControl"] = value;
            }
        }

        /// <summary>
        ///  NO控件集合
        /// </summary>
        public string UCNoControl
        {
            get
            {
                if (ViewState["UCNoControl"] == null)
                {
                    ViewState["UCNoControl"] = string.Empty;
                }
                return ViewState["UCNoControl"] as string;
            }
            set
            {
                ViewState["UCNoControl"] = value;
            }
        }

        /// <summary>
        /// Name控件集合
        /// </summary>
        public string UCNameControl
        {
            get
            {
                if (ViewState["UCNameControl"] == null)
                {
                    ViewState["UCNameControl"] = string.Empty;
                }
                return ViewState["UCNameControl"] as string;
            }
            set
            {
                ViewState["UCNameControl"] = value;
            }
        }

        /// <summary>
        /// 是否单选 true:单选 false:多选 (默认单选)
        /// </summary>
        public bool UCIsSingle
        {
            get
            {
                //if (ViewState["UCIsSingle"] == null)
                //{
                //    ViewState["UCIsSingle"] = true;
                //}
                //return Convert.ToBoolean(ViewState["UCIsSingle"]);
                return false;
            }
            //set
            //{
            //    ViewState["UCIsSingle"] = value;
            //}
        }
        #endregion
    }
}