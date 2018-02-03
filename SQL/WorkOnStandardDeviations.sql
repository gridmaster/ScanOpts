

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

DECLARE symbol_cursor CURSOR FOR  
SELECT DISTINCT Symbol
FROM dbo.BollingerBands
ORDER BY Symbol

OPEN symbol_cursor   
FETCH NEXT FROM symbol_cursor INTO @symbol

WHILE @@FETCH_STATUS = 0   
BEGIN  

SELECT @first = dbo.GetFirstClose(@symbol)
SELECT @last = dbo.GetLastClose(@symbol)

IF(@last > @first)
	BEGIN
		UPDATE StandardDeviationCountTable SET First = @first, Last = @Last, Slope = 'Up', Difference = @last - @first
	END
ELSE 
	BEGIN
		UPDATE StandardDeviationCountTable SET  First = @first, Last = @Last, Slope = 'Down', Difference = @last - @first
	END
		--SELECT @symbol
		--SELECT @first
		--SELECT @last
		
		--IF @first < @last
		--UPDATE StandardDeviationCountTable SET First = @first, Last = @Last, Slope = 'Up', Difference = @last - @first
		--ELSE
		--UPDATE StandardDeviationCountTable SET  First = @first, Last = @Last, Slope = 'Down', Difference = @last - @first

		FETCH NEXT FROM symbol_cursor INTO @symbol
END   

SELECT * FROM StandardDeviationCountTable WHERE Symbol = @symbol

CLOSE symbol_cursor   
DEALLOCATE symbol_cursor
		
--SELECT * FROM StandardDeviationCountTable
