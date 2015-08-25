using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FounderSoftware.Framework.Business;

namespace OA.DEVOVLE.OA2DC
{
    class EntityOADevolveHistory : EntityMaster
    {
        public const string TABLE_NAME = "T_OA_Devolve";

        #region Construction

        public EntityOADevolveHistory()
            : base(EntityOADevolveHistory.TABLE_NAME)
        {
        }

        protected sealed override void InitColumnSelf()
        {
            base.InitColumn("No", "No", true);
            base.InitColumn("OAID", "OAID", true);
            base.InitColumn("FK_ArchiveId", "档案ID", true);
            base.InitColumn("DevolveDate", "归档日期", true);
            base.InitColumn("DevolveStatus", "归档状态", true);
            base.InitColumn("RejectReason", "拒绝原因", true);
            base.InitColumn("TransferXML", "传输XML", true);
            base.InitColumn("ProcessDate", "处理日期", true);
            base.InitColumn("SerialNo", "流水号", true);
            base.InitColumn("FormsData", "表单数据", true);
            base.InitColumn("ProcessName", "流程名称", true);
            base.InitColumn("Accepter", "接收人", true);
        }

        #endregion

        #region Prop
        /// <summary>
        /// No
        /// </summary>
        public string No
        {
            get { return base.GetValStr("No"); }
            set { base.SetVal("No", value); }
        }

        /// <summary>
        /// OAID
        /// </summary>
        public int OAID
        {
            get { return base.GetValInt("OAID"); }
            set { base.SetVal("OAID", value); }
        }

        /// <summary>
        /// 档案ID
        /// </summary>
        public int FK_ArchiveId
        {
            get { return base.GetValInt("FK_ArchiveId"); }
            set { base.SetVal("FK_ArchiveId", value); }
        }

        /// <summary>
        /// 归档日期
        /// </summary>
        public DateTime DevolveDate
        {
            get { return base.GetValDateTime("DevolveDate"); }
            set { base.SetVal("DevolveDate", value); }
        }

        /// <summary>
        /// 归档状态
        /// </summary>
        public int DevolveStatus
        {
            get { return base.GetValInt("DevolveStatus"); }
            set { base.SetVal("DevolveStatus", value); }
        }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RejectReason
        {
            get { return base.GetValStr("RejectReason"); }
            set { base.SetVal("RejectReason", value); }
        }

        /// <summary>
        /// 传输XML
        /// </summary>
        public string TransferXML
        {
            get { return base.GetValStr("TransferXML"); }
            set { base.SetVal("TransferXML", value); }
        }

        /// <summary>
        /// 处理日期
        /// </summary>
        public DateTime ProcessDate
        {
            get { return base.GetValDateTime("ProcessDate"); }
            set { base.SetVal("ProcessDate", value); }
        }

        /// <summary>
        /// 流水号
        /// </summary>
        public int SerialNo
        {
            get { return base.GetValInt("SerialNo"); }
            set { base.SetVal("SerialNo", value); }
        }

        /// <summary>
        /// 表单数据
        /// </summary>
        public string FormsData
        {
            get { return base.GetValStr("FormsData"); }
            set { base.SetVal("FormsData", value); }
        }

        public string ProcessName
        {
            get { return base.GetValStr("ProcessName"); }
            set { base.SetVal("ProcessName", value); }
        }

        public string Accepter
        {
            get { return base.GetValStr("Accepter"); }
            set { base.SetVal("Accepter", value); }
        }
        #endregion
    }
}
