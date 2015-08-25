/*****************************************************************/
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：程序文件子类维护
//
// 创 建 者：黄 琦
// 创建标识: C_2010.01.12
//
// 修改标识：
// 修改描述：
/*****************************************************************/
using System;
using System.Web;
using FounderSoftware.ADIM.OA.OA2DC;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using System.Collections.Generic;

namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC2_ProgramFile
    {
        private string CheckDateTime(string strDateTime)
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

        private string Devolve2DC(HN_OA2DC oaDev, B_PF enProFile, string strDraftDept, string dealUser, string sProcessName)
        {
            string sXml = "";
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CArchiveNode ar = new HNDC_CArchiveNode();
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CFileNode fl = new HNDC_CFileNode();

            #region 开始生成Archive节点
            ar.System = "OA_" + base.IdentityID.ToString();
            string ObjPlatForm = oaDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessName + "']/Object");
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


            // 文件编码-程序编码
            ar.Code = enProFile.ProgramCode;
            // 编制单位-主办部门
            ar.FormationDept = strDraftDept;
            // 题名-程序名称
            ar.Title = enProFile.DocumentTitle;
            // 功能领域-程序子类
            ar.FunctionField = B_ProgramFileInfo.GetProSubTypeName(enProFile.ProgramFileID);
            // 版本-版次
            ar.Revision = enProFile.Edition;
            // 页数-总页数
            ar.DocPages = enProFile.TextPageSum;
            // 批准者-批准操作人
            ar.Authorizer = enProFile.ApproveName;
            // 生效日期-批准日期
            ar.AuthorizeTime = CheckDateTime(enProFile.ApproveDate.ToString());
            // 审核者-审核操作人
            ar.Auditby = enProFile.AuditName;
            // 审核日期-审核日期
            ar.AuditDate = CheckDateTime(enProFile.AuditDate.ToString());
            // 校核者-校核操作人
            ar.Checkthose = enProFile.CheckName;
            // 校核日期-校核日期
            ar.CheckDate = CheckDateTime(enProFile.CheckDate.ToString());
            // 编制人-编制操作人
            ar.Author = enProFile.WriteName;
            // 编制日期-编制日期
            ar.FormationTime = CheckDateTime(enProFile.WriteDate.ToString());


            //ar.FK_CategoryID = "903";
            // 公共常量
            //ar.GatherLevel = "件";
            //ar.D_StorageCarrierType = "纸质";
            //ar.Amount = "1";
            //ar.D_Language = "中文";
            ar.ElectronicDocumentCount = enProFile.FileList.Count.ToString();
            #endregion
            sXml = oaDev.GenOAArchiveNode(ar, sProcessName);

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
            for (int i = 0; i < enProFile.FileList.Count; i++)
            {
                HNDC_CAttachmentNode at = new HNDC_CAttachmentNode();
                CFuJian file = enProFile.FileList[i];

                at.DocumentName = ProcessConstString.TemplateName.PROGRAM_FILE; //file.FileName;
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

        #region 程序文件归档到DP
        //private string Devolve2DP(FounderSoftware.ADIM.OA.OA2DP.HN_OA2DP hn_oa2dp)
        //{
        //string sXml = "";
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

        //    return sXml;
        //}
        #endregion

        public string Devolve(B_PF enProFile, string strDraftDept, string dealUser, out string sResult)
        {
            sResult = "";
            string sProcessName = "";
            switch (enProFile.ProgramSort)
            {
                case "部门级管理程序":
                    if ("IDP部门管理程序" == enProFile.ProgramType1)
                    {
                        sProcessName = "部门管理程序";
                    }
                    break;
                case "管理程序":
                    switch (enProFile.ProgramType1)
                    {
                        case "PRG项目管理分大纲":
                        case "GPG质量保证大纲":
                            sProcessName = "大纲级管理程序";
                            break;
                        case "PRC公司管理程序":
                            sProcessName = "公司级管理程序";
                            break;
                    }
                    break;
                case "工作程序":
                    sProcessName = enProFile.ProgramSort;
                    break;
            }

            string sPath = HttpContext.Current.Server.MapPath((@"~\Config\DevolveConfig.xml"));
            FounderSoftware.ADIM.OA.OA2DC.HN_OA2DC dcDev = new HN_OA2DC(sPath);
            //FounderSoftware.ADIM.OA.OA2DP.HN_OA2DP dpDev = new HN_OA2DP(sPath);

            // 开始发送归档
            /*
             * 根据DevoleConfig.xml配置文件的中相应流程的<Object>DC</Object>节点来判断归档到哪个系统中
             * 若配置中为DC则表明调用OA2DC.DLL接口进行归档,若为DP则表明调用OA2DP.DLL接口进行归档
             */
            string xml = "";
            string objDevolve = dcDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessName + "']/Object");
            string s = "";
            if (objDevolve.ToUpper() == "DC" || objDevolve.ToUpper() == "FC")
            {
                FounderSoftware.ADIM.OA.OA2DC.OA2DC oa2dc = new FounderSoftware.ADIM.OA.OA2DC.OA2DC();
                xml = Devolve2DC2(dcDev, enProFile, strDraftDept, dealUser, sProcessName);

                //B_PF cEntity = new B_PF();
                //SetEntity(cEntity);
                B_PF cEntity = this.ControlToEntity(false) as B_PF;
                cEntity.FormsData = XmlUtility.SerializeXml(cEntity);
                s = oa2dc.SendDevolve(xml, cEntity.FormsData, "程序文件");
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
            else
            {
                return "尚无归档模板，请联系系统管理员。";
            }

            return s;
            //if (objDevolve.ToUpper() == "DP")
            //{
            //    FounderSoftware.ADIM.OA.OA2DP.OA2DP oa2dp = new FounderSoftware.ADIM.OA.OA2DP.OA2DP();
            //    xml = Devolve2DP(dpDev);
            //    s = oa2dp.SendDevolve(xml);
            //}
            //string rest = oa2dc.CallBack(Convert.ToInt32(s), true, s);

            //IMessage ms = new WebFormMessage(Page, s);
            //ms.Show();
        }

        private string Devolve2DC2(HN_OA2DC oaDev, B_PF enProFile, string strDraftDept, string dealUser, string sProcessName)
        {
            string sXml = "";

            List<FounderSoftware.ADIM.OA.OA2DC.DevKVItem> ls = oaDev.MapFunction("DC", sProcessName);

            FounderSoftware.ADIM.OA.OA2DC.HNDC_CArchiveNode ar = new HNDC_CArchiveNode();
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CFileNode fl = new HNDC_CFileNode();

            #region 开始生成Archive节点
            ar.System = "OA_" + base.IdentityID.ToString();
            string ObjPlatForm = oaDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessName + "']/Object");
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
                ar.SetAttribute(skey, oaDev.PreHandel(enProFile, sName, sType)/*(entity.GetVal(sName) as string)*/);
            }
            ar.SetOAArchiveNode();
            // 其他节点处理应放置在SetOAArchiveNode方法之后
            // 编制单位-主办部门
            ar.FormationDept = strDraftDept;
            // 功能领域-程序子类
            ar.FunctionField = B_ProgramFileInfo.GetProSubTypeName(enProFile.ProgramFileID);

            //return Devolve2DP(oaDev, sDPID, ar);
            #endregion 根据DevolveConfig.xml配置参数设置Archive归档节点

            ar.ElectronicDocumentCount = enProFile.FileList.Count.ToString();
            #endregion
            sXml = oaDev.GenOAArchiveNode(ar, sProcessName);

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
            for (int i = 0; i < enProFile.FileList.Count; i++)
            {
                HNDC_CAttachmentNode at = new HNDC_CAttachmentNode();
                CFuJian file = enProFile.FileList[i];

                at.DocumentName = ProcessConstString.TemplateName.PROGRAM_FILE; //file.FileName;
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
    }
}
