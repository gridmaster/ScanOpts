using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Interface;
using Core.ORMModels;
using DIContainer;
using ORMService.Context;
using Core;

namespace ORMService
{
    public class OptionORMService : BaseService, IOptionORMService
    {
        #region Constructors

        public OptionORMService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        public List<CallPuts> ExtractCallsAndPutsFromOptionChain(string symbol, int newId, List<Straddles> straddles)
        {
            List<CallPuts> callputs = new List<CallPuts>();

            try
            {
                foreach (Straddles option in straddles)
                {
                    if (option.call != null)
                    {
                        CallPuts call = new CallPuts
                        {
                            QuoteId = newId,
                            CallOrPut = "Call",
                            Symbol = symbol,
                            ExpirationRaw = option.call.expiration.raw,
                            StrikeRaw = option.call.strike.raw,
                            ExpirationFmt = option.call.expiration.fmt ?? "",
                            ExpirationLongFmt = option.call.expiration.longFmt ?? "",
                            StrikeFmt = option.call.strike.fmt ?? "",
                            StrikeLongFmt = option.call.strike.longFmt ?? "",
                            Date = DateTime.Now,
                            PercentChangeRaw = option.call.PercentChange.raw,
                            PercentChangeFmt = option.call.PercentChange.fmt ?? "",
                            PercentChangeLongFmt = option.call.PercentChange.longFmt ?? "",
                            OpenInterestRaw = option.call.openInterest.raw,
                            OpenInterestFmt = option.call.openInterest.fmt ?? "",
                            OpenInterestLongFmt = option.call.openInterest.longFmt ?? "",
                            ChangeRaw = option.call.change.raw,
                            ChangeFmt = option.call.change.fmt ?? "",
                            ChangeLongFmt = option.call.change.longFmt ?? "",
                            InTheMoney = option.call.inTheMoney,
                            ImpliedVolatilityRaw = option.call.impliedVolatility.raw,
                            ImpliedVolatilityFmt = option.call.impliedVolatility.fmt ?? "",
                            ImpliedVolatilityLongFmt = option.call.impliedVolatility.longFmt ?? "",
                            VolumeRaw = option.call.volume.raw,
                            VolumeFmt = option.call.volume.fmt ?? "",
                            VolumeLongFmt = option.call.volume.longFmt ?? "",
                            ContractSymbol = option.call.contractSymbol ?? "",
                            AskRaw = option.call.Ask.raw,
                            AskFmt = option.call.Ask.fmt ?? "",
                            AskLongFmt = option.call.Ask.longFmt ?? "",
                            LastTradeDateRaw = option.call.lastTradeDate.raw,
                            LastTradeDateFmt = option.call.lastTradeDate.fmt ?? "",
                            LastTradeDateLongFmt = option.call.lastTradeDate.longFmt ?? "",
                            ContractSize = option.call.contractSize ?? "",
                            Currency = option.call.currency ?? "",
                            BidRaw = option.call.bid.raw,
                            BidFmt = option.call.bid.fmt ?? "",
                            BidLongFmt = option.call.bid.longFmt ?? "",
                            LastPriceRaw = option.call.lastPrice.raw,
                            LastPriceFmt = option.call.lastPrice.fmt ?? "",
                            LastPriceLongFmt = option.call.lastPrice.longFmt ?? ""
                        };
                        callputs.Add(call);
                    }

                    if (option.put != null)
                    {
                        CallPuts put = new CallPuts
                        {
                            QuoteId = newId,
                            CallOrPut = "Put",
                            Symbol = symbol,
                            ExpirationRaw = option.put.expiration.raw,
                            StrikeRaw = option.put.strike.raw,
                            ExpirationFmt = option.put.expiration.fmt ?? "",
                            ExpirationLongFmt = option.put.expiration.longFmt ?? "",
                            StrikeFmt = option.put.strike.fmt ?? "",
                            StrikeLongFmt = option.put.strike.longFmt ?? "",
                            Date = DateTime.Now,
                            PercentChangeRaw = option.put.PercentChange.raw,
                            PercentChangeFmt = option.put.PercentChange.fmt ?? "",
                            PercentChangeLongFmt = option.put.PercentChange.longFmt ?? "",
                            OpenInterestRaw = option.put.openInterest.raw,
                            OpenInterestFmt = option.put.openInterest.fmt ?? "",
                            OpenInterestLongFmt = option.put.openInterest.longFmt ?? "",
                            ChangeRaw = option.put.change.raw,
                            ChangeFmt = option.put.change.fmt ?? "",
                            ChangeLongFmt = option.put.change.longFmt ?? "",
                            InTheMoney = option.put.inTheMoney,
                            ImpliedVolatilityRaw = option.put.impliedVolatility.raw,
                            ImpliedVolatilityFmt = option.put.impliedVolatility.fmt ?? "",
                            ImpliedVolatilityLongFmt = option.put.impliedVolatility.longFmt ?? "",
                            VolumeRaw = option.put.volume.raw,
                            VolumeFmt = option.put.volume.fmt ?? "",
                            VolumeLongFmt = option.put.volume.longFmt ?? "",
                            ContractSymbol = option.put.contractSymbol ?? "",
                            AskRaw = option.put.Ask.raw,
                            AskFmt = option.put.Ask.fmt ?? "",
                            AskLongFmt = option.put.Ask.longFmt ?? "",
                            LastTradeDateRaw = option.put.lastTradeDate.raw,
                            LastTradeDateFmt = option.put.lastTradeDate.fmt ?? "",
                            LastTradeDateLongFmt = option.put.lastTradeDate.longFmt ?? "",
                            ContractSize = option.put.contractSize ?? "",
                            Currency = option.put.currency ?? "",
                            BidRaw = option.put.bid.raw,
                            BidFmt = option.put.bid.fmt ?? "",
                            BidLongFmt = option.put.bid.longFmt ?? "",
                            LastPriceRaw = option.put.lastPrice.raw,
                            LastPriceFmt = option.put.lastPrice.fmt ?? "",
                            LastPriceLongFmt = option.put.lastPrice.longFmt ?? ""
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
