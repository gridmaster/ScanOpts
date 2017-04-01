namespace Core.JsonQuoteSummary
{
    public class NetSharePurchaseActivity
    {
        public int maxAge { get; set; }
        public string period { get; set; }
        public Holders.BuyInfoCount buyInfoCount { get; set; }
        public Holders.BuyInfoShares buyInfoShares { get; set; }
        public Holders.SellInfoCount sellInfoCount { get; set; }
        public Holders.SellInfoShares sellInfoShares { get; set; }
        public Holders.SellPercentInsiderShares sellPercentInsiderShares { get; set; }
        public Holders.NetInfoCount netInfoCount { get; set; }
        public Holders.NetInfoShares netInfoShares { get; set; }
        public Holders.NetPercentInsiderShares netPercentInsiderShares { get; set; }
        public Holders.NetInstSharesBuying netInstSharesBuying { get; set; }
        public Holders.NetInstBuyingPercent netInstBuyingPercent { get; set; }
        public Holders.TotalInsiderShares totalInsiderShares { get; set; }
    }
}
