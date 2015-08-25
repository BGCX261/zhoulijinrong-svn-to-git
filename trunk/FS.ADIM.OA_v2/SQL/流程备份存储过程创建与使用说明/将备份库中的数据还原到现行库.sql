--将APData全部替换为AgilePoint数据库
--将DB_Adim全部替换为ADIM数据库
USE DB_Adim
IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'P_ADIM_BACKUP_Restore' AND type = 'P')
   DROP PROCEDURE P_ADIM_BACKUP_Restore
GO
CREATE PROC [dbo].[P_ADIM_BACKUP_Restore]  
@ADIM_DB_Name varchar(100),
@AP_DB_Name varchar(100)
AS
IF ISNULL(@ADIM_DB_Name,'')='' 
BEGIN 
	print '请输入要导入现行数据库的ADIM数据库名称' 
	return
END
IF ISNULL(@AP_DB_Name,'')='' 
BEGIN 
	print '请输入要导入现行数据库的Ap数据库名称' 
	return
END
begin transaction
declare @SQL varchar(max)
declare @ReadCol  varchar(max)
set @ReadCol='[CreateDate],[CreaterID],[CreaterName],[EditDate],[EditorID],[EditorName],[RecordStatus],[ID],[No],[ProcessID],[WorkItemID],[SendUserID],[SendUserName],[SendDateTime],[ReceiveUserID],[ReceiveUserName],[ReceiveUserDept],[Is_Dept],[Comment],[Is_Read],[Is_Cancel],[Is_Inbox],[ParentID],[LevelCode],[Type],[Role]'
set @SQL='
insert into T_OA_GF_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, Is_Common, CommonID, FormsData, SerialID, UrgentDegree, DocumentNo, Signer, SignerName, SignDate, SignComment, DeptSigners, DeptSignComment, LeadSigners, LeadSignComment, Verifier, VerifyDate, CheckDrafter, CheckDate, HostDept, Drafter, DraftDate, PhoneNum, DocumentTitle, SubjectWord, MainSenders, CopySenders, SendDate, ShareCount, SheetCount, Typist, Checker, ReChecker, Prompt, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ReceiveUserRole)
SELECT ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, 
ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, 
D_StepStatus, Is_Common, CommonID, FormsData, SerialID, UrgentDegree, 
DocumentNo, Signer, SignerName, SignDate, SignComment, DeptSigners, 
DeptSignComment, LeadSigners, LeadSignComment, Verifier, VerifyDate,
CheckDrafter, CheckDate, HostDept, Drafter, DraftDate, PhoneNum,
DocumentTitle, SubjectWord, MainSenders, CopySenders, SendDate, 
ShareCount, SheetCount, Typist, Checker, ReChecker, Prompt, CreateDate,
CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ReceiveUserRole 
from '+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK

insert into T_OA_HF_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, D_StepStatus, Is_Common, CommonID, FormsData, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, DocumentNo, DocumentTitle, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, Drafter, DraftDate, UrgentDegree)
select ID, No, ProcessID, WorkItemID, StepName, D_StepStatus, Is_Common, CommonID, FormsData, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, DocumentNo, DocumentTitle, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, Drafter, DraftDate, UrgentDegree 
from '+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK

insert into T_OA_GS_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree)
select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree
from '+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK

insert into T_OA_HS_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, ReceiptDate, UrgentDegree, FileEncoding, CommunicationUnit, Pages, Remarks, ChuanYueLeader, AssistDept, AssistDeptName, ChuanYueDept, LeaderShip, LeaderShipName, LS_Comment, UnderTakeID, UnderTake, UnderTake_Comment, UnderTakeLeaders, ReadDeadline, NiBanRen, NiBanRenName, NiBanComment, SecondPloter, SecondPloterName, SecondPloterComment, Drafter, DraftDate, RegisterID, Prompt, ChuanYueDeptID, HJPrompt, ChuanYueLeaderID)
select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, ReceiptDate, UrgentDegree, FileEncoding, CommunicationUnit, Pages, Remarks, ChuanYueLeader, AssistDept, AssistDeptName, ChuanYueDept, LeaderShip, LeaderShipName, LS_Comment, UnderTakeID, UnderTake, UnderTake_Comment, UnderTakeLeaders, ReadDeadline, NiBanRen, NiBanRenName, NiBanComment, SecondPloter, SecondPloterName, SecondPloterComment, Drafter, DraftDate, RegisterID, Prompt, ChuanYueDeptID, HJPrompt, ChuanYueLeaderID
from '+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK

insert into T_OA_RR_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], LeaderOpinion, DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, Undertake, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, SendUserID, Type, UrgentDegree, ReceiveUserRole)
select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], LeaderOpinion, DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, Undertake, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, SendUserID, Type, UrgentDegree, ReceiveUserRole
from '+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK'

exec (@SQL)

set @SQL='
insert into T_OA_WR_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, UrgentDegree, ReceiveUserRole)
select ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, UrgentDegree, ReceiveUserRole
from '+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK

insert into T_OA_HN_PF_WorkItems_BAK(ID, No, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, FormsData, ProcessID, WorkItemID, StepName, D_StepStatus, SubmitAction, Is_Common, CommonID, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, DocumentNo, DocumentTitle, Drafter, DraftDate, UrgentDegree, ProgramFileID)
select ID, No, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, FormsData, ProcessID, WorkItemID, StepName, D_StepStatus, SubmitAction, Is_Common, CommonID, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, DocumentNo, DocumentTitle, Drafter, DraftDate, UrgentDegree, ProgramFileID
from '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK

insert into T_OA_DJGTF_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, D_StepStatus, Is_Common, CommonID, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, AgentUserName, AgentUserID, FormsData, SerialID, UrgentDegree, DocumentNo, Signer, SignDate, DeptSigners, LeadSigners, Verifier, VerifyDate, CheckDrafter, CheckDraftDate, HostDept, HostDeptID, Drafter, DraftDate, Assigner, AssignDate, PhoneNum, DocumentTitle, SubjectWord, MainSenders, CopySenders, SendDate, ShareCount, SheetCount, Typist, Checker, ReChecker,SendType)
SELECT ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, D_StepStatus, Is_Common, CommonID, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, AgentUserName, AgentUserID, FormsData, SerialID, UrgentDegree, DocumentNo, Signer, SignDate, DeptSigners, LeadSigners, Verifier, VerifyDate, CheckDrafter, CheckDraftDate, HostDept, HostDeptID, Drafter, DraftDate, Assigner, AssignDate, PhoneNum, DocumentTitle, SubjectWord, MainSenders, CopySenders, SendDate, ShareCount, SheetCount, Typist, Checker, ReChecker, SendType
from '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK

insert into T_OA_MS_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree, AgentUserName, AgentUserID, TemplateName)
select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree, AgentUserName, AgentUserID, TemplateName
from '+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK'

exec (@SQL)

set @SQL='
insert into T_OA_GF_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate

insert into T_OA_GS_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate

insert into T_OA_HF_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate

insert into T_OA_HS_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate

insert into T_OA_WR_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate

insert into T_OA_RR_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate

insert into T_OA_HN_PF_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate

insert into T_OA_MS_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate

insert into T_OA_DJGTF_Circulate('+@ReadCol+')
select '+@ReadCol+'
from '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate'


exec (@SQL)

set @SQL='
insert into T_OA_HS_Edit([ID],[No],[ProcessName],[ProcessID],[ReplyDocumentNo],[DocumentNo],[DocumentTitle],[FileEncoding],[OtherEncoding],[LetterType],[CommunicationUnit],[FormationDate],[ReceiptDate],[Pages],[ContractNumber],[EquipmentCode],[UrgentDegree],[Remarks],[XingWenDate],[KeepTime],[HNCode],[FileData],[CreateDate],[CreaterID],[CreaterName],[EditDate],[EditorID],[EditorName],[RecordStatus])
select [ID],[No],[ProcessName],[ProcessID],[ReplyDocumentNo],[DocumentNo],[DocumentTitle],[FileEncoding],[OtherEncoding],[LetterType],[CommunicationUnit],[FormationDate],[ReceiptDate],[Pages],[ContractNumber],[EquipmentCode],[UrgentDegree],[Remarks],[XingWenDate],[KeepTime],[HNCode],[FileData],[CreateDate],[CreaterID],[CreaterName],[EditDate],[EditorID],[EditorName],[RecordStatus]
from '+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit

insert into T_OA_Receive_Edit([CreateDate],[CreaterID],[CreaterName],[EditDate],[EditorID],[EditorName],[RecordStatus],[ID],[No],[ProcessID],[ProcessName],[ReceiveUnit],[ReceiveYear],[ReceiveNo],[ReceiveDate],[SendLetterNo],[SendLetterDate],[DocumentTitle],[SubjectWord],[PageCount],[ShareCount],[AttachmentCount],[KeepTime],[SecretLevel],[UrgentDegree],[SendLetterUnit],[PreVolumeNo],[Remarks],[ArchiveStatus],[Is_Archive],[FileData])
select [CreateDate],[CreaterID],[CreaterName],[EditDate],[EditorID],[EditorName],[RecordStatus],[ID],[No],[ProcessID],[ProcessName],[ReceiveUnit],[ReceiveYear],[ReceiveNo],[ReceiveDate],[SendLetterNo],[SendLetterDate],[DocumentTitle],[SubjectWord],[PageCount],[ShareCount],[AttachmentCount],[KeepTime],[SecretLevel],[UrgentDegree],[SendLetterUnit],[PreVolumeNo],[Remarks],[ArchiveStatus],[Is_Archive],[FileData]
from '+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit


insert into WF_PROC_DEVOLVE([ProcessTemp],[ProcessID])
select [ProcessTemp],[ProcessID]
from '+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE'

exec (@SQL)

set @SQL='
insert into APData.dbo.WF_MANUAL_WORKITEMS([WORK_ITEM_ID],[NAME],[LAST_MODIFIED_BY],[ASSIGNED_DATE],[LAST_MODIFIED_DATE],[CANCELLED_DATE],[COMPLETED_DATE],[DUE_DATE],[CREATED_DATE],[SOURCE_WORK_ITEM_ID],[DUE_HANDLED],[WAIT_WORKPERFORMED],[PROC_DEF_ID],[PROC_INST_ID],[ACTIVITY_INST_ID],[STATUS],[USER_ID],[WORK_OBJECT_ID],[PENDING],[APPL_NAME],[BEING_PROCESSED],[SESSION_],[POOL_ID],[ORIGINAL_USER_ID],[CLIENT_DATA],[POOL_INFO])
select [WORK_ITEM_ID],[NAME],[LAST_MODIFIED_BY],[ASSIGNED_DATE],[LAST_MODIFIED_DATE],[CANCELLED_DATE],[COMPLETED_DATE],[DUE_DATE],[CREATED_DATE],[SOURCE_WORK_ITEM_ID],[DUE_HANDLED],[WAIT_WORKPERFORMED],[PROC_DEF_ID],[PROC_INST_ID],[ACTIVITY_INST_ID],[STATUS],[USER_ID],[WORK_OBJECT_ID],[PENDING],[APPL_NAME],[BEING_PROCESSED],[SESSION_],[POOL_ID],[ORIGINAL_USER_ID],[CLIENT_DATA],[POOL_INFO]
from '+@AP_DB_Name+'.dbo.WF_MANUAL_WORKITEMS

insert into APData.dbo.WF_PROC_INSTS([PROC_INST_ID],[PROC_INST_NAME],[DEF_ID],[DEF_NAME],[VERSION],[SUPER_PROC_INST_ID],[STATUS],[STARTED_DATE],[DUE_DATE],[COMPLETED_DATE],[WORK_OBJECT_ID],[LAST_SWAP_DATE],[LAST_MODIFIED_DATE],[LAST_MODIFIED_BY],[LAST_RUNNING_START_TIME],[LAST_RUNNING_END_TIME],[APPL_NAME],[PROC_INITIATOR],[PROC_INITIATOR_LOC],[WORK_OBJECT_INFO],[SOURCE_PROC_INST_ID],[TARGET_PROC_INST_ID])
select [PROC_INST_ID],[PROC_INST_NAME],[DEF_ID],[DEF_NAME],[VERSION],[SUPER_PROC_INST_ID],[STATUS],[STARTED_DATE],[DUE_DATE],[COMPLETED_DATE],[WORK_OBJECT_ID],[LAST_SWAP_DATE],[LAST_MODIFIED_DATE],[LAST_MODIFIED_BY],[LAST_RUNNING_START_TIME],[LAST_RUNNING_END_TIME],[APPL_NAME],[PROC_INITIATOR],[PROC_INITIATOR_LOC],[WORK_OBJECT_INFO],[SOURCE_PROC_INST_ID],[TARGET_PROC_INST_ID]
from '+@AP_DB_Name+'.dbo.WF_PROC_INSTS
'
exec (@SQL)
IF(@@ERROR<>0)    
BEGIN
ROLLBACK   TRANSACTION  
print '还原失败!'
return  
END 
ELSE 
BEGIN
COMMIT TRANSACTION 
print '还原完成!'
END 
