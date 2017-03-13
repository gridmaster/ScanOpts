using Core.JsonModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IDailyQuotesORMService
    {
        void AddMany(List<DailyQuotes> quotes);
        List<DailyQuotes> ExtractDailyQuotes(string symbol, JsonModels.ORMModels.JsonResult symbolHistory);
        Core.JsonModels.ORMModels.Dividends GetDividends(string symbol, Core.JsonModels.ORMModels.JsonResult symbolHistory);
    }
}
