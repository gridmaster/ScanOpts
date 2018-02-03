/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
INTO [ScanOpts].[dbo].[SlopeAndBBCountsDaily012618]
  FROM [ScanOpts].[dbo].[SlopeAndBBCounts]
  --WHERE Symbol = 'QCLN' --'NVDA' --'SBNA'