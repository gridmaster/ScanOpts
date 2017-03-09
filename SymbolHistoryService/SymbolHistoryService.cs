using Core;
using Core.Interface;
using Core.JsonModels;
using DIContainer;
using Newtonsoft.Json;
using ORMService;
using System;
using System.Collections.Generic;

namespace SymbolHistoryService
{
    public class SymbolHistoryService : IHistoryService
    {
        public void RunHistoryCollection()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("RunHistoryCollection - GetSymbols");
            List<string> symbols = IOCContainer.Instance.Get<SymbolsORMService>().GetSymbols();
            RunHistoryCollection(symbols);
        }

        public void RunHistoryCollection(List<string> symbols)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start - RunHistoryCollection");

            try
            {
                foreach (string symbol in symbols)
                {
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {0} history page", symbol);
                    // this gets the history
                    string uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1=-252356400&period2=1488949200&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";
                    string sPage = WebPage.Get(String.Format(uriString, symbol));
                    IOCContainer.Instance.Get<ILogger>().Info("Page captured");

                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}", symbol);
                    JsonResult symbolHistory = JsonConvert.DeserializeObject<JsonResult>(sPage);
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized", symbol);
                }
            }
            catch (Exception exc)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("Sucker blew up: {0}", exc);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().Info("End - RunOptionsCollection");
                //IOCContainer.Instance.Get<ILogger>().InfoFormat("We're done'...{0}", Environment.NewLine);
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }
    }
}
