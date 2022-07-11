
// Type: CashmereDeposit.ViewModels.ReferenceAccountNumberVerifyDetailsScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Collections.Generic;
using System.Runtime.InteropServices;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  [Guid("7375104A-BDE3-40DF-B926-7DF69A010C79")]
  internal class ReferenceAccountNumberVerifyDetailsScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
    public List<SummaryListItem> SummaryList { get; set; }

    public ReferenceAccountNumberVerifyDetailsScreenViewModel(
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
      ApplicationViewModel.CurrentSession.AccountVerified = true;
      ApplicationViewModel.CurrentSession.ReferenceAccountVerified = true;
      ApplicationViewModel.NavigateNextScreen();
    }
  }
}
