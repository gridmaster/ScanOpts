using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.JsonQuote
{
    public class Events
    {
        [JsonProperty(PropertyName = "dividends")]
        public JsonDividends dividends { get; set; }

        [JsonProperty(PropertyName = "splits")]
        public JsonSplits splits { get; set; }
    }

    public class JsonDividends
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> dividend { get; set; }
    }

    public class AmountDate
    {
        public decimal amount { get; set; }
        public int date { get; set; }
    }

    public class JsonSplits
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> split { get; set; }
    }

    public class SplitDetails
    {
        public int date { get; set; }
        public int numerator { get; set; }
        public int denominator { get; set; }
        public string splitRatio { get; set; }
    }
}
