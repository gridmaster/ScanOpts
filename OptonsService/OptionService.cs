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

namespace OptonService
{
    public class OptionService : IOptionService
    {
        public void RunOptionsCollection()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("RunOptionsCollection - GetSymbols");
            List<string> symbols = IOCContainer.Instance.Get<SymbolsORMService>().GetSymbols();
            RunOptionsCollection(symbols);
        }

        public void RunOptionsCollection(List<string> symbols)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start - RunOptionsCollection");
            decimal date = UnixTimeConverter.ToUnixTime(DateTime.Now.AddDays(-1)); // 1489708800;

            try
            {
                foreach (string symbol in symbols)
                {
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {0} page", symbol);

                    // this gets the options chain ... need the dates.
                    string uriString = "https://query2.finance.yahoo.com/v7/finance/options/{0}?formatted=true&crumb=bE4Li32tCWR&lang=en-US&region=US&straddle=true&date={1}&corsDomain=";
                    string sPage = WebPage.Get(String.Format(uriString, symbol, date));
                    IOCContainer.Instance.Get<ILogger>().Info("Page captured");

                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}", symbol);

                    // dont look here to see if this is correctly populated. This is just to get the dates
                    JsonResult optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized", symbol);

                    List<decimal> expireDates = optionChain.OptionChain.Result[0].ExpirationDates;

                    Statistics statistics = new Statistics();
                    int newId = 0;

                    foreach (decimal eDate in expireDates)
                    {
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {0} page for expiration date {1}", symbol, eDate);
                        sPage = WebPage.Get(String.Format(uriString, symbol, eDate));
                        IOCContainer.Instance.Get<ILogger>().Info("Page captured");

                        IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}", symbol);
                        optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized", symbol);

                        if (String.IsNullOrEmpty(statistics.Symbol))
                        {
                            statistics = IOCContainer.Instance.Get<IStatisticORMService>().ExtractAndSaveStatisticFromOptionChain(optionChain);

                            newId = IOCContainer.Instance.Get<IStatisticORMService>().Add(statistics);
                        }

                        if (newId == 0) return;

                        List<CallPut> callputs = IOCContainer.Instance.Get<IOptionORMService>().ExtractCallsAndPutsFromOptionChain(statistics.Symbol, newId, optionChain.OptionChain.Result[0].Options[0].Straddles);

                        IOCContainer.Instance.Get<ICallPutORMService>().AddMany(callputs);
                    }
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("RunOptionsCollection: {0}", ex);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().Info("End - RunOptionsCollection");
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }
    }
}
