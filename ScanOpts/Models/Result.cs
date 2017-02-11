using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScanOpts.Models
{
    public class Result
    {
        [JsonProperty(PropertyName = "underlyingSymbol")]
        public string UnderlyingSymbol { get; set; }

        [JsonProperty(PropertyName = "expirationDates")]
        public List<decimal> ExpirationDates { get; set; }

        [JsonProperty(PropertyName = "strikes")]
        public List<decimal> Strikes { get; set; }

        [JsonProperty(PropertyName = "hasMiniOptions")]
        public bool HasMiniOptions { get; set; }

        [JsonProperty(PropertyName = "quote")]
        public Quote Quote { get; set; }

        [JsonProperty(PropertyName = "options")]
        public Options Options { get; set; }
    }
}
