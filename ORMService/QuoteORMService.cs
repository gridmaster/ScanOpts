using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Interface;
using Core.JsonModels;
using ORMService.Contracts;
using ORMService.Context;

namespace ORMService
{
    public class QuoteORMService : IQuoteORMService
    {

        public Quote ExtractAndSaveQuoteFromOptionChain(JsonResult optionChain)
        {
            Result result = optionChain.OptionChain.Result[0];
            Quote quote = new Quote
            {
                ExpirationDates = String.Join(";", result.ExpirationDates),
                Strikes = String.Join(";", result.Strikes),
                Ask = result.Quote.Ask,
                AskSize = result.Quote.AskSize,
                AverageDailyVolume10Day = result.Quote.AverageDailyVolume10Day,
                AverageDailyVolume3Month = result.Quote.AverageDailyVolume3Month,
                Bid = result.Quote.Bid,
                BidSize = result.Quote.BidSize,
                Currency = result.Quote.Currency,
                Date = DateTime.Now,
                Exchange = result.Quote.Exchange,
                ExchangeTimezoneName = result.Quote.ExchangeTimezoneName,
                ExchangeTimezoneShortName = result.Quote.ExchangeTimezoneShortName,
                FiftyDayAverage = result.Quote.FiftyDayAverage,
                FiftyDayAverageChange = result.Quote.FiftyDayAverageChange,
                FiftyDayAverageChangePercent = result.Quote.FiftyDayAverageChangePercent,
                FiftyTwoWeekHigh = result.Quote.FiftyTwoWeekHigh,
                FiftyTwoWeekHighChange = result.Quote.FiftyTwoWeekHighChange,
                FiftyTwoWeekHighChangePercent = result.Quote.FiftyTwoWeekHighChangePercent,
                FiftyTwoWeekLow = result.Quote.FiftyTwoWeekLow,
                FiftyTwoWeekLowChange = result.Quote.FiftyTwoWeekLowChange,
                FiftyTwoWeekLowChangePercent = result.Quote.FiftyTwoWeekLowChangePercent,
                FullExchangeName = result.Quote.FullExchangeName,
                GmtOffSetMilliseconds = result.Quote.GmtOffSetMilliseconds,
                HasMiniOptions = result.Quote.HasMiniOptions,
                Id = result.Quote.Id,
                LongName = result.Quote.LongName,
                Market = result.Quote.Market,
                MarketCap = result.Quote.MarketCap,
                MarketState = result.Quote.MarketState,
                MessageBoardId = result.Quote.MessageBoardId,
                PreMarketChange = result.Quote.PreMarketChange,
                PreMarketChangePercent = result.Quote.PreMarketChangePercent,
                PreMarketPrice = result.Quote.PreMarketPrice,
                PreMarketTime = result.Quote.PreMarketTime,
                PostMarketChange = result.Quote.PostMarketChange,
                PostMarketChangePercent = result.Quote.PostMarketChangePercent,
                PostMarketPrice = result.Quote.PostMarketPrice,
                PostMarketTime = result.Quote.PostMarketTime,
                QuoteSourceName = result.Quote.QuoteSourceName,
                QuoteType = result.Quote.QuoteType,
                RegularMarketChange = result.Quote.RegularMarketChange,
                RegularMarketChangePercent = result.Quote.RegularMarketChangePercent,
                RegularMarketDayHigh = result.Quote.RegularMarketDayHigh,
                RegularMarketDayLow = result.Quote.RegularMarketDayLow,
                RegularMarketOpen = result.Quote.RegularMarketOpen,
                RegularMarketPreviousClose = result.Quote.RegularMarketPreviousClose,
                RegularMarketPrice = result.Quote.RegularMarketPrice,
                RegularMarketTime = result.Quote.RegularMarketTime,
                RegularMarketVolume = result.Quote.RegularMarketVolume,
                SharesOutstanding = result.Quote.SharesOutstanding,
                ShortName = result.Quote.ShortName,
                SourceInterval = result.Quote.SourceInterval,
                Symbol = result.Quote.Symbol,
                TwoHundredDayAverage = result.Quote.TwoHundredDayAverage,
                TwoHundredDayAverageChange = result.Quote.TwoHundredDayAverageChange,
                TwoHundredDayAverageChangePercent = result.Quote.TwoHundredDayAverageChangePercent
            };

            return quote;
        }

        #region Implement IRepository

        public Quote Single(Expression<Func<Quote, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quote> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quote> Query(Expression<Func<Quote, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public int Add(Quote entity)
        {
            try
            {
                using (var db = new ScanOptsContext())
                {
                    db.Quotes.Add(entity);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var i = 0;
                i++;
            }

            return entity.Id;

        }

        public void Delete(Quote entity)
        {
            throw new NotImplementedException();
        }

        #endregion Implement IRepository
    }
}
