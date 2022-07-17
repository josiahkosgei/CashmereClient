
//.DepositorNameInputScreenViewModel




using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
  [Guid("1DA2D457-711D-429D-B755-E9B44E1F33B1")]
  public class DepositorNameInputScreenViewModel : CustomerPrepopReferenceScreenBase
  {
    public DepositorNameInputScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(applicationViewModel?.CurrentTransaction?.DepositorName, screenTitle, applicationViewModel, required)
    {
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.CurrentTransaction.DepositorName = CustomerInput;
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
        ApplicationViewModel.CurrentTransaction.DepositorName = CustomerInput;
        ApplicationViewModel.NavigateNextScreen();
      }
      else
      {
        ApplicationViewModel.CloseDialog(false);
        CanNext = true;
      }
    }

    private bool Validate()
    {
        return ClientValidation(CustomerInput);
    }
  }
}
