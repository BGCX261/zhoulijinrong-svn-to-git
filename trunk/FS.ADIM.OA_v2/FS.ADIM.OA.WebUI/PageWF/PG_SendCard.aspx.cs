//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：发文卡
// 
// 
// 创建标识：wangbinyi 20100118
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi.Menu;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class PG_SendCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ProcessID = Request.QueryString["ProcessType"] == null ? "" : Request.QueryString["ProcessID"].ToString();// ProcessID

            string WorkItemID = Request.QueryString["WorkItemID"] == null ? "" : Request.QueryString["WorkItemID"].ToString();// ProcessID

            B_Circulate l_busCirculate = new B_Circulate(TableName.OtherTableName.V_OA_Circulate);

            string cTable = TableName.CirculateTableName.T_OA_GF_Circulate;
            string fTable = TableName.WorkItemsTableName.T_OA_GF_WorkItems;

            DataTable CcDt = l_busCirculate.GetCirculatesByID(cTable, ProcessID, 0);

            DataTable CfDt = l_busCirculate.GetFormByID(fTable, WorkItemID);

            RepeaterForm.DataSource = CfDt;
            RepeaterForm.DataBind();

            RepeaterSend.DataSource = CcDt;
            RepeaterSend.DataBind();
        }

        public static List<B_Circulate> CirculateEntity(List<B_Circulate> list)
        {
            return (from p in list
                    select new B_Circulate(TableName.OtherTableName.V_OA_Circulate)
                    {
                        ID = p.ID,

                    }
                    ).ToList<B_Circulate>();
        }
    }
}
