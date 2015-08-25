using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using FS.ADIM.OA.MOSSS;
using System.Collections;

namespace FS.ADIM.OA.MOSS_Service
{
    /// <summary>
    /// OADocumentService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    public class OA_DocumentService : System.Web.Services.WebService
    {

        #region 测试是否可以访问到该webservice
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int Test()
        {
            return 1;
        }
        #endregion

        #region 验证
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public int CheckHave(string[] fileInfo)
        {
            try
            {
                return DocumentManager.CheckHave(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region 下载
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public byte[] Download(string[] fileInfo)
        {
            try
            {
                return DocumentManager.Download(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public string Download_New(string[] fileInfo, string TempFileName)
        {
            try
            {
                return DocumentManager.Download_New(fileInfo, TempFileName);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public long FileLength(string[] fileInfo)
        {
            try
            {
                return DocumentManager.FileLength(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region 上传
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="bStream"></param>
        /// <param name="deMeta"></param>
        /// <param name="overwrite"></param>
        /// <returns>1.全地址 2.文件夹+文件名</returns>
        [WebMethod]
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public string[] Upload(string[] fileInfo, byte[] bStream, DictionaryEntry[] deMeta, bool overwrite)
        {
            try
            {
                return DocumentManager.Upload(fileInfo, bStream, deMeta, overwrite);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="bStream"></param>
        /// <param name="deMeta"></param>
        /// <param name="overwrite"></param>
        /// <returns>1.全地址 2.文件夹+文件名</returns>
        [WebMethod]
        //fileInfo=1.站点url 2.文档库名 3.文件夹 4.文件
        public string[] Upload_New(string[] fileInfo, string strFileTempUrl, DictionaryEntry[] deMeta, bool overwrite)
        {
            try
            {
                return DocumentManager.Upload(fileInfo, strFileTempUrl, deMeta, overwrite);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region 签入
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public int CheckIn(string[] fileInfo)
        {
            try
            {
                return DocumentManager.CheckIn(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region 签出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public int CheckOut(string[] fileInfo)
        {
            try
            {
                return DocumentManager.CheckOut(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region 撤销签出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public int UndoCheckOut(string[] fileInfo)
        {
            try
            {
                return DocumentManager.UndoCheckOut(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除 可假删除
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public int Remove2(string[] fileInfo, bool isTrueDel)
        {
            try
            {
                return DocumentManager.Remove(fileInfo, isTrueDel);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public int Remove(string[] fileInfo)
        {
            try
            {
                return DocumentManager.Remove(fileInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
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
        [WebMethod]
        public int UpdateMeta(string[] fileInfo, DictionaryEntry[] deMeta)
        {
            try
            {
                return DocumentManager.UpdateMeta(fileInfo, deMeta);
            }
            catch (Exception ex)
            {
                throw (ex);
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
        [WebMethod]
        public string[] CopyToNew(string[] fileInfo, string[] newFileInfo, bool overwrite)
        {
            try
            {
                return DocumentManager.CopyToNew(fileInfo, newFileInfo, overwrite);
            }
            catch (Exception ex)
            {
                throw (ex);
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
        [WebMethod]
        public string[] CopyTo(string[] fileInfo, string newFileName, bool overwrite)
        {
            try
            {
                return DocumentManager.CopyTo(fileInfo, newFileName, overwrite);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region 转换成自定义DictionaryEntry[]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        [WebMethod]
        public DictionaryEntry[] ConvertToDE(DictionaryEntry[] entries)
        {
            return DocumentManager.ConvertToDE(entries);
        }
        #endregion
    }
}
