using Core.JsonModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IDailyQuotesORMService
    {
        void AddMany(List<DailyQuotes> quotes);
        List<DailyQuotes> ExtractDailyQuotes(string symbol, JsonModels.HistoryDetail.JsonResult symbolHistory);
        Core.JsonModels.HistoryDetail.Dividends GetDividends(string symbol, Core.JsonModels.HistoryDetail.JsonResult symbolHistory);
    }
}
