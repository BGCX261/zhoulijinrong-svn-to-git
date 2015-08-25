using System.Text;
using FS.ADIM.OU.OutBLL;
using System.Data;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public class Recv_Print
    {
        private string GetUnderTakeList(DataTable dtUnderTakeList)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dtUnderTakeList.Rows.Count; i++)
            {
                sb.Append("[" + dtUnderTakeList.Rows[i]["UserName"].ToString() + "] ");
                sb.Append(dtUnderTakeList.Rows[i]["FinishTime"].ToString() + " ");
                if (i == dtUnderTakeList.Rows.Count - 1)
                {
                    sb.Append(dtUnderTakeList.Rows[i]["Content"].ToString());
                }
                else
                {
                    sb.Append(dtUnderTakeList.Rows[i]["Content"].ToString() + "\r\n");
                }
            }

            return sb.ToString();
        }

        private string GetValueByColName(string ColName, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0) return sb.ToString();
            // 若内容为空
            if (!string.IsNullOrEmpty(dt.Rows[0][ColName].ToString()))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(dt.Rows[i][ColName].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[i][ColName].ToString()))
                        sb.Append(";");
                }
            }
            else
            {
                if (ColName == "Content") return "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(dt.Rows[i]["UserName"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[i]["UserName"].ToString()))
                        sb.Append(";");
                }
            }

            return sb.ToString();
        }

        private string GetValueByColNameEx(string ColName, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append(dt.Rows[i][ColName].ToString());
                if (!string.IsNullOrEmpty(dt.Rows[i][ColName].ToString()))
                    sb.Append(";");
            }

            return sb.ToString();
        }

        public void SetPrintBeginExport(UC_Print ucPrint, B_MergeReceiveBase cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "党纪工团收文表单":
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);                          //<col>文件名称：|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentNo);                             //<col>收文编号：|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DocumentReceiveDate.ToShortDateString()));//<col>收文日期：|right</col>
                    ucPrint.ExportData.Add(cEntity.SendNo);                                 //<col>原文号：|right</col>
                    //ucPrint.ExportData.Add(cEntity.VolumeNo);                               //<col>卷号：|right</col>
                    //ucPrint.ExportData.Add(OAUser.GetUserName(cEntity.Officer));            //<col>党群工作处处长|right</col>
                    ucPrint.ExportData.Add(cEntity.Officer_Comment);                        //<col>党群工作处处长意见意见：|right</col>
                    ucPrint.ExportData.Add(OAUser.GetUserName(cEntity.LeaderShip));         //<col>公司领导：|right</col>
                    ucPrint.ExportData.Add(cEntity.LS_Comment);                             //<col>领导批示：|right</col>
                    ucPrint.ExportData.Add(GetValueByColName("DeptName", cEntity.UnderTakeList));//ucPrint.ExportData.Add(sDept);//<col>承办部门：|right</col>
                    ucPrint.ExportData.Add(GetValueByColName("Content", cEntity.UnderTakeList));//<col>承办意见：|right</col>
                    ucPrint.ExportData.Add(GetValueByColNameEx("ReceiveUserName", cEntity.CirculateList));   //<col>传阅人员：|right</col>
                    ucPrint.ExportData.Add(GetValueByColNameEx("Comment", cEntity.CirculateList));   //<col>传阅意见：|right</col>
                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));//<col>附件:|down</col>

                    //ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "提示信息：";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Down;
                    break;
            }
        }

        public void SetPrintAttachExport(UC_Print ucPrint, B_MergeReceiveBase cEntity)
        {
            switch (ucPrint.FileName)
            {

                case "公文处理单":
                    //ucPrint.WriteContent("日期 ", WriteMode.Down, 1);
                    ucPrint.WriteAttach();
                    break;
                case "公司收文表单":
                    //ucPrint.WriteContent("传阅意见：", WriteMode.Down, 1);
                    //ucPrint.WriteAttach();
                    break;
            }
        }
    }
}
