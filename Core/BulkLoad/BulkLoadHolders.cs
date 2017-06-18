using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.BulkLoad
{
    class BulkLoadHolders : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] { "Symbol", "Date", "Exchange", "Timestamp",
            "SharesInsidersFivePctOwners", "PctInstitutionalAndMutualFund", "PctFloatInstitutionalAndMutualFund", "NumberInstitutionsHolding",
            "PurchasesShares", "PurchasesTransactions", "SalesShares", "SalesTransactions", "NetPurchasedOrSoldShares", "NetPurchasedOrSoldTransactions",
            "TotalInsiderSharesHeld", "PctNetSharesPurchasedOrSoldShares", "PctNetSharesPurchasedOrSoldTransactions", "NetSharesPurchasedOrSold", "PctChangeInInstitutionalSharesHeld" };

        public BulkLoadHolders(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithDailyHistory(IEnumerable<DailyQuotes> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                var sValue = value.Symbol + "^" + value.Date + "^" + value.Exchange + "^" + value.InstrumentType + "^" + value.Timestamp + "^" + value.Close + "^" + value.High
                    + "^" + value.Low + "^" + value.Open + "^" + value.Volume + "^" + value.UnadjHigh + "^" + value.UnadjLow + "^" + value.UnadjClose + "^" + value.UnadjOpen;

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
        ~BulkLoadHolders()
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
