//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：UI基类
//
// 
// 创建标识： 2009-12-28
//
// 修改标识：2010-01-08
// 修改描述：增加了保存，以及保存草稿箱。流程的统一处理改用兼容老版本的方式。
//
// 修改标识：2010-02-02
// 修改描述：修改了弹出消息格式
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using Ascentn.Workflow.Base;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.OA.Framework.Logging;
using FS.OA.Framework.WorkFlow;
using FounderSoftware.ADIM.OU.BLL.Busi;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.UIBase
{
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract class FormsUIBase : FormBase
    {
        #region 页面加载

        /// <summary>
        /// 重写OnLoad事件
        /// 初始化或加载表单数据
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            DataTable l_dtbDataTable = null;
            DataRow l_dtrDataRow = null;
            B_ProcessInstance l_busProcessInstance = null;

            if (!Page.IsPostBack)
            {
                //是否为预览状态
                if (!String.IsNullOrEmpty(Request.QueryString[ConstString.QueryString.IS_PREVIEW]))
                {
                    this.IsPreview = true;
                }

                //表单TableID
                this.IdentityID = Convert.ToInt32(Request.QueryString[ConstString.QueryString.IdentityID]);

                //哪个菜单进入的 1.待办 2.公办 3.待阅 4.已阅
                this.EntryAction = Request.QueryString[ConstString.QueryString.ENTRY_ACTION];

                //流程实例号
                this.ProcessID = Request.QueryString[ConstString.QueryString.PROCESS_ID];

                //流程名称
                this.TemplateName = Request.QueryString[ConstString.QueryString.TEMPLATE_NAME];

                //步骤节点名称
                this.StepName = Request.QueryString[ConstString.QueryString.STEP_NAME];

                //步骤节点ID
                this.WorkItemID = Request.QueryString[ConstString.QueryString.WORKITEM_ID];

                if (!String.IsNullOrEmpty(this.WorkItemID))
                {
                    l_busProcessInstance = new B_ProcessInstance();

                    //根据WorkItemID获取流程信息
                    l_dtbDataTable = l_busProcessInstance.GetProcessByWorkItemID(this.WorkItemID);

                    //验证WorkItemID是否有效
                    if (l_dtbDataTable == null || l_dtbDataTable.Rows.Count == 0)
                    {
                        //无效的WorkItemID,请确保WorkItemID的正确性
                        //return;
                    }
                    else
                    {
                        l_dtrDataRow = l_dtbDataTable.Rows[0];

                        //流程实例号
                        this.ProcessID = l_dtrDataRow["PROC_INST_ID"].ToString();

                        //子流程实例号
                        this.SubProcessID = l_dtrDataRow["SUB_PROC_INST_ID"].ToString();

                        //流程步骤名称
                        this.StepName = l_dtrDataRow["STEP_NAME"].ToString();

                        //流程模版名称
                        this.TemplateName = l_dtrDataRow["PDEF_NAME"].ToString();

                        //流程相关WorkObjectID
                        this.WorkObjectID = l_dtrDataRow["WORK_OBJECT_ID"].ToString();

                        //公办组ID
                        this.CommonID = l_dtrDataRow["POOL_ID"].ToString();
                    }
                }

                //待阅
                if (this.EntryAction == "3")
                {
                    this.StepName = ProcessConstString.StepName.STEP_CIRCULATE;
                }

                if (String.IsNullOrEmpty(this.StepName) || this.StepName == "1")
                {
                    if (this.TemplateName == ProcessConstString.TemplateName.COMPANY_RECEIVE || this.TemplateName == ProcessConstString.TemplateName.LETTER_RECEIVE)
                    {
                        base.IsFromDraft = true;
                    }
                    switch (this.TemplateName)
                    {
                        case ProcessConstString.TemplateName.COMPANY_RECEIVE://公司收文 
                        case ProcessConstString.TemplateName.MERGED_RECEIVE:
                            this.StepName = ProcessConstString.StepName.ReceiveStepName.STEP_INITIAL;
                            break;
                        case ProcessConstString.TemplateName.LETTER_RECEIVE://函件收文 
                            this.StepName = ProcessConstString.StepName.LetterReceiveStepName.STEP_INITIAL;
                            break;
                        case ProcessConstString.TemplateName.AFFILIATION://工作联系单 
                        case ProcessConstString.TemplateName.DJGT_Send: //党纪工团发文 
                        case ProcessConstString.TemplateName.COMPANY_SEND://公司发文 
                        case ProcessConstString.TemplateName.FINANCE_TRIPAPPLY://出差申请 
                            //this.StepName = ProcessConstString.StepName.FinanceStepName.STEP_APPLY;
                            //break;
                        case ProcessConstString.TemplateName.INSTUCTION_REPORT://请示报告 
                            this.StepName = ProcessConstString.StepName.STEP_DRAFT;
                            break;
                        case ProcessConstString.TemplateName.LETTER_SEND://函件发文 
                            this.StepName = ProcessConstString.StepName.LetterSend.发起函件;
                            break;
                        case ProcessConstString.TemplateName.PROGRAM_FILE: //程序文件 
                            this.StepName = ProcessConstString.StepName.ProgramFile.STEP_WRITE;
                            break;
                        case ProcessConstString.TemplateName.FinanceCCBX_APPLY: //出差培训报销单 
                            this.StepName = ProcessConstString.StepName.STEP_DRAFT;
                            break;
                        case ProcessConstString.TemplateName.FinanceZDBX_APPLY: //招待费报销单 
                            this.StepName = ProcessConstString.StepName.STEP_DRAFT;
                            break;
                        case ProcessConstString.TemplateName.FinanceHWBX_APPLY: //会务费用报销单 
                            this.StepName = ProcessConstString.StepName.STEP_DRAFT;
                            break;
                        case ProcessConstString.TemplateName.FinanceJK_APPLY: //借款申请单 
                            this.StepName = ProcessConstString.StepName.FinanceJKStepName.STEP_NiGao;
                            break;
                    }
                }
            }

            if (String.IsNullOrEmpty(this.ProcessID))
            {
                //发起流程，产生流程实例号
                this.ProcessID = Guid.NewGuid().ToString("N").ToUpper();
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.BindOUControl();

                this.EntityToControl();
            }

            this.SetControlStatus();
        }

        #endregion

        #region 视图状态

        /// <summary>
        /// 保存自定义视图状态
        /// </summary>
        protected override object SaveViewState()
        {
            Object l_objViewState = base.SaveViewState();
            ViewState.Add(ConstString.ViewState.IDENTITY_ID, this.IdentityID);
            ViewState.Add(ConstString.ViewState.TEMPLATE_NAME, this.TemplateName);
            ViewState.Add(ConstString.ViewState.SUB_PROCESSID, this.SubProcessID);
            ViewState.Add(ConstString.ViewState.STEP_NAME, this.StepName);
            ViewState.Add(ConstString.ViewState.PROCESS_ID, this.ProcessID);
            ViewState.Add(ConstString.ViewState.WORKITEM_ID, this.WorkItemID);
            ViewState.Add(ConstString.ViewState.WORKOBJECT_ID, this.WorkObjectID);
            ViewState.Add(ConstString.ViewState.COMMON_ID, this.CommonID);
            ViewState.Add(ConstString.ViewState.IS_PREVIEW, this.IsPreview);
            ViewState.Add(ConstString.ViewState.ENTRY_ACTION, this.EntryAction);
            ViewState.Add(ConstString.ViewState.COMMENT_LIST, this.Comments);
            l_objViewState = base.SaveViewState();
            return l_objViewState;
        }

        /// <summary>
        /// 加载自定义视图状态
        /// </summary>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            if (ViewState[ConstString.ViewState.IDENTITY_ID] != null)
            {
                m_intIdentityID = Convert.ToInt32(ViewState[ConstString.ViewState.IDENTITY_ID]);
            }
            if (ViewState[ConstString.ViewState.TEMPLATE_NAME] != null)
            {
                m_strTemplateName = ViewState[ConstString.ViewState.TEMPLATE_NAME].ToString();
            }
            if (ViewState[ConstString.ViewState.SUB_PROCESSID] != null)
            {
                m_strSubProcessID = ViewState[ConstString.ViewState.SUB_PROCESSID].ToString();
            }
            if (ViewState[ConstString.ViewState.STEP_NAME] != null)
            {
                m_strStepName = ViewState[ConstString.ViewState.STEP_NAME].ToString();
            }
            if (ViewState[ConstString.ViewState.PROCESS_ID] != null)
            {
                m_strProcessID = ViewState[ConstString.ViewState.PROCESS_ID].ToString();
            }
            if (ViewState[ConstString.ViewState.WORKITEM_ID] != null)
            {
                m_strWorkItemID = ViewState[ConstString.ViewState.WORKITEM_ID].ToString();
            }
            if (ViewState[ConstString.ViewState.WORKOBJECT_ID] != null)
            {
                m_strWorkObjectID = ViewState[ConstString.ViewState.WORKOBJECT_ID].ToString();
            }
            if (ViewState[ConstString.ViewState.COMMON_ID] != null)
            {
                m_strCommonID = ViewState[ConstString.ViewState.COMMON_ID].ToString();
            }
            if (ViewState[ConstString.ViewState.IS_PREVIEW] != null)
            {
                m_blnIsPreview = Convert.ToBoolean(ViewState[ConstString.ViewState.IS_PREVIEW]);
            }
            if (ViewState[ConstString.ViewState.ENTRY_ACTION] != null)
            {
                m_strEntryAction = ViewState[ConstString.ViewState.ENTRY_ACTION].ToString();
            }
            if (ViewState[ConstString.ViewState.COMMENT_LIST] != null)
            {
                m_listComment = ViewState[ConstString.ViewState.COMMENT_LIST] as List<CYiJian>;
            }
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 绑定组织机构
        /// </summary>
        protected virtual void BindOUControl() { }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        protected virtual void SetControlStatus() { }

        /// <summary>
        /// 绑定实体到控件
        /// </summary>
        protected virtual void EntityToControl() { }

        /// <summary>
        /// 绑定控件到实体
        /// </summary>
        /// <returns></returns>
        protected virtual EntityBase ControlToEntity(Boolean p_blnIsSaveDraft)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_blnIsSaveDraft"></param>
        /// <param name="p_strActionName"></param>
        /// <param name="p_htAttributes"></param>
        /// <param name="p_entEntityData"></param>
        protected void FormSubmit(Boolean p_blnIsSaveDraft, String p_strActionName, Hashtable p_htAttributes, EntityBase p_entEntityData)
        {
            String l_strMessage = String.Empty;
            User l_objUser = null;
            Boolean l_blnIsProcessCreated = true;

            try
            {
                ////M_20100406  
                ////huangqi
                ////修改说明：流程与附件模板名称是否匹配
                ////Begin
                //if (p_entEntityData.FileList.Count > 0 && p_entEntityData.FileList[0].ProcessType != base.TemplateName)
                //{
                //    this.ShowMsgBox(this.Page, MsgType.VbInformation, "流程与附件模板名称不比配，请注销后重新处理该文件。", base.EntryAction);
                //    return;
                //}
                ////End

                if (String.IsNullOrEmpty(base.WorkItemID))
                {
                    l_blnIsProcessCreated = false;
                }

                //保存实体数据
                ReturnInfo l_objReturnInfo = FormSave.SaveEntity(p_entEntityData, this, p_blnIsSaveDraft, p_strActionName, base.TemplateName);
                if (!l_objReturnInfo.IsSucess)
                {
                    //this.ShowMsgBox(this.Page, MsgType.VbExclamation, "表单保存出错,后续操作不能进行！" + l_objReturnInfo.ErrMessage);
                    JScript.Alert("表单保存出错,后续操作不能进行！" + l_objReturnInfo.ErrMessage, TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE);
                    return;
                }

                if (p_blnIsSaveDraft)
                {
                    if (l_blnIsProcessCreated)
                    {
                        //this.ShowMsgBox(this.Page, MsgType.VbInformation, "保存草稿成功！");
                        bool isUseUp=false;
                        if( TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE)
                            isUseUp=true;
                         else if( TemplateName == ProcessConstString.TemplateName.FinanceCCBX_APPLY)
                            isUseUp=true;
                        else
                            isUseUp=false;
                        JScript.Alert("保存草稿成功！", isUseUp);
                    }
                    else
                    {
                        this.ShowMsgBox(this.Page, MsgType.VbInformation, "保存草稿成功,可从草稿箱查看已保存文件！", "7");
                    }
                    return;
                }

                //流程操作
                DoWorkItem(p_htAttributes, l_blnIsProcessCreated);

            //获取下一个节点
            getnext: DataTable l_dtbDataTable = B_FormsData.GetNextWorkItem(this.ProcessID, this.WorkItemID);
            if (l_dtbDataTable.Rows.Count == 0)
            {
                //有时候反应慢 数据还没产生
                System.Threading.Thread.Sleep(500);
                l_dtbDataTable = B_FormsData.GetNextWorkItem(this.ProcessID, this.WorkItemID);
            }
                //防止协办与交办的冲突
                if (base.TemplateName == ProcessConstString.TemplateName.LETTER_RECEIVE && base.SubAction != ProcessConstString.SubmitAction.LetterReceiveAction.ACTION_TJCB)
                {
                    if (l_dtbDataTable != null && l_dtbDataTable.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow l_dtrDataRow in l_dtbDataTable.Rows)
                        {
                            if (l_dtrDataRow["NAME"].ToString() != ProcessConstString.StepName.LetterReceiveStepName.STEP_HELP_DIRECTOR)
                            {
                                i = 1;
                                break;
                            }
                            else if (base.SubAction == ProcessConstString.SubmitAction.ACTION_SUBMIT && base.StepName == ProcessConstString.StepName.LetterReceiveStepName.STEP_LETTER_ADMIN)
                            {
                                i = 1;
                                break;
                            }
                        }
                        if (i == 0)
                        {
                            goto getnext;
                        }
                    }
                }

                l_strMessage = p_strActionName + "  ";
                String l_strStepName = String.Empty;
                foreach (DataRow l_dtrDataRow in l_dtbDataTable.Rows)
                {
                    EntityBase l_entEntityData = EntityFactory.CreateEntity2(base.TemplateName);
                    EntityBase l_entEntityBaseData = base.EntityData;

                    l_entEntityBaseData.Clone(l_entEntityData);

                    //流程实例ID
                    l_entEntityData.ProcessID = this.ProcessID;
                    l_entEntityBaseData.ProcessID = this.ProcessID;

                    //步骤节点ID
                    l_entEntityData.WorkItemID = l_dtrDataRow["WORK_ITEM_ID"].ToString();
                    l_entEntityBaseData.WorkItemID = l_dtrDataRow["WORK_ITEM_ID"].ToString();

                    //步骤名称
                    l_entEntityData.StepName = l_dtrDataRow["NAME"].ToString();
                    l_entEntityBaseData.StepName = l_dtrDataRow["NAME"].ToString();

                    //步骤状态
                    l_entEntityData.D_StepStatus = l_dtrDataRow["STATUS"].ToString();
                    l_entEntityBaseData.D_StepStatus = l_dtrDataRow["STATUS"].ToString();

                    //接收时间
                    l_entEntityData.ReceiveDateTime = Convert.ToDateTime(l_dtrDataRow["CREATED_DATE"]);
                    l_entEntityBaseData.ReceiveDateTime = Convert.ToDateTime(l_dtrDataRow["CREATED_DATE"]);

                    //接收人账号
                    l_entEntityData.ReceiveUserID = l_dtrDataRow["USER_ID"].ToString();
                    l_entEntityBaseData.ReceiveUserID = l_dtrDataRow["USER_ID"].ToString();

                    //接收人姓名
                    l_objUser = OAUser.GetUser(l_entEntityData.ReceiveUserID);
                    if (l_objUser != null)
                    {
                        l_entEntityData.ReceiveUserName = l_objUser.Name;
                        l_entEntityBaseData.ReceiveUserName = l_objUser.Name;
                    }

                    //公办ID
                    l_entEntityData.CommonID = l_dtrDataRow["POOL_ID"].ToString();
                    l_entEntityBaseData.CommonID = l_dtrDataRow["POOL_ID"].ToString();

                    //提交动作
                    l_entEntityData.SubmitAction = null;
                    l_entEntityBaseData.SubmitAction = null;

                    //代理人信息
                    l_entEntityData.AgentUserID = String.Empty;
                    l_entEntityData.AgentUserName = String.Empty;

                    l_entEntityBaseData.AgentUserID = String.Empty;
                    l_entEntityBaseData.AgentUserName = String.Empty;

                    l_entEntityData.IsFormSave = false;
                    l_entEntityBaseData.IsFormSave = false;

                    if (base.TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE || base.TemplateName == ProcessConstString.TemplateName.LETTER_RECEIVE)
                    {
                        l_entEntityData.CommentList.Clear();
                        l_entEntityBaseData.CommentList.Clear();
                    }

                    l_entEntityData.FormsData = XmlUtility.SerializeXml(l_entEntityBaseData);

                    if (l_strStepName != l_entEntityData.StepName)
                    {
                        l_strStepName = l_entEntityData.StepName;
                        if (l_strMessage != p_strActionName + "  ")
                        {
                            l_strMessage += "待处理！";
                        }
                        l_strMessage += "\\n[" + l_strStepName + "]:";
                    }

                    if (!String.IsNullOrEmpty(l_entEntityData.ReceiveUserName))
                    {
                        l_strMessage += l_entEntityData.ReceiveUserName + "  ";
                    }
                    else
                    {
                        l_strMessage += l_entEntityData.ReceiveUserID + "  ";
                    }

                    if ((B_FormsData.AscertainSameRecord(base.ProcessID, l_entEntityData.WorkItemID, base.TemplateName)))
                    {
                        continue;
                    }

                    if (!l_entEntityData.Save())
                    {
                        String l_strErrorMessage = SysString.GetErrorMsgs(l_entEntityData.ErrMsgs);
                        ILogger log = LoggerFactory.GetLogger(LogType.TxtFile, @"Log", this.TemplateName);
                        log.WriteLog("保存待办节点出错:" + l_strErrorMessage);
                    }
                }
                if (l_strMessage == p_strActionName + "  ")
                {
                    l_strMessage = "完成！";
                }
                else
                {
                    l_strMessage += "待处理！";
                }
                this.ShowMsgBox(this.Page, MsgType.VbInformation, l_strMessage, base.EntryAction);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex.Message);
                l_strMessage = ex.Message;
                this.ShowMsgBox(this.Page, MsgType.VbInformation, l_strMessage, base.EntryAction);
            }
        }

        #region 程序文件 交办动作处理
        /// <summary>
        /// 程序文件 交办动作处理 
        /// </summary>
        /// <param name="p_strActionName"></param>
        /// <param name="entity"></param>
        protected ReturnInfo FormSubmitForProFile(Boolean bIsSave, String strActionName, String strParentTbID, String strStepStatus, EntityBase entity)
        {
            User l_objUser = null;
            ReturnInfo l_objReturnInfo = FormSave.SaveEntity(entity, this, bIsSave, this.SubAction, this.TemplateName);

            if (!l_objReturnInfo.IsSucess)
            {
                this.ShowMsgBox(this.Page, MsgType.VbExclamation, "表单保存出错,后续操作不能进行！" + l_objReturnInfo.ErrMessage);
                return l_objReturnInfo;
            }

            if (!bIsSave)
            {
                //处理交办人相关信息
                if (String.IsNullOrEmpty(strParentTbID))
                {
                    DataTable dt = B_PF.GetNextWorkItemIdForAssign(base.ProcessID);//当前ProcessID 父流程ID
                    EntityBase entitybase = base.EntityData != null ? base.EntityData : new EntityBase();
                    entitybase.AgentUserID = String.Empty;
                    entitybase.AgentUserName = String.Empty;
                    EntityBase entitybase2 = EntityFactory.CreateEntity2(base.TemplateName); ;
                    base.EntityData.Clone(entitybase2);
                    entitybase2.AgentUserID = String.Empty;
                    entitybase2.AgentUserName = String.Empty;

                    entitybase2.ProcessID = dt.Rows[0]["PROC_INST_ID"].ToString();
                    entitybase.ProcessID = dt.Rows[0]["PROC_INST_ID"].ToString();

                    entitybase2.WorkItemID = dt.Rows[0]["WORK_ITEM_ID"].ToString();
                    entitybase.WorkItemID = dt.Rows[0]["WORK_ITEM_ID"].ToString();

                    entitybase2.D_StepStatus = dt.Rows[0]["STATUS"].ToString();
                    entitybase.D_StepStatus = dt.Rows[0]["STATUS"].ToString();

                    entitybase2.ReceiveUserID = dt.Rows[0]["USER_ID"].ToString();
                    entitybase.ReceiveUserID = dt.Rows[0]["USER_ID"].ToString();

                    entitybase2.StepName = dt.Rows[0]["NAME"].ToString();
                    entitybase.StepName = dt.Rows[0]["NAME"].ToString();

                    entitybase2.SubmitAction = strActionName;
                    entitybase.SubmitAction = strActionName;

                    //接收人姓名
                    l_objUser = OAUser.GetUser(entitybase.ReceiveUserID);
                    if (l_objUser != null)
                    {
                        entitybase2.ReceiveUserName = l_objUser.Name;
                        entitybase.ReceiveUserName = l_objUser.Name;
                    }

                    entitybase2.FormsData = XmlUtility.SerializeXml(entitybase);
                    if (!entitybase2.Save())
                    {
                        l_objReturnInfo.IsSucess = false;
                        l_objReturnInfo.ErrMessage = SysString.GetErrorMsgs(entitybase2.ErrMsgs);

                        ILogger log = LoggerFactory.GetLogger(LogType.TxtFile, @"Log", "程序文件");
                        log.WriteLog("保存实体出错:" + l_objReturnInfo.ErrMessage);

                        return l_objReturnInfo;
                    }
                }//新增协助会签节点数据
                else
                {
                    B_PF pfEntity = new B_PF();
                    pfEntity.ID = int.Parse(strParentTbID);
                    pfEntity = XmlUtility.DeSerializeXml<B_PF>(pfEntity.FormsData);
                    pfEntity.ID = int.Parse(strParentTbID);
                    //清空子流程ProcessID,用于标识当部门领导自己提交表单撤销子流程（如果不为空，则需要注销子流程）
                    pfEntity.ChildProcessID = String.Empty;
                    pfEntity.AssistContent = (entity as B_PF).AssistContent;
                    pfEntity.FormsData = XmlUtility.SerializeXml(pfEntity);//.Replace("<ID>" + pfEntity.ID + "</ID>", "<ID>" + int.MinValue.ToString() + "</ID>");
                    if (!pfEntity.Save())
                    {
                        throw new Exception(pfEntity.ErrMsgs.ToString());
                    }
                    WorkFlowBase.CompleteWorkItem(base.WorkItemID, null);
                }//更新交办会签节点数据
            }

            l_objReturnInfo.IsSucess = true;
            return l_objReturnInfo;
        }
        #endregion

        /// <summary>
        /// 撤消表单
        /// </summary>
        /// <param name="p_blnIsInProcess">true提交 false保存</param>
        /// <param name="p_strActionName">提交动作</param>
        /// <param name="p_htAttributes">流程数据Hashtable</param>
        protected void FormCancel(EntityBase entity)
        {
            String l_strMessage = "";
            try
            {
                //更新流程状态为完成。
                String strformdada = XmlUtility.SerializeXml(entity);
                UpdateDBWorkItem(this.WorkItemID, false, this.SubAction, strformdada, "Completed", "", false);
                //流程操作
                WorkFlowBase.CancelProcess(this.ProcessID);
                l_strMessage = "撤销成功！";

            }
            catch (Exception ex)
            {
                ILogger log = LoggerFactory.GetLogger(LogType.TxtFile, @"Log", this.TemplateName);
                log.WriteLog(ex.Message);
                l_strMessage = ex.Message;
            }
            this.ShowMsgBox(this.Page, MsgType.VbInformation, l_strMessage, base.EntryAction);
        }
        #endregion

        #region 数据保存

        protected ReturnInfo SaveNewEntity(String ActionName, EntityBase entity)
        {
            String strformdada = XmlUtility.SerializeXml(entity);
            entity.FormsData = strformdada;
            return FormSave.SaveNewEntity(entity, this, ActionName);
        }
        #endregion

        #region 流程方法
        //更新流程第一个节点的WorkItemID
        private void UpdateDBWorkItem(String wid, Boolean isAddWorkItemID, String subaction, String strform, String stepstatus, String step, Boolean isChangStep)
        {
            B_FormsData l_busFormsData = new B_FormsData();
            l_busFormsData.UpdateWorkItemId(this.WorkItemTable, this.ProcessID, wid, isAddWorkItemID, subaction, strform, stepstatus, step, isChangStep);
        }

        private void DoWorkItem(Hashtable p_nvAttributes, Boolean p_blnIsProcessCreated)
        {
            B_FormsData l_busFormsData = null;
            try
            {
                if (!p_blnIsProcessCreated)
                {
                    //创建流程
                    WorkFlowBase.CreateProcess(this.TemplateName, this.ProcessID, p_nvAttributes);

                    //获取第一个节点并且完成。
                    l_busFormsData = new B_FormsData();
                    this.WorkItemID = l_busFormsData.GetFirstWorkItemID(this.ProcessID);

                    UpdateDBWorkItem(this.WorkItemID, true, "", "", "", this.StepName, true);

                    if (base.TemplateName.Contains("收文"))
                    {
                        WorkFlowBase.CompleteWorkItem(this.WorkItemID, p_nvAttributes);//完成第一个节点
                    }
                    if (base.TemplateName.Contains("会务费用报销单") || base.TemplateName.Contains("招待费报销单"))
                    {
                        WorkFlowBase.CompleteWorkItem(this.WorkItemID, p_nvAttributes);//完成第一个节点
                    }
                }
                else
                {
                    WorkFlowBase.CompleteWorkItem(this.WorkItemID, p_nvAttributes);//完成当前节点

                    if (base.TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE &&
                       (base.StepName == ProcessConstString.StepName.ProgramFile.STEP_DEPTSIGN ||
                        base.StepName == ProcessConstString.StepName.ProgramFile.STEP_LEADERSIGN))
                    {
                        EntityBase entitybase = ControlToEntity(false);

                        B_PF pfEntity = entitybase != null ? entitybase as B_PF : new B_PF();
                        List<String> strIDs = B_FormsData.GetMultiParapllelInfo(TableName.WorkItemsTableName.T_OA_HN_PF_WorkItems, base.ProcessID, base.StepName, pfEntity.TimesFlag);
                        foreach (String strID in strIDs)
                        {
                            B_PF entity = new B_PF();
                            //更新其它会签记录
                            entity.ID = Convert.ToInt32(strID);
                            entity = XmlUtility.DeSerializeXml<B_PF>(entity.FormsData);
                            entity.ID = Convert.ToInt32(strID);
                            entity.LeaderSignList = pfEntity.LeaderSignList;//领导已会签意见内容
                            entity.DeptSignList = B_PF.GetDeptSignList(entity.DeptSignList, strIDs);//部门已会签意见内容
                            entity.IsSignReject = pfEntity.IsSignReject;//是否否决

                            if (entity.WorkItemID == pfEntity.WorkItemID)
                            {
                                entity.D_StepStatus = "Completed";
                                entity.SubmitAction = this.SubAction;
                                entity.CommentList = pfEntity.CommentList;
                                //entity.DeptSignList = B_PF.GetDeptSignList(entity.DeptSignList, strIDs);
                            }//更新当前节点用户
                            entity.FormsData = XmlUtility.SerializeXml(entity);
                            if (!entity.Save())
                            {
                                throw new Exception(entity.ErrMsgs.ToString());
                            }
                        }
                        pfEntity.ID = int.MinValue;//清空ID
                    }

                    if (base.TemplateName == ProcessConstString.TemplateName.AFFILIATION &&
                       (base.StepName == ProcessConstString.StepName.WorkRelationStepName.STEP_DEPTSIGN))
                    {
                        EntityBase entitybase = ControlToEntity(false);

                        B_WorkRelation wrEntity = entitybase != null ? entitybase as B_WorkRelation : new B_WorkRelation();
                        List<String> strIDs = B_FormsData.GetMultiParapllelInfo(TableName.WorkItemsTableName.T_OA_WR_WorkItems, base.ProcessID, base.StepName, wrEntity.TimesFlag);
                        foreach (String strID in strIDs)
                        {
                            B_WorkRelation entity = new B_WorkRelation();
                            //更新其它会签记录
                            entity.ID = Convert.ToInt32(strID);
                            entity = XmlUtility.DeSerializeXml<B_WorkRelation>(entity.FormsData);
                            entity.ID = Convert.ToInt32(strID);
                            entity.DeptSignList = B_WorkRelation.GetDeptSignList(entity.DeptSignList, strIDs);//部门已会签意见内容
                            entity.IsSignReject = wrEntity.IsSignReject;//是否否决

                            if (entity.WorkItemID == wrEntity.WorkItemID)
                            {
                                if (base.SubAction == ProcessConstString.SubmitAction.ACTION_SUBMIT)
                                {
                                    entity.D_StepStatus = "Completed";
                                }
                                entity.SubmitAction = this.SubAction;
                                entity.CommentList = wrEntity.CommentList;
                            }//更新当前节点用户
                            entity.FormsData = XmlUtility.SerializeXml(entity);
                            if (!entity.Save())
                            {
                                throw new Exception(entity.ErrMsgs.ToString());
                            }
                        }
                        wrEntity.ID = int.MinValue;//清空ID
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //protected Boolean IsAllSubProcessCompleted()
        //{
        //    Boolean l_blnIsAllCompleted = true;
        //    B_ProcessInstance l_busProcessInstance = new B_ProcessInstance();
        //    DataTable l_dtbDataTable = l_busProcessInstance.GetSubProcess(this.ProcessID, String.Empty);

        //    if (l_dtbDataTable == null)
        //    {
        //        throw new Exception("获取子流程出错");
        //    }

        //    foreach (DataRow l_dtrDataRow in l_dtbDataTable.Rows)
        //    {
        //        if (l_dtrDataRow["PROC_INST_ID"].ToString() == this.SubProcessID)
        //        {
        //            continue;
        //        }
        //        if (l_dtrDataRow["STATUS"].ToString() == ProcessConstString.ProcessStatus.STATUS_RUNNING)
        //        {
        //            l_blnIsAllCompleted = false;
        //            break;
        //        }
        //    }
        //    return l_blnIsAllCompleted;
        //}

        /// <summary>
        /// 创建子流程
        /// huang qi add
        /// </summary>
        /// <param name="strSubTemplateName">子流程模板名称</param>
        /// <param name="nValues">流程参数</param>
        /// <returns>ProcInstID</returns>
        protected String GetCreateSubProcessID(String strSubTemplateName, Hashtable nValues)
        {
            FSProcessInstance objProcessInstance = null;
            String l_strPIID = UUID.GetID();
            objProcessInstance = WorkFlowBase.CreateProcess(strSubTemplateName, l_strPIID, this.ProcessID, nValues);
            return objProcessInstance == null ? String.Empty : objProcessInstance.ProcInstID;
        }

        #endregion

        #region 其他方法
        /// <summary>
        /// 传阅部门和人员的方法
        /// </summary>
        /// <param name="p_strDeptIDS">部门ID</param>
        /// <param name="CType">传阅方式（0.所有人1.职位大于副处加负责人和部门领导2.根据角色 3.部门领导）</param>
        /// <param name="p_strRoleName">传阅的角色</param>
        /// <param name="p_strUserIDS">传阅人员ID</param>
        /// <param name="Type">1.被抄送1(表单上第一次抄送) 2.被抄送2(表单上追加分发) 3.继续传阅  4. 二次分发</param>
        /// <param name="p_blnIsAssign">是否重发true是，false否</param>
        /// <param name="p_strComment">意见</param>
        /// <param name="p_blnIsReDirect">是否跳转</param>
        /// <returns></returns>
        public void Circulate(String p_strDeptIDS, String CType, String p_strRoleName, String p_strUserIDS, String Type, Boolean p_blnIsAssign, String p_strComment, Boolean p_blnIsReDirect)
        {
            B_ToCirculate l_busCirculate = new B_ToCirculate();
            l_busCirculate.ToProcessID = this.ProcessID;
            l_busCirculate.ToWorkItemID = this.WorkItemID;
            l_busCirculate.YiJian = p_strComment;
            l_busCirculate.IsAgain = p_blnIsAssign;
            l_busCirculate.ToProcessType = this.TemplateName;
            l_busCirculate.Type = Type;
            l_busCirculate.ToUserIDS = p_strUserIDS;
            l_busCirculate.ToDeptIDS = p_strDeptIDS;
            switch (CType)
            {
                case "0":
                    l_busCirculate.ToCType = FS.ADIM.OA.BLL.Busi.Process.B_ToCirculate.CirculateType.所有人;
                    break;
                case "1":
                    l_busCirculate.ToCType = FS.ADIM.OA.BLL.Busi.Process.B_ToCirculate.CirculateType.职位大于副科加负责人和部门领导;
                    break;
                case "2":
                    l_busCirculate.ToCType = FS.ADIM.OA.BLL.Busi.Process.B_ToCirculate.CirculateType.根据角色;
                    l_busCirculate.ToRoleName = p_strRoleName;
                    break;
                case "3":
                    l_busCirculate.ToCType = FS.ADIM.OA.BLL.Busi.Process.B_ToCirculate.CirculateType.部门领导;
                    break;
            }

            String info = l_busCirculate.ChuanYueToDB();

            if (p_blnIsReDirect)
            {
                this.ShowMsgBox(this.Page, MsgType.VbInformation, info, base.EntryAction);
            }
            else
            {
                JScript.Alert(info, true);
            }
        }

        /// <summary>
        /// 弹出消息(不用)
        /// </summary>
        /// <param name="p_objPage">Page</param>
        /// <param name="p_enmMessageType">消息类型</param>
        /// <param name="p_strMessageContent">消息内容</param>
        public void ShowMsgBox(Page p_objPage, MsgType p_enmMessageType, String p_strMessageContent)
        {
            StringBuilder l_objStringBuilder = null;
            if (!String.IsNullOrEmpty(p_strMessageContent))
            {
                l_objStringBuilder = new StringBuilder();
                l_objStringBuilder.Append("<script language=\"VBScript\">Call MsgBox(");
                l_objStringBuilder.AppendFormat("\"{0}\"", p_strMessageContent);
                l_objStringBuilder.Append(",vbOKOnly");

                switch (p_enmMessageType)
                {
                    case MsgType.VbCritical:
                        l_objStringBuilder.Append("+vbCritical");
                        break;
                    case MsgType.VbQuestion:
                        l_objStringBuilder.Append("+vbQuestion");
                        break;
                    case MsgType.VbExclamation:
                        l_objStringBuilder.Append("+vbExclamation");
                        break;
                    case MsgType.VbInformation:
                        l_objStringBuilder.Append("+vbInformation");
                        break;
                    default:
                        l_objStringBuilder.Append("+vbInformation");
                        break;
                }
                l_objStringBuilder.Append(" )</script>");

                if (TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE)
                {
                    ScriptManager.RegisterStartupScript(p_objPage, p_objPage.GetType(), "callMsg", "alert('" + p_strMessageContent + "')", true);
                }
                else
                {
                    ClientScriptManager l_objClientScript = p_objPage.ClientScript;
                    l_objClientScript.RegisterClientScriptBlock(p_objPage.GetType(), "callMsg", l_objStringBuilder.ToString(), false);
                }
                this.Alertback();
            }
        }

        /// <summary>
        /// 弹出消息并跳转
        /// </summary>
        /// <param name="p_objPage">Page</param>
        /// <param name="p_enmMessageType">消息类型</param>
        /// <param name="p_strMessageContent">消息内容</param>
        /// <param name="ms">页面入口</param>
        public void ShowMsgBox(Page p_objPage, MsgType p_enmMessageType, String p_strMessageContent, String ms)
        {
            StringBuilder l_objStringBuilder = null;
            if (!String.IsNullOrEmpty(p_strMessageContent))
            {
                l_objStringBuilder = new StringBuilder();
                l_objStringBuilder.AppendFormat(" location.href='{0}';window.parent.left.location.href='Left.aspx';", this.GetRedirectPagePath(ms));
                if (TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE)
                {
                    JScript.AlertInfo(p_strMessageContent, l_objStringBuilder.ToString(), true);
                }
                else if (TemplateName == ProcessConstString.TemplateName.FinanceCCBX_APPLY)
                {
                    JScript.AlertInfo(p_strMessageContent, l_objStringBuilder.ToString(), true);
                }
                else
                {
                    JScript.Alert(p_strMessageContent);
                    ResponseScript(p_objPage, l_objStringBuilder.ToString());
                }
            }
        }

        /// <summary>
        /// 返回上一页面
        /// </summary>
        public void Alertback()
        {
            Page PageCurrent = (Page)System.Web.HttpContext.Current.Handler;
            String script = "history.go(-1);";
            ScriptManager.RegisterClientScriptBlock(PageCurrent, PageCurrent.GetType(), DateTime.Now.Ticks.ToString(), script, true);
        }

        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="uc">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public void ResponseScript(System.Web.UI.Page page, String script)
        {
            String strScript = "<script language='javascript'>" + script + "</script>";

            if (TemplateName == ProcessConstString.TemplateName.PROGRAM_FILE)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "message", strScript, false);
            }
            else
            {
                ClientScriptManager CScript = page.ClientScript;
                CScript.RegisterClientScriptBlock(page.GetType(), "MyScript", strScript);
            }
        }

        /// <summary>
        /// 获取跳转页面的路径
        /// </summary>
        /// <param name="ms"></param>
        public String GetRedirectPagePath(String ms)
        {
            String url = "Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkflowMenu.";

            //1 待办 2已处理文件 3已处理公办

            if (ms != "")
            {
                if (ms == "3")
                {
                    url += "Circulate.PG_WaitReading";
                }
                else if (ms == "2")
                {
                    url += "ToDoTask.PG_CommonWaitHandle";
                }
                else if (ms == "7")
                {
                    url += "ToDoTask.PG_DraftList";
                }
                else
                {
                    url += "ToDoTask.PG_WaitHandle";
                }
            }
            else
            {
                url += "CompleteFiles.PG_CompletedHandle";
            }
            return url;
        }
        #endregion

        /// <summary>
        /// 功能描述:初始化打印参数
        /// 作者:王晓
        /// 时间:2010-3-29
        /// E-MAIL:wang_xiao@founder.com
        /// </summary>
        /// <param name="sProcName">OA工作流程名称</param>
        /// <param name="sStepName">OA工作流步骤名称</param>
        public virtual void InitPrint(FS.ADIM.OA.WebUI.PageWF.UC_Print print, string sProcName, string sStepName, string sStartTime, string sEndTime)
        { }

        /// <summary>
        /// 将错误信息写入日志（临时方法）
        /// </summary>
        /// <param name="strMessage">错误信息</param>
        protected void WriteLog(string strErrorMessage)//renjinquan+
        {
            ILogger log = LoggerFactory.GetLogger(LogType.TxtFile, @"Log", this.TemplateName);
            log.WriteLog(strErrorMessage);
        }

        /// <summary>
        /// 在流程归档之后执行，写入以归档的流程表
        /// </summary>
        /// <param name="str_ProcessID">流程ID</param>
        /// <param name="str_ProcessTemp">流程模板名称</param>
        protected void Devolved(string str_ProcessID, string str_ProcessTemp)
        {
            if (!base.IsDevolve)
            {
                B_ProcessInstance.ProcessDevolve(str_ProcessID, str_ProcessTemp);
            }
            base.IsDevolve = true;
            this.SetControlStatus();
        }
    }
}