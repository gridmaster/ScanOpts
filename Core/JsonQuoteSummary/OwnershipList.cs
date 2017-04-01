namespace Core.JsonQuoteSummary
{
    public class OwnershipList
    {
        public int maxAge { get; set; }
        public Holders.ReportDate reportDate { get; set; }
        public string organization { get; set; }
        public Holders.PctHeld pctHeld { get; set; }
        public Holders.Position position { get; set; }
        public Holders.Value value { get; set; }
    }
}
