using System;

namespace Core.JsonQuote
{
    public class EventsBase
    {
        public DateTime date { get; set; }
        public string symbol { get; set; }
        public string exchange { get; set; }
    }
}
