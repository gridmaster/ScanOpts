using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.ORMModels;
using ORMService.Contracts;

namespace ORMService.Models
{
    class StatisticRepo : IRepository<Statistics>
    {
        public void Add(Statistics entity)
        {            
            throw new NotImplementedException();
        }

        public void Delete(Statistics entity)
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

        public Option Single(Expression<Func<Statistics, bool>> filter)
        {
            throw new NotImplementedException();
        }

        Statistics IRepository<Statistics>.Single(Expression<Func<Statistics, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}