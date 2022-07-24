using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ITransactionRepository: IAsyncRepository<Transaction>
    {
        public Task<Transaction> GetFirst();
        public Task<IList<Transaction>> GetCompleted();
    }
 
}
