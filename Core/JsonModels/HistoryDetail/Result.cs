using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.JsonModels.HistoryDetail
{
    public class Result
    {
        public Meta meta { get; set; }
        public List<int> timestamp { get; set; }
        public Events events { get; set; }
        public Indicators indicators { get; set; }
    }
}
