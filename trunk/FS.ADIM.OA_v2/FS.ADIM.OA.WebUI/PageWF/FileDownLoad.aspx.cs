using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.WebUI.PageOU;
using FS.ADIM.OA.MOSSS;
using FS.OA.Framework;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class FileDownLoad : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String l_strTemplateName = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];
                String l_strUrl = Request.QueryString["URL"];
                String l_strExtension = System.IO.Path.GetExtension(l_strUrl);
                String l_strAlias = Request.QueryString["Alias"];
                String l_strTotalFileName = l_strAlias + l_strExtension;

                if (l_strTemplateName.Contains("新版"))
                {
                    l_strTemplateName = l_strTemplateName.Substring(2);
                }

                String downName = HttpUtility.UrlEncode(System.Text.UTF8Encoding.UTF8.GetBytes(l_strTotalFileName));

                long fileLength = MossObject.FileLength(l_strTemplateName, l_strUrl);

                //TODO:对于大附件的处理
                //小附件：直接使用WebService，返回Byte[]
                //大附件：将附件从MOSS库写入文件，保存到服务器端
                if (fileLength <= MossObject.middleFileSize * 1024 * 1024)
                {
                    Byte[] fileByte = MossObject.DownLoad(l_strTemplateName, l_strUrl);

                    Response.Buffer = true;
                    Response.ContentType = "application/octet-stream";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;

                    Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", downName));
                    Response.BinaryWrite(fileByte);
                    Response.End();
                }
                else
                {
                    string tempFilePath = MossObject.DownLoad_New(l_strTemplateName, l_strUrl);

                    this.DownLoad_File(tempFilePath, downName);
                }
            }
        }

        /// <summary>
        /// 从服务器端下载附件到客户端
        /// </summary>
        /// <param name="filePath_New"></param>
        /// <param name="filename_Show"></param>
        private void DownLoad_File(string filePath_New, string filename_Show)
        {
            System.IO.Stream iStream = null;

            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10000];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file name.
            string filename = System.IO.Path.GetFileName(filePath_New);

            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(filePath_New, System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.Read);

                // Total bytes to read:
                dataToRead = iStream.Length;

                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename_Show);

                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream.
                        Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        Response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }

                Response.End();
            }
            catch (Exception ex)
            {
                JScript.ShowMsgBox(Page, MsgType.VbExclamation, ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }

                if (System.IO.File.Exists(filePath_New))
                {
                    System.IO.File.Delete(filePath_New);
                }
            }
        }
    }
}
