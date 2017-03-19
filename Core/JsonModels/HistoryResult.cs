using Newtonsoft.Json;
using Core.JsonQuote;

namespace Core.JsonModels
{
    public class HistoryResult
    {
        [JsonProperty(PropertyName = "meta")]
        public Meta meta { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public string timestamp { get; set; }

        [JsonProperty(PropertyName = "events")]
        public Core.JsonQuote.Events events { get; set; }

        [JsonProperty(PropertyName = "indicators")]
        public Indicators indicators { get; set; }
    }
}
