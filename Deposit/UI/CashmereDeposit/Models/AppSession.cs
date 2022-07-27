﻿using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.Utils.AlertClasses;
using CashmereDeposit.ViewModels;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.Models
{
    public class AppSession : PropertyChangedBase
    {
        private AppTransaction _transaction;
        private ApplicationViewModel _applicationViewModel;
        private static IDeviceRepository _iDeviceRepository { get; set; }

        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;
        private readonly IDepositorSessionRepository _depositorSessionRepository;
        private readonly ITransactionTypeListItemRepository _transactionTypeListItemRepository;

        public AppSession(ApplicationViewModel applicationViewModel)
        {

            _iDeviceRepository = IoC.Get<IDeviceRepository>();
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            _depositorSessionRepository = IoC.Get<IDepositorSessionRepository>();
            _transactionTypeListItemRepository = IoC.Get<ITransactionTypeListItemRepository>();

            _applicationViewModel = applicationViewModel;
            Device = ApplicationViewModel.ApplicationModel.GetDeviceAsync();
            DepositorSession = new DepositorSession()
            {
                Id = GuidExt.UuidCreateSequential(),
                DeviceId = Device.Id,
                SessionStart = DateTime.Now,
                SessionEnd = new DateTime?(),
                LanguageCode = "en-gb"
            };
            SessionID = DepositorSession.Id;
            TermsAccepted = false;
            SessionStart = DateTime.Now;
            SessionComplete = false;
            PropertyChanged += new PropertyChangedEventHandler(OnPropertyChangedEvent);
            _depositorSessionRepository.AddAsync(DepositorSession);
            ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SessionStart), "Session", "Session {0} started", SessionID.ToString().ToUpper());
        }

        public Guid SessionID
        {
            get => DepositorSession.Id;
            set
            {
                DepositorSession.Id = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionID Set", "Tx Property Changed", "SessionID set to {0}", SessionID);
                NotifyOfPropertyChange(nameof(SessionID));
            }
        }

        public DateTime SessionStart
        {
            get => DepositorSession.SessionStart;
            set
            {
                DepositorSession.SessionStart = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionStart Set", "Tx Property Changed", "SessionStart set to {0}", SessionStart);
                NotifyOfPropertyChange(nameof(SessionStart));
            }
        }

        public DateTime SessionEnd
        {
            get => DepositorSession.SessionEnd.Value;
            set
            {
                DepositorSession.SessionEnd = new DateTime?(value);
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionEnd Set", "Tx Property Changed", "SessionEnd set to {0}", SessionEnd);
                NotifyOfPropertyChange(nameof(SessionEnd));
            }
        }

        public AppTransaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                NotifyOfPropertyChange(nameof(Transaction));
            }
        }

        public CultureInfo Culture { get; set; }

        public CultureInfo UICulture { get; set; }

        public string Language
        {
            get => DepositorSession.LanguageCode;
            set
            {
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "Language Changed", "Tx Property Changed", "Language changed from {0} to {1}", DepositorSession.LanguageCode, value);
                DepositorSession.LanguageCode = value;
                NotifyOfPropertyChange<string>((Expression<Func<string>>)(() => Language));
                Culture = new CultureInfo(Language);
                NotifyOfPropertyChange<CultureInfo>((Expression<Func<CultureInfo>>)(() => Culture));
                UICulture = new CultureInfo(Language);
                NotifyOfPropertyChange<CultureInfo>((Expression<Func<CultureInfo>>)(() => UICulture));
            }
        }

        public bool SessionComplete
        {
            get => DepositorSession.Complete;
            set
            {
                DepositorSession.Complete = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionComplete Set", "Tx Property Changed", "SessionComplete set to {0}", SessionComplete);
                NotifyOfPropertyChange(nameof(SessionComplete));
            }
        }

        public bool SessionCompleteSuccess
        {
            get => DepositorSession.CompleteSuccess;
            set
            {
                DepositorSession.CompleteSuccess = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionCompleteSuccess Set", "Tx Property Changed", "SessionCompleteSuccess set to {0}", SessionCompleteSuccess);
                NotifyOfPropertyChange(nameof(SessionCompleteSuccess));
            }
        }

        public int SessionErrorCode
        {
            get => DepositorSession.ErrorCode.Value;
            set
            {
                DepositorSession.ErrorCode = new int?(value);
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionErrorCode Set", "Tx Property Changed", "SessionErrorCode set to {0}", SessionErrorCode);
                NotifyOfPropertyChange(nameof(SessionErrorCode));
            }
        }

        public string SessionErrorMessage
        {
            get => DepositorSession.ErrorMessage;
            set
            {
                DepositorSession.ErrorMessage = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "SessionErrorMessage Set", "Tx Property Changed", "SessionErrorMessage set to {0}", SessionErrorMessage);
                NotifyOfPropertyChange(nameof(SessionErrorMessage));
            }
        }

        public bool TermsAccepted
        {
            get => DepositorSession.TermsAccepted;
            set
            {
                DepositorSession.TermsAccepted = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "TermsAccepted Set", "Tx Property Changed", "TermsAccepted set to {0}", TermsAccepted);
                NotifyOfPropertyChange(nameof(TermsAccepted));
            }
        }

        public bool AccountVerified
        {
            get => DepositorSession.AccountVerified;
            set
            {
                DepositorSession.AccountVerified = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "AccountVerified Set", "Tx Property Changed", "AccountVerified set to {0}", AccountVerified);
                NotifyOfPropertyChange(nameof(AccountVerified));
            }
        }

        public bool ReferenceAccountVerified
        {
            get => DepositorSession.ReferenceAccountVerified;
            set
            {
                DepositorSession.ReferenceAccountVerified = value;
                ApplicationViewModel.Log.InfoFormat(GetType().Name, "ReferenceAccountVerified Set", "Tx Property Changed", "ReferenceAccountVerified set to {0}", ReferenceAccountVerified);
                NotifyOfPropertyChange(nameof(ReferenceAccountVerified));
            }
        }

        internal bool HasCounted { get; set; } = false;

        internal bool CountingStarted { get; set; } = false;

        internal bool CountingEnded { get; set; } = false;

        public ApplicationViewModel ApplicationViewModel => _applicationViewModel;

        public Device Device { get; }

        public DepositorSession DepositorSession { get; set; }

        public override bool Equals(object obj)
        {
            Guid? nullable = obj is AppSession appSession ? new Guid?(appSession.SessionID) : new Guid?();
            Guid sessionId = SessionID;
            return nullable.HasValue && (!nullable.HasValue || nullable.GetValueOrDefault() == sessionId);
        }

        public override int GetHashCode() => SessionID.GetHashCode();

        internal async void CreateTransaction(TransactionTypeListItem transactionType)
        {
            var transactionTypeListItem = _transactionTypeListItemRepository.GetTransactionTypeScreenList(transactionType.Id);
            Transaction = new AppTransaction(this, transactionTypeListItem, transactionType.DefaultAccountCurrency);
            Transaction.TransactionLimitReachedEvent += new EventHandler<EventArgs>(Transaction_TransactionLimitReachedEvent);
            //TODO: Remove
            var alertTransactionStarted = new AlertTransactionStarted(Transaction, Device, DateTime.Now);
            //ApplicationViewModel.AlertManager.SendAlert(alertTransactionStarted);
        }

        private void Transaction_TransactionLimitReachedEvent(object sender, EventArgs e) => OnTransactionLimitReachedEvent(this, e);

        internal void LogSessionError(ApplicationErrorConst result, string errormessage)
        {
            SessionErrorCode = (int)result;
            SessionErrorMessage = errormessage;
        }

        internal void EndSession(
          bool success,
          int errorCode,
          ApplicationErrorConst result,
          string errorMessage)
        {
            ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(EndSession), "Session", "Session Result = {0}, transaction result = {1},errorcode = {2}, errormessage={3}", success, result, errorCode, errorMessage);
            SessionComplete = true;
            SessionCompleteSuccess = success;
            if (result != 0)
                LogSessionError(result, errorMessage);
            SessionEnd = DateTime.Now;
            if (Transaction != null && !Transaction.Completed)
                Transaction.EndTransaction(result, errorMessage);

            //_depositorSessionRepository.UpdateAsync(SessionEnd);
            //_depositorDBContext.SaveChangesAsync();
            //_depositorDBContext.Dispose();
        }

        private void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            // SaveToDatabase();
        }

        public event EventHandler<EventArgs> TransactionLimitReachedEvent;

        private void OnTransactionLimitReachedEvent(object sender, EventArgs e)
        {
            if (TransactionLimitReachedEvent == null)
                return;
            TransactionLimitReachedEvent(this, e);
        }

        //internal void SaveToDatabase()
        //{
        //    //  _depositorDBContext.SaveChangesAsync();
        //}
    }
}