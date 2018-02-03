using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Core;
using Core.Business;
using Core.Interface;
using Core.ORMModels;
using Core.BulkLoad;
using ORMService;

namespace StandardDeviationService
{
    public class StandardDeviationServices : BaseService, IStandardDeviationService
   {

        #region Private properties
        private bool success;
        private List<string> symbolList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private StandardDeviationORMService standardDeviationORMService = new StandardDeviationORMService();
        private DailyQuotesORMService dailyQuotesORMService = null;
        private BulkLoadStandardDeviations bulkLoadStandardDeviations = null;
        private bool _runDaily = true;
        #endregion Private properties

        #region Public properties

        #endregion Public properties

        #region Constructors

        public StandardDeviationServices(ILogger logger, StandardDeviationORMService standardDeviationORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService, BulkLoadStandardDeviations bulkLoadStandardDeviations)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.standardDeviationORMService = standardDeviationORMService;
            this.bulkLoadStandardDeviations = bulkLoadStandardDeviations;
        }

        #endregion Constructors

        #region Public Methods

        public void RunStandardDeviation()
        {
            logger.InfoFormat("RunKeyStatisticsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunStandardDeviation(symbols);
        }

        public void RunStandardDeviation(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            RunStandardDeviation(syms);
        }

        public void RunStandardDeviation(List<string> symbols)
        {
            List<StandardDeviations> sdcList = new List<StandardDeviations>();

            logger.InfoFormat("Start - RunStandardDeviation");
            try
            {
                foreach (string symbol in symbols)
                {
                    logger.InfoFormat("Get {0} page", symbol);
                    List<StandardDeviations> sdc = standardDeviationORMService.RunStandardDeviation(symbols);

                    sdcList.AddRange(sdc);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("RunStandardDeviationCount: {0}", ex);
            }
            finally
            {
                logger.Info("End - RunStandardDeviationCount");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }

        public void LoadStandardDeviations(List<Symbols> symbols, double standardDeviation)
        {
            logger.Info("Start LoadStandardDeviations");

            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }

            try
            {
                List<StandardDeviations> sdcList = standardDeviationORMService.LoadStandardDeviations(syms, standardDeviation);
                logger.Info("Start BulkLoadStandardDeviations");

                BulkLoadStandardDeviations(sdcList);
                logger.Info("End BulkLoadStandardDeviations");
                //return sdcList;
            }
            catch (Exception ex)
            {
                logger.Fatal("LoadStandardDeviations: {0}", ex);
            }
            finally
            {
                logger.Info("End - LoadStandardDeviations");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }
        #endregion Public Methods

        #region Private Methods
        private bool BulkLoadStandardDeviations(List<StandardDeviations> standardDeviations)
        {
            bool success = false;
            try
            {
                bulkLoadStandardDeviations.TruncateTable("StandardDeviations");

                var dt = bulkLoadStandardDeviations.ConfigureDataTable();

                dt = bulkLoadStandardDeviations.LoadDataTableWithStandardDeviations(standardDeviations, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadStandardDeviations", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadStandardDeviations.BulkCopy<StandardDeviations>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadStandardDeviations returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Standard Deviations Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }
        #endregion Private Methods
   }
}
 