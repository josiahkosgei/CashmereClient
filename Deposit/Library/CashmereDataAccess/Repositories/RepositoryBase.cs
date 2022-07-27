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

        protected readonly DepositorDBContext _depositorDBContext;

        public RepositoryBase(IConfiguration configuration)
        {
            _dbContextFactory = new DepositorContextFactory(configuration);
            _depositorDBContext = _dbContextFactory.CreateDbContext(null);
        }


        public virtual T AddAsync(T entity)
        {
          
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                dbContext.Set<T>().Add(entity);
                dbContext.SaveChanges();

            }
            return entity;
        }

        public virtual void DeleteAsync(T entity)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();

            }

        }

        public bool Exists(int id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.Set<T>().Find(id);
            return result != null;

            }
        }

        public virtual IReadOnlyList<T> GetAllAsync()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.Set<T>().ToList();
            return result;

            }
        }

        public virtual IReadOnlyList<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.Set<T>().Where(predicate).ToList();
            return result;

            }

        }

        public virtual T GetByIdAsync(int id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.Set<T>().Find(id);
            return result;


            }
        }
        public virtual T GetByIdAsync(Guid id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {

                var result = dbContext.Set<T>().Find(id);
                return result;
            }

        }

        public virtual T UpdateAsync(T entity)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                dbContext.SaveChanges();
                return entity;

            }
        }

    }
}
