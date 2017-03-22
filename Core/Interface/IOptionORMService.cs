using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IOptionORMService
    {
        List<CallPuts> ExtractCallsAndPutsFromOptionChain(string symbol, int newId, List<Straddles> straddles);
    }
}
