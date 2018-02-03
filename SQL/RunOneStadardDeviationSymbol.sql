DECLARE @symbol as NVARCHAR(20)
DECLARE @first as DECIMAL(12,2)
DECLARE @last as DECIMAL(12,2)

CREATE TABLE StandardDeviationCountTable (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](60) NOT NULL,
	[Count] decimal NOT NULL,
	[First] DECIMAL(12,2) NULL,
	[Last] DECIMAL(12,2) NULL,
	[Slope] [varchar](4) NULL,
	[Difference] decimal(12,2) NULL
)

SET @symbol = 'AGC'
		INSERT INTO StandardDeviationCountTable
		SELECT Symbol, COUNT(StandardDeviation) AS 'Count', 0, 0, '', 0
		FROM BollingerBands
		WHERE Symbol = @symbol
		AND StandardDeviation < .35
		AND Date > '2016-12-31 00:00:00.000'
		GROUP BY Symbol
				
		SET @first = (SELECT TOP 1 bb.[Close] FROM BollingerBands bb
		JOIN StandardDeviationCountTable sd ON bb.Symbol = sd.Symbol
				WHERE bb.Symbol = @symbol
				AND sd.Count > 45
				AND bb.StandardDeviation < .35
				AND Date > '2016-12-31 00:00:00.000')
				
		SELECT TOP 1 bb.Symbol, bb.date, bb.[Close] 
		INTO #GetClose
		FROM BollingerBands bb
		JOIN StandardDeviationCountTable sd ON bb.Symbol = sd.Symbol
			WHERE bb.Symbol = @symbol
			AND sd.Count > 45
			AND bb.StandardDeviation < .35
			AND Date > '2016-12-31 00:00:00.000'
			ORDER BY bb.[Date] DESC
		
		SET @last = (SELECT [Close] FROM #GetClose)
		DROP TABLE #GetClose
		
		SELECT @first
		SELECT @last
		SELECT @last - @first
		
		IF @first < @last
		UPDATE StandardDeviationCountTable SET First = @first, Last = @Last, Slope = 'Up', Difference = @last - @first
		ELSE
		UPDATE StandardDeviationCountTable SET  First = @first, Last = @Last, Slope = 'Down', Difference = @last - @first
		
		SELECT * FROM StandardDeviationCountTable
DROP TABLE StandardDeviationCountTable