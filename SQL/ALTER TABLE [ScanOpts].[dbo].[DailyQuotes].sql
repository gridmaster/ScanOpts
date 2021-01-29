ALTER TABLE [ScanOpts].[dbo].[DailyQuotes]
ADD SMAHigh60Slope decimal(18,2) null
GO

ALTER TABLE [ScanOpts].[dbo].[DailyQuotes]
ADD SMALow60Slope decimal(18,2) null
GO

ALTER TABLE [ScanOpts].[dbo].[DailyQuotes]
ADD SMAClose60Slope decimal(18,2) null
GO

ALTER TABLE [ScanOpts].[dbo].[DailyQuotes]
ADD SMAVolume60Slope int null
GO

Invalid column name 'Slope60High'.
Invalid column name 'Slope60Low'.
Invalid column name 'Slope60Close'.
Invalid column name 'Slope60Volume'.