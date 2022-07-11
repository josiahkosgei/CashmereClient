
//Logging.UtilLogEntry


namespace Cashmere.Library.CashmereDataAccess.Logging
{
  public class UtilLogEntry
  {
    public const string format = "\x0002{5}|{0:yyyy-MM-dd HH:mm:ss.ffff zzzz}|{1}|{3}|{4}|{2}\x0003";
    public string component;
    public string eventName;
    public string eventType;
    public string eventDetail;
    public UtilLoggingLevel level;
    public DateTime date = DateTime.Now;

    public UtilLogEntry(
      string Component,
      string EventName,
      string EventType,
      string EventDetail,
      UtilLoggingLevel Level)
    {
      level = Level;
      component = Component;
      eventName = EventName;
      eventType = EventType;
      eventDetail = EventDetail;
    }
  }
}
