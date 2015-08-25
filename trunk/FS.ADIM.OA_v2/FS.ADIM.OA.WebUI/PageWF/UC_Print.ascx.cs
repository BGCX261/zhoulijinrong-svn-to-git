//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：Word输出类
// 
// 创建标识：2010-02-21 王晓
//
// 修改标识：2010-05-06 任金权
// 修改描述：1.修改BatchAddPicture函数，针对函件发文时间格式异常做的处理（'姓名 日期 时间'），去掉姓名
//           2.修改BatchAddPicture函数，扩展函件发文，多人会签时的签名图片打印
//           3.修改BatchAddPicture函数，扩展公司发文，多个部门会签时的签名图片打印
//  
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------

//#define EVENTLOG  //使用EVENTLOG WINDOWS日志功能

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FS.ADIM.OA.BLL.Entity;
using WordMgr;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Common;
using System.Threading;
using FS.OA.Framework.Logging;
using FS.ADIM.OA.BLL.Busi;
using System.Data;

namespace FS.ADIM.OA.WebUI.PageWF
{
    /// <summary>
    /// 1.创建WORD模板,保存至对应流程名称目录下,作为表单模板应保存至对应流程模板下的[文件处理单]目录下
    /// 2.修改process.xml template.xml配置文件;
    ///   process.xml保存每个流程对应流程中每个流程节点需要打印的模板文件,这些文件应存在于[文件处理单]目录下;
    ///   template.xml记录每个打印模板文件的打印项,在每个流程下由print.cs源代码文件添加对应的打印项字符串值;
    /// 3.添加相应流程print.cs文件
    /// </summary>
    public partial class UC_Print : OAUCBase
    {
        #region 打印用户控件委托
        public delegate void ExportHandler(object sender, EventArgs e);
        /// <summary>
        /// 操作由template.xml文件读取对应流程对应模板的待写入信息.
        /// 将待写入数据(由实体提供)传入UCPrint.ExportData.Add方法中.
        /// 注意:写入顺序必须要与template.xml配置文件中记录的顺序相同.
        /// </summary>
        public event ExportHandler OnBeginExport;// = null;
        /// <summary>
        /// 该OnCompletionExport委托已经释放WORD组件资源,因此不要任何放入使用WORD组件操作WORD文档的代码
        /// </summary>
        public event ExportHandler OnCompletionExport;// = null;
        /// <summary>
        /// 对WORD以及WORD兼容版本的文档导入到当前文档的操作放入该事件中.
        /// 使用UCPrint.WriteAttach或UCPrint.WriteContent分别处理正文附件和非正文附件
        /// </summary>
        public event ExportHandler OnAttachExport;
        /// <summary>
        /// 处理WORD模板中特殊情况,例如由DataTable作为数据源写入的表格,以及操作WORD为文档的页眉等
        /// 写入表格可调用WriteTable,写入页眉可调用WriteHeaderFooter
        /// 注意:组件中所有集合下标均从1开始,而不是0开始.
        /// </summary>
        public event ExportHandler OnExtraExport;
        /// <summary>
        /// 在关闭,释放WORD组件前最后进行对WORD文档的操作,例如统计文档页数或调整文档格式等
        /// </summary>
        public event ExportHandler OnBeforeClosed;
        #endregion

        //static int count = 0;

        #region 成员变量
        private ArrayList alContentFiles = new ArrayList();
        private ArrayList alAttachFiles = new ArrayList();
        private bool m_bPrinting = false;

        public bool m_bBatch = false;       // 指定是否批量打印,TRUE为批量打印,否则为普通打印
        public List<EntityBase> m_ls;       // 存储待批量打印的实体集合
        public EntityBase m_CurrEntity;
        /**************************使用打印用户控件所需设置参数*********************/
        #region 写死的路径
        private string _sExportPath = @"~\Download\";
        /// <summary>
        /// 导出文件所存路径
        /// </summary>
        public string ExportPath
        {
            get { return _sExportPath; }
            set { _sExportPath = value; }
        }
        private string _tmpAttachFilesDirectory = @"~\Download\tmp\";
        /// <summary>
        /// 保存附件文件的临时目录
        /// </summary>
        public string TmpAttachFilesDirectory
        {
            get { return _tmpAttachFilesDirectory; }
            set { _tmpAttachFilesDirectory = value; }
        }

        /// <summary>
        /// 另存为导出文件的名称
        /// </summary>
        public String SaveFileName
        {
            get
            {
                if (ViewState["SaveFileName"] == null)
                    ViewState["SaveFileName"] = "";
                return ViewState["SaveFileName"] as String;
            }
            set
            {
                ViewState["SaveFileName"] = value;
            }
        }
        #endregion
        #endregion

        #region 属性

        public DocFormat FontStyle
        {
            get
            {
                return word.Format;
            }
            set
            {
                word.Format = value;
            }
        }

        /// <summary>
        /// 模版文件名称
        /// </summary>
        public String FileName
        {
            get
            {
                if (ViewState["FileName"] == null)
                    return String.Empty;
                return ViewState["FileName"] as String;
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }

        /// <summary>
        /// 流程类型
        /// </summary>
        public String UCTemplateName
        {
            get
            {
                if (ViewState[ConstString.ViewState.TEMPLATE_NAME] == null)
                    return String.Empty;
                return ViewState[ConstString.ViewState.TEMPLATE_NAME] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.TEMPLATE_NAME] = value;
            }
        }

        /// <summary>
        /// 流程步骤ID
        /// </summary>
        public String UCStepName
        {
            get
            {
                if (ViewState[ConstString.ViewState.STEP_NAME] == null)
                    return String.Empty;
                return ViewState[ConstString.ViewState.STEP_NAME] as String;
            }
            set
            {
                ViewState[ConstString.ViewState.STEP_NAME] = value;
            }
        }

        /// <summary>
        /// 模版文件所存路径
        /// </summary>
        public string TemplatePath
        {
            get
            {
                if (ViewState["TemplatePath"] == null)
                    return String.Empty;
                return ViewState["TemplatePath"] as string;
            }
            set
            {
                ViewState["TemplatePath"] = value;
            }
        }

        private ArrayList _alExportData = new ArrayList();
        /// <summary>
        /// 待导出数据，必须与模版配置文件中描述的列顺序相同
        /// </summary>
        public ArrayList ExportData
        {
            get { return _alExportData; }
            set { _alExportData = value; }
        }

        private List<CFuJian> _AttachFileList;
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<CFuJian> AttachFileList
        {
            get { return _AttachFileList; }
            set
            {
                _AttachFileList = value;
                InitAttachFiles();
            }
        }

        private string _Position;
        /// <summary>
        /// 填充行所需填充的参考位置
        /// </summary>
        public string Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        /* 暂时保留
        /// <summary>
        /// 当前文档总页数
        /// </summary>
        private int iPages = 0;
        public string Pages
        {
            get { return word.Pages; }
        }
        */

        private WriteMode _wmMode;
        /// <summary>
        /// 填充空行模式
        /// Up, 在参考位置的上两个段落后插入空行
        /// Right, 在参考位置的下一个段落后插入空行
        /// Inner, 在参考位置处后插入空行
        /// Down, 在参考位置的下两个段落后插入空行
        /// Down_Append, 在参考位置的最后一个段落后插入空行
        /// </summary>
        public WriteMode Mode
        {
            get { return _wmMode; }
            set { _wmMode = value; }
        }

        /*****************************************************************************/
        private WordMgr.IWord word;


        /// <summary>
        /// 是否将导出WORD文档转换为PDF文件，若为TRUE则转换PDF文件，否则转换
        /// </summary>
        public Boolean ToPDF
        {
            get
            {
                if (ViewState["ToPDF"] == null)
                    ViewState["ToPDF"] = false;
                return (Boolean)ViewState["ToPDF"];
            }
            set
            {
                ViewState["ToPDF"] = value;
            }
        }

        private bool ChkParam()
        {
            // 提示参数不能为空
            return string.IsNullOrEmpty(this.TemplatePath)
                    || string.IsNullOrEmpty(this.ExportPath)
                    || string.IsNullOrEmpty(FileName)
                    || (this.ExportData.Count == 0);
        }
        #endregion

        #region 加载
        public void InitEx()
        {
            drpMoBan.Items.Clear();
            if (!this.m_bBatch)
            {
                OnBeginExport = null;
                OnCompletionExport = null;
            }
            // 根据流程类型与流程步骤ID获取打印模板参数
            ArrayList alPrintParam = GetPrintParamByProcessViewID(UCTemplateName, UCStepName);
            if (alPrintParam.Count == 0) return;
            if (((string)alPrintParam[0]).ToUpper() == "LAST")
            {
                TemplatePath = string.Format(@"~\template\{0}\", UCTemplateName);

                //加载模板
                string[] files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(TemplatePath));

                for (int i = 0; i < files.Length; i++)
                {
                    string sFileName = files[i].Split(new char[] { '\\' })[files[i].Split(new char[] { '\\' }).Length - 1];
                    if ("~$" == sFileName.Substring(0, 2))
                    {
                        File.Delete(files[i]);
                        continue;
                    }
                    if ("~" == sFileName.Substring(0, 1))
                    {
                        continue;
                    }
                    //工程会议纪要 公文报告模版1
                    string s = System.IO.Path.GetFileNameWithoutExtension(files[i]);
                    //if (s == "工程会议纪要")// 暂时
                    //drpMoBan.Items.Add(new ListItem(s, s));
                    //if (s == "公文报告模版1")// 暂时
                    drpMoBan.Items.Add(new ListItem(s, s));
                }
            }
            else
            {
                TemplatePath = string.Format(@"~\template\{0}\", UCTemplateName);
                string[] str = null;
                for (int i = 0; i < alPrintParam.Count; i++)
                {
                    string tmp = TemplatePath;
                    tmp += alPrintParam[i];

                    //加载模板
                    str = tmp.Split(new char[] { '\\', '.' });
                    if (str.Length == 0) return;
                    //string[] files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(TemplatePath));
                    //if (files.Length == 0) return;
                    //string s = System.IO.Path.GetFileNameWithoutExtension(files[0]);
                    drpMoBan.Items.Add(new ListItem(str[str.Length - 2], str[str.Length - 2]));
                }
                if (str == null) return;
                TemplatePath += str[3] + "\\";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitEx();
            }
            if (this.m_bBatch)
            {
                InitEx();
            }
        }
        #endregion

        #region 私有方法
        #region 打印相关操作
        ///******************************************************************************************************/
        //private string[] GetDownLoadFileInfo(string UCProcessType, string filePath)
        //{
        //    string[] ret = new string[4];
        //    IConfig config = ConfigFactory.GetConfig(ConfigType.Ini, ConstString.ConfigPath.Config);
        //    ret[0] = config.GetValue("MOSS认证", "ServerWeb");
        //    ret[1] = UCProcessType;

        //    int index = filePath.LastIndexOf('/');

        //    ret[2] = filePath.Substring(0, index);

        //    ret[3] = filePath.Substring(index + 1, filePath.Length - index - 1);
        //    return ret;
        //}

        private void GenTmpAttachFile(string UCProcessType, string URL, string SavePath)
        {
            try
            {
                Byte[] fileByte = MossObject.GetMOSSAPI().Download(MossObject.GetDownLoadFileInfo(UCProcessType, URL));
                File.WriteAllBytes(SavePath, fileByte);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 根据是否正文标志以及时间戳获取临时目录下的附件文件列表清单
        /// </summary>
        /// <param name="sPath">临时目录</param>
        /// <param name="TimeStamp">时间戳</param>
        /// <returns>指定时间戳的文件列表</returns>
        private ArrayList GetFilesListByTimeStamp(string sPath, string TimeStamp, bool bContent)
        {
            ArrayList al = new ArrayList();
            DirectoryInfo TheFolder = new DirectoryInfo(sPath);

            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                string[] sRet = NextFile.Name.Split(new char[] { '.' });
                if (sRet.Length != 4) continue;
                if (bContent)
                {
                    if ("content" == sRet[0].ToLower() && TimeStamp == sRet[2])
                    {
                        al.Add(sPath + "\\" + NextFile.Name);
                    }
                }
                else
                {
                    if ("attach" == sRet[0].ToLower() && TimeStamp == sRet[2])
                    {
                        al.Add(sPath + "\\" + NextFile.Name);
                    }
                }
            }
            return al;
        }
        /******************************************************************************************************/
        #endregion

        /// <summary>
        /// 读取Process.xml文件获取打印模板参数
        /// </summary>
        /// <param name="UCProcessType">流程类型</param>
        /// <param name="ViewID">流程步骤ID</param>
        /// <returns>打印模板参数</returns>
        public ArrayList GetPrintParamByProcessViewID(string UCProcessType, string ViewID)
        {
            XmlDocument xmldoc = new XmlDocument();
            ArrayList al = new ArrayList();
            xmldoc.Load(HttpContext.Current.Server.MapPath((@"~\template\process.xml")));

            if (xmldoc.DocumentElement == null) return al;
            XmlNode root = xmldoc.DocumentElement;

            XmlNodeList Nodes = root.SelectNodes("process [ @name = '" + UCProcessType + "']");

            if (Nodes.Count == 1)
            {
                XmlNodeList subNodes = Nodes[0].SelectNodes("template [@id = '" + ViewID + "']");

                if (subNodes.Count == 1)
                {
                    XmlNodeList LeafNode = subNodes[0].ChildNodes;
                    for (int i = 0; i < LeafNode.Count; i++)
                    {
                        al.Add(LeafNode[i].InnerText);
                    }
                }

            }

            return al;
        }

        /// <summary>
        /// 下载打印文件
        /// </summary>
        /// <param name="sFile"></param>
        private void PushFileToClient(string sFile)
        {
            ////////////////////////////向客户端发送待打印文件//////////////////////////////////////////

            string ff = HttpContext.Current.Server.MapPath(@"~\Download\" + sFile);
            FileStream fs = File.OpenRead(ff);
            Byte[] fileByte = new Byte[fs.Length];
            fs.Read(fileByte, 0, (int)fs.Length);

            Response.Buffer = true;

            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Unicode;

            string s = System.Web.HttpUtility.UrlEncode(System.Text.UTF8Encoding.UTF8.GetBytes(sFile));

            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", s));

            Response.BinaryWrite(fileByte);
            Response.End();

            ///////////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// 检查站点所在的目录下的是否存在\Download\tmp目录,若不存在则创建之,并返回TRUE,创建失败则返回FALSE
        /// 若存在则返回TRUE
        /// </summary>
        /// <returns></returns>
        private bool ChkExportFolder()
        {
            string sFolder = HttpContext.Current.Server.MapPath("\\Download");
            if (!Directory.Exists(sFolder))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(sFolder);
                if (DirInfo != null)
                {
                    sFolder = HttpContext.Current.Server.MapPath("\\Download\\tmp");
                    DirInfo = Directory.CreateDirectory(sFolder);
                    if (DirInfo != null)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            else
            {
                sFolder = HttpContext.Current.Server.MapPath("\\Download\\tmp");
                if (!Directory.Exists(sFolder))
                {
                    DirectoryInfo DirInfo = Directory.CreateDirectory(sFolder);
                    if (DirInfo != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }

        private bool ChkExportFolder(string sPath) //"\\Download"  "\\Download\\tmp"
        {
            string sFolder = HttpContext.Current.Server.MapPath(sPath);
            if (!Directory.Exists(sFolder))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(sFolder);
                if (DirInfo != null)
                {
                    sFolder = HttpContext.Current.Server.MapPath(sPath + "\\tmp");
                    DirInfo = Directory.CreateDirectory(sFolder);
                    if (DirInfo != null)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            else
            {
                sFolder = HttpContext.Current.Server.MapPath(sPath + "\\tmp");
                if (!Directory.Exists(sFolder))
                {
                    DirectoryInfo DirInfo = Directory.CreateDirectory(sFolder);
                    if (DirInfo != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.m_bBatch)
            {
                /*StartWorkerThread();*/
                BatchPrint(this.m_ls);
                return;
            }
            //FS.ADIM.OA.WebUI.Print.CEventLog.Delete();
            if (m_bPrinting) return;
            m_bPrinting = true;

            if (!ChkExportFolder()) // 检查站点目录下是否存在\Download\tmp
                return;       // 若不存在,并且创建失败则返回FALSE;
            //count++;
            //Debug.WriteLine(count.ToString());

            Random r1 = new Random();
            int n1 = r1.Next(0, 128);

            SaveFileName = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "." + n1.ToString();
            //从选择层上得到模板
            FileName = drpMoBan.SelectedValue;
            if (string.IsNullOrEmpty(FileName)) return;
            //word还是pdf
            if (rdoWord.Checked)
            {
                ToPDF = false;
            }
            else
            {
                ToPDF = true;
            }
            if (OnBeginExport != null)
                OnBeginExport(this, e);

            if (ChkParam())
            {
                m_bPrinting = false;
                return;    // 参数不合法，无法继续
            }

            word = FactoryWord.CreateWordObj();
            if (word == null)
            {
                m_bPrinting = false;
                return;
            }
#if EVENTLOG
            FS.ADIM.OA.WebUI.Print.CEventLog.Instance();
#endif
            word.Template = HttpContext.Current.Server.MapPath(this.TemplatePath + this.FileName + ".docx");

            word.SavePath = HttpContext.Current.Server.MapPath(this.ExportPath + this.SaveFileName + ".docx");

            if (word.Config._alTemplateInfo.Count == 0)
            {
                word.Close();
                m_bPrinting = false;
                return;
            }

            if (word.Open())
            {
                #region 模板替换处理
                try
                {
                    for (int i = 0; i < word.Config._alTemplateInfo.Count; i++)
                    {
                        SubItem item = ((SubItem)word.Config._alTemplateInfo[i]);
                        //string sValue = (_alExportData[i] as string).Replace("<br>", "\r\n");

                        switch (item.ExtraPara)
                        {
                            case "up":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Up);
                                break;
                            case "right":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Right);
                                break;
                            case "inner":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Inner);
                                break;
                            case "down":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Down);
                                break;
                            case "shift":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Shift);
                                break;
                            case "down_append":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Down_Append);
                                break;
                        }
                    }
                    if (OnExtraExport != null)
                        OnExtraExport(this, e);   // 调用IWord.Write(string key, string value, WriteMode mode, int offset);接口功能

                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex.Message);
#if EVENTLOG
                    FS.ADIM.OA.WebUI.Print.CEventLog.Log("HN  " + ex.Message);
#endif
                    goto CONTINUE;
                }
                #endregion

            CONTINUE:
                try
                {
                    if (OnAttachExport != null)
                        OnAttachExport(this, e);

                    //word.DocLayout(this.Position, this.Mode);
                    if (OnBeforeClosed != null)
                        OnBeforeClosed(this, e);
                    //this.Pages = word.Pages;
                    word.Save();

                    if (ToPDF)
                    {
                        string sPdf = HttpContext.Current.Server.MapPath(this.ExportPath + this.SaveFileName + ".pdf");
                        string[] sRet = sPdf.Split(new char[] { '\\' });
                        word.Convert2pdf(sPdf);
                        word.Close();

                        if (OnCompletionExport != null)
                            OnCompletionExport(this, e);

                        //TODO:清理附件列表生成的临时文件
                        ClearFileListTmpDirectory(TmpAttachFilesDirectory); //@"~\docdata\tmp\"

                        m_bPrinting = false;

                        PushFileToClient(sRet[sRet.Length - 1]);
                    }
                    else
                    {
                        string sDocx = HttpContext.Current.Server.MapPath(this.ExportPath + this.SaveFileName + ".docx");
                        word.Close();

                        if (OnCompletionExport != null)
                            OnCompletionExport(sender, e);

                        //TODO:清理附件列表生成的临时文件
                        ClearFileListTmpDirectory(TmpAttachFilesDirectory); //@"~\docdata\tmp\"

                        m_bPrinting = false;

                        PushFileToClient(this.SaveFileName + ".docx");
                    }
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex.Message);
#if EVENTLOG
                    FS.ADIM.OA.WebUI.Print.CEventLog.Log("HN  " + ex.Message);
#endif
                    new Exception("由于某些原因本次打印任务失败,请重新打印");
                }
            }
            else
            {
                //throw new Exception("打开模板失败");
                //Response.Write("<script>alert('打开模板失败,请重试!')</script>");
                Alert("打开模板失败,请重试!");
            }

        }

        public static void Alert(string message)
        {
            Page PageCurrent = (Page)System.Web.HttpContext.Current.Handler;
            string script = string.Format("alert('{0}');", message);
            //PageCurrent.ClientScript.RegisterStartupScript(PageCurrent.GetType(), DateTime.Now.Ticks.ToString(), script, true);
            PageCurrent.ClientScript.RegisterClientScriptBlock(PageCurrent.GetType(), DateTime.Now.Ticks.ToString(), script, true);
        }

        /// <summary>
        /// 清除指定目录下的文件
        /// </summary>
        /// <param name="sTmpDir"></param>
        private void ClearFileListTmpDirectory(string sTmpDir)
        {
            string sFullPath = HttpContext.Current.Server.MapPath(sTmpDir); //@"~\docdata\" + sFile
            try
            {
                if (Directory.Exists(sFullPath))
                {
                    string[] strDirs = Directory.GetDirectories(sFullPath);
                    string[] strFiles = Directory.GetFiles(sFullPath);
                    foreach (string strFile in strFiles)
                    {
                        File.Delete(strFile);
                    }
                    foreach (string strdir in strDirs)
                    {
                        Directory.Delete(strdir, true);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void InitAttachFiles()
        {
            /**********************根据上传的附件生成附件文件保存到临时目录下***********************/
            string ts = DateTime.Now.ToFileTime().ToString();
            for (int i = 0; i < AttachFileList.Count; i++)
            {
                if ((AttachFileList[i] as CFuJian).Type == "docx" || (AttachFileList[i] as CFuJian).Type == "doc")
                {
                    string sFileName = ((AttachFileList[i] as CFuJian).IsZhengWen == "1") ? "content" : "attach";
                    GenTmpAttachFile((AttachFileList[i] as CFuJian).ProcessType,
                                      (AttachFileList[i] as CFuJian).URL,
                                      HttpContext.Current.Server.MapPath(@"~\Download\tmp\" + sFileName + "." + i.ToString() + "." + ts + "." + (AttachFileList[i] as CFuJian).Type));
                }
            }
            /**********************根据上传的附件生成附件文件保存到临时目录下***********************/

            /***************************************************************************************/
            alContentFiles = GetFilesListByTimeStamp(HttpContext.Current.Server.MapPath(@"~\Download\tmp"), ts, true);
            alAttachFiles = GetFilesListByTimeStamp(HttpContext.Current.Server.MapPath(@"~\Download\tmp"), ts, false);
            /***************************************************************************************/
        }

        private void Write(string Key, string sParam, WriteMode mode)
        {
            if (File.Exists(sParam))
            {
                string[] arr = sParam.Split(new char[] { '\\' });
                if (arr.Length == 0) return;
                string[] arrFile = ((string)arr[arr.Length - 1]).Split(new char[] { '.' });
                if (arrFile.Length != 4) return;
                if (arrFile[0] == "content")
                {
                    word.WriteFile(Key, sParam, mode, 1);
                }
                else
                {
                    word.WriteFile(sParam);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sParam)) return;
                word.Write(Key, sParam, mode);
            }
        }

        /// <summary>
        /// 检查系统指定目录下是否存在\template\Singer目录,用于存放从数据库中导出的签名图片文件
        /// </summary>
        /// <returns>若该目录不存在则创建之,若创建成功返回TRUE,否则返回FALSE;若存在则返回TRUE</returns>
        private bool ChkSingerFolder()
        {
            string sFolder = HttpContext.Current.Server.MapPath(@"~\template\Singer");
            if (!Directory.Exists(sFolder))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(sFolder);
                if (DirInfo == null) return false;
                return true;
            }
            return true;
        }

        //TODO:批量导出WORD文档(根据实体对象信息)
        private bool BackGroundPrint(UC_Print sender, out string sFileName, string sID, string sProcName, string sStepName)
        {
            sFileName = "";
            if (m_bPrinting) return false;
            m_bPrinting = true;

            if (!ChkExportFolder("\\Batch")) // 检查站点目录下是否存在\Download\tmp
                return false;       // 若不存在,并且创建失败则返回FALSE;

            Random r1 = new Random();
            int n1 = r1.Next(0, 128);

            SaveFileName = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "." + n1.ToString();
            //从选择层上得到模板
            FileName = drpMoBan.SelectedValue;
            if (string.IsNullOrEmpty(FileName)) return false;
            //word还是pdf
            if (rdoWord.Checked)
            {
                ToPDF = false;
            }
            else
            {
                ToPDF = true;
            }
            if (OnBeginExport != null)
                OnBeginExport(this, null);

            if (ChkParam())
            {
                m_bPrinting = false;
                return false;    // 参数不合法，无法继续
            }

            word = FactoryWord.CreateWordObj();
            if (word == null)
            {
                m_bPrinting = false;
                return false;
            }
#if EVENTLOG
            FS.ADIM.OA.WebUI.Print.CEventLog.Instance();
#endif
            word.Template = HttpContext.Current.Server.MapPath(this.TemplatePath + this.FileName + ".docx");

            sFileName = word.SavePath = HttpContext.Current.Server.MapPath(this.ExportPath + this.SaveFileName + ".docx");

            if (word.Config._alTemplateInfo.Count == 0)
            {
                word.Close();
                m_bPrinting = false;
                return false;
            }

            if (word.Open())
            {
                #region 模板替换处理
                try
                {
                    for (int i = 0; i < word.Config._alTemplateInfo.Count; i++)
                    {
                        SubItem item = ((SubItem)word.Config._alTemplateInfo[i]);
                        //string sValue = (_alExportData[i] as string).Replace("<br>", "\r\n");

                        switch (item.ExtraPara)
                        {
                            case "up":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Up);
                                break;
                            case "right":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Right);
                                break;
                            case "inner":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Inner);
                                break;
                            case "down":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Down);
                                break;
                            case "shift":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Shift);
                                break;
                            case "down_append":
                                word.Write(item.ColName, (string)_alExportData[i], WriteMode.Down_Append);
                                break;
                        }
                    }
                    if (OnExtraExport != null)
                        OnExtraExport(this, null);   // 调用IWord.Write(string key, string value, WriteMode mode, int offset);接口功能

                }
                catch (Exception ex)
                {
                    goto CONTINUE;
                }
                #endregion

            CONTINUE:
                try
                {
                    if (OnAttachExport != null)
                        OnAttachExport(this, null);

                    //word.DocLayout(this.Position, this.Mode);
                    if (OnBeforeClosed != null)
                        OnBeforeClosed(this, null);
                    //this.Pages = word.Pages;
                    word.Save();

                    if (ToPDF)
                    {
                        string sPdf = HttpContext.Current.Server.MapPath(this.ExportPath + this.SaveFileName + ".pdf");
                        string[] sRet = sPdf.Split(new char[] { '\\' });
                        word.Convert2pdf(sPdf);
                        word.Close();

                        if (OnCompletionExport != null)
                            OnCompletionExport(this, null);

                        m_bPrinting = false;
                        //TODO:清理附件列表生成的临时文件
                        ClearFileListTmpDirectory(TmpAttachFilesDirectory); //@"~\docdata\tmp\"
                        //PushFileToClient(sRet[sRet.Length - 1]);
                    }
                    else
                    {
                        string sDocx = HttpContext.Current.Server.MapPath(this.ExportPath + this.SaveFileName + ".docx");
                        word.Close();

                        if (OnCompletionExport != null)
                            OnCompletionExport(sender, null);

                        //TODO:清理附件列表生成的临时文件
                        ClearFileListTmpDirectory(TmpAttachFilesDirectory); //@"~\docdata\tmp\"

                        m_bPrinting = false;

                        //PushFileToClient(this.SaveFileName + ".docx");
                    }
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex.Message);
#if EVENTLOG
                    FS.ADIM.OA.WebUI.Print.CEventLog.Log("HN  " + ex.Message);
#endif
                    new Exception("由于某些原因本次打印任务失败,请重新打印");
                }
            }
            else
            {
                //throw new Exception("打开模板失败");
                //Response.Write("<script>alert('打开模板失败,请重试!')</script>");
                Alert("打开模板失败,请重试!");

            }

            return true;
        }
        #endregion

        #region 公开方法
        public void StartWorkerThread()
        {
            Thread thread = new Thread(Worker);
            thread.Start(this.m_ls as object);
        }
        public void Worker(object obj)
        {
            BatchPrint(obj as List<EntityBase>);
        }

        public void BatchPrint(List<EntityBase> arr)
        {
            //System.AppDomain.CurrentDomain.BaseDirectory
            TxtFileLogger log = new TxtFileLogger(HttpContext.Current.Server.MapPath(@"\Batch"), "");
            log.WriteLog("序号\tProcessID\t\t\t\tWorkItemID\t\t\t\t步骤\t文号\t\t\t标题");
            for (int i = 0; i < arr.Count; i++)
            {
                m_CurrEntity = arr[i];
                string sFileName = "";
                if (BackGroundPrint(this, out sFileName, "",
                    "",
                    ""))
                {
                    log.WriteLog((i + 1).ToString() + "\t"
                        + m_CurrEntity.ProcessID + "\t"
                        + m_CurrEntity.WorkItemID + "\t"
                        + m_CurrEntity.StepName + "\t"
                        + m_CurrEntity.DocumentNo + "\t"
                        + m_CurrEntity.DocumentTitle + "\t成功 " + sFileName);
                }
                else
                {
                    log.WriteLog((i + 1).ToString() + "\t"
                        + m_CurrEntity.ProcessID + "\t"
                        + m_CurrEntity.WorkItemID + "\t"
                        + m_CurrEntity.StepName + "\t"
                        + m_CurrEntity.DocumentNo + "\t"
                        + m_CurrEntity.DocumentTitle + "\t失败");
                }
            }
        }

        /// <summary>
        /// 党纪工团发文	
        ///    党委公文首页纸模板:
        ///        会签：
        ///    工会首页纸
        ///        会签：
        ///    共青团首页纸
        ///        会签：
        ///    纪律检查委员会首页纸
        ///        会签：
        /// 公司发文
        ///    公文首页纸
        ///        会签：核稿：
        /// 函件发文
        ///    函件发文表单
        ///        会签：
        /// </summary>
        /// <param name="sProcess"></param>
        /// <param name="sTemplate"></param>
        /// <param name="sNodeName"></param>
        /// <returns>如果待签名图片在如上流程模板的节点中出现则返回TRUE,否则返回FALSE</returns>
        private bool IsMultiSigners(string sProcess, string sTemplate, string[] sParams,
                                    string sNodeName, CRect rect, EntityBase entity,
                                    double fLenRatio, double fWidRatio, string sDisplay)
        {
            bool bRet = false;
            DataTable dt = null; //new DataTable();
            Dictionary<string, string> dictSigners = new Dictionary<string, string>();
            switch (sProcess)
            {
                case "党纪工团发文":
                    if ((sTemplate == "党委公文首页纸模板" ||
                        sTemplate == "工会首页纸" ||
                        sTemplate == "共青团首页纸" ||
                        sTemplate == "纪律检查委员会首页纸") && sNodeName == "会签：")
                    {
                        bRet = true;

                        //string DetpSigners = FormsMethod.GetSingers4Print(entity.ProcessID, entity.WorkItemID, "部门会签", "党纪工团发文");
                        //string[] results = DetpSigners.Split(new char[] { '[', ']', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        //string Stamp = "";
                        //for (int k = 1; k < results.Length; k += 3)
                        //{
                        //    Stamp += (results[k] + " " + results[k + 1] + "\n");
                        //}
                        dt = B_FormsData.GetStepInfo(entity.ProcessID, entity.ReceiveDateTime);
                        DataRow[] rows = dt.Select("NAME = '部门会签'  and  COMPLETED_DATE IS NOT NULL", "COMPLETED_DATE DESC");
                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (!dictSigners.ContainsKey(rows[i]["USER_ID"].ToString()))
                            {
                                dictSigners.Add(rows[i]["USER_ID"].ToString(), rows[i]["COMPLETED_DATE"].ToString());
                                word.AddPicture(rect, sParams, rows[i]["COMPLETED_DATE"].ToString(), fLenRatio, fWidRatio, sDisplay, rows[i]["USER_ID"].ToString());
                            }
                        }
                    }
                    break;
                case "公司发文":
                    if (sTemplate == "公文首页纸")
                    {

                        //string[] results = DetpSigners.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        //string sVerify = (string.IsNullOrEmpty(entity.GetVal("ZhuRenSigner").ToString()) ? "" : entity.GetVal("ZhuRenSigner").ToString() + " " + ucPrint.CheckDateTime(entity.GetVal("ZhuRenSignDate").ToString()) + "\r\n");
                        //string sCVerify = (string.IsNullOrEmpty(entity.GetVal("Verifier").ToString()) ? "" : entity.GetVal("Verifier").ToString() + " " + ucPrint.CheckDateTime(entity.GetVal("VerifyDate").ToString()));
                        if (sNodeName == "会签：")
                        {
                            bRet = true;
                            //string DetpSigners = FormsMethod.GetSingers4Print(entity.ProcessID, entity.WorkItemID, "部门会签", "公司发文");
                            //string[] result = DetpSigners.Split(new char[] { '[', ']', ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            //string Stamp = "";
                            //for (int k = 1; k < result.Length; k += 3)
                            //{
                            //    Stamp += result[k] + " " + result[k + 1] + "\n";
                            //}
                            dt = B_FormsData.GetStepInfo(entity.ProcessID, entity.ReceiveDateTime);
                            DataRow[] rows = dt.Select("NAME = '部门会签'  and  COMPLETED_DATE IS NOT NULL", "COMPLETED_DATE DESC");
                            for (int i = 0; i < rows.Length; i++)
                            {
                                if (!dictSigners.ContainsKey(rows[i]["USER_ID"].ToString()))
                                {
                                    dictSigners.Add(rows[i]["USER_ID"].ToString(), rows[i]["COMPLETED_DATE"].ToString());
                                    word.AddPicture(rect, sParams, rows[i]["COMPLETED_DATE"].ToString(), fLenRatio, fWidRatio, sDisplay, rows[i]["USER_ID"].ToString());
                                }
                            }
                        }

                        if (sNodeName == "核稿：")
                        {
                            bRet = true;
                            //string value = "";
                            //if (!(entity.GetVal("ZhuRenSignDate").ToString() == "" || entity.GetVal("ZhuRenSignDate").ToString() == DateTime.MinValue.ToString()))
                            //{
                            //    value = entity.GetVal("ZhuRenSignDate").ToString() + "\n";
                            //}
                            //if (!(entity.GetVal("VerifyDate").ToString() == "" || entity.GetVal("VerifyDate").ToString() == DateTime.MinValue.ToString()))
                            //{
                            //    value += entity.GetVal("VerifyDate").ToString() + "\n";
                            //}
                            //word.AddPicture(rect, sParams, value, fLenRatio, fWidRatio, sDisplay, sUserID);
                            dt = B_FormsData.GetStepInfo(entity.ProcessID, entity.ReceiveDateTime);
                            DataRow[] rows = dt.Select("NAME = '主任核稿'  and  COMPLETED_DATE IS NOT NULL", "COMPLETED_DATE DESC");
                            DataRow[] rows1 = dt.Select("NAME = '核稿'  and  COMPLETED_DATE IS NOT NULL", "COMPLETED_DATE DESC");
                            for (int i = 0; i < rows.Length; i++)
                            {
                                if (!dictSigners.ContainsKey(rows[i]["USER_ID"].ToString()))
                                {
                                    dictSigners.Add(rows[i]["USER_ID"].ToString(), rows[i]["COMPLETED_DATE"].ToString());
                                    word.AddPicture(rect, sParams, rows[i]["COMPLETED_DATE"].ToString(), fLenRatio, fWidRatio, sDisplay, rows[i]["USER_ID"].ToString());
                                }
                            }
                            if (rows1.Length != null && rows1.Length > 0)
                                word.AddPicture(rect, sParams, rows1[0]["COMPLETED_DATE"].ToString(), fLenRatio, fWidRatio, sDisplay, rows1[0]["USER_ID"].ToString());
                        }
                    }
                    break;
                case "函件发文":
                    if (sTemplate == "函件发文表单" && sNodeName == "会签/日期：")
                    {
                        bRet = true;
                        //string sTimeStamp = entity.GetVal("huiqianDates").ToString();

                        //string[] res = sTimeStamp.Split(new char[] { ' ', ';' });
                        //string Stamp = "";
                        //for (int k = 1; k < res.Length; k += 3)
                        //{
                        //    Stamp += res[k] + " " + res[k + 1] + "\n";
                        //}
                        //word.AddPicture(rect, sParams, Stamp, fLenRatio, fWidRatio, sDisplay, sUserID);

                        dt = B_FormsData.GetStepInfo(entity.ProcessID, entity.ReceiveDateTime);
                        DataRow[] rows = dt.Select("NAME = '会签'  and  COMPLETED_DATE IS NOT NULL", "COMPLETED_DATE DESC");
                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (!dictSigners.ContainsKey(rows[i]["USER_ID"].ToString()))
                            {
                                dictSigners.Add(rows[i]["USER_ID"].ToString(), rows[i]["COMPLETED_DATE"].ToString());
                                word.AddPicture(rect, sParams, rows[i]["COMPLETED_DATE"].ToString(), fLenRatio, fWidRatio, sDisplay, rows[i]["USER_ID"].ToString());
                            }
                        }
                    }
                    break;
            }
            return bRet;
        }

        private int IsMultiPersons(DataTable dt)
        {
            string sName = "";
            int iCnt = 0;
            if(dt != null)
            {    
                DataRow[] rows = dt.Select("", "USER_ID ASC");
                if(rows != null && rows.Length >= 1)
                {
                    sName = rows[0]["USER_ID"].ToString();
                }
                for (int i = 0; i < rows.Length; i++)
                {
                    if (sName == rows[i]["USER_ID"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        iCnt++;
                        sName = rows[i]["USER_ID"].ToString();
                    }
                }
            }
            return iCnt;
        }

        public void BatchAddPicture(string sProcess, string sTemplate, EntityBase entity)
        {
            if (!ChkSingerFolder()) return;
            XmlDocument doc = new XmlDocument();
            if (sProcess.Equals("程序文件")){
                doc.Load(HttpContext.Current.Server.MapPath(@"~\template\SingerCfgProcess.xml"));
            } else {
                doc.Load(HttpContext.Current.Server.MapPath(@"~\template\SingerCfg.xml"));
            }

            #region 处理签名图片缩放比例
            /**********************处理签名图片缩放比例 START******************************/
            XmlNode rootNode = doc.SelectSingleNode("/Signer");
            double fLenRatio = 0.0;
            double fWidRatio = 0.0;
            if (rootNode.Attributes.Count != 0)
            {
                string sRatio = rootNode.Attributes["Ratio"].Value;
                string[] arrRatio = sRatio.Split(':');

                if (arrRatio != null && arrRatio.Length == 2)
                {
                    if (!Double.TryParse(arrRatio[0], out fLenRatio))
                    {
                        fLenRatio = 0;
                    }
                    if (!Double.TryParse(arrRatio[1], out fWidRatio))
                    {
                        fWidRatio = 0;
                    }
                }
            }
            /**********************处理签名图片缩放比例 END*********************************/
            #endregion

            //XmlNode sNode = doc.SelectSingleNode("/Signer/Process[@Name='" + sProcess + "']/Template[@Name='" + sTemplate + "']");
            //sNode.AppendChild(
            XmlNodeList nodes = doc.SelectNodes("/Signer/Process[@Name='" + sProcess + "']/Template[@Name='" + sTemplate + "']");
            if (nodes == null) return;

            DataTable dtStepComleted = B_FormsData.GetStepInfo(entity.ProcessID, entity.ReceiveDateTime);
            
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].ChildNodes.Count; j++)
                {
                    try
                    {
                        XmlNode node = nodes[i].ChildNodes[j];
                        //string skey = node.Attributes["Name"].Value;
                        //string sFileName = HttpContext.Current.Server.MapPath(@"~\template\Singer\" + skey + ".png");
                        CRect rect = new CRect();
                        if (node.Attributes["Left"] != null)
                            rect.Left = Convert.ToInt32((string.IsNullOrEmpty(node.Attributes["Left"].Value) ? "0" : node.Attributes["Left"].Value));
                        if (node.Attributes["Top"] != null)
                            rect.Top = Convert.ToInt32((string.IsNullOrEmpty(node.Attributes["Top"].Value) ? "0" : node.Attributes["Top"].Value));
                        if (node.Attributes["Width"] != null)
                            rect.Width = Convert.ToInt32((string.IsNullOrEmpty(node.Attributes["Width"].Value) ? "0" : node.Attributes["Width"].Value));
                        if (node.Attributes["Height"] != null)
                            rect.Height = Convert.ToInt32((string.IsNullOrEmpty(node.Attributes["Height"].Value) ? "0" : node.Attributes["Height"].Value));
                        string[] sParams = node.InnerText.Split('|');
                        if (sParams.Length != 2) return;

                        #region Display属性处理
                        string sDisplay = "";
                        if (node.Attributes["Display"] != null)
                        {
                            sDisplay = node.Attributes["Display"].Value.ToLower();
                        }
                        #endregion
                        string sTimeStamp = "";
                        string sUserID = "";
                        
                        #region StampName属性处理
                        /*
                        if (node.Attributes["StampName"] != null)
                        {
                            string[] result = node.Attributes[4].Value.Split(new char[] { ';', ',' });

                            //string value = node.Attributes[4].Value;
                            string value = "";
                            try
                            {
                                value = entity.GetVal(result[0]).ToString();
                            }
                            catch
                            {
                                word.AddPicture(rect, sParams, "", fLenRatio, fWidRatio, sDisplay, sUserID);
                                continue;
                            }
                            if (string.IsNullOrEmpty(value))
                            {
                                if (result.Length == 2 &&
                                    (entity.GetVal(result[1]).GetType().Name == "DateTime"
                                    || entity.GetVal(result[1]).GetType().Name == "String")
                                   )
                                {
                                    sTimeStamp = entity.GetVal(result[1]).ToString();
                                    DateTime dt = Convert.ToDateTime(sTimeStamp);
                                    if (dt == DateTime.MinValue)
                                        sTimeStamp = "";
                                }
                            }
                            else
                            {
                                if (entity.GetVal(result[0]).GetType().Name == "DateTime"
                                    || entity.GetVal(result[0]).GetType().Name == "String")
                                {
                                    sTimeStamp = entity.GetVal(result[0]).ToString();
                                    if (result[0] == "niGaoRenDate")
                                    {
                                        string[] res = sTimeStamp.Split(' ');
                                        if (res.Length > 2)
                                            sTimeStamp = res[1] + ' ' + res[2];
                                        else
                                            sTimeStamp = "";
                                    }
                                    //if (result[0] == "huiqianDates")
                                    //{
                                    //    string[] res = sTimeStamp.Split(' ');
                                    //    if (res.Length > 2)
                                    //        sTimeStamp = res[1] + ' ' + res[2];
                                    //    else
                                    //        sTimeStamp = "";
                                    //}
                                    DateTime dt = Convert.ToDateTime(sTimeStamp);
                                    if (dt == DateTime.MinValue)
                                        sTimeStamp = "";
                                }
                            }
                            #region 忽略
                            ////renjinquan+ 针对函件发文时间格式异常做的处理（'姓名 日期 时间'），去掉姓名
                            //if (ProcessConstString.TemplateName.LETTER_SEND.Contains(this.UCTemplateName) &&
                            //    (result[0].Equals("niGaoRenDate", StringComparison.CurrentCultureIgnoreCase) ||
                            //    result[0].Equals("huiqianDates", StringComparison.CurrentCultureIgnoreCase)))
                            //{
                            //    //会签时可能有多个人
                            //    foreach (string str in value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            //    {
                            //        if (str.Split(' ').Length > 2)
                            //        {
                            //            sTimeStamp = str.Substring(str.IndexOf(' ') + 1);
                            //        }
                            //        word.AddPicture(rect, sParams, sTimeStamp);

                            //    }
                            //    continue;
                            //}

                            //}
                            //else if (this.UCTemplateName == ProcessConstString.TemplateName.COMPANY_SEND && node.InnerText.Contains("会签"))
                            //{
                            //    string[] results = FormsMethod.GetSingers4Print(entity.ProcessID, entity.WorkItemID, "部门会签", "公司发文").Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                            //    for (int k = 1; k < results.Length; k += 2)
                            //    {
                            //        word.AddPicture(rect, sParams, results[k]);
                            //    }
                            //    continue;
                            //}
                            #endregion

                        }
                        */
                        #endregion
                        
                        #region Step属性节点处理
                        DataRow[] rowsStamp = null;

                        if (node.Attributes["Step"] != null)
                        {
                            string[] sResult = node.Attributes["Step"].Value.Split(':');
                            string sDateFlag = ""; // 较早 较晚
                            if (sResult != null && sResult.Length == 2) //sResult[0] 步骤 sResult[1] 较早/较晚
                            {
                                #region 多人会签/多人核稿 签名图片处理
                                /********多人会签/多人核稿 签名图片处理 START********/
                                /*
                                    党纪工团发文	
                                        党委公文首页纸模板:
                                            会签
                                        工会首页纸
                                            会签
                                        共青团首页纸
                                            会签
                                        纪律检查委员会首页纸
                                            会签
                                    公司发文
                                        公文首页纸
                                            会签、核稿
                                    函件发文
                                        函件发文表单
                                            会签
                                 */
                                if (IsMultiSigners(sProcess, sTemplate, sParams,
                                                    sParams[0], rect, entity,
                                                    fLenRatio, fWidRatio, sDisplay))
                                {
                                    continue;
                                }
                                /*******多人会签/多人核稿 签名图片处理 END***********/
                                #endregion
                                if (sParams[1].ToLower() == "downs")
                                {
                                    if (sResult[1] == "较早")
                                        sDateFlag = "COMPLETED_DATE ASC";
                                    if (sResult[1] == "较晚")
                                        sDateFlag = "COMPLETED_DATE DESC";
                                    rowsStamp = dtStepComleted.Select("NAME='" + sResult[0] + "' and  COMPLETED_DATE IS NOT NULL", sDateFlag);
                                    if (rowsStamp != null && rowsStamp.Length > 0)
                                    {
                                        for (int n = 0; n < rowsStamp.Length; n++)
                                        {
                                            sTimeStamp += rowsStamp[n]["COMPLETED_DATE"].ToString() + "\n";
                                            sUserID += rowsStamp[n]["USER_ID"].ToString() + "\n";
                                        }
                                    }
                                }
                                else
                                {
                                    if (sResult[1] == "较早")
                                        sDateFlag = "COMPLETED_DATE ASC";
                                    if (sResult[1] == "较晚")
                                        sDateFlag = "COMPLETED_DATE DESC";
                                    rowsStamp = dtStepComleted.Select("NAME='" + sResult[0] + "' and  COMPLETED_DATE IS NOT NULL", sDateFlag);
                                    if (rowsStamp != null && rowsStamp.Length > 0)
                                    {
                                        sTimeStamp = rowsStamp[0]["COMPLETED_DATE"].ToString();
                                        sUserID = rowsStamp[0]["USER_ID"].ToString();
                                    }
                                }
                            }
                        }
                        #endregion

                        word.AddPicture(rect, sParams, sTimeStamp, fLenRatio, fWidRatio, sDisplay, sUserID);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            // 清除签名图片所在的临时目录下的文件
            //ClearFileListTmpDirectory(@"~\template\Singer\");
        }

        public void WriteByFont(int iChar, int lblMark, string FontName)
        {
            word.WriteByFont(iChar, lblMark, FontName);
        }

        public void WriteEx(string key, string value, WriteMode mode)
        {
            word.WriteEx(key, value, mode);
        }

        public void Write(string key, string value, WriteMode mode, int offset)
        {
            word.Write(key, value, mode, offset);
        }

        public ArrayList List2ArrayList(object oList)
        {
            ArrayList al = new ArrayList();

            return al;
        }

        public void WriteTable(int index, ArrayList al)
        {
            word.WriteTable(index, al);
        }

        public void WriteTable(int TblIndex, int SubTblIdx, ArrayList al)
        {
            word.WriteTable(TblIndex, SubTblIdx, al);
        }

        public void WriteHeaderFooter(string key, string[] value, WriteMode mode)
        {
            word.WriteHeaderFooter(key, value, mode);
        }

        public void DocLayout(int tableIndex, int iRow, int iCol)
        {
            word.DocLayout(tableIndex, iRow, iCol);
        }

        public void WriteContent(string key, WriteMode mode, int offset)
        {
            if (alContentFiles.Count > 0)
            {
                for (int i = 0; i < alContentFiles.Count; i++)
                    word.WriteFile(key, (string)alContentFiles[i], mode, offset);
            }
        }

        public void WriteAttach()
        {
            if (alAttachFiles.Count > 0)
            {
                for (int i = 0; i < alAttachFiles.Count; i++)
                    word.WriteFile((string)alAttachFiles[i]);
            }
        }

        public void DeleteTable(int TblIdx)
        {
            word.DeleteTable(TblIdx);
        }

        public void DeleteString(string sKey)
        {
            word.DeleteString(sKey);
        }
        /// <summary>
        /// 将附件列表名称按顺序输出到 "表单模板" 的附件位置处
        /// </summary>
        /// <param name="filelist">附件集合</param>
        /// <returns>生成待导出到表单模板的字符串</returns>
        public string AttachFilesList(List<CFuJian> filelist)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < filelist.Count; i++)
            {
                sb.Append((i + 1).ToString() + ". " + filelist[i].Alias + "." + filelist[i].Type);
                sb.Append("\r\n");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 验证是否有中文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsCN(string s)
        {
            bool b = false;
            char[] cs = s.ToCharArray();
            for (int n = 0; n < cs.Length; n++)
            {
                if (cs[n] > 128)//   中文字符
                {
                    b = true;
                    break;
                }
            }
            return b;
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
            if (IsCN(strDateTime) == false)
                strDateTime = SysConvert.ToDateTime(strDateTime).ToShortDateString();
            return strDateTime;
        }

        public string FormatDataTime(string sDateTime)
        {
            if (string.IsNullOrEmpty(sDateTime)) return "";

            string[] result = sDateTime.Split(new char[] { '/', '-', '年', '月', '日' });
            if (result.Length == 3)
            {
                if (result[0].Length == 4)
                {
                    string sYear = TransformDateFormat(result[0]);//年份
                    string sMonth = TransformDateFormat(result[1]);//月份
                    string sDay = TransformDateFormat(result[2]);//日期
                    return sYear + "年" + sMonth + "月" + sDay + "日";
                }
                else
                {
                    string sMonth = TransformDateFormat(result[0]);//月份
                    string sDay = TransformDateFormat(result[1]);//日期
                    string sYear = TransformDateFormat(result[2]);//年份
                    return sMonth + "月" + sDay + "日" + sYear + "年";
                }

            }
            return sDateTime;
        }

        public string TransformDateFormat(string sDateTime)
        {
            string sResult = "";
            for (int i = 0; i < sDateTime.Length; i++)
            {
                sResult += ConvertArebToCharacter(sDateTime[i]);
            }
            return sResult;
        }

        public char ConvertArebToCharacter(char cNumber)
        {
            char cResult = new char();
            switch (cNumber)
            {
                case '0':
                    cResult = '〇';
                    break;
                case '1':
                    cResult = '一';
                    break;
                case '2':
                    cResult = '二';
                    break;
                case '3':
                    cResult = '三';
                    break;
                case '4':
                    cResult = '四';
                    break;
                case '5':
                    cResult = '五';
                    break;
                case '6':
                    cResult = '六';
                    break;
                case '7':
                    cResult = '七';
                    break;
                case '8':
                    cResult = '八';
                    break;
                case '9':
                    cResult = '九';
                    break;
            }
            return cResult;
        }

        #endregion
    }
}
