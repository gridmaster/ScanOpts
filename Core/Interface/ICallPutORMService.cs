using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface ICallPutORMService
    {
        //void ExtractAndSaveBaseCallPuts(string symbol, List<Straddles> straddles);

        void AddMany(List<CallPut> callputs);
    }
}
