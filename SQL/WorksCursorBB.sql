

IF OBJECT_ID (N'dbo.GetFirstClose', N'FN') IS NOT NULL  
    DROP FUNCTION GetFirstClose;  
GO  
  CREATE FUNCTION dbo.GetFirstClose(@sym NVARCHAR(20))
  RETURNS DECIMAL(12,2)
  AS
  BEGIN
  DECLARE @close DECIMAL(12,2);
  
  SELECT @close = (SELECT TOP 1 bb.[Close]
		FROM BollingerBands bb
		JOIN StandardDeviationCountTable sd ON bb.Symbol = sd.Symbol
				WHERE bb.Symbol = @sym
				AND sd.Count > 45
				AND bb.StandardDeviation < .35
				AND Date > '2016-12-31 00:00:00.000')

  RETURN @close;
  END
GO

IF OBJECT_ID (N'dbo.GetLastClose', N'FN') IS NOT NULL  
    DROP FUNCTION GetLastClose;  
GO  
  CREATE FUNCTION dbo.GetLastClose(@sym NVARCHAR(20))
  RETURNS DECIMAL(12,2)
  AS
  BEGIN
  DECLARE @close DECIMAL(12,2);
  
  SELECT @close = (SELECT TOP 1 bb.[Close]
		FROM BollingerBands bb
		JOIN StandardDeviationCountTable sd ON bb.Symbol = sd.Symbol
				WHERE bb.Symbol = @sym
				AND sd.Count > 45
				AND bb.StandardDeviation < .35
				AND Date > '2016-12-31 00:00:00.000'
				ORDER BY bb.[Date] DESC)
  RETURN @close;
  END
GO

--CREATE TABLE StandardDeviationCountTable (
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[Symbol] [varchar](60) NOT NULL,
--	[Count] decimal NOT NULL,
--	[First] DECIMAL(12,2) NULL,
--	[Last] DECIMAL(12,2) NULL,
--	[Slope] [varchar](4) NULL,
--	[Difference] decimal(12,2) NULL
--)

DECLARE @symbol as NVARCHAR(20)
DECLARE @sloap as NVARCHAR(4)
DECLARE @first as DECIMAL(12,2)
DECLARE @last as DECIMAL(12,2)

DECLARE symbol_cursor CURSOR FOR  
SELECT DISTINCT Symbol
FROM dbo.BollingerBands
ORDER BY Symbol

OPEN symbol_cursor   
FETCH NEXT FROM symbol_cursor INTO @symbol

WHILE @@FETCH_STATUS = 0   
BEGIN

	SELECT @symbol
	--SELECT TOP 1 * FROM BollingerBands WHERE Symbol = @symbol ORDER BY [Date] DESC

	SELECT @first = dbo.GetFirstClose(@symbol)
	SELECT @last = dbo.GetLastClose(@symbol)

	SELECT dbo.GetFirstClose(@symbol)
	SELECT @first

	IF(@last > @first)
		SELECT @sloap = 'Up'
	ELSE 
		SELECT @sloap = 'Down'

		--UPDATE StandardDeviationCountTable
		----SELECT 
		----	Symbol
		----	,COUNT(StandardDeviation) AS 'Count'
		--	SET First = @first, Last = @last, Slope = @sloap, Difference = @last - @first
		--	--,@last  AS 'Last'
		--	--,@sloap AS 'Slope'
		--	--,@last - @first  AS 'Difference'
		--FROM BollingerBands bb
		--WHERE bb.Symbol = @symbol
		--AND StandardDeviation < .35
		--AND Date > '2016-12-31 00:00:00.000'
		--GROUP BY Symbol

		FETCH NEXT FROM symbol_cursor INTO @symbol
END

CLOSE symbol_cursor   
DEALLOCATE symbol_cursor	


SELECT * FROM StandardDeviationCountTable
--DROP TABLE StandardDeviationCountTable	