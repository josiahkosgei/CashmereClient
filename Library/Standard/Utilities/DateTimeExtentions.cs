
//DateTimeExtentions


using System;

namespace Cashmere.Library.Standard.Utilities
{
  public static class DateTimeExtentions
  {
    public static int IsWithin(this DateTime date, DateTime fromDate, DateTime toDate)
    {
      if (fromDate >= toDate)
        throw new ArgumentException(string.Format("fromDate of {0:yyyy-MM-dd HH:mm:ss.fff ZZ} >= toDate of {1:yyyy-MM-dd HH:mm:ss.fff ZZ}", fromDate, toDate));
      if (date < fromDate)
        return -1;
      return !(date <= toDate) ? 1 : 0;
    }

    public static bool IsBetween(this DateTime date, DateTime fromDate, DateTime toDate) => date.IsWithin(fromDate, toDate) == 0;
  }
}
