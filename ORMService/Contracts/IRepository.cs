using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ORMService.Contracts
{
    public interface IRepository<T> where T : class
    {
        T Single(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
        IEnumerable<T> Query(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Delete(T entity);
    }
}


