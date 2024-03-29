﻿using Core;
using Core.Interface;
using Core.JsonQuote;
using Core.ORMModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ORMService
{
    public class DailyQuotesORMService : BaseService, IDailyQuotesORMService
    {
        //private IHistoryService historyService;

        #region Constructors
        public DailyQuotesORMService(ILogger logger) //, IHistoryService historyService) //, BulkLoadAnalyticsService bulkLoadAnalyticsService)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;

            //this.historyService = historyService;
        }

        #endregion Constructors

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
                IQueryable<string> symbols = db.DailyQuotes.Select(s => s.Symbol).Distinct();

                result = symbols.ToList().OrderBy(s => s).ToList();                
            }

            return result;
        }

        //public DailyQuotes UpdateExchange(string symbol, string exchange)
        //{
        //    DailyQuotes result = new DailyQuotes();

        //    using (var db = new ScanOptsContext())
        //    {
        //        result = db.DailyQuotes.Where(q => q.Symbol == symbol).FirstOrDefault<DailyQuotes>();

        //        result.Exchange = exchange;

        //        db.SaveChanges();
        //    }

        //    return result;
        //}

        public bool UpdateExchange(string symbol, string exchange)
        {
            bool result = false;

            try
            {
                using (var db = new ScanOptsContext())
                {
                    List<DailyQuotes> quotes = db.DailyQuotes.Where(q => q.Symbol == symbol).ToList();
                    quotes.ForEach(q => q.Exchange = exchange);

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                var what = ex.Message;
            }     
           
            return result;
        }

        public List<DailyQuotes> GetDailyQuotes(string symbol)
        {
            List<DailyQuotes> dailyQuotes = new List<DailyQuotes>();

            try
            {
                using (var db = new ScanOptsContext())
                {
                    dailyQuotes = db.DailyQuotes.Where(q => q.Symbol == symbol).ToList();

                    //db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                var what = ex.Message;
            }

            return dailyQuotes;
        }

        public bool UpdateDailyQuotes(List<DailyQuotes> dailyQuotes)
        {
            bool result = false;

            try
            {
                using (var db = new ScanOptsContext())
                {
                    string symbol = dailyQuotes.FirstOrDefault().Symbol;

                    List<DailyQuotes> quotes = db.DailyQuotes.Where(q => q.Symbol == symbol).ToList();

                    foreach(DailyQuotes dq in dailyQuotes)
                    {
                        var thisquote = quotes.Find(q => q.Timestamp == dq.Timestamp);

                        thisquote.SMA60High = dq.SMA60High;
                        thisquote.SMA60Low = dq.SMA60Low;
                        thisquote.SMA60Close = dq.SMA60Close;
                        thisquote.SMA60Volume = dq.SMA60Volume;

                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                var what = ex.Message;
            }

            return result;
        }

        public DailyQuotes GetDailyQuote(string symbol, int timestamp)
        {
            DailyQuotes result = new DailyQuotes();

            try
            {
                using (var db = new ScanOptsContext())
                {
                    //quotes = db.DailyQuotes.Where(q => q.Symbol == symbol).Where(q => q.Timestamp == timestamp);
                    IQueryable<DailyQuotes> quotes = db.DailyQuotes.Where(q => q.Symbol == symbol && q.Timestamp == timestamp);

                    //IQueryable<string> symbols = db.DailyQuotes.Select(s => s.Symbol).Distinct();

                    result = quotes.ToList().OrderBy(s => s.Timestamp).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                var what = ex.Message;
            }

            return result;
        }

        public List<DailyQuotes> GetSymbolDailyData(string symbol)
        {
            List<DailyQuotes> result = new List<DailyQuotes>();

            using (var db = new ScanOptsContext())
            {
                result = db.DailyQuotes.Where(a => a.Symbol == symbol).ToList();
            }

            return result;
        }
        
        public List<DailyQuotes> ExtractDailyQuotesx(string symbol, JsonResult symbolHistory)
        {
            List<DailyQuotes> quotesList = new List<DailyQuotes>();

            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;

            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;

            for (int i = 0; i < symbolHistory.Chart.Result[0].timestamp.Count; i++)
            {
                string exchange = GetFullExchangeName(symbol);

                int holdInt = 0;

                DailyQuotes quote = new DailyQuotes();
                quote.Date = Core.Business.UnixTimeConverter.UnixTimeStampToDateTime(timestamps[i]);
                quote.Symbol = symbol;
                quote.Exchange = exchangeName;
                quote.InstrumentType = instrumentType;
                quote.Timestamp = int.TryParse(timestamps[i].ToString(), out holdInt) ? timestamps[i] : (int?)null;

                quote.Open = symbolHistory.Chart.Result[0].indicators.quote[0].open[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].open[i].ToString());
                quote.Close = symbolHistory.Chart.Result[0].indicators.quote[0].close[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].close[i].ToString());
                quote.High = symbolHistory.Chart.Result[0].indicators.quote[0].high[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].high[i].ToString());
                quote.Low = symbolHistory.Chart.Result[0].indicators.quote[0].low[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].low[i].ToString());
                quote.Volume = symbolHistory.Chart.Result[0].indicators.quote[0].volume[i] == null ? 0 : ConvertStringToInteger(symbolHistory.Chart.Result[0].indicators.quote[0].volume[i].ToString());
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

        public string GetFullExchangeName(string symbol)
        {
            //            logger.InfoFormat("GetFullExchangeName - GetSymbols");

            string result = string.Empty;
            string uriString = "https://query2.finance.yahoo.com/v7/finance/quote?formatted=true&crumb=qJcTEExdoWL&lang=en-US&region=US&symbols={0}&fields=messageBoardId%2ClongName%2CshortName%2CmarketCap%2CunderlyingSymbol%2CunderlyingExchangeSymbol%2CheadSymbolAsString%2CregularMarketPrice%2CregularMarketChange%2CregularMarketChangePercent%2CregularMarketVolume%2Cuuid%2CregularMarketOpen%2CfiftyTwoWeekLow%2CfiftyTwoWeekHigh%2CtoCurrency%2CfromCurrency%2CtoExchange%2CfromExchange&corsDomain=finance.yahoo.com";

            try
            {
                string sPage = WebPage.Get(String.Format(uriString, symbol));

                string exchangeName = sPage.Substring(sPage.IndexOf("fullExchangeName\":\"") + "fullExchangeName\":\"".Length);
                result = exchangeName.Substring(0, exchangeName.IndexOf("\""));

                //write to database...
                //                dailyQuotesORMService.UpdateExchange(symbol, result);
            }
            catch (Exception ex)
            {
                logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
            }

            //logger.Info("End - GetFullExchangeName");
            //logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);

            return result;
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

        private static int ConvertStringToInteger(string value)
        {
            int holdInt = 0;
            int.TryParse(value.ToString(), out holdInt);
            return holdInt;
        }

    }
}
