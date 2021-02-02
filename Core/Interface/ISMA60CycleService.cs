using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface ISMA60CycleService
    {
        List<DailyQuotes> GenerateSMA60s(List<string> symbols);
        List<DailyQuotes> GenerateSMA60sNoUpdate(List<string> symbols);
    }
}
