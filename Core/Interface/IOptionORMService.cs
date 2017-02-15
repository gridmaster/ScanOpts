using Core.Models;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IOptionORMService
    {
        void ExtractAndSaveOptionChainForExpireDate(List<Straddles> straddles);
    }
}
