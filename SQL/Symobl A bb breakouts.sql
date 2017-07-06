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
--INTO SaveBBsWeeks
  FROM [ScanOpts].[dbo].[SaveBBsWeeks]
 WHERE Symbol = 'AMZN'
 --ORDER by Date DESC
 
 