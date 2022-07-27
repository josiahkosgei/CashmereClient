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
        {//TODO: Remove
            //_depositorDBContext.Set<T>().Add(entity);
            //_depositorDBContext.SaveChanges();
            return entity;
        }

        public virtual void DeleteAsync(T entity)
        {
            _depositorDBContext.Set<T>().Remove(entity);
            _depositorDBContext.SaveChanges();


        }

        public bool Exists(int id)
        {
            var result = _depositorDBContext.Set<T>().Find(id);
            return result != null;
        }

        public virtual IReadOnlyList<T> GetAllAsync()
        {

            var result = _depositorDBContext.Set<T>().ToList();
            return result;
        }

        public virtual IReadOnlyList<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var result = _depositorDBContext.Set<T>().Where(predicate).ToList();
            return result;

        }

        public virtual T GetByIdAsync(int id)
        {
            var result = _depositorDBContext.Set<T>().Find(id);
            return result;

        }
        public virtual T GetByIdAsync(Guid id)
        {
            var result = _depositorDBContext.Set<T>().Find(id);
            return result;

        }

        public virtual T UpdateAsync(T entity)
        {
            var saved = false;

            //TODO: Remove
            //_depositorDBContext.Entry(entity).State = EntityState.Modified;
            //_depositorDBContext.SaveChanges();
            return entity;
        }

    }
}
