using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<bool> Exists(int? id)
        {
            if (id == null) return false;

            var entity = await Get(id);
            if (entity == null) return false;
                       
            return true;
        }

        public void Add(T entity)
        {
            if (entity == null) return;
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            if (entities == null) return;
            dbSet.AddRange(entities);
        }

        public async Task<T> Get(int? id)
        {
            if (id == null) return null;
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
           
            return await query.FirstOrDefaultAsync();
        }

        public async Task Remove(int? id)
        {
            if (id == null) return;
            Remove(await Get(id));
        }

        public void Remove(T entity)
        {
            if (entity == null) return;
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null) return;
            dbSet.RemoveRange(entities);
        }
    }
}
