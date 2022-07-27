
//UtilNLogLogger


using Cashmere.Library.CashmereDataAccess.Logging;
using NLog;

namespace Cashmere.Library.CashmereDataAccess
{
    public class UtilNLogLogger : ICashmereLogger
    {
        private readonly Logger _innerLogger;

        public UtilNLogLogger(Type type) => _innerLogger = LogManager.GetLogger(type.Name);

        public UtilNLogLogger(string type) => _innerLogger = LogManager.GetLogger(type);

        public void ErrorFormat(Exception exception) => _innerLogger.Error(exception, "{0}>>{1}>>{2}>>{3}", new object[4]
        {
      exception.Message,
      exception.InnerException?.Message,
      exception.InnerException?.InnerException?.Message,
      exception.InnerException?.InnerException?.InnerException?.Message
        });

        public void ErrorFormat(UtilLogEntry logEntry) => _innerLogger.Error("\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003", new object[6]
        {
      logEntry.date,
      logEntry.eventName,
      logEntry.eventDetail,
      logEntry.eventType,
      logEntry.component,
      logEntry.level.ToString().PadRight(5)
        });

        public void InfoFormat(UtilLogEntry logEntry) => _innerLogger.Info("\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003", new object[6]
        {
      logEntry.date,
      logEntry.eventName,
      logEntry.eventDetail,
      logEntry.eventType,
      logEntry.component,
      logEntry.level.ToString().PadRight(5)
        });

        public void InfoFormat(string format, params object[] args) => _innerLogger.Info(format, args);

        public void WarnFormat(string format, params object[] args) => _innerLogger.Warn(format, args);

        public void WarnFormat(UtilLogEntry logEntry) => _innerLogger.Warn("\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003", new object[6]
        {
      logEntry.date,
      logEntry.eventName,
      logEntry.eventDetail,
      logEntry.eventType,
      logEntry.component,
      logEntry.level.ToString().PadRight(5)
        });

        public void FatalFormat(string format, params object[] args) => _innerLogger.Fatal(format, args);

        public void FatalFormat(UtilLogEntry logEntry) => _innerLogger.Fatal("\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003", new object[6]
        {
      logEntry.date,
      logEntry.eventName,
      logEntry.eventDetail,
      logEntry.eventType,
      logEntry.component,
      logEntry.level.ToString().PadRight(5)
        });

        public void DebugFormat(string format, params object[] args) => _innerLogger.Debug(format, args);

        public void DebugFormat(UtilLogEntry logEntry) => _innerLogger.Debug("\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003", new object[6]
        {
      logEntry.date,
      logEntry.eventName,
      logEntry.eventDetail,
      logEntry.eventType,
      logEntry.component,
      logEntry.level.ToString().PadRight(5)
        });

        public void TraceFormat(string format, params object[] args) => _innerLogger.Trace(format, args);

        public void TraceFormat(UtilLogEntry logEntry) => _innerLogger.Trace("\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003", new object[6]
        {
      logEntry.date,
      logEntry.eventName,
      logEntry.eventDetail,
      logEntry.eventType,
      logEntry.component,
      logEntry.level.ToString().PadRight(5)
        });

        public bool IsTraceEnabled => _innerLogger.IsTraceEnabled;

        public bool IsDebugEnabled => _innerLogger.IsDebugEnabled;

        public bool IsErrorEnabled => _innerLogger.IsErrorEnabled;

        public bool IsFatalEnabled => _innerLogger.IsFatalEnabled;

        public bool IsInfoEnabled => _innerLogger.IsInfoEnabled;

        public bool IsWarnEnabled => _innerLogger.IsWarnEnabled;
    }
}
