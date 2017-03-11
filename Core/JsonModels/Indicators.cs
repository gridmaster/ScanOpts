using System.Collections.Generic;

namespace Core.JsonModels
{
    public class Indicators
    {
        public List<DailyQuote> quote {get;set;}

        public List<UnAdjClose> unadjclose { get; set; }

        public List<UnAdjQuote> unadjquote { get; set; }
    }

    public class UnAdjClose
    {
        public List<object> unadjclose { get; set; }
    }

    public class UnAdjQuote
    {
        public List<object> unadjhigh { get; set; }
        public List<object> unadjlow { get; set; }
        public List<object> unadjclose { get; set; }
        public List<object> unadjopen { get; set; }
    }
}
