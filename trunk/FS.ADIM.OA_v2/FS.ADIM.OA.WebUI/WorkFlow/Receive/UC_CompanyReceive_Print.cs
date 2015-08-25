using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public partial class UC_CompanyReceive
    {
        private UC_Print m_print = null;
        private B_GS_WorkItems m_Entity = null;

        public void InitPrint()
        {
            #region 打印委托
            this.ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            this.ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            this.ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            this.ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            #endregion

            #region 打印属性设置
            //打印
            this.ucPrint.UCTemplateName = "公司收文"; //ConstString.ProcessTemplate.COMPANY_RECEIVE;
            this.ucPrint.UCStepName = this.StepName;   //base.ViewIDorName;
            #endregion
        }

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            ComRecv_Print print = new ComRecv_Print();
            //B_GS_WorkItems cEntity = new B_GS_WorkItems();
            //this.PopulateEntity(cEntity, base.ViewIDorName, base.WorkItemID);
            //SetEntity(cEntity);
            B_GS_WorkItems cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_GS_WorkItems : new B_GS_WorkItems();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_GS_WorkItems;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            ComRecv_Print print = new ComRecv_Print();
            //B_GS_WorkItems cEntity = new B_GS_WorkItems();
            //this.PopulateEntity(cEntity, base.ViewIDorName, base.WorkItemID);
            print.sDept = this.txtUnderTakeDeptName.Text;
            //SetEntity(cEntity);
            B_GS_WorkItems cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_GS_WorkItems : new B_GS_WorkItems();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_GS_WorkItems;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {

        }
        /*待定*/
        /*公文处理单中的 传阅签名 日期*/
        private void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            // 当模板中无法用之前的方法正确导出数据时,在本事件中调用
            // ucPrint.Write(string key, string value, WriteMode mode, int offset)方法处理导出数据

            ComRecv_Print print = new ComRecv_Print();
            //B_GS_WorkItems cEntity = new B_GS_WorkItems();
            //this.PopulateEntity(cEntity, base.ViewIDorName, base.WorkItemID);
            B_GS_WorkItems cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_GS_WorkItems : new B_GS_WorkItems();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_GS_WorkItems;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
            for (int i = 0; i < cEntity.CirculateList.Rows.Count; i++)
            {
                if (i + 1 < 14)
                {
                    ucPrint.Write("传阅签名", cEntity.CirculateList.Rows[i]["ReceiveUserName"].ToString(), WordMgr.WriteMode.Right, i + 1);
                    ucPrint.Write("日期", cEntity.CirculateList.Rows[i]["EditDate"].ToString(), WordMgr.WriteMode.Right, i + 1);
                }
                else
                {
                    ucPrint.Write("传阅签名 ", cEntity.CirculateList.Rows[i]["ReceiveUserName"].ToString(), WordMgr.WriteMode.Right, i - 12);
                    DateTime oDt = new DateTime();
                    try
                    {
                        oDt = Convert.ToDateTime(cEntity.CirculateList.Rows[i]["EditDate"].ToString());
                    }
                    catch
                    {
                        return;
                    }
                    ucPrint.Write("日期 ", ucPrint.CheckDateTime(oDt.ToShortDateString()), WordMgr.WriteMode.Right, i - 12);
                }
            }

            switch (ucPrint.FileName)
            {
                case "公文处理单":
                    ucPrint.BatchAddPicture("公司收文", "公文处理单", cEntity);
                    break;
                case "公司收文表单":
                    ucPrint.BatchAddPicture("公司收文", "公司收文表单", cEntity);
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
            ComRecv_Print print = new ComRecv_Print();

            //print.m_ProcessID = base.ProcessID;
            //print.m_TemplateID = base.TemplateName; //base.TemplateID;
            m_Entity = m_print.m_CurrEntity as B_GS_WorkItems;
            print.SetPrintBeginExport(m_print, m_Entity);
        }

        private void Print_OnCompletionExport(object sender, EventArgs e)
        {

        }
        private void Print_OnAttachExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            ComRecv_Print print = new ComRecv_Print();

            print.SetPrintAttachExport(m_print, m_Entity);
        }
        private void Print_OnExtraExport(object sender, EventArgs e)
        {
            if (m_print == null || m_Entity == null) return;
            ComRecv_Print print = new ComRecv_Print();
            for (int i = 0; i < m_Entity.CirculateList.Rows.Count; i++)
            {
                m_print.Write("传阅签名", m_Entity.CirculateList.Rows[i]["ReceiveUserName"].ToString(), WordMgr.WriteMode.Right, i + 1);
                m_print.Write("日期", m_Entity.CirculateList.Rows[i]["EditDate"].ToString(), WordMgr.WriteMode.Right, i + 1);
                if (i + 1 > 13)
                {
                    m_print.Write("传阅签名 ", m_Entity.CirculateList.Rows[i]["ReceiveUserName"].ToString(), WordMgr.WriteMode.Right, i + 1);
                    DateTime oDt = new DateTime();
                    try
                    {
                        oDt = Convert.ToDateTime(m_Entity.CirculateList.Rows[i]["EditDate"].ToString());
                    }
                    catch
                    {
                        return;
                    }
                    m_print.Write("日期 ", m_print.CheckDateTime(oDt.ToShortDateString()), WordMgr.WriteMode.Right, i + 1);
                }
            }

            switch (m_print.FileName)
            {
                case "公文处理单":
                    m_print.BatchAddPicture("公司收文", "公文处理单", m_Entity);
                    break;
                case "公司收文表单":
                    m_print.BatchAddPicture("公司收文", "公司收文表单", m_Entity);
                    break;
            }
        }
        #endregion #region 批量打印接口
    }
}
