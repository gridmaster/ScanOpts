
DECLARE @symbol as NVARCHAR(20)
DECLARE @first as DECIMAL(12,2)
DECLARE @last as DECIMAL(12,2)

DECLARE symbol_cursor CURSOR FOR  
SELECT DISTINCT Symbol
FROM dbo.BollingerBands
ORDER BY Symbol

CREATE TABLE StandardDeviationCount (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](60) NOT NULL,
	[Count] INT NULL,
	[First] DECIMAL(12,2) NULL,
	[Last] DECIMAL(12,2) NULL,
	[Slope] [varchar](4) NULL,
	[Difference] decimal(12,2) NULL
)

OPEN symbol_cursor   
FETCH NEXT FROM symbol_cursor INTO @symbol

WHILE @@FETCH_STATUS = 0   
BEGIN   

		INSERT INTO StandardDeviationCountx
		SELECT Symbol, COUNT(StandardDeviation) AS 'Count', 0, 0, '', 0
		FROM BollingerBands
		WHERE Symbol = @symbol
		AND StandardDeviation < .35
		AND Date > '2016-12-31 00:00:00.000'
		GROUP BY Symbol

		FETCH NEXT FROM symbol_cursor INTO @symbol
END   

CLOSE symbol_cursor   
DEALLOCATE symbol_cursor
		
SELECT * FROM StandardDeviationCount
--DROP TABLE StandardDeviationCount