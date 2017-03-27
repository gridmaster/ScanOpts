using System.Data.Entity;
using Core.ORMModels;
using Core.JsonQuote;

namespace ORMService.Context
{
    public class ScanOptsContext : DbContext
    {
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<CallPuts> CallPut { get; set; }
        public DbSet<DailyQuotes> DailyQuotes { get; set; }
        public DbSet<Symbols> Symbols { get; set; }
        public DbSet<Dividend> Dividends { get; set; }
        public DbSet<Split> Splits { get; set; }
    }
}
