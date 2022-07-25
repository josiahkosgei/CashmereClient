using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListItemRepository : RepositoryBase<TransactionTypeListItem>, ITransactionTypeListItemRepository
    {
        public TransactionTypeListItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<TransactionTypeListItem> GetTransactionTypeScreenList(int transactionChosenId)
        {
            return await DbContext.TransactionTypeListItems.Where(x => x.Id == transactionChosenId)
                .Include(tx => tx.TxTypeGUIScreenlistNavigation.GuiScreenListScreens)
                .ThenInclude(x => x.GUIScreenNavigation.GUIScreenType)
                .Include(tx => tx.TxTypeGUIScreenlistNavigation.GuiScreenListScreens)
                .ThenInclude(x => x.GUIScreenNavigation.GUIScreenText)
                .Include(i => i.TxTypeGUIScreenlistNavigation)
                .Include(i => i.DefaultAccountCurrencyNavigation)
                .Include(i => i.TransactionTextNav)
                .Include(i => i.TxLimitListNavigation)
                .Include(i => i.TxTextNavigationText)
                .Include(i => i.TxTypeNavigation)
                .Include(x => x.TxTextNavigationText.DisclaimerNavigation)
                .FirstOrDefaultAsync();
        }
    }
}
