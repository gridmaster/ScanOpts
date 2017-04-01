namespace Core.JsonQuoteSummary
{
    public class Transaction
    {
        public int maxAge { get; set; }
        public Holders.Shares shares { get; set; }
        public Holders.Value2 value { get; set; }
        public string filerUrl { get; set; }
        public string transactionText { get; set; }
        public string filerName { get; set; }
        public string filerRelation { get; set; }
        public string moneyText { get; set; }
        public Holders.StartDate startDate { get; set; }
        public string ownership { get; set; }
    }
}
