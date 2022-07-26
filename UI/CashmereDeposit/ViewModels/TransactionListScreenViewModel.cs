
//.TransactionListScreenViewModel




using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.Library.Standard.Statuses;
using CashmereUtil.Imaging;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

using CashmereDeposit.Models;
using CashmereDeposit.Utils;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.ViewModels
{
    [Guid("F6BEA4EB-B0C9-4EB3-B225-1F83F73BFD70")]
    internal class TransactionListScreenViewModel : CustomerListScreenBaseViewModel
    {


        private readonly ITransactionTypeListItemRepository _transactionTypeListItemRepository;
        private readonly ITransactionTextRepository _transactionTextRepository;
        private static IDeviceRepository _iDeviceRepository { get; set; }
        public TransactionListScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {

            _iDeviceRepository = IoC.Get<IDeviceRepository>();
            _transactionTypeListItemRepository = IoC.Get<ITransactionTypeListItemRepository>();
            _transactionTextRepository = IoC.Get<ITransactionTextRepository>();
            FullList = ApplicationViewModel.TransactionTypesAvailable.Select(x => new ATMSelectionItem<object>(ImageManipuation.GetBitmapFromBytes(x.Icon), ApplicationViewModel.CashmereTranslationService.TranslateUserText("TransactionListScreenViewModel.listItem_caption", _transactionTextRepository.GetByIdAsync(x.Id).ContinueWith(x => x.Result).Result?.ListItemCaption, "No Text"), x)).ToList();
            GetFirstPage();
        }

        public void Cancel()
        {
            ApplicationViewModel.CancelSessionOnUserInput();
        }

        public override void PerformSelection()
        {
            ErrorText = "Please login to access this transaction";
            if (!(SelectedFilteredList.Value is TransactionTypeListItem transactionTypeListItem))
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
            if (transactionTypeListItem.InitUserRequired)
                ApplicationViewModel.ShowDialog(new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, this, this, "USER_DEPOSIT", loginSuccessCallBack: new UserLoginViewModel.LoginSuccessCallBack(HandleLogin)));
            else
                PerformSelectionFinal();
        }

        private void HandleLogin(ApplicationUser applicationUser, bool isAuth)
        {
            if (!(SelectedFilteredList.Value is TransactionTypeListItem transactionTypeListItem))
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");
            if (isAuth)
            {
                if (applicationUser != null)
                {
                    ApplicationViewModel.ValidatingUser = applicationUser;
                    PerformSelectionFinal();
                }
                else
                {
                    SelectedFilteredList = null;
                    ApplicationViewModel.CurrentUser = null;
                    ApplicationViewModel.ValidatingUser = null;
                }
            }
            else if (applicationUser != null)
            {
                ApplicationViewModel.CurrentUser = applicationUser;
                if (transactionTypeListItem.AuthUserRequired)
                    ApplicationViewModel.ShowDialog(new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, this, this, "USER_DEPOSIT", true, loginSuccessCallBack: new UserLoginViewModel.LoginSuccessCallBack(HandleLogin)));
                else
                    PerformSelectionFinal();
            }
            else
            {
                SelectedFilteredList = null;
                ApplicationViewModel.CurrentUser = null;
                ApplicationViewModel.ValidatingUser = null;
            }
        }

        private async void PerformSelectionFinal()
        {
            var SelectedTransactionListItem = SelectedFilteredList.Value as TransactionTypeListItem;
            if (SelectedTransactionListItem == null)
                throw new NullReferenceException(GetType().Name + ".PerformSelection SelectedTransactionListItem");

            ClearErrorText();
            var str1 = "";
            var str2 = SelectedTransactionListItem?.DefaultAccount ?? "";
            if (!string.IsNullOrWhiteSpace(SelectedTransactionListItem.DefaultAccount) && SelectedTransactionListItem.ValidateDefaultAccount)
            {
                var result = Task.Run(() => ValidateAsync(SelectedTransactionListItem.DefaultAccount, SelectedTransactionListItem.DefaultAccountCurrency, SelectedTransactionListItem.Id)).Result;
                if (result == null || !result.IsSuccess || !result.CanTransact)
                {
                    ErrorText = "Transaction Type is offline. Please try again later";
                    ApplicationViewModel.Log.ErrorFormat(GetType().Name, 99, ApplicationErrorConst.ERROR_TRANSACTION_ACCOUNT_INVALID.ToString(), "cb={0},sv={1}", result.PublicErrorMessage, result?.ServerErrorMessage);
                    ApplicationViewModel.CloseDialog(false);
                    return;
                }
                str1 = result.AccountName;
                str2 = SelectedTransactionListItem.DefaultAccount;
            }
            ApplicationViewModel.CreateTransaction(SelectedTransactionListItem);
            ApplicationViewModel.CurrentTransaction.AccountNumber = str2;
            ApplicationViewModel.CurrentTransaction.AccountName = str1;
            ApplicationViewModel.CurrentTransaction.Transaction.InitUser = ApplicationViewModel.CurrentUser?.Id;
            ApplicationViewModel.CurrentTransaction.Transaction.AuthUser = ApplicationViewModel.ValidatingUser?.Id;
            var translationService = ApplicationViewModel.CashmereTranslationService;
            string? str3;
            if (translationService == null)
            {
                str3 = null;
            }
            else
            {
                var transactionTypeId = ApplicationViewModel?.CurrentTransaction?.TransactionType.Id;
                var disclaimerNavigation = await _transactionTypeListItemRepository.GetTransactionTypeScreenList((int)transactionTypeId);
                //var disclaimer = ApplicationViewModel?.CurrentTransaction?.TransactionType?.TxTextNavigationText?.Disclaimer;
                var s = translationService.TranslateUserText(GetType().Name + ".PerformSelection disclaimer", disclaimerNavigation.TxTextNavigationText.Disclaimer, null);
                str3 = s != null ? s.CashmereReplace(ApplicationViewModel) : null;
            }
            var message = str3;
            var title = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_DisclaimerTitle_Caption", "Disclaimer");
            if (!string.IsNullOrWhiteSpace(message))
                ApplicationViewModel.ShowUserMessageScreen(title, message);
            else
                ApplicationViewModel.NavigateNextScreen();

        }

        public async Task<AccountNumberValidationResponse> ValidateAsync(
          string accountNumber,
          string currency,
          int txType)
        {
            return await ApplicationViewModel.ValidateAccountNumberAsync(accountNumber, currency, txType);
        }
    }
}
