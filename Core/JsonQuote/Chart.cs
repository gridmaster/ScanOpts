using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.JsonQuote
{
    public class Chart
    {
        [JsonProperty(PropertyName = "result")]
        public List<Result> Result { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
