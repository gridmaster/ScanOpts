using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using Core.Interface;
using Core.ORMModels;
using DIContainer;
using ORMService;
using Core.JsonOptions;
using Core.Business;
using Core.BulkLoad;

namespace OptonService
{
    public class OptionService : BaseService, IOptionService
    {

        #region Constructors

        public OptionService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        public void RunOptionsCollection()
        {
            logger.InfoFormat("RunOptionsCollection - GetSymbols");
            List<string> symbols = IOCContainer.Instance.Get<SymbolsORMService>().GetSymbols();
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
                            statistics = IOCContainer.Instance.Get<IStatisticORMService>().ExtractAndSaveStatisticFromOptionChain(optionChain);

                            newId = IOCContainer.Instance.Get<IStatisticORMService>().Add(statistics);
                        }

                        //if (newId == 0) return;

                        List<CallPuts> callputs = IOCContainer.Instance.Get<IOptionORMService>()
                            .ExtractCallsAndPutsFromOptionChain(statistics.Symbol, newId, optionChain.OptionChain.Result[0].Options[0].Straddles);

                        allCallPuts.AddRange(callputs);
                        //IOCContainer.Instance.Get<ICallPutORMService>().AddMany(callputs);
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

        private bool BulkLoadCallPuts(List<CallPuts> allCallPuts)
        {
            bool success = false;
            try
            {
                var dt = IOCContainer.Instance.Get<BulkLoadCallPuts>().ConfigureDataTable();

                dt = IOCContainer.Instance.Get<BulkLoadCallPuts>().LoadDataTableWithSymbols(allCallPuts, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on LoadDataTableWithSymbols", Environment.NewLine);
                }
                else
                {
                    success = IOCContainer.Instance.Get<BulkLoadCallPuts>().BulkCopy<CallPuts>(dt, "ScanOptsContext");
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
    }
}
