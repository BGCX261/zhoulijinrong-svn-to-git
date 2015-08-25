using System;
using System.Collections.Generic;
using System.Web;
using FounderSoftware.ADIM.OA.OA2DC;
using FounderSoftware.ADIM.OA.OA2DP;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.WebUI.UIBase;
using System.Data;
using System.Text;
using System.Xml;
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI
{
    public partial class BatchDevolve : System.Web.UI.Page
    {
        //List<EntityBase> ls = null;
        private StringBuilder sbScript = new StringBuilder();
        
        string[] GF_Step = new string[] { "拟稿", "审稿", "部门会签", "核稿", "主任核稿", "领导会签", "签发", "校对", "分发", "传阅" }; // 公司发文
        string[] GS_Step = new string[] { "发起流程", "办公室批阅", "领导批示", "收文处理中心", "收文处理中心归档", "处室承办", "科室承办", "人员承办", "传阅" }; // 公司收文
        string[] HF_Step = new string[] { "发起函件", "核稿", "会签", "签发", "函件分发", "传阅", "二次分发" }; // 函件发文
        string[] HS_Step = new string[] { "发起流程", "拟办", "领导批示", "二次拟办", "处室承办", "科室承办", "人员承办", "协办", "传阅", "函件管理员处理" }; // 函件收文
        string[] RR_Step = new string[] { "拟稿", "核稿", "签发", "秘书审核", "主任审核", "领导批示", "秘书分发", "处室承办", "科室承办", "人员承办", "承办审核", "传阅" }; // 请示报告
        string[] WR_Step = new string[] { "拟稿", "核稿", "签发", "传阅", "处室承办", "科室承办", "人员承办" }; // 工作联系单
        string[] PF_Step = new string[] { "编制", "校核", "审核", "质保审查", "部门会签", "领导会签", "批准", "分发", "传阅" }; // 程序文件
        //string[] MF_Step = new string[] { "拟稿", "审稿", "部门会签", "核稿", "领导会签", "签发", "分发", "校对" }; // 党政工团发文
        //string[] MS_Step = new string[] { "发起流程", "拟办", "收文处理中心", "领导批示", "收文处理中心归档", "传阅", "处室承办", "科室承办", "人员承办", "追加分发"}; // 党政工团收文

        private static String m_strDevolveRoleConfig = @"Config\DevolveRole.xml";

        private void InitProcStep()
        {
            //XmlDocument doc = new XmlDocument();
            //String l_strBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //doc.LoadXml(l_strBaseDirectory + m_strDevolveRoleConfig);
            //doc.
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 待注册脚本
            string sScript = @"<script type='text/javascript' language='javascript'>
                                    var objReq;
                                    var result;
                                    
                                    function SendRequest()
                                    {
                                        objReq = Init();
                                        
                                        var dtStart = window.document.getElementById('CaleStart').value;
                                        var dtEnd = window.document.getElementById('CaleEnd').value;
                                        var sProc = window.document.getElementById('ddlProc').value;
                                        var sStep = window.document.getElementById('ddlStep').value;
                                        var arr = new Array();
                                        var i = 0;
                                        if(window.document.getElementById('FSGridView1') == null) {
                                            window.form1.Text1.value = '无有效归档记录,请重新筛选!';
                                            return;
                                          }
                                        
                                          window.form1.Text1.value = '正在批量归档,请稍候...';
                                          window.form1.Button1.disabled = true;
                                        while(i < window.document.getElementById('FSGridView1').cells.length)
                                        {
                                            if(window.document.getElementById('FSGridView1').cells[i].nodeName == 'TD')
                                            {
                                                if(window.document.getElementById('FSGridView1').cells[i].childNodes[0].checked == true)
                                                {
                                                    arr.push(window.document.getElementById('FSGridView1').cells[i+2].innerText);
                                                }
                                            }
                                            i = i+6;
                                        }
                                        
                                        var content = 'dtStart=' + dtStart + '&' 
                                                    + 'dtEnd=' + dtEnd + '&' 
                                                    + 'sProc=' + encodeURIComponent(sProc) + '&'
                                                    + 'sStep=' + encodeURIComponent(sStep) + '&'
                                                    + 'arr=' + arr;
                                                    
                                        objReq.open('POST', 'DevolveHandler.ashx', true);
                                        objReq.setRequestHeader('Content-Length',content.length);
                                        objReq.setRequestHeader('CONTENT-TYPE','application/x-www-form-urlencoded');
                                        
                                        objReq.onreadystatechange = processRequest;
                                        objReq.send(content);
                                    }
                                    
                                    function Init()
                                    {
                                        if(window.XMLHttpRequest)
                                            return new XMLHttpRequest();
                                        else
                                        {
                                            if(window.ActiveXObject)
                                                return new ActiveXObject('Microsfot.XMLHTTP');
                                        }
                                    }
                                    function processRequest()
                                    {
                                        if(objReq.readyState==4) 
                                        {
                                            if(objReq.status==200) 
                                            {
　                                              processResponse();
                                            }
                                        }
                                    }
                                    function processResponse()
                                    {
                                        result = objReq.responseText;
                                        objReq.abort();
                                        window.form1.Text1.value = result;
                                        window.form1.Button1.disabled = false;
                                    }
                                    </script>";
            #endregion
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "", sScript);
        }

        private void InitListItems(string[] str)
        {
            ddlStep.Items.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                ddlStep.Items.Add(str.GetValue(i) as string);
            }
        }

        protected void ddlProc_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlStep.Items.Clear();
            DataTable dt = OAConfig.GetDevolveRoleConfig(ddlProc.Text/*this.ddlProc.SelectedItem.Text*/);
            if (dt == null) return;
            for (int i = 0; i < dt.Rows.Count; i++)
                this.ddlStep.Items.Add(dt.Rows[i]["StepName"].ToString());
            /*
            switch (ddlProc.Text)
            {
                case "公司发文":
                    InitListItems(GF_Step);
                    break;
                case "公司收文":
                    InitListItems(GS_Step);
                    break;
                case "新版函件发文":
                    InitListItems(HF_Step);
                    break;
                case "新版函件收文":
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
            }*/
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            List<EntityBase> ls = B_FormsData.GetEntities(ddlProc.Text,
                                                            ddlStep.Text,
                                                            DateTime.Parse(string.IsNullOrEmpty(CaleStart.Text) ? DateTime.MinValue.ToString() : CaleStart.Text),
                                                            DateTime.Parse(string.IsNullOrEmpty(CaleEnd.Text) ? DateTime.MinValue.ToString() : CaleEnd.Text),
                                                            true);
            //ls[0].ID INT
            //ls[0].ProcessID STR
            //ls[0].WorkItemID STR
            DataTable dt = new DataTable();
            dt.Columns.Add("IsDevolved");
            dt.Columns.Add("ID");
            dt.Columns.Add("Title");
            dt.Columns.Add("ProcessID");
            dt.Columns.Add("WorkItemID");

            for (int i = 0; i < ls.Count; i++)
            {
                DataRow row = dt.NewRow();
                if (B_ProcessInstance.Is_Devolve(ls[i].ProcessID, ddlProc.Text).ToString().ToUpper() == "TRUE")
                {
                    row["IsDevolved"] = "已归档";
                }
                else
                {
                    row["IsDevolved"] = "未归档";
                }
                row["ID"] = ls[i].ID.ToString();
                row["Title"] = ls[i].DocumentTitle;
                row["ProcessID"] = ls[i].ProcessID;
                row["WorkItemID"] = ls[i].WorkItemID;
                dt.Rows.Add(row);
            }

            this.FSGridView1.DataSource = dt;
            this.FSGridView1.DataBind();
        }
    }
}
