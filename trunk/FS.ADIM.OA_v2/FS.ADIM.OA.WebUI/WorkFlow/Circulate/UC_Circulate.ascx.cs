using System;
using System.Web.UI;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.WebUI.UIBase;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.WorkFlow.Circulate
{
    public partial class UC_Circulate : FormsUIBase
    {
        protected Boolean IsGoOnCirculate
        {
            get
            {
                if (ViewState["GoOnCirculate"] == null)
                {
                    return false;
                }
                return (Boolean)ViewState["GoOnCirculate"];
            }
            set
            {
                ViewState["GoOnCirculate"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String l_strCirculateTime = null;

            //加载流程表单
            LoadProcessForm();

            if (!IsPostBack)
            {
                this.ucCirculateList.UCProcessID = base.ProcessID;
                this.ucCirculateList.UCTemplateName = base.TemplateName;
                this.ucCirculateList.SetButtonVisible();

                this.ucGoOnCirculate.UCProcessID = base.ProcessID;
                this.ucGoOnCirculate.UCProcessType = base.TemplateName;
                this.ucGoOnCirculate.UCWorkItemID = base.WorkItemID;
                this.ucGoOnCirculate.UCShowComment = true;//显示传阅意见
                this.ucGoOnCirculate.LoadComment(); //加载传阅意见

                //公司收文传阅超过30天，意见框不显示
                if (base.TemplateName == ProcessConstString.TemplateName.COMPANY_RECEIVE)
                {
                    l_strCirculateTime = Request.QueryString["RDT"];
                    if (!String.IsNullOrEmpty(l_strCirculateTime))
                    {
                        DateTime l_datCirculateTime = DateTime.Parse(l_strCirculateTime);
                        String l_strValidDays = OAConfig.GetConfig("传阅有效期", "天数");
                        int l_intDefaultDays = 7;
                        if (!String.IsNullOrEmpty(l_strValidDays))
                        {
                            l_intDefaultDays = SysConvert.ToInt32(l_strValidDays);
                        }
                        DateTime l_datMergeTimes = l_datCirculateTime.AddDays(l_intDefaultDays);
                        ucGoOnCirculate.IsVisible = l_datMergeTimes >= System.DateTime.Now ? true : false;
                    }
                }

                if (Request.QueryString["IsRead"] == "True")
                {
                    this.btnRead.Visible = false;
                    //this.ucGoOnCirculate.UCIsRead = true;
                }

                ucGoOnCirculate.InitLoad();
                this.btnGoOnCirculate.Visible = ucGoOnCirculate.UCIsGoOnCirculate;
            }
        }

        private void LoadProcessForm()
        {
            UserControl l_objUserControl = null;

            switch (base.TemplateName)
            {
                //党纪工团发文
                case ProcessConstString.TemplateName.DJGT_Send:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\Send\\UC_Send.ascx");
                    break;
                //党纪工团收文
                case ProcessConstString.TemplateName.MERGED_RECEIVE:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\Receive\\UC_Receive.ascx");
                    break;
                //公司收文
                case ProcessConstString.TemplateName.COMPANY_RECEIVE:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\Receive\\UC_CompanyReceive.ascx");
                    break;
                //请示报告
                case ProcessConstString.TemplateName.INSTUCTION_REPORT:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\RequestReport\\UC_RequestReport.ascx");
                    break;
                //程序文件
                case ProcessConstString.TemplateName.PROGRAM_FILE:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\ProgramFile\\UC2_ProgramFile.ascx");
                    break;
                //工作联系单
                case ProcessConstString.TemplateName.AFFILIATION:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\WorkRelation\\UC_WorkRelation.ascx");
                    break;
                //公司发文
                case ProcessConstString.TemplateName.COMPANY_SEND:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\Send\\UC_CompanySend.ascx");
                    break;
                //新版函件发文
                case ProcessConstString.TemplateName.LETTER_SEND:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\LetterSend\\UC_LetterSend.ascx");
                    break;
                //新版函件收文
                case ProcessConstString.TemplateName.LETTER_RECEIVE:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\Receive\\UC_LetterReceived.ascx");
                    break;
                //借款申请
                case ProcessConstString.TemplateName.FinanceJK_APPLY:
                    l_objUserControl = (UserControl)base.LoadControl("..\\..\\WorkFlow\\Finance\\UC_FinanceJK.ascx");
                    break;
                default:
                    break;
            }
            if (l_objUserControl != null)
            {
                this.phlContent.Controls.Add(l_objUserControl);
            }
        }

        private void DoCirculate()
        {
            String sSuccesName = "";
            String sFailedName = "";
            String l_strMessage = String.Empty;
            if (!ucGoOnCirculate.CheckBeforeCirculate(ref l_strMessage))
            {
                base.ShowMsgBox(this.Page, MsgType.VbExclamation, l_strMessage);
                return;
            }
            ucGoOnCirculate.DoCirculate(ref sSuccesName, ref sFailedName);
            l_strMessage = String.Format("{0}", sSuccesName);
            base.ShowMsgBox(this.Page, MsgType.VbInformation, l_strMessage, base.EntryAction);
        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString[ConstString.QueryString.CIRCULATE_ID]))
            {
                if (IsGoOnCirculate)
                {
                    DoCirculate();
                }
                String cyTB = TableName.GetCirculateTableName(base.TemplateName);
                B_Circulate l_burCirculate = new B_Circulate(cyTB);

                l_burCirculate.ID = SysConvert.ToInt32(Request.QueryString[ConstString.QueryString.CIRCULATE_ID]);
                l_burCirculate.Comment = this.ucGoOnCirculate.UCComment;
                l_burCirculate.Is_Read = true;

                Boolean l_blnIsSuccess = l_burCirculate.Save();
                if (!l_blnIsSuccess)
                {
                    JScript.Alert(SysString.GetErrMsgs(l_burCirculate.ErrMsgs));
                    return;
                }

                btnRead.Visible = false;

                base.ShowMsgBox(this.Page, MsgType.VbInformation, "处理成功", "3");
            }
        }

        protected void btnGoOnCirculate_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["IsRead"] != "True")
            {
                String cyTB = TableName.GetCirculateTableName(base.TemplateName);
                B_Circulate l_burCirculate = new B_Circulate(cyTB);

                l_burCirculate.ID = SysConvert.ToInt32(Request.QueryString[ConstString.QueryString.CIRCULATE_ID]);
                l_burCirculate.Comment = this.ucGoOnCirculate.UCComment;
                l_burCirculate.Is_Read = true;

                Boolean l_blnIsSuccess = l_burCirculate.Save();
                if (!l_blnIsSuccess)
                {
                    JScript.Alert(SysString.GetErrMsgs(l_burCirculate.ErrMsgs));
                    return;
                }
            }
            DoCirculate();
        }
    }
}