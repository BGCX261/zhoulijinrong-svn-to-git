using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.WorkRelation
{
    public partial class UC_WorkRelation
    {
        private UC_Print m_print = null;
        private B_WorkRelation m_Entity = null;

        public void InitPrint()
        {
            ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            //ucPrint.OnBeforeClosed += new UCPrint.ExportHandler(ucPrint_OnBeforeClosed);
            //打印
            ucPrint.UCTemplateName = "工作联系单";
            ucPrint.UCStepName = this.StepName;
        }

        #region 打印
        void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            //B_WorkRelation cEntity = ControlToEntity(false) as B_WorkRelation;
            B_WorkRelation cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_WorkRelation : new B_WorkRelation();
            }
            else
            {
                cEntity = ControlToEntity(false) as B_WorkRelation;//GetFormData(cEntity, Submit.保存);
            }
            switch (ucPrint.FileName)
            {
                case "工作联系单表单":
                    ucPrint.BatchAddPicture("工作联系单", "工作联系单表单", cEntity);
                    break;
                case "工作联系单":
                    ucPrint.BatchAddPicture("工作联系单", "工作联系单", cEntity);
                    break;
            }
        }
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            Print print = new Print();
            //B_WorkRelation cEntity = new B_WorkRelation();
            B_WorkRelation cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_WorkRelation : new B_WorkRelation();
            }
            else
            {
                cEntity = ControlToEntity(false) as B_WorkRelation;//GetFormData(cEntity, Submit.保存);
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            Print print = new Print();
            //B_WorkRelation cEntity = new B_WorkRelation();
            B_WorkRelation cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_WorkRelation : new B_WorkRelation();
            }
            else
            {
                 cEntity = ControlToEntity(false) as B_WorkRelation;//GetFormData(cEntity, Submit.保存);
            }
            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {
            //switch (base.StepName)
            //{
            //    case ProcessConstString.ProcessStepName.LetterSend.发起函件:
            //        break;
            //    case ProcessConstString.ProcessStepName.LetterSend.核稿:
            //        break;
            //    case ProcessConstString.ProcessStepName.LetterSend.会签:
            //        break;
            //    case ProcessConstString.ProcessStepName.LetterSend.签发:
            //        break;
            //    case ProcessConstString.ProcessStepName.LetterSend.函件分发:
            //        break;
            //    case ProcessConstString.ProcessStepName.LetterSend.二次分发:
            //        break;
            //    default: break;
            //}
        }

        //private void ucPrint_OnBeforeClosed(object sender, EventArgs e)
        //{
        //    ucPrint.Write("共    页", "共" + ucPrint.Pages + "页", WordMgr.WriteMode.Shift, 1);
        //}
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
            m_Entity = m_print.m_CurrEntity as B_WorkRelation;
            //B_WorkRelation cEntity = new B_WorkRelation();
            //B_WorkRelation cEntity = ControlToEntity(false) as B_WorkRelation;//GetFormData(cEntity, Submit.保存);
            print.SetPrintBeginExport(m_print, m_Entity);
        }

        private void Print_OnCompletionExport(object sender, EventArgs e)
        {

        }
        private void Print_OnAttachExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            Print print = new Print();
            //B_WorkRelation cEntity = new B_WorkRelation();
            //B_WorkRelation cEntity = ControlToEntity(false) as B_WorkRelation;//GetFormData(cEntity, Submit.保存);
            print.SetPrintAttachExport(m_print, m_Entity);
        }
        private void Print_OnExtraExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            Print print = new Print();
            switch (ucPrint.FileName)
            {
                case "工作联系单表单":
                    m_print.BatchAddPicture("工作联系单", "工作联系单表单", m_Entity);
                    break;
                case "工作联系单":
                    m_print.BatchAddPicture("工作联系单", "工作联系单", m_Entity);
                    break;
            }
        }
        private void Print_OnBeforeClosed(object sender, EventArgs e)
        {

        }
        #endregion #region 批量打印接口
    }
}
