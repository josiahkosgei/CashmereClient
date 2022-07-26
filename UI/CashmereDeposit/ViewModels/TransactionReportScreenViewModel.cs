
//.TransactionReportScreenViewModel




using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    public class TransactionReportScreenViewModel : DepositorScreenViewModelBase
    {
        private const int txPageSize = 10;
        private int maxPage;
        private int _currentPage;
        //  private readonly DepositorDBContext _depositorDBContext;
        private IQueryable<Transaction> txQuery;
        private IEnumerable<Transaction> _transactionList;
        private Transaction selectedTransaction;
        private IEnumerable<DenominationDetail> _denominationList;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionReportScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, callingObject)
        {
            _transactionRepository = IoC.Get<ITransactionRepository>();
            txQuery = _transactionRepository.GetByDateRange(txQueryStartDate, txQueryEndDate);
            Activated += new EventHandler<ActivationEventArgs>(TransactionReportScreenViewModel_Activated);
        }

        private void TransactionReportScreenViewModel_Activated(object sender, ActivationEventArgs e)
        {
            PageFirst_Transaction();
        }

        public int CurrentTxPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPageFirst_Transaction));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPageLast_Transaction));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPageNext_Transaction));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPagePrevious_Transaction));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>)(() => PageNumberText));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanEmailTransactionList));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPrintReceipt));
            }
        }

        public DateTime txQueryStartDate { get; set; } = DateTime.MinValue;

        public DateTime txQueryEndDate { get; set; } = DateTime.Now;

        public string txQueryType { get; set; }

        public string txQueryCurrency { get; set; }

        public IEnumerable<Transaction> Transactions
        {
            get => _transactionList;
            set
            {
                _transactionList = value;
                maxPage = (int)Math.Ceiling(txQuery.Count() / 10.0) - 1;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<IEnumerable<Transaction>>>)(() => Transactions));
            }
        }

        public Transaction SelectedTransaction
        {
            get => selectedTransaction;
            set
            {
                selectedTransaction = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<Transaction>>)(() => SelectedTransaction));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<IEnumerable<DenominationDetail>>>)(() => DenominationList));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPrintReceipt));
            }
        }

        public IEnumerable<DenominationDetail> DenominationList
        {
            get
            {
                var selectedTransaction = SelectedTransaction;
                return selectedTransaction == null ? null : (IEnumerable<DenominationDetail>)selectedTransaction.DenominationDetails;
            }
        }

        public string PageNumberText => string.Format("Page {0} of {1}", CurrentTxPage + 1, maxPage + 1);

        public bool CanPageFirst_Transaction => CurrentTxPage > 0;

        public void PageFirst_Transaction()
        {
            CurrentTxPage = 0;
            Page_Transaction();
        }

        public bool CanPagePrevious_Transaction => CurrentTxPage > 0;

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

        public bool CanPageNext_Transaction => CurrentTxPage < maxPage;

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

        public bool CanPageLast_Transaction => CurrentTxPage < maxPage;

        public void PageLast_Transaction()
        {
            CurrentTxPage = maxPage;
            Page_Transaction();
        }

        public void Page_Transaction()
        {
            Transactions = txQuery.Skip(CurrentTxPage * 10).Take(10).ToList();
        }

        public bool CanEmailTransactionList => txQuery.Count() > 0;

        public void EmailTransactionList()
        {
            var num = (int)MessageBox.Show("Mail Sent");
        }

        public bool CanPrintReceipt => SelectedTransaction != null;

        public void PrintReceipt()
        {
            ApplicationViewModel.PrintReceipt(SelectedTransaction, true);
            var num = (int)MessageBox.Show("Sent to printer");
        }
    }
}
