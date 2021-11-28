namespace ApiTrading.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using DbContext;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApiTradingDatabaseContext Context;

        public GenericRepository(ApiTradingDatabaseContext context)
        {
            Context = context;
        }

        public virtual T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }

        public virtual async Task Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual async Task AddRange(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
        }

        public virtual async Task Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual async Task RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }

        public virtual IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public virtual async Task Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public async Task SaveChangeAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}