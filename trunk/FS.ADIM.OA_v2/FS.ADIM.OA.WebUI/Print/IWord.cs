using System.IO;
using System.Xml;
using System.Web;
using System.Collections;

namespace WordMgr
{
    /// <summary>
    /// WORD文档字体格式类，用于控制字体、字号等视觉效果
    /// </summary>
    public class DocFormat
    {
        string _FontName;
        public string FontName
        {
            get { return _FontName; }
            set { _FontName = value; }
        }

        float _FontSize;
        public float FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }

        int _Bold;
        public int Bold
        {
            get { return _Bold; }
            set { _Bold = value; }
        }

        int _Italic;
        public int Italic
        {
            get { return _Italic; }
            set { _Italic = value; }
        }

        public DocFormat()
        {
            _Bold = 0;
            _Italic = 0;
            _FontName = "仿宋";
            _FontSize = 16;
        }

        public DocFormat(string FontName, int FontSize, int Bold, int Italic)
        {
            _Bold = Bold;
            _Italic = Italic;
            _FontName = FontName;
            _FontSize = FontSize;
        }
    }

    /// <summary>
    /// 写入WORD文档模式
    /// </summary>
    public enum WriteMode
    { 
        Up,         
        Right,      
        Inner,     
        Down,      
        Shift,      
        Row,        
        Down_Append,
        File,       
        Attach      
    };

    public enum WordType
    { 
        Words,
        Sentences,
        Paragrap
    }

    class SubItem
    {
        string _ColName = "";

        public string ColName
        {
            get { return _ColName; }
            set { _ColName = value; }
        }
        string _ExtraPara = "";

        public string ExtraPara
        {
            get { return _ExtraPara; }
            set { _ExtraPara = value; }
        }
    }

    public class CRect
    {
        public int Left = 1;
        public int Top = 1;
        public int Width = 100;
        public int Height = 50;
    }

    /// <summary>
    /// 保存打印模板配置文件信息类
    /// </summary>
    public class ConfigInfo
    {
        /// <summary>
        /// OA流程类型名称
        /// </summary>
        private string _ProcessName;
        public string ProcessName
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }

        /// <summary>
        /// OA流程打印模板文件名称
        /// </summary>
        private string _TemplateName;
        public string TemplateName
        {
            get { return _TemplateName; }
            set { _TemplateName = value; }
        }

        public ArrayList _alTemplateInfo = new ArrayList();
    }

    public interface IWord
    {
        /// <summary>
        /// 模版文件所在路径
        /// </summary>
        string Template { get;set; }

        /// <summary>
        /// 导出待打印文件路径
        /// </summary>
        string SavePath { get;set; }

        /// <summary>
        /// 模版文件配置信息（暂时保留）
        /// </summary>
        ConfigInfo Config { get; }

        /// <summary>
        /// 设置写入文本的字体格式(包括，字体名称，字体大小，斜体，粗体)
        /// </summary>
        DocFormat Format { get; set; }

        /// <summary>
        /// 当前文档中页数
        /// </summary>
        string Pages { get; }

        /// <summary>
        /// 打开Template属性所指定的模版文件
        /// </summary>
        /// <returns></returns>
        bool Open();

        void WriteByFont(int iChar, int lblmark, string FontName);

        void WriteEx(string key, string value, WriteMode mode);

        /// <summary>
        /// 写入key关键字所在mode描述的相对位置的value文本信息
        /// </summary>
        /// <param name="key">模版文件中指定的字段值</param>
        /// <param name="value">待写入文本信息</param>
        /// <param name="mode">相对于key所在的相对位置</param>
        void Write(string key, string value, WriteMode mode);

        /// <summary>
        /// 写入key关键字所在mode描述的相对位置的value文本信息
        /// </summary>
        /// <param name="key">模版文件中指定的字段值</param>
        /// <param name="value">待写入文本信息</param>
        /// <param name="mode">相对于key所在的相对位置</param>
        /// <param name="offset">相对于key所在的相对位置的偏移量</param>
        void Write(string key, string value, WriteMode mode, int offset);

        /// <summary>
        /// 在最后插入sFile文件的内容到最后
        /// </summary>
        /// <param name="sFile"></param>
        void WriteFile(string sFile);

        /// <summary>
        /// 在指定位置插入sFile文件的内容到最后
        /// </summary>
        /// <param name="Key">模版文件中指定的字段值</param>
        /// <param name="sFile">待写入文档所在路径</param>
        /// <param name="mode">相对于key所在的相对位置</param>
        /// <param name="offset">相对于key所在的相对位置的偏移量</param>
        void WriteFile(string Key, string sFile, WriteMode mode, int offset);

        /// <summary>
        /// 将数据写入到模板指定表格索引
        /// </summary>
        /// <param name="TblIndex">WORD文档中表格索引,下标从'零'开始</param>
        /// <param name="al">待写入到表格中的数据</param>
        void WriteTable(int TblIndex, ArrayList al);
        void WriteTable(int TblIndex, int SubTblIdx, ArrayList al);

        void WriteHeaderFooter(string key, string[] value, WriteMode mode);

        void AddPicture(CRect rect, string[] sParams, string sTimeStamp, 
                        double fLenRatio, double fWidRatio, string sDisplay,
                        string sUserID);
        /// <summary>
        /// 填充空行，保证导出文档满页
        /// </summary>
        /// <param name="LayoutZone">待写入空行关键字</param>
        /// <param name="mode">相对于key所在的相对位置</param>
        void DocLayout(string LayoutZone, WriteMode mode);
        void DocLayout(int tableIndex, int iRow, int iCol);

        /// <summary>
        /// 转换导出文件为PDF格式，并保存于SavePath描述的路径
        /// </summary>
        /// <param name="SavePath">保存FDF文件路径及名称</param>
        void Convert2pdf(string SavePath);

        /// <summary>
        /// 保存在当前导出文件到SavePath描述的路径及名称
        /// </summary>
        void Save();
        
        /// <summary>
        /// 关闭WORD导出类，释放COM组建资源
        /// </summary>
        void Close();

        void DeleteTable(int TblIdx);

        void DeleteString(string sKey);
    }

    /// <summary>
    /// IWord接口基类
    /// </summary>
    public abstract class WordBase : IWord
    {
        private string m_sTemplate;
        public string Template
        {
            get { return m_sTemplate; }
            set { 
                m_sTemplate = value;
                m_ConfigInfo = GetConfigInfo();
            }
        }
        private string m_SavePath;
        public string SavePath
        {
            get { return m_SavePath; }
            set { m_SavePath = value; }
        }

        public abstract DocFormat Format { get; set; }

        private ConfigInfo m_ConfigInfo;
        public ConfigInfo Config
        {
            get {
                return m_ConfigInfo;
            }
        }

        protected bool Available(string sPath)
        {
            return File.Exists(sPath);
        }

        /// <summary>
        /// 获取打印末班配置文件信息
        /// </summary>
        /// <returns>打印模板文件信息</returns>
        protected ConfigInfo GetConfigInfo()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(HttpContext.Current.Server.MapPath((@"~\template\template.xml")));

            ConfigInfo cfginfo = new ConfigInfo();

            if (xmldoc.DocumentElement == null) return cfginfo;
            XmlNode root = xmldoc.DocumentElement;

            string stmp = this.Template;
            string[] arr = stmp.Split(new char[] { '\\' });

            XmlNodeList Nodes = root.SelectNodes("template [ @name = '" + arr[arr.Length - 1] + "']");

            if( Nodes.Count == 1)
            {
                for(int i=0;i<Nodes[0].ChildNodes.Count;i++)
                {
                    SubItem item = new SubItem();
                    string[] sValue = Nodes[0].ChildNodes[i].InnerText.Split(new char[]{'|'});
                    if (sValue.Length != 0)
                    {
                        item.ColName = sValue[0];
                        item.ExtraPara = sValue[1];
                        cfginfo._alTemplateInfo.Add(item);
                    }
                }
            }

            return cfginfo;
        }

        /// <summary>
        /// 处理含WORD段落控制符的字符串
        /// </summary>
        /// <param name="srcStr">待处理字符串</param>
        /// <returns>去掉段落符</returns>
        protected string HandleString(string srcStr)
        {
            string sTmp = srcStr;
            if (sTmp == "\r\a")
            {
                return srcStr;
            }
            if (sTmp.Contains("\r\a"))
            {
                sTmp = sTmp.Substring(0, sTmp.Length - 2);
            }
            if (sTmp.Contains("\r"))
            {
                sTmp = sTmp.Substring(0, sTmp.Length - 1);
            }
            if (sTmp.Contains("\n"))
            {
                sTmp = sTmp.Substring(0, sTmp.Length - 1);
            }
            return sTmp;
        }

        public abstract string Pages { get; }

        public abstract bool Open();

        public abstract void WriteByFont(int iChar, int lblmark, string FontName);

        public abstract void WriteEx(string key, string value, WriteMode mode);

        public abstract void Write(string key, string value, WriteMode mode);

        public abstract void Write(string key, string value, WriteMode mode, int offset);

        public abstract void WriteFile(string sFile);

        public abstract void WriteFile(string Key, string sFile, WriteMode mode, int offset);

        public abstract void WriteTable(int TblIndex, ArrayList al);

        public abstract void WriteTable(int TblIndex, int SubTblIdx, ArrayList al);

        public abstract void AddPicture(CRect rect, string[] sParams, string sTimeStamp, 
                                        double fLenRatio, double fWidRatio, string sDisplay,
                                        string sUserID);

        public abstract void WriteHeaderFooter(string key, string[] value, WriteMode mode);

        public abstract void DocLayout(string LayoutZone, WriteMode mode);

        public abstract void DocLayout(int tableIndex, int iRow, int iCol);

        public abstract void Convert2pdf(string SavePath);

        public abstract void Save();
        
        public abstract void Close();

        public abstract void DeleteTable(int TblIdx);

        public abstract void DeleteString(string sKey);
    }
}
