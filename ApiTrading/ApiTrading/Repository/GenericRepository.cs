namespace ApiTrading.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;
    using DbContext;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApiTradingDatabaseContext _context;

        public GenericRepository(ApiTradingDatabaseContext context)
        {
            _context = context;
        }

        public virtual T  GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public virtual async Task Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual async Task AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public virtual async  Task Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual async Task RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public virtual IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
          return  _context.Set<T>().Where(expression).AsNoTracking();
        }

        public virtual async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}