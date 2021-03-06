use master
IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'P_Create_DB' AND type = 'P')
   DROP PROCEDURE P_Create_DB
go
CREATE PROC P_Create_DB
	@NAME varchar(50),
	@FILENAME varchar(100)
AS 
declare @SQL varchar(max)
SET @SQL='CREATE DATABASE '+@NAME+' ON  PRIMARY 
( NAME = '''+@NAME+''', FILENAME = '''+@FILENAME+'.mdf'' , SIZE = 457728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = '''+@NAME+'_log'', FILENAME = '''+@FILENAME+'.ldf'' , SIZE = 1475904KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)'
exec (@SQL)
