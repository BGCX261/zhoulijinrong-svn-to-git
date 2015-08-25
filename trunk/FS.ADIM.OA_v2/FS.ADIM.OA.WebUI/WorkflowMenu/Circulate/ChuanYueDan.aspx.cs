using System;
using System.Data;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Circulate
{
    public partial class ChuanYueDan : System.Web.UI.Page
    {
        protected String m_strTemplateName = null;
        protected String m_strProcessID = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_strTemplateName = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];
                m_strProcessID = Request.QueryString[ConstString.QueryString.PROCESS_ID];

                if (String.IsNullOrEmpty(m_strTemplateName) || String.IsNullOrEmpty(m_strProcessID))
                {
                    return;
                }

                B_Circulate l_busCirculate = new B_Circulate(TableName.OtherTableName.V_OA_Circulate);

                //String l_strCirculateTableName = TableName.GetCirculateTableName(m_strTemplateName);

                //if (String.IsNullOrEmpty(l_strCirculateTableName))
                //{
                //    return;
                //}

                DataTable l_dtbDataTable = B_Circulate.GetCirculateList(m_strTemplateName, m_strProcessID);

                RepeaterSend.DataSource = l_dtbDataTable;
                RepeaterSend.DataBind();
            }
        }
        /// <summary>
        /// 获得用户姓名
        /// </summary>
        /// <param name="deptName"></param>
        /// <param name="uid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected String GetUserName(String p_strUserID, String p_strUserName)
        {
            if (!String.IsNullOrEmpty(p_strUserName))
            {
                return p_strUserName;
            }
            FounderSoftware.ADIM.OU.BLL.Busi.User l_objUser = FS.ADIM.OU.OutBLL.OAUser.GetUser(p_strUserID);
            if (l_objUser == null)
            {
                return String.Empty;
            }
            return l_objUser.Name;
        }

        /// <summary>
        /// 获得阅知时间
        /// </summary>
        /// <param name="isRead"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        protected String GetReadDate(String isRead, String date)
        {
            if (isRead == "True")
            {
                return "<b>" + SysConvert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss") + "</b>";
            }
            return String.Empty;
        }

        /// <summary>
        /// 获得分发时间
        /// </summary>
        /// <param name="sendUserName"></param>
        /// <param name="date"></param>
        /// <param name="stepName"></param>
        /// <returns></returns>
        protected String GetDistributeDate(String lvCode, String p_strSendUserName, String date)
        {
            String stepHtml = p_strSendUserName + "/";
            if (p_strSendUserName != "")
            {
                return String.Format("<Span Title='分发人：{0}'>", p_strSendUserName) + stepHtml + SysConvert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss") + "</Span>";
            }
            else
            {
                return stepHtml + date;
            }
        }
    }
}
