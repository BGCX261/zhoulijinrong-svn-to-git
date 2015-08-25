using System;
using System.Data;
using System.Web.UI;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.OA.Framework.WorkFlow;
using Ascentn.Workflow.Base;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail
{
    public partial class UC_ReDetail : UCBase
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

        public string ProcessTemplate
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
        protected void Page_Load(object sender, EventArgs e)
        {
            B_ReceiveEdit l_BusReceiveEdit = null;
            B_Circulate l_BusCirculate = null;
            DataTable l_dtbCirculate = null;
            DataTable l_dtbProcessSteps = null;
            String l_strRegisterID = null;

            M_ReceiveBase l_objWorkItem = null;

            //WFBaseProcessInstance l_objProcessInstance = null;
            if (!Page.IsPostBack)
            {
                this.ucAttachment.UCIsEditable = false;

                PreviousPageUrl = Request.UrlReferrer.ToString();
                l_strRegisterID = Request.QueryString[ConstString.QueryString.REGISTER_ID];
                l_BusReceiveEdit = new B_ReceiveEdit();
                l_BusReceiveEdit.ID = Convert.ToInt32(l_strRegisterID);

                if (l_BusReceiveEdit == null)
                {
                    JScript.Alert("当前的ID或者无效，或者已经被删除！");
                    return;
                }

                this.ucAttachment.UCTemplateName = l_BusReceiveEdit.ProcessName;

                PopulateReceiveField(l_BusReceiveEdit);

                if (String.IsNullOrEmpty(l_BusReceiveEdit.ProcessID))
                {
                    divPrompt.Visible = true;
                    return;
                }
                WFBaseProcessInstance fp = null;
                fp = WFFactory.GetWF(WFType.AgilePoint).GetAPI().GetProcInst(l_BusReceiveEdit.ProcessID);
                //l_objProcessInstance = AgilePointWF.GetAPI().GetProcInst();

                if (fp != null)
                {
                    ProcessTemplate = fp.DefName;

                    String l_strCirculateTableName = FS.ADIM.OA.BLL.Common.TableName.GetCirculateTableName(fp.DefName);
                    l_BusCirculate = new B_Circulate(l_strCirculateTableName);
                    l_dtbCirculate = l_BusCirculate.GetCirculatesByID(l_strCirculateTableName, l_BusReceiveEdit.ProcessID, 0);
                    gdvCirculate.DataSource = l_dtbCirculate;
                    gdvCirculate.DataBind();
                    string strTableName = TableName.WorkItemsTableName.T_OA_GS_WorkItems;
                    switch (fp.DefName)
                    {
                        case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                            l_objWorkItem = new B_GS_WorkItems();
                            strTableName = TableName.WorkItemsTableName.T_OA_GS_WorkItems;
                            break;
                        default:
                            strTableName = TableName.WorkItemsTableName.T_OA_MS_WorkItems;
                            l_objWorkItem = new B_MergeReceiveBase();
                            break;
                    }
                    strTableName+=((fp.Status == ProcessConstString.ProcessStatus.STATUS_COMPLETED || fp.Status == ProcessConstString.ProcessStatus.STATUS_CANCELED)?"_BAK":"");
                    l_dtbProcessSteps = l_objWorkItem.GetStepsByProcessID(l_BusReceiveEdit.ProcessID, strTableName, l_objWorkItem);

                    DataView l_dtvDataView = new DataView(l_dtbProcessSteps);
                    l_dtvDataView.RowFilter = "D_StepStatus = 'Completed'";

                    rptProcessDetail.DataSource = l_dtvDataView;
                    rptProcessDetail.DataBind();
                }
            }
        }

        /// <summary>
        /// Populated the edit field with the Entity
        /// </summary>
        /// <param name="p_BusReceiveEdit"></param>
        private void PopulateReceiveField(B_ReceiveEdit p_BusReceiveEdit)
        {
            //单位
            txtReceiveUnit.Text = p_BusReceiveEdit.ReceiveUnit;

            //年份
            txtReceiveYear.Text = p_BusReceiveEdit.ReceiveYear;

            //收文号
            txtReceiveNo.Text = p_BusReceiveEdit.ReceiveNo;

            //收文日期
            txtReceiveDate.Text = p_BusReceiveEdit.ReceiveDate.ToString("yyyy-MM-dd");

            //行文号
            txtSendNo.Text = p_BusReceiveEdit.SendLetterNo;

            //行文日期
            txtSendDate.Text = p_BusReceiveEdit.SendLetterDate.ToString("yyyy-MM-dd");

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
            txtKeepTime.Text = p_BusReceiveEdit.KeepTime.ToString();

            //密级
            txtSecretLevel.Text = p_BusReceiveEdit.SecretLevel;

            //紧急程度
            txtUrgentDegree.Text = p_BusReceiveEdit.UrgentDegree;

            //来文单位
            txtSendUnit.Text = p_BusReceiveEdit.SendLetterUnit;

            //预立卷号
            txtPreVolumnNo.Text = p_BusReceiveEdit.PreVolumeNo;

            //备注
            txtRemark.Text = p_BusReceiveEdit.Remarks;

            //归档状态
            txtArchiveStatus.Text = p_BusReceiveEdit.ArchiveStatus;

            //是否直接归档
            chkIsArchive.Checked = p_BusReceiveEdit.Is_Archive == "1" ? true : false;

            //附件信息
            this.ucAttachment.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(p_BusReceiveEdit.FileData);
        }

        protected void rptProcessDetail_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            DataRowView l_drvRowItem = null;
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                UC_Step l_objStep = (UC_Step)e.Item.FindControl("ucStep");
                l_drvRowItem = e.Item.DataItem as DataRowView;

                EntityBase entity = null;
                //entity.file

                l_objStep.Participant = l_drvRowItem["ReceiveUserID"].ToString() + "(" + l_drvRowItem["ReceiveUserName"].ToString() + ")";
                l_objStep.m_strStepName = l_drvRowItem["StepName"].ToString();
                l_objStep.SubmitDataTime = l_drvRowItem["SubmitDate"].ToString();
                l_objStep.SubmitAction = l_drvRowItem["SubmitAction"].ToString();

                switch (ProcessTemplate)
                {
                    //case ConstString.ProcessTemplate.TRADE_UNION_RECEIVE:
                    //    l_objWorkItem = XmlUtility.DeSerializeXml<B_GHS_WorkItems>(l_drvRowItem["FormsData"].ToString());
                    //    break;
                    //case ConstString.ProcessTemplate.PARTY_DISCIPLINE_RECEIVE:
                    //    l_objWorkItem = XmlUtility.DeSerializeXml<B_DJS_WorkItems>(l_drvRowItem["FormsData"].ToString());
                    //    break;
                    //case ConstString.ProcessTemplate.YOUTH_LEAGUE_RECEIVE:
                    //    l_objWorkItem = XmlUtility.DeSerializeXml<B_TWS_WorkItems>(l_drvRowItem["FormsData"].ToString());
                    //    break;
                    case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                        B_GS_WorkItems l_objWorkItem = XmlUtility.DeSerializeXml<B_GS_WorkItems>(l_drvRowItem["FormsData"].ToString());
                        entity = l_objWorkItem;
                        l_objStep.Prompt = l_objWorkItem.PromptEdit;
                        switch (l_objStep.m_strStepName)
                        {
                            //步骤名称-承办
                            case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_DIRECTOR:
                            case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_CHIEF:
                            case ProcessConstString.StepName.ReceiveStepName.STEP_SECTION_MEMBER:
                                l_objStep.Comment = l_objWorkItem.UnderTake_Comment;
                                break;
                            //步骤名称-领导批示
                            case ProcessConstString.StepName.ReceiveStepName.STEP_INSTRUCTION:
                                l_objStep.Comment = l_objWorkItem.LS_Comment;
                                break;
                            case ProcessConstString.StepName.ReceiveStepName.STEP_OFFICE:
                                l_objStep.Comment = l_objWorkItem.Officer_Comment;
                                break;
                            default:
                                break;
                        }
                        break;
                }

                //l_objStep.Prompt = l_drvRowItem["Prompt"].ToString();

                //附件信息
                if (entity != null && entity.FileList.Count > 0)
                {
                    l_objStep.Attachment.UCProcessID = l_drvRowItem["ProcessID"].ToString();
                    l_objStep.Attachment.UCTemplateName = ProcessTemplate;
                    l_objStep.Attachment.UCDataList = entity.FileList;
                }
                else
                {
                    l_objStep.HiddenAttach();
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.Register.PG_Register&TemplateName=" + ProcessTemplate + "&ID=" + Request.QueryString[ConstString.QueryString.REGISTER_ID]);
        }
    }
}