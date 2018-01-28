using System.Collections.Generic;
using Core.ORMModels;

namespace Core.Interface
{
    public interface IBollingerBandService
    {
        void RunDaily(bool value);

        void RunBollingerBandsCheck();

        void RunBollingerBandsCheck(List<Symbols> symbols);

        void RunBollingerBandsCheck(List<string> symbols);

        List<BollingerBands> CalculateBollingerBands(List<DailyQuotes> quotesList);
    }
}
