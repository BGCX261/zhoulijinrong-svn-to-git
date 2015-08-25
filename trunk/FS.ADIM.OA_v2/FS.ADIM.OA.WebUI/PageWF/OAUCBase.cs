using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public class OAUCBase : System.Web.UI.UserControl
    {
        #region 弹出层的相关ID
        ///// <summary>
        ///// 最外层div ID
        ///// </summary>
        //protected string MainDivID
        //{
        //    get { return "MainDivID_" + this.ID; }

        //}
        /// <summary>
        /// 显示层div ID
        /// </summary>
        public string PopDivID
        {
            get { return "PopDivID_" + this.ID; }
        }

        /// <summary>
        /// iframe ID
        /// </summary>
        public string PopIframeID
        {
            get { return "PopIframeID_" + this.ID; }
        }

        /// <summary>
        /// div Title ID
        /// </summary>
        protected string DivTitleID
        {
            get { return "DivTitleID_" + this.ID; }
        }

        //width height
        private string _DivWidth = "";
        protected string DivWidth
        {
            get { return _DivWidth; }
            set { _DivWidth = value; }
        }
        private string _DivHeight = "";
        protected string DivHeight
        {
            get { return _DivHeight; }
            set { _DivHeight = value; }
        }
        private string _SHead = "";
        protected string SHead
        {
            get { return _SHead; }
            set { _SHead = value; }
        }
        #endregion

        /// <summary>
        /// 获得url参数传递的值
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        protected string GetQueryString(string name)
        {
            if (Request.QueryString[name] != null)
            {
                return Request.QueryString[name].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
