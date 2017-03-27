using System;

namespace Core.JsonQuote
{
    public class EventsBase
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
    }
}
