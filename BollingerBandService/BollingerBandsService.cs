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
            logger.InfoFormat("Start - RunKeyStatisticsCollection");

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

                    if (quotesList.Count < 1) continue;

                    DailyQuotes dq = quotesList[quotesList.Count-1];


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
                logger.Info("End - RunKeyStatisticsCollection");
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
    }
}
