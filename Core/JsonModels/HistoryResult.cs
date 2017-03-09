using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.JsonModels
{
    public class HistoryResult
    {
        [JsonProperty(PropertyName = "meta")]
        public Meta meta { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public string timestamp { get; set; }

        [JsonProperty(PropertyName = "events")]
        public Events events { get; set; }

        [JsonProperty(PropertyName = "indicators")]
        public Indicators indicators { get; set; }
    }
}
