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
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi;
using FounderSoftware.Framework.UI.WebCtrls;
using Ascentn.Workflow.Base;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.WebUI.UIBase;
using System.Collections;
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.Register
{
    public partial class UC_Register : ListUIBase, ICallbackEventHandler
    {
        /// <summary>
        /// 流程模版名称
        /// </summary>
        protected String TemplateName
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

        /// <summary>
        /// 流程模版名称
        /// </summary>
        protected String SubTemplateName
        {
            get
            {
                if (ViewState["SubTemplateName"] == null)
                {
                    return String.Empty;
                }
                return Convert.ToString(ViewState["SubTemplateName"]);
            }
            set
            {
                ViewState["SubTemplateName"] = value;
            }
        }

        /// 流程模版名称
        /// </summary>
        protected String FormID
        {
            get
            {
                if (ViewState["ID"] == null)
                {
                    ViewState["ID"] = Request.QueryString["ID"] == null ? "" : Request.QueryString["ID"].ToString();
                }
                return Convert.ToString(ViewState["ID"]);
            }
            set
            {
                ViewState["ID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet l_dstDataSet = null;
            if (!Page.IsPostBack)
            {
                this.ucSendUnit.UCNameControl = this.txtReceiveUnit.ClientID;
                this.CompanyUC2.UCNameControl = this.txtQueryRecUnit.ClientID;

                this.TemplateName = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];

                if (String.IsNullOrEmpty(this.TemplateName))
                {
                    JScript.ShowMsgBox(this.Page, MsgType.VbCritical, "没有指定收文流程模版！", "Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.PG_WaitHandle");
                    return;
                }

                l_dstDataSet = OAConfig.GetRankConfig();

                DataTable l_dtbDataTable = l_dstDataSet.Tables[this.TemplateName == ProcessConstString.TemplateName.COMPANY_RECEIVE ? this.TemplateName : ProcessConstString.TemplateName.MERGED_RECEIVE];//renjinquan+

                String[] l_strAryRoleName = l_dtbDataTable.Rows[0]["角色"].ToString().Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries);
                if (l_strAryRoleName.Length == 0)
                {
                    JScript.RedirectPage(this.Page, "未配置收文登记权限", this.TemplateName, "1");
                    return;
                }

                String[] l_strAryUserRoleNames = CurrentUserInfo.RoleName.ToArray();

                IEnumerable<String> l_enumRole = l_strAryUserRoleNames.Intersect(l_strAryRoleName);

                if (l_enumRole.Count() == 0)
                {
                    JScript.RedirectPage(this.Page, string.Format("只有[{0}] {1} 可以访问该表单！", l_strAryRoleName[0], OAUser.GetUserByRole(l_strAryRoleName[0]).GetFieldVals("Name", ",")), TemplateName, "1");
                    return;
                }

                if (TemplateName !=ProcessConstString.TemplateName.COMPANY_RECEIVE)
                {
                    trProcessTemplate.Visible = true;
                    this.ddlProcessTemplate.SelectedValue = this.TemplateName;
                }
                else
                {
                    SubTemplateName = TemplateName;
                }

                //设置客户端只读
                txtReceiveUnit.Attributes.Add("readOnly", "true");
                ////////////////////////////////////////////////////20110124 扬子江
                //if (!(TemplateName.Equals("党纪工团收文")))
                //{
                    txtDocumentNo.Attributes.Add("readonly", "true");
                //}

                this.ucFileList.UCTemplateName = TemplateName;

                //收文年份默认加载前后十年,并且默认选择当前年份
                int l_intYear = DateTime.Now.Year;
                for (int i = l_intYear - 10; i < l_intYear + 10; i++)
                {
                    ddlReceiveYear.Items.Add(i.ToString());
                    ddlQueryRecYear.Items.Add(i.ToString());
                }
                ddlReceiveYear.Text = l_intYear.ToString();
                txtReceiveDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                LoadRegisterList();

                if (!String.IsNullOrEmpty(FormID))
                {
                    SetFormByID(FormID);
                }
                if (this.TemplateName != ProcessConstString.TemplateName.COMPANY_RECEIVE && this.ddlProcessTemplate.SelectedIndex >= 0)
                {
                    ddlProcessTemplate_SelectedIndexChanged(null, null);
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
            B_ReceiveEdit l_BusReceiveEdit = null;

            //表单的合法性验证
            if (!VerifyEditField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            //行文号是否重复
            if (B_ReceiveEdit.IsHaveSendNo(this.txtSendNo.Text.TrimEnd()))//任金权增加
            {
                JScript.ShowMsgBox(this.Page, "行文号重复", false);
                this.txtSendNo.Focus();
                return;
            }

            l_BusReceiveEdit = new B_ReceiveEdit();

            PopulateEntity(l_BusReceiveEdit);

            
            //l_BusReceiveEdit.ReceiveNo = B_ReceiveID.GenerateReceiveNo(DateTime.Now.Year.ToString(), TemplateName);
            ////////////////////////////////////////////////////20110124 扬子江
            if (!(TemplateName.Equals("党纪工团收文")))
            {
                l_BusReceiveEdit.ReceiveNo = B_ReceiveID.GenerateReceiveNo(DateTime.Now.Year.ToString(), TemplateName);
            }
            else
            {
                this.SubTemplateName = ddlProcessTemplate.SelectedValue;
                if (this.SubTemplateName == "工会收文")
                {
                    l_BusReceiveEdit.ReceiveNo = B_ReceiveID.GenerateDJGTReceiveNo("H" + DateTime.Now.Year.ToString(), this.SubTemplateName);
                }
                if (this.SubTemplateName == "团委收文")
                {
                    l_BusReceiveEdit.ReceiveNo = B_ReceiveID.GenerateDJGTReceiveNo("T" + DateTime.Now.Year.ToString(), this.SubTemplateName);
                }
                if (this.SubTemplateName == "党委纪委收文")
                {
                    l_BusReceiveEdit.ReceiveNo = B_ReceiveID.GenerateDJGTReceiveNo("D" + DateTime.Now.Year.ToString(), this.SubTemplateName);
                }
            }

            //归档状态
            if (chkIsArchive.Checked)
            {
                l_BusReceiveEdit.ArchiveStatus = "已归档";
            }
            else
            {
                l_BusReceiveEdit.ArchiveStatus = "未完成";
            }

            if (l_BusReceiveEdit.Save())
            {
                JScript.ShowMsgBox(this.Page, "新增成功", false);
                PopulateEditField(l_BusReceiveEdit);
                btnQuery_Click(null, null);
            }
        }

        /// <summary>
        /// 修改按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnModify_Click(object sender, EventArgs e)
        {
            B_ReceiveEdit l_BusReceiveEdit = null;

            if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            {
                JScript.ShowMsgBox(this.Page, "请先选择一条收文记录！", false);
                return;
            }
            if (!VerifyEditField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            l_BusReceiveEdit = new B_ReceiveEdit();
            l_BusReceiveEdit.ID = Convert.ToInt32(txtRegisterID.Text);

            if (this.txtSendNo.Text.TrimEnd() != l_BusReceiveEdit.SendLetterNo)//任金权增加
            {
                //行文号是否重复
                if (B_ReceiveEdit.IsHaveSendNo(this.txtSendNo.Text.TrimEnd()))
                {
                    JScript.ShowMsgBox(this.Page, "行文号重复", false);
                    this.txtSendNo.Focus();
                    return;
                }
            }

            if (l_BusReceiveEdit.CreateDate == DateTime.MinValue)
            {
                JScript.ShowMsgBox(this.Page, "当前收文登记信息已经被删除，不能做修改动作！", false);
                return;
            }

            PopulateEntity(l_BusReceiveEdit);

            //归档状态
            if (chkIsArchive.Checked)
            {
                l_BusReceiveEdit.ArchiveStatus = "已归档";
            }
            else
            {
                l_BusReceiveEdit.ArchiveStatus = "未完成";
            }

            if (l_BusReceiveEdit.Save())
            {
                JScript.ShowMsgBox(this.Page, "保存成功", false);
                PopulateEditField(l_BusReceiveEdit);
                LoadRegisterList();
            }
        }

        /// <summary>
        /// 删除按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            B_ReceiveEdit l_BusReceiveEdit = null;

            if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            {
                JScript.ShowMsgBox(this.Page, "请先选择一条收文记录！", false);
                return;
            }
            l_BusReceiveEdit = new B_ReceiveEdit();

            l_BusReceiveEdit.ID = Convert.ToInt32(txtRegisterID.Text);

            if (l_BusReceiveEdit.Delete())
            {
                JScript.ShowMsgBox(this.Page, "删除成功", false);
                ClearEditField();
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
            B_ReceiveEdit l_BusReceiveEdit = null;

            if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            {
                JScript.ShowMsgBox(this.Page, "请先选择一条收文记录！", false);
                return;
            }

            l_BusReceiveEdit = new B_ReceiveEdit();

            l_BusReceiveEdit.ID = Convert.ToInt32(txtRegisterID.Text);

            l_BusReceiveEdit.Save();
            if (!String.IsNullOrEmpty(l_BusReceiveEdit.ProcessID))
            {
                JScript.ShowMsgBox(this.Page, "当前选择的收文登记记录已经启动！", false);
                return;
            }

            String l_strSuffix = ConstString.QueryString.REGISTER_ID + "=" + Convert.ToString(txtRegisterID.Text);

            switch (SubTemplateName)
            {
                case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                    Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.PG_CompanyReceive&TemplateName=" + Server.UrlEncode(ProcessConstString.TemplateName.COMPANY_RECEIVE) + "&" + l_strSuffix, true);
                    break;
                case ProcessConstString.TemplateName.TRADE_UNION_RECEIVE:
                    Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.PG_Receive&TemplateName=" +  Server.UrlEncode(ProcessConstString.TemplateName.MERGED_RECEIVE) + "&" + l_strSuffix, true);
                    break;
                case ProcessConstString.TemplateName.PARTY_DISCIPLINE_RECEIVE:
                    Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.PG_Receive&TemplateName=" +  Server.UrlEncode(ProcessConstString.TemplateName.MERGED_RECEIVE) + "&" + l_strSuffix, true);
                    break;
                case ProcessConstString.TemplateName.YOUTH_LEAGUE_RECEIVE:
                    Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.PG_Receive&TemplateName=" +  Server.UrlEncode(ProcessConstString.TemplateName.MERGED_RECEIVE) + "&" + l_strSuffix, true);
                    break;
            }
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
                JScript.ShowMsgBox(this.Page, "请先选择一条收文记录！", false);
                return;
            }
            Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail.PG_ReDetail&" + ConstString.QueryString.REGISTER_ID + "=" + Convert.ToString(txtRegisterID.Text), true);
        }

        /// <summary>
        /// 打印按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.TemplateName != ProcessConstString.TemplateName.COMPANY_RECEIVE)
            {
                if (this.ddlProcessTemplate.SelectedValue == string.Empty)
                {
                    JScript.ShowMsgBox(this.Page, "请先选择流程类型！", false);
                    return;
                }
                else
                {
                    this.TemplateName = this.ddlProcessTemplate.SelectedValue;
                }
            }
            //if (String.IsNullOrEmpty(txtDocumentNo.Text.TrimEnd()))
            //{
            //    JScript.ShowMsgBox(this.Page, "请先选择一条收文记录！", false);
            //    return;
            //}
            Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.RePrint.PG_RePrint&" + ConstString.QueryString.TEMPLATE_NAME + "=" + TemplateName + "&" + ConstString.QueryString.REGISTER_ID + "=" + Convert.ToString(txtRegisterID.Text), true);
            //Response.Redirect("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.RePrint.PG_RePrint&" + ConstString.QueryString.REGISTER_ID + "=" + Convert.ToString(txtRegisterID.Text), true);
        }

        /// <summary>
        /// 清楚表单编辑区域内容，恢复至加载时状态
        /// </summary>
        private void ClearEditField()
        {
            this.txtRegisterID.Text = String.Empty;
            txtReceiveUnit.Text = String.Empty;
            ddlReceiveYear.Text = String.Empty;
            ddlReceiveYear.Text = String.Empty;
            txtDocumentNo.Text = String.Empty;
            txtReceiveDate.Text = String.Empty;
            txtSendNo.Text = String.Empty;
            txtSendDate.Text = String.Empty;
            txtDocumentTitle.Text = String.Empty;           
            txtKeyWord.Text = String.Empty;
            txtPageCount.Text = String.Empty;
            txtShareCount.Text = String.Empty;
            txtAttchCount.Text = String.Empty;
            ddlKeepTime.Text = String.Empty;
            ddlSecretLevel.Text = String.Empty;
            ddlUrgentDegree.Text = String.Empty;
            txtPreVolumnNo.Text = String.Empty;
            txtRemark.Text = String.Empty;
            txtArchiveStatus.Text = String.Empty;
            chkIsArchive.Checked = false;
        }

        /// <summary>
        /// 查询按钮的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (!VerifyQueryField())
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, m_strAryMessages);
                return;
            }

            LoadRegisterList();
        }

        /// <summary>
        /// 编辑区域元素验证
        /// </summary>
        /// <returns></returns>
        private bool VerifyEditField()
        {
            Boolean l_blnIsValid = true;

            if (this.TemplateName == "党纪工团收文" && String.IsNullOrEmpty(ddlProcessTemplate.SelectedValue))
            {
                m_strAryMessages.Add("收文登记:必须选择流程类型");
                l_blnIsValid = false;
            }

            //文件标题
            if (String.IsNullOrEmpty(txtDocumentTitle.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:文件标题不能为空");
                txtDocumentTitle.Focus();
                l_blnIsValid = false;
            }
            if (txtDocumentTitle.Text.Contains("#") || txtDocumentTitle.Text.Contains("'"))
            {
                m_strAryMessages.Add("含有特殊字符，请替换后再上传");
                txtDocumentTitle.Focus();
                l_blnIsValid = false;
            }  
            //来文单位
            if (String.IsNullOrEmpty(txtReceiveUnit.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:来文单位不能为空");
                l_blnIsValid = false;
            }
            //收文日期
            if (String.IsNullOrEmpty(txtReceiveDate.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:收文日期不能为空");
                l_blnIsValid = false;
            }
            else if (!ValidateUtility.IsDateTime(txtReceiveDate.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:收文日期格式不正确");
                l_blnIsValid = false;
            }
            //行文号
            if (String.IsNullOrEmpty(txtSendNo.Text.TrimEnd()) || txtSendNo.Text.TrimEnd() == "〔〕")
            {
                m_strAryMessages.Add("收文登记:行文号不能为空");
                l_blnIsValid = false;
            }
            else
            {
                //行文号格式验证
                if (txtSendNo.Text.IndexOf('〔') > txtSendNo.Text.IndexOf('〕') || txtSendNo.Text.IndexOf('〔') < 0)
                {
                    m_strAryMessages.Add("收文登记:行文号格式错误");
                    l_blnIsValid = false;
                }
            }
            //行文日期
            if (String.IsNullOrEmpty(txtSendDate.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:行文日期不能为空");
                l_blnIsValid = false;
            }
            else if (!ValidateUtility.IsDateTime(txtSendDate.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:行文日期格式不正确");
                l_blnIsValid = false;
            }
            //正文页数
            if (txtPageCount.Text.TrimEnd() != String.Empty && !ValidateUtility.IsInt(txtPageCount.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:正文页数格式不正确,须为正整数");
                l_blnIsValid = false;
            }
            //份数
            if (txtShareCount.Text.TrimEnd() != String.Empty && !ValidateUtility.IsInt(txtShareCount.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:份数格式不正确,须为正整数");
                l_blnIsValid = false;
            }
            //附件/页数
            if (txtAttchCount.Text.TrimEnd() != String.Empty && !ValidateUtility.IsInt(txtAttchCount.Text.TrimEnd()))
            {
                m_strAryMessages.Add("收文登记:附件/页数格式不正确,须为正整数");
                l_blnIsValid = false;
            }

            return l_blnIsValid;
        }

        /// <summary>
        /// 查询区块元素验证
        /// </summary>
        /// <returns></returns>
        private bool VerifyQueryField()
        {
            Boolean l_blnIsValid = true;

            //收文日期-从
            if (!String.IsNullOrEmpty(txtQueryRecDateFrom.Text.TrimEnd()) && !ValidateUtility.IsDateTime(txtQueryRecDateFrom.Text.TrimEnd()))
            {
                m_strAryMessages.Add("查询区块-收文开始日期格式不正确");
                l_blnIsValid = false;
            }

            //收文日期-到
            if (!String.IsNullOrEmpty(txtQueryRecDateTo.Text.TrimEnd()) && !ValidateUtility.IsDateTime(txtQueryRecDateTo.Text.TrimEnd()))
            {
                m_strAryMessages.Add("查询区块-收文结束日期格式不正确");
                l_blnIsValid = false;
            }

            return l_blnIsValid;
        }

        /// <summary>
        /// 用表单元素内容填充实体内容
        /// </summary>
        /// <param name="l_BusReceiveEdit"></param>
        private void PopulateEntity(B_ReceiveEdit l_BusReceiveEdit)
        {
            //单位
            l_BusReceiveEdit.ReceiveUnit = txtReceiveUnit.Text.TrimEnd();

            //年份
            l_BusReceiveEdit.ReceiveYear = ddlReceiveYear.Text.TrimEnd();

            //收文日期
            l_BusReceiveEdit.ReceiveDate = txtReceiveDate.ValDate;

            //行文号
            l_BusReceiveEdit.SendLetterNo = txtSendNo.Text.TrimEnd();

            //行文日期
            l_BusReceiveEdit.SendLetterDate = txtSendDate.ValDate;

            //文件标题
            l_BusReceiveEdit.DocumentTitle = txtDocumentTitle.Text.TrimEnd();

            //主题词
            l_BusReceiveEdit.SubjectWord = txtKeyWord.Text.TrimEnd();

            //正文页数
            if (ValidateUtility.IsInt(txtPageCount.Text.TrimEnd()))
            {
                l_BusReceiveEdit.PageCount = Convert.ToInt32(txtPageCount.Text.TrimEnd());
            }

            //份数
            if (ValidateUtility.IsInt(txtShareCount.Text.TrimEnd()))
            {
                l_BusReceiveEdit.ShareCount = Convert.ToInt32(txtShareCount.Text.TrimEnd());
            }

            //附件/页数
            if (ValidateUtility.IsInt(txtAttchCount.Text.TrimEnd()))
            {
                l_BusReceiveEdit.AttachmentCount = Convert.ToInt32(txtAttchCount.Text.TrimEnd());
            }

            //保管期限
            l_BusReceiveEdit.KeepTime = ddlKeepTime.Text.TrimEnd();

            //密级
            l_BusReceiveEdit.SecretLevel = ddlSecretLevel.Text.TrimEnd();

            //紧急程度
            l_BusReceiveEdit.UrgentDegree = ddlUrgentDegree.Text.TrimEnd();

            //预立卷号
            l_BusReceiveEdit.PreVolumeNo = txtPreVolumnNo.Text.TrimEnd();

            //备注
            l_BusReceiveEdit.Remarks = txtRemark.Text.TrimEnd();

            //流程名称
            l_BusReceiveEdit.ProcessName = SubTemplateName;

            //是否直接归档
            l_BusReceiveEdit.Is_Archive = chkIsArchive.Checked ? "1" : "0";

            //附件信息
            l_BusReceiveEdit.FileData = XmlUtility.SerializeXml(this.ucFileList.UCDataList);
        }

        /// <summary>
        /// 根据获取的ID填充表单
        /// </summary>
        /// <param name="ID"></param>
        public void SetFormByID(string ID)
        {
            B_ReceiveEdit l_receiveedit = new B_ReceiveEdit();

            l_receiveedit.ID = Convert.ToInt32(ID);

            PopulateEditField(l_receiveedit);
        }

        /// <summary>
        /// 用实体中的内容填充表单上编辑区域的内容
        /// </summary>
        /// <param name="p_BusReceiveEdit"></param>
        private void PopulateEditField(B_ReceiveEdit p_BusReceiveEdit)
        {
            if (p_BusReceiveEdit.ProcessName != ProcessConstString.TemplateName.COMPANY_RECEIVE)
            {
                this.ddlProcessTemplate.SelectedValue = p_BusReceiveEdit.ProcessName;
            }
            this.SubTemplateName = p_BusReceiveEdit.ProcessName;
            //单位
            txtReceiveUnit.Text = p_BusReceiveEdit.ReceiveUnit;

            //收文年份
            ddlReceiveYear.Text = p_BusReceiveEdit.ReceiveYear;

            //收文号
            txtDocumentNo.Text = p_BusReceiveEdit.ReceiveNo;

            //收文日期
            txtReceiveDate.Text = p_BusReceiveEdit.ReceiveDate.ToString(ConstString.DateFormat.Normal);

            //行文号
            txtSendNo.Text = p_BusReceiveEdit.SendLetterNo;

            //行文日期
            txtSendDate.Text = p_BusReceiveEdit.SendLetterDate.ToString(ConstString.DateFormat.Normal);

            //文件标题
            txtDocumentTitle.Text = p_BusReceiveEdit.DocumentTitle;

            //主题词
            txtKeyWord.Text = p_BusReceiveEdit.SubjectWord;

            //正文页数
            if (p_BusReceiveEdit.PageCount == int.MinValue)
            {
                txtPageCount.Text = String.Empty;
            }
            else
            {
                txtPageCount.Text = p_BusReceiveEdit.PageCount.ToString();
            }

            //份数
            if (p_BusReceiveEdit.ShareCount == int.MinValue)
            {
                txtShareCount.Text = String.Empty;
            }
            else
            {
                txtShareCount.Text = p_BusReceiveEdit.ShareCount.ToString();
            }

            //附件/页数
            if (p_BusReceiveEdit.AttachmentCount == int.MinValue)
            {
                txtAttchCount.Text = String.Empty;
            }
            else
            {
                txtAttchCount.Text = p_BusReceiveEdit.AttachmentCount.ToString();
            }

            //保管期限
            ddlKeepTime.Text = p_BusReceiveEdit.KeepTime;

            //密级
            ddlSecretLevel.Text = p_BusReceiveEdit.SecretLevel;

            //紧急程度
            ddlUrgentDegree.Text = p_BusReceiveEdit.UrgentDegree;

            //预立卷号
            txtPreVolumnNo.Text = p_BusReceiveEdit.PreVolumeNo;

            //备注
            txtRemark.Text = p_BusReceiveEdit.Remarks;

            //归档状态
            txtArchiveStatus.Text = p_BusReceiveEdit.ArchiveStatus;

            //是否直接归档
            chkIsArchive.Checked = p_BusReceiveEdit.Is_Archive == "1" ? true : false;

            if (p_BusReceiveEdit.ArchiveStatus == "已归档" || !String.IsNullOrEmpty(p_BusReceiveEdit.ProcessID))
            {
                btnLaunch.Enabled = false;
                chkIsArchive.Enabled = false;
                this.btnDetail.Enabled = true;
            }
            else
            {
                btnLaunch.Enabled = true;
                chkIsArchive.Enabled = true;
                this.btnDetail.Enabled = false;
            }

            //附件信息
            this.ucFileList.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(p_BusReceiveEdit.FileData);

            txtRegisterID.Text = p_BusReceiveEdit.ID.ToString();

        }

        private void LoadRegisterList()
        {
            B_ReceiveEdit l_BusReceiveEdit = null;

            //if (String.IsNullOrEmpty(SubTemplateName))
            //{
            //    return;
            //}

            l_BusReceiveEdit = new B_ReceiveEdit();

            l_BusReceiveEdit.ProcessName = SubTemplateName;

            //收文号-从
            l_BusReceiveEdit.ReceiveNoFrom = FormsMethod.Filter(txtQueryDocNoFrom.Text.TrimEnd());

            //收文号-到
            l_BusReceiveEdit.ReceiveNoTo = FormsMethod.Filter(txtQueryDocNoTo.Text.TrimEnd());

            //文件标题
            l_BusReceiveEdit.DocumentTitle = FormsMethod.Filter(txtQueryDocTitle.Text.TrimEnd());

            //收文日期-从
            l_BusReceiveEdit.ReceiveDateFrom = txtQueryRecDateFrom.ValDate;

            //收文日期-到
            l_BusReceiveEdit.ReceiveDateTo = txtQueryRecDateTo.ValDate;

            //来文单位
            l_BusReceiveEdit.ReceiveUnit =FormsMethod.Filter(txtQueryRecUnit.Text.TrimEnd());

            //收文年份
            l_BusReceiveEdit.ReceiveYear = ddlQueryRecYear.Text.TrimEnd();

            //状态
            if (ddlQueryStatus.SelectedItem != null)
            {
                l_BusReceiveEdit.Status = ddlQueryStatus.SelectedItem.Text;
            }

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

        protected String GetStatus(Object p_objProcessID, Object p_objArchiveStatus)
        {
            if (p_objProcessID == DBNull.Value || String.IsNullOrEmpty(p_objProcessID.ToString()))
            {
                return "<font style='color:green'>未启动</font>";
            }
            if (p_objArchiveStatus.ToString() == "已归档")
            {
                return "<font style='color:blue'>已归档</font>";
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

        public void RaiseCallbackEvent(string eventArgument)
        {
            B_ReceiveEdit l_BusReceiveEdit = new B_ReceiveEdit();

            l_BusReceiveEdit.ID = Convert.ToInt32(eventArgument);

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

        #endregion

        protected void ddlProcessTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SubTemplateName = ddlProcessTemplate.SelectedValue;
            //this.TemplateName = ddlProcessTemplate.SelectedValue;
            LoadRegisterList();
        }
    }
}