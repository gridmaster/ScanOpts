using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using Core.Interface;
using Core.ORMModels;
using ORMService;
using Core.JsonOptions;
using Core.Business;
using Core.BulkLoad;

namespace OptonService
{
    public class OptionService : BaseService, IOptionService
    {
        #region Private Methods
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private StatisticORMService statisticORMService = null;
        private BulkLoadCallPuts bulkLoadCallPuts = null;
        private OptionORMService optionORMService = null;
        #endregion Private Methods

        #region Constructors

        public OptionService(ILogger logger, SymbolsORMService symbolORMServic, StatisticORMService statisticORMService, OptionORMService optionORMService)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.statisticORMService = new StatisticORMService(logger);
            this.bulkLoadCallPuts = new BulkLoadCallPuts(logger);
            this.optionORMService = new OptionORMService(logger);
        }

        #endregion Constructors

        #region Public Methods

        public void RunOptionsCollection()
        {
            logger.InfoFormat("RunOptionsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunOptionsCollection(symbols);
        }

        public void RunOptionsCollection(List<string> symbols)
        {
            logger.InfoFormat("Start - RunOptionsCollection");
            decimal date = UnixTimeConverter.ToUnixTime(DateTime.Now.AddDays(-1)); // 1489708800;
            List<CallPuts> allCallPuts = new List<CallPuts>();

            try
            {
                foreach (string symbol in symbols)
                {
                    logger.InfoFormat("Get {0} page", symbol);

                    // this gets the options chain ... need the dates.
                    string uriString = "https://query2.finance.yahoo.com/v7/finance/options/{0}?formatted=true&crumb=bE4Li32tCWR&lang=en-US&region=US&straddle=true&date={1}&corsDomain=";
                    string sPage = WebPage.Get(String.Format(uriString, symbol, date));
                    //logger.Info("Page captured");

                    //logger.InfoFormat("Deserialize {0}", symbol);

                    // dont look here to see if this is correctly populated. This is just to get the dates
                    JsonResult optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                    //logger.InfoFormat("{0} deserialized", symbol);

                    List<decimal> expireDates = optionChain.OptionChain.Result[0].ExpirationDates;

                    Statistics statistics = new Statistics();
                    int newId = 0;

                    foreach (decimal eDate in expireDates)
                    {
                        logger.InfoFormat("Get {0} page for expiration date {1}", symbol, eDate);
                        sPage = WebPage.Get(String.Format(uriString, symbol, eDate));
                        //logger.Info("Page captured");

                        //logger.InfoFormat("Deserialize {0}", symbol);
                        optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                        //logger.InfoFormat("{0} deserialized", symbol);

                        if (String.IsNullOrEmpty(statistics.Symbol))
                        {
                            statistics = statisticORMService.ExtractAndSaveStatisticFromOptionChain(optionChain);

                            newId = statisticORMService.Add(statistics);
                        }

                        //if (newId == 0) return;

                        List<CallPuts> callputs = optionORMService.ExtractCallsAndPutsFromOptionChain(statistics.Symbol, newId, optionChain.OptionChain.Result[0].Options[0].Straddles);

                        allCallPuts.AddRange(callputs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("RunOptionsCollection: {0}", ex);
            }
            finally
            {
                logger.Info("RunOptionsCollection: BulkLoadCallPuts...");
                BulkLoadCallPuts(allCallPuts);
                logger.Info("End - RunOptionsCollection");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool BulkLoadCallPuts(List<CallPuts> allCallPuts)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadCallPuts.ConfigureDataTable();

                dt = bulkLoadCallPuts.LoadDataTableWithSymbols(allCallPuts, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on LoadDataTableWithSymbols", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadCallPuts.BulkCopy<CallPuts>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadOptions returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Options Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        #endregion Private Methods
    }
}
