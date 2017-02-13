USE [ScanOpts]
GO

/****** Object:  Table [dbo].[KeyStatisticModels]    Script Date: 02/12/2017 16:06:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Quotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [Symbol] [nvarchar](60) NOT NULL,
    [ExpirationDate] [decimal](18,0) NOT NULL,
    [Date] [DateTime] NOT NULL,
    [Exchange] [nvarchar](20),
    [QuoteType] [nvarchar](20),
	[QuoteSourceName] [nvarchar](60),
    [Currency] [nvarchar](20),
    [MarketState] [nvarchar](40),
    [ShortName] [nvarchar](400),
    [Market] [nvarchar](60),
    [LongName] [nvarchar](500),
    [PostMarketChangePercent] [decimal](18,14),
    [PostMarketTime] [decimal](18,14),
    [PostMarketPrice] [decimal](18,4),
    [PostMarketChange] [decimal](18,14),
    [ExchangeTimezoneName] [nvarchar](60),
    [ExchangeTimezoneShortName] [nvarchar](10),
    [GmtOffSetMilliseconds] [decimal](18,0),
    [RegularMarketChangePercent] [decimal](18,14),
    [RegularMarketPreviousClose] [decimal](18,4),
    [Bid] [decimal](18,4),
    [Ask] [decimal](18,4),
    [BidSize] [decimal](18,4),
    [AskSize] [decimal](18,4),
    [MessageBoardId] [nvarchar](40),
    [FullExchangeName] [nvarchar](60),
    [AverageDailyVolume3Month] [decimal](18,4),
    [AverageDailyVolume10Day] [decimal](18,4),
    [FiftyTwoWeekLowChange] [decimal](18,14),
    [FiftyTwoWeekLowChangePercent] [decimal](18,14),
    [FiftyTwoWeekHighChange] [decimal](18,14),
    [FiftyTwoWeekHighChangePercent] [decimal](18,14),
    [FiftyTwoWeekLow] [decimal](18,4),
    [FiftyTwoWeekHigh] [decimal](18,4),
    [RegularMarketPrice] [decimal](18,4),
    [RegularMarketTime] [int],
    [RegularMarketChange] [decimal](18,14),
    [RegularMarketOpen] [decimal](18,4),
    [RegularMarketDayHigh] [decimal](18,4),
    [RegularMarketDayLow] [decimal](18,4),
    [RegularMarketVolume] [decimal](18,4),
    [SharesOutstanding] [int],
    [FiftyDayAverage] [decimal](18,14),
    [FiftyDayAverageChange] [decimal](18,14),
    [FiftyDayAverageChangePercent] [decimal](18,14),
    [TwoHundredDayAverage] [decimal](18,14),
    [TwoHundredDayAverageChange] [decimal](18,14),
    [TwoHundredDayAverageChangePercent] [decimal](18,14),
    [MarketCap]  [decimal](18,14),
    [SourceInterval] [decimal](18,4),
    [ExpirationDates] [nvarchar](200),
    [Strikes] [nvarchar](120),
    [HasMiniOptions] [bit],
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_dbo.Quotes] PRIMARY KEY CLUSTERED 
(
    [Symbol] ASC,
    [ExpirationDate] ASC,
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


