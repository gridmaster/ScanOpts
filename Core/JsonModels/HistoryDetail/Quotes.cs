using System;
using System.Collections.Generic;

namespace Core.JsonModels.HistoryDetail
{
    public class Quotes
    {
        public string symbol { get; set; }
        public string exchangeName { get; set; }
        public string instrumentType { get; set; }
        public int? timestamp { get; set; }
        public DateTime date { get; set; }
        public decimal? close { get; set; }
        public decimal? high { get; set; }
        public decimal? low { get; set; }
        public decimal? open { get; set; }
        public decimal? volume { get; set; }
        public decimal? unadjhigh { get; set; }
        public decimal? unadjlow { get; set; }
        public decimal? unadjclose { get; set; }
        public decimal? unadjopen { get; set; }
    }
}
