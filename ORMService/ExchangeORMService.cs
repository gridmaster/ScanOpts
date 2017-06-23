using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;

namespace ORMService
{
    public class ExchangeORMService : IExchangeORMService
    {
        public List<string> GetExchanges()
        {
            List<string> exchanges = new List<string> {
                //New York Stock Exchange[NYSE]
                "http://eoddata.com/stocklist/NYSE/{0}.htm",
                 //NASDAQ Stock Exchange[NASDAQ]
                "http://eoddata.com/stocklist/NASDAQ/{0}.htm",
                // American Stock Exchange [AMEX]
                "http://eoddata.com/stocklist/AMEX/{0}.htm",
                //Euronext Amsterdam[AMS]
                "http://eoddata.com/stocklist/AMS/{0}.htm",
                //Australian Securities Exchange[ASX]
                "http://eoddata.com/stocklist/ASX/{0}.htm",
                //Euronext Brussels[BRU]
                "http://eoddata.com/stocklist/BRU/{0}.htm",
                //Chicago Board of Trade[CBOT]
                "http://eoddata.com/stocklist/CBOT/{0}.htm",
                //Chicago Futures Exchange[CFE]
                "http://eoddata.com/stocklist/CFE/Q{0}.htm",
                //Chicago Merchantile Exchange[CME]
                "http://eoddata.com/stocklist/CME/{0}.htm",
                //New York Commodity Exchange[COMEX]
                "http://eoddata.com/stocklist/COMEX/{0}.htm",
                //EUREX Futures Exchange[EUREX]
                "http://eoddata.com/stocklist/EUREX/{0}.htm",
                //Foreign Exchange[FOREX]
                "http://eoddata.com/stocklist/FOREX/{0}.htm",
                //Hong Kong Stock Exchange[HKEX]
                "http://eoddata.com/stocklist/HKEX/{0}.htm",
                //Global Indices[INDEX]
                "http://eoddata.com/stocklist/INDEX/{0}.htm",
                //Kansas City Board of Trade[KCBT]
                "http://eoddata.com/stocklist/KCBT/{0}.htm",
                //LIFFE Futures and Options[LIFFE]
                "http://eoddata.com/stocklist/LIFFE/{0}.htm",
                //Euronext Lisbon[LIS]
                "http://eoddata.com/stocklist/LIS/{0}.htm",
                //London Stock Exchange[LSE]
                "http://eoddata.com/stocklist/LSE/{0}.htm",
                //Minneapolis Grain Exchange[MGEX]
                "http://eoddata.com/stocklist/MGEX/{0}.htm",
                //Milan Stock Exchange[MLSE]
                "http://eoddata.com/stocklist/MLSE/{0}.htm",
                //Madrid Stock Exchange[MSE]
                "http://eoddata.com/stocklist/MSE/A.htm",
                //New York Board of Trade[NYBOT]
                "http://eoddata.com/stocklist/NYBOT/{0}.htm",
                //New York Merchantile Exchange[NYMEX]
                "http://eoddata.com/stocklist/NYMEX/B.htm",
                //New Zealand Exchange[NZX]
                "http://eoddata.com/stocklist/NZX/A.htm",
                //OTC Bulletin Board[OTCBB]
                "http://eoddata.com/stocklist/OTCBB/{0}.htm",
                //Euronext Paris[PAR]
                "http://eoddata.com/stocklist/PAR/{0}.htm",
                //Singapore Stock Exchange[SGX]
                "http://eoddata.com/stocklist/SGX/{0}.htm",
                //Toronto Stock Exchange[TSX]
                "http://eoddata.com/stocklist/TSX/{0}.htm",
                //Toronto Venture Exchange[TSXV]
                "http://eoddata.com/stocklist/TSXV/{0}.htm",
                //Mutual Funds[USMF]
                "http://eoddata.com/stocklist/USMF/{0}.htm",
                //Winnipeg Commodity Exchange[WCE]
                "http://eoddata.com/stocklist/WCE/{0}.htm"
            };
            return exchanges;
        }

        public List<string> GetUSExchanges()
        {
            List<string> exchanges = new List<string> {
                //New York Stock Exchange[NYSE]
                "http://eoddata.com/stocklist/NYSE/{0}.htm",
                 //NASDAQ Stock Exchange[NASDAQ]
                "http://eoddata.com/stocklist/NASDAQ/{0}.htm",
                // American Stock Exchange [AMEX]
                "http://eoddata.com/stocklist/AMEX/{0}.htm"
            };
            return exchanges;
        }


        public List<Exchanges> GetSomeExchanges()
        {
            DateTime date = DateTime.Now;
            List<Exchanges> exchanges = new List<Exchanges>() {
                new Exchanges() { FullExchangeName = "New York Stock Exchange", Exchange = "NYSE", ExchangeURL = "http://eoddata.com/stocklist/NYSE/{0}.htm", Date = date },
                new Exchanges() { FullExchangeName = "NASDAQ Stock Exchange", Exchange = "NASDAQ", ExchangeURL = "http://eoddata.com/stocklist/NASDAQ/{0}.htm", Date = date },
                new Exchanges() { FullExchangeName = "American Stock Exchange", Exchange = "AMEX", ExchangeURL = "http://eoddata.com/stocklist/AMEX/{0}.htm", Date = date }
            };

            return exchanges;
        }
    }
}
