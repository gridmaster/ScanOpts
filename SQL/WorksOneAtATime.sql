

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

DECLARE @symbol as NVARCHAR(20)
DECLARE @sloap as NVARCHAR(4)
DECLARE @first as DECIMAL(12,2)
DECLARE @last as DECIMAL(12,2)

SET @symbol = 'AEH'
SELECT @first = dbo.GetFirstClose(@symbol)
SELECT @last = dbo.GetLastClose(@symbol)

IF(@last > @first)
	SELECT @sloap = 'Up'
ELSE 
	SELECT @sloap = 'Down'

		SELECT 
			Symbol
			,COUNT(StandardDeviation) AS 'Count'
			,@first --dbo.GetFirstClose(@symbol) AS 'First'
			,@last  --dbo.GetLastClose(@symbol) AS 'Last'
			,@sloap AS 'Slope'
			,@last - @first  AS 'Difference'
		FROM BollingerBands
		WHERE Symbol = @symbol
		AND StandardDeviation < .35
		AND Date > '2016-12-31 00:00:00.000'
		GROUP BY Symbol