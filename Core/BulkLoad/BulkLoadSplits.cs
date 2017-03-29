using Core.Interface;
using Core.JsonQuote;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoadSplits : BaseBulkLoad, IDisposable
    {

        private static readonly string[] ColumnNames = new string[] { "Symbol", "Date", "Exchange", "SplitDate", "Numerator", "Denominator", "Ratio" };

        public BulkLoadSplits(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithDividends(Splits dStats, DataTable dt)
        {
            for(int i = 0; i < dStats.splits.Count; i++ )
            {
                var sValue = dStats.splits[i].Symbol + "^" + dStats.splits[i].Date + "^" + dStats.splits[i].Exchange + "^" + dStats.splits[i].SplitDate + "^" + 
                                dStats.splits[i].Numerator + "^" + dStats.splits[i].Denominator + "^" + dStats.splits[i].Ratio;

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
        ~BulkLoadSplits()
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
