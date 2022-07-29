using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.Models;
using CashmereDeposit.Utils;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CashmereDeposit.ViewModels.V3
{
    [Guid("DCFF4D71-8F1C-4A75-ACD8-96A21154A878")]
    public class AccountNumberViewModel : AccountNumberBase
    {
        private bool accountNameFrameIsVisible;
        private string validateButtonCaption;
        private string accountNameLabelCaption;
        private string accountDetailsTitleCaption;
        private string changeInputButtonCaption;
        private string accountNumberLabelCaption;
        private string accountNumber;
        private string accountName;

        public AccountNumberViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
            AccountDetailsTitleCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("AccountNumberViewModel._ctor", "sys_AccountDetailsTitleCaption", "Account Details");
            AccountNameLabelCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("AccountNumberViewModel._ctor", "sys_AccountNameLabelCaption", "Account Name");
            ValidateButtonCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("AccountNumberViewModel._ctor", "sys_AccountNumberValidateButtonCaption", "Validate");
            ChangeInputButtonCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("AccountNumberViewModel._ctor", "sys_ChangeAccountNumberButtonCaption", "Change Account");
            AccountNumberLabelCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("AccountNumberViewModel._ctor", "sys_AccountNumberLabelCaption", "Account Number");
            CanNext = false;
        }

        public bool KeyboardGridIsVisible => !AccountDetailsFrameIsVisible;

        public bool AccountDetailsFrameIsVisible
        {
            get => accountNameFrameIsVisible;
            set
            {
                accountNameFrameIsVisible = value;
                NotifyOfPropertyChange(() => KeyboardGridIsVisible);
                NotifyOfPropertyChange(nameof(AccountDetailsFrameIsVisible));
                NotifyOfPropertyChange(() => AccountEntryFrameIsVisible);
            }
        }

        public bool AccountEntryFrameIsVisible => !AccountDetailsFrameIsVisible;

        public string AccountDetailsTitleCaption
        {
            get => accountDetailsTitleCaption;
            set
            {
                accountDetailsTitleCaption = value;
                NotifyOfPropertyChange(nameof(AccountDetailsTitleCaption));
            }
        }

        public string AccountNameLabelCaption
        {
            get => accountNameLabelCaption;
            set
            {
                accountNameLabelCaption = value;
                NotifyOfPropertyChange(nameof(AccountNameLabelCaption));
            }
        }

        public string ValidateButtonCaption
        {
            get => validateButtonCaption;
            set
            {
                validateButtonCaption = value;
                NotifyOfPropertyChange(nameof(ValidateButtonCaption));
            }
        }

        public string ChangeInputButtonCaption
        {
            get => changeInputButtonCaption;
            private set
            {
                changeInputButtonCaption = value;
                NotifyOfPropertyChange(nameof(ChangeInputButtonCaption));
            }
        }

        public string AccountNumberLabelCaption
        {
            get => accountNumberLabelCaption;
            private set
            {
                accountNumberLabelCaption = value;
                NotifyOfPropertyChange(nameof(AccountNumberLabelCaption));
            }
        }

        public string AccountName
        {
            get => accountName;
            set
            {
                accountName = value;
                NotifyOfPropertyChange(nameof(AccountName));
            }
        }

        public string AccountNumber
        {
            get => accountNumber;
            set
            {
                accountNumber = value;
                NotifyOfPropertyChange(nameof(AccountNumber));
            }
        }

        public override void Next()
        {
            ApplicationViewModel.CurrentSession.AccountVerified = true;
            base.Next();
        }

        public bool CanValidate
        {
            get
            {
                ApplicationViewModel applicationViewModel = ApplicationViewModel;
                int num1;
                if (applicationViewModel == null)
                {
                    num1 = 0;
                }
                else
                {
                    int? validationTries = applicationViewModel.CurrentTransaction?.ValidationTries;
                    int num2 = 3;
                    num1 = validationTries.GetValueOrDefault() < num2 & validationTries.HasValue ? 1 : 0;
                }
                return num1 != 0;
            }
        }

        public void Validate()
        {
            ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false
            };
            backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
            backgroundWorker.RunWorkerAsync();
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool flag = false;
            try
            {
                ApplicationViewModel applicationViewModel1 = ApplicationViewModel;
                int? nullable1;
                int num1;
                if (applicationViewModel1 == null)
                {
                    num1 = 0;
                }
                else
                {
                    nullable1 = applicationViewModel1.CurrentTransaction?.ValidationTries;
                    int num2 = 3;
                    num1 = nullable1.GetValueOrDefault() < num2 & nullable1.HasValue ? 1 : 0;
                }
                if (num1 != 0 && ClientValidation(CustomerInput))
                {
                    if (Task.Run(() => ValidateAsync(CustomerInput)).Result)
                    {
                        AccountName = ApplicationViewModel.CurrentTransaction.AccountName;
                        AccountNumber = ApplicationViewModel.CurrentTransaction.AccountNumber;
                        AccountDetailsFrameIsVisible = true;
                        flag = true;
                        ApplicationViewModel.Log.Info(nameof(AccountNumberViewModel), "SUCCESS", "Validate", "Account Number '{0}' = Account Name '{1}'", new object[2]
                        {
               CustomerInput,
               AccountName
                        });
                    }
                    else
                    {
                        int validationTries = ApplicationViewModel.CurrentTransaction.ValidationTries;
                        ++ApplicationViewModel.CurrentTransaction.ValidationTries;
                        ApplicationViewModel.Log.Warning(nameof(AccountNumberViewModel), "FAILED", "Validate", "Validation of Account Number '{0}' Failed. Incrementing attempts from {1} to {2}", new object[3]
                        {
               CustomerInput,
               validationTries,
               ApplicationViewModel.CurrentTransaction.ValidationTries
                        });
                        ErrorText += string.Format(" {0} attempt(s) left.", 3 - ApplicationViewModel.CurrentTransaction.ValidationTries);
                    }
                }
                ApplicationViewModel applicationViewModel2 = ApplicationViewModel;
                int num3;
                if (applicationViewModel2 == null)
                {
                    num3 = 0;
                }
                else
                {
                    nullable1 = applicationViewModel2.CurrentTransaction?.ValidationTries;
                    int num4 = 3;
                    num3 = nullable1.GetValueOrDefault() >= num4 & nullable1.HasValue ? 1 : 0;
                }
                if (num3 != 0)
                {
                    DepositorLogger log = ApplicationViewModel.Log;
                    object[] objArray = new object[1];
                    ApplicationViewModel applicationViewModel3 = ApplicationViewModel;
                    int? nullable2;
                    if (applicationViewModel3 == null)
                    {
                        nullable1 = new int?();
                        nullable2 = nullable1;
                    }
                    else
                    {
                        AppTransaction currentTransaction = applicationViewModel3.CurrentTransaction;
                        if (currentTransaction == null)
                        {
                            nullable1 = new int?();
                            nullable2 = nullable1;
                        }
                        else
                            nullable2 = new int?(currentTransaction.ValidationTries);
                    }
                    objArray[0] = nullable2;
                    log.Warning(nameof(AccountNumberViewModel), "Block Validation", "Validate", "Validation tries exceeded. Tries = {0}", objArray);
                    ErrorText = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("AccountNumberViewModel.Validate", "sys_ValidateAccountNumberCallsExceededErrorMessage", "Too many validation requests. Kindly restart the transaction.");
                }
                NotifyOfPropertyChange(() => CanValidate);
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(AccountNumberViewModel), ex.GetType().Name, "Validate", ex.MessageString(), Array.Empty<object>());
                throw;
            }
            if (flag)
                CanNext = true;
            else
                CanNext = false;
            ApplicationViewModel.CloseDialog(false);
        }

        public void ChangeInput()
        {
            ClearErrorText();
            AccountDetailsFrameIsVisible = false;
        }
    }
}
