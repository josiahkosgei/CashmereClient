
// Type: Cashmere.Library.Standard.Logging.ICashmereAPILogger


namespace Cashmere.Library.Standard.Logging
{
    public interface ICashmereAPILogger
    {
        void Debug(
          string SessionID,
          string MessageID,
          string AppName,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Error(
          string SessionID,
          string MessageID,
          string AppName,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Fatal(
          string SessionID,
          string MessageID,
          string AppName,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Info(
          string SessionID,
          string MessageID,
          string AppName,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Trace(
          string SessionID,
          string MessageID,
          string AppName,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Warning(
          string SessionID,
          string MessageID,
          string AppName,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);
    }
}
