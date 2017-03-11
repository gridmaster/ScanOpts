using Core;
using Core.Interface;
using Core.JsonModels;
using Core.JsonModels.HistoryDetail;
using DIContainer;
using Newtonsoft.Json;
using ORMService;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

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

                    Core.JsonModels.HistoryDetail.JsonResult symbolHistory = JsonConvert.DeserializeObject<Core.JsonModels.HistoryDetail.JsonResult>(sPage);
                    List<Quotes> quotesList = new List<Quotes>();

                    var timestamps = symbolHistory.Chart.Result[0].timestamp;
                    string exchangeName = symbolHistory.Chart.Result[0].meta.exchangeName;
                    string instrumentType = symbolHistory.Chart.Result[0].meta.instrumentType;
                    DateTime date = DateTime.Now;

                    for (i = 0; i < symbolHistory.Chart.Result[0].timestamp.Count; i++)
                    {
                        int holdInt = 0;

                        Core.JsonModels.HistoryDetail.Quotes quote = new Core.JsonModels.HistoryDetail.Quotes();
                        quote.date = date;
                        quote.symbol = symbol;
                        quote.exchangeName = exchangeName;
                        quote.instrumentType = instrumentType;
                        quote.timestamp = int.TryParse(timestamps[i].ToString(), out holdInt) ? timestamps[i] : (int?)null;

                        quote.open = symbolHistory.Chart.Result[0].indicators.quote[0].open[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].open[i].ToString());
                        quote.close = symbolHistory.Chart.Result[0].indicators.quote[0].close[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].close[i].ToString());
                        quote.high = symbolHistory.Chart.Result[0].indicators.quote[0].high[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].high[i].ToString());
                        quote.low = symbolHistory.Chart.Result[0].indicators.quote[0].low[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].low[i].ToString());
                        quote.volume = symbolHistory.Chart.Result[0].indicators.quote[0].volume[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.quote[0].volume[i].ToString());
                        quote.unadjopen = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjopen[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjopen[i].ToString());
                        quote.unadjclose = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjclose[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjclose[i].ToString());
                        quote.unadjhigh = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjhigh[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjhigh[i].ToString());
                        quote.unadjlow = symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjlow[i] == null ? 0 : ConvertStringToDecimal(symbolHistory.Chart.Result[0].indicators.unadjquote[0].unadjlow[i].ToString());
                        quotesList.Add(quote);
                    }

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

        public static decimal ConvertStringToDecimal(string value)
        {
            decimal holdDecimal = 0;
            decimal.TryParse(value.ToString(), out holdDecimal);
            return holdDecimal;
        }
    }
}
