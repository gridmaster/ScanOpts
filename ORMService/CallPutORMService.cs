using Core.Interface;
using Core.JsonModels;
using DIContainer;
using ORMService.Context;
using System;
using System.Collections.Generic;
using Core.ORMModels;

namespace ORMService
{
    public class CallPutORMService : ICallPutORMService
    {
        public void Add(Option entity)
        {
            using (var db = new ScanOptsContext())
            {
                db.Option.Add(entity);
                db.SaveChanges();
            }
        }

        public void AddMany(List<CallPuts> callputs)
        {
            using (var db = new ScanOptsContext())
            {
                try {
                    foreach (CallPuts callput in callputs)
                    {
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("CallPutORMService - AddMany {0}", callput.Symbol);
                        db.CallPut.Add(callput);
                    }
                    IOCContainer.Instance.Get<ILogger>().Info("CallPutORMService - AddMany Saving Changes");

                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    IOCContainer.Instance.Get<ILogger>().ErrorFormat("CallPutORMService - AddMany error: {1}{2}", ex.Message, Environment.NewLine);
                }
            }
        }
    }
}
