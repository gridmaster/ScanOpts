using Core.BusinessModels;
using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IAnalyticsService
    {
        List<SlopeAndBBCounts> FindRising50SMATrends(List<Symbols> symbols);
        List<SlopeAndBBCounts> FindRising50SMATrends(List<string> symbols);
    }
}
