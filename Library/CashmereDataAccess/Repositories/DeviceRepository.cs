using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Device> GetDevice(string machineName) => await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                .Include(x => x.Branch)
                     .Include(x => x.ConfigGroupNavigation)
                     .Include(x => x.LanguageListNavigation)
                         .ThenInclude(x => x.LanguageListLanguages)
                     .Include(x => x.GUIScreenListNavigation)
                         .ThenInclude(x => x.GuiScreenListScreens)
                         .ThenInclude(x => x.GUIScreenNavigation)
                         .ThenInclude(x => x.GUIScreenType)

                     .Include(x => x.GUIScreenListNavigation)
                         .ThenInclude(a => a.GuiScreenListScreens)
                         .ThenInclude(x => x.GUIScreenNavigation)
                         .ThenInclude(x => x.GUIScreenText)
                         .ThenInclude(x => x.BtnAcceptCaptionNavigation)

                   .Include(x => x.GUIScreenListNavigation)
                             .ThenInclude(a => a.GuiScreenListScreens)
                             .ThenInclude(x => x.GUIScreenNavigation)
                             .ThenInclude(x => x.GUIScreenText)
                         .ThenInclude(x => x.BtnBackCaptionNavigation)

                   .Include(x => x.GUIScreenListNavigation)
                             .ThenInclude(a => a.GuiScreenListScreens)
                             .ThenInclude(x => x.GUIScreenNavigation)
                             .ThenInclude(x => x.GUIScreenText)
                             .ThenInclude(x => x.BtnCancelCaptionNavigation)

                   .Include(x => x.GUIScreenListNavigation)
                             .ThenInclude(a => a.GuiScreenListScreens)
                             .ThenInclude(x => x.GUIScreenNavigation)
                             .ThenInclude(x => x.GUIScreenText)
                             .ThenInclude(x => x.FullInstructionsNavigation)

                   .Include(x => x.GUIScreenListNavigation)
                             .ThenInclude(a => a.GuiScreenListScreens)
                             .ThenInclude(x => x.GUIScreenNavigation)
                             .ThenInclude(x => x.GUIScreenText)
                             .ThenInclude(x => x.ScreenTitleInstructionNavigation)

                   .Include(x => x.GUIScreenListNavigation)
                             .ThenInclude(a => a.GuiScreenListScreens)
                             .ThenInclude(x => x.GUIScreenNavigation)
                             .ThenInclude(x => x.GUIScreenText)
                             .ThenInclude(x => x.ScreenTitleNavigation)

                     .Include(x => x.CurrencyListNavigation)
                     .Include(x => x.CurrencyListNavigation.DefaultCurrencyNavigation)
                     .Include(x => x.ConfigGroupNavigation)
                     .Include(x => x.TransactionTypeListNavigation)
                     .Include(x => x.TransactionTypeListNavigation)
                         .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                         .ThenInclude(x => x.TxtypeListItemNavigation)
                         .ThenInclude(x => x.TxTypeGUIScreenlistNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTypeGUIScreenlistNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TransactionTextNav)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.DisclaimerNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AccountNameCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AccountNumberCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AliasAccountNameCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AliasAccountNameCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AliasAccountNameCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.DepositorNameCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.FullInstructionsNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.FundsSourceCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.IdNumberCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ListItemCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.NarrationCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.PhoneNumberCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ReceiptTemplateNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ReferenceAccountNameCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ReferenceAccountNumberCaptionNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.TermsNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ValidationTextErrorMessageNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ValidationTextSuccessMessageNavigation)

                     .Include(x => x.TransactionTypeListNavigation)
                     .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                     .ThenInclude(x => x.TxtypeListItemNavigation)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.TxItemNavigation)
                     .AsNoTracking()
                     .AsSplitQuery()
                     .FirstOrDefaultAsync();

        public async Task<IList<Device>> GetAllAsync()
        {
            return await DbContext.Devices.ToListAsync();
        }
    }
}
