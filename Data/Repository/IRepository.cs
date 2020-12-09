﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IRepository<T> where T:class
    {
        Task<bool> Exists(int? id);

        Task<T> Get(int? id);

        Task<T> GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null);

        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null);

        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        Task Remove(int? id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
