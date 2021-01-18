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
        void LoadAllSymbolsFromUSExchangesNoSave();
        List<Symbols> LoadAllSymbolsFromAllExchanges(List<string> exchanges, bool save = true);
        List<string> LoadAllSymbolsFromAllExchanges(List<string> exchanges);
        List<Symbols> GetSymbols();
        List<string> GetSymbolStringList();
    }
}
