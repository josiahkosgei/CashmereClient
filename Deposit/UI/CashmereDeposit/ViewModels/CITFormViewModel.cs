
//.CITFormViewModel




//using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    internal class CITFormViewModel : FormViewModelBase
    {
        private string _bagNumber;
        private string _sealNumber;

        protected string NewBagNumber
        {
            get => _bagNumber;
            set
            {
                _bagNumber = value;
                NotifyOfPropertyChange("Bagnumber");
            }
        }

        protected string SealNumber
        {
            get => _sealNumber;
            set
            {
                _sealNumber = value;
                NotifyOfPropertyChange(nameof(SealNumber));
            }
        }

        private CIT CIT { get; set; }

        private CITDenomination CITDenominations { get; set; }

        private DateTime thisCITToDate { get; set; }

        private DateTime thisCITFromDate { get; set; }

        private readonly ICITRepository _citRepository;
        private readonly ICITTransactionRepository _cITTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDeviceCITSuspenseAccountRepository _deviceCITSuspenseAccountRepository;
        private readonly IDeviceSuspenseAccountRepository _deviceSuspenseAccountRepository;


        private Device Device { get; }

        public CITFormViewModel(
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject,
          bool isNewEntry)
          : base(applicationViewModel, conductor, callingObject, isNewEntry)
        {
            _citRepository = IoC.Get<ICITRepository>();
            _cITTransactionRepository = IoC.Get<ICITTransactionRepository>();
            _transactionRepository = IoC.Get<ITransactionRepository>();
            _deviceCITSuspenseAccountRepository = IoC.Get<IDeviceCITSuspenseAccountRepository>();
            _deviceSuspenseAccountRepository = IoC.Get<IDeviceSuspenseAccountRepository>();

            Device = applicationViewModel.ApplicationModel.GetDeviceAsync();
            ScreenTitle = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_CITFormScreenTitle", "CIT Operation");
            NextCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor NextCaption", "sys_StartCITCommand_Caption", "Start CIT");
            Fields.Add(new FormListItem()
            {
                DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_CIT_NewBagNumber_Caption", "sys_CIT_NewBagNumber_Caption", "New Bag Number"),
                Validate = new Func<string, string>(ValidateBagNumber),
                ValidatedText = NewBagNumber,
                DataTextBoxLabel = NewBagNumber,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_CIT_SealNumber_Caption", "sys_CIT_SealNumber_Caption", "Seal Number"),
                Validate = new Func<string, string>(ValidateSealNumber),
                ValidatedText = SealNumber,
                DataTextBoxLabel = SealNumber,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            ActivateItemAsync(new FormListViewModel(this));
        }

        public string ValidateBagNumber(string newBagNumber)
        {
            if (string.IsNullOrWhiteSpace(newBagNumber))
                return "Please enter a Bag Number";
            NewBagNumber = newBagNumber;
            return null;
        }

        public string ValidateSealNumber(string newSealNumber)
        {
            bool result = _citRepository.ValidateSealNumberAsync(newSealNumber);
            if (result)
                return "Seal Number must be unique and unused.";
            SealNumber = newSealNumber;
            return null;
        }

        public override async Task<string> SaveForm()
        {
            var num = FormValidation();
            if (num > 0)
                return string.Format("Form validation failed with {0:0} errors. Kindly correct them and try saving again", num);
            var CIT = createCIT();
            if (CIT != null)
            {

                try
                {
                     _citRepository.AddAsync(CIT);
                    //_depositorDBContext.SaveChangesAsync();
                    ApplicationViewModel.StartCIT(SealNumber);
                    ApplicationViewModel.AdminMode = false;
                    ApplicationViewModel.ShowErrorDialog(new OutOfOrderScreenViewModel(ApplicationViewModel));
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Error(GetType().Name, nameof(CITFormViewModel), nameof(SaveForm), "Error saving CIT to database: {0}", new object[1]
                    {
            ex.MessageString()
                    });
                    return "Error occurred. Contact administrator.";
                }
            }
            return null;
        }

        public override int FormValidation()
        {
            var num = 0;
            foreach (var field in Fields)
            {
                var validate = field.Validate;
                var str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
                if (str != null)
                {
                    field.ErrorMessageTextBlock = str;
                    ++num;
                }
            }
            return num;
        }

        private CIT createCIT()
        {
            try
            {
                ApplicationViewModel.HandleIncompleteSession();
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.WarningFormat("CITFormViewModel.createCIT()", "Error closing incomplete sessions", "System", "{0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
            }
            var lastCIT = ApplicationViewModel.lastCIT;
            int num;
            if (lastCIT == null)
            {
                num = 1;
            }
            else
            {
                var toDate = lastCIT.ToDate;
                num = 0;
            }
            thisCITFromDate = num != 0 ? DateTime.MinValue : lastCIT.ToDate;
            thisCITToDate = DateTime.Now;
            CIT = new CIT()
            {
                Id = GuidExt.UuidCreateSequential(),
                CITDate = DateTime.Now,
                StartUser = ApplicationViewModel.CurrentUser.Id,
                AuthUser = new Guid?(ApplicationViewModel.ValidatingUser.Id),
                FromDate = new DateTime?(thisCITFromDate),
                ToDate = thisCITToDate,
                DeviceId = Device.Id,
                OldBagNumber = lastCIT?.NewBagNumber,
                NewBagNumber = NewBagNumber,
                SealNumber = SealNumber,
                CITError = 0,
                Complete = false
            };
            var transactions = _transactionRepository.GetByDeviceDateRange(new Guid(), Device.Id, (DateTime)CIT.FromDate, CIT.ToDate);
            foreach (var transaction in transactions)
                transaction.CIT = CIT;
            foreach (var denominationByDatesResult in depositorContextProcedures.GetCITDenominationByDatesAsync(new DateTime?(thisCITFromDate), new DateTime?(thisCITToDate)).Result.OrderBy(x => x.denom).ToList())
                CIT.CITDenominations.Add(new CITDenomination()
                {
                    Id = GuidExt.UuidCreateSequential(),
                    CurrencyId = denominationByDatesResult.tx_currency,
                    Datetime = new DateTime?(thisCITToDate),
                    Denom = denominationByDatesResult.denom,
                    Count = denominationByDatesResult.count.Value,
                    Subtotal = denominationByDatesResult.subtotal.Value
                });
            CreateCITTransactions(CIT);
            return CIT;
        }

        private async void CreateCITTransactions(CIT CIT)
        {
            try
            {
                ApplicationViewModel.Log.DebugFormat("ApplicationViewModel", "Generate CITTransaction", nameof(CreateCITTransactions), "Generating CITTransactions for CIT id={0}", CIT.Id);
                var citTransactionList = new List<CITTransaction>(5);
                foreach (var grouping in CIT.CITDenominations.GroupBy(denom => denom.CurrencyId))
                {
                    var currency = grouping;
                    var num = currency.Sum(x => x.Subtotal);
                    if (num > 0L)
                    {
                        var deviceCITSuspenseAccount =  _deviceCITSuspenseAccountRepository.GetAccountNumber(CIT.DeviceId, currency.Key);
                        var suspenseAccount =  _deviceSuspenseAccountRepository.GetAccountNumber(CIT.DeviceId, currency.Key);
                         _cITTransactionRepository.AddAsync(new CITTransaction()
                        {
                            Id = Guid.NewGuid(),
                            CITId = CIT.Id,
                            AccountNumber = deviceCITSuspenseAccount.AccountNumber ?? throw new NullReferenceException(string.Format("No valid CITSuspenseAccount found for currency {0}", currency)),
                            SuspenseAccount = suspenseAccount.AccountNumber ?? throw new NullReferenceException(string.Format("No valid DeviceSuspenseAccount found for currency {0}", currency)),
                            Datetime = DateTime.Now,
                            Amount = num,
                            Currency = currency.Key,
                            Narration = string.Format("CIT {0} on {1:yyyyMMddTHHmmss}", Device.DeviceNumber, CIT.CITDate)
                        });
                    }
                }
                //_depositorDBContext.CITTransactions.AddRange(citTransactionList);
                //_depositorDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.ErrorFormat("ApplicationViewModel.CreateCITTransactions", 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), "Error posting CIT {0}: {1}", CIT.Id, ex.MessageString());
                throw;
            }
        }
    }
}
