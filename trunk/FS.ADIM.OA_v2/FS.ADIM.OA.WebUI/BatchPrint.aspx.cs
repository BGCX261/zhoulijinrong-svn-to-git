using System;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.WebUI.WorkFlow.LetterSend;
using FS.ADIM.OA.WebUI.WorkFlow.ProgramFile;
using FS.ADIM.OA.WebUI.WorkFlow.Receive;
using FS.ADIM.OA.WebUI.WorkFlow.RequestReport;
using FS.ADIM.OA.WebUI.WorkFlow.Send;
using FS.ADIM.OA.WebUI.WorkFlow.WorkRelation;

namespace FS.ADIM.OA.WebUI
{
    public partial class BatchPrint : System.Web.UI.Page
    {
        string[] GF_Step = new string[] { "拟稿", "审稿", "部门会签", "核稿", "主任核稿", "领导会签", "签发", "校对", "分发", "传阅" }; // 公司发文
        string[] GS_Step = new string[] { "发起流程", "办公室批阅", "领导批示", "收文处理中心", "收文处理中心归档", "处室承办", "科室承办", "人员承办", "传阅" }; // 公司收文
        string[] HF_Step = new string[] { "发起函件", "核稿", "会签", "签发", "函件分发", "传阅", "二次分发" }; // 函件发文
        string[] HS_Step = new string[] { "发起流程", "拟办", "领导批示", "二次拟办", "处室承办", "科室承办", "人员承办", "协办", "传阅", "函件管理员处理" }; // 函件收文
        string[] RR_Step = new string[] { "拟稿", "核稿", "签发", "秘书审核", "主任审核", "领导批示", "秘书分发", "处室承办", "科室承办", "人员承办", "承办审核", "传阅" }; // 请示报告
        string[] WR_Step = new string[] { "拟稿", "核稿", "签发", "传阅", "处室承办", "科室承办", "人员承办" }; // 工作联系单
        string[] PF_Step = new string[] { "编制", "校核", "审核", "质保审查", "部门会签", "领导会签", "批准", "分发", "传阅" }; // 程序文件
        //string[] MF_Step = new string[] { "拟稿", "审稿", "部门会签", "核稿", "领导会签", "签发", "分发", "校对" }; // 党政工团发文
        //string[] MS_Step = new string[] { "发起流程", "拟办", "收文处理中心", "领导批示", "收文处理中心归档", "传阅", "处室承办", "科室承办", "人员承办", "追加分发"}; // 党政工团收文

        protected void Page_Load(object sender, EventArgs e)
        {
            FormsUIBase ui;
            switch(ddlProc.Text)
            {
                case "公司发文":
                    ui = new UC_CompanySend();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                case "公司收文":
                    ui = new UC_CompanyReceive();//new B_GS_WorkItems();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                case "函件发文":
                    ui = new UC_LetterSend();//new EntityLetterSend();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                case "函件收文":
                    ui = new UC_LetterReceived();//new B_LetterReceive();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                case "请示报告":
                    ui = new UC_RequestReport();//new B_RequestReport();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                case "工作联系单":
                    ui = new UC_WorkRelation();//new B_WorkRelation();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                case "程序文件":
                    ui = new UC2_ProgramFile();//new B_PF();
                    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                    break;
                //case "党政工团发文":
                //    ui = new UC_Send();//new M_DJGTSend();
                //    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                //    break;
                //case "党政工团收文":
                //    ui = new UC_Receive();//new M_MergeReceiveBase();
                //    ui.InitPrint(this.UC_Print1, ddlProc.Text, ddlStep.Text, CaleStart.Text, CaleEnd.Text);
                //    break;
            }

            List<EntityBase> ls = B_FormsData.GetEntities(ddlProc.Text,
                                                            ddlStep.Text,
                                                            DateTime.Parse(string.IsNullOrEmpty(CaleStart.Text) ? DateTime.MinValue.ToString() : CaleStart.Text),
                                                            DateTime.Parse(string.IsNullOrEmpty(CaleEnd.Text) ? DateTime.MinValue.ToString() : CaleEnd.Text),
                                                            true);
            this.TextBox1.Text = "当前满足条件的记录为:" + ls.Count.ToString();
        }

        protected void CaleEnd_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void InitListItems(string[]  str)
        {
            ddlStep.Items.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                ddlStep.Items.Add(str.GetValue(i) as string);
            }
        }

        protected void ddlProc_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlProc.Text)
            {
                case "公司发文":
                    InitListItems(GF_Step);
                    break;
                case "公司收文":
                    InitListItems(GS_Step);
                    break;
                case "函件发文":
                    InitListItems(HF_Step);
                    break;
                case "函件收文":
                    InitListItems(HS_Step);
                    break;
                case "请示报告":
                    InitListItems(RR_Step);
                    break;
                case "工作联系单":
                    InitListItems(WR_Step);
                    break;
                case "程序文件":
                    InitListItems(PF_Step);
                    break;
                //case "党政工团发文":
                //    InitListItems(MF_Step);
                //    break;
                //case "党政工团收文":
                //    InitListItems(MS_Step);
                //    break;
            }
        }
    }
}
