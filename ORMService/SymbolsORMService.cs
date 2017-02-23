using Core.Interface;
using System.Collections.Generic;

namespace ORMService
{
    public class SymbolsORMService : ISymbolORMService
    {
        public List<string> GetSymbols()
        {
            List<string> symbols = new List<string> {
                "AAPL",
                "AMZN",
                "BABA",
                "BAC",
                "BMY",
                "CAT",
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
                "KLAC",
                "KO",
                "MAR",
                "MAT",
                "MGM",
                "MZZ",
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
                "X",
                "XLF",
                "XLV",
                "XOP"
            };
            //"SDD",
            //"XIV",
            return symbols;
        }
    }
}
