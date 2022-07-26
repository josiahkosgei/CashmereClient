using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{

    public class RepositoryBase<T> : IAsyncRepository<T> where T : class, new()

    {
        protected readonly DepositorContextFactory _dbContextFactory;
        protected readonly DepositorDBContext depositorDBContext;
        public RepositoryBase(IConfiguration configuration)
        {
            _dbContextFactory = new DepositorContextFactory(configuration);
        }


        public virtual async Task<T> AddAsync(T entity)
        {
            using (var dbContextFactory = _dbContextFactory.CreateDbContext(null))
            {
                dbContextFactory.Set<T>().Add(entity);
                dbContextFactory.SaveChanges();


            }
            return await Task.Run<T>(() => entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            using (var dbContextFactory = _dbContextFactory.CreateDbContext(null))
            {
                dbContextFactory.Set<T>().Remove(entity);
                dbContextFactory.SaveChanges();
            }
        }

        public async Task<bool> Exists(int id)
        {
            var dbContextFactory = _dbContextFactory.CreateDbContext(null);
            var result = dbContextFactory.Set<T>().Find(id);
            return await Task.Run(() => result != null);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var dbContextFactory = _dbContextFactory.CreateDbContext(null);

            var result = dbContextFactory.Set<T>().ToList();
            return await Task.Run<IReadOnlyList<T>>(() => result);
        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var dbContextFactory = _dbContextFactory.CreateDbContext(null);
            var result = dbContextFactory.Set<T>().Where(predicate).ToList();
            return await Task.Run<IReadOnlyList<T>>(() => result);

        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var dbContextFactory = _dbContextFactory.CreateDbContext(null);
            var result = dbContextFactory.Set<T>().Find(id);
            return await Task.Run<T>(() => result);

        }
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var dbContextFactory = _dbContextFactory.CreateDbContext(null);
            var result = dbContextFactory.Set<T>().Find(id);
            return await Task.Run<T>(() => result);

        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            using (var dbContextFactory = _dbContextFactory.CreateDbContext(null))
            {
                dbContextFactory.Entry(entity).State = EntityState.Modified;
                dbContextFactory.SaveChanges();


            }
            return await Task.Run<T>(() => entity);
        }

    }
}
