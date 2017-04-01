namespace Core.JsonQuoteSummary
{
    public class Result
    {
        public InsiderHolders insiderHolders { get; set; }
        public MajorHoldersBreakdown majorHoldersBreakdown { get; set; }
        public InstitutionOwnership institutionOwnership { get; set; }
        public MajorDirectHolders majorDirectHolders { get; set; }
        public NetSharePurchaseActivity netSharePurchaseActivity { get; set; }
        public InsiderTransactions insiderTransactions { get; set; }
        public FundOwnership fundOwnership { get; set; }
    }

}
