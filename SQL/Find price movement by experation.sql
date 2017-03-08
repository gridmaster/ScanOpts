DECLARE @previousClose decimal
DECLARE @Symbol NVARCHAR(20)
DECLARE @Experation NVARCHAR(20)
DECLARE @Id INT
DECLARE @cutoff DateTime

SET @cutoff = '2017-02-21 15:00:00.000'
SET @Symbol = 'VXX'
SET @Experation = '2017-03-10'
SET @Id = (SELECT MAX(Id) FROM Quotes WHERE Symbol = @Symbol)
SET @previousClose = (SELECT [RegularMarketPreviousClose] FROM QUOTES WHERE Symbol = @Symbol AND Id = @Id);

WITH subTable (Symbol, Experation, PreviousClose, lastPrice_raw, inTheMoney, CallOrPut, StrikeRaw, [Date], PercentChangeFmt, OpenInterestFmt)
  AS (
SELECT @Symbol as 'Symbol', @Experation as 'Experation', @previousClose AS 'Previous Close', lastPrice_raw, inTheMoney, CallOrPut, StrikeRaw, [Date], PercentChangeFmt, OpenInterestFmt
  FROM [ScanOpts].[dbo].[CallPuts]
  WHERE Symbol = @Symbol AND ExpirationFmt = @Experation
  AND StrikeRaw > @previousClose - 3 AND StrikeRaw < @previousClose + 3 )
  --ORDER BY StrikeRaw, CallOrPut, Date )
SELECT DISTINCT st.Symbol, st.Experation, CallOrPut, q.RegularMarketPreviousClose as 'Previous Close', q.RegularMarketPreviousClose - StrikeRaw as 'Difference', lastPrice_raw, inTheMoney, StrikeRaw, st.[Date], PercentChangeFmt, OpenInterestFmt FROM subTable st
JOIN Quotes q on q.Symbol = st.Symbol AND DATEDIFF(day, q.Date, st.Date) = 0
WHERE DATEDIFF(hour, CONVERT(NVARCHAR(10), CONVERT(date, st.Date)) + ' 15:00:00.000', st.Date) > -1
AND st.Date > '2017-02-22 23:59:59.999'
ORDER BY StrikeRaw, CallOrPut, st.Date