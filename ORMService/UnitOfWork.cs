using Core.Interface;
using Core.ORMModels;
using ORMService.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMService
{
    public class UnitOfWork : IDisposable
    {
        private ScanOptsContext context = new ScanOptsContext();
        private Repository<BollingerBands> bollingerBandsRepository;
        //  private GenericRepository<Course> courseRepository;
        ILogger _log;

        public UnitOfWork(ILogger log)
        {
            _log = log;
        }

        public Repository<BollingerBands> BollingerBandsRepository
        {
            get
            {
                return bollingerBandsRepository ?? new Repository<BollingerBands>(context, _log);
            }
        }

        //public GenericRepository<Course> CourseRepository
        //{
        //    get
        //    {
        //        return this.courseRepository ?? new GenericRepository<Course>(context);
        //    }
        //}

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
