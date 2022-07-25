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

        public async Task<Device> GetDevicedd(string machineName)
        {
            var tasks = await Task.Run(() =>
                      {
                          return DbContext.Devices
                                     .Where(x => x.MachineName == machineName)
                                     //.ToListAsync()
                                     //.Include(x => x.Branch)
                                     .FirstOrDefaultAsync();
                      });
            var device = Task.Run(() => tasks);
            return device.Result ?? null;
        }
        public async Task<Device> GetDeviceWithNavigations(string machineName)
        {
            return await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                 .Include(x => x.Branch)
                    .Include(x => x.DeviceSuspenseAccounts)
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
                 .FirstOrDefaultAsync();
        }
        public async Task<IList<Device>> GetAllAsync()
        {
            return await DbContext.Devices.ToListAsync();
        }

        public async Task<Device> GetByIdAsync(Guid DeviceId)
        {
            return await DbContext.Devices
             .Where(x => x.Id == DeviceId)
             .Include(x => x.Branch)
              .FirstOrDefaultAsync();
        }
        public async Task<Device> GetDevice(string machineName)
        {
            return await DbContext.Devices
             .Where(x => x.MachineName == machineName)
             .Include(x => x.Branch)
             .Include(x => x.ConfigGroupNavigation)
             .FirstOrDefaultAsync();
        }

        public async Task<Device> GetDeviceScreenList(string machineName)
        {
            return await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                .Include(x => x.GUIScreenListNavigation.GuiScreenListScreens)
                .ThenInclude(x => x.GUIScreenNavigation.GUIScreenType)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.DisclaimerNavigation)
                     .ThenInclude(s => s.TransactionTextTerms)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AccountNameCaptionNavigation)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AccountNumberCaptionNavigation)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AliasAccountNameCaptionNavigation)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AliasAccountNameCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.AliasAccountNameCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.DepositorNameCaptionNavigation)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.FullInstructionsNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.FundsSourceCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.IdNumberCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ListItemCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.NarrationCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.PhoneNumberCaptionNavigation)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ReceiptTemplateNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ReferenceAccountNameCaptionNavigation)

                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ReferenceAccountNumberCaptionNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.TermsNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ValidationTextErrorMessageNavigation)


                .Include(x => x.GUIScreenListNavigation)
                     .ThenInclude(x => x.TransactionTypeListItems)
                     .ThenInclude(x => x.TxTextNavigationText)
                     .ThenInclude(x => x.ValidationTextSuccessMessageNavigation)
                 .FirstOrDefaultAsync();
        }

        public async Task<Device> GetDeviceLanguageList(string machineName)
        {
            return await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                .Include(x => x.LanguageListNavigation.LanguageListLanguages).ThenInclude(x => x.LanguageItemNavigation)
                 .FirstOrDefaultAsync();
        }

        public async Task<Device> GetDeviceCurrencyList(string machineName)
        {
            return await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                .Include(x => x.CurrencyListNavigation.CurrencyListCurrencies).ThenInclude(x => x.CurrencyItemNavigation)
                 .FirstOrDefaultAsync();
        }
        public async Task<Device> GetDeviceTransactionTypeList(string machineName)
        {
            return await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                .Include(x => x.TransactionTypeListNavigation.TransactionTypeListTransactionTypeListItems).ThenInclude(x => x.TxtypeListItemNavigation)
                 .FirstOrDefaultAsync();
        }

        public async Task<List<ApplicationUser>> GetByUserGroupAsync(int? UserGroup)
        {
            var response = await DbContext.Devices
             .Where(x => x.UserGroup == UserGroup)
             .Include(i => i.UserGroupNavigation).ThenInclude(t => t.ApplicationUsers).ThenInclude(u => u.Role)
             .SelectMany(dv => dv.UserGroupNavigation.ApplicationUsers.ToList())
             .ToListAsync();

            return response;
        }
        public async Task<ApplicationUser> GetByUserGroupAsync(int? UserGroup, string Username)
        {
            var response = await DbContext.Devices
                .Where(de => de.UserGroup == UserGroup)
                .Select(dv => dv.UserGroupNavigation.ApplicationUsers.FirstOrDefault(x => !(bool)x.UserDeleted && x.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefaultAsync();

            return response;
        }
    }
}
