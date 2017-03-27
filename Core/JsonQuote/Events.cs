using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.JsonQuote
{
    public class Events
    {
        [JsonProperty(PropertyName = "dividends")]
        public JsonDividends Dividends { get; set; }

        [JsonProperty(PropertyName = "splits")]
        public JsonSplits Splits { get; set; }
    }

    public class JsonDividends
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> Dividend { get; set; }
    }

    public class AmountDate
    {
        public decimal Amount { get; set; }
        public int Date { get; set; }
    }

    public class JsonSplits
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> Split { get; set; }
    }

    public class SplitDetails
    {
        public int Date { get; set; }
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public string SplitRatio { get; set; }
    }
}
