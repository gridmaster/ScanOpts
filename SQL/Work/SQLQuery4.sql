SELECT lastPrice_raw, *
  FROM [ScanOpts].[dbo].[CallPuts]
  WHERE Symbol = 'VXX' AND ExpirationFmt = '2017-03-03'
  ORDER BY StrikeRaw, CallOrPut, Date