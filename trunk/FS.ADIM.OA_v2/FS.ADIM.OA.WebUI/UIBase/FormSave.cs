using System;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using System.Collections;
using System.Data;
using System.Text;
using FS.OA.Framework;
using FS.ADIM.OA.BLL;
using FS.OA.Framework.Logging;

namespace FS.ADIM.OA.WebUI.UIBase
{

    public class FormSave
    {
        /// <summary>
        /// 同步所有流程表单ID
        /// </summary>
        public static int SetID()
        {
            try
            {
                String[] tables = TableName.GetAllProcessTemplateName();
                StringBuilder strsqlall = new StringBuilder();
                String strsql = "";
                String ID = "";
                int ret = 0;
                foreach (String item in tables)
                {
                    String entityName = TableName.GetEntityName(item);
                    strsql = "select ID from " + TableName.GetWorkItemsTableName(item) + " where D_StepStatus in ('Assigned','New')";
                    DataTable dt = SQLHelper.GetDataTable1(strsql);
                    String tabname = TableName.GetWorkItemsTableName(item);
                    if (dt != null)
                    {
                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            i++;
                            ID = dr[0].ToString();
                            if (ID != "")
                            {
                                strsqlall.AppendFormat(@" update {0} set FormsData.modify('replace value of(/{1}/ID/text())[1] with {2}') where ID={2}", tabname, entityName, ID);
                            }
                            if (i % 100 == 0)
                            {
                                ret += SQLHelper.ExecuteNonQuery1(strsqlall.ToString());
                                strsqlall = new StringBuilder();
                            }
                        }
                        if (strsqlall.Length > 0)
                            ret += SQLHelper.ExecuteNonQuery1(strsqlall.ToString());
                        //FounderSoftware.Framework.Business.Entity.RunNoQuery(strsqlall.ToString());
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  保存实体
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="UI"></param>
        /// <param name="IsSave">是否保存true保存</param>
        /// <param name="ActionName"></param>
        public static ReturnInfo SaveEntity(EntityBase p_entEntityData, FormsUIBase p_objFormUIBase, Boolean p_blnIsSaveDraft, String p_strActionName, String p_strTemplateName)
        {
            ReturnInfo l_objReturnInfo = new ReturnInfo();

            EntityBase l_entArchive = p_entEntityData;

            //当流程类型为“程序文件”且为协助会签步骤，取SubProcessID
            if (p_strTemplateName == ProcessConstString.TemplateName.PROGRAM_FILE &&
                l_entArchive.StepName == ProcessConstString.StepName.ProgramFile.STEP_ASSIST_SIGN)
            {
                l_entArchive.ProcessID = p_objFormUIBase.SubProcessID;
            }
            else
            {
                l_entArchive.ProcessID = p_objFormUIBase.ProcessID;
            }

            //WorkItemID为空的情况为初始节点，流程尚未发起
            if (String.IsNullOrEmpty(p_objFormUIBase.WorkItemID))
            {
                //如果是保存草稿的状态
                if (p_blnIsSaveDraft)
                {
                    //节点名称设置为"保存",以供草稿列表里查询使用
                    l_entArchive.StepName = ProcessConstString.SubmitAction.ACTION_SAVE_DRAFT;
                }
                else
                {
                    l_entArchive.StepName = p_objFormUIBase.StepName;
                }
            }

            if (string.IsNullOrEmpty(l_entArchive.ReceiveUserID))
            {
                l_entArchive.ReceiveUserID = CurrentUserInfo.UserName;
                l_entArchive.ReceiveUserName = CurrentUserInfo.DisplayName;
                l_entArchive.ReceiveDateTime = DateTime.Now;
            }
            else if (OAConfig.GetConfig(ConstString.Config.Section.Start_WORKFLOW_AGENT, ConstString.Config.Key.IS_START) == "1") //流程代理
            {
                if (l_entArchive.ReceiveUserID != CurrentUserInfo.UserName)
                {
                    l_entArchive.AgentUserID = CurrentUserInfo.UserName;
                    l_entArchive.AgentUserName = CurrentUserInfo.DisplayName;
                }
            }

            //如果是保存草稿的状态
            if (p_blnIsSaveDraft ||
                (p_strTemplateName == ProcessConstString.TemplateName.PROGRAM_FILE &&
                p_strActionName == ProcessConstString.SubmitAction.ProgramFile.ACTION_ASSIGN))
            {
                l_entArchive.D_StepStatus = ProcessConstString.StepStatus.STATUS_ASSIGNED;
            }
            else
            {
                l_entArchive.D_StepStatus = ProcessConstString.StepStatus.STATUS_COMPLETED;
            }
            l_entArchive.WorkItemID = p_objFormUIBase.WorkItemID;
            l_entArchive.CommonID = p_objFormUIBase.CommonID;
            l_entArchive.SubmitAction = p_strActionName;
            l_entArchive.IsFormSave = p_blnIsSaveDraft;

            if (l_entArchive.DraftDate == DateTime.MinValue)
            {
                l_entArchive.DraftDate = DateTime.Now;
            }

            l_entArchive.FormsData = XmlUtility.SerializeXml(l_entArchive);

            //开始事务
            l_entArchive.EnTrans.Begin();

            if (l_entArchive.Save())
            {
                l_objReturnInfo.IsSucess = true;
                l_entArchive.EnTrans.Commit();
            }
            else
            {
                l_objReturnInfo.IsSucess = false;
                l_objReturnInfo.ErrMessage = SysString.GetErrorMsgs(l_entArchive.ErrMsgs);
                l_entArchive.EnTrans.Rollback();
                ILogger log = LoggerFactory.GetLogger(LogType.TxtFile, @"Log", p_strTemplateName);
                log.WriteLog("保存实体出错:" + l_objReturnInfo.ErrMessage);
            }
            return l_objReturnInfo;
        }

        public static ReturnInfo SaveEntity(EntityBase entity, bool isSerialize)
        {
            ReturnInfo RetInfo = new ReturnInfo();
            ArrayList ErrList = new ArrayList();

            if (isSerialize)
            {
                String strformdada = XmlUtility.SerializeXml(entity);
                entity.FormsData = strformdada;
            }
            entity.EnTrans.Begin();
            if (!entity.Save())
            {
                ErrList.AddRange(entity.ErrMsgs);
            }
            String strErr = String.Empty;
            foreach (String str in ErrList)
            {
                strErr += str + "\\n";
            }
            if (String.IsNullOrEmpty(strErr.Trim()))
            {
                //提交事务
                RetInfo.IsSucess = true;
                entity.EnTrans.Commit();
            }
            else
            {
                //实体保存出错，回滚
                RetInfo.IsSucess = false;
                RetInfo.ErrMessage = strErr.Trim();
                entity.EnTrans.Rollback();
            }
            return RetInfo;
        }

        //二次分发和追加分发调用
        public static ReturnInfo SaveNewEntity(EntityBase EntityData, FormsUIBase info, String ActionName)
        {
            ReturnInfo RetInfo = new ReturnInfo();
            ArrayList ErrList = new ArrayList();

            EntityData.ReceiveDateTime = DateTime.Now;
            EntityData.ProcessID = info.ProcessID;
            EntityData.WorkItemID = info.WorkItemID;
            EntityData.StepName = info.StepName;
            EntityData.D_StepStatus = ProcessConstString.StepStatus.STATUS_COMPLETED;
            EntityData.SubmitAction = ActionName;

            EntityData.EnTrans.Begin();
            if (!EntityData.Save())
            {
                ErrList.AddRange(EntityData.ErrMsgs);
            }
            else
            {
                info.IdentityID = EntityData.ID;
            }

            String strErr = String.Empty;
            foreach (String str in ErrList)
            {
                strErr += str + "\\n";
            }
            if (String.IsNullOrEmpty(strErr.Trim()))
            {
                //提交事务
                RetInfo.IsSucess = true;
                EntityData.EnTrans.Commit();
            }
            else
            {
                //实体保存出错，回滚
                RetInfo.IsSucess = false;
                RetInfo.ErrMessage = strErr.Trim();
                EntityData.EnTrans.Rollback();
            }
            return RetInfo;
        }
        #endregion
    }
}
