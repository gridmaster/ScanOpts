using Core;
using Core.Interface;
using Core.ORMModels;
using System.Collections.Generic;

namespace ORMService
{
    public class BollingerBandORMService : BaseService, IBollingerBandORMService
    {
        #region Constructors

        public BollingerBandORMService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            log = logger;
        }

        #endregion Constructors

        static ILogger log;

        private UnitOfWork unitOfWork = new UnitOfWork(log);

        public IEnumerable<BollingerBands> GetSymbolData(string symbol)
        {
            int i = 0;
            IEnumerable<BollingerBands> symbols = unitOfWork.BollingerBandsRepository.Get(s => s.Symbol == symbol);
            //BollingerBands wtf = unitOfWork.BollingerBandsRepository.GetByID(2422667);

            foreach (var sym in symbols)
            {
                i++;
            }

            return symbols;
        }

    }
}
