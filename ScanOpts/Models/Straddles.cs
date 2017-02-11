using System.Collections.Generic;

namespace ScanOpts.Models
{
    public class Straddles
    {
        //public List<Strike> strike { get; set; }
        //public List<Call> call { get; set; }
        //public List<Put> put { get; set; }
        public Strike strike { get; set; }
        public Call call { get; set; }
        public Put put { get; set; }
    }

    public class Strike : BaseRawFmt
    {
    }

    public class Call : BaseCallPut
    {
    }

    public class Put : BaseCallPut
    {
    }
}

