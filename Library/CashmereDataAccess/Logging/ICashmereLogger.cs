
//Logging.ICashmereLogger


namespace Cashmere.Library.CashmereDataAccess.Logging
{
  public interface ICashmereLogger
  {
    void TraceFormat(UtilLogEntry logEntry);

    void TraceFormat(string format, params object[] args);

    void DebugFormat(UtilLogEntry logEntry);

    void DebugFormat(string format, params object[] args);

    void InfoFormat(UtilLogEntry logEntry);

    void InfoFormat(string format, params object[] args);

    void WarnFormat(UtilLogEntry logEntry);

    void WarnFormat(string format, params object[] args);

    void ErrorFormat(UtilLogEntry logEntry);

    void ErrorFormat(Exception exception);

    void FatalFormat(UtilLogEntry logEntry);

    void FatalFormat(string format, params object[] args);

    bool IsTraceEnabled { get; }

    bool IsDebugEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }
  }
}
