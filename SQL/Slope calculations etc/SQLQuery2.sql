/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [ScanOpts].[dbo].[BollingerBands]
  WHERE Symbol = 'ABC'
  ORDER BY Id DESC