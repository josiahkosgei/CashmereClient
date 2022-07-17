
//.EscrowJamStatusReportScreenViewModel




using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using System;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  public class EscrowJamStatusReportScreenViewModel : FormViewModelBase
  {
    private bool canNext;

    public EscrowJamStatusReportScreenViewModel(
      ApplicationViewModel applicationViewModel,
      Conductor<Screen> conductor,
      Screen callingObject,
      bool isNewEntry)
      : base(applicationViewModel, conductor, callingObject, isNewEntry)
    {
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

    private void EscrowJamStatusReportScreenViewModel_Activated(
      object sender,
      ActivationEventArgs e)
    {
      if (ApplicationViewModel.EscrowJam == null)
      {
        Transaction transaction = depositorDbContext.Transactions.OrderByDescending(x => x.TxStartDate).FirstOrDefault();
        if (!transaction.TxCompleted || transaction.TxErrorCode == 85)
        {
          ApplicationViewModel.EscrowJam = new EscrowJam()
          {
            Id = Guid.NewGuid(),
            DateDetected = DateTime.Now
          };
          transaction.EscrowJams.Add(ApplicationViewModel.EscrowJam);
          ApplicationViewModel.SaveToDatabase(depositorDbContext);
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
        get { return canNext; }
        set
      {
        canNext = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanNext));
      }
    }

    public void Next()
    {
      CanNext = false;
      ApplicationViewModel.ShowDialog(new EscrowJamFormViewModel(ApplicationViewModel, ApplicationViewModel, new OutOfOrderScreenViewModel(ApplicationViewModel), false));
    }
  }
}
