
// Type: CashmereDeposit.ViewModels.DepositorCustomerScreenBaseViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

using CashmereDeposit.Interfaces;
using CashmereDeposit.Utils;

namespace CashmereDeposit.ViewModels
{
    public class DepositorCustomerScreenBaseViewModel : TimeoutScreenBase
    {
        private GuiScreenText _currentGUIScreenText;
        private string _customerInput;
        private string _cancelCaption;
        private string _nextCaption;
        private string _backCaption;
        private string _GetPreviousPage_Caption;
        private string _GetNextPageButton_Caption;
        private bool _fullInstructionsExpanderIsVisible;
        private string _fullInstructions;
        private string _screenTitleInstruction;
        private string _ShowFullInstructions_Caption;
        private string _HideFullInstructions_Caption;
        private string _FullInstructionsTitle;
        private KeyboardType _keyboard = KeyboardType.ALPHANUMERIC;
        private string _errorText;
        private bool _canNext = true;
        private bool _canCancel = true;
        private bool _canShowFullInstructions = true;

        public DepositorCustomerScreenBaseViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required,
          bool enableIdleTimer = true,
          double timeoutInterval = 0.0)
          : base(screenTitle, applicationViewModel, timeoutInterval)
        {
            if (applicationViewModel == null)
                throw new NullReferenceException("applicationViewModel cannot be null in DepositorCustomerScreenBaseViewModel.DepositorCustomerScreenBaseViewModel()");
            EnableIdleTimer = enableIdleTimer;
            TimeOutInterval = timeoutInterval > 0.0 ? timeoutInterval : (ApplicationViewModel.DeviceConfiguration.USER_SCREEN_TIMEOUT > 0 ? ApplicationViewModel.DeviceConfiguration.USER_SCREEN_TIMEOUT : 30.0);
            applicationViewModel?.SetLanguage();
            Required = required;
            InitialiseScreen();
        }

        public GuiScreenText CurrentGUIScreenText
        {
            get { return _currentGUIScreenText; }
            set
            {
                _currentGUIScreenText = value;
                NotifyOfPropertyChange((Expression<Func<GuiScreenText>>)(() => CurrentGUIScreenText));
            }
        }

        public string CustomerInput
        {
            get { return _customerInput; }
            set
            {
                _customerInput = !string.IsNullOrWhiteSpace(value) ? value : ApplicationViewModel?.CurrentGUIScreen?.PrefillText;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => CustomerInput));
            }
        }

        public string CancelCaption
        {
            get { return _cancelCaption; }
            set
            {
                _cancelCaption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => CancelCaption));
            }
        }

        public string NextCaption
        {
            get { return _nextCaption; }
            set
            {
                _nextCaption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => NextCaption));
            }
        }

        public string BackCaption
        {
            get { return _backCaption; }
            set
            {
                _backCaption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => BackCaption));
            }
        }

        public string GetPreviousPageCaption
        {
            get { return _GetPreviousPage_Caption; }
            set
            {
                _GetPreviousPage_Caption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => GetPreviousPageCaption));
            }
        }

        public string GetNextPageCaption
        {
            get { return _GetNextPageButton_Caption; }
            set
            {
                _GetNextPageButton_Caption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => GetNextPageCaption));
            }
        }

        public bool FullInstructionsExpanderIsVisible
        {
            get { return _fullInstructionsExpanderIsVisible; }
            set
            {
                _fullInstructionsExpanderIsVisible = value;
                NotifyOfPropertyChange((Expression<Func<bool>>)(() => FullInstructionsExpanderIsVisible));
            }
        }

        public string FullInstructions
        {
            get { return _fullInstructions; }
            set
            {
                _fullInstructions = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => FullInstructions));
            }
        }

        public string ScreenTitleInstruction
        {
            get { return _screenTitleInstruction; }
            set
            {
                _screenTitleInstruction = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => ScreenTitleInstruction));
            }
        }

        public string ShowFullInstructionsCaption
        {
            get { return _ShowFullInstructions_Caption; }
            set
            {
                _ShowFullInstructions_Caption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => ShowFullInstructionsCaption));
            }
        }

        public string HideFullInstructionsCaption
        {
            get { return _HideFullInstructions_Caption; }
            set
            {
                _HideFullInstructions_Caption = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => HideFullInstructionsCaption));
            }
        }

        public string FullInstructionsTitle
        {
            get { return _FullInstructionsTitle; }
            set
            {
                _FullInstructionsTitle = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => FullInstructionsTitle));
            }
        }

        public bool AlphanumericKeyboardIsVisible
        {
            get { return Keyboard == KeyboardType.ALPHANUMERIC; }
        }

        public bool FullAlphanumericKeyboardIsVisible
        {
            get { return Keyboard == KeyboardType.FULLALPHANUMERIC; }
        }

        public bool NumericKeypadIsVisible
        {
            get { return Keyboard == KeyboardType.NUMERIC; }
        }

        public KeyboardType Keyboard
        {
            get { return _keyboard; }
            set
            {
                _keyboard = value;
                RefreshKeyboard();
            }
        }

        public bool CanNext
        {
            get { return _canNext; }
            set
            {
                _canNext = value;
                NotifyOfPropertyChange((Expression<Func<bool>>)(() => CanNext));
            }
        }

        public bool CanCancel
        {
            get { return _canCancel; }
            set
            {
                _canCancel = value;
                NotifyOfPropertyChange((Expression<Func<bool>>)(() => CanCancel));
            }
        }

        public bool CanShowFullInstructions
        {
            get { return _canShowFullInstructions; }
            set
            {
                _canShowFullInstructions = value;
                NotifyOfPropertyChange((Expression<Func<bool>>)(() => CanShowFullInstructions));
            }
        }

        public bool Required { get; set; }

        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                NotifyOfPropertyChange((Expression<Func<string>>)(() => ErrorText));
            }
        }

        private void InitialiseScreen()
        {
            CurrentGUIScreenText = ApplicationViewModel?.CurrentGUIScreen?.GuiScreenText;
            ApplicationViewModel applicationViewModel1 = ApplicationViewModel;
            int? nullable1;
            int num1;
            if (applicationViewModel1 == null)
            {
                num1 = 0;
            }
            else
            {
                GuiScreen currentGuiScreen = applicationViewModel1.CurrentGUIScreen;
                if (currentGuiScreen == null)
                {
                    num1 = 0;
                }
                else
                {
                    nullable1 = currentGuiScreen.Keyboard;
                    num1 = nullable1.HasValue ? 1 : 0;
                }
            }
            int num2;
            if (num1 == 0)
            {
                num2 = 2;
            }
            else
            {
                ApplicationViewModel applicationViewModel2 = ApplicationViewModel;
                int? nullable2;
                if (applicationViewModel2 == null)
                {
                    nullable1 = new int?();
                    nullable2 = nullable1;
                }
                else
                {
                    GuiScreen currentGuiScreen = applicationViewModel2.CurrentGUIScreen;
                    if (currentGuiScreen == null)
                    {
                        nullable1 = new int?();
                        nullable2 = nullable1;
                    }
                    else
                        nullable2 = currentGuiScreen.Keyboard;
                }
                nullable1 = nullable2;
                num2 = nullable1.Value;
            }
            Keyboard = (KeyboardType)num2;
            ScreenTitle = string.IsNullOrWhiteSpace(ScreenTitle) ? ApplicationViewModel.CashmereTranslationService?.TranslateUserText("ScreenTitle", CurrentGUIScreenText?.ScreenTitleId, ApplicationViewModel?.CurrentGUIScreen?.Name) :this.ScreenTitle;
            CancelCaption = ApplicationViewModel.CashmereTranslationService?.TranslateUserText("CancelCaption", CurrentGUIScreenText?.BtnCancelCaptionId, "Cancel");
            BackCaption = ApplicationViewModel.CashmereTranslationService?.TranslateUserText("BackCaption", CurrentGUIScreenText?.BtnBackCaptionId, "Back");
            NextCaption = ApplicationViewModel.CashmereTranslationService?.TranslateUserText("NextCaption", CurrentGUIScreenText?.BtnAcceptCaptionId, "Next");
            GetPreviousPageCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("GetPreviousPageCaption", "sys_GetPreviousPage_Caption", "Prev");
            GetNextPageCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("GetNextPageCaption", "sys_GetNextPage_Caption", "Next");
            FullInstructions = CustomerInputScreenReplace(ApplicationViewModel.CashmereTranslationService?.TranslateUserText("FullInstructions", CurrentGUIScreenText?.FullInstructionsId, null));
            ScreenTitleInstruction = CustomerInputScreenReplace(ApplicationViewModel.CashmereTranslationService?.TranslateUserText("ScreenTitleInstruction", CurrentGUIScreenText?.ScreenTitleInstructionId, null));
            ShowFullInstructionsCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("ShowFullInstructionsCaption", "sys_ShowFullInstructions_Caption", "Help");
            HideFullInstructionsCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("HideFullInstructionsCaption", "sys_Dialog_OK_Caption", "OK");
            FullInstructionsTitle = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("FullInstructionsTitle", "sys_FullInstructionsExpander_TitleCaption", "Instructions");
            if (!string.IsNullOrWhiteSpace(FullInstructions))
                return;
            CanShowFullInstructions = false;
        }

        protected string CustomerInputScreenReplace(string s)
        {
            if (s == null)
                return null;
            return s.CashmereReplace(ApplicationViewModel)?.Replace("{screen_title}", ScreenTitle)?.Replace("{btn_accept_caption}", NextCaption)?.Replace("{btn_back_caption}", BackCaption)?.Replace("{btn_cancel_caption}", CancelCaption)?.Replace("{btn_page_next_caption}", ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_GetNextPage_Caption", "More"))?.Replace("{btn_page_previous_caption}", ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_GetPreviousPageButton_Caption", "Prev"))?.Replace("{btn_escrow_reject_caption}", ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_EscrowRejectButton_Caption", "Reject"))?.Replace("{btn_escrow_drop_caption}", ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof(CustomerInputScreenReplace), "sys_EscrowDropButton_Caption", "Drop"));
        }

        private void RefreshKeyboard()
        {
            NotifyOfPropertyChange((Expression<Func<KeyboardType>>)(() => Keyboard));
            NotifyOfPropertyChange((Expression<Func<bool>>)(() => AlphanumericKeyboardIsVisible));
            NotifyOfPropertyChange((Expression<Func<bool>>)(() => FullAlphanumericKeyboardIsVisible));
            NotifyOfPropertyChange((Expression<Func<bool>>)(() => NumericKeypadIsVisible));
        }

        internal void PrintErrorText(string errorText)
        {
            ErrorText = string.Format("[{0:HH:mm:ss.fff}] {1}", DateTime.Now, errorText);
        }

        internal void ClearErrorText()
        {
            ErrorText = "";
        }

        internal bool ClientValidation(string valueToValidate)
        {
            try
            {
                if (Required && IsInputNull(valueToValidate))
                {
                    PrintErrorText(ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(ClientValidation), "sys_ValidationRequiredFieldError", "Field is required")?.ToUpperInvariant());
                    return false;
                }

                using DepositorDBContext depositorDbContext = new DepositorDBContext();
                GuiScreen guiScreen = depositorDbContext.GuiScreens.Where(z => z.Id == ApplicationViewModel.CurrentGUIScreen.Id).FirstOrDefault();
                ValidationList validationList = guiScreen != null ? guiScreen.GuiScreenListScreens.Where(x => x.GuiScreenListId == ApplicationViewModel.CurrentTransaction.TransactionType.TxTypeGuiScreenListId).FirstOrDefault()?.ValidationList : null;
                if (validationList != null)
                {
                    List<ValidationListValidationItem> listValidationItemList;
                    if (validationList == null)
                    {
                        listValidationItemList = null;
                    }
                    else
                    {
                        ICollection<ValidationListValidationItem> listValidationItem = validationList.ValidationListValidationItems;
                        listValidationItemList = listValidationItem != null ? listValidationItem.ToList() : null;
                    }
                    foreach (ValidationListValidationItem listValidationItem in listValidationItemList)
                    {
                        if (listValidationItem.Enabled && listValidationItem.ValidationItem.Enabled && (listValidationItem.ValidationItem.ValidationType.Enabled && listValidationItem.ValidationItem.ValidationType.Name == "Regex") && (listValidationItem.ValidationItem.ValidationItemValues.Count <= 0 || !ClientValidationRules.RegexValidation(valueToValidate, listValidationItem.ValidationItem.ValidationItemValues.OrderBy(x => x.Order).ToList()[0].Value.Replace("\r\n", "\n").Replace("\n", ""))))
                        {
                            try
                            {
                                PrintErrorText(ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".ClientValidation Validation.ErrorMessage", listValidationItem?.ValidationItem?.ValidationText?.ErrorMessageId, "Validation Failed"));
                            }
                            catch (Exception ex)
                            {
                                ApplicationViewModel.Log.WarningFormat(GetType().Name, "Validation Error Message", "TranslationError", "Failed to print error message for ValidationItem {0}", listValidationItem?.ValidationItem?.Name);
                                PrintErrorText("Validation Failed");
                            }
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 31, ApplicationErrorConst.ERROR_INVALID_DATA.ToString(), "Error during regex validation: {0}", ex.MessageString());
                PrintErrorText("Invalid Regex Exception. Contact Administrator");
                return false;
            }
        }

        protected bool IsInputNull(string valueToValidate)
        {
            return string.IsNullOrWhiteSpace(valueToValidate) || string.Equals(valueToValidate,
                ApplicationViewModel?.CurrentGUIScreen?.PrefillText, StringComparison.InvariantCultureIgnoreCase);
        }

        public void ShowFullInstructions()
        {
            FullInstructionsExpanderIsVisible = true;
        }

        public void HideFullInstructions()
        {
            FullInstructionsExpanderIsVisible = false;
        }
    }
}
