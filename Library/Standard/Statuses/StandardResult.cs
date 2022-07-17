// StandardResult


using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class StandardResult : EventArgs
  {
    public int resultCode { get; set; }

    public string extendedResult { get; set; }

    public ErrorLevel level { get; set; }

    public Exception message { get; set; }

    public object data { get; set; }
  }
}
