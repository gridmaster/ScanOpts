using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.JsonOptions
{
    public class OptionChain
    {
        [JsonProperty(PropertyName = "result")]
        public List<Result> Result { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
