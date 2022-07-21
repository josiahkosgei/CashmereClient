
// Type: Cashmere.Library.Standard.Logging.ICashmereWebLogger


namespace Cashmere.Library.Standard.Logging
{
    public interface ICashmereWebLogger
    {
        void Debug(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Error(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Fatal(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Info(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Trace(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);

        void Warning(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects);
    }
}
