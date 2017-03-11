using System;

namespace Core.JsonModels
{
    public class EventsBase
    {
        public DateTime date { get; set; }
        public string symbol { get; set; }
        public string exchange { get; set; }
    }
}
