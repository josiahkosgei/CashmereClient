
//Logging.APILogger


namespace Cashmere.Library.CashmereDataAccess.Logging
{
  public class APILogger : UtilDepositorLogger
  {
    public APILogger(Version version, string type)
      : base(version, type)
    {
    }

    public APILogger()
    {
    }

    public void Debug(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      string EventDetail) => Log(requestID, Component, EventName, EventType, UtilLoggingLevel.DEBUG, EventDetail);

    public void DebugFormat(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      string EventDetailFormat,
      params object[] EventDetailFormatObjects) => LogFormat(requestID, Component, EventName, EventType, UtilLoggingLevel.DEBUG, EventDetailFormat, EventDetailFormatObjects);

    public void Info(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      string EventDetail) => Log(requestID, Component, EventName, EventType, UtilLoggingLevel.INFO, EventDetail);

    public void InfoFormat(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      string EventDetailFormat,
      params object[] EventDetailFormatObjects) => LogFormat(requestID, Component, EventName, EventType, UtilLoggingLevel.INFO, EventDetailFormat, EventDetailFormatObjects);

    public void Warning(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      string EventDetail) => Log(requestID, Component, EventName, EventType, UtilLoggingLevel.WARN, EventDetail);

    public void WarningFormat(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      string EventDetailFormat,
      params object[] EventDetailFormatObjects) => LogFormat(requestID, Component, EventName, EventType, UtilLoggingLevel.WARN, EventDetailFormat, EventDetailFormatObjects);

    public void Error(
      string requestID,
      string Component,
      int Code,
      string ErrorName,
      string ErrorDetail) => Log(requestID, Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorDetail);

    public void ErrorFormat(
      string requestID,
      string Component,
      int Code,
      string ErrorName,
      string ErrorMessageFormat,
      params object[] ErrorMessageFormatObjects)
    {
      LogFormat(requestID, Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.ERROR, ErrorMessageFormat, ErrorMessageFormatObjects);
      string.Format(ErrorMessageFormat, ErrorMessageFormatObjects);
    }

    public void Fatal(
      string requestID,
      string Component,
      int Code,
      string ErrorName,
      string ErrorDetail) => Log(requestID, Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.FATAL, ErrorDetail);

    public void FatalFormat(
      string requestID,
      string Component,
      int Code,
      string ErrorName,
      string ErrorMessageFormat,
      params object[] ErrorMessageFormatObjects)
    {
      LogFormat(requestID, Component, ErrorName, "ERROR " + Code.ToString(), UtilLoggingLevel.FATAL, ErrorMessageFormat, ErrorMessageFormatObjects);
      string.Format(ErrorMessageFormat, ErrorMessageFormatObjects);
    }

    private void Log(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      UtilLoggingLevel Level,
      string EventDetail) => LogFormat(requestID, Component, EventName, EventType, Level, "{0}", EventDetail);

    public void LogFormat(
      string requestID,
      string Component,
      string EventName,
      string EventType,
      UtilLoggingLevel Level,
      string EventDetailFormat,
      params object[] EventDetailFormatObjects) => LogToFileFormat(Component, EventName, EventType, Level, requestID + "|" + EventDetailFormat, EventDetailFormatObjects);
  }
}
