/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [Date], BidFmt, AskFmt, *
  FROM [ScanOpts].[dbo].[CallPuts]
  Where contractSymbol = 'VXX170310P00018000'
  ORDER BY 1 
  
  SELECT [Date], BidFmt, AskFmt, *
  FROM [ScanOpts].[dbo].[CallPuts] cp
  Where contractSymbol = 'VXX170310C00018000'
  ORDER BY 1 