USE [Accounting]
GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceID]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInvoiceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetInvoiceID] 
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
/****** Object:  UserDefinedFunction [dbo].[GetLocalion]    Script Date: 03.05.2020 21:27:31  ******/
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
	where AudienceTable.ID=@PlaceID

	SELECT TOP(1) @temp =  N''Аудитория '' + Audience.Name 
	FROM Audience inner join AudienceTable on 
	Audience.ID=AudienceTable.AudienceID
	WHERE AudienceTable.ID=@PlaceID

	set @res = @temp + @temp2
	RETURN @res

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNameOS]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNameOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetNameOS] 
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
/****** Object:  UserDefinedFunction [dbo].[GetNextInventoryNumber]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNextInventoryNumber]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetNextInventoryNumber] ()
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
/****** Object:  UserDefinedFunction [dbo].[GetNumberInvoice]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNumberInvoice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetNumberInvoice] 
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
/****** Object:  UserDefinedFunction [dbo].[GetOSID]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOSID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetOSID] 
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
/****** Object:  UserDefinedFunction [dbo].[GetPaperSize]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaperSize]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetPaperSize] 
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
/****** Object:  UserDefinedFunction [dbo].[GetPaperSizeID]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaperSizeID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetPaperSizeID] 
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
/****** Object:  UserDefinedFunction [dbo].[GetProjectorTechnology]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProjectorTechnology]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetProjectorTechnology] 
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
/****** Object:  UserDefinedFunction [dbo].[GetProjectorTechnologyID]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProjectorTechnologyID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetProjectorTechnologyID] 
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
/****** Object:  UserDefinedFunction [dbo].[GetTable]    Script Date: 03.05.2020 21:27:31  ******/
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
/****** Object:  UserDefinedFunction [dbo].[GetTypeDevice]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeDevice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetTypeDevice] 
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
/****** Object:  UserDefinedFunction [dbo].[GetTypeNotebook]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeNotebook]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetTypeNotebook] 
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
/****** Object:  UserDefinedFunction [dbo].[GetTypePrinter]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypePrinter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

CREATE FUNCTION [dbo].[GetTypePrinter] 
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
/****** Object:  UserDefinedFunction [dbo].[GetTypePrinterID]    Script Date: 03.05.2020 21:27:31  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypePrinterID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[GetTypePrinterID] 
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
/****** Object:  UserDefinedFunction [dbo].[INVALID_GetPlaceID]    Script Date: 03.05.2020 21:27:32  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INVALID_GetPlaceID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[INVALID_GetPlaceID] 
(
	@Location nvarchar(100)
)
RETURNS int
AS
BEGIN
	DECLARE @AudienceName nvarchar(5)
	DECLARE @AudienceName2 nvarchar(5)
	Select @AudienceName = SUBSTRING(@Location, 10, 4)
	Select @AudienceName2 = SUBSTRING(@Location, 10, 2)
	Declare @AudienceID int
	SELECT @AudienceID = ID FROM Audience where Name=@AudienceName OR Name=@AudienceName2

	-- Return the result of the function
	RETURN @AudienceID

END
' 
END
GO
/****** Object:  Table [dbo].[Frequency]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllFrequency]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  Table [dbo].[MatrixTechnology]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMatrixTechnology]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  Table [dbo].[Invoice]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllInvoice]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID, Number
	FROM Invoice
)
' 
END
GO
/****** Object:  Table [dbo].[PaperSize]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPaperSize]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  Table [dbo].[ProjectorTechnology]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjectorTechnology]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  Table [dbo].[Resolution]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllResolution]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  Table [dbo].[VideoConnector]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllVideoConnector]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  Table [dbo].[OS]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PC]    Script Date: 03.05.2020 21:27:32  ******/
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
 CONSTRAINT [PK_PC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Notebook]    Script Date: 03.05.2020 21:27:32  ******/
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
 CONSTRAINT [PK_Notebook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOSInstaledList]    Script Date: 03.05.2020 21:27:32  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOSInstaledList]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetOSInstaledList] ()
RETURNS TABLE 
AS
RETURN 
(
	select Name as N''Название'', SUM(countOS) as N''Количество'' from (
		select OS.Name, COUNT(Notebook.OSID) as countOS
		from Notebook inner join OS on OSID=OS.ID
		group by OS.Name
		union all
		select OS.Name, COUNT(PC.OSID) as countOS
		from PC inner join OS on OSID=OS.ID
		group by OS.Name
	) as q
	group by q.Name
)
' 
END
GO
/****** Object:  Table [dbo].[InteractiveWhiteboard]    Script Date: 03.05.2020 21:27:32  ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllBoard]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	Diagonal as ''Диагональ'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from InteractiveWhiteboard
)
' 
END
GO
/****** Object:  Table [dbo].[AudienceTable]    Script Date: 03.05.2020 21:27:32  ******/
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
	[Row] [int] NOT NULL,
	[Col] [int] NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_AudienceTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Place]    Script Date: 03.05.2020 21:27:32  ******/
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
 CONSTRAINT [PK_Place] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllLocation]    Script Date: 03.05.2020 21:27:32  ******/
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
	/*SELECT (N''Аудитория '' + Audience.Name + dbo.GetTable(AudienceTableID)) as locationName 
	FROM Place inner join AudienceTable on Place.AudienceTableID=AudienceTable.ID 
	inner join Audience on AudienceTable.ID=Audience.ID
	SELECT ID, dbo.GetLocalion(ID) as N''Место'' FROM AudienceTable*/
	SELECT Place.ID, dbo.GetLocalion(AudienceTable.ID) as N''Место'', dbo.GetTypeDevice(Place.TypeDeviceID) as N''Тип''
	FROM AudienceTable inner join Place on Place.AudienceTableID=AudienceTable.ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllLocationByTypeDeviceID]    Script Date: 03.05.2020 21:27:32  ******/
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
	/*SELECT (N''Аудитория '' + Audience.Name + dbo.GetTable(AudienceTableID)) as locationName 
	FROM Place inner join AudienceTable on Place.AudienceTableID=AudienceTable.ID 
	inner join Audience on AudienceTable.ID=Audience.ID where Place.TypeDeviceID=@TypeDeviceID*/
	SELECT Place.ID as PlaceID, dbo.GetLocalion(AudienceTable.ID) as N''Place'' 
	FROM AudienceTable inner join Place on Place.AudienceTableID=AudienceTable.ID
	where Place.TypeDeviceID=@TypeDeviceID
)
' 
END
GO
/****** Object:  Table [dbo].[Monitor]    Script Date: 03.05.2020 21:27:32  ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllMonitor]    Script Date: 03.05.2020 21:27:32  ******/
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
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from Monitor
)
' 
END
GO
/****** Object:  Table [dbo].[NetworkSwitch]    Script Date: 03.05.2020 21:27:32  ******/
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
 CONSTRAINT [PK__NetworkS__3214EC27D73498D3] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNetworkSwitch]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	NumberOfPorts as ''Количество портов'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from NetworkSwitch
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllNotebook]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	CPUModel as ''Процессор'', 
	str(RAMGB, 3, 0) + N'' ГБ'' as ''ОЗУ'', 
	VideoCard as ''Видеокарта'', 
	ScreenDiagonal as ''Диагональ экрана'', 
	str(HDDCapacityGB, 4, 1) + N'' ГБ'' as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from Notebook
)
' 
END
GO
/****** Object:  Table [dbo].[OtherEquipment]    Script Date: 03.05.2020 21:27:32  ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllOtherEquipment]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from OtherEquipment
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPC]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
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
	str(HDDCapacityGB, 4, 1) + N'' ГБ'' as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	VideoConnectors, 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from PC
)
' 
END
GO
/****** Object:  Table [dbo].[PrinterScanner]    Script Date: 03.05.2020 21:27:32  ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllPrinterScanner]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	dbo.GetPaperSize(PaperSizeID) as ''Максимальный формат'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from PrinterScanner
)
' 
END
GO
/****** Object:  Table [dbo].[Projector]    Script Date: 03.05.2020 21:27:32  ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllProjector]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	MaxDiagonal as ''Максимальная диагональ'', 
	dbo.GetProjectorTechnology(ProjectorTechnologyID) as ''Технология проецирования'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from Projector
)
' 
END
GO
/****** Object:  Table [dbo].[ProjectorScreen]    Script Date: 03.05.2020 21:27:32  ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetAllScreen]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	str(Cost, 10, 2) + N'' ₽'' as ''Цена'', 
	Diagonal as ''Диагональ'', 
	IsElectronicDrive as ''С электроприводом'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from ProjectorScreen
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetBoardByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	Diagonal as ''Диагональ'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID
	from InteractiveWhiteboard 
	where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetMonitorByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	ScreenDiagonal as ''Диагональ экрана'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID
	from Monitor where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNetworkSwitchByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	NumberOfPorts as ''Количество портов'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID
	from NetworkSwitch where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNotebookByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	dbo.GetTypeNotebook(TypeNotebookID) as ''Тип'', 
	Name as ''Наименование'', 
	Notebook.Cost as ''Цена'', 
	CPUModel as ''Процессор'', 
	RAMGB as ''ОЗУ'', 
	VideoCard as ''Видеокарта'', 
	ScreenDiagonal as ''Диагональ экрана'', 
	HDDCapacityGB as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID
	from Notebook where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOtherEquipmentByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID 
	from OtherEquipment where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPCByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	-- Add the SELECT statement with parameter references here
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	MotherBoard as ''Материнская плата'', 
	CPUModel as ''Процессор'', 
	RAMGB as ''ОЗУ'', 
	VideoCard as ''Видеокарта'', 
	HDDCapacityGB as ''Объем HDD'', 
	dbo.GetNameOS(OSID) as ''Операционная система'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID
	from PC where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPrinterScannerByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	dbo.GetTypePrinter(TypePrinterID) as ''Тип'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	dbo.GetPaperSize(PaperSizeID) as ''Максимальный формат'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'' 
	from PrinterScanner where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetProjectorByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	MaxDiagonal as ''Максимальная диагональ'', 
	dbo.GetProjectorTechnology(ProjectorTechnologyID) as ''Технология проецирования'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID 
	from Projector where ID=@ID
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetScreenByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	SELECT ID as ''ID'', 
	InventoryNumber as ''Инвентарный номер'', 
	Name as ''Наименование'', 
	Cost as ''Цена'', 
	Diagonal as ''Диагональ'', 
	IsElectronicDrive as ''С электроприводом'', 
	dbo.GetNumberInvoice(InvoiceID) as ''Номер накладной'', 
	dbo.GetLocalion(PlaceID) as ''Расположение'',
	PlaceID
	from ProjectorScreen where ID=@ID
)
' 
END
GO
/****** Object:  Table [dbo].[AspectRatio]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Audience]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InstalledSoftwareNotebook]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InstalledSoftwarePC]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LicenseSoftware]    Script Date: 03.05.2020 21:27:32  ******/
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
	[Count] [nchar](10) NOT NULL,
	[TotalCost]  AS ([Price]*[Count]),
	[InvoiceID] [int] NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_LicenseSoftware] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ScreenInstalled]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeDevice]    Script Date: 03.05.2020 21:27:32  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeDevice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypeDevice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_TypeDevice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeNetworkSwitch]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeNotebook]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypePrinter]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TypeSoftLicense]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[WiFiFrequency]    Script Date: 03.05.2020 21:27:32  ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Audience]    Script Date: 03.05.2020 21:27:32  ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Audience]') AND name = N'IX_Audience')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Audience] ON [dbo].[Audience]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PaperSize]    Script Date: 03.05.2020 21:27:32  ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PaperSize]') AND name = N'IX_PaperSize')
CREATE UNIQUE NONCLUSTERED INDEX [IX_PaperSize] ON [dbo].[PaperSize]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PC]    Script Date: 03.05.2020 21:27:32  ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PC]') AND name = N'IX_PC')
CREATE UNIQUE NONCLUSTERED INDEX [IX_PC] ON [dbo].[PC]
(
	[InventoryNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TypeDevice]    Script Date: 03.05.2020 21:27:32  ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TypeDevice]') AND name = N'IX_TypeDevice')
CREATE UNIQUE NONCLUSTERED INDEX [IX_TypeDevice] ON [dbo].[TypeDevice]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TypePrinter]    Script Date: 03.05.2020 21:27:32  ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TypePrinter]') AND name = N'IX_TypePrinter')
CREATE UNIQUE NONCLUSTERED INDEX [IX_TypePrinter] ON [dbo].[TypePrinter]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LicenseSoftware_Count]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LicenseSoftware] ADD  CONSTRAINT [DF_LicenseSoftware_Count]  DEFAULT ((1)) FOR [Count]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AudienceTable_Audience]') AND parent_object_id = OBJECT_ID(N'[dbo].[AudienceTable]'))
ALTER TABLE [dbo].[AudienceTable]  WITH CHECK ADD  CONSTRAINT [FK_AudienceTable_Audience] FOREIGN KEY([AudienceID])
REFERENCES [dbo].[Audience] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AudienceTable_Audience]') AND parent_object_id = OBJECT_ID(N'[dbo].[AudienceTable]'))
ALTER TABLE [dbo].[AudienceTable] CHECK CONSTRAINT [FK_AudienceTable_Audience]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwareNotebook_LicenseSoftware] FOREIGN KEY([LicenseID])
REFERENCES [dbo].[LicenseSoftware] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook] CHECK CONSTRAINT [FK_InstalledSoftwareNotebook_LicenseSoftware]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_Notebook]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwareNotebook_Notebook] FOREIGN KEY([NotebookID])
REFERENCES [dbo].[Notebook] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwareNotebook_Notebook]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwareNotebook]'))
ALTER TABLE [dbo].[InstalledSoftwareNotebook] CHECK CONSTRAINT [FK_InstalledSoftwareNotebook_Notebook]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwarePC_LicenseSoftware] FOREIGN KEY([LicenseID])
REFERENCES [dbo].[LicenseSoftware] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_LicenseSoftware]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC] CHECK CONSTRAINT [FK_InstalledSoftwarePC_LicenseSoftware]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_PC]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC]  WITH CHECK ADD  CONSTRAINT [FK_InstalledSoftwarePC_PC] FOREIGN KEY([PCID])
REFERENCES [dbo].[PC] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstalledSoftwarePC_PC]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstalledSoftwarePC]'))
ALTER TABLE [dbo].[InstalledSoftwarePC] CHECK CONSTRAINT [FK_InstalledSoftwarePC_PC]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard]  WITH CHECK ADD  CONSTRAINT [FK_InteractiveWhiteboard_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard] CHECK CONSTRAINT [FK_InteractiveWhiteboard_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard]  WITH CHECK ADD  CONSTRAINT [FK_InteractiveWhiteboard_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InteractiveWhiteboard_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[InteractiveWhiteboard]'))
ALTER TABLE [dbo].[InteractiveWhiteboard] CHECK CONSTRAINT [FK_InteractiveWhiteboard_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseSoftware_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseSoftware]'))
ALTER TABLE [dbo].[LicenseSoftware]  WITH CHECK ADD  CONSTRAINT [FK_LicenseSoftware_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
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
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Frequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Frequency]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_MatrixTechnology] FOREIGN KEY([MatrixTechnologyID])
REFERENCES [dbo].[MatrixTechnology] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor] CHECK CONSTRAINT [FK_Monitor_MatrixTechnology]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Monitor_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Monitor]'))
ALTER TABLE [dbo].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
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
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch] CHECK CONSTRAINT [FK_NetworkSwitch_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NetworkSwitch_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[NetworkSwitch]'))
ALTER TABLE [dbo].[NetworkSwitch]  WITH CHECK ADD  CONSTRAINT [FK_NetworkSwitch_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
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
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Frequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Frequency]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_MatrixTechnology] FOREIGN KEY([MatrixTechnologyID])
REFERENCES [dbo].[MatrixTechnology] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_MatrixTechnology]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_MatrixTechnology]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_OS] FOREIGN KEY([OSID])
REFERENCES [dbo].[OS] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook] CHECK CONSTRAINT [FK_Notebook_OS]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notebook_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notebook]'))
ALTER TABLE [dbo].[Notebook]  WITH CHECK ADD  CONSTRAINT [FK_Notebook_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
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
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OS_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OS]'))
ALTER TABLE [dbo].[OS] CHECK CONSTRAINT [FK_OS_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment]  WITH CHECK ADD  CONSTRAINT [FK_OtherEquipment_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment] CHECK CONSTRAINT [FK_OtherEquipment_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment]  WITH CHECK ADD  CONSTRAINT [FK_OtherEquipment_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OtherEquipment_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[OtherEquipment]'))
ALTER TABLE [dbo].[OtherEquipment] CHECK CONSTRAINT [FK_OtherEquipment_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_OS] FOREIGN KEY([OSID])
REFERENCES [dbo].[OS] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_OS]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_OS]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC]  WITH CHECK ADD  CONSTRAINT [FK_PC_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PC_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PC]'))
ALTER TABLE [dbo].[PC] CHECK CONSTRAINT [FK_PC_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_AudienceTable]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_AudienceTable] FOREIGN KEY([AudienceTableID])
REFERENCES [dbo].[AudienceTable] ([ID])
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
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrinterScanner_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrinterScanner]'))
ALTER TABLE [dbo].[PrinterScanner]  WITH CHECK ADD  CONSTRAINT [FK_PrinterScanner_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
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
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector] CHECK CONSTRAINT [FK_Projector_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projector_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projector]'))
ALTER TABLE [dbo].[Projector]  WITH CHECK ADD  CONSTRAINT [FK_Projector_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
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
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_AspectRatio]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen]  WITH CHECK ADD  CONSTRAINT [FK_ProjectorScreen_AspectRatio] FOREIGN KEY([AspectRatioID])
REFERENCES [dbo].[AspectRatio] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_AspectRatio]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen] CHECK CONSTRAINT [FK_ProjectorScreen_AspectRatio]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen]  WITH CHECK ADD  CONSTRAINT [FK_ProjectorScreen_Invoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoice] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_Invoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen] CHECK CONSTRAINT [FK_ProjectorScreen_Invoice]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen]  WITH CHECK ADD  CONSTRAINT [FK_ProjectorScreen_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen] CHECK CONSTRAINT [FK_ProjectorScreen_Place]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_ScreenInstalled]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen]  WITH CHECK ADD  CONSTRAINT [FK_ProjectorScreen_ScreenInstalled] FOREIGN KEY([ScreenInstalledID])
REFERENCES [dbo].[ScreenInstalled] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectorScreen_ScreenInstalled]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectorScreen]'))
ALTER TABLE [dbo].[ProjectorScreen] CHECK CONSTRAINT [FK_ProjectorScreen_ScreenInstalled]
GO
/****** Object:  StoredProcedure [dbo].[AddInteractiveWhiteboard]    Script Date: 03.05.2020 21:27:32  ******/
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
	@PlaceID int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Insert into InteractiveWhiteboard 
	(InventoryNumber, Name, Cost, InvoiceID, Diagonal, PlaceID) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @Diagonal, @PlaceID)
END
GO
/****** Object:  StoredProcedure [dbo].[AddInvoice]    Script Date: 03.05.2020 21:27:32  ******/
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
	if (@Date = null)
	begin
	select @Date = CONVERT(date, GETDATE())
	end
	insert into [dbo].[Invoice] ([Number], [Date]) Values (@Number, @Date)
END
GO
/****** Object:  StoredProcedure [dbo].[AddMonitor]    Script Date: 03.05.2020 21:27:32  ******/
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
	@VConnectors int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Insert into Monitor (InventoryNumber, Name, Cost, InvoiceID, ScreenDiagonal, PlaceID, ResolutionID, FrequencyID, MatrixTechnologyID, VideoConnectors) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @Diagonal, @PlaceID, @ResolutionID, @FrequencyID, @MatrixID, @VConnectors)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNetworkSwitch]    Script Date: 03.05.2020 21:27:32  ******/
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
	@PlaceID int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Insert into NetworkSwitch 
	(InventoryNumber, Name, Cost, InvoiceID, NumberOfPorts, PlaceID) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @Ports, @PlaceID)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNotebook]    Script Date: 03.05.2020 21:27:32  ******/
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
	@HDD int = Null,
	@Video nvarchar(30) = Null,
	@OSName nvarchar(20) = null,
	@ResolutionID int = NULL,
	@FrequencyID int = NULL,
	@MatrixID int = NULL,
	@VConnectors int = NULL,
	@Cores int = NULL,
	@Frequency int = NULL,
	@MaxFrequency int = NULL,
	@VRAM int = NULL,
	@FrequencyRAM int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Insert into Notebook 
	(TypeNotebookID, InventoryNumber, Name, Cost, InvoiceID, ScreenDiagonal, 
	PlaceID, CPUModel, RAMGB, HDDCapacityGB, VideoCard, OSID, ResolutionID, 
	FrequencyID, MatrixTechnologyID, VideoConnectors, NumberOfCores, FrequencyProcessor,
	MaxFrequencyProcessor, VideoRAMGB, FrequencyRAM) 
	values (@Type, @InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @Diagonal, 
	@PlaceID, @CPU, @RAM, @HDD, @Video, dbo.GetOSID(@OSName), @ResolutionID, 
	@FrequencyID, @MatrixID, @VConnectors, @Cores, @Frequency, @MaxFrequency, 
	@VRAM, @FrequencyRAM)
END
GO
/****** Object:  StoredProcedure [dbo].[AddOtherEquipment]    Script Date: 03.05.2020 21:27:32  ******/
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
	@PlaceID int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Insert into OtherEquipment 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @PlaceID)
END
GO
/****** Object:  StoredProcedure [dbo].[AddPC]    Script Date: 03.05.2020 21:27:32  ******/
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
	@Video nvarchar(30) = Null,
	@OSName nvarchar(20) = null,
	@MB nvarchar(30) = null,
	@Cores int = NULL,
	@Frequency int = NULL,
	@MaxFrequency int = NULL,
	@VRAM int = NULL,
	@FrequencyRAM int = NULL,
	@VConnectors int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	Insert into PC (InventoryNumber, Name, Cost, InvoiceID, PlaceID, 
	CPUModel, RAMGB, HDDCapacityGB, VideoCard, OSID, Motherboard,
	NumberOfCores, FrequencyProcessor, MaxFrequencyProcessor, VideoRAMGB, FrequencyRAM, VideoConnectors) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), 
	@PlaceID, @CPU, @RAM, @HDD, @Video, dbo.GetOSID(@OSName), @MB,
	@Cores, @Frequency, @MaxFrequency, @VRAM, @FrequencyRAM, @VConnectors)
END
GO
/****** Object:  StoredProcedure [dbo].[AddPrinterScanner]    Script Date: 03.05.2020 21:27:32  ******/
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
	@Paper nvarchar(10) = null,
	@Type nvarchar(30) = null
AS
BEGIN
	SET NOCOUNT ON;
	Insert into PrinterScanner 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, TypePrinterID, PaperSizeID) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @PlaceID, 
	dbo.GetTypePrinterID(@Type), dbo.GetPaperSizeID(@Paper))
END
GO
/****** Object:  StoredProcedure [dbo].[AddProjector]    Script Date: 03.05.2020 21:27:32  ******/
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
	@Technology nvarchar(10) = null
AS
BEGIN
	SET NOCOUNT ON;
	Insert into Projector 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, MaxDiagonal, ProjectorTechnologyID) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), 
	@PlaceID, @Diagonal, dbo.GetProjectorTechnologyID(@Technology))
END
GO
/****** Object:  StoredProcedure [dbo].[AddProjectorScreen]    Script Date: 03.05.2020 21:27:32  ******/
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
	@IsElectronic bit = null
AS
BEGIN
	SET NOCOUNT ON;
	Insert into ProjectorScreen 
	(InventoryNumber, Name, Cost, InvoiceID, PlaceID, Diagonal, IsElectronicDrive) 
	values (@InvN, @Name, @Cost, dbo.GetInvoiceID(@InvoiceNumber), @PlaceID, @Diagonal, @IsElectronic)
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteInteractiveWhiteboardByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteMonitorByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteNetworkSwitchByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteNotebookByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteOtherEquipmentByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeletePCByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeletePrinterScannerByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteProjectorByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteProjectorScreenByID]    Script Date: 03.05.2020 21:27:32  ******/
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
/****** Object:  StoredProcedure [dbo].[UpdateInteractiveWhiteboardByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@InvoiceNumber nvarchar(50),
	@Diagonal real,
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update InteractiveWhiteboard Set 
	InventoryNumber=@InvN, Name=@Name, 
	Cost=@Cost, InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	Diagonal=@Diagonal, PlaceID=@PlaceID Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateMonitorByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@InvoiceNumber nvarchar(50),
	@Diagonal real,
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update Monitor Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	ScreenDiagonal=@Diagonal, PlaceID=@PlaceID 
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateNetworkSwitchByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@InvoiceNumber nvarchar(50),
	@NumberOfPorts int,
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update NetworkSwitch Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	NumberOfPorts=@NumberOfPorts, PlaceID=@PlaceID 
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateNotebookByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@Name nvarchar(200),
	@Cost money,
	@InvoiceNumber nvarchar(50),
	@CPU nvarchar(20),
	@RAM int,
	@HDD int,
	@Video nvarchar(30),
	@OSName nvarchar(20),
	@Diagonal real,
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update Notebook Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	ScreenDiagonal=@Diagonal, 
	PlaceID=@PlaceID, CPUModel=@CPU, 
	RAMGB=@RAM, HDDCapacityGB=@HDD, 
	VideoCard=@Video, OSID=dbo.GetOSID(@OSName) 
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateOtherEquipmentByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@InvoiceNumber nvarchar(50),
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update OtherEquipment Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	PlaceID=@PlaceID 
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePCByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@CPU nvarchar(20) = NULL,
	@RAM int = Null,
	@HDD int = Null,
	@Video nvarchar(30) = Null,
	@OSName nvarchar(20) = null,
	@MB nvarchar(30) = null
AS
BEGIN
	SET NOCOUNT ON;
	Update PC Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	Motherboard=@mb, PlaceID=@PlaceID, 
	CPUModel=@CPU, RAMGB=@RAM, 
	HDDCapacityGB=@HDD, 
	VideoCard=@Video, 
	OSID=dbo.GetOSID(@OSName)
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePrinterScannerByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@InvoiceNumber nvarchar(50),
	@Type nvarchar(30),
	@Paper nvarchar(10),
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update PrinterScanner Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	TypePrinterID=dbo.GetTypePrinterID(@Type), 
	PaperSizeID=dbo.GetPaperSizeID(@Paper), 
	PlaceID=@PlaceID 
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProjectorByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@Technology nvarchar(10),
	@Diagonal real,
	@PlaceID int
AS
BEGIN
	SET NOCOUNT ON;
	Update Projector Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	MaxDiagonal=@Diagonal, 
	ProjectorTechnologyID=dbo.GetProjectorTechnologyID(@Technology), 
	PlaceID=@PlaceID 
	Where ID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProjectorScreenByID]    Script Date: 03.05.2020 21:27:32  ******/
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
	@Diagonal real,
	@IsEDrive bit
AS
BEGIN
	SET NOCOUNT ON;
	Update ProjectorScreen Set 
	InventoryNumber=@InvN, 
	Name=@Name, Cost=@Cost, 
	InvoiceID=dbo.GetInvoiceID(@InvoiceNumber), 
	Diagonal=@Diagonal, 
	IsElectronicDrive=@IsEDrive, 
	PlaceID=@PlaceID 
	Where ID=@ID
END
GO
USE [master]
GO
ALTER DATABASE [Accounting] SET  READ_WRITE 
GO
