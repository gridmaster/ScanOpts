
WITH TempRows AS (SELECT Id, Symbol, Date, [Close], SMA50, SMA200
  FROM [ScanOpts].[dbo].[BollingerBands01122018Weekley]
  WHERE Date = '2018-01-08 00:00:00.000' AND SMA50 < SMA200
  OR Date > '2018-01-12 00:00:00.000' AND SMA50> SMA200
  ),
  SelectList AS (SELECT COUNT(Symbol) as 'Count', Symbol
FROM TempRows
Group By Symbol)
SELECT * FROM TempRows TR
JOIN SelectList SL ON TR.Symbol = SL.Symbol
WHERE [COUNT] > 1  ORDER BY tr.Symbol


/*
SELECT Id, Symbol, Date, [Close], SMA50, SMA200
  FROM [ScanOpts].[dbo].[BollingerBands01122018Weekley]
  WHERE Symbol = 'QGEN'
  ORDER BY [Date]  DESC
*/  