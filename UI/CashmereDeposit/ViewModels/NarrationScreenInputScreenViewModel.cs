
//.NarrationScreenInputScreenViewModel




using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
  [Guid("60768315-94CE-4E14-B26B-3D57212FC9CE")]
  public class NarrationScreenInputScreenViewModel : CustomerPrepopReferenceScreenBase
  {
    public NarrationScreenInputScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(applicationViewModel?.CurrentTransaction?.Narration, screenTitle, applicationViewModel, required)
    {
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.CurrentTransaction.Narration = CustomerInput;
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
        ApplicationViewModel.CurrentTransaction.Narration = CustomerInput;
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
