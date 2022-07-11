
// Type: CashmereDeposit.ViewModels.TransactionSearchScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Cashmere.API.Messaging.CDM.GUIControl.AccountsLists;
using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.Library.Standard.Statuses;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess;


namespace CashmereDeposit.ViewModels
{
  [Guid("65FA2238-C8E3-49DD-BC1B-C9EF19DD58BB")]
  internal class TransactionSearchScreenViewModel : CustomerSearchScreenBaseViewModel
  {
    public TransactionSearchScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
      using (new DepositorDBContext())
      {
        ScrollList();
        PerformSearch();
      }
    }
        
    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public override void PerformSelection()
    {
      Account SelectedTransactionListItem = SelectedFilteredList.Value as Account;
      if (SelectedTransactionListItem == null)
        throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
      using (new DepositorDBContext())
      {
        string str1 = SelectedTransactionListItem?.account_name ?? "";
        string str2 = SelectedTransactionListItem?.account_number ?? "";
        if (ApplicationViewModel.CurrentTransaction.TransactionType.ValidateDefaultAccount)
        {
          AccountNumberValidationResponse result = Task.Run((Func<Task<AccountNumberValidationResponse>>) (() => ValidateAsync(SelectedTransactionListItem.account_number, SelectedTransactionListItem.currency))).Result;
          if (result == null || !result.IsSuccess || !result.CanTransact)
          {
            ErrorText = result != null ? result?.PublicErrorMessage : "Transaction Type is offline. Please try again later";
            ApplicationViewModel.Log.ErrorFormat(GetType().Name, 99, ApplicationErrorConst.ERROR_TRANSACTION_ACCOUNT_INVALID.ToString(), "cb={0},sv={1}", result.PublicErrorMessage, result?.ServerErrorMessage);
            ApplicationViewModel.CloseDialog(false);
            return;
          }
          str1 = result.AccountName;
          str2 = SelectedTransactionListItem.account_number;
        }
        ApplicationViewModel.CurrentTransaction.AccountNumber = str2;
        ApplicationViewModel.CurrentTransaction.AccountName = str1;
        ApplicationViewModel.NavigateNextScreen();
      }
    }

    public async Task<AccountNumberValidationResponse> ValidateAsync(
      string accountNumber,
      string currency)
    {
      TransactionSearchScreenViewModel searchScreenViewModel = this;
      return await searchScreenViewModel.ApplicationViewModel.ValidateAccountNumberAsync(accountNumber, currency, searchScreenViewModel.ApplicationViewModel.CurrentTransaction.TransactionType.Id);
    }
  }
}
