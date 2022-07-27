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
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Caliburn.Micro;
using System.Data;

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
        private static IDeviceRepository _deviceRepository { get; set; }

        private readonly DepositorDBContext _depositorDBContext;

        private static IPasswordPolicyRepository _passwordPolicyRepository { get; set; }
        private static ITransactionTypeListItemRepository _transactionTypeListItemRepository { get; set; }

        public ApplicationModel(ApplicationViewModel ApplicationViewModel)
        {
            _deviceRepository = IoC.Get<IDeviceRepository>();
            _depositorDBContext = IoC.Get<DepositorDBContext>();
            _passwordPolicyRepository = IoC.Get<IPasswordPolicyRepository>();
            _transactionTypeListItemRepository = IoC.Get<ITransactionTypeListItemRepository>();
            _applicationViewModel = ApplicationViewModel;
        }

        public bool TestConnection()
        {
            ApplicationViewModel.Log.Debug(GetType().Name, "Database Connectivity Test", "Initialisation", "Testing Database Connection....");

            DbConnection connection = _depositorDBContext.Database.GetDbConnection();
            for (int index = 0; index < 10; ++index)
            {
                ApplicationViewModel.Log.DebugFormat(GetType().Name, "DB Connection Attempt", "Initialisation", "Attempt {0}", index + 1);
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, "DB Connect SUCCESS", "Initialisation", "Database Connection OK after {0} tries", index + 1);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.ErrorFormat(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_OFFLINE.ToString(), "Database Connection FAILED: {0}", ex.MessageString());
                }
            }
            return false;

        }

        internal void InitialiseApplicationModel()
        {

            _passwordPolicy = GetPasswordPolicy();
            GenerateTransactionTypeList();
            GenerateCurrencyList();
            GenerateLanguageList();
            GenerateScreenList();

        }

        internal string GetDeviceNumber()
        {
            return GetDeviceAsync()?.DeviceNumber;
        }

        internal Device GetDeviceAsync()
        {
            try
            {


                var device = _deviceRepository.GetDevice(Environment.MachineName);
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
            PasswordPolicy passwordPolicy = _passwordPolicyRepository.GetFirst();
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

        internal string getSplashVideo() => "resources/bank.mp4";

        public async void GenerateScreenList()
        {
            ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateScreenList), "Initialisation", "Generating Screens List");

            var device =  _deviceRepository.GetDeviceScreenList(Environment.MachineName);
            _dbGUIScreens = device.GUIScreenListNavigation.GuiScreenListScreens.Where(x => x.Enabled).OrderBy(x => x.ScreenOrder).Select(x => x.GUIScreenNavigation).ToList();
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

        public async void GenerateLanguageList()
        {

            ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateLanguageList), "Initialisation", "Generating Language List");
            var device =  _deviceRepository.GetDeviceLanguageList(Environment.MachineName);
            _dblanguageList = device.LanguageListNavigation.LanguageListLanguages.Where(x => (bool)x.LanguageListNavigation.Enabled).OrderBy(x => x.LanguageOrder).Select(x => x.LanguageItemNavigation).ToList();
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

        public async void GenerateCurrencyList()
        {
            ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateCurrencyList), "Initialisation", "Generating Currency List");
            var device =  _deviceRepository.GetDeviceCurrencyList(Environment.MachineName);
            _dbcurrencyList = device.CurrencyListNavigation.CurrencyListCurrencies.OrderBy(x => x.CurrencyOrder).Select(x => x.CurrencyItemNavigation).Skip(1).ToList();
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

        public async void GenerateTransactionTypeList()
        {

            try
            {
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(GenerateTransactionTypeList), "Initialisation", "Generating Transaction Type List");
                var device =  _deviceRepository.GetDeviceTransactionTypeList(Environment.MachineName);
                _txTypeList = device.TransactionTypeListNavigation.TransactionTypeListTransactionTypeListItems.OrderBy(x => x.ListOrder)
                                                                                                                        .Select(x => x.TxtypeListItemNavigation)
                                                                                                                        .Where(x => (bool)x.Enabled)
                                                                                                                        .ToList();
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

        public List<GUIScreen> GetTransactionTypeScreenList(
          TransactionTypeListItem transactionChosen)
        {

            var uIScreens = _transactionTypeListItemRepository.GetTransactionTypeScreenList(transactionChosen.Id);

            var gUIScreens = uIScreens.TxTypeGUIScreenlistNavigation.GuiScreenListScreens.Where(x => x.Enabled)
                  .OrderBy(x => x.ScreenOrder)
                  .Select(x => x.GUIScreenNavigation).Where(x => x.Enabled).ToList();
            return gUIScreens;
        }
        public List<GUIScreen> GetTransactionTypeScreenList_a(
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
