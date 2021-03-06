/****** Script for SelectTopNRows command from SSMS  ******/
SELECT Symbol, Date, [Close], SMA50, SMA200
INTO #TempData
  FROM [ScanOpts].[dbo].[BollingerBands]
  WHERE Date = '2018-01-11 09:30:00.000' AND SMA50 < SMA200
  OR Date > '2018-01-12 00:00:00.000' AND SMA50> SMA200
    ORDER BY Id
    
SELECT COUNT(Symbol) as 'Count', Symbol
FROM #TempData
--WHERE 'Count' > 1
Group By Symbol
ORDER BY 1 DESC

SELECT *
FROM #TempData

--WITH <alias_name> AS (sql_subquery_statement)
--SELECT column_list FROM <alias_name>[,tablename]
--[WHERE <join_condition>]

WITH TempRows AS (SELECT Id, Symbol, Date, [Close], SMA50, SMA200
  FROM [ScanOpts].[dbo].[BollingerBands]
  WHERE Date = '2018-01-11 09:30:00.000' AND SMA50 < SMA200
  OR Date > '2018-01-12 00:00:00.000' AND SMA50> SMA200
  ),
  SelectList AS (SELECT COUNT(Symbol) as 'Count', Symbol
FROM TempRows
Group By Symbol)
SELECT * FROM TempRows TR
JOIN SelectList SL ON TR.Symbol = SL.Symbol
WHERE [COUNT] > 1  ORDER BY Id

