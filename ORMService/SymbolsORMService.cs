using Core.Interface;
using Core.ORMModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.JsonOptions;

namespace ORMService
{
    public class SymbolsORMService : ISymbolORMService
    {
        public List<string> GetSymbols()
        {
            List<string> symbols = new List<string> {
                "AAPL",
                "AMD",
                "AMZN",
                "BA",
                "BABA",
                "BAC",
                "BMY",
                "CAT",
                "CRM",
                "CSCO",
                "DB",
                "DIS",
                "EEM",
                "EFA",
                "F",
                "FB",
                "FCX",
                "GIS",
                "GM",
                "GOOGL",
                "GS",
                "INTC",
                "IWM",
                "JNJ",
                "K",
                "KO",
                "MAR",
                "MAT",
                "MGM",
                "MZZ",
                "NVDA",
                "PEP",
                "PG",
                "PIR",
                "QID",
                "QQQ",
                "S",
                "SBUX",
                "SDS",
                "SMN",
                "SPY",
                "SSO",
                "SVXY",
                "SYF",
                "T",
                "TSLA",
                "TWM",
                "TZA",
                "UAA",
                "UNP",
                "UVXY",
                "V",
                "VSAT",
                "VXX",
                "VZ",
                "WMT",
                "X",
                "XLF",
                "XLV",
                "XOP"
            };
            //"SDD",
            //"XIV",
            return symbols;
        }

        public List<Symbols> GetFromDBSymbolsFromTheseExchanges(List<string> exchanges)
        {
            List<Symbols> symbols = new List<Symbols>();

            #region not working
            //using (var db = new ScanOptsContext())
            //{
            //    var ss = (db.Symbols.SqlQuery(
            //        "Select * FROM dbo.Symbols where Symbol = '@p0'",
            //        "VXX").FirstOrDefaultAsync()); //, "NASDAQ", "AMEX");


            //    var wtf = db.Symbols;

            //    var morewtf = wtf;

            //    //symbols = context.Database.SqlQuery<string>(
            //    //       "SELECT Name FROM dbo.Blogs").ToList()


            //    //db.Symbols.SqlQuery()

            //    var one = db.Symbols.SqlQuery("Select * FROM dbo.Symbols where Symbol = 'p0'", "VXX");
            //    var symbs = from s in db.Symbols
            //                where s.Exchange == "NYSE"
            //                  || s.Exchange == "NASDAQ"
            //                  || s.Exchange == "AMEX"
            //                select s;
            //}
            #endregion not working

            string constr = ConfigurationManager.ConnectionStrings["ScanOptsContext"].ToString();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Symbols WHERE [Selected] = 1", conn))
                {
                    conn.Open();

                    DateTime dt = DateTime.Now;
                     
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Symbols s = new Symbols
                            {
                                Id = System.Convert.ToInt32(reader["Id"]),
                                Symbol = reader["Symbol"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                Exchange = reader["Exchange"].ToString(),
                                FullExchangeName = reader["FullExchangeName"].ToString(),
                                Date = System.Convert.ToDateTime(reader["Date"].ToString()),
                                Selected = reader["Selected"].ToString() == "True" ? true : false
                            };

                            symbols.Add(s);
                        }
                    }
                }
            }

            return symbols;
        }

        public Statistics ExtractAndSaveStatisticFromOptionChain(JsonResult optionChain)
        {
            throw new NotImplementedException();
        }

        public void Add(Symbols entity)
        {
            using (var db = new ScanOptsContext())
            {
                db.Symbols.Add(entity);
                db.SaveChanges();
            }
        }

        public void AddMany(List<Symbols> symbols)
        {
            using (var db = new ScanOptsContext())
            {
                try
                {
                    foreach (Symbols symbol in symbols)
                    {
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("SymbolsORMService - AddMany {0}", symbol.Symbol);
                        db.Symbols.Add(symbol);
                    }
                    IOCContainer.Instance.Get<ILogger>().Info("SymbolsORMService - AddMany Saving Changes");

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    IOCContainer.Instance.Get<ILogger>().ErrorFormat("SymbolsORMService - AddMany error: {1}{2}", ex.Message, Environment.NewLine);
                }
            }
        }
    }
}
