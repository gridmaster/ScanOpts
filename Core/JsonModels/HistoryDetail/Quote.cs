using System.Collections.Generic;

namespace Core.JsonModels.HistoryDetail
{
    public class Quote
    {
        List<decimal> low { get; set; }
        List<decimal> high { get; set; }
        List<decimal> open { get; set; }
        List<decimal> close { get; set; }
        List<int> volume { get; set; }
    }
}
