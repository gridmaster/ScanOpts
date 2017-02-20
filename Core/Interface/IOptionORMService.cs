using Core.JsonModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IOptionORMService
    {
        List<CallPut> ExtractCallsAndPutsFromOptionChain(string symbol, int newId, List<Straddles> straddles);
    }
}
