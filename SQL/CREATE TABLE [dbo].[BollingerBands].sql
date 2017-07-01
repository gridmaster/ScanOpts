USE [ScanOpts]
GO

/****** Object:  Table [dbo].[BollingerBands]    Script Date: 07/01/2017 07:56:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BollingerBands](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](60) NULL,
	[Date] [datetime] NOT NULL,
	[Open] [decimal](18, 2) NULL,
	[High] [decimal](18, 2) NULL,
	[Low] [decimal](18, 2) NULL,
	[Close] [decimal](18, 2) NULL,
	[SMA20] [decimal](18, 2) NULL,
	[StandardDeviation] [decimal](18, 2) NULL,
	[UpperBand] [decimal](18, 2) NULL,
	[LowerBand] [decimal](18, 2) NULL,
	[BandRatio] [decimal](18, 2) NULL,
	[Volume] [decimal](18, 2) NULL,
	[Timestamp] [int] NULL,
 CONSTRAINT [PK_dbo.BollingerBands] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


