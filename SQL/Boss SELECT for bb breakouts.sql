
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
  WHERE convert(varchar(10), [Date], 126) = '2017-04-21'
  AND ([Low] > [SMA20] or [Low] = [SMA20])
  AND [Close] < [UpperBand]
  AND StandardDeviation < 0.30
    
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
  WHERE convert(varchar(10), [Date], 126) = '2017-04-24'
  AND [Close] > [UpperBand]
  AND [StandardDeviation] > .40
  AND [Volume] > 10000
  AND Symbol in (SELECT Symbol FROM #Table1)
  ORDER BY [StandardDeviation] desc
  
  
  DROP TABLE #Table1