//----------------------------------------------------------------
// Copyright (C) 2009 方正国际软件有限公司
//
// 文件功能描述：Word输出类
// 
// 创建标识：
//
// 修改标识：2010-05-10 任金权
// 修改描述：1.修改Page_Load函数，去除TextToHtmlCode使用，WriteComment等没有使用过HtmlToTextCode
//  
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------
using System;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
 
namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class PFInfoDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"].ToString()))
                {
                    B_PF pfEntity = new B_PF();
                    pfEntity.ID = int.Parse(Request.QueryString["ID"].ToString());

                    pfEntity = pfEntity.GetPFEntity(pfEntity.ID.ToString());
                    switch (Server.UrlDecode(Request.QueryString["column"].ToString().Trim()))
                    {
                        //case ProcessConstString.StepName.ProgramFile.STEP_WRITE:
                        //    lblInfo.Text = SysString.TextToHtmlCode(pfEntity.WriteComment);
                        //    break;
                        //case ProcessConstString.StepName.ProgramFile.STEP_CHECK:
                        //    lblInfo.Text = SysString.TextToHtmlCode(pfEntity.CheckComment);
                        //    break;
                        //case ProcessConstString.StepName.ProgramFile.STEP_AUDIT:
                        //    lblInfo.Text = SysString.TextToHtmlCode(pfEntity.AuditComment);
                        //    break;
                        //case ProcessConstString.StepName.ProgramFile.STEP_APPROVE:
                        //    lblInfo.Text = SysString.TextToHtmlCode(pfEntity.ApproveComment);
                        //    break;
                        //case ProcessConstString.StepName.ProgramFile.STEP_QG:
                        //    lblInfo.Text = SysString.TextToHtmlCode(pfEntity.QualityComment);
                        //    break;
                        case ProcessConstString.StepName.ProgramFile.STEP_WRITE:
                            lblInfo.Text = pfEntity.WriteComment;
                            break;
                        case ProcessConstString.StepName.ProgramFile.STEP_CHECK:
                            lblInfo.Text = pfEntity.CheckComment;
                            break;
                        case ProcessConstString.StepName.ProgramFile.STEP_AUDIT:
                            lblInfo.Text = pfEntity.AuditComment;
                            break;
                        case ProcessConstString.StepName.ProgramFile.STEP_APPROVE:
                            lblInfo.Text = pfEntity.ApproveComment;
                            break;
                        case ProcessConstString.StepName.ProgramFile.STEP_QG:
                            lblInfo.Text = pfEntity.QualityComment;
                            break;
                    }

                }
            }
        }
    }
}
