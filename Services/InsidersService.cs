﻿using Core;
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

        #endregion Private Methods
    }
}
