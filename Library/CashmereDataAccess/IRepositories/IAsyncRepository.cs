using System.Linq.Expressions;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAsyncRepository<T> where T : class 
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> Exists(int id);
    }
}
