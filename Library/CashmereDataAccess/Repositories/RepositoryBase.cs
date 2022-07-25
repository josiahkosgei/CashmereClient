using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{

    public class RepositoryBase<T> : IAsyncRepository<T> where T : class, new()

    {
        protected readonly DepositorDBContext DbContext;
        public RepositoryBase(DepositorDBContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public virtual async Task<T> AddAsync(T entity)
        {
            using (var dbContextTransaction = await DbContext.Database.BeginTransactionAsync())
            {
                DbContext.Set<T>().Add(entity);
                await DbContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();
            }
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            using (var dbContextTransaction = await DbContext.Database.BeginTransactionAsync())
            {
                DbContext.Set<T>().Remove(entity);
                await DbContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            var result = await DbContext.Set<T>().FindAsync(id);
            return result != null;
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var result = await DbContext.Set<T>().ToListAsync();
            return result;

        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>().Where(predicate).ToListAsync();

        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);

        }
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await DbContext.Set<T>().FindAsync(id);

        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            using (var dbContextTransaction = await DbContext.Database.BeginTransactionAsync())
            {
                DbContext.Entry(entity).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();
            }
            return entity;
        }

    }
}
