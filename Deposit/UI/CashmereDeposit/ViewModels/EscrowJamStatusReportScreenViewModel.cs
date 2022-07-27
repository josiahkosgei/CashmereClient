using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using System;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    public class EscrowJamStatusReportScreenViewModel : FormViewModelBase
    {
        private bool canNext;
        private readonly ICITRepository _citRepository;
        private readonly ICITTransactionRepository _cITTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDeviceCITSuspenseAccountRepository _deviceCITSuspenseAccountRepository;
        private readonly IDeviceSuspenseAccountRepository _deviceSuspenseAccountRepository;

        public EscrowJamStatusReportScreenViewModel(
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
            ScreenTitle = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_EscrowJamFormScreenTitle", "Clear Escrow Jam");
            NextCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor NextCaption", "sys_EndEscrowJamRecoveryCommand_Caption", "Complete");
            CancelCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.CancelCaption", "sys_CancelButton_Caption", "Cancel");
            BackCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackCaption", "sys_BackButton_Caption", "Back");
            ActivateItemAsync(new FormListViewModel(this));
            Activated += new EventHandler<ActivationEventArgs>(EscrowJamStatusReportScreenViewModel_Activated);
            ApplicationViewModel.DeviceManager.EscrowJamEndRequestEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndRequestEvent);
        }

        private void DeviceManager_EscrowJamEndRequestEvent(object sender, EventArgs e)
        {
            CanNext = true;
        }

        private async void EscrowJamStatusReportScreenViewModel_Activated(
          object sender,
          ActivationEventArgs e)
        {
            if (ApplicationViewModel.EscrowJam == null)
            {
                var transaction =  _transactionRepository.GetFirstSortBy();// _depositorDBContext.Transactions.OrderByDescending(x => x.TxStartDate).FirstOrDefault();
                if (!transaction.TxCompleted || transaction.TxErrorCode == 85)
                {
                    ApplicationViewModel.EscrowJam = new EscrowJam()
                    {
                        Id = Guid.NewGuid(),
                        DateDetected = DateTime.Now
                    };
                    transaction.EscrowJams.Add(ApplicationViewModel.EscrowJam);
                     _transactionRepository.UpdateAsync(transaction);
                }
            }
            CanNext = ApplicationViewModel.DeviceManager.CurrentState == DeviceManagerState.ESCROWJAM_END_REQUEST;
            ApplicationViewModel.ClearEscrowJam();
        }

        public void Back()
        {
            ApplicationViewModel.ShowDialog(CallingObject);
        }

        public void Cancel()
        {
            ApplicationViewModel.CloseDialog();
        }

        public bool CanNext
        {
            get => canNext;
            set
            {
                canNext = value;
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public void Next()
        {
            CanNext = false;
            ApplicationViewModel.ShowDialog(new EscrowJamFormViewModel(ApplicationViewModel, ApplicationViewModel, new OutOfOrderScreenViewModel(ApplicationViewModel), false));
        }
    }
}
