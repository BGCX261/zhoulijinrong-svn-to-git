using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.UIBase
{
    public class TemplateAdmin
    {
        public static TemplateAdmin CreateTemplateAdmin(Page page)
        {
            TemplateAdmin TAdmin = new TemplateAdmin(page);
            if (TAdmin == null)
                throw new Exception("模板Config文件初始化失败");
            return TAdmin;
        }
        public static TemplateAdmin CreateTemplateAdmin(Page page, bool ResetConfig)
        {
            TemplateAdmin TAdmin = new TemplateAdmin(page, ResetConfig);
            if (TAdmin == null)
                throw new Exception("模板Config文件初始化失败");
            return TAdmin;
        }

        #region Parameters
        private string _ConfigFile;

        /// <summary>
        /// Config文件路径
        /// </summary>
        public string ConfigFile
        {
            get { return _ConfigFile; }
            set { _ConfigFile = value; }
        }
        private Templates _Templates;

        /// <summary>
        /// 模板列表对象
        /// </summary>
        public Templates Templates
        {
            get
            {
                if (_Templates == null)
                    _Templates = new Templates();
                return _Templates;
            }
            set
            {
                _Templates = value;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page">页面Page对象,用于获取Request对象用</param>
        public TemplateAdmin(Page page)
        {
            SetConfigFilePath(page);
            ReadConfigFile();
        }

        public TemplateAdmin()
        {
            this.ConfigFile = HttpContext.Current.Server.MapPath(@"/Config/Template.config");
            ReadConfigFile();
        }
        private void SetConfigFilePath(Page page)
        {
            string strPath = page.MapPath("");
            if (strPath.EndsWith("Forms"))
            {
                this.ConfigFile = strPath + @"\Config\Template.config";
            }
            else if (strPath.EndsWith("Management"))
            {

                this.ConfigFile = strPath.Substring(0, strPath.LastIndexOfAny(@"\Management".ToCharArray()) - @"\Management".Length + 1) + @"\Templates\Template.config";
            }
            else
            {
                // this.ConfigFile = page.MapPath("") + @"\Template.config";
                this.ConfigFile = HttpContext.Current.Server.MapPath(@"/Config/Template.config");
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page">页面Page对象,用于获取Request对象用</param>
        /// <param name="ResetConfig">是否Reset 配置文件</param>
        public TemplateAdmin(Page page, bool ResetConfig)
        {
            SetConfigFilePath(page);
            if (ResetConfig)
            {
                Templates = new Templates();
                SaveConfigFile();
            }
            else
            {
                ReadConfigFile();
            }

        }

        /// <summary>
        /// 读取模板Config文件
        /// </summary>
        public void ReadConfigFile()
        {
            object obj = null;
            TextReader reader = null;
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(Templates));

                reader = new StreamReader(ConfigFile, System.Text.Encoding.Unicode);
                obj = x.Deserialize(reader);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception("Templates config file not found", ex);
            }
            catch (System.InvalidOperationException ex)
            {
                throw new Exception("Invalid Operation", ex.InnerException);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            Templates = (Templates)obj;
        }

        /// <summary>
        /// 保存模板Config文件
        /// </summary>
        public void SaveConfigFile()
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(Templates));
                writer = new StreamWriter(ConfigFile, false, System.Text.Encoding.Unicode);
                x.Serialize(writer, Templates);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception("Templates config file not found", ex);
            }
            catch (System.InvalidOperationException ex)
            {
                throw new Exception("Invalid Operation", ex.InnerException);
            }
            finally { writer.Close(); }
        }
    }

    [Serializable]
    [XmlInclude(typeof(Template))]
    [XmlInclude(typeof(TemplateVersion))]
    [XmlInclude(typeof(TemplateView))]
    public class Templates
    {
        private Collection<Template> _TemplateList;

        /// <summary>
        /// 模板对象存储列表
        /// </summary>
        public Collection<Template> TemplateList
        {
            get
            {
                if (_TemplateList == null)
                    _TemplateList = new Collection<Template>();
                return _TemplateList;
            }
        }

        /// <summary>
        /// 添加一个新的模板对象
        /// </summary>
        /// <param name="template"></param>
        public void AddTemplate(Template template)
        {
            if (this.TemplateList.Contains(template))
            {
                throw new Exception("该模板已存在");
            }
            template.ID = Guid.NewGuid().ToString();
            TemplateList.Add(template);
        }

        /// <summary>
        /// 根据模板ID移除模板
        /// </summary>
        /// <param name="templateID"></param>
        public void RemoveTemplate(Guid templateID)
        {
            foreach (Template template in this.TemplateList)
            {
                if (template.ID.Equals(templateID))
                {
                    this.TemplateList.Remove(template);
                }
            }
        }

        /// <summary>
        ///  获取模板中各版本列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTemplateList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            foreach (Template template in this.TemplateList)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = template.ID;
                dr["Name"] = template.Name;
                dr["Remark"] = template.Remark;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 根据模板ID获取模板对象
        /// </summary>
        /// <param name="templateID">模板ID</param>
        /// <returns></returns>
        public Template GetTemplate(Guid templateID)
        {
            foreach (Template template in this.TemplateList)
            {
                if (template.ID.Equals(templateID.ToString()))
                    return template;
            }
            throw new Exception("找不到对应的模板ID");
        }

        /// <summary>
        /// 根据模板名称获取模板对象
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns></returns>
        public Template GetTemplate(string templateName)
        {
            foreach (Template template in this.TemplateList)
            {
                if (template.Name.Equals(templateName))
                    return template;
            }
            throw new Exception("找不到对应的模板名称");
        }
    }

    [Serializable]
    public class Template
    {
        private string _ID;
        private string _Name;
        private Collection<TemplateVersion> _VersionList;
        private string _Remark;

        #region 公开属性
        /// <summary>
        /// 模板编号
        /// Get Only
        /// </summary>
        public string ID
        {
            get
            {
                if (_ID == "")
                    _ID = new Guid().ToString();
                return _ID;
            }
            set { _ID = value; }
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 模板版本列表
        /// Get Only
        /// </summary>
        public Collection<TemplateVersion> VersionList
        {
            get
            {
                if (_VersionList == null)
                    _VersionList = new Collection<TemplateVersion>();
                return _VersionList;
            }
        }

        /// <summary>
        /// 模板备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        /// <summary>
        /// 当前模板最大的版本号
        /// </summary>
        public int MaxVersion
        {
            get
            {
                int maxVersion = 0;
                foreach (TemplateVersion Tversion in this.VersionList)
                {
                    if (maxVersion < Tversion.ID)
                        maxVersion = Tversion.ID;
                }
                return maxVersion;
            }
        }
        #endregion

        #region 构造函数
        public Template()
        { }
        #endregion

        #region 公共方法
        /// <summary>
        /// 添加一个View至当前模板版本
        /// </summary>
        /// <param name="view"></param>
        public void AddVersion(TemplateVersion version)
        {
            version.ID = MaxVersion + 1;
            this.VersionList.Add(version);
        }

        /// <summary>
        /// 从当前模板版本中移除ID对应的View
        /// </summary>
        /// <param name="ViewID"></param>
        public void RemoveVersion(int versionId)
        {
            foreach (TemplateVersion templateVersion in this.VersionList)
            {
                if (templateVersion.ID.Equals(versionId))
                {
                    this.VersionList.Remove(templateVersion);
                }
            }
        }

        /// <summary>
        ///  获取模板中各版本列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetVersionList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            foreach (TemplateVersion TVersion in this.VersionList)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = TVersion.ID;
                dr["Remark"] = TVersion.Remark;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 根据版本ID获取模板对应版本对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TemplateVersion GetVersion(int id)
        {
            if (VersionList.Count == 0)
            {
                throw new Exception("当前模板没有添加版本，请添加!");
            }
            foreach (TemplateVersion TVersion in this.VersionList)
            {
                if (TVersion.ID == id)
                    return TVersion;
            }
            throw new Exception("找不到对应模板的模板版本!");
        }

        /// <summary>
        /// 获取最新模板版本的集合
        /// </summary>
        /// <returns>最新模板版本的集合DataTable</returns>
        public DataTable GetLastedTemplates()
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("Guid", typeof(Guid)));
            //dt.Columns.Add(new DataColumn("Name", typeof(string)));
            //dt.Columns.Add(new DataColumn("Version", typeof(string)));
            //dt.Columns.Add(new DataColumn("UiPath", typeof(string)));
            //dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            //foreach (Template template in this.TemplatesList)
            //{
            //        DataRow dr = dt.NewRow();
            //        dr["Guid"] = template.TemplateId;
            //        dr["Name"] = template.TemplateName;
            //        dr["Version"] = template.LastedTemplateContents.TemplateVersion;
            //        dr["UiPath"] = template.LastedTemplateContents.TemplateConent.UiFileName;
            //        dr["Remark"] = template.LastedTemplateContents.TemplateConent.Remark;
            //        dt.Rows.Add(dr);
            //}
            return dt;
        }
        #endregion

        #region Override Method
        public override bool Equals(object obj)
        {
            Template template = obj as Template;
            if (template == null)
                throw new Exception("Object is not a template");
            return (this.Name == template.Name || this.ID == template.ID);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    [Serializable]
    public class TemplateVersion
    {
        private int _ID;
        private Collection<TemplateView> _ViewList;
        private string _Remark;

        #region 公开属性
        /// <summary>
        /// 版本编号
        /// </summary>
        public int ID
        {
            get
            {
                if (_ID == 0)
                    _ID = 1;
                return _ID;
            }
            set { _ID = value; }
        }

        /// <summary>
        /// 当前版本模板视图集合
        /// </summary>
        public Collection<TemplateView> ViewList
        {
            get
            {
                if (_ViewList == null)
                    _ViewList = new Collection<TemplateView>();
                return _ViewList;
            }
        }

        /// <summary>
        /// 版本备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        /// <summary>
        /// 当前模板版本最大的View号
        /// </summary>
        public int MaxViewID
        {
            get
            {
                int maxViewID = 0;
                foreach (TemplateView TView in this.ViewList)
                {
                    if (maxViewID < TView.ID)
                        maxViewID = TView.ID;
                }
                return maxViewID;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 添加一个View至当前模板版本
        /// </summary>
        /// <param name="view"></param>
        public void AddView(TemplateView view)
        {
            if (this.ViewList.Contains(view))
            {
                throw new Exception("该视图已存在");
            }
            view.ID = MaxViewID + 1;
            this.ViewList.Add(view);
        }

        /// <summary>
        /// 从当前模板版本中移除ID对应的View
        /// </summary>
        /// <param name="ViewID"></param>
        public void RemoveView(int ViewID)
        {
            foreach (TemplateView view in this.ViewList)
            {
                if (view.ID.Equals(ViewID))
                {
                    this.ViewList.Remove(view);
                }
            }
        }

        /// <summary>
        ///  获取模板版本中各视图列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetViewList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Path", typeof(string)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            foreach (TemplateView TView in this.ViewList)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = TView.ID;
                dr["Name"] = TView.Name;
                dr["Path"] = TView.Path;
                dr["Remark"] = TView.Remark;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public TemplateView GetView(string viewNameorID)
        {
            if (viewNameorID.Equals("函件收文发起"))//老版函件收文为“函件收文发起”
            {
                viewNameorID = ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL;
            }
            if (ViewList.Count == 0)
            {
                throw new Exception("当前版本模板还没有添加过视图,请添加后重试!");
            }
            int viewid = int.MinValue;
            if (int.TryParse(viewNameorID, out viewid))
            {
                foreach (TemplateView TView in this.ViewList)
                {
                    if (TView.ID == viewid)
                        return TView;
                }
            }
            else
            {
                foreach (TemplateView TView in this.ViewList)
                {
                    if (TView.Name.Equals(viewNameorID))
                        return TView;
                }
            }
            throw new Exception("找不到对应的表单！");
        }
        #endregion

        #region Override Method
        public override bool Equals(object obj)
        {
            TemplateVersion templateVersion = obj as TemplateVersion;
            if (templateVersion == null)
                throw new Exception("Object is not a TemplateVersion");
            return this.ID == templateVersion.ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// 模板视图
    /// </summary>
    [Serializable]
    public class TemplateView
    {
        private int _ID;
        private string _Path;
        private string _Remark;
        private string _Name;

        /// <summary>
        /// 视图编号
        /// Get Only
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 视图路径
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        /// <summary>
        /// 视图备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        public TemplateView()
        { }
        public TemplateView(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
        }

        #region Override Method
        public override bool Equals(object obj)
        {
            TemplateView TView = obj as TemplateView;
            if (TView == null)
                throw new Exception("Object is not a TemplateView");
            return (this.ID == TView.ID || this.Name == TView.Name);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    #region 控件数据智能绑定
    /// <summary>
    /// ControlDataBind : 提供页面控件数据源的智能绑定
    /// Author by: Flyear 2008-07-14
    /// </summary>
    public static class ControlDataBind
    {
        /// <summary>
        /// 将DataTable的数据绑定到DropDownList,
        /// 当DataTable.Rows.Count != 1时,第一条记录SelectValue = 0
        /// </summary>
        /// <param name="ddl">DropDownList 名称</param>
        /// <param name="dt">DataTable 名称</param>
        public static bool InitDDL(DropDownList ddl, DataTable dt, string DataTextField, string DataValueField, bool ShowSelectText)
        {
            if (dt.Equals(null) || dt.Rows.Count < 1)
            {
                ddl.Items.Clear();
                ListItem li = new ListItem("NO DATA", "0");
                ddl.Items.Insert(0, li);
                ddl.Enabled = false;
            }
            else if (dt.Rows.Count == 1)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = DataTextField;
                ddl.DataValueField = DataValueField;
                ddl.DataBind();
                ddl.Enabled = true;
            }
            else if (dt.Rows.Count > 1)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = DataTextField;
                ddl.DataValueField = DataValueField;
                ddl.DataBind();

                if (ShowSelectText)
                {
                    ListItem li = new ListItem("Please Select ...", "0");
                    ddl.Items.Insert(0, li);
                }
                ddl.Enabled = true;
            }
            //if (ddl.SelectedIndexChanged != null && !ddl.Items[0].Equals(new ListItem("Please Select ...", "0")))
            //{
            //        ListItem li = new ListItem("Please Select ...", "0");
            //        ddl.Items.Insert(0, li);
            //}
            return ddl.Enabled;
        }
        public static bool InitDDL(DropDownList ddl, DataTable dt, bool SelectText)
        {
            return InitDDL(ddl, dt, "DATATEXT", "DATAVALUE", SelectText);
        }
        public static bool InitDDL(DropDownList ddl, DataTable dt)
        {
            return InitDDL(ddl, dt, "DATATEXT", "DATAVALUE", true);
        }
        /// <summary>
        /// 将DataTable的数据绑定到DropDownList,
        /// 当DataTable.Rows.Count != 1时,第一条记录SelectValue = 0
        /// </summary>
        /// <param name="ddl">DropDownList 名称</param>
        /// <param name="dt">DataSet 名称</param>
        public static bool InitDDL(DropDownList ddl, DataSet ds, bool SelectText)
        {
            return InitDDL(ddl, ds.Tables[0], SelectText);
        }
        public static bool InitDDL(DropDownList ddl, DataSet ds, string DataTextField, string DataValueField, bool SelectText)
        {
            return InitDDL(ddl, ds.Tables[0], DataTextField, DataValueField, SelectText);
        }
        public static bool InitDDL(DropDownList ddl, DataSet ds)
        {
            return InitDDL(ddl, ds.Tables[0], true);
        }
        public static bool InitDDL(DropDownList ddl, DataSet ds, string DataTextField, string DataValueField)
        {
            return InitDDL(ddl, ds.Tables[0], DataTextField, DataValueField, true);
        }

    }
    #endregion
}
