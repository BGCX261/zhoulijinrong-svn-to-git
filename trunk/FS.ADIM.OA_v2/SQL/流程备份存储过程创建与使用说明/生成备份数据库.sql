--��APDataȫ���滻ΪAgilePoint���ݿ�
--��DB_Adimȫ���滻ΪADIM���ݿ�
USE DB_Adim
IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'P_ADIM_BACKUP' AND type = 'P')
   DROP PROCEDURE P_ADIM_BACKUP
GO
CREATE PROC [dbo].[P_ADIM_BACKUP]  
@YEAR int
as
declare @FileName varchar(100)
declare @ADIM_DB_Name varchar(50)
declare @AP_DB_Name varchar(50)
declare @SQL varchar(max)
SEt @ADIM_DB_Name=N'OA_ADIM_BAK_'+cast(@YEAR as varchar(10))
SEt @AP_DB_Name=N'OA_AP_BAK_'+cast(@YEAR as varchar(10))

select @FileName=filename from   master.dbo.sysdatabases where name = 'DB_Adim'
set @FileName=Replace(@FileName,'DB_Adim.mdf',@ADIM_DB_Name)
--print '���ڴ������ݿ�...'
--����ADIM���ݿ�
exec master.dbo.P_Create_DB @ADIM_DB_Name,@FileName
--����AP���ݿ�
select @FileName=filename from   master.dbo.sysdatabases where name = 'DB_Adim'
set @FileName=Replace(@FileName,'DB_Adim.mdf',@AP_DB_Name)
exec master.dbo.P_Create_DB @AP_DB_Name,@FileName

--��ʼ����
BEGIN TRANSACTION
--��������ѯ������д�뼴��ִ�б��ݵ����ݿ�
--������ʱ�����潫Ҫ���ݵ�����ID
if object_id('tempdb..#OA_BAK_PROCESS') is not null
drop table #OA_BAK_PROCESS

--print '���ڲ�ѯ��Ҫ���ݵ�����...'
SELECT PROC_INST_ID INTO #OA_BAK_PROCESS FROM( 
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and year(LAST_RUNNING_END_TIME)=@YEAR
union all
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where SUPER_PROC_INST_ID in (select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and  year(LAST_RUNNING_END_TIME)=@YEAR))a

--��˾���ı�

set @SQL='if exists (select * from sysobjects where id = OBJECT_ID('''+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table '+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK from T_OA_GS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--��˾���ı�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID('''+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table '+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK from T_OA_GF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--�������ı�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID('''+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK from T_OA_HS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--�������ı�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK from T_OA_HF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--���͹��ŷ��ı�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK from T_OA_DJGTF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--���͹������ı�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK from T_OA_MS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--������ϵ��
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK from T_OA_WR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--��ʾ����
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK from T_OA_RR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--�����ļ�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK from T_OA_HN_PF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'


--��˾���Ĵ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate from T_OA_GF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--��˾���Ĵ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate from T_OA_GS_Circulate  where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--�������Ĵ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate from T_OA_HF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--�����ļ�����
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate from T_OA_HN_PF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--�������Ĵ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate from T_OA_HS_Circulate  where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--��ʾ���洫��
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate from T_OA_RR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--������ϵ������
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate from T_OA_WR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--���͹��ŷ��Ĵ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate from T_OA_DJGTF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--���͹������Ĵ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate from T_OA_DJGTF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'


--�������ĵǼ�
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit from T_OA_HS_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit from T_OA_Receive_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--AP�����
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS]
select * into '+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS from WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)
if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS]
select * into '+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS from WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--���̹鵵״̬
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE]
select * into '+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE from WF_PROC_DEVOLVE where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--AP���ݱ���
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@AP_DB_Name+'.dbo.WF_PROC_INSTS]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@AP_DB_Name+'.dbo.WF_PROC_INSTS]
select * into '+@AP_DB_Name+'.dbo.WF_PROC_INSTS from APData.dbo.WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)
if exists (select * from sysobjects where id = OBJECT_ID(''['+@AP_DB_Name+'.dbo.WF_MANUAL_WORKITEMS]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@AP_DB_Name+'.dbo.WF_MANUAL_WORKITEMS]
select * into '+@AP_DB_Name+'.dbo.WF_MANUAL_WORKITEMS from APData.dbo.WF_MANUAL_WORKITEMS  where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

exec (@SQL)
IF(@@ERROR<>0)    
BEGIN
ROLLBACK   TRANSACTION  
print '����ʧ��!'
return  
END 
ELSE 
BEGIN
COMMIT TRANSACTION 
print '�������!'
END 
--������֤
exec P_ADIM_BACKUP_Check @ADIM_DB_Name,@ADIM_DB_Name,@YEAR