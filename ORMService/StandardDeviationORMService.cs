using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ORMService.Context;
using System.Configuration;
using Core.ORMModels;
using Core.Interface;

namespace ORMService
{
    public class StandardDeviationORMService : IStandardDeviationORMService
    {
        public List<StandardDeviations> RunStandardDeviation(List<string> symbols)
        {
            List<StandardDeviations> sdcList = new List<StandardDeviations>();

            try
            {
                foreach (string symbol in symbols)
                {
                   // logger.InfoFormat("Get {0} page", symbol);


                    string constr = ConfigurationManager.ConnectionStrings["ScanOptsContext"].ToString();

                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM BollingerBands WHERE [Symbol] = '{0}'", symbol), conn))
                        {
                            conn.Open();

                            DateTime dt = DateTime.Now;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    StandardDeviations sdc = new StandardDeviations
                                    {
                                        Symbol = reader["Symbol"].ToString(),
                                        Count = System.Convert.ToInt32(reader["Count"]),
                                        First = System.Convert.ToDouble(reader["First"]),
                                        Last = System.Convert.ToDouble(reader["Last"]),
                                        FirstDate = System.Convert.ToDateTime(reader["FirstDate"]),
                                        LastDate = System.Convert.ToDateTime(reader["LastDate"]),
                                        Slope = reader["Slope"].ToString(),
                                        Difference = System.Convert.ToDouble(reader["Difference"])
                                    };

                                    sdcList.Add(sdc);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
//                logger.Fatal("RunStandardDeviation: {0}", ex);
            }
            finally
            {
                //logger.Info("End - RunStandardDeviation");
                //logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }

            return sdcList;
        }

        public List<StandardDeviations> LoadStandardDeviations(List<string> symbols, double standardDeviation)
        {
            List<StandardDeviations> sdcList = new List<StandardDeviations>();
            List<BollingerBands> bbList = new List<BollingerBands>();
            try
            {
                foreach (string symbol in symbols)
                {
                    // logger.InfoFormat("Get {0} page", symbol);
                    bbList.Clear();

                    string constr = ConfigurationManager.ConnectionStrings["ScanOptsContext"].ToString();

                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM BollingerBands WHERE [Symbol] = '{0}'", symbol), conn))
                        {
                            conn.Open();

                            DateTime dt = DateTime.Now;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    BollingerBands bb = new BollingerBands
                                    {
                                        Symbol = reader["Symbol"].ToString(),
                                        Date = System.Convert.ToDateTime(reader["Date"]),
                                        Open = System.Convert.ToDouble(reader["Open"]),
                                        High = System.Convert.ToDouble(reader["High"]),
                                        Low = System.Convert.ToDouble(reader["Low"]),
                                        Close = System.Convert.ToDouble(reader["Close"]),
                                        SMA20 = System.Convert.ToDouble(reader["SMA20"]),
                                        StandardDeviation = System.Convert.ToDouble(reader["StandardDeviation"]),
                                        UpperBand = System.Convert.ToDouble(reader["UpperBand"]),
                                        LowerBand = System.Convert.ToDouble(reader["LowerBand"]),
                                        BandRatio = System.Convert.ToDouble(reader["BandRatio"]),
                                        Volume = System.Convert.ToDouble(reader["Volume"])
                                    };

                                    bbList.Add(bb);
                                }
                            }
                        }
                    }

                    StandardDeviations sdc = CreateStandardDeviationRecord(bbList, standardDeviation);
                    if (sdc != null)
                        sdcList.Add(sdc);
                    else
                        sdc = null;
                }
            }
            catch (Exception ex)
            {
                //                logger.Fatal("RunStandardDeviation: {0}", ex);
            }
            finally
            {
                //logger.Info("End - RunStandardDeviation");
                //logger.InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }

            return sdcList;
        }

        public StandardDeviations CreateStandardDeviationRecord(List<BollingerBands> bbList, double standardDeviation)
        {
            int count = bbList.Count(s => s.StandardDeviation < standardDeviation);

            if (count < 20) return null;

            double first = bbList.Where(s => s.StandardDeviation < standardDeviation).Select(s => s.Close).First();
            double last = bbList.Where(s => s.StandardDeviation < standardDeviation).OrderByDescending(s => s.Date).Select(s => s.Close).First();
            double difference = last - first;

            StandardDeviations sdc = new StandardDeviations()
            {
                Symbol = bbList[0].Symbol,
                Count = count,
                First = first,
                Last = last,
                FirstDate = bbList.Where(s => s.StandardDeviation < standardDeviation).Select(s => s.Date).First(),
                LastDate = bbList.Where(s => s.StandardDeviation < standardDeviation).OrderByDescending(s => s.Date).Select(s => s.Date).First(),
                Difference = difference,
                Slope = difference > 0 ? "Up" : "Down"
            };


            //DateTime firstDate = bbList.Where(s => s.StandardDeviation < standardDeviation).Select(s => s.Date).First();
            //DateTime lastDate = bbList.Where(s => s.StandardDeviation < standardDeviation).OrderByDescending(s => s.Date).Select(s => s.Date).First(); double difference = last - first;
            //string sloap = difference > 0 ? "Up" : "Down";
            return sdc;
        }
    }
}
