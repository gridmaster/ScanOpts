using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.JsonModels
{
    public class Events
    {
        public Dividends dividends { get; set; }
        public Splits splits { get; set; }
    }

    public class Dividends
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> dividend { get; set; }
    }

    public class AmountDate
    {
        public decimal amount { get; set; }
        public int date { get; set; }
    }

    public class Splits
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
