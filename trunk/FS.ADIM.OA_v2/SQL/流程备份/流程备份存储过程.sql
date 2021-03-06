--1.将DB_ADIM_BAK全部替换为用于备份的临时数据库
--2.将DB_Adim_OldAPData全部替换为AgilePoint数据库
--3.将DB_Adim_OldOA替换成现行ADIM数据库
USE DB_Adim_OldOA
IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'P_ADIM_BACKUP' AND type = 'P')
   DROP PROCEDURE P_ADIM_BACKUP
GO
USE DB_Adim_OldOA
GO
/****** 对象:  StoredProcedure [dbo].[P_ADIM_BACKUP]    脚本日期: 04/14/2010 10:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[P_ADIM_BACKUP]  
    @STARTDATE DATETIME,  
    @ENDDATE DATETIME, 
	@FILENAME NVARCHAR(500),
    @PATH NVARCHAR(500)  
AS  
    IF ISNULL(@FILENAME,'')='' 
	BEGIN 
		print '请输入保存备份的名称' 
	END
	ELSE IF ISNULL(@PATH,'')='' 
	BEGIN 
		print '请输入保存备份的路径' 
	END
	ELSE IF @ENDDATE IS NULL or @ENDDATE=''
	BEGIN 
		print '请输入备份流程的结束日期' 
	END
	ELSE
	BEGIN
		--开始事务
		BEGIN TRANSACTION
		--将条件查询的数据写入即将执行备份的数据库
			--创建临时表，保存将要备份的流程ID
			if object_id('tempdb..#OA_BAK_PROCESS') is not null
			drop table #OA_BAK_PROCESS
			
			print '正在查询需要备份的数据...'

			IF @STARTDATE IS NULL 			
			set @STARTDATE=''
			
			SELECT PROC_INST_ID INTO #OA_BAK_PROCESS FROM( 
			select PROC_INST_ID from WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and STARTED_DATE>=@STARTDATE and STARTED_DATE<dateadd(day, 1,@ENDDATE) 
			union all
			select PROC_INST_ID from WF_PROC_INSTS where SUPER_PROC_INST_ID in (select PROC_INST_ID from WF_PROC_INSTS where (SUPER_PROC_INST_ID is null or SUPER_PROC_INST_ID='') and (STATUS='Cancelled' or STATUS='Completed') and STARTED_DATE>=@STARTDATE and STARTED_DATE<dateadd(day, 1,@ENDDATE)))a

			--公司收文表
			if exists (select * from sysobjects where id = OBJECT_ID('DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK
			select * into DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK from T_OA_GS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--公司发文表
			if exists (select * from sysobjects where id = OBJECT_ID('DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK
			select * into DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK from T_OA_GF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--函件收文表
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK from T_OA_HS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--函件发文表
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK from T_OA_HF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--党纪工团发文表
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK from T_OA_DJGTF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--党纪工团收文表
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK from T_OA_MS_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--工作联系单
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK from T_OA_WR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			
			--请示报告
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK from T_OA_RR_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)

			--程序文件
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK]
			select * into DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK from T_OA_HN_PF_WorkItems_BAK where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)


			--公司发文传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_GF_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_GF_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_GF_Circulate from T_OA_GF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK group by ProcessID)

			--公司收文传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_GS_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_GS_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_GS_Circulate from T_OA_GS_Circulate  where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK group by ProcessID)

			--函件发文传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HF_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HF_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_HF_Circulate from T_OA_HF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK group by ProcessID)

			--程序文件传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HN_PF_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HN_PF_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_HN_PF_Circulate from T_OA_HN_PF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK group by ProcessID)

			--函件收文传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HS_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HS_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_HS_Circulate from T_OA_HS_Circulate  where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK group by ProcessID)

			--请示报告传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_RR_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_RR_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_RR_Circulate from T_OA_RR_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK group by ProcessID)

			--工作联系单传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_WR_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_WR_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_WR_Circulate from T_OA_WR_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK group by ProcessID)

			--党纪工团发文传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_DJGTF_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_DJGTF_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_DJGTF_Circulate from T_OA_DJGTF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK group by ProcessID)
		
			--党纪工团收文传阅
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_MS_Circulate]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_MS_Circulate]
			select * into DB_ADIM_BAK.dbo.T_OA_MS_Circulate from T_OA_DJGTF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK group by ProcessID)


			--函件收文登记
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_HS_Edit]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_HS_Edit]
			select * into DB_ADIM_BAK.dbo.T_OA_HS_Edit from T_OA_HS_Edit where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK group by ProcessID)
	        
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.T_OA_Receive_Edit]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.T_OA_Receive_Edit]
			select * into DB_ADIM_BAK.dbo.T_OA_Receive_Edit from T_OA_Receive_Edit where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK group by ProcessID union all select ProcessID from DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK group by ProcessID )
			
			--AP镜像表
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.WF_PROC_INSTS]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.WF_PROC_INSTS]
			select * into DB_ADIM_BAK.dbo.WF_PROC_INSTS from WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.WF_MANUAL_WORKITEMS]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.WF_MANUAL_WORKITEMS]
			select * into DB_ADIM_BAK.dbo.WF_MANUAL_WORKITEMS from WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)


			--流程归档状态
			if exists (select * from sysobjects where id = OBJECT_ID('[DB_ADIM_BAK.dbo.WF_PROC_DEVOLVE]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
			drop table [DB_ADIM_BAK.dbo.WF_PROC_DEVOLVE]
			select * into DB_ADIM_BAK.dbo.WF_PROC_DEVOLVE from WF_PROC_DEVOLVE where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
		IF(@@ERROR<>0)    
		BEGIN
			ROLLBACK   TRANSACTION  
			return  
		END 
		ELSE 
		BEGIN
			COMMIT TRANSACTION 
		END 
		--执行备份ADIM数据库
		print '正在备份ADIM数据库...'
		DECLARE @SQL NVARCHAR(4000) 
		DECLARE @DBNAME NVARCHAR(100)
		DECLARE @DATE varchar(50)
		DECLARE @NEWFILENAME NVARCHAR(100)
		set @DATE=Datename(yy,getdate())+Datename(m,getdate())+Datename(d,getdate())+Datename(hh,getdate())+Datename(mi,getdate())+Datename(ss,getdate())+Datename(ms,getdate())
		SET @DBNAME='DB_ADIM_BAK'
		SET @PATH=@PATH+N'\'  
		SET @NEWFILENAME = @PATH+@FILENAME+'_ADIM'+@DATE+'.bak'  
		BACKUP DATABASE @DBNAME TO DISK =@NEWFILENAME
		print 'ADIM数据库备份成功，！'+@NEWFILENAME
		--执行备份AgilePoint数据库
		print '正在备份AgilePoint数据库...'
		SET @DBNAME='DB_Adim_OldAPData'
		SET @NEWFILENAME = @PATH+@FILENAME+'_AgilePoint'+@DATE+'.bak'  
		BACKUP DATABASE @DBNAME TO DISK = @NEWFILENAME
		print 'AgilePoint数据库备份成功！'+@NEWFILENAME
		IF(@@ERROR=0)    
		BEGIN
			print '正在删除已经备份的数据...'
			--删除已经备份AgilePoint数据库的流程
			delete DB_Adim_OldAPData.dbo.WF_PROC_INSTS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			delete DB_Adim_OldAPData.dbo.WF_MANUAL_WORKITEMS where PROC_INST_ID in (select PROC_INST_ID from #OA_BAK_PROCESS)

			--删除已经备份的节点数据
			delete T_OA_GS_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK group by ProcessID)
			delete T_OA_GF_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK group by ProcessID)
			delete T_OA_HS_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK group by ProcessID)
			delete T_OA_HF_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK group by ProcessID)
			delete T_OA_WR_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK group by ProcessID)
			delete T_OA_RR_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK group by ProcessID)
			delete T_OA_HN_PF_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK group by ProcessID)
			delete T_OA_DJGTF_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK group by ProcessID)
			delete T_OA_MS_WorkItems_BAK where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK group by ProcessID)
			--删除已经备份的传阅数据
			delete T_OA_GF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK group by ProcessID)
			delete T_OA_GS_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK group by ProcessID)
			delete T_OA_HF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK group by ProcessID)
			delete T_OA_HS_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK group by ProcessID)
			delete T_OA_WR_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK group by ProcessID)
			delete T_OA_RR_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK group by ProcessID)
			delete T_OA_HN_PF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK group by ProcessID)
			delete T_OA_MS_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK group by ProcessID)
			delete T_OA_DJGTF_Circulate where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK group by ProcessID)
			--删除已经备份的收文登记数据
			delete T_OA_HS_Edit where  ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK group by ProcessID)
			DELETE T_OA_Receive_Edit where ProcessID in (select ProcessID from DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK group by ProcessID union all select ProcessID from DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK group by ProcessID )
			--删除已经备份的归档状态
			delete WF_PROC_DEVOLVE where ProcessID in (select PROC_INST_ID from #OA_BAK_PROCESS)
			print '已经备份的数据删除成功！'
		END 
		--删除备份数据库中临时创建的表
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_GS_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_GF_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HF_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HS_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_MS_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_DJGTF_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_WR_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_RR_WorkItems_BAK
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HN_PF_WorkItems_BAK

		DROP TABLE DB_ADIM_BAK.dbo.T_OA_GF_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_GS_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HF_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HS_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_MS_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_DJGTF_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_RR_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_WR_Circulate
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HN_PF_Circulate

		DROP TABLE DB_ADIM_BAK.dbo.T_OA_HS_Edit
		DROP TABLE DB_ADIM_BAK.dbo.T_OA_Receive_Edit

		DROP TABLE DB_ADIM_BAK.dbo.WF_MANUAL_WORKITEMS
		DROP TABLE DB_ADIM_BAK.dbo.WF_PROC_INSTS

		DROP TABLE DB_ADIM_BAK.dbo.WF_PROC_DEVOLVE
		print '备份完成！'
	END