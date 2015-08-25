using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FS.ADIM.OA.WebUI.MOSSOA;
using FS.OA.Framework;
using System.IO;
using System.Data;
using FS.ADIM.OA.BLL.Entity;
using Brettle.Web.NeatUpload;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.MOSSS;
using FS.ADIM.OA.BLL.Common;
using System.Data.SqlClient;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common.Utility;
using FounderSoftware.ADIM.SSO.Utility;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public class MossObject
    {
        public static int middleFileSize = OAConfig.GetConfig("MOSS附件大小设置", "MiddleFileSize") == null ? 30 : int.Parse(OAConfig.GetConfig("MOSS附件大小设置", "MiddleFileSize"));
        public static int maxFileSize = OAConfig.GetConfig("MOSS附件大小设置", "MaxFileSize") == null ? 200 : int.Parse(OAConfig.GetConfig("MOSS附件大小设置", "MaxFileSize"));

        public static OA_DocumentService GetMOSSAPI()
        {
            string user = OAConfig.GetConfig("MOSS认证", "Credential_User");
            string pwd = OAConfig.GetConfig("MOSS认证", "Credential_Pwd");
            string domain = OAConfig.GetConfig("MOSS认证", "Credential_Domain");
            string url = OAConfig.GetConfig("MOSS认证", "MossServiceUrl");

            //OA_DocumentService
            OA_DocumentService api = new OA_DocumentService();
            api.Credentials = new System.Net.NetworkCredential(user, pwd, domain);
            api.Url = url;

            try
            {
                int ret = api.Test();
            }
            catch
            {
                api.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            return api;
        }

        public static OA_DocumentService GetMOSSOldAPI()
        {
            string user = OAConfig.GetConfig("旧版函件归档", "Credential_User");
            string pwd = OAConfig.GetConfig("旧版函件归档", "Credential_Pwd");
            string domain = OAConfig.GetConfig("旧版函件归档", "Credential_Domain");
            string url = OAConfig.GetConfig("旧版函件归档", "MossServiceUrl");

            OA_DocumentService api = new OA_DocumentService();
            api.Credentials = new System.Net.NetworkCredential(user, pwd, domain);
            api.Url = url;
            try
            {
                int ret = api.Test();
            }
            catch
            {
                api.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            return api;
        }
        
        
        private string _ServerWeb = "";
        /// <summary>
        /// 站点集名
        /// </summary>
        public string ServerWeb
        {
            get
            {
                return _ServerWeb;
            }
            set
            {
                _ServerWeb = value;
            }
        }

        private string _DocumentName = "";
        /// <summary>
        /// 文档库名
        /// </summary>
        public string DocumentName
        {
            get
            {
                return _DocumentName;
            }
            set
            {
                _DocumentName = value;
            }
        }

        private string _OldFileName = "";
        /// <summary>
        /// 原文件名
        /// </summary>
        public string OldFileName
        {
            get
            {
                return _OldFileName;
            }
            set
            {
                _OldFileName = value;
            }
        }

        private string _FolderName = "";
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                _FolderName = value;
            }
        }


        private string _FileName = "";
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }


        private string _Extension = "";
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension
        {
            get
            {
                return _Extension;
            }
            set
            {
                _Extension = value;
            }
        }

        private FS.ADIM.OA.WebUI.MOSSOA.DictionaryEntry[] _DocumentEntry = null;
        /// <summary>
        /// 文档库须更新栏位
        /// </summary>
        public FS.ADIM.OA.WebUI.MOSSOA.DictionaryEntry[] DocumentEntry
        {
            get { return _DocumentEntry; }
            set { _DocumentEntry = value; }
        }

        private string _UploadFullName = "";
        /// <summary>
        /// 返回的全路径
        /// </summary>
        public string UploadFullName
        {
            get
            {
                return _UploadFullName;
            }
            set
            {
                _UploadFullName = value;
            }
        }

        private string _UploadURL = "";
        /// <summary>
        /// 返回的文件夹+文件名
        /// </summary>
        public string UploadURL
        {
            get
            {
                return _UploadURL;
            }
            set
            {
                _UploadURL = value;
            }
        }

        private Stream _UploadFilesStream = null;
        /// <summary>
        /// 字节流
        /// </summary>
        public Stream UploadFilesStream
        {
            get
            {
                return _UploadFilesStream;
            }
            set
            {
                _UploadFilesStream = value;
            }
        }

        private byte[] _DownLoadBy = null;
        /// <summary>
        /// 下载回来的字节数组
        /// </summary>
        public byte[] DownLoadBy
        {
            get
            {
                return _DownLoadBy;
            }
            set
            {
                _DownLoadBy = value;
            }
        }

        public DictionaryEntry[] ConvertToDE(DictionaryEntry[] entrys)
        {
            OA_DocumentService api = MossObject.GetMOSSAPI();
            return api.ConvertToDE(entrys);
        }

        public bool Upload()
        {
            OA_DocumentService api = MossObject.GetMOSSAPI();
            string[] saveUrl = api.Upload(GetUploadFileInfo_new(), StreamToBytes(UploadFilesStream), this.DocumentEntry, false);  //service上传
            UploadFullName = saveUrl[0];  //全路径
            UploadURL = saveUrl[1];   //文件夹+文件名

            return true;
        }


        #region 函件收发归档到旧版ADIM
        //函件收发归档 iNum第几个附件
        public static string[] GetUploadFileInfoOld(string UCProcessType, string filePath, string documentNo, int iNum)
        {
            string[] ret = new string[4];
            if (UCProcessType == "函件发文" || UCProcessType == "新版函件发文")
                UCProcessType = OAConfig.GetConfig("旧版函件归档", "函件发文归档库");
            else
                UCProcessType = OAConfig.GetConfig("旧版函件归档", "函件收文归档库");
            ret[0] = OAConfig.GetConfig("旧版函件归档", "ServerWeb");
            if (UCProcessType == "")
            {
                ret[1] = "Temp";
            }
            else
            {
                ret[1] = UCProcessType;
            }
            ret[2] = ""; //旧版没文件夹
            if (iNum == 0)
            {
                ret[3] = documentNo + System.IO.Path.GetExtension(filePath); //文件名
            }
            else
            {
                ret[3] = documentNo + "[" + iNum.ToString() + "]" + System.IO.Path.GetExtension(filePath); //文件名
            }
            return ret;
        }
        public static string[] GetDownLoadFileInfo(string UCProcessType, string filePath)
        {
            string[] ret = new string[4];
            ret[0] = OAConfig.GetConfig("MOSS认证", "ServerWeb");
            if (UCProcessType == "")
            {
                ret[1] = "Temp";
            }
            else
            {
                ret[1] = UCProcessType;
            }

            int index = filePath.LastIndexOf('/');

            ret[2] = filePath.Substring(0, index);

            ret[3] = filePath.Substring(index + 1, filePath.Length - index - 1);
            return ret;
        }
        #endregion

        #region 附件处理方法
        //获得上传路径,文件信息的数组
        public string[] GetUploadFileInfo_new()
        {
            if (ServerWeb == "")
            {
                throw new Exception("文档库站点不能为空！");
            }
            if (DocumentName == "")
            {
                throw new Exception("文档库名不能为空！");
            }
            if (OldFileName == "")
            {
                throw new Exception("原文件名不能为空！");
            }
            string extension = System.IO.Path.GetExtension(OldFileName);
            if (extension != "")
            {
                if (extension.Contains("."))
                {
                    if (extension.IndexOf(".") > 0)
                    {
                        extension = "." + extension;
                    }
                }
                else
                {
                    extension = "." + extension;
                }
            }
            string[] ret = new string[4];
            ret[0] = ServerWeb;
            ret[1] = DocumentName;
            ret[2] = GetFolderName(); //文件夹
            ret[3] = GetFileName() + extension; //文件名

            FolderName = ret[2];
            FileName = ret[3];
            return ret;
        }
        public static string[] GetUploadFileInfo(string UCProcessType, string filePath)
        {
            string[] ret = new string[4];
            ret[0] = OAConfig.GetConfig("MOSS认证", "ServerWeb");
            if (UCProcessType == "")
            {
                ret[1] = "Temp";
            }
            else
            {
                ret[1] = UCProcessType;
            }
            ret[2] = MossObject.GetFolderName(); //文件夹
            ret[3] = MossObject.GetFileName() + System.IO.Path.GetExtension(filePath); //文件名
            return ret;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            stream.Close();
            return bytes;
        }
        public static string ToFileSize(long fSize)
        {
            string StrSize = "";
            if (fSize < 1024)
            {
                StrSize = fSize + "字节";
            }
            else if (fSize >= 1024 && fSize < 1024 * 1024)
            {
                StrSize = ((double)fSize / 1024).ToString(".##") + "K";
            }
            else if (fSize >= 1024 * 1024 && fSize < 1024 * 1024 * 1024)
            {
                StrSize = ((double)fSize / 1024 / 1024).ToString(".##") + "M";
            }
            else if (fSize >= 1024 * 1024 * 1024 && fSize < unchecked(1024 * 1024 * 1024 * 1024))
            {
                StrSize = ((double)fSize / 1024 / 1024 / 1024).ToString(".##") + "M";
            }
            return StrSize;
        }
        public string ToFileSize_new(long fSize)
        {
            string StrSize = "";
            if (fSize < 1024)
            {
                StrSize = fSize + "字节";
            }
            else if (fSize >= 1024 && fSize < 1024 * 1024)
            {
                StrSize = ((double)fSize / 1024).ToString(".##") + "K";
            }
            else if (fSize >= 1024 * 1024 && fSize < 1024 * 1024 * 1024)
            {
                StrSize = ((double)fSize / 1024 / 1024).ToString(".##") + "M";
            }
            else if (fSize >= 1024 * 1024 * 1024 && fSize < unchecked(1024 * 1024 * 1024 * 1024))
            {
                StrSize = ((double)fSize / 1024 / 1024 / 1024).ToString(".##") + "M";
            }
            return StrSize;
        }
        //获得当前年月的文件夹名
        public static string GetFolderName()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            return year + month;
        }
        //获得当前日期的文件名
        public static string GetFileName()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();
            string millisecond = DateTime.Now.Millisecond.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            if (hour.Length == 1)
            {
                hour = "0" + hour;
            }
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            Random r = new Random();
            int x = r.Next(0, 9);
            return year + month + day + hour + minute + second + millisecond + x.ToString();
        }


        private string GetProcessType(string workItemID)
        {
            string sql = string.Format(@"SELECT DEF_NAME FROM dbo.WF_MANUAL_WORKITEMS a
INNER JOIN 
WF_PROC_DEFS b on a.PROC_DEF_ID=b.DEF_ID
WHERE WORK_ITEM_ID='{0}'", workItemID);

            DataTable dt = SQLHelper.GetDataTable2(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                throw new Exception(workItemID + "未找到对应的流程类型！");
            }

        }
        #endregion

        #region 处理历史附件
        //以下为处理历史附件
        public List<CFuJian> GetNewListFuJian(string UCProcessType, string workItemID, List<CFuJian> list)
        {
            try
            {
                if (workItemID == "") //第一次 草稿箱
                {
                    return list;
                }
                string serverWeb = OAConfig.GetConfig("MOSS认证", "ServerWeb");

                bool isNewList = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].WorkItemID != workItemID)
                    {
                        string[] ret = new string[4];
                        ret[0] = serverWeb;
                        ret[1] = list[i].ProcessType;
                        ret[2] = list[i].FolderName; //文件夹
                        ret[3] = list[i].FileName; //文件名

                        string newFileName = GetFileName() + System.IO.Path.GetExtension(list[i].FileName);

                        string[] newRet = null;
                        if (OAConfig.GetConfig("MOSS认证", "是否启用DLL") == "1")
                        {
                            newRet = DocumentManager.CopyTo(ret, newFileName, true);
                        }
                        else
                        {
                            newRet = GetMOSSAPI().CopyTo(ret, newFileName, true);
                        }
                        list[i].Title = newFileName;
                        list[i].FileName = newFileName; //新文件名
                        list[i].fullURL = newRet[0];
                        list[i].URL = newRet[1];
                        list[i].Edition = list[i].Edition + 1;
                        list[i].WorkItemID = workItemID;
                        isNewList = true;
                    }
                }
                int ret2 = 0;
                if (isNewList)
                    ret2 = SaveNewList(UCProcessType, workItemID, list);

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int SaveNewList(string UCProcessType, string workItemID, List<CFuJian> newList)
        {
            return SaveNewList(UCProcessType, workItemID, newList, "");
        }
        public int SaveNewList(string UCProcessType, string workItemID, List<CFuJian> newList, string TBID)
        {
            if (workItemID == "" && TBID == "")
            {
                return 1;
            }
            string tableName = TableName.GetWorkItemsTableName(UCProcessType);

            DataTable dt = new DataTable();
            if (workItemID != "")
            {
                string sql = string.Format("SELECT FormsData FROM {0} WHERE WorkItemID='{1}'", tableName, workItemID);
                dt = SQLHelper.GetDataTable1(sql);
            }
            else
            {
                string sql = string.Format("SELECT FormsData FROM {0} WHERE ID='{1}'", tableName, TBID);
                dt = SQLHelper.GetDataTable1(sql);
            }

            if (dt.Rows.Count > 0)
            {
                string xml = dt.Rows[0][0].ToString();
                string newXml = "";
                if (UCProcessType.Contains("新版"))
                {
                    UCProcessType = UCProcessType.Substring(2);
                }
                switch (UCProcessType)
                {

                    #region 获得新实体
                    #region 发文
                    //case "公司发文":
                    //    EntitySend entity1 = XmlUtility.DeSerializeXml<EntitySend>(xml);
                    //    if (entity1 != null)
                    //    {
                    //        entity1.FileList = newList;

                    //        newXml = XmlUtility.SerializeXml<EntitySend>(entity1);
                    //    }
                    //    break;
                    //case "党委发文":
                    //    EntitySend entity2 = XmlUtility.DeSerializeXml<EntitySend>(xml);
                    //    if (entity2 != null)
                    //    {
                    //        entity2.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<EntitySend>(entity2);
                    //    }
                    //    break;
                    //case "工会发文":
                    //    EntitySend entity3 = XmlUtility.DeSerializeXml<EntitySend>(xml);
                    //    if (entity3 != null)
                    //    {
                    //        entity3.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<EntitySend>(entity3);
                    //    }
                    //    break;
                    //case "纪委发文":
                    //    EntitySend entity4 = XmlUtility.DeSerializeXml<EntitySend>(xml);
                    //    if (entity4 != null)
                    //    {
                    //        entity4.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<EntitySend>(entity4);
                    //    }
                    //    break;
                    //case "团委发文":
                    //    M_TWF_WorkItems entity5 = XmlUtility.DeSerializeXml<M_TWF_WorkItems>(xml);
                    //    if (entity5 != null)
                    //    {
                    //        entity5.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<M_TWF_WorkItems>(entity5);
                    //    }
                    //    break;
                    #endregion

                    #region 收文
                    case "公司收文":
                        B_GS_WorkItems entity6 = XmlUtility.DeSerializeXml<B_GS_WorkItems>(xml);
                        if (entity6 != null)
                        {
                            entity6.FileList = newList;
                            newXml = XmlUtility.SerializeXml<B_GS_WorkItems>(entity6);
                        }
                        break;
                        //case "工会收文":
                        //    B_GHS_WorkItems entity7 = XmlUtility.DeSerializeXml<B_GHS_WorkItems>(xml);
                        //    if (entity7 != null)
                        //    {
                        //        entity7.FileList = newList;
                        //        newXml = XmlUtility.SerializeXml<B_GHS_WorkItems>(entity7);
                        //    }
                        //    break;
                        //case "团委收文":
                        //    B_TWS_WorkItems entity8 = XmlUtility.DeSerializeXml<B_TWS_WorkItems>(xml);
                        //    if (entity8 != null)
                        //    {
                        //        entity8.FileList = newList;
                        //        newXml = XmlUtility.SerializeXml<B_TWS_WorkItems>(entity8);
                        //    }
                        //    break;
                        //case "党委纪委收文":
                        //    B_DJS_WorkItems entity9 = XmlUtility.DeSerializeXml<B_DJS_WorkItems>(xml);
                        //    if (entity9 != null)
                        //    {
                        //        entity9.FileList = newList;
                        //        newXml = XmlUtility.SerializeXml<B_DJS_WorkItems>(entity9);
                        //    }
                        break;
                    #endregion

                    case "工作联系单":
                        B_WorkRelation entity10 = XmlUtility.DeSerializeXml<B_WorkRelation>(xml);
                        if (entity10 != null)
                        {
                            entity10.FileList = newList;
                            newXml = XmlUtility.SerializeXml<B_WorkRelation>(entity10);
                        }
                        break;
                    //case "请示报告":
                    //    B_RequestReport entity11 = XmlUtility.DeSerializeXml<B_RequestReport>(xml);
                    //    if (entity11 != null)
                    //    {
                    //        entity11.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<B_RequestReport>(entity11);
                    //    }
                    //    break;
                    //case "程序文件":
                    //    B_PF entity12 = XmlUtility.DeSerializeXml<B_PF>(xml);
                    //    if (entity12 != null)
                    //    {
                    //        entity12.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<B_PF>(entity12);
                    //    }
                    //    break;
                    case "函件发文":
                        EntityLetterSend entity13 = XmlUtility.DeSerializeXml<EntityLetterSend>(xml);
                        if (entity13 != null)
                        {
                            entity13.FileList = newList;
                            newXml = XmlUtility.SerializeXml<EntityLetterSend>(entity13);
                        }
                        break;
                    //case "函件收文":
                    //    B_LetterReceive entity14 = XmlUtility.DeSerializeXml<B_LetterReceive>(xml);
                    //    if (entity14 != null)
                    //    {
                    //        entity14.FileList = newList;
                    //        newXml = XmlUtility.SerializeXml<B_LetterReceive>(entity14);
                    //    }
                    //    break;
                    default: newXml = ""; break;
                    #endregion
                }
                if (newXml != "")
                {
                    //更新新实体
                    int ret = UpdateNewFormsData(tableName, workItemID, newXml, TBID);
                    return ret;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                //throw new Exception(UCProcessType + workItemID + tableName + "未找到对应的记录！");
                return 1;
            }
        }
        private int UpdateNewFormsData(string tableName, string workItemID, string newXml, string TBID)
        {
            int ret = 0;
            if (workItemID != "")
            {
                string sql = "";
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@FormsData",newXml),
                new SqlParameter("@WorkItemID",workItemID),
            };
                sql = string.Format("UPDATE {0} SET FormsData=@FormsData WHERE WorkItemID=@WorkItemID", tableName);
                ret = SQLHelper.ExecuteNonQuery1(sql, param);
            }
            else
            {
                string sql = "";
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@FormsData",newXml),
                new SqlParameter("@ID",TBID),
            };
                sql = string.Format("UPDATE {0} SET FormsData=@FormsData WHERE ID=@ID", tableName);
                ret = SQLHelper.ExecuteNonQuery1(sql, param);
            }

            return ret;
        }

        #endregion


        #region 上传 下载 删除
        public static CFuJian Upload(UploadedFile file, string UCProcessType, string UCProcessID, string UCWorkItemID)
        {
            CFuJian cFuJian = new CFuJian();

            OA_DocumentService api = MossObject.GetMOSSAPI();

            string[] fileInfo = GetUploadFileInfo(UCProcessType, file.FileName);

            cFuJian.Type = System.IO.Path.GetExtension(file.FileName); //文件类型 扩展名
            if (cFuJian.Type.IndexOf('.') > -1)
            {
                cFuJian.Type = cFuJian.Type.Substring(1);
            }
            cFuJian.Alias = file.FileName.Substring(0, file.FileName.Length - cFuJian.Type.Length - 1); //别名
            //ff.Title = ff.Alias + "." + ff.Type;
            cFuJian.Title = fileInfo[3];
            if (cFuJian.Type.Length == 0)//没有扩展名
            {
                cFuJian.Alias = file.FileName;
            }
            cFuJian.Alias = cFuJian.Alias.Replace(" ", "");

            cFuJian.FolderName = fileInfo[2];

            cFuJian.FileName = fileInfo[3];

            cFuJian.Size = MossObject.ToFileSize(file.ContentLength); //文件大小
            cFuJian.ProcessType = UCProcessType;
            cFuJian.WorkItemID = UCWorkItemID;

            string[] saveUrl;

            #region DLL
            if (OAConfig.GetConfig("MOSS认证", "是否启用DLL") == "1")
            {
                #region 更新栏位
                List<System.Collections.DictionaryEntry> lst = new List<System.Collections.DictionaryEntry>();
                System.Collections.DictionaryEntry de = new System.Collections.DictionaryEntry();
                de.Key = "流程实例";
                de.Value = UCProcessID;
                lst.Add(de);

                de = new System.Collections.DictionaryEntry();
                de.Key = "别名";
                de.Value = cFuJian.Alias;
                lst.Add(de);

                de = new System.Collections.DictionaryEntry();
                de.Key = "上次修改者";
                de.Value = CurrentUserInfo.DisplayName;
                lst.Add(de);
                #endregion

                System.Collections.DictionaryEntry[] result = DocumentManager.ConvertToDE(lst.ToArray());

                if (file.ContentLength <= MossObject.middleFileSize * 1024 * 1024)
                {
                    saveUrl = DocumentManager.Upload(fileInfo, MossObject.StreamToBytes(file.InputStream), result, false);

                }
                else
                {
                    string strFileTemp = "D:\\FileTemp\\";

                    if (System.IO.Directory.Exists(strFileTemp) == false)
                    {
                        System.IO.Directory.CreateDirectory(strFileTemp);
                    }

                    string fileTemp = strFileTemp + "OA" + Current.UserName + Path.GetFileNameWithoutExtension(file.TmpFile.Name);

                    string filePath = fileTemp + file.FileName;

                p1:
                    if (System.IO.File.Exists(filePath))
                    {
                        filePath = fileTemp + new Random(1).Next(100).ToString() + file.FileName;
                        goto p1;
                    }
                    else
                    {
                        file.TmpFile.MoveTo(filePath);
                    }

                    saveUrl = DocumentManager.Upload(fileInfo, filePath, result, false);

                    File.Delete(filePath);
                }

                ////int ret = api.CopyTo(fileInfo, "322.doc", true);
                //file.TmpFile.Delete(); //删除临时文件
                //cFuJian.fullURL = saveUrl[0]; //全路径
                //cFuJian.URL = saveUrl[1];//文件夹+/文件名
                //cFuJian.Encode = "";//文件编码
                //return cFuJian;
            }
            #endregion

            #region webservice
            else
            {
                #region 更新栏位
                List<DictionaryEntry> lst = new List<DictionaryEntry>();
                DictionaryEntry de = new DictionaryEntry();
                de.Key = "流程实例";
                de.Value = UCProcessID;
                lst.Add(de);

                de = new DictionaryEntry();
                de.Key = "别名";
                de.Value = cFuJian.Alias;
                lst.Add(de);

                de = new DictionaryEntry();
                de.Key = "上次修改者";
                de.Value = CurrentUserInfo.DisplayName;
                lst.Add(de);
                #endregion

                DictionaryEntry[] result = api.ConvertToDE(lst.ToArray());

                if (file.ContentLength <= MossObject.middleFileSize * 1024 * 1024)
                {
                    //上传到文档库
                    saveUrl = api.Upload(fileInfo, MossObject.StreamToBytes(file.InputStream), result, false);
                }
                else
                {
                    string strFileTemp = "D:\\FileTemp\\";

                    if (System.IO.Directory.Exists(strFileTemp) == false)
                    {
                        System.IO.Directory.CreateDirectory(strFileTemp);
                    }

                    string fileTemp = strFileTemp + "OA" + Current.UserName + Path.GetFileNameWithoutExtension(file.TmpFile.Name);

                    string filePath = fileTemp + file.FileName;

                p1:
                    if (System.IO.File.Exists(filePath))
                    {
                        filePath = fileTemp + new Random(1).Next(100).ToString() + file.FileName;
                        goto p1;
                    }
                    else
                    {
                        file.TmpFile.MoveTo(filePath);
                    }

                    saveUrl = api.Upload_New(fileInfo, filePath, result, false);

                    File.Delete(filePath);
                }
            }

            #endregion

            file.TmpFile.Delete(); //删除临时文件
           
            cFuJian.fullURL = saveUrl[0]; //全路径
            cFuJian.URL = saveUrl[1];//文件夹+/文件名
            cFuJian.Encode = "";//文件编码
            return cFuJian;
        }


        /// <summary>
        /// MOSS附件大小
        /// </summary>
        /// <param name="UCProcessType"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static long FileLength(string UCProcessType, string URL)
        {
            long fileLength;
            if (OAConfig.GetConfig("MOSS认证", "是否启用DLL") == "1")
            {
                fileLength = DocumentManager.FileLength(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
            }
            else
            {
                fileLength = MossObject.GetMOSSAPI().FileLength(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
            }
            return fileLength;
        }

        /// <summary>
        /// 将附件下载为 字节流
        /// </summary>
        /// <param name="UCProcessType"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Byte[] DownLoad(string UCProcessType, string URL)
        {
            Byte[] fileByte = null;
            if (OAConfig.GetConfig("MOSS认证", "是否启用DLL") == "1")
            {
                fileByte = DocumentManager.Download(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
            }
            else
            {
                fileByte = MossObject.GetMOSSAPI().Download(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
            }
            return fileByte;
        }

        /// <summary>
        /// 将附件下载 写入文件 
        /// </summary>
        /// <param name="UCProcessType"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string DownLoad_New(string UCProcessType, string URL)
        {
            string filePath = null;

            string strTempFileName = "Download_OA_" + Current.UserName + "_";

            if (OAConfig.GetConfig("MOSS认证", "是否启用DLL") == "1")
            {
                filePath = DocumentManager.Download_New(MossObject.GetDownLoadFileInfo(UCProcessType, URL), strTempFileName);
            }
            else
            {
                filePath = MossObject.GetMOSSAPI().Download_New(MossObject.GetDownLoadFileInfo(UCProcessType, URL), strTempFileName);
            }
            return filePath;
        }

        public static int Del(string UCProcessType, string URL)
        {
            int ret = 1;
            //if (OAConfig.GetConfig("MOSS认证", "是否启用DLL") == "1")
            //{
            //    ret = DocumentManager.Remove(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
            //}
            //else
            //{
            //    ret = MossObject.GetMOSSAPI().Remove(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
            //}
            return ret;
        }

        #endregion

        #region 字符串处理
        //把附件list序列化成xml传回页面 再反序列化出来
        public static string FuJianList2Xml(List<CFuJian> dataList)
        {
            if (dataList.Count == 0)
            {
                return "";
            }
            string xml = XmlUtility.ObjListToXml(dataList, "附件");

            string ret = xml;
            return ret;
        }
        public static List<CFuJian> Xml2FuJianList(string xml)
        {
            if (xml == "")
            {
                return new List<CFuJian>();
            }
            string text = xml.Replace("&", "");
            //附件内带&符号报错
            List<CFuJian> dataList = XmlUtility.XmlToObjList<CFuJian>(text, "附件");
            return dataList;
        }
        #endregion
    }
}
