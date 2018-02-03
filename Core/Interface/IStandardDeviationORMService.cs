using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ORMModels;

namespace Core.Interface
{
    public interface IStandardDeviationORMService
    {
        List<StandardDeviations> RunStandardDeviation(List<string> symbols);
        List<StandardDeviations> LoadStandardDeviations(List<string> symbols, double standardDeviation);
    }
}
