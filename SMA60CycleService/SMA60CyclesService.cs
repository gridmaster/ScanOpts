using Core;
using Core.Interface;
using Core.ORMModels;
using ORMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMA60CycleService
{
    public class SMA60CyclesService : BaseService, ISMA60CycleService
    {
        #region Private properties
        private bool success;
        private List<string> symbolList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private ORMService.DailyQuotesORMService dailyQuotesORMService = null;
        private bool _runDaily = true;
        #endregion Private properties

        #region Public properties

        #endregion Public properties
        
        #region Constructors
        //public SMA60CyclesService(ILogger logger) : base(logger)
        //{
        //}

        public SMA60CyclesService(ILogger logger, ORMService.DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService) //, BulkLoadBollingerBands bulkLoadBollingerBands)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.dailyQuotesORMService = dailyQuotesORMService;
        }
        #endregion Constructors

        #region Public Methods
        public List<DailyQuotes> GenerateSMA60s()
        {
            List<DailyQuotes> result = new List<DailyQuotes>();

            List<string> symbols = dailyQuotesORMService.GetSymbols();

            result = GenerateSMA60s(symbols);

            return result;
        }

        public List<DailyQuotes> GenerateSMA60s(List<string> symbols)
        {
            logger.Info("Start - GenerateSMA60s");

            List<DailyQuotes> result = new List<DailyQuotes>();

            List<decimal?> SMA60High = new List<decimal?>();
            List<decimal?> SMA60Low = new List<decimal?>();
            List<decimal?> SMA60Close = new List<decimal?>();
            List<long?> SMA60Volume = new List<long?>();
            int iNdx = 0;

            foreach (string symbol in symbols)
            {
                var quotes = dailyQuotesORMService.GetSymbolDailyData(symbol).OrderBy(s => s.Timestamp);

                logger.InfoFormat("{0} - Processing symbol {1}", DateTime.Now, symbol);

                foreach (DailyQuotes quote in quotes) {

                    //List<DailyQuotes> wtf = quotes.Where(q => q.High == 0);

                    if (quote.High == 0)
                    {
                        logger.InfoFormat("{0} - Processing symbol {1} quote.High == 0", DateTime.Now, symbol);
                        break;
                    }

                    SMA60High.Add(quote.High);
                    SMA60Low.Add(quote.Low);
                    SMA60Close.Add(quote.Close);
                    SMA60Volume.Add(quote.Volume);
                    iNdx++;

                    if(iNdx > 59)
                    {
                        try
                        {
                            DailyQuotes dq = dailyQuotesORMService.GetDailyQuote(symbol, (int)quote.Timestamp);

                            dq.SMA60High = SMA60High.Sum() / 60;
                            dq.SMA60Low = SMA60Low.Sum() / 60;
                            dq.SMA60Close = SMA60Close.Sum() / 60;
                            dq.SMA60Volume = (int)(SMA60Volume.Sum() / 60);

                            if(dq.SMA60Close > 0)
                                result.Add(dq);

                            SMA60High.RemoveAt(0);
                            SMA60Low.RemoveAt(0);
                            SMA60Close.RemoveAt(0);
                            SMA60Volume.RemoveAt(0);
                        }
                        catch (Exception ex)
                        {
                            //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                            var what = ex.Message;
                        }
                    }
                }

                if (result.Count > 0)
                {
                    dailyQuotesORMService.UpdateDailyQuotes(result);
                }

                // clean up before next symbol
                result.Clear();
                SMA60High.Clear();
                SMA60Low.Clear();
                SMA60Close.Clear();
                SMA60Volume.Clear();
                iNdx = 0;
            }

            logger.Info("End - GenerateSMA60s");

            return result;
        }
        
        public List<DailyQuotes> GenerateSMA60sNoUpdate(List<string> symbols)
        {
            logger.Info("Start - GenerateSMA60sNoUpdate");

            List<DailyQuotes> result = new List<DailyQuotes>();
            List<DailyQuotes> fullResult = new List<DailyQuotes>();

            List<decimal?> SMA60High = new List<decimal?>();
            List<decimal?> SMA60Low = new List<decimal?>();
            List<decimal?> SMA60Close = new List<decimal?>();
            List<long?> SMA60Volume = new List<long?>();
            int iNdx = 0;

            foreach (string symbol in symbols)
            {
                var quotes = dailyQuotesORMService.GetSymbolDailyData(symbol).OrderBy(s => s.Timestamp);

                //logger.InfoFormat("{0} - Processing symbol {1}", DateTime.Now, symbol);

                foreach (DailyQuotes quote in quotes)
                {

                    //List<DailyQuotes> wtf = quotes.Where(q => q.High == 0);

                    if (quote.High == 0)
                    {
                        logger.InfoFormat("{0} - Processing symbol {1} quote.High == 0", DateTime.Now, symbol);
                        break;
                    }

                    SMA60High.Add(quote.High);
                    SMA60Low.Add(quote.Low);
                    SMA60Close.Add(quote.Close);
                    SMA60Volume.Add(quote.Volume);
                    iNdx++;

                    if (iNdx > 59)
                    {
                        try
                        {
                            DailyQuotes dq = dailyQuotesORMService.GetDailyQuote(symbol, (int)quote.Timestamp);

                            dq.SMA60High = SMA60High.Sum() / 60;
                            dq.SMA60Low = SMA60Low.Sum() / 60;
                            dq.SMA60Close = SMA60Close.Sum() / 60;
                            dq.SMA60Volume = (int)(SMA60Volume.Sum() / 60);

                            if (dq.SMA60Close > 0)
                            {
                                result.Add(dq);
                            }

                            SMA60High.RemoveAt(0);
                            SMA60Low.RemoveAt(0);
                            SMA60Close.RemoveAt(0);
                            SMA60Volume.RemoveAt(0);
                        }
                        catch (Exception ex)
                        {
                            //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                            var what = ex.Message;
                        }
                    }
                }

                if (result.Count > 0)
                {
                    fullResult.AddRange(result);
                    //dailyQuotesORMService.UpdateDailyQuotes(result);
                }

                // clean up before next symbol
                result.Clear();
                SMA60High.Clear();
                SMA60Low.Clear();
                SMA60Close.Clear();
                SMA60Volume.Clear();
                iNdx = 0;
            }

            logger.Info("End - GenerateSMA60sNoUpdate");

            return fullResult;
        }

        public List<DailyQuotes> GenerateSMA60ForSymbol(ref List<DailyQuotes> quotes)
        {
            logger.Info("Start - GenerateSMA60ForSymbol");

            List<DailyQuotes> result = new List<DailyQuotes>();

            List<decimal?> SMA60High = new List<decimal?>();
            List<decimal?> SMA60Low = new List<decimal?>();
            List<decimal?> SMA60Close = new List<decimal?>();
            List<long?> SMA60Volume = new List<long?>();
            int iNdx = 0;

            foreach (DailyQuotes quote in quotes)
            {
                if (quote.High == 0)
                {
                    logger.InfoFormat("{0} - Processing symbol {1} quote.High == 0", DateTime.Now, quote.Symbol);
                    break;
                }

                SMA60High.Add(quote.High);
                SMA60Low.Add(quote.Low);
                SMA60Close.Add(quote.Close);
                SMA60Volume.Add(quote.Volume);
                iNdx++;

                if (iNdx > 59)
                {
                    try
                    {
                        //DailyQuotes dq = dailyQuotesORMService.GetDailyQuote(quote.Symbol, (int)quote.Timestamp);

                        quote.SMA60High = SMA60High.Sum() / 60;
                        quote.SMA60Low = SMA60Low.Sum() / 60;
                        quote.SMA60Close = SMA60Close.Sum() / 60;
                        quote.SMA60Volume = (int)(SMA60Volume.Sum() / 60);

                        //if (quote.SMA60Close > 0)
                        //{
                        //    result.Add(dq);
                        //}

                        SMA60High.RemoveAt(0);
                        SMA60Low.RemoveAt(0);
                        SMA60Close.RemoveAt(0);
                        SMA60Volume.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        //logger.InfoFormat($@"ERROR - GetFullExchangeName - Error: {ex.Message}");
                        var what = ex.Message;
                    }
                }
            }

            logger.Info("End - GenerateSMA60ForSymbol");

            return quotes;
        }
        #endregion Public Methods
    }
}
