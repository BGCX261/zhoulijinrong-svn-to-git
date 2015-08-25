using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.WebUI.PageOU;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class UC_FileControl : OAUCBase
    {
        #region 变量定义
        /// <summary>
        /// 在线编辑附件窗体iframe ID
        /// </summary>
        protected String OLIframeID
        {
            get { return "OLIframeID_" + this.ID; }
        }

        /// <summary>
        /// Add By Ivan-Yao
        /// </summary>
        public String HiddenClientID
        {
            get
            {
                return this.txtUCXML.ClientID;
            }
        }

        /// <summary>
        /// 流程类型
        /// </summary>
        public String UCTemplateName
        {
            get
            {
                if (ViewState[ConstString.ViewState.TEMPLATE_NAME] == null)
                {
                    return String.Empty;
                }
                return ViewState[ConstString.ViewState.TEMPLATE_NAME] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.TEMPLATE_NAME] = value;
            }
        }
        /// <summary>
        /// 流程实例号
        /// </summary>
        public String UCProcessID
        {
            get
            {
                if (ViewState[ConstString.ViewState.PROCESS_ID] == null)
                {
                    return String.Empty;
                }
                return ViewState[ConstString.ViewState.PROCESS_ID] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.PROCESS_ID] = value;
            }
        }
        public String UCWorkItemID
        {
            get
            {
                if (ViewState[ConstString.ViewState.WORKITEM_ID] == null)
                {
                    return String.Empty;
                }
                return ViewState[ConstString.ViewState.WORKITEM_ID] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.WORKITEM_ID] = value;
            }
        }
        /// <summary>
        /// 附件列表信息
        /// </summary>
        public List<CFuJian> UCDataList
        {
            get
            {
                return MossObject.Xml2FuJianList(txtUCXML.Value);
            }
            set
            {
                txtUCXML.Value = MossObject.FuJianList2Xml(value as List<CFuJian>);
            }
        }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public Boolean UCIsEditable
        {
            get
            {
                if (ViewState["UCIsEditable"] == null)
                    return true;
                return Convert.ToBoolean(ViewState["UCIsEditable"]);
            }
            set
            {
                ViewState["UCIsEditable"] = value;
            }
        }
        //修改附件
        public String UCIsAgain
        {
            get
            {
                if (ViewState["UCIsAgain"] == null)
                    return String.Empty;
                return ViewState["UCIsAgain"] as String;
            }
            set
            {
                ViewState["UCIsAgain"] = value;
            }
        }
        public String UCTBID
        {
            get
            {
                if (ViewState["UCTBID"] == null)
                    return String.Empty;
                return ViewState["UCTBID"] as String;
            }
            set
            {
                ViewState["UCTBID"] = value;
            }
        }
        #endregion

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    Session["附件ListTemp"] = txtUCXML.Value;

            //    if (!String.IsNullOrEmpty(base.GetQueryString("IsHistory")))
            //    {
            //        UCIsEditable = false;
            //    }

            //    UCWorkItemID = base.GetQueryString("WID");

            //    ClientScriptM.ResponseScript(Page, String.Format("OpenAttachment('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", txtUCXML.ClientID, PopIframeID, UCTemplateName, UCProcessID, UCIsEditable, UCWorkItemID, UCTBID, UCIsAgain));
            //}
            
            Session["附件ListTemp"] = txtUCXML.Value;

            if (!String.IsNullOrEmpty(base.GetQueryString("IsHistory")))
            {
                UCIsEditable = false;//true;// false;  //yangzj 20110630
            }

            UCWorkItemID = base.GetQueryString("WID");

            ClientScriptM.ResponseScript(Page, String.Format("OpenAttachment('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", txtUCXML.ClientID, PopIframeID, UCTemplateName, UCProcessID, UCIsEditable, UCWorkItemID, UCTBID, UCIsAgain));
        }

        /// <summary>
        /// 草稿保存时记住附件ID
        /// </summary>
        public void SetSession()
        {
            Session["UCTBID"] = UCTBID;
        }
        #endregion
    }
}