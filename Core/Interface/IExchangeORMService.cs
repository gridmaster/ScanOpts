using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IExchangeORMService
    {
        List<string> GetExchanges();
        List<Exchanges> GetSomeExchanges();
    }
}
