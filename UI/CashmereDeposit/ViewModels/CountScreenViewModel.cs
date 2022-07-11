
// Type: CashmereDeposit.ViewModels.CountScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using CashAccSysDeviceManager;
using Cashmere.Library.Standard.Statuses;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Cashmere.Library.CashmereDataAccess.Entities;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  [Guid("D0E649AB-3379-4BED-8A93-9FDF29FF6AA0")]
  public class CountScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
    private string _CountButton_Caption;
    private string _EscrowDropButton_Caption;
    private string _EscrowRejectButton_Caption;
    private string _CountScreenTotal_Caption;
    private bool _lastCanNext;
    private bool _lastCanCancel;
    private bool _lastCanCount;
    private bool _lastCanEscrowDrop;
    private bool _lastCanEscrowReject;

    public object TimeoutLock { get; set; } = new object();

    private bool TimeoutMode { get; set; }

    private bool TimeoutModeComplete { get; set; }

    public AppTransaction CurrentTransaction { get; set; }

    public bool AutoDropChecked { get; set; }

    public bool CanAutoDropChecked
    {
      get
      {
        DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
        return deviceConfiguration == null || deviceConfiguration.AUTODROP_CHANGE_ALLOWED;
      }
    }

    public bool AutoCountChecked { get; set; }

    public bool CanAutoCountChecked
    {
      get
      {
        DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
        return deviceConfiguration == null || deviceConfiguration.AUTOCOUNT_CHANGE_ALLOWED;
      }
    }

    public string CountButtonCaption
    {
        get { return _CountButton_Caption; }
        set
      {
        _CountButton_Caption = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>) (() => CountButtonCaption));
      }
    }

    public string EscrowDropButtonCaption
    {
        get { return _EscrowDropButton_Caption; }
        set
      {
        _EscrowDropButton_Caption = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>) (() => EscrowDropButtonCaption));
      }
    }

    public string EscrowRejectButtonCaption
    {
        get { return _EscrowRejectButton_Caption; }
        set
      {
        _EscrowRejectButton_Caption = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>) (() => EscrowRejectButtonCaption));
      }
    }

    public string CountScreenTotalCaption
    {
        get { return _CountScreenTotal_Caption; }
        set
      {
        _CountScreenTotal_Caption = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>) (() => CountScreenTotalCaption));
      }
    }

    public CountScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
      CurrentTransaction = applicationViewModel.CurrentTransaction;
      ApplicationViewModel.DeviceStatusChangedEvent += new EventHandler<DeviceStatusChangedEventArgs>(ApplicationViewModel_DeviceStatusChangedEvent);
      ApplicationViewModel.CountPauseEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountPauseEvent);
      ApplicationViewModel.CountEndEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountEndEvent);
      ApplicationViewModel.TransactionStatusEvent += new EventHandler<DeviceTransactionResult>(ApplicationViewModel_TransactionStatusEvent);
      ApplicationViewModel.NotifyCurrentTransactionStatusChangedEvent += new EventHandler<EventArgs>(ApplicationViewModel_NotifyCurrentTransactionStatusChanged);
      ApplicationViewModel.DeviceManager.PropertyChanged += new PropertyChangedEventHandler(DeviceManager_PropertyChanged);
      ApplicationViewModel.DeviceManager.CashAccSysSerialFix.PropertyChanged += new PropertyChangedEventHandler(CashAccSysSerialFix_PropertyChanged);
      DeviceConfiguration deviceConfiguration1 = ApplicationViewModel.DeviceConfiguration;
      AutoDropChecked = deviceConfiguration1 != null && deviceConfiguration1.AUTODROP_CHECKED;
      DeviceConfiguration deviceConfiguration2 = ApplicationViewModel.DeviceConfiguration;
      AutoCountChecked = deviceConfiguration2 != null && deviceConfiguration2.AUTOCOUNT_CHECKED;
      InitialiseScreen();
    }

    private void CashAccSysSerialFix_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    private void HandleTimeoutOperation()
    {
      if (!TimeoutMode || TimeoutModeComplete)
        return;
      if (ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation == DE50OperatingState.Storing_start_request)
      {
        ApplicationViewModel.Log.Info(nameof (CountScreenViewModel), "Processing", "DoCancelTransactionOnTimeout", "cash in the escrow detected. Dropping notes...");
        EscrowDrop();
      }
      else if (ApplicationViewModel.DeviceManager.ControllerState == ControllerState.DROP)
      {
        EscrowReject();
      }
      else
      {
        if (!ApplicationViewModel.CanTransactionEnd || ApplicationViewModel.DeviceManager.ControllerState != ControllerState.IDLE || ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation != DE50OperatingState.Waiting && ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation != DE50OperatingState.Counting_start_request)
          return;
        ApplicationViewModel.Log.Info(nameof (CountScreenViewModel), "Processing", "DoCancelTransactionOnTimeout", "no cash in escrow, end the transaction");
        base.DoCancelTransactionOnTimeout();
        TimeoutModeComplete = true;
      }
    }

    private void DeviceManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!e.PropertyName.Equals("ClearHopperRequest"))
        return;
      string str = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("CountScreenViewModel.DeviceManager_PropertyChanged", "sys_ClearHopperRequest_Caption", "Kindly remove notes from hopper.", ApplicationViewModel.CurrentLanguage);
      if (ApplicationViewModel.DeviceManager.ClearHopperRequest)
      {
        string errorText = ErrorText;
        ErrorText = ((errorText != null ? (errorText.Length > 0 ? 1 : 0) : 0) != 0 ? "\r\n" : "") + str;
        ApplicationViewModel.Log.Warning(nameof (CountScreenViewModel), "Notes in hopper", "Clear Notes", str);
      }
      else
        ErrorText = ErrorText.Replace(str, "");
    }

    private void InitialiseScreen()
    {
      EscrowDropButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("EscrowDropButtonCaption", "sys_EscrowDropButton_Caption", "Drop");
      EscrowRejectButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("EscrowRejectButtonCaption", "sys_EscrowRejectButton_Caption", "Reject");
      CountButtonCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("CountButtonCaption", "sys_CountButton_Caption", "Count");
      CountScreenTotalCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("CountScreenTotalCaption", "sys_CountScreenTotal_Caption", "Total");
    }

    private void ApplicationViewModel_NotifyCurrentTransactionStatusChanged(
      object sender,
      EventArgs e)
    {
        NotifyCurrentTransactionStatusChanged();
    }

    private void NotifyCurrentTransactionStatusChanged()
    {
      int num = (int) IsCountWithinTheLimits();
      NotifyOfPropertyChange("CurrentTransaction");
      NotifyOfPropertyChange("CanCount");
      NotifyOfPropertyChange("CanPauseCount");
      NotifyOfPropertyChange("CanEscrowDrop");
      NotifyOfPropertyChange("CanEscrowReject");
      NotifyOfPropertyChange("CanNext");
      NotifyOfPropertyChange("CanCancel");
      HandleTimeoutOperation();
    }

    private void ApplicationViewModel_DeviceStatusChangedEvent(
      object sender,
      DeviceStatusChangedEventArgs e)
    {
    }

    private void ApplicationViewModel_TransactionStatusEvent(
      object sender,
      DeviceTransactionResult e)
    {
    }

    private void ApplicationViewModel_CountPauseEvent(object sender, DeviceTransactionResult e)
    {
      if (!AutoDropChecked)
        return;
      ApplicationViewModel.EscrowDrop();
    }

    private void ApplicationViewModel_CountEndEvent(object sender, DeviceTransactionResult e)
    {
    }

    private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        ApplicationViewModel.DeviceTransactionEnd();
    }

    public bool LastCanNext
    {
        get { return _lastCanNext; }
        set
      {
        if (_lastCanNext == value)
          return;
        _lastCanNext = value;
        ResetIdleTimerOnUserInteraction();
      }
    }

    public new bool CanNext
    {
      get
      {
        if (!IsCountWithinTheLimits().HasFlag(CountLimitCheckResult.UNDERDEPOSIT))
        {
          ApplicationViewModel applicationViewModel = ApplicationViewModel;
          int num1;
          if (applicationViewModel == null)
          {
            num1 = 0;
          }
          else
          {
            long? droppedAmountCents = applicationViewModel.CurrentTransaction?.DroppedAmountCents;
            long num2 = 0;
            num1 = droppedAmountCents.GetValueOrDefault() > num2 & droppedAmountCents.HasValue ? 1 : 0;
          }
          if (num1 != 0 && CanCancel && !TimeoutMode)
            return !ApplicationViewModel.DeviceManager.ClearHopperRequest;
        }
        return false;
      }
    }

    public void Next()
    {
      ClearErrorText();
      PrintErrorText("processing, please wait...");
      ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.WorkerReportsProgress = false;
      backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
      backgroundWorker.RunWorkerAsync();
    }

    public bool LastCanCancel
    {
        get { return _lastCanCancel; }
        set
      {
        if (_lastCanCancel == value)
          return;
        _lastCanCancel = value;
        ResetIdleTimerOnUserInteraction();
      }
    }

    public new bool CanCancel
    {
      get
      {
        bool flag = !ApplicationViewModel.DeviceManager.HasEscrow ? ApplicationViewModel.CanTransactionEnd : (AutoCountChecked ? CurrentTransaction.CountedAmountCents == 0L && CanEscrowDrop : ApplicationViewModel.CanTransactionEnd && !TimeoutMode && !ApplicationViewModel.DeviceManager.ClearHopperRequest);
        LastCanCancel = flag;
        return flag;
      }
    }

    public void Cancel()
    {
      if (MessageBox.Show("Cancel Deposit?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
        return;
      ApplicationViewModel.Log.Warning(GetType().Name, nameof (Cancel), "Command", "Customer has cancelled the transaction");
      Next();
    }

    public bool LastCanCount
    {
        get { return _lastCanCount; }
        set
      {
        if (_lastCanCount == value)
          return;
        _lastCanCount = value;
        ResetIdleTimerOnUserInteraction();
      }
    }

    public bool CanCount
    {
      get
      {
        CountLimitCheckResult limitCheckResult = IsCountWithinTheLimits() & ~CountLimitCheckResult.OVERDEPOSIT;
        bool flag = !AutoCountChecked && ApplicationViewModel.CanCount && !TimeoutMode && limitCheckResult == CountLimitCheckResult.OK;
        LastCanCount = flag;
        return flag;
      }
    }

    public void Count()
    {
        ApplicationViewModel.Count();
    }

    public bool LastCanEscrowDrop
    {
        get { return _lastCanEscrowDrop; }
        set
      {
        if (_lastCanEscrowDrop == value)
          return;
        _lastCanEscrowDrop = value;
        ResetIdleTimerOnUserInteraction();
      }
    }

    public bool CanEscrowDrop
    {
      get
      {
        CountLimitCheckResult limitCheckResult = IsCountWithinTheLimits() & ~CountLimitCheckResult.OVERDEPOSIT;
        bool flag = !AutoDropChecked && ApplicationViewModel.CanEscrowDrop && (!TimeoutMode && limitCheckResult == CountLimitCheckResult.OK) && !limitCheckResult.HasFlag(CountLimitCheckResult.UNDERDEPOSIT);
        LastCanEscrowDrop = flag;
        return flag;
      }
    }

    public void EscrowDrop()
    {
        ApplicationViewModel.EscrowDrop();
    }

    public bool LastCanEscrowReject
    {
        get { return _lastCanEscrowReject; }
        set
      {
        if (_lastCanEscrowReject == value)
          return;
        _lastCanEscrowReject = value;
        ResetIdleTimerOnUserInteraction();
      }
    }

    public bool CanEscrowReject
    {
      get
      {
        bool flag = ApplicationViewModel.CanEscrowReject && !TimeoutMode;
        LastCanEscrowReject = flag;
        return flag;
      }
    }

    public void EscrowReject()
    {
        ApplicationViewModel.EscrowReject();
    }

    public CountLimitCheckResult IsCountWithinTheLimits()
    {
      CountLimitCheckResult limitCheckResult = CountLimitCheckResult.OK;
      AppTransaction currentTransaction1 = CurrentTransaction;
      if ((currentTransaction1 != null ? (currentTransaction1.IsUnderDeposit ? 1 : 0) : 0) != 0)
        limitCheckResult |= CountLimitCheckResult.UNDERDEPOSIT;
      long num;
      if (limitCheckResult.HasFlag(CountLimitCheckResult.UNDERDEPOSIT))
      {
        if (string.IsNullOrWhiteSpace(ErrorText))
        {
          string s = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("CountScreenViewModel.IsCountWithinTheLimits", "sys_UnderDeposit_ErrorMessage", "Kindly deposit {transaction.currency} {transaction.Underdeposit_amount} or more to continue or cancel the transaction");
          string format = ApplicationViewModel.DeviceConfiguration?.APPLICATION_MONEY_FORMAT ?? "#,##0.00";
          AppTransaction currentTransaction2 = CurrentTransaction;
          string str1;
          if (currentTransaction2 == null)
          {
            str1 = null;
          }
          else
          {
              var transactionLimits = currentTransaction2.TransactionLimits;
            if (transactionLimits == null)
            {
              str1 = null;
            }
            else
            {
              num = transactionLimits.UnderdepositAmount;
              str1 = num.ToString(format);
            }
          }
          string newValue = str1;
          string str2 = CustomerInputScreenReplace(s)?.Replace("{transaction.Underdeposit_amount}", newValue)?.Replace("{transaction.currency}", CurrentTransaction?.CurrencyCode);
          PrintErrorText(string.Format("[{0}] ", 1019) + str2);
        }
      }
      else
      {
        string errorText = ErrorText;
        if ((errorText != null ? (errorText.Contains(string.Format("[{0}]", 1019)) ? 1 : 0) : 0) != 0)
          ClearErrorText();
      }
      AppTransaction currentTransaction3 = CurrentTransaction;
      if ((currentTransaction3 != null ? (currentTransaction3.IsOverCount ? 1 : 0) : 0) != 0)
        limitCheckResult |= CountLimitCheckResult.OVERCOUNT;
      if (limitCheckResult.HasFlag(CountLimitCheckResult.OVERCOUNT))
      {
        if (string.IsNullOrWhiteSpace(ErrorText))
        {
          string s = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("CountScreenViewModel.IsCountWithinTheLimits", "sys_OverCount_ErrorMessage", "WARNING: deposits greater than {transaction.currency} {transaction.OverCount_amount} are not allowed.");
          string newValue = CurrentTransaction?.TransactionLimits?.OvercountAmount.ToString(ApplicationViewModel.DeviceConfiguration?.APPLICATION_MONEY_FORMAT ?? "#,##0.00");
          string str = CustomerInputScreenReplace(s)?.Replace("{transaction.OverCount_amount}", newValue)?.Replace("{transaction.currency}", CurrentTransaction?.CurrencyCode);
          PrintErrorText(string.Format("[{0}] ", 1017) + str);
        }
      }
      else
      {
        string errorText = ErrorText;
        if ((errorText != null ? (errorText.Contains(string.Format("[{0}]", 1017)) ? 1 : 0) : 0) != 0)
          ClearErrorText();
      }
      AppTransaction currentTransaction4 = CurrentTransaction;
      if ((currentTransaction4 != null ? (currentTransaction4.IsOverDeposit ? 1 : 0) : 0) != 0)
        limitCheckResult |= CountLimitCheckResult.OVERDEPOSIT;
      if (!limitCheckResult.HasFlag(CountLimitCheckResult.OVERCOUNT) && limitCheckResult.HasFlag(CountLimitCheckResult.OVERDEPOSIT))
      {
        if (string.IsNullOrWhiteSpace(ErrorText))
        {
          string s = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("CountScreenViewModel.IsCountWithinTheLimits", "sys_OverDeposit_ErrorMessage", "WARNING: deposits greater than {transaction.currency} {transaction.Overdeposit_amount} will not be credited.");
          string format = ApplicationViewModel.DeviceConfiguration?.APPLICATION_MONEY_FORMAT ?? "#,##0.00";
          AppTransaction currentTransaction2 = CurrentTransaction;
          string str1;
          if (currentTransaction2 == null)
          {
            str1 = null;
          }
          else
          {
            var transactionLimits = currentTransaction2.TransactionLimits;
            if (transactionLimits == null)
            {
              str1 = null;
            }
            else
            {
              num = transactionLimits.OverdepositAmount;
              str1 = num.ToString(format);
            }
          }
          string newValue = str1;
          string str2 = CustomerInputScreenReplace(s)?.Replace("{transaction.Overdeposit_amount}", newValue)?.Replace("{transaction.currency}", CurrentTransaction?.CurrencyCode);
          PrintErrorText(string.Format("[{0}] ", 1016) + str2);
        }
      }
      else
      {
        string errorText = ErrorText;
        if ((errorText != null ? (errorText.Contains(string.Format("[{0}]", 1016)) ? 1 : 0) : 0) != 0)
          ClearErrorText();
      }
      if (IsBagFullOverFlow)
        limitCheckResult |= CountLimitCheckResult.BAGOVERFLOW;
      return limitCheckResult;
    }

    public bool IsBagFullOverFlow
    {
      get
      {
        long num1 = ApplicationViewModel?.DeviceManager?.CurrentDeviceStatus?.Bag?.NoteLevel ?? int.MaxValue;
        long num2 = ApplicationViewModel?.DeviceManager?.CurrentDeviceStatus?.Bag?.NoteCapacity ?? int.MinValue;
        DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
        long num3 = deviceConfiguration != null ? deviceConfiguration.BAGFULL_OVERFLOW_COUNT : 1000L;
        long num4 = num2 + num3;
        int num5 = num1 >= num4 ? 1 : 0;
        if (num5 != 0)
        {
          string errorText = ErrorText;
          if ((errorText != null ? (!errorText.Contains(string.Format("[{0}]", 1015)) ? 1 : 0) : 0) == 0)
            return num5 != 0;
          if (!(Application.Current.FindResource("Bagfull_Overflow_WarningMessage") is string s))
            s = "DEFAULT: Counting Complete. Press {btn_next_caption} to complete the deposit or {btn_escrow_reject_caption} to Cancel.";
          PrintErrorText(string.Format(string.Format("[{0}] {1}", 1015, CustomerInputScreenReplace(s))));
          return num5 != 0;
        }
        string errorText1 = ErrorText;
        if ((errorText1 != null ? (errorText1.Contains(string.Format("[{0}]", 1015)) ? 1 : 0) : 0) == 0)
          return num5 != 0;
        ClearErrorText();
        return num5 != 0;
      }
    }

    protected override void DoCancelTransactionOnTimeout()
    {
      TimeoutMode = true;
      if (ApplicationViewModel.DeviceManager.ControllerState != ControllerState.DROP || ApplicationViewModel.DeviceManager.CashAccSysSerialFix.DE50Operation != DE50OperatingState.Waiting)
        return;
      EscrowReject();
    }

    public override void Dispose()
    {
      base.Dispose();
      ApplicationViewModel.DeviceStatusChangedEvent -= new EventHandler<DeviceStatusChangedEventArgs>(ApplicationViewModel_DeviceStatusChangedEvent);
      ApplicationViewModel.CountPauseEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountPauseEvent);
      ApplicationViewModel.CountEndEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_CountEndEvent);
      ApplicationViewModel.TransactionStatusEvent -= new EventHandler<DeviceTransactionResult>(ApplicationViewModel_TransactionStatusEvent);
      ApplicationViewModel.NotifyCurrentTransactionStatusChangedEvent -= new EventHandler<EventArgs>(ApplicationViewModel_NotifyCurrentTransactionStatusChanged);
      ApplicationViewModel.DeviceManager.PropertyChanged -= new PropertyChangedEventHandler(DeviceManager_PropertyChanged);
      ApplicationViewModel.DeviceManager.CashAccSysSerialFix.PropertyChanged -= new PropertyChangedEventHandler(CashAccSysSerialFix_PropertyChanged);
    }

    [Flags]
    public enum CountLimitCheckResult
    {
      ERROR = 0,
      OK = 1,
      UNDERDEPOSIT = 2,
      OVERDEPOSIT = 4,
      OVERCOUNT = 8,
      BAGOVERFLOW = 16, // 0x00000010
    }
  }
}
