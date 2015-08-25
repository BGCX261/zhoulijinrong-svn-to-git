using System;
using FS.ADIM.OA.WebUI.PageWF;
using System.Collections;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Entity;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC2_ProgramFile
    {
        private UC_Print m_print = null;
        private B_PF m_Entity = null;

        public void InitPrint()
        {
            ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            ucPrint.OnBeforeClosed += new UC_Print.ExportHandler(ucPrint_OnBeforeClosed);

            UCPrint2.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            UCPrint2.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            UCPrint2.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            UCPrint2.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            UCPrint2.OnBeforeClosed += new UC_Print.ExportHandler(ucPrint_OnBeforeClosed);

            UCPrint3.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            UCPrint3.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            UCPrint3.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            UCPrint3.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            UCPrint3.OnBeforeClosed += new UC_Print.ExportHandler(ucPrint_OnBeforeClosed);
            //打印
            ucPrint.UCTemplateName = "程序文件";
            ucPrint.UCStepName = this.StepName;  //base.ViewIDorName;

            UCPrint2.UCTemplateName = "程序文件";
            UCPrint2.UCStepName = this.StepName;  //base.ViewIDorName;

            UCPrint3.UCTemplateName = "程序文件";
            UCPrint3.UCStepName = this.StepName;  //base.ViewIDorName;
        }

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            UC_Print ucPrint = sender as UC_Print;
            Print print = new Print();
            B_PF cEntity = null;
            //SetEntity(cEntity);
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_PF : new B_PF();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_PF;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            UC_Print ucPrint = sender as UC_Print;
            Print print = new Print();
            B_PF cEntity = null;
            //SetEntity(cEntity);
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_PF : new B_PF();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_PF;
            }
            cEntity.ProcessID = base.ProcessID;
            cEntity.WorkItemID = base.WorkItemID;


            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {

        }

        private string GetLastDate(UC_Print ucPrint, string StepName)
        {
            string sLastDate = "";
            if (base.IsPreview == false
                && base.StepName == StepName
                && B_PF.GetComment(base.ProcessID, base.WorkItemID, StepName).Rows.Count > 0)
            {
                sLastDate = ucPrint.CheckDateTime(B_PF.GetComment(base.ProcessID, base.WorkItemID, StepName).Rows[0]["FinishTime"].ToString());
            }

            return sLastDate;
        }

        private string GetPFComment(string StepName, string StepComment)
        {
            string sComment = "";
            if (base.IsPreview == false
                && base.StepName == StepName
                && !string.IsNullOrEmpty(StepComment))
            {
                sComment = StepComment + "\r\n" + B_PF.GetNotionComment(base.ProcessID, StepName);
            }
            else
            {
                sComment = B_PF.GetNotionComment(base.ProcessID, StepName);
            }
            return sComment;
        }

        private void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            UC_Print ucPrint = sender as UC_Print;
            B_PF cEntity = null;
            B_PF cEntity2 = null;
            if (base.IdentityID == 0)
            {
                cEntity2 = this.ControlToEntity(false) as B_PF;
            }
            else
            {
                cEntity2 = base.EntityData != null ? base.EntityData as B_PF : new B_PF();
            }

            cEntity = this.ControlToEntity(false) as B_PF;

            switch (ucPrint.FileName)
            {
                #region 程序文件表单
                case "程序文件表单":
                    //Print print = new Print();
                    // 批准人
                    ucPrint.Write("操作人", cEntity.ApproveName, WordMgr.WriteMode.Down, 1);
                    ucPrint.Write("同意/否决", cEntity2.ApproverIsAgree, WordMgr.WriteMode.Down, 1);
                    ucPrint.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_APPROVE, cEntity.ApproveComment), WordMgr.WriteMode.Down, 1);
                    if (string.IsNullOrEmpty(lblApproveDate.Text))
                    { ucPrint.Write("日期", GetLastDate(ucPrint, ProcessConstString.StepName.ProgramFile.STEP_APPROVE)/*ucPrint.CheckDateTime(cEntity.ApproveDate.ToShortDateString())*/, WordMgr.WriteMode.Down, 1); }
                    else
                    { ucPrint.Write("日期", ucPrint.CheckDateTime(lblApproveDate.Text.ToString()), WordMgr.WriteMode.Down, 1); }

                    // 质保审查
                    if (cEntity.ProgramSort != "工作程序")
                    {
                        ucPrint.Write("操作人", cEntity.QualityNames, WordMgr.WriteMode.Down, 2);
                        ucPrint.Write("同意/否决", cEntity2.QualityIsAgree, WordMgr.WriteMode.Down, 2);
                        ucPrint.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_QG, cEntity.QualityComment), WordMgr.WriteMode.Down, 2);
                        if (string.IsNullOrEmpty(lblQGDate.Text))
                        { ucPrint.Write("日期", GetLastDate(ucPrint, ProcessConstString.StepName.ProgramFile.STEP_QG), WordMgr.WriteMode.Down, 2); }
                        else
                        { ucPrint.Write("日期", ucPrint.CheckDateTime(lblQGDate.Text.ToString()), WordMgr.WriteMode.Down, 2); }
                    }

                    // 审核
                    ucPrint.Write("操作人", cEntity.AuditName, WordMgr.WriteMode.Down, 3);
                    ucPrint.Write("同意/否决", cEntity2.AuditorIsAgree, WordMgr.WriteMode.Down, 3);
                    ucPrint.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_AUDIT, cEntity.AuditComment), WordMgr.WriteMode.Down, 3);
                    if (string.IsNullOrEmpty(lblAuditDate.Text))
                    { ucPrint.Write("日期", GetLastDate(ucPrint, ProcessConstString.StepName.ProgramFile.STEP_AUDIT), WordMgr.WriteMode.Down, 3); }
                    else
                    { ucPrint.Write("日期", ucPrint.CheckDateTime(lblAuditDate.Text.ToString()), WordMgr.WriteMode.Down, 3); }

                    // 校对
                    ucPrint.Write("操作人", cEntity.CheckName, WordMgr.WriteMode.Down, 4);
                    ucPrint.Write("同意/否决", cEntity2.CheckerIsAgree, WordMgr.WriteMode.Down, 4);
                    ucPrint.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_CHECK, cEntity.CheckComment), WordMgr.WriteMode.Down, 4);
                    if (string.IsNullOrEmpty(lblCheckDate.Text))
                    { ucPrint.Write("日期", GetLastDate(ucPrint, ProcessConstString.StepName.ProgramFile.STEP_CHECK), WordMgr.WriteMode.Down, 4); }
                    else
                    { ucPrint.Write("日期", ucPrint.CheckDateTime(lblCheckDate.Text.ToString()), WordMgr.WriteMode.Down, 4); }

                    // 编写
                    ucPrint.Write("操作人", cEntity.WriteName, WordMgr.WriteMode.Down, 5);
                    ucPrint.Write("同意/否决", cEntity2.WriterIsAgree, WordMgr.WriteMode.Down, 5);
                    ucPrint.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_WRITE, cEntity.WriteComment), WordMgr.WriteMode.Down, 5);
                    //M_20100414 huangqi des:编写时间改为FirstDraftDate 
                    //begin
                    if (cEntity2.FirstDraftDate == DateTime.MinValue)//string.IsNullOrEmpty(lblWriteDate.Text))
                    {
                        if (cEntity2.DraftDate == DateTime.MinValue)
                        {
                            ucPrint.Write("日期", "", WordMgr.WriteMode.Down, 5);
                        }
                        else
                        {
                            ucPrint.Write("日期", cEntity2.DraftDate.ToShortDateString(), WordMgr.WriteMode.Down, 5);
                        }
                    }
                    else
                    { ucPrint.Write("日期", cEntity2.FirstDraftDate.ToShortDateString(), WordMgr.WriteMode.Down, 5); }
                    //end
                    ////////////////////////////////////////////////////////////////////////////////////////
                    ArrayList al1 = new ArrayList();
                    if (cEntity.DeptSignList == null) goto LEADERSIGN;
                    for (int i = 0; i < cEntity.DeptSignList.Count; i++)
                    {
                        ArrayList tmp = new ArrayList();
                        M_ProgramFile.DeptSign item = cEntity.DeptSignList[i];
                        tmp.Add(item.DeptName);
                        tmp.Add(item.Name);
                        tmp.Add(item.IsAgree);
                        tmp.Add(ucPrint.CheckDateTime(item.SubmitDate.ToShortDateString()));
                        tmp.Add(item.Comment);
                        tmp.Add(item.DealCondition);
                        tmp.Add(ucPrint.CheckDateTime(item.DealDate.ToShortDateString()));
                        al1.Add(tmp);
                    }
                    //al1.AddRange(cEntity.DeptSignList);
                    ucPrint.WriteTable(2, al1);
                //////////////////////////////////////////////////////////////////////////////////////////
                LEADERSIGN:
                    ArrayList al2 = new ArrayList();
                if (cEntity.LeaderSignList == null) break;
                for (int i = 0; i < cEntity.LeaderSignList.Count; i++)
                {
                    ArrayList tmp = new ArrayList();
                    M_ProgramFile.LeaderSign item = cEntity.LeaderSignList[i];
                    tmp.Add(item.Name);
                    tmp.Add(item.IsAgree);
                    tmp.Add(ucPrint.CheckDateTime(item.Date.ToShortDateString()));
                    tmp.Add(item.Comment);
                    tmp.Add(item.DealCondition);
                    tmp.Add(ucPrint.CheckDateTime(item.DealDate.ToShortDateString()));
                    al2.Add(tmp);
                }
                //al2.AddRange(cEntity.LeaderSignList);
                ucPrint.WriteTable(3, al2);
                break;
                #endregion

                #region 程序审查意见落实表
                case "程序审查意见落实表":
                ArrayList al22 = new ArrayList();
                al22 = GeneratorTableData(2, cEntity);
                ucPrint.WriteTable(2, al22);
                ArrayList al3 = new ArrayList();
                al3 = GeneratorTableData(3, cEntity);
                ucPrint.WriteTable(3, al3);
                //ArrayList al5 = new ArrayList();
                //al5 = GeneratorTableData(5, cEntity);
                //ucPrint.WriteTable(5, al5);
                break;
                #endregion

                #region 管理程序封面模板
                case "管理程序封面模板":
                ArrayList alPage1 = new ArrayList();
                ArrayList newarr = new ArrayList();
                ArrayList childarr = new ArrayList();
                alPage1 = GeneratorTableDataEx(ucPrint, 2, cEntity);
                int curr = -1;
                for (int i = 0; i < alPage1.Count; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        newarr.Add((alPage1[i] as ArrayList)[j].ToString());
                        if ((alPage1[i] as ArrayList)[j].ToString() == "质量保证处")
                        {
                            curr = i * 6 + j;
                        }
                    }
                }
                if (curr != -1)
                {
                    childarr.Add(newarr[curr].ToString());
                    childarr.Add(newarr[curr + 1].ToString());
                    childarr.Add(newarr[curr + 2].ToString());
                    newarr.RemoveRange(curr, 3);
                    newarr.Insert(0, childarr[0].ToString());
                    newarr.Insert(1, childarr[1].ToString());
                    newarr.Insert(2, childarr[2].ToString());
                    alPage1 = new ArrayList();
                    for (int k = 0; k < newarr.Count; k += 6)
                    {
                        childarr = new ArrayList();

                        childarr.Add(newarr[k].ToString());
                        childarr.Add(newarr[k + 1].ToString());
                        childarr.Add(newarr[k + 2].ToString());
                        childarr.Add(newarr[k + 3].ToString());
                        childarr.Add(newarr[k + 4].ToString());
                        childarr.Add(newarr[k + 5].ToString());
                        alPage1.Add(childarr);
                    }
                }
                ucPrint.WriteTable(2, alPage1);

                ArrayList alPage2 = new ArrayList();
                alPage2 = GeneratorTableDataEx(ucPrint, 3, cEntity);
                ucPrint.WriteTable(3, alPage2);

                ArrayList alPage3 = new ArrayList();
                alPage3 = GeneratorTableDataEx(ucPrint, 5, cEntity);
                ucPrint.WriteTable(5, alPage3);

                string[] value = { cEntity.DocumentTitle, cEntity.ProgramCode, cEntity.Edition };
                ucPrint.WriteHeaderFooter("程序名称", value, WordMgr.WriteMode.Shift);

                if (alPage1.Count == 0)
                {
                    ucPrint.DeleteString("部门会签表（排列不分先后）");
                    ucPrint.DeleteTable(2);
                    if (alPage2.Count == 0)
                    {
                        ucPrint.DeleteString("公司领导审定");
                        ucPrint.DeleteTable(2);

                        if (!cEntity.IsProgramCompanCheck)
                        {
                            ucPrint.DeleteString("中国核电工程有限公司会签");
                            ucPrint.DeleteTable(2);
                        }
                    }
                }
                else if (alPage2.Count == 0)
                {
                    ucPrint.DeleteString("公司领导审定");
                    ucPrint.DeleteTable(3);

                    if (!cEntity.IsProgramCompanCheck)
                    {
                        ucPrint.DeleteString("中国核电工程有限公司会签");
                        ucPrint.DeleteTable(3);
                    }
                }
                else
                {
                    if (!cEntity.IsProgramCompanCheck)
                    {
                        ucPrint.DeleteString("中国核电工程有限公司会签");
                        ucPrint.DeleteTable(4);
                    }
                }

                break;
                #endregion
            }

            switch (ucPrint.FileName)
            {
                case "程序审查意见落实表":
                    ucPrint.BatchAddPicture("程序文件", "程序审查意见落实表", cEntity2);
                    break;
                case "程序文件表单":
                    ucPrint.BatchAddPicture("程序文件", "程序文件表单", cEntity2);
                    break;
                case "管理程序封面模板": 
                    ucPrint.BatchAddPicture("程序文件", "管理程序封面模板", cEntity2);
                    break;
            }
        }

        private void ucPrint_OnBeforeClosed(object sender, EventArgs e)
        {

        }

        private ArrayList GeneratorTableDataEx(UC_Print ucPrint, int TblIndex, B_PF cEntity)
        {
            ArrayList al = new ArrayList();
            ArrayList tmp = new ArrayList();
            switch (TblIndex)
            {
                case 2:
                    if (cEntity.DeptSignList == null) break;

                    //lsqkdeptyijian += yijian.Content + "\r\a";
                    //lsqkdeptyijianluoshi += yijian.DealCondition + "\r\a";
                    for (int i = 0; i < cEntity.DeptSignList.Count; i += 2)
                    {
                        tmp = new ArrayList();
                        M_ProgramFile.DeptSign item = cEntity.DeptSignList[i];
                        tmp.Add(item.DeptName);
                        tmp.Add(item.Name);
                        tmp.Add(ucPrint.CheckDateTime(item.SubmitDate.ToShortDateString()));

                        if (cEntity.DeptSignList.Count <= i + 1)
                        {
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                            al.Add(tmp);
                            return al;
                        }
                        item = cEntity.DeptSignList[i + 1];
                        tmp.Add(item.DeptName);
                        tmp.Add(item.Name);
                        tmp.Add(ucPrint.CheckDateTime(item.SubmitDate.ToShortDateString()));
                        al.Add(tmp);
                    }
                    break;
                case 3:
                    if (cEntity.LeaderSignList == null) break;
                    for (int i = 0; i < cEntity.LeaderSignList.Count; i++)
                    {
                        tmp = new ArrayList();
                        M_ProgramFile.LeaderSign item = cEntity.LeaderSignList[i];
                        tmp.Add(item.Name);
                        tmp.Add(ucPrint.CheckDateTime(item.Date.ToShortDateString()));
                        al.Add(tmp);
                    }
                    break;
                case 5:
                    List<B_PF.ProgramFile> lst = B_PF.GetProgramFileEditionHistory(cEntity.ProgramCode, cEntity.ProgramFileID);
                    if (lst == null) break;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        tmp = new ArrayList();
                        tmp.Add(lst[i].Edition);
                        tmp.Add(lst[i].Writer);
                        tmp.Add(lst[i].Approver);
                        tmp.Add(ucPrint.CheckDateTime(lst[i].FinishTime.ToShortDateString()));
                        tmp.Add(lst[i].Reason);
                        al.Add(tmp);
                    }
                    break;
            }
            return al;
        }

        private ArrayList GeneratorTableData(int TblIndex, B_PF cEntity)//任金权 修改
        {
            ArrayList al = new ArrayList();
            B_PF pf = new B_PF();
            switch (TblIndex)
            {
                case 2:
                    List<M_ProgramFile.DeptSign> Dhqlist = new List<M_ProgramFile.DeptSign>();
                    Dhqlist = this.ucBuMenHuiQian.UCGetHQList();
                    for (int i = 0; i < Dhqlist.Count; i++)
                    {
                        M_ProgramFile.DeptSign item = Dhqlist[i];
                        foreach (B_PF.YiJian yijian in pf.GetSignComment(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN, item.ID, string.Empty))
                        {
                            ArrayList tmp = new ArrayList();
                            tmp.Add(item.DeptName);
                            tmp.Add(yijian.Content);
                            tmp.Add(yijian.DealCondition);
                            al.Add(tmp);
                        }
                    }
                    ArrayList tmpEx = new ArrayList();
                    string lsqkdeptyijian = string.Empty;
                    string lsqkdeptyijianluoshi = string.Empty;
                    foreach (B_PF.YiJian yijian in pf.GetSignComment(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.ProgramFile.STEP_QG, this.wfQualityIDs.Text, string.Empty))
                    {
                        //lsqkdeptyijian += yijian.Content + "\r\a";
                        //lsqkdeptyijianluoshi += yijian.DealCondition + "\r\a";
                        tmpEx = new ArrayList();
                        tmpEx.Add("质量保证处");
                        tmpEx.Add(yijian.Content);
                        tmpEx.Add(yijian.DealCondition);
                        al.Add(tmpEx);
                    }
                    break;
                case 3:
                    List<M_ProgramFile.LeaderSign> lhqlist = new List<M_ProgramFile.LeaderSign>();
                    lhqlist = this.ucLDHuiQian.UCGetHQList();
                    for (int i = 0; i < lhqlist.Count; i++)
                    {
                        M_ProgramFile.LeaderSign item = lhqlist[i];
                        foreach (B_PF.YiJian yijian in pf.GetSignComment(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN, item.ID, string.Empty))
                        {
                            ArrayList tmp = new ArrayList();
                            tmp.Add(yijian.UserName);
                            tmp.Add(yijian.Content);
                            tmp.Add(yijian.DealCondition);
                            al.Add(tmp);
                        }
                    }
                    tmpEx = new ArrayList();
                    string lsqkpeopleyijian = string.Empty;
                    string lsqkpeopleyijianluoshi = string.Empty;
                    if (!string.IsNullOrEmpty(this.ddlApprove.SelectedValue))
                    {
                        foreach (B_PF.YiJian yijian in pf.GetSignComment(base.ProcessID, base.WorkItemID, ProcessConstString.StepName.ProgramFile.STEP_APPROVE, this.ddlApprove.SelectedValue, string.Empty))
                        {
                            //lsqkpeopleyijian += yijian.Content + "\r\a";
                            //lsqkpeopleyijianluoshi += yijian.DealCondition + "\r\a";
                            tmpEx = new ArrayList();
                            tmpEx.Add(yijian.UserName);
                            tmpEx.Add(yijian.Content);
                            tmpEx.Add(yijian.DealCondition);
                            al.Add(tmpEx);
                        }
                    }
                    break;
                case 5:
                    if (cEntity.DeptSignList == null) break;
                    for (int i = 0; i < cEntity.DeptSignList.Count; i++)
                    {
                        ArrayList tmp = new ArrayList();
                        M_ProgramFile.DeptSign item = cEntity.DeptSignList[i];
                        if (string.IsNullOrEmpty(item.Comment)) continue;
                        tmp.Add(item.DeptName);
                        tmp.Add(item.Comment);
                        tmp.Add(item.DealCondition);
                        al.Add(tmp);
                    }
                    break;
            }

            return al;
        }
        #endregion

        #region 批量打印接口
        public override void InitPrint(UC_Print ucprint, string sProcName, string sStepName,
            string sStartTime, string sEndTime)
        {
            m_print = ucprint;

            m_print.OnBeginExport += new UC_Print.ExportHandler(Print_OnBeginExport);
            m_print.OnCompletionExport += new UC_Print.ExportHandler(Print_OnCompletionExport);
            m_print.OnAttachExport += new UC_Print.ExportHandler(Print_OnAttachExport);
            m_print.OnExtraExport += new UC_Print.ExportHandler(Print_OnExtraExport);
            m_print.OnBeforeClosed += new UC_Print.ExportHandler(Print_OnBeforeClosed);

            //打印
            m_print.UCTemplateName = sProcName;
            m_print.UCStepName = sStepName;

            //m_print.m_ls = B_FormsData.GetEntities("FA3707F767DE49769DB675CD00278308",
            //    null, sProcName, sStepName, true);
            string[] sDateTimes = sStartTime.Split('-');
            DateTime dtStart = new DateTime(Convert.ToInt32(sDateTimes[0]), Convert.ToInt32(sDateTimes[1]), Convert.ToInt32(sDateTimes[2]));
            sDateTimes = sEndTime.Split('-');
            DateTime dtEnd = new DateTime(Convert.ToInt32(sDateTimes[0]), Convert.ToInt32(sDateTimes[1]), Convert.ToInt32(sDateTimes[2]));
            m_print.m_ls = B_FormsData.GetEntities(sProcName, sStepName, dtStart, dtEnd, true);

            //m_print.Init();
            m_print.m_bBatch = true;
            m_print.ExportPath = @"\Batch\";
            m_print.TmpAttachFilesDirectory = @"\Batch\tmp\";
        }

        private void Print_OnBeginExport(object sender, EventArgs e)
        {
            if (m_print == null || m_print.m_ls == null || m_print.m_ls.Count == 0) return;
            Print print = new Print();

            m_Entity = m_print.m_CurrEntity as B_PF;
            m_Entity.ProcessID = base.ProcessID;
            m_Entity.WorkItemID = base.WorkItemID;
            print.SetPrintBeginExport(m_print, m_Entity);
        }

        private void Print_OnCompletionExport(object sender, EventArgs e)
        {

        }
        private void Print_OnAttachExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            Print print = new Print();

            print.SetPrintAttachExport(m_print, m_Entity);
        }
        private void Print_OnExtraExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            Print print = new Print();

            switch (m_print.FileName)
            {
                case "程序审查意见落实表":
                    m_print.BatchAddPicture("程序文件", "程序审查意见落实表", m_Entity);
                    break;
                case "程序文件表单":
                    m_print.BatchAddPicture("程序文件", "程序文件表单", m_Entity);
                    break;
                case "管理程序封面模板":
                    m_print.BatchAddPicture("程序文件", "管理程序封面模板", m_Entity);
                    break;
            }

            switch (m_print.FileName)
            {
                case "程序文件表单":
                    //Print print = new Print();
                    // 批准人
                    m_print.Write("操作人", m_Entity.ApproveName, WordMgr.WriteMode.Down, 1);
                    m_print.Write("同意/否决", m_Entity.ApproverIsAgree, WordMgr.WriteMode.Down, 1);
                    m_print.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_APPROVE, m_Entity.ApproveComment), WordMgr.WriteMode.Down, 1);
                    if (string.IsNullOrEmpty(lblApproveDate.Text))
                    { m_print.Write("日期", GetLastDate(m_print, ProcessConstString.StepName.ProgramFile.STEP_APPROVE)/*m_print.CheckDateTime(m_Entity.ApproveDate.ToShortDateString())*/, WordMgr.WriteMode.Down, 1); }
                    else
                    { m_print.Write("日期", m_print.CheckDateTime(lblApproveDate.Text.ToString()), WordMgr.WriteMode.Down, 1); }

                    // 质保审查
                    if (m_Entity.ProgramSort != "工作程序")
                    {
                        m_print.Write("操作人", m_Entity.QualityNames, WordMgr.WriteMode.Down, 2);
                        m_print.Write("同意/否决", m_Entity.QualityIsAgree, WordMgr.WriteMode.Down, 2);
                        m_print.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_QG, m_Entity.QualityComment), WordMgr.WriteMode.Down, 2);
                        if (string.IsNullOrEmpty(lblQGDate.Text))
                        { m_print.Write("日期", GetLastDate(m_print, ProcessConstString.StepName.ProgramFile.STEP_QG), WordMgr.WriteMode.Down, 2); }
                        else
                        { m_print.Write("日期", m_print.CheckDateTime(lblQGDate.Text.ToString()), WordMgr.WriteMode.Down, 2); }
                    }

                    // 审核
                    m_print.Write("操作人", m_Entity.AuditName, WordMgr.WriteMode.Down, 3);
                    m_print.Write("同意/否决", m_Entity.AuditorIsAgree, WordMgr.WriteMode.Down, 3);
                    m_print.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_AUDIT, m_Entity.AuditComment), WordMgr.WriteMode.Down, 3);
                    if (string.IsNullOrEmpty(lblAuditDate.Text))
                    { m_print.Write("日期", GetLastDate(m_print, ProcessConstString.StepName.ProgramFile.STEP_AUDIT), WordMgr.WriteMode.Down, 3); }
                    else
                    { m_print.Write("日期", m_print.CheckDateTime(lblAuditDate.Text.ToString()), WordMgr.WriteMode.Down, 3); }

                    // 校对
                    m_print.Write("操作人", m_Entity.CheckName, WordMgr.WriteMode.Down, 4);
                    m_print.Write("同意/否决", m_Entity.CheckerIsAgree, WordMgr.WriteMode.Down, 4);
                    m_print.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_CHECK, m_Entity.CheckComment), WordMgr.WriteMode.Down, 4);
                    if (string.IsNullOrEmpty(lblCheckDate.Text))
                    { m_print.Write("日期", GetLastDate(m_print, ProcessConstString.StepName.ProgramFile.STEP_CHECK), WordMgr.WriteMode.Down, 4); }
                    else
                    { m_print.Write("日期", m_print.CheckDateTime(lblCheckDate.Text.ToString()), WordMgr.WriteMode.Down, 4); }

                    // 编写
                    m_print.Write("操作人", m_Entity.WriteName, WordMgr.WriteMode.Down, 5);
                    m_print.Write("同意/否决", m_Entity.WriterIsAgree, WordMgr.WriteMode.Down, 5);
                    m_print.Write("意见", GetPFComment(ProcessConstString.StepName.ProgramFile.STEP_WRITE, m_Entity.WriteComment), WordMgr.WriteMode.Down, 5);
                    //M_20100414 huangqi des:编写时间改为FirstDraftDate 
                    //begin
                    if (m_Entity.FirstDraftDate == DateTime.MinValue)//string.IsNullOrEmpty(lblWriteDate.Text))
                    {
                        if (m_Entity.DraftDate == DateTime.MinValue)
                        {
                            ucPrint.Write("日期", "", WordMgr.WriteMode.Down, 5);
                        }
                        else
                        {
                            ucPrint.Write("日期", m_Entity.DraftDate.ToShortDateString(), WordMgr.WriteMode.Down, 5);
                        }
                    }
                    else
                    { m_print.Write("日期", m_Entity.FirstDraftDate.ToShortDateString(), WordMgr.WriteMode.Down, 5); }
                    //end
                    ////////////////////////////////////////////////////////////////////////////////////////
                    ArrayList al1 = new ArrayList();
                    if (m_Entity.DeptSignList == null) goto LEADERSIGN;
                    for (int i = 0; i < m_Entity.DeptSignList.Count; i++)
                    {
                        ArrayList tmp = new ArrayList();
                        M_ProgramFile.DeptSign item = m_Entity.DeptSignList[i];
                        tmp.Add(item.DeptName);
                        tmp.Add(item.Name);
                        tmp.Add(item.IsAgree);
                        tmp.Add(m_print.CheckDateTime(item.SubmitDate.ToShortDateString()));
                        tmp.Add(item.Comment);
                        tmp.Add(item.DealCondition);
                        tmp.Add(m_print.CheckDateTime(item.DealDate.ToShortDateString()));
                        al1.Add(tmp);
                    }
                    //al1.AddRange(m_Entity.DeptSignList);
                    m_print.WriteTable(2, al1);
                //////////////////////////////////////////////////////////////////////////////////////////
                LEADERSIGN:
                    ArrayList al2 = new ArrayList();
                if (m_Entity.LeaderSignList == null) break;
                for (int i = 0; i < m_Entity.LeaderSignList.Count; i++)
                {
                    ArrayList tmp = new ArrayList();
                    M_ProgramFile.LeaderSign item = m_Entity.LeaderSignList[i];
                    tmp.Add(item.Name);
                    tmp.Add(item.IsAgree);
                    tmp.Add(m_print.CheckDateTime(item.Date.ToShortDateString()));
                    tmp.Add(item.Comment);
                    tmp.Add(item.DealCondition);
                    tmp.Add(m_print.CheckDateTime(item.DealDate.ToShortDateString()));
                    al2.Add(tmp);
                }
                //al2.AddRange(m_Entity.LeaderSignList);
                m_print.WriteTable(3, al2);
                break;

                case "程序审查意见落实表":
                ArrayList al22 = new ArrayList();
                al22 = GeneratorTableData(2, m_Entity);
                m_print.WriteTable(2, al22);
                ArrayList al3 = new ArrayList();
                al3 = GeneratorTableData(3, m_Entity);
                m_print.WriteTable(3, al3);
                //ArrayList al5 = new ArrayList();
                //al5 = GeneratorTableData(5, m_Entity);
                //m_print.WriteTable(5, al5);
                break;

                case "管理程序封面模板":
                ArrayList alPage1 = new ArrayList();
                ArrayList newarr = new ArrayList();
                ArrayList childarr = new ArrayList();
                alPage1 = GeneratorTableDataEx(m_print, 2, m_Entity);
                int curr = -1;
                for (int i = 0; i < alPage1.Count; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        newarr.Add((alPage1[i] as ArrayList)[j].ToString());
                        if ((alPage1[i] as ArrayList)[j].ToString() == "质量保证处")
                        {
                            curr = i * 6 + j;
                        }
                    }
                }
                if (curr != -1)
                {
                    childarr.Add(newarr[curr].ToString());
                    childarr.Add(newarr[curr + 1].ToString());
                    childarr.Add(newarr[curr + 2].ToString());
                    newarr.RemoveRange(curr, 3);
                    newarr.Insert(0, childarr[0].ToString());
                    newarr.Insert(1, childarr[1].ToString());
                    newarr.Insert(2, childarr[2].ToString());
                    alPage1 = new ArrayList();
                    for (int k = 0; k < newarr.Count; k += 6)
                    {
                        childarr = new ArrayList();

                        childarr.Add(newarr[k].ToString());
                        childarr.Add(newarr[k + 1].ToString());
                        childarr.Add(newarr[k + 2].ToString());
                        childarr.Add(newarr[k + 3].ToString());
                        childarr.Add(newarr[k + 4].ToString());
                        childarr.Add(newarr[k + 5].ToString());
                        alPage1.Add(childarr);
                    }
                }
                m_print.WriteTable(2, alPage1);

                ArrayList alPage2 = new ArrayList();
                alPage2 = GeneratorTableDataEx(m_print, 3, m_Entity);
                m_print.WriteTable(3, alPage2);

                ArrayList alPage3 = new ArrayList();
                alPage3 = GeneratorTableDataEx(m_print, 5, m_Entity);
                m_print.WriteTable(5, alPage3);

                string[] value = { m_Entity.DocumentTitle, m_Entity.ProgramCode, m_Entity.Edition };
                m_print.WriteHeaderFooter("程序名称", value, WordMgr.WriteMode.Shift);

                if (alPage1.Count == 0)
                {
                    m_print.DeleteString("部门会签表（排列不分先后）");
                    m_print.DeleteTable(2);
                    if (alPage2.Count == 0)
                    {
                        m_print.DeleteString("公司领导审定");
                        m_print.DeleteTable(2);

                        if (!m_Entity.IsProgramCompanCheck)
                        {
                            m_print.DeleteString("中国核电工程有限公司会签");
                            m_print.DeleteTable(2);
                        }
                    }
                }
                else if (alPage2.Count == 0)
                {
                    m_print.DeleteString("公司领导审定");
                    m_print.DeleteTable(3);

                    if (!m_Entity.IsProgramCompanCheck)
                    {
                        m_print.DeleteString("中国核电工程有限公司会签");
                        m_print.DeleteTable(3);
                    }
                }
                else
                {
                    if (!m_Entity.IsProgramCompanCheck)
                    {
                        m_print.DeleteString("中国核电工程有限公司会签");
                        m_print.DeleteTable(4);
                    }
                }

                break;
            }
        }
        private void Print_OnBeforeClosed(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
        }
        #endregion #region 批量打印接口
    }
}
