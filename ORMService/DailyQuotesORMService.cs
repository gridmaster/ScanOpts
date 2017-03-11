using Core.Interface;
using Core.JsonModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;

namespace ORMService
{
    public class DailyQuotesORMService : IDailyQuotesORMService
    {
        public void Add(DailyQuotes entity)
        {
            using (var db = new ScanOptsContext())
            {
                db.DailyQuotes.Add(entity);

                IOCContainer.Instance.Get<ILogger>().Info("DailyQuotesORMService - Add Saving Changes");
                db.SaveChanges();
            }
        }

        public void AddMany(List<DailyQuotes> quotes)
        {
            using (var db = new ScanOptsContext())
            {
                try
                {
                    foreach (DailyQuotes quote in quotes)
                    {
                        //IOCContainer.Instance.Get<ILogger>().InfoFormat("DailyQuotesORMService - AddMany {0}", quote.Symbol);
                        db.DailyQuotes.Add(quote);
                    }
                    IOCContainer.Instance.Get<ILogger>().Info("DailyQuotesORMService - AddMany Saving Changes");

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    IOCContainer.Instance.Get<ILogger>().ErrorFormat("DailyQuotesORMService - AddMany error: {1}{2}", ex.Message, Environment.NewLine);
                }
            }
        }
    }
}
