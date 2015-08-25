using System;
using System.Web;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common.Utility;
using FounderSoftware.ADIM.OA.OA2DP;
using FS.ADIM.OA.BLL.Entity;
using FounderSoftware.ADIM.OA.OA2DC;
using System.Collections.Generic;

namespace FS.ADIM.OA.WebUI.WorkFlow.Finance
{
    public partial class UC_Finance 
    {
        private string Devolve2DC(HN_OA2DC oaDev)
        {
            string sXml = "";

            List<FounderSoftware.ADIM.OA.OA2DC.DevKVItem> ls = oaDev.MapFunction("DC", base.TemplateName);

            B_Finance entity = this.ControlToEntity(false) as B_Finance;
            entity.FormsData = XmlUtility.SerializeXml(entity);

            FounderSoftware.ADIM.OA.OA2DC.HNDC_CArchiveNode ar = new HNDC_CArchiveNode();
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CFileNode fl = new HNDC_CFileNode();

            //string sPath = @"E:\QWDMS\Non Baseline Library\Development\03 Code\FounderSoftware.ADIM\FounderSoftware.ADIM.OA\OADevolveLib\DevolveConfig.xml";

            #region 开始生成Archive节点
            ar.System = "OA_" + base.IdentityID.ToString();
            string ObjPlatForm = oaDev.GetCfgNodeValues("/Devolve/Process[@Name='" + base.TemplateName + "']/Object");
            switch (ObjPlatForm)
            {
                case "DC":
                    ar.SysFlag = "1";
                    break;
                case "FC":
                    ar.SysFlag = "2";
                    break;
                default:
                    ar.SysFlag = "1";
                    break;
            }

            #region 根据DevolveConfig.xml配置参数设置Archive归档节点
            for (int i = 0; i < ls.Count; i++)
            {
                string skey = (ls[i] as FounderSoftware.ADIM.OA.OA2DC.DevKVItem).sKey;
                string sName = (ls[i] as FounderSoftware.ADIM.OA.OA2DC.DevKVItem).sValue;
                string sType = (ls[i] as FounderSoftware.ADIM.OA.OA2DC.DevKVItem).sType;
                ar.SetAttribute(skey, oaDev.PreHandel(entity, sName, sType)/*(entity.GetVal(sName) as string)*/);
            }
            ar.SetOAArchiveNode();
            // 其他节点处理应放置在SetOAArchiveNode方法之后

            //return Devolve2DP(oaDev, sDPID, ar);
            #endregion 根据DevolveConfig.xml配置参数设置Archive归档节点

            ar.ElectronicDocumentCount = ucAttachment.UCDataList.Count.ToString();
            #endregion
            sXml = oaDev.GenOAArchiveNode(ar, base.TemplateName);

            /****开始生成File节点************************************************************************/
            #region 开始生成File节点
            fl.AuthorizeTime = ar.AuthorizeTime;
            fl.Code = ar.Code;
            fl.Code19 = ar.Code19;
            fl.D_FileStatus = ar.D_FileStatus;
            fl.DocCodesExplain = "";
            fl.DocPages = ar.DocPages;
            fl.ElectronicDocumentTransceiverTime = ar.ElectronicDocumentTransceiverTime;
            fl.Ext_1 = ar.Ext_1;
            fl.Ext_2 = ar.Ext_2;
            fl.Ext_3 = ar.Ext_3;
            fl.Ext_4 = ar.Ext_4;
            fl.Ext_5 = ar.Ext_5;
            fl.Title = ar.Title;
            fl.FK_ArchiveID = ar.FK_Archive;
            fl.FK_CategoryID = ar.FK_CategoryID;
            fl.FormationDept = ar.FormationDept;
            fl.FormationTime = ar.FormationTime;
            fl.Importer = ar.Importer;
            fl.ImporterTime = ar.ImporterTime;
            fl.OriginalID = ar.OriginalID;
            fl.OtherTitle = ar.OtherTitle;
            fl.PaperDocumentTransceiverTime = ar.PaperDocumentTransceiverTime;
            fl.RelatedCode = ar.RelatedCode;
            fl.Revision = ar.Revision;
            #endregion
            sXml = oaDev.GenOAFileNode(fl);
            /****完成生成File节点************************************************************************/

            #region 始生成Attachment节点
            /****开始生成Attachment节点******************************************************************/
            string sServerWeb = oaDev.GetCfgNodeValues("/Devolve/Other/ServerWeb");
            //"http://172.29.128.239";
            //string sDocumentName = oaDev.GetCfgNodeValues("/Devolve/Other/DocumentName");
            for (int i = 0; i < ucAttachment.UCDataList.Count; i++)
            {
                HNDC_CAttachmentNode at = new HNDC_CAttachmentNode();
                CFuJian file = ucAttachment.UCDataList[i];

                at.DocumentName = "请示报告";//file.FileName;
                at.FK_FileID = "";
                at.MakeDate = "";
                at.MakeUnit = "";
                at.PublishedTime = "";
                at.Remark = "";

                at.ServerWeb = sServerWeb;
                Double iSize = 0;
                string sSize = file.Size.ToUpper();
                if (sSize.Contains("K"))
                {
                    sSize = sSize.Replace("K", "");
                    iSize = Convert.ToDouble(sSize);
                    iSize = iSize * 1024;
                }
                if (sSize.Contains("M"))
                {
                    sSize = sSize.Replace("M", "");
                    iSize = Convert.ToDouble(sSize);
                    iSize = iSize * 1024 * 1024;
                }
                at.Size = ((int)iSize).ToString();
                at.TimeSize = "";
                at.Title = file.Alias;
                at.Type = file.Type;
                at.Url = file.URL;

                sXml = oaDev.GenOAAttachmentNode(fl.FK_ArchiveID, at);
            }
            #endregion
            /****完成生成Attachment节点******************************************************************/

            return sXml;
        }

        private string Devolve2DP(FounderSoftware.ADIM.OA.OA2DP.HN_OA2DP hn_oa2dp, string sDPID)
        {
            string sXml = "";
            B_Finance entity = this.ControlToEntity(false) as B_Finance;
            #region ArchiveNode
            FounderSoftware.ADIM.OA.OA2DP.HNDP_CArchiveNode ar = new HNDP_CArchiveNode();
            ar.System = "OA_" + base.IdentityID.ToString();
            ar.FK_DPID = sDPID;

            ar.UndertakeDepartment = entity.BianZhiBuMenID;
            ar.Title = entity.DocumentTitle;
            ar.MainDispenseUnit = entity.MainSend;
            ar.RelatedDespenseUnit = entity.CopySend;
            ar.FormationDept = entity.Department;
            ar.EffectTime = entity.ConfirmDate.ToShortDateString();
            ar.Auditby = entity.CheckDrafter;
            ar.AuditDate = entity.CheckDate.ToShortDateString();
            ar.Author = entity.Drafter;

            ar.ElectronicDocumentCount = ucAttachment.UCDataList.Count.ToString();
            #endregion
            sXml = hn_oa2dp.GenOAArchiveNode(ar, base.TemplateName);

            FounderSoftware.ADIM.OA.OA2DP.HNDP_CFileNode fl = new HNDP_CFileNode();
            /****开始生成File节点************************************************************************/
            #region 开始生成File节点
            fl.AuthorizeTime = ar.AuthorizeTime;
            fl.Code = ar.Code;
            fl.Code19 = ar.Code19;
            fl.D_FileStatus = ar.D_FileStatus;
            fl.DocCodesExplain = "";
            fl.DocPages = ar.DocPages;
            fl.ElectronicDocumentTransceiverTime = ar.ElectronicDocumentTransceiverTime;
            fl.Ext_1 = "";
            fl.Ext_2 = ar.Ext_2;
            fl.Ext_3 = ar.Ext_3;
            fl.Ext_4 = ar.Ext_4;
            fl.Ext_5 = ar.Ext_5;
            fl.Title = ar.Title;
            //fl.FK_ArchiveID = ar.FK_Archive;
            fl.FK_CategoryID = ar.FK_CategoryID;
            fl.FormationDept = ar.FormationDept;
            fl.FormationTime = ar.FormationTime;
            fl.Importer = ar.Importer;
            fl.ImporterTime = ar.ImporterTime;
            fl.OriginalID = ar.OriginalID;
            fl.OtherTitle = ar.OtherTitle;
            fl.PaperDocumentTransceiverTime = ar.PaperDocumentTransceiverTime;
            fl.RelatedCode = ar.RelatedCode;
            fl.Revision = ar.Revision;
            #endregion
            sXml = hn_oa2dp.GenOAFileNode(fl);
            /****完成生成File节点************************************************************************/

            #region 始生成Attachment节点
            /****开始生成Attachment节点******************************************************************/
            string sServerWeb = hn_oa2dp.GetCfgNodeValues("/Devolve/Other/ServerWeb");
            //"http://172.29.128.239";
            //string sDocumentName = oaDev.GetCfgNodeValues("/Devolve/Other/DocumentName");
            for (int i = 0; i < ucAttachment.UCDataList.Count; i++)
            {
                FounderSoftware.ADIM.OA.OA2DP.HNDP_CAttachmentNode at = new HNDP_CAttachmentNode();
                CFuJian file = ucAttachment.UCDataList[i];

                at.DocumentName = "请示报告";//file.FileName;
                at.FK_FileID = "";
                at.MakeDate = "";
                at.MakeUnit = "";
                at.PublishedTime = "";
                at.Remark = "";

                at.ServerWeb = sServerWeb;
                Double iSize = 0;
                string sSize = file.Size.ToUpper();
                if (sSize.Contains("K"))
                {
                    sSize = sSize.Replace("K", "");
                    iSize = Convert.ToDouble(sSize);
                    iSize = iSize * 1024;
                }
                if (sSize.Contains("M"))
                {
                    sSize = sSize.Replace("M", "");
                    iSize = Convert.ToDouble(sSize);
                    iSize = iSize * 1024 * 1024;
                }
                at.Size = ((int)iSize).ToString();
                at.TimeSize = "";
                at.Title = file.Alias;
                at.Type = file.Type;
                at.Url = file.URL;

                sXml = hn_oa2dp.GenOAAttachmentNode(fl.FK_ArchiveID, at);
            }
            #endregion
            /****完成生成Attachment节点******************************************************************/

            return sXml;
        }

        public void Devolve(out string sResult)
        {
            sResult = "";
            string sPath = HttpContext.Current.Server.MapPath((@"~\Config\DevolveConfig.xml"));
            FounderSoftware.ADIM.OA.OA2DC.HN_OA2DC dcDev = new HN_OA2DC(sPath);
            FounderSoftware.ADIM.OA.OA2DP.HN_OA2DP dpDev = new HN_OA2DP(sPath);

            // 开始发送归档
            /*
             * 根据DevoleConfig.xml配置文件的中相应流程的<Object>DC</Object>节点来判断归档到哪个系统中
             * 若配置中为DC则表明调用OA2DC.DLL接口进行归档,若为DP则表明调用OA2DP.DLL接口进行归档
             */
            string xml = "";
            string objDevolve = dcDev.GetCfgNodeValues("/Devolve/Process[@Name='" + base.TemplateName + "']/Object");
            string s = "";

            //EntityLetterSend entity = new EntityLetterSend(base.tableName);
            //SetEntity(entity, true);
            B_Finance entity = this.ControlToEntity(false) as B_Finance;
            
            entity.FormsData = XmlUtility.SerializeXml(entity);
            if (objDevolve.ToUpper() == "DC" || objDevolve.ToUpper() == "FC")
            {
                FounderSoftware.ADIM.OA.OA2DC.OA2DC oa2dc = new FounderSoftware.ADIM.OA.OA2DC.OA2DC();
                xml = Devolve2DC(dcDev);

                s = oa2dc.SendDevolve(xml, entity.FormsData, "请示报告");
                sResult += (s + @"\r\n");
                try
                {
                    Convert.ToInt32(s);
                }
                catch
                {
                    throw new Exception(s);
                }
                //JScript.ShowMsgBox(Page, MsgType.VbCritical, s);
            }
                       

            if (objDevolve.ToUpper() == "DP")
            {
                FounderSoftware.ADIM.OA.OA2DP.OA2DP oa2dp = new FounderSoftware.ADIM.OA.OA2DP.OA2DP();
                string sOADPIDs = dpDev.GetCfgNodeValues("/Devolve/Process[@Name='" + base.TemplateName + "']/DPID");

                // 若DevolveConifg.xml配置文件中未配置CategID则根据流程实体中的处室ID归档

                string[] arrDPID = sOADPIDs.Split(new char[] { ',', ';' });
                for (int i = 0; i < arrDPID.Length; i++)
                {
                    try
                    {
                        Convert.ToInt32(arrDPID[i]);
                    }
                    catch
                    {
                        continue;
                    }
                    xml = Devovle2DP2(dpDev, arrDPID[i]);
                    s = oa2dp.SendDevolve(xml, entity.FormsData, "请示报告");
                    sResult += (s + @"\r\n");
                    try
                    {
                        Convert.ToInt32(s);
                    }
                    catch
                    {
                        throw new Exception(s);
                    }
                    //JScript.ShowMsgBox(Page, MsgType.VbCritical, s);
                }

                string sOptionDPID = dpDev.GetCfgNodeValues("/Devolve/Process[@Name='" + base.TemplateName + "']/OptionDPID");
                string[] arrOpDPID = sOptionDPID.Split(new char[] { ',' });
                for (int i = 0; i < arrOpDPID.Length; i++)
                {
                    if (entity.GetVal(arrOpDPID[i]) == null) continue;
                    string sDPID = entity.GetVal(arrOpDPID[i]).ToString();
                    string[] IDs = sDPID.Split(new char[] { ';', ',' });
                    for (int j = 0; j < IDs.Length; j++)
                    {
                        try
                        {
                            Convert.ToInt32(IDs[j]);
                        }
                        catch
                        {
                            continue;
                        }
                        xml = Devovle2DP2(dpDev, IDs[j]);
                        s = oa2dp.SendDevolve(xml, entity.FormsData, "请示报告");
                        sResult += (s + @"\r\n");
                        try
                        {
                            Convert.ToInt32(s);
                        }
                        catch
                        {
                            throw new Exception(s);
                        }
                    }
                    //JScript.ShowMsgBox(Page, MsgType.VbCritical, s);
                }

                #region 注释保留
                //if (string.IsNullOrEmpty(sOADPIDs))
                //{
                //    string[] sDPID1 = entity.BianZhiBuMenID.Split(new char[] { ',', ';' });
                //    string[] sDPID2 = entity.ChengBanBuMenID.Split(new char[] { ',', ';' });
                //    if (sDPID1 != null)
                //    {
                //        for (int i = 0; i < sDPID1.Length; i++)
                //        {
                //            if (string.IsNullOrEmpty(sDPID1[i])) continue;
                //            xml = Devolve2DP(dpDev, sDPID1[i]);
                //            s = oa2dp.SendDevolve(xml, entity.FormsData, "请示报告");
                //            //JScript.ShowMsgBox(Page, MsgType.VbCritical, s);
                //        }
                //    }

                //    if (sDPID2 != null)
                //    {
                //        for (int i = 0; i < sDPID2.Length; i++)
                //        {
                //            if (string.IsNullOrEmpty(sDPID2[i])) continue;
                //            xml = Devolve2DP(dpDev, sDPID2[i]);
                //            s = oa2dp.SendDevolve(xml, entity.FormsData, "请示报告");
                //            //JScript.ShowMsgBox(Page, MsgType.VbCritical, s);
                //        }
                //    }
                //}
                //else
                //// 根据配置文件的处室ID归档
                //{
                //    string[] arrDPID = sOADPIDs.Split(new char[] { ',' });
                //    for (int i = 0; i < arrDPID.Length; i++)
                //    {
                //        xml = Devolve2DP(dpDev, arrDPID[i]);
                //        s = oa2dp.SendDevolve(xml, entity.FormsData, "请示报告");
                //        //JScript.ShowMsgBox(Page, MsgType.VbCritical, s);
                //    }
                //}
                #endregion 
            }
            //string rest = oa2dc.CallBack(Convert.ToInt32(s), true, s);
            //string rest = CDevolve.CallBack(Convert.ToInt32(s), true, s, doc.InnerXml);
            
            //IMessage ms = new WebFormMessage(Page, s);
            //ms.Show();
        }

        private string Devovle2DP2(HN_OA2DP hn_oa2dp, string sDPID)
        {
            string sXml = "";
            FounderSoftware.ADIM.OA.OA2DP.HNDP_CArchiveNode ar = new HNDP_CArchiveNode();

            List<FounderSoftware.ADIM.OA.OA2DP.DevKVItem> ls = hn_oa2dp.MapFunction("DP", base.TemplateName);

            B_Finance entity = this.ControlToEntity(false) as B_Finance;
            entity.FormsData = XmlUtility.SerializeXml(entity);

            //ar.SetAttribute("FormationTime"/*配置文件中的归档字段*/, "2009-12-12"/*cEntity.GetValue(配置文件中的实体属性名)*/);
            //string sValue = ar.GetAttribute("FormationTime");
            #region 生成Archive节点
            ar.System = "OA_" + base.IdentityID.ToString();

            #region 根据DevolveConfig.xml配置参数设置Archive归档节点
            for (int i = 0; i < ls.Count; i++)
            {
                string skey = (ls[i] as FounderSoftware.ADIM.OA.OA2DP.DevKVItem).sKey;
                string sName = (ls[i] as FounderSoftware.ADIM.OA.OA2DP.DevKVItem).sValue;
                string sType = (ls[i] as FounderSoftware.ADIM.OA.OA2DP.DevKVItem).sType;
                ar.SetAttribute(skey, hn_oa2dp.PreHandel(entity, sName, sType, true)/*(entity.GetVal(sName) as string)*/);
            }
            ar.SetOAArchiveNode();
            // 其他节点处理应放置在SetOAArchiveNode方法之后
            ar.FK_DPID = sDPID;
            //return Devolve2DP(oaDev, sDPID, ar);
            #endregion 根据DevolveConfig.xml配置参数设置Archive归档节点

            ar.ElectronicDocumentCount = ucAttachment.UCDataList.Count.ToString();
            #endregion
            sXml = hn_oa2dp.GenOAArchiveNode(ar, base.TemplateName);

            FounderSoftware.ADIM.OA.OA2DP.HNDP_CFileNode fl = new HNDP_CFileNode();
            /****开始生成File节点************************************************************************/
            #region 开始生成File节点
            fl.AuthorizeTime = ar.AuthorizeTime;
            fl.Code = ar.Code;
            fl.Code19 = ar.Code19;
            fl.D_FileStatus = ar.D_FileStatus;
            fl.DocCodesExplain = "";
            fl.DocPages = ar.DocPages;
            fl.ElectronicDocumentTransceiverTime = ar.ElectronicDocumentTransceiverTime;
            fl.Ext_1 = "";
            fl.Ext_2 = ar.Ext_2;
            fl.Ext_3 = ar.Ext_3;
            fl.Ext_4 = ar.Ext_4;
            fl.Ext_5 = ar.Ext_5;
            fl.Title = ar.Title;
            //fl.FK_ArchiveID = ar.FK_Archive;
            fl.FK_CategoryID = ar.FK_CategoryID;
            fl.FormationDept = ar.FormationDept;
            fl.FormationTime = ar.FormationTime;
            fl.Importer = ar.Importer;
            fl.ImporterTime = ar.ImporterTime;
            fl.OriginalID = ar.OriginalID;
            fl.OtherTitle = ar.OtherTitle;
            fl.PaperDocumentTransceiverTime = ar.PaperDocumentTransceiverTime;
            fl.RelatedCode = ar.RelatedCode;
            fl.Revision = ar.Revision;
            #endregion
            sXml = hn_oa2dp.GenOAFileNode(fl);
            /****完成生成File节点************************************************************************/

            #region 始生成Attachment节点
            /****开始生成Attachment节点******************************************************************/
            string sServerWeb = hn_oa2dp.GetCfgNodeValues("/Devolve/Other/ServerWeb");
            //"http://172.29.128.239";
            //string sDocumentName = oaDev.GetCfgNodeValues("/Devolve/Other/DocumentName");
            for (int i = 0; i < ucAttachment.UCDataList.Count; i++)
            {
                FounderSoftware.ADIM.OA.OA2DP.HNDP_CAttachmentNode at = new HNDP_CAttachmentNode();
                CFuJian file = ucAttachment.UCDataList[i];

                at.DocumentName = "请示报告";//file.FileName;
                at.FK_FileID = "";
                at.MakeDate = "";
                at.MakeUnit = "";
                at.PublishedTime = "";
                at.Remark = "";

                at.ServerWeb = sServerWeb;
                Double iSize = 0;
                string sSize = file.Size.ToUpper();
                if (sSize.Contains("K"))
                {
                    sSize = sSize.Replace("K", "");
                    iSize = Convert.ToDouble(sSize);
                    iSize = iSize * 1024;
                }
                if (sSize.Contains("M"))
                {
                    sSize = sSize.Replace("M", "");
                    iSize = Convert.ToDouble(sSize);
                    iSize = iSize * 1024 * 1024;
                }
                at.Size = ((int)iSize).ToString();
                at.TimeSize = "";
                at.Title = file.Alias;
                at.Type = file.Type;
                at.Url = file.URL;

                sXml = hn_oa2dp.GenOAAttachmentNode(fl.FK_ArchiveID, at);
            }
            #endregion
            /****完成生成Attachment节点******************************************************************/

            return sXml;
        }
    }
}
