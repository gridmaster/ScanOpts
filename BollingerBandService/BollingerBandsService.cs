﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Core;
using Core.Business;
using Core.Interface;
using Core.ORMModels;
using Core.BulkLoad;
using ORMService;

namespace BollingerBandService
{
    public class BollingerBandsService : BaseService, IBollingerBandService
    {
        #region Private properties
        private bool success;
        private List<string> symbolList;
        private SymbolsORMService symbolORMService = new SymbolsORMService();
        private ExchangeORMService exchangeORMService = new ExchangeORMService();
        private DailyQuotesORMService dailyQuotesORMService = null;
        private BulkLoadBollingerBands bulkLoadBollingerBands = null;
        #endregion Private properties

        #region Constructors

        public BollingerBandsService(ILogger logger, DailyQuotesORMService dailyQuotesORMService, SymbolsORMService symbolORMService, ExchangeORMService exchangeORMService, BulkLoadBollingerBands bulkLoadBollingerBands)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            this.symbolORMService = symbolORMService;
            this.exchangeORMService = exchangeORMService;
            this.dailyQuotesORMService = dailyQuotesORMService;
            this.bulkLoadBollingerBands = bulkLoadBollingerBands;
        }

        #endregion Constructors

        #region Public Methods

        public void RunBollingerBandsCheck()
        {
            logger.InfoFormat("RunKeyStatisticsCollection - GetSymbols");
            List<string> symbols = symbolORMService.GetSymbols();
            RunBollingerBandsCheck(symbols);
        }

        public void RunBollingerBandsCheck(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            RunBollingerBandsCheck(syms);
        }

        public void RunBollingerBandsCheck(List<string> symbols)
        {
            logger.InfoFormat("Start - RunBollingerBandsCheck");

            string uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1={1}&period2={2}&interval=1d&events=div%7Csplit&corsDomain=finance.yahoo.com";
            //string uriString = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?formatted=true&crumb=8ajQnG2d93l&lang=en-US&region=US&period1={1}&period2={2}&interval=1wk&events=div%7Csplit&corsDomain=finance.yahoo.com";

            // dates run from oldest to newest
            var endDate = DateTime.Now.ToUnixTime();
            var startDate = DateTime.Now.AddMonths(-18).ToUnixTime();

            bulkLoadBollingerBands.TruncateTable("BollingerBands");

            try
            {
                foreach (string symbol in symbols)
                {
                    logger.InfoFormat("Get {0} page", symbol);
                    //if (symbol != "ABEOW")
                    //{
                    //    continue;
                    //}
                    string sPage = WebPage.Get(String.Format(uriString, symbol, startDate, endDate));

                    if (sPage.Contains("(404) Not Found")) continue;
                    if (sPage.Contains("(400) Bad Request")) continue;
                    if (sPage.Contains("(502) Bad Gateway.")) continue;
                    Core.JsonQuote.JsonResult symbolHistory = new Core.JsonQuote.JsonResult();
                    try {
                        symbolHistory = JsonConvert.DeserializeObject<Core.JsonQuote.JsonResult>(sPage);
                    }
                    catch(Exception ex)
                    {
                        logger.Fatal("RunKeyStatisticsCollection: {0}", ex);
                        logger.InfoFormat("Page: {0}", sPage);
                    }
                    if (symbolHistory.Chart.Result[0].indicators.unadjclose[0].unadjclose == null) continue;
                    List<DailyQuotes> quotesList = dailyQuotesORMService.ExtractDailyQuotes(symbol, symbolHistory);

                    if(SkipThisSymbol(quotesList)) continue;

                    var bollingerBands= CalculateBollingerBands(quotesList);
                   
                    var result = BulkLoadBollingerBands(bollingerBands);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("RunKeyStatisticsCollection: {0}", ex);
            }
            finally
            {
                logger.Info("End - RunBollingerBandsCheck");
                logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
        }

        //public void LoadKeyStatisticsInfo()
        //{
        //    string url = "https://query1.finance.yahoo.com/v10/finance/quoteSummary/CAT?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=defaultKeyStatistics%2CfinancialData%2CcalendarEvents&corsDomain=finance.yahoo.com";
        //    string sPage = WebPage.Get(url);
        //    BaseObject.RootObject bo = JsonConvert.DeserializeObject<BaseObject.RootObject>(sPage);
        //}


        public List<BollingerBands> CalculateBollingerBands(List<DailyQuotes> quotesList)
        {
            List<BollingerBands> bolbands = new List<BollingerBands>();
            double doubleOut = 0;
            int intOut = 0;
            List<double> current20 = new List<double>();

            for (int i = 0; i < 20; i++)
            {
                BollingerBands bb = new BollingerBands();

                bb.Symbol = quotesList[i].Symbol;
                double.TryParse(quotesList[i].Close.ToString(), out doubleOut);
                bb.Close = doubleOut;
                current20.Add(doubleOut);
                bb.Date = UnixTimeConverter.UnixTimeStampToDateTime(quotesList[i].Timestamp.Value);
                double.TryParse(quotesList[i].High.ToString(), out doubleOut);
                bb.High = doubleOut;
                double.TryParse(quotesList[i].Low.ToString(), out doubleOut);
                bb.Low = doubleOut;
                double.TryParse(quotesList[i].Open.ToString(), out doubleOut);
                bb.Open = doubleOut;
                int.TryParse(quotesList[i].Volume.ToString(), out intOut);
                bb.Volume = intOut;

                bolbands.Add(bb);
            };
            bolbands[19].SMA20 = current20.Sum() / 20;

            double sd = CalculateStandardDeviation(bolbands, bolbands[19].SMA20);
            bolbands[19].StandardDeviation = sd;
            bolbands[19].UpperBand = bolbands[19].SMA20 + (sd * 2);
            bolbands[19].LowerBand = bolbands[19].SMA20 - (sd * 2);
            bolbands[19].BandRatio = (1 - (bolbands[19].LowerBand/bolbands[19].UpperBand))*100;

            for( int c = 0, i = 20; i < quotesList.Count(); i++)
            {
                BollingerBands bb = new BollingerBands();

                bb.Symbol = quotesList[i].Symbol;
                double.TryParse(quotesList[i].Close.ToString(), out doubleOut);
                bb.Close = doubleOut;

                if (i%20 == 0)
                {
                    c = 0;
                }
                current20[c++] = doubleOut;
                
                bb.Date = UnixTimeConverter.UnixTimeStampToDateTime(quotesList[i].Timestamp.Value);
                double.TryParse(quotesList[i].High.ToString(), out doubleOut);
                bb.High = doubleOut;
                double.TryParse(quotesList[i].Low.ToString(), out doubleOut);
                bb.Low = doubleOut;
                double.TryParse(quotesList[i].Open.ToString(), out doubleOut);
                bb.Open = doubleOut;
                double.TryParse(quotesList[i - 20].UnadjClose.ToString(), out doubleOut);

                int.TryParse(quotesList[i].Volume.ToString(), out intOut);
                bb.Volume = intOut;

                bb.SMA20 = current20.Sum() / 20;
                bolbands.Add(bb);
                
                sd = CalculateStandardDeviation(bolbands, bb.SMA20);
                bolbands[i].StandardDeviation = sd;
                bolbands[i].UpperBand = bolbands[i].SMA20 + (sd * 2);
                bolbands[i].LowerBand = bolbands[i].SMA20 - (sd * 2);
                bolbands[i].BandRatio = (1 - (bolbands[i].LowerBand / bolbands[i].UpperBand)) * 100;
            }

            return bolbands;
        }

        #endregion Public Methods

        #region Private Methods

        private bool SkipThisSymbol(List<DailyQuotes> quotes)
        {
            if (quotes.Count < 30) return true;

            DailyQuotes dq = quotes[quotes.Count - 1];
            if (dq.Close < 3) return true;

            return false;
        }

        private double CalculateStandardDeviation(List<BollingerBands> bolbands, double mean)
        {
            List<double> closeLessMean = new List<double>();
            List<double> closeLessMeanX2 = new List<double>();

            for(int c = 0, i = bolbands.Count-20; i< bolbands.Count; i++)
            {
                closeLessMean.Add(bolbands[i].Close - mean);
                closeLessMeanX2.Add(Math.Pow(closeLessMean[c++], 2));
            }

            double StdDeveation = closeLessMeanX2.Sum()/20;
            return Math.Sqrt(StdDeveation);
        }

        private bool BulkLoadBollingerBands(List<BollingerBands> bollband)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadBollingerBands.ConfigureDataTable();

                dt = bulkLoadBollingerBands.LoadDataTableWithDailyHistory(bollband, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadHistory", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadBollingerBands.BulkCopy<BollingerBands>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadBollingerBands returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Bollinger Bands Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        public class ClosingPrices
        {
            public double ClosingPrice { get; set; }
        }

        #endregion Private Methods
    }
}
