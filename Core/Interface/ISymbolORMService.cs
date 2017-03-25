using Core.ORMModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ISymbolORMService
    {
        List<string> GetSymbols();
        //Task<List<Symbols>> GetFromDBSymbolsFromTheseExchanges(List<string> exchanges);
        List<Symbols> GetFromDBSymbolsFromTheseExchanges(List<string> exchanges);
        void AddMany(List<Symbols> symbols);
        void Add(Symbols symbol);
    }
}
