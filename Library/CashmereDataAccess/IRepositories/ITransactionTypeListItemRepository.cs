using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ITransactionTypeListItemRepository : IAsyncRepository<TransactionTypeListItem>
    {
        public Task<TransactionTypeListItem> GetTransactionTypeScreenList(int transactionChosenId);
    }

}
