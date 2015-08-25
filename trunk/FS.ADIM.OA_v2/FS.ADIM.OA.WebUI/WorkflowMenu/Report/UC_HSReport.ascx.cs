using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Common;


namespace FS.ADIM.OA.WebUI.WorkflowMenu.Report
{
    public partial class UC_HSReport : UCBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProcessTemplate();
            }
        }
        /// <summary>
        /// 初始化类型
        /// </summary>
        private void LoadProcessTemplate()
        {
            string strsql = "SELECT  LEFT(FileEncoding, 6) AS type  FROM T_OA_HS_Edit  WHERE (FileEncoding LIKE '%CANH') AND (LEN(FileEncoding) = 14) GROUP BY LEFT(FileEncoding, 6) having COUNT(LEFT(FileEncoding, 6))>1";
            DataTable dt = SQLHelper.GetDataTable1(strsql);
            ddlProcessTemplate.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ddlProcessTemplate.Items.Add(new ListItem(dr[0].ToString()));
            }            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProcessList();        
        }
        
        private void LoadProcessList()
        {
            DataTable dt = new DataTable();
            DataTable typedt = new DataTable();
            DataTable resultdt = new DataTable();
            resultdt.Columns.Add("type", Type.GetType("System.String"));
            resultdt.Columns.Add("number", Type.GetType("System.String"));
            resultdt.Columns.Add("receivecompany", Type.GetType("System.String"));            
            string strsqlInsert = "";

            int iStart = gvProcessList.PageIndex * gvProcessList.PageSize;
            int iEnd = gvProcessList.PageIndex * gvProcessList.PageSize + gvProcessList.PageSize;
            int icount = 0;

            if (this.ddlProcessTemplate.SelectedValue.ToString() == "")
            {
                string strsql = "SELECT     LEFT(FileEncoding, 6) AS type  FROM T_OA_HS_Edit  WHERE (FileEncoding LIKE '%CANH') AND (LEN(FileEncoding) = 14) GROUP BY LEFT(FileEncoding, 6) having COUNT(LEFT(FileEncoding, 6))>1";
                dt = SQLHelper.GetDataTable1(strsql);
                foreach (DataRow dr in dt.Rows)
                {
                    strsqlInsert = "select * from (SELECT     LEFT(FileEncoding, 6) AS type, SUBSTRING(FileEncoding, 7, 4) AS number, RIGHT(FileEncoding, 4) AS receivecompany FROM   T_OA_HS_Edit WHERE     (FileEncoding LIKE '%CANH') AND (LEN(FileEncoding) = 14)) B where B.type='" + dr[0].ToString() + "' order by B.number";
                    typedt = SQLHelper.GetDataTable1(strsqlInsert);
                    int min = 0;
                    foreach (DataRow typedr in typedt.Rows)
                    {

                        int max = int.Parse(typedr[1].ToString());
                        if (max - min != 1 && min != 0)
                        {
                            if (icount >= iStart && icount < iEnd)
                            {
                                DataRow resultrow = resultdt.NewRow();
                                resultrow[0] = typedr[0].ToString();
                                resultrow[1] = (max - 1).ToString();
                                resultrow[2] = typedr[2].ToString();
                                resultdt.Rows.Add(resultrow);
                            }
                            icount = icount + 1;
                        }
                        min = int.Parse(typedr[1].ToString());
                    }
                }
            }
            else
            {
                strsqlInsert = "select * from (SELECT     LEFT(FileEncoding, 6) AS type, SUBSTRING(FileEncoding, 7, 4) AS number, RIGHT(FileEncoding, 4) AS receivecompany FROM   T_OA_HS_Edit WHERE     (FileEncoding LIKE '%CANH') AND (LEN(FileEncoding) = 14)) B where B.type='" + this.ddlProcessTemplate.SelectedValue.ToString() + "' order by B.number";
                typedt = SQLHelper.GetDataTable1(strsqlInsert);
                int min = 0;
                foreach (DataRow typedr in typedt.Rows)
                {

                    int max = int.Parse(typedr[1].ToString());
                    if (max - min != 1 && min != 0)
                    {
                        if (icount >= iStart && icount < iEnd)
                        {
                            DataRow resultrow = resultdt.NewRow();
                            resultrow[0] = typedr[0].ToString();
                            resultrow[1] = (max - 1).ToString();
                            resultrow[2] = typedr[2].ToString();
                            resultdt.Rows.Add(resultrow);
                        }
                        icount = icount + 1;
                    }
                    min = int.Parse(typedr[1].ToString());
                }
            }
            //绑定数据           
            this.gvProcessList.RecordCount = icount;
            this.gvProcessList.DataSource = resultdt;
            this.gvProcessList.DataBind();
        }
        protected void gvProcessList_ExteriorPaging(GridViewPageEventArgs e)
        {
            LoadProcessList();
        }        
    }
}