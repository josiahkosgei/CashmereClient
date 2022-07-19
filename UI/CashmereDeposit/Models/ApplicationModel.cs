using Cashmere.Library.Standard.Security;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.Models
{
    public class ApplicationModel
    {
        private List<GUIScreen> _dbGUIScreens;
        private List<Currency> _dbcurrencyList;
        private PasswordPolicyItems _passwordPolicy;
        public List<Language> _dblanguageList;
        public List<TransactionTypeListItem> _txTypeList;
        private ApplicationViewModel _applicationViewModel;

        public List<GUIScreen> dbGUIScreens => _dbGUIScreens;

        public List<Currency> CurrencyList => _dbcurrencyList;

        public PasswordPolicyItems PasswordPolicy => _passwordPolicy;

        public List<Language> LanguageList => _dblanguageList;

        public string SplashVideoPath => getSplashVideo();

        public List<TransactionTypeListItem> TransactionList => _txTypeList;

        public ApplicationViewModel ApplicationViewModel => _applicationViewModel;

        public ApplicationModel(ApplicationViewModel ApplicationViewModel) => _applicationViewModel = ApplicationViewModel;

        public bool TestConnection()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, "Database Connectivity Test", "Initialisation", "Testing Database Connection....");
                DbConnection connection = depositorDbContext.Database.GetDbConnection();
                for (int index = 0; index < 10; ++index)
                {
                    ApplicationViewModel.Log.DebugFormat(GetType().Name, "DB Connection Attempt", "Initialisation", "Attempt {0}", index + 1);
                    try
                    {
                        connection.Open();
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, "DB Connect SUCCESS", "Initialisation", "Database Connection OK after {0} tries", index + 1);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ApplicationViewModel.Log.ErrorFormat(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_OFFLINE.ToString(), "Database Connection FAILED: {0}", ex.MessageString());
                    }
                }
                return false;
            }
        }

        internal void InitialiseApplicationModel()
        {
            using (new DepositorDBContext())
            {
                _passwordPolicy = GetPasswordPolicy();
                GenerateTransactionTypeList();
                GenerateCurrencyList();
                GenerateLanguageList();
                GenerateScreenList();
            }
        }

        internal string GetDeviceNumber()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
                return GetDevice(DBContext)?.DeviceNumber;
        }

        internal Device GetDevice(DepositorDBContext DBContext)
        {
            try
            {

                var device = DBContext.Devices.Include(x => x.Branch).Include(x => x.ConfigGroupNavigation)
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
                    .FirstOrDefault(x => x.MachineName == Environment.MachineName);
                if (device != null)
                    return device;
                ApplicationViewModel.Log.Fatal(nameof(ViewModels.ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not get device info from database, terminating");
                throw new InvalidOperationException(string.Format("Device with machine name = {0} does not exists in the local database.", Environment.MachineName));
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.FatalFormat(nameof(ViewModels.ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "{0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                throw;
            }
        }

        internal PasswordPolicyItems GetPasswordPolicy()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                PasswordPolicy passwordPolicy = depositorDbContext.PasswordPolicies.FirstOrDefault();
                return new PasswordPolicyItems()
                {
                    LowerCaseLength = passwordPolicy.MinLowercase,
                    MinimumLength = passwordPolicy.MinLength,
                    SpecialLength = passwordPolicy.MinSpecial,
                    NumericLength = passwordPolicy.MinDigits,
                    UpperCaseLength = passwordPolicy.MinUppercase,
                    HistorySize = passwordPolicy.HistorySize
                };
            }
        }

        internal string getSplashVideo() => "resources/bank.mp4";

        public void GenerateScreenList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateScreenList), "Initialisation", "Generating Screens List");
                _dbGUIScreens = GetDevice(DBContext).GUIScreenListNavigation.GuiScreenListScreens.OrderBy(x => x.ScreenOrder).Select(x => x.GUIScreenNavigation).Where(x => x.Enabled).ToList<GUIScreen>();
                int num;
                if (_dbGUIScreens != null)
                {
                    List<GUIScreen> dbGuiScreens = _dbGUIScreens;
                    // ISSUE: explicit non-virtual call
                    num = dbGuiScreens != null ? dbGuiScreens.Count == 0 ? 1 : 0 : 0;
                }
                else
                    num = 1;
                if (num != 0)
                {
                    ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate screen list from database");
                    throw new Exception("Could not generate screen list from database");
                }
            }
        }

        public void GenerateLanguageList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateLanguageList), "Initialisation", "Generating Language List");
                _dblanguageList = GetDevice(DBContext).LanguageListNavigation.LanguageListLanguages.Where(x => (bool)x.LanguageListNavigation.Enabled).OrderBy(x => x.LanguageOrder).Select(x => x.LanguageItemNavigation).ToList();
                int num;
                if (_dblanguageList != null)
                {
                    List<Language> dblanguageList = _dblanguageList;
                    // ISSUE: explicit non-virtual call
                    num = dblanguageList != null ? dblanguageList.Count == 0 ? 1 : 0 : 0;
                }
                else
                    num = 1;
                if (num != 0)
                {
                    ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate language list from database");
                    throw new Exception("Could not generate language list from database");
                }
            }
        }

        public void GenerateCurrencyList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateCurrencyList), "Initialisation", "Generating Currency List");
                _dbcurrencyList = GetDevice(DBContext).CurrencyListNavigation.CurrencyListCurrencies.OrderBy(x => x.CurrencyOrder).Select(x => x.CurrencyItemNavigation).Skip(1).ToList();
                int num;
                if (_dbcurrencyList != null)
                {
                    List<Currency> dbcurrencyList = _dbcurrencyList;
                    // ISSUE: explicit non-virtual call
                    num = dbcurrencyList != null ? dbcurrencyList.Count == 0 ? 1 : 0 : 0;
                }
                else
                    num = 1;
                if (num == 0)
                    return;
                ApplicationViewModel.Log.Info(GetType().Name, nameof(GenerateCurrencyList), "Initialisation", "No extra currencies available. Using signle currency.");
            }
        }

        public void GenerateTransactionTypeList()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                try
                {
                    ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateTransactionTypeList), "Initialisation", "Generating Transaction Type List");
                    _txTypeList = GetDevice(DBContext).TransactionTypeListNavigation.TransactionTypeListTransactionTypeListItems.OrderBy(x => x.ListOrder).Select(x => x.TxtypeListItemNavigation).Where(x => (bool)x.Enabled).ToList();
                    int num;
                    if (_txTypeList != null)
                    {
                        List<TransactionTypeListItem> txTypeList = _txTypeList;
                        // ISSUE: explicit non-virtual call
                        num = txTypeList != null ? txTypeList.Count == 0 ? 1 : 0 : 0;
                    }
                    else
                        num = 1;
                    if (num != 0)
                    {
                        ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate transactiontype list from database");
                        throw new Exception("Could not generate transactiontype list from database");
                    }
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Fatal(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not generate transactiontype list from database");
                    throw new Exception("Could not generate transactiontype list from database: " + ex?.Message, ex);
                }
            }
        }

        public List<GUIScreen> GetTransactionTypeScreenList(
          TransactionTypeListItem transactionChosen)
        {
            return transactionChosen.TxTypeGUIScreenlistNavigation.GuiScreenListScreens.Where(x => x.Enabled).OrderBy(x => x.ScreenOrder).Select(x => x.GUIScreenNavigation).Where(x => x.Enabled).ToList();
        }

        public event EventHandler<EventArgs> DatabaseStorageErrorEvent;

        private void OnDatabaseStorageErrorEvent(object sender, EventArgs e)
        {
            if (DatabaseStorageErrorEvent == null)
                return;
            DatabaseStorageErrorEvent(this, e);
        }
    }
}
