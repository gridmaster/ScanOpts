
Declare @firstDate DateTime;
Declare @secondDate DateTime;
Declare @lowSDVA decimal(6,2)
Declare @highSDVA decimal(6,2)
DECLARE @movement decimal(6,2)
SET @lowSDVA = .28
SET @highSDVA = .30
SET @movement = 1.20
 
SELECT TOP 2 convert(varchar(10), [Date], 126) AS 'Date'
INTO #dates
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
AND convert(varchar(10), [Date], 126) > convert(varchar(10), DATEADD(day, -130, SYSDATETIME()), 126)
ORDER BY 1 DESC

SET @firstDate = (SELECT TOP 1 [Date] FROM #dates ORDER BY [Date])
SET @secondDate = (SELECT TOP 1 [Date] FROM #dates ORDER BY [Date] DESC)
--SELECT @firstDate
--SELECT @secondDate

DROP TABLE #dates

	SELECT [Id] 
		  ,[Symbol]
		  ,[Date]
		  ,[Open]
		  ,[High]
		  ,[Low]
		  ,[Close]
		  ,[SMA20]
		  ,[StandardDeviation]
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
	  AND StandardDeviation < @lowSDVA	    
	  
	  SELECT b.Symbol, b.Date, b.[StandardDeviation], t.[StandardDeviation], b.[StandardDeviation] / t.[StandardDeviation] AS 'Movement', t.*, b.*
	  FROM [ScanOpts].[dbo].[BollingerBands] b
	  JOIN #Table1 t on t.Symbol = b.Symbol 
	  WHERE convert(varchar(10), b.[Date], 126) = @secondDate
	  AND b.[StandardDeviation] <> 0
	  AND t.[StandardDeviation] <> 0
	  AND b.[Close] > b.[UpperBand]
	  AND b.[StandardDeviation] / t.[StandardDeviation] > @movement
	  AND b.[Volume] > 10000
	  ORDER BY b.[Id] --desc
  
	  DROP TABLE #Table1
  