using Core;
using Core.Interface;
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

        public SMA60CyclesService(ILogger logger) : base(logger)
        {
        }

        #region Constructors

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

    }
}
