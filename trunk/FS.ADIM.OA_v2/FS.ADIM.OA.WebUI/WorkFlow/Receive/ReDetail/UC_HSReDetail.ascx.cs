using System;
using System.Data;
using System.Web.UI;
using Ascentn.Workflow.Base;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail
{
    public partial class UC_HSReDetail : UCBase
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

        protected string m_strProcessTemplate = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            B_HSEdit l_BusReceiveEdit = null;
            B_Circulate l_BusCirculate = null;
            DataTable l_dtbCirculate = null;
            DataTable l_dtbProcessSteps = null;
            String l_strRegisterID = null;

            B_LetterReceive l_objWorkItem = null;

            WFBaseProcessInstance l_objProcessInstance = null;

            if (!Page.IsPostBack)
            {
                this.ucAttachment.UCIsEditable = false;

                PreviousPageUrl = Request.UrlReferrer.ToString();
                l_strRegisterID = Request.QueryString[ConstString.QueryString.REGISTER_ID];
                l_BusReceiveEdit = new B_HSEdit();
                l_BusReceiveEdit.ID = Convert.ToInt32(l_strRegisterID);

                if (l_BusReceiveEdit == null)
                {
                    ValidateUtility.ShowMsgBox(this.Page, FS.ADIM.OA.BLL.Common.Utility.MessageType.VbCritical, "当前的ID或者无效，或者已经被删除！");
                    return;
                }

                this.ucAttachment.UCTemplateName = l_BusReceiveEdit.ProcessName;

                PopulateField(l_BusReceiveEdit);

                if (String.IsNullOrEmpty(l_BusReceiveEdit.ProcessID))
                {
                    divPrompt.Visible = true;
                    return;
                }
                l_objProcessInstance = FS.OA.Framework.WorkFlow.WFFactory.GetWF(FS.OA.Framework.WorkFlow.WFType.AgilePoint).GetAPI().GetProcInst(l_BusReceiveEdit.ProcessID);

                if (l_objProcessInstance != null)
                {
                    m_strProcessTemplate = l_objProcessInstance.DefName;

                    String l_strCirculateTableName = FS.ADIM.OA.BLL.Common.TableName.GetCirculateTableName(l_objProcessInstance.DefName);
                    l_BusCirculate = new B_Circulate(l_strCirculateTableName);
                    l_dtbCirculate = l_BusCirculate.GetCirculatesByID(l_strCirculateTableName, l_BusReceiveEdit.ProcessID, 0);
                    gdvCirculate.DataSource = l_dtbCirculate;
                    gdvCirculate.DataBind();

                    l_objWorkItem = new B_LetterReceive();

                    l_dtbProcessSteps = l_objWorkItem.GetStepsByProcessID(l_BusReceiveEdit.ProcessID, TableName.WorkItemsTableName.T_OA_HS_WorkItems + ((l_objProcessInstance.Status == ProcessConstString.ProcessStatus.STATUS_COMPLETED || l_objProcessInstance.Status == ProcessConstString.ProcessStatus.STATUS_CANCELED) ? "_BAK" : ""));

                    DataView l_dtvDataView = new DataView(l_dtbProcessSteps);
                    l_dtvDataView.RowFilter = "D_StepStatus = 'Completed'";

                    rptProcessDetail.DataSource = l_dtvDataView;
                    rptProcessDetail.DataBind();
                }
            }
        }

        /// <summary>
        /// 用实体中的内容填充表单上编辑区域的内容
        /// </summary>
        /// <param name="p_BusReceiveEdit"></param>
        private void PopulateField(B_HSEdit p_BusReceiveEdit)
        {
            //单位
            txtReceiveUnit.Text = p_BusReceiveEdit.CommunicationUnit;

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
            txtXingWenDate.Text = p_BusReceiveEdit.XingWenDate != DateTime.MinValue ? p_BusReceiveEdit.XingWenDate.ToString("yyyy-MM-dd") : "";

            //文件标题
            txtDocumentTitle.Text = p_BusReceiveEdit.DocumentTitle;

            //函件类型
            txtDocumentType.Text = OAList.GetHJName(p_BusReceiveEdit.LetterType);//任金权修改2009115 只显示编号不能显示名称

            //形成日期
            txtFormationDate.Text = p_BusReceiveEdit.FormationDate != DateTime.MinValue ? p_BusReceiveEdit.FormationDate.ToString("yyyy-MM-dd") : "";

            //紧急程度
            txtUrgentDegree.Text = p_BusReceiveEdit.UrgentDegree;

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
            txtKeepTime.Text = p_BusReceiveEdit.KeepTime;

            //对应合同号
            txtContractNumber.Text = p_BusReceiveEdit.ContractNumber;

            //设备代码
            txtEquipmentCode.Text = p_BusReceiveEdit.EquipmentCode;

            //HN编码
            txtHNCode.Text = p_BusReceiveEdit.HNCode;

            //备注
            txtRemark.Text = p_BusReceiveEdit.Remarks;

            //附件信息
            this.ucAttachment.UCDataList = XmlUtility.DeSerializeXml<List<CFuJian>>(p_BusReceiveEdit.FileData);
        }

        protected void rptProcessDetail_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            DataRowView l_drvRowItem = null;
            M_LetterReceive l_objWorkItem = null;
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
          

                UC_Step l_objStep = (UC_Step)e.Item.FindControl("ucStep");
                l_drvRowItem = e.Item.DataItem as DataRowView;
      l_objWorkItem = XmlUtility.DeSerializeXml<B_LetterReceive>(l_drvRowItem["FormsData"].ToString());
                l_objStep.Participant = l_drvRowItem["ReceiveUserID"].ToString() + "(" + l_drvRowItem["ReceiveUserName"].ToString() + ")";
                l_objStep.m_strStepName = l_drvRowItem["StepName"].ToString();
                l_objStep.SubmitDataTime = l_drvRowItem["SubmitDate"].ToString();
                l_objStep.SubmitAction = l_drvRowItem["SubmitAction"].ToString();
                l_objStep.IsNoPrompt();
                switch (l_objStep.m_strStepName)
                {
                    //步骤名称-承办
                    case ProcessConstString.StepName.LetterReceiveStepName.STEP_CHECK:
                        l_objStep.Comment =l_objWorkItem.NiBanComment;
                        break;
                    //步骤名称-领导批示
                    case ProcessConstString.StepName.LetterReceiveStepName.STEP_INSTRUCTION:
                        l_objStep.Comment = l_objWorkItem.LS_Comment;
                        break;
                    //步骤名称-政工办批阅
                    case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_DIRECTOR:
                    //步骤名称-团委书记批阅
                    case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_CHIEF:
                    //步骤名称-工会副主席拟办
                    case ProcessConstString.StepName.LetterReceiveStepName.STEP_SECTION_MEMBER:
                        l_objStep.Comment = l_objWorkItem.UnderTake_Comment;
                        break;
                    default:
                        break;
                }

                //附件信息
                if (l_objWorkItem.FileList.Count > 0)
                {
                    l_objStep.Attachment.UCIsEditable = false;
                    ucAttachment.UCWorkItemID = l_objWorkItem.WorkItemID;
                    //ucAttachment.UCTBID = l_objWorkItem.;
                    l_objStep.Attachment.UCProcessID = l_drvRowItem["ProcessID"].ToString();
                    l_objStep.Attachment.UCTemplateName = m_strProcessTemplate;
                    l_objStep.Attachment.UCDataList = l_objWorkItem.FileList;
                }
                else
                {
                    l_objStep.HiddenAttach();
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Receive.Register.PG_HSRegister&ID=" + Request.QueryString[ConstString.QueryString.REGISTER_ID]);
        }
    }
}
