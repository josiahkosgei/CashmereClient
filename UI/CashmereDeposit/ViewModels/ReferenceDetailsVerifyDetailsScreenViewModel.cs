
// Type: CashmereDeposit.ViewModels.ReferenceDetailsVerifyDetailsScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  [Guid("DF433792-DDBD-41EB-A95E-8730A9313505")]
  public class ReferenceDetailsVerifyDetailsScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
    public List<SummaryListItem> SummaryList { get; set; }

    public ReferenceDetailsVerifyDetailsScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
        SummaryList = applicationViewModel.CurrentTransaction.TransactionReferences;
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.ReferencesAccepted(false);
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
      ApplicationViewModel.StartCountingProcess();
      ApplicationViewModel.NavigateNextScreen();
    }
  }
}
