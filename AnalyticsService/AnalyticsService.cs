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
        #endregion Private properties

        #region Public properties

        #endregion Public properties

        #region Constructors

        public AnalyticsService(ILogger logger, BollingerBandORMService bollingerBandORMService, BulkLoadSlopeCounts bulkloadSlopeCounts) //, BulkLoadAnalyticsService bulkLoadAnalyticsService)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;

            this.bollingerBandORMService = bollingerBandORMService;
            this.bulkLoadSlopeCounts = bulkloadSlopeCounts;
        }

        #endregion Constructors

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
                    if (symbol != "SBS") continue;

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
                    //decimal RatioClose = 0;
                    //decimal Ratio20 = 0;
                    //decimal Ratio50 = 0;
                    //decimal Ratio200 = 0;
                    //decimal RatioStandardDeviation = 0;
                    //decimal RatioUpperBand = 0;
                    //decimal RatioLowerBand = 0;
                    //decimal RatioBandRatio = 0;

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

                        //if (sbbr.RatioClose < 0) RatioClose = 0;
                        //if (sbbr.Ratio20 < 0) Ratio20 = 0;
                        //if (sbbr.Ratio50 < 0) Ratio50 = 0;
                        //if (sbbr.Ratio200 < 0) Ratio200 = 0;
                        //if (sbbr.RatioStandardDeviation < 0) RatioStandardDeviation = 0;
                        //if (sbbr.RatioUpperBand < 0) RatioUpperBand = 0;
                        //if (sbbr.RatioLowerBand < 0) RatioLowerBand = 0;
                        //if (sbbr.RatioBandRatio < 0) RatioBandRatio = 0;

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
