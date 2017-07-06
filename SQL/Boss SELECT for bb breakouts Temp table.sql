CREATE TABLE [dbo].[TempBollingerBands](
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
	[Timestamp] [int] NULL
)
GO


Declare @firstDate DateTime;
Declare @secondDate DateTime;
Declare @lowSDVA decimal(6,2)
Declare @highSDVA decimal(6,2)
DECLARE @movement decimal(6,2)
SET @lowSDVA = .28
SET @highSDVA = .30
SET @movement = 1.30

DECLARE Cur1 CURSOR FOR
SELECT convert(varchar(10), [Date], 126) AS 'Date'
FROM [ScanOpts].[dbo].[BollingerBands]
WHERE Symbol = 'A'
AND convert(varchar(10), [Date], 126) > convert(varchar(10), DATEADD(day, -10, SYSDATETIME()), 126)
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
	    
	  SET @secondDate = (SELECT TOP 1 convert(varchar(10), [Date], 126) AS 'Date'
													FROM [ScanOpts].[dbo].[BollingerBands]
													WHERE Symbol = 'A'
													AND convert(varchar(10), [Date], 126) > @firstDate)
	  SELECT @firstDate	  
	  SELECT @secondDate
	  
	  SELECT b.[StandardDeviation], t.[StandardDeviation], b.[StandardDeviation] / t.[StandardDeviation] AS 'Movement', *
	  FROM [ScanOpts].[dbo].[BollingerBands] b
	  JOIN #Table1 t on t.Symbol = b.Symbol AND b.[Date] = @firstDate
	  WHERE convert(varchar(10), b.[Date], 126) = @secondDate
	  AND t.[Close] > t.[UpperBand]
	  AND b.[StandardDeviation] / t.[StandardDeviation] > @movement
	  AND t.[Volume] > 10000
	  --AND t.Symbol in (SELECT Symbol FROM #Table1)
	  --ORDER BY [StandardDeviation] desc
  
	  FETCH NEXT FROM Cur1 INTO @firstDate;
	  DROP TABLE #Table1
  END
  CLOSE Cur1;
  DEALLOCATE Cur1;
  