using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FounderSoftware.ADIM.OA.OA2DC;
using FounderSoftware.ADIM.OA.OA2DP;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using LsEntity = System.Collections.Generic.List<FS.ADIM.OA.BLL.Entity.EntityBase>;
using FS.ADIM.OA.WebUI;
using System.Threading;

namespace FS.ADIM.OA.WebUI.ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DevolveHandler : IHttpHandler
    {
        int iTimeout = 0;
        int iSuccess = 0;
        int iFail = 0;
        public void ProcessRequest(HttpContext context)
        {
            //while (true)
            //{
            //    if (SelItems != null && SelItems.Count > 0)
            //    {
            //        SelItems = FS.ADIM.OA.WebUI.BatchDevolve.SelItems;
            //        break;
            //    }
            //    else
            //    {
            //        Thread.Sleep(100);
            //        if ((iTimeout += 100) > 5000)
            //        {
            //            break;
            //        }
            //    }
            //}

            //context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Cache-Control", "no-cache");
            //context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Request.ContentEncoding = System.Text.Encoding.UTF8;

            string sdtStart = context.Request["dtStart"];
            string sdtEnd = context.Request["dtEnd"];
            string sProc = context.Request["sProc"];
            string sStep = context.Request["sStep"];

            string[] arr = context.Request["arr"].Split(',');
            
            DateTime dtStart = DateTime.Parse(string.IsNullOrEmpty(sdtStart) ? DateTime.MinValue.ToString() : sdtStart);
            DateTime dtEnd = DateTime.Parse(string.IsNullOrEmpty(sdtEnd) ? DateTime.MinValue.ToString() : sdtEnd);

            LsEntity entitys = GetList(sProc, sStep, dtStart, dtEnd);
            string sResult = "";
            string sPlatForm = "";
            string sFinalResult = "归档完成! " + DateTime.Now.ToString();
            if (entitys.Count == 0)
            {
                sFinalResult = "无归档记录! " + DateTime.Now.ToString();
                context.Response.Write(sFinalResult);
                context.Response.End();
            }
            for (int i = 0; i < entitys.Count; i++)
            {
                if (IsExist(entitys[i], arr))
                {
                    try
                    {
                        Devolve(entitys[i], sProc, out sResult, out sPlatForm);
                        iSuccess++;
                    }
                    catch (Exception ex)
                    {
                        sFinalResult += (entitys[i].ID.ToString() + ex.Message + "\r\n");
                        iFail++;
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            sFinalResult += "\r\n\r\n成功归档 " + iSuccess.ToString() + " 条";
            sFinalResult += "\r\n失败归档 " + iFail.ToString() + " 条";
            string sPlat = "无效";
            switch (sPlatForm)
            { 
                case "DC":
                    sPlat = "文档中心(DC)";
                    break;
                case "FC":
                    sPlat = "文件中心(FC)";
                    break;
                case "DP":
                    sPlat = "处室平台(DP)";
                    break;
            }
            sFinalResult += "\r\n归档目标平台 " + sPlat;
            context.Response.Write(sFinalResult);
        }

        private bool IsExist(EntityBase entity, string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (entity.ID.ToString() == (arr.GetValue(i) as string))
                    return true;
                else
                    continue;
            }
            return false;
        }

        public void Devolve(EntityBase entity, string sProcessType, out string sResult, out string sPlatForm)
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
            string objDevolve = dcDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessType + "']/Object");
            string s = "";
            sPlatForm = objDevolve.ToUpper();

            entity.FormsData = XmlUtility.SerializeXml(entity);
            if (objDevolve.ToUpper() == "DC" || objDevolve.ToUpper() == "FC")
            {
                FounderSoftware.ADIM.OA.OA2DC.OA2DC oa2dc = new FounderSoftware.ADIM.OA.OA2DC.OA2DC();
                xml = Devolve2DC2(entity, dcDev, sProcessType);

                s = oa2dc.SendDevolve(xml, entity.FormsData, sProcessType);
                sResult += (s + @"\r\n");
                try
                {
                    Convert.ToInt32(s);
                }
                catch
                {
                    throw new Exception(s);
                }
                B_ProcessInstance.ProcessDevolve(entity.ProcessID, sProcessType);
            }

            if (objDevolve.ToUpper() == "DP")
            {
                FounderSoftware.ADIM.OA.OA2DP.OA2DP oa2dp = new FounderSoftware.ADIM.OA.OA2DP.OA2DP();
                string sOADPIDs = dpDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessType + "']/DPID");

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
                    xml = Devovle2DP2(entity, dpDev, arrDPID[i], sProcessType);
                    s = oa2dp.SendDevolve(xml, entity.FormsData, sProcessType);
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

                string sOptionDPID = dpDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessType + "']/OptionDPID");
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
                        xml = Devovle2DP2(entity, dpDev, IDs[j], sProcessType);
                        s = oa2dp.SendDevolve(xml, entity.FormsData, sProcessType);
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
                }

                B_ProcessInstance.ProcessDevolve(entity.ProcessID, sProcessType);
            }
        }

        private string Devolve2DC2(EntityBase entity, HN_OA2DC oaDev, string sProcessType)
        {
            string sXml = "";

            List<FounderSoftware.ADIM.OA.OA2DC.DevKVItem> ls = oaDev.MapFunction("DC", sProcessType);

            //B_GS_WorkItems entity = this.ControlToEntity(false) as B_GS_WorkItems;
            entity.FormsData = XmlUtility.SerializeXml(entity);

            FounderSoftware.ADIM.OA.OA2DC.HNDC_CArchiveNode ar = new HNDC_CArchiveNode();
            FounderSoftware.ADIM.OA.OA2DC.HNDC_CFileNode fl = new HNDC_CFileNode();

            //string sPath = @"E:\QWDMS\Non Baseline Library\Development\03 Code\FounderSoftware.ADIM\FounderSoftware.ADIM.OA\OADevolveLib\DevolveConfig.xml";

            #region 开始生成Archive节点
            ar.System = "OA_" + entity.ID.ToString();
            string ObjPlatForm = oaDev.GetCfgNodeValues("/Devolve/Process[@Name='" + sProcessType + "']/Object");
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

            ar.ElectronicDocumentCount = entity.FileList.Count.ToString();
            #endregion
            sXml = oaDev.GenOAArchiveNode(ar, sProcessType);

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
            for (int i = 0; i < entity.FileList.Count; i++)
            {
                HNDC_CAttachmentNode at = new HNDC_CAttachmentNode();
                CFuJian file = entity.FileList[i];

                at.DocumentName = sProcessType;//file.FileName;
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

        private string Devovle2DP2(EntityBase entity, HN_OA2DP hn_oa2dp, string sDPID, string sProcessType)
        {
            string sXml = "";
            FounderSoftware.ADIM.OA.OA2DP.HNDP_CArchiveNode ar = new HNDP_CArchiveNode();

            List<FounderSoftware.ADIM.OA.OA2DP.DevKVItem> ls = hn_oa2dp.MapFunction("DP", sProcessType);

            //B_GS_WorkItems entity = this.ControlToEntity(false) as B_GS_WorkItems;
            entity.FormsData = XmlUtility.SerializeXml(entity);

            //ar.SetAttribute("FormationTime"/*配置文件中的归档字段*/, "2009-12-12"/*cEntity.GetValue(配置文件中的实体属性名)*/);
            //string sValue = ar.GetAttribute("FormationTime");
            #region 生成Archive节点
            ar.System = "OA_" + entity.ID.ToString();

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

            ar.ElectronicDocumentCount = entity.FileList.Count.ToString();
            #endregion
            sXml = hn_oa2dp.GenOAArchiveNode(ar, sProcessType);

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
            for (int i = 0; i < entity.FileList.Count; i++)
            {
                FounderSoftware.ADIM.OA.OA2DP.HNDP_CAttachmentNode at = new HNDP_CAttachmentNode();
                CFuJian file = entity.FileList[i];

                at.DocumentName = sProcessType;//file.FileName;
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

        public LsEntity GetList(string sProc, string sStep, DateTime dtStart, DateTime dtEnd )
        {
            return B_FormsData.GetEntities(sProc, sStep, dtStart, dtEnd, true);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
