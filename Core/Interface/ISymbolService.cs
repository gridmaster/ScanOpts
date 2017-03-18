using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface ISymbolService
    {
        void LoadSymbols();
        void LoadAllSymbolsFromAllExchanges();
        List<Symbols> LoadAllSymbolsFromAllExchanges(List<string> exchanges);
    }
}
