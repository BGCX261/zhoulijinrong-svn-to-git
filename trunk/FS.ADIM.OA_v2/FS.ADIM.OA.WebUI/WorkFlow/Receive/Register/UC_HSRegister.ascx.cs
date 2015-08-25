//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：收文登记表单
//
// 
// 创建标识：任金权 2009-12-29
//
// 修改标识：任金权 2010-5-11
// 修改描述：修改LoadRegisterList函数，过滤危险字符。如"'"
//
// 修改标识：任金权 2010-5-12
// 修改描述：修改btnSecFF_Click，修改了提示信息。
//----------------------------------------------------------------
using System;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebCtrls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using Newtonsoft.Json;
using FS.ADIM.OA.BLL.Entity;
using System.Collections.Generic;
using FS.ADIM.OA.BLL;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.WebUI.UIBase;
using System.Data;
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.Register
{
    public partial class UC_HSRegister : ListUIBase, ICallbackEventHandler
    {
        public string m_ID
        {
            get
            {
                if (Request["ID"] != null && !string.IsNullOrEmpty(Request["ID"].ToString()))
                {
                    return Request["ID"].ToString();
                }
                return null;
            }
        }

        public string TemplateName
        {
            get
            {
                if (ViewState[ConstString.ViewState.TEMPLATE_NAME] == null)
                {
                    ViewState[ConstString.ViewState.TEMPLATE_NAME] = ProcessConstString.TemplateName.LETTER_RECEIVE.Replace("新版", "");
                }
                return Convert.ToString(ViewState[ConstString.ViewState.TEMPLATE_NAME]);
            }
            set
            {
                ViewState[ConstString.QueryString.TEMPLATE_NAME] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet l_dstDataSet = null;
            if (!Page.IsPostBack)
            {
                this.TemplateName = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];

                if (String.IsNullOrEmpty(this.TemplateName))
                {
                    JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有指定收文流程模版！", "Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle");
                    return;
                }

                this.ucCompany.UCNameControl = this.txtReceiveUnit.ClientID;
                this.ucQueryCompany.UCNameControl = this.txtQueryRecUnit.ClientID;
                OAList.BindHJLX2(ddlDocumentType, false);//任金权修改
                ddlDocumentType.Items.Insert(0, new ListItem());

                this.ucAttachment.UCTemplateName = TemplateName;

                l_dstDataSet = OAConfig.GetRankConfig();

                DataTable l_dtbDataTable = l_dstDataSet.Tables[TemplateName];

                String[] l_strAryRoleName = l_dtbDataTable.Rows[0]["角色"].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (l_strAryRoleName.Length == 0)
                {
                    JScript.RedirectPage(this.Page, "未配置收文登记权限", TemplateName, "1");
                    return;
                }

                String[] l_strAryUserRoleNames = CurrentUserInfo.RoleName.ToArray();

                IEnumerable<String> l_enumRole = l_strAryUserRoleNames.Intersect(l_strAryRoleName);

                if (l_enumRole.Count() == 0)
                {
                    JScript.RedirectPage(this.Page, string.Format("只有[{0}] {1} 可以访问该表单！", l_strAryRoleName[0], OAUser.GetUserByRole(l_strAryRoleName[0]).GetFieldVals("Name", ",")), TemplateName, "1");
                    return;
                }

                //初始化设置控件的验证功能
                txtReceiveUnit.RequiredType = RequiredType.NotNull;
                txtReceiveDate.RequiredType = RequiredType.NotNull;
                txtDocumentTitle.RequiredType = RequiredType.NotNull;
                txtPageCount.RequiredType = RequiredType.PositiveInteger;

                txtReceiveUnit.BackColor = System.Drawing.Color.Empty;
                txtReceiveDate.BackColor = System.Drawing.Color.Empty;
                txtXingWenDate.BackColor = System.Drawing.Color.Empty;
                txtDocumentTitle.BackColor = System.Drawing.Color.Empty;
                
                //设置客户端只读
                txtReceiveUnit.Attributes.Add("readOnly", "true");
                txtQueryRecUnit.Attributes.Add("readOnly", "true");
                txtDocumentNo.Attributes.Add("readonly", "true");

                //收文年份默认加载前后十年,并且默认选择当前年份
                txtReceiveDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtXingWenDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFormationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ddlUrgentDegree.SelectedIndex = 1;
                txtPageCount.Text = "1";
                ddlKeepTime.SelectedIndex = 3;
                LoadRegisterList();
                if (m_ID != null)
                {
                    SetFormByID(m_ID);
                }
            }
        }

        /// <summary>
        /// 新增按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            B_HSEdit l_BusReceiveEdit = null;

            //表单的合法性验证
            if (!VerifyEditField())
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "表单验证失败");
                return;
            }

            l_BusReceiveEdit = new B_HSEdit();
            PopulateEntity(l_BusReceiveEdit);
            l_BusReceiveEdit.DocumentNo = B_ReceiveID.GenerateReceiveNo(DateTime.Now.Year.ToString(), TemplateName);

            if (l_BusReceiveEdit.Save())
            {
                JScript.ShowMsgBox(Page, "新增成功", false);
                PopulateEditField(l_BusReceiveEdit);
                LoadRegisterList();
            }
        }

        /// <summary>
        /// 修改按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnModify_Click(object sender, EventArgs e)
        {
            B_HSEdit l_BusReceiveEdit = null;

            if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbExclamation, "请先选择一条收文记录！");
                return;
            }

            if (!VerifyEditField())
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "表单验证失败");
                return;
            }

            l_BusReceiveEdit = new B_HSEdit();
            l_BusReceiveEdit.ID = Convert.ToInt32(txtRegisterID.Text);

            if (l_BusReceiveEdit.CreateDate == DateTime.MinValue)
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "当前收文登记信息已经被删除，不能做修改动作！");
                return;
            }

            PopulateEntity(l_BusReceiveEdit);

            if (l_BusReceiveEdit.Save())
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbInformation, "保存成功");
                PopulateEditField(l_BusReceiveEdit);
                LoadRegisterList();
            }
        }

        /// <summary>
        /// 启动流程按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLaunch_Click(object sender, EventArgs e)
        {
            B_HSEdit l_BusReceiveEdit = null;

            if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbExclamation, "请先选择一条收文记录！");
                return;
            }

            l_BusReceiveEdit = new B_HSEdit();

            l_BusReceiveEdit.ID = Convert.ToInt32(txtRegisterID.Text);

            if (!String.IsNullOrEmpty(l_BusReceiveEdit.ProcessID))
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbExclamation, "当前选择的收文登记记录已经启动！");
                return;
            }

            String l_strSuffix = ConstString.QueryString.REGISTER_ID + "=" + Convert.ToString(txtRegisterID.Text);

            Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.PG_LetterReceived&TemplateName=" + Server.UrlEncode(ProcessConstString.TemplateName.LETTER_RECEIVE) + "&" + l_strSuffix, true);

        }

        /// <summary>
        /// 详细信息按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbExclamation, "请先选择一条收文记录！");
                return;
            }
            Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail.PG_HSReDetail&" + ConstString.QueryString.REGISTER_ID + "=" + Convert.ToString(txtRegisterID.Text), true);
        }

        /// <summary>
        /// 编辑区域元素验证
        /// </summary>
        /// <returns></returns>
        private bool VerifyEditField()
        {
            //收文日期
            if (txtReceiveDate.Text.TrimEnd() != String.Empty && !ValidateUtility.IsDateTime(txtReceiveDate.Text.TrimEnd()))
            {
                txtReceiveDate.Focus();
                return false;
            }

            //行文日期
            if (txtXingWenDate.Text.TrimEnd() != String.Empty && !ValidateUtility.IsDateTime(txtXingWenDate.Text.TrimEnd()))
            {
                txtXingWenDate.Focus();
                return false;
            }

            //形成日期
            if (txtFormationDate.Text.TrimEnd() != String.Empty && !ValidateUtility.IsDateTime(txtFormationDate.Text.TrimEnd()))
            {
                txtFormationDate.Focus();
                return false;
            }

            //页数
            if (txtPageCount.Text.TrimEnd() != String.Empty && !ValidateUtility.IsInt(txtPageCount.Text.TrimEnd()))
            {
                txtPageCount.Focus();
                return false;
            }

            return true;
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

        /// <summary>
        /// 清楚表单编辑区域内容，恢复至加载时状态
        /// </summary>
        private void ClearEditField()
        {
            //单位
            txtReceiveUnit.Text = String.Empty;
            //收文号
            txtDocumentNo.Text = String.Empty;
            //文件编码
            txtDocumentEncoding.Text = String.Empty;
            //收文日期
            txtReceiveDate.Text = String.Empty;
            //答复文号
            txtReplyDocumentNo.Text = String.Empty;
            //其他编码
            txtOtherEncoding.Text = String.Empty;
            //行文日期
            txtXingWenDate.Text = String.Empty;
            //文件标题
            txtDocumentTitle.Text = String.Empty;
            //函件类型
            ddlDocumentType.Text = String.Empty;
            //形成日期
            txtFormationDate.Text = String.Empty;
            //紧急程度
            ddlUrgentDegree.Text = String.Empty;
            //页数
            txtPageCount.Text = String.Empty;
            //保管期限
            ddlKeepTime.Text = String.Empty;
            //对应合同号
            txtContractNumber.Text = String.Empty;
            //设备代码
            txtEquipmentCode.Text = String.Empty;
            //HN编码
            txtHNCode.Text = String.Empty;
            ////附件信息
            this.ucAttachment.UCDataList = new List<CFuJian>();
            //
            txtRegisterID.Text = String.Empty;
        }

        /// <summary>
        /// 用表单元素内容填充实体内容
        /// </summary>
        /// <param name="l_BusReceiveEdit"></param>
        private void PopulateEntity(B_HSEdit l_BusReceiveEdit)
        {
            //单位
            l_BusReceiveEdit.CommunicationUnit = txtReceiveUnit.Text.TrimEnd();

            //文件编码
            l_BusReceiveEdit.FileEncoding = txtDocumentEncoding.Text.TrimEnd();

            //收文日期
            if (ValidateUtility.IsDateTime(txtReceiveDate.Text.TrimEnd()))
            {
                l_BusReceiveEdit.ReceiptDate = Convert.ToDateTime(txtReceiveDate.Text.TrimEnd());
            }

            //答复文号
            l_BusReceiveEdit.ReplyDocumentNo = txtReplyDocumentNo.Text.TrimEnd();

            //其他编码
            l_BusReceiveEdit.OtherEncoding = txtOtherEncoding.Text.TrimEnd();

            //行文日期
            if (txtXingWenDate.Text == "" || !ValidateUtility.IsDateTime(txtXingWenDate.Text.TrimEnd()))
            {
                l_BusReceiveEdit.XingWenDate = DateTime.MinValue;
            }
            else
            {
                l_BusReceiveEdit.XingWenDate = Convert.ToDateTime(txtXingWenDate.Text.TrimEnd());
            }

            //文件标题
            l_BusReceiveEdit.DocumentTitle = txtDocumentTitle.Text.TrimEnd();

            //函件类型
            l_BusReceiveEdit.LetterType = ddlDocumentType.Text;

            //形成日期
            if (ValidateUtility.IsDateTime(txtFormationDate.Text.TrimEnd()))
            {
                l_BusReceiveEdit.FormationDate = Convert.ToDateTime(txtFormationDate.Text.TrimEnd());
            }

            //紧急程度
            l_BusReceiveEdit.UrgentDegree = ddlUrgentDegree.Text.TrimEnd();

            //页数
            if (ValidateUtility.IsInt(txtPageCount.Text.TrimEnd()))
            {
                l_BusReceiveEdit.Pages = Convert.ToInt32(txtPageCount.Text.TrimEnd());
            }

            //保管期限
            l_BusReceiveEdit.KeepTime = ddlKeepTime.Text.TrimEnd();

            //对应合同号
            l_BusReceiveEdit.ContractNumber = txtContractNumber.Text.TrimEnd();

            //设备代码
            l_BusReceiveEdit.EquipmentCode = txtEquipmentCode.Text.TrimEnd();

            //HN编码
            l_BusReceiveEdit.HNCode = txtHNCode.Text.TrimEnd();

            //流程名称
            l_BusReceiveEdit.ProcessName = TemplateName;

            ////附件信息
            l_BusReceiveEdit.FileData = XmlUtility.SerializeXml(this.ucAttachment.UCDataList);
            //备注
            l_BusReceiveEdit.Remarks = this.txtRemark.Text;//renjinquan+
        }

        /// <summary>
        /// 用实体中的内容填充表单上编辑区域的内容
        /// </summary>
        /// <param name="p_BusReceiveEdit"></param>
        private void PopulateEditField(B_HSEdit p_BusReceiveEdit)
        {
            //单位
            txtReceiveUnit.Text = p_BusReceiveEdit.CommunicationUnit;

            //流程ID
            this.txtProcessID.Text = p_BusReceiveEdit.ProcessID;

            this.btnDetail.Enabled = !string.IsNullOrEmpty(p_BusReceiveEdit.ProcessID);
            this.btnLaunch.Enabled = string.IsNullOrEmpty(p_BusReceiveEdit.ProcessID);
            this.btnSecFF.Enabled = !string.IsNullOrEmpty(p_BusReceiveEdit.ProcessID);
            //收文号
            txtDocumentNo.Text = p_BusReceiveEdit.DocumentNo;

            //文件编码
            txtDocumentEncoding.Text = p_BusReceiveEdit.FileEncoding;

            //收文日期
            txtReceiveDate.Text = p_BusReceiveEdit.ReceiptDate.ToString("yyyy-MM-dd");

            //答复文号
            txtReplyDocumentNo.Text = p_BusReceiveEdit.ReplyDocumentNo;

            //其他编码
            txtOtherEncoding.Text = p_BusReceiveEdit.OtherEncoding;

            //行文日期
            if (!ValidateUtility.IsDateTime(p_BusReceiveEdit.XingWenDate.ToString()) || p_BusReceiveEdit.XingWenDate == DateTime.MinValue)//任金权修改，为了实现为空
            {
                txtXingWenDate.Text = "";
            }
            else
            {
                txtXingWenDate.Text = p_BusReceiveEdit.XingWenDate.ToString("yyyy-MM-dd");
            }

            //文件标题
            txtDocumentTitle.Text = p_BusReceiveEdit.DocumentTitle;

            //函件类型
            ddlDocumentType.Text = p_BusReceiveEdit.LetterType;

            //形成日期
            txtFormationDate.Text = p_BusReceiveEdit.FormationDate.ToString("yyyy-MM-dd");

            //紧急程度
            ddlUrgentDegree.Text = p_BusReceiveEdit.UrgentDegree;

            //页数
            if (p_BusReceiveEdit.Pages == int.MinValue)
            {
                txtPageCount.Text = String.Empty;
            }
            else
            {
                txtPageCount.Text = p_BusReceiveEdit.Pages.ToString();
            }

            //保管期限
            ddlKeepTime.Text = p_BusReceiveEdit.KeepTime;

            //对应合同号
            txtContractNumber.Text = p_BusReceiveEdit.ContractNumber;

            //设备代码
            txtEquipmentCode.Text = p_BusReceiveEdit.EquipmentCode;

            //HN编码
            txtHNCode.Text = p_BusReceiveEdit.HNCode;

            ////附件信息
            this.ucAttachment.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(p_BusReceiveEdit.FileData);

            //
            txtRegisterID.Text = p_BusReceiveEdit.ID.ToString();
            //备注信息
            this.txtRemark.Text = p_BusReceiveEdit.Remarks;//renjinquan+
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (!VerifyQueryField())
            {
                ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "表单验证失败");
                return;
            }

            LoadRegisterList();
        }

        private void LoadRegisterList()
        {
            B_HSEdit l_BusReceiveEdit = new B_HSEdit();

            l_BusReceiveEdit.ProcessName = TemplateName;

            //收文号-从
            l_BusReceiveEdit.ReceiveNoFrom = FormsMethod.Filter(txtQueryDocNoFrom.Text.TrimEnd());

            //收文号-到
            l_BusReceiveEdit.ReceiveNoTo = FormsMethod.Filter(txtQueryDocNoTo.Text.TrimEnd());

            //文件标题
            l_BusReceiveEdit.DocumentTitle = FormsMethod.Filter(txtQueryDocTitle.Text.TrimEnd());

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
            l_BusReceiveEdit.CommunicationUnit = FormsMethod.Filter(txtQueryRecUnit.Text.TrimEnd());

            l_BusReceiveEdit.Start = gvRegisterList.PageIndex * gvRegisterList.PageSize;
            l_BusReceiveEdit.End = gvRegisterList.PageIndex * gvRegisterList.PageSize + gvRegisterList.PageSize;
            l_BusReceiveEdit.Sort = SortExpression;

            this.gvRegisterList.DataSource = l_BusReceiveEdit.QueryRegisterInfo(l_BusReceiveEdit);
            this.gvRegisterList.RecordCount = l_BusReceiveEdit.RowCount;
            this.gvRegisterList.DataBind();
        }

        protected void gvRegisterList_ExteriorPaging(GridViewPageEventArgs e)
        {
            LoadRegisterList();
        }

        //protected void textchanged()
        //{
        //    if (this.txtDocumentEncoding.Text != "")
        //    {
        //        string strsql = "SELECT  FileEncoding  FROM T_OA_HS_Edit where fileencoding= '" + this.txtDocumentEncoding.Text + "'";
        //        DataTable dt = SQLHelper.GetDataTable1(strsql);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "", "alert('通讯渠道号重复，请确认!')", true);
        //        }
        //    }
        //}

        

        protected void gvRegisterList_ExteriorSorting(FounderSoftware.Framework.UI.WebCtrls.FSGridViewSortEventArgs e)
        {
            SortExpression = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending)
            {
                SortExpression += " ASC";
            }
            else
            {
                SortExpression += " DESC";
            }
            LoadRegisterList();
        }

        /// <summary>
        /// 根据传递的值绑定表单
        /// </summary>
        /// <param name="ID">页面的传值</param>
        public void SetFormByID(string ID)
        {
            B_HSEdit l_BusReceiveEdit = new B_HSEdit();
            if (m_ID != null)
            {
                l_BusReceiveEdit.ID = Convert.ToInt32(m_ID);
                PopulateEditField(l_BusReceiveEdit);
            }
        }

        protected String GetStatus(Object p_objProcessID)
        {
            if (p_objProcessID == DBNull.Value)
            {
                return "<font style='color:green'>未启动</font>";
            }
            else
            {
                return "<font style='color:red'>已启动</font>";
            }
        }

        #region ICallbackEventHandler 成员
        String m_strCallBack = null;
        public string GetCallbackResult()
        {
            return m_strCallBack;
        }

        
        //    public string GetCallbackResult()
        //    {
        //        // 返回服务器端处理结果给receiveServerResult方法
        //        return m_strResult;
        //    }

        //    public void RaiseCallbackEvent(string eventArgument)
        //    {
        //        // eventArgument是客户端传来的变量，对应arg变量
        //        // 在这里添加服务器端处理逻辑...
        //        m_strResult = eventArgument;
        //        if (m_strResult != "")
        //        {
        //            string strsql = "SELECT  FileEncoding  FROM T_OA_HS_Edit where fileencoding= '" + m_strResult + "'";
        //            DataTable dt = SQLHelper.GetDataTable1(strsql);
        //            m_strResult = dt.Rows.Count.ToString();
        //        }
        //    }

        //    #endregion
        //}


        public void RaiseCallbackEvent(string eventArgument)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            if (rex.IsMatch(eventArgument))
            {
                B_HSEdit l_BusReceiveEdit = new B_HSEdit();

                l_BusReceiveEdit.ID = Convert.ToInt32(eventArgument);
                if (B_HSEdit.GetLastWorkItem(l_BusReceiveEdit.ProcessID) == ProcessConstString.SubmitAction.ACTION_CANCEL)//renjinquan修改  可以重新发起撤销的流程
                {
                    l_BusReceiveEdit.ProcessID = "";
                    B_HSEdit.CandelProcess(eventArgument);
                }
                //附件信息
                if (!String.IsNullOrEmpty(l_BusReceiveEdit.FileData))
                {
                    List<CFuJian> l_objAttach = XmlUtility.DeSerializeXml<List<CFuJian>>(l_BusReceiveEdit.FileData);
                    l_BusReceiveEdit.FileData = SysString.FuJianList2Xml(l_objAttach);
                }

                StringWriter l_strWriter = new StringWriter(CultureInfo.InvariantCulture);
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                using (JsonWriter jsonWriter = new JsonWriter(l_strWriter))
                {
                    jsonSerializer.Serialize(jsonWriter, l_BusReceiveEdit);
                }

                Session["附件ListTemp"] = l_BusReceiveEdit.FileData;

                m_strCallBack = l_strWriter.ToString();
            }
            else
            {
                string strsql = "SELECT  FileEncoding  FROM T_OA_HS_Edit where fileencoding= '" + eventArgument + "'";
                DataTable dt = SQLHelper.GetDataTable1(strsql);
                m_strCallBack = dt.Rows.Count.ToString(); 
            }                        
        }

        #endregion

        protected void btnSecFF_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtProcessID.Text == string.Empty)
                {
                    JScript.Alert("只有已经启动的流程才能二次分发！");
                    return;
                }
                List<EntityBase> bworkitems = B_FormsData.GetEntities(this.txtProcessID.Text, "", ProcessConstString.TemplateName.LETTER_RECEIVE, "", true);
                B_LetterReceive NewEntity = new B_LetterReceive();
                B_LetterReceive bworkitem = bworkitems != null && bworkitems.Count > 0 ? bworkitems[0] as B_LetterReceive : new B_LetterReceive();

                NewEntity.ProcessID = this.txtProcessID.Text;

                NewEntity.WorkItemID = Guid.NewGuid().ToString("N");
                bworkitem.WorkItemID = NewEntity.WorkItemID;

                bworkitem.StepName = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF2;
                NewEntity.StepName = ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_FF2;

                bworkitem.SubmitDate = System.DateTime.Now;
                NewEntity.SubmitDate = System.DateTime.Now;

                bworkitem.CommentList.Clear();

                bworkitem.UrgentDegree = this.ddlUrgentDegree.SelectedValue;
                NewEntity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;

                bworkitem.DocumentNo = this.txtDocumentNo.Text;
                NewEntity.DocumentNo = this.txtDocumentNo.Text;

                bworkitem.ReceiptDate = this.txtReceiveDate.Text != string.Empty ? DateTime.Parse(this.txtReceiveDate.Text) : DateTime.MinValue;
                NewEntity.ReceiptDate = this.txtReceiveDate.Text != string.Empty ? DateTime.Parse(this.txtReceiveDate.Text) : DateTime.MinValue;

                bworkitem.FileEncoding = this.txtDocumentEncoding.Text;
                NewEntity.FileEncoding = this.txtDocumentEncoding.Text;

                bworkitem.CommunicationUnit = this.txtReceiveUnit.Text;
                NewEntity.CommunicationUnit = this.txtReceiveUnit.Text;

                bworkitem.DocumentTitle = this.txtDocumentTitle.Text;
                NewEntity.DocumentTitle = this.txtDocumentTitle.Text;

                bworkitem.Remarks = this.txtRemark.Text;
                NewEntity.Remarks = this.txtRemark.Text;

                bworkitem.FileList = this.ucAttachment.UCDataList;
                NewEntity.FileList = this.ucAttachment.UCDataList;

                bworkitem.ReceiveDateTime = DateTime.Now;
                NewEntity.ReceiveDateTime = DateTime.Now;

                bworkitem.ReceiveUserID = CurrentUserInfo.UserName;
                NewEntity.ReceiveUserID = CurrentUserInfo.UserName;

                bworkitem.ReceiveUserName = CurrentUserInfo.DisplayName;
                NewEntity.ReceiveUserName = CurrentUserInfo.DisplayName;
                NewEntity.D_StepStatus = bworkitem.D_StepStatus;
                NewEntity.ReceiptDate = bworkitem.ReceiptDate;
                NewEntity.Pages = bworkitem.Pages;
                NewEntity.ChuanYueLeader = bworkitem.ChuanYueLeader;
                NewEntity.AssistDept = bworkitem.AssistDept;
                NewEntity.AssistDeptName = bworkitem.AssistDeptName;
                NewEntity.ChuanYueLeader = bworkitem.ChuanYueLeader;
                NewEntity.ChuanYueDept = bworkitem.ChuanYueDept;
                NewEntity.LeaderShip = bworkitem.LeaderShip;
                NewEntity.LeaderShipName = bworkitem.LeaderShipName;
                NewEntity.LS_Comment = bworkitem.LS_Comment;
                NewEntity.UnderTakeID = bworkitem.UnderTakeID;
                NewEntity.UnderTake = bworkitem.UnderTake;
                NewEntity.UnderTake_Comment = bworkitem.UnderTake_Comment;
                NewEntity.UnderTakeLeaders = bworkitem.UnderTakeLeaders;
                NewEntity.NiBanRen = bworkitem.NiBanRen;
                NewEntity.NiBanRenName = bworkitem.NiBanRenName;
                NewEntity.NiBanComment = bworkitem.NiBanComment;
                NewEntity.Drafter = bworkitem.Drafter;
                NewEntity.DraftDate = bworkitem.DraftDate;
                NewEntity.RegisterID = bworkitem.RegisterID;
                NewEntity.Prompt = bworkitem.Prompt;
                NewEntity.ChuanYueDeptID = bworkitem.ChuanYueDeptID;
                NewEntity.HJPrompt = bworkitem.HJPrompt;
                NewEntity.ChuanYueLeaderID = bworkitem.ChuanYueLeaderID;
                NewEntity.AgentUserName = bworkitem.AgentUserName;
                NewEntity.AgentUserID = bworkitem.AgentUserID;

                NewEntity.FormsData = XmlUtility.SerializeXml(bworkitem as EntityBase);
                FormSave.SaveEntity(NewEntity, false);

                if (B_ProcessInstance.IsCompletedOrCancel(NewEntity.ProcessID))
                {
                    B_ProcessInstance.MoveToBakTable(NewEntity.WorkItemID, "T_OA_HS_WorkItems", "T_OA_HS_WorkItems_BAK");//在备份库中插入数据
                    B_ProcessInstance.UpdateRecordStatus(NewEntity.WorkItemID,2);//将现行库数据更新为不可用。
                }

                btnModify_Click(null, null);

                //string filedata = "<FileList />";
                //if (this.ucAttachment.UCDataList.Count > 0)
                //{
                //    string formdata = XmlUtility.SerializeXml<B_LetterReceive>(bworkitem);
                //    int iStartIndex = formdata.IndexOf("<FileList>");
                //    int iEndIndex = formdata.IndexOf("</FileList>");
                //    if (iEndIndex > iStartIndex)
                //    {
                //        filedata = formdata.Substring(iStartIndex, iEndIndex - iStartIndex);
                //        filedata += "</FileList>";
                //    }
                //}
                //FormsMethod.UpdateAssignFileData(bworkitem.ProcessID,ProcessConstString.TemplateName.LETTER_RECEIVE, "", filedata);
                List<EntityBase> bworkitemsAssign = B_FormsData.GetEntities(this.txtProcessID.Text, "", ProcessConstString.TemplateName.LETTER_RECEIVE, "", true, "'New','Assigned'");
                string strUpdatePeople = string.Empty;
                foreach (EntityBase entity in bworkitemsAssign)
                {
                    B_LetterReceive lentity = entity as B_LetterReceive;

                    lentity.SubmitDate = System.DateTime.Now;

                    lentity.UrgentDegree = this.ddlUrgentDegree.SelectedValue;

                    lentity.DocumentNo = this.txtDocumentNo.Text;

                    lentity.ReceiptDate = this.txtReceiveDate.Text != string.Empty ? DateTime.Parse(this.txtReceiveDate.Text) : DateTime.MinValue;
                    lentity.FileEncoding = this.txtDocumentEncoding.Text;

                    lentity.CommunicationUnit = this.txtReceiveUnit.Text;

                    lentity.DocumentTitle = this.txtDocumentTitle.Text;

                    lentity.Remarks = this.txtRemark.Text;

                    lentity.FileList = this.ucAttachment.UCDataList;
                    FormSave.SaveEntity(lentity as EntityBase, true);
                    strUpdatePeople += lentity.ReceiveUserName + ",";
                }
                B_ToCirculate l_busCirculate = new B_ToCirculate();
                l_busCirculate.IsAgain = true;
                l_busCirculate.ToProcessType = ProcessConstString.TemplateName.LETTER_RECEIVE;
                l_busCirculate.ToProcessID = bworkitem.ProcessID;
                l_busCirculate.ToWorkItemID = bworkitem.WorkItemID;
                l_busCirculate.ToUserIDS = B_FormsData.GetProcessPeople("'Completed','Removed'", this.txtProcessID.Text, ";", true);
                string strInfo = string.Empty;
                if (l_busCirculate.ToUserIDS != string.Empty)
                {
                    strInfo = l_busCirculate.ChuanYueToDB();
                }
                strUpdatePeople=strUpdatePeople.Length>0?strUpdatePeople.Substring(0,strUpdatePeople.Length-1):"";
                strInfo = strInfo.Replace("传阅成功", "二次分发成功");
                strInfo += string.IsNullOrEmpty(strInfo) ? "" : "\\n";
                strInfo +=string.IsNullOrEmpty(strUpdatePeople) ? "" : ("("+strUpdatePeople + ")更新成功！");
                if (strInfo != string.Empty)
                {
                    JScript.Alert(strInfo);
                }
                else
                {
                    JScript.Alert("二次分发成功！无人员传阅和数据更新！");
                }
            }
            catch
            {
                JScript.Alert("二次分发失败！");
            }
        }
    }
}