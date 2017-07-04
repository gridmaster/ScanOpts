CREATE TABLE [dbo].[TempBollingerBands](
	[Id] [int] NOT NULL,
	[Symbol] [varchar](60) NULL,
	[Date] [datetime] NOT NULL,
	[Open] [decimal](18, 2) NULL,
	[High] [decimal](18, 2) NULL,
	[Low] [decimal](18, 2) NULL,
	[Close] [decimal](18, 2) NULL,
	[SMA20] [decimal](18, 2) NULL,
	[StandardDeviation] [decimal](18, 2) NULL,
	[LowSDV] [decimal](6,2) NULL,
	[Move] [decimal](6,2) NULL,
	[UpperBand] [decimal](18, 2) NULL,
	[LowerBand] [decimal](18, 2) NULL,
	[BandRatio] [decimal](18, 2) NULL,
	[Volume] [decimal](18, 2) NULL,
	[Timestamp] [int] NULL
)
GO

Declare @firstDate DateTime;
Declare @secondDate DateTime;
Declare @lowSDVA decimal(6,2)
Declare @highSDVA decimal(6,2)
Declare @gainPercent decimal(6,2)
SET @lowSDVA = .30
SET @highSDVA = .40
SET @gainPercent = 1.30

DECLARE Cur1 CURSOR FOR
SELECT convert(varchar(10), [Date], 126) AS 'Date'
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
AND convert(varchar(10), [Date], 126) > convert(varchar(10), DATEADD(day, -120, SYSDATETIME()), 126)
ORDER BY 1 --DESC

--SELECT TOP 1 convert(varchar(10), [Date], 126) AS 'Date'
--FROM [ScanOpts].[dbo].[BollingerBands]
--WHERE Symbol = 'A'
--AND convert(varchar(10), [Date], 126) > @firstDate


OPEN Cur1
FETCH NEXT FROM Cur1 INTO @firstDate;
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT [Id] 
		  ,[Symbol]
		  ,[Date]
		  ,[Open]
		  ,[High]
		  ,[Low]
		  ,[Close]
		  ,[SMA20]
		  ,[StandardDeviation]
		  ,0 AS [LowSDV]
		  ,0 AS [Move]
		  ,[UpperBand]
		  ,[LowerBand]
		  ,[BandRatio]
		  ,[Volume]
		  ,[Timestamp]
	  INTO #Table1
	  FROM [ScanOpts].[dbo].[BollingerBands]
	  WHERE convert(varchar(10), [Date], 126) = @firstDate
	  AND ([Close] > [SMA20]) --or [Low] = [SMA20])
	  AND [Close] < [UpperBand]

--SELECT * FROM [dbo].[TempBollingerBands]
--SELECT * FROM #Table1

	  INSERT INTO [dbo].[TempBollingerBands]
	  SELECT [Id] 
		  ,[Symbol]
		  ,[Date]
		  ,[Open]
		  ,[High]
		  ,[Low]
		  ,[Close]
		  ,[SMA20]
		  ,[StandardDeviation]
		  ,[LowSDV]
		  ,[Move]
		  ,[UpperBand]
		  ,[LowerBand]
		  ,[BandRatio]
		  ,[Volume]
		  ,[Timestamp]
	  FROM #Table1

	  FETCH NEXT FROM Cur1 INTO @firstDate;
	  DROP TABLE #Table1
  END
  CLOSE Cur1;
  DEALLOCATE Cur1;

SELECT DISTINCT tb.StandardDeviation, b.StandardDeviation, tb.StandardDeviation / b.StandardDeviation, *
FROM [dbo].[TempBollingerBands] tb --ORDER BY Id
JOIN BollingerBands b ON b.Id = tb.Id - 1 AND tb.StandardDeviation / b.StandardDeviation > 1.3
WHERE tb.StandardDeviation <> 0 
AND b.StandardDeviation <> 0
AND b.[Close] > b.UpperBand
ORDER BY tb.Date DESC

--  SELECT * FROM [dbo].[TempBollingerBands] WHERE SYMBOL = 'A'

--  DROP TABLE [dbo].[TempBollingerBands]
	    
--	  --SET IDENTITY_INSERT [dbo].[TempBollingerBands].[Id] ON;  
--	  INSERT INTO [dbo].[TempBollingerBands]
--	  SELECT --[Id] 
--		   [Symbol]
--		  ,[Date]
--		  ,[Open]
--		  ,[High]
--		  ,[Low]
--		  ,[Close]
--		  ,[SMA20]
--		  ,[StandardDeviation]
--		  ,@lowSDVA
--		  ,[StandardDeviation] / @lowSDVA
--		  ,[UpperBand]
--		  ,[LowerBand]
--		  ,[BandRatio]
--		  ,[Volume]
--		  ,[Timestamp]
--	  INTO #Table2
--	  FROM [ScanOpts].[dbo].[BollingerBands] b
--	  WHERE convert(varchar(10), [Date], 126) = (SELECT TOP 1 convert(varchar(10), [Date], 126) AS 'Date'
--													FROM [ScanOpts].[dbo].[BollingerBands]
--													WHERE Symbol = 'A'
--													AND convert(varchar(10), [Date], 126) > @firstDate)
--	  AND [Close] > [UpperBand]
----	  AND [StandardDeviation] / @lowSDVA > @gainPercent
--	  AND [Volume] > 10000
--	  AND Symbol in (SELECT Symbol FROM #Table1)
--	  ORDER BY [StandardDeviation] desc
  
--	  FETCH NEXT FROM Cur1 INTO @firstDate;
--	  DROP TABLE #Table1
--  END
--  CLOSE Cur1;
--  DEALLOCATE Cur1;

--  SELECT * FROM [dbo].[TempBollingerBands]
--  DROP TABLE [dbo].[TempBollingerBands]