EXEC sp_configure 'show advanced options', 1;  
;   
RECONFIGURE;  
;  
EXEC sp_configure 'xp_cmdshell', 1;  
;   
RECONFIGURE;  
;  

USE [master]
;

create table #output (output varchar(255) null)
exec master..xp_cmdshell 'md %localappdata%\MaliSoftware'
insert #output exec master..xp_cmdshell 'echo %localappdata%\MaliSoftware'
declare @qwerty nvarchar(100)
set @qwerty = (select top(1) [output] from #output where [output] is not null)

drop table #output
declare @query nvarchar(max)
set @query=
'IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N''Accounting'')
BEGIN
CREATE DATABASE [Accounting]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N''Accounting'', FILENAME = N''' + (select @qwerty) + N'\Accounting.mdf' + ''', SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N''Accounting_log'', FILENAME = N''' + (select @qwerty) + N'\Accounting_log.ldf' + ''' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
END'

EXEC(@query)

;
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Accounting].[dbo].[sp_fulltext_database] @action = 'enable'
end
;
ALTER DATABASE [Accounting] SET ANSI_NULL_DEFAULT OFF 
;
ALTER DATABASE [Accounting] SET ANSI_NULLS OFF 
;
ALTER DATABASE [Accounting] SET ANSI_PADDING OFF 
;
ALTER DATABASE [Accounting] SET ANSI_WARNINGS OFF 
;
ALTER DATABASE [Accounting] SET ARITHABORT OFF 
;
ALTER DATABASE [Accounting] SET AUTO_CLOSE OFF 
;
ALTER DATABASE [Accounting] SET AUTO_SHRINK OFF 
;
ALTER DATABASE [Accounting] SET AUTO_UPDATE_STATISTICS ON 
;
ALTER DATABASE [Accounting] SET CURSOR_CLOSE_ON_COMMIT OFF 
;
ALTER DATABASE [Accounting] SET CURSOR_DEFAULT  GLOBAL 
;
ALTER DATABASE [Accounting] SET CONCAT_NULL_YIELDS_NULL OFF 
;
ALTER DATABASE [Accounting] SET NUMERIC_ROUNDABORT OFF 
;
ALTER DATABASE [Accounting] SET QUOTED_IDENTIFIER OFF 
;
ALTER DATABASE [Accounting] SET RECURSIVE_TRIGGERS OFF 
;
ALTER DATABASE [Accounting] SET  DISABLE_BROKER 
;
ALTER DATABASE [Accounting] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
;
ALTER DATABASE [Accounting] SET DATE_CORRELATION_OPTIMIZATION OFF 
;
ALTER DATABASE [Accounting] SET TRUSTWORTHY OFF 
;
ALTER DATABASE [Accounting] SET ALLOW_SNAPSHOT_ISOLATION OFF 
;
ALTER DATABASE [Accounting] SET PARAMETERIZATION SIMPLE 
;
ALTER DATABASE [Accounting] SET READ_COMMITTED_SNAPSHOT OFF 
;
ALTER DATABASE [Accounting] SET HONOR_BROKER_PRIORITY OFF 
;
ALTER DATABASE [Accounting] SET RECOVERY SIMPLE 
;
ALTER DATABASE [Accounting] SET  MULTI_USER 
;
ALTER DATABASE [Accounting] SET PAGE_VERIFY CHECKSUM  
;
ALTER DATABASE [Accounting] SET DB_CHAINING OFF 
;
ALTER DATABASE [Accounting] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
;
ALTER DATABASE [Accounting] SET TARGET_RECOVERY_TIME = 60 SECONDS 