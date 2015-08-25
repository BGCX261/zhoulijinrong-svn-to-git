using System;
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using System.Data;
using FounderSoftware.Framework.UI.WebCtrls;
using System.Web.UI.WebControls;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.RePrint
{
    public partial class UC_RePrint : UCBase
    {
        private String PreviousPageUrl
        {
            get
            {
                return ViewState["PreviousPageUrl"] as string;
            }
            set
            {
                ViewState["PreviousPageUrl"] = value;
            }
        }
        /// <summary>
        /// 流程模版名称
        /// </summary>
        protected String ProcessTemplate
        {
            get
            {
                if (ViewState[ConstString.ViewState.TEMPLATE_NAME] == null)
                {
                    return String.Empty;
                }
                return Convert.ToString(ViewState[ConstString.ViewState.TEMPLATE_NAME]);
            }
            set
            {
                ViewState[ConstString.QueryString.TEMPLATE_NAME] = value;

            }
        }
        //private string TID
        //{
        //    get
        //    {
        //        if (ViewState["id"] == null)
        //        {
        //            if (Request.QueryString[ConstString.QueryString.REGISTER_ID] != null)
        //            {
        //                ViewState["id"] = Request.QueryString[ConstString.QueryString.REGISTER_ID].ToString();
        //            }
        //            else
        //            {
        //                ViewState["id"] = "";
        //            }
        //        }
        //        return ViewState["id"].ToString();
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ProcessTemplate = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];
                PreviousPageUrl = Request.UrlReferrer.ToString();
                if (String.IsNullOrEmpty(ProcessTemplate))
                {
                    ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "没有指定收文流程模版！", "Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle");
                    return;
                }

                //收文年份默认加载前后十年,并且默认选择当前年份
                int l_intYear = DateTime.Now.Year;
                for (int i = l_intYear - 10; i < l_intYear + 10; i++)
                {
                    ddlQueryRecYear.Items.Add(i.ToString());
                }

                this.ucCompanyQuery.UCNameControl = this.txtQueryRecUnit.ClientID;
            }
        }

        /// <summary>
        /// 查询按钮的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            B_ReceiveEdit l_BusReceiveEdit = null;

            if (!VerifyQueryField())
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "表单验证失败");
                return;
            }

            l_BusReceiveEdit = new B_ReceiveEdit();

            l_BusReceiveEdit.ProcessName = ProcessTemplate;

            //收文号-从
            l_BusReceiveEdit.ReceiveNoFrom = txtQueryDocNoFrom.Text.TrimEnd();

            //收文号-到
            l_BusReceiveEdit.ReceiveNoTo = txtQueryDocNoTo.Text.TrimEnd();

            //文件标题
            l_BusReceiveEdit.DocumentTitle = txtQueryDocTitle.Text.TrimEnd();

            //收文日期-从
            if (ValidateUtility.IsDateTime(txtQueryRecDateFrom.Text.TrimEnd()))
            {
                l_BusReceiveEdit.ReceiveDateFrom = txtQueryRecDateFrom.ValDate;
            }

            //收文日期-到
            if (ValidateUtility.IsDateTime(txtQueryRecDateTo.Text.TrimEnd()))
            {
                l_BusReceiveEdit.ReceiveDateTo = txtQueryRecDateTo.ValDate;
            }

            //来文单位
            l_BusReceiveEdit.ReceiveUnit = txtQueryRecUnit.Text.TrimEnd();

            //收文年份
            l_BusReceiveEdit.ReceiveYear = ddlQueryRecYear.Text.TrimEnd();

            //状态
            if (ddlQueryStatus.SelectedItem != null)
            {
                l_BusReceiveEdit.Status = ddlQueryStatus.SelectedItem.Text;
            }
            l_BusReceiveEdit.Start = this.gdvList.PageIndex * this.gdvList.PageSize;
            l_BusReceiveEdit.End = this.gdvList.PageIndex * this.gdvList.PageSize + this.gdvList.PageSize;
            l_BusReceiveEdit.Sort = null;

            this.gdvList.DataSource = l_BusReceiveEdit.QueryRegisterInfo(l_BusReceiveEdit);
            this.gdvList.RecordCount = l_BusReceiveEdit.RowCount;
            this.gdvList.DataBind();
        }

        /// <summary>
        /// 查询区块元素验证
        /// </summary>
        /// <returns></returns>
        private bool VerifyQueryField()
        {
            //收文日期-从
            if (!String.IsNullOrEmpty(txtQueryRecDateFrom.Text.TrimEnd()) && !ValidateUtility.IsDateTime(txtQueryRecDateFrom.Text.TrimEnd()))
            {
                return false;
            }

            //收文日期-到
            if (!String.IsNullOrEmpty(txtQueryRecDateTo.Text.TrimEnd()) && !ValidateUtility.IsDateTime(txtQueryRecDateTo.Text.TrimEnd()))
            {
                return false;
            }

            return true;
        }
        //private bool ChosseProTypeIsByIdProType()
        //{
        //    B_ReceiveEdit l_BusReceiveEdit = new B_ReceiveEdit();
        //    //if (this.TID != string.Empty)
        //    //{
        //    //    l_BusReceiveEdit.ID = int.Parse(this.TID);
        //    //}
        //    return l_BusReceiveEdit.ProcessName == this.ProcessTemplate;
        //}

        protected void gvRegisterList_ExteriorPaging(GridViewPageEventArgs e)
        {
            btnQuery_Click(null, null);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.Register.PG_Register&TemplateName=" + ProcessTemplate + "&ID=" + Request.QueryString[ConstString.QueryString.REGISTER_ID]);
        }
    }
}