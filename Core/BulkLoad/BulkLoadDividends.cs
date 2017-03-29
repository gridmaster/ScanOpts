using Core.Interface;
using Core.JsonQuote;
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoadDividends : BaseBulkLoad, IDisposable
    {

        private static readonly string[] ColumnNames = new string[] { "Symbol", "Date", "Exchange", "DividendDate", "DividendAmount" };

        public BulkLoadDividends(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithDividends(Dividends dStats, DataTable dt)
        {
            for(int i = 0; i < dStats.dividends.Count; i++)
            {
                var sValue = dStats.dividends[i].Symbol + "^" + dStats.dividends[i].Date + "^" + dStats.dividends[i].Exchange + "^" + dStats.dividends[i].DividendDate + "^" + dStats.dividends[i].DividendAmount;

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
        ~BulkLoadDividends()
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
