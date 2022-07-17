
//.FundsSourceInputScreenViewModel




using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
  [Guid("33EC330E-FB51-4626-906D-1A3F77AAA5E2")]
  public class FundsSourceInputScreenViewModel : CustomerPrepopReferenceScreenBase
  {
    public FundsSourceInputScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(applicationViewModel?.CurrentTransaction?.FundsSource, screenTitle, applicationViewModel, required)
    {
        ApplicationViewModel.Log.Info(GetType().Name, "Screen Loaded", "Navigation", "Screen Loaded");
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.CurrentTransaction.FundsSource = CustomerInput;
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
        var backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = false;
        backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
        backgroundWorker.RunWorkerAsync();
      }
    }

    private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      if (Validate())
      {
        ApplicationViewModel.CurrentTransaction.FundsSource = CustomerInput;
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
