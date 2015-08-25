--将APData全部替换为AgilePoint数据库
--将DB_Adim全部替换为ADIM数据库
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
	print '请输入要验证的ADIM数据库' 
	return
END
IF ISNULL(@AP_DB_NAME,'')='' 
BEGIN 
	print '请输入要验证的AP数据库' 
	return
END
BEGIN TRANSACTION
declare @SQL varchar(max)
--查询验证使用的数据
SELECT PROC_INST_ID INTO #OA_BAK_PROCESS FROM( 
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and year(LAST_RUNNING_END_TIME)=@YEAR
union all
select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where SUPER_PROC_INST_ID in (select PROC_INST_ID from APData.dbo.WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and  year(LAST_RUNNING_END_TIME)=@YEAR))a

set @SQL='
if (select count(0) from T_OA_GS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GS_WorkItems_BAK)
begin
print ''公司收文T_OA_GS_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_GF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GF_WorkItems_BAK)
begin
print ''公司发文T_OA_GF_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_HS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HS_WorkItems_BAK)
begin
print ''函件收文T_OA_HS_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_HF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HF_WorkItems_BAK)
begin
print ''函件发文T_OA_HF_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_DJGTF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_DJGTF_WorkItems_BAK)
begin
print ''党纪工团发文T_OA_DJGTF_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_MS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_MS_WorkItems_BAK)
begin
print ''党纪工团收文T_OA_MS_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_WR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_WR_WorkItems_BAK)
begin
print ''工作联系单T_OA_WR_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_RR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_RR_WorkItems_BAK)
begin
print ''请示报告T_OA_RR_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_HN_PF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HN_PF_WorkItems_BAK)
begin
print ''程序文件T_OA_HN_PF_WorkItems_BAK数据出错,验证失败!''
end
else if (select count(0) from T_OA_GF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GF_Circulate)
begin
print ''公司发文传阅T_OA_GF_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_GS_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_GS_Circulate)
begin
print ''公司收文传阅T_OA_GS_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_HF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HF_Circulate)
begin
print ''函件发文传阅T_OA_HF_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_HN_PF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HN_PF_Circulate)
begin
print ''程序文件传阅T_OA_HN_PF_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_HS_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HS_Circulate)
begin
print ''函件收文传阅T_OA_HS_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_RR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_RR_Circulate)
begin
print ''请示报告传阅T_OA_RR_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_WR_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_WR_Circulate)
begin
print ''工作联系单传阅T_OA_WR_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_DJGTF_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_DJGTF_Circulate)
begin
print ''党纪工团发文传阅T_OA_DJGTF_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_MS_Circulate where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_MS_Circulate)
begin
print ''党纪工团收文传阅T_OA_MS_Circulate数据出错,验证失败!''
end
else if (select count(0) from T_OA_HS_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_HS_Edit)
begin
print ''函件登记表T_OA_HS_Edit数据出错,验证失败!''
end
else if (select count(0) from T_OA_Receive_Edit where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.T_OA_Receive_Edit)
begin
print ''收文登记表T_OA_Receive_Edit数据出错,验证失败!''
end
else if (select count(0) from WF_PROC_DEVOLVE where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.WF_PROC_DEVOLVE)
begin
print ''归档状态表WF_PROC_DEVOLVE数据出错,验证失败!''
end
else if (select count(0) from WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.WF_PROC_INSTS)
begin
print ''ADIM数据库WF_PROC_INSTS数据出错,验证失败!''
end
else if (select count(0) from WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@ADIM_DB_NAME+'.dbo.WF_MANUAL_WORKITEMS)
begin
print ''ADIM数据库WF_MANUAL_WORKITEMS数据出错,验证失败!''
end
else if (select count(0) from APData.dbo.WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@AP_DB_NAME+'.dbo.WF_MANUAL_WORKITEMS)
begin
print ''AP数据库WF_MANUAL_WORKITEMS数据出错,验证失败!''
end
else if (select count(0) from APData.dbo.WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS))<>(select count(0) from '+@AP_DB_NAME+'.dbo.WF_PROC_INSTS)
begin
print ''AP数据库WF_PROC_INSTS数据出错,验证失败!''
end
else
begin
print ''验证成功!''
end'
exec (@SQL)
IF(@@ERROR<>0)    
BEGIN
ROLLBACK   TRANSACTION  
print '验证失败!'
return  
END 
ELSE 
BEGIN
COMMIT TRANSACTION 
END 