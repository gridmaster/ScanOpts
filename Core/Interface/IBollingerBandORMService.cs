using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IBollingerBandORMService
    {
        IEnumerable<BollingerBands> GetSymbolData(string symbol);
    }
}
