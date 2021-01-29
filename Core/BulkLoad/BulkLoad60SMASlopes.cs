using Core.BusinessModels;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoad60SMASlopes : BaseBulkLoad, IDisposable
    {

        private static readonly string[] ColumnNames = new string[] { "SymbolId", "Symbol", "Date", "Exchange", "InstrumentType", "Timestamp", "Close", "High", "Low", "Open", "Volume",
            "SMA60High", "SMA60Low", "SMA60Close", "SMA60Volume", "Slope60High", "Slope60Low", "Slope60Close", "Slope60Volume", "Ratio60High", "Ratio60Low", "Ratio60Close"};

        public BulkLoad60SMASlopes(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWith60CycleSlopes(IEnumerable<SlopeAnd60sCounts> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.SymbolId + "^" + value.Symbol + "^" + value.Date + "^" + value.Exchange + "^" + value.InstrumentType + "^" + value.Timestamp + "^" + value.Close + "^" + value.High
                    + "^" + value.Low + "^" + value.Open + "^" + value.Volume + "^" + value.SMA60High + "^" + value.SMA60Low + "^" + value.SMA60Close + "^" + value.SMA60Volume
                    + "^" + value.Slope60High + "^" + value.Slope60Low + "^" + value.Slope60Close + "^" + value.Slope60Volume + value.Ratio60High + "^" + value.Ratio60Low + "^" + value.Ratio60Close;

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
        ~BulkLoad60SMASlopes()
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
