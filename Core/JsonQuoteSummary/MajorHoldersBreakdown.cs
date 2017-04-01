namespace Core.JsonQuoteSummary
{
    public class MajorHoldersBreakdown
    {
        public int maxAge { get; set; }
        public Holders.InsidersPercentHeld insidersPercentHeld { get; set; }
        public Holders.InstitutionsPercentHeld institutionsPercentHeld { get; set; }
        public Holders.InstitutionsFloatPercentHeld institutionsFloatPercentHeld { get; set; }
        public Holders.InstitutionsCount institutionsCount { get; set; }
    }
}
