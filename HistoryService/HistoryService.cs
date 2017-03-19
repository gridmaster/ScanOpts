using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using Core.Interface;
using Core.ORMModels;
using DIContainer;
using ORMService;

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
            var i = 0;

            try
            {
                foreach (string symbol in symbols)
                {
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {0} history page", symbol);
                    // this gets the history
                    string uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1=-252356400&period2=1488949200&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";
                    string sPage = WebPage.Get(String.Format(uriString, symbol));

                    IOCContainer.Instance.Get<ILogger>().Info("Page captured");

                    Core.JsonQuote.JsonResult symbolHistory = JsonConvert.DeserializeObject<Core.JsonQuote.JsonResult>(sPage);

                    List<DailyQuotes> quotesList = IOCContainer.Instance.Get<IDailyQuotesORMService>().ExtractDailyQuotes(symbol, symbolHistory);

                    IOCContainer.Instance.Get<IDailyQuotesORMService>().AddMany(quotesList);
                    
                    var wtf = IOCContainer.Instance.Get<IDailyQuotesORMService>().GetDividends(symbol, symbolHistory);

                    //var timestamps = symbolHistory.Chart.Result[0].timestamp;
                    //string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
                    //string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
                    //DateTime date = DateTime.Now;

                    //    if (symbolHistory.Chart.Result[0].events.splits != null)
                    //    {
                    //        Core.JsonModels.ORMModels.Splits splits = new Core.JsonModels.ORMModels.Splits();
                    //        Split split = new Split();

                    //        foreach (var item in symbolHistory.Chart.Result[0].events.splits.split)
                    //        {
                    //            split.date = date;
                    //            split.symbol = symbol;
                    //            split.exchange = exchangeName;
                    //            split.splitDate = (int)item.Value["date"];
                    //            split.numerator = (int)item.Value["numerator"];
                    //            split.denominator = (int)item.Value["denominator"];
                    //            split.ratio = item.Value["splitRatio"].ToString();
                    //            splits.Add(split);
                    //        }
                    //    }
                    //}

                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized", symbol);
                }
            }
            catch (Exception exc)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("RunHistoryCollection: {0}", exc);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().Info("End - RunQuoteHistoryCollection");
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }
    }
}
