using Core;
using Core.Interface;
using Core.ORMModels;
using ORMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMA60CycleService
{
    public class SMA60CyclesService : BaseService, ISMA60CycleService
    {
        #region Private properties
        private bool success;
        private List<string> symbolList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private DailyQuotesORMService dailyQuotesORMService = null;
        private bool _runDaily = true;
        #endregion Private properties

        #region Public properties

        #endregion Public properties


        #region Constructors
        //public SMA60CyclesService(ILogger logger) : base(logger)
        //{
        //}

        public SMA60CyclesService(ILogger logger, DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService) //, BulkLoadBollingerBands bulkLoadBollingerBands)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.dailyQuotesORMService = dailyQuotesORMService;
        }
        #endregion Constructors

        #region Public Methods
        
        public List<DailyQuotes> GenerateSMA60s()
        {
            List<DailyQuotes> result = new List<DailyQuotes>();

            List<string> symbols = dailyQuotesORMService.GetSymbols();

            result = GenerateSMA60s(symbols);

            return result;
        }

        public List<DailyQuotes> GenerateSMA60s(List<string> symbols)
        {
            List<DailyQuotes> result = new List<DailyQuotes>();

            foreach(string symbol in symbols)
            {
                List<DailyQuotes> quotes = (List<DailyQuotes>)dailyQuotesORMService.GetSymbolDailyData(symbol);


            }

            return result;
        }

        #endregion Public Methods
    }
}
