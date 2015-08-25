using System;
using WordMgr;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public class Print
    {
        private void SetBaseExportData(UC_Print ucPrint, B_PF cEntity)
        {
            //ucPrint.AttachFileList = cEntity.FileList;
            //ucPrint.Position = "主题词：";//(string)ucPrint.ExportData[2];
            //ucPrint.Mode = WriteMode.Up;

            ucPrint.ExportData.Add(cEntity.ApplyStyle);    //<col>申请类型:|right</col>
            //ucPrint.ExportData.Add(cEntity.ApplyReason);    //<col>申请原因:|right</col>
            //ucPrint.ExportData.Add(cEntity.RelationProgram);    //<col>可能受影响程序:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentTitle);    //<col>程序名称:|right</col>
            ucPrint.ExportData.Add(cEntity.ProgramCode);    //<col>编码:|right</col>
            ucPrint.ExportData.Add(cEntity.Edition);    //<col>版次:|right</col>       
        }

        public void SetPrintBeginExport(UC_Print ucPrint, B_PF cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "程序文件表单":
                    SetBaseExportData(ucPrint, cEntity);
                    ucPrint.ExportData.Add(cEntity.CirculateSignDept);    //<col>部门:|inner</col>
                    ucPrint.ExportData.Add(cEntity.CirculateSignUserName);        //<col>人员:|inner</col>
                    ucPrint.ExportData.Add(cEntity.CirculateDeptName);    //<col>部门: |inner</col>
                    ucPrint.ExportData.Add(cEntity.CirculateName);//<col>人员: |inner</col>
                    ucPrint.ExportData.Add(cEntity.CirculateComment);       //<col>意见:|right</col>

                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));     //<col>附件:|down</col>
                    break;
                case "管理程序变更申请表":
                    if (cEntity.ProgramSort == "管理程序")
                    {
                        ucPrint.ExportData.Add("管理程序变更申请表");//<col>管理程序变更申请表|shift</col>
                        ucPrint.ExportData.Add("HN-LL431");//<col>HN-LL431|shift</col>
                    }
                    if (cEntity.ProgramSort == "工作程序")
                    {
                        ucPrint.ExportData.Add("工作程序变更申请表");//<col>管理程序变更申请表|shift</col>
                        ucPrint.ExportData.Add("HN-LL432");//<col>HN-LL431|shift</col>
                    }
                    if (cEntity.ProgramSort == "部门级管理程序")
                    {
                        ucPrint.ExportData.Add("管理程序变更申请表");//<col>管理程序变更申请表|shift</col>
                        ucPrint.ExportData.Add("HN-LL433");//<col>HN-LL431|shift</col>
                    }
                    ucPrint.ExportData.Add(string.IsNullOrEmpty(cEntity.Year) ? DateTime.Now.Year.ToString() : cEntity.Year);  //<col>年份|shift</col>
                    /*待定*/
                    ucPrint.ExportData.Add(cEntity.SerialID);                          //<col>序号|shift</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);                       //<col>文件名称|right</col>
                    ucPrint.ExportData.Add(cEntity.ProgramCode + " , " + cEntity.Edition);//<col>编码版本|right</col>
                    ucPrint.ExportData.Add(OADept.GetDeptName(cEntity.SendDeptID)); //<col>申请部门|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));     //<col>申请日期|right</col>
                    ucPrint.ExportData.Add(cEntity.ApplyStyle);                        //<col>申请类型|right</col>
                    //ucPrint.ExportData.Add(cEntity.ApplyReason);                       //<col>原因:|inner</col>
                    //ucPrint.ExportData.Add(cEntity.RelationProgram/*RelationDemand*/);                    //<col>可能受到影响的程序:|inner</col>
                    ucPrint.ExportData.Add(cEntity.Drafter);                           //<col>申请人姓名|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));     //<col>日期|right</col>
                    ucPrint.ExportData.Add(OAUser.GetDeptManager(cEntity.SendDeptID, ConstString.Grade.ZERO));                         //<col>编制部门负责人姓名|right</col>
                    //M_20100414 huangqi des:编写时间改为FirstDraftDate
                    //begin
                    if (cEntity.FirstDraftDate != DateTime.MinValue)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.FirstDraftDate.ToShortDateString()));     //<col>日期 |right</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));
                    }
                    //end

                    //ucPrint.ExportData.Add(cEntity.QualityApproveComment
                    //PF.GetNotionComment(cEntity.ProcessID, ConstString.ProcessStepName.PROGRAM_QUALITY)*/);                      //<col>意见:|inner</col>
                    ucPrint.ExportData.Add(cEntity.AuditName);                         //<col>审核人姓名|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.AuditDate.ToShortDateString()));     //<col>日期  |right</col>
                    ucPrint.ExportData.Add(cEntity.QualityNames);                      //<col>质保处负责人姓名|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.QualityDate.ToShortDateString()));   //<col>日期   |right</col>
                    ucPrint.ExportData.Add("该程序已经更新发布");                      //<col>网页更新结果:|inner</col>
                    /*待定*/
                    ucPrint.ExportData.Add(""/*OAList.GetUserNameByWorkItemID(cEntity.WorkItemID)*/);                                        //<col>信息文档处操作人姓名|right</col>
                    //ucPrint.ExportData.Add(string.IsNullOrEmpty(cEntity.SendDate.ToShortDateString()) ? 
                    //    DateTime.Now.ToShortDateString() : ucPrint.CheckDateTime(cEntity.SendDate.ToShortDateString())
                    //    );                                        //<col>日期    |right</col>

                    ucPrint.ExportData.Add(string.IsNullOrEmpty(ucPrint.CheckDateTime(cEntity.SendDate.ToShortDateString())) ?
                        DateTime.Now.ToShortDateString() : ucPrint.CheckDateTime(cEntity.SendDate.ToShortDateString()));  //<col>日期    |right</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    ucPrint.Position = "可能受到影响的程序:";//(string)ucPrint.ExportData[2];
                    ucPrint.Mode = WriteMode.Up;
                    break;
                case "管理程序封面模板":
                    if (cEntity.ProgramSort == "管理程序")
                    {
                        ucPrint.ExportData.Add("管理程序");    //<col>管理程序|shift</col>
                        //ucPrint.ExportData.Add("HN-LL431-" + (string.IsNullOrEmpty(cEntity.Year) ? DateTime.Now.Year.ToString() : cEntity.Year) + "-" + cEntity.SerialID);    //<col>HN编码:|right</col>
                    }
                    if (cEntity.ProgramSort == "工作程序")
                    {
                        ucPrint.ExportData.Add("工作程序");    //<col>管理程序|shift</col>
                        //ucPrint.ExportData.Add("HN-LL432-" + (string.IsNullOrEmpty(cEntity.Year) ? DateTime.Now.Year.ToString() : cEntity.Year) + "-" + cEntity.SerialID);    //<col>HN编码:|right</col>
                        cEntity.QualityNames = "";
                    }
                    if (cEntity.ProgramSort == "部门级管理程序")
                    {
                        ucPrint.ExportData.Add("管理程序");    //<col>管理程序|shift</col>
                        //ucPrint.ExportData.Add("HN-LL433-" + (string.IsNullOrEmpty(cEntity.Year) ? DateTime.Now.Year.ToString() : cEntity.Year) + "-" + cEntity.SerialID);    //<col>HN编码:|right</col>
                    }
                    ucPrint.ExportData.Add(cEntity.ProgramCode);
                    ucPrint.ExportData.Add(cEntity.Edition);    //<col>版次:|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);    //<col>程序名称|shift</col>
                    if (cEntity.ApproveDate.ToShortDateString() == DateTime.MinValue.ToShortDateString())
                    {
                        ucPrint.ExportData.Add("");
                        ucPrint.ExportData.Add("");

                    }
                    else
                    {
                        ucPrint.ExportData.Add(cEntity.ApproveName);    //<col>批准实施:|right</col>
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ApproveDate.ToShortDateString()));    //<col>生效日期:|right</col>
                    }
                    ucPrint.ExportData.Add(cEntity.WriteName);    //<col>编制姓名|shift</col>
                    //M_20100414 huangqi des:编写时间改为FirstDraftDate
                    //begin
                    if (cEntity.FirstDraftDate != DateTime.MinValue)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.FirstDraftDate.ToShortDateString()));     //<col>日期 |shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));
                    }
                    //end
                    ucPrint.ExportData.Add(cEntity.CheckName);    //<col>校核姓名|shift</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.CheckDate.ToShortDateString()));    //<col>校核日期|shift</col>
                    ucPrint.ExportData.Add(cEntity.AuditName);    //<col>审核姓名|shift</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.AuditDate.ToShortDateString()));    //<col>审核日期|shift</col>
                    ucPrint.ExportData.Add(cEntity.QualityNames);    //<col>质保姓名|shift</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.QualityDate.ToShortDateString()));    //<col>质保日期|shift</col>
                    ucPrint.ExportData.Add(OADept.GetDeptName(cEntity.SendDeptID));    //<col>程序编制部门:|shift</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                case "程序审查意见落实表":
                    //string num = "";
                    //if (cEntity.ProgramSort == "管理程序")
                    //{
                    //    num = "HN-LL431-";
                    //}
                    //if (cEntity.ProgramSort == "工作程序")
                    //{
                    //    num = "HN-LL432-";
                    //}
                    //if (cEntity.ProgramSort == "部门级管理程序")
                    //{
                    //    num = "HN-LL433-";
                    //}
                    //ucPrint.ExportData.Add(num + (string.IsNullOrEmpty(cEntity.Year) ? DateTime.Now.Year.ToString() : cEntity.Year) + "-" + cEntity.SerialID);    //<col>年份|shift</col>
                    ucPrint.ExportData.Add(cEntity.ProgramCode);
                    ///*待定*/
                    ucPrint.ExportData.Add("");    //<col>第|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);    //<col>程序名称:|inner</col>
                    ucPrint.ExportData.Add(cEntity.ProgramCode + "    " + cEntity.Edition);    //<col>程序编码和版本:|inner</col>

                    //renjinquan+
                    //if (cEntity.ProgramSort == "管理程序")
                    //{
                    //    //ucPrint.ExportData.Add("管理程序");    //<col>管理程序|shift</col>
                    //    ucPrint.ExportData.Add("HN-LL431"+"    " + cEntity.Edition);    //<col>HN编码:|right</col>
                    //}
                    //if (cEntity.ProgramSort == "工作程序")
                    //{
                    //    //ucPrint.ExportData.Add("工作程序");    //<col>管理程序|shift</col>
                    //    ucPrint.ExportData.Add("HN-LL432"+"    " + cEntity.Edition);    //<col>HN编码:|right</col>
                    //}
                    //if (cEntity.ProgramSort == "部门级管理程序")
                    //{
                    //    //ucPrint.ExportData.Add("管理程序");    //<col>管理程序|shift</col>
                    //    ucPrint.ExportData.Add("HN-LL433"+"    " + cEntity.Edition);    //<col>HN编码:|right</col>
                    //}

                    //ucPrint.ExportData.Add(cEntity.RelationDemand);    //<col>对相关程序修订要求或建议:|inner</col>
                    //ucPrint.ExportData.Add(cEntity.RelationProgram/*cEntity.DocumentTitle + "" + cEntity.ProgramCode*/);    //<col>相关程序名称和编码:|inner</col>
                    //ucPrint.ExportData.Add(cEntity.ApproveName);    //<col>批准|right</col>
                    //ucPrint.ExportData.Add(cEntity.ApproveDate.ToShortDateString());    //<col>日期|right</col>
                    ucPrint.ExportData.Add(cEntity.AuditName);    //<col>审核|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.AuditDate.ToShortDateString()));    //<col>日期 |right</col>
                    ucPrint.ExportData.Add(cEntity.CheckName);    //<col>校核|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.CheckDate.ToShortDateString()));    //<col>日期  |right</col>
                    ucPrint.ExportData.Add(cEntity.WriteName);    //<col>编制|right</col>
                    //M_20100414 huangqi des:编写时间改为FirstDraftDate
                    //begin
                    if (cEntity.FirstDraftDate != DateTime.MinValue)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.FirstDraftDate.ToShortDateString()));     //<col>日期 |right</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));
                    }
                    //end
                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Up;
                    break;
            }

        }
        public void SetPrintAttachExport(UC_Print ucPrint, B_PF cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "程序文件表单":
                    //ucPrint.WriteContent("主题词：", WriteMode.Up, 1);
                    //ucPrint.WriteAttach();
                    break;
                case "管理程序变更申请表":
                    ucPrint.WriteAttach();
                    break;
                case "管理程序封面模板":
                    ucPrint.WriteAttach();
                    break;
                case "程序审查意见落实表":
                    ucPrint.WriteAttach();
                    break;
            }
        }
    }
}
