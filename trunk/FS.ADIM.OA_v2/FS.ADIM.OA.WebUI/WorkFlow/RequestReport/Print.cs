//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：Word输出类
// 
// 创建标识：
//
// 修改标识：2010-05-10 任金权
// 修改描述：1.修改SetBaseExportData函数，去除content使用HtmlToTextCode，数据已经重新统一调整
//  
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------
using WordMgr;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.RequestReport
{
    public class Print
    {
        private void SetBaseExportData(UC_Print ucPrint, B_RequestReport cEntity)
        {
            //ucPrint.AttachFileList = cEntity.FileList;
            //ucPrint.Position = "内容:";//(string)ucPrint.ExportData[2];
            //ucPrint.Mode = WriteMode.Down;
            if (cEntity.Type == "请示")
                ucPrint.ExportData.Add("海南核电有限公司请示");
            else
                ucPrint.ExportData.Add("海南核电有限公司报告");

            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));//<col>拟稿日期:|right</col>
            ucPrint.ExportData.Add(cEntity.MainSend);                      //<col>主送:|right</col>
            ucPrint.ExportData.Add(cEntity.Number);                        //<col>编号:|right</col>
            ucPrint.ExportData.Add(cEntity.Department);                    //<col>编制部门:|right</col>
            ucPrint.ExportData.Add(cEntity.CopySend);                      //<col>抄送:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentTitle);                       //<col>主题:|right</col>
            //ucPrint.ExportData.Add(SysString.HtmlToTextCode(cEntity.Content));                       //<col>内容:|right</col>
            ucPrint.ExportData.Add(cEntity.Content);//renjinquan+     
            ucPrint.ExportData.Add(cEntity.LeaderOpinion);                 //<col>领导批示:|right</col>
            ucPrint.ExportData.Add(cEntity.UndertakeCircs);                //<col>承办情况:|right</col>
            //ucPrint.ExportData.Add(cEntity.Message);     //<col>提示信息:|right</col>
            //ucPrint.ExportData.Add(cEntity.MessageAdd);  //<col>添加:|right</col>
            ucPrint.ExportData.Add(cEntity.DeptPrincipal);                 //<col>部门负责人:|right</col>
            ucPrint.ExportData.Add(cEntity.CheckDrafter);                  //<col>核稿人:|right</col>
            ucPrint.ExportData.Add(cEntity.Drafter);                       //<col>拟稿人:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ConfirmDate.ToShortDateString()));//<col>签发日期:|right</col>
        }

        public void SetPrintBeginExport(UC_Print ucPrint, B_RequestReport cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "公司报告":
                case "公司请示":
                    ucPrint.ExportData.Add(cEntity.MainSend); //<col>主送：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Number); //<col>编码：|inner</col>
                    ucPrint.ExportData.Add(cEntity.CopySend); //<col>抄送：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Department); //<col>编制处室：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Drafter); //<col>编写：|inner</col>
                    ucPrint.ExportData.Add(cEntity.DeptPrincipal); //<col>审定：|inner</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ConfirmDate.ToShortDateString())); //<col>日期：|inner</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle); //<col>主题：|inner</col>
                    //string tmp = SysString.HtmlToTextCode(cEntity.Content);
                    string tmp = cEntity.Content;//renjinquan+

                    if (!string.IsNullOrEmpty(tmp))
                    {
                        ucPrint.ExportData.Add(tmp);
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    //ucPrint.ExportData.Add(cEntity.Content); //<col>正文|shift</col>
                    string sLeaderOpinion = cEntity.LeaderOpinion.Replace("(领导批示)", "");
                    sLeaderOpinion = sLeaderOpinion.Replace("\n", "");
                    string[] result = sLeaderOpinion.Split(new char[]{'[', ']'});
                    if (result.Length == 0)
                    {
                        ucPrint.ExportData.Add(sLeaderOpinion); //<col>领导批示：|inner</col>
                    }
                    else
                    {
                        if (result.Length >= 3)
                            ucPrint.ExportData.Add(result[0] + "\n" + result[1] + "\n" + result[2]);
                        else
                            ucPrint.ExportData.Add("");
                    }

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "领导批示：";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                case "请示报告表单":
                    SetBaseExportData(ucPrint, cEntity);
                    //ucPrint.ExportData.Add(cEntity.Message);       //<col>伴随信息:|right</col>
                    //ucPrint.ExportData.Add(cEntity.UndertakeCircs);//<col>批示意见:|right</col>
                    ucPrint.ExportData.Add(cEntity.FenFaFanWei);   //<col>部门:|inner</col>
                    ucPrint.ExportData.Add(cEntity.GongSiLingDao);   //<col>公司领导:|inner</col>
                    //ucPrint.ExportData.Add(cEntity.FenFaFanWei);   //<col>分发范围:|right</col>

                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList)); //<col>附件:|down</col>
                    break;
            }
        }
        public void SetPrintAttachExport(UC_Print ucPrint, B_RequestReport cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "公司报告":
                case "公司请示":
                    ucPrint.WriteContent("领导批示：", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;
                case "请示报告表单":
                    //ucPrint.WriteContent("内容:", WriteMode.Down, 1);
                    //ucPrint.WriteAttach();
                    break;
            }
        }
    }
}
