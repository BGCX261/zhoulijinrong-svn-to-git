//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：Word输出类
// 
// 创建标识：2010-02-21 王晓
//
// 修改标识：2010-05-06 胥寿春
// 修改描述：1.扩展InsertPicture函数，统一处理日期格式
//           2.扩展AddTextOutsideImg函数，调整日期的字体
//
// 修改标识
// 修改描述：
//
//----------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Web;
using FS.OA.Framework;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using FS.ADIM.OU.OutBLL;

namespace WordMgr
{
    public class KeyPosition
    {
        private string _sCnKey;   // KEY值

        public string CnKey
        {
            get { return _sCnKey; }
            set { _sCnKey = value; }
        }

        private string _sEnKey; // E文KEY值

        public string EnKey
        {
            get { return _sEnKey; }
            set { _sEnKey = value; }
        }

        private int _iTblIndex;

        public int TblIndex
        {
            get { return _iTblIndex; }
            set { _iTblIndex = value; }
        }

        private int _iRowIndex; // KEY值所在表格的行索引

        public int RowIndex
        {
            get { return _iRowIndex; }
            set { _iRowIndex = value; }
        }
        private int _iColIndex; // KEY值所在表格的列索引

        public int ColIndex
        {
            get { return _iColIndex; }
            set { _iColIndex = value; }
        }
        private int _SenIndex;  // KEY值所在文档的语句索引

        public int SenIndex
        {
            get { return _SenIndex; }
            set { _SenIndex = value; }
        }

        public KeyPosition()
        {
            _sCnKey = "";
            _iRowIndex = 0;
            _iColIndex = 0;
            _SenIndex = 0;
        }

        public KeyPosition(string sKey, int iRow, int iCol, int iSen)
        {
            _iRowIndex = iRow;
            _iColIndex = iCol;
            _SenIndex = iSen;
        }
    }

    public class Word07 : WordBase
    {
        private const int _TRY_COUNT_ = 5;

        private const int _INVALID_WORDPROC_TIMEOUT_ = 6;

        private Word._Application m_WordApp = null;
        private Word._Document m_doc = null;
        private object oMissing = System.Reflection.Missing.Value;

        Array m_arKeyPosition;
        //ArrayList m_arKeyPosition = new ArrayList();
        DocFormat m_DocFormat;
        public override DocFormat Format { 
            get{
                return m_DocFormat;
            } 
            set{
                m_DocFormat = value;
            }
        }

        public Word07()
        {
            KillWordProcess(new TimeSpan(0, _INVALID_WORDPROC_TIMEOUT_, 0));
            m_WordApp = new Application();
            m_DocFormat = new DocFormat();
        }

        /// <summary>
        /// 检查待打印WORD模板文件扩展名是否是DOCX
        /// </summary>
        /// <returns>若是返回TRUE，否则返回FALSE</returns>
        private bool ChkExtOfDocx()
        {
            string sTmp = this.Template;
            string sRet = sTmp.Substring(sTmp.Length - 4, 4);
            return (sRet.ToLower() == "docx");
        }

        /// <summary>
        /// 尝试拷贝待打印模板文件到制定输出目录
        /// </summary>
        /// <param name="Source">待拷贝源路径</param>
        /// <param name="Dest">待拷贝目标路径</param>
        /// <returns>拷贝成功则返回TRUE，否则返回FALSE</returns>
        private bool TryCopy(string Source, string Dest)
        {
            try
            {
                File.Copy(Source, Dest, true);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 打开待打印WORD模板文件，以开始打印处理
        /// </summary>
        /// <returns>打开成功则返回TRUE，否则返回FALSE</returns>
        public override bool Open()
        {
            object template = this.Template;
            object ReadOnly = false;
            object revert = true;
            object SavePath = this.SavePath;
            
            if (this.Available(this.Template))
            {
                if (!ChkExtOfDocx()) return false;
                int TryCount = 0;
                while (true)
                {
                    if (TryCopy(this.Template, this.SavePath))
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        TryCount++;
                        if (TryCount > _TRY_COUNT_)
                        {
                            return false;
                        }
                    }
                }
                //Monitor.Enter(template);
                //try {
                //    File.Copy(this.Template, this.SavePath, true);
                //}
                //catch {
                //    Thread.Sleep(200);
                //    try
                //    {
                //        File.Copy(this.Template, this.SavePath, true);
                //    }
                //    catch { return false; }
                //}
                //Monitor.Exit(template);
            
                if (!File.Exists(this.SavePath)) return false;
                m_doc = m_WordApp.Documents.Open(ref SavePath, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                                ref oMissing, ref revert, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                m_doc.Activate();
                //m_doc.ActiveWindow.Document.Activate();

                if (m_doc.Revisions.Count > 0)
                {
                    m_doc.ShowRevisions = true;
                    m_doc.TrackRevisions = true;
                    m_doc.Revisions.AcceptAll();
                }

                m_arKeyPosition = new ArrayList[m_doc.Tables.Count];
                for (int iCount = 0; iCount < m_doc.Tables.Count; iCount++)
                {
                    ArrayList alkp = TraverTable(iCount + 1);
                    m_arKeyPosition.SetValue(alkp, iCount);
                }
                return true;
            }
            return false;
        } 
    
        /// <summary>
        /// 将待写入WORD模板内的字符串设置为制定字体格式
        /// </summary>
        /// <param name="iChar"></param>
        /// <param name="lblmark"></param>
        /// <param name="FontName"></param>
        public override void WriteByFont(int iChar, int lblmark, string FontName)
        {
            object oMark = lblmark;
            object fontname = FontName;//"Wingdings 2";   
            object uic = true;

            m_doc.Bookmarks.get_Item(ref oMark).Range.InsertSymbol(iChar/*-4014*/, ref fontname, ref uic, ref oMissing); 
        }

        /// <summary>
        /// 将VALUE值按照mode模式写入到key相关的位置处
        /// </summary>
        /// <param name="key">打印模板参考位置</param>
        /// <param name="value">待输出到WORD文档中的字符串</param>
        /// <param name="mode">写入时的辅助参考值</param>
        public override void WriteEx(string key, string value, WriteMode mode)
        {
            for (int i = 1; i <= m_doc.Paragraphs.Count; i++)
            {
                if (this.HandleString(m_doc.Paragraphs[i].Range.Text) == key)
                {
                    switch (mode)
                    {
                        case WriteMode.Up:
                            m_doc.Paragraphs[i - 2].Range.InsertBefore(value);
                            //SetTextFormat(m_doc.Paragraphs[i - 1].Range);
                            break;
                        case WriteMode.Down:
                            m_doc.Paragraphs[i + 2].Range.InsertBefore(value);
                            //SetTextFormat(m_doc.Paragraphs[i + 2].Range);
                            break;
                        case WriteMode.Inner:
                            m_doc.Paragraphs[i].Range.InsertBefore(value);
                            m_doc.Paragraphs[i].Range.Words[6].Font.Color = WdColor.wdColorWhite;
                            m_doc.Paragraphs[i + 1].Range.Delete(ref oMissing, ref oMissing);
                            //int len = m_doc.Paragraphs[i].Range.Words.Count;
                            //m_doc.Paragraphs[i].Range.Words[len - 1].Text = " ";
                            //m_doc.Paragraphs[i].Range.Words[len - 2].Text = " ";
                            //SetTextFormat(m_doc.Paragraphs[i].Range);
                            break;
                        case WriteMode.Right:
                            m_doc.Paragraphs[i + 1].Range.InsertBefore(value);
                            //SetTextFormat(m_doc.Paragraphs[i + 1].Range);
                            break;
                        case WriteMode.Shift:
                            //m_doc.Paragraphs[i].Range.Text = value;
                            m_doc.Paragraphs[i].Range.InsertBefore(value);
                            int length = m_doc.Paragraphs[i].Range.Words.Count;
                            m_doc.Paragraphs[i].Range.Words[length - 1].Text = " ";
                            m_doc.Paragraphs[i].Range.Words[length - 2].Text = " ";
                            //SetTextFormat(m_doc.Paragraphs[i].Range);
                            break;
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 将VALUE值按照mode模式写入到key相关的位置处
        /// </summary>
        /// <param name="key">打印模板参考位置</param>
        /// <param name="value">待输出到WORD文档中的字符串</param>
        /// <param name="mode">写入时的辅助参考值</param>
        public override void Write(string key, string value, WriteMode mode)
        {
            KeyPosition kp = new KeyPosition();
            for (int idx = 0; idx < m_arKeyPosition.Length; idx++)
            {
                if ((ArrayList)m_arKeyPosition.GetValue(idx) != null)
                {
                    kp = GetPositionByKey(key, (ArrayList)m_arKeyPosition.GetValue(idx));
                    if (kp.RowIndex != 0)
                    {
                        switch (mode)
                        {
                            case WriteMode.Up:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex - 1].Cells[kp.ColIndex].Range.InsertBefore(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex - 1].Cells[kp.ColIndex].Range);
                                break;
                            case WriteMode.Down:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range.InsertBefore(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range);
                                break;
                            case WriteMode.Inner:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.InsertAfter(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range);
                                //m_doc.Tables[1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.Text += value;
                                break;
                            case WriteMode.Right:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex + 1].Range.InsertBefore(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex + 1].Range);
                                //m_doc.Tables[1].Rows[kp.RowIndex].Cells[kp.ColIndex + 1].Range.Text += value;
                                break;
                            case WriteMode.Shift:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.Text = value;
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range);
                                break;
                            //case WriteMode.Row:
                            //    object row = (object)m_doc.Tables[idx].Rows.Last;
                            //    m_doc.Tables[idx].Rows.Add(ref row);
                            //    break;
                            case WriteMode.Down_Append:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range.InsertAfter(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range);
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    for (int i = 1; i <= m_doc.Paragraphs.Count; i++)
                    {
                        if (this.HandleString(m_doc.Paragraphs[i].Range.Text) == key)
                        {
                            switch (mode)
                            { 
                                case WriteMode.Up:
                                    m_doc.Paragraphs[i - 2].Range.InsertBefore(value);
                                    //SetTextFormat(m_doc.Paragraphs[i - 1].Range);
                                    break;
                                case WriteMode.Down:
                                    m_doc.Paragraphs[i + 2].Range.InsertBefore(value);
                                    //SetTextFormat(m_doc.Paragraphs[i + 2].Range);
                                    break;
                                case WriteMode.Inner:
                                    m_doc.Paragraphs[i].Range.InsertAfter(value);
                                    //SetTextFormat(m_doc.Paragraphs[i].Range);
                                    break;
                                case WriteMode.Right:
                                    m_doc.Paragraphs[i + 1].Range.InsertBefore(value);
                                    //SetTextFormat(m_doc.Paragraphs[i + 1].Range);
                                    break;
                                case WriteMode.Shift:
                                    m_doc.Paragraphs[i].Range.Text = value;
                                    //SetTextFormat(m_doc.Paragraphs[i].Range);
                                    break;
                            }
                            return;
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 将VALUE值按照mode模式写入到key相关的位置处
        /// </summary>
        /// <param name="key">打印模板参考位置</param>
        /// <param name="value">待输出到WORD文档中的字符串</param>
        /// <param name="mode">写入时的辅助参考值</param>
        /// <param name="offset">相对mode的偏移值</param>
        public override void Write(string key, string value, WriteMode mode, int offset)
        {
            KeyPosition kp = new KeyPosition();
            for (int idx = 0; idx < m_arKeyPosition.Length; idx++)
            {
                if ((ArrayList)m_arKeyPosition.GetValue(idx) != null)
                {
                    kp = GetPositionByKey(key, (ArrayList)m_arKeyPosition.GetValue(idx));
                    if (kp.RowIndex != 0)
                    {
                        switch (mode)
                        {
                            case WriteMode.Up:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex - offset].Cells[kp.ColIndex].Range.InsertBefore(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex - offset].Cells[kp.ColIndex].Range);
                                break;
                            case WriteMode.Down:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex + offset].Cells[kp.ColIndex].Range.InsertBefore(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex + offset].Cells[kp.ColIndex].Range);
                                break;
                            case WriteMode.Inner:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.InsertAfter(value);
                                //m_doc.Tables[1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.Text += value;
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range);
                                break;
                            case WriteMode.Right:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex + offset].Range.InsertBefore(value);
                                //m_doc.Tables[1].Rows[kp.RowIndex].Cells[kp.ColIndex + 1].Range.Text += value;
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex + offset].Range);
                                break;
                            case WriteMode.Shift:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.Text = value;
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range);
                                break;

                            case WriteMode.Down_Append:
                                m_doc.Tables[idx + 1].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range.InsertAfter(value);
                                //SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range);
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    for (int i = 1; i <= m_doc.Paragraphs.Count; i++)
                    {
                        if (this.HandleString(m_doc.Paragraphs[i].Range.Text) == key)
                        {
                            switch (mode)
                            {
                                case WriteMode.Up:
                                    m_doc.Paragraphs[i - offset].Range.InsertBefore(value);
                                    //SetTextFormat(m_doc.Paragraphs[i - offset].Range);
                                    break;
                                case WriteMode.Down:
                                    m_doc.Paragraphs[i + offset].Range.InsertBefore(value);
                                    //SetTextFormat(m_doc.Paragraphs[i + offset].Range);
                                    break;
                                case WriteMode.Inner:
                                    m_doc.Paragraphs[i].Range.InsertAfter(value);
                                    //SetTextFormat(m_doc.Paragraphs[i].Range);
                                    break;
                                case WriteMode.Right:
                                    m_doc.Paragraphs[i + offset].Range.InsertBefore(value);
                                    //SetTextFormat(m_doc.Paragraphs[i + offset].Range);
                                    break;
                                case WriteMode.Shift:
                                    m_doc.Paragraphs[i].Range.Text = value;
                                    //SetTextFormat(m_doc.Paragraphs[i].Range);
                                    break;
                            }
                            return;
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 将sFile描述的WORD文件写入到WORD模板文件中
        /// </summary>
        /// <param name="sFile">待写入到ORD模板文件中的WORD文档</param>
        public override void WriteFile(string sFile)
        {
            try
            {
                if (File.Exists(sFile))
                {
                    //m_doc.Paragraphs.Last.Range.InsertAfter("\r\n");
                    object oDirection = WdCollapseDirection.wdCollapseEnd;
                    m_doc.Paragraphs.Last.Range.Collapse(ref oDirection);

                    object pBreak = WdBreakType.wdSectionBreakNextPage;
                    m_doc.Paragraphs.Last.Range.InsertBreak(ref pBreak);
                    object oLink = false;
                    object oConersion = true;

                    m_doc.Paragraphs.Last.Range.InsertFile(sFile, ref oMissing, ref oConersion, ref oLink, ref oMissing);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 将sFile描述的WORD文件写入到WORD模板文件中
        /// </summary>
        /// <param name="Key">打印模板参考位置</param>
        /// <param name="sFile">待写入到ORD模板文件中的WORD文档</param>
        /// <param name="mode">写入时的辅助参考值</param>
        /// <param name="offset">相对mode的偏移值</param>
        public override void WriteFile(string Key, string sFile, WriteMode mode, int offset)
        {
            try
            {
                if (File.Exists(sFile))
                {
                    KeyPosition kp = new KeyPosition();
                    for (int idx = 0; idx < m_arKeyPosition.Length; idx++)
                    {
                        if ((ArrayList)m_arKeyPosition.GetValue(idx) != null)
                        {
                            kp = GetPositionByKey(Key, (ArrayList)m_arKeyPosition.GetValue(idx));
                            if (kp.RowIndex != 0)
                            {
                                switch (mode)
                                {
                                    case WriteMode.Up:
                                        m_doc.Tables[idx + 1].Rows[kp.RowIndex - offset].Cells[kp.ColIndex].Range.Paragraphs.Last.Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                        SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex - offset].Cells[kp.ColIndex].Range);
                                        break;
                                    case WriteMode.Down:
                                        m_doc.Tables[idx + 1].Rows[kp.RowIndex + offset].Cells[kp.ColIndex].Range.Paragraphs.Last.Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                        SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex + offset].Cells[kp.ColIndex].Range);
                                        break;
                                    case WriteMode.Inner:
                                        m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.Paragraphs.Last.Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                        SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range);
                                        //m_doc.Tables[1].Rows[kp.RowIndex].Cells[kp.ColIndex].Range.Text += value;
                                        break;
                                    case WriteMode.Right:
                                        m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex + offset].Range.Paragraphs.Last.Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                        SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex].Cells[kp.ColIndex + offset].Range);
                                        //m_doc.Tables[1].Rows[kp.RowIndex].Cells[kp.ColIndex + 1].Range.Text += value;
                                        break;
                                    case WriteMode.Down_Append:
                                        m_doc.Tables[idx + 1].Rows[kp.RowIndex + offset].Cells[kp.ColIndex].Range.Paragraphs.Last.Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                        SetTextFormat(m_doc.Tables[idx + 1].Rows[kp.RowIndex + offset].Cells[kp.ColIndex].Range);
                                        break;
                                }
                                break;
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= m_doc.Paragraphs.Count; i++)
                            {
                                if (this.HandleString(m_doc.Paragraphs[i].Range.Text) == Key)
                                {
                                    Word._Application WordApp = new Application();
                                    Word._Document doc = null;
                                    object oFile = (object)sFile;
                                    doc = WordApp.Documents.Open(ref oFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                                    doc.Activate();
                                    object oFirst = doc.Paragraphs.First;
                                    object oLast = doc.Paragraphs.Last;
                                    SetTextFormat(doc.Range(ref oFirst, ref oLast));
                                    doc.Save();

                                    if (WordApp != null)
                                    {
                                        if (doc != null)
                                            doc.Close(ref oMissing, ref oMissing, ref oMissing);
                                        WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                                        doc = null;
                                        WordApp = null;
                                    }
                                    KillWordProcess(new TimeSpan(0, _INVALID_WORDPROC_TIMEOUT_, 0));

                                    switch (mode)
                                    {
                                        case WriteMode.Up:
                                            m_doc.Paragraphs[i - offset].Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                            //this.m_WordApp.Selection.Document.Paragraphs[i - offset].Range
                                            break;
                                        case WriteMode.Down:
                                            m_doc.Paragraphs[i + offset].Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                            break;
                                        case WriteMode.Inner:
                                            m_doc.Paragraphs[i].Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                            break;
                                        case WriteMode.Right:
                                            m_doc.Paragraphs[i + offset].Range.InsertFile(sFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                            break;
                                    }
                                    return;
                                }
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 插入图片到WORD打印模板中
        /// </summary>
        /// <param name="rect">待插入图片设置的区域大小</param>
        /// <param name="sParams">待写入参数</param>
        /// <param name="sTimeStamp">生成图片时写入的时间戳</param>
        /// <param name="fLenRatio">图片长比例</param>
        /// <param name="fWidRatio">图片宽比例</param>
        public override void AddPicture(CRect rect, string[] sParams, string sTimeStamp,
                                        double fLenRatio, double fWidRatio, string sDisplay,
                                        string sUserID)
        {
            KeyPosition kp = new KeyPosition();
            for (int idx = 0; idx < m_arKeyPosition.Length; idx++)
            {
                if ((ArrayList)m_arKeyPosition.GetValue(idx) != null)
                {
                    kp = GetPositionByKey(sParams[0], (ArrayList)m_arKeyPosition.GetValue(idx));
                    if (kp.RowIndex != 0)
                    {
                        switch (sParams[1].ToLower())
                        { 
                            case "down":
                                InsertPicture(m_doc.Tables[kp.TblIndex].Rows[kp.RowIndex + 1].Cells[kp.ColIndex].Range,
                                              rect, sTimeStamp, fLenRatio, fWidRatio, sDisplay, sUserID);
                                return;
                            case "right":
                                InsertPicture(m_doc.Tables[kp.TblIndex].Rows[kp.RowIndex].Cells[kp.ColIndex + 1].Range,
                                              rect, sTimeStamp, fLenRatio, fWidRatio, sDisplay, sUserID);
                                return;
                            case "downs":
                                string[] arrStamp = sTimeStamp.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
                                string[] arrsUserID = sUserID.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 1; i <= m_doc.Tables[kp.TblIndex].Rows.Count; i++)
                                {
                                    sTimeStamp = arrStamp[i-1]; sUserID = arrsUserID[i-1];
                                    InsertPicture(m_doc.Tables[kp.TblIndex].Rows[kp.RowIndex + i].Cells[kp.ColIndex].Range,
                                                  rect, sTimeStamp, fLenRatio, fWidRatio, sDisplay, sUserID);
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将生成的图片插入到range描述的WORD文档位置
        /// </summary>
        /// <param name="range">WORD文档位置</param>
        /// <param name="rect">待插入图片设置的区域大小</param>
        /// <param name="sTimeStamp">生成图片时写入的时间戳</param>
        /// <param name="fLenRatio">图片长比例</param>
        /// <param name="fWidRatio">图片宽比例</param>
        private void InsertPicture(Range range, CRect rect, string sTimeStamp, 
                                    double fLenRatio, double fWidRatio, string sDisplay,
                                    string sUserID)
        {
            
            int iCur = 0;
            string[] Stamps = null;
            if (sTimeStamp.Contains("\n"))
            {
                Stamps = sTimeStamp.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
            }
            object Link2File = false;
            object SaveWithDoc = true;

            object Left = null;
            object Top = null;
            object Width = null;
            object Height = null;

            Left = rect.Left;
            Top = rect.Top;
            // 长宽比例若无效则从配置中(Width Height)取得对应长宽值,否则更具长宽比例计算长宽值
            if ((fLenRatio >= -double.Epsilon && fLenRatio <= double.Epsilon) ||
                (fWidRatio >= -double.Epsilon && fWidRatio <= double.Epsilon))
            {
                
                Width = rect.Width;
                Height = rect.Height;
            }
            else
            {
                // 配置高度无效则根据长宽比例计算处高度
                if (Height == null || rect.Height == 0)
                {
                    Width = rect.Width;
                    Height =(fWidRatio / fLenRatio) * rect.Width;
                }
                // 配置宽度无效则根据高宽比例计算处长度
                if (Width == null || rect.Width == 0)
                {
                    Width = (fLenRatio / fWidRatio) * rect.Height;
                    Height = rect.Height; 
                }
            }

            for (int i = 1; i <= range.Paragraphs.Count; i++)
            {
                try
                {
                    string sName = HandleString(range.Paragraphs[i].Range.Text).Trim();
                    if (!string.IsNullOrEmpty(sUserID))
                    {
                        string sdbName = OAUser.GetUserName(sUserID);
                        if (sName != sdbName)
                        {
                            range.Paragraphs[i].Range.Font.Color = WdColor.wdColorWhite;
                            continue;
                        }
                        else
                        {
                            sName = sdbName;
                        }
                    }
                    string sFilePath = HttpContext.Current.Server.MapPath(@"~\template\Singer\" + sName + ".png");
                    if (!File.Exists(sFilePath))
                    {
                        if (Stamps == null)
                        {
                            sFilePath = GenSingerPic(sName, sFilePath, sTimeStamp, sDisplay);
                        }
                        else
                        {
                            if (Stamps.Length > iCur)
                                sFilePath = GenSingerPic(sName, sFilePath, Stamps[iCur], sDisplay);
                        }
                    }

                    if (File.Exists(sFilePath))
                    {
                        object objRange = range.Paragraphs[i].Range;

                        //int iCount = 0;
                        //while (range.Paragraphs[1].Range.Text != "\r\a")
                        //{
                        //    range.Paragraphs[i].Range.Font.Color = WdColor.wdColorWhite;
                        //    //if (iCount++ >= range.Paragraphs.Count) break;
                        //}
                        
                        m_doc.Shapes.AddPicture(sFilePath,
                                                ref Link2File,
                                                ref SaveWithDoc,
                                                ref Left, ref Top,
                                                ref Width, ref Height,
                                                ref objRange);
                        File.Delete(sFilePath);
                        iCur++;
                    }
                    range.Paragraphs[i].Range.Font.Color = WdColor.wdColorWhite;
                }
                catch {
                    range.Paragraphs[i].Range.Font.Color = WdColor.wdColorWhite;
                    continue;
                }
            }
        }

        /// <summary>
        /// 读取数据图片数据字段并保存到sPath所在目录
        /// </summary>
        /// <param name="sID">查询条件</param>
        /// <param name="sPath">图片文件保存路径</param>
        /// <returns>若成功则返回图片所在路劲,否则返回空</returns>
        private string GenSingerPic(string sID, string sPath, string sTimeStamp, string sDisplay)
        {
            try
            {
                string sTableName = "T_OU_User";

                //string sColImgData = "";
                string sSQL = "SELECT [Image] FROM " + sTableName + " WHERE [Name] = '" + sID + "'";


                System.Data.DataTable dtImgData = FounderSoftware.Framework.Business.Entity.RunQuery(sSQL, "Sql",
                                                OAConfig.GetConfig("数据库", "DataSource"),
                                                OAConfig.GetConfig("数据库", "DataBase"),
                                                OAConfig.GetConfig("数据库", "uid"),
                                                OAConfig.GetConfig("数据库", "pwd"));
                if (dtImgData.Rows.Count == 0) return "";
                
                //Image.FromStream(new MemoryStream((byte[])dtImgData.Rows[0]["Image"])).Save(sPath);
                string text = "";
                string timestamp = "";
                if (string.IsNullOrEmpty(sTimeStamp))
                {
                    // 取不到时间，则更具配置参数决定是输出当前日期，还是直接输出签名图片不显示时间戳
                    if (sDisplay == "none") // 不输出时间戳,直接生成签名图片
                    {
                        MemoryStream ms = new MemoryStream((byte[])dtImgData.Rows[0]["Image"]);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                        Bitmap newbitmap = new Bitmap(image.Width, image.Height);
                        Graphics g = Graphics.FromImage(newbitmap);
                        g.DrawImageUnscaled(image, 0, 0);
                        newbitmap.Save(sPath, ImageFormat.Png);
                        System.Drawing.Image.FromStream(ms).Save(sPath);
                        g.Dispose();
                        newbitmap.Dispose();
                        image.Dispose();

                        return sPath;
                    }
                    else // 使用当前日期时间生成时间戳
                    {
                        text = DateTime.Now.ToString();
                        timestamp = text.Replace(" ", "\r\n");
                    }
                }
                else
                {
                    // 取不到时间，则更具配置参数决定是输出当前日期，还是直接输出签名图片不显示时间戳
                    if (sDisplay == "none") // 不输出时间戳,直接生成签名图片
                    {                        MemoryStream ms = new MemoryStream((byte[])dtImgData.Rows[0]["Image"]);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                        Bitmap newbitmap = new Bitmap(image.Width, image.Height);
                        Graphics g = Graphics.FromImage(newbitmap);
                        g.DrawImageUnscaled(image, 0, 0);
                        newbitmap.Save(sPath, ImageFormat.Png);
                        System.Drawing.Image.FromStream(ms).Save(sPath);
                        g.Dispose();
                        newbitmap.Dispose();
                        image.Dispose();


                        return sPath;
                    }
                    text = sTimeStamp;
                    timestamp = text.Replace(" ", "\r\n");
                }
                //timestamp = timestamp.Replace('-', '.');

                DateTime dtTemp = DateTime.MinValue;
                if (DateTime.TryParse(timestamp, out dtTemp))
                {
                    timestamp = dtTemp.ToString("yyyy.MM.dd\r\nHH:mm:ss");
                }

                //string SingerPath = sPath.Substring(0, sPath.Length - 4);
                //SingerPath += "Signer.png";
                AddTextOutsideImg(new MemoryStream((byte[])dtImgData.Rows[0]["Image"]), timestamp, sPath);
                //AddTextInsideImg(sPath, timestamp, SingerPath);
                return sPath;
            }
            catch (Exception ex){
                return ex.Message;
            }
        }

        /// <summary>
        /// 生成图片时，待写入时间戳字体的字号大小（暂时保留，未使用）
        /// </summary>
        /// <param name="g"></param>
        /// <param name="GraphicsWidth"></param>
        /// <param name="GraphicsHeigh"></param>
        /// <param name="sText"></param>
        /// <returns></returns>
        private float GetFontSize(Graphics g,
                                  int GraphicsWidth,
                                  int GraphicsHeigh,
                                  string sText)
        {
            SizeF layoutArea = new SizeF((float)GraphicsWidth, (float)GraphicsHeigh);
            float fFontSize = 2.0f;
            
            //return g.MeasureString(sText, new System.Drawing.Font("宋体", fFontSize), layoutArea);

            while (true)
            {
                /*测试像素大小*/
                SizeF sizeF = g.MeasureString(sText, new System.Drawing.Font("宋体", fFontSize));
                //if ((GraphicsWidth < sizeF.Width) || (GraphicsHeigh < sizeF.Height))
                if (Math.Abs(GraphicsWidth - sizeF.Width) <= 10 && Math.Abs(GraphicsHeigh - sizeF.Height) <= 16)
                {
                    return fFontSize;
                }
                else
                {
                    fFontSize += 2.0f;
                    if (fFontSize >= 40.0f) break;
                }
            }
            return fFontSize;
        }

        /// <summary>
        /// 添加时间戳到图片右侧(时间戳背景为透明色)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        /// <param name="SavePath"></param>
        private void AddTextOutsideImg(Stream sm, string text, string SavePath)
        {
            if (sm == null) return;
            //if (!File.Exists(fileName))
            //{
            //    throw new FileNotFoundException("The file don't exist!");
            //}

            //if (text == string.Empty ||
            //    fileName.Substring(fileName.Length - 3, 3) != "png")
            //{
            //    return;
            //}

            System.Drawing.Image image = System.Drawing.Image.FromStream(sm);
            Bitmap newbitmap = new Bitmap(image.Width + image.Width, image.Height);

            Graphics g = Graphics.FromImage(newbitmap/*bitmap*/);
            g.DrawImageUnscaled(image, 0, 0);
            float fontSize = 38.0f;    //字体大小
            ////下面定义一个矩形区域，以后在这个矩形里画上白底黑字
            float rectWidth = image.Width;
            float rectHeight = image.Height;
            float rectX = (image.Width);
            float rectY = 0;
            //声明矩形域
            RectangleF textArea = new RectangleF(rectX, rectY, rectWidth, rectHeight);
            //fontSize = GetFontSize(g, image.Width, image.Height, text);
            //fontSize -= 2;
            System.Drawing.Font font = new System.Drawing.Font("宋体", fontSize);   //定义字体
            Brush whiteBrush = new SolidBrush(Color.Black);                         //黑笔刷，画文字用
            Brush blackBrush = new SolidBrush(Color.Transparent);                   //白笔刷，画背景用
            SizeF sizeF = g.MeasureString(text, new System.Drawing.Font("宋体", fontSize));
            //DateTime dtTemp=DateTime.MinValue;
            //if (DateTime.TryParse(sTimeStamp, out dtTemp))
            //{
            //    sTimeStamp = dtTemp.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            g.FillRectangle(blackBrush, rectX, rectY, rectWidth, rectHeight);
            g.DrawString(text, font, whiteBrush, rectWidth, (rectHeight - sizeF.Height) / 2);

            MemoryStream ms = new MemoryStream();
            RectangleF rectf = new RectangleF(0.0f, 0.0f, /*(float)(newbitmap.Width * 0.75)*/670.0f, newbitmap.Height);
            newbitmap = newbitmap.Clone(rectf, PixelFormat.DontCare);
            newbitmap.Save(ms, ImageFormat.Png);
            System.Drawing.Image.FromStream(ms).Save(SavePath);

            g.Dispose();
            newbitmap.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 添加时间戳到图片内部(时间戳背景为透明色)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        /// <param name="SavePath"></param>
        private void AddTextInsideImg(Stream sm, string text, string SavePath)
        {
            if (sm == null) return;
            //if (!File.Exists(fileName))
            //{
            //    throw new FileNotFoundException("The file don't exist!");
            //}

            //if (text == string.Empty ||
            //    fileName.Substring(fileName.Length - 3, 3) != "png")
            //{
            //    return;
            //}
            System.Drawing.Image image = System.Drawing.Image.FromStream(sm);
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);

            Graphics g = Graphics.FromImage(bitmap);
            float fontSize = 8.0f;    //字体大小
            float textWidth = text.Length * fontSize;  //文本的长度
            //下面定义一个矩形区域，以后在这个矩形里画上白底黑字

            float rectWidth = text.Length * (fontSize + 8);
            float rectHeight = fontSize + 14;
            float rectX = (image.Width) / 4; //0;
            float rectY = (image.Height - rectHeight) / 2;//0;
            //声明矩形域
            RectangleF textArea = new RectangleF(rectX, rectY, rectWidth, rectHeight);

            System.Drawing.Font font = new System.Drawing.Font("宋体", fontSize);   //定义字体
            Brush whiteBrush = new SolidBrush(Color.Gray);   //黑笔刷，画文字用
            Brush blackBrush = new SolidBrush(Color.Transparent);   //白笔刷，画背景用

            g.FillRectangle(blackBrush, rectX, rectY, rectWidth, rectHeight);

            g.DrawString(text, font, whiteBrush, textArea);
            MemoryStream ms = new MemoryStream();
            //保存为Jpg类型
            bitmap.Save(ms, ImageFormat.Png);
            System.Drawing.Image.FromStream(ms).Save(SavePath);
            //输出处理后的图像，这里为了演示方便，我将图片显示在页面中了
            //Response.Clear();
            //Response.ContentType = "image/jpeg";
            //Response.BinaryWrite(ms.ToArray());

            g.Dispose();
            bitmap.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 写入二维表数据到WORD模板表格中
        /// </summary>
        /// <param name="TblIndex">WORD模板表格索引（基数从1开始）</param>
        /// <param name="al">待写入WORD模板表格的二维表数据</param>
        public override void WriteTable(int TblIndex, ArrayList al)
        {
            if (m_doc.Tables.Count == 0 || al.Count == 0 ) return;
            Word.Table table = null;
            try
            {
                table = m_doc.Tables[TblIndex];
                if (table == null) return;
            }
            catch
            {
                return;
            }
            
            FillData2Table(table, al);
        }

        /// <summary>
        /// 写入二维表数据到WORD模板表格中
        /// </summary>
        /// <param name="TblIndex">WORD模板表格索引（基数从1开始）</param>
        /// <param name="SubTblIdx">WORD模板表格中的子表索引（基数从1开始）</param>
        /// <param name="al">待写入WORD模板表格的二维表数据</param>
        public override void WriteTable(int TblIndex, int SubTblIdx, ArrayList al)
        {
            if (m_doc.Tables.Count == 0 || m_doc.Tables[TblIndex].Tables.Count == 0 || al.Count == 0) return;
            Word.Table table = null;
            try
            {
                table = m_doc.Tables[TblIndex].Tables[SubTblIdx];
                if (table == null) return;
            }
            catch
            {
                return;
            }

            FillData2Table(table, al);
        }

        /// <summary>
        /// 将数据写入WORD模板页眉页尾
        /// </summary>
        /// <param name="key">打印模板参考位置</param>
        /// <param name="value">待输出到WORD文档中的字符串</param>
        /// <param name="mode">写入时的辅助参考值</param>
        public override void WriteHeaderFooter(string key, string[] value, WriteMode mode)
        {
            if (value.Length != 3) return;
            for (int i = 1; i <= m_doc.Sections.Count; i++)
            {
                m_doc.Sections[i].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Paragraphs[2].Range.Text = value[0];
                m_doc.Sections[i].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Paragraphs[3].Range.Text = "编码：" + value[1] + "\r";
                //m_doc.Sections[i].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Paragraphs[4].Range.Text = "页码： 2 / 3\r";
                m_doc.Sections[i].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Paragraphs[5].Range.Text = "版次： " + value[2] + "版";
            }
        }

        /// <summary>
        /// 过滤最小值的日期字符串
        /// </summary>
        /// <param name="strDateTime">待过滤日期字符串</param>
        /// <returns>若未最小值日期字符串则返回空，否则返回日期字符串</returns>
        public string CheckDateTime(string strDateTime)
        {
            if (string.IsNullOrEmpty(strDateTime)) return "";

            if (strDateTime == "0001-1-1")
            {
                return (strDateTime = "");
            }

            return strDateTime;
        }

        private void FillData2Table(Word.Table table, ArrayList al)
        {
            object oRow = table.Rows.Last;
            Word.Row newRow = table.Rows.Add(ref oRow);
            String sID = "";
            String strFilename = "";
            String sTableName = "T_OU_User";
            String sSQL = "";
            object Link2File = false;
            object SaveWithDoc = true;
            object cRange = null;

            for (int row = 0; row < al.Count; row++)
            {
                try{
                    switch (table.Columns.Count)
                    { 
                        case 2:
                            //newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            ///////begin/////////////////////////////////////////////////////////////////////////////////////////
                            sID = ((ArrayList)(al[row]))[0].ToString();

                            strFilename = HttpContext.Current.Server.MapPath(@"~\template\Singer\" + ((ArrayList)(al[row]))[0].ToString() + ".png");
                            
                            try
                            {
                                //string sColImgData = "";
                                sSQL = "SELECT [Image] FROM " + sTableName + " WHERE [Name] = '" + sID + "'";

                                System.Data.DataTable dtImgData = FounderSoftware.Framework.Business.Entity.RunQuery(sSQL, "Sql",
                                                                OAConfig.GetConfig("数据库", "DataSource"),
                                                                OAConfig.GetConfig("数据库", "DataBase"),
                                                                OAConfig.GetConfig("数据库", "uid"),
                                                                OAConfig.GetConfig("数据库", "pwd"));
                                if (dtImgData.Rows.Count == 0) break;

                                //Image.FromStream(new MemoryStream((byte[])dtImgData.Rows[0]["Image"])).Save(sPath);

                                // 取不到时间，则更具配置参数决定是输出当前日期，还是直接输出签名图片不显示时间戳
                                MemoryStream ms = new MemoryStream((byte[])dtImgData.Rows[0]["Image"]);
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                Bitmap newbitmap = new Bitmap(image.Width, image.Height);
                                Graphics g = Graphics.FromImage(newbitmap);
                                g.DrawImageUnscaled(image, 0, 0);
                                newbitmap.Save(strFilename, ImageFormat.Png);
                                System.Drawing.Image.FromStream(ms).Save(strFilename);
                                g.Dispose();
                                newbitmap.Dispose();
                                image.Dispose();

                            }
                            catch (Exception ex)
                            {
                            }

                            //////////end////////////////////////////////////////////////////////////////////////////////////////////
                            cRange = newRow.Cells[1].Range;

                            object Left = 23;
                            object Top = 0;
                            object Width = 98;
                            object Height = 40;


                            m_doc.Shapes.AddPicture(strFilename,
                                                    ref Link2File,
                                                    ref SaveWithDoc,
                                                    ref Left, ref Top,
                                                    ref Width, ref Height,
                                                    ref cRange);

                            //newRow.Cells[1].Range.InlineShapes.AddPicture(strFilename, ref Link2File, ref SaveWithDoc, ref cRange);
                            //newRow.Cells[1].Range.InlineShapes[1].Height = 25f;
                            //newRow.Cells[1].Range.InlineShapes[1].Width = 55f;
                            cRange = newRow.Cells[2].Range;
                            newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();

                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);

                            File.Delete(strFilename);

                            break;
                        case 3:
                            newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();
                            newRow.Cells[3].Range.Text = ((ArrayList)(al[row]))[2].ToString();

                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);
                            //if ("\r\a" == table.Rows.Last.Cells[1].Range.Text)
                            //    table.Rows.Last.Delete();
                            break;
                        case 4:
                            newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();
                            newRow.Cells[3].Range.Text = ((ArrayList)(al[row]))[2].ToString();
                            newRow.Cells[4].Range.Text = ((ArrayList)(al[row]))[3].ToString();
                            
                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);
                            //if ("\r\a" == table.Rows.Last.Cells[1].Range.Text)
                            //    table.Rows.Last.Delete();
                            break;
                        case 5:
                            newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();
                            newRow.Cells[3].Range.Text = ((ArrayList)(al[row]))[2].ToString();
                            newRow.Cells[4].Range.Text = ((ArrayList)(al[row]))[3].ToString();
                            newRow.Cells[5].Range.Text = ((ArrayList)(al[row]))[4].ToString();

                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);
                            //if ("\r\a" == table.Rows.Last.Cells[1].Range.Text)
                            //    table.Rows.Last.Delete();
                            break;
                        case 6:
                            newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            
                            /////////////////部门会签人名///////////////////////////////////
                            //newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();

                            sID = ((ArrayList)(al[row]))[1].ToString();

                            strFilename = HttpContext.Current.Server.MapPath(@"~\template\Singer\" + ((ArrayList)(al[row]))[1].ToString() + ".png");

                            try
                            {
                                sSQL = "SELECT [Image] FROM " + sTableName + " WHERE [Name] = '" + sID + "'";

                                System.Data.DataTable dtImgData = FounderSoftware.Framework.Business.Entity.RunQuery(sSQL, "Sql",
                                                                OAConfig.GetConfig("数据库", "DataSource"),
                                                                OAConfig.GetConfig("数据库", "DataBase"),
                                                                OAConfig.GetConfig("数据库", "uid"),
                                                                OAConfig.GetConfig("数据库", "pwd"));
                                if (dtImgData.Rows.Count == 0) break;

                                // 直接输出签名图片不显示时间戳
                                MemoryStream ms = new MemoryStream((byte[])dtImgData.Rows[0]["Image"]);
                                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                //Bitmap newbitmap = new Bitmap(image.Width, image.Height);
                                //Graphics g = Graphics.FromImage(newbitmap);
                                //g.DrawImageUnscaled(image, 0, 0);
                                //newbitmap.Save(strFilename, ImageFormat.Png);
                                //System.Drawing.Image.FromStream(ms).Save(strFilename);
                                //g.Dispose();
                                //newbitmap.Dispose();
                                //image.Dispose();
                                AddTextOutsideImg(ms, ((ArrayList)(al[row]))[2].ToString(), strFilename);
                            }
                            catch (Exception ex)
                            {
                            }

                            cRange = newRow.Cells[2].Range;
                            object Left1 = -5;
                            object Top1 = 4;
                            object Width1 = 170;
                            object Height1 = 40;


                            m_doc.Shapes.AddPicture(strFilename,
                                                    ref Link2File,
                                                    ref SaveWithDoc,
                                                    ref Left1, ref Top1,
                                                    ref Width1, ref Height1,
                                                    ref cRange);

                           File.Delete(strFilename);
                        
                        ///////////////////////////////////////////////////////
                            newRow.Cells[3].Range.Text = ((ArrayList)(al[row]))[2].ToString();

                            newRow.Cells[4].Range.Text = ((ArrayList)(al[row]))[3].ToString();

                            ////////////////部门会签人名/////////////////////////////////////////////////
                            //newRow.Cells[5].Range.Text = ((ArrayList)(al[row]))[4].ToString();
                            sID = ((ArrayList)(al[row]))[4].ToString();

                            strFilename = HttpContext.Current.Server.MapPath(@"~\template\Singer\" + ((ArrayList)(al[row]))[4].ToString() + ".png");

                            try
                            {
                                sSQL = "SELECT [Image] FROM " + sTableName + " WHERE [Name] = '" + sID + "'";

                                System.Data.DataTable dtImgData = FounderSoftware.Framework.Business.Entity.RunQuery(sSQL, "Sql",
                                                                OAConfig.GetConfig("数据库", "DataSource"),
                                                                OAConfig.GetConfig("数据库", "DataBase"),
                                                                OAConfig.GetConfig("数据库", "uid"),
                                                                OAConfig.GetConfig("数据库", "pwd"));
                                if (dtImgData.Rows.Count == 0) break;

                                // 直接输出签名图片不显示时间戳
                                MemoryStream ms = new MemoryStream((byte[])dtImgData.Rows[0]["Image"]);
                                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                //Bitmap newbitmap = new Bitmap(image.Width, image.Height);
                                //Graphics g = Graphics.FromImage(newbitmap);
                                //g.DrawImageUnscaled(image, 0, 0);
                                //newbitmap.Save(strFilename, ImageFormat.Png);
                                //System.Drawing.Image.FromStream(ms).Save(strFilename);
                                //g.Dispose();
                                //newbitmap.Dispose();
                                //image.Dispose();
                                AddTextOutsideImg(ms, ((ArrayList)(al[row]))[2].ToString(), strFilename);
                            }
                            catch (Exception ex)
                            {
                            }

                            cRange = newRow.Cells[5].Range;
                            object Left2 = -15;
                            object Top2 = 4;
                            object Width2 = 170;
                            object Height2 = 40;


                            m_doc.Shapes.AddPicture(strFilename,
                                                    ref Link2File,
                                                    ref SaveWithDoc,
                                                    ref Left2, ref Top2,
                                                    ref Width2, ref Height2,
                                                    ref cRange);

                            File.Delete(strFilename);

                            ///////////////////////////////////////////////////////////
                            newRow.Cells[6].Range.Text = ((ArrayList)(al[row]))[5].ToString();

                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);
                            //if ("\r\a" == table.Rows.Last.Cells[1].Range.Text)
                            //    table.Rows.Last.Delete();
                            break;
                        case 7:
                            newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();
                            newRow.Cells[3].Range.Text = ((ArrayList)(al[row]))[2].ToString();
                            newRow.Cells[4].Range.Text = ((ArrayList)(al[row]))[3].ToString();
                            newRow.Cells[5].Range.Text = ((ArrayList)(al[row]))[4].ToString();
                            newRow.Cells[6].Range.Text = ((ArrayList)(al[row]))[5].ToString();
                            newRow.Cells[7].Range.Text = ((ArrayList)(al[row]))[6].ToString();

                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);
                            //if ("\r\a" == table.Rows.Last.Cells[1].Range.Text)
                            //    table.Rows.Last.Delete();
                            break;
                        case 8:
                            newRow.Cells[1].Range.Text = ((ArrayList)(al[row]))[0].ToString();
                            newRow.Cells[2].Range.Text = ((ArrayList)(al[row]))[1].ToString();
                            newRow.Cells[3].Range.Text = ((ArrayList)(al[row]))[2].ToString();
                            newRow.Cells[4].Range.Text = ((ArrayList)(al[row]))[3].ToString();
                            newRow.Cells[5].Range.Text = ((ArrayList)(al[row]))[4].ToString();
                            newRow.Cells[6].Range.Text = ((ArrayList)(al[row]))[5].ToString();
                            newRow.Cells[7].Range.Text = ((ArrayList)(al[row]))[6].ToString();
                            newRow.Cells[8].Range.Text = ((ArrayList)(al[row]))[7].ToString();

                            oRow = table.Rows.Last;
                            newRow = table.Rows.Add(ref oRow);
                            //if ("\r\a" == table.Rows.Last.Cells[2].Range.Text)
                            //    table.Rows.Last.Delete();
                            break;
                    }
                }
                catch
                {
                    continue;
                }
            }
            while (true)
            {
                int iCount = 0;
                try
                {
                    if (table.Rows.Last.Cells[1].Range.Text == "\r\a"
                        && table.Rows.Last.Cells[2].Range.Text == "\r\a")
                    {
                        table.Rows.Last.Delete();
                    }
                    else
                    {
                        break;
                    }
                    iCount++;
                    if (iCount >= 50) break;
                }
                catch
                {
                    break;
                }

            }
        }

        private bool TrySave()
        {
            try
            {
                m_doc.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override void Save()
        {
            object SavePath = this.SavePath;
            object ReadOnlyRecommended = false;
            //m_doc.SaveAs(ref SavePath, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            //            ref oMissing, ref ReadOnlyRecommended, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            //            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            int TryCount = 0;
            while(true)
            {
                if (!TrySave())
                {
                    Thread.Sleep(500);
                    TryCount++;
                    if (TryCount > _TRY_COUNT_)
                    {
                        throw new Exception("导出打印文件失败");
                    }
                }
                else
                {
                    break;
                }
                
            }
            //try
            //{
            //    m_doc.Save();
            //}
            //catch {
            //    Thread.Sleep(200);
            //    m_doc.Save();
            //}
        }

        public override void Close()
        {
            if (m_WordApp != null)
            {
                if(m_doc != null)
                    m_doc.Close(ref oMissing, ref oMissing, ref oMissing);
                m_WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                m_doc = null;
                m_WordApp = null;
            }
            KillWordProcess(new TimeSpan(0, _INVALID_WORDPROC_TIMEOUT_, 0));
        }

        /// <summary>
        /// 清楚word进程
        /// </summary>
        public static void KillWordProcess(TimeSpan interval)
        {
            //string sLogPath = HttpContext.Current.Server.MapPath(@"~\template\printlog.txt");
            //FileStream fs = new FileStream(sLogPath, FileMode.OpenOrCreate, FileAccess.Write);
            //ASCIIEncoding encoding = new ASCIIEncoding();

            System.Diagnostics.Process[] myPs;
            myPs = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in myPs)
            {
                if (p.Id != 0)
                {
                    //string myS = "WINWORD.EXE" + p.ProcessName + "  ID:" + p.Id.ToString();
                    //TimeSpan ts = DateTime.Now - p.StartTime;
                    //string str = p.ProcessName + "--" + ts.Minutes.ToString() + "\r\n";
                    //fs.Write(encoding.GetBytes(str), 0, encoding.GetByteCount(str));
                    //if (p.ProcessName.ToUpper() == "WINWORD" && (DateTime.Now - p.StartTime >= interval))
                    //{
                    //    fs.Write(encoding.GetBytes("Killed\r\n"), 0, encoding.GetByteCount("Killed\r\n"));
                    //    p.Kill();
                    //}
                    try
                    {
                        if (p.Modules != null)
                            if (p.Modules.Count > 0)
                            {
                                System.Diagnostics.ProcessModule pm = p.Modules[0];
                                //myS += "\n Modules[0].FileName:" + pm.FileName;
                                //myS += "\n Modules[0].ModuleName:" + pm.ModuleName;
                                //myS += "\n Modules[0].FileVersionInfo:\n" + pm.FileVersionInfo.ToString();

                                //TimeSpan ts = DateTime.Now - p.StartTime;
                                //string str = pm.ModuleName + "--" + ts.Minutes.ToString() + "\r\n";
                                //fs.Write(encoding.GetBytes(str), 0, encoding.GetByteCount(str));
                                if (pm.ModuleName.ToLower() == "winword.exe" && (DateTime.Now - p.StartTime >= interval))
                                {
                                    //fs.Write(encoding.GetBytes("Killed\r\n"), 0, encoding.GetByteCount("Killed\r\n"));
                                    p.Kill();
                                }
                            }
                    }
                    catch
                    { }
                    finally
                    {
                        ;
                    }
                }
            }
            //fs.Close();
        }

        /// <summary>
        /// 清楚word进程
        /// </summary>
        public static void KillWordProcess()
        {
            System.Diagnostics.Process[] myPs;
            myPs = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in myPs)
            {
                if (p.Id != 0)
                {
                    string myS = "WINWORD.EXE" + p.ProcessName + "  ID:" + p.Id.ToString();
                    try
                    {
                        if (p.Modules != null)
                            if (p.Modules.Count > 0)
                            {
                                System.Diagnostics.ProcessModule pm = p.Modules[0];
                                //myS += "\n Modules[0].FileName:" + pm.FileName;
                                //myS += "\n Modules[0].ModuleName:" + pm.ModuleName;
                                //myS += "\n Modules[0].FileVersionInfo:\n" + pm.FileVersionInfo.ToString();
                                if (pm.ModuleName.ToLower() == "winword.exe")
                                    p.Kill();
                            }
                    }
                    catch
                    { }
                    finally
                    {
                        ;
                    }
                }
            }
        }

        private void SetTextFormat(Word.Range range)
        {
            //range.Bold = this.Format.Bold;
            range.Font.Name = this.Format.FontName;
            range.Font.Size = this.Format.FontSize;
            range.Font.Bold = this.Format.Bold;
            //range.Italic = this.Format.Italic;
        }

        private int InsertBlankParagrap(string key, WriteMode mode)
        {
            int ParagraphIndex = 0;
            for (int i = 1; i <= m_doc.Paragraphs.Count; i++)
            {
                try
                {
                    if (m_doc.Paragraphs[i].Range.Text.Contains(key))
                    {
                        switch (mode)
                        {
                            case WriteMode.Up:
                                m_doc.Paragraphs[i - 2].Range.InsertAfter("\r\a");
                                ParagraphIndex = i - 1;
                                break;
                            case WriteMode.Down:
                                m_doc.Paragraphs[i + 2].Range.InsertAfter("\r\a");
                                ParagraphIndex = i + 2;
                                break;
                            case WriteMode.Inner:
                                m_doc.Paragraphs[i].Range.InsertAfter("\r\a");
                                ParagraphIndex = i;
                                break;
                            case WriteMode.Right:
                                m_doc.Paragraphs[i + 1].Range.InsertAfter("\r\a");
                                ParagraphIndex = i + 1;
                                break;
                            case WriteMode.Down_Append:
                                m_doc.Paragraphs.Last.Range.InsertAfter("\r\a");
                                ParagraphIndex = m_doc.Paragraphs.Count - 1;
                                break;
                        }
                        return ParagraphIndex;
                    }
                }
                catch {
                    continue;
                }
            }
            return ParagraphIndex;
        }

        private string iPages = "0";
        public override string Pages
        {
            get
            {
                object obj = null;
                if (m_doc == null) iPages = "0";
                return (iPages = m_doc.ComputeStatistics(WdStatistic.wdStatisticPages, ref obj).ToString());
            }
        }

        /// <summary>
        /// 设置WORD布局，保证某些文档段落沉底
        /// </summary>
        /// <param name="tableIndex">待插入空行的表格索引</param>
        /// <param name="iRow">待插入空行的表格行索引</param>
        /// <param name="iCol">待插入空行的表格列索引</param>
        public override void DocLayout(int tableIndex, int iRow, int iCol)
        {
            if (tableIndex == 0) return;
            object obj = null;

            int iPages = m_doc.ComputeStatistics(WdStatistic.wdStatisticPages, ref obj);
            int iLines = m_doc.ComputeStatistics(WdStatistic.wdStatisticLines, ref obj);

            Word.Table table = m_doc.Tables[tableIndex];
            while (true)
            {
                table.Rows[iRow].Cells[iCol].Range.InsertAfter("\r\a");

                int iCurPages = m_doc.ComputeStatistics(WdStatistic.wdStatisticPages, ref obj);
                if (iPages < iCurPages)
                {
                    table.Rows[iRow].Cells[iCol].Range.Paragraphs.First.Range.Delete(ref oMissing, ref oMissing);
                    return;
                }
            }
        }

        public override void DocLayout(string LayoutZone, WriteMode mode)
        {
            if (string.IsNullOrEmpty(LayoutZone)) return;
            object obj = null;

            int iPages = m_doc.ComputeStatistics(WdStatistic.wdStatisticPages, ref obj);
            int iLines = m_doc.ComputeStatistics(WdStatistic.wdStatisticLines, ref obj);

            int iParagraphIndex = 0;
            try
            {
                while (true)
                {
                    iParagraphIndex = InsertBlankParagrap(LayoutZone, mode);
                    if (iParagraphIndex == 0) return;

                    int iCurPages = m_doc.ComputeStatistics(WdStatistic.wdStatisticPages, ref obj);
                    if (iPages < iCurPages)
                    {
                        m_doc.Paragraphs[iParagraphIndex + 1].Range.Delete(ref oMissing, ref oMissing);
                        iParagraphIndex = iParagraphIndex - 2;
                        m_doc.Paragraphs[iParagraphIndex].Range.Delete(ref oMissing, ref oMissing);
                        return;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 将WORD文档转换未PDF格式（注：必须在服务器端安装WORD转换PDF插件）
        /// </summary>
        /// <param name="SavePath"></param>
        public override void Convert2pdf(string SavePath)
        {
            try
            {
                object oSavePath = SavePath;
                object oReadOnlyRecommended = true;
                object oPwd = "321";
                object oWritePwd = "123";
                object oSaveFormat = WdSaveFormat.wdFormatPDF;
                m_doc.SaveAs(ref oSavePath, ref oSaveFormat, ref oMissing, ref oPwd, ref oMissing, ref oWritePwd,
                             ref oReadOnlyRecommended, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                             ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            }
            catch
            {
                throw new Exception("请安装OFFICE WORD转换PDF文件插件");
            }
        }


        /// <summary>
        /// 遍历模版文档表格数据，记录得到的KEY值所在的表格位置，以及Sentences位置
        /// </summary>
        /// <returns></returns>
        private ArrayList TraverTable(int TableIndex)
        {
            try
            {
                ArrayList arTmp = new ArrayList();

                if (m_doc.Tables.Count > 0)
                {
                    for (int iRow = 1; iRow <= m_doc.Tables[TableIndex].Rows.Count; iRow++)
                    {
                        for (int iCol = 1; iCol <= m_doc.Tables[TableIndex].Rows[iRow].Cells.Count; iCol++)
                        {
                            KeyPosition kp = new KeyPosition();
                            kp.TblIndex = TableIndex;
                            if (m_doc.Tables[TableIndex].Rows[iRow].Cells[iCol].Range.Paragraphs.Count > 1)
                            {
                                kp.CnKey = HandleString(m_doc.Tables[TableIndex].Rows[iRow].Cells[iCol].Range.Paragraphs[1].Range.Text);
                                kp.EnKey = HandleString(m_doc.Tables[TableIndex].Rows[iRow].Cells[iCol].Range.Paragraphs[2].Range.Text);
                            }
                            else
                            {
                                kp.CnKey = HandleString(m_doc.Tables[TableIndex].Rows[iRow].Cells[iCol].Range.Paragraphs[1].Range.Text);
                            }
                            kp.RowIndex = iRow;
                            kp.ColIndex = iCol;
                            //kp.SenIndex = TraverSentence(kp.CnKey);
                            arTmp.Add(kp);
                        }
                    }
                }
                return arTmp;
            }
            catch
            {
                return null;
            }
        }

        #region 保留功能
        /// <summary>
        /// 根据KEY值遍历得到该KEY值所在表格位置
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        private KeyPosition TraverTable(string sKey)
        {
            if (m_doc.Tables.Count > 0)
            {
                for (int idx = 1; idx <= m_doc.Tables[1].Rows.Count; idx++)
                {
                    Word.Row row = m_doc.Tables[1].Rows[idx];
                    int iColIndex = TraverRow(row, sKey);
                    if (iColIndex != 0)
                    {
                        return (new KeyPosition(sKey, row.Index, iColIndex, 0/*TraverSentence(sKey)*/));
                    }
                }
            }
            return (new KeyPosition());
        }

        private int TraverRow(Word.Row row, string sKey)
        {
            if (row == null || row.Cells.Count == 0) return 0;
            for (int iColIdx = 1; iColIdx <= row.Cells.Count; iColIdx++)
            {
                if (row.Cells[iColIdx].Range.Text == sKey)
                {
                    return iColIdx;
                }
            }
            return 0;
        }
        #endregion

        private int TraverSentence(string sKey)
        {
            if (m_doc.Sentences.Count == 0) return 0;
            for (int iSenIdx = 1; iSenIdx <= m_doc.Sentences.Count; iSenIdx++)
            {
                if (sKey == m_doc.Sentences[iSenIdx].Text)
                {                    
                    return iSenIdx;
                }
            }
            return 0;
        }

        private KeyPosition GetPositionByKey(string sKey, ArrayList alkp)
        {
            if (alkp.Count == 0) return new KeyPosition();
            for (int i = 0; i < alkp.Count; i++)
            {
                if (HandleString(((KeyPosition)alkp[i]).CnKey) == sKey)
                {
                    return ((KeyPosition)alkp[i]);
                }
            }
            return new KeyPosition();
        }

        public override void DeleteTable(int TblIdx)
        {
            Word.Table table = m_doc.Tables[TblIdx];
            if (table == null) return;
            table.Delete();
        }

        public override void DeleteString(string sKey)
        {
            for (int i = 1; i <= m_doc.Paragraphs.Count; i++)
            {
                if (base.HandleString(m_doc.Paragraphs[i].Range.Text) == sKey)
                {
                    m_doc.Paragraphs[i].Range.Delete(ref oMissing, ref oMissing);
                    return;
                }
            }
        }
    }
}