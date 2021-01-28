using Core.BusinessModels;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoadSlopeCounts : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] { "SymbolId", "Symbol", "Date", "Close",
            "SMA20", "SMA50", "SMA200", "StandardDeviation", "UpperBand", "LowerBand", "BandRatio", "Volume",
            "SlopeClose", "Slope20", "Slope50", "Slope200", "SlopeStandardDeviation", "SlopeUpperBand", "SlopeLowerBand", "SlopeBandRatio",
            "RatioClose", "Ratio20", "Ratio50", "Ratio200", "RatioStandardDeviation", "RatioUpperBand", "RatioLowerBand", "RatioBandRatio",
            "CountClose", "Count20", "Count50", "Count200", "CountStandardDeviation", "CountUpperBand", "CountLowerBand", "CountBandRatio"};

        public BulkLoadSlopeCounts(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithDailyHistory(IEnumerable<SlopeAndBBCounts> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.SymbolId + "^" + value.Symbol + "^" + value.Date + "^" + value.Close + "^" +
                    value.SMA20 + "^" + value.SMA50 + "^" + value.SMA200 + "^" + value.StandardDeviation + "^" + value.UpperBand + "^" + value.LowerBand + "^" + value.BandRatio + "^" + value.Volume + "^" +
                    value.SlopeClose + "^" + value.Slope20 + "^" + value.Slope50 + "^" + value.Slope200 + "^" + value.SlopeStandardDeviation + "^" + value.SlopeUpperBand + "^" + value.SlopeLowerBand + "^" + value.SlopeBandRatio + "^" +
                    value.RatioClose + "^" + value.Ratio20 + "^" + value.Ratio50 + "^" + value.Ratio200 + "^" + value.RatioStandardDeviation + "^" + value.RatioUpperBand + "^" + value.RatioLowerBand + "^" + value.RatioBandRatio + "^" +
                    value.CountClose + "^" + value.Count20 + "^" + value.Count50 + "^" + value.Count200 + "^" + value.CountStandardDeviation + "^" + value.CountUpperBand + "^" + value.CountLowerBand + "^" + value.CountBandRatio;
                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }

        #region Implement IDisposable
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        //More Info

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~BulkLoadSlopeCounts()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }

        #endregion Implement IDisposable
    }
}

