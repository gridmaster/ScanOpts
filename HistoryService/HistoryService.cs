using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using Core.Interface;
using Core.ORMModels;
using ORMService;
using Core.BulkLoad;
using Core.JsonQuote;
using SMA60CycleService;
using DataAnalyticsService;

namespace SymbolHistoryService
{
    public class HistoryService : BaseService, IHistoryService
    {
        #region Private properties
        private DailyQuotesORMService dailyQuotesORMService = null;
        private BulkLoadHistory bulkLoadHistory = null;
        private SymbolsORMService symbolsORMService = null;
        private BulkLoadDividends bulkLoadDividends = null;
        private BulkLoadSplits bulkLoadSplits = null;
        private SMA60CyclesService sMA60CyclesService = null;
        private AnalyticsService analyticsService = null;
        #endregion Private properties

        #region Constructors

        public HistoryService(ILogger logger, SMA60CyclesService sMA60CyclesService, AnalyticsService analyticsService, DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolsORMService, BulkLoadHistory bulkLoadHistory, BulkLoadDividends bulkLoadDividends, BulkLoadSplits bulkLoadSplits)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.dailyQuotesORMService = dailyQuotesORMService;
            this.bulkLoadHistory = bulkLoadHistory;
            this.symbolsORMService = symbolsORMService;
            this.bulkLoadDividends = bulkLoadDividends;
            this.bulkLoadSplits = bulkLoadSplits;
            this.sMA60CyclesService = sMA60CyclesService;
        }

        #endregion Constructors

        #region Public methods

        public List<string> GetSymbols()
        {
            logger.Info("Start - GetSymbols");

            List<string> quotesList = dailyQuotesORMService.GetSymbols();

            logger.Info("End - GetSymbols");
            logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            return quotesList;
        }

        public void RunHistoryCollection()
        {
            logger.InfoFormat("RunHistoryCollection - GetSymbols");
            List<string> symbols = symbolsORMService.GetSymbols();
            RunHistoryCollection(symbols);

            logger.Info("End - RunHistoryCollection");
            logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);

        }

        public void RunHistoryCollection(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }

            RunHistoryCollection(syms);
        }

        public string GetFullExchangeName()
        {
            // logger.InfoFormat("GetFullExchangeName - GetSymbols");
            List<string> symbols = symbolsORMService.GetSymbols();
            return GetFullExchangeName(symbols);
        }

        public string GetFullExchangeName(List<string> symbols)
        {
            logger.InfoFormat("GetFullExchangeName - GetSymbols");

            string result = string.Empty;
            string uriString = "https://query2.finance.yahoo.com/v7/finance/quote?formatted=true&crumb=qJcTEExdoWL&lang=en-US&region=US&symbols={0}&fields=messageBoardId%2ClongName%2CshortName%2CmarketCap%2CunderlyingSymbol%2CunderlyingExchangeSymbol%2CheadSymbolAsString%2CregularMarketPrice%2CregularMarketChange%2CregularMarketChangePercent%2CregularMarketVolume%2Cuuid%2CregularMarketOpen%2CfiftyTwoWeekLow%2CfiftyTwoWeekHigh%2CtoCurrency%2CfromCurrency%2CtoExchange%2CfromExchange&corsDomain=finance.yahoo.com";

            try
            {
                foreach (string symbol in symbols)
                {
                    string sPage = WebPage.Get(String.Format(uriString, symbol));

                    if (sPage.Contains("(404) Not Found")) continue;
                    if (sPage.Contains("(400) Bad Request")) continue;

                    string exchangeName = sPage.Substring(sPage.IndexOf("fullExchangeName\":\"") + "fullExchangeName\":\"".Length);
                    result = exchangeName.Substring(0, exchangeName.IndexOf("\""));

                    //write to database...
                    dailyQuotesORMService.UpdateExchange(symbol, result);
                }
            }
            catch(Exception ex)
            {
                logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
            }

            logger.Info("End - GetFullExchangeName");
            logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);

            return result;
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

        public void RunHistoryCollection(List<string> symbols)
        {
            logger.InfoFormat("Start - RunHistoryCollection");
            var i = 0;
            int startDate = (int)Core.Business.UnixTimeConverter.ToUnixTime(DateTime.Now.AddMonths(-15));
            int endDate = (int)Core.Business.UnixTimeConverter.ToUnixTime(DateTime.Now);

            try
            {
                foreach (string symbol in symbols)
                {
                    //if (!symbol.Contains("MO")) continue; 

                    logger.InfoFormat("Get {0} history page", symbol);

                    // this gets the history
                    // 5 days 5 minute intervals
                    // string uriString = $@"https://query1.finance.yahoo.com/v8/finance/chart/TSLA?symbol=TSLA&period1=1611085200&period2=1611946140&useYfid=true&interval=5m&includePrePost=true&events=div%7Csplit%7Cearn&lang=en-US&region=US&crumb=qUOJAwBbdUN&corsDomain=finance.yahoo.com"

                    // 15 months 1 day iintervals
                    string uriString = $@"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1={startDate}&period2={endDate}&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";
                    string sPage = WebPage.Get(uriString);
                    
                    if (sPage.Contains("(404) Not Found")) continue;
                    if (sPage.Contains("(400) Bad Request")) continue;
                    //logger.Info("Page captured");

                    //continue;

                    try
                    {
                        Core.JsonQuote.JsonResult symbolHistory = JsonConvert.DeserializeObject<Core.JsonQuote.JsonResult>(sPage);

                        List<DailyQuotes> quotesList = ExtractDailyQuotes(symbol, symbolHistory);
                        sMA60CyclesService.GenerateSMA60ForSymbol(ref quotesList);
                        analyticsService.FindRising60SMATrends(ref quotesList);

                        bool success = BulkLoadHistory(quotesList);

                        //var dividends = dailyQuotesORMService.GetDividends(symbol, symbolHistory);
                        //success = BulkLoadDividends(dividends);

                        //var splits = dailyQuotesORMService.GetSplits(symbol, symbolHistory);
                        //success = BulkLoadSplits(splits);
                    }
                    catch (Exception ex)
                    {
                        logger.InfoFormat("RunHistoryCollection: {0}", ex);
                        continue;
                    }
                }
            }
            catch (Exception exc)
            {
                logger.Fatal("RunHistoryCollection: {0}", exc);
            }
            finally
            {
                logger.Info("End - RunQuoteHistoryCollection");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }
        #endregion Public methods

        public List<DailyQuotes> ExtractDailyQuotes(string symbol, JsonResult symbolHistory)
        {
            List<DailyQuotes> quotesList = new List<DailyQuotes>();

            var timestamps = symbolHistory.Chart.Result[0].timestamp;
            string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
            string exchange = GetFullExchangeName(symbol);

            string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
            DateTime date = DateTime.Now;

            for (int i = 0; i < symbolHistory.Chart.Result[0].timestamp.Count; i++)
            {

                int holdInt = 0;

                DailyQuotes quote = new DailyQuotes();
                quote.Date = Core.Business.UnixTimeConverter.UnixTimeStampToDateTime(timestamps[i]);
                quote.Symbol = symbol;
                quote.Exchange = exchange;
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
                else
                {
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

        #region classes
        public class Pre
        {
            public string timezone { get; set; }
            public int end { get; set; }
            public int start { get; set; }
            public int gmtoffset { get; set; }
        }

        public class Regular
        {
            public string timezone { get; set; }
            public int end { get; set; }
            public int start { get; set; }
            public int gmtoffset { get; set; }
        }

        public class Post
        {
            public string timezone { get; set; }
            public int end { get; set; }
            public int start { get; set; }
            public int gmtoffset { get; set; }
        }

        public class CurrentTradingPeriod
        {
            public Pre pre { get; set; }
            public Regular regular { get; set; }
            public Post post { get; set; }
        }

        public class Meta
        {
            public string currency { get; set; }
            public string symbol { get; set; }
            public string exchangeName { get; set; }
            public string instrumentType { get; set; }
            public object firstTradeDate { get; set; }
            public int gmtoffset { get; set; }
            public string timezone { get; set; }
            public CurrentTradingPeriod currentTradingPeriod { get; set; }
            public string dataGranularity { get; set; }
            public List<string> validRanges { get; set; }
        }

        public class Quote
        {
        }

        public class Unadjclose
        {
        }

        public class Unadjquote
        {
        }

        public class Indicators
        {
            public List<Quote> quote { get; set; }
            public List<Unadjclose> unadjclose { get; set; }
            public List<Unadjquote> unadjquote { get; set; }
        }

        public class Result
        {
            public Meta meta { get; set; }
            public Indicators indicators { get; set; }
        }

        public class Chart
        {
            public List<Result> result { get; set; }
            public object error { get; set; }
        }

        public class RootObject
        {
            public Chart chart { get; set; }
        }
        #endregion classes

        #region Private Bulk save Methods

        private bool BulkLoadHistory(List<DailyQuotes> dailyQuotes)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadHistory.ConfigureDataTable();

                dt = bulkLoadHistory.LoadDataTableWithDailyHistory(dailyQuotes, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadHistory", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadHistory.BulkCopy<DailyQuotes>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadHistory returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Daily History Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        private bool BulkLoadDividends(Dividends dividends)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadDividends.ConfigureDataTable();

                dt = bulkLoadDividends.LoadDataTableWithDividends(dividends, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadDividends", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadDividends.BulkCopy<Dividends>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadDividends returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Dividends Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        private bool BulkLoadSplits(Splits splits)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadSplits.ConfigureDataTable();

                dt = bulkLoadSplits.LoadDataTableWithDividends(splits, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadSplits", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadSplits.BulkCopy<Splits>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadSplits returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Splits Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        #endregion Private Bulk save Methods
    }
}



