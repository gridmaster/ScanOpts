using Newtonsoft.Json;

namespace ScanOpts.Models
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "optionChain")]
        public OptionChain OptionChain { get; set; }
    }
}
