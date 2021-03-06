/****** Script for SelectTopNRows command from SSMS  ******/
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
  FROM [ScanOpts].[dbo].[BollingerBands]
  WHERE Symbol = 'A'
  ORDER BY Date DESC
  --WHERE Date > '2017-06-19 09:30:00.000'
  --AND StandardDeviation < 0.33
 -- TRUNCATE TABLE [ScanOpts].[dbo].[BollingerBands]