using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface ISymbolORMService
    {
        List<string> GetSymbols();
        void AddMany(List<Symbols> symbols);
        void Add(Symbols symbol);
    }
}
