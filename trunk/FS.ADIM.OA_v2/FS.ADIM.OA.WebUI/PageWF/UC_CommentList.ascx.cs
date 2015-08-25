using System;
using System.Data;
using FS.ADIM.OA.BLL.Common;
using FS.OA.Framework;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class UC_CommentList : System.Web.UI.UserControl
    {
        #region 变量定义
        /// <summary>
        /// 流程实例
        /// </summary>
        public String UCProcessID
        {
            get
            {
                if (ViewState[ConstString.ViewState.PROCESS_ID] == null)
                {
                    ViewState[ConstString.ViewState.PROCESS_ID] = Request.QueryString[ConstString.QueryString.PROCESS_ID];
                }
                return ViewState[ConstString.ViewState.PROCESS_ID] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.PROCESS_ID] = value;
            }
        }
        /// <summary>
        /// 流程类型
        /// </summary>
        public String UCTemplateName
        {
            get
            {
                if (ViewState[ConstString.ViewState.TEMPLATE_NAME] == null)
                {
                    ViewState[ConstString.ViewState.TEMPLATE_NAME] = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];
                }
                return ViewState[ConstString.ViewState.TEMPLATE_NAME] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.TEMPLATE_NAME] = value;
            }
        }
        /// <summary>
        /// 意见查询时间（<）
        /// </summary>
        public String UCDateTime
        {
            get
            {
                if (ViewState[ConstString.ViewState.SUBMIT_DATE] == null)
                {
                    return String.Empty;
                }
                return ViewState[ConstString.ViewState.SUBMIT_DATE] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.SUBMIT_DATE] = value;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCommentList();
            }
        }

        private void LoadCommentList()
        {
            DataTable l_dtbDataTable = new DataTable();

            String l_strQuery = String.Format(@"SELECT ID,WorkItemID,SubmitAction,
FormsData.query('{0}/CommentList/CYiJian/UserID/text()') AS UserID,
FormsData.query('{0}/CommentList/CYiJian/UserName/text()') AS UserName,
FormsData.query('{0}/CommentList/CYiJian/ViewName/text()') AS ViewName,
FormsData.query('{0}/CommentList/CYiJian/FinishTime/text()') AS FinishTime,
FormsData.query('{0}/CommentList/CYiJian/Content/text()') AS Content
FROM {1}
WHERE ProcessID in ({2}) and D_StepStatus='Completed' AND CAST(FormsData.query('{0}/CommentList/CYiJian/ViewName/text()') AS VARCHAR(100)) <> ''", GetEntityName(), TableName.GetWorkItemsTableName(UCTemplateName), GetMainProcessID());
            if (!String.IsNullOrEmpty(UCDateTime))
            {
                l_strQuery += " and SubmitDate<='" + UCDateTime + "'";
            }
            l_strQuery += String.Format(@" union all SELECT ID,WorkItemID,SubmitAction,
FormsData.query('{0}/CommentList/CYiJian/UserID/text()') AS UserID,
FormsData.query('{0}/CommentList/CYiJian/UserName/text()') AS UserName,
FormsData.query('{0}/CommentList/CYiJian/ViewName/text()') AS ViewName,
FormsData.query('{0}/CommentList/CYiJian/FinishTime/text()') AS FinishTime,
FormsData.query('{0}/CommentList/CYiJian/Content/text()') AS Content
FROM {1}
WHERE ProcessID in ({2}) and D_StepStatus='Completed' AND CAST(FormsData.query('{0}/CommentList/CYiJian/ViewName/text()') AS VARCHAR(100)) <> ''", GetEntityName(), TableName.GetWorkItemsTableName(UCTemplateName)+"_BAK", GetMainProcessID());
            if (!String.IsNullOrEmpty(UCDateTime))
            {
                l_strQuery += " and SubmitDate<='" + UCDateTime + "'";
            }
            l_dtbDataTable = FounderSoftware.Framework.Business.Entity.RunQuery(l_strQuery);

            if (l_dtbDataTable != null && l_dtbDataTable.Rows.Count != 0)
            {
                rptCommentList.DataSource = l_dtbDataTable;
                rptCommentList.DataBind();
            }
        }

        private String GetMainProcessID()
        {
            //自动判断是否是子流程
            String sql = String.Format("SELECT PROC_INST_ID FROM WF_PROC_INSTS WHERE SUPER_PROC_INST_ID='{0}'", UCProcessID);
            DataSet dtAP = SQLHelper.GetDataSet2(sql);
            if (dtAP == null || dtAP.Tables.Count == 0 || dtAP.Tables[0].Rows.Count == 0)
            {
                return "'" + UCProcessID + "'";
            }
            else
            {
                String pids = SysString.GetStringFormatForDT(dtAP.Tables[0], "PROC_INST_ID", ",", true);
                if (pids != "")
                {
                    pids += "," + "'" + UCProcessID + "'";
                }
                else
                {
                    pids = "'" + UCProcessID + "'";
                }
                return pids;
            }
        }

        private String GetEntityName()
        {
            String entityName = "";
            switch (UCTemplateName)
            {
                case ProcessConstString.TemplateName.COMPANY_RECEIVE: entityName = "B_GS_WorkItems"; break;
                case "工会发文": entityName = "EntitySend"; break;
                case "纪委发文": entityName = "EntitySend"; break;
                case "团委发文": entityName = "M_TWF_WorkItems"; break;
                case "党委发文": entityName = "EntitySend"; break;
                case "工会收文": entityName = "B_GHS_WorkItems"; break;
                case "团委收文": entityName = "B_TWS_WorkItems"; break;
                case "党委纪委收文": entityName = "B_DJS_WorkItems"; break;
                case "工作联系单": entityName = "B_WorkRelation"; break;
                case "请示报告": entityName = "B_RequestReport"; break;
                case "程序文件": entityName = "B_PF"; break;
                case "函件发文": entityName = "EntityLetterSend"; break;
                case ProcessConstString.TemplateName.LETTER_RECEIVE: entityName = "B_LetterReceive"; break;
                case ProcessConstString.TemplateName.MERGED_RECEIVE: entityName = "B_MergeReceiveBase"; break;
                default: entityName = ""; break;
            }
            return entityName;
        }
    }
}