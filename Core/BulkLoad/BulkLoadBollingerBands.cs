using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoadBollingerBands : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] { "Symbol", "Date", "Open", "High", "Low", "Close", "SMA20", "SMA50", "SMA200", "StandardDeviation", "UpperBand", "LowerBand", "BandRatio", "Volume" };

        public BulkLoadBollingerBands(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithDailyHistory(IEnumerable<BollingerBands> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.Symbol + "^" + value.Date + "^" + value.Open + "^" + value.High + "^" + value.Low + "^" + value.Close
                    + "^" + value.SMA20 + "^" + value.SMA50 + "^" + value.SMA200
                    + "^" + value.StandardDeviation + "^" + value.UpperBand + "^" + value.LowerBand + "^" + value.BandRatio + "^" + value.Volume;

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
        ~BulkLoadBollingerBands()
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
