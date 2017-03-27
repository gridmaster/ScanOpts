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
                            ExpirationRaw = option.call.Expiration.raw,
                            StrikeRaw = option.call.Strike.raw,
                            ExpirationFmt = option.call.Expiration.fmt ?? "",
                            ExpirationLongFmt = option.call.Expiration.longFmt ?? "",
                            StrikeFmt = option.call.Strike.fmt ?? "",
                            StrikeLongFmt = option.call.Strike.longFmt ?? "",
                            Date = DateTime.Now,
                            PercentChangeRaw = option.call.PercentChange.raw,
                            PercentChangeFmt = option.call.PercentChange.fmt ?? "",
                            PercentChangeLongFmt = option.call.PercentChange.longFmt ?? "",
                            OpenInterestRaw = option.call.OpenInterest.raw,
                            OpenInterestFmt = option.call.OpenInterest.fmt ?? "",
                            OpenInterestLongFmt = option.call.OpenInterest.longFmt ?? "",
                            ChangeRaw = option.call.Change.raw,
                            ChangeFmt = option.call.Change.fmt ?? "",
                            ChangeLongFmt = option.call.Change.longFmt ?? "",
                            InTheMoney = option.call.InTheMoney,
                            ImpliedVolatilityRaw = option.call.ImpliedVolatility.raw,
                            ImpliedVolatilityFmt = option.call.ImpliedVolatility.fmt ?? "",
                            ImpliedVolatilityLongFmt = option.call.ImpliedVolatility.longFmt ?? "",
                            VolumeRaw = option.call.Volume.raw,
                            VolumeFmt = option.call.Volume.fmt ?? "",
                            VolumeLongFmt = option.call.Volume.longFmt ?? "",
                            ContractSymbol = option.call.ContractSymbol ?? "",
                            AskRaw = option.call.Ask.raw,
                            AskFmt = option.call.Ask.fmt ?? "",
                            AskLongFmt = option.call.Ask.longFmt ?? "",
                            LastTradeDateRaw = option.call.LastTradeDate.raw,
                            LastTradeDateFmt = option.call.LastTradeDate.fmt ?? "",
                            LastTradeDateLongFmt = option.call.LastTradeDate.longFmt ?? "",
                            ContractSize = option.call.ContractSize ?? "",
                            Currency = option.call.Currency ?? "",
                            BidRaw = option.call.Bid.raw,
                            BidFmt = option.call.Bid.fmt ?? "",
                            BidLongFmt = option.call.Bid.longFmt ?? "",
                            LastPriceRaw = option.call.LastPrice.raw,
                            LastPriceFmt = option.call.LastPrice.fmt ?? "",
                            LastPriceLongFmt = option.call.LastPrice.longFmt ?? ""
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
                            ExpirationRaw = option.put.Expiration.raw,
                            StrikeRaw = option.put.Strike.raw,
                            ExpirationFmt = option.put.Expiration.fmt ?? "",
                            ExpirationLongFmt = option.put.Expiration.longFmt ?? "",
                            StrikeFmt = option.put.Strike.fmt ?? "",
                            StrikeLongFmt = option.put.Strike.longFmt ?? "",
                            Date = DateTime.Now,
                            PercentChangeRaw = option.put.PercentChange.raw,
                            PercentChangeFmt = option.put.PercentChange.fmt ?? "",
                            PercentChangeLongFmt = option.put.PercentChange.longFmt ?? "",
                            OpenInterestRaw = option.put.OpenInterest.raw,
                            OpenInterestFmt = option.put.OpenInterest.fmt ?? "",
                            OpenInterestLongFmt = option.put.OpenInterest.longFmt ?? "",
                            ChangeRaw = option.put.Change.raw,
                            ChangeFmt = option.put.Change.fmt ?? "",
                            ChangeLongFmt = option.put.Change.longFmt ?? "",
                            InTheMoney = option.put.InTheMoney,
                            ImpliedVolatilityRaw = option.put.ImpliedVolatility.raw,
                            ImpliedVolatilityFmt = option.put.ImpliedVolatility.fmt ?? "",
                            ImpliedVolatilityLongFmt = option.put.ImpliedVolatility.longFmt ?? "",
                            VolumeRaw = option.put.Volume.raw,
                            VolumeFmt = option.put.Volume.fmt ?? "",
                            VolumeLongFmt = option.put.Volume.longFmt ?? "",
                            ContractSymbol = option.put.ContractSymbol ?? "",
                            AskRaw = option.put.Ask.raw,
                            AskFmt = option.put.Ask.fmt ?? "",
                            AskLongFmt = option.put.Ask.longFmt ?? "",
                            LastTradeDateRaw = option.put.LastTradeDate.raw,
                            LastTradeDateFmt = option.put.LastTradeDate.fmt ?? "",
                            LastTradeDateLongFmt = option.put.LastTradeDate.longFmt ?? "",
                            ContractSize = option.put.ContractSize ?? "",
                            Currency = option.put.Currency ?? "",
                            BidRaw = option.put.Bid.raw,
                            BidFmt = option.put.Bid.fmt ?? "",
                            BidLongFmt = option.put.Bid.longFmt ?? "",
                            LastPriceRaw = option.put.LastPrice.raw,
                            LastPriceFmt = option.put.LastPrice.fmt ?? "",
                            LastPriceLongFmt = option.put.LastPrice.longFmt ?? ""
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
