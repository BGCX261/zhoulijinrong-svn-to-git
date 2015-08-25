//----------------------------------------------------------------
// Copyright (C) 2009 方正软件有限公司
//
// 文件功能描述：UI基类
//
// 
// 创建标识： 2009-12-28
//
// 修改标识：2010-01-08
// 修改描述：实体从数据库抓取。
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.Entity;
using FS.OA.Framework.Logging;
using FS.ADIM.OA.BLL;

namespace FS.ADIM.OA.WebUI.UIBase
{
    public class FormBase : System.Web.UI.UserControl
    {
        #region 属性定义
        protected String m_strProcessID = String.Empty;
        protected String m_strWorkItemID = String.Empty;
        protected String m_strCommonID = String.Empty;
        protected String m_strWorkObjectID = String.Empty;
        protected String m_strEntryAction = String.Empty;
        protected String m_strTemplateName = String.Empty;
        protected String m_strStepName = String.Empty;
        protected String m_strSubProcessID = String.Empty;
        protected Boolean m_blnIsPreview = false;
        protected Int32 m_intIdentityID = 0;
        protected EntityBase m_EntityData = null;
        protected List<CYiJian> m_listComment = null;
        protected List<String> m_strAryMessages = new List<String>();
        protected String m_strWorkItemTable = String.Empty;
        protected String m_subAction = String.Empty;

        protected Boolean m_blnIsFromDraft = false;
        /// <summary>
        /// 流程实例ID
        /// </summary>
        public String ProcessID
        {
            get
            {
                return m_strProcessID;
            }
            set
            {
                m_strProcessID = value;
            }
        }

        /// <summary>
        /// 流程步骤号(每个步骤ID唯一)
        /// </summary>
        public String WorkItemID
        {
            get
            {
                return m_strWorkItemID;
            }
            set
            {
                m_strWorkItemID = value;
            }
        }

        /// <summary>
        /// 公办组ID
        /// </summary>
        public String CommonID
        {
            get
            {
                return m_strCommonID;
            }
            set
            {
                m_strCommonID = value;
            }
        }

        /// <summary>
        /// 数据库表中行标示ID 0表示发起流程
        /// </summary>
        public int IdentityID
        {
            get
            {
                return m_intIdentityID;
            }
            set
            {
                m_intIdentityID = value;
            }
        }

        /// <summary>
        /// 流程模板名称
        /// </summary>
        public String TemplateName
        {
            get
            {
                return m_strTemplateName;
            }
            set
            {
                m_strTemplateName = value;
            }
        }

        protected String RegisterID
        {
            get
            {
                if (ViewState[ConstString.ViewState.REGISTER_ID] == null)
                {
                    return String.Empty;
                }
                return Convert.ToString(ViewState[ConstString.ViewState.REGISTER_ID]);
            }
            set { ViewState[ConstString.ViewState.REGISTER_ID] = value; }
        }

        /// <summary>
        /// 流程步骤名称
        /// </summary>
        public String StepName
        {
            get
            {
                return m_strStepName;
            }
            set
            {
                m_strStepName = value;
            }
        }

        //设置自定义属性
        protected String WorkObjectID
        {
            get
            {
                return m_strWorkObjectID;
            }
            set
            {
                m_strWorkObjectID = value;
            }
        }

        /// <summary>
        ///是否为预览状态  true为历史表单
        /// </summary>
        public Boolean IsPreview
        {
            get
            {
                return m_blnIsPreview;
            }
            set
            {
                m_blnIsPreview = value;
            }
        }

        /// <summary>
        /// 从哪个菜单进入的 1.待办 2.公办 3.待阅 4.查看自己办理公办 5.查看他人办理公办 6发文维护 7草稿箱
        /// </summary>
        public String EntryAction
        {
            get
            {
                return m_strEntryAction;
            }
            set
            {
                m_strEntryAction = value;
            }
        }

        /// <summary>
        /// 表单数据实体
        /// </summary>
        public virtual EntityBase EntityData
        {
            get
            {
                B_FormsData l_busFormsData = new B_FormsData();
                EntityBase l_entEntityData = null;

                String l_strFormsData = l_busFormsData.GetFormsDataByID(this.WorkItemTable, String.IsNullOrEmpty(this.SubProcessID) ? this.ProcessID : this.SubProcessID, this.WorkItemID);

                if (String.IsNullOrEmpty(l_strFormsData))
                {
                    return null;
                }

                try
                {
                    switch (this.TemplateName)
                    {
                        case ProcessConstString.TemplateName.COMPANY_RECEIVE://公司收文 
                            l_entEntityData = XmlUtility.DeSerializeXml<B_GS_WorkItems>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.COMPANY_SEND://公司发文
                            l_entEntityData = XmlUtility.DeSerializeXml<EntitySend>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.AFFILIATION://工作联系单
                            l_entEntityData = XmlUtility.DeSerializeXml<B_WorkRelation>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.INSTUCTION_REPORT://请示报告
                            l_entEntityData = XmlUtility.DeSerializeXml<B_RequestReport>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.LETTER_RECEIVE://函件收文
                            l_entEntityData = XmlUtility.DeSerializeXml<B_LetterReceive>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.LETTER_RECEIVE_OLD://函件收文
                            l_entEntityData = XmlUtility.DeSerializeXml<B_LetterReceive>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.LETTER_SEND://函件发文
                            l_entEntityData = XmlUtility.DeSerializeXml<EntityLetterSend>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.LETTER_SEND_OLD://函件发文
                            l_entEntityData = XmlUtility.DeSerializeXml<EntityLetterSend>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.PROGRAM_FILE: //程序文件
                            l_entEntityData = XmlUtility.DeSerializeXml<B_PF>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.MERGED_RECEIVE://党纪工团收文
                            l_entEntityData = XmlUtility.DeSerializeXml<B_MergeReceiveBase>(l_strFormsData);
                            break;

                        case ProcessConstString.TemplateName.DJGT_Send://党纪工团发文
                            l_entEntityData = XmlUtility.DeSerializeXml<B_DJGTSend>(l_strFormsData);
                            break;
                        case ProcessConstString.TemplateName.FINANCE_TRIPAPPLY:
                            l_entEntityData = XmlUtility.DeSerializeXml<B_Finance>(l_strFormsData);
                            break;
                        case ProcessConstString.TemplateName.FinanceCCBX_APPLY://出差报销单
                            l_entEntityData = XmlUtility.DeSerializeXml<B_FinanceCCBX>(l_strFormsData);
                            break;
                        case ProcessConstString.TemplateName.FinanceHWBX_APPLY://会务费用报销单
                            l_entEntityData = XmlUtility.DeSerializeXml<B_FinanceHWBX>(l_strFormsData);
                            break;
                        case ProcessConstString.TemplateName.FinanceZDBX_APPLY://招待费用报销单
                            l_entEntityData = XmlUtility.DeSerializeXml<B_FinanceZDBX>(l_strFormsData);
                            break;
                        case ProcessConstString.TemplateName.FinanceJK_APPLY://借款申请单
                            l_entEntityData = XmlUtility.DeSerializeXml<B_FinanceJK>(l_strFormsData);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ILogger log = LoggerFactory.GetLogger(LogType.TxtFile, @"Log", this.TemplateName);
                    log.WriteLog(ex.Message);
                    JScript.Alert("表单内容加载发生错误，XML反序列化失败！");
                }
                return l_entEntityData;
            }
        }

        /// <summary>
        /// 意见列表
        /// </summary>
        public virtual List<CYiJian> Comments
        {
            get
            {
                if (m_listComment == null)
                {
                    m_listComment = new List<CYiJian>();
                }
                return m_listComment;
            }
            set
            {
                m_listComment = value;
            }
        }

        /// <summary>
        /// 子流程实例ID
        /// </summary>
        public String SubProcessID
        {
            get
            {
                return m_strSubProcessID;
            }
            set
            {
                m_strSubProcessID = value;
            }
        }

        /// <summary>
        /// 流程对应的节点表名称
        /// </summary>
        public String WorkItemTable
        {
            get
            {
                if (String.IsNullOrEmpty(m_strWorkItemTable))
                {
                    m_strWorkItemTable = TableName.GetWorkItemsTableName(this.TemplateName);
                }
                return m_strWorkItemTable;
            }
            set
            {
                m_strWorkItemTable = value;
            }
        }

        /// <summary>
        /// 节点动作
        /// </summary>
        public String SubAction
        {
            get
            {
                return m_subAction;
            }
            set { m_subAction = value; }

        }

        /// <summary>
        /// 新版OA启用时间,用于方便于老板OA的数据填充。
        /// </summary>
        public DateTime OAStartTime
        {
            get
            {
                try
                {
                    if (ViewState["OAStartTime"] == null)
                    {
                        ViewState["OAStartTime"] = FS.OA.Framework.OAConfig.GetConfig("新版OA启用时间", "时间");
                    }
                    return DateTime.Parse(ViewState["OAStartTime"].ToString());
                }
                catch
                {
                    JScript.Alert("请在配置文件中配置正确的[新版OA启用时间]");
                }
                return DateTime.MinValue;
            }
        }

        /// <summary>
        ///是否是草稿箱进入的
        /// </summary>
        public Boolean IsFromDraft
        {
            get
            {
                return m_blnIsFromDraft;
            }
            set
            {
                m_blnIsFromDraft = value;
            }
        }
        /// <summary>
        /// 是否已归档
        /// </summary>
        public Boolean IsDevolve
        {
            get
            {
                if (ViewState[ConstString.ViewState.IS_DEVOLVE] == null)
                {
                    ViewState[ConstString.ViewState.IS_DEVOLVE] = B_ProcessInstance.Is_Devolve(this.ProcessID, this.TemplateName);
                }
                string strDevolveState = ViewState[ConstString.ViewState.IS_DEVOLVE].ToString();
                return Boolean.Parse(strDevolveState != string.Empty ? strDevolveState : false.ToString());
            }
            set
            {
                ViewState[ConstString.ViewState.IS_DEVOLVE] = value;
            }
        }

        /// <summary>
        /// 当前表单是否可以归档
        /// </summary>
        public Boolean IsCanDevolve
        {
            get
            {
                if (ViewState[ConstString.ViewState.IS_CANDEVOLVE] == null)
                {
                    ViewState[ConstString.ViewState.IS_CANDEVOLVE] = B_ProcessInstance.CanDevolve(this.StepName,this.TemplateName,CurrentUserInfo.RoleName);
                }
                string strDevolveState = ViewState[ConstString.ViewState.IS_CANDEVOLVE].ToString();
                return Boolean.Parse(strDevolveState != string.Empty ? strDevolveState : false.ToString());
            }
        }
        #endregion
    }
}
