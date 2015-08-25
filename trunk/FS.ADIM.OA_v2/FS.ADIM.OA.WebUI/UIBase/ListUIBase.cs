using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FounderSoftware.Framework.UI.WebPageFrame;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.UIBase
{
    public class ListUIBase : UCBase
    {
        protected List<String> m_strAryMessages = new List<String>();
        protected String SortExpression
        {
            get
            {
                if (ViewState[ConstString.ViewState.SORT_EXPRESSION] == null)
                {
                    return null;
                }
                return Convert.ToString(ViewState[ConstString.ViewState.SORT_EXPRESSION]);
            }
            set
            {
                ViewState[ConstString.ViewState.SORT_EXPRESSION] = value;
            }
        }

        protected String GetProcessStatus(Object p_strStatusName)
        {
            String l_strProcessStatus = "";
            switch (p_strStatusName.ToString())
            {
                case ProcessConstString.ProcessStatus.STATUS_RUNNING: l_strProcessStatus = "<font style='color:green'>运行中</font>"; break;
                case ProcessConstString.ProcessStatus.STATUS_COMPLETED: l_strProcessStatus = "<font style='color:red'>已完成</font>"; break;
                case ProcessConstString.ProcessStatus.STATUS_CANCELED: l_strProcessStatus = "<font style='color:blue'>已取消</font>"; break;
                case ProcessConstString.ProcessStatus.STATUS_SUSPENDED: l_strProcessStatus = "<font style='color:purple'>已暂停</font>"; break;
                default: break;
            }
            return l_strProcessStatus;
        }

        protected void DistinctUrgentDegree(Object p_objUrgentDegree, TableCell p_objCell)
        {
            if (p_objUrgentDegree == DBNull.Value)
            {
                return;
            }
            if (p_objUrgentDegree.ToString() == ConstString.CommonStr.Urgent)
            {
                p_objCell.Text = "<span style='color:Red;font-family:Arial Black;font-size:16px;'>!</span>";
                p_objCell.Attributes.Add("title", ConstString.CommonStr.Urgent);
            }
            else
            {
                p_objCell.Text = string.Empty;
            }
        }
        
        internal void IndicateNoData(Object p_objColumn, GridViewRow p_objRow)//renjinquan 改为 internal
        {
            if (p_objColumn == DBNull.Value)
            {
                p_objRow.Attributes.Add("title", "无数据");
                foreach (TableCell cell in p_objRow.Cells)
                {
                    cell.Style.Add(HtmlTextWriterStyle.BackgroundColor, "pink");
                }
            }
        }
    }
}
