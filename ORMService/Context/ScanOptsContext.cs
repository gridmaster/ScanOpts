using Core.JsonModels;
using System.Data.Entity;

namespace ORMService.Context
{
    public class ScanOptsContext : DbContext
    {
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<CallPut> CallPut { get; set; }
        public DbSet<DailyQuotes> DailyQuotes { get; set; }
    }
}
