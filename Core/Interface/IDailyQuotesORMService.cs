﻿using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IDailyQuotesORMService
    {
        void AddMany(List<DailyQuotes> quotes);
        List<string> GetSymbols();
        List<DailyQuotes> ExtractDailyQuotes(string symbol, JsonQuote.JsonResult symbolHistory);
        Core.JsonQuote.Dividends GetDividends(string symbol, Core.JsonQuote.JsonResult symbolHistory);
        Core.JsonQuote.Splits GetSplits(string symbol, Core.JsonQuote.JsonResult symbolHistory);
    }
}
