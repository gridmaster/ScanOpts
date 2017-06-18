using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Core;
using Core.Interface;
using Core.ORMModels;
using Core.JsonKeyStatistics;
using ORMService;

namespace Services
{
    public class KeyStatisticsService : BaseService
    {
        #region Private properties
        private bool success;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        //private BulkLoadInsiders bulkLoadInsiders = null;
        #endregion Private properties

        #region Constructors

        public KeyStatisticsService(ILogger logger, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService) // , BulkLoadInsiders bulkLoadKeyStatistics)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            //this.bulkLoadKeyStatistics = new BulkLoadKeyStatistics(logger);
        }

        #endregion Constructors

        #region Public Methods

        public void RunKeyStatisticsCollection()
        {
            logger.InfoFormat("RunKeyStatisticsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunKeyStatisticsCollection(symbols);
        }

        public void RunKeyStatisticsCollection(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            RunKeyStatisticsCollection(syms);
        }

        public void RunKeyStatisticsCollection(List<string> symbols)
        {
            logger.InfoFormat("Start - RunKeyStatisticsCollection");

            string url = "https://query1.finance.yahoo.com/v10/finance/quoteSummary/{0}?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=defaultKeyStatistics%2CfinancialData%2CcalendarEvents&corsDomain=finance.yahoo.com";

            try
            {
                foreach (string sym in symbols)
                {
                    List<CallPuts> allCallPuts = new List<CallPuts>();
                    logger.InfoFormat("Get {0} page", sym);

                    string sPage = WebPage.Get(string.Format(url, sym));
                    BaseObject.RootObject quoteSummary = JsonConvert.DeserializeObject<BaseObject.RootObject>(sPage);

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

        #region Private Methods
        private bool BulkLoadKeyStatistics(List<Symbols> allSymbols)
        {
            bool success = false;
            try
            {
                //var dt = bulkLoadSymbol.ConfigureDataTable();

                //dt = bulkLoadSymbol.LoadDataTableWithSymbols(allSymbols, dt);

                //if (dt == null)
                //{
                //    logger.InfoFormat("{0}BulkLoadKeyStatistics: No data returned on LoadDataTableWithSymbols", Environment.NewLine);
                //}
                //else
                //{
                //    success = bulkLoadSymbol.BulkCopy<Symbols>(dt, "ScanOptsContext");
                //    logger.InfoFormat("{0}BulkLoadKeyStatistics returned with: {1}", Environment.NewLine,
                //                            success ? "Success" : "Fail");
                //}
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}BulkLoadKeyStatistics: Bulk Load Symbols Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        #endregion Private Methods
    }
}
