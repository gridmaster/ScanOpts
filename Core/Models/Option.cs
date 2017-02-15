using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Option
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public long ExpirationDate { get; set; }
        public bool HasMiniOptions { get; set; }

        [JsonProperty(PropertyName = "straddles")]
        public List<Straddles> Straddles { get; set; }
    }
}
