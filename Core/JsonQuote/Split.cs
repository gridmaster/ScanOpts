namespace Core.JsonModels
{
    public class Split : EventsBase
    {
        public int splitDate { get; set; }
        public int numerator { get; set; }
        public int denominator { get; set; }
        public string ratio { get; set; }
    }
}
