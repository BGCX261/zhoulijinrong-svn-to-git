using System;
using System.Collections;
using Microsoft.SharePoint;
using System.Collections.Specialized;
using System.IO;

namespace FS.ADIM.OA.MOSSS
{
    public class DocumentManager
    {
        #region 变量定义
        private const string logPath =@"D:\Adim\OA.WebUI\Log"; //System.Environment.CurrentDirectory; 

        private string swdUrl = string.Empty;//站点+库
        private string fullUrl = string.Empty; //全路径


        private string docLibUrlName = string.Empty; //文档库英文名 doc
        private string docLibUrlNameNew = string.Empty; //文档库英文名 doc


        private string siteUrl = string.Empty; //站点地址
        private string webUrl = string.Empty; //网站地址
        private string docLibName = string.Empty; //文档库中文名 公司发文
        private string folderUrlName = string.Empty; //文件夹路径   1/2  1
        private string fileName = string.Empty; //文件名

        private string siteUrlNew = string.Empty; //站点地址
        private string webUrlNew = string.Empty; //网站地址
        private string docLibNameNew = string.Empty; //文档库中文名 公司发文
        private string folderUrlNameNew = string.Empty; //文件夹路径   1/2  1
        private string fileNameNew = string.Empty; //文件名

        private string foliName = string.Empty; //文件夹+文件名

        #endregion

        #region 验证文件是否上传成功
        public int CheckHave(string[] fileInfo)
        {
            SetUrl(fileInfo);
            int index = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb(webUrl))
                    {
                        SPList list = web.Lists[docLibName]; //文档库中文名
                        SPListItemCollection splc = list.Items;
                        index = GetFileIndex(splc, fileName);
                        WriteLog(docLibName + " " + foliName + " 验证 " + index, "上传");
                    }
                }
            });
            return index;
        }
        #endregion

        #region 上传

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileInfo">文件信息fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件</param>
        /// <param name="bStream">字节数组</param>
        /// <param name="deMeta">栏位信息</param>
        /// <param name="overwrite">存在相同文件名是否覆盖</param>
        /// <returns>1.全地址 2.文件夹+文件名</returns>
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public string[] Upload(string[] fileInfo, byte[] bStream, DictionaryEntry[] deMeta, bool overwrite)
        {
            try
            {
                SetUrl(fileInfo);

                //或使用模拟身份验证
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;//要更新栏位必须写这句
                            SPFile file = null;

                            //检查是否存在文档库或文件夹 如不存在则自动创建
                            CheckWebList(web, docLibName);

                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址

                            Hashtable ht = new Hashtable();
                            if (deMeta != null)
                            {
                                ht = ConvertToHT(deMeta);
                                CheckListField(list, ht);
                            }

                            #region 如果有文件夹
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                                foliName = folderUrlName + "/" + fileName;
                                SPFolder folder = web.GetFolder(fullUrl);

                                if (folder.Exists) //如果文档库存在此文件夹
                                {
                                    if (deMeta == null)
                                    {
                                        file = folder.Files.Add(fullUrl, bStream, overwrite);
                                    }
                                    else
                                    {
                                        file = folder.Files.Add(fullUrl, bStream, ht, overwrite);
                                    }
                                }
                                else
                                {
                                    if (deMeta == null)
                                    {
                                        SPFolder newFolder = web.Folders.Add(docLibUrlName + "/" + folderUrlName);
                                        file = newFolder.Files.Add(fileName, bStream, overwrite);
                                    }
                                    else
                                    {
                                        SPFolder newFolder = web.Folders.Add(docLibUrlName + "/" + folderUrlName);
                                        file = newFolder.Files.Add(fileName, bStream, ht, overwrite);
                                    }
                                }
                            }
                            #endregion

                            #region 如果没有文件夹
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                                foliName = fileName;
                                if (deMeta == null)
                                {
                                    file = web.Files.Add(fullUrl, bStream, overwrite);
                                }
                                else
                                {
                                    file = web.Files.Add(fullUrl, bStream, ConvertToHT(deMeta), overwrite);
                                }
                            }
                            #endregion
                            if (file != null)
                            {
                                if (ht.Count > 0)
                                {
                                    WriteLog(docLibName + " " + fullUrl + " " + ht["别名"].ToString() + " 上传成功 " + ht["上次修改者"].ToString(), docLibName + @"\上传");
                                }
                                else
                                {
                                    WriteLog(docLibName + " " + fullUrl + " " + " 上传成功", docLibName + @"\上传");
                                }
                                if (file.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                                {
                                    file.CheckIn("", SPCheckinType.OverwriteCheckIn);
                                    file.Update();
                                }
                            }
                            else
                            {
                                WriteLog(docLibName + " " + fullUrl + " " + ht["别名"].ToString() + " 上传失败,但没有异常", docLibName + @"\上传");
                            }
                        }
                    }

                });

                return new string[] { fullUrl, foliName }; //全路径 和 文件夹+文件名
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\上传出错");
                throw new Exception(ex.Message + " " + docLibName + " " + fullUrl);
            }
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileInfo">文件信息fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件</param>
        /// <param name="bStream">字节数组</param>
        /// <param name="deMeta">栏位信息</param>
        /// <param name="overwrite">存在相同文件名是否覆盖</param>
        /// <returns>1.全地址 2.文件夹+文件名</returns>
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public string[] Upload(string[] fileInfo, string TempFilePath, DictionaryEntry[] deMeta, bool overwrite)
        {
            try
            {
                SetUrl(fileInfo);

                //或使用模拟身份验证
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;//要更新栏位必须写这句
                            SPFile file = null;

                            //检查是否存在文档库或文件夹 如不存在则自动创建
                            CheckWebList(web, docLibName);

                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址

                            Hashtable ht = new Hashtable();
                            if (deMeta != null)
                            {
                                ht = ConvertToHT(deMeta);
                                CheckListField(list, ht);
                            }

                            using (FileStream fs = File.OpenRead(TempFilePath))
                            {

                                #region 如果有文件夹

                                if (folderUrlName.Length > 0)
                                {
                                    fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                                    foliName = folderUrlName + "/" + fileName;
                                    SPFolder folder = web.GetFolder(fullUrl);

                                    if (folder.Exists) //如果文档库存在此文件夹
                                    {
                                        if (deMeta == null)
                                        {
                                            file = folder.Files.Add(fullUrl, fs, overwrite);
                                        }
                                        else
                                        {
                                            file = folder.Files.Add(fullUrl, fs, ht, overwrite);
                                        }
                                    }
                                    else
                                    {
                                        if (deMeta == null)
                                        {
                                            SPFolder newFolder = web.Folders.Add(docLibUrlName + "/" + folderUrlName);
                                            file = newFolder.Files.Add(fileName, fs, overwrite);
                                        }
                                        else
                                        {
                                            SPFolder newFolder = web.Folders.Add(docLibUrlName + "/" + folderUrlName);
                                            file = newFolder.Files.Add(fileName, fs, ht, overwrite);
                                        }
                                    }
                                }
                                #endregion

                                #region 如果没有文件夹
                                else
                                {
                                    fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                                    foliName = fileName;
                                    if (deMeta == null)
                                    {
                                        file = web.Files.Add(fullUrl, fs, overwrite);
                                    }
                                    else
                                    {
                                        file = web.Files.Add(fullUrl, fs, ConvertToHT(deMeta), overwrite);
                                    }
                                }
                                #endregion

                                if (file != null)
                                {
                                    if (ht.Count > 0)
                                        WriteLog(docLibName + " " + fullUrl + " " + ht["别名"].ToString() + " 上传成功 " + ht["上次修改者"].ToString(), docLibName + @"\上传");
                                    else
                                        WriteLog(docLibName + " " + fullUrl + " " + " 上传成功", docLibName + @"\上传");
                                    if (file.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                                    {
                                        file.CheckIn("", SPCheckinType.OverwriteCheckIn);
                                        file.Update();
                                    }
                                }
                                else
                                {
                                    WriteLog(docLibName + " " + fullUrl + " " + ht["别名"].ToString() + " 上传失败,但没有异常", docLibName + @"\上传");
                                }
                            };
                        }
                    }

                });

                return new string[] { fullUrl, foliName }; //全路径 和 文件夹+文件名
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\上传出错");
                throw new Exception(ex.Message + " " + docLibName + " " + fullUrl);
            }
        }

        public string[] Upload(string ServerWeb, string DocumentName,string fileName, byte[] bStream, DictionaryEntry[] deMeta, bool overwrite)
        {
            string[] fileInfo = this.GetUploadFileInfo(ServerWeb, DocumentName, fileName);
            return this.Upload(fileInfo, bStream, deMeta, overwrite);
        }

        #endregion

        #region 下载
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public byte[] Download(string[] fileInfo)
        {
            try
            {
                byte[] bStream = null;

                SetUrl(fileInfo);
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }
                            SPFile file = GetSPFile(list);

                            if (file == null)
                            {
                                WriteLog("文件未找到" + " " + docLibName + " " + fullUrl, docLibName + @"\下载出错");
                                throw new Exception("文件未找到 " + fullUrl);
                            }

                            bStream = file.OpenBinary();
                        }
                    }
                });
                return bStream;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\下载出错");
                throw new Exception(ex.Message + " " + docLibName + " " + fullUrl);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="TempFileName"></param>
        /// <returns></returns>
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public string Download_New(string[] fileInfo, string TempFileName)
        {
            try
            {
                string filePath = string.Empty;

                SetUrl(fileInfo);
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }

                            SPFile file = GetSPFile(list);

                            if (file == null)
                            {
                                throw new Exception("文件未找到 " + fullUrl);
                            }

                            string dirPath = "D:\\FileTemp\\";

                            string strFilePathTemp = dirPath + TempFileName + GetFileName_New() + "_";

                            string strFilePath = strFilePathTemp + fileName;

                            if (!System.IO.Directory.Exists(dirPath))
                            {
                                System.IO.Directory.CreateDirectory(dirPath);
                            }


                        p1:
                            if (System.IO.File.Exists(strFilePath))
                            {
                                strFilePath = strFilePathTemp + new Random(1).Next(100).ToString() + fileName;
                                goto p1;
                            }

                            using (Stream strm = file.OpenBinaryStream())
                            {
                                using (FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write))
                                {
                                    byte[] bytes = new byte[32768];

                                    bool reading = true;

                                    while (reading)
                                    {
                                        int read = strm.Read(bytes, 0, bytes.Length);

                                        if (read <= 0)
                                        {
                                            reading = false;
                                        }

                                        else
                                        {
                                            fs.Write(bytes, 0, read);
                                        }
                                    }
                                }
                            }

                            filePath = strFilePath;
                        }
                    }
                });

                return filePath;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\下载出错");
                throw new Exception(ex.Message + " " + docLibName + " " + fullUrl);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public long FileLength(string[] fileInfo)
        {
            try
            {
                long fileLength = 0;
                SetUrl(fileInfo);
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }

                            SPFile file = GetSPFile(list);

                            fileLength = file.Length;

                            if (file == null)
                            {
                                throw new Exception("文件未找到 " + fullUrl);
                            }
                        }
                    }
                });
                return fileLength;
            }

            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\文件长度获取出错");
                throw new Exception(ex.Message + " " + docLibName + " " + fullUrl);
            }
        }

        #endregion

        #region 签入
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public int CheckIn(string[] fileInfo)
        {
            try
            {
                SetUrl(fileInfo);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }
                            SPFile file = GetSPFile(list);

                            if (file == null)
                            {
                                WriteLog("文件未找到" + " " + docLibName + " " + fullUrl, docLibName + @"\签入");
                            }
                            else
                            {
                                if (file.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                                {
                                    file.CheckIn("", SPCheckinType.OverwriteCheckIn);
                                    file.Update();
                                    WriteLog(docLibName + " " + fullUrl + " 签入成功", docLibName + @"\签入");
                                }
                            }
                        }
                    }
                });
                return 1;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\签入出错");
                throw new Exception(ex.Message + " " + fullUrl);
            }
        }
        #endregion

        #region 签出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public int CheckOut(string[] fileInfo)
        {
            try
            {
                SetUrl(fileInfo);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }
                            SPFile file = GetSPFile(list);
                            if (file == null)
                            {
                                WriteLog("文件未找到" + " " + docLibName + " " + fullUrl, docLibName + @"\签出");
                            }
                            else
                            {
                                if (file.CheckOutStatus == SPFile.SPCheckOutStatus.None)
                                {
                                    file.CheckOut();
                                    file.Update();
                                    WriteLog(docLibName + " " + fullUrl + " 签出成功", docLibName + @"\签出");
                                }
                            }
                        }
                    }
                });
                return 1;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\签出出错");
                throw new Exception(ex.Message + " " + fullUrl);
            }
        }
        #endregion

        #region 撤销签出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public int UndoCheckOut(string[] fileInfo)
        {
            try
            {
                SetUrl(fileInfo);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }
                            SPFile file = GetSPFile(list);

                            if (file == null)
                            {
                                //throw new Exception("文件未找到 " + fullUrl);
                            }
                            else
                            {
                                if (file.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                                {
                                    file.UndoCheckOut();
                                    file.Update();
                                }
                            }
                        }
                    }
                });
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + fullUrl);
            }
        }
        #endregion

        #region 删除
        public int Remove(string[] fileInfo)
        {
            return Remove(fileInfo, true);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public int Remove(string[] fileInfo, bool isTrueDel)
        {
            try
            {
                SetUrl(fileInfo);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                            }
                            SPFile file = GetSPFile(list);

                            if (file == null)
                            {
                                WriteLog("文件未找到" + " " + foliName + " 删除失败", docLibName + @"\删除");
                            }
                            else
                            {
                                try
                                {
                                    if (isTrueDel)
                                    {
                                        file.Delete();
                                        WriteLog(docLibName + " " + foliName + " 删除成功", docLibName + @"\删除");
                                    }
                                    else
                                    {
                                        WriteLog(docLibName + " " + foliName + " 假删除", docLibName + @"\删除");
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    WriteLog(ex1.Message + " " + docLibName + " " + fullUrl, docLibName + @"\删除出错");
                                }
                            }
                        }
                    }
                });
                return 1;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + fullUrl);
            }
        }
        #endregion

        #region 更新栏位
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="deMeta"></param>
        /// <returns></returns>
        public int UpdateMeta(string[] fileInfo, DictionaryEntry[] deMeta)
        {
            try
            {
                SPSite site = null;
                SPWeb web = null;

                SetUrl(fileInfo);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    site = new SPSite(siteUrl);
                    web = site.OpenWeb(webUrl);
                });

                web.AllowUnsafeUpdates = true;//要更新栏位必须写这句

                CheckWebList(web, docLibName);

                SPList list = web.Lists[docLibName]; //文档库中文名
                docLibUrlName = list.RootFolder.Url; //获得文档库英文地址

                Hashtable ht = new Hashtable();
                if (deMeta != null)
                {
                    ht = ConvertToHT(deMeta);
                    CheckListField(list, ht);

                    SPListItemCollection splc = list.Items;
                    int index = GetFileIndex(splc, fileName);
                    if (index > -1)
                    {
                        SPListItem item = splc[index];
                        foreach (object key in ht.Keys)
                        {
                            item[key.ToString()] = ht[key].ToString();
                        }
                        item.Update();
                    }
                }
                web.Dispose();
                site.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + fullUrl);
            }
        }
        #endregion

        #region 转移文档
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="newFileInfo"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public string[] CopyToNew(string[] fileInfo, string[] newFileInfo, bool overwrite)
        {
            try
            {
                SetUrl(fileInfo);
                SetUrlNew(fileInfo);

                string newFullUrl = "";
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            using (SPWeb webNew = site.OpenWeb(webUrlNew))
                            {
                                SPList listNew = web.Lists[docLibNameNew]; //文档库中文名
                                docLibUrlNameNew = listNew.RootFolder.Url; //获得文档库英文地址

                                SPList list = web.Lists[docLibName]; //文档库中文名
                                docLibUrlName = list.RootFolder.Url; //获得文档库英文地址

                                if (folderUrlName.Length > 0)
                                {
                                    fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                                    newFullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrlNew, webUrlNew, docLibUrlName, folderUrlNameNew, fileNameNew);
                                    foliName = folderUrlNameNew + "/" + fileNameNew;
                                }
                                else
                                {
                                    fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                                    newFullUrl = string.Format("{0}{1}/{2}/{3}", siteUrlNew, webUrlNew, docLibUrlNameNew, fileNameNew);
                                    foliName = fileNameNew;
                                }

                                SPFile file = GetSPFile(list);

                                if (file == null)
                                {
                                    throw new Exception("文件未找到 " + fullUrl);
                                }

                                file.CopyTo(docLibUrlNameNew + "/" + folderUrlNameNew + "/" + fileNameNew, overwrite);
                            }
                        }
                    }

                });
                return new string[] { newFullUrl, foliName }; //全路径 和 文件夹+文件名
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + fullUrl + " " + fileNameNew);
            }
        }
        #endregion

        #region 复制文档
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="newFileName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public string[] CopyTo(string[] fileInfo, string newFileName, bool overwrite)
        {
            try
            {
                SetUrl(fileInfo);

                string newFullUrl = "";
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists[docLibName]; //文档库中文名
                            docLibUrlName = list.RootFolder.Url; //获得文档库英文地址
                            if (folderUrlName.Length > 0)
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, fileName);
                                newFullUrl = string.Format("{0}{1}/{2}/{3}/{4}", siteUrl, webUrl, docLibUrlName, folderUrlName, newFileName);
                                foliName = folderUrlName + "/" + newFileName;
                            }
                            else
                            {
                                fullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, fileName);
                                newFullUrl = string.Format("{0}{1}/{2}/{3}", siteUrl, webUrl, docLibUrlName, newFileName);
                                foliName = newFileName;
                            }

                            SPFile file = GetSPFile(list);

                            if (file == null)
                            {
                                WriteLog("文件未找到 " + foliName + " 复制失败", docLibName + @"\复制");
                                throw new Exception("文件未找到 " + fullUrl);
                            }

                            file.CopyTo(docLibUrlName + "/" + folderUrlName + "/" + newFileName, overwrite);
                            WriteLog(docLibName + " " + fullUrl + " - " + newFileName + " 复制成功", docLibName + @"\复制");
                        }
                    }

                });
                return new string[] { newFullUrl, foliName }; //全路径 和 文件夹+文件名
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + " " + docLibName + " " + fullUrl, docLibName + @"\复制出错");
                throw new Exception(ex.Message + " " + fullUrl + " " + newFileName);
            }
        }
        #endregion

        #region 转换成自定义DictionaryEntry[]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public DictionaryEntry[] ConvertToDE(DictionaryEntry[] entries)
        {
            //用ListDictionary主要是為了稍後可以直接CopyTo轉DictionaryEntry[]
            ListDictionary list = new ListDictionary();
            foreach (DictionaryEntry entry in entries)
            {
                list.Add(entry.Key, entry.Value);
            }
            DictionaryEntry[] result = new DictionaryEntry[list.Count];
            list.CopyTo(result, 0);
            return result;
        }
        #endregion

        #region 私有方法

        #region 获得全路径
        //1.站点url 2.文档库名 3.文件夹 4.文件
        private void SetUrl(string[] fileInfo)//网站+站点
        {
            string swUrl = fileInfo[0];
            if (swUrl.LastIndexOf("/") == swUrl.Length - 1) //如果最后由斜杠则截掉
            {
                swUrl = swUrl.Substring(0, swUrl.Length - 1);
            }
            int index1 = swUrl.IndexOf("/", 7);//除http://外的第一个斜杠
            if (index1 > -1)
            {
                //有的话截取
                siteUrl = swUrl.Substring(0, index1);
                webUrl = swUrl.Substring(siteUrl.Length, swUrl.Length - siteUrl.Length);
            }
            else//没有的话就没有weburl
            {
                siteUrl = swUrl;
                webUrl = "";
            }
            docLibName = fileInfo[1];
            folderUrlName = fileInfo[2];
            fileName = fileInfo[3];
        }
        private void SetUrlNew(string[] fileInfo)//网站+站点
        {
            string swUrlNew = fileInfo[0];
            if (swUrlNew.LastIndexOf("/") == swUrlNew.Length - 1) //如果最后由斜杠则截掉
            {
                swUrlNew = swUrlNew.Substring(0, swUrlNew.Length - 1);
            }
            int index1 = swUrlNew.IndexOf("/", 7);//除http://外的第一个斜杠
            if (index1 > -1)
            {
                //有的话截取
                siteUrlNew = swUrlNew.Substring(0, index1);
                webUrlNew = swUrlNew.Substring(siteUrlNew.Length, swUrlNew.Length - siteUrlNew.Length);
            }
            else//没有的话就没有weburl
            {
                siteUrlNew = swUrlNew;
                webUrlNew = "";
            }
            docLibNameNew = fileInfo[1];
            folderUrlNameNew = fileInfo[2];
            fileNameNew = fileInfo[3];
        }

        #endregion

        #region 转换成Hastable
        /// <summary>
        /// 把DictionaryEntry[]转换成Hashtable
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        private Hashtable ConvertToHT(DictionaryEntry[] entries)
        {

            Hashtable list = new Hashtable();
            foreach (DictionaryEntry entry in entries)
            {
                list.Add(entry.Key, entry.Value);
            }
            return list;
        }
        #endregion

        #region CAML语句查文件
        private SPListItem GetSPItem(SPList list)
        {
            SPQuery query = GetSPQuery(list);
            SPListItemCollection items = list.GetItems(query);
            if (items.Count == 1)
            {
                return items[0];
            }
            return null;
        }
        private SPFile GetSPFile(SPList list)
        {
            SPQuery query = GetSPQuery(list);
            SPListItemCollection items = list.GetItems(query);
            if (items.Count == 1)
            {
                return items[0].File;
            }
            return null;
        }
        private SPQuery GetSPQuery(SPList list)
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format(@"<Where>
                                      <Eq>
                                         <FieldRef Name='FileLeafRef' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>", fileName);
            if (folderUrlName != "")
            {
                query.Folder = GetSubFolders(list, folderUrlName);
            }
            return query;
        }
        private SPFolder GetSubFolders(SPList list, string sfolder)
        {
            if (sfolder.Length > 0)
            {
                //如果字符串
                if (folderUrlName.IndexOf("/") == 0)
                {
                    folderUrlName = folderUrlName.Substring(1);
                }
                if (folderUrlName.LastIndexOf("/") == folderUrlName.Length - 1)
                {
                    folderUrlName = folderUrlName.Substring(0, folderUrlName.Length - 1);
                }
                string[] strArr = sfolder.Split('/');

                SPFolder folder = null;
                folder = list.RootFolder;
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (folder.Name != strArr[i])
                        folder = folder.SubFolders[strArr[i]];
                }
                return folder;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 验证文档
        //获得文件在文档库的索引
        private int GetFileIndex(SPDocumentLibrary docLib, string fileName)
        {
            for (int i = 0; i < docLib.RootFolder.Files.Count; i++)
            {
                if (docLib.RootFolder.Files[i].Name == fileName)
                {
                    return i;
                }
            }
            return -1;//找不到文件
        }
        private int GetFileIndex(SPListItemCollection itemColl, string fileName)
        {
            for (int i = 0; i < itemColl.Count; i++)
            {
                if (itemColl[i].Name == fileName)
                {
                    return i;
                }
            }
            return -1;//找不到文件
        }
        #endregion

        #region 验证是否存在 不存在自动创建
        private void CheckSPWeb()
        {

        }
        private void CheckWebList(SPWeb web, string name)
        {
            bool IsExist = false;
            for (int i = 0; i < web.Lists.Count; i++)
            {
                if (web.Lists[i].Title == name)
                {
                    IsExist = true;
                }
            }

            if (!IsExist)  //如果不存在则自动创建
            {
                Guid id = web.Lists.Add(name, "", SPListTemplateType.DocumentLibrary);
                SPList list = web.Lists[id];
                list.Update();
            }
        }
        private void CheckListField(SPList list, Hashtable ht)
        {
            SPFieldCollection fields = list.Fields;
            SPView defaultView = list.DefaultView;
            bool IsAdd = false;
            foreach (object key in ht.Keys)
            {
                bool IsExist = false;
                IsExist = list.Fields.ContainsField(key.ToString());

                if (!IsExist)
                {
                    SPField field = fields.CreateNewField("Text", key.ToString());
                    string internalName = fields.Add(field);
                }
            }
            if (IsAdd)
            {
                list.Update();
            }
        }

        //获得当前日期的文件名
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetFileName_New()
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
        #endregion

        private void WriteLog(string message, string folder)
        {
            TxtFileLogger log = new TxtFileLogger(logPath, folder);
            log.WriteLog(message);
        }

        //获得上传路径,文件信息的数组
        private string[] GetUploadFileInfo(string ServerWeb, string DocumentName, string filePath)
        {
            string[] ret = new string[4];
            ret[0] = ServerWeb;
            if (DocumentName == "")
            {
                ret[1] = "Temp";
            }
            else
            {
                ret[1] = DocumentName;
            }
            ret[2] = GetFolderName(); //文件夹
            ret[3] = GetFileName() + System.IO.Path.GetExtension(filePath); //文件名
            return ret;
        }

        //获得当前年月的文件夹名
        private string GetFolderName()
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
        private string GetFileName()
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

        #endregion
    }
}
