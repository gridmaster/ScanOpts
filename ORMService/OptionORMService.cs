using Core.Interface;
using Core.Models;
using ORMService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ORMService
{
    public class OptionORMService : IRepository<Option>, IOptionORMService
    {
        public void ExtractAndSaveOptionChainForExpireDate(List<Straddles> straddles)
        {
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
