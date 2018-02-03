
DECLARE @symbol as NVARCHAR(20)

DECLARE symbol_cursor CURSOR FOR  
SELECT DISTINCT Symbol
FROM dbo.BollingerBands
ORDER BY Symbol

CREATE TABLE SymbolCountTable (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](60) NOT NULL,
	[Count] decimal NOT NULL
)


OPEN symbol_cursor   
FETCH NEXT FROM symbol_cursor INTO @symbol

WHILE @@FETCH_STATUS = 0   
BEGIN   
		INSERT INTO SymbolCountTable
		SELECT Symbol, COUNT(StandardDeviation) AS 'Count'
		
		FROM BollingerBands
		WHERE Symbol = @symbol
		AND StandardDeviation < 1.4
		GROUP BY Symbol

		FETCH NEXT FROM symbol_cursor INTO @symbol
END   

SELECT * FROM SymbolCountTable

CLOSE symbol_cursor   
DEALLOCATE symbol_cursor

--DROP TABLE SymbolCountTable