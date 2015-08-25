using System;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL;
using System.Data;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.WorkFlow.Circulate
{
    public partial class UC_GoOnCirculate : System.Web.UI.UserControl
    {
        /// <summary>
        /// 是否显示意见
        /// </summary>
        public Boolean UCShowComment
        {
            get
            {
                if (ViewState["UCShowComment"] == null)
                    return false;
                return (Boolean)ViewState["UCShowComment"];
            }
            set
            {
                ViewState["UCShowComment"] = value;
            }
        }

        /// <summary>
        /// 意见
        /// </summary>
        public string UCComment
        {
            get
            {
                return FormsMethod.GetPrompt(this.txtCommentView.Text, this.txtCommentEdit.Text);
            }
        }

        /// <summary>
        /// 是否有继续传阅的权限
        /// </summary>
        public Boolean UCIsGoOnCirculate
        {
            get
            {
                if (ViewState["UCIsGoOnCirculate"] == null)
                    return false;
                return (Boolean)ViewState["UCIsGoOnCirculate"];
            }
            set
            {
                ViewState["UCIsGoOnCirculate"] = value;
            }
        }

        //任金权20091104 添加
        /// <summary>
        /// 意见框是否显示
        /// </summary>
        public Boolean IsVisible
        {
            get
            {
                if (ViewState["IsVisible"] == null)
                {
                    return true;
                }
                return (Boolean)ViewState["IsVisible"];
            }
            set
            {
                ViewState["IsVisible"] = value;
            }
        }


        /// <summary>
        /// 流程实例号
        /// </summary>
        public String UCProcessID
        {
            get
            {
                if (ViewState["UCProcessID"] == null)
                    return String.Empty;
                return ViewState["UCProcessID"] as String;
            }
            set
            {
                ViewState["UCProcessID"] = value;
            }
        }
        /// <summary>
        /// 流程类型
        /// </summary>
        public String UCProcessType
        {
            get
            {
                if (ViewState["UCProcessType"] == null)
                    return String.Empty;
                return ViewState["UCProcessType"] as String;
            }
            set
            {
                ViewState["UCProcessType"] = value;
            }
        }
        /// <summary>
        /// 步骤节点ID
        /// </summary>
        public String UCWorkItemID
        {
            get
            {
                if (ViewState["UCWorkItemID"] == null)
                    return String.Empty;
                return ViewState["UCWorkItemID"] as String;
            }
            set
            {
                ViewState["UCWorkItemID"] = value;
            }
        }

        //private bool isread = false;
        ///// <summary>
        ///// 是否是已阅
        ///// </summary>
        //public bool UCIsRead
        //{
        //    get
        //    {
        //        return isread;
        //    }
        //    set
        //    {
        //        isread = value;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UCDeptMember1.UCDeptIDControl = this.txtCirculatesDeptIDs.ClientID;
                UCDeptMember1.UCDeptUserIDControl = txtCirculatesIDs.ClientID;
                UCDeptMember1.UCDeptAndUserControl = this.txtCirculateNames.ClientID;
                UCDeptMember1.UCSelectType = "1";
                this.UCDeptMember1.UCTemplateName = this.UCProcessType.Replace("新版","");
                this.UCDeptMember1.UCFormName = "继续传阅";

                txtCirculateNames.Attributes.Add("readonly", "readonly");
                txtCommentView.Attributes.Add("readonly", "readonly");

                if (UCShowComment)
                {
                    lblComment.Visible = true;
                    txtCommentView.Visible = true;
                }
                InitLoad();
            }
        }

        #region 外部调用分发
        /// <summary>
        /// 分发 对与已经分发过的则不再分发
        /// </summary>
        /// <param name="yiFenFaUser">已分发人员</param>
        /// <returns></returns>
        public void DoCirculate(ref string sSuccesName, ref string sFailedName)
        {
            if (!string.IsNullOrEmpty(this.txtCirculatesIDs.Value) || !string.IsNullOrEmpty(this.txtCirculatesDeptIDs.Value))
            {
                string id = Request["CirculateID"] != null ? Request["CirculateID"].ToString() : "-1";
                string sql = string.Format(@"SELECT * FROM {0} WHERE ID={1}", TableName.GetCirculateTableName(UCProcessType), id);
                DataTable dt = Entity.RunQuery(sql);
                B_ToCirculate tocir = new B_ToCirculate();
                tocir.ToProcessID = this.UCProcessID;
                tocir.ToWorkItemID = this.UCWorkItemID;
                tocir.YiJian = FormsMethod.GetPrompt(this.txtCommentView.Text, this.txtCommentEdit.Text);
                tocir.IsAgain = false;
                tocir.ToProcessType = this.UCProcessType;
                tocir.ToUserIDS = this.txtCirculatesIDs.Value;
                tocir.ToiLevelCode = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["ID"].ToString() : "0";
                tocir.ToiLastLevel = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["LevelCode"].ToString() : "";
                if (!string.IsNullOrEmpty(this.txtCirculatesDeptIDs.Value))
                {
                    string strUserids = OAUser.GetUserByDeptPostArray(this.txtCirculatesDeptIDs.Value, OUConstString.PostName.FUKEZHANG, true, true)[0];
                    if (string.IsNullOrEmpty(strUserids) == false && strUserids.Length > 0)
                    {
                        strUserids = strUserids.Replace(";", ",");
                    }
                    if (!string.IsNullOrEmpty(this.txtCirculatesIDs.Value))
                    {
                        tocir.ToUserIDS += "," + strUserids;
                    }
                    else
                    {
                        tocir.ToUserIDS += strUserids;
                    }
                }
                string info = tocir.ChuanYueToDB();
                sSuccesName = info;
            }
        }
        #endregion

        public void LoadComment()
        {
            if (Request.QueryString["ID"] != null)
            {
                B_Circulate l_objCirculate = new B_Circulate(TableName.GetCirculateTableName(UCProcessType));
                l_objCirculate.ID = SysConvert.ToInt32(Request.QueryString["ID"].ToString());
                txtCommentView.Text = l_objCirculate.Comment;
            }
        }

        public void InitLoad()
        {
            UCIsGoOnCirculate = true;

            string deptID = OADept.GetDeptIDByUser(CurrentUserInfo.LoginName);

            if (deptID == "-1")
            {
                tr1.Visible = false;
                UCDeptMember1.Visible = false;
                txtCommentEdit.Visible = false;
                UCIsGoOnCirculate = false;
            }
            else if (deptID == "")
            {
                //公司领导不做限制
                UCDeptMember1.UCShowDeptID = "";
                UCIsGoOnCirculate = true;
            }
            else
            {
                UCDeptMember1.UCShowDeptID = deptID;
                UCIsGoOnCirculate = true;
            }
        }

        public Boolean CheckBeforeCirculate(ref String p_strMessage)
        {
            if (txtCirculateNames.Text.TrimEnd() == String.Empty)
            {
                p_strMessage = "请选择传阅人员，否则无法继续！";
                return false;
            }
            if (txtCommentEdit.Text.TrimEnd().Length > 500)
            {
                p_strMessage = "传阅意见不能大于500字符";
                return false;
            }
            return true;
        }
    }
}