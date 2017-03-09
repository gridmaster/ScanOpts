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
    public class HistoryService : IHistoryService
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

                    //sPage = "{ \"chart\": { \"result\": [{ \"meta\": { \"currency\": \"USD\", \"symbol\": \"CAT\", \"exchangeName\": \"NYQ\", \"instrumentType\": \"EQUITY\", \"firstTradeDate\": -252342000, \"gmtoffset\": -18000, \"timezone\": \"EST\", \"currentTradingPeriod\": { \"pre\": { \"timezone\": \"EST\", \"end\": 1488983400, \"start\": 1488963600, \"gmtoffset\": -18000}, \"regular\": {\"timezone\": \"EST\", \"end\": 1489006800, \"start\": 1488983400, \"gmtoffset\": -18000}, \"post\": {\"timezone\": \"EST\", \"end\": 1489021200, \"start\": 1489006800, \"gmtoffset\": -18000}}, \"dataGranularity\": \"1d\", \"validRanges\": [\"1d\", \"5d\", \"1mo\", \"3mo\", \"6mo\", \"1y\", \"2y\", \"5y\", \"10y\", \"ytd\", \"max\"]}}]{ \"chart\": { \"result\": [{ \"meta\": { \"currency\": \"USD\", \"symbol\": \"CAT\"}}], \"error\": null}}}}";
                    //sPage = "{ \"chart\": { \"result\": [{ \"meta\": { \"currency\": \"USD\", \"symbol\": \"CAT\"}}], \"error\": null}}";
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}", symbol);
                    Core.JsonModels.HistoryDetail.JsonResult symbolHistory = JsonConvert.DeserializeObject< Core.JsonModels.HistoryDetail.JsonResult> (sPage);
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized", symbol);
                    break;
                    // List<decimal> expireDates = symbolHistory.OptionChain.Result[0].ExpirationDates;

                    Quote quote = new Quote();
                    int newId = 0;

                    //foreach (decimal eDate in expireDates)
                    //{
                    //    IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {0} page for expiration date {1}", symbol, eDate);
                    //    sPage = WebPage.Get(String.Format(uriString, symbol, eDate));
                    //    IOCContainer.Instance.Get<ILogger>().Info("Page captured");

                    //    IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}", symbol);
                    //    optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                    //    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized", symbol);

                    //    if (String.IsNullOrEmpty(quote.Symbol))
                    //    {
                    //        quote = IOCContainer.Instance.Get<IQuoteORMService>().ExtractAndSaveQuoteFromOptionChain(optionChain);

                    //        newId = IOCContainer.Instance.Get<IQuoteORMService>().Add(quote);
                    //    }

                    //    if (newId == 0) return;

                    //    List<Straddles> wtf = optionChain.OptionChain.Result[0].Options[0].Straddles;

                    //    List<CallPut> callputs = IOCContainer.Instance.Get<IOptionORMService>().ExtractCallsAndPutsFromOptionChain(quote.Symbol, newId, optionChain.OptionChain.Result[0].Options[0].Straddles);

                    //    IOCContainer.Instance.Get<ICallPutORMService>().AddMany(callputs);
                    //}
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
