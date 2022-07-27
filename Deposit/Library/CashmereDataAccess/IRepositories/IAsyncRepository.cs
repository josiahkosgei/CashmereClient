using System.Linq.Expressions;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAsyncRepository<T> where T : class, new()
    {
        IReadOnlyList<T> GetAllAsync();
        IReadOnlyList<T> GetAsync(Expression<Func<T, bool>> predicate);
        T GetByIdAsync(int id);
        T GetByIdAsync(Guid id);
        T AddAsync(T entity);
        T UpdateAsync(T entity);
        void DeleteAsync(T entity);
        bool Exists(int id);
    }
}
