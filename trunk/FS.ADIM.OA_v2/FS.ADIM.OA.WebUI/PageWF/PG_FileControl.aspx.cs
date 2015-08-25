using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FS.ADIM.OA.WebUI.PageOU;
using FS.ADIM.OA.BLL.Entity;
using Brettle.Web.NeatUpload;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OA.BLL.Common;
using System.IO;
using FS.OA.Framework;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.WebUI.MOSSOA;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.PageWF
{
    public partial class PG_FileControl : OAPGBase
    {
        #region 变量定义

        /// <summary>
        /// 附件
        /// </summary>
        public List<CFuJian> UCDataList
        {
            get
            {
                if (ViewState["UCDataList"] == null)
                    ViewState["UCDataList"] = MossObject.Xml2FuJianList(txtFJXML.Value);
                return ViewState["UCDataList"] as List<CFuJian>;
            }
            set
            {
                txtFJXML.Value = MossObject.FuJianList2Xml(value);
                ViewState["UCDataList"] = value;
            }
        }
        /// <summary>
        /// ID 草稿箱的附件判断
        /// </summary>
        public String UCTBID
        {
            get
            {
                if (ViewState["UCTBID"] == null)
                {
                    String sID = base.GetQueryString("UCTBID");
                    if (sID == "")
                    {
                        if (Session["UCTBID"] != null)
                        {
                            sID = Session["UCTBID"].ToString();
                            Session.Remove("UCTBID");
                        }
                    }
                    ViewState["UCTBID"] = sID;
                }
                return ViewState["UCTBID"] as String;
            }
        }

        /// <summary>
        /// 流程类型
        /// </summary>
        public String UCProcessType
        {
            get
            {
                String ProcessType = base.GetQueryString("UCProcessType");
                if (ProcessType.Contains("新版"))
                {
                    ProcessType = ProcessType.Replace("新版", "");
                }
                return ProcessType;
            }
        }
        /// <summary>
        /// 流程实例号 
        /// </summary>
        public String UCProcessID
        {
            get
            {
                return base.GetQueryString("UCProcessID");
            }
        }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public Boolean UCIsEditable
        {
            get
            {
                return Convert.ToBoolean(base.GetQueryString("UCIsEditable"));
            }
        }
        /// <summary>
        /// 流程节点作业号
        /// </summary>
        public String UCWorkItemID
        {
            get
            {
                return base.GetQueryString("UCWorkItemID");
            }
        }
        //ID控件
        protected String UCControlID
        {
            get
            {
                return base.GetQueryString("UCControlID");
            }
        }

        /// <summary>
        /// SelectURL 
        /// </summary>
        protected String SelectURL
        {
            get
            {
                if (ViewState["SelectURL"] == null)
                    ViewState["SelectURL"] = "";
                return ViewState["SelectURL"] as String;
            }
            set
            {
                ViewState["SelectURL"] = value;
            }
        }

        public String UCIsAgain
        {
            get
            {
                return base.GetQueryString("UCIsAgain");
            }
        }
        #endregion

        #region 加载
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //进度条
                inlineProgressBar.AddTrigger(btnUpload);

                //首次加载
                if (!IsPostBack)
                {
                    //Response.Expires = -1;
                    //Response.ExpiresAbsolute = DateTime.Now;
                    //Response.CacheControl = "no-cache";

                    //设置控件状态
                    if (UCIsAgain == "")
                    {
                        if (!UCIsEditable)
                        {
                            tbUpload.Visible = false;
                            divUpload.Visible = false;
                            btnOL.Visible = false;
                        }
                    }

                    #region 接收值

                    //初始化赋值
                    if (Session["附件ListTemp"] != null) //从用户控件接收过来
                    {

                        UCDataList = MossObject.Xml2FuJianList((String)Session["附件ListTemp"]);
                        txtFJXML.Value = Session["附件ListTemp"].ToString();
                        Session.Remove("附件ListTemp"); //移除Session
                    }
                    #endregion

                    multiFile.StorageConfig["tempDirectory"] = Path.Combine("App_Data", "file1temp");


                    //zhouli 处理历史附件
                    if (UCDataList.Count > 0 && UCWorkItemID != "" && UCIsEditable)
                    {
                        MossObject mossObject = new MossObject();
                        UCDataList = mossObject.GetNewListFuJian(UCProcessType, UCWorkItemID, UCDataList);
                    }
                    RepeaterFiles.DataSource = UCDataList;
                    RepeaterFiles.DataBind();

                    #region 在线编辑设置
                    //在线编辑设置
                    //第一个是webservice地址 第二个是本地路径地址 第三个moss server web地址 第四个当前用户名
                    txtServicePath.Value = OAConfig.GetConfig("MOSS认证", "MossServiceUrl") + @",C:\\LocalTemp," + OAConfig.GetConfig("MOSS认证", "ServerWeb") + "," + CurrentUserInfo.DisplayName + ",";
                    if (!UCIsEditable)
                    {
                        Panel1.Visible = false;
                    }
                    else
                    {
                        btnOL.Attributes.Add("onClick", String.Format("AttachmentWork('{0}','{1}');", UCControlID, txtServicePath.ClientID));
                    }
                    #endregion

                    RunScript();
                }

            }
            catch (Exception ex)
            {
                JScript.ShowMsgBox(Page, MsgType.VbExclamation, ex.Message);
            }
        }
        protected void RepeaterFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int i = e.Item.ItemIndex;

            CheckBox rb = (CheckBox)e.Item.FindControl("chkStatus");

            if (!UCIsEditable)
            {
                if (rb != null)
                    rb.Enabled = false;
            }

            String type = (e.Item.DataItem as CFuJian).Type;
            if (type == "doc" || type == "docx")
            {
                rb.Visible = true;
                //正文打勾
                if ((e.Item.DataItem as CFuJian).IsZhengWen == "1")
                {
                    rb.Checked = true;
                }
            }
            else
            {
                rb.Visible = false;
            }

            LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
            if (!UCIsEditable)
            {
                btnEdit.Visible = false;
            }
        }

        #endregion

        #region 上传 删除
        /// <summary>
        /// 回传值
        /// </summary>
        private void RunScript()
        {
            String script = "";

            script += base.GetJSscriptXMLValue(UCControlID, MossObject.FuJianList2Xml(UCDataList));

            //组成一整条js后运行
            ClientScriptM.ResponseScript(this, script);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(UCProcessType))
                {
                    JScript.ShowMsgBox(Page, MsgType.VbExclamation, "文档库设置不能为空！");
                    return;
                }
                if (multiFile.Files.Length <= 0)
                {
                    return;
                }

                foreach (UploadedFile file in multiFile.Files)
                {
                    if (file.FileName.Contains("#") || file.FileName.Contains("'"))
                    {
                        JScript.ShowMsgBox(Page, MsgType.VbExclamation, file.FileName + "含有特殊字符，请替换后再上传");
                        JScript.ResponseScript(this, "javascript:__doPostBack('LinkButton1','')");
                        return;
                    }                    
                }
                foreach (UploadedFile file in multiFile.Files)
                {
                    if (file.ContentLength <= MossObject.maxFileSize * 1024 * 1024)
                    {
                        MossObject attach = new MossObject();
                        attach.ServerWeb= OAConfig.GetConfig("MOSS认证", "ServerWeb");
                        attach.OldFileName = file.FileName;
                        attach.DocumentName = UCProcessType;
                        #region 更新栏位
                        List<DictionaryEntry> lst = new List<DictionaryEntry>();
                        DictionaryEntry de = new DictionaryEntry();
                        de.Key = "流程实例";
                        de.Value = UCProcessID;
                        lst.Add(de);

                        de = new DictionaryEntry();
                        de.Key = "别名";
                        de.Value = file.FileName;
                        lst.Add(de);

                        de = new DictionaryEntry();
                        de.Key = "上次修改者";
                        de.Value = CurrentUserInfo.DisplayName;
                        lst.Add(de);
                        #endregion
                        DictionaryEntry[] result = attach.ConvertToDE(lst.ToArray());
                        attach.DocumentEntry = result;
                        //文件扩展名
                        string fileType = System.IO.Path.GetExtension(file.FileName);
                        if (fileType.IndexOf('.') == 0)
                        {
                            fileType = fileType.Substring(1);
                        }
                        bool IsSuccess = false;
                        if ("exe,dll".Contains(fileType.ToLower()))
                        {
                            IMessage im = new WebFormMessage(Page, "不可上传exe或dll文件！");
                            im.Show();

                            IsSuccess = false;
                        }
                        else
                        {
                            attach.UploadFilesStream = file.InputStream;
                            //上传附件并更新栏位
                            IsSuccess = attach.Upload();
                        }
                        if (IsSuccess)
                        {
                            CFuJian ff = new CFuJian();
                            ff.Type = System.IO.Path.GetExtension(attach.FileName);

                            if (ff.Type.IndexOf('.') == 0)
                            {
                                ff.Type = ff.Type.Substring(1);

                            }
                            ff.Title = attach.FileName;
                            ff.Alias = System.IO.Path.GetFileNameWithoutExtension(attach.OldFileName);
                            ff.FolderName = attach.FolderName;
                            ff.FileName = attach.FileName;

                            ff.Size = attach.ToFileSize_new(file.ContentLength); //文件大小
                            ff.ProcessType = UCProcessType;
                            ff.WorkItemID = UCWorkItemID;

                            ff.fullURL = attach.UploadFullName; //全路径
                            ff.URL = attach.UploadURL;//文件夹+/文件名
                            UCDataList.Add(ff);
                        }
                        //CFuJian l_objAttachment = MossObject.Upload(file, UCProcessType, UCProcessID, UCWorkItemID);
                        //UCDataList.Add(l_objAttachment);
                    }
                    else
                    {
                        JScript.ShowMsgBox(Page, MsgType.VbExclamation, file.FileName + "大小超过" + MossObject.maxFileSize + "M");
                    }
                }

                RepeaterFiles.DataSource = UCDataList;
                RepeaterFiles.DataBind();
                RunScript();
            }
            catch (Exception ex)
            {
                JScript.ShowMsgBox(Page, MsgType.VbExclamation, ex.Message);
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                String URL = (sender as LinkButton).CommandName;

                if (UCIsAgain == "1")// 二次分发不刷新实体和不真正删除附件
                {
                    Remove(URL);
                }
                else
                {
                    int ret = MossObject.Del(UCProcessType, URL);

                    if (ret > 0)
                    {
                        Remove(URL);
                        //刷新实体
                        MossObject mossObject = new MossObject();
                        mossObject.SaveNewList(UCProcessType, UCWorkItemID, UCDataList, UCTBID);
                    }
                }
                RepeaterFiles.DataSource = UCDataList;
                RepeaterFiles.DataBind();

                RunScript();
            }
            catch (Exception ex)
            {
                JScript.ShowMsgBox(Page, MsgType.VbExclamation, ex.Message);
            }
        }

        /// <summary>
        /// 在线编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btnEdit = sender as LinkButton;
            String title = btnEdit.CommandName;
            ClientScriptM.ResponseScript(this, String.Format("AttachmentOpen('{0}','{1}','{2}');", UCControlID, txtServicePath.ClientID, title), true);
        }
        #endregion

        #region 私有方法

        private String GetAbsoluteUrl(String RelatedUrl)
        {
            String UrlPrefix = Request.Url.Scheme + @"://" + Request.Url.Authority;
            if (Request.ApplicationPath.Equals("/"))
                return UrlPrefix + RelatedUrl.Replace("~", String.Empty);
            else
                return UrlPrefix + Request.ApplicationPath + RelatedUrl.Replace("~", String.Empty);
        }
        private String GetFileAlias(String url)
        {
            for (int i = 0; i < UCDataList.Count; i++)
            {
                if ((UCDataList[i] as CFuJian).URL == url)
                    return (UCDataList[i] as CFuJian).Alias + "." + (UCDataList[i] as CFuJian).Type;
            }
            return "default";
        }
        private CFuJian GetFileInfo(String url)
        {
            for (int i = 0; i < UCDataList.Count; i++)
            {
                if ((UCDataList[i] as CFuJian).URL == url)
                    return UCDataList[i] as CFuJian;
            }
            return null;
        }
        private void Remove(String url)
        {
            int index = -1;
            for (int i = 0; i < UCDataList.Count; i++)
            {
                if ((UCDataList[i] as CFuJian).URL == url)
                    index = i;
            }
            UCDataList.RemoveAt(index);
        }
        #endregion

        #region 页面按钮状态

        //判断是否是传阅还是可编辑
        protected String CheckChuanYue2(String type)
        {
            if (UCIsAgain == "1")
            {
                return "删除";
            }
            if (!UCIsEditable)
            {
                return "";
            }
            else
            {
                return "删除";
            }
        }
        protected String GetTitleName(String alias, String type)
        {
            if (type == "")
            {
                return SysString.CutHtml(alias, 20);
            }
            else
            {
                return SysString.CutHtml(alias, 20) + "." + type;
            }
        }
        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.RepeaterFiles.Items)
            {
                CheckBox rb = (CheckBox)item.FindControl("chkStatus");
                LinkButton lBtn = (LinkButton)item.FindControl("btnDownload");
                String ff = "";

                if (lBtn != null)
                {
                    ff = lBtn.CommandName;
                }

                if (rb != null)
                {
                    if (ff == SelectURL)
                    {
                        rb.Checked = false;
                    }
                }
            }
            bool isALLFalse = true;
            foreach (RepeaterItem item in this.RepeaterFiles.Items)
            {
                CheckBox rb = (CheckBox)item.FindControl("chkStatus");
                LinkButton lBtn = (LinkButton)item.FindControl("btnDownload");
                String ff = "";

                if (lBtn != null)
                {
                    ff = lBtn.CommandName;
                }
                if (rb != null)
                {
                    if (rb.Checked)
                    {
                        isALLFalse = false;
                        SelectURL = ff; //URL
                    }
                }
            }
            if (isALLFalse)
            {
                SelectURL = "";
            }
            for (int i = 0; i < UCDataList.Count; i++)
            {
                if (isALLFalse)
                {
                    UCDataList[i].IsZhengWen = "0";
                }
                else
                {
                    if (UCDataList[i].URL == SelectURL)
                    {
                        UCDataList[i].IsZhengWen = "1";
                    }
                    else
                    {
                        UCDataList[i].IsZhengWen = "0";
                    }
                }
            }
            RunScript();
        }
        #endregion

        #region 在线编辑后
        protected void btnOL_Click(object sender, EventArgs e)
        {
            //RepeaterFiles.DataSource = UCDataList;//MossObject.Xml2FuJianList(txtFJXML.Value);
            RepeaterFiles.DataSource = MossObject.Xml2FuJianList(txtFJXML.Value);
            RepeaterFiles.DataBind();
            RunScript();
        }
        #endregion
    }
}
