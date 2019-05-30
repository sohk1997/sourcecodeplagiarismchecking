using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Root.Data.Infrastructure
{
    public class Repository<T> : Disposable, IDisposable, IRepository<T> where T : class
    {
        private TestContext _dataContext;
        private readonly DbSet<T> _dbset;

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected TestContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }

        public Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>(); 
        }

        public void Add(T entity)
        {
            try
            {
                _dbset.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _dbset.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            try
            {
                IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
                _dbset.RemoveRange(objects);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> ExecWithStoreProcedureWithType(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbset.AsQueryable();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).AsEnumerable();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
