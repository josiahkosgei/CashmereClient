
//.CustomerPrepopReferenceScreenBase




using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
    public abstract class CustomerPrepopReferenceScreenBase : DepositorCustomerScreenBaseViewModel, ICustomerComboBoxInputScreen
    {
        private GuiScreenListScreen _GuiScreenListScreens;
        private GUIPrepopList _GUIPrepopList;
        private string _EditComboBoxButtonCaption;
        private string _CancelEditComboBoxButtonCaption;
        private bool _ComboBoxGridIsVisible;
        private bool _TextBoxGridIsVisible;
        private bool _KeyboardGridIsVisible;
        private bool _AllowFreeText;
        private bool _IsComboBoxEditMode;
        private bool _EditComboBoxIsVisible;
        private bool _ComboBoxButtonsIsVisible;
        private bool _CancelEditComboBoxIsVisible;

        public ObservableCollection<string> CustomerComboBoxInput { get; set; }

        public string SelectedCustomerComboBoxInput
        {
            get => CustomerInput;
            set
            {
                if (value != null)
                    CustomerInput = value;
                NotifyOfPropertyChange(() => SelectedCustomerComboBoxInput);
            }
        }

        public GuiScreenListScreen GuiScreenListScreens
        {
            get => _GuiScreenListScreens;
            set
            {
                _GuiScreenListScreens = value;
                NotifyOfPropertyChange((Expression<Func<GuiScreenListScreen>>)(() => GuiScreenListScreens));
            }
        }

        public GUIPrepopList GUIPrepopList
        {
            get => _GUIPrepopList;
            set
            {
                _GUIPrepopList = value;
                NotifyOfPropertyChange((Expression<Func<GUIPrepopList>>)(() => GUIPrepopList));
            }
        }

        public string EditComboBoxButtonCaption
        {
            get => _EditComboBoxButtonCaption;
            set
            {
                _EditComboBoxButtonCaption = value;
                NotifyOfPropertyChange(() => EditComboBoxButtonCaption);
            }
        }

        public string CancelEditComboBoxButtonCaption
        {
            get => _CancelEditComboBoxButtonCaption;
            set
            {
                _CancelEditComboBoxButtonCaption = value;
                NotifyOfPropertyChange(() => CancelEditComboBoxButtonCaption);
            }
        }

        public bool ComboBoxGridIsVisible
        {
            get => _ComboBoxGridIsVisible;
            set
            {
                _ComboBoxGridIsVisible = value;
                NotifyOfPropertyChange(() => ComboBoxGridIsVisible);
            }
        }

        public bool TextBoxGridIsVisible
        {
            get => _TextBoxGridIsVisible;
            set
            {
                _TextBoxGridIsVisible = value;
                NotifyOfPropertyChange(() => TextBoxGridIsVisible);
            }
        }

        public bool KeyboardGridIsVisible
        {
            get => _KeyboardGridIsVisible;
            set
            {
                _KeyboardGridIsVisible = value;
                NotifyOfPropertyChange(() => KeyboardGridIsVisible);
            }
        }

        public bool AllowFreeText
        {
            get
            {
                var guiPrepopList = GUIPrepopList;
                return guiPrepopList == null || guiPrepopList.AllowFreeText;
            }
            set
            {
                _AllowFreeText = value;
                ComboBoxButtonsIsVisible = _AllowFreeText;
                NotifyOfPropertyChange(() => AllowFreeText);
            }
        }

        public bool IsComboBoxEditMode
        {
            get => _IsComboBoxEditMode;
            set
            {
                _IsComboBoxEditMode = value;
                KeyboardGridIsVisible = _IsComboBoxEditMode;
                TextBoxGridIsVisible = _IsComboBoxEditMode;
                CancelEditComboBoxIsVisible = _IsComboBoxEditMode;
                ComboBoxGridIsVisible = !_IsComboBoxEditMode;
                EditComboBoxIsVisible = !_IsComboBoxEditMode;
                NotifyOfPropertyChange(() => IsComboBoxEditMode);
            }
        }

        public bool EditComboBoxIsVisible
        {
            get => _EditComboBoxIsVisible;
            set
            {
                if (_EditComboBoxIsVisible == value)
                    return;
                _EditComboBoxIsVisible = value;
                NotifyOfPropertyChange(() => EditComboBoxIsVisible);
            }
        }

        public bool ComboBoxButtonsIsVisible
        {
            get => _ComboBoxButtonsIsVisible;
            set
            {
                if (_ComboBoxButtonsIsVisible == value)
                    return;
                _ComboBoxButtonsIsVisible = value;
                NotifyOfPropertyChange(() => ComboBoxButtonsIsVisible);
            }
        }

        public bool CancelEditComboBoxIsVisible
        {
            get => _CancelEditComboBoxIsVisible;
            set
            {
                if (_CancelEditComboBoxIsVisible == value)
                    return;
                _CancelEditComboBoxIsVisible = value;
                NotifyOfPropertyChange(() => CancelEditComboBoxIsVisible);
            }
        }

        public CustomerPrepopReferenceScreenBase(
          string customerInput,
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required,
          bool enableIdleTimer = true,
          double timeoutInterval = 0.0)
          : base(screenTitle, applicationViewModel, required, enableIdleTimer, timeoutInterval)
        {
            var referenceScreenBase = this;
            IsComboBoxEditMode = true;
            CustomerInput = customerInput;
            GuiScreenListScreens = applicationViewModel.CurrentGUIScreen.GuiScreenListScreens.FirstOrDefault(x => x.GuiScreenList == applicationViewModel.CurrentTransaction.TransactionType.TxTypeGUIScreenlistNavigation.Id);
            EditComboBoxButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof(EditComboBoxButtonCaption), "sys_EditComboBoxButtonCaption", "Edit");
            CancelEditComboBoxButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof(CancelEditComboBoxButtonCaption), "sys_CancelEditComboBoxButtonCaption", "Choose");
            if ((bool)!GuiScreenListScreens?.GUIPrepopList?.Enabled)
                return;
            var screenListScreen = GuiScreenListScreens;
            int num1;
            if (screenListScreen == null)
            {
                num1 = 0;
            }
            else
            {
                var count = screenListScreen.GUIPrepopList?.GUIPrepopListItems?.Count;
                var num2 = 0;
                num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
            }
            if (num1 == 0)
                return;
            GUIPrepopList = GuiScreenListScreens.GUIPrepopList;
            var guiPrepopList = GUIPrepopList;
            List<string> list;
            if (guiPrepopList == null)
            {
                list = null;
            }
            else
            {
                var guiPrepopListItem = guiPrepopList.GUIPrepopListItems;
                if (guiPrepopListItem == null)
                {
                    list = null;
                }
                else
                {
                    var source1 = guiPrepopListItem.Where(x => (bool)x.GUIPrepopItem.Enabled);
                    if (source1 == null)
                    {
                        list = null;
                    }
                    else
                    {
                        var source2 = source1.OrderBy(x => x.ListOrder);
                        list = source2 != null ? source2.Select(x => ApplicationViewModel.CashmereTranslationService.TranslateUserText("CustomerPrepopReferenceScreenBase.CustomerComboBoxInput", new Guid?(x.GUIPrepopItem.Value), "Empty ListItem")).ToList() : null;
                    }
                }
            }
            CustomerComboBoxInput = new ObservableCollection<string>(list);
            SetComboBoxDefault();
            AllowFreeText = GUIPrepopList.AllowFreeText;
            int num3;
            if (!IsInputNull(CustomerInput))
            {
                var customerComboBoxInput = CustomerComboBoxInput;
                // ISSUE: explicit non-virtual call
                num3 = customerComboBoxInput != null ? !customerComboBoxInput.Contains(CustomerInput) ? 1 : 0 : 0;
            }
            else
                num3 = 0;
            IsComboBoxEditMode = num3 != 0;
            NotifyOfPropertyChange(() => CanEditComboBox);
            NotifyOfPropertyChange(() => CanCancelEditComboBox);
        }

        protected void SetComboBoxDefault(bool overrideWithDefault = false)
        {
            var guiPrepopList = GUIPrepopList;
            if ((guiPrepopList != null ? (bool)guiPrepopList.UseDefault ? 1 : 0 : 0) == 0 || !overrideWithDefault && !string.IsNullOrWhiteSpace(CustomerInput))
                return;
            SelectedCustomerComboBoxInput = CustomerComboBoxInput[GUIPrepopList.DefaultIndex.Clamp(0, GUIPrepopList.GUIPrepopListItems.Count - 1)];
        }

        public bool CanEditComboBox => AllowFreeText && !IsComboBoxEditMode;

        public void EditComboBox()
        {
            IsComboBoxEditMode = true;
            NotifyOfPropertyChange(() => CanEditComboBox);
            NotifyOfPropertyChange(() => CanCancelEditComboBox);
        }

        public bool CanCancelEditComboBox => AllowFreeText && IsComboBoxEditMode;

        public void CancelEditComboBox()
        {
            IsComboBoxEditMode = false;
            SetComboBoxDefault(true);
            NotifyOfPropertyChange(() => CanEditComboBox);
            NotifyOfPropertyChange(() => CanCancelEditComboBox);
        }
    }
}
