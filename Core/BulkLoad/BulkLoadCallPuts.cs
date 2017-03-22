using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Core.BulkLoad
{
    public class BulkLoadCallPuts : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[] {"QuoteId", "CallOrPut", "Symbol", "ExpirationRaw", "StrikeRaw", "ExpirationFmt", "ExpirationLongFmt",
                "StrikeFmt", "StrikeLongFmt", "Date", "PercentChangeRaw", "PercentChangeFmt", "PercentChangeLongFmt", "OpenInterestRaw", "OpenInterestFmt", "OpenInterestLongFmt",
                "ChangeRaw", "ChangeFmt", "ChangeLongFmt", "InTheMoney", "ImpliedVolatilityRaw", "ImpliedVolatilityFmt", "ImpliedVolatilityLongFmt", "VolumeRaw", "VolumeFmt",
                "VolumeLongFmt", "ContractSymbol", "AskRaw", "AskFmt", "AskLongFmt", "LastTradeDateRaw", "LastTradeDateFmt", "LastTradeDateLongFmt", "ContractSize",
                "Currency", "BidRaw", "BidFmt", "BidLongFmt", "LastPriceRaw", "LastPriceFmt", "LastPriceLongFmt"};

        public BulkLoadCallPuts() : base(ColumnNames)
        {

        }

        public DataTable LoadDataTableWithSymbols(IEnumerable<CallPuts> dStats, DataTable dt)
        {
            //for( int i = 0; i< ColumnNames.Length; i++)
            //{
            //    dt.Columns[ColumnNames[i]].SetOrdinal(i);
            //}

            foreach (var value in dStats)
            {
                var sValue = value.QuoteId + "^" + value.CallOrPut + "^" + value.Symbol + "^" + value.ExpirationRaw + "^" + value.StrikeRaw + "^"  + value.ExpirationFmt + "^" + value.ExpirationLongFmt + "^"
                + value.StrikeFmt + "^" + value.StrikeLongFmt + "^" + value.Date + "^" + value.PercentChangeRaw + "^" + value.PercentChangeFmt + "^" + value.PercentChangeLongFmt + "^" 
                + value.OpenInterestRaw + "^" + value.OpenInterestFmt + "^" + value.OpenInterestLongFmt + "^"+ value.ChangeRaw + "^" + value.ChangeFmt + "^" + value.ChangeLongFmt + "^" + value.InTheMoney + "^" 
                + value.ImpliedVolatilityRaw + "^" + value.ImpliedVolatilityFmt + "^" + value.ImpliedVolatilityLongFmt + "^" + value.VolumeRaw + "^" + value.VolumeFmt + "^"+ value.VolumeLongFmt + "^" 
                + value.ContractSymbol + "^" + value.AskRaw + "^" + value.AskFmt + "^" + value.AskLongFmt + "^" + value.LastTradeDateRaw + "^" + value.LastTradeDateFmt + "^" + value.LastTradeDateLongFmt + "^" 
                + value.ContractSize + "^" + value.Currency + "^" + value.BidRaw + "^" + value.BidFmt + "^" + value.BidLongFmt + "^" + value.LastPriceRaw + "^" + value.LastPriceFmt + "^" + value.LastPriceLongFmt;

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
        ~BulkLoadCallPuts()
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
