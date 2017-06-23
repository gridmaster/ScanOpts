﻿using Core;
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
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private BulkLoadSymbol bulkLoadSymbol = null;
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
            symbols = LoadAllSymbolsFromAllExchanges(exchanges, false);
        }

        public void LoadAllSymbolsFromAllExchanges()
        {
            logger.InfoFormat("LoadAllSymbolsFromAllExchanges - GetSymbols");
            List<string> exchanges = exchangeORMService.GetExchanges();
            symbols = LoadAllSymbolsFromAllExchanges(exchanges);
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
                dq.Selected = false;

                tempquote.Add(dq);
            }

            return tempquote;
        }
        #endregion Private Methods

    }
}
