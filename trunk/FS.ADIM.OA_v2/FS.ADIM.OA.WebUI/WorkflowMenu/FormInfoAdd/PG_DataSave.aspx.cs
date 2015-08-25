using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.FormInfoAdd
{
    public partial class PG_DataSave : System.Web.UI.Page
    {
        public string TID
        {
            get
            {
                if (ViewState["TID"] == null)
                    ViewState["TID"] = "";
                return ViewState["TID"] as String;
            }
            set
            {
                ViewState["TID"] = value;
            }
        }
        public string TBID
        {
            get
            {
                if (ViewState["TBID"] == null)
                    ViewState["TBID"] = "";
                return ViewState["TBID"] as String;
            }
            set
            {
                ViewState["TBID"] = value;
            }
        }
        public string TBName
        {
            get
            {
                if (ViewState["TBName"] == null)
                    ViewState["TBName"] = "";
                return ViewState["TBName"] as String;
            }
            set
            {
                ViewState["TBName"] = value;
            }
        }
        public string WARE
        {
            get
            {
                if (ViewState["WARE"] == null)
                    ViewState["WARE"] = "";
                return ViewState["WARE"] as String;
            }
            set
            {
                ViewState["WARE"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Label lbl = new Label();
                //lbl.ID = "lblTitle1";
                //lbl.Text = "wwww";
                //Panel1.Controls.Add(lbl);
                //TID = "公司发文";
                //TBID = "2";
                if (Request.QueryString["TID"] != null)
                {
                    TID = Request.QueryString["TID"].ToString();
                }
                else
                {
                    btnSave.Enabled = false;
                    return;
                }
                if (Request.QueryString["TBID"] != null)
                {
                    TBID = Request.QueryString["TBID"].ToString();
                }
                else
                {
                    btnSave.Enabled = false;
                    return;
                }
                if (TBID == "")
                {
                    btnSave.Enabled = false;
                    return;
                }
                if (Request.QueryString["WARE"] != null)
                {
                    WARE = Request.QueryString["WARE"].ToString();
                }
                else
                {
                    btnSave.Enabled = false;
                    return;
                }
                if (WARE == "历史库")
                {
                    TBName = TableName.GetWorkItemsBKTableName(TID);
                }
                else
                {
                    TBName = TableName.GetWorkItemsTableName(TID);
                }

                string sql = string.Format(@"select name,xtype,length,isnullable from syscolumns where id =
(select top 1 ID from sysobjects where name = '{0}')", TBName);
                DataTable dtField = SQLHelper.GetDataTable1(sql);

                Panel1.Controls.Add(new LiteralControl("<table width='100%' border='0' cellspacing='0' cellpadding='0'>"));
                for (int i = 0; i < dtField.Rows.Count; i++)
                {
                    Panel1.Controls.Add(new LiteralControl("<tr>"));

                    Panel1.Controls.Add(new LiteralControl("<td style='width:100px'>"));
                    //数据库字段名
                    //<span id="Span2" class="label">
                    Panel1.Controls.Add(new LiteralControl("<span class='label'>"));
                    Panel1.Controls.Add(new LiteralControl(string.Format("{0}", dtField.Rows[i]["name"].ToString())));
                    Panel1.Controls.Add(new LiteralControl("</span>"));

                    Panel1.Controls.Add(new LiteralControl("</td>"));

                    Panel1.Controls.Add(new LiteralControl("<td>"));
                    //动态加载文本框
                    TextBox textBox = new TextBox();
                    textBox.ID = dtField.Rows[i]["name"].ToString();
                    textBox.Text = "";
                    int length = SysConvert.ToInt32(dtField.Rows[i]["length"]);
                    if (length == -1)
                    {
                        textBox.TextMode = TextBoxMode.MultiLine;
                        textBox.Height = 800;
                        textBox.Width = 500;
                    }
                    else
                    {
                        if (length > 10)
                        {
                            textBox.MaxLength = length;
                        }
                    }
                    textBox.Width = 400;
                    Panel1.Controls.Add(textBox);

                    Panel1.Controls.Add(new LiteralControl("</td>"));

                    Panel1.Controls.Add(new LiteralControl("</tr>"));
                }
                Panel1.Controls.Add(new LiteralControl("</table>"));

                Bind(dtField);

                if (TBID == "")
                {
                    btnSave.Enabled = false;
                }
            }
        }

        private void Bind(DataTable dtField)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE ID={1}", TBName, TBID);
            DataTable dtData = SQLHelper.GetDataTable1(sql);
            if (dtData.Rows.Count > 0)
            {
                for (int i = 0; i < dtField.Rows.Count; i++)
                {
                    TextBox textBox = (Panel1.Page.FindControl(dtField.Rows[i]["name"].ToString()) as TextBox);
                    if (dtField.Rows[i]["name"].ToString() == "FormsData")
                    {
                        textBox.Text = dtData.Rows[0][i].ToString().Replace("><", ">\r\n<");
                    }
                    else
                    {
                        textBox.Text = dtData.Rows[0][i].ToString();
                    }
                }
            }
        }

        private void Save()
        {
            string sqlField = string.Format(@"select name,xtype,length,isnullable from syscolumns where id =
(select top 1 ID from sysobjects where name = '{0}')", TBName);
            DataTable dtField = SQLHelper.GetDataTable1(sqlField);

            string sql = string.Format("UPDATE {0} SET ", TBName);

            for (int i = 0; i < dtField.Rows.Count; i++)
            {
                string FieldName = dtField.Rows[i]["name"].ToString();
                if (i > 0)
                {
                    sql += ",";
                }
                if (FieldName == "FormsData")
                {
                    sql += FieldName + "='" + FormsMethod.Filter2(Request.Form[FieldName].ToString().Replace(">\r\n<", "><")) + "'";
                }
                else
                {
                    sql += FieldName + "='" + FormsMethod.Filter2(Request.Form[FieldName].ToString()) + "'";
                }
            }

            sql += string.Format(" WHERE ID={0}", TBID);

            int ret = SQLHelper.ExecuteNonQuery1(sql);
            if (ret > 0)
            {
                JScript.ShowMsgBox(Page, MsgType.VbCritical, "保存成功");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}