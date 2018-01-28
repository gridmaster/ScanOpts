using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Core;
using Core.Business;
using Core.Interface;
using Core.ORMModels;
using Core.BulkLoad;
using ORMService;

namespace BollingerBandService
{
    public class BollingerBandsService : BaseService, IBollingerBandService
    {
        #region Private properties
        private bool success;
        private List<string> symbolList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private DailyQuotesORMService dailyQuotesORMService = null;
        private BulkLoadBollingerBands bulkLoadBollingerBands = null;
        private bool _runDaily = true;
        #endregion Private properties

        #region Public properties

        #endregion Public properties

        #region Constructors

        public BollingerBandsService(ILogger logger, DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService, BulkLoadBollingerBands bulkLoadBollingerBands)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.dailyQuotesORMService = dailyQuotesORMService;
            this.bulkLoadBollingerBands = bulkLoadBollingerBands;
        }

        #endregion Constructors

        #region Public Methods

        public void RunBollingerBandsCheck()
        {
            logger.InfoFormat("RunKeyStatisticsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunBollingerBandsCheck(symbols);
        }

        public void RunBollingerBandsCheck(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            RunBollingerBandsCheck(syms);
        }

        public void RunBollingerBandsCheck(List<string> symbols)
        {
            logger.InfoFormat("Start - RunBollingerBandsCheck");
  
            string uriString = string.Empty;
            var endDate = DateTime.Now.ToUnixTime();
            var startDate = endDate;

            if (_runDaily)
            {
                uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1={1}&period2={2}&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";
                startDate = DateTime.Now.AddMonths(-36).ToUnixTime();
            }
            else { 
                uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1={1}&period2={2}&interval=1wk&events=div%7Csplit&corsDomain=finance.yahoo.com";
                startDate = DateTime.Now.AddYears(-10).ToUnixTime();
            }

            // dates run from oldest to newest

            bulkLoadBollingerBands.TruncateTable("BollingerBands");

            try
            {
                foreach (string symbol in symbols)
                {
                    logger.InfoFormat("Get {0} page", symbol);
                    //if (symbol != "ABEOW")
                    //{
                    //    continue;
                    //}
                    string sPage = WebPage.Get(String.Format(uriString, symbol, startDate, endDate));

                    if (sPage.Contains("(404) Not Found")) continue;
                    if (sPage.Contains("(400) Bad Request")) continue;
                    if (sPage.Contains("(502) Bad Gateway.")) continue;
                    Core.JsonQuote.JsonResult symbolHistory = new Core.JsonQuote.JsonResult();
                    try {
                        symbolHistory = JsonConvert.DeserializeObject<Core.JsonQuote.JsonResult>(sPage);
                    }
                    catch(Exception ex)
                    {
                        logger.Fatal("RunBollingerBandsCheck: {0}", ex);
                        logger.InfoFormat("Page: {0}", sPage);
                    }
                    if (symbolHistory.Chart.Result[0].indicators.unadjclose[0].unadjclose == null) continue;
                    List<DailyQuotes> quotesList = dailyQuotesORMService.ExtractDailyQuotes(symbol, symbolHistory);

                    if(SkipThisSymbol(quotesList)) continue;

                    var bollingerBands = CalculateBollingerBands(quotesList);
                    
                    var result = false;
                    if ( bollingerBands != null)
                        result = BulkLoadBollingerBands(bollingerBands);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("RunKeyStatisticsCollection: {0}", ex);
            }
            finally
            {
                logger.Info("End - RunBollingerBandsCheck");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }

        //public void LoadKeyStatisticsInfo()
        //{
        //    string url = "https://query1.finance.yahoo.com/v10/finance/quoteSummary/CAT?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=defaultKeyStatistics%2CfinancialData%2CcalendarEvents&corsDomain=finance.yahoo.com";
        //    string sPage = WebPage.Get(url);
        //    BaseObject.RootObject bo = JsonConvert.DeserializeObject<BaseObject.RootObject>(sPage);
        //}

        public void RunDaily(bool value)
        {
            _runDaily = value;
        }

        public List<BollingerBands> CalculateBollingerBands(List<DailyQuotes> quotesList)
        {
            List<BollingerBands> bolbands = new List<BollingerBands>();
            //double doubleOut = 0;
            decimal decimalOut = 0;
            int intOut = 0;
            List<decimal> current20 = new List<decimal>();
            List<decimal> current50 = new List<decimal>();
            List<decimal> current200 = new List<decimal>();

            for (int i = 0; i < 20; i++)
            {
                BollingerBands bb = new BollingerBands();

                bb.Symbol = quotesList[i].Symbol;
                decimal.TryParse(quotesList[i].Close.ToString(), out decimalOut);
                bb.Close = decimalOut;
                current20.Add(decimalOut);
                current50.Add(decimalOut);
                current200.Add(decimalOut);
                bb.Date = UnixTimeConverter.UnixTimeStampToDateTime(quotesList[i].Timestamp.Value);
                decimal.TryParse(quotesList[i].High.ToString(), out decimalOut);
                bb.High = decimalOut;
                decimal.TryParse(quotesList[i].Low.ToString(), out decimalOut);
                bb.Low = decimalOut;
                decimal.TryParse(quotesList[i].Open.ToString(), out decimalOut);
                bb.Open = decimalOut;
                int.TryParse(quotesList[i].Volume.ToString(), out intOut);
                bb.Volume = intOut;
                if(intOut == 0) return null;

                bolbands.Add(bb);
            };
            bolbands[19].SMA20 = current20.Sum() / 20;
            bolbands[19].SMA50 = 0;
            bolbands[19].SMA200 = 0;

            decimal sd = CalculateStandardDeviation(bolbands, bolbands[19].SMA20);
            bolbands[19].StandardDeviation = sd;
            bolbands[19].UpperBand = bolbands[19].SMA20 + (sd * 2);
            bolbands[19].LowerBand = bolbands[19].SMA20 - (sd * 2);
            bolbands[19].BandRatio = (1 - (bolbands[19].LowerBand/bolbands[19].UpperBand))*100;

            for(int i=20; i<50; i++)
            {
                current50.Add(0);
                current200.Add(0);
            }
            for (int i = 50; i < 200; i++)
            {
                 current200.Add(0);
            }

            for (int c20 = 0, c50 = 20, c200 = 20, i = 20; i < quotesList.Count(); i++)
            {
                BollingerBands bb = new BollingerBands();

                bb.Symbol = quotesList[i].Symbol;
                decimal.TryParse(quotesList[i].Close.ToString(), out decimalOut);
                bb.Close = decimalOut;

                if (decimalOut == 0)
                {
                    var wtf = decimalOut;
                    wtf++;
                }

                if (i%20 == 0)
                {
                    c20 = 0;
                }
                if (i % 50 == 0)
                {
                    c50 = 0;
                }
                if (i % 200 == 0)
                {
                    c200 = 0;
                }

                if (i>28)
                {
                    var wtf = decimalOut;
                    wtf++;
                }

                current20[c20++] = decimalOut;
                current50[c50++] = decimalOut;
                current200[c200++] = decimalOut;

                bb.Date = UnixTimeConverter.UnixTimeStampToDateTime(quotesList[i].Timestamp.Value);
                decimal.TryParse(quotesList[i].High.ToString(), out decimalOut);
                bb.High = decimalOut;
                decimal.TryParse(quotesList[i].Low.ToString(), out decimalOut);
                bb.Low = decimalOut;
                decimal.TryParse(quotesList[i].Open.ToString(), out decimalOut);
                bb.Open = decimalOut;
                decimal.TryParse(quotesList[i - 20].UnadjClose.ToString(), out decimalOut);

                int.TryParse(quotesList[i].Volume.ToString(), out intOut);
                bb.Volume = intOut;
                if (intOut == 0) return null;

                bb.SMA20 = current20.Sum() / 20;
                if (i > 48)
                    bb.SMA50 = current50.Sum() / 50;
                if (i > 198)
                    bb.SMA200 = current200.Sum() / 200;
                bolbands.Add(bb);
                
                sd = CalculateStandardDeviation(bolbands, bb.SMA20);
                bolbands[i].StandardDeviation = sd;
                bolbands[i].UpperBand = bolbands[i].SMA20 + (sd * 2);
                bolbands[i].LowerBand = bolbands[i].SMA20 - (sd * 2);
                bolbands[i].BandRatio = (1 - (bolbands[i].LowerBand / bolbands[i].UpperBand)) * 100;
            }

            return bolbands;
        }

        #endregion Public Methods

        #region Private Methods

        private bool SkipThisSymbol(List<DailyQuotes> quotes)
        {
            if (quotes.Count < 30) return true;

            DailyQuotes dq = quotes[quotes.Count - 1];
            if (dq.Close < 3) return true;

            return false;
        }

        private decimal CalculateStandardDeviation(List<BollingerBands> bolbands, decimal mean)
        {
            List<decimal> closeLessMean = new List<decimal>();
            List<decimal> closeLessMeanX2 = new List<decimal>();
            decimal decimalCast;
            double doubleCast;

            for (int c = 0, i = bolbands.Count-20; i< bolbands.Count; i++)
            {
                closeLessMean.Add(bolbands[i].Close - mean);
                doubleCast = (double)closeLessMean[c++];
                decimalCast = (decimal)Math.Pow(doubleCast, 2);
                closeLessMeanX2.Add(decimalCast);
            }

            double StdDeveation = (double)closeLessMeanX2.Sum()/20;
            doubleCast = Math.Sqrt(StdDeveation);

            return (decimal)doubleCast;
        }

        private bool BulkLoadBollingerBands(List<BollingerBands> bollband)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadBollingerBands.ConfigureDataTable();

                dt = bulkLoadBollingerBands.LoadDataTableWithDailyHistory(bollband, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadHistory", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadBollingerBands.BulkCopy<BollingerBands>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadBollingerBands returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Bollinger Bands Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        public class ClosingPrices
        {
            public double ClosingPrice { get; set; }
        }

        #endregion Private Methods
    }
}
