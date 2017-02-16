using Core.JsonModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IOptionORMService
    {
        void ExtractAndSaveOptionChainForExpireDate(string symbol, List<Straddles> straddles);
    }
}
