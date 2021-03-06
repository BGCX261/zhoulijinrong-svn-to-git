USE [master]
GO
/****** 对象:  Database [DB_ADIM_BAK]    脚本日期: 04/15/2010 16:13:13 ******/
CREATE DATABASE [DB_ADIM_BAK] ON  PRIMARY 
( NAME = N'DB_ADIM_BAK', FILENAME = N'D:\数据库\DB_ADIM_BAK.mdf' , SIZE = 88064KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DB_ADIM_BAK_log', FILENAME = N'D:\数据库\DB_ADIM_BAK_log.ldf' , SIZE = 69760KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Chinese_PRC_CI_AS
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'DB_ADIM_BAK', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_ADIM_BAK].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_ADIM_BAK] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [DB_ADIM_BAK] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_ADIM_BAK] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_ADIM_BAK] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB_ADIM_BAK] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_ADIM_BAK] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_ADIM_BAK] SET  READ_WRITE 
GO
ALTER DATABASE [DB_ADIM_BAK] SET RECOVERY FULL 
GO
ALTER DATABASE [DB_ADIM_BAK] SET  MULTI_USER 
GO
ALTER DATABASE [DB_ADIM_BAK] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_ADIM_BAK] SET DB_CHAINING OFF 