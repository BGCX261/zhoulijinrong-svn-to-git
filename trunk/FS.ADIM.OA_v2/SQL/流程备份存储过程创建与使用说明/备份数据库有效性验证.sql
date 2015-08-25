--��APDataȫ���滻ΪAgilePoint���ݿ�
--��DB_Adimȫ���滻ΪADIM���ݿ�
USE DB_Adim
IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'P_ADIM_BACKUP_Check' AND type = 'P')
   DROP PROCEDURE P_ADIM_BACKUP_Check
GO
CREATE PROC [dbo].[P_ADIM_BACKUP_Check]  
@ADIM_DB_NAME varchar(50),
@AP_DB_NAME varchar(50),
@YEAR int
AS
IF ISNULL(@ADIM_DB_NAME,'')='' 
BEGIN 
	print '������Ҫ��֤��ADIM���ݿ�' 
	return
END
IF ISNULL(@AP_DB_NAME,'')='' 
BEGIN 
	print '������Ҫ��֤��AP���ݿ�' 
	return
END
BEGIN TRANSACTION
declare @SQL varchar(max)
--��ѯ��֤ʹ�õ�����
SELECT PROC_INST_ID INTO #OA_BAK_PROCESS FROM( 
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and year(LAST_RUNNING_END_TIME)=@YEAR
union all
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where SUPER_PROC_INST_ID in (select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and  year(LAST_RUNNING_END_TIME)=@YEAR))a

set @SQL='
if (select count(0) from T_OA_GS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GS_WorkItems_BAK)
begin
print ''��˾����T_OA_GS_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_GF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GF_WorkItems_BAK)
begin
print ''��˾����T_OA_GF_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HS_WorkItems_BAK)
begin
print ''��������T_OA_HS_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HF_WorkItems_BAK)
begin
print ''��������T_OA_HF_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_DJGTF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_DJGTF_WorkItems_BAK)
begin
print ''���͹��ŷ���T_OA_DJGTF_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_MS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_MS_WorkItems_BAK)
begin
print ''���͹�������T_OA_MS_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_WR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_WR_WorkItems_BAK)
begin
print ''������ϵ��T_OA_WR_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_RR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_RR_WorkItems_BAK)
begin
print ''��ʾ����T_OA_RR_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HN_PF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HN_PF_WorkItems_BAK)
begin
print ''�����ļ�T_OA_HN_PF_WorkItems_BAK���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_GF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GF_Circulate)
begin
print ''��˾���Ĵ���T_OA_GF_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_GS_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GS_Circulate)
begin
print ''��˾���Ĵ���T_OA_GS_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HF_Circulate)
begin
print ''�������Ĵ���T_OA_HF_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HN_PF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HN_PF_Circulate)
begin
print ''�����ļ�����T_OA_HN_PF_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HS_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HS_Circulate)
begin
print ''�������Ĵ���T_OA_HS_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_RR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_RR_Circulate)
begin
print ''��ʾ���洫��T_OA_RR_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_WR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_WR_Circulate)
begin
print ''������ϵ������T_OA_WR_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_DJGTF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_DJGTF_Circulate)
begin
print ''���͹��ŷ��Ĵ���T_OA_DJGTF_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_MS_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_MS_Circulate)
begin
print ''���͹������Ĵ���T_OA_MS_Circulate���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_HS_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HS_Edit)
begin
print ''�����ǼǱ�T_OA_HS_Edit���ݳ���,��֤ʧ��!''
end
else if (select count(0) from T_OA_Receive_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_Receive_Edit)
begin
print ''���ĵǼǱ�T_OA_Receive_Edit���ݳ���,��֤ʧ��!''
end
else if (select count(0) from WF_PROC_DEVOLVE where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.WF_PROC_DEVOLVE)
begin
print ''�鵵״̬��WF_PROC_DEVOLVE���ݳ���,��֤ʧ��!''
end
else if (select count(0) from WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.WF_PROC_INSTS)
begin
print ''ADIM���ݿ�WF_PROC_INSTS���ݳ���,��֤ʧ��!''
end
else if (select count(0) from WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.WF_MANUAL_WORKITEMS)
begin
print ''ADIM���ݿ�WF_MANUAL_WORKITEMS���ݳ���,��֤ʧ��!''
end
else if (select count(0) from APData.dbo.WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@AP_DB_NAME+'.dbo.WF_MANUAL_WORKITEMS)
begin
print ''AP���ݿ�WF_MANUAL_WORKITEMS���ݳ���,��֤ʧ��!''
end
else if (select count(0) from APData.dbo.WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@AP_DB_NAME+'.dbo.WF_PROC_INSTS)
begin
print ''AP���ݿ�WF_PROC_INSTS���ݳ���,��֤ʧ��!''
end
else
begin
print ''��֤�ɹ�!''
end'
exec (@SQL)
IF(@@ERROR<>0)    
BEGIN
ROLLBACK   TRANSACTION  
print '��֤ʧ��!'
return  
END 
ELSE 
BEGIN
COMMIT TRANSACTION 
END 