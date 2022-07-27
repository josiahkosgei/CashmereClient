﻿
//.DepositorCustomerScreenBaseViewModel




using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using CashmereDeposit.Utils;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    public class DepositorCustomerScreenBaseViewModel : TimeoutScreenBase
    {
        private GUIScreenText _currentGUIScreenText;
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
        private readonly IGUIScreenRepository _gUIScreenRepository;

        public DepositorCustomerScreenBaseViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required,
          bool enableIdleTimer = true,
          double timeoutInterval = 0.0)
          : base(screenTitle, applicationViewModel, timeoutInterval)
        {
            _gUIScreenRepository = IoC.Get<IGUIScreenRepository>();

            if (applicationViewModel == null)
                throw new NullReferenceException("applicationViewModel cannot be null in DepositorCustomerScreenBaseViewModel.DepositorCustomerScreenBaseViewModel()");
            EnableIdleTimer = enableIdleTimer;
            TimeOutInterval = timeoutInterval > 0.0 ? timeoutInterval : ApplicationViewModel.DeviceConfiguration.USER_SCREEN_TIMEOUT > 0 ? ApplicationViewModel.DeviceConfiguration.USER_SCREEN_TIMEOUT : 30.0;
            applicationViewModel?.SetLanguage();
            Required = required;
            InitialiseScreen();
        }

        public GUIScreenText CurrentGUIScreenText
        {
            get => _currentGUIScreenText;
            set
            {
                _currentGUIScreenText = value;
                NotifyOfPropertyChange((Expression<Func<GUIScreenText>>)(() => CurrentGUIScreenText));
            }
        }

        public string CustomerInput
        {
            get => _customerInput;
            set
            {
                _customerInput = !string.IsNullOrWhiteSpace(value) ? value : ApplicationViewModel?.CurrentGUIScreen?.PrefillText;
                NotifyOfPropertyChange(() => CustomerInput);
            }
        }

        public string CancelCaption
        {
            get => _cancelCaption;
            set
            {
                _cancelCaption = value;
                NotifyOfPropertyChange(() => CancelCaption);
            }
        }

        public string NextCaption
        {
            get => _nextCaption;
            set
            {
                _nextCaption = value;
                NotifyOfPropertyChange(() => NextCaption);
            }
        }

        public string BackCaption
        {
            get => _backCaption;
            set
            {
                _backCaption = value;
                NotifyOfPropertyChange(() => BackCaption);
            }
        }

        public string GetPreviousPageCaption
        {
            get => _GetPreviousPage_Caption;
            set
            {
                _GetPreviousPage_Caption = value;
                NotifyOfPropertyChange(() => GetPreviousPageCaption);
            }
        }

        public string GetNextPageCaption
        {
            get => _GetNextPageButton_Caption;
            set
            {
                _GetNextPageButton_Caption = value;
                NotifyOfPropertyChange(() => GetNextPageCaption);
            }
        }

        public bool FullInstructionsExpanderIsVisible
        {
            get => _fullInstructionsExpanderIsVisible;
            set
            {
                _fullInstructionsExpanderIsVisible = value;
                NotifyOfPropertyChange(() => FullInstructionsExpanderIsVisible);
            }
        }

        public string FullInstructions
        {
            get => _fullInstructions;
            set
            {
                _fullInstructions = value;
                NotifyOfPropertyChange(() => FullInstructions);
            }
        }

        public string ScreenTitleInstruction
        {
            get => _screenTitleInstruction;
            set
            {
                _screenTitleInstruction = value;
                NotifyOfPropertyChange(() => ScreenTitleInstruction);
            }
        }

        public string ShowFullInstructionsCaption
        {
            get => _ShowFullInstructions_Caption;
            set
            {
                _ShowFullInstructions_Caption = value;
                NotifyOfPropertyChange(() => ShowFullInstructionsCaption);
            }
        }

        public string HideFullInstructionsCaption
        {
            get => _HideFullInstructions_Caption;
            set
            {
                _HideFullInstructions_Caption = value;
                NotifyOfPropertyChange(() => HideFullInstructionsCaption);
            }
        }

        public string FullInstructionsTitle
        {
            get => _FullInstructionsTitle;
            set
            {
                _FullInstructionsTitle = value;
                NotifyOfPropertyChange(() => FullInstructionsTitle);
            }
        }

        public bool AlphanumericKeyboardIsVisible => Keyboard == KeyboardType.ALPHANUMERIC;

        public bool FullAlphanumericKeyboardIsVisible => Keyboard == KeyboardType.FULLALPHANUMERIC;

        public bool NumericKeypadIsVisible => Keyboard == KeyboardType.NUMERIC;

        public KeyboardType Keyboard
        {
            get => _keyboard;
            set
            {
                _keyboard = value;
                RefreshKeyboard();
            }
        }

        public bool CanNext
        {
            get => _canNext;
            set
            {
                _canNext = value;
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public bool CanCancel
        {
            get => _canCancel;
            set
            {
                _canCancel = value;
                NotifyOfPropertyChange(() => CanCancel);
            }
        }

        public bool CanShowFullInstructions
        {
            get => _canShowFullInstructions;
            set
            {
                _canShowFullInstructions = value;
                NotifyOfPropertyChange(() => CanShowFullInstructions);
            }
        }

        public bool Required { get; set; }

        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                NotifyOfPropertyChange(() => ErrorText);
            }
        }

        private void InitialiseScreen()
        {
            CurrentGUIScreenText = ApplicationViewModel?.CurrentGUIScreen?.GUIScreenText;
            var applicationViewModel1 = ApplicationViewModel;
            int? nullable1;
            int num1;
            if (applicationViewModel1 == null)
            {
                num1 = 0;
            }
            else
            {
                var currentGuiScreen = applicationViewModel1.CurrentGUIScreen;
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
                var applicationViewModel2 = ApplicationViewModel;
                int? nullable2;
                if (applicationViewModel2 == null)
                {
                    nullable1 = new int?();
                    nullable2 = nullable1;
                }
                else
                {
                    var currentGuiScreen = applicationViewModel2.CurrentGUIScreen;
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
            //if (CurrentGUIScreenText != null)
            //{
            ScreenTitle = string.IsNullOrWhiteSpace(ScreenTitle) ? ApplicationViewModel.CashmereTranslationService?.TranslateUserText("ScreenTitle", CurrentGUIScreenText?.ScreenTitle, ApplicationViewModel?.CurrentGUIScreen?.Name) : ScreenTitle;
            CancelCaption = ApplicationViewModel.CashmereTranslationService?.TranslateUserText("CancelCaption", CurrentGUIScreenText?.BtnCancelCaption, "Cancel");
            BackCaption = ApplicationViewModel.CashmereTranslationService?.TranslateUserText("BackCaption", CurrentGUIScreenText?.BtnBackCaption, "Back");
            NextCaption = ApplicationViewModel.CashmereTranslationService?.TranslateUserText("NextCaption", CurrentGUIScreenText?.BtnAcceptCaption, "Next");
            GetPreviousPageCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("GetPreviousPageCaption", "sys_GetPreviousPage_Caption", "Prev");
            GetNextPageCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("GetNextPageCaption", "sys_GetNextPage_Caption", "Next");
            FullInstructions = CustomerInputScreenReplace(ApplicationViewModel.CashmereTranslationService?.TranslateUserText("FullInstructions", CurrentGUIScreenText?.FullInstructions, null));
            ScreenTitleInstruction = CustomerInputScreenReplace(ApplicationViewModel.CashmereTranslationService?.TranslateUserText("ScreenTitleInstruction", CurrentGUIScreenText?.ScreenTitleInstruction, null));
            ShowFullInstructionsCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("ShowFullInstructionsCaption", "sys_ShowFullInstructions_Caption", "Help");
            HideFullInstructionsCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("HideFullInstructionsCaption", "sys_Dialog_OK_Caption", "OK");
            FullInstructionsTitle = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("FullInstructionsTitle", "sys_FullInstructionsExpander_TitleCaption", "Instructions");
            if (!string.IsNullOrWhiteSpace(FullInstructions))
                return;
            CanShowFullInstructions = false;

            //}
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
            NotifyOfPropertyChange(() => AlphanumericKeyboardIsVisible);
            NotifyOfPropertyChange(() => FullAlphanumericKeyboardIsVisible);
            NotifyOfPropertyChange(() => NumericKeypadIsVisible);
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

                var guiScreen = _gUIScreenRepository.GetByIdAsync(ApplicationViewModel.CurrentGUIScreen.Id);
                var validationList = guiScreen != null ? guiScreen.GuiScreenListScreens.Where(x => x.GuiScreenList == ApplicationViewModel.CurrentTransaction.TransactionType.TxTypeGUIScreenlist).FirstOrDefault()?.ValidationList : null;
                if (validationList != null)
                {
                    List<ValidationListValidationItem> listValidationItemList;
                    if (validationList == null)
                    {
                        listValidationItemList = null;
                    }
                    else
                    {
                        var listValidationItem = validationList.ValidationListValidationItems;
                        listValidationItemList = listValidationItem != null ? listValidationItem.ToList() : null;
                    }
                    foreach (var listValidationItem in listValidationItemList)
                    {
                        if ((bool)listValidationItem.Enabled && listValidationItem.ValidationItem.Enabled && listValidationItem.ValidationItem.ValidationType.Enabled && listValidationItem.ValidationItem.ValidationType.Name == "Regex" && (listValidationItem.ValidationItem.ValidationItemValues.Count <= 0 || !ClientValidationRules.RegexValidation(valueToValidate, listValidationItem.ValidationItem.ValidationItemValues.OrderBy(x => x.Order).ToList()[0].Value.Replace("\r\n", "\n").Replace("\n", ""))))
                        {
                            try
                            {
                                PrintErrorText(ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".ClientValidation Validation.ErrorMessage", listValidationItem?.ValidationItem?.ValidationText?.ErrorMessage, "Validation Failed"));
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