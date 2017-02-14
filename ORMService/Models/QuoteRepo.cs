using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ORMService.Contracts;
using ScanOpts.Core.Models;

namespace ORMService.Models
{
    class QuoteRepo : IRepository<Quote>
    {
        public void Add(Quote entity)
        {

            throw new NotImplementedException();
        }

        public void Delete(Quote entity)
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

        public Option Single(Expression<Func<Quote, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
