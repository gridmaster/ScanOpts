DECLARE @previousClose decimal
DECLARE @Symbol NVARCHAR(20)
DECLARE @Experation NVARCHAR(20)
DECLARE @Id INT

SET @Symbol = 'SSO'
SET @Experation = '2017-03-10'
SET @Id = (SELECT MAX(Id) FROM Quotes WHERE Symbol = @Symbol)
SET @previousClose = (SELECT [RegularMarketPreviousClose] FROM QUOTES WHERE Symbol = @Symbol AND Id = @Id);

SELECT @Symbol as 'Symbol', @Experation as 'Experation', @previousClose AS 'Previous Close', lastPrice_raw, inTheMoney, CallOrPut, StrikeRaw, [Date], PercentChangeFmt, OpenInterestFmt
  FROM [ScanOpts].[dbo].[CallPuts]
  WHERE Symbol = @Symbol AND ExpirationFmt = @Experation
  AND StrikeRaw > @previousClose - 3 AND StrikeRaw < @previousClose + 3
  ORDER BY StrikeRaw, CallOrPut, Date 
  