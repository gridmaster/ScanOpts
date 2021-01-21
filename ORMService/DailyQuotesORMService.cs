using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using Core.Interface;
using Core.JsonQuote;
using Core.ORMModels;
using DIContainer;
using ORMService.Context;

namespace ORMService
{
    public class DailyQuotesORMService : IDailyQuotesORMService
    {
        public void Add(DailyQuotes entity)
        {
            using (var db = new ScanOptsContext())
            {
                db.DailyQuotes.Add(entity);

                IOCContainer.Instance.Get<ILogger>().Info("DailyQuotesORMService - Add Saving Changes");
                db.SaveChanges();
            }
        }

        public void AddMany(List<DailyQuotes> quotes)
        {
            using (var db = new ScanOptsContext())
            {
                try
                {
                    foreach (DailyQuotes quote in quotes)
                    {
                        //IOCContainer.Instance.Get<ILogger>().InfoFormat("DailyQuotesORMService - AddMany {0}", quote.Symbol);
                        db.DailyQuotes.Add(quote);
                    }
                    IOCContainer.Instance.Get<ILogger>().Info("DailyQuotesORMService - AddMany Saving Changes");

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    IOCContainer.Instance.Get<ILogger>().ErrorFormat("DailyQuotesORMService - AddMany error: {1}{2}", ex.Message, Environment.NewLine);
                }
            }
        }

        public List<string> GetSymbols()
        {
            List<string> result = new List<string>();

            using (var db = new ScanOptsContext())
            {
                var symbols = db.DailyQuotes.Select(s => s.Symbol).Distinct();

                var wtf = symbols.ToList().OrderBy(s => s);

                result = wtf.ToList();


            }

            return result;
        }

        public List<DailyQuotes> ExtractDailyQuotes(string symbol, JsonResult symbolHistory)
        {
            List<DailyQuotes> quotesList = new List<DailyQuotes>();

            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;

            //var currentQuote = Core.Business.UnixTimeConverter.UnixTimeStampToDateTime((double)timestamps[timestamps.Count-1]);

           // double currentClose = (double)symbolHistory.Chart.Result[0].indicators.unadjclose[0].unadjclose[timestamps.Count - 1];

            //if (currentClose < 10)
              //  return quotesList;


            for (int i = 0; i < symbolHistory.Chart.Result[0].timestamp.Count; i++)
            {
                int holdInt = 0;

                DailyQuotes quote = new DailyQuotes();
                quote.Date = date;
                quote.Symbol = symbol;
                quote.Exchange = exchangeName;
                quote.InstrumentType = instrumentType;
                quote.Timestamp = int.TryParse(timestamps[i].ToString(), out holdInt) ? timestamps[i] : (int?)null;

                quote.Open = symbolHistory.Chart.Result[0].indicators.quote[0].open[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].open[i].ToString());
                quote.Close = symbolHistory.Chart.Result[0].indicators.quote[0].close[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].close[i].ToString());
                quote.High = symbolHistory.Chart.Result[0].indicators.quote[0].high[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].high[i].ToString());
                quote.Low = symbolHistory.Chart.Result[0].indicators.quote[0].low[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].low[i].ToString());
                quote.Volume = symbolHistory.Chart.Result[0].indicators.quote[0].volume[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].volume[i].ToString());
                if (symbolHistory.Chart.Result[0].indicators.unadjquote != null)
                {
                    quote.UnadjOpen = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjopen[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjopen[i].ToString());
                    quote.UnadjClose = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjclose[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjclose[i].ToString());
                    quote.UnadjHigh = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjhigh[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjhigh[i].ToString());
                    quote.UnadjLow = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjlow[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjlow[i].ToString());
                }
                else {
                    quote.UnadjOpen = 0;
                    quote.UnadjClose = 0;
                    quote.UnadjHigh = 0;
                    quote.UnadjLow = 0;
                }

                quote.SMA60Close = 0;
                quote.SMA60High = 0;
                quote.SMA60Low = 0;
                quote.SMA60Volume = 0;

                quotesList.Add(quote);
            
            }

            return quotesList;
        }

        public Dividends GetDividends(string symbol, JsonResult symbolHistory)
        {
            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;
            Dividends dividends = new Dividends();
            dividends.dividends = new List<Dividend>();

            if (symbolHistory.Chart.Result[0].events != null)
            {
                if (symbolHistory.Chart.Result[0].events.Dividends != null)
                {
                    foreach (var item in symbolHistory.Chart.Result[0].events.Dividends.Dividend)
                    {
                        Dividend dividend = new Dividend();
                        dividend.Date = date;
                        dividend.Symbol = symbol;
                        dividend.Exchange = exchangeName;
                        dividend.DividendDate = (int)item.Value["date"];
                        decimal amount = 0;
                        decimal.TryParse(item.Value["amount"].ToString(), out amount);
                        dividend.DividendAmount = amount;
                        dividends.dividends.Add(dividend);
                    }
                }
            }
            return dividends;
        }

        public Splits GetSplits(string symbol, JsonResult symbolHistory)
        {
            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;
            Splits splits = new Splits();
            splits.splits = new List<Split>();

            if (symbolHistory.Chart.Result[0].events != null)
            {
                if (symbolHistory.Chart.Result[0].events.Splits != null)
                {
                    foreach (var item in symbolHistory.Chart.Result[0].events.Splits.Split)
                    {
                        Split split = new Split();
                        split.Date = date;
                        split.Symbol = symbol;
                        split.Exchange = exchangeName;
                        split.SplitDate = (int)item.Value["date"];
                        int Amount = 0;
                        int.TryParse(item.Value["numerator"].ToString(), out Amount);
                        split.Numerator = Amount;
                        int.TryParse(item.Value["denominator"].ToString(), out Amount);
                        split.Denominator = Amount;
                        split.Ratio = item.Value["splitRatio"].ToString();

                        splits.splits.Add(split);
                    }
                }
            }
            return splits;
        }
        private static decimal ConvertStringToDecimal(string value)
        {
            decimal holdDecimal = 0;
            decimal.TryParse(value.ToString(), out holdDecimal);
            return holdDecimal;
        }
    }
}
