Declare @firstDate varchar(10);

Declare @lowSDVA decimal(6,2)
Declare @highSDVA decimal(6,2)
Declare @gainPercent decimal(6,2)
SET @lowSDVA = .30
SET @highSDVA = .40
SET @gainPercent = .130

SET @firstDate = (SELECT Top 1 convert(varchar(10), [Date], 126) AS 'Date'
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
ORDER BY [Date] DESC)

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
	  --AND StandardDeviation < @lowSDVA
	    
	  SET @lowSDVA = (	SELECT TOP 1 [StandardDeviation] FROM #Table1)
	    
	  SELECT [Id] 
		  ,[Symbol]
		  ,[Date]
		  ,[Open]
		  ,[High]
		  ,[Low]
		  ,[Close]
		  ,[SMA20]
		  ,[StandardDeviation]
  		  ,@lowSDVA AS 'Low'
		  ,@gainPercent AS 'WTF'
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
	  AND [StandardDeviation] / @lowSDVA > @gainPercent
	  AND [Volume] > 10000
	  AND Symbol in (SELECT Symbol FROM #Table1)
	  ORDER BY [StandardDeviation] desc
	  
DROP TABLE #Table1