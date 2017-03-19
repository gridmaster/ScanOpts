using ORMService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.JsonModels;
using Core.ORMModels;

namespace ORMService.Models
{
    public class OptionRepo : IRepository<Option>
    {
        public void Add(Option entity)
        {

            throw new NotImplementedException();
        }

        public void Delete(Option entity)
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

        public Option Single(Expression<Func<Option, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}