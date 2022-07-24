using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.Utils.AlertClasses;
using CashmereDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.Models;

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
   //  private static DepositorDBContext _depositorDBContext { get; set; }

    public AppTransaction(
        AppSession session,
        TransactionTypeListItem transactionType,
        string currency)
    {

        _depositorDBContext = IoC.Get<DepositorDBContext>();
        AppTransaction appTransaction = this;
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
        Currency = _depositorDBContext.Currencies.FirstOrDefault(x => x.Code == currency);
        TransactionType = transactionType;
        ApplicationModel applicationModel = session.ApplicationViewModel.ApplicationModel;
        string str;
        if (applicationModel == null)
        {
            str = null;
        }
        else
        {
            Device device = applicationModel.GetDeviceAsync();
            if (device == null)
            {
                str = null;
            }
            else
            {
                ICollection<DeviceSuspenseAccount> suspenseAccounts = device.DeviceSuspenseAccounts;
                str = suspenseAccounts != null ? suspenseAccounts.FirstOrDefault(x => x.CurrencyCode == appTransaction.Currency.Code?.ToUpper())?.AccountNumber : (string)null;
            }
        }
        SuspenseAccount = str;
        _transaction.TxRandomNumber = new int?(session.ApplicationViewModel.Rand.Next(0, 999999999));
        PropertyChanged += new PropertyChangedEventHandler(OnPropertyChangedEvent);
        session.SaveToDatabase();
        ApplicationViewModel.Log.InfoFormat(GetType().Name, "TransactionStart", nameof(Transaction), "Transaction {0} started", _transaction.Id.ToString().ToUpper());
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
            NotifyOfPropertyChange(nameof(StartDate));
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
            NotifyOfPropertyChange(() => EndDate);
        }
    }

    public string SuspenseAccount
    {
        get => Transaction.TxSuspenseAccount;
        set
        {
            Transaction.TxSuspenseAccount = value;
            ApplicationViewModel.Log.InfoFormat(GetType().Name, "SuspenseAccount Set", "Tx Property Changed", "SuspenseAccount set to {0}", Transaction.TxSuspenseAccount);
            NotifyOfPropertyChange(() => SuspenseAccount);
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
            NotifyOfPropertyChange(nameof(AccountNumber));
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
            NotifyOfPropertyChange(nameof(AccountName));
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
            NotifyOfPropertyChange(nameof(ReferenceAccount));
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
            NotifyOfPropertyChange(nameof(ReferenceAccountName));
        }
    }

    internal void ToRawEscrowJamString(StringBuilder sb)
    {
        sb.AppendLine("================================================================================================================================");
        sb.AppendLine("Escrow Jams");
        sb.AppendLine("================================================================================================================================");
        sb.AppendLine("date_detected\tdropped_amount\tescrow_amount\tposted_amount\tretreived_amount\trecovery_date\tInitiating User\tAuthorising User");
        foreach (EscrowJam escrowJam in Transaction.EscrowJams)
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
            NotifyOfPropertyChange(nameof(Narration));
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
            NotifyOfPropertyChange(nameof(DepositorName));
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
            NotifyOfPropertyChange(nameof(IDNumber));
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
            NotifyOfPropertyChange(nameof(FundsSource));
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
            NotifyOfPropertyChange(nameof(Phone));
        }
    }

    public int ValidationTries { get; set; }
    [IgnoreDataMember]
    private DepositorDBContext DepositorDBContext => Session.DBContext;

    [IgnoreDataMember]
    public object PostingLock { get; set; } = new object();

    [IgnoreDataMember]
    public bool isPosting { get; set; } = false;

    public bool hasPosted { get; set; } = false;

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
            NotifyOfPropertyChange(nameof(Session));
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
            ApplicationViewModel.Log.InfoFormat(GetType().Name, "Currency Changed", "Tx Property Changed", "Currency changed from {0} to {1}", (object)_currency?.Code, (object)value?.Code);
            _currency = _depositorDBContext.Currencies.First(x => x.Code == value.Code);
            Transaction.TxCurrencyNavigation = _currency;
            NotifyOfPropertyChange(nameof(Currency));
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
            NotifyOfPropertyChange(() => NotesRejected);
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
            NotifyOfPropertyChange(() => NoteJamDetected);
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
            NotifyOfPropertyChange((Expression<Func<long>>)(() => TotalAmountCents));
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
            NotifyOfPropertyChange((Expression<Func<long>>)(() => CountedAmountCents));
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
            NotifyOfPropertyChange((Expression<Func<long>>)(() => DroppedAmountCents));
            NotifyOfPropertyChange("DroppedDisplayAmount");
            NotifyOfPropertyChange("DroppedDisplayAmountString");
        }
    }

    public Denomination TotalDenomination
    {
        get => _totalDenominationResult;
        set
        {
            long? totalValue1 = _totalDenominationResult?.TotalValue;
            long? totalValue2 = value?.TotalValue;
            if (totalValue1.GetValueOrDefault() == totalValue2.GetValueOrDefault() & totalValue1.HasValue == totalValue2.HasValue)
            {
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "TotalDenominationResult", "TotalDenominationResult", "Denom remains at {0}", TotalDenomination?.ToString() ?? "{null}");
            }
            else
            {
                _totalDenominationResult = value;
                ApplicationViewModel.Log.Info(GetType().Name, "TotalDenominationResult Changed", "Tx Property Changed", TotalDenomination?.ToString() ?? "{null}");
                NotifyOfPropertyChange((Expression<Func<Denomination>>)(() => TotalDenomination));
            }
        }
    }

    public Denomination CountedDenomination
    {
        get => _countedDenominationResult;
        set
        {
            _countedDenominationResult = value;
            ApplicationViewModel.Log.Info(GetType().Name, "CountedDenominationResult Changed", "Tx Property Changed", CountedDenomination?.ToString() ?? "{null}");
            NotifyOfPropertyChange((Expression<Func<Denomination>>)(() => CountedDenomination));
        }
    }

    public Denomination DroppedDenomination
    {
        get => _droppedDenominationResult;
        set
        {
            long? totalValue1 = _droppedDenominationResult?.TotalValue;
            long? totalValue2 = value?.TotalValue;
            if (totalValue1.GetValueOrDefault() == totalValue2.GetValueOrDefault() & totalValue1.HasValue == totalValue2.HasValue)
            {
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "DroppedDenominationResult", "DroppedDenominationResult", "Denom remains at {0}", DroppedDenomination?.ToString() ?? "{null}");
            }
            else
            {
                lock (DroppedDenominationUpdateLock)
                {
                    _droppedDenominationResult = value;
                    foreach (DenominationItem denominationItem in value.DenominationItems)
                    {
                        DenominationItem denom = denominationItem;
                        List<DenominationDetail> list = Transaction.DenominationDetails.Where(x => x.Denom == denom.denominationValue).ToList();
                        if (list.Count() > 1)
                        {
                            foreach (DenominationDetail denominationDetail in list)
                                Transaction.DenominationDetails.Remove(denominationDetail);
                        }
                        DenominationDetail denominationDetail1 = Transaction.DenominationDetails.FirstOrDefault(x => x.Denom == denom.denominationValue);
                        if (denominationDetail1 == null)
                        {
                            Transaction transaction = Transaction;
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
                NotifyOfPropertyChange((Expression<Func<Denomination>>)(() => DroppedDenomination));
                Session.SaveToDatabase();
            }
        }
    }

    public TransactionLimitListItem TransactionLimits
    {
        get
        {
            TransactionTypeListItem transactionType = TransactionType;
            TransactionLimitListItem transactionLimits;
            if (transactionType == null)
            {
                transactionLimits = null;
            }
            else
            {
                TransactionLimitList transactionLimitList = transactionType.TxLimitListNavigation;
                if (transactionLimitList == null)
                {
                    transactionLimits = null;
                }
                else
                {
                    ICollection<TransactionLimitListItem> transactionLimitListItems = transactionLimitList.TransactionLimitListItems;
                    transactionLimits = transactionLimitListItems != null ? transactionLimitListItems.FirstOrDefault(x => string.Equals(x.CurrencyCode, CurrencyCode, StringComparison.InvariantCultureIgnoreCase)) : (TransactionLimitListItem)null;
                }
            }
            return transactionLimits;
        }
    }

    [IgnoreDataMember]
    public TransactionText TransactionText => TransactionType?.TxTextNavigationText;

    [IgnoreDataMember]
    public TransactionTypeListItem TransactionType
    {
        get => _transaction?.TxTypeNavigation;
        set
        {
            ApplicationViewModel.Log.InfoFormat(GetType().Name, "TransactionType Changed", "Tx Property Changed", "TransactionType changed from {0} to {1}", (object)_transactionType?.CbTxType, (object)value?.CbTxType);
            _transactionType = value;
            Transaction.TxTypeNavigation = _depositorDBContext.TransactionTypeListItems.FirstOrDefault(x => x.Id == value.Id);
            NotifyOfPropertyChange(nameof(TransactionType));
        }
    }

    [IgnoreDataMember]
    public List<SummaryListItem> TransactionAccountReferences
    {
        get
        {
            List<SummaryListItem> accountReferences = new List<SummaryListItem>();
            if (Transaction.TxTypeNavigation.DefaultAccount == null)
            {
                if (AccountNumber != null)
                    accountReferences.Add(new SummaryListItem()
                    {
                        Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Account Number", TransactionType?.TxTextNavigationText?.AccountNumberCaption, "Account Number"),
                        Value = AccountNumber.Trim()
                    });
                if (AccountName != null)
                    accountReferences.Add(new SummaryListItem()
                    {
                        Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Account Name", TransactionType?.TxTextNavigationText?.AccountNameCaption, "Account Name"),
                        Value = AccountName.Trim()
                    });
            }
            if (ReferenceAccount != null)
                accountReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Reference Number", TransactionType?.TxTextNavigationText?.ReferenceAccountNumberCaption, "Reference Number"),
                    Value = ReferenceAccount.Trim()
                });
            if (ReferenceAccountName != null)
                accountReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionAccountReferences Reference Name", TransactionType?.TxTextNavigationText?.ReferenceAccountNameCaption, "Reference Name"),
                    Value = ReferenceAccountName.Trim()
                });
            return accountReferences;
        }
    }

    public List<SummaryListItem> TransactionReferences
    {
        get
        {
            List<SummaryListItem> transactionReferences = new List<SummaryListItem>(TransactionAccountReferences);
            if (Currency != null)
                transactionReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(GetType().Name + ".TransactionReferences", "sys_Currency_Caption", "Currency"),
                    Value = Currency.Code.ToUpper().Trim()
                });
            if (FundsSource != null)
                transactionReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  FundsSource", TransactionType?.TxTextNavigationText?.FundsSourceCaption, "Source of Funds"),
                    Value = FundsSource.Trim()
                });
            if (Narration != null)
                transactionReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  Narration", TransactionType?.TxTextNavigationText?.NarrationCaption, "Narration"),
                    Value = Narration.Trim()
                });
            if (DepositorName != null)
                transactionReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences Depositor Name", TransactionType?.TxTextNavigationText?.DepositorNameCaption, "Depositor Name"),
                    Value = DepositorName.Trim()
                });
            if (IDNumber != null)
                transactionReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  ID Number", TransactionType?.TxTextNavigationText?.IdNumberCaption, "ID Number"),
                    Value = IDNumber.Trim()
                });
            if (_phone != null)
                transactionReferences.Add(new SummaryListItem()
                {
                    Title = ApplicationViewModel.CashmereTranslationService.TranslateUserText(GetType().Name + ".TransactionReferences  Phone", TransactionType?.TxTextNavigationText?.PhoneNumberCaption, "Phone"),
                    Value = _phone.Trim()
                });
            return transactionReferences;
        }
    }

    public bool IsOverCount
    {
        get
        {
            DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
            int num;
            if ((deviceConfiguration != null ? deviceConfiguration.USE_MAX_DEPOSIT_COUNT ? 1 : 0 : 0) != 0 && TransactionType?.TxLimitListNavigation?.Get_overcount_amount(Currency) > 0L && (bool)TransactionType?.TxLimitListNavigation?.Get_prevent_overcount(Currency))
            {
                Decimal totalDisplayAmount = TotalDisplayAmount;
                long? overcountAmount = TransactionType?.TxLimitListNavigation?.Get_overcount_amount(Currency);
                Decimal? nullable = overcountAmount.HasValue ? new Decimal?(overcountAmount.GetValueOrDefault()) : new Decimal?();
                Decimal valueOrDefault = nullable.GetValueOrDefault();
                num = totalDisplayAmount >= valueOrDefault & nullable.HasValue ? 1 : 0;
            }
            else
                num = 0;
            return num != 0;
        }
    }

    public bool IsOverDeposit
    {
        get
        {
            int num;
            if (TransactionType?.TxLimitListNavigation?.Get_overdeposit_amount(Currency) > 0L && (bool)TransactionType?.TxLimitListNavigation?.Get_prevent_overdeposit(Currency))
            {
                Decimal totalDisplayAmount = TotalDisplayAmount;
                long? overdepositAmount = TransactionType?.TxLimitListNavigation?.Get_overdeposit_amount(Currency);
                Decimal? nullable = overdepositAmount.HasValue ? new Decimal?(overdepositAmount.GetValueOrDefault()) : new Decimal?();
                Decimal valueOrDefault = nullable.GetValueOrDefault();
                num = totalDisplayAmount >= valueOrDefault & nullable.HasValue ? 1 : 0;
            }
            else
                num = 0;
            return num != 0;
        }
    }

    public bool IsUnderDeposit
    {
        get
        {
            TransactionLimitListItem transactionLimits = TransactionLimits;
            int num;
            if ((transactionLimits != null ? (bool)transactionLimits.PreventUnderdeposit ? 1 : 0 : 0) != 0 && TotalDisplayAmount > 0M)
            {
                Decimal totalDisplayAmount = TotalDisplayAmount;
                long? underdepositAmount = TransactionLimits?.UnderdepositAmount;
                Decimal? nullable = underdepositAmount.HasValue ? new Decimal?(underdepositAmount.GetValueOrDefault()) : new Decimal?();
                Decimal valueOrDefault = nullable.GetValueOrDefault();
                num = totalDisplayAmount < valueOrDefault & nullable.HasValue ? 1 : 0;
            }
            else
                num = 0;
            return num != 0;
        }
    }

    public bool ShowFundsSource
    {
        get
        {
            TransactionLimitListItem transactionLimits = TransactionLimits;
            int num;
            if ((transactionLimits != null ? transactionLimits.ShowFundsSource ? 1 : 0 : 0) != 0)
            {
                Decimal totalDisplayAmount = TotalDisplayAmount;
                long? fundsSourceAmount = TransactionLimits?.FundsSourceAmount;
                Decimal? nullable = fundsSourceAmount.HasValue ? new Decimal?(fundsSourceAmount.GetValueOrDefault()) : new Decimal?();
                Decimal valueOrDefault = nullable.GetValueOrDefault();
                num = totalDisplayAmount >= valueOrDefault & nullable.HasValue ? 1 : 0;
            }
            else
                num = 0;
            return num != 0;
        }
    }

    internal void LogTransactionError(ApplicationErrorConst result, string errorMessage)
    {
        _transaction.TxErrorCode = (int)result;
        _transaction.TxErrorMessage = errorMessage.Left(254);
    }

    internal void EndTransaction(ApplicationErrorConst result, string errorMessage = null)
    {
        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(EndTransaction), "Transaction", "Terminating transaction with result= {0}", result.ToString());
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
            ApplicationViewModel.Log.ErrorFormat(GetType().Name, (int)result, result.ToString(), "Transaction ended with error {0}", errorMessage);
            Session.ApplicationViewModel.AlertManager.SendAlert(new AlertTransactionEndedFailed(this, Session.Device, DateTime.Now));
        }
        if (Session.HasCounted && Transaction.Printouts.FirstOrDefault(x => !x.IsCopy) == null)
            Session.ApplicationViewModel.PrintReceipt(Transaction);
        Session.SaveToDatabase();
    }

    public void HandleDenominationResult(TransactionStatusResponseResult TransactionResult)
    {
        string sessionId = TransactionResult?.data?.SessionID;
        Guid guid = Session.SessionID;
        string b1 = guid.ToString();
        int num1;
        if (string.Equals(sessionId, b1, StringComparison.OrdinalIgnoreCase))
        {
            string transactionId = TransactionResult?.data?.TransactionID;
            guid = Transaction.Id;
            string b2 = guid.ToString();
            num1 = string.Equals(transactionId, b2, StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        }
        else
            num1 = 0;
        if (num1 != 0)
        {
            if (!Completed)
            {
                long? nullable1;
                int num2;
                if (TotalAmountCents == TransactionResult.data.EscrowPlusDroppedTotalCents)
                {
                    long droppedAmountCents = DroppedAmountCents;
                    nullable1 = TransactionResult?.data?.TotalDroppedAmountCents;
                    long valueOrDefault = nullable1.GetValueOrDefault();
                    if (droppedAmountCents == valueOrDefault & nullable1.HasValue)
                    {
                        int? count;
                        if (TotalDenomination != null)
                        {
                            Denomination totalDenomination = TotalDenomination;
                            int num3;
                            if (totalDenomination == null)
                            {
                                num3 = 0;
                            }
                            else
                            {
                                count = totalDenomination.DenominationItems?.Count;
                                int num4 = 0;
                                num3 = count.GetValueOrDefault() == num4 & count.HasValue ? 1 : 0;
                            }
                            if (num3 == 0)
                            {
                                num2 = 0;
                                goto label_17;
                            }
                        }
                        if (TransactionResult == null)
                        {
                            num2 = 0;
                            goto label_17;
                        }
                        else
                        {
                            count = TransactionResult.data?.CurrentDropStatus?.data?.DenominationResult?.data.DenominationItems.Count;
                            int num5 = 0;
                            num2 = count.GetValueOrDefault() > num5 & count.HasValue ? 1 : 0;
                            goto label_17;
                        }
                    }
                }
                num2 = 1;
            label_17:
                if (num2 != 0)
                {
                    DroppedDenomination = TransactionResult?.data?.TotalDroppedNotes;
                    ApplicationViewModel.Log.Trace(nameof(AppTransaction), "dropped denom", nameof(HandleDenominationResult), DroppedDenomination?.ToString());
                    CountedDenomination = TransactionResult?.data?.CurrentDropStatus?.data?.DenominationResult?.data;
                    ApplicationViewModel.Log.Trace(nameof(AppTransaction), "counted denom", nameof(HandleDenominationResult), CountedDenomination?.ToString());
                    TotalDenomination = DroppedDenomination + CountedDenomination;
                    ApplicationViewModel.Log.Trace(nameof(AppTransaction), "total denom", nameof(HandleDenominationResult), TotalDenomination?.ToString());
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
                    DroppedAmountCents = nullable1.GetValueOrDefault();
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
                    CountedAmountCents = nullable1.GetValueOrDefault();
                    TotalAmountCents = DroppedAmountCents + CountedAmountCents;
                    HandleTransactionLimit();
                }
                NotifyOfPropertyChange((Expression<Func<List<DenominationItem>>>)(() => TotalDenomination.DenominationItems));
            }
            else
                ApplicationViewModel.Log.WarningFormat(GetType().Name, "Transaction Already Complete", "AddDenominationResult", "Cannot add denomination {0} as transaction is already complete.", TransactionResult?.data?.TotalDroppedAmountDisplayString);
        }
        else
            ApplicationViewModel.Log.Error(nameof(AppTransaction), 1, nameof(HandleDenominationResult), string.Format("Incorrect transaction IDs. SessionID {0} >> {1} && TransactionID {2} >> {3}", TransactionResult?.data?.SessionID, Session.SessionID, TransactionResult?.data?.TransactionID, Transaction.Id));
    }

    private void HandleTransactionLimit()
    {
        if (Session == null)
            return;
        if (DroppedDenomination == null)
        {
            ApplicationViewModel.Log?.Warning(GetType().Name, "Null Reference for DroppedDenomination", nameof(HandleTransactionLimit), "DenominationResult");
        }
        else
        {
            TransactionLimitListItem transactionLimits1 = TransactionLimits;
            int num1;
            if ((transactionLimits1 != null ? transactionLimits1.PreventOverdeposit ? 1 : 0 : 0) != 0)
            {
                TransactionLimitListItem transactionLimits2 = TransactionLimits;
                if ((transactionLimits2 != null ? transactionLimits2.OverdepositAmount > 0L ? 1 : 0 : 0) != 0)
                {
                    long num2 = DroppedAmountCents + CountedAmountCents;
                    TransactionLimitListItem transactionLimits3 = TransactionLimits;
                    long? nullable = transactionLimits3 != null ? new long?(transactionLimits3.OverdepositAmount * 100L) : new long?();
                    long valueOrDefault = nullable.GetValueOrDefault();
                    num1 = num2 >= valueOrDefault & nullable.HasValue ? 1 : 0;
                    goto label_10;
                }
            }
            num1 = 0;
        label_10:
            if (num1 == 0)
                ;
            if (!IsOverDeposit)
                ;
            if (!IsOverDeposit)
                return;
            ApplicationViewModel.Log.InfoFormat(GetType().Name, "Count", "Device Management", "Transaction of {0} is above transaction value limit of {1}, Locking Counter", DroppedDenomination.TotalValue / 100L, TransactionLimits?.OverdepositAmount);
            OnTransactionLimitReachedEvent(this, EventArgs.Empty);
        }
    }

    public string ToRawTextString()
    {
        StringBuilder stringBuilder1 = new StringBuilder(byte.MaxValue);
        stringBuilder1.AppendLine("----------------------------------------");
        stringBuilder1.AppendLine("             Transaction");
        stringBuilder1.AppendLine("----------------------------------------");
        StringBuilder stringBuilder2 = stringBuilder1;
        object[] objArray = new object[16]
        {
            _transaction.TxStartDate,
            _transaction.TxEndDate,
            _transaction.TxTypeNavigation.Name,
            _transaction.TxAccountNumber,
            _transaction.CbAccountName,
            _transaction.TxRefAccount,
            _transaction.CbRefAccountName,
            _transaction.TxNarration,
            _transaction.TxDepositorName,
            _transaction.TxIdNumber,
            _transaction.TxPhone,
            _transaction.TxCurrency,
            null,
            null,
            null,
            null
        };
        long? txAmount = _transaction.TxAmount;
        long num1 = 100;
        objArray[12] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?();
        objArray[13] = _transaction.FundsSource;
        objArray[14] = _transaction.TxErrorCode;
        objArray[15] = _transaction.TxErrorMessage;
        string str = string.Format("\r\nStart Date:                 {0}\r\nEnd Date:                   {1}\r\nTransaction Type:           {2}\r\nAccount Number:             {3}\r\nAccount Name:               {4}\r\nReference Account Number:   {5}\r\nReference Account Name:     {6}            \r\nNarration:                  {7}\r\nDepositor Name:             {8}\r\nDepositor ID:               {9}\r\nDepositor Phone:            {10}\r\nCurrency:                   {11}\r\nAmount:                     {12}\r\nSource of Funds:            {13}\r\nError Code:                 {14}\r\nError Message:              {15}", objArray);
        stringBuilder2.AppendLine(str);
        stringBuilder1.AppendLine("________________________________________");
        stringBuilder1.AppendLine(string.Format("{0,-12}{1,7}{2,21}", "Denomination", "Count", "Sub Total"));
        stringBuilder1.AppendLine("________________________________________");
        List<DenominationDetail> list = _transaction.DenominationDetails.ToList();
        foreach (DenominationDetail denominationDetail in list)
        {
            double num2 = denominationDetail.Denom / 100.0;
            long count = denominationDetail.Count;
            double num3 = denominationDetail.Denom * denominationDetail.Count / 100.0;
            int num4 = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 11;
            stringBuilder1.AppendLine(string.Format("{0,-12:0.##}{1,7:##,0}{2,23:##,0.##}", num2, count, num3));
        }
        stringBuilder1.AppendLine("========================================");
        stringBuilder1.AppendLine(string.Format("{0,-10}{1,7:##,0}{2,23:##,0.##}", "TOTAL:", list.Sum(x => x.Count), list.Sum(x => x.Subtotal) / 100.0));
        stringBuilder1.AppendLine("========================================");
        return stringBuilder1.ToString();
    }

    public string ToEmailString()
    {
        StringBuilder stringBuilder1 = new StringBuilder(byte.MaxValue);
        stringBuilder1.AppendLine("<p><hr /><h3>Transaction</h3><hr />");
        StringBuilder stringBuilder2 = stringBuilder1;
        object[] objArray = new object[16]
        {
            _transaction.TxStartDate,
            _transaction.TxEndDate,
            _transaction.TxTypeNavigation.Name,
            _transaction.TxAccountNumber,
            _transaction.CbAccountName,
            _transaction.TxRefAccount,
            _transaction.CbRefAccountName,
            _transaction.TxNarration,
            _transaction.TxDepositorName,
            _transaction.TxIdNumber,
            _transaction.TxPhone,
            _transaction.TxCurrency,
            null,
            null,
            null,
            null
        };
        long? txAmount = _transaction.TxAmount;
        long num1 = 100;
        objArray[12] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?();
        objArray[13] = _transaction.FundsSource;
        objArray[14] = _transaction.TxErrorCode;
        objArray[15] = _transaction.TxErrorMessage;
        string str = string.Format("<p><table border=\"0\" cellpadding=\"20\" cellspacing=\"0\" width=\"600\">\r\n< tr><th>Start Date</th><th>End Date</th><th>Transaction Type</th><th>Account Number</th><th>Account Name</th><th>Reference Account Number</th><th>Reference Account Name</th>\r\n<th>Narration</th><th>Depositor Name</th><th>Depositor ID</th><th>Depositor Phone</th><th>Currency</th><th>Amount</th><th>Source of Funds</th>\r\n<th>Error Code</th><th>Error Message</th></tr>\r\n<tr><td>{0}</td>  <td>{1}</td>  <td>{2}</td>  <td>{3}</td>  <td>{4}</td>  <td>{5}</td>  <td>{6}</td>  <td>{7}</td>  <td>{8}</td>  <td>{9}</td>  <td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td></tr>\r\n</table></p>", objArray);
        stringBuilder2.AppendLine(str);
        stringBuilder1.AppendLine("<p><table style=\"text - align: left\">");
        stringBuilder1.AppendLine("<tr><th>Denomination</th><th>Count</th><th>Sub Total</th></tr>");
        List<DenominationDetail> list = _transaction.DenominationDetails.ToList();
        foreach (DenominationDetail denominationDetail in list)
        {
            double num2 = denominationDetail.Denom / 100.0;
            long count = denominationDetail.Count;
            double num3 = denominationDetail.Denom * denominationDetail.Count / 100.0;
            int num4 = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 11;
            stringBuilder1.AppendLine(string.Format("<tr><td>{0:##,0.##}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", num2, count, num3));
        }
        stringBuilder1.AppendLine(string.Format("<tr><td>{0}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", "TOTAL:", list.Sum(x => x.Count), list.Sum(x => x.Subtotal) / 100.0));
        stringBuilder1.AppendLine("</table></p></p>");
        return stringBuilder1.ToString();
    }

    private void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
    {
        ApplicationViewModel.Log.Trace(GetType().Name, nameof(OnPropertyChangedEvent), "DATABASE", "Saving transaction on property change.");
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