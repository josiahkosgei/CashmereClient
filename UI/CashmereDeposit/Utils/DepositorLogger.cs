
// Type: CashmereDeposit.Utils.DepositorLogger




using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Logging;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Utils
{
    public class DepositorLogger : CashmereLogger
    {
        private CashmereLogger _innerLogger;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IApplicationLogRepository _iApplicationLogRepository;

        private ApplicationViewModel ApplicationViewModel { get; }

        public DepositorLogger(ApplicationViewModel applicationViewModel, string LoggerName = "CashmereDepositLog")
          : base(Assembly.GetCallingAssembly().GetName().Version.ToString(), LoggerName, null)
        {
            _deviceRepository = IoC.Get<IDeviceRepository>();
            _iApplicationLogRepository = IoC.Get<IApplicationLogRepository>();
            if (string.IsNullOrWhiteSpace(LoggerName))
                LoggerName = "CashmereDepositLog";
            ApplicationViewModel = applicationViewModel;
            _innerLogger = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), LoggerName, null);
        }

        public void Trace(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.TRACE, EventDetail);

        public void TraceFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects) => LogFormat(Component, EventName, EventType, UtilLoggingLevel.TRACE, EventDetailFormat, EventDetailFormatObjects);

        public void Debug(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.DEBUG, EventDetail);

        public void DebugFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects) => LogFormat(Component, EventName, EventType, UtilLoggingLevel.DEBUG, EventDetailFormat, EventDetailFormatObjects);

        public void Info(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.INFO, EventDetail);

        public void InfoFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects) => LogFormat(Component, EventName, EventType, UtilLoggingLevel.INFO, EventDetailFormat, EventDetailFormatObjects);

        public void Warning(string Component, string EventName, string EventType, string EventDetail) => Log(Component, EventName, EventType, UtilLoggingLevel.WARN, EventDetail);

        public void WarningFormat(
          string Component,
          string EventName,
          string EventType,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects) => LogFormat(Component, EventName, EventType, UtilLoggingLevel.WARN, EventDetailFormat, EventDetailFormatObjects);

        public void Error(string Component, int Code, string ErrorName, string ErrorDetail) => Log(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorDetail);

        public void ErrorFormat(
          string Component,
          int Code,
          string ErrorName,
          string ErrorMessageFormat,
          params object[] ErrorMessageFormatObjects) => LogFormat(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorMessageFormat, ErrorMessageFormatObjects);

        public void Fatal(string Component, int Code, string ErrorName, string ErrorDetail) => Log(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.FATAL, ErrorDetail);

        public void FatalFormat(
          string Component,
          int Code,
          string ErrorName,
          string ErrorMessageFormat,
          params object[] ErrorMessageFormatObjects)
        {
            LogFormat(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.FATAL, ErrorMessageFormat, ErrorMessageFormatObjects);
            string.Format(ErrorMessageFormat, ErrorMessageFormatObjects);
        }

        private void Log(
          string Component,
          string EventName,
          string EventType,
          UtilLoggingLevel Level,
          string EventDetail) => LogFormat(Component, EventName, EventType, Level, "{0}", EventDetail);

        public void LogFormat(
          string Component,
          string EventName,
          string EventType,
          UtilLoggingLevel Level,
          string EventDetailFormat,
          params object[] EventDetailFormatObjects)
        {
            try
            {
                switch (Level)
                {
                    case UtilLoggingLevel.TRACE:
                        _innerLogger.Trace(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.DEBUG:
                        _innerLogger.Debug(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.INFO:
                        _innerLogger.Info(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.WARN:
                        _innerLogger.Warning(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.ERROR:
                        _innerLogger.Error(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                    case UtilLoggingLevel.FATAL:
                        _innerLogger.Fatal(Component, EventName, EventType, EventDetailFormat, EventDetailFormatObjects);
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            string str = string.Format(EventDetailFormat, EventDetailFormatObjects);
            if (string.IsNullOrEmpty(str) || Level < (UtilLoggingLevel)ApplicationViewModel.DeviceConfiguration.LOGGING_LEVEL)
                return;

            try
            {
                var device = _deviceRepository.GetDevice(Environment.MachineName).ContinueWith(r => r.Result).Result;
                ApplicationLog applicationLog = new ApplicationLog
                {
                    Id = GuidExt.UuidCreateSequential(),
                    LogDate = DateTime.Now,
                    Component = Component,
                    DeviceId = device.Id,
                    SessionId = ApplicationViewModel?.SessionID,
                    EventDetail = str,
                    EventName = EventName,
                    EventType = EventType,
                    LogLevel = (int)Level,
                    MachineName = Environment.MachineName
                };
                ApplicationLog entity = new();
                entity = _iApplicationLogRepository.AddAsync(applicationLog).ContinueWith(r => r.Result).Result;
            }
            catch (Exception ex)
            {
                _innerLogger.Warning(GetType().Name, "Logging Failed", "LogEvent", string.Format("{0}>>{1}>>{2}>stack>{3}", ex.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message, ex.StackTrace), Array.Empty<object>());
            }
        }
    }
}
