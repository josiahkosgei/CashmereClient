
// Type: CashmereDeposit.ViewModels.PhoneNumberInputScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
  [Guid("29D7E648-8ADD-47AA-B512-A0F966E27509")]
  public class PhoneNumberInputScreenViewModel : CustomerPrepopReferenceScreenBase
  {
    public PhoneNumberInputScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(applicationViewModel?.CurrentTransaction?.Phone, screenTitle, applicationViewModel, required)
    {
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.CurrentTransaction.Phone = CustomerInput;
      ApplicationViewModel.NavigatePreviousScreen();
    }

    public void Next()
    {
      lock (ApplicationViewModel.NavigationLock)
      {
        if (!CanNext)
          return;
        CanNext = false;
        ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = false;
        backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
        backgroundWorker.RunWorkerAsync();
      }
    }

    private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      if (Validate())
      {
        ApplicationViewModel.CurrentTransaction.Phone = CustomerInput;
        ApplicationViewModel.NavigateNextScreen();
      }
      else
      {
        ApplicationViewModel.CloseDialog(false);
        CanNext = true;
      }
    }

    public bool Validate()
    {
        return ClientValidation(CustomerInput);
    }
  }
}
