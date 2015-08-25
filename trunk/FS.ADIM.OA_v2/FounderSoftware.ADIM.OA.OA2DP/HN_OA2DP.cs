using System;
using System.Collections.Generic;
using System.Xml;
using FS.ADIM.OA.BLL.Entity;
using Dicts = System.Collections.Generic.Dictionary<string, int>;
using FS.ADIM.OU.OutBLL;

namespace FounderSoftware.ADIM.OA.OA2DP
{
    public class HNDP_CArchiveNode
    {
        public string System;
        public string CallBackWebServiceUrl;
        public string CallBackWebServiceMethod;
        public string IsDeleteAfterProcess;
        public string IsDownLoadAttachment;
        public string DeptName = "";

        #region 属性
        public string FK_CategoryID = "";
        public string DCTableName = "";
        public string FK_DC_ArchiveId = "";
        public string FK_DPID = "";
        public string Is_Piece = "";
        public string FormationDept = "";
        public string FormationTime = "";
        public string Authorizer = "";
        public string AuthorizeTime = "";
        public string ReceiveCode = "";
        public string PaperDocumentTransceiverTime = "";
        public string ElectronicDocumentTransceiverTime = "";
        public string CommunicationChannelCode = "";
        public string Importer = "";
        public string ImporterTime = "";
        public string MainDispenseUnit = "";
        public string D_FillingUnit = "";
        public string FillingTime = "";
        public string D_DocStorageLife = "";
        public string D_SecretLevel = "";
        public string DocStorageLife = "";
        public string InvalidTime = "";
        public string D_InvalidType = "";
        public string Code = "";
        public string Code19 = "";
        public string RelatedCode = "";
        public string Revision = "";
        public string DocCode = "";
        public string D_StorageCarrierType = "";
        public string FrameCode = "";
        public string D_DocStandards = "";
        public string DocPages = "";
        public string Amount = "";
        public string Title = "";
        public string OtherTitle = "";
        public string SubTitle = "";
        public string KeyWords = "";
        public string ThemeWord = "";
        public string Remark = "";
        public string CoverRange = "";
        public string SystemCode = "";
        public string D_Reactor = "";
        public string D_EquipmentRelation = "";
        public string OverhaulCode = "";
        public string NucleusCode = "";
        public string FunctionField = "";
        public string StaffCode = "";
        public string RecorderCode = "";
        public string Profession = "";
        public string GatherLevel = "";
        public string ElectronicDocumentCount = "";
        public string PictureCode = "";
        public string D_Language = "";
        public string QualitySafeLevel = "";
        public string CDNumber = "";
        public string D_FileStatus = "";
        public string DevolveStatus = "";
        public string Is_Lend = "";
        public string Is_Dispose = "";
        public string OriginalID = "";
        public string D_AuditStatus = "";
        public string OriginalInfoURL = "";
        public string Is_Transfer = "";
        public string Ext_2 = "";
        public string Ext_3 = "";
        public string Ext_4 = "";
        public string Ext_5 = "";

        public string FK_ArchiveID = "";
        public string Author = "";
        public string DigitalResourceFormationDept = "";
        public string DigitalResourceFormationTime = "";
        public string D_ReceiveType = "";
        public string ReceiveTime = "";
        public string OriginalInfomationSystem = "";
        public string EffectTime = "";
        public string UndertakeDepartment = "";
        public string RelatedDespenseUnit = "";
        public string FormMaker = "";
        public string D_DisposeStatus = "";
        public string DocCodesExplain = "";
        public string Summary = "";
        public string DocmentType = "";
        public string ExpertOpinion = "";
        public string D_ProcessStatus = "";
        public string ArchiveName = "";
        public string FondNumber = "";
        public string FondName = "";
        public string PlantCode = "";
        public string PlantName = "";
        public string Checkthose = "";
        public string CheckDate = "";
        public string Auditby = "";
        public string AuditDate = "";
        public string Countersigner = "";
        public string CountersignedDate = "";
        public string Proposer = "";
        public string ProposeDate = "";
        public string Pace = "";
        public string Instructioner = "";
        public string InstructionDate = "";
        public string MainDeliveryDate = "";
        public string DistributionCode = "";
        public string DistributionUnits = "";
        public string DistributionForm = "";
        public string DistributeCopies = "";
        public string DistributionTime = "";
        public string IdentificationDate = "";
        public string Identifier = "";
        public string AuthorizationObject = "";
        public string AuthorizationAct = "";
        public string AuthorizedStartTime = "";
        public string LicenseExpiryDate = "";
        public string Changer = "";
        public string Modified = "";
        public string Modifications = "";
        public string EditDescription = "";
        public string Disposaler = "";
        public string DisposalDate = "";
        public string DisposalType = "";
        public string DisposalResults = "";
        public string DestructionApproval = "";
        public string Destroyer = "";
        public string DestructionDate = "";
        public string DestructionReason = "";
        public string Transfer = "";
        public string TransferTime = "";
        public string Takeover = "";
        public string User = "";
        public string UseDate = "";
        public string Usetype = "";
        public string Physicallocation = "";
        public string StorageCarrierLogo = "";
        public string RelevanceItemID = "";
        public string AssociationType = "";
        public string Attachment = "";
        #endregion

        private string[] _ls = new string[136];
        Dicts lsMap = new Dictionary<string, int>();

        private void initMap(string sAttr, int index)
        {
            lsMap.Add(sAttr, index);
        }

        #region 构造函数
        public HNDP_CArchiveNode()
        {
            _ls.SetValue(this.FK_CategoryID.Clone(), 0);
            initMap("FK_CategoryID", 0);
            _ls.SetValue(this.DCTableName.Clone(), 1);
            initMap("DCTableName", 1);
            _ls.SetValue(this.FK_DC_ArchiveId.Clone(), 2);
            initMap("FK_DC_ArchiveId", 2);
            _ls.SetValue(this.FK_DPID.Clone(), 3);
            initMap("FK_DPID", 3);
            _ls.SetValue(this.Is_Piece.Clone(), 4);
            initMap("Is_Piece", 4);
            _ls.SetValue(this.FormationDept.Clone(), 5);
            initMap("FormationDept", 5);
            _ls.SetValue(this.FormationTime.Clone(), 6);
            initMap("FormationTime", 6);
            _ls.SetValue(this.Authorizer.Clone(), 7);
            initMap("Authorizer", 7);
            _ls.SetValue(this.AuthorizeTime.Clone(), 8);
            initMap("AuthorizeTime", 8);
            _ls.SetValue(this.ReceiveCode.Clone(), 9);
            initMap("ReceiveCode", 9);
            _ls.SetValue(this.PaperDocumentTransceiverTime.Clone(), 10);
            initMap("PaperDocumentTransceiverTime", 10);
            _ls.SetValue(this.ElectronicDocumentTransceiverTime.Clone(), 11);
            initMap("ElectronicDocumentTransceiverTime", 11);
            _ls.SetValue(this.CommunicationChannelCode.Clone(), 12);
            initMap("CommunicationChannelCode", 12);
            _ls.SetValue(this.Importer.Clone(), 13);
            initMap("Importer", 13);
            _ls.SetValue(this.ImporterTime.Clone(), 14);
            initMap("ImporterTime", 14);
            _ls.SetValue(this.MainDispenseUnit.Clone(), 15);
            initMap("MainDispenseUnit", 15);
            _ls.SetValue(this.D_FillingUnit.Clone(), 16);
            initMap("D_FillingUnit", 16);
            _ls.SetValue(this.FillingTime.Clone(), 17);
            initMap("FillingTime", 17);
            _ls.SetValue(this.D_DocStorageLife.Clone(), 18);
            initMap("D_DocStorageLife", 18);
            _ls.SetValue(this.D_SecretLevel.Clone(), 19);
            initMap("D_SecretLevel", 19);
            _ls.SetValue(this.DocStorageLife.Clone(), 20);
            initMap("DocStorageLife", 20);
            _ls.SetValue(this.InvalidTime.Clone(), 21);
            initMap("InvalidTime", 21);
            _ls.SetValue(this.D_InvalidType.Clone(), 22);
            initMap("D_InvalidType", 22);
            _ls.SetValue(this.Code.Clone(), 23);
            initMap("Code", 23);
            _ls.SetValue(this.Code19.Clone(), 24);
            initMap("Code19", 24);
            _ls.SetValue(this.RelatedCode.Clone(), 25);
            initMap("RelatedCode", 25);
            _ls.SetValue(this.Revision.Clone(), 26);
            initMap("Revision", 26);
            _ls.SetValue(this.DocCode.Clone(), 27);
            initMap("DocCode", 27);
            _ls.SetValue(this.D_StorageCarrierType.Clone(), 28);
            initMap("D_StorageCarrierType", 28);
            _ls.SetValue(this.FrameCode.Clone(), 29);
            initMap("FrameCode", 29);
            _ls.SetValue(this.D_DocStandards.Clone(), 30);
            initMap("D_DocStandards", 30);
            _ls.SetValue(this.DocPages.Clone(), 31);
            initMap("DocPages", 31);
            _ls.SetValue(this.Amount.Clone(), 32);
            initMap("Amount", 32);
            _ls.SetValue(this.Title.Clone(), 33);
            initMap("Title", 33);
            _ls.SetValue(this.OtherTitle.Clone(), 34);
            initMap("OtherTitle", 34);
            _ls.SetValue(this.SubTitle.Clone(), 35);
            initMap("SubTitle", 35);
            _ls.SetValue(this.KeyWords.Clone(), 36);
            initMap("KeyWords", 36);
            _ls.SetValue(this.ThemeWord.Clone(), 37);
            initMap("ThemeWord", 37);
            _ls.SetValue(this.Remark.Clone(), 38);
            initMap("Remark", 38);
            _ls.SetValue(this.CoverRange.Clone(), 39);
            initMap("CoverRange", 39);
            _ls.SetValue(this.SystemCode.Clone(), 40);
            initMap("SystemCode", 40);
            _ls.SetValue(this.D_Reactor.Clone(), 41);
            initMap("D_Reactor", 41);
            _ls.SetValue(this.D_EquipmentRelation.Clone(), 42);
            initMap("D_EquipmentRelation", 42);
            _ls.SetValue(this.OverhaulCode.Clone(), 43);
            initMap("OverhaulCode", 43);
            _ls.SetValue(this.NucleusCode.Clone(), 44);
            initMap("NucleusCode", 44);
            _ls.SetValue(this.FunctionField.Clone(), 45);
            initMap("FunctionField", 45);
            _ls.SetValue(this.StaffCode.Clone(), 46);
            initMap("StaffCode", 46);
            _ls.SetValue(this.RecorderCode.Clone(), 47);
            initMap("RecorderCode", 47);
            _ls.SetValue(this.Profession.Clone(), 48);
            initMap("Profession", 48);
            _ls.SetValue(this.GatherLevel.Clone(), 49);
            initMap("GatherLevel", 49);
            _ls.SetValue(this.ElectronicDocumentCount.Clone(), 50);
            initMap("ElectronicDocumentCount", 50);
            _ls.SetValue(this.PictureCode.Clone(), 51);
            initMap("PictureCode", 51);
            _ls.SetValue(this.D_Language.Clone(), 52);
            initMap("D_Language", 52);
            _ls.SetValue(this.QualitySafeLevel.Clone(), 53);
            initMap("QualitySafeLevel", 53);
            _ls.SetValue(this.CDNumber.Clone(), 54);
            initMap("CDNumber", 54);
            _ls.SetValue(this.D_FileStatus.Clone(), 55);
            initMap("D_FileStatus", 55);
            _ls.SetValue(this.DevolveStatus.Clone(), 56);
            initMap("DevolveStatus", 56);
            _ls.SetValue(this.Is_Lend.Clone(), 57);
            initMap("Is_Lend", 57);
            _ls.SetValue(this.Is_Dispose.Clone(), 58);
            initMap("Is_Dispose", 58);
            _ls.SetValue(this.OriginalID.Clone(), 59);
            initMap("OriginalID", 59);
            _ls.SetValue(this.D_AuditStatus.Clone(), 60);
            initMap("D_AuditStatus", 60);
            _ls.SetValue(this.OriginalInfoURL.Clone(), 61);
            initMap("OriginalInfoURL", 61);
            _ls.SetValue(this.Is_Transfer.Clone(), 62);
            initMap("Is_Transfer", 62);
            _ls.SetValue(this.Ext_2.Clone(), 63);
            initMap("Ext_2", 63);
            _ls.SetValue(this.Ext_3.Clone(), 64);
            initMap("Ext_3", 64);
            _ls.SetValue(this.Ext_4.Clone(), 65);
            initMap("Ext_4", 65);
            _ls.SetValue(this.Ext_5.Clone(), 66);
            initMap("Ext_5", 66);
            _ls.SetValue(this.FK_ArchiveID.Clone(), 67);
            initMap("FK_ArchiveID", 67);
            _ls.SetValue(this.Author.Clone(), 68);
            initMap("Author", 68);
            _ls.SetValue(this.DigitalResourceFormationDept.Clone(), 69);
            initMap("DigitalResourceFormationDept", 69);
            _ls.SetValue(this.DigitalResourceFormationTime.Clone(), 70);
            initMap("DigitalResourceFormationTime", 70);
            _ls.SetValue(this.D_ReceiveType.Clone(), 71);
            initMap("D_ReceiveType", 71);
            _ls.SetValue(this.ReceiveTime.Clone(), 72);
            initMap("ReceiveTime", 72);
            _ls.SetValue(this.OriginalInfomationSystem.Clone(), 73);
            initMap("OriginalInfomationSystem", 73);
            _ls.SetValue(this.EffectTime.Clone(), 74);
            initMap("EffectTime", 74);
            _ls.SetValue(this.UndertakeDepartment.Clone(), 75);
            initMap("UndertakeDepartment", 75);
            _ls.SetValue(this.RelatedDespenseUnit.Clone(), 76);
            initMap("RelatedDespenseUnit", 76);
            _ls.SetValue(this.FormMaker.Clone(), 77);
            initMap("FormMaker", 77);
            _ls.SetValue(this.D_DisposeStatus.Clone(), 78);
            initMap("D_DisposeStatus", 78);
            _ls.SetValue(this.DocCodesExplain.Clone(), 79);
            initMap("DocCodesExplain", 79);
            _ls.SetValue(this.Summary.Clone(), 80);
            initMap("Summary", 80);
            _ls.SetValue(this.DocmentType.Clone(), 81);
            initMap("DocmentType", 81);
            _ls.SetValue(this.ExpertOpinion.Clone(), 82);
            initMap("ExpertOpinion", 82);
            _ls.SetValue(this.D_ProcessStatus.Clone(), 83);
            initMap("D_ProcessStatus", 83);
            _ls.SetValue(this.ArchiveName.Clone(), 84);
            initMap("ArchiveName", 84);
            _ls.SetValue(this.FondNumber.Clone(), 85);
            initMap("FondNumber", 85);
            _ls.SetValue(this.FondName.Clone(), 86);
            initMap("FondName", 86);
            _ls.SetValue(this.PlantCode.Clone(), 87);
            initMap("PlantCode", 87);
            _ls.SetValue(this.PlantName.Clone(), 88);
            initMap("PlantName", 88);
            _ls.SetValue(this.Checkthose.Clone(), 89);
            initMap("Checkthose", 89);
            _ls.SetValue(this.CheckDate.Clone(), 90);
            initMap("CheckDate", 90);
            _ls.SetValue(this.Auditby.Clone(), 91);
            initMap("Auditby", 91);
            _ls.SetValue(this.AuditDate.Clone(), 92);
            initMap("AuditDate", 92);
            _ls.SetValue(this.Countersigner.Clone(), 93);
            initMap("Countersigner", 93);
            _ls.SetValue(this.CountersignedDate.Clone(), 94);
            initMap("CountersignedDate", 94);
            _ls.SetValue(this.Proposer.Clone(), 95);
            initMap("Proposer", 95);
            _ls.SetValue(this.ProposeDate.Clone(), 96);
            initMap("ProposeDate", 96);
            _ls.SetValue(this.Pace.Clone(), 97);
            initMap("Pace", 97);
            _ls.SetValue(this.Instructioner.Clone(), 98);
            initMap("Instructioner", 98);
            _ls.SetValue(this.InstructionDate.Clone(), 99);
            initMap("InstructionDate", 99);
            _ls.SetValue(this.MainDeliveryDate.Clone(), 100);
            initMap("MainDeliveryDate", 100);
            _ls.SetValue(this.DistributionCode.Clone(), 101);
            initMap("DistributionCode", 101);
            _ls.SetValue(this.DistributionUnits.Clone(), 102);
            initMap("DistributionUnits", 102);
            _ls.SetValue(this.DistributionForm.Clone(), 103);
            initMap("DistributionForm", 103);
            _ls.SetValue(this.DistributeCopies.Clone(), 104);
            initMap("DistributeCopies", 104);
            _ls.SetValue(this.DistributionTime.Clone(), 105);
            initMap("DistributionTime", 105);
            _ls.SetValue(this.IdentificationDate.Clone(), 106);
            initMap("IdentificationDate", 106);
            _ls.SetValue(this.Identifier.Clone(), 107);
            initMap("Identifier", 107);
            _ls.SetValue(this.AuthorizationObject.Clone(), 108);
            initMap("AuthorizationObject", 108);
            _ls.SetValue(this.AuthorizationAct.Clone(), 109);
            initMap("AuthorizationAct", 109);
            _ls.SetValue(this.AuthorizedStartTime.Clone(), 110);
            initMap("AuthorizedStartTime", 110);
            _ls.SetValue(this.LicenseExpiryDate.Clone(), 111);
            initMap("LicenseExpiryDate", 111);
            _ls.SetValue(this.Changer.Clone(), 112);
            initMap("Changer", 112);
            _ls.SetValue(this.Modified.Clone(), 113);
            initMap("Modified", 113);
            _ls.SetValue(this.Modifications.Clone(), 114);
            initMap("Modifications", 114);
            _ls.SetValue(this.EditDescription.Clone(), 115);
            initMap("EditDescription", 115);
            _ls.SetValue(this.Disposaler.Clone(), 116);
            initMap("Disposaler", 116);
            _ls.SetValue(this.DisposalDate.Clone(), 117);
            initMap("DisposalDate", 117);
            _ls.SetValue(this.DisposalType.Clone(), 118);
            initMap("DisposalType", 118);
            _ls.SetValue(this.DisposalResults.Clone(), 119);
            initMap("DisposalResults", 119);
            _ls.SetValue(this.DestructionApproval.Clone(), 120);
            initMap("DestructionApproval", 120);
            _ls.SetValue(this.Destroyer.Clone(), 121);
            initMap("Destroyer", 121);
            _ls.SetValue(this.DestructionDate.Clone(), 122);
            initMap("DestructionDate", 122);
            _ls.SetValue(this.DestructionReason.Clone(), 123);
            initMap("DestructionReason", 123);
            _ls.SetValue(this.Transfer.Clone(), 124);
            initMap("Transfer", 124);
            _ls.SetValue(this.TransferTime.Clone(), 125);
            initMap("TransferTime", 125);
            _ls.SetValue(this.Takeover.Clone(), 126);
            initMap("Takeover", 126);
            _ls.SetValue(this.User.Clone(), 127);
            initMap("User", 127);
            _ls.SetValue(this.UseDate.Clone(), 128);
            initMap("UseDate", 128);
            _ls.SetValue(this.Usetype.Clone(), 129);
            initMap("Usetype", 129);
            _ls.SetValue(this.Physicallocation.Clone(), 130);
            initMap("Physicallocation", 130);
            _ls.SetValue(this.StorageCarrierLogo.Clone(), 131);
            initMap("StorageCarrierLogo", 131);
            _ls.SetValue(this.RelevanceItemID.Clone(), 132);
            initMap("RelevanceItemID", 132);
            _ls.SetValue(this.AssociationType.Clone(), 133);
            initMap("AssociationType", 133);
            _ls.SetValue(this.Attachment.Clone(), 134);
            initMap("Attachment", 134);

            _ls.SetValue(this.DeptName.Clone(), 135);
            initMap("DeptName", 135);
        }
        #endregion

        /// <summary>
        /// _ls中的值赋值给HNDC_CArchiveNode类中对应的属性
        /// </summary>
        public void SetOAArchiveNode()
        {
            this.FK_CategoryID = _ls[0];
            this.DCTableName = _ls[1];
            this.FK_DC_ArchiveId = _ls[2];
            this.FK_DPID = _ls[3];
            this.Is_Piece = _ls[4];
            this.FormationDept = _ls[5];
            this.FormationTime = _ls[6];
            this.Authorizer = _ls[7];
            this.AuthorizeTime = _ls[8];
            this.ReceiveCode = _ls[9];
            this.PaperDocumentTransceiverTime = _ls[10];
            this.ElectronicDocumentTransceiverTime = _ls[11];
            this.CommunicationChannelCode = _ls[12];
            this.Importer = _ls[13];
            this.ImporterTime = _ls[14];
            this.MainDispenseUnit = _ls[15];
            this.D_FillingUnit = _ls[16];
            this.FillingTime = _ls[17];
            this.D_DocStorageLife = _ls[18];
            this.D_SecretLevel = _ls[19];
            this.DocStorageLife = _ls[20];
            this.InvalidTime = _ls[21];
            this.D_InvalidType = _ls[22];
            this.Code = _ls[23];
            this.Code19 = _ls[24];
            this.RelatedCode = _ls[25];
            this.Revision = _ls[26];
            this.DocCode = _ls[27];
            this.D_StorageCarrierType = _ls[28];
            this.FrameCode = _ls[29];
            this.D_DocStandards = _ls[30];
            this.DocPages = _ls[31];
            this.Amount = _ls[32];
            this.Title = _ls[33];
            this.OtherTitle = _ls[34];
            this.SubTitle = _ls[35];
            this.KeyWords = _ls[36];
            this.ThemeWord = _ls[37];
            this.Remark = _ls[38];
            this.CoverRange = _ls[39];
            this.SystemCode = _ls[40];
            this.D_Reactor = _ls[41];
            this.D_EquipmentRelation = _ls[42];
            this.OverhaulCode = _ls[43];
            this.NucleusCode = _ls[44];
            this.FunctionField = _ls[45];
            this.StaffCode = _ls[46];
            this.RecorderCode = _ls[47];
            this.Profession = _ls[48];
            this.GatherLevel = _ls[49];
            this.ElectronicDocumentCount = _ls[50];
            this.PictureCode = _ls[51];
            this.D_Language = _ls[52];
            this.QualitySafeLevel = _ls[53];
            this.CDNumber = _ls[54];
            this.D_FileStatus = _ls[55];
            this.DevolveStatus = _ls[56];
            this.Is_Lend = _ls[57];
            this.Is_Dispose = _ls[58];
            this.OriginalID = _ls[59];
            this.D_AuditStatus = _ls[60];
            this.OriginalInfoURL = _ls[61];
            this.Is_Transfer = _ls[62];
            this.Ext_2 = _ls[63];
            this.Ext_3 = _ls[64];
            this.Ext_4 = _ls[65];
            this.Ext_5 = _ls[66];
            this.FK_ArchiveID = _ls[67];
            this.Author = _ls[68];
            this.DigitalResourceFormationDept = _ls[69];
            this.DigitalResourceFormationTime = _ls[70];
            this.D_ReceiveType = _ls[71];
            this.ReceiveTime = _ls[72];
            this.OriginalInfomationSystem = _ls[73];
            this.EffectTime = _ls[74];
            this.UndertakeDepartment = _ls[75];
            this.RelatedDespenseUnit = _ls[76];
            this.FormMaker = _ls[77];
            this.D_DisposeStatus = _ls[78];
            this.DocCodesExplain = _ls[79];
            this.Summary = _ls[80];
            this.DocmentType = _ls[81];
            this.ExpertOpinion = _ls[82];
            this.D_ProcessStatus = _ls[83];
            this.ArchiveName = _ls[84];
            this.FondNumber = _ls[85];
            this.FondName = _ls[86];
            this.PlantCode = _ls[87];
            this.PlantName = _ls[88];
            this.Checkthose = _ls[89];
            this.CheckDate = _ls[90];
            this.Auditby = _ls[91];
            this.AuditDate = _ls[92];
            this.Countersigner = _ls[93];
            this.CountersignedDate = _ls[94];
            this.Proposer = _ls[95];
            this.ProposeDate = _ls[96];
            this.Pace = _ls[97];
            this.Instructioner = _ls[98];
            this.InstructionDate = _ls[99];
            this.MainDeliveryDate = _ls[100];
            this.DistributionCode = _ls[101];
            this.DistributionUnits = _ls[102];
            this.DistributionForm = _ls[103];
            this.DistributeCopies = _ls[104];
            this.DistributionTime = _ls[105];
            this.IdentificationDate = _ls[106];
            this.Identifier = _ls[107];
            this.AuthorizationObject = _ls[108];
            this.AuthorizationAct = _ls[109];
            this.AuthorizedStartTime = _ls[110];
            this.LicenseExpiryDate = _ls[111];
            this.Changer = _ls[112];
            this.Modified = _ls[113];
            this.Modifications = _ls[114];
            this.EditDescription = _ls[115];
            this.Disposaler = _ls[116];
            this.DisposalDate = _ls[117];
            this.DisposalType = _ls[118];
            this.DisposalResults = _ls[119];
            this.DestructionApproval = _ls[120];
            this.Destroyer = _ls[121];
            this.DestructionDate = _ls[122];
            this.DestructionReason = _ls[123];
            this.Transfer = _ls[124];
            this.TransferTime = _ls[125];
            this.Takeover = _ls[126];
            this.User = _ls[127];
            this.UseDate = _ls[128];
            this.Usetype = _ls[129];
            this.Physicallocation = _ls[130];
            this.StorageCarrierLogo = _ls[131];
            this.RelevanceItemID = _ls[132];
            this.AssociationType = _ls[133];
            this.Attachment = _ls[134];

            this.DeptName = _ls[135];
        }

        public void SetAttribute(string sAttr, string sValue)
        {
            foreach (KeyValuePair<string, int> item in lsMap)
            {
                if (item.Key == sAttr)
                {
                    _ls[item.Value] = sValue;
                    break;
                }
            }
        }

        public string GetAttribute(string sAttr)
        {
            foreach (KeyValuePair<string, int> item in lsMap)
            {
                if (item.Key == sAttr)
                {
                    return _ls[item.Value];
                }
            }
            return "";
        }

    }

    public class HNDP_CFileNode 
    {
        public string FK_ArchiveID;
        public string FK_CategoryID;
        public string FormationDept;
        public string FormationTime;
        public string PaperDocumentTransceiverTime;
        public string ElectronicDocumentTransceiverTime;
        public string Code;
        public string Code19;
        public string RelatedCode;
        public string DocCodesExplain;
        public string Revision;
        public string DocPages;
        public string Title;
        public string OtherTitle;
        public string D_FileStatus;
        public string Importer;
        public string ImporterTime;
        public string AuthorizeTime;
        public string OriginalID;
        public string Ext_1;
        public string Ext_2;
        public string Ext_3;
        public string Ext_4;
        public string Ext_5;
    }

    public class HNDP_CAttachmentNode
    {
        public string FK_FileID;	    //文件ID
        public string Title;		    //标题
        public string Url;	            //URL
        public string PublishedTime;	//发布时间
        public string Remark;		    //备注
        public string Size;	            //大小
        public string Type;	            //文件类型
        public string MakeUnit;	        //制作单位
        public string TimeSize;	        //时长
        public string MakeDate;	        //制作日期
        public string ServerWeb;	    //站点名
        public string DocumentName;		//文档库名
    }

    public class HN_OA2DP
    {
        #region ArchiveNode
        private const string  m_sArchiveNode = @"
<Devolve>
  <Params>
    <System></System>
    <!--系统—_(下划线)系统Id-->
    <!--管理员接收时，需要调用的Web Service-->
    <CallBackWebServiceUrl>null</CallBackWebServiceUrl>
    <!--管理员接收时，需要调用的Web Service的方法名,该方法有有一个参数：int SerialNo ： 流水号, bool Approve：同意或拒绝, string RejectReason：拒绝原因-->
    <CallBackWebServiceMethod>null</CallBackWebServiceMethod>
    <!--处理后是否删除-->
    <IsDeleteAfterProcess>false</IsDeleteAfterProcess>
    <!--处理后是否下载附件-->
    <IsDownLoadAttachment>true</IsDownLoadAttachment>
    <DeptName></DeptName>
  </Params>
  <Archive ArchiveType='0'>
    <FK_CategoryID></FK_CategoryID>
    <DCTableName></DCTableName>
    <FK_DC_ArchiveId></FK_DC_ArchiveId>
    <FK_DPID></FK_DPID>
    <Is_Piece></Is_Piece>
    <FormationDept></FormationDept>
    <FormationTime></FormationTime>
    <Authorizer></Authorizer>
    <AuthorizeTime></AuthorizeTime>
    <ReceiveCode></ReceiveCode>
    <PaperDocumentTransceiverTime></PaperDocumentTransceiverTime>
    <ElectronicDocumentTransceiverTime></ElectronicDocumentTransceiverTime>
    <CommunicationChannelCode></CommunicationChannelCode>
    <Importer></Importer>
    <ImporterTime></ImporterTime>
    <MainDispenseUnit></MainDispenseUnit>
    <D_FillingUnit></D_FillingUnit>
    <FillingTime></FillingTime>
    <D_DocStorageLife></D_DocStorageLife>
    <D_SecretLevel></D_SecretLevel>
    <DocStorageLife></DocStorageLife>
    <InvalidTime></InvalidTime>
    <D_InvalidType></D_InvalidType>
    <Code></Code>
    <Code19></Code19>
    <RelatedCode></RelatedCode>
    <Revision></Revision>
    <DocCode></DocCode>
    <D_StorageCarrierType></D_StorageCarrierType>
    <FrameCode></FrameCode>
    <D_DocStandards></D_DocStandards>
    <DocPages></DocPages>
    <Amount></Amount>
    <Title></Title>
    <OtherTitle></OtherTitle>
    <SubTitle></SubTitle>
    <KeyWords></KeyWords>
    <ThemeWord></ThemeWord>
    <Remark></Remark>
    <CoverRange></CoverRange>
    <SystemCode></SystemCode>
    <D_Reactor></D_Reactor>
    <D_EquipmentRelation></D_EquipmentRelation>
    <OverhaulCode></OverhaulCode>
    <NucleusCode></NucleusCode>
    <FunctionField></FunctionField>
    <StaffCode></StaffCode>
    <RecorderCode></RecorderCode>
    <Profession></Profession>
    <GatherLevel></GatherLevel>
    <ElectronicDocumentCount></ElectronicDocumentCount>
    <PictureCode></PictureCode>
    <D_Language></D_Language>
    <QualitySafeLevel></QualitySafeLevel>
    <CDNumber></CDNumber>
    <D_FileStatus></D_FileStatus>
    <DevolveStatus></DevolveStatus>
    <Is_Lend></Is_Lend>
    <Is_Dispose></Is_Dispose>
    <OriginalID></OriginalID>
    <D_AuditStatus></D_AuditStatus>
    <OriginalInfoURL></OriginalInfoURL>
    <Is_Transfer></Is_Transfer>
    <Ext_2></Ext_2>
    <Ext_3></Ext_3>
    <Ext_4></Ext_4>
    <Ext_5></Ext_5>

    <FK_ArchiveID></FK_ArchiveID>
    <Author></Author>
    <DigitalResourceFormationDept></DigitalResourceFormationDept>
    <DigitalResourceFormationTime></DigitalResourceFormationTime>
    <D_ReceiveType></D_ReceiveType>
    <ReceiveTime></ReceiveTime>
    <OriginalInfomationSystem></OriginalInfomationSystem>
    <EffectTime></EffectTime>
    <UndertakeDepartment></UndertakeDepartment>
    <RelatedDespenseUnit></RelatedDespenseUnit>
    <FormMaker></FormMaker>
    <D_DisposeStatus></D_DisposeStatus>
    <DocCodesExplain></DocCodesExplain>
    <Summary></Summary>
    <DocmentType></DocmentType>
    <ExpertOpinion></ExpertOpinion>
    <D_ProcessStatus></D_ProcessStatus>
    <ArchiveName></ArchiveName>
    <FondNumber></FondNumber>
    <FondName></FondName>
    <PlantCode></PlantCode>
    <PlantName></PlantName>
    <Checkthose></Checkthose>
    <CheckDate></CheckDate>
    <Auditby></Auditby>
    <AuditDate></AuditDate>
    <Countersigner></Countersigner>
    <CountersignedDate></CountersignedDate>
    <Proposer></Proposer>
    <ProposeDate></ProposeDate>
    <Pace></Pace>
    <Instructioner></Instructioner>
    <InstructionDate></InstructionDate>
    <MainDeliveryDate></MainDeliveryDate>
    <DistributionCode></DistributionCode>
    <DistributionUnits></DistributionUnits>
    <DistributionForm></DistributionForm>
    <DistributeCopies></DistributeCopies>
    <DistributionTime></DistributionTime>
    <IdentificationDate></IdentificationDate>
    <Identifier></Identifier>
    <AuthorizationObject></AuthorizationObject>
    <AuthorizationAct></AuthorizationAct>
    <AuthorizedStartTime></AuthorizedStartTime>
    <LicenseExpiryDate></LicenseExpiryDate>
    <Changer></Changer>
    <Modified></Modified>
    <Modifications></Modifications>
    <EditDescription></EditDescription>
    <Disposaler></Disposaler>
    <DisposalDate></DisposalDate>
    <DisposalType></DisposalType>
    <DisposalResults></DisposalResults>
    <DestructionApproval></DestructionApproval>
    <Destroyer></Destroyer>
    <DestructionDate></DestructionDate>
    <DestructionReason></DestructionReason>
    <Transfer></Transfer>
    <TransferTime></TransferTime>
    <Takeover></Takeover>
    <User></User>
    <UseDate></UseDate>
    <Usetype></Usetype>
    <Physicallocation></Physicallocation>
    <StorageCarrierLogo></StorageCarrierLogo>
    <RelevanceItemID></RelevanceItemID>
    <AssociationType></AssociationType>
    <Attachment></Attachment>
  </Archive>
</Devolve>";
        #endregion

        #region FileNode
        private const string m_sFileNode = @"
                        <File>
                            <FK_ArchiveID></FK_ArchiveID>
                            <FK_CategoryID></FK_CategoryID>
                            <FormationDept></FormationDept>
                            <FormationTime></FormationTime>
                            <PaperDocumentTransceiverTime></PaperDocumentTransceiverTime>
                            <ElectronicDocumentTransceiverTime></ElectronicDocumentTransceiverTime>
                            <Code></Code>
                            <Code19></Code19>
                            <RelatedCode></RelatedCode>
                            <DocCodesExplain></DocCodesExplain>
                            <Revision></Revision>
                            <DocPages></DocPages>
                            <Title></Title>
                            <OtherTitle></OtherTitle>
                            <D_FileStatus></D_FileStatus>
                            <Importer></Importer>
                            <ImporterTime></ImporterTime>
                            <AuthorizeTime></AuthorizeTime>
                            <OriginalID></OriginalID>
                            <Ext_1></Ext_1>
                            <Ext_2></Ext_2>
                            <Ext_3></Ext_3>
                            <Ext_4></Ext_4>
                            <Ext_5></Ext_5>
                        </File>";
        #endregion

        #region AttachmentNode
        private const string m_sAttachmentNode = @"
      <Attachment>
        <FK_FileID></FK_FileID>
        <Title></Title>
        <Url></Url>
        <PublishedTime></PublishedTime>
        <Remark></Remark>
        <Size></Size>
        <Type></Type>
        <MakeUnit></MakeUnit>
        <TimeSize></TimeSize>
        <MakeDate></MakeDate>
        <ServerWeb></ServerWeb>
        <DocumentName></DocumentName>
      </Attachment>";
        #endregion

        private XmlDocument m_doc = new XmlDocument();

        private CDevolveCfg cfg;

        public HN_OA2DP(string sPath)
        {
            cfg = new CDevolveCfg(sPath);
        }

        /// <summary>
        /// 设置XML制定节点的值
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="XPath">XML XPath路径</param>
        /// <param name="value">写入到NODE节点的值</param>
        private void SetNodeValue(XmlNode node, string XPath, string value)
        {
            XmlNode element = node.SelectSingleNode(XPath);
            if (element != null)
            {
                if (!string.IsNullOrEmpty(value))
                    element.InnerText = value;
            }
        }

        /// <summary>
        /// 生成归档XML的Attachment节点
        /// </summary>
        /// <param name="ParentFileNodeID">File节点FK_ArchiveID值</param>
        /// <param name="AttachmentNode"></param>
        /// <returns></returns>
        public string GenOAAttachmentNode(string ParentFileNodeID, HNDP_CAttachmentNode AttachmentNode)
        {
            string XmlAttachmentNode = m_sAttachmentNode;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlAttachmentNode);

            if (AttachmentNode != null)
            {
                XmlNode node = doc.SelectSingleNode("/Attachment");
                if (node == null) return m_doc.InnerXml;
                SetNodeValue(node, "/Attachment/FK_FileID", AttachmentNode.FK_FileID);
                SetNodeValue(node, "/Attachment/Title", AttachmentNode.Title);
                SetNodeValue(node, "/Attachment/Url", AttachmentNode.Url);
                SetNodeValue(node, "/Attachment/PublishedTime", CheckDateTime(AttachmentNode.PublishedTime));
                SetNodeValue(node, "/Attachment/Remark", AttachmentNode.Remark);
                SetNodeValue(node, "/Attachment/Size", AttachmentNode.Size);
                SetNodeValue(node, "/Attachment/Type", AttachmentNode.Type);
                SetNodeValue(node, "/Attachment/MakeUnit", AttachmentNode.MakeUnit);
                SetNodeValue(node, "/Attachment/TimeSize", AttachmentNode.TimeSize);
                SetNodeValue(node, "/Attachment/MakeDate", AttachmentNode.MakeDate);
                SetNodeValue(node, "/Attachment/ServerWeb", AttachmentNode.ServerWeb);
                SetNodeValue(node, "/Attachment/DocumentName", AttachmentNode.DocumentName);

                XmlNode SubNode = doc.DocumentElement.CloneNode(true);
                SubNode = m_doc.ImportNode(SubNode, true);
                m_doc.SelectSingleNode("/Devolve/Archive/File[FK_ArchiveID='" + ParentFileNodeID + "']").AppendChild(SubNode);
            }
            return m_doc.InnerXml;
        }

        /// <summary>
        /// 生成归档XML的File节点
        /// </summary>
        /// <param name="FileNode"></param>
        /// <returns></returns>
        public string GenOAFileNode(HNDP_CFileNode FileNode)
        {
            string XmlFileNode = m_sFileNode;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlFileNode);

            if (FileNode != null)
            {
                XmlNode node = doc.SelectSingleNode("/File");
                if (node == null) return m_doc.InnerXml;
                SetNodeValue(node, "/File/FK_ArchiveID", FileNode.FK_ArchiveID);
                SetNodeValue(node, "/File/FK_CategoryID", FileNode.FK_CategoryID);
                SetNodeValue(node, "/File/FormationDept", FileNode.FormationDept);
                SetNodeValue(node, "/File/FormationTime", FileNode.FormationTime);
                SetNodeValue(node, "/File/PaperDocumentTransceiverTime", CheckDateTime(FileNode.PaperDocumentTransceiverTime));
                SetNodeValue(node, "/File/ElectronicDocumentTransceiverTime", CheckDateTime(FileNode.ElectronicDocumentTransceiverTime));
                SetNodeValue(node, "/File/Code", FileNode.Code);
                SetNodeValue(node, "/File/Code19", FileNode.Code19);
                SetNodeValue(node, "/File/RelatedCode", FileNode.RelatedCode);
                SetNodeValue(node, "/File/DocCodesExplain", FileNode.DocCodesExplain);
                SetNodeValue(node, "/File/Revision", FileNode.Revision);
                SetNodeValue(node, "/File/DocPages", FileNode.DocPages);
                SetNodeValue(node, "/File/Title", FileNode.Title);
                SetNodeValue(node, "/File/OtherTitle", FileNode.OtherTitle);
                SetNodeValue(node, "/File/D_FileStatus", FileNode.D_FileStatus);
                SetNodeValue(node, "/File/Importer", FileNode.Importer);
                SetNodeValue(node, "/File/ImporterTime", CheckDateTime(FileNode.ImporterTime));
                SetNodeValue(node, "/File/AuthorizeTime", CheckDateTime(FileNode.AuthorizeTime));
                SetNodeValue(node, "/File/OriginalID", FileNode.OriginalID);
                SetNodeValue(node, "/File/Ext_1", FileNode.Ext_1);
                SetNodeValue(node, "/File/Ext_2", FileNode.Ext_2);
                SetNodeValue(node, "/File/Ext_3", FileNode.Ext_3);
                SetNodeValue(node, "/File/Ext_4", FileNode.Ext_4);
                SetNodeValue(node, "/File/Ext_5", FileNode.Ext_5);

                XmlNode SubNode = doc.DocumentElement.CloneNode(true);
                SubNode = m_doc.ImportNode(SubNode, true);
                m_doc.SelectSingleNode("/Devolve/Archive").AppendChild(SubNode);
            }
            return m_doc.InnerXml;
        }

        /// <summary>
        /// 生成归档XML的Archive节点
        /// </summary>
        /// <param name="ArchiveNode"></param>
        /// <param name="sProcess"></param>
        /// <returns></returns>
        public string GenOAArchiveNode(HNDP_CArchiveNode ArchiveNode, string sProcess)
        {
            m_doc.LoadXml(m_sArchiveNode);
            XmlNode node = m_doc.SelectSingleNode("/Devolve/Params");
            if (node != null && node.HasChildNodes)
            {
                SetNodeValue(node, "/Devolve/Params/System", ArchiveNode.System);
                //XmlNode element = (XmlNode)node.SelectSingleNode("/Devolve/Params/System");
                //if (element != null)
                //    element.InnerText = ArchiveNode.System;
                SetNodeValue(node, "/Devolve/Params/DeptName", ArchiveNode.DeptName);
            }
            node = m_doc.SelectSingleNode("/Devolve/Archive");
            DoConfig(sProcess);
            if (node != null && node.HasChildNodes)
            {
                string CategID = GetCfgNodeValues("/Devolve/Process[@Name='" + sProcess + "']/CategID");
                if (!string.IsNullOrEmpty(CategID))
                    node.Attributes["ArchiveType"].Value = "1";//sArchiveType;
                else
                    node.Attributes["ArchiveType"].Value = "0";
                ArchiveNode.FK_CategoryID = CategID;
                SetNodeValue(node, "/Devolve/Archive/FK_CategoryID", CategID/*ArchiveNode.FK_CategoryID*/);
                SetNodeValue(node, "/Devolve/Archive/DCTableName", ArchiveNode.DCTableName);
                SetNodeValue(node, "/Devolve/Archive/FK_DC_ArchiveId", ArchiveNode.FK_DC_ArchiveId);
                SetNodeValue(node, "/Devolve/Archive/FK_DPID", ArchiveNode.FK_DPID);
                SetNodeValue(node, "/Devolve/Archive/Is_Piece", ArchiveNode.Is_Piece);
                SetNodeValue(node, "/Devolve/Archive/FormationDept", ArchiveNode.FormationDept);
                SetNodeValue(node, "/Devolve/Archive/FormationTime", CheckDateTime(ArchiveNode.FormationTime));
                SetNodeValue(node, "/Devolve/Archive/Authorizer", ArchiveNode.Authorizer);
                SetNodeValue(node, "/Devolve/Archive/AuthorizeTime", CheckDateTime(ArchiveNode.AuthorizeTime));
                SetNodeValue(node, "/Devolve/Archive/ReceiveCode", ArchiveNode.ReceiveCode);
                SetNodeValue(node, "/Devolve/Archive/PaperDocumentTransceiverTime", CheckDateTime(ArchiveNode.PaperDocumentTransceiverTime));
                SetNodeValue(node, "/Devolve/Archive/ElectronicDocumentTransceiverTime", CheckDateTime(ArchiveNode.ElectronicDocumentTransceiverTime));
                SetNodeValue(node, "/Devolve/Archive/CommunicationChannelCode", ArchiveNode.CommunicationChannelCode);
                SetNodeValue(node, "/Devolve/Archive/Importer", ArchiveNode.Importer);
                SetNodeValue(node, "/Devolve/Archive/ImporterTime", CheckDateTime(ArchiveNode.ImporterTime));
                SetNodeValue(node, "/Devolve/Archive/MainDispenseUnit", ArchiveNode.MainDispenseUnit);
                SetNodeValue(node, "/Devolve/Archive/D_FillingUnit", ArchiveNode.D_FillingUnit);
                SetNodeValue(node, "/Devolve/Archive/FillingTime", ArchiveNode.FillingTime);
                SetNodeValue(node, "/Devolve/Archive/D_DocStorageLife", ArchiveNode.D_DocStorageLife);
                SetNodeValue(node, "/Devolve/Archive/D_SecretLevel", ArchiveNode.D_SecretLevel);
                SetNodeValue(node, "/Devolve/Archive/DocStorageLife", ArchiveNode.DocStorageLife);
                SetNodeValue(node, "/Devolve/Archive/InvalidTime", CheckDateTime(ArchiveNode.InvalidTime));
                SetNodeValue(node, "/Devolve/Archive/D_InvalidType", ArchiveNode.D_InvalidType);
                SetNodeValue(node, "/Devolve/Archive/Code", ArchiveNode.Code);
                SetNodeValue(node, "/Devolve/Archive/Code19", ArchiveNode.Code19);
                SetNodeValue(node, "/Devolve/Archive/RelatedCode", ArchiveNode.RelatedCode);
                SetNodeValue(node, "/Devolve/Archive/Revision", ArchiveNode.Revision);
                SetNodeValue(node, "/Devolve/Archive/DocCode", ArchiveNode.DocCode);
                SetNodeValue(node, "/Devolve/Archive/D_StorageCarrierType", ArchiveNode.D_StorageCarrierType);
                SetNodeValue(node, "/Devolve/Archive/FrameCode", ArchiveNode.FrameCode);
                SetNodeValue(node, "/Devolve/Archive/D_DocStandards", ArchiveNode.D_DocStandards);
                SetNodeValue(node, "/Devolve/Archive/DocPages", ArchiveNode.DocPages);
                SetNodeValue(node, "/Devolve/Archive/Amount", ArchiveNode.Amount);
                SetNodeValue(node, "/Devolve/Archive/Title", ArchiveNode.Title);
                SetNodeValue(node, "/Devolve/Archive/OtherTitle", ArchiveNode.OtherTitle);
                SetNodeValue(node, "/Devolve/Archive/SubTitle", ArchiveNode.SubTitle);
                SetNodeValue(node, "/Devolve/Archive/KeyWords", ArchiveNode.KeyWords);
                SetNodeValue(node, "/Devolve/Archive/ThemeWord", ArchiveNode.ThemeWord);
                SetNodeValue(node, "/Devolve/Archive/Remark", ArchiveNode.Remark);
                SetNodeValue(node, "/Devolve/Archive/CoverRange", ArchiveNode.CoverRange);
                SetNodeValue(node, "/Devolve/Archive/SystemCode", ArchiveNode.SystemCode);
                SetNodeValue(node, "/Devolve/Archive/D_Reactor", ArchiveNode.D_Reactor);
                SetNodeValue(node, "/Devolve/Archive/D_EquipmentRelation", ArchiveNode.D_EquipmentRelation);
                SetNodeValue(node, "/Devolve/Archive/OverhaulCode", ArchiveNode.OverhaulCode);
                SetNodeValue(node, "/Devolve/Archive/NucleusCode", ArchiveNode.NucleusCode);
                SetNodeValue(node, "/Devolve/Archive/FunctionField", ArchiveNode.FunctionField);
                SetNodeValue(node, "/Devolve/Archive/StaffCode", ArchiveNode.StaffCode);
                SetNodeValue(node, "/Devolve/Archive/RecorderCode", ArchiveNode.RecorderCode);
                SetNodeValue(node, "/Devolve/Archive/Profession", ArchiveNode.Profession);
                SetNodeValue(node, "/Devolve/Archive/GatherLevel", ArchiveNode.GatherLevel);
                SetNodeValue(node, "/Devolve/Archive/ElectronicDocumentCount", ArchiveNode.ElectronicDocumentCount);
                SetNodeValue(node, "/Devolve/Archive/PictureCode", ArchiveNode.PictureCode);
                SetNodeValue(node, "/Devolve/Archive/D_Language", ArchiveNode.D_Language);
                SetNodeValue(node, "/Devolve/Archive/QualitySafeLevel", ArchiveNode.QualitySafeLevel);
                SetNodeValue(node, "/Devolve/Archive/CDNumber", ArchiveNode.CDNumber);
                SetNodeValue(node, "/Devolve/Archive/D_FileStatus", ArchiveNode.D_FileStatus);
                SetNodeValue(node, "/Devolve/Archive/DevolveStatus", ArchiveNode.DevolveStatus);
                SetNodeValue(node, "/Devolve/Archive/Is_Lend", ArchiveNode.Is_Lend);
                SetNodeValue(node, "/Devolve/Archive/Is_Dispose", ArchiveNode.Is_Dispose);
                SetNodeValue(node, "/Devolve/Archive/OriginalID", ArchiveNode.OriginalID);
                SetNodeValue(node, "/Devolve/Archive/D_AuditStatus", ArchiveNode.D_AuditStatus);
                SetNodeValue(node, "/Devolve/Archive/OriginalInfoURL", ArchiveNode.OriginalInfoURL);
                SetNodeValue(node, "/Devolve/Archive/Is_Transfer", ArchiveNode.Is_Transfer);
                SetNodeValue(node, "/Devolve/Archive/Ext_2", ArchiveNode.Ext_2);
                SetNodeValue(node, "/Devolve/Archive/Ext_3", ArchiveNode.Ext_3);
                SetNodeValue(node, "/Devolve/Archive/Ext_4", ArchiveNode.Ext_4);
                SetNodeValue(node, "/Devolve/Archive/Ext_5", ArchiveNode.Ext_5);

                SetNodeValue(node, "/Devolve/Archive/FK_ArchiveID", ArchiveNode.FK_ArchiveID);
                SetNodeValue(node, "/Devolve/Archive/Author", ArchiveNode.Author);
                SetNodeValue(node, "/Devolve/Archive/DigitalResourceFormationDept", ArchiveNode.DigitalResourceFormationDept);
                SetNodeValue(node, "/Devolve/Archive/DigitalResourceFormationTime", CheckDateTime(ArchiveNode.DigitalResourceFormationTime));
                SetNodeValue(node, "/Devolve/Archive/D_ReceiveType", ArchiveNode.D_ReceiveType);
                SetNodeValue(node, "/Devolve/Archive/ReceiveTime", CheckDateTime(ArchiveNode.ReceiveTime));
                SetNodeValue(node, "/Devolve/Archive/OriginalInfomationSystem", ArchiveNode.OriginalInfomationSystem);
                SetNodeValue(node, "/Devolve/Archive/EffectTime", CheckDateTime(ArchiveNode.EffectTime));
                SetNodeValue(node, "/Devolve/Archive/UndertakeDepartment", ArchiveNode.UndertakeDepartment);
                SetNodeValue(node, "/Devolve/Archive/RelatedDespenseUnit", ArchiveNode.RelatedDespenseUnit);
                SetNodeValue(node, "/Devolve/Archive/FormMaker", ArchiveNode.FormMaker);
                SetNodeValue(node, "/Devolve/Archive/D_DisposeStatus", ArchiveNode.D_DisposeStatus);
                SetNodeValue(node, "/Devolve/Archive/DocCodesExplain", ArchiveNode.DocCodesExplain);
                SetNodeValue(node, "/Devolve/Archive/Summary", ArchiveNode.Summary);
                SetNodeValue(node, "/Devolve/Archive/DocmentType", ArchiveNode.DocmentType);
                SetNodeValue(node, "/Devolve/Archive/ExpertOpinion", ArchiveNode.ExpertOpinion);
                SetNodeValue(node, "/Devolve/Archive/D_ProcessStatus", ArchiveNode.D_ProcessStatus);
                SetNodeValue(node, "/Devolve/Archive/ArchiveName", ArchiveNode.ArchiveName);
                SetNodeValue(node, "/Devolve/Archive/FondNumber", ArchiveNode.FondNumber);
                SetNodeValue(node, "/Devolve/Archive/FondName", ArchiveNode.FondName);
                SetNodeValue(node, "/Devolve/Archive/PlantCode", ArchiveNode.PlantCode);
                SetNodeValue(node, "/Devolve/Archive/PlantName", ArchiveNode.PlantName);
                SetNodeValue(node, "/Devolve/Archive/Checkthose", ArchiveNode.Checkthose);
                SetNodeValue(node, "/Devolve/Archive/CheckDate", CheckDateTime(ArchiveNode.CheckDate));
                SetNodeValue(node, "/Devolve/Archive/Auditby", ArchiveNode.Auditby);
                SetNodeValue(node, "/Devolve/Archive/AuditDate", CheckDateTime(ArchiveNode.AuditDate));
                SetNodeValue(node, "/Devolve/Archive/Countersigner", ArchiveNode.Countersigner);
                SetNodeValue(node, "/Devolve/Archive/CountersignedDate", CheckDateTime(ArchiveNode.CountersignedDate));
                SetNodeValue(node, "/Devolve/Archive/Proposer", ArchiveNode.Proposer);
                SetNodeValue(node, "/Devolve/Archive/ProposeDate", CheckDateTime(ArchiveNode.ProposeDate));
                SetNodeValue(node, "/Devolve/Archive/Pace", ArchiveNode.Pace);
                SetNodeValue(node, "/Devolve/Archive/Instructioner", ArchiveNode.Instructioner);
                SetNodeValue(node, "/Devolve/Archive/InstructionDate", CheckDateTime(ArchiveNode.InstructionDate));
                SetNodeValue(node, "/Devolve/Archive/MainDeliveryDate", CheckDateTime(ArchiveNode.MainDeliveryDate));
                SetNodeValue(node, "/Devolve/Archive/DistributionCode", ArchiveNode.DistributionCode);
                SetNodeValue(node, "/Devolve/Archive/DistributionUnits", ArchiveNode.DistributionUnits);
                SetNodeValue(node, "/Devolve/Archive/DistributionForm", ArchiveNode.DistributionForm);
                SetNodeValue(node, "/Devolve/Archive/DistributeCopies", ArchiveNode.DistributeCopies);
                SetNodeValue(node, "/Devolve/Archive/DistributionTime", CheckDateTime(ArchiveNode.DistributionTime));
                SetNodeValue(node, "/Devolve/Archive/IdentificationDate", ArchiveNode.IdentificationDate);
                SetNodeValue(node, "/Devolve/Archive/Identifier", ArchiveNode.Identifier);
                SetNodeValue(node, "/Devolve/Archive/AuthorizationObject", ArchiveNode.AuthorizationObject);
                SetNodeValue(node, "/Devolve/Archive/AuthorizationAct", ArchiveNode.AuthorizationAct);
                SetNodeValue(node, "/Devolve/Archive/AuthorizedStartTime", CheckDateTime(ArchiveNode.AuthorizedStartTime));
                SetNodeValue(node, "/Devolve/Archive/LicenseExpiryDate", CheckDateTime(ArchiveNode.LicenseExpiryDate));
                SetNodeValue(node, "/Devolve/Archive/Changer", ArchiveNode.Changer);
                SetNodeValue(node, "/Devolve/Archive/Modified", ArchiveNode.Modified);
                SetNodeValue(node, "/Devolve/Archive/Modifications", ArchiveNode.Modifications);
                SetNodeValue(node, "/Devolve/Archive/EditDescription", ArchiveNode.EditDescription);
                SetNodeValue(node, "/Devolve/Archive/Disposaler", ArchiveNode.Disposaler);
                SetNodeValue(node, "/Devolve/Archive/DisposalDate", CheckDateTime(ArchiveNode.DisposalDate));
                SetNodeValue(node, "/Devolve/Archive/DisposalType", ArchiveNode.DisposalType);
                SetNodeValue(node, "/Devolve/Archive/DisposalResults", ArchiveNode.DisposalResults);
                SetNodeValue(node, "/Devolve/Archive/DestructionApproval", ArchiveNode.DestructionApproval);
                SetNodeValue(node, "/Devolve/Archive/Destroyer", ArchiveNode.Destroyer);
                SetNodeValue(node, "/Devolve/Archive/DestructionDate", CheckDateTime(ArchiveNode.DestructionDate));
                SetNodeValue(node, "/Devolve/Archive/DestructionReason", ArchiveNode.DestructionReason);
                SetNodeValue(node, "/Devolve/Archive/Transfer", ArchiveNode.Transfer);
                SetNodeValue(node, "/Devolve/Archive/TransferTime", CheckDateTime(ArchiveNode.TransferTime));
                SetNodeValue(node, "/Devolve/Archive/Takeover", ArchiveNode.Takeover);
                SetNodeValue(node, "/Devolve/Archive/User", ArchiveNode.User);
                SetNodeValue(node, "/Devolve/Archive/UseDate", CheckDateTime(ArchiveNode.UseDate));
                SetNodeValue(node, "/Devolve/Archive/Usetype", ArchiveNode.Usetype);
                SetNodeValue(node, "/Devolve/Archive/Physicallocation", ArchiveNode.Physicallocation);
                SetNodeValue(node, "/Devolve/Archive/StorageCarrierLogo", ArchiveNode.StorageCarrierLogo);
                SetNodeValue(node, "/Devolve/Archive/RelevanceItemID", ArchiveNode.RelevanceItemID);
                SetNodeValue(node, "/Devolve/Archive/AssociationType", ArchiveNode.AssociationType);
                SetNodeValue(node, "/Devolve/Archive/Attachment", ArchiveNode.Attachment);
            }
            return m_doc.InnerXml;
        }

        private void DoConfig(string sProcess)
        { 
            //string sPath = @"E:\QWDMS\Non Baseline Library\Development\03 Code\FounderSoftware.ADIM\FounderSoftware.ADIM.OA\OADevolveLib\DevolveConfig.xml";
            
            //string value = cfg.GetOtherNodeValues("/Devolve/Other/ServerWeb");
            cfg.XmlDoc = this.m_doc;
            cfg.SetPublicNodeValue();
            cfg.SetPrivateNodeValue(sProcess);
        }

        /// <summary>
        /// 获取DevolveConfig.XML配置文件的指定路径节点的值
        /// </summary>
        /// <param name="sXPath">XML的XPATH路径值</param>
        /// <returns>返回该节点的值</returns>
        public string GetCfgNodeValues(string sXPath)
        {
            return cfg.GetOtherNodeValues(sXPath);
        }

        public List<DevKVItem> MapFunction(string sDevPlat, string sProcName)
        {
            return cfg.MapFunction(sDevPlat, sProcName);
        }

        public string PreHandel(EntityBase _Entity, string sAttr, string sType)
        {
            string sValue = "";
            if (_Entity.GetVal(sAttr) == null)
            {
                sValue += "";
            }
            else
            {
                if (_Entity.GetVal(sAttr).GetType().Name == "DateTime")
                {
                    sValue += CheckDateTime(((DateTime)_Entity.GetVal(sAttr)).ToShortDateString());
                }
                else
                {
                    sValue += _Entity.GetVal(sAttr).ToString();
                    if (sType.ToUpper() != "INT")
                    {
                        if (!string.IsNullOrEmpty(OAUser.GetUserName(sValue)))
                            return OAUser.GetUserName(sValue);
                        else
                            if (!string.IsNullOrEmpty(OADept.GetDeptName(sValue)))
                                return OADept.GetDeptName(sValue);
                    }
                }
            }
            return sValue;
        }

        public string PreHandel(EntityBase _Entity, string sAttr, string sTyps, bool bMulti)
        {
            string[] arr = sAttr.Split(',');
            string sResult = "";
            for (int i = 0; i < arr.Length; i++)
            {
                sResult += PreHandel(_Entity, arr[i], sTyps);
                if ((i + 1) != arr.Length)
                {
                    sResult += ",";
                }
            }
            return sResult;
        }

        public string CheckDateTime(string strDateTime)
        {
            if (string.IsNullOrEmpty(strDateTime)) return "";

            if (strDateTime == "0001-1-1")
            {
                return (strDateTime = "");
            }
            if (strDateTime == "0001年1月1日")
            {
                return (strDateTime = "");
            }
            if (strDateTime == "1/1/0001")
            {
                return (strDateTime = "");
            }
            if (strDateTime == "0001/1/1")
            {
                return (strDateTime = "");
            }
            if (strDateTime == "0001-1-1 0:00:00")
            {
                return (strDateTime = "");
            }
            return strDateTime;
        }
    }
}

