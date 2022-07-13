
// Type: CashmereDeposit.ViewModels.CustomerPrepopReferenceScreenBase

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


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
        get { return CustomerInput; }
        set
      {
        if (value != null)
          CustomerInput = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => SelectedCustomerComboBoxInput));
      }
    }

    public GuiScreenListScreen GuiScreenListScreens
    {
        get { return _GuiScreenListScreens; }
        set
      {
        _GuiScreenListScreens = value;
        NotifyOfPropertyChange((Expression<Func<GuiScreenListScreen>>) (() => GuiScreenListScreens));
      }
    }

    public GUIPrepopList GUIPrepopList
    {
        get { return _GUIPrepopList; }
        set
      {
        _GUIPrepopList = value;
        NotifyOfPropertyChange((Expression<Func<GUIPrepopList>>) (() => GUIPrepopList));
      }
    }

    public string EditComboBoxButtonCaption
    {
        get { return _EditComboBoxButtonCaption; }
        set
      {
        _EditComboBoxButtonCaption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => EditComboBoxButtonCaption));
      }
    }

    public string CancelEditComboBoxButtonCaption
    {
        get { return _CancelEditComboBoxButtonCaption; }
        set
      {
        _CancelEditComboBoxButtonCaption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => CancelEditComboBoxButtonCaption));
      }
    }

    public bool ComboBoxGridIsVisible
    {
        get { return _ComboBoxGridIsVisible; }
        set
      {
        _ComboBoxGridIsVisible = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => ComboBoxGridIsVisible));
      }
    }

    public bool TextBoxGridIsVisible
    {
        get { return _TextBoxGridIsVisible; }
        set
      {
        _TextBoxGridIsVisible = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => TextBoxGridIsVisible));
      }
    }

    public bool KeyboardGridIsVisible
    {
        get { return _KeyboardGridIsVisible; }
        set
      {
        _KeyboardGridIsVisible = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => KeyboardGridIsVisible));
      }
    }

    public bool AllowFreeText
    {
      get
      {
        GUIPrepopList guiPrepopList = GUIPrepopList;
        return guiPrepopList == null || guiPrepopList.AllowFreeText;
      }
      set
      {
        _AllowFreeText = value;
        ComboBoxButtonsIsVisible = _AllowFreeText;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => AllowFreeText));
      }
    }

    public bool IsComboBoxEditMode
    {
        get { return _IsComboBoxEditMode; }
        set
      {
        _IsComboBoxEditMode = value;
        KeyboardGridIsVisible = _IsComboBoxEditMode;
        TextBoxGridIsVisible = _IsComboBoxEditMode;
        CancelEditComboBoxIsVisible = _IsComboBoxEditMode;
        ComboBoxGridIsVisible = !_IsComboBoxEditMode;
        EditComboBoxIsVisible = !_IsComboBoxEditMode;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => IsComboBoxEditMode));
      }
    }

    public bool EditComboBoxIsVisible
    {
        get { return _EditComboBoxIsVisible; }
        set
      {
        if (_EditComboBoxIsVisible == value)
          return;
        _EditComboBoxIsVisible = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => EditComboBoxIsVisible));
      }
    }

    public bool ComboBoxButtonsIsVisible
    {
        get { return _ComboBoxButtonsIsVisible; }
        set
      {
        if (_ComboBoxButtonsIsVisible == value)
          return;
        _ComboBoxButtonsIsVisible = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => ComboBoxButtonsIsVisible));
      }
    }

    public bool CancelEditComboBoxIsVisible
    {
        get { return _CancelEditComboBoxIsVisible; }
        set
      {
        if (_CancelEditComboBoxIsVisible == value)
          return;
        _CancelEditComboBoxIsVisible = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => CancelEditComboBoxIsVisible));
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
      CustomerPrepopReferenceScreenBase referenceScreenBase = this;
      IsComboBoxEditMode = true;
      CustomerInput = customerInput;
      GuiScreenListScreens = applicationViewModel.CurrentGUIScreen.GuiScreenListScreens.FirstOrDefault(x => x.GuiScreenList == applicationViewModel.CurrentTransaction.TransactionType.TxTypeGUIScreenlistNavigation.Id);
      EditComboBoxButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof (EditComboBoxButtonCaption), "sys_EditComboBoxButtonCaption", "Edit");
      CancelEditComboBoxButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(nameof (CancelEditComboBoxButtonCaption), "sys_CancelEditComboBoxButtonCaption", "Choose");
      if ((bool)!GuiScreenListScreens?.GUIPrepopList?.Enabled)
        return;
      GuiScreenListScreen screenListScreen = GuiScreenListScreens;
      int num1;
      if (screenListScreen == null)
      {
        num1 = 0;
      }
      else
      {
        int? count = screenListScreen.GUIPrepopList?.GUIPrepopListItems?.Count;
        int num2 = 0;
        num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
      }
      if (num1 == 0)
        return;
      GUIPrepopList = GuiScreenListScreens.GUIPrepopList;
      GUIPrepopList guiPrepopList = GUIPrepopList;
      List<string> list;
      if (guiPrepopList == null)
      {
        list = null;
      }
      else
      {
        ICollection<GUIPrepopListItem> guiPrepopListItem = guiPrepopList.GUIPrepopListItems;
        if (guiPrepopListItem == null)
        {
          list = null;
        }
        else
        {
          IEnumerable<GUIPrepopListItem> source1 = guiPrepopListItem.Where(x => (bool)x.GUIPrepopItem.Enabled);
          if (source1 == null)
          {
            list = null;
          }
          else
          {
            IOrderedEnumerable<GUIPrepopListItem> source2 = source1.OrderBy(x => x.ListOrder);
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
        ObservableCollection<string> customerComboBoxInput = CustomerComboBoxInput;
        // ISSUE: explicit non-virtual call
        num3 = customerComboBoxInput != null ? (! (customerComboBoxInput.Contains(CustomerInput)) ? 1 : 0) : 0;
      }
      else
        num3 = 0;
      IsComboBoxEditMode = num3 != 0;
      NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanEditComboBox));
      NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanCancelEditComboBox));
    }

    protected void SetComboBoxDefault(bool overrideWithDefault = false)
    {
      GUIPrepopList guiPrepopList = GUIPrepopList;
      if ((guiPrepopList != null ? ((bool)guiPrepopList.UseDefault ? 1 : 0) : 0) == 0 || !overrideWithDefault && !string.IsNullOrWhiteSpace(CustomerInput))
        return;
      SelectedCustomerComboBoxInput = CustomerComboBoxInput[GUIPrepopList.DefaultIndex.Clamp(0, GUIPrepopList.GUIPrepopListItems.Count - 1)];
    }

    public bool CanEditComboBox
    {
        get { return AllowFreeText && !IsComboBoxEditMode; }
    }

    public void EditComboBox()
    {
      IsComboBoxEditMode = true;
      NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanEditComboBox));
      NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanCancelEditComboBox));
    }

    public bool CanCancelEditComboBox
    {
        get { return AllowFreeText && IsComboBoxEditMode; }
    }

    public void CancelEditComboBox()
    {
      IsComboBoxEditMode = false;
      SetComboBoxDefault(true);
      NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanEditComboBox));
      NotifyOfPropertyChange((Expression<Func<bool>>) (() => CanCancelEditComboBox));
    }
  }
}
