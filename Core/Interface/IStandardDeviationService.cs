using System.Collections.Generic;
using Core.ORMModels;

namespace Core.Interface
{
    public interface IStandardDeviationService
    {
        void RunStandardDeviation();

        void RunStandardDeviation(List<Symbols> symbols);

        void RunStandardDeviation(List<string> symbols);

        void LoadStandardDeviations(List<Symbols> symbols, double standardDeviation);
    }
}
