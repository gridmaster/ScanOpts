using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using Core.Interface;
using Core.ORMModels;
using DIContainer;
using ORMService;
using Core.BulkLoad;
using Core.JsonQuote;

namespace SymbolHistoryService
{
    public class HistoryService : BaseService, IHistoryService
    {

        #region Constructors

        public HistoryService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        public void RunHistoryCollection()
        {
            logger.InfoFormat("RunHistoryCollection - GetSymbols");
            List<string> symbols = IOCContainer.Instance.Get<SymbolsORMService>().GetSymbols();
            RunHistoryCollection(symbols);
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

        public void RunHistoryCollection(List<string> symbols)
        {
            logger.InfoFormat("Start - RunHistoryCollection");
            var i = 0;

            try
            {
                foreach (string symbol in symbols)
                {
                    //if (!symbol.Contains("MO")) continue; 

                    logger.InfoFormat("Get {0} history page", symbol);
                    // this gets the history
                    string uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1=-252356400&period2=1488949200&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";
                    string sPage = WebPage.Get(String.Format(uriString, symbol));

                    if (sPage.Contains("(404) Not Found")) continue;
                    if (sPage.Contains("(400) Bad Request")) continue;
                    //logger.Info("Page captured");

                    //continue;

                    try
                    {
                        Core.JsonQuote.JsonResult symbolHistory = JsonConvert.DeserializeObject<Core.JsonQuote.JsonResult>(sPage);

                        List<DailyQuotes> quotesList = IOCContainer.Instance.Get<IDailyQuotesORMService>().ExtractDailyQuotes(symbol, symbolHistory);
                        bool success = BulkLoadHistory(quotesList);

                        var dividends = IOCContainer.Instance.Get<IDailyQuotesORMService>().GetDividends(symbol, symbolHistory);
                        success = BulkLoadDividends(dividends);

                        var splits = IOCContainer.Instance.Get<IDailyQuotesORMService>().GetSplits(symbol, symbolHistory);
                        success = BulkLoadSplits(splits);
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

        private bool BulkLoadHistory(List<DailyQuotes> dailyQuotes)
        {
            bool success = false;
            try
            {
                var dt = IOCContainer.Instance.Get<BulkLoadHistory>().ConfigureDataTable();

                dt = IOCContainer.Instance.Get<BulkLoadHistory>().LoadDataTableWithDailyHistory(dailyQuotes, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadHistory", Environment.NewLine);
                }
                else
                {
                    success = IOCContainer.Instance.Get<BulkLoadHistory>().BulkCopy<DailyQuotes>(dt, "ScanOptsContext");
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
                var dt = IOCContainer.Instance.Get<BulkLoadDividends>().ConfigureDataTable();

                dt = IOCContainer.Instance.Get<BulkLoadDividends>().LoadDataTableWithDividends(dividends, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadDividends", Environment.NewLine);
                }
                else
                {
                    success = IOCContainer.Instance.Get<BulkLoadDividends>().BulkCopy<Dividends>(dt, "ScanOptsContext");
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
                var dt = IOCContainer.Instance.Get<BulkLoadSplits>().ConfigureDataTable();

                dt = IOCContainer.Instance.Get<BulkLoadSplits>().LoadDataTableWithDividends(splits, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadSplits", Environment.NewLine);
                }
                else
                {
                    success = IOCContainer.Instance.Get<BulkLoadSplits>().BulkCopy<Splits>(dt, "ScanOptsContext");
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
    }
}



