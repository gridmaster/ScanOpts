using Core.JsonModels;

namespace Core.Interface
{
    public interface IStatisticORMService
    {
        Statistics ExtractAndSaveStatisticFromOptionChain(JsonResult optionsChain);
        int Add(Statistics entity);
    }
}
