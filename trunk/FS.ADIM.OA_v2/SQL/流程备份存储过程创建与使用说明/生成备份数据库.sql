--将APData全部替换为AgilePoint数据库
--将DB_Adim全部替换为ADIM数据库
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
--print '正在创建数据库...'
--创建ADIM数据库
exec master.dbo.P_Create_DB @ADIM_DB_Name,@FileName
--创建AP数据库
select @FileName=filename from   master.dbo.sysdatabases where name = 'DB_Adim'
set @FileName=Replace(@FileName,'DB_Adim.mdf',@AP_DB_Name)
exec master.dbo.P_Create_DB @AP_DB_Name,@FileName

--开始事务
BEGIN TRANSACTION
--将条件查询的数据写入即将执行备份的数据库
--创建临时表，保存将要备份的流程ID
if object_id('tempdb..#OA_BAK_PROCESS') is not null
drop table #OA_BAK_PROCESS

--print '正在查询需要备份的数据...'
SELECT PROC_INST_ID INTO #OA_BAK_PROCESS FROM( 
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and year(LAST_RUNNING_END_TIME)=@YEAR
union all
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where SUPER_PROC_INST_ID in (select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and  year(LAST_RUNNING_END_TIME)=@YEAR))a

--公司收文表

set @SQL='if exists (select * from sysobjects where id = OBJECT_ID('''+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table '+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GS_WorkItems_BAK from T_OA_GS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--公司发文表
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID('''+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table '+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GF_WorkItems_BAK from T_OA_GF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--函件收文表
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID('''+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HS_WorkItems_BAK from T_OA_HS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--函件发文表
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HF_WorkItems_BAK from T_OA_HF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--党纪工团发文表
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_WorkItems_BAK from T_OA_DJGTF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--党纪工团收文表
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_MS_WorkItems_BAK from T_OA_MS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--工作联系单
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_WR_WorkItems_BAK from T_OA_WR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--请示报告
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_RR_WorkItems_BAK from T_OA_RR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--程序文件
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_WorkItems_BAK from T_OA_HN_PF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'


--公司发文传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GF_Circulate from T_OA_GF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--公司收文传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_GS_Circulate from T_OA_GS_Circulate  where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--函件发文传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HF_Circulate from T_OA_HF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--程序文件传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HN_PF_Circulate from T_OA_HN_PF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--函件收文传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HS_Circulate from T_OA_HS_Circulate  where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--请示报告传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_RR_Circulate from T_OA_RR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--工作联系单传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_WR_Circulate from T_OA_WR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--党纪工团发文传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_DJGTF_Circulate from T_OA_DJGTF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--党纪工团收文传阅
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_MS_Circulate from T_OA_DJGTF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'


--函件收文登记
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_HS_Edit from T_OA_HS_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit]
select * into '+@ADIM_DB_Name+'.dbo.T_OA_Receive_Edit from T_OA_Receive_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--AP镜像表
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS]
select * into '+@ADIM_DB_Name+'.dbo.WF_PROC_INSTS from WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)
if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS]
select * into '+@ADIM_DB_Name+'.dbo.WF_MANUAL_WORKITEMS from WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--流程归档状态
set @SQL=@SQL+' if exists (select * from sysobjects where id = OBJECT_ID(''['+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE]'') and OBJECTPROPERTY(id, ''IsUserTable'') = 1) 
drop table ['+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE]
select * into '+@ADIM_DB_Name+'.dbo.WF_PROC_DEVOLVE from WF_PROC_DEVOLVE where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)'

--AP数据备份
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
print '备份失败!'
return  
END 
ELSE 
BEGIN
COMMIT TRANSACTION 
print '备份完成!'
END 
--数据验证
exec P_ADIM_BACKUP_Check @ADIM_DB_Name,@ADIM_DB_Name,@YEAR