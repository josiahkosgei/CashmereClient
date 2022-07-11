
using System;
using System.Runtime.Serialization;

namespace Cashmere.API.Messaging.Integration.Exceptions
{
  [Serializable]
  internal class CashmereAPIValidationException : Exception
  {
    public CashmereAPIValidationException()
    {
    }

    public CashmereAPIValidationException(string message)
      : base(message)
    {
    }

    public CashmereAPIValidationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected CashmereAPIValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
