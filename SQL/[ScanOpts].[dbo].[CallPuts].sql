/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [ScanOpts].[dbo].[CallPuts]
WHERE QuoteId > 1384 -- 1324 -- 1264 -- 1048 -- 988  -- 928 -- 868 -- 808 -- 748 -- 688 -- 628 -- 568 -- 508
ORDER BY Id
  --DELETE [ScanOpts].[dbo].[CallPuts]
  --WHERE QuoteId > 446
  
  
  
--  DELETE
--  FROM [ScanOpts].[dbo].[CallPuts]
--WHERE QuoteId > 1048 AND QuoteId < 1265