﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Root.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQueryable();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> Query(Expression<Func<T, bool>> where);
        IEnumerable<T> ExecWithStoreProcedureWithType(string query, params object[] parameters);
    }
}
