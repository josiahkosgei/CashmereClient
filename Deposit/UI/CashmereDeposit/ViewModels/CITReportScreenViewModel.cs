﻿
//.CITReportScreenViewModel




using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    public class CITReportScreenViewModel : DepositorScreenViewModelBase
    {
        private const int txPageSize = 10;
        private readonly ICITRepository _citRepository;
        private int maxPage;
        private int _currentPage;
        private IQueryable<CIT> txQuery;
        private IEnumerable<CIT> _citTransactionList;
        private CIT selectedCITTransaction;
        private IEnumerable<CITDenomination> _citDenominationList;

        public CITReportScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, callingObject)
        {
            _citRepository = IoC.Get<ICITRepository>();
            txQuery = _citRepository.GetByDateRange(txQueryStartDate, txQueryEndDate);
            Activated += new EventHandler<ActivationEventArgs>(CITReportScreenViewModel_Activated);
        }

        private void CITReportScreenViewModel_Activated(object sender, ActivationEventArgs e)
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
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanEmailCITTransactionList));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPrintCITReceipt));
            }
        }

        public DateTime txQueryStartDate { get; set; } = DateTime.MinValue;

        public DateTime txQueryEndDate { get; set; } = DateTime.Now;

        public string txQueryType { get; set; }

        public string txQueryCurrency { get; set; }

        public IEnumerable<CIT> CITTransactions
        {
            get => _citTransactionList;
            set
            {
                _citTransactionList = value;
                maxPage = (int)Math.Ceiling(txQuery.Count() / 10.0) - 1;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<IEnumerable<CIT>>>)(() => CITTransactions));
            }
        }

        public CIT SelectedCITTransaction
        {
            get => selectedCITTransaction;
            set
            {
                selectedCITTransaction = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<CIT>>)(() => SelectedCITTransaction));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<IEnumerable<CITDenomination>>>)(() => CITDenominationList));
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<bool>>)(() => CanPrintCITReceipt));
            }
        }

        public IEnumerable<CITDenomination> CITDenominationList
        {
            get
            {
                var selectedCITTransaction = SelectedCITTransaction;
                return selectedCITTransaction == null ? null : (IEnumerable<CITDenomination>)selectedCITTransaction.CITDenominations.Select(x => new CITDenomination()
                {
                    CITId = x.CITId,
                    Denom = x.Denom / 100,
                    Count = x.Count,
                    Subtotal = x.Subtotal / 100L,
                    Currency = x.Currency,
                    CurrencyId = x.CurrencyId,
                    Datetime = x.Datetime,
                    CIT = x.CIT,
                    Id = x.Id
                }).OrderBy(x => x.CurrencyId);
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
            CITTransactions = txQuery.Skip(CurrentTxPage * 10).Take(10).ToList();
        }

        public bool CanEmailCITTransactionList => txQuery.Count() > 0;

        public void EmailCITTransactionList()
        {
            var num = (int)MessageBox.Show("Mail Sent");
        }

        public bool CanPrintCITReceipt => SelectedCITTransaction != null;

        public void PrintCITReceipt()
        {
            ApplicationViewModel.PrintCITReceipt(SelectedCITTransaction, true);
            var num = (int)MessageBox.Show("Printing Complete");
        }
    }
}