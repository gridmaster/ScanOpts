using Core.ORMModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ISymbolService
    {
        List<Symbols> GetFromDBSymbolsFromListOfExchanges(List<string> exchanges);
        //Task<List<Symbols>> GetFromDBSymbolsFromTheseExchanges(List<string> exchanges);
        void LoadAllSymbolsFromWeb();
        void LoadAllSymbolsFromAllExchanges();
        List<Symbols> LoadAllSymbolsFromAllExchanges(List<string> exchanges);
    }
}
