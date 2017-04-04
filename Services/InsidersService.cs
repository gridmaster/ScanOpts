using Core;
using Core.BulkLoad;
using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using ORMService;
using Core.JsonQuoteSummary;
using Newtonsoft.Json;
using Core.Business;

namespace Services
{
    public class InsidersService : BaseService
    {
        #region Private properties
        private bool success;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        //private BulkLoadInsiders bulkLoadInsiders = null;
        #endregion Private properties

        #region Constructors

        public InsidersService(ILogger logger, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService) // , BulkLoadInsiders bulkLoadInsiders)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            //this.bulkLoadInsiders = new BulkLoadInsiders(logger);
        }

        #endregion Constructors

        #region Public Methods

        public void RunInsidersCollection()
        {
            logger.InfoFormat("RunOptionsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunInsidersCollection(symbols);
        }

        public void RunInsidersCollection(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            RunInsidersCollection(syms);
        }
        
        public void RunInsidersCollection(List<string> symbols)
        {
            logger.InfoFormat("Start - RunInsidersCollection");

            string url = "https://query2.finance.yahoo.com/v10/finance/quoteSummary/{0}?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=institutionOwnership%2CfundOwnership%2CmajorDirectHolders%2CmajorHoldersBreakdown%2CinsiderTransactions%2CinsiderHolders%2CnetSharePurchaseActivity&corsDomain=finance.yahoo.com";

            try
            {
                foreach (string sym in symbols)
                {
                    List<CallPuts> allCallPuts = new List<CallPuts>();
                    logger.InfoFormat("Get {0} page", sym);

                    string sPage = WebPage.Get(string.Format(url, sym));
                    RootObject quoteSummary = JsonConvert.DeserializeObject<RootObject>(sPage);

                    int newId = 0;

                    //BulkLoadInsiders(allCallPuts);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("RunInsidersCollection: {0}", ex);
            }
            finally
            {
                logger.Info("End - RunInsidersCollection");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }

        public void LoadInsidersInfo()
        {
            string url = "https://query2.finance.yahoo.com/v10/finance/quoteSummary/{0}?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=institutionOwnership%2CfundOwnership%2CmajorDirectHolders%2CmajorHoldersBreakdown%2CinsiderTransactions%2CinsiderHolders%2CnetSharePurchaseActivity&corsDomain=finance.yahoo.com";
            string sPage = WebPage.Get(url);
            RootObject quoteSummary = JsonConvert.DeserializeObject<RootObject>(sPage);
        }

        #endregion Public Methods

        #region Private Methods
        private bool BulkLoadInsiders(List<Symbols> allSymbols)
        {
            bool success = false;
            try
            {
                //var dt = bulkLoadSymbol.ConfigureDataTable();

                //dt = bulkLoadSymbol.LoadDataTableWithSymbols(allSymbols, dt);

                //if (dt == null)
                //{
                //    logger.InfoFormat("{0}No data returned on LoadDataTableWithSymbols", Environment.NewLine);
                //}
                //else
                //{
                //    success = bulkLoadSymbol.BulkCopy<Symbols>(dt, "ScanOptsContext");
                //    logger.InfoFormat("{0}BulkLoadSymbols returned with: {1}", Environment.NewLine,
                //                            success ? "Success" : "Fail");
                //}
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

            for (int i = 0; i < myPages.Count; i++)
            {
                myPages[i] = myPages[i].Replace("class=\"ls\">", "");
                myPages[i] = myPages[i].Replace("class=\"ld\"><a href=\"", "");
                myPages[i] = myPages[i].Replace("</td>", "").Replace("</a>", "");
                if (i > 0)
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
                symbol = symbol.Substring(symbol.IndexOf(">") + 1);
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
