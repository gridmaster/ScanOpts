using System.Collections.Generic;

namespace Core.JsonModels
{
    public class DailyQuote
    {
        public List<object> close { get; set; }
        public List<object> high { get; set; }
        public List<object> low { get; set; }
        public List<object> open { get; set; }
        public List<object> volume { get; set; }
    }
}
