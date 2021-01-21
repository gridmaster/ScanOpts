using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ISMA60CycleService
    {
        List<DailyQuotes> GenerateSMA60s(List<string> symbols);
    }
}
