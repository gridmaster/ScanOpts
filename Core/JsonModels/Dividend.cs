namespace Core.JsonModels
{
    public class Dividend : EventsBase
    {
        public int dividendDate { get; set; }
        public decimal dividendAmount { get; set; }
    }
}
