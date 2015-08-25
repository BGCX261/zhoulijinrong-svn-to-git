using WordMgr;
using System.Data;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public class LR_Print
    {
        public void SetPrintBeginExport(UC_Print ucPrint, B_LetterReceive cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "函件收文表单":
                    ucPrint.ExportData.Add(cEntity.DocumentNo);    //<col>收文号:|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ReceiptDate.ToShortDateString()));        //<col>收文日期:|right</col>
                    ucPrint.ExportData.Add(cEntity.UrgentDegree);        //<col>紧急程度:|right</col>
                    ucPrint.ExportData.Add(cEntity.FileEncoding);        //<col>文件编号:|right</col>
                    ucPrint.ExportData.Add(cEntity.CommunicationUnit);        //<col>来文单位:|right</col>
                    ucPrint.ExportData.Add(cEntity.Pages);        //<col>页数:|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);        //<col>文件标题:|right</col>
                    ucPrint.ExportData.Add(cEntity.LeaderShipName);        //<col>批示领导:|right</col>
                    ucPrint.ExportData.Add(cEntity.ChuanYueLeader);        //<col>传阅领导:|right</col>
                    ucPrint.ExportData.Add(cEntity.UnderTake);        //<col>主办部门:|right</col>
                    ucPrint.ExportData.Add(cEntity.AssistDeptName);        //<col>协办部门:|right</col>
                    ucPrint.ExportData.Add(cEntity.ChuanYueDept);        //<col>传阅部门:|right</col>
                    ucPrint.ExportData.Add(cEntity.NiBanComment);        //<col>其它意见:|right</col>
                    string sDraftDate = string.IsNullOrEmpty(cEntity.NiBanDate)?"":"[" + cEntity.NiBanDate.Split(' ')[0] + "]";
                    ucPrint.ExportData.Add(cEntity.NiBanRenName + "\n" + sDraftDate);        //<col>拟办人:|right</col>
                    //ucPrint.ExportData.Add(cEntity.SecondPloterName);        //<col>二次拟办人:|right</col>
                    ucPrint.ExportData.Add(cEntity.LS_Comment);        //<col>领导意见:|right</col>
                    string sLeaderDate = string.IsNullOrEmpty(cEntity.LS_Date) ? "" : "[" + cEntity.LS_Date.Split(' ')[0] + "]";
                    ucPrint.ExportData.Add(cEntity.LeaderShipName + "\n" + sLeaderDate);        //<col>领导:|right</col>
                    ucPrint.ExportData.Add(cEntity.UnderTake);        //<col>承办部门:|right</col>
                    ucPrint.ExportData.Add(cEntity.UnderTake_Comment);        //<col>承办意见:|right</col>
                    /*待定*/
                    ucPrint.ExportData.Add(cEntity.UDDeptLeadName /*+ "\n" + "[" + cEntity.UDDeptLeadNameTime + "]"/*GetDeptLeaderName(cEntity.UnderTakeList)*/);        //<col>部门领导:|right</col>
                    /*待定*/
                    ucPrint.ExportData.Add(cEntity.UDSectionLeadName /*+ "\n" + "[" + cEntity.UDSectionLeadNameTime + "]"/*GetKeShiLeaderName(cEntity.UnderTakeList)*/);        //<col>科室领导:|right</col>
                    /*待定*/
                    ucPrint.ExportData.Add(cEntity.UDSectionPeopleName /*+ "\n" + "[" + cEntity.UDSectionPeopleNameTime + "]"/*GetPuTongName(cEntity.UnderTakeList)*/);        //<col>普通人员:|right</col>
                    /*待定*/
                    string[] stringSeparators = new string[] { "\n" };
                    //string[] sPrompt = cEntity.Prompt.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);//<col>备注|right</col>
                    //if (sPrompt.Length > 0)
                    //{
                    //    ucPrint.ExportData.Add(sPrompt[0]);        
                    //}
                    //else
                    //{
                    //    ucPrint.ExportData.Add(cEntity.Prompt);
                    //}
                    ucPrint.ExportData.Add(cEntity.Remarks);    //<col>备注|right</col>
                    /*待定*/
                    //ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));        //<col>附件列表|shift</col>

                    ucPrint.Position = "备注";
                    ucPrint.Mode = WriteMode.Down_Append;
                    break;
            }
        }

        public void SetPrintAttachExport(UC_Print ucPrint, B_LetterReceive cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "公文处理单":
                    ucPrint.WriteContent("日期 ", WriteMode.Down, 1);
                    ucPrint.WriteAttach();
                    break;
            }
        }

        private string GetDeptLeaderName(DataTable dt)
        {
            DataRow[] rows = dt.Select("ViewName = '处室承办'");
            if (rows.Length > 0)
            {
                string[] ret;
                try
                {
                    ret = rows[rows.Length - 1]["FinishTime"].ToString().Split(new char[]{' '});
                }
                catch
                {
                    return rows[rows.Length - 1]["UserName"].ToString();
                }
                return rows[rows.Length - 1]["UserName"].ToString() + "\r\n" + ret[0];
            }
            return "";
        }

        private string GetKeShiLeaderName(DataTable dt)
        {
            DataRow[] rows = dt.Select("ViewName = '科室承办'");
            if (rows.Length > 0)
            {
                string[] ret;
                try
                {
                    ret = rows[0]["FinishTime"].ToString().Split(new char[] { ' ' });
                }
                catch
                {
                    return rows[0]["UserName"].ToString();
                }
                return rows[0]["UserName"].ToString() + "\r\n" + ret[0];
            }
            return "";
        }

        private string GetPuTongName(DataTable dt)
        {
            DataRow[] rows = dt.Select("ViewName = '人员承办'");
            if (rows.Length > 0)
            {
                string[] ret;
                try
                {
                    ret = rows[0]["FinishTime"].ToString().Split(new char[] { ' ' });
                }
                catch
                {
                    return rows[0]["UserName"].ToString();
                }
                return rows[0]["UserName"].ToString() + "\r\n" + ret[0];
            }
            return "";
        }
    }
}
