using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IHistoryService
    {
        List<string> GetSymbols();

        void RunHistoryCollection();

        void RunHistoryCollection(List<string> symbols);

        void RunHistoryCollection(List<Symbols> symbols);

        string GetFullExchangeName();

        string GetFullExchangeName(List<string> symbol);
    }
}
