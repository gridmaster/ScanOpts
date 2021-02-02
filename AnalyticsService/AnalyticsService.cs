using Core;
using Core.BulkLoad;
using Core.BusinessModels;
using Core.Interface;
using Core.ORMModels;
using ORMService;
using System;
using System.Collections.Generic;

namespace DataAnalyticsService
{
    public class AnalyticsService : BaseService, IAnalyticsService
    {
        #region Private properties
        private BollingerBandORMService bollingerBandORMService = null;
        private BulkLoadSlopeCounts bulkLoadSlopeCounts = null;
        private ISMA60CycleService sMA60CycleService = null;
        private BulkLoad60SMASlopes bulkLoad60SMASlopes = null;
        private DailyQuotesORMService dailyQuotesORMService = null;

        #endregion Private properties

        #region Public properties

        #endregion Public properties

        #region Constructors

        public AnalyticsService(ILogger logger, DailyQuotesORMService dailyQuotesORMService, BollingerBandORMService bollingerBandORMService,
            BulkLoadSlopeCounts bulkloadSlopeCounts, BulkLoad60SMASlopes bulkLoad60SMASlopes, ISMA60CycleService sMA60CycleService) //, BulkLoadAnalyticsService bulkLoadAnalyticsService)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;

            this.dailyQuotesORMService = dailyQuotesORMService;

            this.bollingerBandORMService = bollingerBandORMService;
            //this.bulkLoadSlopeCounts = bulkloadSlopeCounts;

            this.sMA60CycleService = sMA60CycleService;
            this.bulkLoad60SMASlopes = bulkLoad60SMASlopes;
        }

        #endregion Constructors

        public List<SlopeAnd60sCounts> FindRising60SMATrends(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }

            return FindRising60SMATrends(syms);
        }

        public List<SlopeAnd60sCounts> FindRising60SMATrends(List<string> symbols)
        {
            logger.Info("Start - FindRising60SMATrends");
            List<SlopeAnd60sCounts> slopeAndCounts = null;

            if (symbols == null || symbols.Count == 0)
            {
                logger.WarnFormat("FindRising60SMATrends: No symbols were sent.");
                return null;
            }

            try
            {
                foreach (string symbol in symbols)
                {
                    //if (symbol != "CCCL") continue;

                    logger.InfoFormat("Processing symbol {0}", symbol);
                    //IEnumerable<BollingerBands> eBollingerBands = bollingerBandORMService.GetSymbolData(symbol);

                    var dailyQuotes = dailyQuotesORMService.GetDailyQuotes(symbol);

                    int start = 0;
                    decimal? lastClose = 0;
                    decimal? lastSMAHigh60 = 0;
                    decimal? lastSMALow60 = 0;
                    decimal? lastSMAClose60 = 0;
                    int? lastSMAVolume60 = 0;
                    decimal? lastSlopeHigh60 = 0;
                    decimal? lastSlopeLow60 = 0;
                    decimal? lastSlopeClose60 = 0;
                    int lastSlopeVolume60 = 0;
                    int Count60 = 0;

                    slopeAndCounts = new List<SlopeAnd60sCounts>();

                    foreach (DailyQuotes item in dailyQuotes)
                    {
                        if (start == 0)
                        {
                            lastClose = item.Close;
                            start++;
                            continue;
                        }

                        //var Date = FromUnixTimestamp(item.Timestamp);

                        SlopeAnd60sCounts sbbr = new SlopeAnd60sCounts()
                        {
                            SymbolId = item.Id,
                            Symbol = item.Symbol,
                            Date = Core.Business.UnixTimeConverter.UnixTimeStampToDateTime((double)item.Timestamp), // , // FromUnixTimestamp(item.Timestamp),
                            Exchange = item.Exchange,
                            InstrumentType = item.InstrumentType,
                            Timestamp = item.Timestamp,
                            Open = item.Open,
                            Close = item.Close,
                            High = item.High,
                            Low = item.Low,
                            Volume = item.Volume,

                            SMA60High = item.SMA60High,
                            SMA60Low = item.SMA60Low,
                            SMA60Close = item.SMA60Close,
                            SMA60Volume = item.SMA60Volume,

                            //Slope60Close = item.Close - lastClose,
                            Slope60Close = lastSMAClose60 > 0 ? item.SMA60Close - lastSMAClose60 : 0,
                            Slope60High = lastSMAHigh60 > 0 ? item.SMA60High - lastSMAHigh60 : 0,
                            Slope60Low = lastSMALow60 > 0 ? item.SMA60Low - lastSMALow60 : 0,
                            Slope60Volume = lastSMAVolume60 > 0 ? item.SMA60Volume - lastSMAVolume60 : 0,

                            Ratio60High = 0,
                            Ratio60Low = 0,
                            Ratio60Close = 0
                        };

                        //sbbr.CountClose = sbbr.SlopeClose > 0 ? ++CountClose : 0;
                        //sbbr.Count20 = sbbr.Slope20 > 0 ? ++Count20 : 0;
                        //sbbr.Count50 = sbbr.Slope50 > 0 ? ++Count50 : 0;
                        //sbbr.Count200 = sbbr.Slope200 > 0 ? ++Count200 : 0;
                        //sbbr.CountStandardDeviation = sbbr.SlopeStandardDeviation > 0 ? ++CountStandardDeviation : 0;
                        //sbbr.CountUpperBand = sbbr.SlopeUpperBand > 0 ? ++CountUpperBand : 0;
                        //sbbr.CountLowerBand = sbbr.SlopeLowerBand > 0 ? ++CountLowerBand : 0;
                        //sbbr.CountBandRatio = sbbr.SlopeBandRatio > 0 ? ++CountBandRatio : 0;

                        //sbbResults.Add(sbbr);

                        slopeAndCounts.Add(sbbr);

                        start++;
                        //if(sbbr.Slope200 > 0 )
                        //    start++;
                        //if (sbbr.SlopeClose <= 0) CountClose = 0;
                        //if (sbbr.Slope20 <= 0) Count20 = 0;
                        //if (sbbr.Slope50 <= 0) Count50 = 0;
                        //if (sbbr.Slope200 <= 0) Count200 = 0;
                        //if (sbbr.SlopeStandardDeviation <= 0) CountStandardDeviation = 0;
                        //if (sbbr.SlopeUpperBand <= 0) CountUpperBand = 0;
                        //if (sbbr.SlopeLowerBand <= 0) CountLowerBand = 0;
                        //if (sbbr.SlopeBandRatio <= 0) CountBandRatio = 0;

                        if (sbbr.Slope60High <= 0) Count60 = 0;

                        if (start > 60)
                        {
                            lastSMAHigh60 = item.SMA60High;
                            lastSMALow60 = item.SMA60Low;
                            lastSMAClose60 = item.SMA60Close;
                            lastSMAVolume60 = item.SMA60Volume;
                        }
                    }

                    //if (sbbResults.Count > 0)
                    //    start = start;

                    //BulkLoadSlopeAndBBCounts(sbbResults);
                    BulkLoadSlope60(slopeAndCounts);
                    var mystart = start;
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("Error - FindRising50SMATrends {0}", ex.Message);
            }

            logger.Info("End - FindRising50SMATrends");
            return slopeAndCounts;
        }
        
        public List<DailyQuotes> FindRising60SMATrends(ref List<DailyQuotes> symbols)
        {
            logger.Info("Start - FindRising60SMATrends");
            List<SlopeAnd60sCounts> slopeAndCounts = null;

            if (symbols == null || symbols.Count == 0)
            {
                logger.WarnFormat("FindRising60SMATrends: No symbols were sent.");
                return null;
            }

            try
            {
                foreach (string symbol in symbols)
                {
                    logger.InfoFormat("Processing symbol {0}", symbol);

                    var dailyQuotes = dailyQuotesORMService.GetDailyQuotes(symbol);

                    int start = 0;
                    decimal? lastClose = 0;
                    decimal? lastSMAHigh60 = 0;
                    decimal? lastSMALow60 = 0;
                    decimal? lastSMAClose60 = 0;
                    int? lastSMAVolume60 = 0;
                    decimal? lastSlopeHigh60 = 0;
                    decimal? lastSlopeLow60 = 0;
                    decimal? lastSlopeClose60 = 0;
                    int lastSlopeVolume60 = 0;
                    int Count60 = 0;

                    slopeAndCounts = new List<SlopeAnd60sCounts>();

                    foreach (DailyQuotes item in dailyQuotes)
                    {
                        if (start == 0)
                        {
                            lastClose = item.Close;
                            start++;
                            continue;
                        }

                        //var Date = FromUnixTimestamp(item.Timestamp);

                        SlopeAnd60sCounts sbbr = new SlopeAnd60sCounts()
                        {
                            SymbolId = item.Id,
                            Symbol = item.Symbol,
                            Date = Core.Business.UnixTimeConverter.UnixTimeStampToDateTime((double)item.Timestamp),
                            Exchange = item.Exchange,
                            InstrumentType = item.InstrumentType,
                            Timestamp = item.Timestamp,
                            Open = item.Open,
                            Close = item.Close,
                            High = item.High,
                            Low = item.Low,
                            Volume = item.Volume,

                            SMA60High = item.SMA60High,
                            SMA60Low = item.SMA60Low,
                            SMA60Close = item.SMA60Close,
                            SMA60Volume = item.SMA60Volume,

                            //Slope60Close = item.Close - lastClose,
                            Slope60Close = lastSMAClose60 > 0 ? item.SMA60Close - lastSMAClose60 : 0,
                            Slope60High = lastSMAHigh60 > 0 ? item.SMA60High - lastSMAHigh60 : 0,
                            Slope60Low = lastSMALow60 > 0 ? item.SMA60Low - lastSMALow60 : 0,
                            Slope60Volume = lastSMAVolume60 > 0 ? item.SMA60Volume - lastSMAVolume60 : 0,

                            Ratio60High = 0,
                            Ratio60Low = 0,
                            Ratio60Close = 0
                        };

                        slopeAndCounts.Add(sbbr);

                        start++;

                        if (sbbr.Slope60High <= 0) Count60 = 0;

                        if (start > 60)
                        {
                            lastSMAHigh60 = item.SMA60High;
                            lastSMALow60 = item.SMA60Low;
                            lastSMAClose60 = item.SMA60Close;
                            lastSMAVolume60 = item.SMA60Volume;
                        }
                    }

                    //if (sbbResults.Count > 0)
                    //    start = start;

                    //BulkLoadSlopeAndBBCounts(sbbResults);
                    BulkLoadSlope60(slopeAndCounts);
                    var mystart = start;
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("Error - FindRising50SMATrends {0}", ex.Message);
            }

            logger.Info("End - FindRising50SMATrends");
            return slopeAndCounts;
        }


        #region SlopeAndBBCounts
        public List<SlopeAndBBCounts> FindRising50SMATrends(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }

            return FindRising50SMATrends(syms);
        }

        public List<SlopeAndBBCounts> FindRising50SMATrends(List<string> symbols)
        {
            logger.Info("Start - FindRising50SMATrends");
            List<SlopeAndBBCounts> sbbResults = null;

            if (symbols == null || symbols.Count == 0)
            {
                logger.WarnFormat("FindRising50SMATrends: No symbols were sent.");
                return null;
            }
            
            try
            {
                foreach (string symbol in symbols)
                {
                    if (symbol != "CCCL") continue;

                    logger.InfoFormat("Processing symbol {0}", symbol);
                    IEnumerable<BollingerBands> eBollingerBands = bollingerBandORMService.GetSymbolData(symbol);

                    int start = 0;
                    decimal lastClose = 0;
                    decimal lastSMA20 = 0;
                    decimal lastSMA50 = 0;
                    decimal lastSMA200 = 0;
                    decimal lastStandardDeviation = 0;
                    decimal lastUpperBand = 0;
                    decimal lastLowerBand = 0;
                    decimal lastBandRatio = 0;
                    int CountClose = 0;
                    int Count20 = 0;
                    int Count50 = 0;
                    int Count200 = 0;
                    int CountStandardDeviation = 0;
                    int CountUpperBand = 0;
                    int CountLowerBand = 0;
                    int CountBandRatio = 0;

                    sbbResults = new List<SlopeAndBBCounts>();

                    foreach (BollingerBands item in eBollingerBands)
                    {
                        if (start == 0)
                        {
                            lastClose = item.Close;
                            start++;
                            continue;
                        }

                        SlopeAndBBCounts sbbr = new SlopeAndBBCounts()
                        {
                            SymbolId = item.Id,
                            Symbol = item.Symbol,
                            Date = item.Date,
                            Close = item.Close,
                            SMA20 = item.SMA20,
                            SMA50 = item.SMA50,
                            SMA200 = item.SMA200,
                            StandardDeviation = item.StandardDeviation,
                            UpperBand = item.UpperBand,
                            LowerBand = item.LowerBand,
                            BandRatio = item.BandRatio,
                            Volume = item.Volume,
                            SlopeClose = item.Close - lastClose,
                            Slope20 = lastSMA20 > 0 ? item.SMA20 - lastSMA20 : 0,
                            Slope50 = lastSMA50 > 0 ? item.SMA50 - lastSMA50 : 0,
                            Slope200 = lastSMA200 > 0 ? item.SMA200 - lastSMA200 : 0,
                            SlopeStandardDeviation = lastStandardDeviation > 0 ? item.StandardDeviation - lastStandardDeviation : 0,
                            SlopeUpperBand = lastUpperBand > 0 ? item.UpperBand - lastUpperBand : 0,
                            SlopeLowerBand = lastLowerBand > 0 ? item.LowerBand - lastLowerBand : 0,
                            SlopeBandRatio = lastBandRatio > 0 ? item.BandRatio - lastBandRatio : 0,
                            RatioClose = item.Close / lastClose,
                            Ratio20 = lastSMA20 > 0 ? item.SMA20 / lastSMA20 : 0,
                            Ratio50 = lastSMA50 > 0 ? item.SMA50 / lastSMA50 : 0,
                            Ratio200 = lastSMA200 > 0 ? item.SMA200 / lastSMA200 : 0,
                            RatioStandardDeviation = lastStandardDeviation > 0 ? item.StandardDeviation / lastStandardDeviation : 0,
                            RatioUpperBand = lastUpperBand > 0 ? item.UpperBand / lastUpperBand : 0,
                            RatioLowerBand = lastLowerBand > 0 ? item.LowerBand / lastLowerBand : 0,
                            RatioBandRatio = lastBandRatio > 0 ? item.BandRatio / lastBandRatio : 0
                        };

                        sbbr.CountClose = sbbr.SlopeClose > 0 ? ++CountClose : 0;
                        sbbr.Count20 = sbbr.Slope20 > 0 ? ++Count20 : 0;
                        sbbr.Count50 = sbbr.Slope50 > 0 ? ++Count50 : 0;
                        sbbr.Count200 = sbbr.Slope200 > 0 ? ++Count200 : 0;
                        sbbr.CountStandardDeviation = sbbr.SlopeStandardDeviation > 0 ? ++CountStandardDeviation : 0;
                        sbbr.CountUpperBand = sbbr.SlopeUpperBand > 0 ? ++CountUpperBand : 0;
                        sbbr.CountLowerBand = sbbr.SlopeLowerBand > 0 ? ++CountLowerBand : 0;
                        sbbr.CountBandRatio = sbbr.SlopeBandRatio > 0 ? ++CountBandRatio : 0;

                        sbbResults.Add(sbbr);

                        start++;
                        //if(sbbr.Slope200 > 0 )
                        //    start++;
                        if (sbbr.SlopeClose <= 0) CountClose = 0;
                        if (sbbr.Slope20 <= 0) Count20 = 0;
                        if (sbbr.Slope50 <= 0) Count50 = 0;
                        if (sbbr.Slope200 <= 0) Count200 = 0;
                        if (sbbr.SlopeStandardDeviation <= 0) CountStandardDeviation = 0;
                        if (sbbr.SlopeUpperBand <= 0) CountUpperBand = 0;
                        if (sbbr.SlopeLowerBand <= 0) CountLowerBand = 0;
                        if (sbbr.SlopeBandRatio <= 0) CountBandRatio = 0;

                        if (start > 18)
                        {
                            lastSMA20 = item.SMA20;
                            lastStandardDeviation = item.StandardDeviation;
                            lastUpperBand = item.UpperBand;
                            lastLowerBand = item.LowerBand;
                            lastBandRatio = item.BandRatio;
                        }
                        if (start > 48)
                            lastSMA50 = item.SMA50;
                        if (start > 198)
                            lastSMA200 = item.SMA200;
                        lastClose = sbbr.Close;
                    }

                    //if (sbbResults.Count > 0)
                    //    start = start;

                    BulkLoadSlopeAndBBCounts(sbbResults);
                }
            }
            catch(Exception ex)
            {
                logger.InfoFormat("Error - FindRising50SMATrends {0}", ex.Message);
            }

            logger.Info("End - FindRising50SMATrends");
            return sbbResults;
        }
        #endregion SlopeAndBBCounts

        private bool BulkLoadSlope60(List<SlopeAnd60sCounts> counts)
        {
            bool success = false;
            try
            {
                var dt = bulkLoad60SMASlopes.ConfigureDataTable();

                dt = bulkLoad60SMASlopes.LoadDataTableWith60CycleSlopes(counts, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadSlopeAndBBCounts", Environment.NewLine);
                }
                else
                {
                    success = bulkLoad60SMASlopes.BulkCopy<SlopeAnd60sCounts>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadSlopeAndBBCounts returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Bollinger Bands Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }

        private bool BulkLoadSlopeAndBBCounts(List<SlopeAndBBCounts> counts)
        {
            bool success = false;
            try
            {
                var dt = bulkLoadSlopeCounts.ConfigureDataTable();

                dt = bulkLoadSlopeCounts.LoadDataTableWithDailyHistory(counts, dt);

                if (dt == null)
                {
                    logger.InfoFormat("{0}No data returned on BulkLoadSlopeAndBBCounts", Environment.NewLine);
                }
                else
                {
                    success = bulkLoadSlopeCounts.BulkCopy<SlopeAndBBCounts>(dt, "ScanOptsContext");
                    logger.InfoFormat("{0}BulkLoadSlopeAndBBCounts returned with: {1}", Environment.NewLine,
                                            success ? "Success" : "Fail");
                }
            }
            catch (Exception ex)
            {
                logger.InfoFormat("{0}Bulk Load Bollinger Bands Error: {1}", Environment.NewLine, ex.Message);
            }
            return success;
        }
    }
}
