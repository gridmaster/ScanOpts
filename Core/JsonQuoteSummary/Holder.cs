namespace Core.JsonQuoteSummary
{
    public class Holder
    {
        public int MaxAge { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string Url { get; set; }
        public string TransactionDescription { get; set; }
        public Holders.LatestTransDate LatestTransDate { get; set; }
        public Holders.PositionDirect PositionDirect { get; set; }
        public Holders.PositionDirectDate PositionDirectDate { get; set; }
    }
}
