using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScanOpts.Models
{
    public class Option
    {
        
        public long ExpirationDate { get; set; }
        public bool HasMiniOptions { get; set; }

        [JsonProperty(PropertyName = "straddles")]
        public List<Straddles> Straddles { get; set; }
    }
}
