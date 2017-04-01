using System.Collections.Generic;

namespace Core.JsonQuoteSummary
{
    public class QuoteSummary
    {
        public List<Result> result { get; set; }
        public object error { get; set; }
    }
}
