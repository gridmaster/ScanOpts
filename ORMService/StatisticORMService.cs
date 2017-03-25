using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core;
using Core.Interface;
using Core.JsonOptions;
using Core.ORMModels;
using DIContainer;
using ORMService.Context;


namespace ORMService
{
    public class StatisticORMService : BaseService, IStatisticORMService
    {
        #region Constructors

        public StatisticORMService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        public Statistics ExtractAndSaveStatisticFromOptionChain(JsonResult optionChain)
        {
            Result result = optionChain.OptionChain.Result[0];
            Statistics Statistics = new Statistics
            {
                ExpirationDates = String.Join(";", result.ExpirationDates),
                Strikes = String.Join(";", result.Strikes),
                Ask = result.Statistics.Ask,
                AskSize = result.Statistics.AskSize,
                AverageDailyVolume10Day = result.Statistics.AverageDailyVolume10Day,
                AverageDailyVolume3Month = result.Statistics.AverageDailyVolume3Month,
                Bid = result.Statistics.Bid,
                BidSize = result.Statistics.BidSize,
                Currency = result.Statistics.Currency,
                Date = DateTime.Now,
                Exchange = result.Statistics.Exchange,
                ExchangeTimezoneName = result.Statistics.ExchangeTimezoneName,
                ExchangeTimezoneShortName = result.Statistics.ExchangeTimezoneShortName,
                FiftyDayAverage = result.Statistics.FiftyDayAverage,
                FiftyDayAverageChange = result.Statistics.FiftyDayAverageChange,
                FiftyDayAverageChangePercent = result.Statistics.FiftyDayAverageChangePercent,
                FiftyTwoWeekHigh = result.Statistics.FiftyTwoWeekHigh,
                FiftyTwoWeekHighChange = result.Statistics.FiftyTwoWeekHighChange,
                FiftyTwoWeekHighChangePercent = result.Statistics.FiftyTwoWeekHighChangePercent,
                FiftyTwoWeekLow = result.Statistics.FiftyTwoWeekLow,
                FiftyTwoWeekLowChange = result.Statistics.FiftyTwoWeekLowChange,
                FiftyTwoWeekLowChangePercent = result.Statistics.FiftyTwoWeekLowChangePercent,
                FullExchangeName = result.Statistics.FullExchangeName,
                GmtOffSetMilliseconds = result.Statistics.GmtOffSetMilliseconds,
                HasMiniOptions = result.Statistics.HasMiniOptions,
                Id = result.Statistics.Id,
                LongName = result.Statistics.LongName,
                Market = result.Statistics.Market,
                MarketCap = result.Statistics.MarketCap,
                MarketState = result.Statistics.MarketState,
                MessageBoardId = result.Statistics.MessageBoardId,
                PreMarketChange = result.Statistics.PreMarketChange,
                PreMarketChangePercent = result.Statistics.PreMarketChangePercent,
                PreMarketPrice = result.Statistics.PreMarketPrice,
                PreMarketTime = result.Statistics.PreMarketTime,
                PostMarketChange = result.Statistics.PostMarketChange,
                PostMarketChangePercent = result.Statistics.PostMarketChangePercent,
                PostMarketPrice = result.Statistics.PostMarketPrice,
                PostMarketTime = result.Statistics.PostMarketTime,
                QuoteSourceName = result.Statistics.QuoteSourceName,
                QuoteType = result.Statistics.QuoteType,
                RegularMarketChange = result.Statistics.RegularMarketChange,
                RegularMarketChangePercent = result.Statistics.RegularMarketChangePercent,
                RegularMarketDayHigh = result.Statistics.RegularMarketDayHigh,
                RegularMarketDayLow = result.Statistics.RegularMarketDayLow,
                RegularMarketOpen = result.Statistics.RegularMarketOpen,
                RegularMarketPreviousClose = result.Statistics.RegularMarketPreviousClose,
                RegularMarketPrice = result.Statistics.RegularMarketPrice,
                RegularMarketTime = result.Statistics.RegularMarketTime,
                RegularMarketVolume = result.Statistics.RegularMarketVolume,
                SharesOutstanding = result.Statistics.SharesOutstanding,
                ShortName = result.Statistics.ShortName,
                SourceInterval = result.Statistics.SourceInterval,
                Symbol = result.Statistics.Symbol,
                TwoHundredDayAverage = result.Statistics.TwoHundredDayAverage,
                TwoHundredDayAverageChange = result.Statistics.TwoHundredDayAverageChange,
                TwoHundredDayAverageChangePercent = result.Statistics.TwoHundredDayAverageChangePercent
            };

            return Statistics;
        }

        #region Implement IRepository

        public Statistics Single(Expression<Func<Statistics, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Statistics> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Statistics> Query(Expression<Func<Statistics, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public int Add(Statistics entity)
        {
            try
            {
                using (var db = new ScanOptsContext())
                {
                    db.Statistics.Add(entity);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().ErrorFormat("StatisticORMService - Add<{0}> - Add error: {1}{2}", entity.Symbol, ex.Message, Environment.NewLine);
            }

            return entity.Id;

        }

        public void Delete(Statistics entity)
        {
            throw new NotImplementedException();
        }

        #endregion Implement IRepository
    }
}
