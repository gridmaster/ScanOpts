using Core.BusinessModels;
using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IAnalyticsService
    {
        List<SlopeAnd60sCounts> FindRising60SMATrends(List<Symbols> symbols);
        List<SlopeAnd60sCounts> FindRising60SMATrends(List<string> symbols);
        List<DailyQuotes> FindRising60SMATrends(ref List<DailyQuotes> symbols);

        List<SlopeAndBBCounts> FindRising50SMATrends(List<Symbols> symbols);
        List<SlopeAndBBCounts> FindRising50SMATrends(List<string> symbols);
    }
}
