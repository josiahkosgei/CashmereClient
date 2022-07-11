
// Type: Cashmere.Library.Standard.Logging.ICashmereLogger


namespace Cashmere.Library.Standard.Logging
{
  public interface ICashmereLogger
  {
    void Debug(
      string Component,
      string EventName,
      string EventType,
      string Message,
      params object[] MessageFormatObjects);

    void Error(
      string Component,
      string EventName,
      string EventType,
      string Message,
      params object[] MessageFormatObjects);

    void Fatal(
      string Component,
      string EventName,
      string EventType,
      string Message,
      params object[] MessageFormatObjects);

    void Info(
      string Component,
      string EventName,
      string EventType,
      string Message,
      params object[] MessageFormatObjects);

    void Trace(
      string Component,
      string EventName,
      string EventType,
      string Message,
      params object[] MessageFormatObjects);

    void Warning(
      string Component,
      string EventName,
      string EventType,
      string Message,
      params object[] MessageFormatObjects);
  }
}
