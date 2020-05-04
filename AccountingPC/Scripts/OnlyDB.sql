EXEC sp_configure 'show advanced options', 1;  
GO   
RECONFIGURE;  
GO  
EXEC sp_configure 'xp_cmdshell', 1;  
GO   
RECONFIGURE;  
GO  

USE [master]
GO

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

GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Accounting].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Accounting] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Accounting] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Accounting] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Accounting] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Accounting] SET ARITHABORT OFF 
GO
ALTER DATABASE [Accounting] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Accounting] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Accounting] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Accounting] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Accounting] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Accounting] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Accounting] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Accounting] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Accounting] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Accounting] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Accounting] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Accounting] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Accounting] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Accounting] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Accounting] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Accounting] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Accounting] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Accounting] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Accounting] SET  MULTI_USER 
GO
ALTER DATABASE [Accounting] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Accounting] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Accounting] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Accounting] SET TARGET_RECOVERY_TIME = 60 SECONDS 