Declare @firstDate varchar(10);

--SELECT Top 1 convert(varchar(10), [Date], 126) AS 'Date'
--FROM [ScanOpts].[dbo].[BollingerBands]
--WHERE Symbol = 'A'
--ORDER BY [Date] DESC

--SET @firstDate = (SELECT convert(varchar(10), SYSDATETIME(), 126))
--SET @firstDate = convert(varchar(10), (DATEADD(day, -4, SYSDATETIME())), 126)
--SELECT @firstDate
Declare @lowSDVA decimal(6,2)
Declare @highSDVA decimal(6,2)
SET @lowSDVA = .25
SET @highSDVA = .25

SET @firstDate = (SELECT Top 1 convert(varchar(10), [Date], 126) AS 'Date'
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
ORDER BY [Date] DESC)
--SELECT @firstDate

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
	 -- AND ([Low] > [SMA20] or [Low] = [SMA20])
	  AND [Close] > [SMA20]
	  AND [Close] < [UpperBand]
	  AND StandardDeviation < @lowSDVA
	    
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
	  FROM [ScanOpts].[dbo].[BollingerBands] b
	  WHERE convert(varchar(10), [Date], 126) = (SELECT TOP 1 convert(varchar(10), [Date], 126) AS 'Date'
													FROM [ScanOpts].[dbo].[BollingerBands]
													WHERE Symbol = 'A'
													AND convert(varchar(10), [Date], 126) > @firstDate)
	  AND [Close] > [UpperBand]
	  AND [StandardDeviation] > @highSDVA
	  AND [Volume] > 10000
	  AND Symbol in (SELECT Symbol FROM #Table1)
	  ORDER BY [StandardDeviation] desc
	  
DROP TABLE #Table1