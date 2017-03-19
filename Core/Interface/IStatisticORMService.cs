using Core.JsonOptions;
using Core.ORMModels;

namespace Core.Interface
{
    public interface IStatisticORMService
    {
        Statistics ExtractAndSaveStatisticFromOptionChain(JsonResult optionsChain);
        int Add(Statistics entity);
    }
}
