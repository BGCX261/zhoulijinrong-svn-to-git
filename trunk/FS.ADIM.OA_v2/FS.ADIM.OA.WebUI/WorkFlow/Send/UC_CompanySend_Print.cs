using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.Send
{
    public partial class UC_CompanySend
    {
        private UC_Print m_print = null;
        private EntitySend m_Entity = null;

        public void InitPrint()
        {
            ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            ucPrint.OnBeforeClosed += new UC_Print.ExportHandler(ucPrint_OnBeforeClosed);

            //打印
            ucPrint.UCTemplateName = "公司发文";
            ucPrint.UCStepName = this.StepName;//base.ViewIDorName;
        }
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
            ComSnd_Print print = new ComSnd_Print();
            
            print.m_ProcessID = base.ProcessID;
            print.m_TemplateID = base.TemplateName; //base.TemplateID;
            print.m_WorkItemID = base.WorkItemID;
            m_Entity = m_print.m_CurrEntity as EntitySend;
            print.SetPrintBeginExport(m_print, m_Entity);
        }

        private void Print_OnCompletionExport(object sender, EventArgs e)
        {

        }
        private void Print_OnAttachExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            ComSnd_Print print = new ComSnd_Print();
            
            print.SetPrintAttachExport(m_print, m_Entity);
        }
        private void Print_OnExtraExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            ComSnd_Print print = new ComSnd_Print();
            
            switch (m_print.FileName)
            {
                case "工程会议纪要":
                    m_print.WriteEx("第期", m_Entity.DocumentNo, WordMgr.WriteMode.Shift);
                    m_print.WriteEx("参数",
                                    "海南核电有限公司                         " + DateTime.Now.ToString("yyyy年MM月dd日") + "  ",
                                    WordMgr.WriteMode.Inner);
                    m_print.BatchAddPicture("公司发文", "工程会议纪要", m_Entity);
                    break;
                case "公文报告模版":
                    m_print.WriteEx("参数", "  " + m_Entity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    m_print.BatchAddPicture("公司发文", "公文报告模版", m_Entity);
                    break;
                case "公文函模版":
                    m_print.WriteEx("参数", "  " + m_Entity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    m_print.BatchAddPicture("公司发文", "公文函模版", m_Entity);
                    break;
                case "公文请示模版":
                    m_print.WriteEx("参数",
                                     m_Entity.DocumentNo + "                      " + "签发人：" + m_Entity.SignerName + "  ",
                                     WordMgr.WriteMode.Shift);
                    m_print.BatchAddPicture("公司发文", "公文请示模版", m_Entity);
                    break;
                case "公文首页纸":
                    // TODO:签名功能使用前需要配置 \Template\SingerCfg.xml文件
                    m_print.BatchAddPicture("公司发文", "公文首页纸", m_Entity);
                    break;
                case "公文通知模版":
                    m_print.WriteEx("参数", "  " + m_Entity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    m_print.BatchAddPicture("公司发文", "公文通知模版", m_Entity);
                    break;
                case "会议纪要模版":

                    break;
                case "简报模版":
                    m_print.WriteEx("第期", m_Entity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    m_print.WriteEx("参数",
                                    "海南核电有限公司党群工作处编制         " + DateTime.Now.ToString("yyyy年MM月dd日") + "  ",
                                    WordMgr.WriteMode.Shift);
                    m_print.BatchAddPicture("公司发文", "简报模版", m_Entity);
                    break;
                case "简讯模版":
                    m_print.WriteEx("第期", m_Entity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    m_print.WriteEx("参数",
                                    "海南核电有限公司办公室编               " + DateTime.Now.ToString("yyyy年MM月dd日") + "  ",
                                    WordMgr.WriteMode.Shift);
                    m_print.BatchAddPicture("公司发文", "简讯模版", m_Entity);
                    break;
            }
        }
        private void Print_OnBeforeClosed(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            ComSnd_Print print = new ComSnd_Print();
           
            switch (m_print.FileName)
            {
                case "工程会议纪要":
                    m_print.DocLayout(1, 6, 1);
                    break;
                case "公文报告模版":
                    m_print.DocLayout(1, 7, 1);
                    break;
                case "公文函模版":
                    m_print.DocLayout(1, 7, 1);
                    break;
                case "公文请示模版":
                    m_print.DocLayout(1, 7, 1);
                    break;
                case "公文首页纸":
                    m_print.DocLayout(2, 12, 1);
                    break;
                case "公文通知模版":
                    ucPrint.DocLayout(1, 7, 1);
                    break;
                case "会议纪要模版":
                    m_print.DocLayout(1, 9, 1);
                    break;
                case "简报模版":
                    m_print.DocLayout(1, 4, 1);
                    break;
                case "简讯模版":
                    m_print.DocLayout(1, 6, 1);
                    break;
            }
        }
        #endregion #region 批量打印接口

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            ComSnd_Print print = new ComSnd_Print();
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            EntitySend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntitySend;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            ComSnd_Print print = new ComSnd_Print();
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            EntitySend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntitySend;
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
            ComSnd_Print print = new ComSnd_Print();
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            EntitySend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntitySend;
            }
            switch (ucPrint.FileName)
            {
                case "工程会议纪要":
                    ucPrint.WriteEx("第期", cEntity.DocumentNo, WordMgr.WriteMode.Shift);
                    ucPrint.WriteEx("参数",
                                    "海南核电有限公司                         " + DateTime.Now.ToString("yyyy年MM月dd日") + "  ",
                                    WordMgr.WriteMode.Inner);
                    ucPrint.BatchAddPicture("公司发文", "工程会议纪要", cEntity);
                    break;
                case "公文报告模版":
                    ucPrint.WriteEx("参数", "  " + cEntity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture("公司发文", "公文报告模版", cEntity);
                    break;
                case "公文函模版":
                    ucPrint.WriteEx("参数", "  " + cEntity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture("公司发文", "公文函模版", cEntity);
                    break;
                case "公文请示模版":
                    ucPrint.WriteEx("参数",
                                     cEntity.DocumentNo + "                      " + "签发人：" + cEntity.SignerName + "  ",
                                     WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture("公司发文", "公文请示模版", cEntity);
                    break;
                case "公文首页纸":
                    // TODO:签名功能使用前需要配置 \Template\SingerCfg.xml文件
                    ucPrint.BatchAddPicture("公司发文", "公文首页纸", cEntity);
                    break;
                case "公文通知模版":
                    ucPrint.WriteEx("参数", "  " + cEntity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture("公司发文", "公文通知模版", cEntity);
                    break;
                case "会议纪要模版":
                    ucPrint.BatchAddPicture("公司发文", "会议纪要模版", cEntity);
                    break;
                case "简报模版":
                    ucPrint.WriteEx("第期", cEntity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    ucPrint.WriteEx("参数",
                                    "海南核电有限公司党群工作处编制         " + DateTime.Now.ToString("yyyy年MM月dd日") + "  ",
                                    WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture("公司发文", "简报模版", cEntity);
                    break;
                case "简讯模版":
                    ucPrint.WriteEx("第期", cEntity.DocumentNo + "  ", WordMgr.WriteMode.Shift);
                    ucPrint.WriteEx("参数",
                                    "海南核电有限公司办公室编               " + DateTime.Now.ToString("yyyy年MM月dd日") + "  ",
                                    WordMgr.WriteMode.Shift);
                    ucPrint.BatchAddPicture("公司发文", "简讯模版", cEntity);
                    break;
            }
        }

        public void ucPrint_OnBeforeClosed(object sender, EventArgs e)
        {
            if (this.StepName != ProcessConstString.StepName.SendStepName.STEP_DISTRIBUTE) return;
            ComSnd_Print print = new ComSnd_Print();
            //EntitySend cEntity = new EntitySend();
            //SetEntity(cEntity);
            EntitySend cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as EntitySend : new EntitySend();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as EntitySend;
            }
            switch (ucPrint.FileName)
            {
                case "工程会议纪要":
                    ucPrint.DocLayout(1, 6, 1);
                    break;
                case "公文报告模版":
                    ucPrint.DocLayout(1, 7, 1);
                    break;
                case "公文函模版":
                    ucPrint.DocLayout(1, 7, 1);
                    break;
                case "公文请示模版":
                    ucPrint.DocLayout(1, 7, 1);
                    break;
                case "公文首页纸":
                    ucPrint.DocLayout(2, 12, 1);
                    break;
                case "公文通知模版":
                    ucPrint.DocLayout(1, 7, 1);
                    break;
                case "会议纪要模版":
                    ucPrint.DocLayout(1, 9, 1);
                    break;
                case "简报模版":
                    ucPrint.DocLayout(1, 4, 1);
                    break;
                case "简讯模版":
                    ucPrint.DocLayout(1, 6, 1);
                    break;
            }
        }
        #endregion
    }
}
