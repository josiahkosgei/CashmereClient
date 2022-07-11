
// Type: CashmereDeposit.ViewModels.IDNumberInputScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
  [Guid("085D1E91-E970-4A62-8353-253C35E0D8E8")]
  public class IDNumberInputScreenViewModel : CustomerPrepopReferenceScreenBase
  {
    public IDNumberInputScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(applicationViewModel?.CurrentTransaction?.IDNumber, screenTitle, applicationViewModel, required)
    {
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.CurrentTransaction.IDNumber = CustomerInput;
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
        ApplicationViewModel.CurrentTransaction.IDNumber = CustomerInput;
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
