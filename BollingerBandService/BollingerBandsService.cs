using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            decimal adjClose = 0;
            BollingerBandsList bolbands = new BollingerBandsList();
            bolbands.BollingerBands = new List<BollingerBand>();

            for (int i = 0; i < 20; i++)
            {
                adjClose += quotesList[i].UnadjClose.Value;
            }

            bolbands.BollingerBands.Add(LoadBollingerBand(quotesList[20]));

            for (int i = 20; i < quotesList.Count; i++)
            {
                adjClose += quotesList[i].UnadjClose.Value;
                if (i > 19) // once we have at lest 20
                { 

                }

            }

            return result;
        }

        private BollingerBand LoadBollingerBand(DailyQuotes quotesList)
        {
            var bb = new BollingerBand
            {
                Symbol = quotesList.Symbol,
                Close = quotesList.Close.Value,
                Date = UnixTimeConverter.UnixTimeStampToDateTime(quotesList.Timestamp.Value),
                // Exchange = quotesList.Exchange,
                High = quotesList.High.Value,
                //Id = quotesList.Id,
                //InstrumentType
                Low = quotesList.Low.Value,
                Open = quotesList.Open.Value
                //Timestamp
                //Volume
            };
            return bb;
        }

        #endregion Private Methods
    }
}
