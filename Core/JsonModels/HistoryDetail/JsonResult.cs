using Newtonsoft.Json;

namespace Core.JsonModels.HistoryDetail
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "chart")]
        public Chart Chart { get; set; }
    }
}
