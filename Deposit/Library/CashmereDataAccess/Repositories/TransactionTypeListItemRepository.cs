using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListItemRepository : RepositoryBase<TransactionTypeListItem>, ITransactionTypeListItemRepository
    {
        public TransactionTypeListItemRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public TransactionTypeListItem GetTransactionTypeScreenList(int transactionChosenId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.TransactionTypeListItems.Where(x => x.Id == transactionChosenId)
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
                    .FirstOrDefault();
                return result;

            }
        }
    }
}
