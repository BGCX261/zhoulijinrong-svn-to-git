using System;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public partial class UC_Receive
    {
        public void InitPrint()
        {
            #region 打印委托
            this.ucPrint.OnBeginExport += new UC_Print.ExportHandler(ucPrint_OnBeginExport);
            this.ucPrint.OnCompletionExport += new UC_Print.ExportHandler(ucPrint_OnCompletionExport);
            this.ucPrint.OnAttachExport += new UC_Print.ExportHandler(ucPrint_OnAttachExport);
            this.ucPrint.OnExtraExport += new UC_Print.ExportHandler(ucPrint_OnExtraExport);
            #endregion

            #region 打印属性设置

            this.ucPrint.UCTemplateName = base.TemplateName;
            this.ucPrint.UCStepName = base.StepName;
            #endregion
        }

        #region 打印
        private void ucPrint_OnAttachExport(object sender, EventArgs e)
        {
            Recv_Print print = new Recv_Print();
            B_MergeReceiveBase cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_MergeReceiveBase : new B_MergeReceiveBase();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_MergeReceiveBase;
            }
            print.SetPrintAttachExport(ucPrint, cEntity);
        }
        private void ucPrint_OnBeginExport(object sender, EventArgs e)
        {
            Recv_Print print = new Recv_Print();
            B_MergeReceiveBase cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_MergeReceiveBase : new B_MergeReceiveBase();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_MergeReceiveBase;
            }
            print.SetPrintBeginExport(ucPrint, cEntity);
        }
        private void ucPrint_OnCompletionExport(object sender, EventArgs e)
        {

        }

        /*公文处理单中的 传阅签名 日期*/
        private void ucPrint_OnExtraExport(object sender, EventArgs e)
        {
            Recv_Print print = new Recv_Print();
            B_MergeReceiveBase cEntity = null;
            if (base.IsPreview)
            {
                cEntity = base.EntityData != null ? base.EntityData as B_MergeReceiveBase : new B_MergeReceiveBase();
            }
            else
            {
                cEntity = this.ControlToEntity(false) as B_MergeReceiveBase;
            }
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
                case "党纪工团收文表单":
                    ucPrint.BatchAddPicture(base.TemplateName, "党纪工团收文表单", cEntity);
                    break;
            }
        }
        #endregion
    }
}
