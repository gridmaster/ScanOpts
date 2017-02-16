using Core.Interface;
using Core.JsonModels;
using ORMService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ORMService
{
    public class OptionORMService : IRepository<Option>, IOptionORMService
    {
        public void ExtractAndSaveOptionChainForExpireDate(string symbol, List<Straddles> straddles)
        {
            foreach (Straddles option in straddles)
            {
                if (option.call != null)
                {
                    Call call = new Call
                    {
                        Symbol = symbol,
                        lastTradeDate = option.call.lastTradeDate,
                        strike = option.call.strike,
                        Ask = option.call.Ask,
                        bid = option.call.bid,
                        change = option.call.change,
                        contractSize = option.call.contractSize,
                        contractSymbol = option.call.contractSymbol,
                        currency = option.call.currency,
                        Date = DateTime.Now,
                        expiration = option.call.expiration,
                        impliedVolatility = option.call.impliedVolatility,
                        inTheMoney = option.call.inTheMoney,
                        lastPrice = option.call.lastPrice,
                        openInterest = option.call.openInterest,
                        PercentChange = option.call.PercentChange,
                        QuoteID = option.call.QuoteID
                    };
                }

                if (option.put != null)
                {
                    Put put = new Put
                    {
                        Ask = option.put.Ask,
                        bid = option.put.bid,
                        change = option.put.change,
                        contractSize = option.put.contractSize,
                        contractSymbol = option.put.contractSymbol,
                        currency = option.put.currency,
                        Date = DateTime.Now,
                        expiration = option.put.expiration,
                        impliedVolatility = option.put.impliedVolatility,
                        inTheMoney = option.put.inTheMoney,
                        lastPrice = option.put.lastPrice,
                        lastTradeDate = option.put.lastTradeDate,
                        openInterest = option.put.openInterest,
                        PercentChange = option.put.PercentChange,
                        QuoteID = option.put.QuoteID,
                        strike = option.put.strike,
                        Symbol = option.put.Symbol,
                    };
                }                
            }            
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
            throw new NotImplementedException();
        }

        public void Delete(Option entity)
        {
            throw new NotImplementedException();
        }

        #endregion Implement IRepository

    }
}
