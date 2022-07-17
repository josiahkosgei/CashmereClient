
//.AccountNumberVerifyDetailsScreenViewModel




using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  [Guid("80EA20BF-BF5F-4B87-A722-2C29882790FB")]
  public class AccountNumberVerifyDetailsScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
    public List<SummaryListItem> SummaryList { get; set; }

    public AccountNumberVerifyDetailsScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
        SummaryList = applicationViewModel.CurrentTransaction.TransactionAccountReferences;
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
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
      ApplicationViewModel.CurrentSession.AccountVerified = true;
      ApplicationViewModel.NavigateNextScreen();
    }
  }
}
