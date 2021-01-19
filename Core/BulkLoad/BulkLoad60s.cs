using Core.BusinessModels;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.BulkLoad
{
    public class BulkLoad60s : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] { "SymbolId", "Symbol", "Date", "High", "Low", "Close", "Volume",
            "SMA60High", "SMA60Low", "SMA60Close", "Slope60High", "Slope60Low", "Slope60Close", "Ratio60High", "Ratio60Low", "Ratio60Close"};

        public BulkLoad60s(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithDailyHistory(IEnumerable<SlopeAnd60sCounts> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.SymbolId + "^" + value.Symbol + "^" + value.Date + "^" + value.High + "^" +
                    value.Low + "^" + value.Close + "^" + value.Volume + "^" + value.SMA60High + "^" + value.SMA60Low + "^" + value.SMA60Close
                    + value.Slope60High + "^" + value.Slope60Low + "^" + value.Slope60Close + value.Ratio60High + "^" + value.Ratio60Low + "^" + value.Ratio60Close;
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
        ~BulkLoad60s()
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
