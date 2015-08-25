using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Entity;
using System.Collections;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.LetterSend
{
    public partial class UC_LetterSend
    {
        private UC_Print m_print = null;
        private EntityLetterSend m_Entity = null;

        public void InitPrint()
        {
            ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            ucPrint.OnBeforeClosed += new UC_Print.ExportHandler(ucPrint_OnBeforeClosed);
            //打印
            ucPrint.UCTemplateName = "函件发文";
            ucPrint.UCStepName = this.StepName;
        }

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            Print print = new Print();
            EntityLetterSend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntityLetterSend;
            }
            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            //string sRes = "";
            //this.Devolve(out sRes);
            Print print = new Print();
            //EntityLetterSend entity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();
            EntityLetterSend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntityLetterSend;
            }
            print.SetPrintBeginExport(ucPrint, cEntity);
        }

        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {

        }

        private void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            string str = string.Empty;
            EntityLetterSend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntityLetterSend : new EntityLetterSend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntityLetterSend;
            }

            switch (ucPrint.FileName)
            {
                case "函件发文表单":
                    ucPrint.BatchAddPicture("函件发文", "函件发文表单", cEntity);
                    break;
            }
            
            if (cEntity.jinJi)
            {
                ucPrint.WriteByFont(0x0052, 2, "Wingdings 2");
            }
            else
            {
                ucPrint.WriteByFont(0x00A3, 2, "Wingdings 2");
            }
            if (cEntity.huiZhi)
            {
                ucPrint.WriteByFont(0x0052, 1, "Wingdings 2");
            }
            else
            {
                ucPrint.WriteByFont(0x00A3, 1, "Wingdings 2");
            }

            ArrayList al = new ArrayList();
            for (int i = 0; i < cEntity.FileList.Count; i++)
            {
                ArrayList tmp = new ArrayList();
                tmp.Add((i + 1).ToString());
                tmp.Add(cEntity.FileList[i].Alias + "." + cEntity.FileList[i].Type);
                tmp.Add(cEntity.FileList[i].Encode/* + "  " + cEntity.FileList[i].Edition*/);
                tmp.Add(cEntity.FileList[i].iPage);
                al.Add(tmp);
            }
            ucPrint.WriteTable(1, 1, al);
        }

        private void ucPrint_OnBeforeClosed(object sender, EventArgs e)
        {
            switch (base.StepName)
            {
                case ProcessConstString.StepName.LetterSend.发起函件:
                case ProcessConstString.StepName.LetterSend.核稿:
                case ProcessConstString.StepName.LetterSend.会签:
                case ProcessConstString.StepName.LetterSend.签发:
                case ProcessConstString.StepName.LetterSend.函件分发:
                case ProcessConstString.StepName.LetterSend.二次分发:
                    ucPrint.DocLayout(1, 10, 1);   // 使函件发文满页
                    break;
            }
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

            m_Entity = m_print.m_CurrEntity as EntityLetterSend;
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
                case "函件发文表单":
                    m_print.BatchAddPicture("函件发文", "函件发文表单", m_Entity);
                    break;
            }

            if (this.IsPreview)
            {
                //m_Entity = ControlToEntity(false) as EntityLetterSend;
            }
            else
            {
                m_Entity = ControlToEntity(false) as EntityLetterSend;
            }

            if (m_Entity.jinJi)
            {
                m_print.WriteByFont(0x0052, 2, "Wingdings 2");
            }
            else
            {
                m_print.WriteByFont(0x00A3, 2, "Wingdings 2");
            }
            if (m_Entity.huiZhi)
            {
                m_print.WriteByFont(0x0052, 1, "Wingdings 2");
            }
            else
            {
                m_print.WriteByFont(0x00A3, 1, "Wingdings 2");
            }

            ArrayList al = new ArrayList();
            for (int i = 0; i < m_Entity.FileList.Count; i++)
            {
                ArrayList tmp = new ArrayList();
                tmp.Add((i + 1).ToString());
                tmp.Add(m_Entity.FileList[i].Alias + "." + m_Entity.FileList[i].Type);
                tmp.Add(m_Entity.FileList[i].Encode/* + "  " + m_Entity.FileList[i].Edition*/);
                tmp.Add(m_Entity.FileList[i].iPage);
                al.Add(tmp);
            }
            m_print.WriteTable(1, 1, al);
        }
        private void Print_OnBeforeClosed(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            switch (base.StepName)
            {
                case ProcessConstString.StepName.LetterSend.发起函件:
                case ProcessConstString.StepName.LetterSend.核稿:
                case ProcessConstString.StepName.LetterSend.会签:
                case ProcessConstString.StepName.LetterSend.签发:
                case ProcessConstString.StepName.LetterSend.函件分发:
                    m_print.DocLayout(1, 10, 1);   // 使函件发文满页
                    break;
                case ProcessConstString.StepName.LetterSend.二次分发:

                    break;
            }
        }
        #endregion #region 批量打印接口
    }
}
