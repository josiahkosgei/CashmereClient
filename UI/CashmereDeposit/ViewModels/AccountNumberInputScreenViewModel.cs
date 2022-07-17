
//.AccountNumberInputScreenViewModel




using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CashmereDeposit.ViewModels
{
  [Guid("DECF0FAF-2BA5-47CB-933A-FD45BDD58ECC")]
  public class AccountNumberInputScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
    public AccountNumberInputScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
        CustomerInput = applicationViewModel.CurrentTransaction.AccountNumber;
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
      if (Task.Run((Func<Task<bool>>) (() => ValidateAsync(CustomerInput))).Result)
      {
        ApplicationViewModel.NavigateNextScreen();
      }
      else
      {
        ApplicationViewModel.CloseDialog(false);
        CanNext = true;
      }
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public void Back()
    {
      ApplicationViewModel.CurrentTransaction.AccountNumber = CustomerInput;
      ApplicationViewModel.NavigatePreviousScreen();
    }

    public async Task<bool> ValidateAsync(string accountNumber)
    {
      AccountNumberInputScreenViewModel inputScreenViewModel = this;
      if (!inputScreenViewModel.ClientValidation(accountNumber))
        return false;
      AccountNumberValidationResponse validationResponse = await inputScreenViewModel.ApplicationViewModel.ValidateAccountNumberAsync(inputScreenViewModel.CustomerInput, inputScreenViewModel.ApplicationViewModel.CurrentTransaction?.CurrencyCode.ToUpper(), inputScreenViewModel.ApplicationViewModel.CurrentTransaction.TransactionType.Id);
      if (validationResponse != null && validationResponse.IsSuccess && validationResponse.CanTransact)
      {
        inputScreenViewModel.ApplicationViewModel.CurrentTransaction.AccountNumber = inputScreenViewModel.CustomerInput;
        inputScreenViewModel.ApplicationViewModel.CurrentTransaction.AccountName = validationResponse.AccountName;
        return true;
      }
      inputScreenViewModel.PrintErrorText(validationResponse.PublicErrorMessage);
      return false;
    }
  }
}
