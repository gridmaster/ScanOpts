namespace Core.JsonModels
{
    public class CurrentTradingPeriod
    {
        public Pre pre { get; set; }
        public Regular regular { get; set; }
        public Post post { get; set; }
    }

    public class Pre : BaseTradingPeriod
    {
    }

    public class Regular : BaseTradingPeriod
    {
    }

    public class Post : BaseTradingPeriod
    {
    }
}