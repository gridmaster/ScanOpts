using Core;
using Core.BulkLoad;
using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using ORMService;

namespace DailySymbolService
{
    public class SymbolService : BaseService, ISymbolService
    {
        #region Private properties
        private bool success;
        private List<Symbols> symbols;
        private List<string> symbolStringList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private BulkLoadSymbol bulkLoadSymbol = null;

        private string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "Y", "Z" };
        #endregion Private properties

        #region Constructors

        public SymbolService(ILogger logger, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService, BulkLoadSymbol bulkLoadSymbol)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.bulkLoadSymbol = new BulkLoadSymbol(logger);
        }

        #endregion Constructors

        #region Public Methods
        public List<Symbols> GetFromDBSymbolsFromListOfExchanges(List<string> exchanges)
        {
            //List<Symbols> symbols = new List<Symbols>();

            var symbols = symbolORMService.GetFromDBSymbolsFromTheseExchanges(null);

            return symbols;
        }
        
        public void LoadAllSymbolsFromWeb()
        {
            LoadAllSymbolsFromAllExchanges();
        }

        public void LoadAllSymbolsFromUSExchangesNoSave()
        {
            logger.InfoFormat("LoadAllSymbolsFromAllExchanges - GetSymbols");
            List<string> exchanges = exchangeORMService.GetUSExchanges();
            symbolStringList = LoadAllSymbolsFromAllExchanges(exchanges);
        }

        public void LoadAllSymbolsFromAllExchanges()
        {
            logger.InfoFormat("LoadAllSymbolsFromAllExchanges - GetSymbols");
            List<string> exchanges = exchangeORMService.GetExchanges();
            symbols = LoadAllSymbolsFromAllExchanges(exchanges, true);
        }

        public List<string> LoadAllSymbolsFromAllExchanges(List<string> exchanges)
        {
            logger.InfoFormat("Start - LoadAllSymbolsFromAllExchanges");

            List<string> allSymbols = new List<string>();
            string exchangeSave = "";
            string itemSave = "";
            try
            {
                foreach (string exchange in exchanges)
                {
                    exchangeSave = exchange;

                    foreach (string letter in alphabet)
                    {
                        string sPage = WebPage.Get(String.Format(exchange, letter));

                        string[] splitPages = GetRowsOfSymbolData(sPage);

                        string pages = string.Empty;

                        List<string> symbolLookups = GetPagesLookup(splitPages);

                        allSymbols.AddRange(symbolLookups);
                    }

                    //string sub1 = string.Empty;
                    //string sub3 = string.Empty;

                    ////nasdaq = "http://eoddata.com/stocklist/NASDAQ/{0}.htm";
                    //List<Symbols> symbolList = new List<Symbols>();

                    //string xchange = exchange.Replace("http://eoddata.com/stocklist/", "").Replace("/{0}.htm", "");

                    //foreach (string item in symbolLookups)
                    //{
                    //    itemSave = item;
                    //    sPage = WebPage.Get(string.Format(exchange, item));
                    //    sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));

                    //    string sub2 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                    //    string symbolz = sub2.Substring(0, sub2.IndexOf("</table>") + "</table>".Length);
                        
                    //    symbolList.AddRange(GetSymbolLookup(symbolz, xchange));
                    //}

                    //allSymbols.AddRange(symbolList);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("LoadAllSymbolsFromAllExchanges: {0}", ex);
            }
            finally
            {
                //if (save)
                //{
                //    success = BulkLoadSymbols(allSymbols);
                //}

                logger.Info("End - LoadAllSymbolsFromAllExchanges");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
            return allSymbols;
        }

        public List<Symbols> LoadAllSymbolsFromAllExchanges(List<string> exchanges, bool save = true)
        {
            logger.InfoFormat("Start - LoadAllSymbolsFromAllExchanges");

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

                    List<string> symbolLookups = GetPagesLookup_save(pages);

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

                    allSymbols.AddRange(symbolList);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("LoadAllSymbolsFromAllExchanges: {0}", ex);
            }
            finally
            {
                if (save)
                {
                    success = BulkLoadSymbols(allSymbols);
                }

                logger.Info("End - LoadAllSymbolsFromAllExchanges");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
            return allSymbols;
        }


        public List<Symbols> LoadAllSymbolsFromAllExchanges(List<Exchanges> exchanges)
        {
            logger.InfoFormat("Start - LoadAllSymbolsFromAllExchanges");

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

                    List<string> symbolLookups = GetPagesLookup_save(pages);

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
                logger.Fatal("LoadAllSymbolsFromAllExchanges: {0}", ex);
            }
            finally
            {
                success = BulkLoadSymbols(allSymbols);

                logger.Info("End - LoadAllSymbolsFromAllExchanges");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
            return allSymbols;
        }

        public List<Symbols> GetSymbols()
        {
            return symbols;
        }

        public List<string> GetSymbolStringList()
        {
            return symbolStringList;
        }

        #endregion Public Methods

        #region Private Methods
        private bool BulkLoadSymbols(List<Symbols> allSymbols)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadSymbol.ConfigureDataTable();

                dt = bulkLoadSymbol.LoadDataTableWithSymbols(allSymbols, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on LoadDataTableWithSymbols", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadSymbol.BulkCopy<Symbols>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadSymbols returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Symbols Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        private string[] GetRowsOfSymbolData(string sPage)
        {
            sPage = sPage.Substring(sPage.IndexOf("cph1_bsa1_divSymbols"));
            int startIndex = sPage.IndexOf("</tr") + "</tr>".Length;
            sPage = sPage.Substring(startIndex, sPage.Length - startIndex);

            sPage = sPage.Trim().Replace("\r", string.Empty);
            sPage = sPage.Trim().Replace("\n", string.Empty);
            sPage = sPage.Replace(Environment.NewLine, string.Empty);

            string pages = sPage.Substring(0, sPage.IndexOf("</table>"));

            //pages = pages.Substring(pages.IndexOf("<tr"), pages.Length - 2);

            string tempPages = pages;
            tempPages = tempPages.Replace("</tr>", "~");
            return tempPages.Split('~');
        }

        private List<string> GetPagesLookup(string[] pages)
        {
            List<string> myPages = new List<string>();
            string work = "";

            try
            {
                for(int i = 0; i < pages.Length; i++)
                {
                    string page = pages[i];

                    if (string.IsNullOrEmpty(page)) continue;
                    // extract symbol
                    int startNdx = page.IndexOf("<td>");
                    work = page.Substring(startNdx, page.Length - startNdx);
                    startNdx = work.IndexOf("hideInfo();\">") + "hideInfo();\">".Length;
                    int endNdx = work.IndexOf("</td>");
                    string symbol = work.Substring(startNdx, endNdx - startNdx);
                    symbol = symbol.Substring(0, symbol.IndexOf("<"));

                    //Code, Name, High, Low, Close, Volume, Change
                    // extract closing price
                    // skip next td block - Name
                    work = work.Substring(4, work.Length - 4);
                    startNdx = work.IndexOf("<td>");
                    endNdx = work.Length - startNdx;
                    work = work.Substring(work.IndexOf("<td>") + "<td>".Length);

                    startNdx = work.IndexOf("<td") + "<td".Length;
                    endNdx = work.Length - startNdx;
                    work = work.Substring(startNdx, endNdx);

                    // skip next 2 td blocks - High, Low
                    for (int indx = 0; indx < 2; indx++)
                    {
                        startNdx = work.IndexOf("<td") + "<td".Length;
                        endNdx = work.Length - startNdx;
                        work = work.Substring(startNdx, endNdx);
                    }

                    // extract Close
                    startNdx = work.IndexOf(">") + ">".Length;
                    endNdx = work.IndexOf("<") - startNdx;
                    string closingPrice = work.Substring(startNdx, endNdx);

                    // clean up work string
                    startNdx = work.IndexOf("<td") + "<td".Length;
                    endNdx = work.Length - startNdx;
                    work = work.Substring(work.IndexOf("<td"));

                    // check if > 9.99
                    decimal close = 0;
                    bool tryParseResult = decimal.TryParse(closingPrice, out close);

                    if (tryParseResult)
                    {
                        if (close < 10)
                            continue;
                    }
                    else
                    {
                        // something went wrong
                        continue;
                    }

                    // extract volumn
                    startNdx = work.IndexOf(">") + ">".Length;
                    endNdx = work.IndexOf("</") - startNdx;
                    string closingVolumn = work.Substring(startNdx, endNdx);

                    // check if > 9.99
                    decimal result = 0;
                    tryParseResult = decimal.TryParse(closingVolumn, out result);
                    decimal lowclose = 75;

                    if (tryParseResult)
                    {
                        if (result < 1000000 && close < lowclose)
                            continue;
                    }
                    else
                    {
                        // something went wrong
                        continue;
                    }

                    myPages.Add(symbol);

                    if (myPages.Contains("AZUL"))
                    {
                        string wtf = "AXUL";
                    }

                }
            }
            catch(Exception ex)
            {
                int i = 22;
                i++;
            }
            //work = pages.Substring(pages.IndexOf("<tr>") + "<tr>".Length);
            //work = work.Substring(0, work.IndexOf("</tr>"));

            //work = work.Replace("<td ", "|");
            //work = work.Substring(1);
            //myPages = work.Split('|').ToList<string>();

            //for (int i = 0; i < myPages.Count; i++)
            //{
            //    myPages[i] = myPages[i].Replace("class=\"ls\">", "");
            //    myPages[i] = myPages[i].Replace("class=\"ld\"><a href=\"", "");
            //    myPages[i] = myPages[i].Replace("</td>", "").Replace("</a>", "");
            //    if (i > 0)
            //        myPages[i] = myPages[i].Substring(myPages[i].Length - 1);
            //}

            return myPages;
        }

    
        private List<string> GetPagesLookup_save(string pages)
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
                dq.Selected = false;

                tempquote.Add(dq);
            }

            return tempquote;
        }
        #endregion Private Methods

    }
}
