
// Type: CashmereDeposit.Models.AppTransaction

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.Utils;
using CashmereDeposit.Utils.AlertClasses;
using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Models
{
  public class AppTransaction : PropertyChangedBase
  {
    private DateTime _endDate;
    private string _accountNumber;
    private string _accountName;
    private string _referenceAccount;
    private string _referenceAccountName;
    private string _narration;
    private string _depositorName;
    private string _idnumber;
    private string _fundsSource;
    private string _phone;
    private Transaction _transaction;
    private AppSession _session;
    private Currency _currency;
    private bool _notesRejected;
    private bool _noteJamDetected;
    private long _totalAmountCents;
    private long _countedAmountCents;
    private long _droppedAmountCents;
    private Denomination _totalDenominationResult;
    private Denomination _countedDenominationResult;
    private Denomination _droppedDenominationResult;
    private object DroppedDenominationUpdateLock = new object();
    private TransactionTypeListItem _transactionType;

    public AppTransaction(
      AppSession session,
      TransactionTypeListItem transactionType,
      string currency)
    {
      var appTransaction = this;
      if (currency == null)
        throw new ArgumentNullException();
      Session = session;
      _transaction = new Transaction()
      {
        Id = GuidExt.UuidCreateSequential(),
        SessionId = session.DepositorSession.Id,
        DeviceId = session.Device.Id,
        TxStartDate = DateTime.Now,
        TxCurrency = currency,
        TxErrorCode = 0,
        TxAccountNumber = "",
        TxPhone = "",
        TxIdNumber = "",
        TxNarration = "",
        FundsSource = "",
        TxDepositorName = "",
        CbAccountName = "",
        TxAmount = new long?(0L)
      };
      session.DBContext.Transactions.Add(_transaction);
      Currency = DepositorDBContext.Currencies.FirstOrDefault(x => x.Code == currency);
      TransactionType = transactionType;
      var applicationModel = session.ApplicationViewModel.ApplicationModel;
      string str;
      if (applicationModel == null)
      {
        str = null;
      }
      else
      {
        var device = applicationModel.GetDevice(DepositorDBContext);
        if (device == null)
        {
          str = null;
        }
        else
        {
          var suspenseAccounts = device.DeviceCITSuspenseAccounts;
          str = suspenseAccounts != null ? suspenseAccounts.FirstOrDefault(x => x.CurrencyCode == appTransaction.Currency.Code?.ToUpper())?.AccountNumber : null;
        }
      }
      SuspenseAccount = str;
      _transaction.TxRandomNumber = new int?(session.ApplicationViewModel.Rand.Next(0, 999999999));
      PropertyChanged += new PropertyChangedEventHandler(OnPropertyChangedEvent);
      session.SaveToDatabase();
      ApplicationViewModel.Log.InfoFormat(GetType().Name, "TransactionStart", nameof (Transaction), "Transaction {0} started", _transaction.Id.ToString().ToUpper());
    }

    public AppTransaction()
    {
    }

    public string DeviceNumber => _transaction.Device.DeviceNumber;

    public string BranchName => _transaction.Device.Branch.Name;

    public string ReceiptTransactionID => _transaction.CbTxNumber;

    public int ReceiptTransactionSeqNumber => _transaction.TxRandomNumber.Value;

    public DateTime StartDate
    {
      get => Transaction.TxStartDate;
      set
      {
        Transaction.TxStartDate = value;
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "StartDate Set", "Tx Property Changed", "StartDate set to {0}", Transaction.TxStartDate);
        NotifyOfPropertyChange(nameof (StartDate));
      }
    }

    public DateTime EndDate
    {
      get => _endDate;
      set
      {
        _endDate = value;
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "EndDate Set", "Tx Property Changed", "EndDate set to {0}", _endDate);
        Transaction.TxEndDate = new DateTime?(value);
        NotifyOfPropertyChange((Expression<Func<DateTime>>) (() => EndDate));
      }
    }

    public string SuspenseAccount
    {
      get => Transaction.TxSuspenseAccount;
      set
      {
        Transaction.TxSuspenseAccount = value;
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "SuspenseAccount Set", "Tx Property Changed", "SuspenseAccount set to {0}", Transaction.TxSuspenseAccount);
        NotifyOfPropertyChange((Expression<Func<string>>) (() => SuspenseAccount));
      }
    }

    public string CurrencyCode => Currency.Code.ToUpper();

    public long TransactionValue { get; set; }

    public string AccountNumber
    {
      get => _accountNumber;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "AccountNumber Changed", "Tx Property Changed", "AccountNumber changed from {0} to {1}", _accountNumber, value);
        _accountNumber = value.Left(49);
        Transaction.TxAccountNumber = _accountNumber;
        NotifyOfPropertyChange(nameof (AccountNumber));
      }
    }

    public string AccountName
    {
      get => _accountName;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "AccountName Changed", "Tx Property Changed", "AccountName changed from {0} to {1}", _accountName, value);
        _accountName = value.Left(49);
        Transaction.CbAccountName = _accountName;
        NotifyOfPropertyChange(nameof (AccountName));
      }
    }

    public string TransactionTypeString => TransactionType.Name;

    public string ReferenceAccount
    {
      get => _referenceAccount;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "ReferenceAccount Changed", "Tx Property Changed", "ReferenceAccount changed from {0} to {1}", _referenceAccount, value);
        _referenceAccount = value.Left(49);
        Transaction.TxRefAccount = _referenceAccount;
        NotifyOfPropertyChange(nameof (ReferenceAccount));
      }
    }

    public string ReferenceAccountName
    {
      get => _referenceAccountName;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "ReferenceAccountName Changed", "Tx Property Changed", "ReferenceAccountName changed from {0} to {1}", _referenceAccountName, value);
        _referenceAccountName = value.Left(49);
        Transaction.CbRefAccountName = _referenceAccountName;
        NotifyOfPropertyChange(nameof (ReferenceAccountName));
      }
    }

    internal void ToRawEscrowJamString(StringBuilder sb)
    {
      sb.AppendLine("================================================================================================================================");
      sb.AppendLine("Escrow Jams");
      sb.AppendLine("================================================================================================================================");
      sb.AppendLine("date_detected\tdropped_amount\tescrow_amount\tposted_amount\tretreived_amount\trecovery_date\tInitiating User\tAuthorising User");
      foreach (var escrowJam in Transaction.EscrowJams)
        sb.AppendLine(escrowJam.ToRawTextString());
    }

    public string Narration
    {
      get => _narration;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "Narration Changed", "Tx Property Changed", "Narration changed from {0} to {1}", _narration, value);
        _narration = value.Left(49);
        Transaction.TxNarration = _narration;
        NotifyOfPropertyChange(nameof (Narration));
      }
    }

    public string DepositorName
    {
      get => _depositorName;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "DepositorName Changed", "Tx Property Changed", "DepositorName changed from {0} to {1}", _depositorName, value);
        _depositorName = value.Left(49);
        Transaction.TxDepositorName = _depositorName;
        NotifyOfPropertyChange(nameof (DepositorName));
      }
    }

    public string IDNumber
    {
      get => _idnumber;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "IDNumber Changed", "Tx Property Changed", "IDNumber changed from {0} to {1}", _idnumber, value);
        _idnumber = value.Left(49);
        Transaction.TxIdNumber = _idnumber;
        NotifyOfPropertyChange(nameof (IDNumber));
      }
    }

    public string FundsSource
    {
      get => _fundsSource;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "FundsSource Changed", "Tx Property Changed", "FundsSource changed from {0} to {1}", _fundsSource, value);
        _fundsSource = value.Left(49);
        Transaction.FundsSource = _fundsSource;
        NotifyOfPropertyChange(nameof (FundsSource));
      }
    }

    public string Phone
    {
      get => _phone;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "Phone Changed", "Tx Property Changed", "Phone changed from {0} to {1}", _phone, value);
        _phone = value.Left(49);
        Transaction.TxPhone = _phone;
        NotifyOfPropertyChange(nameof (Phone));
      }
    }

    [IgnoreDataMember]
    private DepositorDBContext DepositorDBContext => Session.DBContext;

    [IgnoreDataMember]
    public object PostingLock { get; set; } = new object();

    [IgnoreDataMember]
    public bool isPosting { get; set; }

    public bool hasPosted { get; set; }

    [IgnoreDataMember]
    public Transaction Transaction => _transaction;

    [IgnoreDataMember]
    public List<Printout> Printouts => _transaction.Printouts.ToList();

    [IgnoreDataMember]
    public AppSession Session
    {
      get => _session;
      set
      {
        _session = value;
        NotifyOfPropertyChange(nameof (Session));
      }
    }

    [IgnoreDataMember]
    public bool Completed => Transaction.TxCompleted;

    [IgnoreDataMember]
    public Currency Currency
    {
      get => _currency;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "Currency Changed", "Tx Property Changed", "Currency changed from {0} to {1}", _currency?.Code, value?.Code);
        _currency = DepositorDBContext.Currencies.First(x => x.Code == value.Code);
        Transaction.TxCurrencyNavigation = _currency;
        NotifyOfPropertyChange(nameof (Currency));
      }
    }

    [IgnoreDataMember]
    public bool NotesRejected
    {
      get => _notesRejected;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "NotesRejected Changed", "Tx Property Changed", "NotesRejected changed from {0} to {1}", _notesRejected, value);
        _notesRejected = value;
        Transaction.NotesRejected = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => NotesRejected));
      }
    }

    [IgnoreDataMember]
    public bool NoteJamDetected
    {
      get => _noteJamDetected;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "NoteJamDetected Changed", "Tx Property Changed", "NoteJamDetected changed from {0} to {1}", _noteJamDetected, value);
        _noteJamDetected = value;
        Transaction.JamDetected = value;
        NotifyOfPropertyChange((Expression<Func<bool>>) (() => NoteJamDetected));
      }
    }

    public string DroppedDisplayAmountString => (DroppedAmountCents / 100.0).ToString(ApplicationViewModel.DeviceConfiguration.APPLICATION_MONEY_FORMAT ?? "#,##0.00");

    public Decimal DroppedDisplayAmount => DroppedAmountCents / 100.0M;

    public string CountedDisplayAmountString => (CountedAmountCents / 100.0).ToString(ApplicationViewModel.DeviceConfiguration.APPLICATION_MONEY_FORMAT ?? "#,##0.00");

    public Decimal CountedDisplayAmount => CountedAmountCents / 100.0M;

    public string TotalDisplayAmountString => (TotalAmountCents / 100.0M).ToString(ApplicationViewModel.DeviceConfiguration.APPLICATION_MONEY_FORMAT ?? "#,##0.00");

    public Decimal TotalDisplayAmount => TotalAmountCents / 100.0M;

    public long TotalAmountCents
    {
      get => _totalAmountCents;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "Amount Changed", "Tx Property Changed", "TotalAmount changed from {0} to {1}", _totalAmountCents / 100L, value / 100L);
        _totalAmountCents = value;
        NotifyOfPropertyChange((Expression<Func<long>>) (() => TotalAmountCents));
        NotifyOfPropertyChange("TotalDisplayAmount");
        NotifyOfPropertyChange("TotalDisplayAmountString");
      }
    }

    public long CountedAmountCents
    {
      get => _countedAmountCents;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "Amount Changed", "Tx Property Changed", "CountedAmount changed from {0} to {1}", _countedAmountCents / 100L, value / 100L);
        _countedAmountCents = value;
        NotifyOfPropertyChange((Expression<Func<long>>) (() => CountedAmountCents));
        NotifyOfPropertyChange("CountedDisplayAmount");
        NotifyOfPropertyChange("CountedDisplayAmountString");
      }
    }

    public long DroppedAmountCents
    {
      get => _droppedAmountCents;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "Amount Changed", "Tx Property Changed", "DroppedAmount changed from {0} to {1}", _droppedAmountCents / 100L, value / 100L);
        _droppedAmountCents = value;
        Transaction.TxAmount = new long?(value);
        NotifyOfPropertyChange((Expression<Func<long>>) (() => DroppedAmountCents));
        NotifyOfPropertyChange("DroppedDisplayAmount");
        NotifyOfPropertyChange("DroppedDisplayAmountString");
      }
    }

    public Denomination TotalDenominationResult
    {
      get => _totalDenominationResult;
      set
      {
        var totalValue1 = _totalDenominationResult?.TotalValue;
        var totalValue2 = value?.TotalValue;
        if (totalValue1 == totalValue2 & totalValue1.HasValue == totalValue2.HasValue)
        {
          ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof (TotalDenominationResult), nameof (TotalDenominationResult), "Denom remains at {0}", TotalDenominationResult?.ToString() ?? "{null}");
        }
        else
        {
          _totalDenominationResult = value;
          ApplicationViewModel.Log.Info(GetType().Name, "TotalDenominationResult Changed", "Tx Property Changed", TotalDenominationResult?.ToString() ?? "{null}");
          NotifyOfPropertyChange((Expression<Func<Denomination>>) (() => TotalDenominationResult));
        }
      }
    }

    public Denomination CountedDenominationResult
    {
      get => _countedDenominationResult;
      set
      {
        var totalValue1 = _countedDenominationResult?.TotalValue;
        var totalValue2 = value?.TotalValue;
        if (totalValue1 == totalValue2 & totalValue1.HasValue == totalValue2.HasValue)
        {
          ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof (CountedDenominationResult), nameof (CountedDenominationResult), "Denom remains at {0}", CountedDenominationResult?.ToString() ?? "{null}");
        }
        else
        {
          _countedDenominationResult = value;
          ApplicationViewModel.Log.Info(GetType().Name, "CountedDenominationResult Changed", "Tx Property Changed", CountedDenominationResult?.ToString() ?? "{null}");
          NotifyOfPropertyChange((Expression<Func<Denomination>>) (() => CountedDenominationResult));
        }
      }
    }

    public Denomination DroppedDenomination
    {
      get => _droppedDenominationResult;
      set
      {
        var totalValue1 = _droppedDenominationResult?.TotalValue;
        var totalValue2 = value?.TotalValue;
        if (totalValue1 == totalValue2 & totalValue1.HasValue == totalValue2.HasValue)
        {
          ApplicationViewModel.Log.InfoFormat(GetType().Name, "DroppedDenominationResult", "DroppedDenominationResult", "Denom remains at {0}", DroppedDenomination?.ToString() ?? "{null}");
        }
        else
        {
          lock (DroppedDenominationUpdateLock)
          {
            _droppedDenominationResult = value;
            foreach (var denominationItem in value.denominationItems)
            {
              var denom = denominationItem;
              var list = Transaction.DenominationDetails.Where(x => x.Denom == denom.denominationValue).ToList();
              if (list.Count() > 1)
              {
                foreach (var denominationDetail in list)
                  Transaction.DenominationDetails.Remove(denominationDetail);
              }
              var denominationDetail1 = Transaction.DenominationDetails.FirstOrDefault(x => x.Denom == denom.denominationValue);
              if (denominationDetail1 == null)
              {
                var transaction = Transaction;
                if (transaction != null)
                  transaction.DenominationDetails.Add(new DenominationDetail()
                  {
                    Id = GuidExt.UuidCreateSequential(),
                    Denom = denom.denominationValue,
                    Count = denom.count,
                    Subtotal = denom.count * denom.denominationValue,
                    Datetime = new DateTime?(DateTime.Now)
                  });
              }
              else
              {
                denominationDetail1.Count = denom.count;
                denominationDetail1.Subtotal = denom.count * denom.denominationValue;
              }
            }
          }
          ApplicationViewModel.Log.Info(GetType().Name, "DroppedDenominationResult Changed", "Tx Property Changed", DroppedDenomination?.ToString() ?? "{null}");
          NotifyOfPropertyChange((Expression<Func<Denomination>>) (() => DroppedDenomination));
          Session.SaveToDatabase();
        }
      }
    }

    public TransactionLimitListItem TransactionLimits
    {
      get
      {
        var transactionType = TransactionType;
        if (transactionType == null)
          return null;
        var transactionLimitList = transactionType.TxLimitListNavigation;
        if (transactionLimitList == null)
          return null;
        var transactionLimitListItems = transactionLimitList.TransactionLimitListItems;
        return transactionLimitListItems == null ? (TransactionLimitListItem) null : transactionLimitListItems.FirstOrDefault(x => string.Equals(x.CurrencyCode, CurrencyCode, StringComparison.InvariantCultureIgnoreCase));
      }
    }

    [IgnoreDataMember]
    public TransactionText TransactionText => TransactionType?.TransactionText;

    [IgnoreDataMember]
    public TransactionTypeListItem TransactionType
    {
      get => _transaction?.TxTypeNavigation;
      set
      {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "TransactionType Changed", "Tx Property Changed", "TransactionType changed from {0} to {1}", _transactionType?.CbTxType, value?.CbTxType);
        _transactionType = value;
        Transaction.TxTypeNavigation = DepositorDBContext.TransactionTypeListItems.FirstOrDefault(x => x.Id == value.Id);
        NotifyOfPropertyChange(nameof (TransactionType));
      }
    }

    [IgnoreDataMember]
    public List<SummaryListItem> TransactionAccountReferences
    {
      get
      {
        var summaryListItemList = new List<SummaryListItem>();
        if (Transaction.TxTypeNavigation.DefaultAccount == null)
        {
          if (AccountNumber != null)
            summaryListItemList.Add(new SummaryListItem()
            {
              Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Account Number", TransactionType?.TransactionText?.AccountNumberCaption, "Account Number"),
              Value = AccountNumber.Trim()
            });
          if (AccountName != null)
            summaryListItemList.Add(new SummaryListItem()
            {
              Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Account Name", TransactionType?.TransactionText?.AccountNameCaption, "Account Name"),
              Value = AccountName.Trim()
            });
        }
        if (ReferenceAccount != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Reference Number", TransactionType?.TransactionText?.ReferenceAccountNumberCaption, "Reference Number"),
            Value = ReferenceAccount.Trim()
          });
        if (ReferenceAccountName != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Reference Name", TransactionType?.TransactionText?.ReferenceAccountNameCaption, "Reference Name"),
            Value = ReferenceAccountName.Trim()
          });
        return summaryListItemList;
      }
    }

    public List<SummaryListItem> TransactionReferences
    {
      get
      {
        var summaryListItemList = new List<SummaryListItem>(TransactionAccountReferences);
        if (Currency != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(GetType().Name + ".TransactionReferences", "sys_Currency_Caption", "Currency"),
            Value = Currency.Code.ToUpper().Trim()
          });
        if (FundsSource != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  FundsSource", TransactionType?.TransactionText?.FundsSourceCaption, "Source of Funds"),
            Value = FundsSource.Trim()
          });
        if (Narration != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  Narration", TransactionType?.TransactionText?.NarrationCaption, "Narration"),
            Value = Narration.Trim()
          });
        if (DepositorName != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences Depositor Name", TransactionType?.TransactionText?.DepositorNameCaption, "Depositor Name"),
            Value = DepositorName.Trim()
          });
        if (IDNumber != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  ID Number", TransactionType?.TransactionText?.IdNumberCaption, "ID Number"),
            Value = IDNumber.Trim()
          });
        if (Phone != null)
          summaryListItemList.Add(new SummaryListItem()
          {
            Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  Phone", TransactionType?.TransactionText?.PhoneNumberCaption, "Phone"),
            Value = Phone.Trim()
          });
        return summaryListItemList;
      }
    }
        
        
    public bool IsOverCount
    {
        get
        {
            DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
            if ((deviceConfiguration != null ? (deviceConfiguration.USE_MAX_DEPOSIT_COUNT ? 1 : 0) : 0) == 0 || this.TransactionType?.TxLimitListNavigation?.Get_overcount_amount(this.Currency) <= 0L || (bool)!this.TransactionType?.TxLimitListNavigation?.Get_prevent_overcount(this.Currency))
                return false;
            Decimal totalDisplayAmount = this.TotalDisplayAmount;
            long? overcountAmount = this.TransactionType?.TxLimitListNavigation?.Get_overcount_amount(this.Currency);
            Decimal? nullable = overcountAmount.HasValue ? new Decimal?((Decimal) overcountAmount.GetValueOrDefault()) : new Decimal?();
            Decimal valueOrDefault = nullable.GetValueOrDefault();
            return totalDisplayAmount >= valueOrDefault & nullable.HasValue;
        }
    }

    public bool IsOverDeposit
    {
        get
        {
            if (this.TransactionType?.TxLimitListNavigation?.Get_overdeposit_amount(this.Currency) <= 0L || (bool)!this.TransactionType?.TxLimitListNavigation?.Get_prevent_overdeposit(this.Currency))
                return false;
            Decimal totalDisplayAmount = this.TotalDisplayAmount;
            long? overdepositAmount = this.TransactionType?.TxLimitListNavigation?.Get_overdeposit_amount(this.Currency);
            Decimal? nullable = overdepositAmount.HasValue ? new Decimal?((Decimal) overdepositAmount.GetValueOrDefault()) : new Decimal?();
            Decimal valueOrDefault = nullable.GetValueOrDefault();
            return totalDisplayAmount >= valueOrDefault & nullable.HasValue;
        }
    }

    public bool IsUnderDeposit
    {
        get
        {
            var transactionLimits = this.TransactionLimits;
            if ((transactionLimits != null ? ((bool)transactionLimits.PreventUnderdeposit ? 1 : 0) : 0) == 0 || !(this.TotalDisplayAmount > 0M))
                return false;
            Decimal totalDisplayAmount = this.TotalDisplayAmount;
            long? underdepositAmount = this.TransactionLimits?.UnderdepositAmount;
            Decimal? nullable = underdepositAmount.HasValue ? new Decimal?((Decimal) underdepositAmount.GetValueOrDefault()) : new Decimal?();
            Decimal valueOrDefault = nullable.GetValueOrDefault();
            return totalDisplayAmount < valueOrDefault & nullable.HasValue;
        }
    }

    public bool ShowFundsSource
    {
        get
        {
            var transactionLimits = this.TransactionLimits;
            if ((transactionLimits != null ? (transactionLimits.ShowFundsSource ? 1 : 0) : 0) == 0)
                return false;
            Decimal totalDisplayAmount = this.TotalDisplayAmount;
            long? fundsSourceAmount = this.TransactionLimits?.FundsSourceAmount;
            Decimal? nullable = fundsSourceAmount.HasValue ? new Decimal?((Decimal) fundsSourceAmount.GetValueOrDefault()) : new Decimal?();
            Decimal valueOrDefault = nullable.GetValueOrDefault();
            return totalDisplayAmount >= valueOrDefault & nullable.HasValue;
        }
    }

    internal void LogTransactionError(ApplicationErrorConst result, string errorMessage)
    {
      _transaction.TxErrorCode = (int) result;
      _transaction.TxErrorMessage = errorMessage.Left(254);
    }

    internal void EndTransaction(ApplicationErrorConst result, string errorMessage = null)
    {
      ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof (EndTransaction), "Transaction", "Terminating transaction with result= {0}", result.ToString());
      EndDate = DateTime.Now;
      _transaction.TxEndDate = new DateTime?(EndDate);
      _transaction.TxCompleted = true;
      if (result != ApplicationErrorConst.ERROR_NONE)
        LogTransactionError(result, errorMessage);
      if (result == ApplicationErrorConst.ERROR_NONE)
      {
        Session.ApplicationViewModel.AlertManager.SendAlert(new AlertTransactionEndedSuccess(this, Session.Device, DateTime.Now));
      }
      else
      {
        ApplicationViewModel.Log.ErrorFormat(GetType().Name, (int) result, result.ToString(), "Transaction ended with error {0}", errorMessage);
        Session.ApplicationViewModel.AlertManager.SendAlert(new AlertTransactionEndedFailed(this, Session.Device, DateTime.Now));
      }
      if (Session.HasCounted && Transaction.Printouts.FirstOrDefault(x => !x.IsCopy) == null)
        Session.ApplicationViewModel.PrintReceipt(Transaction, txDBContext: DepositorDBContext);
      Session.SaveToDatabase();
    }

    public void HandleDenominationResult(TransactionStatusResponseResult TransactionResult)
    {
      if (string.Equals(TransactionResult?.data?.SessionID, this.Session.SessionID.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(TransactionResult?.data?.TransactionID, this.Transaction.Id.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        if (!this.Completed)
        {
          long? nullable1;
          if (this.TotalAmountCents == TransactionResult.data.EscrowPlusDroppedTotalCents)
          {
            long droppedAmountCents = this.DroppedAmountCents;
            nullable1 = TransactionResult?.data?.TotalDroppedAmountCents;
            long valueOrDefault = nullable1.GetValueOrDefault();
            if (droppedAmountCents == valueOrDefault & nullable1.HasValue)
              return;
          }
          this.DroppedDenomination = TransactionResult?.data?.TotalDroppedNotes;
          this.CountedDenominationResult = TransactionResult?.data?.CurrentDropStatus?.data?.DenominationResult?.data;
          long? nullable2;
          if (TransactionResult == null)
          {
            nullable1 = new long?();
            nullable2 = nullable1;
          }
          else
          {
            TransactionStatusResponseData data = TransactionResult.data;
            if (data == null)
            {
              nullable1 = new long?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new long?(data.TotalDroppedAmountCents);
          }
          nullable1 = nullable2;
          this.DroppedAmountCents = nullable1.GetValueOrDefault();
          long? nullable3;
          if (TransactionResult == null)
          {
            nullable1 = new long?();
            nullable3 = nullable1;
          }
          else
          {
            TransactionStatusResponseData data1 = TransactionResult.data;
            if (data1 == null)
            {
              nullable1 = new long?();
              nullable3 = nullable1;
            }
            else
            {
              DropStatusResult currentDropStatus = data1.CurrentDropStatus;
              if (currentDropStatus == null)
              {
                nullable1 = new long?();
                nullable3 = nullable1;
              }
              else
              {
                DropStatusResultData data2 = currentDropStatus.data;
                if (data2 == null)
                {
                  nullable1 = new long?();
                  nullable3 = nullable1;
                }
                else
                {
                  DenominationResult denominationResult = data2.DenominationResult;
                  if (denominationResult == null)
                  {
                    nullable1 = new long?();
                    nullable3 = nullable1;
                  }
                  else
                  {
                    Denomination data3 = denominationResult.data;
                    if (data3 == null)
                    {
                      nullable1 = new long?();
                      nullable3 = nullable1;
                    }
                    else
                      nullable3 = new long?(data3.TotalValue);
                  }
                }
              }
            }
          }
          nullable1 = nullable3;
          this.CountedAmountCents = nullable1.GetValueOrDefault();
          this.TotalAmountCents = this.DroppedAmountCents + this.CountedAmountCents;
          this.HandleTransactionLimit();
        }
        else
          ApplicationViewModel.Log.WarningFormat(this.GetType().Name, "Transaction Already Complete", "AddDenominationResult", "Cannot add denomination {0} as transaction is already complete.", (object) TransactionResult?.data?.TotalDroppedAmountDisplayString);
      }
      else
        ApplicationViewModel.Log.Error(nameof (AppTransaction), 1, nameof (HandleDenominationResult), string.Format("Incorrect transaction IDs. SessionID {0} >> {1} && TransactionID {2} >> {3}", (object) TransactionResult?.data?.SessionID, (object) this.Session.SessionID, (object) TransactionResult?.data?.TransactionID, (object) this.Transaction.Id));
    }

    private void HandleTransactionLimit()
    {
      if (this.Session == null)
        return;
      if (this.DroppedDenomination == null)
      {
        ApplicationViewModel.Log?.Warning(this.GetType().Name, "Null Reference for DroppedDenomination", nameof (HandleTransactionLimit), "DenominationResult");
      }
      else
      {
        var transactionLimits1 = this.TransactionLimits;
        long? nullable1;
        if ((transactionLimits1 != null ? (transactionLimits1.PreventOverdeposit ? 1 : 0) : 0) != 0)
        {
          var transactionLimits2 = this.TransactionLimits;
          if ((transactionLimits2 != null ? (transactionLimits2.OverdepositAmount > 0L ? 1 : 0) : 0) != 0)
          {
            long num1 = this.DroppedAmountCents + this.CountedAmountCents;
            var transactionLimits3 = this.TransactionLimits;
            nullable1 = transactionLimits3 != null ? new long?(transactionLimits3.OverdepositAmount * 100L) : new long?();
            long valueOrDefault = nullable1.GetValueOrDefault();
            int num2 = num1 >= valueOrDefault & nullable1.HasValue ? 1 : 0;
          }
        }
        int num = this.IsOverDeposit ? 1 : 0;
        if (!this.IsOverDeposit)
          return;
        DepositorLogger log = ApplicationViewModel.Log;
        string name = this.GetType().Name;
        object[] objArray = new object[2]
        {
          (object) (this.DroppedDenomination.TotalValue / 100L),
          null
        };
        var transactionLimits4 = this.TransactionLimits;
        long? nullable2;
        if (transactionLimits4 == null)
        {
          nullable1 = new long?();
          nullable2 = nullable1;
        }
        else
          nullable2 = new long?(transactionLimits4.OverdepositAmount);
        objArray[1] = (object) nullable2;
        log.InfoFormat(name, "Count", "Device Management", "Transaction of {0} is above transaction value limit of {1}, Locking Counter", objArray);
        this.OnTransactionLimitReachedEvent((object) this, EventArgs.Empty);
      }
    }

    public string ToRawTextString()
    {
      StringBuilder stringBuilder1 = new StringBuilder((int) byte.MaxValue);
      stringBuilder1.AppendLine("----------------------------------------");
      stringBuilder1.AppendLine("             Transaction");
      stringBuilder1.AppendLine("----------------------------------------");
      StringBuilder stringBuilder2 = stringBuilder1;
      object[] objArray = new object[16]
      {
        (object) this._transaction.TxStartDate,
        (object) this._transaction.TxEndDate,
        (object) this._transaction.TxTypeNavigation.Name,
        (object) this._transaction.TxAccountNumber,
        (object) this._transaction.CbAccountName,
        (object) this._transaction.TxRefAccount,
        (object) this._transaction.CbRefAccountName,
        (object) this._transaction.TxNarration,
        (object) this._transaction.TxDepositorName,
        (object) this._transaction.TxIdNumber,
        (object) this._transaction.TxPhone,
        (object) this._transaction.TxCurrency,
        null,
        null,
        null,
        null
      };
      long? txAmount = this._transaction.TxAmount;
      long num1 = 100;
      objArray[12] = (object) (txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?());
      objArray[13] = (object) this._transaction.FundsSource;
      objArray[14] = (object) this._transaction.TxErrorCode;
      objArray[15] = (object) this._transaction.TxErrorMessage;
      string str = string.Format("\r\nStart Date:                 {0}\r\nEnd Date:                   {1}\r\nTransaction Type:           {2}\r\nAccount Number:             {3}\r\nAccount Name:               {4}\r\nReference Account Number:   {5}\r\nReference Account Name:     {6}            \r\nNarration:                  {7}\r\nDepositor Name:             {8}\r\nDepositor ID:               {9}\r\nDepositor Phone:            {10}\r\nCurrency:                   {11}\r\nAmount:                     {12}\r\nSource of Funds:            {13}\r\nError Code:                 {14}\r\nError Message:              {15}", objArray);
      stringBuilder2.AppendLine(str);
      stringBuilder1.AppendLine("________________________________________");
      stringBuilder1.AppendLine(string.Format("{0,-12}{1,7}{2,21}", (object) "Denomination", (object) "Count", (object) "Sub Total"));
      stringBuilder1.AppendLine("________________________________________");
      List<DenominationDetail> list = this._transaction.DenominationDetails.ToList<DenominationDetail>();
      foreach (DenominationDetail denominationDetail in list)
      {
        double num2 = (double) denominationDetail.Denom / 100.0;
        long count = denominationDetail.Count;
        double num3 = (double) ((long) denominationDetail.Denom * denominationDetail.Count) / 100.0;
        int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
        stringBuilder1.AppendLine(string.Format("{0,-12:0.##}{1,7:##,0}{2,23:##,0.##}", (object) num2, (object) count, (object) num3));
      }
      stringBuilder1.AppendLine("========================================");
      stringBuilder1.AppendLine(string.Format("{0,-10}{1,7:##,0}{2,23:##,0.##}", (object) "TOTAL:", (object) list.Sum<DenominationDetail>((Func<DenominationDetail, long>) (x => x.Count)), (object) ((double) list.Sum<DenominationDetail>((Func<DenominationDetail, long>) (x => x.Subtotal)) / 100.0)));
      stringBuilder1.AppendLine("========================================");
      return stringBuilder1.ToString();
    }

    public string ToEmailString()
    {
      StringBuilder stringBuilder1 = new StringBuilder((int) byte.MaxValue);
      stringBuilder1.AppendLine("<p><hr /><h3>Transaction</h3><hr />");
      StringBuilder stringBuilder2 = stringBuilder1;
      object[] objArray = new object[16]
      {
        (object) this._transaction.TxStartDate,
        (object) this._transaction.TxEndDate,
        (object) this._transaction.TxTypeNavigation.Name,
        (object) this._transaction.TxAccountNumber,
        (object) this._transaction.CbAccountName,
        (object) this._transaction.TxRefAccount,
        (object) this._transaction.CbRefAccountName,
        (object) this._transaction.TxNarration,
        (object) this._transaction.TxDepositorName,
        (object) this._transaction.TxIdNumber,
        (object) this._transaction.TxPhone,
        (object) this._transaction.TxCurrency,
        null,
        null,
        null,
        null
      };
      long? txAmount = this._transaction.TxAmount;
      long num1 = 100;
      objArray[12] = (object) (txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?());
      objArray[13] = (object) this._transaction.FundsSource;
      objArray[14] = (object) this._transaction.TxErrorCode;
      objArray[15] = (object) this._transaction.TxErrorMessage;
      string str = string.Format("<p><table border=\"0\" cellpadding=\"20\" cellspacing=\"0\" width=\"600\">\r\n< tr><th>Start Date</th><th>End Date</th><th>Transaction Type</th><th>Account Number</th><th>Account Name</th><th>Reference Account Number</th><th>Reference Account Name</th>\r\n<th>Narration</th><th>Depositor Name</th><th>Depositor ID</th><th>Depositor Phone</th><th>Currency</th><th>Amount</th><th>Source of Funds</th>\r\n<th>Error Code</th><th>Error Message</th></tr>\r\n<tr><td>{0}</td>  <td>{1}</td>  <td>{2}</td>  <td>{3}</td>  <td>{4}</td>  <td>{5}</td>  <td>{6}</td>  <td>{7}</td>  <td>{8}</td>  <td>{9}</td>  <td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td></tr>\r\n</table></p>", objArray);
      stringBuilder2.AppendLine(str);
      stringBuilder1.AppendLine("<p><table style=\"text - align: left\">");
      stringBuilder1.AppendLine("<tr><th>Denomination</th><th>Count</th><th>Sub Total</th></tr>");
      List<DenominationDetail> list = this._transaction.DenominationDetails.ToList<DenominationDetail>();
      foreach (DenominationDetail denominationDetail in list)
      {
        double num2 = (double) denominationDetail.Denom / 100.0;
        long count = denominationDetail.Count;
        double num3 = (double) ((long) denominationDetail.Denom * denominationDetail.Count) / 100.0;
        int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
        stringBuilder1.AppendLine(string.Format("<tr><td>{0:##,0.##}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", (object) num2, (object) count, (object) num3));
      }
      stringBuilder1.AppendLine(string.Format("<tr><td>{0}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", (object) "TOTAL:", (object) list.Sum<DenominationDetail>((Func<DenominationDetail, long>) (x => x.Count)), (object) ((double) list.Sum<DenominationDetail>((Func<DenominationDetail, long>) (x => x.Subtotal)) / 100.0)));
      stringBuilder1.AppendLine("</table></p></p>");
      return stringBuilder1.ToString();
    }

    private void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
    {
      ApplicationViewModel.Log.Trace(GetType().Name, nameof (OnPropertyChangedEvent), "DATABASE", "Saving transaction on property change.");
      Session.SaveToDatabase();
    }

    public event EventHandler<EventArgs> TransactionLimitReachedEvent;

    private void OnTransactionLimitReachedEvent(object sender, EventArgs e)
    {
      if (TransactionLimitReachedEvent == null)
        return;
      TransactionLimitReachedEvent(this, e);
    }
  }
}
