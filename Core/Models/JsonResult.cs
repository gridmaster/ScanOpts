using Newtonsoft.Json;

namespace ScanOpts.Core.Models
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "optionChain")]
        public OptionChain OptionChain { get; set; }
    }
}
