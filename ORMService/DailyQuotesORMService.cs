using Core.Interface;
using Core.JsonModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;

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

        public List<DailyQuotes> ExtractDailyQuotes(string symbol, Core.JsonModels.ORMModels.JsonResult symbolHistory)
        {
            List<DailyQuotes> quotesList = new List<DailyQuotes>();

            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;

            for (int i = 0; i < symbolHistory.Chart.Result[0].timestamp.Count; i++)
            {
                int holdInt = 0;

                DailyQuotes quote = new DailyQuotes();
                quote.Date = date;
                quote.Symbol = symbol;
                quote.Exchange = exchangeName;
                quote.instrumentType = instrumentType;
                quote.timestamp = int.TryParse(timestamps[i].ToString(), out holdInt) ? timestamps[i] : (int?)null;

                quote.open = symbolHistory.Chart.Result[0].indicators.quote[0].open[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].open[i].ToString());
                quote.close = symbolHistory.Chart.Result[0].indicators.quote[0].close[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].close[i].ToString());
                quote.high = symbolHistory.Chart.Result[0].indicators.quote[0].high[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].high[i].ToString());
                quote.low = symbolHistory.Chart.Result[0].indicators.quote[0].low[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].low[i].ToString());
                quote.volume = symbolHistory.Chart.Result[0].indicators.quote[0].volume[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].volume[i].ToString());
                quote.unadjopen = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjopen[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjopen[i].ToString());
                quote.unadjclose = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjclose[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjclose[i].ToString());
                quote.unadjhigh = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjhigh[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjhigh[i].ToString());
                quote.unadjlow = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjlow[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjlow[i].ToString());
                quotesList.Add(quote);
            }

            return quotesList;
        }

        public Core.JsonModels.ORMModels.Dividends GetDividends(string symbol, Core.JsonModels.ORMModels.JsonResult symbolHistory)
        {
            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;
            Core.JsonModels.ORMModels.Dividends dividends = new Core.JsonModels.ORMModels.Dividends();
            dividends.dividends = new List<Dividend>();

            if (symbolHistory.Chart.Result[0].events != null)
            {
                if (symbolHistory.Chart.Result[0].events.dividends != null)
                {
                    foreach (var item in symbolHistory.Chart.Result[0].events.dividends.dividend)
                    {
                        Dividend dividend = new Dividend();
                        dividend.date = date;
                        dividend.symbol = symbol;
                        dividend.exchange = exchangeName;
                        dividend.dividendDate = (int)item.Value["date"];
                        decimal amount = 0;
                        decimal.TryParse(item.Value["amount"].ToString(), out amount);
                        dividend.dividendAmount = amount;
                        dividends.dividends.Add(dividend);
                    }
                }
            }
            return dividends;
        }

        private static decimal ConvertStringToDecimal(string value)
        {
            decimal holdDecimal = 0;
            decimal.TryParse(value.ToString(), out holdDecimal);
            return holdDecimal;
        }
    }
}
