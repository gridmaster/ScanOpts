namespace Core.JsonModels
{
    public class Straddles
    {
        public Strike strike { get; set; }
        public Call call { get; set; }
        public Put put { get; set; }
    }

    public class Strike : BaseRawFmt
    {
        public int ID { get; set; }
        public int QuoteID { get; set; }
    }

    public class Call : BaseCallPut
    {
    }

    public class Put : BaseCallPut
    {
    }
}

