using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Core.ORMModels;

namespace Core.BulkLoad
{
    public class BulkLoadSymbol : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] { "Symbol", "CompanyName", "Exchange", "FullExchangeName", "Date" };

        public BulkLoadSymbol() : base(ColumnNames)
        {

        }

        public DataTable LoadDataTableWithSymbols(IEnumerable<Symbols> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.Symbol + "^" + value.CompanyName + "^" + value.Exchange + "^" + value.FullExchangeName +  "^" + value.Date;

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
        ~BulkLoadSymbol()
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

        public DataTable LoadDataTableWithSymbols(object symbolList, DataTable dt)
        {
            throw new NotImplementedException();
        }
        #endregion Implement IDisposable
    }
}
