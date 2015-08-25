using System.Text;
using System.Data;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public class ComRecv_Print
    {
        public string sDept = "";

        private string GetUnderTakeList(DataTable dtUnderTakeList)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dtUnderTakeList.Rows.Count; i++)
            {
                sb.Append("["+dtUnderTakeList.Rows[i]["UserName"].ToString()+"] ");
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

        public void SetPrintBeginExport(UC_Print ucPrint, B_GS_WorkItems cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "公文处理单":
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);   //<col>收文标题：|right</col>chen
                    ucPrint.ExportData.Add(cEntity.DocumentNo);    //<col>收文编号|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ReceiveDateTime.ToShortDateString()));   //<col>收文日期|right</col>
                    ucPrint.ExportData.Add(cEntity.SendNo);        //<col>原文号|right</col>
                    string sName = OAUser.GetUserName(cEntity.OfficerName);
                    string sResult = (string.IsNullOrEmpty(sName) ? cEntity.OfficerName : sName);
                    ucPrint.ExportData.Add((string.IsNullOrEmpty(sResult) ? "" : "[" + sResult + "]") + cEntity.Officer_Date.ToString() + cEntity.Officer_Comment);    //<col>公司办拟办意见：|right</col>
                    sName = OAUser.GetUserName(cEntity.LeaderShipName);
                    sResult = (string.IsNullOrEmpty(sName) ? cEntity.LeaderShipName : sName);
                    ucPrint.ExportData.Add(OAUser.GetUserName(cEntity.LeaderShip));  //<col>公司领导：|right</col>
                    ucPrint.ExportData.Add((string.IsNullOrEmpty(sResult) ? "" : "[" + sResult + "]") + cEntity.LS_Date.ToString() + cEntity.LS_Comment); //<col>领导批示|right</col>

                    //string[] sUnderTakeList = GetValueByColName("Content", cEntity.UnderTakeList).Split(new char[] { ';' });
                    ucPrint.ExportData.Add(GetValueByColName("DeptName", cEntity.UnderTakeList));//<col>承办部门：|right</col>
                    ucPrint.ExportData.Add(GetValueByColName("Content", cEntity.UnderTakeList));//<col>承办情况：|right</col>
                    ucPrint.ExportData.Add(GetValueByColNameEx("ReceiveUserName", cEntity.CirculateList));   //<col>传阅人员：|right</col>
                    ucPrint.ExportData.Add(GetValueByColNameEx("Comment", cEntity.CirculateList));   //<col>传阅意见：|right</col>
                    //if (sUnderTakeList.Length > 0)
                    //{
                    //    ucPrint.ExportData.Add(sUnderTakeList[0]);          //<col>承办情况|right</col>
                    //}
                    //else
                    //{
                    //    ucPrint.ExportData.Add(GetValueByColName("Content", cEntity.UnderTakeList));
                    //}

                    //<col>传阅签名|right</col>
                    //<col>日期|right</col>
                    //<col>传阅签名 |right</col>
                    //<col>日期 |right</col>
                    //ucPrint.ExportData.Add(cEntity.);
                    //ucPrint.ExportData.Add(cEntity.Content);
                    //ucPrint.ExportData.Add(cEntity.UndertakeCircs);
                    //ucPrint.ExportData.Add(cEntity.DeptPrincipal);
                    //ucPrint.ExportData.Add(cEntity.DeptPrincipal);
                    //ucPrint.ExportData.Add(cEntity.Department);
                    //ucPrint.ExportData.Add("共" + "()" + "页");
                    //ucPrint.ExportData.Add("第" + "()" + "页");

                    //ucPrint.AttachFileList = cEntity.FileList;//chen
                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));//<col>附件:|down</col>
                    //ucPrint.Position = "日期 ";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Down;
                    break;
                case "公司收文表单":
                    ucPrint.ExportData.Add(cEntity.DocumentNo);                             //<col>收文编号：|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DocumentReceiveDate.ToShortDateString()));//<col>收文日期：|right</col>
                    ucPrint.ExportData.Add(cEntity.SendNo);                                 //<col>原文号：|right</col>
                    ucPrint.ExportData.Add(cEntity.VolumeNo);                               //<col>卷号：|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);                          //<col>文件名称：|right</col>
                    ucPrint.ExportData.Add(OAUser.GetUserName(cEntity.Officer));            //<col>公司办主任|right</col>
                    ucPrint.ExportData.Add(cEntity.Officer_Comment);                        //<col>意见：|right</col>
                    ucPrint.ExportData.Add(OAUser.GetUserName(cEntity.LeaderShip));         //<col>公司领导：|right</col>
                    ucPrint.ExportData.Add(cEntity.LS_Comment);                             //<col>意见：|right</col>



                    ucPrint.ExportData.Add(GetValueByColName("DeptName", cEntity.UnderTakeList));//ucPrint.ExportData.Add(sDept);//<col>承办部门：|right</col>
                    ucPrint.ExportData.Add(GetValueByColName("Content", cEntity.UnderTakeList));                      //<col>承办意见：|right</col>

                    ucPrint.ExportData.Add(GetValueByColNameEx("ReceiveUserName", cEntity.CirculateList));   //<col>传阅人员：|right</col>
                    ucPrint.ExportData.Add(GetValueByColNameEx("Comment", cEntity.CirculateList));   //<col>传阅意见：|right</col>
                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));

                    //ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "提示信息：";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Down;
                    break;
            }
        }

        public void SetPrintAttachExport(UC_Print ucPrint, B_GS_WorkItems cEntity)
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
