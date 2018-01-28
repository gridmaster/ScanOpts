using Core;
using Core.Interface;
using ORMService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMService
{
    public class AnalyticsORMService : BaseService, IAnalyticsORMService
    {
        #region Constructors

        public AnalyticsORMService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors


        //#region Implement IRepository

        //public Statistics Single(Expression<Func<Statistics, bool>> filter)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Statistics> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Statistics> Query(Expression<Func<Statistics, bool>> filter)
        //{
        //    throw new NotImplementedException();
        //}

        //public int Add(Statistics entity)
        //{
        //    try
        //    {
        //        using (var db = new ScanOptsContext())
        //        {
        //            db.Statistics.Add(entity);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IOCContainer.Instance.Get<ILogger>().ErrorFormat("StatisticORMService - Add<{0}> - Add error: {1}{2}", entity.Symbol, ex.Message, Environment.NewLine);
        //    }

        //    return entity.Id;

        //}

        //public void Delete(Statistics entity)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion Implement IRepository
    }
}
