using Newtonsoft.Json;

namespace Core.JsonModels
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "optionChain")]
        public OptionChain OptionChain { get; set; }
    }
}
