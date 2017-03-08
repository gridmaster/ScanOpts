DECLARE @myDate DateTime
SET @myDate = '2017-02-21 15:36:45.547'

DECLARE @yourDate DateTime
SET @yourDate = CONVERT(NVARCHAR(10), CONVERT(date, GetDate())) + ' 15:00:00.000'

SELECT @yourDate

SELECT DATEDIFF(HOUR, @yourDate, @myDate)

