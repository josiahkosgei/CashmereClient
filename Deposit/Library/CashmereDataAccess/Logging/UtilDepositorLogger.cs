
//Logging.UtilDepositorLogger


using System.Reflection;

namespace Cashmere.Library.CashmereDataAccess.Logging
{
    public class UtilDepositorLogger
    {
        private ICashmereLogger _innerLogger;

        public bool IsTraceEnabled
        {
            get
            {
                ICashmereLogger innerLogger = _innerLogger;
                return innerLogger != null && innerLogger.IsTraceEnabled;
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                ICashmereLogger innerLogger = _innerLogger;
                return innerLogger != null && innerLogger.IsDebugEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                ICashmereLogger innerLogger = _innerLogger;
                return innerLogger != null && innerLogger.IsErrorEnabled;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                ICashmereLogger innerLogger = _innerLogger;
                return innerLogger != null && innerLogger.IsFatalEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                ICashmereLogger innerLogger = _innerLogger;
                return innerLogger != null && innerLogger.IsInfoEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                ICashmereLogger innerLogger = _innerLogger;
                return innerLogger != null && innerLogger.IsWarnEnabled;
            }
        }

        public UtilDepositorLogger()
          : this(Assembly.GetCallingAssembly().GetName().Version, nameof(UtilDepositorLogger))
        {
        }

        public UtilDepositorLogger(Version version, string type)
        {
            _innerLogger = new UtilNLogLogger(type);
            InfoFormat("UtilDepositorLogger.Constructor", "Initialising", "Init", "Initialising logger for type [{0}] version {1}", type, version.ToString());
        }

        public void LogError() => throw new NotImplementedException();

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

        public void Error(string v, int applicerrconst) => throw new NotImplementedException();

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
          params object[] ErrorMessageFormatObjects)
        {
            LogFormat(Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorMessageFormat, ErrorMessageFormatObjects);
            string.Format(ErrorMessageFormat, ErrorMessageFormatObjects);
        }

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
          params object[] EventDetailFormatObjects) => LogToFileFormat(Component, EventName, EventType, Level, EventDetailFormat, EventDetailFormatObjects);

        public void LogToFileFormat(
          string Component,
          string EventName,
          string EventType,
          UtilLoggingLevel Level,
          string ErrorMessageFormat,
          params object[] ErrorMessageFormatObjects)
        {
            string EventDetail = string.Format(ErrorMessageFormat, ErrorMessageFormatObjects).Replace("\r\n", "\n");
            Console.WriteLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] {1}", DateTime.Now, EventDetail));
            switch (Level)
            {
                case UtilLoggingLevel.TRACE:
                    if (!_innerLogger.IsTraceEnabled)
                        break;
                    _innerLogger.TraceFormat(new UtilLogEntry(Component, EventName, EventType, EventDetail, Level));
                    break;
                case UtilLoggingLevel.DEBUG:
                    if (!_innerLogger.IsDebugEnabled)
                        break;
                    _innerLogger.DebugFormat(new UtilLogEntry(Component, EventName, EventType, EventDetail, Level));
                    break;
                case UtilLoggingLevel.INFO:
                    if (!_innerLogger.IsInfoEnabled)
                        break;
                    _innerLogger.InfoFormat(new UtilLogEntry(Component, EventName, EventType, EventDetail, Level));
                    break;
                case UtilLoggingLevel.WARN:
                    if (!_innerLogger.IsWarnEnabled)
                        break;
                    _innerLogger.WarnFormat(new UtilLogEntry(Component, EventName, EventType, EventDetail, Level));
                    break;
                case UtilLoggingLevel.ERROR:
                    if (!_innerLogger.IsErrorEnabled)
                        break;
                    _innerLogger.ErrorFormat(new UtilLogEntry(Component, EventName, EventType, EventDetail, Level));
                    break;
                case UtilLoggingLevel.FATAL:
                    if (!_innerLogger.IsFatalEnabled)
                        break;
                    _innerLogger.FatalFormat(new UtilLogEntry(Component, EventName, EventType, EventDetail, Level));
                    break;
            }
        }
    }
}
