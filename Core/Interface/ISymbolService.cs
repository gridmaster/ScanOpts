using System.Collections.Generic;

namespace Core.Interface
{
    public interface ISymbolService
    {
        void LoadSymbols();
        void LoadAllSymbolsFromAllExchanges();
        void LoadAllSymbolsFromAllExchanges(List<string> exchanges);
    }
}
