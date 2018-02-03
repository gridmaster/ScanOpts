CREATE TABLE StandardDeviations (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](60) NOT NULL,
	[Count] INT NULL,
	[First] DECIMAL(12,2) NULL,
	[FirstDate] DateTime NULL,
	[LastDate] DateTime NULL,
	[Last] DECIMAL(12,2) NULL,
	[Slope] [varchar](4) NULL,
	[Difference] decimal(12,2) NULL
)