using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;
using Core;
using Core.Business;
using Core.Interface;
using Core.ORMModels;
using Core.JsonKeyStatistics;
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

        //private BulkLoadInsiders bulkLoadInsiders = null;
        #endregion Private properties

        #region Constructors

        public BollingerBandsService(ILogger logger, DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService) // , BulkLoadInsiders bulkLoadInsiders)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.dailyQuotesORMService = dailyQuotesORMService;
            //this.bulkLoadInsiders = new BulkLoadInsiders(logger);
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

            string uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1={1}&period2={2}&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";

            var endDate = DateTime.Now.ToUnixTime();
            var startDate = DateTime.Now.AddDays(-200).ToUnixTime();

            try
            {
                foreach (string symbol in symbols)
                {
                    List<CallPuts> allCallPuts = new List<CallPuts>();
                    logger.InfoFormat("Get {0} page", symbol);

                    string sPage = WebPage.Get(String.Format(uriString, symbol, startDate, endDate));

                    if (sPage.Contains("(404) Not Found")) continue;
                    if (sPage.Contains("(400) Bad Request")) continue;

                    Core.JsonQuote.JsonResult symbolHistory = JsonConvert.DeserializeObject<Core.JsonQuote.JsonResult>(sPage);
                    List<DailyQuotes> quotesList = dailyQuotesORMService.ExtractDailyQuotes(symbol, symbolHistory);

                    if(SkipThisSymbol(quotesList)) continue;

                    if (ThisIsAGoodCandidate(symbol, quotesList))
                    {
                        symbolList.Add(symbol);
                    }

                    int newId = 0;

                    //BulkLoadKeyStatistics(allCallPuts);
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

        public void LoadKeyStatisticsInfo()
        {
            string url = "https://query1.finance.yahoo.com/v10/finance/quoteSummary/CAT?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=defaultKeyStatistics%2CfinancialData%2CcalendarEvents&corsDomain=finance.yahoo.com";
            string sPage = WebPage.Get(url);
            BaseObject.RootObject bo = JsonConvert.DeserializeObject<BaseObject.RootObject>(sPage);
        }

        #endregion Public Methods

        #region Private Methods

        private bool SkipThisSymbol(List<DailyQuotes> quotes)
        {
            if (quotes.Count < 1) return true;

            DailyQuotes dq = quotes[quotes.Count - 1];
            if (dq.Close < 3) return true;

            return false;
        }

        private bool ThisIsAGoodCandidate(string symbol, List<DailyQuotes> quotesList)
        {
            bool result = false;
            double adjClose = 0, x2adjClose = 0;
            List<BollingerBand> bolbands = new List<BollingerBand>();

            List<ClosingPrices> closingPrice = new List<ClosingPrices>();

            return quotesList[quotesList.Count - 1].UnadjClose > 2;

            string nums = "90.7, 92.9, 92.98, 91.8, 92.66, 92.68, 92.3, 92.77, 92.54, 92.95, 93.2, 91.07, 89.83, 89.74, 90.4, 90.74, 88.02, 88.09, 88.84, 90.78, 90.54, 91.39, 90.65";

            var numb = nums.Split(',');

            for (int i = 0; i < 20; i++)
            {
                double doubleOut = 0;
                double.TryParse(numb[i].ToString(), out doubleOut);

                BollingerBand bb = new BollingerBand
                {
                    Symbol = "FAKE", // quotesList[20].Symbol,
                    Close = doubleOut, // quotesList[20].Close.Value,
                    Date = DateTime.Now, // UnixTimeConverter.UnixTimeStampToDateTime(quotesList[20].Timestamp.Value),
                    High = doubleOut, //quotesList[20].High.Value,
                    Low = doubleOut, //quotesList[20].Low.Value,
                    Open = doubleOut, //quotesList[20].Open.Value,
                    SMA20 = adjClose / 20
                };
                bolbands.Add(bb);
            }

            double sd = CalculateStandardDeviation(bolbands);

            return result;
        }

        private bool CalculateBollingerBands(string symbol, List<DailyQuotes> quotesList)
        {
            bool result = false;
            double adjClose = 0, x2adjClose = 0;
            List<BollingerBand> bolbands = new List<BollingerBand>();

            string numb = "90.7, 92.9, 92.98, 91.8, 92.66, 92.68, 92.3, 92.77, 92.54, 92.95, 93.2, 91.07, 89.83, 89.74, 90.4, 90.74, 88.02, 88.09, 88.84, 90.78, 90.54, 91.39, 90.65";

            var numz = numb.Split(',');

            for (int i = 0; i < 20; i++)
            {
                double doubleOut = 0;
                double.TryParse(numz[i].ToString(), out doubleOut);

                BollingerBand bb = new BollingerBand
                {
                    Symbol = "FAKE", // quotesList[20].Symbol,
                    Close = doubleOut, // quotesList[20].Close.Value,
                    Date = DateTime.Now, // UnixTimeConverter.UnixTimeStampToDateTime(quotesList[20].Timestamp.Value),
                    High = doubleOut, //quotesList[20].High.Value,
                    Low = doubleOut, //quotesList[20].Low.Value,
                    Open = doubleOut //, //quotesList[20].Open.Value,
                    //SMA20 = adjClose / 20
                };
                bb.SMA20 = adjClose / 20;
                bolbands.Add(bb);
            }

            double sd = CalculateStandardDeviation(bolbands);
            bolbands[19].StandardDeviation = sd;
            bolbands[19].UpperBand = bolbands[19].SMA20 + (sd * 2);
            bolbands[19].LowerBand = bolbands[19].SMA20 - (sd * 2);

            for(int i = 20; i < numz.Count; i++)
            {

            }


            return result;
        }

        private double CalculateStandardDeviation(List<BollingerBand> bolbands)
        {
            List<double> closeLessMean = new List<double>();
            List<double> closeLessMeanX2 = new List<double>();

            double mean = bolbands.Sum<BollingerBand>(s => s.Close)/20;

            for(int i = 0; i<20; i++)
            {
                closeLessMean.Add(bolbands[i].Close - mean);
                closeLessMeanX2.Add(Math.Pow(closeLessMean[i], 2));
            }

            double StdDeveation = closeLessMeanX2.Sum()/20;
            return Math.Sqrt(StdDeveation);
        }

        private BollingerBand LoadBollingerBand(DailyQuotes quotesList)
        {
            var bb = new BollingerBand
            {
                //Symbol = quotesList.Symbol,
                //Close = quotesList.Close.Value,
                //Date = UnixTimeConverter.UnixTimeStampToDateTime(quotesList.Timestamp.Value),
                //// Exchange = quotesList.Exchange,
                //High = quotesList.High.Value,
                ////Id = quotesList.Id,
                ////InstrumentType
                //Low = quotesList.Low.Value,
                //Open = quotesList.Open.Value
                ////Timestamp
                ////Volume
            };
            return bb;
        }

        public class ClosingPrices
        {
            public double ClosingPrice { get; set; }
        }

        #endregion Private Methods
    }
}
