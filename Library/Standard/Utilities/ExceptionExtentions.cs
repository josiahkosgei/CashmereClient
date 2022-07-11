
//ExceptionExtentions


using System;

namespace Cashmere.Library.Standard.Utilities
{
  public static class ExceptionExtentions
  {
    public static string MessageString(this Exception ex, int? MaxCharacters = null)
    {
      if (MaxCharacters.HasValue)
      {
        int? nullable = MaxCharacters;
        int num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          return (ex.Message + ">" + ex.InnerException?.Message + ">" + ex.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.InnerException?.Message).Left(MaxCharacters.Value);
      }
      return ex.Message + ">" + ex.InnerException?.Message + ">" + ex.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.InnerException?.Message;
    }
  }
}
