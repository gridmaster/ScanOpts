using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IExchangeORMService
    {
        List<string> GetExchanges();
        List<string> GetUSExchanges();
        List<Exchanges> GetSomeExchanges();
    }
}
