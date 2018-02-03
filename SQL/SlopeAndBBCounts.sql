USE [ScanOpts]
GO

/****** Object:  Table [dbo].[SlopeAndBBCounts]    Script Date: 01/27/2018 17:47:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SlopeAndBBCounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SymbolId] [int] NOT NULL,
	[Symbol] [varchar](60) NULL,
	[Date] [datetime] NOT NULL,
	[Close] [decimal](18, 2) NULL,
	[SMA20] [decimal](18, 2) NULL,
	[SMA50] [decimal](18, 2) NULL,
	[SMA200] [decimal](18, 2) NULL,
	[StandardDeviation] [decimal](18, 2) NULL,
	[UpperBand] [decimal](18, 2) NULL,
	[LowerBand] [decimal](18, 2) NULL,
	[BandRatio] [decimal](18, 2) NULL,
	[Volume] [decimal](18, 2) NULL,	
	[SlopeClose] [decimal](18, 2) NULL,
	[Slope20] [decimal](18, 2) NOT NULL,
	[Slope50] [decimal](18, 2) NOT NULL,
	[Slope200] [decimal](18, 2) NOT NULL,
	[SlopeStandardDeviation] [decimal](18, 2) NOT NULL,
	[SlopeUpperBand] [decimal](18, 2) NOT NULL,
	[SlopeLowerBand] [decimal](18, 2) NOT NULL,
	[SlopeBandRatio] [decimal](18, 2) NOT NULL,
	[RatioClose] [decimal](18, 2) NULL,
	[Ratio20] [decimal](18, 2) NOT NULL,
	[Ratio50] [decimal](18, 2) NOT NULL,
	[Ratio200] [decimal](18, 2) NOT NULL,
	[RatioStandardDeviation] [decimal](18, 2) NOT NULL,
	[RatioUpperBand] [decimal](18, 2) NOT NULL,
	[RatioLowerBand] [decimal](18, 2) NOT NULL,
	[RatioBandRatio] [decimal](18, 2) NOT NULL
 CONSTRAINT [PK_dbo.SlopeAndBBCounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


