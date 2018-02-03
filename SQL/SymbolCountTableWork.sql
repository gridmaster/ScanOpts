DECLARE @Symbol AS VARCHAR(15)
DECLARE @StandardDeviation AS DECIMAL(4,2)

SET @Symbol = 'TTGT'
SET @StandardDeviation = 0.35

SELECT TOP 1000 [Id]
      ,[Symbol]
      ,CAST([Date] as Date) AS 'Date'
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
  FROM [ScanOpts].[dbo].[BollingerBands]
  WHERE Symbol = @Symbol
  AND StandardDeviation > 0
  AND StandardDeviation < @StandardDeviation
  ORDER BY Id
  
  
  SELECT * FROM SymbolCountTable
WHERE COUNT > 19 AND COUNT < 263
--AND Symbol = 'NVDA'
ORDER BY COUNT DESC