/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  INTO BollingerBands01122018Weekley
  FROM [ScanOpts].[dbo].[BollingerBands]
  ORDER BY Id
  