using Core.Interface;
using Core.ORMModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;

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
