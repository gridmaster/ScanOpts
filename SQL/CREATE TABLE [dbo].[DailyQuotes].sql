USE [ScanOpts]
GO

/****** Object:  Table [dbo].[Quotes]    Script Date: 1/18/2021 9:06:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DailyQuotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](60) NULL,
	[Date] [datetime] NOT NULL,
	[Exchange] [varchar](20) NULL,
	[InstrumentType] [varchar](20) NULL,
	[Timestamp] [int] NULL,
	[Close] [decimal](18, 2) NULL,
	[High] [decimal](18, 2) NULL,
	[Low] [decimal](18, 2) NULL,
	[Open] [decimal](18, 2) NULL,
	[Volume] [decimal](18, 2) NULL,
	[UnadjHigh] [decimal](18, 2) NULL,
	[UnadjLow] [decimal](18, 2) NULL,
	[UnadjClose] [decimal](18, 2) NULL,
	[UnadjOpen] [decimal](18, 2) NULL,
 CONSTRAINT [PK_dbo.DailyQuotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


