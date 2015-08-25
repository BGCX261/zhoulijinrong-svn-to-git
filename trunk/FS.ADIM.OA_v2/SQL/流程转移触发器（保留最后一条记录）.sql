IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'TR' AND NAME = 'T_WF_PROC_INSTS_UPDATE')  
DROP TRIGGER T_WF_PROC_INSTS_UPDATE
GO
create TRIGGER [dbo].[T_WF_PROC_INSTS_UPDATE]
ON [dbo].[WF_PROC_INSTS]
FOR UPDATE
AS 
IF UPDATE(STATUS)
begin
    if exists(select * from INSERTED where STATUS='Completed' or STATUS='Cancelled')
	begin
		begin transaction
		declare @defname varchar(50)
		declare @defID varchar(50)
		select top 1 @defID=PROC_INST_ID,@defname=DEF_NAME from INSERTED where STATUS='Completed' or STATUS='Cancelled'
		if (Charindex('公司发文',@defname)>0)
		begin
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
				from T_OA_GF_WorkItems 
				where ProcessID=@defID
			delete T_OA_GF_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_GF_WorkItems where ProcessID=@defID)
			update T_OA_GF_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_GF_WorkItems where ProcessID=@defID)
		end
		else if(Charindex('函件发文',@defname)>0)
		begin
			insert into T_OA_HF_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, D_StepStatus, Is_Common, CommonID, FormsData, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, DocumentNo, DocumentTitle, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, Drafter, DraftDate, UrgentDegree)
				select ID, No, ProcessID, WorkItemID, StepName, D_StepStatus, Is_Common, CommonID, FormsData, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, DocumentNo, DocumentTitle, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, Drafter, DraftDate, UrgentDegree 
				from T_OA_HF_WorkItems
				where ProcessID=@defID
			delete T_OA_HF_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_HF_WorkItems where ProcessID=@defID)
			update T_OA_HF_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_HF_WorkItems where ProcessID=@defID)
		end
		else if(Charindex('公司收文',@defname)>0)
		begin
			insert into T_OA_GS_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree)
				select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree
				from T_OA_GS_WorkItems
				where ProcessID=@defID
			delete T_OA_GS_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_GS_WorkItems where ProcessID=@defID)
			update T_OA_GS_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_GS_WorkItems where ProcessID=(select top 1 PROC_INST_ID from INSERTED where (STATUS='Completed' or STATUS='Cancelled') and DEF_NAME like '%公司收文%'))
		end
		else if(Charindex('函件收文',@defname)>0)
		begin
			insert into T_OA_HS_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, ReceiptDate, UrgentDegree, FileEncoding, CommunicationUnit, Pages, Remarks, ChuanYueLeader, AssistDept, AssistDeptName, ChuanYueDept, LeaderShip, LeaderShipName, LS_Comment, UnderTakeID, UnderTake, UnderTake_Comment, UnderTakeLeaders, ReadDeadline, NiBanRen, NiBanRenName, NiBanComment, SecondPloter, SecondPloterName, SecondPloterComment, Drafter, DraftDate, RegisterID, Prompt, ChuanYueDeptID, HJPrompt, ChuanYueLeaderID)
				select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, ReceiptDate, UrgentDegree, FileEncoding, CommunicationUnit, Pages, Remarks, ChuanYueLeader, AssistDept, AssistDeptName, ChuanYueDept, LeaderShip, LeaderShipName, LS_Comment, UnderTakeID, UnderTake, UnderTake_Comment, UnderTakeLeaders, ReadDeadline, NiBanRen, NiBanRenName, NiBanComment, SecondPloter, SecondPloterName, SecondPloterComment, Drafter, DraftDate, RegisterID, Prompt, ChuanYueDeptID, HJPrompt, ChuanYueLeaderID
				from T_OA_HS_WorkItems
				where ProcessID=@defID
			delete T_OA_HS_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_HS_WorkItems where ProcessID=@defID)
			update T_OA_HS_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_HS_WorkItems where ProcessID=@defID)	
	end
		else if(Charindex('请示报告',@defname)>0)
		begin
			insert into T_OA_RR_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], LeaderOpinion, DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, Undertake, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, SendUserID, Type, UrgentDegree, ReceiveUserRole)
				select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], LeaderOpinion, DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, Undertake, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, SendUserID, Type, UrgentDegree, ReceiveUserRole
				from T_OA_RR_WorkItems
				where ProcessID=@defID
			delete T_OA_RR_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_RR_WorkItems where ProcessID=@defID)
			update T_OA_RR_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_RR_WorkItems where ProcessID=@defID)
		end
		else if(Charindex('工作联系单',@defname)>0)
		begin
			insert into T_OA_WR_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, UrgentDegree, ReceiveUserRole)
				select ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, SubmitAction, D_StepStatus, FormsData, DocumentNo, DocumentTitle, Is_Common, CommonID, SerialID, MainSend, CopySend, Department, Number, Subject, [Content], DeptPrincipal, ConfirmDate, CheckDrafter, CheckDate, Drafter, DraftDate, UndertakeCircs, DispenseReader, DeptLeader, SectionLeader, Contractor, Message, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, UrgentDegree, ReceiveUserRole
				from T_OA_WR_WorkItems
				where ProcessID=@defID
			delete T_OA_WR_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_WR_WorkItems where ProcessID=@defID)
			update T_OA_WR_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_WR_WorkItems where ProcessID=@defID)
		end
		else if(Charindex('程序文件',@defname)>0 and Charindex('协助会签',@defname)<=0)
		begin
			insert into T_OA_HN_PF_WorkItems_BAK(ID, No, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, FormsData, ProcessID, WorkItemID, StepName, D_StepStatus, SubmitAction, Is_Common, CommonID, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, DocumentNo, DocumentTitle, Drafter, DraftDate, UrgentDegree, ProgramFileID)
				select ID, No, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, FormsData, ProcessID, WorkItemID, StepName, D_StepStatus, SubmitAction, Is_Common, CommonID, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, DocumentNo, DocumentTitle, Drafter, DraftDate, UrgentDegree, ProgramFileID
				from T_OA_HN_PF_WorkItems
				where ProcessID=@defID or ProcessID in (select PROC_INST_ID from WF_PROC_INSTS with(nolock) where SUPER_PROC_INST_ID=@defID )
			delete T_OA_HN_PF_WorkItems where (ProcessID=@defID or ProcessID in (select PROC_INST_ID from WF_PROC_INSTS with(nolock) where SUPER_PROC_INST_ID=@defID)) and ID<>(select max(ID) from T_OA_HN_PF_WorkItems where ProcessID=@defID or ProcessID in (select PROC_INST_ID from WF_PROC_INSTS with(nolock) where SUPER_PROC_INST_ID=@defID))
			update T_OA_HN_PF_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_HN_PF_WorkItems where ProcessID=@defID or ProcessID in (select PROC_INST_ID from WF_PROC_INSTS with(nolock) where SUPER_PROC_INST_ID=@defID))
		end
		else if(Charindex('党纪工团发文',@defname)>0)
		begin
			insert into T_OA_DJGTF_WorkItems_BAK(ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, D_StepStatus, Is_Common, CommonID, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, AgentUserName, AgentUserID, FormsData, SerialID, UrgentDegree, DocumentNo, Signer, SignDate, DeptSigners, LeadSigners, Verifier, VerifyDate, CheckDrafter, CheckDraftDate, HostDept, HostDeptID, Drafter, DraftDate, Assigner, AssignDate, PhoneNum, DocumentTitle, SubjectWord, MainSenders, CopySenders, SendDate, ShareCount, SheetCount, Typist, Checker, ReChecker,SendType)
				SELECT ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, D_StepStatus, Is_Common, CommonID, CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, AgentUserName, AgentUserID, FormsData, SerialID, UrgentDegree, DocumentNo, Signer, SignDate, DeptSigners, LeadSigners, Verifier, VerifyDate, CheckDrafter, CheckDraftDate, HostDept, HostDeptID, Drafter, DraftDate, Assigner, AssignDate, PhoneNum, DocumentTitle, SubjectWord, MainSenders, CopySenders, SendDate, ShareCount, SheetCount, Typist, Checker, ReChecker, SendType
				from T_OA_DJGTF_WorkItems 
				where ProcessID=@defID
			delete T_OA_DJGTF_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_DJGTF_WorkItems where ProcessID=@defID)
			update T_OA_DJGTF_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_DJGTF_WorkItems where ProcessID=@defID)
		end
		else if(Charindex('党纪工团收文',@defname)>0)
		begin
			insert into T_OA_MS_WorkItems_BAK(CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree, AgentUserName, AgentUserID, TemplateName)
				select CreateDate, CreaterID, CreaterName, EditDate, EditorID, EditorName, RecordStatus, ID, No, ProcessID, WorkItemID, StepName, ReceiveUserID, ReceiveUserName, ReceiveDateTime, ReceiveUserDept, ReceiveUserRole, SubmitAction, SubmitDate, D_StepStatus, CommonID, Is_Common, DocumentNo, DocumentTitle, SendNo, DocumentReceiveDate, VolumeNo, Officer, Officer_Comment, LeaderShip, LS_Comment, UnderTakeDept, UnderTake_Comment, Prompt, FormsData, Drafter, DraftDate, RegisterID, UrgentDegree, AgentUserName, AgentUserID, TemplateName
				from T_OA_MS_WorkItems
				where ProcessID=@defID
			delete T_OA_MS_WorkItems where ProcessID=@defID and ID<>(select max(ID) from T_OA_MS_WorkItems where ProcessID=@defID)
			update T_OA_MS_WorkItems set RecordStatus=2 where ID=(select max(ID) from T_OA_MS_WorkItems where ProcessID=@defID)
		end
		if(@@error<>0)    
			rollback   transaction     
		else 
			commit transaction 
	end
end