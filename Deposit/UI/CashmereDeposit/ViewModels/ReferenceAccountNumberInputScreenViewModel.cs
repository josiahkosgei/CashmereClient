using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CashmereDeposit.ViewModels
{
    [Guid("DDF37150-7D4B-40EE-9B33-E6ADC937C29A")]
    internal class ReferenceAccountNumberInputScreenViewModel : DepositorCustomerScreenBaseViewModel
    {
        public ReferenceAccountNumberInputScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            CustomerInput = applicationViewModel.CurrentTransaction.ReferenceAccount;
            ApplicationViewModel.CurrentTransaction.AccountNumber = ApplicationViewModel.CurrentTransaction.TransactionType.DefaultAccount;
            ApplicationViewModel.CurrentTransaction.AccountName = ApplicationViewModel.CurrentTransaction.TransactionType.DefaultAccountName;
            try
            {
                var referenceAccountNumberCaption = applicationViewModel?.CurrentTransaction?.TransactionType?.TxTextNavigationText.ReferenceAccountNumberCaption;
                ScreenTitle = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".ReferenceAccountNumberInputScreenViewModel  ScreenTitle", referenceAccountNumberCaption, "Reference Number");
            }
            catch (Exception ex)
            {
            }
        }

        public void Next()
        {
            lock (ApplicationViewModel.NavigationLock)
            {
                if (!CanNext)
                    return;
                CanNext = false;
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                var backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = false
                };
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Task.Run(() => ValidateAsync(CustomerInput)).Result)
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
            ApplicationViewModel.CurrentTransaction.ReferenceAccount = CustomerInput;
            ApplicationViewModel.NavigatePreviousScreen();
        }

        public async Task<bool> ValidateAsync(string refAaccountNumber)
        {
            var inputScreenViewModel = this;
            if (!inputScreenViewModel.ClientValidation(refAaccountNumber))
                return false;
            inputScreenViewModel.ApplicationViewModel.CurrentTransaction.ReferenceAccount = refAaccountNumber;
            var currentTransaction = inputScreenViewModel.ApplicationViewModel.CurrentTransaction;
            int num;
            if (currentTransaction == null)
            {
                num = 0;
            }
            else
            {
                var referenceAccount = currentTransaction.TransactionType?.ValidateReferenceAccount;
                var flag = true;
                num = referenceAccount.GetValueOrDefault() == flag & referenceAccount.HasValue ? 1 : 0;
            }
            if (num == 0)
                return true;
            var validationResponse = await inputScreenViewModel.ApplicationViewModel.ValidateReferenceAccountNumberAsync(inputScreenViewModel.ApplicationViewModel.CurrentTransaction.AccountNumber, refAaccountNumber, inputScreenViewModel.ApplicationViewModel.CurrentTransaction?.TransactionType?.CbTxType);
            if (validationResponse != null && validationResponse.IsSuccess && validationResponse.CanTransact)
            {
                inputScreenViewModel.ApplicationViewModel.CurrentTransaction.ReferenceAccountName = validationResponse.AccountName;
                return true;
            }
            inputScreenViewModel.PrintErrorText(validationResponse?.PublicErrorMessage);
            return false;
        }
    }
}
