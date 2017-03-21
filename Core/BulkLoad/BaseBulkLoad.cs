using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using SymbolService.Logs;

namespace Core.BulkLoad
{
    public class BaseBulkLoad
    {
        private string[] ColumnNames;

        public BaseBulkLoad(string[] columnNames)
        {
            ColumnNames = columnNames;
        }

        public DataTable ConfigureDataTable()
        {
            var dt = new DataTable();

            for (int i = 0; i < ColumnNames.Length; i++)
            {
                dt.Columns.Add(new DataColumn());
                dt.Columns[i].ColumnName = ColumnNames[i];
            }
            return dt;
        }

        public bool BulkCopy<T>(DataTable dt, String context)
        {
            string connString = ConfigurationManager.ConnectionStrings[context].ConnectionString;

            string tableName = typeof(T).Name;
            bool success = false;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connString))
            {
                for (int i = 0; i < ColumnNames.Length; i++)
                    bulkCopy.ColumnMappings.Add(i, ColumnNames[i]);

                bulkCopy.BulkCopyTimeout = 600; // in seconds 
                bulkCopy.DestinationTableName = tableName;
                try
                {
                    bulkCopy.WriteToServer(dt);
                    success = true;
                }
                catch (Exception ex)
                {
                    //logger.Error(string.Format("BaseBulkLoad - BulkCopy<{0}> Bulk load error: {1}", tableName, ex.Message));
                }

                bulkCopy.Close();
            }
            return success;
        }

        public void BulkCopy<T>(DataTable dt)
        {
            string connString = ConfigurationManager.ConnectionStrings["ScanOptsContext"].ConnectionString;

            string tableName = typeof(T).Name;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connString))
            {
                for (int i = 0; i < ColumnNames.Length; i++)
                    bulkCopy.ColumnMappings.Add(i, ColumnNames[i]);

                bulkCopy.BulkCopyTimeout = 60; // in seconds 
                bulkCopy.DestinationTableName = tableName;
                try
                {
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    //Log.WriteLog(new LogEvent("BulkLoadSector - BulkCopy<" + tableName + ">", "Bulk load error: " + ex.Message));
                }
                bulkCopy.Close();
            }
        }
    }
}