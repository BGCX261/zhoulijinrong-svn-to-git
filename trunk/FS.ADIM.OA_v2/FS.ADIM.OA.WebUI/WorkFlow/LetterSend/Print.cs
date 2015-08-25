//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：Word输出类
// 
// 创建标识：
//
// 修改标识：2010-05-10 任金权
// 修改描述：1.修改SetPrintBeginExport函数，去除content使用HtmlToTextCode，数据已经重新统一调整
//  
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------
using WordMgr;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkFlow.LetterSend
{
    public class Print
    {
        public void SetPrintBeginExport(UC_Print ucPrint, EntityLetterSend cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "函件发文表单":

                    ucPrint.ExportData.Add(cEntity.company1);  //<col>主送单位:|right</col>   
                    ucPrint.ExportData.Add(cEntity.ourRef);    //<col>我方发文号:|right</col>
                    ucPrint.ExportData.Add(cEntity.to);       //<col>主送人:|right</col>
                    ucPrint.ExportData.Add(cEntity.yourRef);   //<col>对方发文号:|right</col>

                    ucPrint.ExportData.Add(cEntity.ccCompany); //<col>抄送单位:|right</col>
                    ucPrint.ExportData.Add(cEntity.ccDept + "\r\n" + cEntity.ccLeader);   //<col>内部抄送:|right</col>
                    ucPrint.ExportData.Add(cEntity.pages);     //<col>页数:|right</col>
                        
                    //string str = string.Empty;

                    //System.Drawing.Font font = new Font("Wingdings 2", 10);
                    
                    //if (cEntity.jinJi)
                    //{
                    //    //str = "紧急";
                    //    //ucPrint.FontStyle.FontName = "Wingdings 2";
                    //    //str += (char)0x0052;
                    //    //ucPrint.FontStyle.FontName = "仿宋";
                    //}
                    //else
                    //{
                    //    str = "紧急□";
                    //}
                    //ucPrint.ExportData.Add(str);
                    //if (cEntity.huiZhi)
                    //{
                    //    //str = "回复";
                    //    //ucPrint.FontStyle.FontName = "Wingdings 2";
                    //    //str += (char)0x0052;
                    //    //ucPrint.FontStyle.FontName = "仿宋";
                    //}
                    //else
                    //{
                    //    str = "回复□";
                    //}
                    //ucPrint.ExportData.Add(str);
                    if (ucPrint.IsCN(cEntity.signDate) == true)
                        ucPrint.ExportData.Add(cEntity.signDate);           //<col>签发/日期:|right</col>
                    else
                        ucPrint.ExportData.Add(cEntity.qianFaRen+"\n"+ucPrint.CheckDateTime(cEntity.signDate));

                    ucPrint.ExportData.Add(cEntity.DocumentTitle);            ////<col>主题：|right</col>//.subject
                    //string tmp = cEntity.content.Replace("<br/>", "\r\n");  //<col>内容|shift</col>
                    //tmp = tmp.Replace("&nbsp", " ");
                    //tmp = tmp.Replace("&lt", "<");
                    //tmp = tmp.Replace("&gt", ">");
                    //tmp = tmp.Replace("&quot", "\"");
                    //string tmp = SysString.HtmlToTextCode(cEntity.content);
                    string tmp = cEntity.content;//renjinquan+
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        ucPrint.ExportData.Add(tmp);
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    ucPrint.ExportData.Add(cEntity.Drafter + "\n" + ucPrint.CheckDateTime(cEntity.DraftDate.ToString()));       //<col>拟稿/日期:|right</col>

                    if (ucPrint.IsCN(cEntity.heGaoRenDate)==false)
                        ucPrint.ExportData.Add(cEntity.heGaoRen+"\n"+ ucPrint.CheckDateTime(cEntity.heGaoRenDate));       //<col>核稿/日期:|right</col>
                    else
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.heGaoRenDate));
                    string result1 = FormatSplit(EntityToHuiQian(cEntity), new char[] { ';' });
                    result1 = FormatSplit(result1, new char[] { ' ' });
                    ucPrint.ExportData.Add(result1);                   //<col>会签/日期:|right</col>
                    //ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));                   //<col>附件:|down</col>
                    
                    //ucPrint.AttachFileList = cEntity.FileList;
                    ucPrint.Position = "拟稿/日期:";
                    ucPrint.Mode = WriteMode.Up;
                    break;
            }
        }
        private string FormatSplit(string value, char[] chars)
        {
            string ret = "";
            string[] result = value.Split(chars);
            for (int i = 0; i < result.Length; i++)
            {
               ret += result[i] + "\n";
            }
            return ret;
        }

        private const string DateFormat = "yyyy-MM-dd";
        private string EntityToHuiQian(EntityLetterSend entity)
        {
            if (entity.huiQian.Count > 0)//新版从list里取
            {
                string shuiqian = "";
                if (entity.huiQian.Count > 0)
                {
                    for (int i = 0; i < entity.huiQian.Count; i++)
                    {
                        if (entity.huiQian[i].ICount == entity.iHuiQianCount)
                        {
                            shuiqian += ";" + entity.huiQian[i].UserName + " " + entity.huiQian[i].Date.ToString(DateFormat);
                        }
                    }
                    if (shuiqian.Length > 0)
                    {
                        shuiqian = shuiqian.Substring(1);
                    }
                }
                return shuiqian;
            }
            else
            {            //兼容旧版
                if (entity.huiqianDates != "")
                {
                    return entity.huiqianDates;
                }
            }
            return "";
        }
        public void SetPrintAttachExport(UC_Print ucPrint, EntityLetterSend cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "函件发文表单":
                    //ucPrint.WriteContent("拟稿/日期：", WriteMode.Up, 1);
                    //ucPrint.WriteAttach();
                    break;
            }
        }
    }
}
