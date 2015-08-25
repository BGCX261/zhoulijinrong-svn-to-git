--��APDataȫ���滻ΪAgilePoint���ݿ�
--��DB_Adimȫ���滻ΪADIM���ݿ�
USE DB_Adim
IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'P_ADIM_BACKUP_Delete' AND type = 'P')
   DROP PROCEDURE P_ADIM_BACKUP_Delete
GO
CREATE PROC [dbo].[P_ADIM_BACKUP_Delete]  
@ADIM_DB_Name varchar(100),
@AP_DB_Name varchar(100)
AS
IF ISNULL(@ADIM_DB_Name,'')='' 
BEGIN 
	print '��������Ϊɾ����ADIM(����)���ݿ�' 
	return
END
IF ISNULL(@AP_DB_Name,'')='' 
BEGIN 
	print '��������Ϊɾ��������AP(����)���ݿ�' 
	return
END
BEGIN TRANSACTION
declare @SQL varchar(max)
set @SQL='
delete T_OA_GS_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK group by ProcessID)
delete T_OA_GF_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK group by ProcessID)
delete T_OA_HS_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK group by ProcessID)
delete T_OA_HF_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK group by ProcessID)
delete T_OA_WR_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK group by ProcessID)
delete T_OA_RR_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK group by ProcessID)
delete T_OA_HN_PF_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK group by ProcessID)
delete T_OA_DJGTF_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK group by ProcessID)
delete T_OA_MS_WorkItems_BAK where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK group by ProcessID)

delete T_OA_GF_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate group by ProcessID)
delete T_OA_GS_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate group by ProcessID)
delete T_OA_HF_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate group by ProcessID)
delete T_OA_HS_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate group by ProcessID)
delete T_OA_WR_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate group by ProcessID)
delete T_OA_RR_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate group by ProcessID)
delete T_OA_HN_PF_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate group by ProcessID)
delete T_OA_MS_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate group by ProcessID)
delete T_OA_DJGTF_Circulate where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate group by ProcessID)

delete T_OA_HS_Edit where ID in (select ID from '+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit)
delete T_OA_Receive_Edit where ID in (select ID from '+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit)
delete WF_PROC_DEVOLVE where ProcessID in (select ProcessID from '+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE)

delete APData.dbo.WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from '+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS)
delete APData.dbo.WF_MANUAL_WORKITEMS where WORK_ITEM_ID in (select WORK_ITEM_ID from '+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS)
'
exec (@SQL)

IF(@@ERROR<>0)    
BEGIN
ROLLBACK   TRANSACTION  
print 'ɾ��ʧ��!'
return  
END 
ELSE 
BEGIN
COMMIT TRANSACTION 
print 'ɾ�����!'
END 