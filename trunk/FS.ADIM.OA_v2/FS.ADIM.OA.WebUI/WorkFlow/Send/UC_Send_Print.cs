using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.Send
{
    public partial class UC_Send
    {
        //private Snd_Print print = new Snd_Print();
        //private B_DJGTSend cEntity = null;
        //public UC_Send()
        //{
        //    cEntity = this.ControlToEntity(false) as B_DJGTSend;
        //}

        public void InitPrint()
        {
            ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            ucPrint.OnBeforeClosed += new UC_Print.ExportHandler(ucPrint_OnBeforeClosed);

            //打印
            ucPrint.UCTemplateName = base.TemplateName;
            ucPrint.UCStepName = this.StepName;
        }

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            Snd_Print print = new Snd_Print();
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            B_DJGTSend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_DJGTSend : new B_DJGTSend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_DJGTSend;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            Snd_Print print = new Snd_Print();
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            B_DJGTSend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_DJGTSend : new B_DJGTSend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_DJGTSend;
            }
            print.m_ProcessID = base.ProcessID;
            print.m_TemplateID = base.TemplateName; //base.TemplateID;
            print.m_WorkItemID = base.WorkItemID;

            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {

        }

        private void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            if (this.StepName != ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE) return;
            Snd_Print print = new Snd_Print();
            string sProcName = "党纪工团发文";
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            B_DJGTSend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_DJGTSend : new B_DJGTSend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_DJGTSend;
            }
            switch (ucPrint.FileName)
            {
                case "党委部门文件模版":
                    ucPrint.WriteEx("参数 ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "党委部门文件模版", cEntity);
                    break;
                case "党委请示模版":
                    ucPrint.WriteEx("参数 "/*"海核党发[2009]1号                                签发："*/,
                        cEntity.DocumentNo + "                        签发：" + cEntity.Signer, 
                        WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "党委请示模版", cEntity);
                    break;
                case "海南党委文件模版":
                    ucPrint.WriteEx("参数 ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南党委文件模版", cEntity);
                    break;
                case "海南工会请示模版":
                    ucPrint.WriteEx("参数 ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南工会请示模版", cEntity);
                    break;
                case "海南共青团文件模版":
                    ucPrint.WriteEx("参数 ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南共青团文件模版", cEntity);
                    break;
                case "海南纪委文件模版":
                    ucPrint.WriteEx("参数 ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南纪委文件模版", cEntity);
                    break;
                case "海南工会文件模版":
                    ucPrint.WriteEx("% ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南工会文件模版", cEntity);
                    break;
                case "海南纪委请示文件模版":
                    ucPrint.WriteEx("% ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南纪委请示文件模版", cEntity);
                    break;
                case "海南共青团请示文件模版":
                    ucPrint.WriteEx("% ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "海南共青团请示文件模版", cEntity);
                    break;
                #region 会议纪要模板
                case "党群工作全例会会议纪要模版":
                    ucPrint.WriteEx("% ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "党群工作全例会会议纪要模版", cEntity);
                    break;
                case "党委会议纪要模版":
                    ucPrint.WriteEx("% ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "党委会议纪要模版", cEntity);
                    break;
                case "党政联席会纪要模版":
                    ucPrint.WriteEx("% ", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "党委会议纪要模版", cEntity);
                    break;
                case "党群简报模版":
                    ucPrint.WriteEx("第期", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.WriteEx("% ",
                        "海南核电有限公司党群工作处编制            " + DateTime.Now.ToString("yyyy年MM月dd日") + "   ", WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture(sProcName, "党群简报模版", cEntity);
                    break;
                #endregion 
                #region 公文首页纸模板
                case "党委公文首页纸模板":
                    ucPrint.BatchAddPicture("党纪工团发文", "党委公文首页纸模板", cEntity);
                    break;
                case "工会首页纸":
                    ucPrint.BatchAddPicture("党纪工团发文", "工会首页纸", cEntity);
                    break;
                case "共青团首页纸":
                    ucPrint.BatchAddPicture("党纪工团发文", "共青团首页纸", cEntity);
                    break;
                case "纪律检查委员会首页纸":
                    ucPrint.BatchAddPicture("党纪工团发文", "纪律检查委员会首页纸", cEntity);
                    break;
                #endregion
            }
        }

        public void ucPrint_OnBeforeClosed(object sender, EventArgs e)
        {
            if (this.StepName != ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE) return;
            //Snd_Print print = new Snd_Print();
            //B_DJGTSend cEntity = this.ControlToEntity(false) as B_DJGTSend;
            switch (ucPrint.FileName)
            {
                case "党委部门文件模版":
                case "党委请示模版":
                case "海南党委文件模版":
                case "海南工会请示模版":
                    ucPrint.DocLayout(1, 7, 1);
                    break;
                case "海南共青团文件模版":
                case "海南纪委文件模版":
                case "海南工会文件模版":
                case "海南纪委请示文件模版":
                    ucPrint.DocLayout(1, 8, 1);
                    break;
                case "海南共青团请示文件模版":
                    ucPrint.DocLayout(1, 9, 1);
                    break;

                case "党群工作全例会会议纪要模版":
                case "党委会议纪要模版":
                case "党政联席会纪要模版":
                case "党群简报模版":
                    ucPrint.DocLayout(1, 3, 1);
                    break;

                case "党委公文首页纸模板":
                case "工会首页纸":
                case "共青团首页纸":
                case "纪律检查委员会首页纸":
                    ucPrint.DocLayout(2, 14, 1);
                    break;
            }
        }
        #endregion
    }
}
