using Core;
using Core.BulkLoad;
using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using DIContainer;
using ORMService;

namespace DailySymbolService
{
    public class SymbolService : BaseService, ISymbolService
    {
        private bool success;

        #region Constructors

        public SymbolService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        public List<Symbols> GetFromDBSymbolsFromListOfExchanges(List<string> exchanges)
        {
            //List<Symbols> symbols = new List<Symbols>();

            var symbols = IOCContainer.Instance.Get<SymbolsORMService>().GetFromDBSymbolsFromTheseExchanges(null);

            return symbols;
        }
        
        public void LoadAllSymbolsFromWeb()
        {
            LoadAllSymbolsFromAllExchanges();
        }

        public void LoadAllSymbolsFromAllExchanges()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("LoadAllSymbolsFromAllExchanges - GetSymbols");
            List<string> exchanges = IOCContainer.Instance.Get<ExchangeORMService>().GetExchanges();
            LoadAllSymbolsFromAllExchanges(exchanges);
        }

        public List<Symbols> LoadAllSymbolsFromAllExchanges(List<string> exchanges)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start - LoadAllSymbolsFromAllExchanges");

            List<Symbols> allSymbols = new List<Symbols>();
            string exchangeSave = "";
            string itemSave = "";
            try
            {
                foreach (string exchange in exchanges)
                {
                    exchangeSave = exchange;
                    string sPage = WebPage.Get(String.Format(exchange, "0"));
                    string sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));
                    string pages = sub1.Substring(0, sub1.IndexOf("</table>") + "</table>".Length);

                    List<string> symbolLookups = GetPagesLookup(pages);

                    string sub3 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                    string symbols = sub3.Substring(0, sub3.IndexOf("</table>") + "</table>".Length);

                    //nasdaq = "http://eoddata.com/stocklist/NASDAQ/{0}.htm";
                    List<Symbols> symbolList = new List<Symbols>();

                    string xchange = exchange.Replace("http://eoddata.com/stocklist/", "").Replace("/{0}.htm", "");

                    foreach (string item in symbolLookups)
                    {
                        itemSave = item;
                        sPage = WebPage.Get(string.Format(exchange, item));
                        sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));

                        string sub2 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                        string symbolz = sub2.Substring(0, sub2.IndexOf("</table>") + "</table>".Length);
                        
                        symbolList.AddRange(GetSymbolLookup(symbolz, xchange));
                    }

                    //IOCContainer.Instance.Get<SymbolsORMService>().Add(symbolList[0]);

                    allSymbols.AddRange(symbolList);
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("LoadAllSymbolsFromAllExchanges: {0}", ex);
            }
            finally
            {
                success = BulkLoadSymbols(allSymbols);

                IOCContainer.Instance.Get<ILogger>().Info("End - LoadAllSymbolsFromAllExchanges");
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
            return allSymbols;
        }

        public List<Symbols> LoadAllSymbolsFromAllExchanges(List<Exchanges> exchanges)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start - LoadAllSymbolsFromAllExchanges");

            List<Symbols> allSymbols = new List<Symbols>();
            string exchangeSave = "";
            string itemSave = "";
            try
            {
                foreach (Exchanges exchange in exchanges)
                {
                    exchangeSave = exchange.Exchange;
                    string sPage = WebPage.Get(String.Format(exchange.Exchange, "0"));
                    string sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));
                    string pages = sub1.Substring(0, sub1.IndexOf("</table>") + "</table>".Length);

                    List<string> symbolLookups = GetPagesLookup(pages);

                    string sub3 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                    string symbols = sub3.Substring(0, sub3.IndexOf("</table>") + "</table>".Length);

                    //nasdaq = "http://eoddata.com/stocklist/NASDAQ/{0}.htm";
                    List<Symbols> symbolList = new List<Symbols>();

                    foreach (string item in symbolLookups)
                    {
                        itemSave = item;
                        sPage = WebPage.Get(string.Format(exchange.Exchange, item));
                        sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));

                        string sub2 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                        string symbolz = sub2.Substring(0, sub2.IndexOf("</table>") + "</table>".Length);

                        symbolList.AddRange(GetSymbolLookup(symbolz, exchange.Exchange));
                    }

                    allSymbols.AddRange(symbolList);
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("LoadAllSymbolsFromAllExchanges: {0}", ex);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().Info("End - LoadAllSymbolsFromAllExchanges");
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
            return allSymbols;
        }

        private bool BulkLoadSymbols(List<Symbols> allSymbols)
        {
             bool success = false;
            try
            {
                var dt = IOCContainer.Instance.Get<BulkLoadSymbol>().ConfigureDataTable();

                dt = IOCContainer.Instance.Get<BulkLoadSymbol>().LoadDataTableWithSymbols(allSymbols, dt);

                if (dt == null)
                {
                    IOCContainer.Instance.Get<ILogger>()
                                .InfoFormat("{0}No data returned on LoadDataTableWithSymbols", Environment.NewLine);
                }
                else
                {
                    success = IOCContainer.Instance.Get<BulkLoadSymbol>().BulkCopy<Symbols>(dt, "ScanOptsContext");
                    IOCContainer.Instance.Get<ILogger>()
                                .InfoFormat("{0}BulkLoadOptions returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Bulk Load Options Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        private List<string> GetPagesLookup(string pages)
        {
            List<string> myPages = null;
            string work = "";

            work = pages.Substring(pages.IndexOf("<tr>") + "<tr>".Length);
            work = work.Substring(0, work.IndexOf("</tr>"));

            work = work.Replace("<td ", "|");
            work = work.Substring(1);
            myPages = work.Split('|').ToList<string>();

            for(int i = 0; i < myPages.Count; i++)
            {
                myPages[i] = myPages[i].Replace("class=\"ls\">", "");
                myPages[i] = myPages[i].Replace("class=\"ld\"><a href=\"", "");
                myPages[i] = myPages[i].Replace("</td>", "").Replace("</a>", "");
                if( i>0)
                    myPages[i] = myPages[i].Substring(myPages[i].Length - 1);
            }

            return myPages;
        }

        private List<Symbols> GetSymbolLookup(string pages, string exchange)
        {
            List<string> myPages = null;
            string work = "";

            work = pages.Substring(pages.ToUpper().IndexOf("<TR "));

            work = work.Replace("<tr ", "|");
            work = work.Substring(1);
            myPages = work.Split('|').ToList<string>();
            List<Symbols> tempquote = new List<Symbols>();
            
            for (int i = 0; i < myPages.Count; i++)
            {
                Symbols dq = new Symbols();
                dq.Exchange = exchange;
                //dq.FullExchangeName = exchange.FullExchangeName;
                string symbol = myPages[i].Substring(myPages[i].IndexOf("<A href="));
                symbol = symbol.Substring(symbol.IndexOf(">")+1);
                myPages[i] = symbol;
                dq.Symbol = symbol.Substring(0, symbol.IndexOf("<"));

                string company = myPages[i].Substring(myPages[i].IndexOf("<td>") + 4);
                myPages[i] = company;
                if (company.IndexOf("<") > 0)
                    dq.CompanyName = company.Substring(0, company.IndexOf("<") - 1);
                else
                    dq.CompanyName = "";
                myPages[i] = myPages[i].Substring(myPages[i].IndexOf("</td>") + 5);
                myPages[i] = myPages[i].Replace("<td align=right>", "|");
                myPages[i] = myPages[i].Substring(1);
                var prices = myPages[i].Split('|');
                dq.Date = DateTime.Now;
                dq.Select = false;

                tempquote.Add(dq);
            }

            return tempquote;
        }
    }
}
