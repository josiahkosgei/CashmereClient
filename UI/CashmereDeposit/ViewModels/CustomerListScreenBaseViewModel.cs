using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
    public abstract class CustomerListScreenBaseViewModel : DepositorCustomerScreenBaseViewModel
    {
        private int NumItemsPerPage = 4;
        private int _currentPage;
        private ATMSelectionItem<object> selected;

        public CustomerListScreenBaseViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(screenTitle, applicationViewModel, required)
        {
        }

        private int MaxPage => (int)Math.Max(Math.Ceiling(FullList.Count * 1.0 / NumItemsPerPage) - 1.0, 0.0);

        private int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value.Clamp(0, MaxPage);
                RefreshPagination();
            }
        }

        public bool CanGetFirstPage => CurrentPage > 0;

        public bool CanGetPrevPage => CurrentPage > 0;

        public bool CanGetNextPage => CurrentPage < MaxPage;

        public bool CanGetLastPage => CurrentPage != MaxPage;

        public string PaginationInfoString => string.Format("Page {0} of {1}", CurrentPage + 1, MaxPage + 1);

        public List<ATMSelectionItem<object>> FullList { get; set; }

        public List<ATMSelectionItem<object>> FilteredList { get; set; }

        public ATMSelectionItem<object> SelectedFilteredList
        {
            get => selected;
            set
            {
                selected = value;
                NotifyOfPropertyChange(nameof(SelectedFilteredList));
                if (value == null)
                    return;
                PrintErrorText("Processing, please wait...");
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                var backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerReportsProgress = false;
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }

        public void ShowPage(int pageNumber)
        {
            FilteredList = FullList.Skip(CurrentPage * NumItemsPerPage).Take(NumItemsPerPage).ToList();
            NotifyOfPropertyChange((Expression<Func<List<ATMSelectionItem<object>>>>)(() => FilteredList));
        }

        public void GetFirstPage()
        {
            CurrentPage = 0;
            ShowPage(CurrentPage);
        }

        public void GetPrevPage()
        {
            --CurrentPage;
            ShowPage(CurrentPage);
        }

        public void GetNextPage()
        {
            ++CurrentPage;
            ShowPage(CurrentPage);
        }

        public void GetLastPage()
        {
            CurrentPage = MaxPage;
            ShowPage(CurrentPage);
        }

        private void RefreshPagination()
        {
            NotifyOfPropertyChange((Expression<Func<int>>)(() => CurrentPage));
            NotifyOfPropertyChange(() => CanGetFirstPage);
            NotifyOfPropertyChange(() => CanGetPrevPage);
            NotifyOfPropertyChange(() => CanGetNextPage);
            NotifyOfPropertyChange(() => CanGetLastPage);
            NotifyOfPropertyChange(() => PaginationInfoString);
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PerformSelection();
        }

        public abstract void PerformSelection();
    }
}
