using Core.Interface;
using Core.JsonModels;
using ORMService.Context;
using System.Collections.Generic;

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

        public void AddMany(List<CallPut> callputs)
        {
            using (var db = new ScanOptsContext())
            {
                foreach (CallPut callput in callputs)
                {
                    db.CallPut.Add(callput);
                }
                db.SaveChanges();
            }
        }
    }
}
