using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Business;
using Core.Interface;
using Core.ORMModels;
using Core.BulkLoad;
using ORMService;

namespace StandardDeviation
{
    public class StandartDeviationCount : BaseService, IStandardDeviation
    {

        #region Private properties
        private bool success;
        private List<string> symbolList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private DailyQuotesORMService dailyQuotesORMService = null;
        private BulkLoadBollingerBands bulkLoadBollingerBands = null;
        private bool _runDaily = true;
        #endregion Private properties

        #region Public properties

        #endregion Public properties

        #region Constructors

        public StandartDeviationCount(ILogger logger, DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService, BulkLoadBollingerBands bulkLoadBollingerBands)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.dailyQuotesORMService = dailyQuotesORMService;
            this.bulkLoadBollingerBands = bulkLoadBollingerBands;
        }

        #endregion Constructors

        #region Public Methods

        public void RunStandardDeviationCount()
        {
            logger.InfoFormat("RunKeyStatisticsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunStandardDeviationCount(symbols);
        }

        public void RunStandardDeviationCount(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            RunStandardDeviationCount(syms);
        }

        public void RunStandardDeviationCount(List<string> symbols)
        {
        }
        #endregion Public Methods
    }
}
