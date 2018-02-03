/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
INTO [ScanOpts].[dbo].[SlopeAndBBCountsFirstRun]
  FROM [ScanOpts].[dbo].[SlopeAndBBCounts]
  --WHERE Symbol = 'QCLN' --'NVDA' --'SBNA'