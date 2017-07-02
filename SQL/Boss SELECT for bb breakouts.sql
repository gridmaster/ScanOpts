Declare @firstDate DateTime;
Declare @secondDate DateTime;
Declare @lowSDVA decimal(6,2)
Declare @highSDVA decimal(6,2)
SET @lowSDVA = .28
SET @highSDVA = .30

DECLARE Cur1 CURSOR FOR
SELECT convert(varchar(10), [Date], 126) AS 'Date'
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
AND convert(varchar(10), [Date], 126) > convert(varchar(10), DATEADD(day, -120, SYSDATETIME()), 126)
ORDER BY 1 --DESC

SELECT TOP 1 convert(varchar(10), [Date], 126) AS 'Date'
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
AND convert(varchar(10), [Date], 126) > @firstDate


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
  
	  FETCH NEXT FROM Cur1 INTO @firstDate;
	  DROP TABLE #Table1
  END
  CLOSE Cur1;
  DEALLOCATE Cur1;
  