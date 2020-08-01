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
GO

/****** Object:  UserDefinedFunction [dbo].[CanUsedPlace]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanUsedPlace]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CanUsedPlace] (@ID int)
RETURNS bit
AS
BEGIN
	DECLARE @res bit
	if (@ID not in (
		select PlaceID from (
			select PlaceID from InteractiveWhiteboard
			union
			select PlaceID from Monitor
			union
			select PlaceID from NetworkSwitch
			union
			select PlaceID from Notebook
			union
			select PlaceID from PC
			union
			select PlaceID from PrinterScanner
			union
			select PlaceID from Projector
			union
			select PlaceID from ProjectorScreen
		) as q
		where PlaceID is not null)
	)
	begin
		set @res = 1
	end
	else
	begin
		set @res = 0
	end
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAcquisitionDate]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAcquisitionDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAcquisitionDate] 
(
	@InvoiceID int
)
RETURNS date
AS
BEGIN
	DECLARE @res date
	SELECT TOP(1) @res = Date from Invoice where ID=@InvoiceID
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAspectRatio]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAspectRatio]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAspectRatio] 
(
	@ID int
)
RETURNS nvarchar(20)
AS
BEGIN
	DECLARE @res nvarchar(20)
	SELECT TOP(1) @res = [Name] from AspectRatio where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAudienceID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAudienceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAudienceID] 
(
	@ID int
)
RETURNS int
AS
BEGIN
	DECLARE @res int, @AudienceTableID int, @AudienceID int
	SELECT TOP(1) @AudienceTableID = AudienceTableID from Place where ID=@ID
	SELECT TOP(1) @AudienceID = AudienceID from AudienceTable where ID=@AudienceTableID
	SELECT TOP(1) @res = ID from Audience where ID=@AudienceID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAudienceName]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAudienceName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAudienceName] 
(
	@ID int
)
RETURNS nvarchar(10)
AS
BEGIN
	DECLARE @res nvarchar(10), @AudienceTableID int, @AudienceID int
	SELECT TOP(1) @AudienceTableID = AudienceTableID from Place where ID=@ID
	SELECT TOP(1) @AudienceID = AudienceID from AudienceTable where ID=@AudienceTableID
	SELECT TOP(1) @res = [Name] from Audience where ID=@AudienceID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAudienceTableID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAudienceTableID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAudienceTableID] 
(
	@ID int
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = AudienceTableID from Place where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAudienceTableName]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAudienceTableName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAudienceTableName] 
(
	@PlaceID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)

	select @res = CASE AudienceTable.Description WHEN N''стол'' THEN N'' '' + 
	AudienceTable.Description + N'' '' + AudienceTable.Name ELSE N'' '' + 
	AudienceTable.Description END from AudienceTable 
	where AudienceTable.ID=@PlaceID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCostOS]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCostOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCostOS] (@ID int)
RETURNS int
AS
BEGIN
	DECLARE @res int
	select @res = Price from OS where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCostSoft]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCostSoft]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCostSoft] (@ID int)
RETURNS int
AS
BEGIN
	DECLARE @res int
	select @res = Price from LicenseSoftware where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCountLicense]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCountLicense]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCountLicense] (@ID int)
RETURNS int
AS
BEGIN
	DECLARE @res int
	select @res = Count from LicenseSoftware where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCountOS]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCountOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCountOS] (@ID int)
RETURNS int
AS
BEGIN
	DECLARE @res int
	select @res = Count from OS where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDeviceOnPlaceID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDeviceOnPlaceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetDeviceOnPlaceID] (@ID int)
RETURNS int
AS
BEGIN
	DECLARE @res int
	select Top(1) @res = PlaceID from (
		select PlaceID from InteractiveWhiteboard
		union
		select PlaceID from Monitor
		union
		select PlaceID from NetworkSwitch
		union
		select PlaceID from Notebook
		union
		select PlaceID from PC
		union
		select PlaceID from PrinterScanner
		union
		select PlaceID from Projector
		union
		select PlaceID from ProjectorScreen
	) as q
	where PlaceID is not null and PlaceID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDevicesInLocation]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDevicesInLocation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetDevicesInLocation] 
(
	@ID int
)
RETURNS nvarchar(500)
AS
BEGIN
	DECLARE @res nvarchar(500) = N''''
	Declare @table table(ID int not null identity(1,1), Name nvarchar(50), [Count] int)

	insert into @table (Name, Count)
	SELECT RussianName, COUNT(TypeDeviceID) as Count
	from Place inner join TypeDevice on Place.TypeDeviceID=TypeDevice.ID
	where AudienceTableID=@ID group by TypeDeviceID, RussianName
	Declare @count int, @i int
	select @count = COUNT(Name) from @table
	set @i=1
	while @i<=@count
	begin
		Select @res+=Name+N'' x''+str(Count, 1, 1) from @table where ID=@i
		if @i<@count
			set @res+=N'', ''
		set @i+=1
	end
	return @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetFrequency]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFrequency]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetFrequency] 
(
	@ID int
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = [Name] from Frequency where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetInstalledCount]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInstalledCount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetInstalledCount] 
(
	@ID int
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res =  SUM(countLicense) from (
		select LicenseID, COUNT(InstalledSoftwareNotebook.ID) as countLicense
		from Notebook inner join InstalledSoftwareNotebook on Notebook.ID=NotebookID
		inner join LicenseSoftware on LicenseID=LicenseSoftware.ID
		group by LicenseID
		union all
		select LicenseID, COUNT(InstalledSoftwarePC.ID) as countLiocense
		from PC inner join InstalledSoftwarePC on PC.ID=PCID
		inner join LicenseSoftware on LicenseID=LicenseSoftware.ID
		group by LicenseID
	) as q
	where q.LicenseID=@ID
	group by q.LicenseID
	if @res=NULL
		Return 0
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInvoiceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetInvoiceID] 
(
	@InvoiceNumber nvarchar(50)
)
RETURNS int
AS
BEGIN
	if not exists (select * from Invoice where Invoice.Number=@InvoiceNumber)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	DECLARE @res int
	SELECT TOP(1) @res = [ID] from Invoice where Invoice.Number=@InvoiceNumber
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIsWorkingCondition]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIsWorkingCondition]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetIsWorkingCondition] 
(
	@I bit
)
RETURNS nvarchar(30)
AS
BEGIN
	DECLARE @res nvarchar(30)
	if (@I=1 or @I is null)
		Set  @res =  N''Работает''
	else
		set @res = N''Не работает''
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetLocalion]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLocalion]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetLocalion] 
(
	@PlaceID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @temp nvarchar(20)
	DECLARE @temp2 nvarchar(50)
	DECLARE @res nvarchar(100)

	select @temp2 = CASE AudienceTable.Description WHEN N''стол'' THEN N'' '' + 
	AudienceTable.Description + N'' '' + AudienceTable.Name ELSE N'' '' + 
	AudienceTable.Description END from AudienceTable 
	where AudienceTable.ID=dbo.GetAudienceTableID(@PlaceID)

	SELECT TOP(1) @temp =  N''Аудитория '' + Audience.Name 
	FROM Audience inner join AudienceTable on 
	Audience.ID=AudienceTable.AudienceID
	WHERE AudienceTable.ID=dbo.GetAudienceTableID(@PlaceID)

	set @res = @temp + @temp2
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetMatrix]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMatrix]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetMatrix] 
(
	@ID int
)
RETURNS nvarchar(5)
AS
BEGIN
	DECLARE @res nvarchar(5)
	SELECT TOP(1) @res = [Name] from MatrixTechnology where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNameOS]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNameOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetNameOS] 
(
	@OSID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from OS where ID=@OSID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNextInventoryNumber]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNextInventoryNumber]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetNextInventoryNumber] ()
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT @res = MAX(InventoryNumber)+1 from (
		select InventoryNumber from InteractiveWhiteboard
		union
		select InventoryNumber from Monitor
		union
		select InventoryNumber from NetworkSwitch
		union
		select InventoryNumber from Notebook
		union
		select InventoryNumber from PC
		union
		select InventoryNumber from PrinterScanner
		union
		select InventoryNumber from Projector
		union
		select InventoryNumber from ProjectorScreen
	) as query
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNumberInvoice]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNumberInvoice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetNumberInvoice] 
(
	@InvoiceID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Number] from Invoice where ID=@InvoiceID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOSID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOSID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetOSID] 
(
	@OSName nvarchar(20)
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = [ID] from OS where Name=@OSName
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPaperSize]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaperSize]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetPaperSize] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from PaperSize where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPaperSizeID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaperSizeID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetPaperSizeID] 
(
	@Name nvarchar(10)
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = [ID] from PaperSize where Name=@Name
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetProjectorTechnology]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProjectorTechnology]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetProjectorTechnology] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from ProjectorTechnology where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetProjectorTechnologyID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProjectorTechnologyID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetProjectorTechnologyID] 
(
	@Name nvarchar(10)
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = [ID] from ProjectorTechnology where ID=@Name
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetResolution]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetResolution]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetResolution] 
(
	@ID int
)
RETURNS nvarchar(61)
AS
BEGIN
	DECLARE @res nvarchar(61)
	SELECT TOP(1) @res = [Name] from Resolution where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetScreenInstalled]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScreenInstalled]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetScreenInstalled] 
(
	@ID int
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @res nvarchar(50)
	SELECT TOP(1) @res = [Name] from ScreenInstalled where ID=@ID
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetSoftwareName]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSoftwareName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetSoftwareName] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = Name FROM LicenseSoftware Where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTable]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTable]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTable]
(
	@ID int
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @res nvarchar(50)
	select @res = CASE AudienceTable.Description WHEN N''стол'' THEN N'' '' + 
	AudienceTable.Description + N'' '' + AudienceTable.Name ELSE N'' '' + 
	AudienceTable.Description END from AudienceTable
	where AudienceTable.ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTableName]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTableName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTableName] 
(
	@ID int
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @res nvarchar(50)
	SELECT TOP(1) @res = [Name] from TypeDevice where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeDevice]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeDevice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypeDevice] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from TypeDevice where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeDeviceID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeDeviceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypeDeviceID] 
(
	@Name nvarchar(100)
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = ID from TypeDevice where Name=@Name
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeNetworkSwitch]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeNetworkSwitch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypeNetworkSwitch] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from TypeNetworkSwitch where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeNotebook]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeNotebook]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypeNotebook] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from TypeNotebook where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypePrinter]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypePrinter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypePrinter] 
(
	@ID int
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @res nvarchar(100)
	SELECT TOP(1) @res = [Name] from TypePrinter where ID=@ID
	RETURN @res
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypePrinterID]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypePrinterID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypePrinterID] 
(
	@Name nvarchar(30)
)
RETURNS int
AS
BEGIN
	DECLARE @res int
	SELECT TOP(1) @res = [ID] from TypePrinter where Name=@Name
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeSoftwareLicense]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeSoftwareLicense]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTypeSoftwareLicense] 
(
	@ID int
)
RETURNS nvarchar(20)
AS
BEGIN
	DECLARE @res nvarchar(20)
	SELECT TOP(1) @res = Name from TypeSoftLicense where ID=@ID
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetWiFiFrequency]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWiFiFrequency]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetWiFiFrequency] 
(
	@ID int
)
RETURNS nvarchar(20)
AS
BEGIN
	DECLARE @res nvarchar(20)
	SELECT TOP(1) @res = [Name] from WiFiFrequency where ID=@ID
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[IsAvailableInventoryNumber]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsAvailableInventoryNumber]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[IsAvailableInventoryNumber] (@InvN int)
RETURNS int
AS
BEGIN
	DECLARE @res bit
	if not exists (Select InventoryNumber from (
		select InventoryNumber from InteractiveWhiteboard
		union
		select InventoryNumber from Monitor
		union
		select InventoryNumber from NetworkSwitch
		union
		select InventoryNumber from Notebook
		union
		select InventoryNumber from PC
		union
		select InventoryNumber from PrinterScanner
		union
		select InventoryNumber from Projector
		union
		select InventoryNumber from ProjectorScreen
	) as query where InventoryNumber=@InvN)
	set @res=1
	else set @res=0
	RETURN @res

END
' 
END
GO
/****** Object:  Table [dbo].[AspectRatio]    Script Date: 14.06.2020 23:04:16  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspectRatio]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspectRatio](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name]  AS ((CONVERT([nvarchar],[Width])+N':')+CONVERT([nvarchar],[Height])),
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
 CONSTRAINT [PK_AspectRatio] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Audience]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Audience]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Audience](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Audience] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AudienceTable]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AudienceTable]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AudienceTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AudienceID] [int] NOT NULL,
	[Name] [nvarchar](20) NULL,
	[Row] [int] NULL,
	[Col] [int] NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_AudienceTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Frequency]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Frequency]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Frequency](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [int] NOT NULL,
 CONSTRAINT [PK_Frequency] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Image]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Image]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Image](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Image] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InstalledSoftwareNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InstalledSoftwareNotebook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NotebookID] [int] NOT NULL,
	[LicenseID] [int] NOT NULL,
 CONSTRAINT [PK_InstalledSoftwareNotebook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InstalledSoftwarePC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InstalledSoftwarePC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PCID] [int] NOT NULL,
	[LicenseID] [int] NOT NULL,
 CONSTRAINT [PK_InstalledSoftwarePC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InteractiveWhiteboard]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InteractiveWhiteboard](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[Diagonal] [real] NULL,
	[PlaceID] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invoice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Invoice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[Date] [date] NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Journal]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Journal]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Journal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableNameID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[TypeChange] [int] NOT NULL,
	[OldValue] [xml] NULL,
	[NewValue] [xml] NOT NULL,
	[Date] [date] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LicenseSoftware]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LicenseSoftware]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LicenseSoftware](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Price] [money] NULL,
	[Count] [int] NOT NULL,
	[TotalCost]  AS ([Price]*[Count]),
	[InvoiceID] [int] NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_LicenseSoftware] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MatrixTechnology]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MatrixTechnology]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MatrixTechnology](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](5) NOT NULL,
 CONSTRAINT [PK_MatrixTechnology] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Monitor]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Monitor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Monitor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[ScreenDiagonal] [real] NULL,
	[PlaceID] [int] NULL,
	[ResolutionID] [int] NULL,
	[FrequencyID] [int] NULL,
	[MatrixTechnologyID] [int] NULL,
	[VideoConnectors] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[NetworkSwitch]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[NetworkSwitch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [int] NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[NumberOfPorts] [int] NULL,
	[PlaceID] [int] NULL,
	[WiFiFrequencyID] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
 CONSTRAINT [PK__NetworkS__3214EC27D73498D3] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Notebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notebook]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Notebook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeNotebookID] [int] NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Cost] [money] NOT NULL,
	[CPUModel] [nvarchar](20) NULL,
	[RAMGB] [int] NULL,
	[SSDCapacityGB] [int] NULL,
	[HDDCapacityGB] [int] NULL,
	[VideoCard] [nvarchar](30) NULL,
	[PlaceID] [int] NULL,
	[OSID] [int] NULL,
	[InvoiceID] [int] NULL,
	[ScreenDiagonal] [real] NULL,
	[ResolutionID] [int] NULL,
	[FrequencyID] [int] NULL,
	[MatrixTechnologyID] [int] NULL,
	[VideoConnectors] [int] NULL,
	[NumberOfCores] [int] NULL,
	[FrequencyProcessor] [int] NULL,
	[MaxFrequencyProcessor] [int] NULL,
	[VideoRAMGB] [int] NULL,
	[FrequencyRAM] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
 CONSTRAINT [PK_Notebook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[OS]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OS]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Price] [money] NULL,
	[Count] [int] NOT NULL,
	[TotalCost]  AS ([Price]*[Count]),
	[InvoiceID] [int] NULL,
 CONSTRAINT [PK_OS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[OtherEquipment]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OtherEquipment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OtherEquipment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[PlaceID] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PaperSize]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PaperSize]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PaperSize](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_PaperSize] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PC]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[CPUModel] [nvarchar](20) NULL,
	[RAMGB] [int] NULL,
	[SSDCapacityGB] [int] NULL,
	[HDDCapacityGB] [int] NULL,
	[VideoCard] [nvarchar](30) NULL,
	[Motherboard] [nvarchar](30) NULL,
	[PlaceID] [int] NULL,
	[OSID] [int] NULL,
	[InvoiceID] [int] NULL,
	[NumberOfCores] [int] NULL,
	[FrequencyProcessor] [int] NULL,
	[MaxFrequencyProcessor] [int] NULL,
	[VideoRAMGB] [int] NULL,
	[FrequencyRAM] [int] NULL,
	[VideoConnectors] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
 CONSTRAINT [PK_PC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Place]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Place]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Place](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AudienceTableID] [int] NOT NULL,
	[TypeDeviceID] [int] NOT NULL,
	[Description] [nvarchar](50) NULL,
	[CanUsed]  AS ([dbo].[CanUsedPlace]([ID])),
 CONSTRAINT [PK_Place] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PrinterScanner]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrinterScanner]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrinterScanner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[TypePrinterID] [int] NULL,
	[PaperSizeID] [int] NULL,
	[PlaceID] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Projector]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projector]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Projector](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[MaxDiagonal] [real] NULL,
	[ProjectorTechnologyID] [int] NULL,
	[PlaceID] [int] NULL,
	[ResolutionID] [int] NULL,
	[VideoConnectors] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ProjectorScreen]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProjectorScreen](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryNumber] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [money] NOT NULL,
	[InvoiceID] [int] NULL,
	[Diagonal] [real] NULL,
	[IsElectronicDrive] [bit] NULL,
	[PlaceID] [int] NULL,
	[AspectRatioID] [int] NULL,
	[ScreenInstalledID] [int] NULL,
	[ImageID] [int] NULL,
	[IsWorkingCondition] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ProjectorTechnology]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectorTechnology]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProjectorTechnology](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_ProjectorTechnology] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Resolution]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resolution]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Resolution](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Name]  AS ((CONVERT([nvarchar],[Width])+N'x')+CONVERT([nvarchar],[Height])),
	[AspectRatioID] [int] NULL,
 CONSTRAINT [PK_Resolution] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ScreenInstalled]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScreenInstalled]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ScreenInstalled](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_ScreenInstalled] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TableName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TableName]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TableName](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeDevice]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeDevice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypeDevice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[RussianName] [nvarchar](50) NULL,
	[RussianNamePlural] [nvarchar](50) NULL,
 CONSTRAINT [PK_TypeDevice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeNetworkSwitch]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeNetworkSwitch]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypeNetworkSwitch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
 CONSTRAINT [PK_NetworkSwitchType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeNotebook]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypeNotebook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TypeNotebook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypePrinter]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypePrinter]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypePrinter](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_TypePrinter] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeSoftLicense]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeSoftLicense]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypeSoftLicense](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TypeSoftLicense] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[VideoConnector]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VideoConnector]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VideoConnector](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_VideoConnector] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[WiFiFrequency]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WiFiFrequency]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WiFiFrequency](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
 CONSTRAINT [PK_WiFiFrequency] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllDevicesOnPlace]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllDevicesOnPlace]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllDevicesOnPlace] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'') as FullName, PlaceID from (
		select ID, dbo.GetTableName(4) as TableName, InventoryNumber, N''Интерактивная доска'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from InteractiveWhiteboard
		union
		select ID, dbo.GetTableName(6) as TableName, InventoryNumber, N''Монитор'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Monitor
		union
		select ID, dbo.GetTableName(5) as TableName, InventoryNumber, dbo.GetTypeNetworkSwitch(TypeID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from NetworkSwitch
		union
		select ID, dbo.GetTableName(2) as TableName, InventoryNumber, dbo.GetTypeNotebook(TypeNotebookID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Notebook
		union
		select ID, dbo.GetTableName(1) as TableName, InventoryNumber, N''Компьютер'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from PC
		union
		select ID, dbo.GetTableName(3) as TableName, InventoryNumber, dbo.GetTypePrinter(TypePrinterID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from PrinterScanner
		union
		select ID, dbo.GetTableName(7) as TableName, InventoryNumber, N''Проектор'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Projector
		union
		select ID, dbo.GetTableName(8) as TableName, InventoryNumber, N''Экран для проектора'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from ProjectorScreen
		union
		select ID, dbo.GetTableName(9) as TableName, InventoryNumber, N''Прочее'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from OtherEquipment
	) as q inner join AudienceTable on AudienceTable.ID=q.AudienceTableID
	where AudienceTableID is not null and AudienceTableID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypeOnPlace]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypeOnPlace]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllTypeOnPlace](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT Place.ID as PlaceID, Name, RussianName, AudienceTableID
	from Place inner join TypeDevice on Place.TypeDeviceID=TypeDevice.ID
	where AudienceTableID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypeAndDeviceOnPlace]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypeAndDeviceOnPlace]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllTypeAndDeviceOnPlace](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT AllType.PlaceID, AllType.Name as TypeName, RussianName, AllType.AudienceTableID,
	q.ID, TableName, InventoryNumber, Type, q.Name, ImageID, 
	Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'') as FullName
	from dbo.GetAllDevicesonPlace(@ID) as q 
	inner join [dbo].[GetAllTypeOnPlace](@ID) as AllType on AllType.PlaceID=q.PlaceID
	where q.AudienceTableID=@ID
	union
	SELECT AllType.PlaceID, AllType.Name as TypeName, RussianName, AllType.AudienceTableID,
	NULL as ID, NULL as TableName, NULL as InventoryNumber, NULL as Type, NULL as Name, 
	NULL as ImageID, NULL as  FullName
	from [dbo].[GetAllTypeOnPlace](@ID) as AllType 
	where AllType.AudienceTableID=@ID and PlaceID not in (SELECT AllType.PlaceID
	from dbo.GetAllDevicesonPlace(@ID) as q 
	inner join [dbo].[GetAllTypeOnPlace](@ID) as AllType on AllType.PlaceID=q.PlaceID
	where q.AudienceTableID=@ID)
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOSInstaledList]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOSInstaledList]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetOSInstaledList] ()
RETURNS TABLE 
AS
RETURN 
(
	select Name as [Наименование], SUM(countOS) as [Количество], dbo.GetCostOS(q.ID) as ''Цена'', dbo.GetCountOS(q.ID) as ''Общее количество'', dbo.GetCountOS(q.ID)-SUM(countOS) as ''Доступно'' from (
		select OS.ID, OS.Name, COUNT(Notebook.OSID) as countOS
		from Notebook inner join OS on OSID=OS.ID
		group by OS.Name, OS.ID
		union all
		select OS.ID, OS.Name, COUNT(PC.OSID) as countOS
		from PC inner join OS on OSID=OS.ID
		group by OS.Name, OS.ID
	) as q
	group by q.Name, q.ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetSoftInstaledList]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSoftInstaledList]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetSoftInstaledList] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.Name as [Наименование], SUM(countLicense) as [Количество], dbo.GetCostSoft(q.ID) as ''Цена'', dbo.GetCountLicense(q.ID) as ''Общее количество'', dbo.GetCountLicense(q.ID)-SUM(countLicense) as ''Доступно'' from (
		select LicenseSoftware.ID, LicenseSoftware.Name, COUNT(InstalledSoftwareNotebook.ID) as countLicense
		from Notebook inner join InstalledSoftwareNotebook on Notebook.ID=NotebookID
		inner join LicenseSoftware on LicenseID=LicenseSoftware.ID
		group by LicenseSoftware.Name, LicenseSoftware.ID
		union all
		select LicenseSoftware.ID, LicenseSoftware.Name, COUNT(InstalledSoftwarePC.ID) as countLiocense
		from PC inner join InstalledSoftwarePC on PC.ID=PCID
		inner join LicenseSoftware on LicenseID=LicenseSoftware.ID
		group by LicenseSoftware.Name, LicenseSoftware.ID
	) as q inner join LicenseSoftware on LicenseSoftware.Name=q.Name
	group by q.Name, q.ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetSoftAndOSInstaledList]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSoftAndOSInstaledList]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetSoftAndOSInstaledList] ()
RETURNS TABLE 
AS
RETURN 
(
	select N''Программное обеспечение'' as ''Тип'', * from dbo.GetSoftInstaledList()
	union
	select N''Операционная система'' as ''Тип'', * from GetOSInstaledList()
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOS]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetOS](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID as OSID, Name as OSName, Count as OSCount
	FROM OS
	where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPCByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPCByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetPCByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	Name, 
	Cost, 
	MotherBoard, 
	CPUModel, 
	NumberOfCores, 
	FrequencyProcessor, 
	MaxFrequencyProcessor, 
	RAMGB, 
	FrequencyRAM, 
	VideoCard, 
	VideoRAMGB, 
	SSDCapacityGB, 
	HDDCapacityGB, 
	(select OSCount from dbo.GetOS(OSID)) as OSCount, 
	dbo.GetNameOS(OSID) as OSName, 
	OSID,
	VideoConnectors, 
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber, 
	InvoiceID,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from PC where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetSoftwareOnPC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSoftwareOnPC]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetSoftwareOnPC](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct LicenseID, dbo.GetSoftwareName(LicenseID) as SoftwareName, dbo.GetInstalledCount(LicenseID) as InstalledCount From InstalledSoftwarePC where PCID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNotInstalledOnPC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNotInstalledOnPC]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetNotInstalledOnPC](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name, dbo.GetInstalledCount(ID) as InstalledCount FROM LicenseSoftware Where ID not in (select LicenseID From dbo.GetSoftwareOnPC(@ID))
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetSoftwareOnNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSoftwareOnNotebook]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetSoftwareOnNotebook](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct LicenseID, dbo.GetSoftwareName(LicenseID) as SoftwareName, dbo.GetInstalledCount(LicenseID) as InstalledCount From InstalledSoftwareNotebook where NotebookID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNotInstalledOnNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNotInstalledOnNotebook]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetNotInstalledOnNotebook](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name, dbo.GetInstalledCount(ID) as InstalledCount FROM LicenseSoftware Where ID not in (select LicenseID From dbo.GetSoftwareOnNotebook(@ID))
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllAspectRatio]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllAspectRatio]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllAspectRatio]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM AspectRatio
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllAudience]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllAudience]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllAudience]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM Audience
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllBoard]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllBoard]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllBoard] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	Diagonal as ''Диагональ'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from InteractiveWhiteboard
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllBoardForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllBoardForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllBoardForReport] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	Diagonal as ''Диагональ'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from InteractiveWhiteboard
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllBoardName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllBoardName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllBoardName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM InteractiveWhiteboard
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllBoardWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllBoardWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllBoardWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(4) as TableName, InventoryNumber, N''Интерактивная доска'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from InteractiveWhiteboard
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllCanUsedLocationByTypeDeviceID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllCanUsedLocationByTypeDeviceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllCanUsedLocationByTypeDeviceID]
(	
	@TypeDeviceID int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Place.ID, dbo.GetLocalion(AudienceTable.ID)+case Place.CanUsed when 1 then N'' (свободно)'' else N'' (занято)'' end as N''Place'' 
	FROM AudienceTable inner join Place on Place.AudienceTableID=AudienceTable.ID
	where Place.TypeDeviceID=@TypeDeviceID 
	--and Place.CanUsed=1
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllDevices]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllDevices]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllDevices] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Интерактивная доска'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from InteractiveWhiteboard
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Монитор'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Monitor
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNetworkSwitch(TypeID) as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from NetworkSwitch
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Notebook
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N'''' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from OtherEquipment
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PrinterScanner
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'',
	N''Проектор'' as ''Тип'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Projector
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Экран для проектора'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from ProjectorScreen
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union

	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Компьютер'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PC
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllDevicesWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllDevicesWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllDevicesWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(4) as TableName, InventoryNumber, N''Интерактивная доска'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from InteractiveWhiteboard
		union
		select ID, dbo.GetTableName(6) as TableName, InventoryNumber, N''Монитор'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Monitor
		union
		select ID, dbo.GetTableName(5) as TableName, InventoryNumber, dbo.GetTypeNetworkSwitch(TypeID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from NetworkSwitch
		union
		select ID, dbo.GetTableName(2) as TableName, InventoryNumber, dbo.GetTypeNotebook(TypeNotebookID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Notebook
		union
		select ID, dbo.GetTableName(1) as TableName, InventoryNumber, N''Компьютер'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from PC
		union
		select ID, dbo.GetTableName(3) as TableName, InventoryNumber, dbo.GetTypePrinter(TypePrinterID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from PrinterScanner
		union
		select ID, dbo.GetTableName(7) as TableName, InventoryNumber, N''Проектор'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Projector
		union
		select ID, dbo.GetTableName(8) as TableName, InventoryNumber, N''Экран для проектора'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from ProjectorScreen
		union
		select ID, dbo.GetTableName(9) as TableName, InventoryNumber, N''Прочее'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from OtherEquipment
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllFrequency]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllFrequency]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllFrequency]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM Frequency
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllImages]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllImages]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllImages]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Image
	FROM Image
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllInvoice]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllInvoice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllInvoice]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Number, Date
	FROM Invoice
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllLocation]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllLocation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllLocation]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT Place.ID, dbo.GetLocalion(AudienceTable.ID) as N''Место'', dbo.GetTypeDevice(Place.TypeDeviceID) as N''Тип''
	FROM AudienceTable inner join Place on Place.AudienceTableID=AudienceTable.ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllLocationByTypeDeviceID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllLocationByTypeDeviceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllLocationByTypeDeviceID]
(	
	@TypeDeviceID int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT Place.ID, dbo.GetLocalion(AudienceTable.ID) as N''Place'' 
	FROM AudienceTable inner join Place on Place.AudienceTableID=AudienceTable.ID
	where Place.TypeDeviceID=@TypeDeviceID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllLocationInAudience]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllLocationInAudience]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllLocationInAudience](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct AudienceTable.ID, dbo.[GetAudienceTableName](AudienceTable.ID) as N''Место'', dbo.[GetDevicesInLocation](AudienceTable.ID) as N''Техника'', AudienceTable.Name
	FROM AudienceTable inner join Place on Place.AudienceTableID=AudienceTable.ID where AudienceID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMatrixTechnology]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMatrixTechnology]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllMatrixTechnology]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM MatrixTechnology
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMonitor]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMonitor]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllMonitor] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	str(dbo.GetFrequency(FrequencyID)) + N'' Гц'' as ''Частота обновления'', 
	dbo.GetMatrix(MatrixTechnologyID) as ''Технология изготовления матрицы'', 
	VideoConnectors,
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from Monitor
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMonitorForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMonitorForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllMonitorForReport]
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	dbo.GetFrequency(FrequencyID) as ''Частота обновления'', 
	dbo.GetMatrix(MatrixTechnologyID) as ''Технология изготовления матрицы'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Monitor
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMonitorName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMonitorName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllMonitorName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM Monitor
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMonitorWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMonitorWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllMonitorWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(6) as TableName, InventoryNumber, N''Монитор'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Monitor
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMotherboard]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMotherboard]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllMotherboard]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Motherboard
	FROM PC where Motherboard != '''' and Motherboard is not null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNetworkSwitch]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNetworkSwitch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllNetworkSwitch] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNetworkSwitch(TypeID) as ''Тип'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	NumberOfPorts as ''Количество портов'', 
	dbo.GetWiFiFrequency(WiFiFrequencyID) as ''Частота Wi-Fi'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from NetworkSwitch
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNetworkSwitchForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNetworkSwitchForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllNetworkSwitchForReport]
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNetworkSwitch(TypeID) as ''Тип'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	NumberOfPorts as ''Количество портов'', 
	dbo.GetWiFiFrequency(WiFiFrequencyID) as ''Частота Wi-Fi'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from NetworkSwitch
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNetworkSwitchName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNetworkSwitchName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllNetworkSwitchName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM NetworkSwitch
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNetworkSwitchWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNetworkSwitchWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllNetworkSwitchWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(5) as TableName, InventoryNumber, dbo.GetTypeNetworkSwitch(TypeID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from NetworkSwitch
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNotebook]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllNotebook] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	CPUModel as ''Процессор'', 
	NumberOfCores as ''Количество ядер'', 
	str(FrequencyProcessor) + N'' МГц'' as ''Базовая частота'', 
	str(MaxFrequencyProcessor) + N'' МГц'' as ''Максимальная частота'', 
	str(RAMGB, 3, 0) + N'' ГБ'' as ''ОЗУ'', 
	str(FrequencyRAM) + N'' МГц'' as ''Частота памяти'', 
	str(SSDCapacityGB, 4, 1) + N'' ГБ'' as ''Объем SSD'', 
	str(HDDCapacityGB, 4, 1) + N'' ГБ'' as ''Объем HDD'', 
	VideoCard as ''Видеокарта'',
	str(VideoRAMGB, 3, 0) + N'' ГБ'' as ''Видеопамять'',  
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	str(dbo.GetFrequency(FrequencyID)) + N'' Гц'' as ''Частота обновления'', 
	dbo.GetMatrix(MatrixTechnologyID) as ''Технология изготовления матрицы'', 
	VideoConnectors, 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from Notebook
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebookCPU]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNotebookCPU]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllNotebookCPU]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct CPUModel, FrequencyProcessor, MaxFrequencyProcessor, NumberOfCores
	FROM Notebook Where CPUModel != '''' and CPUModel is not null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebookForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNotebookForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllNotebookForReport] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT 
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	CPUModel as ''Процессор'', 
	NumberOfCores as ''Количество ядер'', 
	FrequencyProcessor as ''Базовая частота'', 
	MaxFrequencyProcessor as ''Максимальная частота'', 
	RAMGB as ''ОЗУ'', 
	FrequencyRAM as ''Частота памяти'', 
	SSDCapacityGB as ''Объем SSD'', 
	HDDCapacityGB as ''Объем HDD'', 
	VideoCard as ''Видеокарта'',
	VideoRAMGB as ''Видеопамять'',  
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	dbo.GetFrequency(FrequencyID) as ''Частота обновления'', 
	dbo.GetMatrix(MatrixTechnologyID) as ''Технология изготовления матрицы'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Notebook
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebookName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNotebookName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllNotebookName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM Notebook
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebookvCard]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNotebookvCard]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllNotebookvCard]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct VideoCard
	FROM Notebook Where VideoCard != '''' and VideoCard is not null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebookWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllNotebookWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllNotebookWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(2) as TableName, InventoryNumber, dbo.GetTypeNotebook(TypeNotebookID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Notebook
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOS]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllOS]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	Name as Наименование, 
	Price as Цена, 
	Count as Количество, 
	TotalCost as N''Общая стоимость'', 
	dbo.GetNumberInvoice(InvoiceID) as N''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	InvoiceID
	FROM OS
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOSForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOSForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllOSForReport]
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	Name as Наименование, 
	Price as Цена, 
	Count as Количество, 
	TotalCost as N''Общая стоимость'', 
	dbo.GetNumberInvoice(InvoiceID) as N''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения''
	FROM OS
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOtherEquipment]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOtherEquipment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllOtherEquipment] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from OtherEquipment
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOtherEquipmentForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOtherEquipmentForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllOtherEquipmentForReport] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from OtherEquipment
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOtherEquipmentName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOtherEquipmentName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllOtherEquipmentName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM OtherEquipment
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOtherEquipmentWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOtherEquipmentWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllOtherEquipmentWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(9) as TableName, InventoryNumber, N''Прочее'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from OtherEquipment
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPaperSize]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPaperSize]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllPaperSize]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM PaperSize
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPC]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllPC] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	MotherBoard as ''Материнская плата'', 
	CPUModel as ''Процессор'', 
	NumberOfCores as ''Количество ядер'', 
	str(FrequencyProcessor) + N'' МГц'' as ''Базовая частота'', 
	str(MaxFrequencyProcessor) + N'' МГц'' as ''Максимальная частота'', 
	str(RAMGB, 3, 0) + N'' ГБ'' as ''ОЗУ'', 
	str(FrequencyRAM) + N'' МГц'' as ''Частота памяти'', 
	VideoCard as ''Видеокарта'', 
	str(VideoRAMGB, 3, 0) + N'' ГБ'' as ''Видеопамять'', 
	str(SSDCapacityGB, 4, 1) + N'' ГБ'' as ''Объем SSD'', 
	str(HDDCapacityGB, 4, 1) + N'' ГБ'' as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	VideoConnectors, 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from PC
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPCCPU]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPCCPU]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllPCCPU]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct CPUModel, FrequencyProcessor, MaxFrequencyProcessor, NumberOfCores
	FROM PC Where CPUModel != '''' and CPUModel is not null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPCForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPCForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllPCForReport] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	MotherBoard as ''Материнская плата'', 
	CPUModel as ''Процессор'', 
	NumberOfCores as ''Количество ядер'', 
	FrequencyProcessor as ''Базовая частота'', 
	MaxFrequencyProcessor as ''Максимальная частота'', 
	RAMGB as ''ОЗУ'', 
	FrequencyRAM as ''Частота памяти'', 
	VideoCard as ''Видеокарта'', 
	VideoRAMGB as ''Видеопамять'', 
	SSDCapacityGB as ''Объем SSD'', 
	HDDCapacityGB as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PC
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPCName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPCName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllPCName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM PC
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPCvCard]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPCvCard]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllPCvCard]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct VideoCard
	FROM PC Where VideoCard != '''' and VideoCard is not null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPCWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPCWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllPCWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(1) as TableName, InventoryNumber, N''Компьютер'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from PC
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPrinterScanner]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPrinterScanner]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllPrinterScanner] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	dbo.GetPaperSize(PaperSizeID) as ''Максимальный формат'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from PrinterScanner
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPrinterScannerForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPrinterScannerForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllPrinterScannerForReport]
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetPaperSize(PaperSizeID) as ''Максимальный формат'',  
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PrinterScanner
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPrinterScannerName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPrinterScannerName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllPrinterScannerName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM PrinterScanner
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPrinterScannerWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPrinterScannerWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllPrinterScannerWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(3) as TableName, InventoryNumber, dbo.GetTypePrinter(TypePrinterID) as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from PrinterScanner
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjector]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjector]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllProjector] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	MaxDiagonal as ''Максимальная диагональ'', 
	dbo.GetProjectorTechnology(ProjectorTechnologyID) as ''Технология проецирования'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	VideoConnectors, 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from Projector
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjectorForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllProjectorForReport] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	MaxDiagonal as ''Максимальная диагональ'', 
	dbo.GetProjectorTechnology(ProjectorTechnologyID) as ''Технология проецирования'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Projector
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjectorName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllProjectorName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM Projector
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorScreenName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjectorScreenName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllProjectorScreenName]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT distinct Name
	FROM ProjectorScreen
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorScreenWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjectorScreenWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllProjectorScreenWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(8) as TableName, InventoryNumber, N''Экран для проектора'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from ProjectorScreen
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorTechnology]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjectorTechnology]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllProjectorTechnology]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM ProjectorTechnology
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorWithFullName]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProjectorWithFullName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllProjectorWithFullName] ()
RETURNS TABLE 
AS
RETURN 
(
	select q.ID, TableName, InventoryNumber, Type, q.Name, AudienceTableID, ImageID, Type+N'' ''+q.Name+N'' №''+Format(InventoryNumber, ''000000000000000'')+Case when AudienceTableID is null then N'' (свободный)'' else N'''' end as FullName, PlaceID from (
		select ID, dbo.GetTableName(7) as TableName, InventoryNumber, N''Проектор'' as Type, Name, dbo.[GetAudienceTableID](PlaceID) as AudienceTableID, ImageID, PlaceID from Projector
	) as q
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllResolution]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllResolution]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllResolution]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM Resolution
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllScreen]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllScreen]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetAllScreen] ()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	Diagonal as ''Диагональ'', 
	IsElectronicDrive as ''С электроприводом'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAspectRatio(AspectRatioID) as ''Соотношение сторон'', 
	dbo.GetScreeninstalled(ScreenInstalledID) as ''Установка'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние'',
	IsWorkingCondition,
	InvoiceID,
	ImageID,
	PlaceID
	from ProjectorScreen
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllScreenForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllScreenForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllScreenForReport] 
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	Diagonal as ''Диагональ'', 
	IsElectronicDrive as ''С электроприводом'', 
	dbo.GetAspectRatio(AspectRatioID) as ''Соотношение сторон'', 
	dbo.GetScreeninstalled(ScreenInstalledID) as ''Установка экрана'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from ProjectorScreen
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null

)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllScreenInstalled]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllScreenInstalled]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllScreenInstalled]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM ScreenInstalled
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllSoftware]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllSoftware]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllSoftware]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	Name as Наименование, 
	Price as Цена, 
	Count as Количество, 
	TotalCost as N''Общая стоимость'',
	dbo.[GetTypeSoftwareLicense](Type) as N''Тип лицензии'', 
	dbo.GetNumberInvoice(InvoiceID) as N''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	InvoiceID
	FROM LicenseSoftware
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllSoftwareAndOSForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllSoftwareAndOSForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllSoftwareAndOSForReport]
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	N''ПП'' as N''Тип'',
	Name as Наименование, 
	Price as Цена, 
	Count as Количество, 
	TotalCost as N''Общая стоимость'', 
	dbo.GetNumberInvoice(InvoiceID) as N''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.[GetTypeSoftwareLicense](Type) as N''Тип лицензии''
	FROM LicenseSoftware
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
	union
	SELECT
	N''ОС'' as N''Тип'',
	Name as Наименование, 
	Price as Цена, 
	Count as Количество, 
	TotalCost as N''Общая стоимость'', 
	dbo.GetNumberInvoice(InvoiceID) as N''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	null as N''Тип лицензии''
	FROM OS
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllSoftwareForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllSoftwareForReport]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllSoftwareForReport]
(@From date = ''2000-01-01'', @To date = ''2099-12-31'')
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	Name as Наименование, 
	Price as Цена, 
	Count as Количество, 
	TotalCost as N''Общая стоимость'', 
	dbo.GetNumberInvoice(InvoiceID) as N''Номер накладной'',
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.[GetTypeSoftwareLicense](Type) as N''Тип лицензии''
	FROM LicenseSoftware
	where dbo.[GetAcquisitionDate](InvoiceID) between @From and @To or InvoiceID is null
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypeDevice]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypeDevice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllTypeDevice]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name, RussianName, RussianNamePlural
	FROM TypeDevice
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypeNetworkSwitch]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypeNetworkSwitch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetAllTypeNetworkSwitch]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM TypeNetworkSwitch
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypeNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypeNotebook]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllTypeNotebook]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM TypeNotebook
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypePrinter]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypePrinter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllTypePrinter]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM TypePrinter
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllTypeSoftLicense]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTypeSoftLicense]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllTypeSoftLicense]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM TypeSoftLicense
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllVideoConnector]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllVideoConnector]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetAllVideoConnector]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name, Value
	FROM VideoConnector
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllWiFiFrequency]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllWiFiFrequency]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetAllWiFiFrequency]()
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name
	FROM WiFiFrequency
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetBoardByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBoardByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetBoardByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	Name, 
	Cost, 
	Diagonal, 
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber,
	InvoiceID, 
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from InteractiveWhiteboard 
	where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetLicenseSoftwareByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLicenseSoftwareByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetLicenseSoftwareByID](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name, Price, Count, dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber, Type
	FROM LicenseSoftware
	where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetMonitorByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMonitorByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetMonitorByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	Name, 
	Cost, 
	ScreenDiagonal, 
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber,
	InvoiceID, 
	dbo.GetResolution(ResolutionID) as Resolution,
	ResolutionID, 
	dbo.GetFrequency(FrequencyID) as Frequency,
	FrequencyID, 
	dbo.GetMatrix(MatrixTechnologyID) as MatrixTechnology,
	MatrixTechnologyID, 
	VideoConnectors,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from Monitor where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNetworkSwitchByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNetworkSwitchByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetNetworkSwitchByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	TypeID, 
	Name, 
	Cost, 
	NumberOfPorts, 
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber,
	InvoiceID,
	dbo.GetWiFiFrequency(WiFiFrequencyID) as WiFiFrequency,
	WiFiFrequencyID, 
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from NetworkSwitch where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNotebookByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNotebookByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetNotebookByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	dbo.GetTypeNotebook(TypeNotebookID) as Type, 
	TypeNotebookID, 
	Name, 
	Cost, 
	CPUModel, 
	NumberOfCores, 
	FrequencyProcessor, 
	MaxFrequencyProcessor, 
	RAMGB, 
	FrequencyRAM, 
	SSDCapacityGB, 
	HDDCapacityGB, 
	VideoCard,
	VideoRAMGB,  
	ScreenDiagonal, 
	dbo.GetResolution(ResolutionID) as Resolution, 
	ResolutionID,
	dbo.GetFrequency(FrequencyID) as Frequency,
	FrequencyID,
	dbo.GetMatrix(MatrixTechnologyID) as Matrix, 
	MatrixTechnologyID,
	VideoConnectors, 
	dbo.GetNameOS(OSID) as OSName, 
	OSID,
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber,
	InvoiceID,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from Notebook where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOSByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOSByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetOSByID](@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, Name, Price, Count, dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber
	FROM OS
	where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOtherEquipmentByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOtherEquipmentByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetOtherEquipmentByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	Name, 
	Cost, 
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber, 
	InvoiceID,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from OtherEquipment where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPrinterScannerByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPrinterScannerByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetPrinterScannerByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	dbo.GetTypePrinter(TypePrinterID) as Type, 
	TypePrinterID,
	InventoryNumber, 
	Name, 
	Cost, 
	dbo.GetPaperSize(PaperSizeID) as PaperSize, 
	PaperSizeID,
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber, 
	InvoiceID,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from PrinterScanner where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetProjectorByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProjectorByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetProjectorByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	Name, 
	Cost, 
	MaxDiagonal, 
	dbo.GetProjectorTechnology(ProjectorTechnologyID) as ProjectorTechnology, 
	ProjectorTechnologyID,
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber,
	InvoiceID,
	dbo.GetResolution(ResolutionID) as Resolution, 
	ResolutionID,
	VideoConnectors,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from Projector where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetScreenByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScreenByID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetScreenByID] (@ID int)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID, 
	InventoryNumber, 
	Name, 
	Cost, 
	Diagonal, 
	IsElectronicDrive, 
	dbo.GetNumberInvoice(InvoiceID) as InvoiceNumber, 
	InvoiceID,
	dbo.GetAspectRatio(AspectRatioID) as AspectRatio, 
	AspectRatioID,
	dbo.GetScreeninstalled(ScreenInstalledID) as ScreenInstalled, 
	ScreenInstalledID,
	dbo.GetLocalion(PlaceID) as Location,
	PlaceID, ImageID
	from ProjectorScreen where ID=@ID
)
' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_InteractiveWhiteboard_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InteractiveWhiteboard] ADD  CONSTRAINT [DF_InteractiveWhiteboard_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Journal_Date]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Journal] ADD  CONSTRAINT [DF_Journal_Date]  DEFAULT (getdate()) FOR [Date]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LicenseSoftware_Count]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LicenseSoftware] ADD  CONSTRAINT [DF_LicenseSoftware_Count]  DEFAULT ((1)) FOR [Count]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Monitor_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Monitor] ADD  CONSTRAINT [DF_Monitor_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_NetworkSwitch_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NetworkSwitch] ADD  CONSTRAINT [DF_NetworkSwitch_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Notebook_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Notebook] ADD  CONSTRAINT [DF_Notebook_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_OtherEquipment_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OtherEquipment] ADD  CONSTRAINT [DF_OtherEquipment_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_PC_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PC] ADD  CONSTRAINT [DF_PC_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_PrinterScanner_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PrinterScanner] ADD  CONSTRAINT [DF_PrinterScanner_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Projector_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Projector] ADD  CONSTRAINT [DF_Projector_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_ProjectorScreen_IsWorkingCondition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ProjectorScreen] ADD  CONSTRAINT [DF_ProjectorScreen_IsWorkingCondition]  DEFAULT ((1)) FOR [IsWorkingCondition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AudienceTable_Audience]') AND parent_object_id = OBJECT_ID(N'[dbo].[AudienceTable]'))
ALTER TABLE [dbo].[AudienceTable]  WITH CHECK ADD  CONSTRAINT [FK_AudienceTable_Audience] FOREIGN KEY([AudienceID])
REFERENCES [dbo].[Audience] ([ID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AudienceTable_Audience]') AND parent_object_id = OBJECT_ID(N'[dbo].[AudienceTable]'))
ALTER TABLE [dbo].[AudienceTable] CHECK CONSTRAINT [FK_AudienceTable_Audience]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwareNotebook_LicenseSoftware] FOREIGN KEY([LicenseID])
REFERENCES [dbo].[LicenseSoftware] ([ID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook] CHECK CONSTRAINT [FK_InstalledSoftwareNotebook_LicenseSoftware]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_Notebook]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwareNotebook_Notebook] FOREIGN KEY([NotebookID])
REFERENCES [dbo].[Notebook] ([ID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_Notebook]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook] CHECK CONSTRAINT [FK_InstalledSoftwareNotebook_Notebook]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwarePC_LicenseSoftware] FOREIGN KEY([LicenseID])
REFERENCES [dbo].[LicenseSoftware] ([ID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC] CHECK CONSTRAINT [FK_InstalledSoftwarePC_LicenseSoftware]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_PC]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwarePC_PC] FOREIGN KEY([PCID])
REFERENCES [dbo].[PC] ([ID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_PC]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC] CHECK CONSTRAINT [FK_InstalledSoftwarePC_PC]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard]  WITH CHECK ADD  CONSTRAINT [FK_InteractiveWhiteboard_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard] CHECK CONSTRAINT [FK_InteractiveWhiteboard_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard]  WITH CHECK ADD  CONSTRAINT [FK_InteractiveWhiteboard_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard] CHECK CONSTRAINT [FK_InteractiveWhiteboard_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard]  WITH CHECK ADD  CONSTRAINT [FK_InteractiveWhiteboard_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard] CHECK CONSTRAINT [FK_InteractiveWhiteboard_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseSoftware_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseSoftware]'))
ALTER TABLE [dbo].[LicenseSoftware]  WITH CHECK ADD  CONSTRAINT [FK_LicenseSoftware_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseSoftware_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseSoftware]'))
ALTER TABLE [dbo].[LicenseSoftware] CHECK CONSTRAINT [FK_LicenseSoftware_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseSoftware_TypeSoftLicense]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseSoftware]'))
ALTER TABLE [dbo].[LicenseSoftware]  WITH CHECK ADD  CONSTRAINT [FK_LicenseSoftware_TypeSoftLicense] FOREIGN KEY([Type])
REFERENCES [dbo].[TypeSoftLicense] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseSoftware_TypeSoftLicense]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseSoftware]'))
ALTER TABLE [dbo].[LicenseSoftware] CHECK CONSTRAINT [FK_LicenseSoftware_TypeSoftLicense]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Frequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Frequency] FOREIGN KEY([FrequencyID])
REFERENCES [dbo].[Frequency] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Frequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Frequency]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH NOCHECK ADD  CONSTRAINT [FK_Monitor_MatrixTechnology] FOREIGN KEY([MatrixTechnologyID])
REFERENCES [dbo].[MatrixTechnology] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] NOCHECK CONSTRAINT [FK_Monitor_MatrixTechnology]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Resolution] FOREIGN KEY([ResolutionID])
REFERENCES [dbo].[Resolution] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Resolution]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch] CHECK CONSTRAINT [FK_NetworkSwitch_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch] CHECK CONSTRAINT [FK_NetworkSwitch_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch] CHECK CONSTRAINT [FK_NetworkSwitch_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_TypeNetworkSwitch]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_TypeNetworkSwitch] FOREIGN KEY([TypeID])
REFERENCES [dbo].[TypeNetworkSwitch] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_TypeNetworkSwitch]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch] CHECK CONSTRAINT [FK_NetworkSwitch_TypeNetworkSwitch]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_WiFiFrequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_WiFiFrequency] FOREIGN KEY([WiFiFrequencyID])
REFERENCES [dbo].[WiFiFrequency] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_WiFiFrequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch] CHECK CONSTRAINT [FK_NetworkSwitch_WiFiFrequency]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Frequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Frequency] FOREIGN KEY([FrequencyID])
REFERENCES [dbo].[Frequency] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Frequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Frequency]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_MatrixTechnology] FOREIGN KEY([MatrixTechnologyID])
REFERENCES [dbo].[MatrixTechnology] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_MatrixTechnology]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_OS] FOREIGN KEY([OSID])
REFERENCES [dbo].[OS] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_OS]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Resolution] FOREIGN KEY([ResolutionID])
REFERENCES [dbo].[Resolution] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Resolution]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_TypeNotebook]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_TypeNotebook] FOREIGN KEY([TypeNotebookID])
REFERENCES [dbo].[TypeNotebook] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_TypeNotebook]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_TypeNotebook]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OS_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OS]'))
ALTER TABLE [dbo].[OS]  WITH CHECK ADD  CONSTRAINT [FK_OS_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OS_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OS]'))
ALTER TABLE [dbo].[OS] CHECK CONSTRAINT [FK_OS_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment]  WITH CHECK ADD  CONSTRAINT [FK_OtherEquipment_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment] CHECK CONSTRAINT [FK_OtherEquipment_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment]  WITH CHECK ADD  CONSTRAINT [FK_OtherEquipment_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment] CHECK CONSTRAINT [FK_OtherEquipment_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment]  WITH CHECK ADD  CONSTRAINT [FK_OtherEquipment_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment] CHECK CONSTRAINT [FK_OtherEquipment_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_OS] FOREIGN KEY([OSID])
REFERENCES [dbo].[OS] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_OS]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_AudienceTable]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_AudienceTable] FOREIGN KEY([AudienceTableID])
REFERENCES [dbo].[AudienceTable] ([ID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_AudienceTable]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place] CHECK CONSTRAINT [FK_Place_AudienceTable]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_TypeDevice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_TypeDevice] FOREIGN KEY([TypeDeviceID])
REFERENCES [dbo].[TypeDevice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_TypeDevice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place] CHECK CONSTRAINT [FK_Place_TypeDevice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner]  WITH CHECK ADD  CONSTRAINT [FK_PrinterScanner_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner] CHECK CONSTRAINT [FK_PrinterScanner_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner]  WITH CHECK ADD  CONSTRAINT [FK_PrinterScanner_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner] CHECK CONSTRAINT [FK_PrinterScanner_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_PaperSize]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner]  WITH CHECK ADD  CONSTRAINT [FK_PrinterScanner_PaperSize] FOREIGN KEY([PaperSizeID])
REFERENCES [dbo].[PaperSize] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_PaperSize]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner] CHECK CONSTRAINT [FK_PrinterScanner_PaperSize]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner]  WITH CHECK ADD  CONSTRAINT [FK_PrinterScanner_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner] CHECK CONSTRAINT [FK_PrinterScanner_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_TypePrinter]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner]  WITH CHECK ADD  CONSTRAINT [FK_PrinterScanner_TypePrinter] FOREIGN KEY([TypePrinterID])
REFERENCES [dbo].[TypePrinter] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_TypePrinter]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner] CHECK CONSTRAINT [FK_PrinterScanner_TypePrinter]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Image] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Image]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector] CHECK CONSTRAINT [FK_Projector_Image]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector] CHECK CONSTRAINT [FK_Projector_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector] CHECK CONSTRAINT [FK_Projector_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_ProjectorTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_ProjectorTechnology] FOREIGN KEY([ProjectorTechnologyID])
REFERENCES [dbo].[ProjectorTechnology] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_ProjectorTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector] CHECK CONSTRAINT [FK_Projector_ProjectorTechnology]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_Resolution] FOREIGN KEY([ResolutionID])
REFERENCES [dbo].[Resolution] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector] CHECK CONSTRAINT [FK_Projector_Resolution]
GO
/****** Object:  StoredProcedure [dbo].[AddAudienceTable]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAudienceTable]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddAudienceTable] AS' 
END
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[AddAudienceTable]
	@AudienceID int,
	@TableID int out
AS
BEGIN
	SET NOCOUNT ON;
	Insert into AudienceTable(AudienceID) Values (@AudienceID)

	SELECT @TableID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[AddImage]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddImage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddImage] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddImage]
	@Image varbinary(max),
	@ID int out
AS
BEGIN
	SET NOCOUNT ON;
	if not exists (select * from Image where Image=@Image)
	begin
		Insert into Image Values (@Image)

		SELECT @ID = SCOPE_IDENTITY()
	end
	else
	begin
		IF @Image is not null
		BEGIN
			SET @ID=NULL
		END
		ELSE
		BEGIN
			SELECT @ID = ID From Image where Image=@Image
		END
	end
END
GO
/****** Object:  StoredProcedure [dbo].[AddInstalledSoftwareNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddInstalledSoftwareNotebook]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddInstalledSoftwareNotebook] AS' 
END
GO

ALTER PROCEDURE [dbo].[AddInstalledSoftwareNotebook] 
	@NotebookID int,
	@LicenseID int
AS
BEGIN
	SET NOCOUNT ON;
	Insert into InstalledSoftwareNotebook (NotebookID, LicenseID) values (@NotebookID, @LicenseID)
END
GO
/****** Object:  StoredProcedure [dbo].[AddInstalledSoftwarePC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddInstalledSoftwarePC]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddInstalledSoftwarePC] AS' 
END
GO

ALTER PROCEDURE [dbo].[AddInstalledSoftwarePC] 
	@PCID int,
	@LicenseID int
AS
BEGIN
	SET NOCOUNT ON;
	Insert into InstalledSoftwarePC (PCID, LicenseID) values (@PCID, @LicenseID)
END
GO
/****** Object:  StoredProcedure [dbo].[AddInteractiveWhiteboard]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddInteractiveWhiteboard]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddInteractiveWhiteboard] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddInteractiveWhiteboard] 
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@Diagonal real = NULL,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = null
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
		
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber
	Insert into InteractiveWhiteboard 
	(InventoryNumber, Name, Cost, InvoiceID, Diagonal, PlaceID, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, @Diagonal, @PlaceID, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddInvoice]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddInvoice]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddInvoice] AS' 
END
GO

ALTER PROCEDURE [dbo].[AddInvoice] 
	@Number nvarchar(50),
	@Date date = null
AS
BEGIN
	SET NOCOUNT ON;
	if (@Date is null)
	begin
		select @Date = CONVERT(date, GETDATE())
	end
	if (not exists (select * from Invoice where Invoice.Number=@Number) and @Number is not null)
	begin
		insert into [dbo].[Invoice] ([Number], [Date]) Values (@Number, @Date)
	end
END
GO
/****** Object:  StoredProcedure [dbo].[AddLicenseSoftware]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddLicenseSoftware]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddLicenseSoftware] AS' 
END
GO

ALTER PROCEDURE [dbo].[AddLicenseSoftware] 
	@Name nvarchar(100),
	@Price money,
	@Count int,
	@InvoiceNumber nvarchar(50) = null,
	@Type int
AS
BEGIN
	SET NOCOUNT ON;
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into LicenseSoftware (Name, Price, Count, InvoiceID, Type) values (@Name, @Price, @Count, @InvoiceID, @Type)
END
GO
/****** Object:  StoredProcedure [dbo].[AddLocation]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddLocation]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddLocation] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddLocation]
	@TypeDeviceID int,
	@AudienceTableID int,
	@PlaceID int out
AS
BEGIN
	SET NOCOUNT ON;
	Insert into Place (TypeDeviceID, AudienceTableID) Values (@TypeDeviceID, @AudienceTableID)

	SELECT @PlaceID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[AddMonitor]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddMonitor]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddMonitor] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddMonitor]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@Diagonal real = NULL,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@MatrixID int = NULL,
	@FrequencyID int = NULL,
	@ResolutionID int = NULL,
	@VConnectors int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
		
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into Monitor (InventoryNumber, Name, Cost, InvoiceID, ScreenDiagonal, PlaceID, ResolutionID, FrequencyID, MatrixTechnologyID, VideoConnectors, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, @Diagonal, @PlaceID, @ResolutionID, @FrequencyID, @MatrixID, @VConnectors, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNetworkSwitch]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddNetworkSwitch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddNetworkSwitch] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddNetworkSwitch]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@Ports int = NULL,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@TypeID int,
	@Frequency int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
			
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into NetworkSwitch 
	(InventoryNumber, Name, Cost, InvoiceID, NumberOfPorts, PlaceID, TypeID, WiFiFrequencyID, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, @Ports, @PlaceID, @TypeID, @Frequency, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddNotebook]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddNotebook] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddNotebook]
	@InvN int,
	@Type int,
	@Name nvarchar(200),
	@Cost money,
	@Diagonal real = NULL,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@CPU nvarchar(20) = NULL,
	@RAM int = Null,
	@SSD int = Null,
	@HDD int = Null,
	@Video nvarchar(30) = Null,
	@OSID int = null,
	@ResolutionID int = NULL,
	@FrequencyID int = NULL,
	@MatrixID int = NULL,
	@VConnectors int = NULL,
	@Cores int = NULL,
	@Frequency int = NULL,
	@MaxFrequency int = NULL,
	@VRAM int = NULL,
	@FrequencyRAM int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
			
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into Notebook 
	(TypeNotebookID, InventoryNumber, Name, Cost, InvoiceID, ScreenDiagonal, 
	PlaceID, CPUModel, RAMGB, SSDCapacityGB, HDDCapacityGB, VideoCard, OSID, ResolutionID, 
	FrequencyID, MatrixTechnologyID, VideoConnectors, NumberOfCores, FrequencyProcessor,
	MaxFrequencyProcessor, VideoRAMGB, FrequencyRAM, ImageID, IsWorkingCondition) 
	values (@Type, @InvN, @Name, @Cost, @InvoiceID, @Diagonal, 
	@PlaceID, @CPU, @RAM, @SSD, @HDD, @Video, @OSID, @ResolutionID, 
	@FrequencyID, @MatrixID, @VConnectors, @Cores, @Frequency, @MaxFrequency, 
	@VRAM, @FrequencyRAM, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddOS]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddOS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddOS] AS' 
END
GO

ALTER PROCEDURE [dbo].[AddOS] 
	@Name nvarchar(100),
	@Price money,
	@Count int,
	@InvoiceNumber nvarchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into OS (Name, Price, Count, InvoiceID) values (@Name, @Price, @Count, @InvoiceID)
END
GO
/****** Object:  StoredProcedure [dbo].[AddOtherEquipment]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddOtherEquipment]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddOtherEquipment] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddOtherEquipment]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
			
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into OtherEquipment 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, @PlaceID, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddPC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPC]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddPC] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddPC]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@CPU nvarchar(20) = NULL,
	@RAM int = Null,
	@HDD int = Null,
	@SSD int = Null,
	@Video nvarchar(30) = Null,
	@OSID int = null,
	@MB nvarchar(30) = null,
	@Cores int = NULL,
	@Frequency int = NULL,
	@MaxFrequency int = NULL,
	@VRAM int = NULL,
	@FrequencyRAM int = NULL,
	@VConnectors int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
		
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into PC (InventoryNumber, Name, Cost, InvoiceID, PlaceID, 
	CPUModel, RAMGB, SSDCapacityGB, HDDCapacityGB, VideoCard, OSID, Motherboard,
	NumberOfCores, FrequencyProcessor, MaxFrequencyProcessor, VideoRAMGB, 
	FrequencyRAM, VideoConnectors, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, 
	@PlaceID, @CPU, @RAM, @SSD, @HDD, @Video, @OSID, @MB,
	@Cores, @Frequency, @MaxFrequency, @VRAM, @FrequencyRAM, @VConnectors, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddPrinterScanner]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPrinterScanner]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddPrinterScanner] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddPrinterScanner]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@PaperSizeID int = null,
	@TypeID int,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
			
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into PrinterScanner 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, TypePrinterID, PaperSizeID, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, @PlaceID, 
	@TypeID, @PaperSizeID, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddProjector]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddProjector]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddProjector] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddProjector]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@Diagonal real = null,
	@TechnologyID int = null,
	@VConnectors int = NULL,
	@ResolutionID int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
			
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into Projector 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, MaxDiagonal, 
	ProjectorTechnologyID, VideoConnectors, ResolutionID, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, 
	@PlaceID, @Diagonal, @TechnologyID, @VConnectors, @ResolutionID, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[AddProjectorScreen]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddProjectorScreen]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddProjectorScreen] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddProjectorScreen]
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@Diagonal real = null,
	@IsElectronic bit = null,
	@AspectRatioID int = NULL,
	@InstalledID int = NULL,
	@IsWorkingCondition bit = NULL,
	@Image varbinary(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		set @ImageID = null
			
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Insert into ProjectorScreen 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, Diagonal, IsElectronicDrive, AspectRatioID, ScreenInstalledID, ImageID, IsWorkingCondition) 
	values (@InvN, @Name, @Cost, @InvoiceID, @PlaceID, @Diagonal, @IsElectronic, @AspectRatioID, @InstalledID, @ImageID, @IsWorkingCondition)
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteInstalledSoftwareNotebook]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteInstalledSoftwareNotebook]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteInstalledSoftwareNotebook] AS' 
END
GO

ALTER PROCEDURE [dbo].[DeleteInstalledSoftwareNotebook] 
	@NotebookID int,
	@LicenseID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From InstalledSoftwareNotebook Where NotebookID=@NotebookID and LicenseID=@LicenseID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteInstalledSoftwarePC]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteInstalledSoftwarePC]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteInstalledSoftwarePC] AS' 
END
GO

ALTER PROCEDURE [dbo].[DeleteInstalledSoftwarePC] 
	@PCID int,
	@LicenseID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From InstalledSoftwarePC Where PCID=@PCID and LicenseID=@LicenseID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteInteractiveWhiteboardByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteInteractiveWhiteboardByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteInteractiveWhiteboardByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteInteractiveWhiteboardByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From InteractiveWhiteboard Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteLicenseSoftware]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteLicenseSoftware]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteLicenseSoftware] AS' 
END
GO

ALTER PROCEDURE [dbo].[DeleteLicenseSoftware] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From LicenseSoftware Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteMonitorByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteMonitorByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteMonitorByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteMonitorByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From Monitor Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteNetworkSwitchByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteNetworkSwitchByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteNetworkSwitchByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteNetworkSwitchByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From NetworkSwitch Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteNotebookByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteNotebookByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteNotebookByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteNotebookByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From Notebook Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteOS]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteOS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteOS] AS' 
END
GO

ALTER PROCEDURE [dbo].[DeleteOS] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From OS Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteOtherEquipmentByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteOtherEquipmentByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteOtherEquipmentByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteOtherEquipmentByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From OtherEquipment Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeletePCByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeletePCByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeletePCByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeletePCByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From PC Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeletePrinterScannerByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeletePrinterScannerByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeletePrinterScannerByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeletePrinterScannerByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From PrinterScanner Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteProjectorByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteProjectorByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteProjectorByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteProjectorByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From Projector Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteProjectorScreenByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteProjectorScreenByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteProjectorScreenByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[DeleteProjectorScreenByID] 
	@ID int
AS
BEGIN
	SET NOCOUNT ON;
	Delete From ProjectorScreen Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllBoardForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllBoardForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllBoardForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllBoardForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	Diagonal as ''Диагональ'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from InteractiveWhiteboard
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDevices]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllDevices]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllDevices] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllDevices]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) = N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Интерактивная доска'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from InteractiveWhiteboard
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Монитор'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Monitor
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNetworkSwitch(TypeID) as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from NetworkSwitch
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Notebook
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N'''' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from OtherEquipment
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PrinterScanner
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Проектор'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Projector
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Экран для проектора'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from ProjectorScreen
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end

	set @query += N' union SELECT
	InventoryNumber as ''Инвентарный номер'', 
	N''Компьютер'' as ''Тип'',
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PC
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	begin
		Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'
	end


	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllMonitorForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllMonitorForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllMonitorForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllMonitorForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	dbo.GetFrequency(FrequencyID) as ''Частота обновления'', 
	dbo.GetMatrix(MatrixTechnologyID) as ''Технология изготовления матрицы'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Monitor
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllNetworkSwitchForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllNetworkSwitchForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllNetworkSwitchForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllNetworkSwitchForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNetworkSwitch(TypeID) as ''Тип'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	NumberOfPorts as ''Количество портов'', 
	dbo.GetWiFiFrequency(WiFiFrequencyID) as ''Частота Wi-Fi'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from NetworkSwitch
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllNotebookForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllNotebookForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllNotebookForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllNotebookForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	CPUModel as ''Процессор'', 
	NumberOfCores as ''Количество ядер'', 
	FrequencyProcessor as ''Базовая частота'', 
	MaxFrequencyProcessor as ''Максимальная частота'', 
	RAMGB as ''ОЗУ'', 
	FrequencyRAM as ''Частота памяти'', 
	SSDCapacityGB as ''Объем SSD'', 
	HDDCapacityGB as ''Объем HDD'', 
	VideoCard as ''Видеокарта'',
	VideoRAMGB as ''Видеопамять'',  
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	dbo.GetFrequency(FrequencyID) as ''Частота обновления'', 
	dbo.GetMatrix(MatrixTechnologyID) as ''Технология изготовления матрицы'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Notebook
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllOtherEquipmentForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllOtherEquipmentForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllOtherEquipmentForReport] AS' 
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetAllOtherEquipmentForReport]
	-- Add the parameters for the stored procedure here
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from OtherEquipment
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllPCForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllPCForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllPCForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllPCForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	MotherBoard as ''Материнская плата'', 
	CPUModel as ''Процессор'', 
	NumberOfCores as ''Количество ядер'', 
	FrequencyProcessor as ''Базовая частота'', 
	MaxFrequencyProcessor as ''Максимальная частота'', 
	RAMGB as ''ОЗУ'', 
	FrequencyRAM as ''Частота памяти'', 
	VideoCard as ''Видеокарта'', 
	VideoRAMGB as ''Видеопамять'', 
	SSDCapacityGB as ''Объем SSD'', 
	HDDCapacityGB as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PC
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null)'
	if @Audience is not null and @Audience>0
	Set @query += ' and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllPrinterScannerForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllPrinterScannerForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllPrinterScannerForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllPrinterScannerForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetPaperSize(PaperSizeID) as ''Максимальный формат'',  
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from PrinterScanner
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null) '
	if @Audience is not null and @Audience>0
	Set @query += 'and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllProjectorForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllProjectorForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllProjectorForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllProjectorForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	MaxDiagonal as ''Максимальная диагональ'', 
	dbo.GetProjectorTechnology(ProjectorTechnologyID) as ''Технология проецирования'', 
	dbo.GetResolution(ResolutionID) as ''Максимальное разрешение'', 
	VideoConnectors as ''Видеоразъемы'',
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from Projector
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null) '
	if @Audience is not null and @Audience>0
	Set @query += 'and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllScreenForReport]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllScreenForReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllScreenForReport] AS' 
END
GO
ALTER PROCEDURE [dbo].[sp_GetAllScreenForReport]
	@From date = '2000-01-01', @To date = '2099-12-31', @Audience int = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(max) =N'SELECT
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	Diagonal as ''Диагональ'', 
	IsElectronicDrive as ''С электроприводом'', 
	dbo.GetAspectRatio(AspectRatioID) as ''Соотношение сторон'', 
	dbo.GetScreeninstalled(ScreenInstalledID) as ''Установка экрана'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.[GetAcquisitionDate](InvoiceID) as ''Дата приобретения'',
	dbo.GetAudienceName(PlaceID) as ''Аудитория'',
	dbo.[GetIsWorkingCondition](IsWorkingCondition) as ''Состояние''
	from ProjectorScreen
	where (dbo.[GetAcquisitionDate](InvoiceID) between ''' + convert(nvarchar, @From) + ''' and ''' + convert(nvarchar, @To) +
	''' or InvoiceID is null) '
	if @Audience is not null and @Audience>0
	Set @query += 'and (dbo.GetAudienceID(PlaceID)=' + str(@Audience) + ' and PlaceID is not null)'

	exec(@query)
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateInteractiveWhiteboardByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateInteractiveWhiteboardByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateInteractiveWhiteboardByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateInteractiveWhiteboardByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = null,
	@Diagonal real = null,
	@PlaceID int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from InteractiveWhiteboard where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from InteractiveWhiteboard where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update InteractiveWhiteboard Set 
		InventoryNumber=@InvN, Name=@Name, 
		Cost=@Cost, InvoiceID=@InvoiceID, 
		Diagonal=@Diagonal, PlaceID=@PlaceID, ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update InteractiveWhiteboard Set 
		Name=@Name, Cost=@Cost, Diagonal=@Diagonal, 
		ImageID=@ImageID Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateLicenseSoftware]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateLicenseSoftware]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateLicenseSoftware] AS' 
END
GO

ALTER PROCEDURE [dbo].[UpdateLicenseSoftware] 
	@ID int,
	@Name nvarchar(100),
	@Price money,
	@Count int,
	@InvoiceNumber nvarchar(50) = null,
	@Type int = null
AS
BEGIN
	SET NOCOUNT ON;
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Update LicenseSoftware set Name=@Name, Price=@Price, Count=@Count, InvoiceID=@InvoiceID, Type=@Type where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateMonitorByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMonitorByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateMonitorByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateMonitorByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = null,
	@Diagonal real = null,
	@PlaceID int = null,
	@MatrixID int = null,
	@FrequencyID int = null,
	@ResolutionID int = null,
	@VConnectors int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from Monitor where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from Monitor where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update Monitor Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		ScreenDiagonal=@Diagonal, PlaceID=@PlaceID, 
		MatrixTechnologyID=@MatrixID, FrequencyID=@FrequencyID,
		ResolutionID=@ResolutionID, VideoConnectors=@VConnectors,
		ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update Monitor Set 
		Name=@Name, Cost=@Cost, 
		ScreenDiagonal=@Diagonal, 
		MatrixTechnologyID=@MatrixID, 
		FrequencyID=@FrequencyID,
		ResolutionID=@ResolutionID, 
		VideoConnectors=@VConnectors,
		ImageID=@ImageID Where ID=@ID
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateNetworkSwitchByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateNetworkSwitchByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateNetworkSwitchByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateNetworkSwitchByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = null,
	@NumberOfPorts int = null,
	@PlaceID int = null,
	@TypeID int = null,
	@Frequency int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from NetworkSwitch where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from NetworkSwitch where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber
	if (@IsChangeAnalog=0)
	begin
		Update NetworkSwitch Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		NumberOfPorts=@NumberOfPorts, PlaceID=@PlaceID, 
		TypeID=@TypeID, WiFiFrequencyID=@Frequency, ImageID=@ImageID, 
		IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update NetworkSwitch Set 
		Name=@Name, Cost=@Cost, 
		NumberOfPorts=@NumberOfPorts,
		TypeID=@TypeID, WiFiFrequencyID=@Frequency, 
		ImageID=@ImageID Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateNotebookByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateNotebookByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateNotebookByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateNotebookByID] 
	@ID int,
	@InvN int,
	@Type int,
	@Name nvarchar(200),
	@Cost money,
	@Diagonal real = null,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@CPU nvarchar(20) = null,
	@RAM int = null,
	@SSD int = null,
	@HDD int = null,
	@Video nvarchar(30) = null,
	@OSID int = null,
	@ResolutionID int = null,
	@FrequencyID int = null,
	@MatrixID int = null,
	@VConnectors int = null,
	@Cores int = null,
	@Frequency int = null,
	@MaxFrequency int = null,
	@VRAM int = null,
	@FrequencyRAM int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from Notebook where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from Notebook where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update Notebook Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		ScreenDiagonal=@Diagonal, 
		PlaceID=@PlaceID, CPUModel=@CPU, 
		RAMGB=@RAM, HDDCapacityGB=@HDD, SSDCapacityGB=@SSD,
		VideoCard=@Video, OSID=@OSID, 
		ResolutionID=@ResolutionID,
		FrequencyID=@FrequencyID,
		MatrixTechnologyID=@MatrixID,
		VideoConnectors=@VConnectors,
		NumberOfCores=@Cores,
		FrequencyProcessor=@Frequency,
		MaxFrequencyProcessor=@MaxFrequency,
		VideoRAMGB=@VRAM, FrequencyRAM=@FrequencyRAM,
		ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update Notebook Set 
		Name=@Name, Cost=@Cost, 
		ScreenDiagonal=@Diagonal, CPUModel=@CPU, 
		RAMGB=@RAM, HDDCapacityGB=@HDD, SSDCapacityGB=@SSD,
		VideoCard=@Video, OSID=@OSID, 
		ResolutionID=@ResolutionID,
		FrequencyID=@FrequencyID,
		MatrixTechnologyID=@MatrixID,
		VideoConnectors=@VConnectors,
		NumberOfCores=@Cores,
		FrequencyProcessor=@Frequency,
		MaxFrequencyProcessor=@MaxFrequency,
		VideoRAMGB=@VRAM, FrequencyRAM=@FrequencyRAM,
		ImageID=@ImageID
		Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateOS]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateOS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateOS] AS' 
END
GO

ALTER PROCEDURE [dbo].[UpdateOS] 
	@ID int,
	@Name nvarchar(100),
	@Price money,
	@Count int,
	@InvoiceNumber nvarchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;
	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	Update OS set Name=@Name, Price=@Price, Count=@Count, InvoiceID=@InvoiceID where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateOtherEquipmentByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateOtherEquipmentByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateOtherEquipmentByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateOtherEquipmentByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = null,
	@PlaceID int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from OtherEquipment where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from OtherEquipment where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update OtherEquipment Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		PlaceID=@PlaceID, ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update OtherEquipment Set 
		Name=@Name, Cost=@Cost, ImageID=@ImageID 
		Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePCByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePCByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdatePCByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdatePCByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@CPU nvarchar(20) = null,
	@RAM int = null,
	@SSD int = null,
	@HDD int = null,
	@Video nvarchar(30) = null,
	@OSID int = null,
	@MB nvarchar(30) = null,
	@Cores int = null,
	@Frequency int = null,
	@MaxFrequency int = null,
	@VRAM int = null,
	@FrequencyRAM int = null,
	@VConnectors int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from PC where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from PC where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber
	
	if (@IsChangeAnalog=0)
	begin
		Update PC Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		Motherboard=@mb, PlaceID=@PlaceID, 
		CPUModel=@CPU, RAMGB=@RAM, 
		HDDCapacityGB=@HDD,  SSDCapacityGB=@SSD,
		VideoCard=@Video, 
		OSID=@OSID, ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update PC Set 
		Name=@Name, Cost=@Cost, 
		Motherboard=@mb,
		CPUModel=@CPU, RAMGB=@RAM, 
		HDDCapacityGB=@HDD,  SSDCapacityGB=@SSD,
		VideoCard=@Video, 
		OSID=@OSID, ImageID=@ImageID
		Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePrinterScannerByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePrinterScannerByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdatePrinterScannerByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdatePrinterScannerByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = null,
	@TypeID int,
	@PaperSizeID int = null,
	@PlaceID int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from PrinterScanner where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from PrinterScanner where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update PrinterScanner Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		TypePrinterID=@TypeID, 
		PaperSizeID=@PaperSizeID, 
		PlaceID=@PlaceID, ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update PrinterScanner Set 
		Name=@Name, Cost=@Cost, 
		TypePrinterID=@TypeID, 
		PaperSizeID=@PaperSizeID, 
		ImageID=@ImageID 
		Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProjectorByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateProjectorByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateProjectorByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateProjectorByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@TechnologyID int = null,
	@Diagonal real = null,
	@PlaceID int = null,
	@VConnectors int = null,
	@ResolutionID int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from Projector where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from Projector where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update Projector Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		MaxDiagonal=@Diagonal, 
		ProjectorTechnologyID=@TechnologyID, 
		PlaceID=@PlaceID, VideoConnectors=@VConnectors, 
		ResolutionID=@ResolutionID, ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update Projector Set 
		Name=@Name, Cost=@Cost, 
		MaxDiagonal=@Diagonal, 
		ProjectorTechnologyID=@TechnologyID, 
		VideoConnectors=@VConnectors, 
		ResolutionID=@ResolutionID, ImageID=@ImageID
		Where Name=@OldName and Cost=@OldCost
	end
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProjectorScreenByID]    Script Date: 14.06.2020 23:04:17  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateProjectorScreenByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateProjectorScreenByID] AS' 
END
GO
ALTER PROCEDURE [dbo].[UpdateProjectorScreenByID] 
	@ID int,
	@InvN int,
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50) = NULL,
	@PlaceID int = NULL,
	@Diagonal real = null,
	@IsEDrive bit = null,
	@AspectRatioID int = null,
	@InstalledID int = null,
	@Image varbinary(max) = null,
	@IsWorkingCondition bit = NULL,
	@IsChangeAnalog bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	Declare @OldName nvarchar(200),
	@OldCost money

	select @OldName = Name, @OldCost = Cost from ProjectorScreen where ID=@ID

	Declare @ImageID int
	if not exists (select * from Image where Image=@Image) and (@Image is not null)
	begin
		Insert into Image Values (@Image)
	end
	if (@Image is not null)
		SELECT @ImageID = ID From Image where Image=@Image
	else
		Select @ImageID = ImageID from ProjectorScreen where ID=@ID

	declare @InvoiceID int 
	if (not exists (select * from Invoice where Invoice.Number=@InvoiceNumber) and @InvoiceNumber is not null)
	begin
		execute dbo.AddInvoice @InvoiceNumber
	end
	SELECT TOP(1) @InvoiceID = [ID] from Invoice where Invoice.Number=@InvoiceNumber

	if (@IsChangeAnalog=0)
	begin
		Update ProjectorScreen Set 
		InventoryNumber=@InvN, 
		Name=@Name, Cost=@Cost, 
		InvoiceID=@InvoiceID, 
		Diagonal=@Diagonal, 
		IsElectronicDrive=@IsEDrive, 
		PlaceID=@PlaceID, 
		AspectRatioID=@AspectRatioID,
		ScreenInstalledID=@InstalledID, 
		ImageID=@ImageID, IsWorkingCondition=@IsWorkingCondition
		Where ID=@ID
	end
	if (@IsChangeAnalog=1)
	begin
		Update ProjectorScreen Set 
		Name=@Name, Cost=@Cost, 
		Diagonal=@Diagonal, 
		IsElectronicDrive=@IsEDrive, 
		AspectRatioID=@AspectRatioID,
		ScreenInstalledID=@InstalledID, ImageID=@ImageID
		Where Name=@OldName and Cost=@OldCost
	end
END
GO



INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (16, 9)
GO
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (16, 10)
GO
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (21, 9)
GO
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (32, 9)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (60)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (70)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (75)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (100)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (120)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (144)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (155)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (165)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (170)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (240)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (280)
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A0')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A1')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A2')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A3')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A4')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B0')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B1')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B2')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B3')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B4')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C0')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C1')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C2')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C3')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C4')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'LCD')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'DLP')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'LCoS')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'3LCD')
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1280, 720, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1152, 720, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1360, 768, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1366, 768, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1440, 900, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1600, 900, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1680, 1050, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1920, 1080, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1920, 1200, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2048, 1152, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2560, 1440, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2560, 1600, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3200, 1800, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3440, 1440, 3)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3840, 2400, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3840, 2160, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (4128, 2322, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (5120, 2160, 3)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (5120, 2880, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (7680, 4320, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (7680, 4800, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2560, 1080, 3)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3840, 1080, 4)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2880, 1800, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3072, 1920, 2)
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Напольный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'На раме')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Настенно-потолочный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Настенный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Настольный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'На штативе')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Портативный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Портативно-напольный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Потолочный')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'InteractiveWhiteboard')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'Monitor')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'NetworkSwitch')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'Notebook')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'OtherEquipment')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'PrinterScanner')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'Projector')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'ProjectorScreen')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'PC')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'PC', N'Компьютер', N'Компьютеры')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'Notebook', N'Портативный компьютер', N'Портативные компьютеры')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'PrinterScanner', N'Принтер/Сканер', N'Принтеры/Сканеры')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'InteractiveWhiteboard', N'Интерактивная доска', N'Интерактивные доски')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'NetworkSwitch', N'Сетевое оборудование', N'Сетевое оборудование')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'Monitor', N'Монитор', N'Мониторы')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'Projector', N'Проектор', N'Проекторы')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'ProjectorScreen', N'Экран для проектора', N'Экраны для проекторов')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'OtherEquipment', N'Прочее оборудование', N'Прочее оборудование')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Коммутатор')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Межсетевой экран')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Маршрутизатор')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Wi-Fi Роутер')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Точка доступа Wi-Fi')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Усилитель Wi-Fi')
GO
INSERT [dbo].[TypeNotebook] ([Name]) VALUES (N'Ноутбук')
GO
INSERT [dbo].[TypeNotebook] ([Name]) VALUES (N'Моноблок')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Лазерное МФУ')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Струйное МФУ')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Лазерный принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Струйный принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Матричный принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Плоттер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'3D Принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Сканер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'3D Сканер')
GO
INSERT [dbo].[TypeSoftLicense] ([Name]) VALUES (N'Лицензия')
GO
INSERT [dbo].[TypeSoftLicense] ([Name]) VALUES (N'Подписка на месяц')
GO
INSERT [dbo].[TypeSoftLicense] ([Name]) VALUES (N'Подписка на год')
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DVI', 1)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DVI-D', 2)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DVI-I', 4)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DisplayPort', 8)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'HDMI', 16)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'Mini DisplayPort', 32)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'Thunderbolt', 64)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'USB Type С', 128)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'VGA', 256)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'microHDMI', 512)
GO
INSERT [dbo].[WiFiFrequency] ([Name]) VALUES (N'2.4 ГГц')
GO
INSERT [dbo].[WiFiFrequency] ([Name]) VALUES (N'5 ГГц')
GO
INSERT [dbo].[WiFiFrequency] ([Name]) VALUES (N'2.4 ГГц/5 ГГц')
GO