using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Ascentn.Workflow.Base;
using Ascentn.Workflow.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using FS.OA.Framework;
using FS.ADIM.OU.OutBLL;
using FS.OA.Framework.WorkFlow;

namespace FS.ADIM.OA.WebUI.AgilePoint
{
    /// <summary>
    /// ProcessViewerService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ProcessViewerService : System.Web.Services.WebService
    {
        public DataTable dtALLUser = new DataTable(); //全部用户信息

        public ProcessViewerService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        /// <summary>
        /// For given process instance id, it returns all status of activity
        /// </summary>
        /// <param name="piID">process instance id</param>
        /// <returns>array of KeyValue pairs, key represents activity id, value represents status as string</returns>
        [WebMethod(EnableSession = true)]
        public KeyValue[] GetActivityInstStatus(String piID)
        {
            KeyValue[] ret = null;
            try
            {
                WorkflowService api = WFFactory.GetWF(WFType.AgilePoint).GetAPI();
                if (api != null)
                {
                    ret = api.GetActivityInstStatus(piID);
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        [WebMethod(EnableSession = true)]
        public string GetActivityInstInfo(string piID, string aiID)
        {
            string html = "";
            try
            {
                WorkflowService api = WFFactory.GetWF(WFType.AgilePoint).GetAPI();
                WFBaseProcessInstance pi = api.GetProcInst(piID);
                WFManualWorkItem[] mwks = GetManualWorkItems(api, aiID);
                WFAutomaticWorkItem[] awks = null;
                if (mwks == null || mwks.Length == 0)
                {
                    awks = GetAutomaticWorkItems(api, aiID);
                }
                html = RenderActivityContent(api, pi, aiID, mwks, awks);
            }
            catch (Exception)
            {
            }

            return html;
        }

        private WFManualWorkItem[] GetManualWorkItems(WorkflowService api, string aiID)
        {
            // manual work items
            WFAny any = WFAny.Create(aiID);
            WFQueryExpr expr = new WFQueryExpr("ACTIVITY_INST_ID", SQLExpr.EQ, any, true);

            WFManualWorkItem[] wks = api.QueryWorkList(expr);
            if (wks == null || wks.Length == 0) return null;

            SortedList sl = new SortedList();
            foreach (WFManualWorkItem wk in wks)
            {
                string key = (DateTime.MaxValue - wk.AssignedDate).ToString();
                sl.Add(key + UUID.GetID(), wk);
            }
            sl.Values.CopyTo(wks, 0);
            return wks;
        }

        private WFAutomaticWorkItem[] GetAutomaticWorkItems(WorkflowService api, string aiID)
        {
            // manual work items
            WFAny any = new WFAny();
            any.Type = WFTypeCode._STRING;
            any.Value = aiID;

            WFQueryExpr expr = new WFQueryExpr();
            expr.ColumnName = "ACTIVITY_INST_ID";
            expr.Operator = SQLExpr.EQ;
            expr.Any = any;
            expr.IsValue = true;

            WFAutomaticWorkItem[] wks = api.QueryProcedureList(expr);
            if (wks == null || wks.Length == 0) return null;

            SortedList sl = new SortedList();
            string procedure = null;
            string prefix = WFConstants.BUILT_IN_PROCEDURE;
            foreach (WFAutomaticWorkItem wk in wks)
            {
                procedure = wk.ProcedureInfo;
                if (!procedure.StartsWith(prefix)) continue;
                procedure = procedure.Remove(0, prefix.Length + 1);
                if (!procedure.StartsWith("SubProcess")) continue;

                sl.Add((int.MaxValue - wk.Session) + UUID.GetID(), wk);
            }
            wks = new WFAutomaticWorkItem[sl.Count];
            if (sl.Count > 0) sl.Values.CopyTo(wks, 0);
            return wks;
        }

        private String RenderActivityContent(WorkflowService api, WFBaseProcessInstance pi, string id, WFManualWorkItem[] mwks, WFAutomaticWorkItem[] awks)
        {
            string header =
                //				"<DIV style=\"DISPLAY:none;OVERFLOW: auto\">\n"
                "<TABLE id='popupTable' align=center border=1 bordercolor=lightgrey>\n"
                + "<TR><td>\n"
                + "<TABLE style=\"background-color: #ffffcc;font-family: 宋体,Verdana,sans-serif;font-size: 12;Z-INDEX:106\" cellSpacing=\"0\" cellPadding=\"2\" border=\"0\">\n";
            string footer = "</TABLE>\n" + "</TD></TR>\n" + "</TABLE>\n";
            string content = "";

            WFBaseActivityInstance ai = api.GetActivityInst(id);
            //Retreive process definition and Activity definition
            string xmlString = api.GetProcDefXml(pi.DefID);
            WFProcessDefinition processDef = new WFProcessDefinition();
            ProcDefXmlParser xmlParser = new ProcDefXmlParser(processDef);
            xmlParser.Parse(xmlString);
            IWFActivityDefinition ad = processDef.FindActivityByName(ai.Name);
            WFManualActivityDefinition activityDef = null;
            if (ad != null && ad.GetType() == typeof(WFManualActivityDefinition))
            {
                activityDef = (WFManualActivityDefinition)ad;
            }
            bool isSub = false; //zhouli
            ArrayList arrSub = new ArrayList();
            ArrayList arrSubName = new ArrayList();
            if (ai.Session > 0)
            {
                string status = ConvUtil.GetDisplayStatus(pi.Status, ai.Session, ai.TokenPos.Value, ai.Pending, ai.InStack);

                //zhouli
                string sql = string.Format(@"SELECT Proc_Inst_ID,Proc_Inst_Name FROM WF_PROC_INSTS WHERE SUPER_PROC_INST_ID='{0}'", pi.ProcInstID);
                DataTable dtSub = SQLHelper.GetDataTable2(sql);
                if (dtSub.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSub.Rows)
                    {
                        arrSub.Add(dr["Proc_Inst_ID"].ToString());
                        arrSubName.Add(dr["Proc_Inst_Name"].ToString());
                    }
                }

                status = GetCNStatus(status);
                content += ConstructRow(status, "");
                content += ConstructRow(null, null);
                if (mwks == null || mwks.Length == 0)
                {
                    content += ConstructRow("开始日期：", DTFormat(ai.StartedDate));
                    if (ai.CompletedDate.Ticks > 0)
                    {
                        content += ConstructRow("完成日期：", DTFormat(ai.CompletedDate));
                    }
                    if (awks != null)
                    {
                        Hashtable names = GetSubProcInstName(api, awks);
                        string url = null;
                        string procInstName = null;
                        foreach (WFAutomaticWorkItem wk in awks)
                        {
                            procInstName = (string)names[wk.WorkItemID];
                            url = string.Format("<a href=# onclick=\"parent.showSubProcess('{0}')\">Sub Process '{1}'</a>", wk.WorkItemID, procInstName);
                            content += ConstructRow(string.Format("#{0}", wk.Session), url);
                        }
                    }
                }
                //Show Mannual activity information
                else
                {
                    ArrayList participantList = new ArrayList();
                    ArrayList participantStatus = new ArrayList();
                    //Create a list of participant involved in this activity
                    for (int index = mwks.Length - 1; index >= 0; index--)
                    {
                        if (mwks[index].Status == WFManualWorkItem.ASSIGNED || mwks[index].Status == WFManualWorkItem.OVERDUE || mwks[index].Status == WFManualWorkItem.COMPLETED)
                        {
                            if (!String.IsNullOrEmpty(mwks[index].UserID))
                            {
                                participantList.Add(mwks[index].UserID);
                                participantStatus.Add(mwks[index].Status);
                            }
                        }
                    }


                    //Show type of activity and participant info if it AgileWork of type process adaptation
                    if (activityDef != null && activityDef.CustomProperties.Contains("Ascentn.AgileWork.Premier.ProcessAdaptation"))
                    {
                        //Get type of the AgileWork
                        string activityType = api.GetCustomAttr(pi.WorkObjectID, id + "_ApprovalType") as string;
                        //Show type of the activity
                        content += ConstructRow(null, null);
                        content += ConstructRow("Activity Type:", activityType);

                        //Add those participant who have not been assgined yet(in case of sequential type) to participantList
                        if (activityType == "Sequential")
                        {
                            string activityProperties = api.GetCustomAttr(pi.WorkObjectID, id + "_ActivityProperties") as string;
                            if (!String.IsNullOrEmpty(activityProperties))
                            {
                                string[] approverInfoList = activityProperties.Split(';');
                                //If number of Approver is more than one, only first approver is get assigned
                                //So add the rest approver(who have not been assigned)to the Participant List
                                if (approverInfoList.Length > 1)
                                {
                                    for (int i = 1; i < approverInfoList.Length; i++)
                                    {
                                        string[] userInfoList = approverInfoList[i].Split('|');
                                        string user = userInfoList[0];
                                        if (!String.IsNullOrEmpty(user))
                                        {
                                            participantList.Add(user);
                                            participantStatus.Add("In Queue");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #region 显示人物列表
                    //Show participant list  for the activity
                    if (participantList.Count > 0)
                    {
                        content += ConstructRow("参与该任务的用户", null);

                        for (int i = 0; i < participantList.Count; i++)
                        {
                            if (participantList[i].ToString().ToLower() != @"dev003\dummy")//zhouli
                                content += ConstructRow(participantList[i] + ":", participantStatus[i]);
                        }
                    }
                    foreach (WFManualWorkItem wk in mwks)
                    {
                        if (content.Length > 0)
                            content += ConstructRow(null, null);

                        if (wk.UserID.ToLower() == @"dev003\dummy") //zhouli
                        {
                            for (int sub = 0; sub < arrSub.Count; sub++)
                            {
                                //content += ConstructRow("子流程", arrSub[sub].ToString());
                                string url = string.Format("<a href=# onclick=\"parent.showSubProcess('{0}')\">{1}</a>", arrSub[sub].ToString(), arrSubName[sub].ToString());
                                content += ConstructRow("子流程", url);
                            }
                        }
                        else
                        {
                            content += ConstructRow("会话：", wk.Session);
                            content += ConstructRow("任务：", wk.Name);
                            content += ConstructRow("任务处理人：", GetUserName(wk.UserID)); //zhouli
                            content += ConstructRow("任务接收日期：", DTFormat(wk.AssignedDate));
                            //content += ConstructRow("过期日期：", DTFormat(wk.DueDate));
                            if (wk.CompletedDate.Ticks > 0)
                            {
                                content += ConstructRow("完成日期：", DTFormat(wk.CompletedDate));
                            }
                            content += ConstructRow("状态：", wk.Status);
                        }
                    }
                    #endregion
                }

                //Add custom attributes changes during this activity into content
                content += AddCustomAttribToContent(pi.ProcInstID, ai.StartedDate.ToString("M/d/yyyy H:mm:ss:fff tt"), ai.CompletedDate.ToString("M/d/yyyy H:mm:ss:fff tt"));

                //Show Process Adaptation Link only when AgileWork is of type Ascentn.AgileWork.Premier.ProcessAdaptation and not completed or cancled
                if (status != WFBaseActivityInstance.PASSED && status != WFBaseActivityInstance.CANCELLED
                    && activityDef != null && activityDef.CustomProperties.Contains("Ascentn.AgileWork.Premier.ProcessAdaptation"))
                {
                    // Get Process Adaptation Url from web.config
                    string processAdaptationUrl = (String)ConfigurationManager.AppSettings["ProcessAdaptationUrl"];
                    string queryString = "?ProcessTemplate=" + pi.DefName + "&ProcessInstance=" + pi.ProcInstID + "&ActiveInstance=" + id;
                    string processAdptationLink = string.Format("<a href=# onclick=\"parent.showProcessAdaptation('{0}')\">Open Process Adaptation</a>", processAdaptationUrl + queryString);
                    //Add process adaptation link to the content
                    content += ConstructRow(null, null);
                    content += ConstructRow(processAdptationLink, null);
                }
            }
            else
            {
                content += ConstructRow("信息：", "此步骤还未流转到！");
            }
            return header + content + footer;
        }


        private string ConstructRow(string label, object text)
        {
            string html = null;

            if (label == null && text == null)
            {
                html = "<TR height=1x bgcolor=black><TD colspan=2></TD></TR>\n";
            }
            else
            {
                if (label.Contains(@"\"))//zhouli
                {
                    label = GetUserName(label);
                }
                if (text == null)
                {
                    html = string.Format("<TR><TD colspan=2 nowrap valign=top align=center>{0}</TD></TR>\n", label);
                }
                else
                {
                    html = string.Format("<TR><TD nowrap valign=top align=right>{0}</TD><TD nowrap>{1}</TD></TR>\n", label, GetCNStatus(text.ToString())); //zhouli
                }
            }
            return html;
        }

        string AddCustomAttribToContent(string procInstID, string startedDate, string completedDate)
        {
            SqlConnection sqlCon = null;

            //Retrieve custom attribute information  from DataTracking table 
            try
            {
                //Get Data Tracking Connection String from Web.config
                //string connectionString = ConfigurationSettings.AppSettings["DataTrackingConnectionString"];

                string connectionString = ConfigurationManager.ConnectionStrings["DataTrackingConnectionString"].ToString();
                sqlCon = new SqlConnection(connectionString);
                string sqlCommand = "SELECT NAME,NEW_VALUE FROM WF_DATA_TRACKING WHERE PROC_INST_ID=" + "\'" + procInstID + "\'" + " AND CHANGED_AT between " + "\'" + startedDate + "\'" + " and " + "\'" + completedDate + "\'" + " order by NAME ";

                SqlDataAdapter sqlAdpt = new SqlDataAdapter(sqlCommand, sqlCon);
                DataSet ds = new DataSet();
                sqlAdpt.Fill(ds);
                string content = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    content += ConstructRow(null, null);
                    content += ConstructRow("Data changed during this acivity", null);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        content += ConstructRow(row[0].ToString() + ":", row[1].ToString());
                    }
                }

                return content;
            }
            catch (Exception ex)
            {
                //Write error into Logger
                Logger.WriteLine("Error while retrieving DataTracking for Process Viewer, Error Message: " + ex.Message);
                return null;
            }

            finally
            {
                if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }

            }
        }

        private string DTFormat(DateTime t)
        {
            if (t.Ticks == 0) return "";
            return ShUtil.FnDateTime(t);
        }

        private Hashtable GetSubProcInstName(WorkflowService api, WFAutomaticWorkItem[] wks)
        {
            Hashtable h = new Hashtable();
            if (wks == null || wks.Length == 0) return h;

            Hashtable hPIIDs = new Hashtable(); // ensure unique
            String piIDs = "";
            foreach (WFAutomaticWorkItem w in wks)
            {
                if (hPIIDs[w.WorkItemID] == null)
                {
                    if (piIDs.Length > 0) piIDs += ",";
                    piIDs += String.Format("'{0}'", w.WorkItemID);
                    hPIIDs[w.WorkItemID] = w.WorkItemID;
                }
            }

            WFQueryExpr expr = new WFQueryExpr("PROC_INST_ID", SQLExpr.IN, WFAny.Create(piIDs), true);
            WFBaseProcessInstance[] pis = api.QueryProcInsts(expr);
            if (pis == null) return h;

            foreach (WFBaseProcessInstance pi in pis)
            {
                h[pi.ProcInstID] = pi.ProcInstName;
            }
            return h;
        }

        #region zhoulli
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
                case "Passed": status = "步骤已完成"; break;
                case "Active": status = "步骤活动中"; break;
                default: status = name; break;
            }
            return status;
        }
        private string GetUserName(string userID)
        {
            return OAUser.GetUserName(userID) + " " + userID;
        }
        #endregion
    }
}
