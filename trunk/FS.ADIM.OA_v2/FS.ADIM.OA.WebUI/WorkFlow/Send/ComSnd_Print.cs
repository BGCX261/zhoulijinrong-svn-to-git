using System;
using WordMgr;
using System.Text;
using System.Text.RegularExpressions;
using FS.ADIM.OA.WebUI.PageWF;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.WorkFlow.Send
{
    public class ComSnd_Print
    {
        public string m_ProcessID = "";
        public string m_TemplateID = "";
        public string m_WorkItemID = "";

        private string[] ResolveSignerAndContent(string SignersAndContents)
        {
            string[] str = SignersAndContents.Split(new string[] { "：", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return str;
        }

        private void ResolveSignerAndContent(string SignersAndContents, out string Signers, out string Contents)
        {
            string[] str = SignersAndContents.Split(new string[] { "：", ":", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbSigners = new StringBuilder();
            StringBuilder sbContents = new StringBuilder();

            Regex rx = new Regex(@"(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)");

            for (int i = 0; i < str.Length; i++)
            {
                string tmp = str[i].Trim();
                string[] sDate = tmp.Split(new char[] { '[', ']' });

                if (sDate.Length > 1 && rx.IsMatch(sDate[1]))
                {
                    sbSigners.Append(str[i]);
                    sbSigners.Append("\r\n");
                }
                else
                {
                    string sContent = str[i].Replace('\r', ' ');
                    sContent = sContent.Replace('\n', ' ');
                    sbContents.Append(sContent);
                    sbContents.Append("\r\n");
                }

            }
            Signers = sbSigners.ToString();
            Contents = sbContents.ToString();
        }

        private void SetBaseExportData(UC_Print ucPrint, EntitySend cEntity)
        {
            //ucPrint.AttachFileList = cEntity.FileList;
            //ucPrint.Position = "抄送:";//(string)ucPrint.ExportData[2];
            //ucPrint.Mode = WriteMode.Right;

            ucPrint.ExportData.Add(cEntity.UrgentDegree);//<col>紧急程度:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentNo);//<col>发文号:|right</col>
            ucPrint.ExportData.Add(OAUser.GetUserName(cEntity.Signer));//<col>签发:|right</col>
            ucPrint.ExportData.Add(cEntity.LeadSigners);//<col>会签人:|right</col>
            ucPrint.ExportData.Add(cEntity.DeptSigners);//<col>会签人: |right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.SignDate.ToShortDateString()));//<col>日期:|right</col>
            ucPrint.ExportData.Add(cEntity.Verifier);//<col>秘书:|right</col>
            if (ucPrint.UCStepName == "审稿")
            {
                ucPrint.ExportData.Add("");
            }
            else
            {
                ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.VerifyDate.ToShortDateString()));//<col>日期: |right</col>
            }
            ucPrint.ExportData.Add(cEntity.ZhuRenSigner);//<col>主任:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ZhuRenSignDate.ToShortDateString()));//<col>日期:  |right</col>
            ucPrint.ExportData.Add(cEntity.CheckDrafterName);//<col>审稿人:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.CheckDate.ToShortDateString()));//<col>审稿日期:|right</col>
            ucPrint.ExportData.Add(/*OADept.GetDeptName(*/cEntity.HostDeptName/*)*/);//<col>主办部门:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));//<col>拟稿日期:|right</col>
            ucPrint.ExportData.Add(cEntity.Drafter);//<col>拟稿人:|right</col>
            ucPrint.ExportData.Add(cEntity.PhoneNum);//<col>电话:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentTitle);//<col>标题:|right</col>
            ucPrint.ExportData.Add(cEntity.SubjectWord);//<col>主题词:|right</col>
            ucPrint.ExportData.Add(cEntity.MainSenders);//<col>主送:|right</col>
            ucPrint.ExportData.Add(cEntity.CopySenders);//<col>抄送:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.SendDate.ToShortDateString()));//<col>发文日期:|right</col>
            ucPrint.ExportData.Add(Convert.ToString(cEntity.ShareCount > 0 ? cEntity.ShareCount : 0));//<col>共印|right</col>
            ucPrint.ExportData.Add(Convert.ToString(cEntity.SheetCount > 0 ? cEntity.SheetCount : 0));//<col>份,每份|right</col>
            ucPrint.ExportData.Add(cEntity.Typist);//<col>打字:|right</col>
            ucPrint.ExportData.Add(cEntity.Checker);//<col>校对:|right</col>
            ucPrint.ExportData.Add(cEntity.ReChecker);//<col>复核:|right</col>
            //ucPrint.ExportData.Add(cEntity.Prompt);//<col>提示信息:|right</col>
            //ucPrint.ExportData.Add(cEntity.Prompt);//<col>添加|right</col>
        }

        public void SetPrintBeginExport(UC_Print ucPrint, EntitySend cEntity)
        {
            bool IsContent = false;
            switch (ucPrint.FileName)
            {
                #region 工程会议纪要
                case "工程会议纪要":
                    //ucPrint.ExportData.Add("");    //<col>第一期|shift</col>
                    //ucPrint.ExportData.Add("海南核电有限公司                         " + DateTime.Now.ToString("yyyy年MM月dd日"));//<col>2009年某月某日|shift</col>

                    ucPrint.ExportData.Add(cEntity.DocumentTitle); //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);   //<col>[主送单位]|shift</col>
                    //正文
                    ucPrint.ExportData.Add("");                //<col>[正文仿宋三号,不加粗]|shift</col>

                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);  //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("  分送：" + cEntity.CopySenders);    //<col>分送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                        + cEntity.VerifyDate.Month.ToString() + "月"
                        + cEntity.VerifyDate.Day.ToString() + "日印发"
                        /*cEntity.VerifyDate.ToShortDateString()*/);          //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);      //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);                  //<col>校对：|inner</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "正文";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Shift;
                    break;
                #endregion
                #region 公文报告模版
                case "公文报告模版":
                    //ucPrint.ExportData.Add(cEntity.DocumentNo);    //<col>海核  发﹝2009﹞  号|shift</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle); //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);   //<col>[主送单位]|shift</col>
                    //正文
                    ucPrint.ExportData.Add("");                    //<col>[正文仿宋三号,不加粗]|shift</col>
                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    if (IsContent)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.Year.ToString() + "年"
                                                                        + cEntity.DraftDate.Month.ToString() + "月"
                                                                        + cEntity.DraftDate.Day.ToString() + "日")
                                                ); //<col>[二〇〇九某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);  //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("  抄送：" + cEntity.CopySenders);    //<col>抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                        + cEntity.VerifyDate.Month.ToString() + "月"
                        + cEntity.VerifyDate.Day.ToString() + "日印发"
                        /*cEntity.VerifyDate.ToShortDateString()*/);            //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);                    //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);                  //<col>校对：|inner</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "主题词：" + cEntity.SubjectWord;//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                #region 公文函模版
                case "公文函模版":
                    //ucPrint.ExportData.Add(cEntity.DocumentNo);   //<col>海核  发﹝2009﹞  号|shift</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);//<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);   //<col>[主送单位]|shift</col>
                    //正文
                    ucPrint.ExportData.Add("");                    //<col>[正文仿宋三号,不加粗]|shift</col>

                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    if (IsContent)
                    {

                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.Year.ToString() + "年"
                                                                        + cEntity.DraftDate.Month.ToString() + "月"
                                                                        + cEntity.DraftDate.Day.ToString() + "日")
                                                );      //<col>[二〇〇九年某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    ucPrint.ExportData.Add("主题词:" + cEntity.SubjectWord);       //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("  抄送：" + cEntity.CopySenders);      //<col>  抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                                            + cEntity.VerifyDate.Month.ToString() + "月"
                                            + cEntity.VerifyDate.Day.ToString() + "日印发"
                                            );                  //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);    //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);   //<col>校对：|inner</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "主题词:" + cEntity.SubjectWord;
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                #region 公文请示模版
                case "公文请示模版":
                    //ucPrint.ExportData.Add(cEntity.DocumentNo + "                        " + cEntity.SignerName);   //<col>海核办发[2009]1号|shift</col>
                    //ucPrint.ExportData.Add(cEntity.SignerName);    //<col>签发人：|inner</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle); //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);   //<col>[主送单位]|shift</col>
                    //正文
                    ucPrint.ExportData.Add("");                    //<col>[正文仿宋三号,不加粗]|shift</col>
                    //bool IsContent = false;
                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    if (IsContent)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.Year.ToString() + "年"
                                                                        + cEntity.DraftDate.Month.ToString() + "月"
                                                                        + cEntity.DraftDate.Day.ToString() + "日")
                                               ); //<col>[二〇〇九年某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }

                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);  //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("  抄送：" + cEntity.CopySenders);    //  抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                                            + cEntity.VerifyDate.Month.ToString() + "月"
                                            + cEntity.VerifyDate.Day.ToString() + "日印发"
                                            );  //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);    //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);  //<col>校对：|inner</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "主题词：" + cEntity.SubjectWord;
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                #region 公文首页纸
                case "公文首页纸":
                    ucPrint.ExportData.Add("");                          //<col>密别：|inner</col>
                    string[] result = cEntity.DocumentNo.Split(new char[] { '<', '>', '[', ']', '(', ')', '〔', '〕', '号' }, StringSplitOptions.RemoveEmptyEntries);
                    if (result.Length != 3)
                    {
                        if (result.Length > 3)
                        {
                            ucPrint.ExportData.Add(result[0]);          //<col>海核|shift</col>
                            ucPrint.ExportData.Add(result[1]);          //<col>编号|shift</col>
                            ucPrint.ExportData.Add(result[2]);          //<col>号|shift</col>
                        }
                        else
                        {
                            int ret = 3 - result.Length;
                            for (int i = 0; i < result.Length; i++)
                            {
                                ucPrint.ExportData.Add(result[i]);
                            }
                            for (int j = 0; j < ret; j++)
                            {
                                ucPrint.ExportData.Add("");
                            }
                        }
                    }
                    else
                    {
                        ucPrint.ExportData.Add(result[0]);          //<col>海核|shift</col>
                        ucPrint.ExportData.Add(result[1]);          //<col>编号|shift</col>
                        ucPrint.ExportData.Add(result[2]);          //<col>号|shift</col>
                    }
                    ucPrint.ExportData.Add(cEntity.UrgentDegree);        //<col>紧急程度：|inner</col>
                    ucPrint.ExportData.Add(cEntity.SignerName + "\r\n"
                        + ucPrint.CheckDateTime(cEntity.SignDate.ToShortDateString()));          //<col>签发|shift</col>
                    string DetpSigners = FormsMethod.GetSingers4Print(m_ProcessID, m_WorkItemID, "部门会签", "公司发文");
                    string[] results = DetpSigners.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);//renjinquan+ 去除空值
                    if (results.Length == 0)
                    {
                        ucPrint.ExportData.Add(DetpSigners/*cEntity.DeptSigners*/);         //<col>会签|down</col>
                    }
                    else
                    {
                        //if (results.Length >= 2)
                        //    ucPrint.ExportData.Add(results[0] + "\n" + results[1]);
                        string strNames = "";
                        for (int i = 0; i < results.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                strNames += (results[i].TrimStart(' ') != "\n" ? (results[i] + "\n") : "");
                            }
                            else
                            {
                                try
                                {
                                    strNames += DateTime.Parse(results[i]).ToShortDateString();
                                }
                                catch 
                                {
                                    
                                } 
                                            
                            }
                        }
                        ucPrint.ExportData.Add(strNames);
                    }
                    string sVerify = (string.IsNullOrEmpty(cEntity.ZhuRenSigner) ? "" : cEntity.ZhuRenSigner + "\r\n" + ucPrint.CheckDateTime(cEntity.ZhuRenSignDate.ToShortDateString()) + "\r\n");
                    string sCVerify = (string.IsNullOrEmpty(cEntity.Verifier) ? "" : cEntity.Verifier + "\r\n" + ucPrint.CheckDateTime(cEntity.VerifyDate.ToShortDateString()));

                    ucPrint.ExportData.Add(sVerify + sCVerify);     //<col>核稿：|down</col>
                    ucPrint.ExportData.Add(cEntity.CheckDrafterName + "\r\n"
                        + ucPrint.CheckDateTime(cEntity.CheckDate.ToShortDateString()));    //<col>审稿|right</col>
                    string sDrafterDate = ((cEntity.FirstDraftDate == DateTime.MinValue) ? cEntity.DraftDate.ToShortDateString() : cEntity.FirstDraftDate.ToShortDateString());
                    ucPrint.ExportData.Add(cEntity.Drafter + "\r\n" + sDrafterDate + "\r\n" + cEntity.PhoneNum);             //<col>拟拟及稿电人话|right</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);         //<col>主送：|right</col>
                    ucPrint.ExportData.Add(cEntity.CopySenders);         //<col>抄送：|right</col>
                    ucPrint.ExportData.Add(cEntity.SubjectWord);         //<col>主题词：|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);       //<col>标题：|right</col>
                    ucPrint.ExportData.Add(cEntity.Typist);              //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);             //<col>校对：|right</col>
                    ucPrint.ExportData.Add(cEntity.ShareCount.ToString()); //<col>共印|right</col>
                    //ucPrint.ExportData.Add("第份" + cEntity.SheetCount.ToString() + "张");   //<col>第份       张|shift</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "标题：";
                    //ucPrint.Mode = WriteMode.Down;
                    break;
                #endregion
                #region 公文通知模版
                case "公文通知模版":
                    //ucPrint.ExportData.Add(cEntity.DocumentNo);     //<col>海核  发﹝2009﹞  号|shift</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle); //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);   //<col>[主送单位]|shift</col>
                    //正文
                    ucPrint.ExportData.Add("");                    //<col>[正文仿宋三号,不加粗]|shift</col>
                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    if (IsContent)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.Year.ToString() + "年"
                                                                        + cEntity.DraftDate.Month.ToString() + "月"
                                                                        + cEntity.DraftDate.Day.ToString() + "日")
                                                ); //<col>[二〇〇九年某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);  //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("  抄送：" + cEntity.CopySenders);    //<col>  抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                                            + cEntity.VerifyDate.Month.ToString() + "月"
                                            + cEntity.VerifyDate.Day.ToString() + "日印发"
                                            );  //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);    //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);   //<col>校对：|inner</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "主题词：" + cEntity.SubjectWord;//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                #region 会议纪要模版
                case "会议纪要模版":
                    ucPrint.ExportData.Add(cEntity.DocumentNo);    //<col>海核纪要[2009]号|shift</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle); //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);   //<col>[主送单位]|shift</col>
                    ucPrint.ExportData.Add("");                    //<col>[正文仿宋三号,不加粗]|shift</col>
                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    if (IsContent)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.Year.ToString() + "年"
                                                                        + cEntity.DraftDate.Month.ToString() + "月"
                                                                        + cEntity.DraftDate.Day.ToString() + "日")
                                                ); //<col>[二〇〇九年某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);    //<col>主题词：***  **  纪要（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add(" 抄送：" + cEntity.CopySenders);        //<col>抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                                            + cEntity.VerifyDate.Month.ToString() + "月"
                                            + cEntity.VerifyDate.Day.ToString() + "日印发"); //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);                //<col>打字：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Checker);               //<col>校对：|inner</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "主题词：" + cEntity.SubjectWord;
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                #region 简报模版
                case "简报模版":
                    //ucPrint.ExportData.Add("");            //<col>中核集团海南核电有限公司深入学习实践科学发展观活动|shift</col>
                    //ucPrint.ExportData.Add("");            //<col>第期|shift</col>
                    //ucPrint.ExportData.Add("");            //<col>海南核电有限公司深入学习实践|shift</col> 
                    //ucPrint.ExportData.Add("");            //<col>科学发展观活动领导小组办公室|shift</col>
                    /*"海南核电有限公司深入学习实践\r\n"
                     +"科学发展观活动领导小组办公室"*/
                    //ucPrint.ExportData.Add(DateTime.Now.ToString("yyyy年MM月dd日"));//<col>二〇〇九年 月 日|shift</col>
                    //ucPrint.ExportData.Add(cEntity.DocumentTitle);             //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);               //<col>[主送单位]|shift</col>
                    ucPrint.ExportData.Add("");                                //<col>[正文仿宋三号,不加粗]|shift</col>

                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    /*
                    if (IsContent)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.SendDate.Year.ToString() + "年"
                                                                        + cEntity.SendDate.Month.ToString() + "月"
                                                                        + cEntity.SendDate.Day.ToString() + "日")
                                                ); //<col>[二〇〇九年某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    
                    ucPrint.ExportData.Add(cEntity.ReceiveUserName);      //<col>分送：|right</col>
                    ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.ReceiveDateTime.Year.ToString() + "年"
                                                                    + cEntity.ReceiveDateTime.Month.ToString() + "月"
                                                                    + cEntity.ReceiveDateTime.Day.ToString() + "日印发")
                                            );  //<col>2009年  月  日印发|shift</col>

                    ucPrint.ExportData.Add(cEntity.Typist);     //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);    //<col>校对：|inner</col>
                    */
                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "分送：";
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                #region 简讯模版
                case "简讯模版":
                    //ucPrint.ExportData.Add("海南核电有限公司办公室编" + "               " + DateTime.Now.ToString("yyyy年MM月dd日"));      //<col>海南核电有限公司办公室编               二〇〇九年  月  日|shift</col>
                    //ucPrint.ExportData.Add(DateTime.Now.ToString("yyyy年MM月dd日"));   //<col>二〇〇九年某月某日|shift</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);                     //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders/*OAList.GetDeptNameByID(cEntity.HostDept)*/);  //<col>[主送单位] |shift</col>
                    //正文
                    ucPrint.ExportData.Add("");                                        //<col>[正文仿宋三号，不加粗] |shift</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "附件";
                    //ucPrint.Mode = WriteMode.Up;
                    break;
                #endregion
                case "公司发文表单":
                    SetBaseExportData(ucPrint, cEntity);

                    string Signers = "";
                    string Contents = "";

                    //ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "部门会签", m_TemplateID), out Signers, out Contents);
                    string[] str1 = ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "部门会签", m_TemplateID));
                    if (str1 != null /*&& str1.Length == 2*/)
                    {
                        for (int i = 0; i < str1.Length; i += 2)
                        {
                            if (i >= str1.Length) break;
                            Signers += (str1[i] + "\n");
                            Contents += (str1[i + 1] + "\n");
                        }
                    }
                    ucPrint.ExportData.Add(Signers/*cEntity.DeptHaveSigners*/);       //<col>已会签人:|right</col>
                    ucPrint.ExportData.Add(Contents/*cEntity.DeptSignComment*/);       //<col>意见:|right</col>

                    Signers = "";
                    Contents = "";

                    //ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "领导会签", m_TemplateID), out Signers, out Contents);
                    string[] str2 = ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "领导会签", m_TemplateID));
                    if (str2 != null /*&& str2.Length == 2*/)
                    {
                        for (int i = 0; i < str2.Length; i += 2)
                        {
                            if (i >= str2.Length) break;
                            Signers += (str2[i] + "\n");
                            Contents += (str2[i + 1] + "\n");
                        }
                    }
                    ucPrint.ExportData.Add(Signers/*cEntity.LeadHaveSigners*/);       //<col>已会签人: |right</col>
                    ucPrint.ExportData.Add(Contents/*cEntity.LeadSignComment*/);       //<col>意见: |right</col>

                    ucPrint.ExportData.Add(cEntity.CirculateAddNames);     //<col>分发范围:|right</col>

                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));
                    break;
            }
        }

        public void SetPrintAttachExport(UC_Print ucPrint, EntitySend cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "工程会议纪要":
                    ucPrint.WriteContent("正文", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;
                case "公文报告模版":
                    ucPrint.WriteContent("  海南核电有限公司办公室", WriteMode.Up, 5);
                    ucPrint.WriteAttach();
                    break;
                case "公文函模版":
                    ucPrint.WriteContent("  海南核电有限公司办公室", WriteMode.Up, 6);
                    ucPrint.WriteAttach();
                    break;
                case "公文请示模版":
                    ucPrint.WriteContent("  海南核电有限公司", WriteMode.Up, 6);
                    ucPrint.WriteAttach();
                    break;
                case "公文首页纸":
                    ucPrint.WriteContent("打字：", WriteMode.Up, 2);
                    ucPrint.WriteAttach();
                    break;
                case "公文通知模版":
                    ucPrint.WriteContent("  海南核电有限公司办公室", WriteMode.Up, 5);
                    ucPrint.WriteAttach();
                    break;
                case "会议纪要模版":
                    ucPrint.WriteContent("海南核电有限公司", WriteMode.Up, 5);
                    ucPrint.WriteAttach();
                    break;
                case "简报模版":
                    ucPrint.WriteContent("正文", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;
                case "简讯模版":
                    ucPrint.WriteContent("正文", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;

                case "公司发文表单":
                    //ucPrint.WriteContent("抄送:", WriteMode.Right, 1);
                    //ucPrint.WriteAttach();
                    break;
            }
        }
    }

    public class Snd_Print
    {
        public string m_ProcessID = "";
        public string m_TemplateID = "";
        public string m_WorkItemID = "";

        private string[] ResolveSignerAndContent(string SignersAndContents)
        {
            string[] str = SignersAndContents.Split(new string[] { "：" }, StringSplitOptions.RemoveEmptyEntries);
            return str;
        }

        private void ResolveSignerAndContent(string SignersAndContents, out string Signers, out string Contents)
        {
            string[] str = SignersAndContents.Split(new string[] { "：", ":", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbSigners = new StringBuilder();
            StringBuilder sbContents = new StringBuilder();

            Regex rx = new Regex(@"(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)");

            for (int i = 0; i < str.Length; i++)
            {
                string tmp = str[i].Trim();
                string[] sDate = tmp.Split(new char[] { '[', ']' });

                if (sDate.Length > 1 && rx.IsMatch(sDate[1]))
                {
                    sbSigners.Append(str[i]);
                    sbSigners.Append("\r\n");
                }
                else
                {
                    string sContent = str[i].Replace('\r', ' ');
                    sContent = sContent.Replace('\n', ' ');
                    sbContents.Append(sContent);
                    sbContents.Append("\r\n");
                }

            }
            Signers = sbSigners.ToString();
            Contents = sbContents.ToString();
        }

        private void SetBaseExportData(UC_Print ucPrint, B_DJGTSend cEntity)
        {
            //TODO:处理非终节点模板
            //ucPrint.AttachFileList = cEntity.FileList;
            //ucPrint.Position = "抄送:";//(string)ucPrint.ExportData[2];
            //ucPrint.Mode = WriteMode.Right;

            ucPrint.ExportData.Add(cEntity.UrgentDegree);//<col>紧急程度:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentNo);//<col>发文号:|right</col>
            ucPrint.ExportData.Add(cEntity.Signer);//<col>签发:|right</col>
            ucPrint.ExportData.Add(cEntity.LeadSigners);//<col>会签人:|right</col>
            ucPrint.ExportData.Add(cEntity.DeptSigners);//<col>会签人: |right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.SignDate.ToShortDateString()));//<col>日期:|right</col>
            ucPrint.ExportData.Add(cEntity.Verifier);//<col>秘书:|right</col>
            if (ucPrint.UCStepName == "审稿")
            {
                ucPrint.ExportData.Add("");
            }
            else
            {
                ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.VerifyDate.ToShortDateString()));//<col>日期: |right</col>
            }
            //ucPrint.ExportData.Add(""/*cEntity.ZhuRenSigner*/);//<col>主任:|right</col>
            //ucPrint.ExportData.Add(""/*ucPrint.CheckDateTime(cEntity.ZhuRenSignDate.ToShortDateString())*/);//<col>日期:  |right</col>
            ucPrint.ExportData.Add(cEntity.CheckDrafter);//<col>审稿人:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.CheckDraftDate.ToShortDateString()));//<col>审稿日期:|right</col>
            ucPrint.ExportData.Add(/*OADept.GetDeptName(*/cEntity.HostDept/*)*/);//<col>主办部门:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.ToShortDateString()));//<col>拟稿日期:|right</col>
            ucPrint.ExportData.Add(cEntity.Drafter);//<col>拟稿人:|right</col>
            ucPrint.ExportData.Add(cEntity.PhoneNum);//<col>电话:|right</col>
            ucPrint.ExportData.Add(cEntity.DocumentTitle);//<col>标题:|right</col>
            ucPrint.ExportData.Add(cEntity.SubjectWord);//<col>主题词:|right</col>
            ucPrint.ExportData.Add(cEntity.MainSenders);//<col>主送:|right</col>
            ucPrint.ExportData.Add(cEntity.CopySenders);//<col>抄送:|right</col>
            ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.SendDate.ToShortDateString()));//<col>发文日期:|right</col>
            ucPrint.ExportData.Add(cEntity.ShareCount);//<col>共印|right</col>
            ucPrint.ExportData.Add(cEntity.SheetCount);//<col>份,每份|right</col>
            ucPrint.ExportData.Add(cEntity.Typist);//<col>打字:|right</col>
            ucPrint.ExportData.Add(cEntity.Checker);//<col>校对:|right</col>
            ucPrint.ExportData.Add(cEntity.ReChecker);//<col>复核:|right</col>
            //ucPrint.ExportData.Add(cEntity.Prompt);//<col>提示信息:|right</col>
            //ucPrint.ExportData.Add(cEntity.Prompt);//<col>添加|right</col>
        }

        public void SetPrintBeginExport(UC_Print ucPrint, B_DJGTSend cEntity)
        {
            bool IsContent = false;
            switch (ucPrint.FileName)
            {
                #region 党委部门文件模版 党委请示模版 海南党委文件模版 海南工会请示模版 海南共青团文件模版 海南纪委文件模版
                case "党委部门文件模版":
                case "党委请示模版":
                case "海南党委文件模版":
                case "海南工会请示模版":
                case "海南共青团文件模版":
                case "海南纪委文件模版":

                case "海南工会文件模版":
                case "海南共青团请示文件模版":
                case "海南纪委请示文件模版":
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);       //<col>[标题]|shift</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);         //<col>[主送单位]|shift</col>
                    ucPrint.ExportData.Add("");                          //<col>[正文仿宋三号,不加粗]|shift</col>
                    for (int i = 0; i < cEntity.FileList.Count; i++)
                    {
                        if (cEntity.FileList[i].IsZhengWen == "1")
                        {
                            IsContent = true;
                            break;
                        }
                    }
                    if (IsContent)
                    {
                        ucPrint.ExportData.Add(ucPrint.CheckDateTime(cEntity.DraftDate.Year.ToString() + "年"
                                                                        + cEntity.DraftDate.Month.ToString() + "月"
                                                                        + cEntity.DraftDate.Day.ToString() + "日")
                                                ); //<col>[二〇〇九某月某日]|shift</col>
                    }
                    else
                    {
                        ucPrint.ExportData.Add("");
                    }
                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);   //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("抄送：" + cEntity.CopySenders);     //<col>抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                        + cEntity.VerifyDate.Month.ToString() + "月"
                        + cEntity.VerifyDate.Day.ToString() + "日印发"
                        /*cEntity.VerifyDate.ToShortDateString()*/);            //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);                     //<col>打字：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Checker);                    //<col>校对：|right</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "正文";//(string)ucPrint.ExportData[2];
                    //ucPrint.Mode = WriteMode.Shift;
                    break;
                #endregion

                #region 会议纪要模板
                case "党群工作全例会会议纪要模版":
                case "党委会议纪要模版":
                case "党政联席会纪要模版":
                    ucPrint.ExportData.Add("主题词：" + cEntity.SubjectWord);   //<col>主题词：（三号黑体，中间空2字符）|shift</col>
                    ucPrint.ExportData.Add("分送：" + cEntity.CopySenders);     //<col>抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                        + cEntity.VerifyDate.Month.ToString() + "月"
                        + cEntity.VerifyDate.Day.ToString() + "日印发"
                        /*cEntity.VerifyDate.ToShortDateString()*/);            //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);                     //<col>打字：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Checker);                    //<col>校对：|right</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    break;
                case "党群简报模版":
                    ucPrint.ExportData.Add("分送：" + cEntity.CopySenders);     //<col>抄送：（仿宋三号，不加粗）|shift</col>
                    ucPrint.ExportData.Add(cEntity.VerifyDate.Year.ToString() + "年"
                        + cEntity.VerifyDate.Month.ToString() + "月"
                        + cEntity.VerifyDate.Day.ToString() + "日印发"
                        /*cEntity.VerifyDate.ToShortDateString()*/);            //<col>2009年  月  日印发|shift</col>
                    ucPrint.ExportData.Add(cEntity.Typist);                     //<col>打字：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Checker);                    //<col>校对：|right</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    break;
                #endregion

                #region 党纪工团发文 首页纸模板
                case "党委公文首页纸模板":
                case "工会首页纸":
                case "共青团首页纸":
                case "纪律检查委员会首页纸":
                    ucPrint.ExportData.Add("");                          //<col>密别：|inner</col>
                    string[] result = cEntity.DocumentNo.Split(new char[] { '<', '>', '[', ']', '(', ')', '〔', '〕', '号' }, StringSplitOptions.RemoveEmptyEntries);
                    if (result.Length != 3)
                    {
                        if (result.Length > 3)
                        {
                            ucPrint.ExportData.Add(result[0]);          //<col>海核|shift</col>
                            ucPrint.ExportData.Add(result[1]);          //<col>编号|shift</col>
                            ucPrint.ExportData.Add(result[2]);          //<col>号|shift</col>
                        }
                        else
                        {
                            int ret = 3 - result.Length;
                            for (int i = 0; i < result.Length; i++)
                            {
                                ucPrint.ExportData.Add(result[i]);
                            }
                            for (int j = 0; j < ret; j++)
                            {
                                ucPrint.ExportData.Add("");
                            }
                        }
                    }
                    else
                    {
                        ucPrint.ExportData.Add(result[0]);          //<col>海核|shift</col>
                        ucPrint.ExportData.Add(result[1]);          //<col>编号|shift</col>
                        ucPrint.ExportData.Add(result[2]);          //<col>号|shift</col>
                    }
                    ucPrint.ExportData.Add(cEntity.UrgentDegree);        //<col>紧急程度：|inner</col>
                    ucPrint.ExportData.Add(cEntity.Signer + "\r\n"
                        + ucPrint.CheckDateTime(cEntity.SignDate.ToShortDateString()));          //<col>签发|shift</col>
                    string DetpSigners = FormsMethod.GetSingers4Print(m_ProcessID, m_WorkItemID, "部门会签", "党纪工团发文");
                    DetpSigners = DetpSigners.Replace("[", "\n[");
                    DetpSigners = DetpSigners.Replace("[", "");
                    DetpSigners = DetpSigners.Replace("]", "");
                    DetpSigners = DetpSigners.Replace(" \n", "\n");
                    //DetpSigners = DetpSigners.Replace(" ", "\r\a");
                    ucPrint.ExportData.Add(DetpSigners/*cEntity.DeptSigners*/);         //<col>会签|down</col>

                    //string sVerify = (string.IsNullOrEmpty(cEntity.Verifiers) ? "" : cEntity.Verifiers + "\r\n" + ucPrint.CheckDateTime(cEntity.VerifyDate.ToShortDateString()) + "\r\n");
                    string sCVerify = (string.IsNullOrEmpty(cEntity.Verifier) ? "" : cEntity.Verifier + "\r\n" + ucPrint.CheckDateTime(cEntity.VerifyDate.ToShortDateString()));

                    ucPrint.ExportData.Add(sCVerify);     //<col>核稿：|down</col>
                    ucPrint.ExportData.Add(cEntity.CheckDrafter + "\r\n"
                        + ucPrint.CheckDateTime(cEntity.CheckDraftDate.ToShortDateString()));    //<col>审稿|right</col>
                    string sDrafterDate = ((cEntity.FirstDraftDate == DateTime.MinValue) ? cEntity.DraftDate.ToShortDateString() : cEntity.FirstDraftDate.ToShortDateString());
                    ucPrint.ExportData.Add(cEntity.Drafter + "\r\n" + sDrafterDate + "\r\n" + cEntity.PhoneNum);             //<col>拟拟及稿电人话|right</col>
                    ucPrint.ExportData.Add(cEntity.MainSenders);         //<col>主送：|right</col>
                    ucPrint.ExportData.Add(cEntity.CopySenders);         //<col>抄送：|right</col>
                    ucPrint.ExportData.Add(cEntity.SubjectWord);         //<col>主题词：|right</col>
                    ucPrint.ExportData.Add(cEntity.DocumentTitle);       //<col>标题：|right</col>
                    ucPrint.ExportData.Add(cEntity.Typist);              //<col>打字：|right</col>
                    ucPrint.ExportData.Add(cEntity.Checker);             //<col>校对：|right</col>
                    ucPrint.ExportData.Add(cEntity.ShareCount.ToString()); //<col>共印|right</col>
                    //ucPrint.ExportData.Add("第份" + cEntity.SheetCount.ToString() + "张");   //<col>第份       张|shift</col>

                    ucPrint.AttachFileList = cEntity.FileList;
                    //ucPrint.Position = "标题：";
                    //ucPrint.Mode = WriteMode.Down;
                    break;
                #endregion

                #region 党纪工团发文 表单模板
                case "党纪工团发文表单":
                    SetBaseExportData(ucPrint, cEntity);

                    string Signers = "";
                    string Contents = "";

                    //ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "部门会签", m_TemplateID), out Signers, out Contents);
                    string[] str1 = ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "部门会签", m_TemplateID));
                    if (str1 != null && str1.Length == 2)
                    {
                        Signers = str1[0];
                        Contents = str1[1];
                    }
                    ucPrint.ExportData.Add(Signers/*cEntity.DeptHaveSigners*/);       //<col>已会签人:|right</col>
                    ucPrint.ExportData.Add(Contents/*cEntity.DeptSignComment*/);       //<col>意见:|right</col>

                    Signers = "";
                    Contents = "";

                    //ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "领导会签", m_TemplateID), out Signers, out Contents);
                    string[] str2 = ResolveSignerAndContent(FormsMethod.GetYiHuiQian(m_ProcessID, "领导会签", m_TemplateID));
                    if (str2 != null && str2.Length == 2)
                    {
                        Signers = str2[0];
                        Contents = str2[1];
                    }
                    ucPrint.ExportData.Add(Signers/*cEntity.LeadHaveSigners*/);       //<col>已会签人: |right</col>
                    ucPrint.ExportData.Add(Contents/*cEntity.LeadSignComment*/);       //<col>意见: |right</col>

                    ucPrint.ExportData.Add(cEntity.Assigners);     //<col>分发范围:|right</col>

                    ucPrint.ExportData.Add(ucPrint.AttachFilesList(cEntity.FileList));
                    break;
                #endregion
            }
        }

        public void SetPrintAttachExport(UC_Print ucPrint, B_DJGTSend cEntity)
        {
            switch (ucPrint.FileName)
            {
                case "党委部门文件模版":
                case "党委请示模版":
                case "海南党委文件模版":
                case "海南工会请示模版":
                case "海南共青团文件模版":
                case "海南纪委文件模版":

                case "海南工会文件模版":
                case "海南共青团请示文件模版":
                case "海南纪委请示文件模版":
                    ucPrint.WriteContent("正文", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;

                case "党群工作全例会会议纪要模版":
                case "党委会议纪要模版":
                case "党政联席会纪要模版":
                case "党群简报模版":
                    ucPrint.WriteContent("正文", WriteMode.Up, 1);
                    ucPrint.WriteAttach();
                    break;

                case "党委公文首页纸模板":
                case "工会首页纸":
                case "共青团首页纸":
                case "纪律检查委员会首页纸":
                    ucPrint.WriteContent("标题：", WriteMode.Down, 1);
                    ucPrint.WriteAttach();
                    break;
            }
        }
    }
}
