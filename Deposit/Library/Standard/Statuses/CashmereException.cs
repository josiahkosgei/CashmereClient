//CashmereException

using Cashmere.Library.Standard.Utilities;
using System;
using System.Runtime.Serialization;

namespace Cashmere.Library.Standard.Statuses
{
  [Serializable]
  public class CashmereException : Exception
  {
    public string PublicErrorCode { get; set; } = "500";

    public string PublicErrorMessage { get; set; } = "Error encountered. Kindly try again later or contact your administrator.";

    public string ServerErrorCode { get; set; } = "500";

    public string ServerErrorMessage { get; set; }

    public CashmereException()
    {
    }

    public CashmereException(string message)
      : base(message)
    {
      this.ServerErrorMessage = this.MessageString();
    }

    public CashmereException(string message, Exception inner)
      : base(message, inner)
    {
      this.ServerErrorMessage = this.MessageString();
    }

    protected CashmereException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
