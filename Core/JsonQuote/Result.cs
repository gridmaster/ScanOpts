using System.Collections.Generic;
using Core.JsonModels;

namespace Core.JsonQuote
{
    public class Result
    {
        public Meta meta { get; set; }
        public List<int> timestamp { get; set; }
        public Core.JsonQuote.Events events { get; set; }
        public Indicators indicators { get; set; }
    }
}
