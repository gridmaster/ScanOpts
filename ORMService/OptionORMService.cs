using Core.Interface;
using Core.JsonModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ORMService
{
    public class OptionORMService : IOptionORMService
    {
        public List<CallPut> ExtractCallsAndPutsFromOptionChain(string symbol, int newId, List<Straddles> straddles)
        {
            List<CallPut> callputs = new List<CallPut>();

            try
            {
                foreach (Straddles option in straddles)
                {
                    if (option.call != null)
                    {
                        CallPut call = new CallPut
                        {
                            CallOrPut = "Call",
                            Symbol = symbol,
                            LastTradeDateRaw = option.call.lastTradeDate.raw,
                            LastTradeDateFmt = option.call.lastTradeDate.fmt,
                            LastTradeDateLongFmt = option.call.lastTradeDate.longFmt,
                            StrikeRaw = option.call.strike.raw,
                            StrikeFmt = option.call.strike.fmt,
                            StrikeLongFmt = option.call.strike.longFmt,
                            AskRaw = option.call.Ask.raw,
                            AskFmt = option.call.Ask.fmt,
                            AskLongFmt = option.call.Ask.longFmt,
                            BidRaw = option.call.bid.raw,
                            BidFmt = option.call.bid.fmt,
                            BidLongFmt = option.call.bid.longFmt,
                            ChangeRaw = option.call.change.raw,
                            ChangeFmt = option.call.change.fmt,
                            ChangeLongFmt = option.call.change.longFmt,
                            contractSize = option.call.contractSize,
                            contractSymbol = option.call.contractSymbol,
                            currency = option.call.currency,
                            Date = DateTime.Now,
                            ExpirationRaw = option.call.expiration.raw,
                            ExpirationFmt = option.call.expiration.fmt,
                            ExpirationLongFmt = option.call.expiration.longFmt,
                            ImpliedVolatilityRaw = option.call.impliedVolatility.raw,
                            ImpliedVolatilityFmt = option.call.impliedVolatility.fmt,
                            ImpliedVolatilityLongFmt = option.call.impliedVolatility.longFmt,
                            inTheMoney = option.call.inTheMoney,
                            lastPrice = option.call.lastPrice,
                            OpenInterestRaw = option.call.openInterest.raw,
                            OpenInterestFmt = option.call.openInterest.fmt,
                            OpenInterestLongFmt = option.call.openInterest.longFmt,
                            PercentChangeRaw = option.call.PercentChange.raw,
                            PercentChangeFmt = option.call.PercentChange.fmt,
                            PercentChangeLongFmt = option.call.PercentChange.longFmt,
                            QuoteId = newId
                        };
                        callputs.Add(call);
                    }

                    if (option.put != null)
                    {
                        CallPut put = new CallPut
                        {
                            CallOrPut = "Put",
                            Symbol = symbol,
                            LastTradeDateRaw = option.put.lastTradeDate.raw,
                            LastTradeDateFmt = option.put.lastTradeDate.fmt ?? "",
                            LastTradeDateLongFmt = option.put.lastTradeDate.longFmt ?? "",
                            StrikeRaw = option.put.strike.raw,
                            StrikeFmt = option.put.strike.fmt ?? "",
                            StrikeLongFmt = option.put.strike.longFmt ?? "",
                            AskRaw = option.put.Ask.raw,
                            AskFmt = option.put.Ask.fmt ?? "",
                            AskLongFmt = option.put.Ask.longFmt ?? "",
                            BidRaw = option.put.bid.raw,
                            BidFmt = option.put.bid.fmt ?? "",
                            BidLongFmt = option.put.bid.longFmt ?? "",
                            ChangeRaw = option.put.change.raw,
                            ChangeFmt = option.put.change.fmt ?? "",
                            ChangeLongFmt = option.put.change.longFmt ?? "",
                            contractSize = option.put.contractSize,
                            contractSymbol = option.put.contractSymbol,
                            currency = option.put.currency,
                            Date = DateTime.Now,
                            ExpirationRaw = option.put.expiration.raw,
                            ExpirationFmt = option.put.expiration.fmt ?? "",
                            ExpirationLongFmt = option.put.expiration.longFmt ?? "",
                            ImpliedVolatilityRaw = option.put.impliedVolatility.raw,
                            ImpliedVolatilityFmt = option.put.impliedVolatility.fmt ?? "",
                            ImpliedVolatilityLongFmt = option.put.impliedVolatility.longFmt ?? "",
                            inTheMoney = option.put.inTheMoney,
                            lastPrice = option.put.lastPrice,
                            OpenInterestRaw = option.put.openInterest.raw,
                            OpenInterestFmt = option.put.openInterest.fmt ?? "",
                            OpenInterestLongFmt = option.put.openInterest.longFmt ?? "",
                            PercentChangeRaw = option.put.PercentChange.raw,
                            PercentChangeFmt = option.put.PercentChange.fmt ?? "",
                            PercentChangeLongFmt = option.put.PercentChange.longFmt ?? "",
                            QuoteId = newId
                        };
                        callputs.Add(put);
                    }
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().ErrorFormat("OptionORMService - ExtractCallsAndPutsFromOptionChain on symbol {0} - Error: {1}{2}", symbol, ex.Message, Environment.NewLine);
            }

            return callputs;
        }

        #region Implement IRepository

        public Option Single(Expression<Func<Option, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Option> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Option> Query(Expression<Func<Option, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Add(Option entity)
        {
            using (var db = new ScanOptsContext())
            {
                db.Option.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Option entity)
        {
            throw new NotImplementedException();
        }

        #endregion Implement IRepository

    }
}
