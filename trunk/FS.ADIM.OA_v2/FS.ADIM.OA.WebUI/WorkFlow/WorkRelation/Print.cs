//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：Word输出类
// 
// 创建标识：
//
// 修改标识：2010-05-10 任金权
// 修改描述：1.修改SetPrintBeginExport函数，去除content使用HtmlToTextCode，数据已经重新统一调整
//  
// 修改标识：2010-5-13 任金权
// 修改描述：1.修改SetBaseExportData函数，以前的属性ChengBanRiQi在新版中不用了。
//
//----------------------------------------------------------------
using WordMgr;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public class Print
    {
        private void SetBaseExportData(UC_Print ucPrint, B_WorkRelation cEntity, string sParam)
        {
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));       //<col>拟稿日期:|right</col>
            ucPrint.ExportData.Add(cEntity.MainSend);        //<col>主送:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentNo);      //<col>编号:|right</col>
            ucPrint.ExportData.Add(cEntity.Department);      //<col>编制部门:|right</col>
            ucPrint.ExportData.Add(cEntity.CopySend);        //<col>抄送:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentTitle);         //<col>主题:|right</col>
            //ucPrint.ExportData.Add(SysString.HtmlToTextCode(cEntity.Content));         //<col>内容:|right</col>
            ucPrint.ExportData.Add(cEntity.Content);         //<col>内容:|right</col>

            ucPrint.ExportData.Add(cEntity.UndertakeCircs);  //<col>答复或处理意见:|right</col>
            ucPrint.ExportData.Add(cEntity.BanLiYiJian);     //<col>办理意见:|right</col>
            ucPrint.ExportData.Add(cEntity.DeptLeader);      //<col>承办部门领导:|right</col>
            ucPrint.ExportData.Add(cEntity.SectionLeader);   //<col>科室领导:|right</col>
            ucPrint.ExportData.Add(cEntity.Contractor);      //<col>承办人:|right</col>

            //ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ChengBanRiQi.ToShortDateString()));    //<col>承办日期:|right</col>
            string strChengBanRiQi=cEntity.StepName==ProcessConstString.StepName.WorkRelationStepName.STEP_DIRECTOR?cEntity.DirectorDate.ToString():cEntity.StepName==ProcessConstString.StepName.WorkRelationStepName.STEP_CHIEF?cEntity.SectionDate.ToString():cEntity.StepName==ProcessConstString.StepName.WorkRelationStepName.STEP_MEMBER?cEntity.MemberDate.ToString():"";
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ChengBanRiQi.ToString() == System.DateTime.MinValue.ToString() ? strChengBanRiQi : cEntity.ChengBanRiQi.ToString()));    //<col>承办日期:|right</col>    renjinquan+ 根据步骤取承办日期。  
    
            //ucPrint.ExportData.Add(cEntity.Message);       //<col>提示信息:|right</col>
            //if (sParam != "工作联系单7")
            //    ucPrint.ExportData.Add(cEntity.MessageAdd);//<col>添加:|right</col>
            ucPrint.ExportData.Add(cEntity.DeptPrincipal);   //<col>签发人:|right</col>
            ucPrint.ExportData.Add(cEntity.CheckDrafter);    //<col>核稿人:|right</col>
            ucPrint.ExportData.Add(cEntity.Drafter);         //<col>拟稿人:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ConfirmDate.ToShortDateString()));     //<col>签发日期:|right</col>
        }


        public void SetPrintBeginExport(UC_Print ucPrint, B_WorkRelation cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "工作联系单":
                    ucPrint.ExportData.Add(cEntity.MainSend);    //<col>主送:|inner</col>
                    ucPrint.ExportData.Add(cEntity.Number);    //<col>编码:|inner</col>
                    ucPrint.ExportData.Add(cEntity.CopySend);    //<col>抄送:|inner</col>
                    ucPrint.ExportData.Add(cEntity.Department);    //<col>编制处室:|inner</col>
                    ucPrint.ExportData.Add(cEntity.Drafter);    //<col>编写:|inner<inner/col>
                    ucPrint.ExportData.Add(cEntity.CheckDrafter);    //<col>校核:|inner</col>
                    ucPrint.ExportData.Add(cEntity.DeptPrincipal);    //<col>审定:|inner</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ConfirmDate.ToShortDateString()));    //<col>日期:|inner</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);    //<col>主题:|inner</col>
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
                    //ucPrint.ExportData.Add(cEntity.Content);    //<col>正文|shift</col>
                    ucPrint.ExportData.Add(cEntity.BanLiYiJian);    //<col>承办意见:|right</col>

                    //string sUndertakeCircs = "";
                    //if (!string.IsNullOrEmpty(cEntity.UndertakeCircs))
                    //{
                    //    sUndertakeCircs = cEntity.UndertakeCircs.Remove(0, 1);
                    //}
                    ucPrint.ExportData.Add(cEntity.UndertakeCircs);    //<col>承办结果:|right</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "内容";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Down;
                    break;
                case "工作联系单表单":
                    SetBaseExportData(ucPrint, cEntity, "工作联系单表单");
                    ucPrint.ExportData.Add(cEntity.ChuanYueRenYuan);    //<col>传阅人员:|right</col>

                    //ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList)); //<col>附件:|down</col>
                    break;
            }
        }
        public void SetPrintAttachExport(UC_Print ucPrint, B_WorkRelation cEntity)
        {

            switch (ucPrint.FileName)
            {
                case "工作联系单":
                    ucPrint.WriteContent("承办意见:", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;
                case "工作联系单表单":
                    //ucPrint.WriteContent("内容:", WriteMode.Down, 1);
                    //ucPrint.WriteAttach();
                    break;
            }
        }
    }
}