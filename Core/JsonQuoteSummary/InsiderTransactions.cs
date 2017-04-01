using System.Collections.Generic;

namespace Core.JsonQuoteSummary
{
    public class InsiderTransactions
    {
        public List<Transaction> transactions { get; set; }
        public int maxAge { get; set; }
    }
}
