using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoadStandardDeviations : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] { "Symbol", "Count", "First", "FirstDate", "LastDate", "Last", "Slope", "Difference" };

        public BulkLoadStandardDeviations(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithStandardDeviations(IEnumerable<StandardDeviations> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.Symbol + "^" + value.Count + "^" + value.First + "^" + value.FirstDate + "^" + value.LastDate + "^" + value.Last
                    + "^" + value.Slope + "^" + value.Difference;

                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }

        #region Implement IDisposable
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~BulkLoadStandardDeviations()
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
