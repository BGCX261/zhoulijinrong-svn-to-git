using System;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.WebUI.UIBase;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Common;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.Entity.Menu;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Process
{
    public partial class PG_ProcessStep : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void Bind()
        {
            //当前登录用户账号
            string strUserName = CurrentUserInfo.UserName;
            //得到检索条件           
            M_ProcessSearch mSearchCond = GetSearchCondition();

            B_ProcessInstance bCompleteFile = new B_ProcessInstance();
            //得到已办文件任务列表数据总数

            //外部分页
            //int iStart = 0;
            //int iEnd = 0;
            //int iCount = bCompleteFile.GetTableCount(mSearchCond, gvProcessStep.PageIndex + 1, gvProcessStep.PageSize, ref iStart, ref iEnd,false);
            ////得到已办文件任务列表数据
            //DataTable dtList = bCompleteFile.GetTableForOneProcess(mSearchCond, iStart, iEnd,false);

            DataTable dtList = bCompleteFile.GetTableForOneProcess(mSearchCond);

            this.gvProcessStep.PageType = PageType.NotPage;
            this.gvProcessStep.RecordCount = dtList.Rows.Count;
            gvProcessStep.ShowPagerRow = true;
            //绑定数据
            this.gvProcessStep.DataSource = dtList;

            this.gvProcessStep.DataBind();
        }

        /// <summary>
        /// 得到检索条件数据实体
        /// </summary>
        /// <returns>检索条件数据实体</returns>
        private M_ProcessSearch GetSearchCondition()
        {
            M_ProcessSearch mSearchCond = new M_ProcessSearch();
            if (Request.QueryString["PID"] != null)
            {
                mSearchCond.ProcessID = Request.QueryString["PID"].ToString();
            }
            if (Request.QueryString["TID"] != null)
            {
                mSearchCond.TemplateName = Request.QueryString["TID"].ToString();
            }
            mSearchCond.IsHistorySearch = Request.QueryString["IsHistory"] != null ? bool.Parse(Request.QueryString["IsHistory"].ToString()) : false;
            return mSearchCond;
        }


        /// <summary>
        /// 看是否表单有数据
        /// </summary>
        /// <param name="tableid"></param>
        /// <returns></returns>
        protected string CheckFormData(string tableid)
        {
            if (tableid == null || SysConvert.ToInt32(tableid) <= 0)
            {
                return "表单没有数据！";
            }
            else
            {
                return "";
            }
        }

        protected string GetUserSatus(string userID, string status)
        {
            if (userID.Contains(@"\"))
            {
                userID = userID.Substring(userID.IndexOf(@"\") + 1, userID.Length - userID.IndexOf(@"\") - 1);
                if (userID.LastIndexOf(":") > -1)
                {
                    userID = userID.Substring(0, userID.Length - 1);
                }
            }
            string userName = "";

            userName = OAUser.GetUserName(userID);

            return userName + " " + GetCNStatus(status);
        }

        protected string GetStepAndAction(string stepname, string action)
        {
            string strsa = stepname;
            if (action != string.Empty)
            {
                strsa += "（" + action + "）";
            }
            return strsa;
        }

        private string GetCNStatus(string name)
        {
            string status = "";
            switch (name)
            {
                case "New": status = "待获取"; break;
                case "Assigned": status = "待处理"; break;
                case "Completed": status = "已完成"; break;
                case "Overdue": status = "已过期"; break;
                case "Cancelled": status = "已取消"; break;
                case "Removed": status = "已移除"; break;
                case "uspend": status = "已暂停"; break;
                default: status = name; break;
            }
            return status;
        }

        protected void gvProcessStep_ExteriorPaging(GridViewPageEventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// 将VB中的人员ID组合为字符串
        /// </summary>
        /// <param name="vb"></param>
        /// <returns></returns>
        private string GetUserIDByViewBase(ViewBase vb)
        {
            string l_strLeaders = string.Empty;
            if (vb != null && vb.Count > 0)
            {
                foreach (FounderSoftware.ADIM.OU.BLL.Busi.User p_user in vb.Ens)
                {
                    l_strLeaders += p_user.DomainUserID + ";";
                }
                l_strLeaders = l_strLeaders.Substring(0, l_strLeaders.Length - 1);
            }
            return l_strLeaders;
        }

        private string GetCanLookPeople
        {
            get
            {
                if (ViewState["CanLookPeople"] == null)
                {
                    string struserList = CurrentUserInfo.UserName;
                    if (SysString.GetStringFormatForList(CurrentUserInfo.RoleName, ";").Contains("管理员") || CurrentUserInfo.RoleName.Contains(ConstString.RoleName.COMPANY_LEADER))
                    {
                        struserList = "";
                    }
                    else
                    {
                        foreach (DataRow dr in CurrentUserInfo.DeptPost.Rows)
                        {
                            struserList += dr["PostName"].ToString() == OUConstString.PostName.CHUZHANG || dr["PostName"].ToString() == OUConstString.PostName.FUCHUZHANG || dr["PostName"].ToString() == OUConstString.PostName.FUKEZHANG || dr["PostName"].ToString() == OUConstString.PostName.KEZHANG ? GetUserIDByViewBase(OAUser.GetUserByDeptID(dr["FK_DeptID"].ToString(), -1)) : "";

                        }
                    }
                    ViewState["CanLookPeople"] = struserList;
                }
                return ViewState["CanLookPeople"].ToString();
            }
        }

        protected void gvProcessStep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                Label lbllook = new Label();
                lbllook.Text = "查看";
                //if (GetCanLookPeople==""||GetCanLookPeople.Contains(dr["User_ID"].ToString()))
                //{
                lbllook.Attributes.Add("onclick", string.Format(@"javascript: window.open('../../Container.aspx?ProcessID={0}&WorkItemID={1}&TemplateName={2}&StepName={3}&TBID={4}&IsHistory=1')", dr["Proc_Inst_ID"], dr["Work_Item_ID"], dr["PDEF_NAME"], dr["StepName"], dr["TBID"]));
                lbllook.Style.Add("text-decoration", "underline");
                lbllook.ForeColor = System.Drawing.Color.Blue;
                //}
                e.Row.Cells[0].Controls.Add(lbllook);
                ListUIBase lu = new ListUIBase();
                lu.IndicateNoData(dr["MARK"], e.Row);
            }
        }
    }
}
