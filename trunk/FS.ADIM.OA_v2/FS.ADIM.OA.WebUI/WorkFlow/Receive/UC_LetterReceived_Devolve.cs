﻿using System;
using System.Web;
using FS.ADIM.OA.BLL.Entity;
using FounderSoftware.ADIM.OA.OA2DC;
using FS.ADIM.OA.BLL.Busi.Process;
using FounderSoftware.ADIM.OA.OA2DP;
using FS.ADIM.OA.BLL.Common.Utility;
using System.Collections.Generic;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive
{
    public partial class UC_LetterReceived
    {
        private string Devolve2DC(HN_OA2DC oaDev)
        {
            string sXml = "";
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CArchiveNode ar = new HNDC_CArchiveNode();
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CFileNode fl = new HNDC_CFileNode();

            //if (String.IsNullOrEmpty(this.id)) return "";

            B_HSEdit l_BusReceiveEdit = new B_HSEdit();
            l_BusReceiveEdit.ID = Convert.ToInt32(B_ReceiveEdit.GetID(base.TemplateName, this.txtReceiveNo.Text));
            //l_BusReceiveEdit.Load(Convert.ToInt32(REGISTER_ID));

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


            // 编制单位-单位
            ar.FormationDept = l_BusReceiveEdit.CommunicationUnit;
            // 题名-文件标题
            ar.Title = l_BusReceiveEdit.DocumentTitle;
            // 收文号-收文号
            ar.ReceiveCode = l_BusReceiveEdit.DocumentNo;
            // 通讯渠道号-文件编码
            ar.CommunicationChannelCode = l_BusReceiveEdit.FileEncoding;
            // 纸质文件接收日期-收文日期
            ar.PaperDocumentTransceiverTime = this.ucPrint.CheckDateTime(l_BusReceiveEdit.ReceiptDate.ToString());
            // 批准日期-形成日期
            ar.AuthorizeTime = this.ucPrint.CheckDateTime(l_BusReceiveEdit.FormationDate.ToString());
            // 缓急程度-紧急程度
            ar.Pace = l_BusReceiveEdit.UrgentDegree;
            // 页数-页数
            ar.DocPages = l_BusReceiveEdit.Pages.ToString();
            // 拟办人-拟办人
            ar.Proposer = txtPlotMember.Text;
            // 拟办日期-拟办日期
            ar.ProposeDate = txtPlotTime.Text;
            // 批示人-公司领导
            ar.Instructioner = txtLeadShipName.Text;
            // 批示日期-批示日期
            ar.InstructionDate = txtLeadShipTime.Text;
            // 承办部门-承办部门
            ar.UndertakeDepartment = txtUnderTake.Text;
            // 登记时间
            ar.ImporterTime = DateTime.Now.ToString();

            // 公共常量
            //ar.GatherLevel = "件";
            //ar.D_StorageCarrierType = "纸质";
            //ar.Amount = "1";
            //ar.D_Language = "中文";
            ar.ElectronicDocumentCount = l_BusReceiveEdit.FileList.Count.ToString();
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
            for (int i = 0; i < l_BusReceiveEdit.FileList.Count; i++)
            {
                HNDC_CAttachmentNode at = new HNDC_CAttachmentNode();
                CFuJian file = l_BusReceiveEdit.FileList[i];

                at.DocumentName = "函件收文";//file.FileName;
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

        private string Devolve2DP(FounderSoftware.ADIM.OA.OA2DP.HN_OA2DP hn_oa2dp)
        {
            string sXml = "";
            //#region ArchiveNode
            //FounderSoftware.ADIM.OA.OA2DP.HNDP_CArchiveNode ar = new HNDP_CArchiveNode();
            //ar.System = "OA_" + base.TableID;
            //// Pace-缓急程度
            ////ar.Pace = chkJinJi.Checked ? "0" : "1";
            //// 页数
            //ar.DocPages = txtPages.Text;
            //// 通讯渠道号
            //ar.CommunicationChannelCode = txtOurRef.Text;
            //#region 签发人 签发日期
            //if (txtSignDate.Text != "")
            //{
            //    if (txtSignDate.Text.Contains(" "))
            //    {
            //        //签发日期-批准日期
            //        ar.AuthorizeTime = txtSignDate.Text.Split(' ')[1];
            //    }
            //    else
            //    {
            //        //签发日期-批准日期
            //        ar.AuthorizeTime = txtSignDate.Text;
            //    }
            //    //签发人-批准者
            //    ar.Authorizer = txtQianFaRen.Text;
            //}
            //#endregion
            //// 会签者
            ////ar.Countersigner = txtHuiQianRen.Text;
            //// 会签日期
            ////ar.CountersignedDate = txtHuiQianRenDates.Text;
            //#region 核稿人-校核者
            //////核稿人-校核者
            ////if (txtHeGaoRenDate.Text != "")
            ////{
            ////    if (txtHeGaoRenDate.Text.Contains(" "))
            ////    {
            ////        //核稿日期-校核日期
            ////        ar.CheckDate = txtHeGaoRenDate.Text.Split(' ')[1];
            ////    }
            ////    else
            ////    {
            ////        //核稿日期-校核日期
            ////        ar.CheckDate = txtHeGaoRenDate.Text;
            ////    }
            ////    //核稿人-校核者
            ////    ar.Checkthose = txtHeGaoRenDate.Text;
            ////}
            //#endregion
            //// 编制人/编制日期
            //// 编制单位
            //ar.FormationDept = drpSendDept.SelectedItem.Text;
            //// 题名
            //ar.Title = txtSubject.Text;
            //// 主送单位
            //ar.MainDispenseUnit = txtCompany.Text;
            //// 抄送单位
            //ar.RelatedDespenseUnit = txtccCompany.Text;
            //// 备注
            //ar.Remark = txtMyPrompt.Text;
            ////ar.FK_CategoryID = "903";
            //// 公共常量
            ////ar.GatherLevel = "件";
            ////ar.D_StorageCarrierType = "纸质";
            ////ar.Amount = "1";
            ////ar.D_Language = "中文";
            //ar.ElectronicDocumentCount = UCFileControl1.UCDataList.Count.ToString();
            //#endregion
            //sXml = hn_oa2dp.GenOAArchiveNode(ar, base.TemplateID);

            //FounderSoftware.ADIM.OA.OA2DP.HNDP_CFileNode fl = new HNDP_CFileNode();
            ///****开始生成File节点************************************************************************/
            //#region 开始生成File节点
            //fl.AuthorizeTime = ar.AuthorizeTime;
            //fl.Code = ar.Code;
            //fl.Code19 = ar.Code19;
            //fl.D_FileStatus = ar.D_FileStatus;
            //fl.DocCodesExplain = "";
            //fl.DocPages = ar.DocPages;
            //fl.ElectronicDocumentTransceiverTime = ar.ElectronicDocumentTransceiverTime;
            //fl.Ext_1 = ar.Ext_1;
            //fl.Ext_2 = ar.Ext_2;
            //fl.Ext_3 = ar.Ext_3;
            //fl.Ext_4 = ar.Ext_4;
            //fl.Ext_5 = ar.Ext_5;
            //fl.Title = ar.Title;
            ////fl.FK_ArchiveID = ar.FK_Archive;
            //fl.FK_CategoryID = ar.FK_CategoryID;
            //fl.FormationDept = ar.FormationDept;
            //fl.FormationTime = ar.FormationTime;
            //fl.Importer = ar.Importer;
            //fl.ImporterTime = ar.ImporterTime;
            //fl.OriginalID = ar.OriginalID;
            //fl.OtherTitle = ar.OtherTitle;
            //fl.PaperDocumentTransceiverTime = ar.PaperDocumentTransceiverTime;
            //fl.RelatedCode = ar.RelatedCode;
            //fl.Revision = ar.Revision;
            //#endregion
            //sXml = hn_oa2dp.GenOAFileNode(fl);
            ///****完成生成File节点************************************************************************/

            //#region 始生成Attachment节点
            ///****开始生成Attachment节点******************************************************************/
            //string sServerWeb = hn_oa2dp.GetCfgNodeValues("/Devolve/Other/ServerWeb");
            ////"http://172.29.128.239";
            ////string sDocumentName = oaDev.GetCfgNodeValues("/Devolve/Other/DocumentName");
            //for (int i = 0; i < UCFileControl1.UCDataList.Count; i++)
            //{
            //    FounderSoftware.ADIM.OA.OA2DP.HNDP_CAttachmentNode at = new HNDP_CAttachmentNode();
            //    CFuJian file = UCFileControl1.UCDataList[i];

            //    at.DocumentName = "函件发文";//file.FileName;
            //    at.FK_FileID = "";
            //    at.MakeDate = "";
            //    at.MakeUnit = "";
            //    at.PublishedTime = "";
            //    at.Remark = "";

            //    at.ServerWeb = sServerWeb;
            //    Double iSize = 0;
            //    string sSize = file.Size.ToUpper();
            //    if (sSize.Contains("K"))
            //    {
            //        sSize = sSize.Replace("K", "");
            //        iSize = Convert.ToDouble(sSize);
            //        iSize = iSize * 1024;
            //    }
            //    if (sSize.Contains("M"))
            //    {
            //        sSize = sSize.Replace("M", "");
            //        iSize = Convert.ToDouble(sSize);
            //        iSize = iSize * 1024 * 1024;
            //    }
            //    at.Size = ((int)iSize).ToString();
            //    at.TimeSize = "";
            //    at.Title = file.Alias;
            //    at.Type = file.Type;
            //    at.Url = file.URL;

            //    sXml = hn_oa2dp.GenOAAttachmentNode(fl.FK_ArchiveID, at);
            //}
            //#endregion
            ///****完成生成Attachment节点******************************************************************/

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

            //B_LetterReceive l_busWorkItems = new B_LetterReceive();
            //PopulateEntity(l_busWorkItems, base.ViewIDorName, base.WorkItemID);
            B_LetterReceive l_busWorkItems = this.ControlToEntity(false) as B_LetterReceive;
            l_busWorkItems.FormsData = XmlUtility.SerializeXml(l_busWorkItems);
            if (objDevolve.ToUpper() == "DC" || objDevolve.ToUpper() == "FC")
            {
                FounderSoftware.ADIM.OA.OA2DC.OA2DC oa2dc = new FounderSoftware.ADIM.OA.OA2DC.OA2DC();
                xml = Devolve2DC2(dcDev);

                s = oa2dc.SendDevolve(xml, l_busWorkItems.FormsData, "函件收文");
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


            if (objDevolve.ToUpper() == "DP")
            {
                FounderSoftware.ADIM.OA.OA2DP.OA2DP oa2dp = new FounderSoftware.ADIM.OA.OA2DP.OA2DP();
                xml = Devolve2DP(dpDev);

                s = oa2dp.SendDevolve(xml, l_busWorkItems.FormsData, "函件收文");
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
            //string rest = oa2dc.CallBack(Convert.ToInt32(s), true, s);

            //IMessage ms = new WebFormMessage(Page, s);
            //ms.Show();
        }

        private string Devolve2DC2(HN_OA2DC oaDev)
        {
            string sXml = "";

            List<FounderSoftware.ADIM.OA.OA2DC.DevKVItem> ls = oaDev.MapFunction("DC", base.TemplateName);

            FounderSoftware.ADIM.OA.OA2DC.HNDC_CArchiveNode ar = new HNDC_CArchiveNode();
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CFileNode fl = new HNDC_CFileNode();

            //if (String.IsNullOrEmpty(this.id)) return "";

            B_HSEdit l_BusReceiveEdit = new B_HSEdit();
            l_BusReceiveEdit.ID = Convert.ToInt32(B_ReceiveEdit.GetID(base.TemplateName, this.txtReceiveNo.Text));
            //l_BusReceiveEdit.Load(Convert.ToInt32(REGISTER_ID));
            B_LetterReceive pEntity = ControlToEntity(false) as B_LetterReceive;
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
                string value = oaDev.PreHandel(l_BusReceiveEdit, sName, sType);
                if (string.IsNullOrEmpty(value))
                {
                    value = oaDev.PreHandel(pEntity, sName, sType);
                }
                ar.SetAttribute(skey, value/*(entity.GetVal(sName) as string)*/);
            }
            ar.SetOAArchiveNode();
            // 其他节点处理应放置在SetOAArchiveNode方法之后
            
            //return Devolve2DP(oaDev, sDPID, ar);
            #endregion 根据DevolveConfig.xml配置参数设置Archive归档节点

            ar.ElectronicDocumentCount = pEntity.FileList.Count.ToString();
                                         //l_BusReceiveEdit.FileList.Count.ToString();
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
            for (int i = 0; i < pEntity.FileList.Count; i++)
            {
                HNDC_CAttachmentNode at = new HNDC_CAttachmentNode();
                CFuJian file = pEntity.FileList[i];

                at.DocumentName = "函件收文";//file.FileName;
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

        private string Devovle2DP2(HN_OA2DP hn_oa2dp, string sDPID)
        {
            string sXml = "";

            List<FounderSoftware.ADIM.OA.OA2DP.DevKVItem> ls = hn_oa2dp.MapFunction("DP", base.TemplateName);
            FounderSoftware.ADIM.OA.OA2DP.HNDP_CArchiveNode ar = new HNDP_CArchiveNode();

            B_HSEdit l_BusReceiveEdit = new B_HSEdit();
            l_BusReceiveEdit.ID = Convert.ToInt32(B_ReceiveEdit.GetID(base.TemplateName, this.txtReceiveNo.Text));
            

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
                ar.SetAttribute(skey, hn_oa2dp.PreHandel(l_BusReceiveEdit, sName, sType, true)/*(entity.GetVal(sName) as string)*/);
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

                at.DocumentName = "函件收文";//file.FileName;
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
