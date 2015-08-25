using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public partial class UC_LetterReceived
    {
        private UC_Print m_print = null;
        private B_LetterReceive m_Entity = null;

        public void InitPrint()
        {
            //打印
            ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            ucPrint.UCTemplateName = "函件收文"; //ConstString.ProcessTemplate.LETTER_REVEIVE;
            ucPrint.UCStepName = this.StepName; //base.ViewIDorName;
        }

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            LR_Print print = new LR_Print();
            //B_LetterReceive cEntity = new B_LetterReceive();
            //this.PopulateEntity(cEntity, base.ViewIDorName, base.WorkItemID);
            B_LetterReceive cEntity = null;//this.ControlToEntity(false) as B_LetterReceive;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_LetterReceive : new B_LetterReceive();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_LetterReceive;
            }
            //SetEntity(cEntity);
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            LR_Print print = new LR_Print();
            //B_LetterReceive cEntity = new B_LetterReceive();
            //this.PopulateEntity(cEntity, base.ViewIDorName, base.WorkItemID);
            B_LetterReceive cEntity = null;//this.ControlToEntity(false) as B_LetterReceive;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_LetterReceive : new B_LetterReceive();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_LetterReceive;
            }
            //SetEntity(cEntity);
            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {

        }

        private void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            B_LetterReceive cEntity = null;//this.ControlToEntity(false) as B_LetterReceive;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_LetterReceive : new B_LetterReceive();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_LetterReceive;
            }

            switch (ucPrint.FileName)
            {
                case "函件收文表单":
                    ucPrint.BatchAddPicture("函件收文", "函件收文表单", cEntity);
                    break;
            }
            // 当模板中无法用之前的方法正确导出数据时,在本事件中调用
            // ucPrint.Write(string key, string value, WriteMode mode, int offset)方法处理导出数据

            //Print print = new Print();
            //B_LetterReceive cEntity = new B_LetterReceive();
            //this.PopulateEntity(cEntity);
            //ucPrint.Write("传阅签名", cEntity.CommonID, WordMgr.WriteMode.Right, 3);
            //ucPrint.Write("日期", "2009.2.6", WordMgr.WriteMode.Right, 3);
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
            LR_Print print = new LR_Print();

            m_Entity = m_print.m_CurrEntity as B_LetterReceive;
            print.SetPrintBeginExport(m_print, m_Entity);
        }

        private void Print_OnCompletionExport(object sender, EventArgs e)
        {

        }
        private void Print_OnAttachExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            LR_Print print = new LR_Print();

            print.SetPrintAttachExport(m_print, m_Entity);
        }
        private void Print_OnExtraExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            switch (ucPrint.FileName)
            {
                case "函件收文表单":
                    m_print.BatchAddPicture("函件收文", "函件收文表单", m_Entity);
                    break;
            }
        }
        #endregion #region 批量打印接口
    }
}
