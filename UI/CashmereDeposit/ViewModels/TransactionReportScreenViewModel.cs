﻿
//.TransactionReportScreenViewModel




using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  public class TransactionReportScreenViewModel : DepositorScreenViewModelBase
  {
    private const int txPageSize = 10;
    private int maxPage;
    private int _currentPage;
    private IQueryable<Transaction> txQuery;
    private IEnumerable<Transaction> _transactionList;
    private Transaction selectedTransaction;
    private IEnumerable<DenominationDetail> _denominationList;

    public TransactionReportScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      Screen callingObject)
      : base(screenTitle, applicationViewModel, callingObject)
    {
      txQuery = DBContext.Transactions.Where(t => t.TxEndDate >= txQueryStartDate && t.TxEndDate < txQueryEndDate).OrderByDescending(t => t.TxEndDate);
      Activated += new EventHandler<ActivationEventArgs>(TransactionReportScreenViewModel_Activated);
    }

    private void TransactionReportScreenViewModel_Activated(object sender, ActivationEventArgs e)
    {
        PageFirst_Transaction();
    }

    public int CurrentTxPage
    {
        get { return _currentPage; }
        set
      {
        _currentPage = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanPageFirst_Transaction));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanPageLast_Transaction));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanPageNext_Transaction));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanPagePrevious_Transaction));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>) (() => PageNumberText));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanEmailTransactionList));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanPrintReceipt));
      }
    }

    public DateTime txQueryStartDate { get; set; } = DateTime.MinValue;

    public DateTime txQueryEndDate { get; set; } = DateTime.Now;

    public string txQueryType { get; set; }

    public string txQueryCurrency { get; set; }

    public IEnumerable<Transaction> Transactions
    {
        get { return _transactionList; }
        set
      {
        _transactionList = value;
        maxPage = (int) Math.Ceiling(txQuery.Count() / 10.0) - 1;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<IEnumerable<Transaction>>>) (() => Transactions));
      }
    }

    public Transaction SelectedTransaction
    {
        get { return selectedTransaction; }
        set
      {
        selectedTransaction = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<Transaction>>) (() => SelectedTransaction));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<IEnumerable<DenominationDetail>>>) (() => DenominationList));
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>) (() => CanPrintReceipt));
      }
    }

    public IEnumerable<DenominationDetail> DenominationList
    {
      get
      {
        Transaction selectedTransaction = SelectedTransaction;
        return selectedTransaction == null ? null : (IEnumerable<DenominationDetail>) selectedTransaction.DenominationDetails;
      }
    }

    public string PageNumberText
    {
        get { return string.Format("Page {0} of {1}", CurrentTxPage + 1, maxPage + 1); }
    }

    public bool CanPageFirst_Transaction
    {
        get { return CurrentTxPage > 0; }
    }

    public void PageFirst_Transaction()
    {
      CurrentTxPage = 0;
      Page_Transaction();
    }

    public bool CanPagePrevious_Transaction
    {
        get { return CurrentTxPage > 0; }
    }

    public void PagePrevious_Transaction()
    {
      if (CurrentTxPage <= 0)
      {
        PageFirst_Transaction();
      }
      else
      {
        --CurrentTxPage;
        Page_Transaction();
      }
    }

    public bool CanPageNext_Transaction
    {
        get { return CurrentTxPage < maxPage; }
    }

    public void PageNext_Transaction()
    {
      if (CurrentTxPage >= maxPage)
      {
        PageLast_Transaction();
      }
      else
      {
        ++CurrentTxPage;
        Page_Transaction();
      }
    }

    public bool CanPageLast_Transaction
    {
        get { return CurrentTxPage < maxPage; }
    }

    public void PageLast_Transaction()
    {
      CurrentTxPage = maxPage;
      Page_Transaction();
    }

    public void Page_Transaction()
    {
        Transactions = txQuery.Skip(CurrentTxPage * 10).Take(10).ToList();
    }

    public bool CanEmailTransactionList
    {
        get { return txQuery.Count() > 0; }
    }

    public void EmailTransactionList()
    {
      int num = (int) MessageBox.Show("Mail Sent");
    }

    public bool CanPrintReceipt
    {
        get { return SelectedTransaction != null; }
    }

    public void PrintReceipt()
    {
      ApplicationViewModel.PrintReceipt(SelectedTransaction, true);
      int num = (int) MessageBox.Show("Sent to printer");
    }
  }
}
