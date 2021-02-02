using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IDailyQuotesORMService
    {
        void AddMany(List<DailyQuotes> quotes);
        List<string> GetSymbols();
        List<DailyQuotes> ExtractDailyQuotesx(string symbol, JsonQuote.JsonResult symbolHistory);
        Core.JsonQuote.Dividends GetDividends(string symbol, Core.JsonQuote.JsonResult symbolHistory);
        Core.JsonQuote.Splits GetSplits(string symbol, Core.JsonQuote.JsonResult symbolHistory);
        bool UpdateDailyQuotes(List<DailyQuotes> dailyQuotes);
        List<DailyQuotes> GetDailyQuotes(string symbol);
        bool UpdateExchange(string symbol, string exchange);
        string GetFullExchangeName(string symbol);
    }
}
